using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniqueUserServices.Models.Base
{
    [DataContract]
    public class BaseAccessObject : Extension
    {
        /// <summary>
        /// Id 
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Id 
        /// </summary>
        [DataMember]
        public int AccessObjectId { get; set; }

        /// <summary>
        /// Id del access
        /// </summary>
        [DataMember]
        public int AccessId { get; set; }

        /// <summary>
        /// Description 
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Url 
        /// </summary>
        [DataMember]
        public string Url { get; set; }

        /// <summary>
        /// Enable 
        /// </summary>
        [DataMember]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Enable Description 
        /// </summary>
        [DataMember]
        public string EnabledDescription { get; set; }

        /// <summary>
        /// Visible 
        /// </summary>
        [DataMember]
        public bool Visible { get; set; }

        /// <summary>
        /// ObjectTypeId
        /// </summary>
        [DataMember]
        public int ObjectTypeId { get; set; }

        /// <summary>
        /// Access Type Description 
        /// </summary>
        [DataMember]
        public string AccessTypeDescription { get; set; }

        /// <summary>
        /// Estado del modulo(modificado o eliminado)
        /// </summary>
        [DataMember]
        public string Status { get; set; }

        /// <summary>
        /// Si está asignado o no
        /// </summary>
        [DataMember]
        public bool Assigned { get; set; }

        /// <summary>
        /// ParentAccessId
        /// </summary>
        [DataMember]
        public int ParentAccessId { get; set; }
    }
}
