using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Integration.UndewritingIntegrationServices.DTOs.Base
{
    [DataContract]
    public class PolicyBaseDTO
    {
        [DataMember]
        public DateTime FromDate { get; set; }

        [DataMember]
        public DateTime ToDate { get; set; }
        [DataMember]
        public int Currency { get; set; }
        [DataMember]
        public decimal ExchangeRate { get; set; }
        [DataMember]
        public decimal CoinsurancePct { get; set; }

    }
}
