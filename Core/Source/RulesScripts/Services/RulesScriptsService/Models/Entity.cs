using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{

    [DataContract]
    public class Entity
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
