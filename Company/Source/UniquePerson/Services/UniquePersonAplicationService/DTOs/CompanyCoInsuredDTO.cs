using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;

namespace Sistran.Company.Application.UniquePersonAplicationServices.DTOs
{
    [DataContract]
    public class CompanyCoInsuredDTO
    {
        [DataMember]
        public int IndividualId { get; set; }
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

        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// InfringementPolicies
        /// </summary>
        [DataMember]
        public virtual List<PoliciesAut> InfringementPolicies { get; set; }

        [DataMember]
        public int OperationId { get; set; }
        
        [DataMember]
        public decimal IvaTypeCode { get; set; }
    }
}
