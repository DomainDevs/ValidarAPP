using Sistran.Core.Application.CommonService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim
{
    [DataContract]
    public class CatastrophicEvent
    {
        /// <summary>
        /// Dirección
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Fecha Desde
        /// </summary>
        [DataMember]
        public DateTime CurrentFrom { get; set; }

        /// <summary>
        /// Fecha hasta
        /// </summary>
        [DataMember]
        public DateTime CurrentTo { get; set; }

        /// <summary>
        /// Dirección
        /// </summary>
        [DataMember]
        public string FullAddress { get; set; }

        [DataMember]
        public Catastrophe Catastrophe { get; set; }

        /// <summary>
        /// Ciudad
        /// </summary>
        [DataMember]
        public City City { get; set; }
    }
}
