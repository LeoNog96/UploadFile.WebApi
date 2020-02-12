using Microsoft.AspNetCore.Http;

namespace UploadFile.WebApi.Services
{
    public class BaseService
    {
        private readonly IHttpContextAccessor _accessor;

        public BaseService (IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public long UserId
        {
            get
            {
                return long.Parse (_accessor.HttpContext.User.FindFirst ("userId").Value);
            }
        }
    }
}