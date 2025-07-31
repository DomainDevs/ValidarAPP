using System.Runtime.Serialization;

namespace Sistran.Core.Application.EventsServices.Models
{
    [DataContract]
    public class EventEntity
    {
        /// <summary>
        /// Atributo para la propiedad entityId.
        /// </summary>
        [DataMember]
        public int EntityId { set; get; }

        /// <summary>
        /// Atributo para la propiedad Description.
        /// </summary>
        [DataMember]
        public string Description { set; get; }

        /// <summary>
        /// Atributo para la propiedad QueryTypeCode.
        /// </summary>
        [DataMember]
        public EventQueryType QueryType { set; get; }

        /// <summary>
        /// Atributo para la propiedad SourceTable.
        /// </summary>
        [DataMember]
        public string SourceTable { set; get; }

        /// <summary>
        /// Atributo para la propiedad SourceCode.
        /// </summary>
        [DataMember]
        public string SourceCode { set; get; }

        /// <summary>
        /// Atributo para la propiedad SourceDescription.
        /// </summary>
        [DataMember]
        public string SourceDescription { set; get; }

        /// <summary>
        /// Atributo para la propiedad JoinTable.
        /// </summary>
        [DataMember]
        public string JoinTable { set; get; }

        /// <summary>
        /// Atributo para la propiedad JoinSourceField.
        /// </summary>
        [DataMember]
        public string JoinSourceField { set; get; }

        /// <summary>
        /// Atributo para la propiedad JoinTargetField.
        /// </summary>
        [DataMember]
        public string JoinTargetField { set; get; }

        /// <summary>
        /// Atributo para la propiedad ParamWhere.
        /// </summary>
        [DataMember]
        public string ParamWhere { set; get; }

        /// <summary>
        /// Atributo para la propiedad LevelId.
        /// </summary>
        [DataMember]
        public int LevelId { set; get; }

        /// <summary>
        /// Atributo para la propiedad ValidationTypeCode.
        /// </summary>
        [DataMember]
        public EventValidationType ValidationType { set; get; }

        /// <summary>
        /// Atributo para la propiedad ValidationProcedure.
        /// </summary>
        [DataMember]
        public string ValidationProcedure { set; get; }

        /// <summary>
        /// Atributo para la propiedad ValidationTable.
        /// </summary>
        [DataMember]
        public string ValidationTable { set; get; }

        /// <summary>
        /// Atributo para la propiedad ValidationKeyField.
        /// </summary>
        [DataMember]
        public string ValidationKeyField { set; get; }

        /// <summary>
        /// Atributo para la propiedad DataKeyTypeCode.
        /// </summary>
        [DataMember]
        public EventDataType DataKeyType { set; get; }

        /// <summary>
        /// Atributo para la propiedad Key1Field.
        /// </summary>
        [DataMember]
        public string Key1Field { set; get; }

        /// <summary>
        /// Atributo para la propiedad Key2Field.
        /// </summary>
        [DataMember]
        public string Key2Field { set; get; }

        /// <summary>
        /// Atributo para la propiedad Key3Field.
        /// </summary>
        [DataMember]
        public string Key3Field { set; get; }

        /// <summary>
        /// Atributo para la propiedad Key4Field.
        /// </summary>
        [DataMember]
        public string Key4Field { set; get; }

        /// <summary>
        /// Atributo para la propiedad ValidationField.
        /// </summary>
        [DataMember]
        public string ValidationField { set; get; }

        /// <summary>
        /// Atributo para la propiedad DataFieldTypeCode.
        /// </summary>
        [DataMember]
        public EventDataType DataFieldType { set; get; }

        /// <summary>
        /// Atributo para la propiedad WhereAdd.
        /// </summary>
        [DataMember]
        public string WhereAdd { set; get; }

        /// <summary>
        /// Atributo para la propiedad GroupByInd.
        /// </summary>
        [DataMember]
        public bool GroupByInd { set; get; }

        /// <summary>
        /// Atributo para la propiedad NumDependences.
        /// </summary>
        [DataMember]
        public int NumDependences { set; get; }

        /// <summary>
        /// Atributo para la propiedad ConditionId.
        /// </summary>
        [DataMember]
        public int ConditionId { set; get; }
    }
}
