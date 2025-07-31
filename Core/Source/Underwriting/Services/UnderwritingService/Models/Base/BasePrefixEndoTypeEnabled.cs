using Sistran.Core.Application.CommonService.Models;
using System.Runtime.Serialization;
using Sistran.Core.Application.Extensions;
namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    /// <summary>
    /// ipos de endosos habilitados para un ramo
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.CommonService.Models.Extension" />
    [DataContract]
    public class BasePrefixEndoTypeEnabled : Extension
    {
        /// <summary>
        /// Identificador del ramo
        /// </summary>
        [DataMember]
        public int PrefixCode { get; set; }

        /// <summary>
        /// Identificador del tipo de endoso
        /// </summary>
        [DataMember]
        public int EndorsementCode { get; set; }

        /// <summary>
        /// Si esta habilitado para el ramo
        /// </summary>
        [DataMember]
        public bool IsEnabled { get; set; }
    }
}
