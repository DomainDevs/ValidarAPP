using Sistran.Core.Application.CommonServices.EEProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Diagnostics;
using COMMEN = Sistran.Core.Application.Common.Entities;
using COMMML = Sistran.Core.Application.CommonService.Models;
using PRODEN = Sistran.Core.Application.Product.Entities;

namespace Sistran.Core.Application.CommonServices.EEProvider.DAOs
{
    public class CurrencyDAO
    {
        public List<COMMML.Currency> GetProductCurrencies()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(PRODEN.ProductCurrency)));
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.Providers.GetCurrencies");

            return ModelAssembler.CreateCurrencies(businessCollection);
        }
        public List<COMMML.Currency> GetCurrencies()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.Currency)));
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.Providers.GetCurrencies");

            return ModelAssembler.CreateCurrencies(businessCollection);
        }

        public List<COMMML.Currency> GetCurrenciesByProductId(int productId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COMMEN.Currency.Properties.CurrencyCode, typeof(COMMEN.Currency).Name);
            filter.Equal();
            filter.Constant(productId);            
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.Currency), filter.GetPredicate()));            

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.Providers.GetCurrencies");

            return ModelAssembler.CreateCurrencies(businessCollection);
        }
    }
}
