namespace Sistran.Company.Application.ProductParamService.Models
{
    using Sistran.Core.Application.ProductParamService.Models.Base;
    using System.Collections.Generic;

    public class CiaParamCoverage : BaseParamCoverage
    {
        /// <summary>
        /// 
        /// </summary>
        public List<CiaParamInsuredObject> InsuredObjects { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<CiaParamForm> Form { get; set; }
    }
}
