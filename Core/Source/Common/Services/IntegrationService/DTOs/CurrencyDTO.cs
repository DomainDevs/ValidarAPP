using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.CommonServices.DTOs
{
    public class CurrencyDTO
    {
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Moneda
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Abreviatura de la descripción
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }

        /// <summary>
        /// Abreviatura 2 de la descripción
        /// </summary>
        [DataMember]
        public string TinyDescription { get; set; }
    }
}
