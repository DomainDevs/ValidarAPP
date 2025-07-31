using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePerson.IntegrationService.Models
{

    [DataContract]
    public class IdentificationDocumentDTO
    {
        /// <summary>
        /// Number
        /// </summary>
        [DataMember]
        public string Number { get; set; }

        /// <summary>
        /// ExpeditionDate
        /// </summary>
        [DataMember]
        public DateTime ExpeditionDate { get; set; }
        [DataMember]
        public DocumentTypeDTO DocumentType { get; set; }
    }
}