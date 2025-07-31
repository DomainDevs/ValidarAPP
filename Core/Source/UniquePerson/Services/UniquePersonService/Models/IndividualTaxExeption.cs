using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.TaxServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UniquePersonService.Models
  {
    public class IndividualTaxExeption : IndividualTax
    {

        /// <summary>
        /// Obtiene o establece el atributo para la propiedad Fecha de Expiración.
        /// </summary>/// 
   

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
        public TaxCategory TaxCategory { get; set; }

        [DataMember]
        public DateTime? OfficialBulletinDate { get; set; }

        /// <summary>
        /// Obtiene o establece el atributo para la propiedad Apellido.
        /// </summary>
        [DataMember]
        public string ResolutionNumber { get; set; }

        [DataMember]
        public bool TotalRetention { get; set; }


    }
}
