using System.Runtime.Serialization;
namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    [DataContract]
    public class Documentation 
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public bool IsEnabled { get; set; }
        [DataMember]
        public bool IsRequired { get; set; }
    }
}
