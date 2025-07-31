using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System.Runtime.Serialization;
using Sistran.Core.Application.Extensions;
namespace Sistran.Core.Application.UniquePersonService.V1.Models
{
    [DataContract]
    public class DocumentType : BaseGeneric
    {
        [DataMember]
        public bool IsAlphanumeric { get; set; }
    }
}
