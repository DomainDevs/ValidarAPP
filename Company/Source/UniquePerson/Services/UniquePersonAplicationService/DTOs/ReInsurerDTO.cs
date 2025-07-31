using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;

namespace Sistran.Company.Application.UniquePersonAplicationServices.DTOs
{
    [DataContract]
    public class ReInsurerDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public DateTime EnteredDate { get; set; }

        [DataMember]
        public DateTime? ModifyDate { get; set; }

        [DataMember]
        public DateTime? DeclinedDate { get; set; }

        [DataMember]
        public int? DeclaredTypeCD { get; set; }

        [DataMember]
        public string Annotations { get; set; }

        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// Listado de las politicas infringidas
        /// </summary>
        [DataMember]
        public virtual List<PoliciesAut> InfringementPolicies { get; set; }

        [DataMember]
        public int OperationId { get; set; }
    }
}
