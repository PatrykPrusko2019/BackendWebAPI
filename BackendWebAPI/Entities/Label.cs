namespace BackendWebAPI.Entities
{
    public class Label
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual List<AdmissionDocument> Documents { get; set; }
    }
}
