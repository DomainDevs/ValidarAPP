using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    [DataContract]
    public class DamageResponsability
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public bool IsEnabled { get; set; }
    }
}
