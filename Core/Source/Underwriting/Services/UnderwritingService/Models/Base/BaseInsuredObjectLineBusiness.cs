using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    /// <summary>
    /// objetos de seguro asociado a la linea del negocio
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.CommonService.Models.Extension" />
    [DataContract]
    public class BaseInsuredObjectLineBusiness : Extension
    {
        /// <summary>
        /// Id del Objeto de seguro
        /// </summary>
        [DataMember]
        public int IdInsuredObject { get; set; }

        /// <summary>
        /// Id del ramo tecnico
        /// </summary>
        [DataMember]
        public int IdLineBusiness { get; set; }
    }
}
