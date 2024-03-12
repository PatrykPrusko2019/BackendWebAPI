using BackendWebAPI.Models.Product;
using BackendWebAPI.Models.Storage;
using BackendWebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendWebAPI.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPut]
        public ActionResult Update([FromBody] UpdateProductDto dto, [FromRoute] int id)
        {
            var isUpdated = _productService.Update(id, dto);
            if (!isUpdated) { return NotFound(); }

            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            var isDeleted = _productService.Delete(id);

            if (isDeleted)
            {
                return NoContent();
            }

            return NotFound();
        }

        [HttpPost]
        public ActionResult CreateProduct([FromBody] CreateProductDto dto)
        {
            int id = _productService.CreateProduct(dto);

            return Created($"api/product/{id}", null);
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProductDto>> GetAll()
        {
            var productDtos = _productService.GetAll();

            return Ok(productDtos);
        }

        [HttpGet("{id}")]
        public ActionResult<ProductDto> GetById([FromRoute] int id)
        {
            var productDto = _productService.GetById(id);

            if (productDto is null)
            {
                return NotFound();
            }

            return Ok(productDto);
        }
    }
}
