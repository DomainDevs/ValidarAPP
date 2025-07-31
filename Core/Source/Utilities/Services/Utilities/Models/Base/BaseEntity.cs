using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UtilitiesServices.Models.Base
{
    [DataContract]
    public class BaseEntity : Extension
    {
        /// <summary>
        /// Asunto del Correo 
        /// </summary>
        [DataMember]
        public int EntityId { get; set; }

        /// <summary>
        /// Atributo para la propiedad Description.
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        /// <summary>
        /// Atributo para la propiedad EntityName.
        /// </summary>
        [DataMember]
        public string EntityName { get; set; }
        /// <summary>
        /// Atributo para la propiedad LevelId.
        /// </summary>
        [DataMember]
        public int LevelId { get; set; }
        /// <summary>
        /// Atributo para la propiedad PackageId.
        /// </summary>
        [DataMember]
        public int PackageId { get; set; }
        /// <summary>
        /// Atributo para la propiedad ConfigFile.
        /// </summary>
        [DataMember]
        public string ConfigFile { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string PropertySearch { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string BusinessView { get; set; }
    }
}
