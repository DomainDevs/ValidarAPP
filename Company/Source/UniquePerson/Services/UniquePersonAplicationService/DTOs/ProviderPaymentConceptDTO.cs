using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonAplicationServices.DTOs
{
    [DataContract]
    public class ProviderPaymentConceptDTO
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }


        /// <summary>
        /// PaymentConceptId
        /// </summary>
        [DataMember]
        public int PaymentConceptId { get; set; }

        /// <summary>
        /// Descripción
        /// </summary>
        [DataMember]
        public string Description { get; set; }


    }
}
