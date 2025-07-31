using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.DTOs.Salvage
{
    public class PaymentQuotaDTO
    {
        /// <summary>
        /// Identificativo de la cuota
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Valor de la Cuota
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Fecha de Vencimiento de la cuota
        /// </summary>
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// Número de la cuota
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Descripción de la moneda
        /// </summary>
        public string CurrencyDescription { get; set; }
    }
}
