using System.Runtime.Serialization;

namespace Sistran.Core.Application.EventsServices.Models
{
    [DataContract]
    public class EntityDependencies
    {
        /// <summary>
        /// Atributo para la propiedad conditionId.
        /// </summary>
        [DataMember]
        public int ConditionId { set; get; }

        /// <summary>
        /// Atributo para la propiedad entityId.
        /// </summary>
        [DataMember]
        public int EntityId { set; get; }

        /// <summary>
        /// Atributo para la propiedad dependsId.
        /// </summary>
        [DataMember]
        public int DependsId { set; get; }

        /// <summary>
        /// Atributo para la propiedad ColumnName.
        /// </summary>
        [DataMember]
        public string ColumnName { set; get; }

        /// <summary>
        /// Atributo para la propiedad EntityDescription.
        /// </summary>
        [DataMember]
        public string EntityDescription { set; get; }

        /// <summary>
        /// Atributo para la propiedad DependsDescription.
        /// </summary>
        [DataMember]
        public string DependsDescription { set; get; }
    }
}
