using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Models.Recovery
{
    [DataContract]
    public class RecoveryControl
    {
        /// <summary>
        /// Acción "I" Insert "U" Update
        /// </summary>
        [DataMember]
        public string Action { get; set; }

        /// <summary>
        /// Identificador del plan de pago
        /// </summary>
        [DataMember]
        public int PaymentPlanId { get; set; }

        /// <summary>
        /// Identificador del recobro
        /// </summary>
        [DataMember]
        public int RecoveryId { get; set; }       

    }
}
