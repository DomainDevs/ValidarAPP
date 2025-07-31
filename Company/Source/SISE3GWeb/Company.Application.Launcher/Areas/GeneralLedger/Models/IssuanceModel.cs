using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models
{
    [KnownType("IssuanceModel")]
    public class IssuanceModel
    {
        public int PolicyId { get; set; }
        public int EndorsementId { get; set; }
        public int BranchId { get; set; }
        public int PrefixId { get; set; }
        public int CurrencyId { get; set; }
        public int EndoTypeId { get; set; }
        public int BusinessTypeId { get; set; }
        public List<ComponentModel> Components { get; set; }
    }

    [KnownType("ComponentsModel")]
    public class ComponentModel
    {
        public int ComponentId { get; set; }
        public decimal Value { get; set; }
    }
}