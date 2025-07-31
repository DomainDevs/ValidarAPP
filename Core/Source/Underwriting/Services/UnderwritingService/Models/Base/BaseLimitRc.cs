using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{

    /// <summary>
    /// Limite de RC
    /// </summary>
    [DataContract]
    public class BaseLimitRc : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Limite de RC
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Es predeterminado?
        /// </summary>
        [DataMember]
        public bool IsDefault { get; set; }

        /// <summary>
        /// Limite Unico
        /// </summary>
        [DataMember]
        public string LimitUnique { get; set; }

        /// <summary>
        /// Limite nro 1
        /// </summary>
        [DataMember]
        public decimal Limit1 { get; set; }

        /// <summary>
        /// Limite nro 2
        /// </summary>
        [DataMember]
        public decimal Limit2 { get; set; }

        /// <summary>
        /// Limite nro 3
        /// </summary>
        [DataMember]
        public decimal Limit3 { get; set; }

        /// <summary>
        /// Suma limites
        /// </summary>
        [DataMember]
        public decimal LimitSum { get; set; }
    }
}
