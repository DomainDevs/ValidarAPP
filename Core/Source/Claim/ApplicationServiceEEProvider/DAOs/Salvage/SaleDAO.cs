using Sistran.Core.Application.ClaimServices.EEProvider.Models.Salvage;
using CLMEN = Sistran.Core.Application.Claims.Entities;
using System.Collections.Generic;
using System.Linq;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.ClaimServices.EEProvider.Assemblers;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Application.ClaimServices.EEProvider.Views.Salvage;
using Sistran.Core.Framework.DAF.Engine;
using PAYMEN = Sistran.Core.Application.Claims.Entities;

namespace Sistran.Core.Application.ClaimServices.EEProvider.DAOs.Salvage
{
    public class SaleDAO
    {
        public Sale CreateSale(Sale sale, int salvageId)
        {
            CLMEN.Sale entitySale = EntityAssembler.CreateSale(sale, salvageId);

            if (entitySale.CancellationReasonCode == 0)
            {
                entitySale.CancellationReasonCode = null;
            }

            DataFacadeManager.Insert(entitySale);

            sale.Id = entitySale.SaleCode;

            return sale;
        }

        public Sale UpdateSale(Sale sale, int salvageId)
        {
            CLMEN.Sale entitySale = EntityAssembler.CreateSale(sale, salvageId);
            DataFacadeManager.Update(entitySale);
            sale.Id = entitySale.SaleCode;

            return sale;
        }


        public List<Sale> GetSalesBySalvageId(int salvageId)
        {
            List<Sale> sales = new List<Sale>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(CLMEN.Sale.Properties.SalvageCode, typeof(CLMEN.Sale).Name, salvageId);

            sales = ModelAssembler.CreateSales(DataFacadeManager.GetObjects(typeof(CLMEN.Sale), filter.GetPredicate()));

            return sales;
        }

        public Sale GetSaleBySaleId(int saleId)
        {
            Sale sale = new Sale();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(CLMEN.Sale.Properties.SaleCode, typeof(CLMEN.Sale).Name);
            filter.Equal();
            filter.Constant(saleId);

            SaleView saleView = new SaleView();
            ViewBuilder viewBuilder = new ViewBuilder("SaleView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, saleView);

            if (saleView.Sales.Count > 0)
            {
                sale = ModelAssembler.CreateSale((CLMEN.Sale)saleView.Sales.First());

                if (saleView.PaymentPlans.Count > 0)
                {
                    sale.PaymentPlan = ModelAssembler.CreatePaymentPlan((PAYMEN.PaymentPlan)saleView.PaymentPlans.First());
                    sale.PaymentPlan.PaymentQuotas = ModelAssembler.CreatePaymentQuotas(saleView.PaymentSchedules);
                }

                return sale;
            }
            else
            {
                return null;
            }
        }

    }
}
