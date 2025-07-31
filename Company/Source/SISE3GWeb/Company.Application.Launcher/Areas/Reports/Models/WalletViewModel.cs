using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Reports.Models
{
    public class WalletViewModel
    {
        public int BranchId { get; set; }

        public int BranchPolicyId { get; set; }

        public int PrefixId { get; set; }

        public int FilterId { get; set; }
    }
}