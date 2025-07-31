using Sistran.Co.Application.Data;
using Sistran.Core.Application.Product.Entities;
using Sistran.Core.Application.UnderwritingServices.Constants;
using Sistran.Core.Application.UnderwritingServices.DTOs.Filter;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Resources;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models.Distribution;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using Model = Sistran.Core.Application.UnderwritingServices.Models;
using PRODEN = Sistran.Core.Application.Product.Entities;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs
{
    public class PaymentPlanDAO
    {

        /// <summary>
        /// Obtener Plan De Pago Por Identificador
        /// </summary>
        /// <param name="paymentPlanId">Identificador</param>
        /// <returns>Plan De Pago</returns>
        public Model.PaymentPlan GetPaymentPlanByPaymentPlanId(int paymentPlanId)
        {
            PrimaryKey key = PaymentSchedule.CreatePrimaryKey(paymentPlanId);
            PaymentSchedule entityPaymentSchedule = (PaymentSchedule)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

            if (entityPaymentSchedule != null)
            {
                return ModelAssembler.CreatePaymentPlan(entityPaymentSchedule);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Obtener Plan De Pago Por Identificador de poliza
        /// <param name="policyId">Identificador</param>
        /// <returns>Plan De Pago</returns>
        public Model.PaymentPlan GetPaymentPlanByPolicyId(int policyId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(ISSEN.EndorsementPayer.Properties.EndorsementId, policyId);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(ISSEN.EndorsementPayer), filter.GetPredicate()));
            if (businessCollection != null && businessCollection.Any())
            {
                return GetPaymentPlanByPaymentPlanId(businessCollection.Cast<ISSEN.EndorsementPayer>().First().PaymentScheduleId);
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// Calcular Cuotas
        /// </summary>
        /// <param name="policy">Póliza</param>
        /// <returns>Cuotas</returns>
        //public List<Model.Quota> CalculateQuotas(Model.Policy policy)
        //{
        //    List<Model.Quota> quotas = new List<Model.Quota>();
        //    DateTime dateStartQuota = new DateTime();
        //    if (policy.PaymentPlan != null)
        //    {
        //        PaymentSchedule paymentSchedule = GetPaymentScheduleByPaymentPlanId(policy.PaymentPlan.Id);

        //        if (paymentSchedule != null)
        //        {
        //            List<Model.PaymentDistribution> distributions = GetPaymentDistributionByPaymentPlanId(policy.PaymentPlan.Id);

        //            if (paymentSchedule.IsGreaterDate == true)
        //            {
        //                if (DateTime.Compare(policy.IssueDate, policy.CurrentFrom) < 0)
        //                {
        //                    dateStartQuota = policy.CurrentFrom;
        //                }
        //                else
        //                {
        //                    dateStartQuota = policy.IssueDate;
        //                }
        //            }
        //            else if (paymentSchedule.IsIssueDate == true)
        //            {
        //                dateStartQuota = policy.IssueDate;
        //            }
        //            else
        //            {
        //                dateStartQuota = policy.CurrentFrom;
        //            }

        //            foreach (Model.PaymentDistribution distribution in distributions)
        //            {
        //                decimal percentageQuota = Convert.ToDecimal(distribution.Percentage);
        //                decimal amountQuota = decimal.Round(((policy.Summary.FullPremium * percentageQuota) / 100), QuoteManager.DecimalRound);

        //                Model.Quota quota = new Model.Quota();
        //                quota.ExpirationDate = CalculateExpirationDate(distribution.CalculationType.GetValueOrDefault(PaymentCalculationType.Month), dateStartQuota, distribution.CalculationQuantity.GetValueOrDefault(1));
        //                quota.Number = distribution.Number;
        //                quota.Percentage = percentageQuota;
        //                quota.Amount = amountQuota;
        //                quotas.Add(quota);
        //                dateStartQuota = quota.ExpirationDate;
        //            }

        //            if (policy.Summary.FullPremium != quotas.Sum(q => q.Amount))
        //            {
        //                decimal[] quotasRound = QuoteManager.RoundCollection(quotas.Select(x => x.Amount).ToArray(), policy.Summary.FullPremium, 2);
        //                quotas[quotas.Count - 1].Amount = quotasRound[quotas.Count - 1];
        //            }
        //        }
        //    }
        //    else
        //    {
        //        throw new Exception("Plan de pago Vacio");
        //    }
        //    return quotas;
        //}

        /// <summary>
        /// Calcular Fecha De Pago
        /// </summary>
        /// <param name="gapUnitType">Tipo De Unidad</param>
        /// <param name="dateStart">Fecha Inicial</param>
        /// <param name="quantityAdd">Cantidad A Incrementar</param>
        /// <returns>Fecha De Pago</returns>
        private DateTime CalculateExpirationDate(PaymentCalculationType calculationType, DateTime dateStart, int quantityAdd)
        {
            switch (calculationType)
            {
                case PaymentCalculationType.Day:
                    dateStart = dateStart.AddDays(quantityAdd);
                    break;
                case PaymentCalculationType.Fortnight:
                    dateStart = dateStart.AddDays(quantityAdd * 15);
                    break;
                case PaymentCalculationType.Month:
                    dateStart = dateStart.AddMonths(quantityAdd);
                    break;
            }

            return dateStart;
        }


        /// <summary>
        /// Gets the payment plans by product identifier.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns></returns>
        public List<Model.PaymentPlan> GetPaymentPlansByProductId(int productId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ProductFinancialPlan.Properties.ProductId, typeof(ProductFinancialPlan).Name);
            filter.Equal();
            filter.Constant(productId);

            PaymentPlanView view = new PaymentPlanView();
            ViewBuilder builder = new ViewBuilder("PaymentPlanView");
            builder.Filter = filter.GetPredicate();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }
            if (view != null && view.ProductFinancialPlans.Any())
            {
                List<Model.PaymentPlan> paymentPlans = ModelAssembler.CreatePaymentPlans(view.PaymentSchedules);

                var paymenPlanDefault = view.ProductFinancialPlans.Cast<PRODEN.ProductFinancialPlan>().FirstOrDefault(x => x.IsDefault == true);
                if (paymenPlanDefault != null)
                {
                    var financialPlan = view.FinancialPlans.Cast<PRODEN.FinancialPlan>().FirstOrDefault(x => x.FinancialPlanId == paymenPlanDefault.FinancialPlanId);
                    paymentPlans.First(x => x.Id == financialPlan.PaymentScheduleId).IsDefault = true;
                }
                else
                {
                    throw new Exception(Errors.ErrorNotExistPaymentPlan);
                }

                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetGroupCoveragesByProductId");
                return paymentPlans;
            }
            else
            {
                throw new Exception(Errors.ErrorNotExistPaymentPlan);
            }
        }

        /// <summary>
        /// Gets the payment plan by product identifier.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns></returns>
        public Model.PaymentPlan GetDefaultPaymentPlanByProductId(int productId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ProductFinancialPlan.Properties.ProductId, typeof(ProductFinancialPlan).Name);
            filter.Equal();
            filter.Constant(productId);
            filter.And();
            filter.Property(ProductFinancialPlan.Properties.IsDefault, typeof(ProductFinancialPlan).Name);
            filter.Equal();
            filter.Constant(true);

            PaymentPlanView view = new PaymentPlanView();
            ViewBuilder builder = new ViewBuilder("PaymentPlanView");
            builder.Filter = filter.GetPredicate();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }
            if (view != null && view.ProductFinancialPlans.Any())
            {

                var paymenPlanDefault = view.ProductFinancialPlans.Cast<PRODEN.ProductFinancialPlan>().FirstOrDefault(x => x.IsDefault == true);
                if (paymenPlanDefault != null)
                {
                    var financialPlan = view.FinancialPlans.Cast<PRODEN.FinancialPlan>().FirstOrDefault(x => x.FinancialPlanId == paymenPlanDefault.FinancialPlanId);
                    var paymentPlan = (PRODEN.PaymentSchedule)view.PaymentSchedules.Where(x => ((PRODEN.PaymentSchedule)x).PaymentScheduleId == financialPlan.PaymentScheduleId).FirstOrDefault();
                    Model.PaymentPlan paymentPlanModel = ModelAssembler.CreatePaymentPlan(paymentPlan);
                    paymentPlanModel.IsDefault = paymenPlanDefault.IsDefault;
                    stopWatch.Stop();
                    Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetGroupCoveragesByProductId");
                    return paymentPlanModel;
                }
                else
                {
                    throw new Exception(Errors.ErrorNotExistPaymentPlan);
                }
            }
            else
            {
                throw new Exception(Errors.ErrorNotExistPaymentPlan);
            }
        }
        /// <summary>
        /// Obtener datos plan de pago
        /// </summary> 
        /// <param name="paymentPlanId">Identificador del plan de pago</param>
        /// <returns></returns>
        private PaymentSchedule GetPaymentScheduleByPaymentPlanId(int paymentPlanId)
        {
            PrimaryKey key = PaymentSchedule.CreatePrimaryKey(paymentPlanId);
            var result = (PaymentSchedule)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            DataFacadeManager.Dispose();
            return result;
        }

        /// <summary>
        /// Obtener datos de las cuotas
        /// </summary> 
        /// <param name="paymentPlanId">Identificador del plan de pago</param>
        /// <returns></returns>
        public static List<Model.PaymentDistribution> GetPaymentDistributionByPaymentPlanId(int paymentPlanId)
        {
            BusinessCollection businessCollection = null;
            List<Model.PaymentDistribution> paymentDistributions = new List<Model.PaymentDistribution>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PaymentDistribution.Properties.PaymentScheduleId);
            filter.Equal();
            filter.Constant(paymentPlanId);
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(PaymentDistribution), filter.GetPredicate()));
            }

            if (businessCollection != null)
            {
                paymentDistributions = ModelAssembler.CreatePaymentDistributions(businessCollection);
                return paymentDistributions;
            }
            return null;
        }

        /// <summary>
        /// Creates the payment distribution.
        /// </summary>
        /// <param name="endorsementId">The endorsement identifier.</param>
        /// <returns></returns>
        public bool CreatePaymentDistribution(int endorsementId)
        {
            DataTable result = null;
            NameValue[] parameters = new NameValue[1];
            parameters[0] = new NameValue("ENDORSEMENT_ID", endorsementId);
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("ISS.CREATE_PAYER_PAYMENT_COMP", parameters);
            }
            if (result != null)
            {
                if (((int)result.Rows[0][0]) == UnderwritingConstant.Save)
                {
                    return true;
                }
                else if (((int)result.Rows[0][0]) == UnderwritingConstant.NotSave)
                {
                    return false;
                }
            }
            return false;
        }
        #region nuevo Calculo Componentes
        public static FinancialPaymentSchedule GetPaymentsScheduleBySheduleId(PaymentScheduleFilterDTO filterBase)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.Property(PRODEN.PaymentSchedule.Properties.PaymentScheduleId, typeof(PRODEN.PaymentSchedule).Name);
            criteriaBuilder.Equal();
            criteriaBuilder.Constant(filterBase.Id);

            SelectQuery selectQuery = new SelectQuery();
            selectQuery.AddSelectValue(new SelectValue(new Column(PRODEN.PaymentSchedule.Properties.IsGreaterDate, typeof(PRODEN.PaymentSchedule).Name), PRODEN.PaymentSchedule.Properties.IsGreaterDate));
            selectQuery.AddSelectValue(new SelectValue(new Column(PRODEN.PaymentSchedule.Properties.IsIssueDate, typeof(PRODEN.PaymentSchedule).Name), PRODEN.PaymentSchedule.Properties.IsIssueDate));
            selectQuery.AddSelectValue(new SelectValue(new Column(PRODEN.PaymentSchedule.Properties.GapUnitCode, typeof(PRODEN.PaymentSchedule).Name), PRODEN.PaymentSchedule.Properties.GapUnitCode));
            selectQuery.AddSelectValue(new SelectValue(new Column(PRODEN.PaymentSchedule.Properties.GapQuantity, typeof(PRODEN.PaymentSchedule).Name), PRODEN.PaymentSchedule.Properties.GapQuantity));
            selectQuery.AddSelectValue(new SelectValue(new Column(PRODEN.PaymentSchedule.Properties.FirstPayQuantity, typeof(PRODEN.PaymentSchedule).Name), PRODEN.PaymentSchedule.Properties.FirstPayQuantity));
            selectQuery.Table = new ClassNameTable(typeof(PRODEN.PaymentSchedule), typeof(PRODEN.PaymentSchedule).Name);
            selectQuery.Where = criteriaBuilder.GetPredicate();
            FinancialPaymentSchedule financialPaymentSchedule = new FinancialPaymentSchedule();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                financialPaymentSchedule = reader.SelectReader(
                    read => new FinancialPaymentSchedule
                    {
                        Id = filterBase.Id,
                        IsGreaterDate = Convert.ToBoolean(read[PRODEN.PaymentSchedule.Properties.IsGreaterDate]),
                        IsIssueDate = Convert.ToBoolean(read[PRODEN.PaymentSchedule.Properties.IsIssueDate]),
                        CalculationType = Convert.ToInt16(read[PRODEN.PaymentSchedule.Properties.GapUnitCode]),
                        Quantity = Convert.ToInt16(read[PRODEN.PaymentSchedule.Properties.FirstPayQuantity])
                    }).FirstOrDefault();
            }
            return financialPaymentSchedule;
        }
        public static List<PaymentDistributionPlan> GetFinancialPaymentDistributionByPaymentPlanId(int paymentPlanId)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.Property(PRODEN.CoPaymentDistributionComponent.Properties.PaymentScheduleId, typeof(PRODEN.CoPaymentDistributionComponent).Name);
            criteriaBuilder.Equal();
            criteriaBuilder.Constant(paymentPlanId);

            SelectQuery selectQuery = new SelectQuery();
            selectQuery.AddSelectValue(new SelectValue(new Column(PRODEN.CoPaymentDistributionComponent.Properties.PaymentNumber, typeof(PRODEN.CoPaymentDistributionComponent).Name), PRODEN.CoPaymentDistributionComponent.Properties.PaymentNumber));
            selectQuery.AddSelectValue(new SelectValue(new Column(PRODEN.CoPaymentDistributionComponent.Properties.ComponentCode, typeof(PRODEN.CoPaymentDistributionComponent).Name), PRODEN.CoPaymentDistributionComponent.Properties.ComponentCode));
            selectQuery.AddSelectValue(new SelectValue(new Column(PRODEN.CoPaymentDistributionComponent.Properties.value, typeof(PRODEN.CoPaymentDistributionComponent).Name), PRODEN.CoPaymentDistributionComponent.Properties.value));

            selectQuery.Table = new ClassNameTable(typeof(PRODEN.CoPaymentDistributionComponent), typeof(PRODEN.CoPaymentDistributionComponent).Name);
            selectQuery.Where = criteriaBuilder.GetPredicate();
            List<PaymentDistributionPlan> paymentDistributionPlans = new List<PaymentDistributionPlan>();
            dynamic dynamic = new ExpandoObject();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                var result = reader.SelectReader(
                    read => new
                    {
                        Number = Convert.ToInt32(read[PRODEN.CoPaymentDistributionComponent.Properties.PaymentNumber]),
                        ComponentId = Convert.ToInt16(read[PRODEN.CoPaymentDistributionComponent.Properties.ComponentCode]),
                        Percentage = Convert.ToDecimal(read[PRODEN.CoPaymentDistributionComponent.Properties.value])

                    }).ToList();
                var data = result.GroupBy(m => m.Number);
                PaymentDistributionPlan paymentDistributionPlan = new PaymentDistributionPlan();
                foreach (IGrouping<int, dynamic> distribution in data)
                {
                    paymentDistributionPlan = new PaymentDistributionPlan();
                    paymentDistributionPlan.Number = distribution.Key;                  
                    List<PaymentDistributionComp> PaymentDistributionComps = new List<PaymentDistributionComp>();
                    PaymentDistributionComps = distribution.Select(a => new PaymentDistributionComp { ComponentId = a.ComponentId, Percentage = a.Percentage }).ToList();
                    paymentDistributionPlan.PaymentDistributionComp = PaymentDistributionComps;
                    paymentDistributionPlans.Add(paymentDistributionPlan);
                }

            }

            return paymentDistributionPlans;

        }
        #endregion nuevo Calculo Componentes

    }
}