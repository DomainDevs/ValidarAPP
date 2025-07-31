using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.ClaimsGeneralLedgerWorkerServices.DTOs
{
    [DataContract]
    public class ConceptDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public int AccountingAccountId { get; set; }

        [DataMember]
        public string AccountingAccountNumber { get; set; }

        [DataMember]
        public int AccountingNatureId { get; set; }

        [DataMember]
        public List<TaxDTO> Taxes { get; set; }
    }
}
