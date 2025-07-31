using Sistran.Core.Application.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.Location.PropertyServices.Models
{
    [DataContract]
    public class DeclarationPeriod : BaseGeneric
    {
        [DataMember]
        public bool IsEnabled { get; set; }
    }
}
