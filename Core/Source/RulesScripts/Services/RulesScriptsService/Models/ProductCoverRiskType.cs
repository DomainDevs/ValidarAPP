using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class ProductCoverRiskType
    {
        /// <summary>
        /// Identificador del Producto
        /// </summary>
        [DataMember]
        public int ProductId { get; set; }

        /// <summary>
        /// Codigo Tipo de Riego de la Covertura
        /// </summary>
        [DataMember]
        public int CoveredRiskTypeCode { get; set; }

        /// <summary>
        /// Riego Maximo Cuantificado
        /// </summary>
        [DataMember]
        public int MaxRiskQuantity { get; set; }

        /// <summary>
        /// Id del Guion 
        /// </summary>
        [DataMember]
        public int? ScriptId { get; set; }

        /// <summary>
        /// Id del Paquete de Reglas
        /// </summary>
        [DataMember]
        public int? RuleSetId { get; set; }

        /// <summary>
        /// Id del Paquete de Reglas Pre
        /// </summary>
        [DataMember]
        public int? PreRuleSetId { get; set; }
    }
}


