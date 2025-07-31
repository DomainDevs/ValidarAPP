using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    [DataContract]
    public class RuleAction
    {
        /// <summary>
        /// Identificador de RuleBase
        /// </summary>
        [DataMember]
        public int RuleBaseId { get; set; }

        /// <summary>
        /// Identificador de Regla
        /// </summary>
        [DataMember]
        public int RuleId { get; set; }

        /// <summary>
        /// Identitificador de Acción de la regla
        /// </summary>
        [DataMember]
        public int ActionId { get; set; }

        /// <summary>
        /// Identificador de Concepto
        /// </summary>
        [DataMember]
        public int? ConceptId { get; set; }

        /// <summary>
        /// Identificador de Entidad
        /// </summary>
        [DataMember]
        public int? EntityId { get; set; }

        /// <summary>
        /// Código de Operador 
        /// </summary>
        [DataMember]
        public int? OperationCode { get; set; }

        /// <summary>
        /// Código de Tipo de Valor
        /// </summary>
        [DataMember]
        public int ValueTypeCode { get; set; }

        /// <summary>
        /// Valor de la acción
        /// </summary>
        [DataMember]
        public int ActionValue { get; set; }

        /// <summary>
        /// valor de la accion
        /// </summary>
        [DataMember]
        public string ActionVal { get; set; }

        /// <summary>
        /// Numero de order de la regla
        /// </summary>
        [DataMember]
        public int OrderNumber { get; set; }
    }
}
