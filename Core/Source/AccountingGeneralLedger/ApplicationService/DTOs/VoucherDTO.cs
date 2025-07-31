using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.AccountingGeneralLedgerServices.DTOs
{
    [DataContract]
    public class VoucherDTO
    {
        [DataMember]
        public List<ConceptDTO> Concepts { get; set; }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int CurrencyId { get; set; }

        [DataMember]
        public decimal ExchangeRate { get; set; }

    }
}
