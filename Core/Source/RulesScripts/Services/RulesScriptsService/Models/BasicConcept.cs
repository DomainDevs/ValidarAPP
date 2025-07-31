using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    /// <summary>
    /// Concept
    /// </summary>
    [DataContract]
    public class BasicConcept : Concept
    {
        /// <summary>
        /// Código de Tipo Basico de BasicConcept
        /// </summary>
        [DataMember]
        public Enums.BasicType BasicTypeCode { get; set; }

        /// <summary>
        /// Valor Minimo
        /// </summary>
        [DataMember]
        public int? MinValue { get; set; }

        /// <summary>
        /// Valor Maximo
        /// </summary>
        [DataMember]
        public int? MaxValue { get; set; }

        [DataMember]
        public DateTime? MinDate { get; set; }

        /// <summary>
        /// Valor Maximo
        /// </summary>
        [DataMember]
        public DateTime? MaxDate { get; set; }


        [DataMember]
        public int? Length { get; set; }
    }
}
