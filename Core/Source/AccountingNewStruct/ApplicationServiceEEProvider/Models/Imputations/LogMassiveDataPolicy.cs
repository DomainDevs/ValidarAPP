

using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{
    [DataContract]
    public class LogMassiveDataPolicy
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int IdProcess { get; set; }

        [DataMember]
        public int TechnicalTransaction { get; set; }

        [DataMember]
        public string LogMessage { get; set; }

        [DataMember]
        public int PolicyNumber { get; set; }

        [DataMember]
        public int EndorsementNumber { get; set; }

        [DataMember]
        public int BranchId { get; set; }

        [DataMember]
        public int PrefixId { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public decimal ExchangeRate { get; set; }

        [DataMember]
        public System.DateTime DateGenerate { get; set; }
    }
}
