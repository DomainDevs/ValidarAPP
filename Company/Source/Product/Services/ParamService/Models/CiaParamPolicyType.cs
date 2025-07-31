namespace Sistran.Company.Application.ProductParamService.Models
{
    using Sistran.Core.Application.ProductParamService.Models.Base;
    using System.Collections.Generic;

    /// <summary>
    ///
    /// </summary>
    public class CiaParamPolicyType : BaseParamPolicyType
    {
        /// <summary>
        /// 
        /// </summary>
        public int PrefixId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<CiaParamLimitsRC> LimitsRC { get; set; }
    }
}
