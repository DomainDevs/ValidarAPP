using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Extensions;
using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.V1.Models.Base
{
    public class BaseIndividualTaxExeption : Extension
    {
        [DataMember]
        public int TaxCode { get; set; }

        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public int IndividualTaxExemptionId { get; set; }
        [DataMember]
        public int ExtentPercentage { get; set; }

        [DataMember]
        public DateTime Datefrom { get; set; }

        [DataMember]
        public DateTime? DateUntil { get; set; }

        [DataMember]
        public State StateCode { get; set; }

        [DataMember]
        public int CountryCode { get; set; }

        [DataMember]
        public DateTime? OfficialBulletinDate { get; set; }

        /// <summary>
        /// Obtiene o establece el atributo para la propiedad Apellido.
        /// </summary>
        [DataMember]
        public string ResolutionNumber { get; set; }

        [DataMember]
        public bool TotalRetention { get; set; }

        [DataMember]
        public int RateTaxId { get; set; }

    }
}
