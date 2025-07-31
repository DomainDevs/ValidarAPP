using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UniquePersonService.V1.Models.Base
{
    public class BaseInsuredAgent
    {
        [DataMember]
        public bool IsMain { get; set; }
    }
}
