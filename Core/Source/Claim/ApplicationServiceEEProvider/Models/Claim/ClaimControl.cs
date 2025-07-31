using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.ClaimServices.DTOs.Claims;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim
{
    /// <summary>
    /// Denuncia
    /// </summary>
    [DataContract]
    public class ClaimControl
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Identificador de la denuncia
        /// </summary>
        [DataMember]
        public int ClaimId { get; set; }

        /// <summary>
        /// Identificador de la modificación
        /// </summary>
        [DataMember]
        public int ClaimModifyId { get; set; }

        /// <summary>
        /// Acción "I" Insert "U" Update
        /// </summary>
        [DataMember]
        public string Action { get; set; }
    }
}
