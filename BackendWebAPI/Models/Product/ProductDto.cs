namespace BackendWebAPI.Models.Product
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public double Price { get; set; }
        public int UnitPieces { get; set; }
        public int AdmissionDocumentId { get; set; }
    }
}
