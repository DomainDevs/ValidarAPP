using System.Runtime.Serialization;
using Sistran.Core.Application.Extensions;

namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    public class EnumRoles
    {
        /// <summary>
        /// Obtiene o setea Identifiador Reinsurer
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember]
        public int Reinsurer { get; set; }

        /// <summary>
        /// Obtiene o setea Identifiador Agent
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember]
        public int Agent { get; set; }

        /// <summary>
        /// Obtiene o setea Identifiador Insured
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember]
        public int Insured { get; set; }

        /// <summary>
        /// Obtiene o setea Identifiador Supplier
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember]
        public int Supplier { get; set; }

        /// <summary>
        /// Obtiene o setea Identifiador Coinsured
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember]
        public int Coinsured { get; set; }

        /// <summary>
        /// Obtiene o setea Identifiador Third
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember]
        public int Third { get; set; }

        /// <summary>
        /// Obtiene o setea Identifiador Employee
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember]
        public int Employee { get; set; }
    }
}
