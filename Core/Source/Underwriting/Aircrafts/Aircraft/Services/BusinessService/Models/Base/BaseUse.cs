
using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Aircrafts.AircraftBusinessService.Models.Base
{
    [DataContract]
    public class BaseUse : BaseGeneric
    {
        [DataMember]
        public int PrefixCode { get; set; }
    }
}
