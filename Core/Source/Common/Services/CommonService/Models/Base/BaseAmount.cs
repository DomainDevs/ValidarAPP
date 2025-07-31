using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.CommonService.Models.Base
{
    [DataContract]
    public class BaseAmount : Extension
    {
        [DataMember]
        public decimal Value { get; set; }
        

    }
}
