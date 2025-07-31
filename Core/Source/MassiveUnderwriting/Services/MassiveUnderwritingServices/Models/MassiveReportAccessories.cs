using System.Runtime.Serialization;

namespace Sistran.Core.Application.MassiveUnderwritingServices.Models
{

    [DataContract]
    public class MassiveReportAccessories
    {
        /// <summary>
        /// Obtiene o establece el ID de riesgo
        /// </summary>
        [DataMember]
        public int RiskId { get; set; }
        /// <summary>
        /// Codigo
        /// </summary>
        [DataMember]
        public string Code { get; set; }

        /// <summary>
        /// Obtiene o establece el Original
        /// </summary>
        [DataMember]
        public string Original { get; set; }

        /// <summary>
        /// Obtiene o establece el Precio
        /// </summary>
        [DataMember]
        public string Price { get; set; }
    }
}
