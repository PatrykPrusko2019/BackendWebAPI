using AutoMapper;
using BackendWebAPI.Entities;
using BackendWebAPI.Models.Product;
using BackendWebAPI.Models.Storage;
using Microsoft.EntityFrameworkCore;

namespace BackendWebAPI.Services
{
    public interface IProductService
    {
        int CreateProduct(CreateProductDto dto);
        bool Delete(int id);
        List<ProductDto> GetAll();
        ProductDto GetById(int id);
        bool Update(int id, UpdateProductDto dto);
    }
    public class ProductService : IProductService
    {
        private readonly DocumentDbContext _documentDbContext;
        private readonly IMapper _mapper;

        public ProductService(DocumentDbContext documentDbContext, IMapper mapper)
        {
            _documentDbContext = documentDbContext;
            _mapper = mapper;
        }

        public int CreateProduct(CreateProductDto dto)
        {
            var document = _documentDbContext.Documents.FirstOrDefault(d => d.Id == dto.AdmissionDocumentId);
            if (document == null) return -1;

            var product = _mapper.Map<Product>(dto);
            _documentDbContext.Products.Add(product);
            _documentDbContext.SaveChanges();

            return product.Id;
        }

        public bool Delete(int id)
        {
            var product = _documentDbContext
                .Products
                .FirstOrDefault(s => s.Id == id);

            if (product is null)
            {
                return false;
            }

            _documentDbContext.Products.Remove(product);
            _documentDbContext.SaveChanges();

            return true;
        }

        public List<ProductDto> GetAll()
        {
            var products = _documentDbContext
                .Products
                .ToList();

            var productDtos = _mapper.Map<List<ProductDto>>(products);
            return productDtos;
        }

        public ProductDto GetById(int id)
        {
            var product = _documentDbContext
                .Products
                .FirstOrDefault(s => s.Id == id);

            if (product is null)
            {
                return null;
            }

            var productDto = _mapper.Map<ProductDto>(product);
            return productDto;
        }

        public bool Update(int id, UpdateProductDto dto)
        {
            var product = _documentDbContext
                .Products
                .FirstOrDefault(s => s.Id == id);

            if (product == null) { return false; }

            product.Name = dto.Name;
            product.Code = dto.Code;
            product.Price = dto.Price;
            product.UnitPieces = dto.UnitPieces;
            _documentDbContext.SaveChanges();

            return true;
        }
    }
}
