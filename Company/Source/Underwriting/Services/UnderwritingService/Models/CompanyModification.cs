using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UnderwritingServices.Models
{
    [DataContract]
    public class CompanyModification
    {
        /// <summary>
        /// Texto
        /// </summary>
        [DataMember]
        public string Text { get; set; }

        /// <summary>
        /// Observaciones
        /// </summary>
        [DataMember]
        public string Observations { get; set; }

        /// <summary>
        /// Numero de Radicado
        /// </summary>
        [DataMember]
        public int? RegistrationNumber { get; set; }

        /// <summary>
        /// Fecha de Radicado
        /// </summary>
        [DataMember]
        public DateTime? RegistrationDate { get; set; }
    }
}
