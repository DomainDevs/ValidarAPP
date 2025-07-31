using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
   [DataContract]
    public class ReportCollectDTO 
   {
        
        [DataMember]
        public int CollectStatus { get; set; }
        [DataMember]
        public int CollectControlId { get; set; }
        [DataMember]
        public int CollectId { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public string AccountName { get; set; }
        [DataMember]
        public string AccountNumber { get; set; }

        [DataMember]
        public int BranchId { get; set; }
        [DataMember]
        public string BranchDescription { get; set; }  //SUCURSAL

        [DataMember]
        public int Status { get; set; }

        [DataMember]
        public DateTime DateTransaction { get; set; }  //FECHA_TRANSACCION
        [DataMember]
        public int CollectItemId { get; set; }
        [DataMember]
        public string Policy { get; set; }  //POLIZA
        [DataMember]
        public string Endorsement { get; set; }  //ENDOSO
        [DataMember]
        public int Quota { get; set; } //CUOTA
        [DataMember]
        public decimal Amount { get; set; }  //MONTO
        [DataMember]
        public decimal AmountReceived { get; set; }  //MONTO_RECIBIDO
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int CollectConceptId { get; set; }
        [DataMember]
        public string CollectConceptDescription { get; set; }
        [DataMember]
        public int PayerId { get; set; }
        [DataMember]
        public string NamePayer { get; set; }
        [DataMember]
        public decimal TotalCollect { get; set; }  //TOTAL_FACTURA
        [DataMember]
        public int PaymentMethodId { get; set; }
        [DataMember]
        public decimal PaymentAmount { get; set; }  //MONTO_PAGO
        [DataMember]
        public string PaymentMethod { get; set; }
        [DataMember]
        public string CollectDescription { get; set; }
        
        [DataMember]
        public decimal IncomeAmount { get; set; }
        [DataMember]
        public decimal ExchangeRate { get; set; }
        [DataMember]
        public string Holder { get; set; }
        [DataMember]
        public string DocumentNumber { get; set; }
        [DataMember]
        public int CurrencyId { get; set; }
        [DataMember]
        public string CurrencyDescription { get; set; }
       
        [DataMember]
        public int CollectNumber { get; set; } //numero de carátula
         
        [DataMember]
        public string PaymentDescription { get; set; }
        [DataMember]
        public int PaymentId { get; set; }

        [DataMember]
        public int TechnicalTransaction { get; set; }

        [DataMember]
        public DateTime AccountingDate { get; set; }
    }

   [DataContract]
   public class CollectReportCollectDTO 
   {
       [DataMember] 
       public int CollectCode { get; set; }
       [DataMember] 
       public string Description { get; set; }
       [DataMember] 
       public string AccountNumber { get; set; }
       [DataMember] 
       public string DocumentNumber { get; set; }
       [DataMember]
       public int CurrencyId { get; set; }
       [DataMember]
       public string CurrencyDescription { get; set; }
       [DataMember] 
       public decimal Amount { get; set; }
       [DataMember]
       public decimal IncomeAmount { get; set; }
       [DataMember]
       public decimal ExchangeRate { get; set; }
       [DataMember] 
       public string PaymentDescription { get; set; }
       [DataMember] 
       public int PayerId { get; set; }
       [DataMember] 
       public string PayerName { get; set; }
       [DataMember] 
       public int UserId { get; set; }
       [DataMember]
       public int BranchCode { get; set; }
       [DataMember]
       public int Status { get; set; }
       [DataMember]
       public int PaymentId { get; set; }

        [DataMember]
        public int TechnicalTransaction { get; set; }
    }
}
