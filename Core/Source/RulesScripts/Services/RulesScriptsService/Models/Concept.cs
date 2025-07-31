using System.Runtime.Serialization;
using enumRule = Sistran.Core.Application.RulesScriptsServices.Enums;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class Concept
    {
        /// <summary>
        /// Identificador de Concepto
        /// </summary>
        [DataMember]
        public int ConceptId { get; set; }

        /// <summary>
        /// Identificador de Entidad
        /// </summary>
        [DataMember]
        public int EntityId { get; set; }

        /// <summary>
        /// Descripción
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Nombre de ListEntityo
        /// </summary>
        [DataMember]
        public string ConceptName { get; set; }

        /// <summary>
        /// Orden en la Clave
        /// </summary>
        [DataMember]
        public int KeyOrder { get; set; }

        /// <summary>
        /// Es Estatico o Dinamico
        /// </summary>
        [DataMember]
        public bool IsStatic { get; set; }

        /// <summary>
        /// Código de Control Web
        /// </summary>
        [DataMember]
        public enumRule.ConceptControlType ConceptControlCode { get; set; }

        /// <summary>
        /// Código de Control Web
        /// </summary>
        [DataMember]
        public enumRule.ConceptType ConceptTypeCode { get; set; }

        /// <summary>
        /// Es visible
        /// </summary>
        [DataMember]
        public bool IsVisible { get; set; }

        /// <summary>
        /// Permite Nulos
        /// </summary>
        [DataMember]
        public bool IsNull { get; set; }

        /// <summary>
        /// Es Persitible
        /// </summary>
        [DataMember]
        public bool IsPersistible { get; set; }

        [DataMember]
        public int OrderNum { get; set; }

        [DataMember]
        public Question Question { get; set; }
    }
}
