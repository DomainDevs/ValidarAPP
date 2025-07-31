using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.AccountingGeneralLedgerServices.DTOs
{
    [DataContract]
    public class TaxDTO
    {
        [DataMember]
        public int TaxId { get; set; }

        [DataMember]
        public decimal TaxValues { get; set; }
    }
}
