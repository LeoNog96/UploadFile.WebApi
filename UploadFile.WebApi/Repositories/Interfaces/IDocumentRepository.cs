
using System.Collections.Generic;
using System.Threading.Tasks;
using UploadFile.WebApi.DTOs;
using UploadFile.WebApi.Models;

namespace UploadFile.WebApi.Repositories.Interfaces
{
    public interface IDocumentRepository : IBaseRepository<Document>
    {
        Task<List<DocumentDTO>> GetDocumentByUser(long userId);
    }
}