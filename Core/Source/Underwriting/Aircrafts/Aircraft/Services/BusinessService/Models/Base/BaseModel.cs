using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Aircrafts.AircraftBusinessService.Models.Base
{
    [DataContract]
    public class BaseModel : BaseGeneric
    {
        /// <summary>
        /// Id Marca
        /// </summary>

        [DataMember]
        public int AircraftMakeCode { get; set; }
    }
}
