using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.SecurityServices.Models.Base
{
    [DataContract]
    public class BaseModule : Extension
    {
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public bool Disabled { get; set; }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Image { get; set; }
        [DataMember]
        public bool isSearchEnabled { get; set; }
        [DataMember]
        public string Path { get; set; }
        [DataMember]
        public string Title { get; set; }
    }
}
