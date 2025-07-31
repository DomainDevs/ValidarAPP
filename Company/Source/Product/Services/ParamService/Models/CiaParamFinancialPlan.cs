namespace Sistran.Company.Application.ProductParamService.Models
{
    using Sistran.Core.Application.ProductParamService.Models.Base;

    /// <summary>
    ///
    /// </summary>
    public class CiaParamFinancialPlan : BaseParamFinancialPlan
    {
        /// <summary>
        /// 
        /// </summary>
        public CiaParamCurrency Currency { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CiaParamPaymentMethod PaymentMethod{ get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CiaParamPaymentSchedule PaymentSchedule { get; set; }
    }
}
