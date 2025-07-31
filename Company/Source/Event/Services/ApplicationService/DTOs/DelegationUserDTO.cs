using System.Runtime.Serialization;

namespace Sistran.Company.Application.Event.ApplicationService.DTOs
{
    [DataContract]
    public class DelegationUserDTO
    {
        [DataMember]
        public bool Authorized { get; set; }

        [DataMember]
        public bool Notificated { get; set; }

        [DataMember]
        public bool NotificatedDefault { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public int PersonId { get; set; }
    }
}
