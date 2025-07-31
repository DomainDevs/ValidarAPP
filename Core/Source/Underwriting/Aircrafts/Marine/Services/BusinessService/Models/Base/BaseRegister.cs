using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Marines.MarineBusinessService.Models.Base
{
    [DataContract]
    public class BaseRegister : BaseGeneric
    {

        /// <summary>
        /// Id Country
        /// </summary>
        [DataMember]
        public int CountryCode { get; set; }
    }
}
