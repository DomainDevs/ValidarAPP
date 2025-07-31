using Sistran.Core.Application.AccountingServices.EEProvider.Models.Underwriting.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Underwriting
{
    [DataContract]
    public class IssuanceInsured : BaseIssuanceInsured
    {

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
        /// Medio de pago
        /// </summary>
        [DataMember]
        public IssuancePaymentMethod PaymentMethod { get; set; }

        /// <summary>
        /// Actividad  economica de la persona
        /// </summary>
        [DataMember]
        public IssuanceEconomicActivity EconomicActivity { get; set; }

    }
}