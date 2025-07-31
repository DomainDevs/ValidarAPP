using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    /// <summary>
    /// Tomador
    /// </summary>
    [DataContract]
    public class BaseHolder
    {
        /// <summary>
        /// Gets or sets the individual identifier.
        /// </summary>
        /// <value>
        /// The individual identifier.
        /// </value>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Fecha de Baja
        /// </summary>
        [DataMember]
        public DateTime DeclinedDate { get; set; }

        /// <summary>
        /// Gets or sets the birth date.
        /// </summary>
        /// <value>
        /// The birth date.
        /// </value>
        [DataMember]
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        /// <value>
        /// The gender.
        /// </value>
        [DataMember]
        public string Gender { get; set; }


        [DataMember]
        public string CustomerTypeDescription { get; set; }

        /// <summary>
        /// Tipo de individuo
        /// </summary>
        [DataMember]
        public IndividualType IndividualType { get; set; }

        /// <summary>
        /// Gets or sets the insured identifier.
        /// </summary>
        /// <value>
        /// The insured identifier.
        /// </value>
        [DataMember]
        public int InsuredId { get; set; }
    }
}
