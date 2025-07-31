using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UniquePersonService.V1.Models.Base
{
    [DataContract]
    public class BaseCoinsured:Individual
    {
        
        [DataMember]
        public int AddressTypeCode { get; set; }
        [DataMember]
        public string Annotations { get; set; }
        [DataMember]
        public int CityCode { get; set; }
        [DataMember]
        public int CountryCode { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public bool EnsureInd { get; set; }
        [DataMember]
        public DateTime? EnteredDate { get; set; }
        [DataMember]
        public decimal InsuraceCompanyId { get; set; }
        [DataMember]
        public DateTime? ModifyDate { get; set; }
        [DataMember]
        public string PhoneNumber { get; set; }
        [DataMember]
        public int PhoneTypeCode { get; set; }
        [DataMember]
        public int StateCode { get; set; }
        [DataMember]
        public string Street { get; set; }
        [DataMember]
        public string TributaryIdNo { get; set; }
        [DataMember]
        public int? ComDeclinedTypeCode { get; set; }
        [DataMember]
        public DateTime? DeclinedDate { get; set; }
    }
}
