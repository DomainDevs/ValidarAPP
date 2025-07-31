using System.Runtime.Serialization;


namespace Sistran.Company.Application.UnderwritingServices.Models
{
    [DataContract]
    public class EventAuthorization
    {
        [DataMember]
        public int AUTHORIZATION_ID { get; set; }

        [DataMember]
        public int GROUP_EVENT_ID { get; set; }

        [DataMember]
        public int EVENT_ID { get; set; }

        [DataMember]
        public int ACCESS_ID { get; set; }

        [DataMember]
        public int HIERARCHY_CD   { get; set; }

        [DataMember]
        public int EVENT_USER_ID  { get; set; }

        [DataMember]
        public int AUTHO_USER_ID  { get; set; }

        [DataMember]
        public string OPERATION1_ID { get; set; }

        [DataMember]
        public string OPERATION2_ID { get; set; }

        [DataMember]
        public string UrlAccess { get; set; }

    }
}
