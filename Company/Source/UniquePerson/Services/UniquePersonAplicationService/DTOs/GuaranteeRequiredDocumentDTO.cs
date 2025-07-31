using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonAplicationServices.DTOs
{
    /// <summary>
    /// Contragarantias Documentacion
    /// </summary>
    [DataContract]
    public class GuaranteeRequiredDocumentDTO 
    {
        /// <summary>
        /// Obtiene o Setea el Identificador
        /// </summary>
        /// <value>
        /// Identificador
        /// </value>
        [DataMember]
        public int GuaranteeCode { get; set; }

        /// <summary>
        /// Codigo Documento
        /// </summary>
        /// <value>
        /// Codigo Documento
        /// </value>
        [DataMember]
        public int DocumentCode { get; set; }

        /// <summary>
        /// Descripcion Documentacion requerida
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
