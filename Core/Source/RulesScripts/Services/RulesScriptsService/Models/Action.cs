using Sistran.Core.Application.RulesScriptsServices.Enums;
using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    /// <summary>
    /// Acciones
    /// </summary>
    [DataContract]
    public class Action
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Exprecion
        /// </summary>
        [DataMember]
        public string Expression {get; set; }
       
        /// <summary>
        /// Tipo de Accion
        /// </summary>
        [DataMember]
        public ActionType ActionType { get; set; }

        /// <summary>
        /// Tipo de Valor
        /// </summary>
        [DataMember]
        public Sistran.Core.Application.RulesScriptsServices.Enums.ValueType ValueType { get; set; }

        /// <summary>
        /// Datos adicionales al Concepto
        /// </summary>
        [DataMember]
        public ConceptControl ConceptControl { get; set; }

        #region AssignConceptAction
        /// <summary>
        /// Concepto Izquierdo
        /// </summary>
        [DataMember]
        public Concept ConceptLeft { get; set; }
        #endregion

        #region AssignAction
        /// <summary>
        /// Tipo de Accion
        /// </summary>
        [DataMember]
        public AssignType AssignType { get; set; }

        /// <summary>
        /// Operador
        /// </summary>
        [DataMember]
        public Operator Operator { get; set; }

        /// <summary>
        /// Valor 
        /// </summary>
        [DataMember]
        public string ValueRight { get; set; }

        /// <summary>
        /// Concepto Derecho
        /// </summary>
        [DataMember]
        public Concept ConceptRight { get; set; }

        /// <summary>
        /// Valor Temporal Derecho
        /// </summary>
        [DataMember]
        public String TemporalNameRight { get; set; }

        #endregion
    
        #region AssignTemporalAction    
        /// <summary>
        /// Valor Temporal Izquierdo
        /// </summary>
        [DataMember]
        public string TemporalNameLeft { get; set; }
        #endregion

        #region InvokeAction
        /// <summary>
        /// Tipo de Invocacion 
        /// </summary>
        [DataMember]
        public InvokeType InvokeType { get; set; }
        #endregion

        #region InvokeFunctionAction
        /// <summary>
        /// Id De La Funcion
        /// </summary>
        [DataMember]
        public string IdFuction { get; set; }

        /// <summary>
        /// Descripcion de la Funcion
        /// </summary>
        [DataMember]
        public string DescriptionFunction { get; set; }

        /// <summary>
        /// Nombre de la Funcion
        /// </summary>
        [DataMember]
        public string FunctionName { get; set; }
        #endregion

        #region InvokeMessageAction
        /// <summary>
        /// Mensaje
        /// </summary>
        [DataMember]
        public string Message { get; set; }
        #endregion

        #region InvokeRuleSetAction
        /// <summary>
        /// Id del Paquete de Reglas
        /// </summary>
        [DataMember]
        public int RuleSetId { get; set; }

        /// <summary>
        /// Descripcion del Paquete de Reglas
        /// </summary>
        [DataMember]
        public string DescriptionRuleSet { get; set; }
        #endregion
    }
}
