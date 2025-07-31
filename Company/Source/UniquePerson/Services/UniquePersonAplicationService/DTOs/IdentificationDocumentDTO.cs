using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonAplicationServices.DTOs
{
    [DataContract]
    public class IdentificationDocumentDTO
    {
        [DataMember]
        public string Number { get; set; }

        /// <summary>
        /// Fecha de expedición
        /// </summary>
        [DataMember]
        public DateTime ExpeditionDate { get; set; }

        [DataMember]
        public DocumentTypeDTO DocumentType { get; set; }
    }
}
