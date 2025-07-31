
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

//este DTO debería estar en Imputation y falta agregar el original DepositPremiumTransactionDTO
namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class DepositPremiumTransactionDTO
    {
        [DataMember]
        public int DepositPremiumTransactionId { get; set; }
        [DataMember]
        public int CollectId { get; set; }
        [DataMember]
        public int PayerId { get; set; }
        [DataMember]
        public DateTime RegisterDate { get; set; }
        [DataMember]
        public int CurrencyId { get; set; }
        [DataMember]
        public string CurrencyDescription { get; set; }
        [DataMember]
        public decimal Amount { get; set; } //valor del exceso de pago
        [DataMember]
        public decimal Used { get; set; } //suma de valores usados
        [DataMember]
        public decimal TempAmount { get; set; } //valores que se añadieron en temporales
        [DataMember]
        public decimal UsedAmount { get; set; } //valor a ser ingresado desde la vista
        [DataMember]
        public decimal TotalAmount { get; set; } //Amount - TempAmount - Used

        //indicador de filas para paginación.
        [DataMember]
        public int Rows { get; set; }
    }

}
