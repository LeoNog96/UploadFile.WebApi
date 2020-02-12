using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UploadFile.WebApi.DTOs;
using UploadFile.WebApi.Models;
using UploadFile.WebApi.Services.Interfaces;

namespace UploadFile.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _service;

        public DocumentController(IDocumentService service)
        {
            _service = service;
        }

        [HttpPost("upload")]
        public async Task<ActionResult> Post ([FromForm] IFormFile std)
        {
            var document = await _service.Save(std);

            if(document == null)
            {
                return BadRequest("Erro ao fazer upload de arquivo");
            }

            return Ok();
        }

        [HttpGet("download/{id}")]
        public async Task<IActionResult> Download(long id)
        {
            var document = await _service.Get(id);

            var memory = new MemoryStream(document.File);

            return File(memory, "application/octet-stream", document.FileName);
        }

        [HttpGet("by-user")]
        public async Task<ActionResult<List<DocumentDTO>>> GetByUser()
        {
            var document = await _service.GetDocumentByUser();

            if (document.Count == 0)
            {
                return NotFound();
            }

            return document;
        }
    }
}