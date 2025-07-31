using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.SecurityServices.Models
{
    [DataContract]
    public class ControlSecurity : Extension
    {
        [DataMember]
        public string ControlID { get; set; }
        [DataMember]
        public bool Enabled { get; set; }
        [DataMember]
        public bool Visible { get; set; }
    }
}
