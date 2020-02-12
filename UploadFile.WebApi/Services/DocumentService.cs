using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UploadFile.WebApi.DTOs;
using UploadFile.WebApi.Models;
using UploadFile.WebApi.Repositories.Interfaces;
using UploadFile.WebApi.Services.Interfaces;

namespace UploadFile.WebApi.Services
{
    public class DocumentService : BaseService, IDocumentService
    {
        private readonly IDocumentRepository _repo;

        public DocumentService(IHttpContextAccessor accessor, IDocumentRepository repo)
        :base(accessor)
        {
            _repo = repo;
        }

        public async Task<Document> Save(IFormFile document)
        {
            if (document == null || document.Length == 0)
            {
                throw new Exception("Arquivo Vazio");
            }

            try
            {
                using var ms = new MemoryStream();

                document.CopyTo(ms);

                var fileBytes = ms.ToArray();

                var newDocument = new Document
                {
                    UserId = UserId,
                    File = fileBytes,
                    FileName = document.FileName
                };

                return await _repo.Save(newDocument);
            }catch(Exception)
            {
                return null;
            }
        }

        public async Task<Document> Get(long id)
        {
            return await _repo.Get(id);
        }

        public async Task<List<DocumentDTO>> GetDocumentByUser()
        {
            return await _repo.GetDocumentByUser(UserId);
        }
    }
}