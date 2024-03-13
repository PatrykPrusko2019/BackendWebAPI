using AutoMapper;
using BackendWebAPI.Entities;
using BackendWebAPI.Models.AdmissionDocument;
using BackendWebAPI.Models.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Reflection.Metadata;

namespace BackendWebAPI.Services
{
    public interface IDocumentService
    {
        int CreateDocument(CreateDocumentDto dto);
        bool Delete(int id);
        List<DocumentDto> GetAll();
        DocumentDto GetById(int id);
        bool Update(int id, UpdateDocumentDto dto);
    }
    public class DocumentService : IDocumentService
    {
        private readonly DocumentDbContext _documentDbContext;
        private readonly IMapper _mapper;

        public DocumentService(DocumentDbContext documentDbContext, IMapper mapper)
        {
            _documentDbContext = documentDbContext;
            _mapper = mapper;
        }

        public int CreateDocument(CreateDocumentDto dto)
        {
            var storage = _documentDbContext.Storages.FirstOrDefault(s => s.Name.ToLower().Equals(dto.TargetWarehouse.ToLower()));
            if (storage == null) return -1;
            var provider = _documentDbContext.Providers.FirstOrDefault(p => p.CompanyName.ToLower().Equals(dto.Vendor.ToLower()));
            if (provider == null) return -1;

            var document = _mapper.Map<AdmissionDocument>(dto);
            document.ProviderId = provider.Id;
            document.StorageId = storage.Id;

            _documentDbContext.Documents.Add(document);
            _documentDbContext.SaveChanges();

            return document.Id;
        }

        public bool Delete(int id)
        {
            var document = _documentDbContext
                .Documents
                .Include(p => p.Products)
                .Include(l => l.Labels)
                .FirstOrDefault(s => s.Id == id);

            if (document is null)
            {
                return false;
            }

            _documentDbContext.Documents.Remove(document);
            _documentDbContext.SaveChanges();

            return true;
        }

        public List<DocumentDto> GetAll()
        {
            var documents = _documentDbContext
                .Documents
                .Include(p => p.Products)
                .Include(l => l.Labels)
                .ToList();

            var documentDtos = _mapper.Map<List<DocumentDto>>(documents);

            //for (int i = 0; i < documentDtos.Count(); i++ ) 
            //{
            //    documentDtos[i].Products = _mapper.Map
            //}

            return documentDtos;
        }

        public DocumentDto GetById(int id)
        {
            var document = _documentDbContext
                .Documents
                .Include(p => p.Products)
                .Include(l => l.Labels)
                .FirstOrDefault(s => s.Id == id);

            if (document is null)
            {
                return null;
            }

            var documentDto = _mapper.Map<DocumentDto>(document);
            return documentDto;
        }

        public bool Update(int id, UpdateDocumentDto dto)
        {
            var document = _documentDbContext
                .Documents
                .FirstOrDefault(s => s.Id == id);

            if (document == null || ( dto.TargetWarehouse.Trim().IsNullOrEmpty() && dto.Vendor.Trim().IsNullOrEmpty() ) ) { return false; }
            
            bool differentStorage = !document.TargetWarehouse.ToLower().Equals(dto.TargetWarehouse.ToLower());
            var updateStorage = default(Storage);
            if (differentStorage)
            {
                updateStorage = _documentDbContext.Storages.FirstOrDefault(s => s.Name.ToLower().Equals(dto.TargetWarehouse.ToLower()));
                if (updateStorage != null)
                {
                    var actuallStorage = _documentDbContext.Storages.FirstOrDefault(s => s.Id == document.StorageId);
                    actuallStorage.Documents.Remove(document);

                    if (updateStorage.Documents.IsNullOrEmpty()) updateStorage.Documents = new List<AdmissionDocument>();
                }

            }

            bool differentProvider = !document.Vendor.ToLower().Equals(dto.Vendor.ToLower());
            var updateProvider = default(Provider);
            if (differentProvider)
            {
                updateProvider = _documentDbContext.Providers.FirstOrDefault(p => p.CompanyName.ToLower().Equals(dto.Vendor.ToLower()));
                if (updateProvider != null)
                {
                    var actuallProvider = _documentDbContext.Providers.FirstOrDefault(s => s.Id == document.ProviderId);
                    actuallProvider.Documents.Remove(document);

                    if (updateProvider.Documents.IsNullOrEmpty()) updateProvider.Documents = new List<AdmissionDocument>();
                }

            }

            if (updateStorage == null && updateProvider == null) return false;

            if (updateStorage != null)
            {
                document.StorageId = updateStorage.Id;
                document.TargetWarehouse = updateStorage.Name;
            }

            if (updateProvider != null)
            {
                document.ProviderId = updateProvider.Id;
                document.Vendor = updateProvider.CompanyName;
            }

            if (updateStorage != null) updateStorage.Documents.Add(document);
            if (updateProvider != null) updateProvider.Documents.Add(document);

            _documentDbContext.SaveChanges();

            return true;
        }
    }
}
