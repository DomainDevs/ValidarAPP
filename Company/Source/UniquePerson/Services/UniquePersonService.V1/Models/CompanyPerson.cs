using System.Collections.Generic;
using System.Runtime.Serialization;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;

namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    /// <summary>
    /// Persona
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.UniquePersonService.V1.Models.Person" />
    [DataContract]
    public class CompanyPerson : Sistran.Core.Application.UniquePersonService.V1.Models.Base.BasePerson
    {
        /// <summary>
        /// role
        /// </summary>
        [DataMember]
        public CompanyRole Role { get; set; }

        /// <summary>
        /// EconomicActivity
        /// </summary>
        public CompanyEconomicActivity EconomicActivity { get; set; }

        /// <summary>
        /// IdentificationDocument
        /// </summary>
        [DataMember]
        public CompanyIdentificationDocument IdentificationDocument { get; set; }

        /// <summary>
        /// MaritalStatus
        /// </summary>
        public CompanyMaritalStatus MaritalStatus { get; set; }

        /// <summary>
        /// EducationLevel
        /// </summary>
        public CompanyEducativeLevel EducativeLevel { get; set; }

        /// <summary>
        /// SocialLayer
        /// </summary>
        public CompanySocialLayer SocialLayer { get; set; }

        /// <summary>
        /// HouseType
        /// </summary>
        public CompanyHouseType HouseType { get; set; }

        /// <summary>
        /// PersonType
        /// </summary>
        public CompanyPersonType PersonType { get; set; }

        /// <summary>
        /// Obtiene o setea una lista de Sarlafts
        /// </summary>
        /// <value>
        /// The sarlaft.
        /// </value>
        [DataMember]
        public List<IndividualSarlaft> Sarlafts { get; set; }

        /// <summary>
        /// Obtiene o setea Exoneration
        /// </summary>
        /// <value>
        /// The exoneration.
        /// </value>
        [DataMember]
        public CompanySarlaftExoneration Exoneration { get; set; }

        /// <summary>
        /// Obtiene o setea cheque a nombre de
        /// </summary>
        /// <value>
        /// Check payable.
        /// </value>
        [DataMember]
        public string CheckPayable { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public bool DataProtection { get; set; }
        
        [DataMember]
        public List<CompanyAddress> CompanyAddress { get; set; }

        [DataMember]
        public List<CompanyPhone> CompanyPhones { get; set; }
        
        /// <summary>
        /// propiedad para concatenar los nombres de la persona consultada
        /// </summary>
        [DataMember]
        public string Names { get; set; }

        [DataMember]
        /// <summary>
        /// Insured
        /// </summary>
        public CompanyInsured Insured { get; set; }

        [DataMember]
        public virtual List<PoliciesAut> InfringementPolicies { get; set; }

        [DataMember]
        public int OperationId { get; set; }
		
    }
}
