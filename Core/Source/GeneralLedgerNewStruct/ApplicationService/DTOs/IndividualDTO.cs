using System.Runtime.Serialization;

namespace Sistran.Core.Application.GeneralLedgerServices.DTOs
{
    [DataContract]
    public class IndividualDTO
    {
        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}
