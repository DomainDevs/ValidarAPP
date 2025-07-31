using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Extensions
{
    [DataContract]
    public class BaseGeneric : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }
        /// <summary>
        /// Descripcion
        /// </summary>
        [DataMember]
        public String Description { get; set; }
        /// <summary>
        /// Descripcion Corta
        /// </summary>
        [DataMember]
        public String SmallDescription { get; set; }
    }
}
