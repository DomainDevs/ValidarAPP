using Sistran.Core.Application.FinancialPlanServices.DTOs;
using Sistran.Core.Framework.BAF;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers.FinancialPlan
{
    /// <summary>
    /// Plan financiero
    /// </summary>
    public class FinancialPlanValidate
    {
        public static void Validate(FilterFinancialPlanDTO filterFinancialPlanDTO)
        {
            if (filterFinancialPlanDTO == null)
            {
                throw new BusinessException(App_GlobalResources.Language.ErrorSavePaymentPlan);
            }
            else if (filterFinancialPlanDTO?.PaymentPlanId < 0)
            {

                throw new BusinessException(App_GlobalResources.Language.ErrorPaymentPlanSchedule);
            }
            else if (filterFinancialPlanDTO?.PaymentMethodId < 0)
            {

                throw new BusinessException(App_GlobalResources.Language.ErrorPaymentMethod);
            }
            else if (filterFinancialPlanDTO?.AccountDate == null)
            {
                throw new BusinessException(App_GlobalResources.Language.ErrorAccountingDate);
            }
            else if (!filterFinancialPlanDTO.IsQuota)
            {

                throw new BusinessException(App_GlobalResources.Language.errorTarifQuota);
            }
        }
    }
}