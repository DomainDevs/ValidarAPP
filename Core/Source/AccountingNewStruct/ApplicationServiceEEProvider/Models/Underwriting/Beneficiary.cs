using Sistran.Core.Application.AccountingServices.EEProvider.Models.Underwriting.Base;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Underwriting
{
    /// <summary>
    /// Beneficiario
    /// </summary>
    [DataContract]
    public class Beneficiary : BaseBeneficiary
    {
        /// Dirección de Notificación
        /// </summary>
        [DataMember]
        public IssuanceCompanyName CompanyName { get; set; }

        /// <summary>
        /// Identificación de la compañia
        /// </summary>
        [DataMember]
        public IssuanceIdentificationDocument IdentificationDocument { get; set; }
    }
}