
using Sistran.Core.Application.UniquePerson.IntegrationService.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UniquePerson.IntegrationService.Models
{
    [DataContract]
    public class IntegrationEconomicGroup : BaseIntegrationEconomicGroup
    {

        /// <summary>
        /// EconomicGroupDetails
        /// </summary>
        [DataMember]
        private List<IntegrationEconomicGroupDetail> economicGroupDetails;
        public List<IntegrationEconomicGroupDetail> EconomicGroupDetails { get => economicGroupDetails == null ? new List<IntegrationEconomicGroupDetail>() : economicGroupDetails; set => economicGroupDetails = value; }
    }
}
