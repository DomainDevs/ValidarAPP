using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.ClaimsGeneralLedgerWorkerServices.DTOs
{
    [DataContract]
    public class IndividualDTO
    {
        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}
