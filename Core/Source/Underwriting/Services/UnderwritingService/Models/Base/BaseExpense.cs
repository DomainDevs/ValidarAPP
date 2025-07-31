using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    /// <summary>
    /// Gastos de Emision
    /// </summary>

    [DataContract]
    public class BaseExpense : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int id { get; set; }

        /// <summary>
        /// Descripcion del gasto
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Abreviacion del gasto
        /// </summary>
        [DataMember]
        public string Abbreviation { get; set; }

        /// <summary>
        /// Tasa del gasto seleccionado
        /// </summary>
        [DataMember]
        public decimal Rate { get; set; }

        /// <summary>
        /// Tipo de Tasa del Gasto
        /// </summary>
        [DataMember]
        public int RateType { get; set; }

        /// <summary>
        /// Nombre de Tasa del Gasto
        /// </summary>
        [DataMember]
        public string RateTypeName { get; set; }

        /// <summary>
        /// Id Regla de Negocio
        /// </summary>
        [DataMember]
        public int? RuleSet { get; set; }

        /// <summary>
        /// Nombre de Regla de Negocio
        /// </summary>
        [DataMember]
        public string RuleSetName { get; set; }

        /// <summary>
        /// Obligatoriedad del Gasto
        /// </summary>
        [DataMember]
        public bool Mandatory { get; set; }

        /// <summary>
        /// Valicacion de inclusion inicial
        /// </summary>
        [DataMember]
        public bool InitiallyIncluded { get; set; }

        /// <summary>
        /// Id de Clase de componente
        /// </summary>
        [DataMember]
        public int ComponentClass { get; set; }

        /// <summary>
        /// Id de tipo de componente
        /// </summary>
        [DataMember]
        public int ComponentType { get; set; }
    }
}
