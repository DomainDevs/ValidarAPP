using Sistran.Company.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Company.Application.UnderwritingServices.EEProvider.ModelsMapper;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingParamService.EEProvider.Entities.Views;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF.Engine;
using System.Collections.Generic;
using System.Linq;
using ParamEntities = Sistran.Core.Application.Parameters.Entities;
using QuotationEntities = Sistran.Core.Application.Quotation.Entities;

namespace Sistran.Company.Application.UnderwritingBusinessServiceProvider.DAO
{
    public class SurchargeDAO
    {
        public List<CompanySurchargeComponent> GetSurcharges()
        {
            List<CompanySurchargeComponent> surcharges = new List<CompanySurchargeComponent>();
            ViewBuilder viewBuilder = new ViewBuilder("SurchargeView");
            SurchargeView surchargeView = new SurchargeView();
            try
            {
                DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, surchargeView);
                foreach (QuotationEntities.SurchargeComponent componentQuotation in surchargeView.Surcharge)
                {
                    QuotationEntities.SurchargeComponent surchargeComponent = surchargeView.Surcharge.Cast<QuotationEntities.SurchargeComponent>().First(x => x.ComponentCode == componentQuotation.ComponentCode);
                    QuotationEntities.Component component = surchargeView.Component.Cast<QuotationEntities.Component>().First(x => x.ComponentCode == componentQuotation.ComponentCode);
                    ParamEntities.RateType rateType = surchargeView.RateType.Cast<ParamEntities.RateType>().First(x => x.RateTypeCode == componentQuotation.RateTypeCode);

                    CompanySurchargeComponentMapper companySurchargeComponentMapper = new CompanySurchargeComponentMapper();
                    companySurchargeComponentMapper.component = component;
                    companySurchargeComponentMapper.surchargeComponent = surchargeComponent;
                    companySurchargeComponentMapper.rateType = rateType;
                    surcharges.Add(ModelAssembler.CreateCompanySurcharge(companySurchargeComponentMapper));
                }
                return surcharges;
            }
            catch (System.Exception ex)
            {
                throw new BusinessException("excepcion en SurchargeDAO.GetSurcharges", ex);
            }
        }
    }
}
