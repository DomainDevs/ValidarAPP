
using CLM = Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.CommonService.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Models.PaymentRequest
{
    [DataContract]
    public class PaymentRequestControl
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Identificador de la solicitud de pago
        /// </summary>
        [DataMember]
        public int PaymentRequestId { get; set; }

        /// <summary>
        /// Acción "I" Insert "U" Update
        /// </summary>
        [DataMember]
        public string Action { get; set; }
    }
}