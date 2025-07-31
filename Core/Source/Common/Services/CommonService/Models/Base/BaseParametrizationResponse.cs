using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.CommonService.Models.Base
{
    [DataContract]
    public class BaseParametrizationResponse : Extension
    {
        [DataMember]
        public int? TotalAdded { get; set; }
        [DataMember]
        public string ErrorAdded { get; set; }
        [DataMember]
        public int? TotalModify { get; set; }
        [DataMember]
        public string ErrorModify { get; set; }
        [DataMember]
        public int? TotalDeleted { get; set; }
        [DataMember]
        public string ErrorDeleted { get; set; }
    }
}
