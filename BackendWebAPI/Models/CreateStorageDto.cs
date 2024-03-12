using System.ComponentModel.DataAnnotations;

namespace BackendWebAPI.Models
{
    public class CreateStorageDto
    {
        [Required]
        [MaxLength(40)]
        public string Name { get; set; }
        public string Symbol { get; set; }
    }
}
