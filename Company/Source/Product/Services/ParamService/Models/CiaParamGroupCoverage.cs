namespace Sistran.Company.Application.ProductParamService.Models
{
    using Sistran.Core.Application.ProductParamService.Models.Base;
    using System.Collections.Generic;

    /// <summary>
    ///
    /// </summary>
    public class CiaParamGroupCoverage : BaseParamGroupCoverage
    {
        /// <summary>
        /// 
        /// </summary>
        public List<CiaParamDeductiblesCoverage> DeductiblesCoverage { get; set; }

    }
}
