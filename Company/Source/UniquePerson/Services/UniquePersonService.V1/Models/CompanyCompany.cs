using Sistran.Core.Application.CommonService.Models;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;

namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    /// <summary>
    /// Compañia
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.UniquePersonService.V1.Models.Company" />
    [DataContract]
    public class CompanyCompany : Sistran.Core.Application.UniquePersonService.V1.Models.Base.BaseCompany
    {

        /// <summary>
        /// role
        /// </summary>
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
        /// CompanyType
        /// </summary>
        public CompanyCompanyType CompanyType { get; set; }


        /// <summary>
        /// Obtiene o setea una lista de Consorcios
        /// </summary>
        /// <value>
        /// The co consortium.
        /// </value>
        [DataMember]
        public List<CompanyConsortium> Consortiums { get; set; }

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
        /// Obtiene o setea Sucursal usuario
        /// </summary>
        /// <value>
        /// Sucursal Usuario
        /// </value>
        [DataMember]
        public Branch UserBranch { get; set; }

        /// <summary>
        /// AssociationType
        /// </summary>
        public CompanyAssociationType AssociationType { get; set; }

        /// <summary>
        /// AssociationType
        /// </summary>
        public CompanyInsured Insured { get; set; }

        /// <summary>
        /// CheckPayable
        /// </summary>
        public string CheckPayable { get; set; }

        [DataMember]
        public virtual List<PoliciesAut> InfringementPolicies { get; set; }
        [DataMember]
        public int OperationId { get; set; }
    }
}
