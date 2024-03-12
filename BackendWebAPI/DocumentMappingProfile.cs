using AutoMapper;
using BackendWebAPI.Entities;
using BackendWebAPI.Models.AdmissionDocument;
using BackendWebAPI.Models.Product;
using BackendWebAPI.Models.Provider;
using BackendWebAPI.Models.Storage;

namespace BackendWebAPI
{
    public class DocumentMappingProfile : Profile
    {
        public DocumentMappingProfile()
        {
            CreateMap<Storage, StorageDto>();

            CreateMap<CreateStorageDto, Storage>();

            CreateMap<AdmissionDocument, AdmissionDocumentDto>();

            CreateMap<Provider, ProviderDto>()
                .ForMember(m => m.City, c => c.MapFrom(s => s.Address.City))
                .ForMember(m => m.Street, c => c.MapFrom(s => s.Address.Street))
                .ForMember(m => m.ZipCode, c => c.MapFrom(s => s.Address.ZipCode));

            CreateMap<CreateProviderDto, Provider>()
                .ForMember(p => p.Address,
                c => c.MapFrom(dto => new Address()
                {City = dto.City, ZipCode = dto.ZipCode, Street = dto.Street }));

            CreateMap<Product, ProductDto>();

            CreateMap<CreateProductDto, Product>();
        }
    }
}
