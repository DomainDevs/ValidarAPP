using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.OperationQuotaServices.EEProvider.Models.OperationQuota
{
    [DataContract]
    public class Base
    {
        [DataMember]
        public decimal? Value { get; set; }

        [DataMember]
        public int? Qualification { get; set; }

        [DataMember]
        public decimal? Weighted { get; set; }

        [DataMember]
        public decimal? Score { get; set; }

        [DataMember]
        public int? YearsOfConstitution { get; set; }

        [DataMember]
        public decimal? FinancialScore { get; set; }

    }
}
