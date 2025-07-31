using Sistran.Core.Application.AccountingServices.DTOs.Imputations;
using System;

namespace Sistran.Core.Application.AccountingServices.DTOs.Application
{
    /// <summary>
    /// Aplicacion Primas
    /// </summary>
    public class PremiumRequestDTO
    {
        public PremiumReceivableTransactionDTO PremiumReceivableTransaction { get; set; }
        public int ApplicationId { get; set; }
        public decimal ExchangeRate { get; set; }
        public int UserId { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime AccountingDate { get; set; }
    }
}
