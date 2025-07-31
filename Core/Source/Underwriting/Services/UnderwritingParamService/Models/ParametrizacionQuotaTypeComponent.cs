// -----------------------------------------------------------------------
// <copyright file="ParametrizationQuota.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
namespace Sistran.Core.Application.UnderwritingParamService.Models
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;

    [DataContract]
    public class ParametrizacionQuotaTypeComponent
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int Value { get; set; }
        [DataMember]
        public int PaymentScheduleId { get; set; }
        [DataMember]
        public int PaymentNumber { get; set; }
    }
}
