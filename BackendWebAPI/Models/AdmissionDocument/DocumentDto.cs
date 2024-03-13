using BackendWebAPI.Entities;

namespace BackendWebAPI.Models.AdmissionDocument
{
    public class DocumentDto
    {
        public int Id { get; set; }
        public string TargetWarehouse { get; set; }
        public string Vendor { get; set; }
        public int ProviderId { get; set; }
        public int StorageId { get; set; }
    }
}
