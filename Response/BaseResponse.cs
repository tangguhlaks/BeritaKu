namespace BeritaKuAPI.Response
{
    public class BaseResponse<T>
    {
        public BaseResponse() { }

        public int ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public T ResponseData { get; set; }
    }
}
