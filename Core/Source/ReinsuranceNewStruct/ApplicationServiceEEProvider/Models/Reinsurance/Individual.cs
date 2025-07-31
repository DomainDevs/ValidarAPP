using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    [DataContract]
    public class Individual
    {
        /// <summary>
        /// Gets or sets the identification document.
        /// </summary>
        /// <value>
        /// The identification document.
        /// </value>
        [DataMember]
        public IdentificationDocument IdentificationDocument { get; set; }

        /// <summary>
        /// Tipo de individuo
        /// </summary>
        [DataMember]
        public IndividualType IndividualType { get; set; }

        [DataMember]
        public EconomicActivity EconomicActivity { get; set; }

        /// <summary>
        /// Tipo de cliente
        /// </summary>
        [DataMember]
        public CustomerType CustomerType { get; set; }

        /// <summary>
        /// Medio de pago
        /// </summary>
        [DataMember]
        public IndividualPaymentMethod PaymentMethod { get; set; }


        /// <summary>
        /// Fecha de ultima actualización
        /// </summary>
        [DataMember]
        public DateTime? LastUpdate { get; set; }

        /// <summary>
        /// Usuario de ultima actualización
        /// </summary>
        [DataMember]
        public string UpdateBy { get; set; }
    }
}
