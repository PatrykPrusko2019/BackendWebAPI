namespace BackendWebAPI.Entities
{
    public class AdmissionDocument
    {
        public int Id { get; set; }
        public string TargetWarehouse { get; set; }
        public string Vendor { get; set; }
        public int ProviderId { get; set; }
        public virtual Provider Provider { get; set; }

        public virtual List<Product> Products { get; set; }

        public int StorageId { get; set; }
        public virtual Storage Storage { get; set; }

        public virtual List<Label> Labels { get; set; }


    }
}
