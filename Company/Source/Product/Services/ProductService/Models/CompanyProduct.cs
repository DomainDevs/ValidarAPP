using Sistran.Company.Application.CommonServices.Models;
using Sistran.Core.Application.ProductServices.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.ProductServices.Models
{

    /// <summary>
    /// Extendida de Productos
    /// </summary>
    [DataContract]
    public class CompanyProduct : BaseProduct
    {
        /// <summary>
        /// Atributo para la propiedad CoveredRisk
        /// </summary> 
        [DataMember]
        public CompanyCoveredRisk CoveredRisk { get; set; }

        /// <summary>
        /// Crea un nuevo objeto copiado de la instancia actual.
        /// </summary>
        /// <returns>
        /// Nuevo objeto que es una copia de esta instancia.
        /// </returns>

        /// <summary>
        /// Atributo para la propiedad Ramo
        /// </summary> 
        [DataMember]
        public CompanyPrefix Prefix { get; set; }

        /// <summary>
        /// Es Producto político?
        /// </summary>
        [DataMember]
        public bool IsPoliticalProduct { get; set; }

        /// <summary>
        /// Suma de Incentivo
        /// </summary>
        [DataMember]
        public decimal IncentiveAmt { get; set; }

        /// <summary>
        /// Está habilitado?
        /// </summary>
        [DataMember]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Tiene puntuación?
        /// </summary>
        [DataMember]
        public bool? IsScore { get; set; }

        /// <summary>
        /// Es Bien?
        /// </summary>
        [DataMember]
        public bool? IsFine { get; set; }

        /// <summary>
        /// Usa Fasecolda?
        /// </summary>
        [DataMember]
        public bool? IsFasecolda { get; set; }

        /// <summary>
        /// Day Valid Quote
        /// </summary>
        [DataMember]
        public int? ValidDaysTempQuote { get; set; }


        /// <summary>
        ///Day Valid Policy
        /// </summary>
        [DataMember]
        public int? ValidDaysTempPolicy { get; set; }        

    }
}
