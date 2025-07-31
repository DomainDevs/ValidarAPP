using Sistran.Core.Application.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.Aircrafts.AircraftBusinessService.Models.Base
{
    [DataContract]
    public class BaseAircraftType : BaseGeneric
    {
        [DataMember]
        public bool IsEnabled { get; set; }
        [DataMember]
        public int PrefixCode { get; set; }
    }
}
