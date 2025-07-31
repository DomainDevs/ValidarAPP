using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace Sistran.Company.Application.OperationQuotaServices.DTOs
{
    public class BaseDTO
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
