//System
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;

//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.AccountingServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Enums;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Underwriting;
using Sistran.Core.Application.AccountingServices.EEProvider.Business;
using Sistran.Core.Application.Utilities.DataFacade;
using System.Threading.Tasks;
using TP = Sistran.Core.Application.Utilities.Utility;
using Sistran.Core.Framework.DAF.Engine.StoredProcedure;
using Sistran.Co.Application.Data;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs.Accounting
{
    public class TempApplicationPremiumDAO
    {
        /// <summary>
        /// GetTempPremiumRecievableTransactionByImputationId
        /// </summary>
        /// <param name="tempAppId"></param>
        /// <param name="moduleId"></param>
        /// <returns>ApplicationPremium</returns>
        public ApplicationPremiumTransaction GetTempApplicationPremiumByTempApplicationId(int tempAppId, int moduleId)
        {
            try
            {
                decimal debits = 0;
                decimal credits = 0;
                Amount amountCredit = new Amount();
                Amount amountDebit = new Amount();

                decimal excessPayment = 0;
                decimal upd = 0;

                ApplicationPremiumTransaction tempPremiumReceivableTransaction = new ApplicationPremiumTransaction();
                tempPremiumReceivableTransaction.Id = tempAppId;
                tempPremiumReceivableTransaction.PremiumReceivableItems = new List<ApplicationPremiumTransactionItem>();

                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplicationPremium.Properties.TempAppCode, tempAppId);

                List<ACCOUNTINGEN.TempApplicationPremium> entityTempApplicationPremiums = DataFacadeManager.GetObjects(
                                typeof(ACCOUNTINGEN.TempApplicationPremium), criteriaBuilder.GetPredicate()).
                                Cast<ACCOUNTINGEN.TempApplicationPremium>().ToList();

                if (entityTempApplicationPremiums != null && entityTempApplicationPremiums.Count > 0)
                {
                    ApplicationBusiness applicationBusiness = new ApplicationBusiness();
                    TempApplicationPremiumComponentDAO tempApplicationPremiumComponentDAO = new TempApplicationPremiumComponentDAO();
                    foreach (ACCOUNTINGEN.TempApplicationPremium tempPremiumReceivableEntity in entityTempApplicationPremiums)
                    {
                        var paymentComponents = applicationBusiness.GetPayerPaymentComponentsByEndorsementIdQuotaNumber(
                            tempPremiumReceivableEntity.EndorsementCode, tempPremiumReceivableEntity.PaymentNum);

                        var premiumPayment = tempApplicationPremiumComponentDAO.GetTempApplicationPremiumComponentsByTemAppPremium(tempPremiumReceivableEntity.TempAppPremiumCode);

                        decimal basePaymentAmount = paymentComponents.Where(x => x.TinyDescription == "P").Sum(x => x.Amount);
                        decimal basePaymentLocalAmount = basePaymentAmount * tempPremiumReceivableEntity.ExchangeRate;

                        decimal paymentAmount = premiumPayment.Where(x => x.ComponentTinyDescription == "P").Sum(x => x.Amount);
                        decimal paymentLocalAmount = premiumPayment.Where(x => x.ComponentTinyDescription == "P").Sum(x => x.LocalAmount);

                        excessPayment = paymentLocalAmount - basePaymentLocalAmount;

                        Policy policy = new Policy();
                        //policy.Id = Convert.ToInt32(tempPremiumReceivableEntity.PolicyId);

                        policy.Endorsement = new Endorsement()
                        {
                            Id = Convert.ToInt32(tempPremiumReceivableEntity.EndorsementCode)
                        };
                        //Pagador
                        policy.DefaultBeneficiaries = new List<Beneficiary>()
                        {
                            new Beneficiary()
                            {
                                CustomerType =  CustomerType.Individual,
                                IndividualId = Convert.ToInt32(tempPremiumReceivableEntity.PayerCode),
                            }
                        };
                        policy.ExchangeRate = new ExchangeRate()
                        {
                            BuyAmount = Convert.ToDecimal(tempPremiumReceivableEntity.ExchangeRate),
                            SellAmount = Convert.ToDecimal(tempPremiumReceivableEntity.ExchangeRate),
                            Currency = new Currency() { Id = Convert.ToInt32(tempPremiumReceivableEntity.CurrencyCode) }
                        };
                        policy.PayerComponents = new List<PayerComponent>()
                        {
                            new PayerComponent()
                            {
                                Amount = Convert.ToDecimal(tempPremiumReceivableEntity.Amount) * Convert.ToDecimal(tempPremiumReceivableEntity.ExchangeRate),
                                BaseAmount = Convert.ToDecimal(tempPremiumReceivableEntity.Amount)
                            }
                        };
                        policy.PaymentPlan = new PaymentPlan()
                        {
                            Quotas = new List<Quota>()
                            {
                                new Quota() { Number = Convert.ToInt32(tempPremiumReceivableEntity.PaymentNum) }
                            }
                        };

                        tempPremiumReceivableTransaction.PremiumReceivableItems.Add(new ApplicationPremiumTransactionItem()
                        {
                            Id = tempPremiumReceivableEntity.TempAppCode,
                            Policy = policy
                        });

                        #region PremiumReceivable
                        if (moduleId == Convert.ToInt32(ImputationItemTypes.PremiumsReceivable))
                        {
                            if (tempPremiumReceivableEntity.LocalAmount >= 0)
                            {
                                if (excessPayment > 0)
                                    credits += tempPremiumReceivableEntity.LocalAmount - excessPayment;
                                else
                                    credits += tempPremiumReceivableEntity.LocalAmount;
                            }
                            else
                            {
                                debits += tempPremiumReceivableEntity.LocalAmount;
                            }
                        }

                        #endregion

                        #region DepositPremiums

                        if (moduleId == Convert.ToInt32(ImputationItemTypes.DepositPremiums))
                        {
                            if (tempPremiumReceivableEntity.Amount >= 0)
                            {
                                if (excessPayment > 0)
                                {
                                    debits += Convert.ToDecimal(upd);            //el uso de primas en depósito va para el débito
                                    credits += Convert.ToDecimal(excessPayment); //el pago en exceso va al crédito
                                }
                                else
                                {
                                    debits += Convert.ToDecimal(upd);
                                }

                                upd = 0;
                            }
                            else
                            {
                                debits += 0;
                            }
                        }

                        #endregion

                        #region CommissionRetained

                        if (moduleId == Convert.ToInt32(ImputationItemTypes.CommissionRetained))
                        {
                            if (tempPremiumReceivableEntity.DiscountedCommission >= 0)
                                debits += Convert.ToDecimal(tempPremiumReceivableEntity.DiscountedCommission);
                            else
                                credits += Convert.ToDecimal(tempPremiumReceivableEntity.DiscountedCommission);
                        }
                        #endregion
                    }
                }

                amountCredit.Value = credits;
                amountDebit.Value = debits;

                tempPremiumReceivableTransaction.TotalCredit = amountCredit;
                tempPremiumReceivableTransaction.TotalDebit = amountDebit;

                return tempPremiumReceivableTransaction;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #region Public Methods
        /// <summary>
        /// GetTempPaymentRequestTransactions
        /// Valida si ya existe una determinada solicitud de pago en temporales o en reales
        /// basado en la solicitud de pago , el número de cuota, número de imputación 
        /// </summary>
        /// <param name="tempApplicationPremiumId"></param>
        /// <param name="tempAppId"></param>
        /// <returns>List<TempClaimPayment/></returns>
        public List<ACCOUNTINGEN.TempApplicationPremium> GetTempApplicationPremiums(int tempApplicationPremiumId, int tempAppId)
        {
            try
            {
                // Busca primero en temporales
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                if (tempAppId > 0)
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplicationPremium.Properties.TempAppCode, tempAppId);
                }
                else
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplicationPremium.Properties.TempAppPremiumCode, tempApplicationPremiumId);
                }

                List<ACCOUNTINGEN.TempApplicationPremium> entityTempApplicationPremiums = DataFacadeManager.GetObjects(
                                typeof(ACCOUNTINGEN.TempApplicationPremium), criteriaBuilder.GetPredicate()).
                                Cast<ACCOUNTINGEN.TempApplicationPremium>().ToList();

                // Encontrado en temporales 
                return entityTempApplicationPremiums;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tempApplicationPremiumId"></param>
        /// <returns></returns>
        public bool DeleteTempApplicationPremium(int tempApplicationPremiumId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.TempApplicationPremium.CreatePrimaryKey(tempApplicationPremiumId);
                DataFacadeManager.Delete(primaryKey);
                return true;
            }
            catch (BusinessException)
            {
                return false;
            }
        }


        /// <summary>
        /// GetTempPaymentRequestTransactionByTempImputationId
        /// </summary>
        /// <param name="tempApplicationId"></param>
        /// <returns>PaymentRequestTransaction</returns>
        public PaymentRequestTransaction GetTempApplicationPremiumByTempApplicationId(int tempApplicationId)
        {
            decimal debits = 0;
            decimal credits = 0;

            PaymentRequestTransaction tempPaymentRequestTransaction = new PaymentRequestTransaction();
            tempPaymentRequestTransaction.Id = tempApplicationId;

            List<ACCOUNTINGEN.TempApplicationPremium> entityTempApplicationPremium = GetTempApplicationPremiums(0, tempApplicationId);

            foreach (ACCOUNTINGEN.TempApplicationPremium tempPaymentRequestEntity in entityTempApplicationPremium)
            {
                if (tempPaymentRequestEntity.Amount < 0)  // Si es negativo es crédito
                {
                    credits = credits + (Convert.ToDecimal(tempPaymentRequestEntity.Amount) * -1);
                }
                else
                {
                    debits = debits + Convert.ToDecimal(tempPaymentRequestEntity.Amount);
                }
            }

            tempPaymentRequestTransaction.TotalCredit = new Amount();
            tempPaymentRequestTransaction.TotalCredit.Value = credits;
            tempPaymentRequestTransaction.TotalDebit = new Amount();
            tempPaymentRequestTransaction.TotalDebit.Value = debits;

            return tempPaymentRequestTransaction;
        }

        public ApplicationPremium GetTempApplicationPremiumByTempApplicationPremiumId(int tempApplicationPremiumId)
        {


            // Crea la Primary key con el código de la entidad
            PrimaryKey primaryKey = ACCOUNTINGEN.TempApplicationPremium.CreatePrimaryKey(tempApplicationPremiumId);


            // Realizar las operaciones con los entities utilizando DAF
            ACCOUNTINGEN.TempApplicationPremium entityApplicationPremium = (ACCOUNTINGEN.TempApplicationPremium)
                (DataFacadeManager.GetObject(primaryKey));


            return ModelAssembler.CreateTempApplicationPremium(entityApplicationPremium);
        }

        /// <summary>
        /// GetTempPaymentRequestTrans
        /// </summary>
        /// <param name="tempApplicationPremiumId"></param>
        /// <param name="tempApplicationId"></param>
        /// <returns></returns>
        public List<ACCOUNTINGEN.TempApplicationPremium> GetTempApplicationPremiumsByApplicationId(int tempApplicationPremiumId, int tempApplicationId)
        {
            try
            {
                List<ACCOUNTINGEN.TempApplicationPremium> entityTempApplicationPremiums = new List<ACCOUNTINGEN.TempApplicationPremium>();

                // Busca primero en temporales
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                if (tempApplicationId > 0)
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplicationPremium.Properties.TempAppCode, tempApplicationId);

                    if (tempApplicationPremiumId > 0)
                    {
                        criteriaBuilder.And();
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempClaimPaymentReqTrans.Properties.PaymentRequestCode, tempApplicationPremiumId);
                    }

                    entityTempApplicationPremiums = DataFacadeManager.GetObjects(
                                typeof(ACCOUNTINGEN.TempApplicationPremium), criteriaBuilder.GetPredicate()).
                                Cast<ACCOUNTINGEN.TempApplicationPremium>().ToList();
                }

                // Encontrado en temporales 
                return entityTempApplicationPremiums;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// Get temp application premiums
        /// </summary>
        /// <param name="tempApplicationId">Temp Application Id</param>
        /// <returns>Application premium list</returns>
        public List<ACCOUNTINGEN.TempApplicationPremium> GetTempApplicationPremiumsByTempApplicationId(int tempApplicationId)
        {
            try
            {
                List<ACCOUNTINGEN.TempApplicationPremium> entityTempApplicationPremiums = new List<ACCOUNTINGEN.TempApplicationPremium>();

                // Busca primero en temporales
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                if (tempApplicationId > 0)
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplicationPremium.Properties.TempAppCode, tempApplicationId);

                    entityTempApplicationPremiums = DataFacadeManager.GetObjects(
                                typeof(ACCOUNTINGEN.TempApplicationPremium), criteriaBuilder.GetPredicate()).
                                Cast<ACCOUNTINGEN.TempApplicationPremium>().ToList();
                }

                // Encontrado en temporales 
                return entityTempApplicationPremiums;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// SavePaymentRequestTransaction
        /// Graba pago de siniestro
        /// </summary>
        /// <param name="applicationPremium"></param>
        /// <param name="applicationId"></param>
        /// <param name="exchangeRate"></param>
        /// <returns>bool</returns>
        public bool SaveApplicationPremium(ApplicationPremium applicationPremium)
        {
            try
            {
                ACCOUNTINGEN.ApplicationPremium entityApplicationPremium = EntityAssembler.CreateApplicationPremium(
                                                     applicationPremium);
                DataFacadeManager.Insert(entityApplicationPremium);
                return true;
            }
            catch (BusinessException)
            {
                return false;
            }
        }

        /// <summary>
        /// DeletePaymentRequestTransactionByImputationId
        /// Borra una solicitud de pagos varios por imputación
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns>bool</returns>
        public bool DeleteApplicationPremiumByApplicationId(int applicationId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ApplicationPremium.Properties.AppCode, applicationId);

                List<ACCOUNTINGEN.ApplicationPremium> entityApplicationPremiums = DataFacadeManager.GetObjects(
                                typeof(ACCOUNTINGEN.ApplicationPremium), criteriaBuilder.GetPredicate()).
                                Cast<ACCOUNTINGEN.ApplicationPremium>().ToList();

                if (entityApplicationPremiums != null && entityApplicationPremiums.Count > 0)
                {
                    foreach (ACCOUNTINGEN.ApplicationPremium applicationPremium in entityApplicationPremiums)
                    {
                        DataFacadeManager.Delete(applicationPremium.PrimaryKey);
                    }
                }
                return true;
            }
            catch (BusinessException)
            {
                return false;
            }
        }

        /// <summary>
        /// DeleteTempPaymentRequestByTempImputationId
        /// Borra registros de la solicitud de pagos varios por la imputacion
        /// </summary>
        /// <param name="tempApplicationId"></param>
        /// <returns>bool</returns>
        public bool DeleteTempApplicationPremiumByTempApplication(int tempApplicationId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplicationPremium.Properties.TempAppCode, tempApplicationId);

                List<ACCOUNTINGEN.TempApplicationPremium> entityTempApplicationPremium = DataFacadeManager.GetObjects(
                                typeof(ACCOUNTINGEN.TempApplicationPremium), criteriaBuilder.GetPredicate()).
                                Cast<ACCOUNTINGEN.TempApplicationPremium>().ToList();

                if (entityTempApplicationPremium != null && entityTempApplicationPremium.Count > 0)
                {
                    foreach (ACCOUNTINGEN.TempApplicationPremium tempApplicationPremium in entityTempApplicationPremium)
                    {
                        criteriaBuilder = new ObjectCriteriaBuilder();
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplicationPremiumCommiss.Properties.TempAppPremiumId, tempApplicationPremium.TempAppPremiumCode);

                        DataFacadeManager.Instance.GetDataFacade().Delete<ACCOUNTINGEN.TempApplicationPremiumCommiss>(criteriaBuilder.GetPredicate());

                        criteriaBuilder = new ObjectCriteriaBuilder();
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempUsedDepositPremium.Properties.TempPremiumReceivableTransCode, tempApplicationPremium.TempAppPremiumCode);

                        DataFacadeManager.Instance.GetDataFacade().Delete<ACCOUNTINGEN.TempUsedDepositPremium>(criteriaBuilder.GetPredicate());

                        criteriaBuilder = new ObjectCriteriaBuilder();
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplicationPremiumComponent.Properties.TempAppPremiumCode, tempApplicationPremium.TempAppPremiumCode);

                        DataFacadeManager.Instance.GetDataFacade().Delete<ACCOUNTINGEN.TempApplicationPremiumComponent>(criteriaBuilder.GetPredicate());

                        DataFacadeManager.Delete(tempApplicationPremium.PrimaryKey);
                        /*List<ACCOUNTINGEN.TempApplicationPremiumCommiss> entityTempApplicationPremiumCommiss = DataFacadeManager.GetObjects(
                                typeof(ACCOUNTINGEN.TempApplicationPremiumCommiss), criteriaBuilder.GetPredicate()).
                                Cast<ACCOUNTINGEN.TempApplicationPremiumCommiss>().ToList();

                        if (entityTempApplicationPremiumCommiss != null && entityTempApplicationPremiumCommiss.Count > 0)
                        {
                            foreach (ACCOUNTINGEN.TempApplicationPremiumCommiss entityCommiss in entityTempApplicationPremiumCommiss)
                            {
                                DataFacadeManager.Delete(entityCommiss.PrimaryKey);
                            }
                        }
                        DataFacadeManager.Delete(tempApplicationPremium.PrimaryKey);*/
                    }
                }
                return true;
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        public void UpdateTempApplicationPremiumAmounts(int tempApplicationPremiumId, decimal amount, decimal localAmount, decimal mainAmount, decimal mainLocalAmount)
        {
            // Convertir de model a entity
            var primaryKey = ACCOUNTINGEN.TempApplicationPremium.CreatePrimaryKey(tempApplicationPremiumId);
            var entityApplicationPremium = (ACCOUNTINGEN.TempApplicationPremium)DataFacadeManager.GetObject(primaryKey);

            entityApplicationPremium.Amount = amount;
            entityApplicationPremium.LocalAmount = localAmount;
            entityApplicationPremium.MainAmount = mainAmount;
            entityApplicationPremium.MainLocalAmount = mainLocalAmount;

            // Realizar las operaciones con los entities utilizando DAF
            DataFacadeManager.Update(entityApplicationPremium);
        }

        public void UpdateCommissonTempApplicationPremium(int tempApplicationPremiumId, decimal commissionAmount)
        {
            // Convertir de model a entity
            var primaryKey = ACCOUNTINGEN.TempApplicationPremium.CreatePrimaryKey(tempApplicationPremiumId);
            var entityApplicationPremium = (ACCOUNTINGEN.TempApplicationPremium)DataFacadeManager.GetObject(primaryKey);

            entityApplicationPremium.DiscountedCommission = commissionAmount;
            // Realizar las operaciones con los entities utilizando DAF
            DataFacadeManager.Update(entityApplicationPremium);
        }

        public List<TempApplicationPremiumComponent> GetPayerPaymentComponentsByEndorsementIdQuotaNumber(int endorsementId, int quotaNumber)
        {
            List<TempApplicationPremiumComponent> tempApplicationPremiumComponents = new List<TempApplicationPremiumComponent>();
            DataTable resultPaymentTable;
            NameValue[] parameters = new NameValue[3];
            parameters[0] = new NameValue("@ENDORSEMENT_CD", endorsementId);
            parameters[1] = new NameValue("@PAYMENT_NUM", quotaNumber);
            parameters[2] = new NameValue("@IS_REV", 0);

            // 2021/11/13 Jhon Gómez, se llama por la librería dataAcces para evitar 
            //   error con la transacción que llama este proceso
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                resultPaymentTable = dynamicDataAccess.ExecuteSPDataTable("[ACC].[GET_PAYMENT_COMPONENTS]", parameters);
            }

            if (resultPaymentTable != null && resultPaymentTable.Rows != null)
            {
                if (resultPaymentTable.Rows.Count > 0)
                {
                    foreach (DataRow row in resultPaymentTable.Rows)
                    {
                        tempApplicationPremiumComponents.Add(new TempApplicationPremiumComponent()
                        {
                            Amount = Convert.ToDecimal(row["Amount"].ToString()),
                            LocalAmount = Convert.ToDecimal(row["LocalAmount"].ToString()),
                            ExchangeRate = Convert.ToDecimal(row["ExchangeRate"].ToString()),
                            ComponentCurrencyCode = Convert.ToInt32(row["CurrencyCode"].ToString()),
                            ComponentCode = Convert.ToInt32(row["ComponentCode"].ToString()),
                            ComponentTinyDescription = row["TinyDescription"].ToString()
                        });
                    }
                }
            }
            return tempApplicationPremiumComponents;




            /*try
            {*/
            /*   Param[] parameters = new Param[3];
               parameters[0] = new Param("@ENDORSEMENT_CD", endorsementId);
               parameters[1] = new Param("@PAYMENT_NUM", quotaNumber);
               parameters[2] = new Param("@IS_REV", 0);

               var result = DataFacadeManager.Instance.GetDataFacade().ExecuteSPReader("[ACC].[GET_PAYMENT_COMPONENTS]", parameters);
               if (result.ToArray().Any())
               {
                   foreach (object[] item in result.ToArray())
                   {
                       tempApplicationPremiumComponents.Add(new TempApplicationPremiumComponent()
                       {
                           Amount = Convert.ToDecimal(item[0].ToString()),
                           LocalAmount = Convert.ToDecimal(item[1].ToString()),
                           ExchangeRate = Convert.ToDecimal(item[2].ToString()),
                           ComponentCurrencyCode = Convert.ToInt32(item[3]),
                           ComponentCode = Convert.ToInt32(item[4]),
                           ComponentTinyDescription = item[5].ToString()
                       });
                   }
               }
               return tempApplicationPremiumComponents;*/
            /* }
             catch (System.InvalidOperationException ex)
             {
                 ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                 criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ApplicationPremium.Properties.IsRev, false);
                 criteriaBuilder.And();
                 criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ApplicationPremium.Properties.EndorsementCode, endorsementId);
                 criteriaBuilder.And();
                 criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ApplicationPremium.Properties.PaymentNum, quotaNumber);


                 Join join = new Join(new ClassNameTable(typeof(ACCOUNTINGEN.ApplicationPremiumComponent), "APC"), new ClassNameTable(typeof(ACCOUNTINGEN.ApplicationPremium), "AP"), JoinType.Inner);
                 join.Criteria = (new ObjectCriteriaBuilder()
                     .Property(ACCOUNTINGEN.ApplicationPremium.Properties.AppPremiumCode, "AP")
                     .Equal()
                     .Property(ACCOUNTINGEN.ApplicationPremiumComponent.Properties.AppPremiumCode, "APC")
                     .GetPredicate());

                 join = new Join(join, new ClassNameTable(typeof(Parameters.Entities.Component), "C"), JoinType.Inner);
                 join.Criteria = (new ObjectCriteriaBuilder()
                     .Property(ACCOUNTINGEN.ApplicationPremiumComponent.Properties.ComponentCode, "APC")
                     .Equal()
                     .Property(Parameters.Entities.Component.Properties.ComponentCode, "C")
                     .GetPredicate());

                 join = new Join(join, new ClassNameTable(typeof(Parameters.Entities.ComponentType), "CT"), JoinType.Inner);
                 join.Criteria = (new ObjectCriteriaBuilder()
                     .Property(Parameters.Entities.Component.Properties.ComponentTypeCode, "C")
                     .Equal()
                     .Property(Parameters.Entities.ComponentType.Properties.ComponentTypeCode, "CT")
                     .GetPredicate());

                 SelectQuery selectQuery = new SelectQuery();
                 selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.ApplicationPremiumComponent.Properties.Amount, "APC"), "Amount"));
                 selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.ApplicationPremiumComponent.Properties.ComponentCode, "APC"), "ComponentCode"));
                 selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.ApplicationPremiumComponent.Properties.CurrencyCode, "APC"), "CurrencyCode"));
                 selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.ApplicationPremiumComponent.Properties.ExchangeRate, "APC"), "ExchangeRate"));
                 selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.ApplicationPremiumComponent.Properties.LocalAmount, "APC"), "LocalAmount"));
                 selectQuery.AddSelectValue(new SelectValue(new Column(Parameters.Entities.ComponentType.Properties.TinyDescription, "CT"), "TinyDescription"));

                 selectQuery.Table = join;
                 selectQuery.Where = criteriaBuilder.GetPredicate();
                 using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
                 {
                     while (reader.Read())
                     {
                         tempApplicationPremiumComponents.Add(new TempApplicationPremiumComponent()
                         {
                             Amount = Convert.ToDecimal(reader["Amount"].ToString()),
                             LocalAmount = Convert.ToDecimal(reader["LocalAmount"].ToString()),
                             ExchangeRate = Convert.ToDecimal(reader["ExchangeRate"].ToString()),
                             ComponentCurrencyCode = Convert.ToInt32(reader["CurrencyCode"].ToString()),
                             ComponentCode = Convert.ToInt32(reader["ComponentCode"].ToString()),
                             ComponentTinyDescription = reader["TinyDescription"].ToString()
                         });
                     }
                 }
                 return tempApplicationPremiumComponents;
             //}

             if (result.ToArray().Any())
             {
                 var item = (result[0] as object[]);

                 decimal.TryParse(item[2].ToString(), out decimal cumuloIndividual);
                 decimal.TryParse(item[6].ToString(), out decimal cumuloGrupoEconomico);

                 return cumuloIndividual + cumuloGrupoEconomico;
             }


             try
             {
                 //var result = await TP.Task.Run(() =>
                 //{

                     ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                     criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ApplicationPremium.Properties.IsRev, false);
                     criteriaBuilder.And();
                     criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ApplicationPremium.Properties.EndorsementCode, endorsementId);
                     criteriaBuilder.And();
                     criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ApplicationPremium.Properties.PaymentNum, quotaNumber);


                     Join join = new Join(new ClassNameTable(typeof(ACCOUNTINGEN.ApplicationPremiumComponent), "APC"), new ClassNameTable(typeof(ACCOUNTINGEN.ApplicationPremium), "AP"), JoinType.Inner);
                     join.Criteria = (new ObjectCriteriaBuilder()
                         .Property(ACCOUNTINGEN.ApplicationPremium.Properties.AppPremiumCode, "AP")
                         .Equal()
                         .Property(ACCOUNTINGEN.ApplicationPremiumComponent.Properties.AppPremiumCode, "APC")
                         .GetPredicate());

                     join = new Join(join, new ClassNameTable(typeof(Parameters.Entities.Component), "C"), JoinType.Inner);
                     join.Criteria = (new ObjectCriteriaBuilder()
                         .Property(Parameters.Entities.Component.Properties.ComponentCode, "C")
                         .Equal()
                         .Property(ACCOUNTINGEN.ApplicationPremiumComponent.Properties.ComponentCode, "APC")
                         .GetPredicate());

                     join = new Join(join, new ClassNameTable(typeof(Parameters.Entities.ComponentType), "CT"), JoinType.Inner);
                     join.Criteria = (new ObjectCriteriaBuilder()
                         .Property(Parameters.Entities.Component.Properties.ComponentTypeCode, "C")
                         .Equal()
                         .Property(Parameters.Entities.ComponentType.Properties.ComponentTypeCode, "CT")
                         .GetPredicate());

                     SelectQuery selectQuery = new SelectQuery();
                     selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.ApplicationPremiumComponent.Properties.Amount, "APC"), "Amount"));
                     selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.ApplicationPremiumComponent.Properties.ComponentCode, "APC"), "ComponentCode"));
                     selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.ApplicationPremiumComponent.Properties.CurrencyCode, "APC"), "CurrencyCode"));
                     selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.ApplicationPremiumComponent.Properties.ExchangeRate, "APC"), "ExchangeRate"));
                     selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.ApplicationPremiumComponent.Properties.LocalAmount, "APC"), "LocalAmount"));
                     selectQuery.AddSelectValue(new SelectValue(new Column(Parameters.Entities.ComponentType.Properties.TinyDescription, "CT"), "TinyDescription"));
                 DataFacadeManager.Instance.GetDataFacade().ExecuteSPReader
                     selectQuery.Table = join;
                     selectQuery.Where = criteriaBuilder.GetPredicate();
                     List<TempApplicationPremiumComponent> tempApplicationPremiumComponents = new List<TempApplicationPremiumComponent>();
                     using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
                     {
                         while (reader.Read())
                         {
                             tempApplicationPremiumComponents.Add(new TempApplicationPremiumComponent()
                             {
                                 Amount = Convert.ToDecimal(reader["Amount"].ToString()),
                                 LocalAmount = Convert.ToDecimal(reader["LocalAmount"].ToString()),
                                 ExchangeRate = Convert.ToDecimal(reader["ExchangeRate"].ToString()),
                                 ComponentCurrencyCode = Convert.ToInt32(reader["CurrencyCode"].ToString()),
                                 ComponentCode = Convert.ToInt32(reader["ComponentCode"].ToString()),
                                 ComponentTinyDescription = reader["TinyDescription"].ToString()
                             });
                         }
                     }
                     return tempApplicationPremiumComponents;
                 //});
                 //return result;
             }
             catch (Exception ex)
             {
                 throw new BusinessException(ex.Message);
             }*/
        }

        public List<ApplicationPremiumComponentLBSB> GetPayerPaymentComponentsLSBSByEndorsementIdQuotaNumbers(int endorsementId, int quotaNumber)
        {
            List<ApplicationPremiumComponentLBSB> applicationPremiumComponentsLBSB = new List<ApplicationPremiumComponentLBSB>();
            DataTable resultPaymentTable;
            NameValue[] parameters = new NameValue[3];
            parameters[0] = new NameValue("@ENDORSEMENT_CD", endorsementId);
            parameters[1] = new NameValue("@PAYMENT_NUM", quotaNumber);
            parameters[2] = new NameValue("@IS_REV", 0);

            // 2021/11/13 Jhon Gómez, se llama por la librería dataAcces para evitar 
            //   error con la transacción que llama este proceso
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                resultPaymentTable = dynamicDataAccess.ExecuteSPDataTable("[ACC].[GET_PAYMENT_COMPONENTS_LSBS]", parameters);
            }

            if (resultPaymentTable != null && resultPaymentTable.Rows != null)
            {
                if (resultPaymentTable.Rows.Count > 0)
                {
                    foreach (DataRow row in resultPaymentTable.Rows)
                    {
                        applicationPremiumComponentsLBSB.Add(new ApplicationPremiumComponentLBSB()
                        {
                            Amount = Convert.ToDecimal(row["Amount"].ToString()),
                            LocalAmount = Convert.ToDecimal(row["LocalAmount"].ToString()),
                            ApplicationComponentId = Convert.ToInt32(row["AppComponentCode"].ToString()),
                            MainAmount = Convert.ToDecimal(row["MainAmount"].ToString()),
                            MainLocalAmount = Convert.ToDecimal(row["MainLocalAmount"].ToString()),
                            LineBussinesId = Convert.ToInt32(row["LineBusinessCode"].ToString()),
                            SubLineBussinesId = Convert.ToInt32(row["SubLineBusinessCode"].ToString())
                        });
                    }
                }
            }
            return applicationPremiumComponentsLBSB;
            /*
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ApplicationPremium.Properties.IsRev, false);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ApplicationPremium.Properties.EndorsementCode, endorsementId);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ApplicationPremium.Properties.PaymentNum, quotaNumber);


            Join join = new Join(new ClassNameTable(typeof(ACCOUNTINGEN.ApplicationPremiumComponent), "APC"), new ClassNameTable(typeof(ACCOUNTINGEN.ApplicationPremium), "AP"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ACCOUNTINGEN.ApplicationPremium.Properties.AppPremiumCode, "AP")
                .Equal()
                .Property(ACCOUNTINGEN.ApplicationPremiumComponent.Properties.AppPremiumCode, "APC")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(ACCOUNTINGEN.ApplicationPremiumComponentLbsb), "APCLBSB"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ACCOUNTINGEN.ApplicationPremiumComponentLbsb.Properties.AppComponentCode, "APCLBSB")
                .Equal()
                .Property(ACCOUNTINGEN.ApplicationPremiumComponent.Properties.AppPremiumCode, "APC")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(Parameters.Entities.Component), "C"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(Parameters.Entities.Component.Properties.ComponentCode, "C")
                .Equal()
                .Property(ACCOUNTINGEN.ApplicationPremiumComponent.Properties.ComponentCode, "APC")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(Parameters.Entities.ComponentType), "CT"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(Parameters.Entities.Component.Properties.ComponentTypeCode, "C")
                .Equal()
                .Property(Parameters.Entities.ComponentType.Properties.ComponentTypeCode, "CT")
                .GetPredicate());

            SelectQuery selectQuery = new SelectQuery();
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.ApplicationPremiumComponentLbsb.Properties.AppComponentCode, "APC"), "AppComponentCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.ApplicationPremiumComponentLbsb.Properties.Amount, "APCLBSB"), "Amount"));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.ApplicationPremiumComponentLbsb.Properties.MainAmount, "APCLBSB"), "MainAmount"));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.ApplicationPremiumComponentLbsb.Properties.MainLocalAmount, "APCLBSB"), "MainLocalAmount"));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.ApplicationPremiumComponentLbsb.Properties.LocalAmount, "APCLBSB"), "LocalAmount"));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.ApplicationPremiumComponentLbsb.Properties.LineBusinessCode, "APCLBSB"), "LineBusinessCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.ApplicationPremiumComponentLbsb.Properties.SubLineBusinessCode, "APCLBSB"), "SubLineBusinessCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(Parameters.Entities.ComponentType.Properties.TinyDescription, "CT"), "TinyDescription"));

            selectQuery.Table = join;
            selectQuery.Where = criteriaBuilder.GetPredicate();

            List<ApplicationPremiumComponentLBSB> applicationPremiumComponentsLBSB = new List<ApplicationPremiumComponentLBSB>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    applicationPremiumComponentsLBSB.Add(new ApplicationPremiumComponentLBSB()
                    {
                        Amount = Convert.ToDecimal(reader["Amount"].ToString()),
                        LocalAmount = Convert.ToDecimal(reader["LocalAmount"].ToString()),
                        ApplicationComponentId = Convert.ToInt32(reader["AppComponentCode"].ToString()),
                        MainAmount = Convert.ToDecimal(reader["MainAmount"].ToString()),
                        MainLocalAmount = Convert.ToDecimal(reader["MainLocalAmount"].ToString()),
                        LineBussinesId = Convert.ToInt32(reader["LineBusinessCode"].ToString()),
                        SubLineBussinesId = Convert.ToInt32(reader["SubLineBusinessCode"].ToString())
                    });
                }
            }
            return applicationPremiumComponentsLBSB;*/
        }

        public Models.Imputations.Application GetTemporalApplicationByEndorsementIdPaymentNumber(int endorsementId, int paymentNumber)
        {
            Models.Imputations.Application application = new Models.Imputations.Application();

            // Realizo un select para ver si la póliza se encuentra en temporales.
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplicationPremium.Properties.EndorsementCode, endorsementId);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplicationPremium.Properties.PaymentNum, paymentNumber);

            List<ACCOUNTINGEN.TempApplicationPremium> entityTempApplicationPremiums = DataFacadeManager.GetObjects(
                                typeof(ACCOUNTINGEN.TempApplicationPremium), criteriaBuilder.GetPredicate()).
                                Cast<ACCOUNTINGEN.TempApplicationPremium>().ToList();

            if (entityTempApplicationPremiums.Count > 0) // Se encuentra en temporales
            {
                var entityTmpApplicationPremium = entityTempApplicationPremiums.FirstOrDefault();
                application = new TempApplicationDAO().GetTempApplicationByTempApplicationId(entityTmpApplicationPremium.TempAppCode);
            }
            return application;
        }

        public List<ApplicationPremium> GetTemporalApplicationPremiumsByEndorsementIdPaymentNumber(int endorsementId, int paymentNumber)
        {
            Models.Imputations.Application application = new Models.Imputations.Application();

            // Realizo un select para ver si la póliza se encuentra en temporales.
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplicationPremium.Properties.EndorsementCode, endorsementId);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplicationPremium.Properties.PaymentNum, paymentNumber);

            List<ACCOUNTINGEN.TempApplicationPremium> entityTempApplicationPremiums = DataFacadeManager.GetObjects(
                                typeof(ACCOUNTINGEN.TempApplicationPremium), criteriaBuilder.GetPredicate()).
                                Cast<ACCOUNTINGEN.TempApplicationPremium>().ToList();

            return ModelAssembler.CreateTempApplicationPremiums(entityTempApplicationPremiums);
        }

        public ApplicationPremium GetTemporalApplicationPremiumByEndorsementIdPaymentNumber(int endorsementId, int paymentNumber, int tempApplicationId)
        {
            // Realizo un select para ver si la póliza se encuentra en temporales.
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplicationPremium.Properties.EndorsementCode, endorsementId);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplicationPremium.Properties.PaymentNum, paymentNumber);
            criteriaBuilder.And();
            criteriaBuilder.Property(ACCOUNTINGEN.TempApplicationPremium.Properties.TempAppCode);
            criteriaBuilder.Distinct();
            criteriaBuilder.Constant(tempApplicationId);

            List<ACCOUNTINGEN.TempApplicationPremium> entityTempApplicationPremiums = DataFacadeManager.GetObjects(
                                typeof(ACCOUNTINGEN.TempApplicationPremium), criteriaBuilder.GetPredicate()).
                                Cast<ACCOUNTINGEN.TempApplicationPremium>().ToList();

            if (entityTempApplicationPremiums != null && entityTempApplicationPremiums.Count > 0)
            {
                return ModelAssembler.CreateTempApplicationPremium(entityTempApplicationPremiums.FirstOrDefault());
            }
            return null;
        }

        public void UpdateTempApplicationPremiumAmounts(TempApplicationPremium tempApplicationPremium)
        {
            // Convertir de model a entity
            var primaryKey = ACCOUNTINGEN.TempApplicationPremium.CreatePrimaryKey(tempApplicationPremium.Id);
            var entityApplicationPremium = (ACCOUNTINGEN.TempApplicationPremium)DataFacadeManager.GetObject(primaryKey);

            entityApplicationPremium.ExchangeRate = tempApplicationPremium.ExchangeRate;
            entityApplicationPremium.Amount = tempApplicationPremium.Amount;
            entityApplicationPremium.LocalAmount = tempApplicationPremium.LocalAmount;
            entityApplicationPremium.MainAmount = tempApplicationPremium.MainAmount;
            entityApplicationPremium.MainLocalAmount = tempApplicationPremium.MainLocalAmount;

            // Realizar las operaciones con los entities utilizando DAF
            DataFacadeManager.Update(entityApplicationPremium);
        }
        #endregion
    }
}
