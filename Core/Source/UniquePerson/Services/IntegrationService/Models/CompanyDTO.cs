using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Sistran.Core.Application.Extensions;

namespace Sistran.Core.Application.UniquePerson.IntegrationService.Models
{
    public class CompanyDTO : Extension
    {
        /// <summary>
        /// CountryId
        /// </summary>
        [DataMember]
        public int CountryId { get; set; }

        /// <summary>
        /// VerifyDigit
        /// </summary>
        public int? VerifyDigit { get; set; }

        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public string FullName { get; set; }

        /// <summary>
        /// Tipo de individuo
        /// </summary>
        [DataMember]
        public IndividualTypeDTO IndividualType { get; set; }


        /// <summary>
        /// Tipo Cliente
        /// </summary>
        [DataMember]
        public CustomerTypeDTO CustomerType { get; set; }

        /// <summary>
        /// Tipo Cliente
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// CustomerTypeDescription
        /// </summary>
        [DataMember]
        public string CustomerTypeDescription { get; set; }

        /// <summary>
        /// role
        /// </summary>
        public RoleDTO Role { get; set; }

        /// <summary>
        /// EconomicActivity
        /// </summary>
        public EconomicActivityDTO EconomicActivity { get; set; }

        /// <summary>
        /// IdentificationDocument
        /// </summary>
        public IdentificationDocumentDTO IdentificationDocument { get; set; }

        /// <summary>
        /// CompanyType
        /// </summary>
        public CompanyTypeDTO CompanyType { get; set; }

        /// <summary>
        /// AssociationType
        /// </summary>
        public AssociationTypeDTO AssociationType { get; set; }

        /// <summary>
        /// Insured
        /// </summary>
        public InsuredDTO Insured { get; set; }

        /// <summary>
        /// Consortiums
        /// </summary>
        public List<ConsortiumDTO> Consortiums { get; set; }
    }
}
