using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniquePersonAplicationServices.DTOs
{
    /// <summary>
    /// Registro Fiadores del Asegurado
    /// </summary>
    [DataContract]
    public class InsuredGuaranteeLogDTO
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
        /// Gets or sets the guarantee identifier.
        /// </summary>
        /// <value>
        /// The guarantee identifier.
        /// </value>
        [DataMember]
        public int GuaranteeId { get; set; }

        /// <summary>
        /// Gets or sets the guarantee status code.
        /// </summary>
        /// <value>
        /// The guarantee status code.
        /// </value>
        [DataMember]
        public int GuaranteeStatusCode { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the log date.
        /// </summary>
        /// <value>
        /// The log date.
        /// </value>
        [DataMember]
        public string LogDate { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string CityName { get; set; }
     
        [DataMember]
        public string StateName { get; set; }
       
        [DataMember]
        public string CountryName { get; set; }

    }
}
