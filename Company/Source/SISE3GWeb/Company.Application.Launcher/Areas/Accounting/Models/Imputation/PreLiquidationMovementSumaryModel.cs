using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Imputation
{
    [KnownType("PreLiquidationMovementSumaryModel")]
    public class PreLiquidationMovementSumaryModel
    {
        public List<MovementSumaryDetails> MovementSumary { get; set; }
    }

    [KnownType("MovementSumaryDetails")]
    public class MovementSumaryDetails
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public double Debit { get; set; }
        public double Credit { get; set; }
    }
}