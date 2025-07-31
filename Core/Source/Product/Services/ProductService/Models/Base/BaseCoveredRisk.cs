using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.ProductServices.Models.Base
{
    /// <summary>
    /// Tipos de Riesgo
    /// </summary>
    [DataContract]
    public class BaseCoveredRisk : Extension
    {
        /// <summary>
        /// Obtiene o establece el identificador
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece la cantidad maxima de riegos permitida
        /// </summary>
        [DataMember]
        public int MaxRiskQuantity { get; set; }

        /// <summary>
        /// Obtiene o establece el paquete de reglas a ejecutar Pos
        /// </summary>
        [DataMember]
        public int? RuleSetId { get; set; }

        /// <summary>
        /// Obtiene o establece el paquete de reglas a ejecutar Pre
        /// </summary>
        [DataMember]
        public int? PreRuleSetId { get; set; }
        /// <summary>
        /// Obtiene o establece el pscript a ajecutar
        /// </summary>
        [DataMember]
        public int? ScriptId { get; set; }

        /// <summary>
        /// Obtiene o establece La descripcion del tipo de riesgo
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece el tipo de riesgo cubierto
        /// </summary>
        [DataMember]
        public CoveredRiskType? CoveredRiskType { get; set; }

        /// <summary>
        /// Subtipo De Riesgo
        /// </summary>
        [DataMember]
        public SubCoveredRiskType? SubCoveredRiskType { get; set; }
    }
}
