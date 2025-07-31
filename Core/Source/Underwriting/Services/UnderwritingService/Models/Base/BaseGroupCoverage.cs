using Sistran.Core.Application.Extensions;
using Sistran.Core.Application.CommonService.Enums;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    /// <summary>
    /// Grupo de Coberturas
    /// </summary>
    [DataContract]
    public class BaseGroupCoverage : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public virtual int Id { get; set; }

        /// <summary>
        /// Grupo de cobertura
        /// </summary>
        [DataMember]
        public virtual string Description { get; set; }

        /// <summary>
        /// Es obligatoria?
        /// </summary>
        [DataMember]
        public virtual bool IsMandatory { get; set; }

        /// <summary>
        /// Seleccionado por defecto
        /// </summary>
        [DataMember]
        public virtual bool IsSelected { get; set; }

        /// <summary>
        /// Número
        /// </summary>
        [DataMember]
        public virtual int Number { get; set; }

        /// <summary>
        /// Obtiene o establece el Identificador del paquete de reglas Pre
        /// </summary>
        [DataMember]
        public virtual int? RuleSetId { get; set; }

        /// <summary>
        /// Obtiene o establece el Identificador del paquete de reglas Pos
        /// </summary>
        [DataMember]
        public virtual int? PosRuleSetId { get; set; }

        /// <summary>
        /// Obtiene o establece el Identificador del paquete de reglas Pos
        /// </summary>
        [DataMember]
        public virtual int MainCoverageId { get; set; }

        /// <summary>
        /// Obtiene o establece el porcentaje del limite
        /// </summary>
        [DataMember]
        public virtual decimal SublimitPercentage { get; set; }

        /// <summary>
        /// Gets or sets the script identifier.
        /// </summary>
        /// <value>
        /// The script identifier.
        /// </value>
        [DataMember]
        public virtual int? ScriptId { get; set; }

        /// <summary>
        /// Tipo de riesgo
        /// </summary>
        [DataMember]
        public virtual CoveredRiskType? CoveredRiskType { get; set; }
    }
}
