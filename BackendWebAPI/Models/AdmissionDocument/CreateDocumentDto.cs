namespace BackendWebAPI.Models.AdmissionDocument
{
    public class CreateDocumentDto
    {
        public string TargetWarehouse { get; set; }
        public int ProviderId { get; set; }
        public string LabelNames { get; set; }
    }
}
