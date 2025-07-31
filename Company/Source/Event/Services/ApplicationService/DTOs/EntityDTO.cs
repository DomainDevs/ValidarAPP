

using System.Runtime.Serialization;

namespace Sistran.Company.Application.Event.ApplicationService.DTOs
{
    [DataContract]
    public class EntityDTO : GenericListDTO
    {

        [DataMember]
        public string EntityDescription { set; get; }

        [DataMember]
        public int QueryTypeId { get; set; }

        [DataMember]
        public string QueryTypeDescription { get; set; }

        [DataMember]
        public string SourceTable { set; get; }

        [DataMember]
        public string SourceCode { set; get; }

        [DataMember]
        public string SourceDescription { set; get; }

        [DataMember]
        public string JoinTable { set; get; }

        [DataMember]
        public string JoinSourceField { set; get; }

        [DataMember]
        public string JoinTargetField { set; get; }

        [DataMember]
        public string ConditionJoinWhere { set; get; }

        [DataMember]
        public int LevelId { set; get; }

        //[DataMember]
        //public EventValidationType ValidationType { set; get; }

        [DataMember]
        public int ValidationTypeId { get; set; }

        [DataMember]
        public string ValidationTypeDescription { get; set; }

        [DataMember]
        public bool Procedure { get; set; }

        [DataMember]
        public string ValidationProcedure { set; get; }

        [DataMember]
        public string ValidationTable { set; get; }

        [DataMember]
        public string ValidationKeyField { set; get; }

        //[DataMember]
        //public EventDataType DataKeyType { set; get; }

        [DataMember]
        public int? DataKeyTypeId { get; set; }

        [DataMember]
        public string DataKeyTypeDescription { get; set; }

        [DataMember]
        public string Key1Field { set; get; }

        [DataMember]
        public string Key2Field { set; get; }

        [DataMember]
        public string Key3Field { set; get; }

        [DataMember]
        public string Key4Field { set; get; }

        [DataMember]
        public string ValidationField { set; get; }

        //[DataMember]
        //public EventDataType DataFieldType { set; get; }

        [DataMember]
        public int? DataFieldTypeId { get; set; }

        [DataMember]
        public string DataFieldTypeDescription { get; set; }

        [DataMember]
        public string ConditionWhere { set; get; }

        [DataMember]
        public bool GroupBy { set; get; }
    }
}
