namespace Sistran.Core.Application.AccountingServices.EEProvider.Models
{
    public class Message
    {
        /// <summary>
        /// Indicates if operaction is succesfull
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Code for trace
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Source code
        /// </summary>
        public int SourceCode { get; set; }

        /// <summary>
        /// Message
        /// </summary>
        public string Info { get; set; }
        
        /// <summary>
        /// Indicates if general ledger complete successfull
        /// </summary>
        public bool GeneralLedgerSuccess { get; set; }

        public Message() { }
    }
}
