using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.Models
{
    /// <summary>
    /// Fiador
    /// </summary>
    [DataContract]
    public class Guarantor : Extension
    {
        /// <summary>
        ///     Id del afianzado
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// Id de la contragarantía
        /// </summary>
        [DataMember]
        public int GuaranteeId { get; set; }

        /// <summary>
        /// Valor de contragarantía
        /// </summary>
        [DataMember]
        public decimal? GuaranteeAmount { get; set; }

        /// <summary>
        /// Id del contragarante
        /// </summary>
        [DataMember]
        public int GuarantorId { get; set; }

        /// <summary>
        /// Nombre comercial
        /// </summary>
        [DataMember]
        public string TradeName { get; set; }

        /// <summary>
        /// Nombre
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Dirección
        /// </summary>
        [DataMember]
        public string Adrress { get; set; }

        /// <summary>
        /// Teléfono
        /// </summary>
        [DataMember]
        public int PhoneNumber { get; set; }

        /// <summary>
        /// Ciudad
        /// </summary>
        [DataMember]
        public string CityText { get; set; }

        /// <summary>
        /// Número de tarjeta
        /// </summary>
        [DataMember]
        public string CardNro { get; set; }

        /// <summary>
        ///  TributaryIdNo
        /// </summary>
        [DataMember]
        public string TributaryIdNo { get; set; }
    }
}
