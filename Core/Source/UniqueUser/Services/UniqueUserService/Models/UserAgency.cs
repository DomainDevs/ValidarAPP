using Sistran.Core.Application.UniqueUserServices.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UniqueUserServices.Models
{
    [DataContract]
    public class UserAgency : BaseUserAgency
    {
        [DataMember]
        public UserAgent Agent { get; set; }
    }
}