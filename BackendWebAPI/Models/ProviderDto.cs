using BackendWebAPI.Entities;

namespace BackendWebAPI.Models
{
    public class ProviderDto
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }

        public string City { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
    }
}
