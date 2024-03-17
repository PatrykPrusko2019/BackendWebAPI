namespace BackendWebAPI.Models.AdmissionDocument
{
    public class UpdateDocumentDto
    {
        public string TargetWarehouse { get; set; }
        public string Vendor { get; set; }
        public string ApprovedDocument { get; set; }
        public string LabelNames { get; set; }
    }
}
