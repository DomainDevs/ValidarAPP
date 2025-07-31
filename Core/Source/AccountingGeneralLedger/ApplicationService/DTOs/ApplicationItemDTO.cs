using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingGeneralLedgerServices.DTOs
{
    [DataContract]
    public class ApplicationItemDTO
    {
        [DataMember]
        public int ApplicationId { get; set; }

        [DataMember]
        public AmountDTO Amount { get; set; }
        
        [DataMember]
        public AmountDTO LocalAmount { get; set; }
        
        [DataMember]
        public int CurrencyId { get; set; }

        [DataMember]
        public ExchangeRateDTO ExchangeRate { get; set; }

        [DataMember]
        public IndividualDTO Beneficiary { get; set; }

        [DataMember]
        public ApplicationAccountingConceptDTO AccountingConcept { get; set; }

        [DataMember]
        public int ApplicationAccountingId { get; set; }
    }
}
