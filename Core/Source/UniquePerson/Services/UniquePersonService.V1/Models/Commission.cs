using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.CommonService.Models.Base;
using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UniquePersonService.V1.Models
{
    public class Commission: BaseCommission
    {
        /// <summary>
        /// Ramo Comercial
        /// </summary>
        [DataMember]
        public Prefix Prefix { get; set; }

        /// <summary>
        /// Ramo Tecnico
        /// </summary>
        [DataMember]
        public LineBusiness LineBusiness { get; set; }

        /// <summary>
        /// Sub Ramo Técnico
        /// </summary>
        [DataMember]
        public SubLineBusiness SubLineBusiness { get; set; }
        [DataMember]
        public int AgentAgencyId { get; set; }

    }
}
