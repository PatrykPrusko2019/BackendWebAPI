using BackendWebAPI.Entities;
using BackendWebAPI.Models.AdmissionDocument;

namespace BackendWebAPI.Models.Storage
{
    public class StorageDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }

        public List<AdmissionDocumentDto> Documents { get; set; }

    }
}
