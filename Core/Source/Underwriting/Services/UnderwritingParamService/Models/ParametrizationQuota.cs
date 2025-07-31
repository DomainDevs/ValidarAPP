// -----------------------------------------------------------------------
// <copyright file="ParametrizationQuota.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>

namespace Sistran.Core.Application.UnderwritingParamService.Models
{
    using Sistran.Core.Application.UnderwritingParamService.Models.Base;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    /// <summary>
    /// Distribucion de cuota de plan de pago
    /// </summary>
    [DataContract]
    public class ParametrizationQuota: BaseParametrizationQuota
    {
        [DataMember]
        public List<ParametrizacionQuotaTypeComponent> ListQuotaComponent { get; set; }
    }
}
