using Sistran.Core.Application.UnderwritingServices.Models.Base;

using System;
using System.Runtime.Serialization;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System.Collections.Generic;
using Sistran.Core.Application.UniquePersonService.V1.Models;

namespace Sistran.Core.Application.UnderwritingServices.Models
{
    [DataContract]
    public class Holder : IssuanceInsured
    {

        [DataMember]
        public int EmailId { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public bool RegimeType { get; set; }

        [DataMember]
        public List<InsuredFiscalResponsibility> FiscalResponsibility { get; set; }
    }
}