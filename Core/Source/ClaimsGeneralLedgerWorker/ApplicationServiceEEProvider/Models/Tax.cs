using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimsGeneralLedgerWorkerServices.EEProvider.Models
{
    [DataContract]
    public class Tax
    {
        [DataMember]
        public int TaxId { get; set; }

        [DataMember]
        public int TaxValues { get; set; }
    }
}
