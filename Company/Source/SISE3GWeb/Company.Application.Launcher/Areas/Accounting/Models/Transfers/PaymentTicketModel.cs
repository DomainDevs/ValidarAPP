using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Transfers
{
    [KnownType("PaymentTicketModel")]
    public class PaymentTicketModel
    {
        public int Id { get; set; }
        public int? Branch { get; set; }
        public string BranchName { get; set; }
        public int? Bank { get; set; }
        public string BankDescription { get; set; }
        public string AccountNumber { get; set; }
        public string CashAmount { get; set; }
        public string CommissionAmount { get; set; }
        public string Checks { get; set; }
        public string DatePayment { get; set; }
        public int PaymentMethodId { get; set; }
        public string Currency { get; set; }
        public string Amount { get; set; }
        public int PaymentId { get; set; }
        public int Select { get; set; }
        public int PaymentTicketId { get; set; }
        public int PaymentTicketItemId { get; set; }
        public int[] DeleteRecords { get; set; }
        public int[] LogRecords { get; set; }
        public int[] UpdateRecords { get; set; }
        public int VoucherNumber { get; set; }
        public int CreditCardTypeCode { get; set; }
    }

    [KnownType("TblChecksModel")]
    public class TblChecksModel
    {
        public List<PaymentTicketModel> PaymentTicket { get; set; }
    }
}