using Sistran.Core.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using COMMEN = Sistran.Core.Application.Common.Entities;
using CommonModel = Sistran.Core.Application.CommonService.Models;
using PRODEN = Sistran.Core.Application.Product.Entities;
using TP = Sistran.Core.Application.Utilities.Utility;
namespace Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs
{
    public class FinancialPlanDAO
    {
        /// <summary>
        /// Obtiene los planes de pago
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public List<FinancialPlan> GetFinancialPlanByProductId(int productId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            FinancialPlanUnderwritingView financialPlanView = null;
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            List<FinancialPlan> financialPlans = new List<FinancialPlan>();
            filter.Property(PRODEN.ProductFinancialPlan.Properties.ProductId, typeof(PRODEN.ProductFinancialPlan).Name).Equal().Constant(productId);
            financialPlanView = new FinancialPlanUnderwritingView();
            ViewBuilder builder = new ViewBuilder("FinancialPlanUnderwritingView");
            builder.Filter = filter.GetPredicate();          
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, financialPlanView);
            }
            //Planes de pago
            if (financialPlanView.ProductFinancialPlanList != null && financialPlanView.ProductFinancialPlanList.Count > 0)
            {
                filter = new ObjectCriteriaBuilder();
                filter.Property("FinancialPlanId", typeof(PRODEN.FinancialPlan).Name);
                filter.In();
                filter.ListValue();
                foreach (PRODEN.ProductFinancialPlan item in financialPlanView.ProductFinancialPlanList)
                {
                    filter.Constant(item.FinancialPlanId);
                }
                filter.EndList();
                financialPlans = GetPaymentSchudeleByFilter(filter.GetPredicate());

                var financialPlansAll = (from financialPlan in financialPlans
                                         join productFinancialPlanList in
                                             financialPlanView.ProductFinancialPlanList.Cast<PRODEN.ProductFinancialPlan>().ToList()
                                             on financialPlan.Id equals productFinancialPlanList.FinancialPlanId
                                         select new FinancialPlan { Id = productFinancialPlanList.FinancialPlanId, Currency = financialPlan.Currency, PaymentMethod = financialPlan.PaymentMethod, PaymentSchedule = financialPlan.PaymentSchedule, IsDefault = productFinancialPlanList.IsDefault }).ToList();
                financialPlans = financialPlansAll;
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.ProductServices.EEProvider.DAOs.GetFinancialPlanByProductId");

            return financialPlans;
        }
        /// <summary>
        /// Gets the payment schudele b filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException">excepcion en  GetPaymentSchudeleByFilter</exception>
        private List<FinancialPlan> GetPaymentSchudeleByFilter(Predicate filter)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            FinancialPlanRelatedEntitiesView view = null;
            try
            {
                view = new FinancialPlanRelatedEntitiesView();
                ViewBuilder builder = new ViewBuilder("FinancialPlanRelatedEntitiesView");
                if (filter != null)
                {
                    builder.Filter = filter;
                }
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    daf.FillView(builder, view);
                }
               

                List<FinancialPlan> FinancialPlanList = ModelAssembler.CreateFinancialPlans(view.FinancialPlanList);
                TP.Parallel.ForEach(FinancialPlanList, financialPlan =>
                {
                    financialPlan.Currency.Description = view.CurrencyList?.Cast<COMMEN.Currency>().ToList()?.FirstOrDefault(x => x.CurrencyCode == financialPlan.Currency.Id)?.Description;
                    financialPlan.PaymentMethod.Description = view.PaymentMethodList?.Cast<COMMEN.PaymentMethod>().ToList()?.FirstOrDefault(x => x.PaymentMethodCode == financialPlan?.PaymentMethod?.Id)?.Description;
                    financialPlan.PaymentSchedule.Description = view.PaymentScheduleList?.Cast<PRODEN.PaymentSchedule>().ToList()?.FirstOrDefault(x => x.PaymentScheduleId == financialPlan?.PaymentSchedule?.Id)?.Description;
                });
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.ProductServices.EEProvider.DAOs.GetPaymentSchudeleByFilter");
                return FinancialPlanList;
            }
            catch (Exception exc)
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.ProductServices.EEProvider.DAOs.GetPaymentSchudeleByFilter");

                throw new BusinessException("excepcion en  GetPaymentSchudeleByFilter", exc);
            }
        }



        /// <summary>
        /// Gets the payment schudele by currencies.
        /// </summary>
        /// <param name="currencies">The currencies.</param>
        /// <returns></returns>
        public List<FinancialPlan> GetPaymentSchudeleByCurrencies(List<CommonModel.Currency> currencies)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            if (currencies != null && currencies.Count > 0)
            {
                filter.Property("CurrencyCode", typeof(PRODEN.FinancialPlan).Name);
                filter.In();
                filter.ListValue();
                foreach (CommonModel.Currency item in currencies)
                {
                    filter.Constant(item.Id);
                }
                filter.EndList();

            }
            return GetPaymentSchudeleByFilter(filter.GetPredicate());
        }
    }
}
