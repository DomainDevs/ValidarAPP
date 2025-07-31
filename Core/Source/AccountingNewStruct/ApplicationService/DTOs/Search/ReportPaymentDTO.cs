using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
   [DataContract]
    public class ReportPaymentDTO 
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
         public int BranchId { get; set; }
         [DataMember]
         public string BranchDescription { get; set; }  //SUCURSAL
         [DataMember]
         public int Status { get; set; }
         [DataMember]
         public DateTime DateTransaction { get; set; }  //FECHA_TRANSACCION
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
         public int CollectConceptId { get; set; }
         [DataMember]
         public string CollectConceptDescription { get; set; } 
         [DataMember]
         public decimal IncomeAmount { get; set; }
         [DataMember]
         public decimal Amount { get; set; }
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
         public int PayerId { get; set; }
         [DataMember]
         public int CollectNumber { get; set; } //numero de carátula

        [DataMember]
        public int TechnicalTransaction { get; set; }
    }
}
