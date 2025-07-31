using Sistran.Company.Application.CommonAplicationServices.Enums;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniquePersonAplicationServices.DTOs
{
    [DataContract]
    public class InsuredDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public int? AgentId { get; set; }

        [DataMember]
        public string IdDescription { get; set; }

        [DataMember]
        public int? AgencyId { get; set; }

        [DataMember]
        public int? BranchId { get; set; }

        [DataMember]
        public int? InsProfileId { get; set; }

        [DataMember]
        public int? InsSegmentId { get; set; }

        [DataMember]
        public string Annotations { get; set; }

        [DataMember]
        public DateTime EnteredDate { get; set; }

        [DataMember]
        public DateTime? ModifyDate { get; set; }

        [DataMember]
        public DateTime UpdateDate { get; set; }

        [DataMember]
        public DateTime? DeclinedDate { get; set; }

        [DataMember]
        public int? InsDeclinesTypeId { get; set; }

        [DataMember]
        public bool? IsBeneficiary { get; set; }

        [DataMember]
        public bool? IsHolder { get; set; }

        [DataMember]
        public bool? IsPayer { get; set; }

        [DataMember]
        public bool? IsInsured { get; set; }


        [DataMember]
        public bool IsSms { get; set; }


        [DataMember]
        public bool IsMailAddress { get; set; }

        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// Listado de las politicas infringidas
        /// </summary>
        [DataMember]
        public virtual List<PoliciesAut> InfringementPolicies { get; set; }

        [DataMember]
        public int OperationId { get; set; }

        [DataMember]
        public bool ElectronicBiller { get; set; }

        [DataMember]
        public bool RegimeType { get; set; }
    }
}
