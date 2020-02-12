using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UploadFile.WebApi.DTOs;
using UploadFile.WebApi.Models;

namespace UploadFile.WebApi.Services.Interfaces
{
    public interface IDocumentService
    {
        Task<Document> Save(IFormFile document);
        Task<Document> Get(long id);
        Task<List<DocumentDTO>> GetDocumentByUser();
    }
}