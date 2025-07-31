//Sistran
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Underwriting;
using Sistran.Core.Application.CommonService.Models;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{
    public class ApplicationPremiumTransactionItem
    {
        /// <summary>
        /// Id 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Policy:Poliza que se carga al item de imputación
        /// </summary>
        public Policy Policy { get; set; }

        /// <summary>
        /// DeductCommission: comisión descontada
        /// </summary>
        public Amount DeductCommission { get; set; }

        /// <summary>
        /// prima
        /// </summary>
        public Amount Amount { get; set; }

        /// <summary>
        /// Obtiene o Setea si es una Reversion de Primas
        /// </summary>
        /// <value>
        ///   <c>true</c> Reversion de Primas <c>false</c>.
        /// </value>
        public bool IsReversion { get; set; }
    }
}
