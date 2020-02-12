using Microsoft.AspNetCore.Http;

namespace UploadFile.WebApi.DTOs
{
    public class DocumentDTO
    {
        public long Id { get; set; }
        public string FileName { get; set;}
        public long UserId { get; set; }
    }
}