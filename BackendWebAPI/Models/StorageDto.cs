using BackendWebAPI.Entities;

namespace BackendWebAPI.Models
{
    public class StorageDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }

        public List<AdmissionDocumentDto> Documents { get; set; }

    }
}
