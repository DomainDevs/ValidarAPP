using Sistran.Core.Application.UnderwritingServices.Models.Base;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.UnderwritingServices.Models
{
    /// <summary>
    /// Beneficiario
    /// </summary>
    [DataContract]
    public class Beneficiary : BaseBeneficiary
    {
        /// <summary>
        /// Tipo del Beneficiario
        /// </summary>
        /// <value>
        /// The type of the beneficiary.
        /// </value>
        [DataMember]
        public BeneficiaryType BeneficiaryType { get; set; }

        /// Dirección de Notificación
        /// </summary>
        [DataMember]
        public IssuanceCompanyName CompanyName { get; set; }

        /// <summary>
        /// Identificación de la compañia
        /// </summary>
        [DataMember]
        public IssuanceIdentificationDocument IdentificationDocument { get; set; }

        public Beneficiary()
        {
            IndividualType = Services.UtilitiesServices.Enums.IndividualType.Person;
        }
    }
}