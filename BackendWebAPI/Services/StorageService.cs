using AutoMapper;
using BackendWebAPI.Entities;
using BackendWebAPI.Models.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendWebAPI.Services
{
    public interface IStorageService
    {
        bool Update(int id, UpdateStorageDto dto);
        bool Delete(int id);
        int CreateStorage(CreateStorageDto dto);
        StorageDto GetById(int id);
        List<StorageDto> GetAll();
    }

    public class StorageService : IStorageService
    {
        private readonly DocumentDbContext _documentDbContext;
        private readonly IMapper _mapper;

        public StorageService(DocumentDbContext documentDbContext, IMapper mapper)
        {
            _documentDbContext = documentDbContext;
            _mapper = mapper;
        }

        public bool Update(int id, UpdateStorageDto dto)
        {
            var storage = _documentDbContext
                .Storages
                .FirstOrDefault(s => s.Id == id);

            if (storage == null) { return false; }

            storage.Name = dto.Name;
            storage.Symbol = dto.Symbol;
            _documentDbContext.SaveChanges();

            return true;
        }

        public int CreateStorage( CreateStorageDto dto)
        {
            var storage = _mapper.Map<Storage>(dto);
            _documentDbContext.Storages.Add(storage);
            _documentDbContext.SaveChanges();

            return storage.Id;
        }

        public bool Delete(int id)
        {
            var storage = _documentDbContext
                .Storages
                .Include(d => d.Documents)
                .FirstOrDefault(s => s.Id == id);

            if (storage is null)
            {
                return false;
            }

            _documentDbContext.Storages.Remove(storage);
            _documentDbContext.SaveChanges();

            return true;
        }

        public List<StorageDto> GetAll()
        {
            var storages = _documentDbContext
                .Storages
                .Include(d => d.Documents)
                .ToList();

            var storageDtos = _mapper.Map<List<StorageDto>>(storages);
            return storageDtos;
        }

        public StorageDto GetById([FromRoute] int id)
        {
            var storage = _documentDbContext
                .Storages
                .Include(d => d.Documents)
                .FirstOrDefault(s => s.Id == id);

            if (storage is null)
            {
                return null;
            }

            var storageDto = _mapper.Map<StorageDto>(storage);
            return storageDto;
        }
    }
}
