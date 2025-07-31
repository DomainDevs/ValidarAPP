using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Underwriting.Base
{
    [DataContract]
    public class BaseIssuancePaymentMethod
    {
        /// <summary>
        /// Obtiene o Setea el Identificador
        /// </summary>
        /// <value>
        /// Identificador
        /// </value>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o Setea el Id Pago
        /// </summary>
        /// <value>
        /// Id Pago
        /// </value>
        [DataMember]
        public int PaymentId { get; set; }

        /// <summary>
        /// Obtiene o Setea el Id Pago
        /// </summary>
        /// <value>
        /// Id Pago
        /// </value>
        [DataMember]
        public string Description { get; set; }
    }
}