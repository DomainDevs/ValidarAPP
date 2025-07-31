using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.TaxServices.Models
{

    /// <summary>
    /// Impuestos
    /// </summary>
    [DataContract]
    public class IndividualTaxCategoryCondition : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// Identificador del departamento
        /// </summary>
        [DataMember]
        public int StateId { get; set; }

        /// <summary>
        /// Identificador del país
        /// </summary>
        [DataMember]
        public int CountryId { get; set; }

        /// <summary>
        /// Número de inscripción
        /// </summary>
        [DataMember]
        public string InscriptionNumber { get; set; }

        /// <summary>
        /// Impuesto
        /// </summary>
        [DataMember]
        public TaxCategoryCondition Tax { get; set; }
    }
}
