using Sistran.Core.Application.Extensions;
using System;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniqueUserServices.Models.Base
{
    [DataContract]
    public class BaseSubModule : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Enabled
        /// </summary>
        [DataMember]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Enabled Description
        /// </summary>
        [DataMember]
        public string EnabledDescription { get; set; }

        /// <summary>
        /// ExpirationDate
        /// </summary>
        [DataMember]
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// VirtualFolder
        /// </summary>
        [DataMember]
        public string VirtualFolder { get; set; }

        /// <summary>
        /// ParentModuleId
        /// </summary>
        [DataMember]
        public int ParentModuleId { get; set; }

        /// <summary>
        /// ParentSubModuleId
        /// </summary>
        [DataMember]
        public int ParentSubModuleId { get; set; }

        /// <summary>
        /// Estado del modulo(modificado o eliminado)
        /// </summary>
        [DataMember]
        public string Status { get; set; }

    }
}
