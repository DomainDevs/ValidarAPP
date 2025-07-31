using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.Transports.Endorsement.Declaration.ApplicationServices.DTOs
{
    [DataContract]
    public class PolicyDTO 
    {
        /// <summary>
        /// Sucursal
        /// </summary>
        /// [DataMember]
        //public string Branch { get; set; }

        /// <summary>
        /// Ramo
        /// </summary>
        /// [DataMember]
        //public string Prefix { get; set; }
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
