using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models
{
    [KnownType("ConditionModel")]
    public class ConditionModel
    {
        public int ConditionId { get; set; }
        public int AccountingRuleId { get; set; }
        public int ParameterId { get; set; }
        public string ParameterName { get; set; }
        public string ParameterDescription { get; set; }
        public int OperatorId { get; set; }
        public string OperatorSign { get; set; }
        public string OperatorDescription { get; set; }
        public string Value { get; set; }
        public string ValueDescription { get; set; }
        public int TrueResultTypeId { get; set; }
        public string TrueResultTypeDescription { get; set; }
        public int TrueResult { get; set; }
        public int FalseResultTypeId { get; set; }
        public string FalseResultTypeDescription { get; set; }
        public int FalseResult { get; set; }
        public string ConditionDescription { get; set; }
        public string TrueResultDesription { get; set; }
        public string FalseResultDesription { get; set; }


        //TODO: Alejandro Villagrán
        //campos para redifinición de modelo.
        public int RightResultId { get; set; }
        public int LeftResultId { get; set; }
        public int ResultId { get; set; }

    }
}