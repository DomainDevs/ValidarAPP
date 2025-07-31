using Sistran.Core.Services.UtilitiesServices.Enums;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniquePersonAplicationServices.DTOs
{
    [DataContract]
    public class InsuredGuaranteePrefixDTO
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
        /// Gets or sets the prefix code.
        /// </summary>
        /// <value>
        /// The prefix code.
        /// </value>
        [DataMember]
        public int PrefixCode { get; set; }

        /// <summary>
        /// Gets or sets the parameter.
        /// </summary>
        /// <value>
        /// The parameter.
        /// </value>
        [DataMember]
        public ParametrizationStatus Parameter { get; set; }

    }
}
