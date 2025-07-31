namespace Sistran.Company.Application.ProductParamService.Models
{
    using Sistran.Core.Application.ProductParamService.Models.Base;
    using System.Collections.Generic;

    public class CiaParamInsuredObject : BaseParamInsuredObject
    {
        /// <summary>
        /// 
        /// </summary>
        public List<CiaParamCoverages> Coverages { get; set; }
    }
}
