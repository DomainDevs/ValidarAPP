using System.Collections.Generic;

namespace Sistran.Core.Framework.UIF.Web.Areas.RulesScripts.Models
{
    public class Rule
    {
        public int idRule { get; set; }
        public List<Condition> Conditions { get; set; }
    }

    public class Condition
    {
        public int idConcept { get; set; }
        public int idEntity { get; set; }
        public Range range1 { get; set; }
        public Range range2 { get; set; }
        public double? valueOperator { get; set; }
        public string symbolOperator { get; set; }
        public string value { get; set; }
    }

    public class Range
    {
        public string name { get; set; }
        public object minValue { get; set; }
        public object maxValue { get; set; }
        public object UminValue { get; set; }
        public object UmaxValue { get; set; }
        public string type { get; set; }
        /// <summary>
        /// determina de es un rango abierto o cerrado (true-incluye los limites, false-no incluye limites)
        /// </summary>
        public bool isInclusive { get; set; }
    }
}