using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs
{
    /// <summary>
    /// Message response
    /// </summary>
    [DataContract]
    public class MessageDTO
    {
        /// <summary>
        /// Indicates if operation complete successfull
        /// </summary>
        [DataMember]
        public bool Success { get; set; }

        /// <summary>
        /// Identifier generated (Technical transaction)
        /// </summary>
        [DataMember]
        public int Code { get; set; }

        /// <summary>
        /// Mesage
        /// </summary>
        [DataMember]
        public string Info { get; set; }

        /// <summary>
        /// Source code identifier
        /// </summary>
        [DataMember]
        public int SourceCode { get; set; }

        /// <summary>
        /// Indicates if general ledger complete successfull
        /// </summary>
        [DataMember]
        public bool GeneralLedgerSuccess { get; set; }
    }
}
