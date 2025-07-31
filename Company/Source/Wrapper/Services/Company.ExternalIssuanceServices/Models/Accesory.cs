using System.Runtime.Serialization;

namespace Sistran.Company.ExternalIssuanceServices.Models
{
    [DataContract]
    public class Accesory
    {
        public int DetailCode { get; set; }
        public string MakeDescription { get; set; }
        public string ModelDescription { get; set; }
        public decimal Value { get; set; }
        public bool IsOriginal { get; set; }

    }
}