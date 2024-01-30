using BeritaKuAPI.Model;
using BeritaKuAPI.Repository;
using BeritaKuAPI.Response;
using BeritaKuAPI.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;

namespace BeritaKuAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BeritaController : ControllerBase
    {
        private readonly IBeritaRepository _repository;
        public BeritaController(IBeritaRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] BeritaCreateUpdateVM request)
        {
            BaseResponse<BeritaResponseCreate> response = new BaseResponse<BeritaResponseCreate>();
            try
            {
                if (request.Title == "")
                {
                    response.ResponseCode = 500;
                    response.ResponseMessage = "Title required!";
                    response.ResponseData = null;
                    return Ok(response);
                }

                var userNameClaim = User.FindFirst(ClaimTypes.Name);

                Berita berita = new Berita();
                berita.Id = Guid.NewGuid();
                berita.Title = request.Title;
                berita.Description = request.Description;
                berita.CreatedAt = DateTime.Now;
                berita.UpdatedAt = DateTime.Now;
                berita.CreatedBy = userNameClaim.Value;
                var Id = _repository.Create(berita);
                if (Id == "")
                {
                    response.ResponseCode = 500;
                    response.ResponseMessage = "Failed";
                    response.ResponseData = null;
                    return Ok(response);
                }

                response.ResponseCode = 200;
                response.ResponseMessage = "Success";
                response.ResponseData = new BeritaResponseCreate(Id);
                return Ok(response);

            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.ResponseMessage = ex.Message;
                response.ResponseData = null;
                return Ok(response);
            }
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            BaseResponse<BeritaResponseGetAll> response = new BaseResponse<BeritaResponseGetAll>();
            try
            {
                response.ResponseCode = 200;
                response.ResponseMessage = "Success";
                response.ResponseData = new BeritaResponseGetAll(_repository.GetAll());
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.ResponseMessage = ex.Message;
                response.ResponseData = null;
                return Ok(response);
            }
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(string id)
        {
            BaseResponse<BeritaResponseGetById> response = new BaseResponse<BeritaResponseGetById>();
            try
            {
                if (id == "")
                {
                    response.ResponseCode = 500;
                    response.ResponseMessage = "Id Required!";
                    response.ResponseData = null;
                    return Ok(response);
                }
                response.ResponseCode = 200;
                response.ResponseMessage = "Success";
                response.ResponseData = new BeritaResponseGetById(_repository.GetById(id));
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.ResponseMessage = ex.Message;
                response.ResponseData = null;
                return Ok(response);
            }
        }

        [HttpGet("GetByCreatedBy")]
        public async Task<IActionResult> GetByCreatedBy(string username)
        {
            BaseResponse<BeritaResponseGetByCreatedBy> response = new BaseResponse<BeritaResponseGetByCreatedBy>();
            try
            {
                if (username == "")
                {
                    response.ResponseCode = 500;
                    response.ResponseMessage = "Username Required!";
                    response.ResponseData = null;
                    return Ok(response);
                }

                response.ResponseCode = 200;
                response.ResponseMessage = "Success";
                response.ResponseData = new BeritaResponseGetByCreatedBy(_repository.GetByCreatedBy(username));
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.ResponseMessage = ex.Message;
                response.ResponseData = null;
                return Ok(response);
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] BeritaCreateUpdateVM request,string id)
        {
            BaseResponse<BeritaResponseUpdate> response = new BaseResponse<BeritaResponseUpdate>();
            try
            {
                if (id == "")
                {
                    response.ResponseCode = 500;
                    response.ResponseMessage = "Id Required!";
                    response.ResponseData = null;
                    return Ok(response);
                }

                if (request.Title == "")
                {
                    response.ResponseCode = 500;
                    response.ResponseMessage = "Title required!";
                    response.ResponseData = null;
                    return Ok(response);
                }

                var userNameClaim = User.FindFirst(ClaimTypes.Name);

                BeritaVM berita = _repository.GetById(id);
                Berita newBerita = new Berita();
                newBerita.Id = berita.Id;
                newBerita.Title = request.Title;
                newBerita.Description = request.Description;
                newBerita.UpdatedAt =DateTime.Now;
                newBerita.CreatedAt = berita.CreatedAt;
                newBerita.CreatedBy = berita.CreatedBy;

                var Id = _repository.Update(newBerita);
                if (Id == "")
                {
                    response.ResponseCode = 500;
                    response.ResponseMessage = "Failed";
                    response.ResponseData = null;
                    return Ok(response);
                }

                response.ResponseCode = 200;
                response.ResponseMessage = "Success";
                response.ResponseData = new BeritaResponseUpdate(Id);
                return Ok(response);

            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.ResponseMessage = ex.Message;
                response.ResponseData = null;
                return Ok(response);
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            BaseResponse<BeritaResponseDelete> response = new BaseResponse<BeritaResponseDelete>();
            try
            {
                if (id == "")
                {
                    response.ResponseCode = 500;
                    response.ResponseMessage = "Id Required!";
                    response.ResponseData = null;
                    return Ok(response);
                }
                if (!_repository.Delete(id))
                {
                    response.ResponseCode = 500;
                    response.ResponseMessage = "Failed!";
                    response.ResponseData = null;
                    return Ok(response);
                }
                response.ResponseCode = 200;
                response.ResponseMessage = "Success";
                response.ResponseData = new BeritaResponseDelete(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.ResponseMessage = ex.Message;
                response.ResponseData = null;
                return Ok(response);
            }
        }

    }
}
