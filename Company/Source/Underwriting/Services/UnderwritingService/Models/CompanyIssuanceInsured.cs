using System.Runtime.Serialization;
using System;
using Sistran.Core.Application.UnderwritingServices.Models.Base;
using Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Company.Application.UnderwritingServices.Models
{
    [DataContract]
    public class CompanyIssuanceInsured : BaseIssuanceInsured
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

        /// <summary>
        /// Puntaje Score.
        /// </summary>}
        [DataMember]
        public ScoreCredit ScoreCredit { get; set; }
        
        /// <summary>
        /// Fecha ingreso
        /// </summary>}
        [DataMember]
        public DateTime EnteredDate { get; set; }

        /// <summary>
        /// Perfil
        /// </summary>}
        [DataMember]
        public int Profile { get; set; }
        [DataMember]
        public IssuanceAssociationType AssociationType { get; set; }
        
    }
}