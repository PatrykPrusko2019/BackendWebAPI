namespace BackendWebAPI.Models.Item
{
    public class ItemDto
    {
        public int Id { get; set; }
        public string NameProduct { get; set; }
        public string CodeProduct { get; set; }
        public int ItemsOfProducts { get; set; }
        public double Price { get; set; }

        public int AdmissionDocumentId { get; set; }
    }
}
