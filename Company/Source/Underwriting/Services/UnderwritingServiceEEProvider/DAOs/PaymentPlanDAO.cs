using Sistran.Co.Application.Data;
using Sistran.Company.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Issuance.Entities;
using Sistran.Core.Application.Product.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CUNMO = Sistran.Core.Application.UnderwritingServices.Models;
using UWMO = Sistran.Core.Application.UnderwritingServices.Models;
using UWPR = Sistran.Core.Application.UnderwritingServices.EEProvider.Assemblers;
using EP = Sistran.Core.Application.Issuance.Entities;
using Sistran.Core.Application.CommonService.Models;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider.DAOs
{
    public class PaymentPlanDAO : Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.PaymentPlanDAO
    {
        /// <summary>
        /// Calcular cuotas mediante el Id de plan de pago
        /// </summary>
        /// <param name="policy"></param>
        /// <returns></returns>
        //public List<UWMO.Quota> CalculateQuotasByPaymentPlanId(UWMO.Policy policy)
        //{
        //    List<UWMO.Quota> listQuota = new List<UWMO.Quota>();
        //    int number = 0;
        //    decimal totalPremiumAmount = policy.Summary.Premium + policy.Summary.Expenses;
        //    decimal totalFirstPaymentPremiumAmount = policy.Summary.Taxes;
        //    decimal totalDistribution = 0;
        //    bool isFirstPayment = true;
        //    List<UWMO.PaymentDistribution> paymentDistributionList = GetPaymentDistributionByPaymentPlanId(policy.PaymentPlan.Id);
        //    if (paymentDistributionList != null && paymentDistributionList.Count > 0)
        //    {
        //        foreach (UWMO.PaymentDistribution paymentDistribution in paymentDistributionList)
        //        {
        //            UWMO.Quota quota = new UWMO.Quota();
        //            quota.Number = ++number;
        //            quota.Percentage = paymentDistribution.Percentage;
        //            totalDistribution += paymentDistribution.Percentage;
        //            quota.ExpirationDate = policy.IssueDate;
        //            quota.Amount = (totalPremiumAmount * paymentDistribution.Percentage) / 100;

        //            // Se agrega el total de los componentes que no se distribuyen en las cuotas.
        //            if (isFirstPayment)
        //            {
        //                quota.Amount += totalFirstPaymentPremiumAmount;
        //                isFirstPayment = false;
        //            }

        //            // Agrega la cuota a la lista de cuotas
        //            listQuota.Add(quota);
        //        }
        //    }
        //    else
        //    {
        //        UWMO.Quota quota = new UWMO.Quota();
        //        totalDistribution = 100;
        //        quota.Number = 1;
        //        quota.Percentage = totalDistribution;
        //        quota.ExpirationDate = policy.IssueDate;
        //        quota.Amount = totalPremiumAmount + totalFirstPaymentPremiumAmount;
        //        // Agrega la cuota a la lista de cuotas
        //        listQuota.Add(quota);
        //    }

        //    // Redondeo de las cuotas
        //    listQuota = RoundPayerPayment(listQuota, totalPremiumAmount + totalFirstPaymentPremiumAmount);

        //    distributePayerPaymentPercentage(listQuota, totalPremiumAmount + totalFirstPaymentPremiumAmount);

        //    if (totalDistribution != 100)
        //    {
        //        string referencia = "Payment Schedule : " + policy.PaymentPlan.Id + " - " + policy.PaymentPlan.Description
        //            + " Total Distribution = " + totalDistribution;
        //        throw new BusinessException("ERRCLC_PAYMENT_DISTRIBUTION_NOT_MATCH_100_PERCENT", new string[] { referencia });
        //    }

        //    return listQuota;
        //}

        /// <summary>
        /// Obtener la distribución de pagos mediante el ID de plan de pago
        /// </summary>
        /// <param name="paymentPlanId"></param>
        /// <returns></returns>
        private new List<UWMO.PaymentDistribution> GetPaymentDistributionByPaymentPlanId(int paymentPlanId)
        {
            List<UWMO.PaymentDistribution> list = new List<UWMO.PaymentDistribution>();
            SelectQuery selectQuery = new SelectQuery();
            selectQuery.Table = new ClassNameTable(typeof(PaymentDistribution));
            ObjectCriteriaBuilder objectCriteriaBuilder = new ObjectCriteriaBuilder();
            objectCriteriaBuilder.Property(PaymentDistribution.Properties.PaymentScheduleId);
            objectCriteriaBuilder.Equal();
            objectCriteriaBuilder.Constant(paymentPlanId);
           
            BusinessCollection businessCollection = null;
            using (IDataFacade daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(PaymentDistribution), objectCriteriaBuilder.GetPredicate()));
            }
            if (businessCollection != null & businessCollection.Any())
            {
                list = UWPR.ModelAssembler.CreatePaymentDistributions(businessCollection);
            }
            return list;
        }

        /// <summary>
        /// Redondear pagos
        /// </summary>
        /// <param name="listQuota"></param>
        /// <param name="totalPremiumAmount"></param>
        /// <returns></returns>
        private List<UWMO.Quota> RoundPayerPayment(List<UWMO.Quota> listQuota, decimal totalPremiumAmount)
        {
            decimal[] roundValues = new decimal[listQuota.Count];

            string antes = "", despues = "";
            int index = 0;
            foreach (UWMO.Quota quota in listQuota)
            {
                antes += " , " + quota.Amount;
                roundValues[index++] = quota.Amount;
            }

            roundValues = RoundCollection(roundValues, totalPremiumAmount, 2);

            index = 0;
            foreach (UWMO.Quota quota in listQuota)
            {
                despues += " , " + quota.Amount;
                quota.Amount = roundValues[index++];
            }

            return listQuota;
        }

        /// <summary>
        /// Redondear Coleccion
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="totalToRound"></param>
        /// <param name="decimalPrecision"></param>
        /// <returns></returns>
        private decimal[] RoundCollection(decimal[] collection, decimal totalToRound, int decimalPrecision)
        {
            int top = collection.GetUpperBound(0);
            int bottom = collection.GetLowerBound(0);
            decimal[] resultList = new decimal[top - bottom + 1];

            decimal roundTotal = 0;
            for (int index = bottom; index <= top; index++)
            {
                decimal roundValue = decimal.Round(collection[index], decimalPrecision);
                roundTotal += roundValue;
                resultList[index] = roundValue;
            }

            // Manda la diferencia al último elemento ...
            resultList[top] += decimal.Round(totalToRound - roundTotal, decimalPrecision);

            return resultList;
        }

        /// <summary>
        /// Distribuir el porcentaje de pago del pagador
        /// </summary>
        /// <param name="payerPaymentList"></param>
        /// <param name="totalPremiumAmount"></param>
        private void distributePayerPaymentPercentage(List<UWMO.Quota> payerPaymentList, decimal totalPremiumAmount)
        {
            decimal totalPremiumAmountRounded = decimal.Round(totalPremiumAmount, 2);
            decimal percentages = 0;
            //Asigna como máximo a la primer fecha.
            UWMO.Quota lastQuote = payerPaymentList[0];

            foreach (UWMO.Quota quota in payerPaymentList)
            {
                //Busca en toda la colección la cuota de fecha más alta.
                if (lastQuote.ExpirationDate < quota.ExpirationDate)
                {
                    //Asigna la cuota más alta.
                    lastQuote = quota;
                }
            }

            foreach (UWMO.Quota quota in payerPaymentList)
            {
                if (quota != lastQuote)
                {
                    if (totalPremiumAmountRounded != 0)
                    {
                        quota.Percentage = decimal.Round(quota.Amount * 100 / totalPremiumAmountRounded, 2);
                        percentages += quota.Percentage;
                    }
                    else
                    {
                        percentages = 0;
                    }
                }
            }
            lastQuote.Percentage = 100 - percentages;
        }

        /// <summary>
        /// Calcular cuotas por programa de pago
        /// </summary>
        /// <param name="policy"></param>
        /// <returns></returns>
        public List<UWMO.Quota> CalculateQuotasByPaymentSchedule(UWMO.Policy policy)
        {
            List<UWMO.Quota> listQuotas = new List<UWMO.Quota>();

            PaymentPlanDAO paymentPlanDAO = new PaymentPlanDAO();
            PaymentScheduleDAO paymentScheduleDAO = new PaymentScheduleDAO();
            CompanyPaymentSchedule paymentSchedule = paymentScheduleDAO.GetPaymentScheduleByPaymentPlanId(policy.PaymentPlan.Id);

            DateTime currentFrom = policy.CurrentFrom;
            DateTime currentTo = policy.CurrentTo;
            DateTime issueDate = policy.IssueDate;
            DateTime expirationDate = DateTime.Now;
            if (paymentSchedule == null)
            {
                return listQuotas;
            }
            if (paymentSchedule.IsGreaterDate)
            {
                if (currentFrom > issueDate)
                {
                    expirationDate = currentFrom;
                }
                else
                {
                    expirationDate = issueDate;
                }
            }
            else
            {
                if (paymentSchedule.IsIssueDate)
                {
                    expirationDate = issueDate;
                }
                else
                {
                    expirationDate = currentFrom;
                }
            }
            expirationDate.AddDays(paymentSchedule.FirstPayerQuantity);
            List<UWMO.PaymentDistribution> paymentDistributionList = GetPaymentDistributionByPaymentPlanId(policy.PaymentPlan.Id);
            decimal totalPremiumAmount = policy.Summary.Premium + policy.Summary.Expenses;
            decimal totalFirstPaymentPremiumAmount = policy.Summary.Taxes;
            decimal totalDistribution = 0;
            bool isFirstPayment = true;
            int number = 0;
            foreach (UWMO.PaymentDistribution paymentDistribution in paymentDistributionList)
            {
                UWMO.Quota quota = new UWMO.Quota();
                quota.Number = ++number;
                quota.Percentage = paymentDistribution.Percentage;
                totalDistribution += paymentDistribution.Percentage;
                quota.Amount = (totalPremiumAmount * paymentDistribution.Percentage) / 100;
                quota.ExpirationDate = expirationDate;

                if (isFirstPayment)
                {
                    quota.Amount += totalFirstPaymentPremiumAmount;
                    isFirstPayment = false;
                }

                // Agrega la cuota a la lista de cuotas
                listQuotas.Add(quota);
            }
            decimal diffPremiumAmount = totalPremiumAmount - listQuotas.AsEnumerable().Sum(q => q.Amount);
            int paymentNumLast = listQuotas.AsEnumerable().Max(q => q.Number);
            if (diffPremiumAmount > 0)
            {
                UWMO.Quota updateQuota = listQuotas.AsEnumerable().Where(q => q.Number == paymentNumLast).FirstOrDefault();
                updateQuota.Amount += diffPremiumAmount;
            }

            foreach (UWMO.Quota quota in listQuotas)
            {
                quota.Percentage = Math.Round((quota.Amount * 100 / policy.Summary.FullPremium), 2);
            }
            return listQuotas;
        }

        public void CreateCompanyPolicyPayer(CompanyPolicy companyPolicy)
        {
            NameValue[] parameters = new NameValue[9];
            parameters[0] = new NameValue("@ENDORSEMENT_ID", companyPolicy.Endorsement.Id);
            parameters[1] = new NameValue("@POLICY_ID", companyPolicy.Endorsement.PolicyId);
            parameters[2] = new NameValue("@POLICYHOLDER_ID", companyPolicy.Holder.IndividualId);
            parameters[3] = new NameValue("@PAYMENT_SCHEDULE_ID", companyPolicy.PaymentPlan.Id);
            parameters[4] = new NameValue("@PAYMENT_METHOD_CD", companyPolicy.Holder.PaymentMethod.Id);
            parameters[5] = new NameValue("@MAIL_ADDRESS_ID", companyPolicy.Holder.CompanyName.Address.Id);
            parameters[6] = new NameValue("@PAYMENT_ID", companyPolicy.Holder.PaymentMethod.PaymentId);

            #region PayerPayment
            DataTable dtQuotas = new DataTable("INSERT_TEMP_PAYER_PAYMENT");
            dtQuotas.Columns.Add("CUSTOMER_TYPE_CD", typeof(int));
            dtQuotas.Columns.Add("PAYER_ID", typeof(int));
            dtQuotas.Columns.Add("PAYMENT_NUM", typeof(int));
            dtQuotas.Columns.Add("PAY_EXP_DATE", typeof(DateTime));
            dtQuotas.Columns.Add("PAYMENT_PCT", typeof(decimal));
            dtQuotas.Columns.Add("AMOUNT", typeof(decimal));
            dtQuotas.Columns.Add("ENDORSEMENT_ID", typeof(int));
            dtQuotas.Columns.Add("PREFIX_CD", typeof(int));
            dtQuotas.Columns.Add("AGT_PAY_EXP_DATE", typeof(DateTime));
            if (companyPolicy.PaymentPlan.Quotas != null)
            {
                foreach (CUNMO.Quota quota in companyPolicy.PaymentPlan.Quotas)
                {
                    DataRow dataRowQuota = dtQuotas.NewRow();
                    dataRowQuota["CUSTOMER_TYPE_CD"] = companyPolicy.Holder.CustomerType;
                    dataRowQuota["PAYER_ID"] = companyPolicy.Holder.IndividualId;
                    dataRowQuota["PAYMENT_NUM"] = quota.Number;
                    dataRowQuota["PAY_EXP_DATE"] = quota.ExpirationDate;
                    dataRowQuota["PAYMENT_PCT"] = quota.Percentage;
                    dataRowQuota["AMOUNT"] = quota.Amount;
                    dataRowQuota["PREFIX_CD"] = companyPolicy.Prefix.Id;

                    dtQuotas.Rows.Add(dataRowQuota);
                }
            }
            parameters[7] = new NameValue("@INSERT_TEMP_PAYER_PAYMENT", dtQuotas);
            #endregion PayerPayment

            #region FirstPayerCompoment
            DataTable dtParamFirstPayer = new DataTable("PARAM_TEMP_FIRST_PAY_COMP");
            dtParamFirstPayer.Columns.Add("CUSTOMER_TYPE_CD", typeof(int));
            dtParamFirstPayer.Columns.Add("PAYER_ID", typeof(int));
            dtParamFirstPayer.Columns.Add("COMPONENT_CD", typeof(int));
            dtParamFirstPayer.Columns.Add("ENDORSEMENT_ID", typeof(int));
            dtParamFirstPayer.Columns.Add("PREFIX_CD", typeof(int));
            if (companyPolicy.PayerComponents != null && companyPolicy.PayerComponents.Count > 0)
            {
                DataRow rwFirstPayComponent = dtParamFirstPayer.NewRow();
                rwFirstPayComponent["CUSTOMER_TYPE_CD"] = companyPolicy.Holder.CustomerType;
                rwFirstPayComponent["PAYER_ID"] = companyPolicy.Holder.IndividualId;
                rwFirstPayComponent["COMPONENT_CD"] = companyPolicy.PayerComponents.First().Component.Id;
                rwFirstPayComponent["ENDORSEMENT_ID"] = DBNull.Value;
                rwFirstPayComponent["PREFIX_CD"] = DBNull.Value;
                dtParamFirstPayer.Rows.Add(rwFirstPayComponent);
            }
            parameters[8] = new NameValue("@INSERT_PARAM_TEMP_FIRST_PAY_COMP", dtParamFirstPayer);
            #endregion

            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                pdb.ExecuteSPNonQuery("ISS.RECORD_POLICY_PAYER", parameters);
            }
        }

        /// <summary>
        /// Obtener Plan De Pago Por Identificador
        /// </summary>
        /// <param name="paymentPlanId">Identificador</param>
        /// <returns>Plan De Pago</returns>
        public CompanyPaymentPlan GetPaymentPlanByPaymentPlanId(int paymentPlanId)
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
        public CompanyPaymentPlan GetPaymentPlanByPolicyId(int policyId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(EndorsementPayer.Properties.EndorsementId, policyId);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(EndorsementPayer), filter.GetPredicate()));
            if (businessCollection != null && businessCollection.Any())
            {
                return GetPaymentPlanByPaymentPlanId(businessCollection.Cast<EndorsementPayer>().First().PaymentScheduleId);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Trae registro de la tabla PAYMENT_SCHEDULE por policyId
        /// </summary>
        /// <param name="policyId"></param>
        public int GetPaymentPlanScheduleByPolicyId(int policyId)
        {
            int paymentPlanSchedule = 0;
            EP.EndorsementPayer endorsementPayer = null;
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(EP.EndorsementPayer.Properties.PolicyId, typeof(EP.EndorsementPayer).Name);
            filter.Equal();
            filter.Constant(policyId);
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                endorsementPayer = daf.List(typeof(EP.EndorsementPayer), filter.GetPredicate()).Cast<EP.EndorsementPayer>().FirstOrDefault();
            }

            if (endorsementPayer != null)
            {
                paymentPlanSchedule = endorsementPayer.PaymentScheduleId;
            }

            return paymentPlanSchedule;
        }

        /// <summary>
        /// Trae registro de la tabla PAYMENT_SCHEDULE por policyId
        /// </summary>
        /// <param name="policyId"></param>
        public int GetPaymentPlanScheduleByPolicyEndorsementId(int policyId, int endorsementId)
        {
            int paymentPlanSchedule = 0;
            EP.EndorsementPayer endorsementPayer = null;
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(EP.EndorsementPayer.Properties.PolicyId, typeof(EP.EndorsementPayer).Name);
            filter.Equal();
            filter.Constant(policyId);
            filter.And();
            filter.Property(EP.EndorsementPayer.Properties.EndorsementId, typeof(EP.EndorsementPayer).Name);
            filter.Equal();
            filter.Constant(endorsementId);
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                endorsementPayer = daf.List(typeof(EP.EndorsementPayer), filter.GetPredicate()).Cast<EP.EndorsementPayer>().FirstOrDefault();
            }

            if (endorsementPayer != null)
            {
                paymentPlanSchedule = endorsementPayer.PaymentScheduleId;
            }

            return paymentPlanSchedule;
        }
		
		public CompanyPolicy ValidateApplyPremiumFinance(CompanyPolicy companyPolicy , CompanyIssuanceInsured companyIssuanceInsured )
        {
            List<int> paymentPlanPremiumFinance = new List<int> { 38, 39, 40 }; //TODO: se debe es modificar la tabla de plan de pagos para que tenga una marca para saber si aplica o no financiamiento
            List<int> idsParameters = new List<int> { (int)Enums.Parameter.RateFinanceParameter, (int)Enums.Parameter.MaxPercentejeToFinanceParameter, (int)Enums.Parameter.PercentageToFinanceParameter };

            if (companyPolicy.PaymentPlan.PremiumFinance != null &&  paymentPlanPremiumFinance.Exists(u => u == companyPolicy.PaymentPlan.Id))
            {
                List<Parameter> paramaters = DelegateService.commonService.GetParametersByIds(idsParameters);
               
                Parameter rateFinanceParameter = paramaters.FirstOrDefault(u => u.Id == (int)Enums.Parameter.RateFinanceParameter);
                Parameter maxPercentejeToFinanceParameter = paramaters.FirstOrDefault(u => u.Id == (int)Enums.Parameter.MaxPercentejeToFinanceParameter);
                Parameter percentageToFinanceParameter = paramaters.FirstOrDefault(u => u.Id == (int)Enums.Parameter.PercentageToFinanceParameter);
                CompanyPremiumFinance companyPremiumFinance = new CompanyPremiumFinance
                {
                    CurrentFrom = companyPolicy.CurrentFrom,
                    CurrentTo = companyPolicy.CurrentTo,
                    FinanceRate = rateFinanceParameter != null ? rateFinanceParameter.AmountParameter.Value : 0,
                    PercentagetoFinance = percentageToFinanceParameter != null ? percentageToFinanceParameter.AmountParameter.Value : 0,
                    MinimumValueToFinance = 0,
                    StatePay = Enums.StatePay.Inicia,
                    PremiumValue = companyPolicy.Summary.Premium,
                    Insured = new CompanyInsuredPremiumFinance
                    {
                        IndividualId = companyIssuanceInsured.IndividualId,
                        FullName = companyIssuanceInsured.Name + companyIssuanceInsured.SecondSurname + companyIssuanceInsured.Surname,
                        InsuredCode = companyIssuanceInsured.InsuredId
                    }
                };
                companyPremiumFinance.FinanceValue = Math.Round((companyPolicy.Summary.Premium + companyPolicy.Summary.Taxes),2);
                companyPremiumFinance.FinanceToValue = Math.Round(companyPremiumFinance.FinanceValue * (percentageToFinanceParameter.AmountParameter.Value / 100), 2);
                companyPremiumFinance.NumberQuotas = companyPolicy.PaymentPlan.PremiumFinance.NumberQuotas;
                companyPolicy.PaymentPlan.PremiumFinance = companyPremiumFinance;

            }
            else
            {
                companyPolicy.PaymentPlan.PremiumFinance = null;
            }
            return companyPolicy;
        }

    }
}