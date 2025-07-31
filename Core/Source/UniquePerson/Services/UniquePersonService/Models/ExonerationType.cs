using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.Models
{
    [DataContract]
    public class ExonerationType : Extension
    {
        [DataMember]
        public int ExonerationTypeCode { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string SmallDescription { get; set; }
        [DataMember]
        public int IndividualTypeCode { get; set; }
        [DataMember]
        public bool Enabled { get; set; }
    }
}
