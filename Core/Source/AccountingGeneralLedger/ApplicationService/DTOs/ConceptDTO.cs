using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.AccountingGeneralLedgerServices.DTOs
{
    [DataContract]
    public class ConceptDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public List<TaxDTO> Taxes { get; set; }
    }
}
