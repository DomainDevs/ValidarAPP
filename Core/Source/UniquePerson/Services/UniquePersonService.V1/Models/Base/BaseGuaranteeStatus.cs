using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.V1.Models.Base
{

    [DataContract]
    public class BaseGuaranteeStatus : BaseGeneric
    {

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
