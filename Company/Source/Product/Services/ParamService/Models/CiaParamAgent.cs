namespace Sistran.Company.Application.ProductParamService.Models
{
    using Sistran.Core.Application.ProductParamService.Models.Base;
    using System.Collections.Generic;

    /// <summary>
    ///
    /// </summary>
    public class CiaParamAgent: BaseParamAgent
    {
        /// <summary>
        /// 
        /// </summary>
        public List<CiaParamAgencyCommiss> AgencyComiss { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //public List<CiaParamIncentive> Incentives { get; set; }
    }
}
