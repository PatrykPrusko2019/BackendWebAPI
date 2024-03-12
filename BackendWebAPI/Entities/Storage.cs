using System.Net.Sockets;

namespace BackendWebAPI.Entities
{
    public class Storage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }

        public virtual List<AdmissionDocument> Documents { get; set; }
    }
}
