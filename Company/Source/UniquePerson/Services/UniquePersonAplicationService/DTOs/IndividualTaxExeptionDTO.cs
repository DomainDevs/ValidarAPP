using Sistran.Company.Application.ModelServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonAplicationServices.DTOs
{
    using Core.Application.AuthorizationPoliciesServices.Models;

    [DataContract]
    public class IndividualTaxExeptionDTO
    {
        [DataMember]
        public int CountryId { get; set; }
        [DataMember]
        public DateTime? DateUntil { get; set; }
        [DataMember]
        public DateTime Datefrom { get; set; }
        [DataMember]
        public int ExtentPercentage { get; set; }
        [DataMember]
        public int IndividualId { get; set; }
        [DataMember]
        public int IndividualTaxExemptionId { get; set; }
        [DataMember]
        public DateTime? OfficialBulletinDate { get; set; }
        [DataMember]
        public string ResolutionNumber { get; set; }
        [DataMember]
        public int StateCode { get; set; }
        [DataMember]
        public string StateCodeDescription { get; set; }
        [DataMember]
        public int TaxId { get; set; }
        [DataMember]
        public string TaxDescription { get; set; }
        [DataMember]
        public int TaxCategoryId { get; set; }
        [DataMember]
        public string TaxCategoryDescription { get; set; }
        [DataMember]
        public int TaxCondition { get; set; }
        [DataMember]
        public string TaxConditionDescription { get; set; }
        [DataMember]
        public bool TotalRetention { get; set; }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public StatusTypeService status { get; set; }

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
        public int RoleId { get; set; }
        [DataMember]
        public string RoleDescription { get; set; }
        [DataMember]
        public int TaxRateId { get; set; }
    }
}
