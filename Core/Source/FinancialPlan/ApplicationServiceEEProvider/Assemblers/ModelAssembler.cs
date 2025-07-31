using Sistran.Core.Application.FinancialPlanServices.EEProvider.Models;

namespace Sistran.Core.Application.FinancialPlanServices.EEProvider.Assemblers
{
    public class ModelAssembler
    {
        public static FinPayerModel CreateFinPayer(FinanPlanModel finanPlanModel)
        {
            var Immaper = AutoMapperAssembler.CreateMapFinPayerModel();
            return Immaper.Map<FinanPlanModel, FinPayerModel>(finanPlanModel);
        }
    }
}
