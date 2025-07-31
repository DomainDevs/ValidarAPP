using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs
{
    [DataContract]
    public class MessageSuccessDTO
    {
        /// <summary>
        /// Message for collect process
        /// </summary>
        [DataMember]
        public string RecordCollectMessage { get; set; }

        /// <summary>
        /// Message for application process
        /// </summary>
        [DataMember]
        public string ImputationMessage { get; set; }

        /// <summary>
        /// Indicates if must to show message
        /// </summary>
        [DataMember]
        public bool ShowMessage { get; set; }

        /// <summary>
        /// Technical transaction
        /// </summary>
        [DataMember]
        public int TechnicalTransaction { get; set; }

        /// <summary>
        /// Bill identfier
        /// </summary>
        [DataMember]
        public int BillId { get; set; }
        
        /// <summary>
        /// Indicates if general ledger complete successfull
        /// </summary>
        [DataMember]
        public bool GeneralLedgerSuccess { get; set; }
    }
}
