using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Sistran.Company.Application.CommonServices.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models.Base;

namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    public class CompanyComissionAgent : BaseCommission
    {
        /// <summary>
        /// Ramo Comercial
        /// </summary>
        [DataMember]
        public CompanyPrefix Prefix { get; set; }

        /// <summary>
        /// Ramo Tecnico
        /// </summary>
        [DataMember]
        public CompanyLineBusiness LineBusiness { get; set; }

        /// <summary>
        /// Sub Ramo Técnico
        /// </summary>
        [DataMember]
        public CompanySubLineBusiness SubLineBusiness { get; set; }
        [DataMember]
        public int AgentAgencyId { get; set; }
        [DataMember]
        public string agency { get; set; }
    }
}
