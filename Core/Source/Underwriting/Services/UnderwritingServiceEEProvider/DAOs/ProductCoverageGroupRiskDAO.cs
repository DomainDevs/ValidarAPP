using Sistran.Core.Application.Product.Entities;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Diagnostics;
using Model = Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs
{
    public static class ProductCoverageGroupRiskDAO
    {

        /// <summary>
        /// Obtener listado de coberturas del producto
        /// </summary>
        /// <param name="productId">Id producto</param>
        /// <returns></returns>
        public static List<Model.GroupCoverage> GetProductCoverageGroupRiskByProductId(int productId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ProductCoverageGroupRiskView view = new ProductCoverageGroupRiskView();
            ViewBuilder builder = new ViewBuilder("ProductCoverageGroupRiskView");

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ProductGroupCover.Properties.ProductId, typeof(ProductGroupCover).Name);
            filter.Equal();
            filter.Constant(productId);

            builder.Filter = filter.GetPredicate();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }
            List<Model.GroupCoverage> groupCoverages = ModelAssembler.GetProductCoverageGroupRisks(view.ProductGroupCovers, view.CoverGroupRiskTypes);

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetProductCoverageGroupRiskByProductId");

            return groupCoverages;
        }

    }
}
