using System.Collections.Generic;
using System.Linq;
using Sistran.Core.Application.UnderwritingServices.Models;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Application.Vehicles.ThirdPartyLiabilityService.EEProvider.Entities.views;
using Sistran.Core.Framework.DAF.Engine;
using System.Diagnostics;
using COMMEN = Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.Vehicles.ThirdPartyLiabilityService.EEProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;

namespace Sistran.Core.Application.Vehicles.ThirdPartyLiabilityService.EEProvider.DAOs
{
    public class ProductDeductibleDAO
    {
        /// <summary>
        /// Obtener deducibles por producto
        /// </summary>
        /// <param name="productId">Id producto</param>
        /// <returns>Lista de deducibles</returns>
        public List<Deductible> GetDeductiblesByProductId(int productId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COMMEN.DeductibleProduct.Properties.ProductId, typeof(COMMEN.DeductibleProduct).Name);
            filter.Equal();
            filter.Constant(productId);

            ProductDeductibleView view = new ProductDeductibleView();
            ViewBuilder builder = new ViewBuilder("ProductDeductibleView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            List<Deductible> deductibles = new List<Deductible>();
            if (view.DeductibleProducts.Count > 0)
            {
                deductibles = ModelAssembler.CreateDeductibles(view.Deductibles);

                foreach (Deductible item in deductibles)
                {
                    PARAMEN.DeductibleUnit deductibleUnit = view.DeductibleUnits.Cast<PARAMEN.DeductibleUnit>().ToList().FirstOrDefault();
                    item.DeductibleUnit.Description = deductibleUnit.Description;

                    deductibleUnit = view.MinimumDeductibleUnits.Cast<PARAMEN.DeductibleUnit>().ToList().FirstOrDefault();
                    item.MinDeductibleUnit.Description = deductibleUnit.Description;

                    COMMEN.Currency currency = view.Currencies.Cast<COMMEN.Currency>().ToList().FirstOrDefault();
                    item.Description = item.DeductValue.ToString() + " " + item.DeductibleUnit.Description;
                    if (currency != null)
                    {
                        item.Currency.Description = currency.Description;
                        item.Description += "(" + item.Currency.Description + ")";
                    }

                    item.Description += " - " + item.MinDeductValue.ToString() + " " + item.MinDeductibleUnit.Description;
                }               
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.Vehicles.ThirdPartyLiabilityService.EEProvider.DAOs.GetDeductiblesByProductId");
            return deductibles;
        }
    }
}
