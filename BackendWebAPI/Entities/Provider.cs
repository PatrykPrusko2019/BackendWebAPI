namespace BackendWebAPI.Entities
{
    public class Provider
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }

        public int AddressId { get; set; }
        public virtual Address Address { get; set; }

        public virtual List<AdmissionDocument> Documents { get; set; }
    }
}
