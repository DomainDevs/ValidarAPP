using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingGeneralLedgerServices.DTOs
{
    [DataContract]
    public class IdentificationDocumentDTO
    {
        [DataMember]
        public DocumentTypeDTO DocumentType { get; set; }
        [DataMember]
        public string Number { get; set; }
        [DataMember]
        public DateTime ExpeditionDate { get; set; }
    }
}
