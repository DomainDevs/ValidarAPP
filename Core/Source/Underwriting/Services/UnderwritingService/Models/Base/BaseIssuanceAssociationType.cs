using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    [DataContract]
    public class BaseIssuanceAssociationType
    {
        /// <summary>
        /// Obtiene o Setea el Identificador
        /// </summary>
        /// <value>
        /// Identificador
        /// </value>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o Setea la descripcion
        /// </summary>
        /// <value>
        /// descripcion
        /// </value>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o Setea la descripcion corta
        /// </summary>
        /// <value>
        /// descripcion corta
        /// </value>
        [DataMember]
        public string SmallDescription { get; set; }

    }
}
