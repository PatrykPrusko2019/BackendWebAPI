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
            if (dto.Price < 1 || dto.UnitPieces < 1) return -1;

            var document = _documentDbContext.Documents
                .Include(i => i.Items)
                .FirstOrDefault(d => d.Id == dto.AdmissionDocumentId);
            if (document == null) return -1;

            var product = _mapper.Map<Product>(dto);
            _documentDbContext.Products.Add(product);
          
            Item item;

            item = document.Items.FirstOrDefault(i => i.NameProduct.ToLower().Equals(product.Name.ToLower()) && i.CodeProduct.ToLower().Equals(product.Code.ToLower()));
            
            if (item == null)
            {
                item = new Item()
                {
                    NameProduct = product.Name,
                    CodeProduct = product.Code,
                    ItemsOfProducts = 1,
                    Price = product.Price
                };
                document.Items.Add(item);
            } 
            else
            {
                item.ItemsOfProducts += 1;
                item.Price += product.Price;
            }

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

            var document = _documentDbContext.Documents.Include(i => i.Items).FirstOrDefault(d => d.Id == product.AdmissionDocumentId);

            var deletedItem = document.Items.FirstOrDefault(i => i.NameProduct == product.Name && i.CodeProduct == product.Code);
            document.Items.Remove(deletedItem);

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
            if (dto.Price < 1 || dto.UnitPieces < 1) return false;

            var product = _documentDbContext
                .Products
                .FirstOrDefault(s => s.Id == id);

            if (product == null) { return false; }

            var document = _documentDbContext.Documents.Include(i => i.Items).FirstOrDefault(d => d.Id == product.AdmissionDocumentId);

            var updatedItem = document.Items.FirstOrDefault(i => i.NameProduct == product.Name && i.CodeProduct == product.Code);

            //Updated actuall item
            if (updatedItem.NameProduct == dto.Name && updatedItem.CodeProduct == dto.Code)
            {
                updatedItem.Price -= product.Price;
                updatedItem.Price += dto.Price;
            }
            else
            {
                //sprawdzamy czy jest juz taki w liscie Items
                var checkedItem = document.Items.FirstOrDefault(i => i.NameProduct == dto.Name && i.CodeProduct == dto.Code);
                if (checkedItem != null) //jesli jest juz na liscie to dodajemy do niego, i odejmujemy od obecnego
                {
                    // added to existing item
                    checkedItem.Price += dto.Price;
                    checkedItem.ItemsOfProducts += 1;

                    // remove given item
                    if (updatedItem.ItemsOfProducts == 1) document.Items.Remove(updatedItem);
                    else
                    {
                        updatedItem.Price -= product.Price;
                        updatedItem.ItemsOfProducts -= 1;
                    }
                }
                else // nie ma to tworzymy nowego, a usuwamy obecnego
                {
                    // create new item to list
                    var newItem = new Item()
                    {
                        NameProduct = dto.Name,
                        CodeProduct = dto.Code,
                        Price = dto.Price,
                        ItemsOfProducts = 1
                    };
                    document.Items.Add(newItem);

                    //remove actuall item
                    document.Items.Remove(updatedItem);
                }

            }

            //create new product
            product.Name = dto.Name;
            product.Code = dto.Code;
            product.Price = dto.Price;
            product.UnitPieces = dto.UnitPieces;

            
            _documentDbContext.SaveChanges();

            return true;
        }
    }
}
