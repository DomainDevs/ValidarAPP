using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Reports.PreLiquidation
{
    [KnownType("MovementSumaryModel")]
    public class MovementSumaryModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public double Debit { get; set; }
        public double Credit { get; set; }
    }
}