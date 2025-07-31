using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.MassiveUnderwritingServices.Models
{
    [DataContract]
    public class RiskExclude
    {
        /// <summary>
        /// Obtiene o establece el identificador masivo.
        /// </summary>
        [DataMember]
        public int? MassiveId { set; get; }
        /// <summary>
        /// Obtiene o establece el identificador temporal.
        /// </summary>
        [DataMember]
        public int TempId { set; get; }
        /// <summary>
        /// Obtiene o establece la placa.
        /// </summary>
        [DataMember]
        public string LicensePlate { set; get; }
        /// <summary>
        /// Obtiene o establece la fecha de inicio.
        /// </summary>
        [DataMember]
        public DateTime? BeginFrom { set; get; }

    }
}
