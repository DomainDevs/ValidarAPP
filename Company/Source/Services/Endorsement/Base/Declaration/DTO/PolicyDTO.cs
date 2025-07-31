using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Declaration.DTO
{
    [DataContract]
    public class PolicyDTO 
    {
        /// <summary>
        /// ProductoId
        /// </summary>
        [DataMember]
        public int ProductId { get; set; }

        /// <summary>
        /// Fecha Emision de la poliza
        /// </summary>
        [DataMember]
        public DateTime CurrentTo { get; set; }

        /// <summary>
        /// Fecha finalización de la poliza
        /// </summary>
        [DataMember]
        public DateTime CurrentFrom { get; set; }

        /// <summary>
        /// Cantidad de dias
        /// </summary>
        [DataMember]
        public int Days { get; set; }

        /// <summary>
        /// Frecuente
        /// </summary>
        [DataMember]
        public bool Current { get; set; }
    }
}
