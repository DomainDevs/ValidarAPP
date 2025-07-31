using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    [DataContract]
    public class CompanyGuarantor
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
        /// Id del contragarante
        /// </summary>
        [DataMember]
        public int GuarantorId { get; set; }

        /// <summary>
        /// Valor de contragarantía
        /// </summary>
        [DataMember]
        public decimal? GuaranteeAmount { get; set; }

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
    }
}
