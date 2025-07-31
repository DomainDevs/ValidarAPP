using System.Runtime.Serialization;

namespace Sistran.Company.Application.CommonServices.Models
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class CompositionType
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string SmallDescription { get; set; }
    }
}
