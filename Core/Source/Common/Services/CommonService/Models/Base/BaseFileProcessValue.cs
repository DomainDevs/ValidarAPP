using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.CommonService.Models.Base
{
    [DataContract]
    public class BaseFileProcessValue : Extension
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int FileId { get; set; }

        [DataMember]
        public int Key1 { get; set; }

        [DataMember]
        public int Key2 { get; set; }

        [DataMember]
        public int Key3 { get; set; }

        [DataMember]
        public int Key4 { get; set; }

        [DataMember]
        public int Key5 { get; set; }
    }
}
