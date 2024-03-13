using BackendWebAPI.Models.AdmissionDocument;
using BackendWebAPI.Models.Storage;
using BackendWebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendWebAPI.Controllers
{
    [Route("api/document")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private IDocumentService _documentService;

        public DocumentController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromBody] UpdateDocumentDto dto, [FromRoute] int id)
        {
            var isUpdated = _documentService.Update(id, dto);
            if (!isUpdated) { return NotFound(); }

            return Ok();
        }

        
    }
}
