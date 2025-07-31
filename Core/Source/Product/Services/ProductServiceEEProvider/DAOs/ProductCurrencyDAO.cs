using Sistran.Core.Application.Product.Entities;
using Sistran.Core.Application.ProductServices.EEProvider.Assemblers;
using Sistran.Core.Application.ProductServices.EEProvider.Entities.Views;
using PROMDL = Sistran.Core.Application.ProductServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Diagnostics;
using CommonModel = Sistran.Core.Application.CommonService.Models;

namespace Sistran.Core.Application.ProductServices.EEProvider.DAOs
{
    public class ProductCurrencyDAO
    {
        /// <summary>
        /// Obtener lista de monedas asociadas a un producto
        /// </summary>
        /// <param name="productId">Id producto</param>
        /// <returns></returns>
        public virtual List<CommonModel.Currency> GetCurrenciesByProductId(int productId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ProductCurrencyView view = new ProductCurrencyView();
            ViewBuilder builder = new ViewBuilder("ProductCurrencyView");

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ProductCurrency.Properties.ProductId, typeof(ProductCurrency).Name)
            .Equal().Constant(productId);

            builder.Filter = filter.GetPredicate();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.ProductServices.EEProvider.DAOs.GetCurrenciesByProductId");
            return ModelAssembler.CreateCurrencies(view.Currencies);
        }
        /// <summary>
        /// Obtener lista de monedas asociadas a un producto
        /// </summary>
        /// <param name="productId">Id producto</param>
        /// <returns></returns>
        public virtual List<PROMDL.ProductCurrency> GetProductCurrencies(int productId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ProductCurrencyView view = new ProductCurrencyView();
            ViewBuilder builder = new ViewBuilder("ProductCurrencyView");

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ProductCurrency.Properties.ProductId, typeof(ProductCurrency).Name)
            .Equal().Constant(productId);

            builder.Filter = filter.GetPredicate();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.ProductServices.EEProvider.DAOs.GetCurrenciesByProductId");
            return ModelAssembler.CreateProductCurrencies(view.ProductCurrencies);
        }

    }
}
