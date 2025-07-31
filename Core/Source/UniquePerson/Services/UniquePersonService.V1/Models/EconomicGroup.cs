using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UniquePersonService.V1.Models
{
    [DataContract]
    public class EconomicGroup : BaseEconomicGroup
    {

        /// <summary>
        /// EconomicGroupDetails
        /// </summary>
        [DataMember]
        private List<EconomicGroupDetail> economicGroupDetails;
        public List<EconomicGroupDetail> EconomicGroupDetails { get => economicGroupDetails == null ? new List<EconomicGroupDetail>() : economicGroupDetails; set => economicGroupDetails = value; }
    }
}
