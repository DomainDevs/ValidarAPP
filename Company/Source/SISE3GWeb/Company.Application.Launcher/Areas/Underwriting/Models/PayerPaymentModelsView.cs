using System;

namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Models
{
    public class PayerPaymentModelsView
    {

        /// <summary>
        /// Id temporal
        /// </summary>
        public int TemporalId { get; set; }
        
        /// <summary>
        /// Identificador de Cuota
        /// </summary>
        
        public int Id { get; set; }

        /// <summary>
        /// Porcentaje  de cuota
        /// </summary>

        public decimal Percentage { get; set; }

        /// <summary>
        /// Valor de cuota
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Fecha de vencimiento de cuota
        /// </summary>

        public DateTime DueDate { get; set; }


    }

}