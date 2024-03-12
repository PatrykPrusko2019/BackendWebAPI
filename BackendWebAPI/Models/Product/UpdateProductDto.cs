namespace BackendWebAPI.Models.Product
{
    public class UpdateProductDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public double Price { get; set; }
        public int UnitPieces { get; set; }
    }
}
