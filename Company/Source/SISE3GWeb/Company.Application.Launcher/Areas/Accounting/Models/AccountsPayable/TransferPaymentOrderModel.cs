using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.AccountsPayable
{
    [KnownType("TransferPaymentOrderModel")]
    public class TransferPaymentOrderModel
    {
        public int Id { get; set; }
        public int AccountBankId { get; set; } //cuenta emisora
        public DateTime DeliveryDate { get; set; }
        public DateTime CancellationDate { get; set; }
        public int StatusId { get; set; }
        public int UserId { get; set; }
        public List<PaymentOrderTransferModel> PaymentOrdersItems { get; set; }
    }

    [KnownType("PaymentOrderTransferModel")]
    public class PaymentOrderTransferModel
    {
        public int PaymentOrderId { get; set; }
        public int AccountBankId { get; set; } //id cuenta receptora
        // Aumentado para BE
        public int BankId { get; set; }//id banco de cuenta receptora
        
        public string AccountBankNumber { get; set; } //número de cuenta receptora
    }
}