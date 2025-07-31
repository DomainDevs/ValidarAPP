using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.CommonService.Models.Base
{
    [DataContract]
    public class BaseDefaultValue : Extension
    {
        [DataMember]
        public int ProfileId { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public int ModuleId { get; set; }
        [DataMember]
        public int SubModuleId { get; set; }
        [DataMember]
        public string ViewName { get; set; }
        [DataMember]
        public string ControlName { get; set; }
        [DataMember]
        public string ControlValue { get; set; }
        [DataMember]
        public string ControlType { get; set; }
    }
}
