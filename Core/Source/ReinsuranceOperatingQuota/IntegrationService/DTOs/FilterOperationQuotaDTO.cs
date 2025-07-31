using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Integration.ReinsuranceOperatingQuotaServices.DTOs
{
    [DataContract]
    public class FilterOperationQuotaDTO
    {
        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public int LineBusiness { get; set; }

        [DataMember]
        public int SubLineBusiness { get; set; }

        [DataMember]
        public DateTime DateCumulus { get; set; }

        [DataMember]
        public bool IsFuture { get; set; }

        [DataMember]
        public bool IsDatePolicy { get; set; }
        [DataMember]
        public int PrefixCd { get; set; }
    }
}
