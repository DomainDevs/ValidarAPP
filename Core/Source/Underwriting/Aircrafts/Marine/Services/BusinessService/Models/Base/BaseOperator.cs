using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Marines.MarineBusinessService.Models.Base
{
    [DataContract]
    public class BaseOperator : BaseGeneric
    {
        /// <summary>
        /// DocumentNumber
        /// </summary>

        [DataMember]
        public string DocumentNumber { get; set; }
    }
}
