using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.EventsServices.Models
{
    [DataContract]
    public class SubModule
    {
        /// <summary>
        /// Atributo para la propiedad moduleCode.
        /// </summary>
        [DataMember]
        public int ModuleCode { set; get; }

        /// <summary>
        /// Atributo para la propiedad submoduleCode.
        /// </summary>
        [DataMember]
        public int SubmoduleCode { set; get; }

        /// <summary>
        /// Atributo para la propiedad Description.
        /// </summary>
        [DataMember]
        public string Description {set;get;}


        /// <summary>
        /// Atributo para la propiedad Enabled.
        /// </summary>
        [DataMember]
        public bool Enabled { set; get; }

        /// <summary>
        /// Atributo para la propiedad ExpirationDate.
        /// </summary>
        [DataMember]
        public DateTime? ExpirationDate { set; get; }

        /// <summary>
        /// Atributo para la propiedad VirtualFolder.
        /// </summary>
        [DataMember]
        public string VirtualFolder { set; get; }

        /// <summary>
        /// Atributo para la propiedad ParentModuleCode.
        /// </summary>
        [DataMember]
        public int? ParentModuleCode { set; get; }

        /// <summary>
        /// Atributo para la propiedad ParentSubmoduleCode.
        /// </summary>
        [DataMember]
        public int? ParentSubmoduleCode { set; get; }
    }
}
