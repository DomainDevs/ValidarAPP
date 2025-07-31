using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Marines.MarineBusinessService.Models.Base
{
    [DataContract]
    public class BaseModel : BaseGeneric
    {
        /// <summary>
        /// Id Marca
        /// </summary>

        [DataMember]
        public int MarineMakeCode { get; set; }
    }
}
