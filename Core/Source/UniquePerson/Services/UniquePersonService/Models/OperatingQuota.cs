using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePersonService.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.Models
{
    /// <summary>
    /// Cupo Operativo
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.UniquePersonService.Models.Individual" />
    [DataContract]
    public class OperatingQuota : BaseOperatingQuota
    {
        /// <summary>
        /// Obtiene o Setea Linea del Negocio
        /// </summary>
        /// <value>
        /// Linea del Negocio
        /// </value>
        [DataMember]
        public LineBusiness LineBusiness { get; set; }

        /// <summary>
        /// Obtiene o Setea Suma 
        /// </summary>
        /// <value>
        /// Suma
        /// </value>
        [DataMember]
        public Amount Amount { get; set; }
    }
}
