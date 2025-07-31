using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    /// <summary>
    /// Limites de Rc
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.CommonService.Models.Extension" />
    [DataContract]
    public class BaseLimitRCRelation : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        ///Descripcion limite rc
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Es predeterminado?
        /// </summary>
        [DataMember]
        public bool IsDefault { get; set; }
    }
}
