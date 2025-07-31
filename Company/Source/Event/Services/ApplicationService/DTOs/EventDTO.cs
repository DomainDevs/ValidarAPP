using System.Runtime.Serialization;

namespace Sistran.Company.Application.Event.ApplicationService.DTOs
{
    [DataContract]
    public class EventDTO : GenericListDTO
    {
        [DataMember]
        public int GroupEventId { set; get; }

        [DataMember]
        public string GroupEventDescription { set; get; }

        [DataMember]
        public int ValidationTypeId { set; get; }

        [DataMember]
        public string ValidationTypeDescription { set; get; }

        [DataMember]
        public string ProcedureName { set; get; }

        [DataMember]
        public int ConditionId { set; get; }

        [DataMember]
        public string ConditionDescription { set; get; }

        [DataMember]
        public bool EnabledStop { set; get; }

        [DataMember]
        public bool EnabledAuthorize { set; get; }

        [DataMember]
        public string DescriptionErrorMessage { set; get; }

        [DataMember]
        public bool Enabled { set; get; }

        [DataMember]
        public int? TypeCode { set; get; }
    }
}
