using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingGeneralLedgerServices.DTOs
{
    [DataContract]
    public class IndividualDTO
    {
        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public EconomicActivityDTO EconomicActivity { get; set; }

        [DataMember]
        public IdentificationDocumentDTO IdentificationDocument { get; set; }
    }
}
