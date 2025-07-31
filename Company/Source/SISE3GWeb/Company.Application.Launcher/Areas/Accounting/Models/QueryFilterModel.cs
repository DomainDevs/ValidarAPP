using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models
{
    public class QueryFilterModel
    {
        public int BranchId { get; set; }
        public int BusinessId { get; set; }
        public int CancellationTypeId { get; set; }
        public string CutDate { get; set; }
        public int GrouperId { get; set; }
        public int InsuredId { get; set; }
        public int IntermediaryId { get; set; }
        public string IssueDateFrom { get; set; }
        public string IssueDateTo { get; set; }
        public string PolicyNumber { get; set; }
        public int PrefixId { get; set; }
        public string ProcessDate { get; set; }
        public int ProcessNumber { get; set; }
        public int SalePointId { get; set; }

    }
}