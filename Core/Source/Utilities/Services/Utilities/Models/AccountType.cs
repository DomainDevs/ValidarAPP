using System.Runtime.Serialization;

namespace Sistran.Core.Application.UtilitiesServices.Models
{
    [DataContract]
    public class AccountType
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public bool IsEnabled { get; set; }
    }
}
