using AutoMapper;
using BackendWebAPI.Entities;
using BackendWebAPI.Models.AdmissionDocument;
using BackendWebAPI.Models.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Extensions.Msal;
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
            var provider = _documentDbContext.Providers.FirstOrDefault(p => p.Id == dto.ProviderId);
            if (provider == null) return -1;


            if (dto.TargetWarehouse.Contains(';'))
            {
                string[] strings = dto.TargetWarehouse.Split(";");
                if (strings.Length != 2) return -1;

                //created new storage
                var storage1 = new Entities.Storage();
                storage1.Name = strings[0];
                storage1.Symbol = strings[1];
                _documentDbContext.Storages.Add(storage1);
                _documentDbContext.SaveChanges();
            }
            else if (!dto.TargetWarehouse.IsNullOrEmpty())
            {
                var storage2 = _documentDbContext.Storages.FirstOrDefault(s => s.Name.ToLower().Equals(dto.TargetWarehouse.ToLower()));
                if (storage2 == null)
                {
                    //created new storage
                    storage2 = new Entities.Storage();
                    storage2.Name = dto.TargetWarehouse;
                    if (dto.TargetWarehouse.Length > 4) storage2.Symbol = dto.TargetWarehouse.Substring(dto.TargetWarehouse.Length - (dto.TargetWarehouse.Length / 2));
                    else storage2.Symbol = "tooShort";
                    _documentDbContext.Storages.Add(storage2);
                    _documentDbContext.SaveChanges();
                }
            }

            var storage = _documentDbContext.Storages.FirstOrDefault(s => s.Name.ToLower().Equals(dto.TargetWarehouse.ToLower()));
            if (storage == null) return -1;


            var document = _mapper.Map<AdmissionDocument>(dto);
            document.TargetWarehouse = dto.TargetWarehouse;
            document.Vendor = provider.CompanyName;
            document.ProviderId = provider.Id;
            document.StorageId = storage.Id;
            document.ApprovedDocument = "WAIT";
            document.LabelNames = "";
            document.Labels = new List<Label>();

            //create new Labels
            if (!dto.LabelNames.IsNullOrEmpty())
            {
                dto.LabelNames = dto.LabelNames.Trim();
                if (dto.LabelNames.Contains(';'))
                {
                    string[] stringsLabels = dto.LabelNames.Split(';');
                    for (int i = 0; i < stringsLabels.Length; i++) 
                    {
                        document.Labels.Add(new Label() { Name = stringsLabels[i] });
                    }
                }
                else
                {
                    document.Labels.Add(new Label() { Name = dto.LabelNames });
                }
            }

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
            var updateStorage = default(Entities.Storage);
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
