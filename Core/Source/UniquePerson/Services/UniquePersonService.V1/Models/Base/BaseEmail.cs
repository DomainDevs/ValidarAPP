using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.V1.Models.Base
{
    [DataContract]
    public class BaseEmail : BaseGeneric
    {
        /// <summary>
        /// Es Email principal?
        /// </summary>
        [DataMember]
        public bool IsPrincipal { get; set; }
    }
}
