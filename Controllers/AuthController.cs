using Azure;
using BeritaKuAPI.Model;
using BeritaKuAPI.Repository;
using BeritaKuAPI.Response;
using BeritaKuAPI.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BeritaKuAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IAuthRepository _repository;
        private readonly IMemoryCache _memoryCache;
        private const int MaxLoginAttempts = 3;
        private const int TimeWindowMinutes = 5;
        private const int AccountLockoutDurationMinutes = 5;

        public AuthController(IAuthRepository repository,IMemoryCache memoryCache, IConfiguration configuration)
        {
            _repository = repository;
            _memoryCache = memoryCache;
            this.configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] AuthVM request)
        {
            BaseResponse<AuthResponse> response = new BaseResponse<AuthResponse>();
            try
            {
                // HashPassword
                string salt = BCrypt.Net.BCrypt.GenerateSalt(12); 
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password, salt);

                User user = new User();
                user.Id = Guid.NewGuid();
                user.Username = request.Username;
                user.Password = hashedPassword;
                user.isActived = true;
                user.UpdatedAt = DateTime.UtcNow;
                user.CreatedAt = DateTime.UtcNow;
                _repository.Register(user);
                response.ResponseCode = 200;
                response.ResponseMessage = "Success";
                response.ResponseData = new AuthResponse("-");
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.ResponseMessage = ex.Message;
                response.ResponseData = new AuthResponse("-");
                return Ok(response);
            }
        }

        [AllowAnonymous]
        [HttpPost("Auth")]
        public async Task<IActionResult> Auth([FromBody] AuthVM request)
        {
            BaseResponse<AuthResponse> response = new BaseResponse<AuthResponse>();
            var attemptsKey = "";
            try
            {
                if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                {
                    response.ResponseCode = 500;
                    response.ResponseMessage = "Please fill all input data.";
                    response.ResponseData = new AuthResponse("-");
                    return Ok(response);
                }

                // check locked account
                var lockoutKey = $"{request.Username}_Lockout";
                var isLockedOut = _memoryCache.Get<bool>(lockoutKey);
                if (isLockedOut)
                {
                    response.ResponseCode = 500;
                    response.ResponseMessage = "Your account has been temporarily locked, try again later!";
                    response.ResponseData = new AuthResponse("-");
                    return Ok(response);
                }

                if (_repository.CheckUserExistByUsername(request.Username))
                {
                    var user = _repository.Authenticate(request.Username, request.Password);
                    if (user == null)
                    {
                        // configuration login locked
                        attemptsKey = $"{request.Username}_LoginAttempts";
                        var attempts = _memoryCache.TryGetValue(attemptsKey, out int loginAttempts) ? loginAttempts : 0;
                        attempts++;
                        _memoryCache.Set(attemptsKey, attempts, TimeSpan.FromMinutes(TimeWindowMinutes));
                        if (attempts >= MaxLoginAttempts)
                        {
                            _memoryCache.Set(lockoutKey, true, TimeSpan.FromMinutes(AccountLockoutDurationMinutes));
                            response.ResponseCode = 500;
                            response.ResponseMessage = "Your account has been temporarily locked due to multiple failed login attempts. Please try again later.";
                            response.ResponseData = new AuthResponse("-");
                            return Ok(response);
                        }
                        response.ResponseCode = 500;
                        response.ResponseMessage = "Wrong password!";
                        response.ResponseData = new AuthResponse("-");
                        return Ok(response);
                    }

                    // generate jwt
                    var jwtToken = string.Empty;
                    var issuer = configuration["Jwt:Issuer"];
                    var audience = configuration["Jwt:Audience"];
                    var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);
                    var signingCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha512Signature
                    );

                    var subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, request.Username),
                        new Claim(JwtRegisteredClaimNames.Sub, request.Username),
                        new Claim(JwtRegisteredClaimNames.Email, request.Username)
                    });

                    var expires = DateTime.UtcNow.AddDays(7);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = subject,
                        Expires = expires,
                        Issuer = issuer,
                        Audience = audience,
                        SigningCredentials = signingCredentials
                    };
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    jwtToken = tokenHandler.WriteToken(token);


                    // reset attempts
                     attemptsKey = $"{request.Username}_LoginAttempts";
                    _memoryCache.Set(attemptsKey, 0, TimeSpan.FromMinutes(TimeWindowMinutes));

                    response.ResponseCode = 200;
                    response.ResponseMessage = "Success";
                    response.ResponseData = new AuthResponse(jwtToken);
                    return Ok(response);
                }
                else
                {
                    response.ResponseCode = 500;
                    response.ResponseMessage = "User with username '" + request.Username + "' not found";
                    response.ResponseData = new AuthResponse("-");
                    return Ok(response);
                }


            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.ResponseMessage = ex.Message;
                response.ResponseData = new AuthResponse("-");
                return Ok(response);
            }
        }

    }

}
