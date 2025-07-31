namespace Sistran.Company.Application.ProductParamService.Models
{
    using System.Collections.Generic;

    public class CiaParamCoverages
    {
        public CiaParamGroupCoverage Coverage { get; set; }
        public List<CiaParamGroupCoverage> CoverageAllied { get; set; }
    }
}
