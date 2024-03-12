using AutoMapper;
using BackendWebAPI.Entities;
using BackendWebAPI.Models.Provider;
using BackendWebAPI.Models.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendWebAPI.Services
{
    public interface IProviderService
    {
        bool Update(int id, UpdateProviderDto dto);
        int CreateProvider(CreateProviderDto dto);
        bool Delete(int id);
        List<ProviderDto> GetAll();
        ProviderDto GetById(int id);
    }

    public class ProviderService : IProviderService
    {
        private readonly DocumentDbContext _documentDbContext;
        private readonly IMapper _mapper;

        public ProviderService(DocumentDbContext documentDbContext, IMapper mapper)
        {
            _documentDbContext = documentDbContext;
            _mapper = mapper;
        }

        public bool Update(int id, UpdateProviderDto dto)
        {
            var provider = _documentDbContext
                .Providers
                .FirstOrDefault(s => s.Id == id);

            if (provider == null) { return false; }

            provider.CompanyName = dto.CompanyName;
            provider.Address.City = dto.City;
            provider.Address.Street = dto.Street;
            provider.Address.ZipCode = dto.ZipCode;
            _documentDbContext.SaveChanges();

            return true;
        }

        public int CreateProvider(CreateProviderDto dto)
        {
            var provider = _mapper.Map<Provider>(dto);
            _documentDbContext.Providers.Add(provider);
            _documentDbContext.SaveChanges();

            return provider.Id;
        }

        public bool Delete(int id)
        {
            var provider = _documentDbContext
                .Providers
                .Include(d => d.Documents)
                .FirstOrDefault(s => s.Id == id);

            if (provider is null)
            {
                return false;
            }

            _documentDbContext.Providers.Remove(provider);
            _documentDbContext.SaveChanges();

            return true;
        }

        public List<ProviderDto> GetAll()
        {
            var providers = _documentDbContext
                .Providers
                .Include(d => d.Documents)
                .Include(a => a.Address)
                .ToList();

            var providerDtos = _mapper.Map<List<ProviderDto>>(providers);
            return providerDtos;
        }

        public ProviderDto GetById([FromRoute] int id)
        {
            var provider = _documentDbContext
                .Providers
                .Include(d => d.Documents)
                .Include(a => a.Address)
                .FirstOrDefault(s => s.Id == id);

            if (provider is null)
            {
                return null;
            }

            var providerDto = _mapper.Map<ProviderDto>(provider);
            return providerDto;
        }
    }
}
