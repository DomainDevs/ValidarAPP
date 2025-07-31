using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.DTOs.Salvage
{
    [DataContract]
    public class BuyerDTO
    {
        /// <summary>
        /// IndividualId de Comprador
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// Nombre Completo
        /// </summary>
        [DataMember]
        public string FullName { get; set; }

        /// <summary>
        /// Número de Documento
        /// </summary>
        [DataMember]
        public string DocumentNumber { get; set; }

        /// <summary>
        /// Direccción
        /// </summary>
        [DataMember]
        public string Address { get; set; }

        /// <summary>
        /// Teléfono
        /// </summary>
        [DataMember]
        public string Phone { get; set; }
    }
}
