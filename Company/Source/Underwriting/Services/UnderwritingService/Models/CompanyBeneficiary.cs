using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models.Base;
using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UnderwritingServices.Models
{
    /// <summary>
    /// Beneficiario
    /// </summary>
    [DataContract]
    public class CompanyBeneficiary : BaseBeneficiary
    {
        /// <summary>
        /// Gets or sets the type of the beneficiary.
        /// </summary>
        /// <value>
        /// The type of the beneficiary.
        /// </value>
        [DataMember]
        public CompanyBeneficiaryType BeneficiaryType { get; set; }

        /// <summary>
        /// Gets or sets the name of the company.
        /// </summary>
        /// <value>
        /// The name of the company.
        /// </value>
        [DataMember]
        public IssuanceCompanyName CompanyName { get; set; }

        /// <summary>
        /// Identificación de la compañia
        /// </summary>
        [DataMember]
        public IssuanceIdentificationDocument IdentificationDocument { get; set; }

        /// <summary> 
        /// Fecha de baja del beneficiario 
        /// </summary> 
        [DataMember]
        public DateTime? DeclinedDate { get; set; }

        /// <summary>
        /// Tipo de asociacion
        /// </summary>}

        [DataMember]
        public IssuanceAssociationType AssociationType { get; set; }
    }
}
