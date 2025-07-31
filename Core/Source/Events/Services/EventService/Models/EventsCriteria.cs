using System.Runtime.Serialization;

namespace Sistran.Core.Application.EventsServices.Models
{
    [DataContract]
    public class EventsCriteria
    {
        /// <summary>
        /// Atributo para la propiedad IdModule.
        /// </summary>
        [DataMember]
        public int IdModule { get; set; }
        /// <summary>
        /// Atributo para la propiedad IdSubmodule.
        /// </summary>
        [DataMember]
        public int IdSubmodule { get; set; }
        /// <summary>
        /// Atributo para la propiedad ObjectName.
        /// </summary>
        [DataMember]
        public string ObjectName { get; set; }
        /// <summary>
        /// Atributo para la propiedad IdUser.
        /// </summary>
        [DataMember]
        public int IdUser { get; set; }
        /// <summary>
        /// Atributo para la propiedad IdTemp.
        /// </summary>
        [DataMember]
        public string IdTemp { get; set; }
        /// <summary>
        /// Atributo para la propiedad key1.
        /// </summary>
        [DataMember]
        public string key1 { get; set; }
        /// <summary>
        /// Atributo para la propiedad key2.
        /// </summary>
        [DataMember]
        public string key2 { get; set; }
        /// <summary>
        /// Atributo para la propiedad key3.
        /// </summary>
        [DataMember]
        public string key3 { get; set; }
        /// <summary>
        /// Atributo para la propiedad key4.
        /// </summary>
        [DataMember]
        public string key4 { get; set; }
    }
}