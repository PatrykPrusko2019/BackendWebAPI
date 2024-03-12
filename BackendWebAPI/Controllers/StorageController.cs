using AutoMapper;
using BackendWebAPI.Entities;
using BackendWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendWebAPI.Controllers
{
    [Route("api/storage")]
    [ApiController]
    public class StorageController : ControllerBase
    {
        private readonly DocumentDbContext _documentDbContext;
        private readonly IMapper _mapper;

        public StorageController(DocumentDbContext documentDbContext ,IMapper mapper)
        {
            _documentDbContext = documentDbContext;
            _mapper = mapper;
        }

        [HttpPost]
        public ActionResult CreateStorage([FromBody] CreateStorageDto dto)
        {
            var storage = _mapper.Map<Storage>(dto);
            _documentDbContext.Storages.Add(storage);
            _documentDbContext.SaveChanges();

            return Created($"api/storage/{storage.Id}", null);
        }

        [HttpGet]
        public ActionResult<IEnumerable<StorageDto>> GetAll()
        {
            var storages = _documentDbContext
                .Storages
                .Include(d => d.Documents)
                .ToList();

            var storageDtos = _mapper.Map<List<StorageDto>>(storages);

            return Ok(storageDtos);
        }

        [HttpGet("{id}")]
        public ActionResult<StorageDto> GetById([FromRoute] int id)
        {
            var storage = _documentDbContext
                .Storages
                .Include(d => d.Documents)
                .FirstOrDefault(s => s.Id == id);

            if (storage is null)
            {
                return NotFound();
            }

            var storageDto = _mapper.Map<StorageDto>(storage);
            return Ok(storageDto);
        }
    }
}
