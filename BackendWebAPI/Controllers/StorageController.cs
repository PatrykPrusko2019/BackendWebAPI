using AutoMapper;
using BackendWebAPI.Entities;
using BackendWebAPI.Models.Storage;
using BackendWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendWebAPI.Controllers
{
    [Route("api/storage")]
    [ApiController]
    public class StorageController : ControllerBase
    {
        private IStorageService _storageService;

        public StorageController(IStorageService storageService)
        {
            _storageService = storageService;
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromBody] UpdateStorageDto dto, [FromRoute]int id)
        {
            var isUpdated = _storageService.Update(id, dto);
            if (!isUpdated) { return NotFound(); }

            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id) 
        {
            var isDeleted = _storageService.Delete(id);

            if (isDeleted)
            {
                return NoContent();
            }

            return NotFound();
        }

        [HttpPost]
        public ActionResult CreateStorage([FromBody] CreateStorageDto dto)
        {
            int id = _storageService.CreateStorage(dto);

            return Created($"api/storage/{id}", null);
        }

        [HttpGet]
        public ActionResult<IEnumerable<StorageDto>> GetAll()
        {
            var storageDtos = _storageService.GetAll();

            return Ok(storageDtos);
        }

        [HttpGet("{id}")]
        public ActionResult<StorageDto> GetById([FromRoute] int id)
        {
            var storageDto = _storageService.GetById(id);

            if (storageDto is null)
            {
                return NotFound();
            }

            return Ok(storageDto);
        }
    }
}
