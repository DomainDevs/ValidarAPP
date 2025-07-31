using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.UniqueUserServices.Enums
{
    public class UniqueUserTypes
    {
        [Flags]
        public enum AuthenticationType
        {
            [EnumMember]
            Integrated = 1,
            [EnumMember]
            Standard = 2
        }
    }
}
