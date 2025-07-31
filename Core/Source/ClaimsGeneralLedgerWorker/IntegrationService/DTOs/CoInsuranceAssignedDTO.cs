using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.ClaimsGeneralLedgerWorkerServices.DTOs
{
    [DataContract]
    public class CoInsuranceAssignedDTO
    {
        [DataMember]
        public int EndorsementId { get; set; }

        [DataMember]
        public int PolicyId { get; set; }

        [DataMember]
        public int InsuranceCompanyId { get; set; }

        [DataMember]
        public decimal PartCiaPercentage { get; set; }

        [DataMember]
        public decimal ExpensesPercentage { get; set; }

        [DataMember]
        public int CompanyNum { get; set; }

        [DataMember]
        public int AccountingAccountId { get; set; }

        [DataMember]
        public string AccountingAccountNumber { get; set; }

        [DataMember]
        public int AccountingNatureId { get; set; }
    }
}
