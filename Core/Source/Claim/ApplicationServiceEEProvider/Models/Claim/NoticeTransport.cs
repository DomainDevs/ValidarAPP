using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim
{
    [DataContract]
    public class NoticeTransport
    {
        /// <summary>
        /// Tipo de carga
        /// </summary>
        [DataMember]
        public string CargoType { get; set; }

        /// <summary>
        /// Tipo de empaque
        /// </summary>
        [DataMember]
        public string PackagingType { get; set; }

        /// <summary>
        /// Origen
        /// </summary>
        [DataMember]
        public string Origin { get; set; }

        /// <summary>
        /// Destino
        /// </summary>
        [DataMember]
        public string Destiny { get; set; }

        /// <summary>
        /// Tipo de transporte
        /// </summary>
        [DataMember]
        public string TransportType { get; set; }

        /// <summary>
        /// Notice
        /// </summary>
        [DataMember]
        public Notice Notice { get; set; }
    }
}
