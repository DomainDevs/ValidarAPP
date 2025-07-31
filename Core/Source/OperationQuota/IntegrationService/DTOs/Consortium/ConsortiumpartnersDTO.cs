using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.OperationQuotaServices.DTOs.Consortium
{
    [DataContract]
    public class ConsortiumpartnersDTO
    {

        [DataMember]
        public int IndividualConsortiumId { get; set; }

        [DataMember]
        public int IndividualPartnerId { get; set; }

        [DataMember]
        public string PartnerName { get; set; }

        [DataMember]
        public int ConsortiumId { get; set; }

        [DataMember]
        public DateTime InitDate { get; set; }

        [DataMember]
        public DateTime EndDate { get; set; }

        [DataMember]
        public decimal ParticipationRate { get; set; }

        [DataMember]
        public bool Enabled { get; set; }
    }
}
