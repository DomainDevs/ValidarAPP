using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimsGeneralLedgerWorkerServices.EEProvider.Models
{
    [DataContract]
    public class Concept
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public List<Tax> Taxes { get; set; }
    }
}
