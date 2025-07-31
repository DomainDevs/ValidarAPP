using Sistran.Core.Application.Locations.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Location.PropertyServices.Models.Base
{
    [DataContract]
    public class BasePropertyRisk : BaseLocation
    {
        public string Street { get; set; }
    }
}
