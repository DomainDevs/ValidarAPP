using System.Runtime.Serialization;
using Sistran.Core.Application.AccountingServices.DTOs;

namespace Sistran.Core.Application.AccountingServices.DTOs
{
    [DataContract]
    public class EndorsementDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int EndorsementReasonId { get; set; }
        [DataMember]
        public int Number { get; set; }
    }
}
