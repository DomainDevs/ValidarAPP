using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.V1.Models.Base
{
    [DataContract]
    public class BaseAddress : BaseGeneric
    {
        /// <summary>
        /// Es la direción principal
        /// </summary>
        [DataMember]
        public bool IsPrincipal { get; set; }

        /// <summary>
        /// Es la tipo de calle
        /// </summary>
        [DataMember]
        public int StreetTypeId { get; set; }
    }
}
