using BackendWebAPI.Models.Provider;
using BackendWebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendWebAPI.Controllers
{
    [Route("api/provider")]
    [ApiController]
    public class ProviderController : ControllerBase
    {
        private IProviderService _providerService;

        public ProviderController(IProviderService providerService)
        {
            _providerService = providerService;
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            var isDeleted = _providerService.Delete(id);

            if (isDeleted)
            {
                return NoContent();
            }

            return NotFound();
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromBody] UpdateProviderDto dto, [FromRoute] int id)
        {
            var isUpdated = _providerService.Update(id, dto);
            if (!isUpdated) { return NotFound(); }

            return Ok();
        }

        [HttpPost]
        public ActionResult CreateProvider([FromBody] CreateProviderDto dto)
        {
            int id = _providerService.CreateProvider(dto);

            return Created($"api/provider/{id}", null);
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProviderDto>> GetAll()
        {
            var providerDtos = _providerService.GetAll();

            return Ok(providerDtos);
        }

        [HttpGet("{id}")]
        public ActionResult<ProviderDto> GetById([FromRoute] int id)
        {
            var providerDto = _providerService.GetById(id);

            if (providerDto is null)
            {
                return NotFound();
            }

            return Ok(providerDto);
        }
    }
}
