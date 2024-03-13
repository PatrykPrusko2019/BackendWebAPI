using AutoMapper;
using BackendWebAPI.Entities;
using BackendWebAPI.Models.AdmissionDocument;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;

namespace BackendWebAPI.Services
{
    public interface IDocumentService
    {
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
