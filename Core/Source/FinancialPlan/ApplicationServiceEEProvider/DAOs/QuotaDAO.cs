using Sistran.Core.Application.FinancialPlanServices.DTOs;
using Sistran.Core.Application.FinancialPlanServices.EEProvider.Assemblers;
using Sistran.Core.Application.FinancialPlanServices.EEProvider.Models;
using Sistran.Core.Application.FinancialPlanServices.Enums;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FPLANEN = Sistran.Core.Application.FinancialPlan.Entities;
using TP = Sistran.Core.Application.Utilities.Utility;
namespace Sistran.Core.Application.FinancialPlanServices.EEProvider.DAOs
{
    /// <summary>
    /// 
    /// </summary>
    public class QuotaDAO
    {
        /// <summary>
        /// Creates the quotas.
        /// </summary>
        /// <param name="finanPlanModel">The finan plan model.</param>
        /// <returns></returns>
        public static bool CreateQuotas(FinanPlanModel finanPlanModel)
        {
            int sequentialId = 0;          
            if (finanPlanModel.QuotaPlan != null)
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(FPLANEN.FinPayer.Properties.EndorsementId, finanPlanModel.endorsementId);
                sequentialId = GetSequentialByEndorsementId(finanPlanModel.endorsementId);
                sequentialId = sequentialId + 1;
                FinPayerModel finPayerModel = ModelAssembler.CreateFinPayer(finanPlanModel);
                finPayerModel.SequentialId = sequentialId;
                finPayerModel.UserId = BusinessContext.Current.UserId;
                FPLANEN.FinPayer finPayer = EntityAssembler.CreateFinPayer(finPayerModel);
                finPayer.AccountingDate = DelegateService.commonServices.GetModuleDateIssue((int)ModuleType.Emision, DateTime.Now);
                finPayer.FinancialDate = DateTime.Now;
                finPayer.IsCurrent = true;
                DataFacadeManager.Instance.GetDataFacade().InsertObject(finPayer);

                finanPlanModel.QuotaPlan.AsParallel().ForAll(m => m.FinPayerId = finPayer.FinPayerId);
                finanPlanModel.QuotaPlanComponents.AsParallel().ForAll(m => m.PaymentId = finPayer.FinPayerId);
                List<FinancialNumber> financialNumbers = CreateQuotasPlan(finanPlanModel);
                finanPlanModel.QuotaPlanComponents.ForEach(a =>
                {
                    a.PaymentId = financialNumbers.Where(m => m.Number == a.Number).First().Id;
                    a.FinPayerPaymentLbSbModel.AsParallel().ForAll(b =>
                    {
                        b.PaymentId = a.PaymentId;
                        b.ComponentId = a.ComponentId;
                    });
                });
                CreateQuotasCompPlan(finanPlanModel);
                CreateQuotasCompLbSbPlan(finanPlanModel);
                DelegateService.integrationUnderwritingService.CreateEndorsementQuotas(finanPlanModel.endorsementId, sequentialId);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Creates the quotas plan.
        /// </summary>
        /// <param name="finanPlanModel">The finan plan model.</param>
        /// <returns></returns>
        private static List<FinancialNumber> CreateQuotasPlan(FinanPlanModel finanPlanModel)
        {
            List<FinancialNumber> financialNumbers = new List<FinancialNumber>();
            List<FPLANEN.FinPayerPayment> finPayers = EntityAssembler.CreateFinPayerPayments(finanPlanModel.QuotaPlan);
            BusinessCollection BusinessCollection = new BusinessCollection();
            FinancialNumber dyObj = new FinancialNumber();
            object objLock = new object();
            TP.Parallel.ForEach<FinPayerPaymentModel, FinancialNumber>(finanPlanModel.QuotaPlan, () => new FinancialNumber(), (quotaPlan, state, financialNumber) =>
             {
                 FPLANEN.FinPayerPayment finPayer = EntityAssembler.CreateFinPayerPayment(quotaPlan);
                 DataFacadeManager.Instance.GetDataFacade().InsertObject(finPayer);
                 financialNumber.Id = finPayer.PayerPaymentId;
                 financialNumber.Number = quotaPlan.PaymentNumber;

                 return financialNumber;
             }, (finalResult) =>
             {
                 lock (objLock)
                 {
                     financialNumbers.Add(finalResult);
                 }
             });
            return financialNumbers;
        }
        private static void CreateQuotasCompPlan(FinanPlanModel finanPlanModel)
        {

            List<FPLANEN.FinPayerPaymentComp> finPayersComp = EntityAssembler.CreateFinPayerPaymentComp(finanPlanModel.QuotaPlanComponents);
            BusinessCollection BusinessCollection = new BusinessCollection();
            TP.Parallel.ForEach(finPayersComp, finPayerComp =>
            {
                DataFacadeManager.Instance.GetDataFacade().InsertObject(finPayerComp);
            }
            );
        }
        private static void CreateQuotasCompLbSbPlan(FinanPlanModel finanPlanModel)
        {

            List<FPLANEN.FinPayerPaymentCompLbsb> finPayers = EntityAssembler.CreateFinPayerPaymentCompLbsb(finanPlanModel.QuotaPlanComponents.SelectMany(a => a.FinPayerPaymentLbSbModel).ToList());
            BusinessCollection BusinessCollection = new BusinessCollection();
            TP.Parallel.ForEach(finPayers, finPayer =>
            {
                DataFacadeManager.Instance.GetDataFacade().InsertObject(finPayer);
            }
            );
        }

        public void DeleteComponentsByFilter(ObjectCriteriaBuilder filter)
        {
            DataFacadeManager.Instance.GetDataFacade().Delete<FPLANEN.FinPayer>(filter.GetPredicate());
            DataFacadeManager.Instance.GetDataFacade().Delete<FPLANEN.FinPayerPayment>(filter.GetPredicate());
        }

        public static int GetSequentialByEndorsementId(int endorsementId)
        {
            int SequentialMax = 0;
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.Property(FPLANEN.FinPayer.Properties.EndorsementId);
            criteriaBuilder.Equal();
            criteriaBuilder.Constant(endorsementId);

            SelectQuery selectQuery = new SelectQuery();
            Function f = new Function(FunctionType.Max);
            f.AddParameter(new Column(FPLANEN.FinPayer.Properties.SequentialId, typeof(FPLANEN.FinPayer).Name));
            selectQuery.AddSelectValue(new SelectValue(f, typeof(FPLANEN.FinPayer).Name));
            selectQuery.Table = new ClassNameTable(typeof(FPLANEN.FinPayer), typeof(FPLANEN.FinPayer).Name);
            selectQuery.Where = criteriaBuilder.GetPredicate();
            selectQuery.GetFirstSelect();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    SequentialMax = Convert.ToInt32(reader[0]);
                }
            }
            return SequentialMax;
        }
    }
}
