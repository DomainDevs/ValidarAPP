using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Bill
{
    [KnownType("BillModel")]
    public class BillModel
    {
        public int BillId { get; set; }
        public int BillingConceptId { get; set; }
        public int BillControlId { get; set; }
        public DateTime RegisterDate { get; set; }
        public string Description { get; set; }
        public decimal PaymentsTotal { get; set; }
        public int PayerId { get; set; }
        public int SourcePaymentId { get; set; } //usado para regularizacion de cheques
        public List<PaymentSummaryModel> PaymentSummary { get; set; }
        //Para el grabado masivo
        public int UserId { get; set; }
        public int PayerTypeId { get; set; }
        public int PayerDocumentTypeId { get; set; }
        public string PayerDocumentNumber { get; set; }
        public string PayerName { get; set; }
    }

    [KnownType("PaymentSummaryModel")]
    public class PaymentSummaryModel
    {
        public int PaymentId { get; set; }
        public int BillId { get; set; }
        public int PaymentMethodId { get; set; }
        public decimal Amount { get; set; }
        public int CurrencyId { get; set; }
        public decimal LocalAmount { get; set; }
        public decimal ExchangeRate { get; set; }
        public List<CheckModel> CheckPayments { get; set; }
        public List<CreditModel> CreditPayments { get; set; }
        public List<TransferModel> TransferPayments { get; set; }
        public List<DepositVoucherModel> DepositVouchers { get; set; }
        public List<ConsignmentCheckModel> ConsignmentChecks { get; set; }
        public List<RetentionReceiptModel> RetentionReceipts { get; set; }

        public int BranchId { get; set; }

        public int PrefixId { get; set; }
    }

    [KnownType("CheckModel")]
    public class CheckModel
    {
        public string DocumentNumber { get; set; }
        public int IssuingBankId { get; set; }
        public string IssuingAccountNumber { get; set; }
        public string IssuerName { get; set; }
        public DateTime Date { get; set; }
    }

    [KnownType("CreditModel")]
    public class CreditModel
    {
        public string CardNumber { get; set; }
        public string Voucher { get; set; }
        public string AuthorizationNumber { get; set; }
        public int CreditCardTypeId { get; set; }
        public int IssuingBankId { get; set; }
        public string Holder { get; set; }
        public int ValidThruYear { get; set; }
        public int ValidThruMonth { get; set; }
        public decimal TaxBase { get; set; }
    }

    [KnownType("TransferModel")]
    public class TransferModel
    {
        public string DocumentNumber { get; set; }
        public int IssuingBankId { get; set; }
        public string IssuingAccountNumber { get; set; }
        public string IssuerName { get; set; }
        public int ReceivingBankId { get; set; }
        public string ReceivingAccountNumber { get; set; }
        public DateTime Date { get; set; }
    }

    [KnownType("DepositVoucherModel")]
    public class DepositVoucherModel
    {
        public string VoucherNumber { get; set; }
        public int ReceivingAccountBankId { get; set; }
        public string ReceivingAccountNumber { get; set; }
        public DateTime Date { get; set; }
        public string DepositorName { get; set; }
    }

    [KnownType("ConsignmentCheckModel")]
    public class ConsignmentCheckModel
    {
        public string VoucherNumber { get; set; }
        public int ReceivingAccountBankId { get; set; }
        public string ReceivingAccountBankDescription { get; set; }
        public string ReceivingAccountNumber { get; set; }
        public DateTime Date { get; set; }
        public string DepositorName { get; set; }
        public string DocumentNumber { get; set; }
        public int IssuingBankId { get; set; }
        public string IssuingAccountNumber { get; set; }
        public string IssuerName { get; set; }
        public DateTime CheckDate { get; set; }
        public int BankAccountingAccountId { get; set; }
    }

    [KnownType("RetentionReceiptModel")]
    public class RetentionReceiptModel
    {
        public string BillNumber { get; set; }
        public string AuthorizationNumber { get; set; }
        public string SerialNumber { get; set; }
        public string VoucherNumber { get; set; }
        public string SerialVoucherNumber { get; set; }
        public DateTime Date { get; set; }

        public DateTime InvoiceDate { get; set; }

        public DateTime IssueDate { get; set; }

        public DateTime ExpirationDate { get; set; }

        public string PolicyNumber { get; set; }

        public int EndorsementNumber { get; set; }

        public int RetentionConceptId { get; set; }
    }
}