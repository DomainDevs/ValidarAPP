using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Enums
{
    [DataContract]
    [Flags]
    public enum ProcessTypes
    {
        [EnumMember]
        Online =1, 
         [EnumMember]
        Movement=2, 
         [EnumMember]
        Massive=3, 
         [EnumMember]
        Manual=4, 

    }
}
