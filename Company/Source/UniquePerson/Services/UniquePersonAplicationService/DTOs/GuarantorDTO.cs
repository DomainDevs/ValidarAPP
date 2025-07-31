using Sistran.Core.Services.UtilitiesServices.Enums;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniquePersonAplicationServices.DTOs
{
    [DataContract]
    public class GuarantorDTO
    {

        /// <summary>
        /// Id del individual
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// GuaranteeId
        /// </summary>
        [DataMember]
        public int GuaranteeId { get; set; }

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
        public long PhoneNumber { get; set; }

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


        /// <summary>
        /// Enumeracion para el codigo de la accion
        /// </summary>
        /// <value>
        /// Codigo de la accion
        /// </value>
        [DataMember]
        public ParametrizationStatus ParametrizationStatus { get; set; }
    }
}
