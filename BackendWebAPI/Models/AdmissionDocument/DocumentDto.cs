using BackendWebAPI.Entities;
using BackendWebAPI.Models.Label;
using BackendWebAPI.Models.Product;

namespace BackendWebAPI.Models.AdmissionDocument
{
    public class DocumentDto
    {
        public int Id { get; set; }
        public string TargetWarehouse { get; set; }
        public string Vendor { get; set; }
        public int ProviderId { get; set; }
        public int StorageId { get; set; }
        public List<ProductDto> Products { get; set; }

        public List<LabelDto> Labels { get; set; }
    }
}
