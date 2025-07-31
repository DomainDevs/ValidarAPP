using Sistran.Core.Application.Extensions;

namespace Sistran.Company.Application.ProductParamService.Models.Base
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseCiaParamIncentive : Extension
    {
        /// <summary>
        /// 
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int IndividualId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int AgentAgencyId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal IncentiveAmount { get; set; }
    }
}
