using AutoMapper;
using BackendWebAPI.Entities;
using BackendWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BackendWebAPI.Controllers
{
    [Route("api/provider")]
    [ApiController]
    public class ProviderController : ControllerBase
    {
        private readonly DocumentDbContext _documentDbContext;
        private readonly IMapper _mapper;

        public ProviderController(DocumentDbContext documentDbContext, IMapper mapper)
        {
            _documentDbContext = documentDbContext;
            _mapper = mapper;
        }

        [HttpPost]
        public ActionResult CreateProvider([FromBody] CreateProviderDto dto)
        {
            var provider = _mapper.Map<Provider>(dto);
            _documentDbContext.Providers.Add(provider);
            _documentDbContext.SaveChanges();

            return Created($"api/provider/{provider.Id}", null);
        }
    }
}
