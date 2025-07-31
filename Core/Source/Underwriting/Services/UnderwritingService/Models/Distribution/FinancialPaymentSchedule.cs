namespace Sistran.Core.Application.UnderwritingServices.Models.Distribution
{
    public class FinancialPaymentSchedule
    {
        public int Id { get; set; }
        /// <summary>
        /// Iniciar Con La Fecha Mayor Entre Emisión E Inicio De Vigencia
        /// </summary>      
        public bool IsGreaterDate { get; set; }

        /// <summary>
        /// Iniciar Con La Fecha De Emisión
        /// </summary>        
        public bool IsIssueDate { get; set; }
        public short CalculationType { get; set; }
        public short Quantity { get; set; }
    }
}
