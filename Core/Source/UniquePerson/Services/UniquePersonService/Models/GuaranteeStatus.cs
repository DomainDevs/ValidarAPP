using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.Models
{
    /// <summary>
    /// Estado Contragarantia
    /// </summary>
    [DataContract]
    public class GuaranteeStatus : Extension
    {

        /// <summary>
        /// Codigo
        /// </summary>
        [DataMember]
        public int Code { get; set; }

        /// <summary>
        /// Descripcion
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// activo
        /// </summary>
        [DataMember]
        public bool IsEnabledInd { get; set; }

        /// <summary>
        /// habilitado Remover
        /// </summary>
        [DataMember]
        public bool IsRemoveInd { get; set; }

        /// <summary>
        /// Documento validado
        /// </summary>
        [DataMember]
        public bool IsValidateDocument { get; set; }

        /// <summary>
        /// Habilitado para emisión
        /// </summary>
        [DataMember]
        public bool IsEnabledSubscription { get; set; }
    }
}
