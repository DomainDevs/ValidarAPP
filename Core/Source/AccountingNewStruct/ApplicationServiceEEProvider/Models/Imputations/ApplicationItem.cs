using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{
    [DataContract]
    public class ApplicationItem
    {
        [DataMember]
        public int ApplicationId { get; set; }

        [DataMember]
        public Amount Amount { get; set; }

        [DataMember]
        public Amount LocalAmount { get; set; }

        [DataMember]
        public int CurrencyId { get; set; }

        [DataMember]
        public ExchangeRate ExchangeRate { get; set; }

        [DataMember]
        public Individual Beneficiary { get; set; }

        [DataMember]
        public AccountingConcept AccountingConcept { get; set; }
    }
}
