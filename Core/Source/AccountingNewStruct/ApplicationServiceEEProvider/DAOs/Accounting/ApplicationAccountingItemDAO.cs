//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using System;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs.Accounting
{
    public class ApplicationAccountingItemDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        ///<summary>
        /// SaveDailyAccountingTransactionItem
        /// </summary>
        /// <param name="applicationAccounting"></param>
        /// <param name="imputationId"></param>
        /// <param name="paymentConceptCode"></param>
        /// <param name="description"></param>
        /// <param name="bankReconciliationId"></param>
        /// <param name="receiptNumber"></param>
        /// <param name="receiptDate"></param>
        /// <returns>int</returns>
        /// 
        public int SaveApplicationAccounting(ApplicationAccounting applicationAccounting, int imputationId,
                                                      int paymentConceptCode, string description, int bankReconciliationId,
                                                      int receiptNumber, DateTime? receiptDate)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.ApplicationAccounting entityApplicationAccounting =
                    EntityAssembler.CreateApplicationAccounting(applicationAccounting, imputationId,
                                                          paymentConceptCode, description, bankReconciliationId,
                                                          receiptNumber, receiptDate);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(entityApplicationAccounting);

                // Return del model
                return entityApplicationAccounting.AppAccountingCode;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteDailyAccountingTransactionItem
        /// </summary>
        /// <param name="appAccountingCode"></param>
        /// <returns>bool</returns>
        public bool DeleteApplicationAccounting(int appAccountingCode)
        {
            bool isDeleted = false;

            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.ApplicationAccounting.CreatePrimaryKey(appAccountingCode);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.ApplicationAccounting entityApplicationAccounting = (ACCOUNTINGEN.ApplicationAccounting)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(entityApplicationAccounting);
                isDeleted = true;
            }
            catch (BusinessException)
            {
                isDeleted = false;
            }
            return isDeleted;
        }


        public List<ApplicationAccounting> GetApplicationAccountsByApplicationId(int applicationId)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ApplicationAccounting.Properties.AppCode, applicationId);
            SelectQuery selectQuery = new SelectQuery();
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.ApplicationAccounting.Properties.AppCode, typeof(ACCOUNTINGEN.ApplicationAccounting).Name), ACCOUNTINGEN.ApplicationAccounting.Properties.AppCode));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.ApplicationAccounting.Properties.AppAccountingCode, typeof(ACCOUNTINGEN.ApplicationAccounting).Name), ACCOUNTINGEN.ApplicationAccounting.Properties.AppAccountingCode));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.ApplicationAccounting.Properties.CurrencyCode, typeof(ACCOUNTINGEN.ApplicationAccounting).Name), ACCOUNTINGEN.ApplicationAccounting.Properties.CurrencyCode));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.ApplicationAccounting.Properties.ExchangeRate, typeof(ACCOUNTINGEN.ApplicationAccounting).Name), ACCOUNTINGEN.ApplicationAccounting.Properties.ExchangeRate));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.ApplicationAccounting.Properties.AccountingConceptCode, typeof(ACCOUNTINGEN.ApplicationAccounting).Name), ACCOUNTINGEN.ApplicationAccounting.Properties.AccountingConceptCode));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.ApplicationAccounting.Properties.AccountingAccountCode, typeof(ACCOUNTINGEN.ApplicationAccounting).Name), ACCOUNTINGEN.ApplicationAccounting.Properties.AccountingAccountCode));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.ApplicationAccounting.Properties.AccountingNature, typeof(ACCOUNTINGEN.ApplicationAccounting).Name), ACCOUNTINGEN.ApplicationAccounting.Properties.AccountingNature));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.ApplicationAccounting.Properties.Description, typeof(ACCOUNTINGEN.ApplicationAccounting).Name), ACCOUNTINGEN.ApplicationAccounting.Properties.Description));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.ApplicationAccounting.Properties.Amount, typeof(ACCOUNTINGEN.ApplicationAccounting).Name), ACCOUNTINGEN.ApplicationAccounting.Properties.Amount));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.ApplicationAccounting.Properties.LocalAmount, typeof(ACCOUNTINGEN.ApplicationAccounting).Name), ACCOUNTINGEN.ApplicationAccounting.Properties.LocalAmount));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.ApplicationAccounting.Properties.BranchCode, typeof(ACCOUNTINGEN.ApplicationAccounting).Name), ACCOUNTINGEN.ApplicationAccounting.Properties.BranchCode));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.ApplicationAccounting.Properties.SalePointCode, typeof(ACCOUNTINGEN.ApplicationAccounting).Name), ACCOUNTINGEN.ApplicationAccounting.Properties.SalePointCode));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.ApplicationAccounting.Properties.IndividualCode, typeof(ACCOUNTINGEN.ApplicationAccounting).Name), ACCOUNTINGEN.ApplicationAccounting.Properties.IndividualCode));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.ApplicationAccounting.Properties.BankReconciliationCode, typeof(ACCOUNTINGEN.ApplicationAccounting).Name), ACCOUNTINGEN.ApplicationAccounting.Properties.BankReconciliationCode));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.ApplicationAccounting.Properties.ReceiptNumber, typeof(ACCOUNTINGEN.ApplicationAccounting).Name), ACCOUNTINGEN.ApplicationAccounting.Properties.ReceiptNumber));
            /***ANALISYS**/
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.ApplicationAccountingAnalysis.Properties.AppAccountingAnalysisCode, typeof(ACCOUNTINGEN.ApplicationAccountingAnalysis).Name), ACCOUNTINGEN.ApplicationAccountingAnalysis.Properties.AppAccountingAnalysisCode));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.ApplicationAccountingAnalysis.Properties.AnalysisCode, typeof(ACCOUNTINGEN.ApplicationAccountingAnalysis).Name), ACCOUNTINGEN.ApplicationAccountingAnalysis.Properties.AnalysisCode));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.ApplicationAccountingAnalysis.Properties.AnalysisConceptCode, typeof(ACCOUNTINGEN.ApplicationAccountingAnalysis).Name), ACCOUNTINGEN.ApplicationAccountingAnalysis.Properties.AnalysisConceptCode));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.ApplicationAccountingAnalysis.Properties.ConceptKey, typeof(ACCOUNTINGEN.ApplicationAccountingAnalysis).Name), ACCOUNTINGEN.ApplicationAccountingAnalysis.Properties.ConceptKey));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.ApplicationAccountingAnalysis.Properties.Description, typeof(ACCOUNTINGEN.ApplicationAccountingAnalysis).Name), ACCOUNTINGEN.ApplicationAccountingAnalysis.Properties.Description));
            /***CostCenter*****/
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.ApplicationAccountingCostCenter.Properties.AppAccountingCostCenterCode, typeof(ACCOUNTINGEN.ApplicationAccountingCostCenter).Name), ACCOUNTINGEN.ApplicationAccountingCostCenter.Properties.AppAccountingCostCenterCode));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.ApplicationAccountingCostCenter.Properties.CostCenterCode, typeof(ACCOUNTINGEN.ApplicationAccountingCostCenter).Name), ACCOUNTINGEN.ApplicationAccountingCostCenter.Properties.CostCenterCode));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.ApplicationAccountingCostCenter.Properties.Percentage, typeof(ACCOUNTINGEN.ApplicationAccountingCostCenter).Name), ACCOUNTINGEN.ApplicationAccountingCostCenter.Properties.Percentage));

            Join join = new Join(new ClassNameTable(typeof(ACCOUNTINGEN.ApplicationAccounting), typeof(ACCOUNTINGEN.ApplicationAccounting).Name), new ClassNameTable(typeof(ACCOUNTINGEN.ApplicationAccountingAnalysis), typeof(ACCOUNTINGEN.ApplicationAccountingAnalysis).Name), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ACCOUNTINGEN.ApplicationAccounting.Properties.AppAccountingCode, typeof(ACCOUNTINGEN.ApplicationAccounting).Name)
                .Equal()
                .Property(ACCOUNTINGEN.ApplicationAccountingAnalysis.Properties.AppAccountingCode, typeof(ACCOUNTINGEN.ApplicationAccountingAnalysis).Name)
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(ACCOUNTINGEN.ApplicationAccountingCostCenter), typeof(ACCOUNTINGEN.ApplicationAccountingCostCenter).Name), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ACCOUNTINGEN.ApplicationAccounting.Properties.AppAccountingCode, typeof(ACCOUNTINGEN.ApplicationAccounting).Name)
                .Equal()
                .Property(ACCOUNTINGEN.ApplicationAccountingCostCenter.Properties.AppAccountingCode, typeof(ACCOUNTINGEN.ApplicationAccountingCostCenter).Name)
                .GetPredicate());
            selectQuery.Table = join;
            selectQuery.Where = criteriaBuilder.GetPredicate();
            

            List<ApplicationAccounting> applicationAccountings = new List<ApplicationAccounting>();
            ApplicationAccounting applicationAccounting = new ApplicationAccounting();
            ApplicationAccountingCostCenter applicationAccountingCostCenter = new ApplicationAccountingCostCenter();
            ApplicationAccountingAnalysis applicationAccountingAnalysis = new ApplicationAccountingAnalysis();
            applicationAccountingAnalysis.AnalysisConcept = new AnalysisConcept();
            int appCostCenterId;
            int appAnalysisId;
            int appCodeId;
            using (IDataReader reader = _dataFacadeManager.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    if (reader[ACCOUNTINGEN.ApplicationAccounting.Properties.AppAccountingCode] == null)
                    {
                        continue;
                    }
                    appCodeId = Convert.ToInt32(reader[ACCOUNTINGEN.ApplicationAccounting.Properties.AppAccountingCode]?.ToString());
                    if (applicationAccountings.Count(x => x.Id == appCodeId) == 0)
                    {
                        applicationAccounting = new ApplicationAccounting { 
                            Id = Convert.ToInt32(reader[ACCOUNTINGEN.ApplicationAccounting.Properties.AppAccountingCode]?.ToString()),
                            Branch = new CommonService.Models.Branch() { Id = Convert.ToInt32(reader[ACCOUNTINGEN.ApplicationAccounting.Properties.BranchCode]?.ToString()) },
                            Beneficiary = new UniquePersonService.V1.Models.Individual () { IndividualId = Convert.ToInt32(reader[ACCOUNTINGEN.ApplicationAccounting.Properties.IndividualCode].ToString()) },
                            SalePoint = new CommonService.Models.SalePoint() { Id = Convert.ToInt32(reader[ACCOUNTINGEN.ApplicationAccounting.Properties.SalePointCode].ToString()) },
                            AccountingNature = Convert.ToInt32(reader[ACCOUNTINGEN.ApplicationAccounting.Properties.AccountingNature]?.ToString()),
                            Amount = new CommonService.Models.Amount() { Value = Convert.ToDecimal(reader[ACCOUNTINGEN.ApplicationAccounting.Properties.Amount]?.ToString()) },
                            LocalAmount = new CommonService.Models.Amount() { Value = Convert.ToDecimal(reader[ACCOUNTINGEN.ApplicationAccounting.Properties.LocalAmount]?.ToString()) },
                            ExchangeRate = new CommonService.Models.ExchangeRate() { SellAmount = Convert.ToDecimal(reader[ACCOUNTINGEN.ApplicationAccounting.Properties.ExchangeRate]?.ToString()) },
                            ApplicationAccountingId = Convert.ToInt32(reader[ACCOUNTINGEN.ApplicationAccounting.Properties.AccountingAccountCode]?.ToString()),
                            AccountingConcept = new AccountingConcept() { Id = reader[ACCOUNTINGEN.ApplicationAccounting.Properties.AccountingConceptCode]?.ToString() },
                            BankReconciliationId = Convert.ToInt32(reader[ACCOUNTINGEN.ApplicationAccounting.Properties.BankReconciliationCode]?.ToString()),
                            Description = reader[ACCOUNTINGEN.ApplicationAccounting.Properties.Description]?.ToString(),
                            AccountingCostCenters = new List<ApplicationAccountingCostCenter>(),
                            AccountingAnalysisCodes = new List<ApplicationAccountingAnalysis>()
                        };
                        applicationAccountings.Add(applicationAccounting);
                    }
                    applicationAccounting = applicationAccountings.Where(x => x.Id == appCodeId).First();
                    if (applicationAccounting == null)
                    {
                        continue;
                    }
                    if (reader[ACCOUNTINGEN.ApplicationAccountingCostCenter.Properties.CostCenterCode] != null)
                    {
                        appCostCenterId = Convert.ToInt32(reader[ACCOUNTINGEN.ApplicationAccountingCostCenter.Properties.CostCenterCode]);
                        if (applicationAccounting.AccountingCostCenters.Count(x => x.Id == appCostCenterId) == 0)
                        {
                            applicationAccountingCostCenter.Percentage = Convert.ToDecimal(reader[ACCOUNTINGEN.ApplicationAccountingCostCenter.Properties.Percentage]?.ToString());
                            applicationAccountingCostCenter.CostCenter = new CostCenter() { CostCenterId = Convert.ToInt32(reader[ACCOUNTINGEN.ApplicationAccountingCostCenter.Properties.CostCenterCode]?.ToString()) };
                            applicationAccounting.AccountingCostCenters.Add(applicationAccountingCostCenter);
                        }
                    }
                    if (reader[ACCOUNTINGEN.ApplicationAccountingAnalysis.Properties.AppAccountingAnalysisCode] != null)
                    {
                        appAnalysisId = Convert.ToInt32(reader[ACCOUNTINGEN.ApplicationAccountingAnalysis.Properties.AppAccountingAnalysisCode]);
                        if (applicationAccounting.AccountingAnalysisCodes.Count(x => x.Id == appAnalysisId) == 0)
                        {

                            applicationAccountingAnalysis.AnalysisConcept.AnalysisConceptId = Convert.ToInt32(reader[ACCOUNTINGEN.ApplicationAccountingAnalysis.Properties.AnalysisConceptCode]?.ToString());
                            applicationAccountingAnalysis.Id = Convert.ToInt32(reader[ACCOUNTINGEN.ApplicationAccountingAnalysis.Properties.AppAccountingAnalysisCode]);
                            applicationAccountingAnalysis.ConceptKey = reader[ACCOUNTINGEN.ApplicationAccountingAnalysis.Properties.ConceptKey]?.ToString();
                            applicationAccountingAnalysis.AnalysisId = Convert.ToInt32(reader[ACCOUNTINGEN.ApplicationAccountingAnalysis.Properties.AnalysisCode]?.ToString());
                            applicationAccountingAnalysis.Description = reader[ACCOUNTINGEN.ApplicationAccountingAnalysis.Properties.Description]?.ToString();
                            applicationAccounting.AccountingAnalysisCodes.Add(applicationAccountingAnalysis);
                        }
                    }
                }
            }
            return applicationAccountings;
        }
        
        public ApplicationAccounting SaveApplicationAccounting(ApplicationAccounting applicationAccounting)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.ApplicationAccounting entityApplicationAccounting = EntityAssembler.CreateApplicationAccounting(applicationAccounting);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(entityApplicationAccounting);

                // Return del model
                return ModelAssembler.CreateApplicationAccounting(entityApplicationAccounting);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}
