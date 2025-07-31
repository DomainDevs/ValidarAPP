using Sistran.Core.Application.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.Transports.TransportBusinessService.Models.Base
{
    [DataContract]
    public class BaseTransport : Extension
    {
        /// <summary>
        /// Origen
        /// </summary>
        [DataMember]
        public String Source { get; set; }
        /// <summary>
        /// Destino
        /// </summary>
        [DataMember]
        public String Destiny { get; set; }

        /// <summary>
        /// Carga
        /// </summary>
        [DataMember]
        public Decimal? FreightAmount { get; set; }
       
        /// <summary>
        /// Límite Máximo por Despacho
        /// </summary>
        [DataMember]
        public Decimal? LimitMaxReleaseAmount { get; set; }
        /// <summary>
        /// Prima Mínima
        /// </summary>
        [DataMember]
        public Decimal MinimumPremium { get; set; }
        /// <summary>
        /// Presupuesto Anual
        /// </summary>
        [DataMember]
        public Decimal ReleaseAmount { get; set; }
        /// <summary>
        /// Fecha de lanzamiento
        /// </summary>
        [DataMember]
        public DateTime ReleaseDate { get; set; }
    }
}
