using System.ComponentModel.DataAnnotations;

namespace BackendWebAPI.Models
{
    public class CreateProviderDto
    {
        [Required]
        [MaxLength(40)]
        public string CompanyName { get; set; }

        public string City { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
    }
}
