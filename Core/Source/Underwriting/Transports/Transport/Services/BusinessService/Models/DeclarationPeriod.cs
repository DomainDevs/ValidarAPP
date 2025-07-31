using Sistran.Core.Application.Extensions;
using Sistran.Core.Application.Transports.TransportBusinessService.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.Transports.TransportBusinessService.Models
{
    [DataContract]
    public class DeclarationPeriod : BaseGeneric
    {
        [DataMember]
        public bool IsEnabled { get; set; }
    }
}
