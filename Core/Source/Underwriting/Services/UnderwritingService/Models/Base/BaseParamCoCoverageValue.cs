using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Sistran.Core.Application.Extensions;

namespace Sistran.Core.Application.UnderwritingServices.Models
{
    [DataContract]
    public class BaseParamCoCoverageValue: Extension
    {    
        [DataMember]
        public decimal? Percentage { get; set;}

    }
}
