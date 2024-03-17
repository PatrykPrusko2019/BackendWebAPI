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

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            var isDeleted = _documentService.Delete(id);

            if (isDeleted)
            {
                return NoContent();
            }

            return NotFound();
        }

        [HttpPost]
        public ActionResult CreateDocument([FromBody] CreateDocumentDto dto)
        {
            int id = _documentService.CreateDocument(dto);
            if (id == -1) return NotFound("no found storage or provider");

            return Created($"api/document/{id}", null);
        }

        [HttpGet]
        public ActionResult<DocumentDto> GetAll()
        {
            var documentDtos = _documentService.GetAll();

            return Ok(documentDtos);
        }

        [HttpGet("{id}")]
        public ActionResult<DocumentDto> GetById([FromRoute] int id)
        {
            var documentDto = _documentService.GetById(id);

            if (documentDto is null)
            {
                return NotFound();
            }

            return Ok(documentDto);
        }


    }
}
