using System.Runtime.Serialization;


namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    [DataContract]
    public class ConceptControl
    {
        /// <summary>
        /// Tipo de Control Basico
        /// </summary>
        [DataMember]
        public Enums.BasicType BasicType { get; set; }

        /// <summary>
        /// Codigo del Tipo de Concepto
        /// </summary>
        [DataMember]
        public int ConceptControlCode { get; set; }

        /// <summary>
        /// Descripcion 
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Requerido
        /// </summary>
        [DataMember]
        public bool Required { get; set; }

        /// <summary>
        /// Tipo de Concepto
        /// </summary>
        [DataMember]
        public Enums.ConceptType ConceptTypeCode { get; set; }
    }
}
