using Sistran.Core.Application.AccountingServices.Assemblers;
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Application.AccountingServices.EEProvider.DAOs;
using Sistran.Core.Application.AccountingServices.EEProvider.DAOs.Accounting;
using Sistran.Core.Application.AccountingServices.EEProvider.DAOs.Accounting.Reversion;
using Sistran.Core.Application.AccountingServices.EEProvider.Models;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Collect;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Reversion;
using Sistran.Core.Application.AccountingServices.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Contexts;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Integration.UndewritingIntegrationServices.DTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using GENERALLEDGEREN = Sistran.Core.Application.GeneralLedger.Entities;
using Transaction = Sistran.Core.Framework.Transactions.Transaction;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Business
{
    internal class ApplicationBusiness
    {
        public List<ApplicationAccounting> GetTempAccountingTransactionItemByTempApplicationId(int tempApplicationId)
        {
            List<ApplicationAccounting> tempApplicationAccounting = new List<ApplicationAccounting>();

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.GetAppTempDailyAccoutingV.Properties.TempAppCode, tempApplicationId);

            List<ACCOUNTINGEN.GetAppTempDailyAccoutingV> entityDailyAccountings = DataFacadeManager.GetObjects(
                    typeof(ACCOUNTINGEN.GetAppTempDailyAccoutingV), criteriaBuilder.GetPredicate()).
                    Cast<ACCOUNTINGEN.GetAppTempDailyAccoutingV>().ToList();

            if (entityDailyAccountings != null && entityDailyAccountings.Count > 0)
            {
                // Load DTO
                foreach (ACCOUNTINGEN.GetAppTempDailyAccoutingV entityDailyAccounting in entityDailyAccountings)
                {
                    tempApplicationAccounting.Add(GetApplicationAccountingFromEntity(entityDailyAccounting));
                }
            }


            return tempApplicationAccounting;
        }

        public ApplicationAccounting GetTempAccountingTransactionByTempAccountingApplicationId(int tempAppAccountingCode)
        {
            ApplicationAccounting tempApplicationAccounting = new ApplicationAccounting();

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.GetAppTempDailyAccoutingV.Properties.TempAppAccountingCode, tempAppAccountingCode);

            List<ACCOUNTINGEN.GetAppTempDailyAccoutingV> entityDailyAccountings = DataFacadeManager.GetObjects(
                    typeof(ACCOUNTINGEN.GetAppTempDailyAccoutingV), criteriaBuilder.GetPredicate()).
                    Cast<ACCOUNTINGEN.GetAppTempDailyAccoutingV>().ToList();

            if (entityDailyAccountings != null && entityDailyAccountings.Count > 0)
                return GetApplicationAccountingFromEntity(entityDailyAccountings.First());

            return tempApplicationAccounting;
        }

        private ApplicationAccounting GetApplicationAccountingFromEntity(ACCOUNTINGEN.GetAppTempDailyAccoutingV entityDailyAccounting)
        {
            string paymentConceptCode = "";
            string paymentConceptDescription = "";

            if (Convert.ToInt32(entityDailyAccounting.AccountingConceptCode) > 0)
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AccountingConcept.Properties.AccountingConceptCode,
                                        Convert.ToInt32(entityDailyAccounting.AccountingConceptCode));

                List<GENERALLEDGEREN.AccountingConcept> entityPaymentsConcepts = DataFacadeManager.GetObjects(
                        typeof(GENERALLEDGEREN.AccountingConcept), criteriaBuilder.GetPredicate()).
                        Cast<GENERALLEDGEREN.AccountingConcept>().ToList();

                foreach (GENERALLEDGEREN.AccountingConcept paymentConcept in entityPaymentsConcepts)
                {
                    paymentConceptCode = Convert.ToString(paymentConcept.AccountingConceptCode);
                    paymentConceptDescription = paymentConcept.Description;
                }
            }
            List<ApplicationAccountingAnalysis> applicationAccountingAnalyses =
                GetTempApplicationAccountingAnalysisByTempAppAccountingId(entityDailyAccounting.TempAppAccountingCode);


            List<ApplicationAccountingCostCenter> applicationAccountingCostCenters =
                GetTempApplicationAccountingCostCentersByTempAppAccountingId(entityDailyAccounting.TempAppAccountingCode);

            return new ApplicationAccounting()
            {
                Id = Convert.ToInt32(entityDailyAccounting.TempAppAccountingCode),
                ApplicationAccountingId = Convert.ToInt32(entityDailyAccounting.TempAppCode),
                Branch = new Branch()
                {
                    Id = Convert.ToInt32(entityDailyAccounting.BranchCode),
                    Description = Convert.ToString(entityDailyAccounting.BranchDescription)
                },
                SalePoint = new SalePoint()
                {
                    Id = Convert.ToInt32(entityDailyAccounting.SalePointCode),
                    Description = Convert.ToString(entityDailyAccounting.SalePointDescription)
                },
                AccountingConcept = new Models.Imputations.AccountingConcept()
                {
                    Id = paymentConceptCode,
                    Description = paymentConceptDescription
                },
                BookAccount = new BookAccount()
                {
                    Id = Convert.ToInt32(entityDailyAccounting.AccountingAccountCode),
                    AccountNumber = Convert.ToString(entityDailyAccounting.AccountingNumber),
                    AccountName = Convert.ToString(entityDailyAccounting.AccountingName)
                },
                AccountingNature = Convert.ToInt32(entityDailyAccounting.AccountingNatureCode),
                AccountingNatureDescription = Resources.Resources.ResourceManager.GetString(
                        Enums.EnumHelper.GetEnumDescription((
                        AccountingServices.Enums.AccountingNature)Convert.ToInt32(entityDailyAccounting.AccountingNatureCode))),
                Amount = new Amount()
                {
                    Currency = new Currency()
                    {
                        Id = Convert.ToInt32(entityDailyAccounting.CurrencyCode),
                        Description = Convert.ToString(entityDailyAccounting.CurrencyDescription)
                    },
                    Value = Convert.ToDecimal(entityDailyAccounting.Amount)
                },
                ExchangeRate = new ExchangeRate()
                {
                    SellAmount = Convert.ToDecimal(entityDailyAccounting.ExchangeRate),
                },
                LocalAmount = new Amount()
                {
                    Value = Convert.ToDecimal(entityDailyAccounting.LocalAmount),
                },
                Description = Convert.ToString(entityDailyAccounting.Description),
                Beneficiary = new Individual()
                {
                    IndividualId = Convert.ToInt32(entityDailyAccounting.IndividualCode),
                    IdentificationDocument = new IdentificationDocument()
                    {
                        Number = Convert.ToString(entityDailyAccounting.BeneficiaryDocumentNumber)
                    },
                    FullName = Convert.ToString(entityDailyAccounting.BeneficiaryName)
                },
                BankReconciliationId = Convert.ToInt32(entityDailyAccounting.BankReconciliationCode),
                ReceiptNumber = Convert.ToInt32(entityDailyAccounting.ReceiptNumber),
                ReceiptDate = Convert.ToDateTime(entityDailyAccounting.ReceiptDate),
                AccountingAnalysisCodes = applicationAccountingAnalyses,
                AccountingCostCenters = applicationAccountingCostCenters,
            };
        }

        public List<ApplicationAccountingAnalysis> GetTempApplicationAccountingAnalysisByTempAppAccountingId(int tempAppAccountingId)
        {
            List<ApplicationAccountingAnalysis> tempApplicationAccountingAnalysis = new List<ApplicationAccountingAnalysis>();

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplicationAccountingAnalysis.Properties.TempAppAccountingCode, tempAppAccountingId);

            List<ACCOUNTINGEN.TempApplicationAccountingAnalysis> entityAnalysisList = DataFacadeManager.GetObjects(
                        typeof(ACCOUNTINGEN.TempApplicationAccountingAnalysis), criteriaBuilder.GetPredicate()).
                        Cast<ACCOUNTINGEN.TempApplicationAccountingAnalysis>().ToList();

            if (entityAnalysisList != null && entityAnalysisList.Count > 0)
            {
                TempApplicationAccountingAnalysisDAO tempApplicationAccountingAnalysisDAO = new TempApplicationAccountingAnalysisDAO();
                foreach (ACCOUNTINGEN.TempApplicationAccountingAnalysis tempDailyAccountingAnalysisEntity in entityAnalysisList)
                {
                    ApplicationAccountingAnalysis tempAnalysisCode = tempApplicationAccountingAnalysisDAO.GetTempAccountingAnalysisCode(tempDailyAccountingAnalysisEntity.TempAppAccountingAnalysisCode);
                    tempApplicationAccountingAnalysis.Add(tempAnalysisCode);
                }
            }
            return tempApplicationAccountingAnalysis;
        }

        public List<ApplicationAccountingCostCenter> GetTempApplicationAccountingCostCentersByTempAppAccountingId(int tempAppAccountingId)
        {
            TempApplicationAccountingCostCenterDAO tempApplicationAccountingCostCenterDAO = new TempApplicationAccountingCostCenterDAO();
            List<ApplicationAccountingCostCenter> tempApplicationAccountingCostCenter = new List<ApplicationAccountingCostCenter>();

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplicationAccountingCostCenter.Properties.TempAppAccountingCode, tempAppAccountingId);

            List<ACCOUNTINGEN.TempApplicationAccountingCostCenter> entityCostCenterList = DataFacadeManager.GetObjects(
                        typeof(ACCOUNTINGEN.TempApplicationAccountingCostCenter), criteriaBuilder.GetPredicate()).
                        Cast<ACCOUNTINGEN.TempApplicationAccountingCostCenter>().ToList();

            if (entityCostCenterList != null && entityCostCenterList.Count > 0)
            {
                foreach (ACCOUNTINGEN.TempApplicationAccountingCostCenter tempDailyAccountingCostCenterEntity in entityCostCenterList)
                {
                    ApplicationAccountingCostCenter tempCostCenter = tempApplicationAccountingCostCenterDAO.GetTempAccountingCostCenter(tempDailyAccountingCostCenterEntity.TempAppAccountingCostCenterCode);
                    tempApplicationAccountingCostCenter.Add(tempCostCenter);
                }
            }

            return tempApplicationAccountingCostCenter;
        }

        public int SaveTempAccountingTransaction(ApplicationAccountingTransaction applicationAccountingTransaction)
        {
            int tempAppAccountingId = 0;

            foreach (ApplicationAccounting applicationAccounting in applicationAccountingTransaction.ApplicationAccountingItems)
            {
                TempApplicationAccountingItemDAO tempApplicationAccountingItemDAO = new TempApplicationAccountingItemDAO();
                TempApplicationAccountingAnalysisDAO tempApplicationAccountingAnalysisDAO = new TempApplicationAccountingAnalysisDAO();
                TempApplicationAccountingCostCenterDAO tempApplicationAccountingCostCenterDAO = new TempApplicationAccountingCostCenterDAO();

                if (applicationAccounting.Id == 0)
                {
                    tempAppAccountingId = tempApplicationAccountingItemDAO.SaveTempApplicationAccounting(applicationAccounting);

                    //graba cOdigos de analisis
                    if (applicationAccounting.AccountingAnalysisCodes != null
                        && applicationAccounting.AccountingAnalysisCodes.Count > 0)
                    {
                        foreach (ApplicationAccountingAnalysis tempApplicationAccountingAnalysis in applicationAccounting.AccountingAnalysisCodes)
                        {
                            tempApplicationAccountingAnalysisDAO.SaveTempApplicationAccountingAnalysis(tempApplicationAccountingAnalysis, tempAppAccountingId);
                        }
                    }

                    //graba centros de costos
                    if (applicationAccounting.AccountingCostCenters != null
                        && applicationAccounting.AccountingCostCenters.Count > 0)
                    {
                        foreach (ApplicationAccountingCostCenter tempApplicationAccountingCostCenter in applicationAccounting.AccountingCostCenters)
                        {
                            tempApplicationAccountingCostCenterDAO.SaveTempAccountingCostCenter(tempApplicationAccountingCostCenter, tempAppAccountingId);
                        }
                    }
                }
                else
                {
                    tempAppAccountingId = tempApplicationAccountingItemDAO.UpdateTempApplicationAccounting(applicationAccounting);

                    //analisis
                    //se eliminan los registros previos.
                    List<ApplicationAccountingAnalysis> tempApplicationAccountingAnalysis = GetTempApplicationAccountingAnalysisByTempAppAccountingId(tempAppAccountingId);

                    if (tempApplicationAccountingAnalysis != null
                        && tempApplicationAccountingAnalysis.Count > 0)
                    {
                        foreach (ApplicationAccountingAnalysis tempDailyAccountingAnalysis in tempApplicationAccountingAnalysis)
                        {
                            tempApplicationAccountingAnalysisDAO.DeleteTempApplicationAccountingAnalysis(tempDailyAccountingAnalysis.Id);
                        }
                    }

                    //se graban los nuevos registros
                    if (applicationAccounting.AccountingAnalysisCodes != null
                        && applicationAccounting.AccountingAnalysisCodes.Count > 0)
                    {
                        foreach (ApplicationAccountingAnalysis tempDailyAccountingAnalysis in applicationAccounting.AccountingAnalysisCodes)
                        {
                            tempApplicationAccountingAnalysisDAO.SaveTempApplicationAccountingAnalysis(tempDailyAccountingAnalysis, tempAppAccountingId);
                        }
                    }

                    //centros de costos
                    //se eliminan los registros previos
                    List<ApplicationAccountingCostCenter> tempApplicationAccountingCostCenters = GetTempApplicationAccountingCostCentersByTempAppAccountingId(tempAppAccountingId);

                    if (tempApplicationAccountingCostCenters != null
                        && tempApplicationAccountingCostCenters.Count > 0)
                    {
                        foreach (ApplicationAccountingCostCenter ApplicationAccountingCostCenter in tempApplicationAccountingCostCenters)
                        {
                            tempApplicationAccountingCostCenterDAO.DeleteTempAccountingCostCenter(ApplicationAccountingCostCenter.Id);
                        }
                    }

                    //se graban los nuevos registros.
                    if (applicationAccounting.AccountingCostCenters != null
                        && applicationAccounting.AccountingCostCenters.Count > 0)
                    {
                        foreach (ApplicationAccountingCostCenter tempDailyAccountingCostCenter in applicationAccounting.AccountingCostCenters)
                        {
                            tempApplicationAccountingCostCenterDAO.SaveTempAccountingCostCenter(tempDailyAccountingCostCenter, tempAppAccountingId);
                        }
                    }
                }
            }

            return tempAppAccountingId;
        }

        public bool DeleteTempApplicationAccounting(int tempApplicationAccountingId)
        {
            TempApplicationAccountingAnalysisDAO tempApplicationAccountingAnalysisDAO = new TempApplicationAccountingAnalysisDAO();
            TempApplicationAccountingCostCenterDAO tempApplicationAccountingCostCenterDAO = new TempApplicationAccountingCostCenterDAO();
            //se eliminan los analisis
            List<ApplicationAccountingAnalysis> tempApplicationAccountingAnalysisList = GetTempApplicationAccountingAnalysisByTempAppAccountingId(tempApplicationAccountingId);

            if (tempApplicationAccountingAnalysisList != null
                && tempApplicationAccountingAnalysisList.Count > 0)
            {
                foreach (ApplicationAccountingAnalysis tempApplicationAccountingAnalysis in tempApplicationAccountingAnalysisList)
                {
                    tempApplicationAccountingAnalysisDAO.DeleteTempApplicationAccountingAnalysis(tempApplicationAccountingAnalysis.Id);
                }
            }

            //se eliminan los centros de costos
            List<ApplicationAccountingCostCenter> tempApplicationAccountingCostCenters = GetTempApplicationAccountingCostCentersByTempAppAccountingId(tempApplicationAccountingId);

            if (tempApplicationAccountingCostCenters != null
                && tempApplicationAccountingCostCenters.Count > 0)
            {
                foreach (ApplicationAccountingCostCenter tempApplicationAccountingCostCenter in tempApplicationAccountingCostCenters)
                {
                    tempApplicationAccountingCostCenterDAO.DeleteTempAccountingCostCenter(tempApplicationAccountingCostCenter.Id);
                }
            }

            return (new TempApplicationAccountingItemDAO()).DeleteTempApplicationAccounting(tempApplicationAccountingId);
        }

        public Models.Imputations.Application GetTempApplicationBySourceCode(int moduleId, int sourceCode)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplication.Properties.ModuleCode, moduleId);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplication.Properties.SourceCode, sourceCode);

            List<ACCOUNTINGEN.TempApplication> entityTempApplications = DataFacadeManager.GetObjects(
                        typeof(ACCOUNTINGEN.TempApplication), criteriaBuilder.GetPredicate()).
                        Cast<ACCOUNTINGEN.TempApplication>().ToList();

            Models.Imputations.Application application = new Models.Imputations.Application();

            if (entityTempApplications != null && entityTempApplications.Count > 0)
            {
                foreach (ACCOUNTINGEN.TempApplication imputationEntity in entityTempApplications)
                {
                    application.Id = imputationEntity.TempAppCode;
                    application.RegisterDate = Convert.ToDateTime(imputationEntity.RegisterDate);
                    application.UserId = Convert.ToInt32(imputationEntity.UserCode);
                    application.SourceId = Convert.ToInt32(imputationEntity.SourceCode);
                }
            }

            return application;
        }

        private int SaveApplicationAccounting(ApplicationAccounting applicationAccounting, int imputationId, int paymentConceptCode, string description, int bankReconciliationId, int receiptNumber, DateTime? receiptDate)
        {
            ApplicationAccountingItemDAO applicationAccountingItemDAO = new ApplicationAccountingItemDAO();
            ApplicationAccountingAnalysisDAO applicationAccountingAnalysisDAO = new ApplicationAccountingAnalysisDAO();
            ApplicationAccountingCostCenterDAO applicationAccountingCostCenterDAO = new ApplicationAccountingCostCenterDAO();

            int applicationAccountingId = applicationAccountingItemDAO.SaveApplicationAccounting(applicationAccounting, imputationId, paymentConceptCode, description, bankReconciliationId, receiptNumber, receiptDate);

            //grabaci󮠤e analisis.
            if (applicationAccounting.AccountingAnalysisCodes != null
                && applicationAccounting.AccountingAnalysisCodes.Count > 0)
            {
                foreach (ApplicationAccountingAnalysis accountingAnalysis in applicationAccounting.AccountingAnalysisCodes)
                {
                    accountingAnalysis.Id = 0;
                    applicationAccountingAnalysisDAO.SaveAccountingAnalysisCode(accountingAnalysis, applicationAccountingId);
                }
            }

            if (applicationAccounting.AccountingCostCenters != null
                && applicationAccounting.AccountingCostCenters.Count > 0)
            {
                foreach (ApplicationAccountingCostCenter accountingCostCenter in applicationAccounting.AccountingCostCenters)
                {
                    accountingCostCenter.Id = 0;
                    applicationAccountingCostCenterDAO.SaveApplicationAccountingCostCenter(accountingCostCenter, applicationAccountingId);
                }
            }
            return applicationAccountingId;
        }

        public bool ConvertTempAccountingTransactionToAccountingTransaction(int sourceCodeId, int imputationTypeId, int applicationId)
        {
            ApplicationAccountingItemDAO applicationAccountingItemDAO = new ApplicationAccountingItemDAO();
            TempApplicationAccountingItemDAO tempApplicationAccountingItemDAO = new TempApplicationAccountingItemDAO();
            TempApplicationAccountingDAO tempApplicationAccountingDAO = new TempApplicationAccountingDAO();

            // Obtengo el temporal.
            Models.Imputations.Application tempApplication = GetTempApplicationBySourceCode(imputationTypeId, sourceCodeId);

            tempApplication.ApplicationItems = new List<TransactionType>();
            DailyAccountingTransaction tempDailyAccountingTransaction = tempApplicationAccountingDAO.GetTempApplicationAccountingByTempApplicationId(tempApplication.Id);

            tempApplication.ApplicationItems.Add(tempDailyAccountingTransaction);

            Models.Imputations.Application application = new Models.Imputations.Application()
            {
                RegisterDate = DateTime.Now,
                UserId = tempApplication.UserId
            };

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplicationAccounting.Properties.TempAppCode, tempApplication.Id);

            List<ACCOUNTINGEN.TempApplicationAccounting> entityTempApplicationAccountings = DataFacadeManager.GetObjects(
                        typeof(ACCOUNTINGEN.TempApplicationAccounting), criteriaBuilder.GetPredicate()).
                        Cast<ACCOUNTINGEN.TempApplicationAccounting>().ToList();

            if (entityTempApplicationAccountings != null && entityTempApplicationAccountings.Count > 0)
            {
                foreach (ACCOUNTINGEN.TempApplicationAccounting tempApplicationAccounting in entityTempApplicationAccountings)
                {
                    int applicationAccountingId = 0;
                    DateTime? receiptDate = null;

                    // Controla cuando la fecha del recibo es nula
                    if (Convert.ToDateTime(tempApplicationAccounting.ReceiptDate) == Convert.ToDateTime("01/01/0001 0:00:00"))
                    {
                        receiptDate = null;
                    }
                    else
                    {
                        receiptDate = Convert.ToDateTime(tempApplicationAccounting.ReceiptDate);
                    }

                    ApplicationAccounting applicationAccounting = new ApplicationAccounting();

                    applicationAccounting.Id = applicationAccountingId;
                    applicationAccounting.Branch = new Branch();
                    applicationAccounting.Branch.Id = Convert.ToInt32(tempApplicationAccounting.BranchCode);
                    applicationAccounting.SalePoint = new SalePoint();
                    applicationAccounting.SalePoint.Id = Convert.ToInt32(tempApplicationAccounting.SalePointCode);
                    applicationAccounting.Beneficiary = new Individual() { IndividualId = Convert.ToInt32(tempApplicationAccounting.IndividualCode) };
                    applicationAccounting.AccountingNature = (int)tempApplicationAccounting.AccountingNature;
                    applicationAccounting.Amount = new Amount();
                    applicationAccounting.Amount.Currency = new Currency();
                    applicationAccounting.Amount.Currency.Id = Convert.ToInt32(tempApplicationAccounting.CurrencyCode);
                    applicationAccounting.Amount.Value = Convert.ToDecimal(tempApplicationAccounting.Amount);
                    applicationAccounting.ExchangeRate = new ExchangeRate() { SellAmount = Convert.ToDecimal(tempApplicationAccounting.ExchangeRate) };
                    applicationAccounting.LocalAmount = new Amount() { Value = Convert.ToDecimal(tempApplicationAccounting.Amount) };
                    applicationAccounting.BookAccount = new BookAccount();
                    applicationAccounting.BookAccount.Id = Convert.ToInt32(tempApplicationAccounting.AccountingAccountCode);
                    applicationAccounting.AccountingAnalysisCodes = GetTempApplicationAccountingAnalysisByTempAppAccountingId(tempApplicationAccounting.TempAppAccountingCode);
                    applicationAccounting.AccountingCostCenters = GetTempApplicationAccountingCostCentersByTempAppAccountingId(tempApplicationAccounting.TempAppAccountingCode);

                    applicationAccountingId = SaveApplicationAccounting(applicationAccounting, applicationId, Convert.ToInt32(tempApplicationAccounting.AccountingConceptCode), tempApplicationAccounting.Description, Convert.ToInt32(tempApplicationAccounting.BankReconciliationCode), Convert.ToInt32(tempApplicationAccounting.ReceiptNumber), receiptDate);

                    // Si graba la transacción, se borra el temporal
                    if (applicationAccountingId > 0)
                    {
                        AccountingApplicationServiceEEProvider deletAnalysCenterCost = new AccountingApplicationServiceEEProvider();
                        deletAnalysCenterCost.DeleteTempApplicationAccounting(tempApplicationAccounting.TempAppAccountingCode);
                        tempApplicationAccountingItemDAO.DeleteTempApplicationAccounting(tempApplicationAccounting.TempAppAccountingCode);
                    }
                    else
                    {
                        criteriaBuilder = new ObjectCriteriaBuilder();
                        criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ApplicationAccounting.Properties.AppCode, applicationId);

                        List<ACCOUNTINGEN.ApplicationAccounting> entityApplicationAccountings = DataFacadeManager.GetObjects(
                                typeof(ACCOUNTINGEN.ApplicationAccounting), criteriaBuilder.GetPredicate()).
                                Cast<ACCOUNTINGEN.ApplicationAccounting>().ToList();

                        if (entityApplicationAccountings != null && entityApplicationAccountings.Count > 0)
                        {
                            foreach (ACCOUNTINGEN.ApplicationAccounting appAccounting in entityApplicationAccountings)
                            {
                                applicationAccountingItemDAO.DeleteApplicationAccounting(appAccounting.AppAccountingCode);
                            }
                        }
                        return false;
                    }
                }
            }
            return true;
        }

        public List<ApplicationPremiumComponent> GetPayableComponentstByEndorsementIdQuoutaId(int temAppplicationId, int endorsementId, int quotaId)
        {
            List<ApplicationPremiumComponent> components = new List<ApplicationPremiumComponent>();

            TempApplicationPremiumComponentDAO tempApplicationPremiumComponentDAO = new TempApplicationPremiumComponentDAO();
            List<TempApplicationPremiumComponent> tempApplicationPremiumComponents = tempApplicationPremiumComponentDAO.GetTempApplicationPremiumComponentsByTemAppPremium(temAppplicationId);

            // Se consulta la información de pago que existe antes de aplicar el pago
            List<PayerPaymentComponentDTO> payerPaymentsComponents =
                GetPayerPaymentComponentsByEndorsementIdQuotaNumber(endorsementId, quotaId);


            components.Add(GetPayableComponentsByEndorsementIdQuoutaIdComponentType(tempApplicationPremiumComponents, payerPaymentsComponents, "P"));
            components.Add(GetPayableComponentsByEndorsementIdQuoutaIdComponentType(tempApplicationPremiumComponents, payerPaymentsComponents, "I"));
            components.Add(GetPayableComponentsByEndorsementIdQuoutaIdComponentType(tempApplicationPremiumComponents, payerPaymentsComponents, "G"));

            return components;
        }

        private ApplicationPremiumComponent GetPayableComponentsByEndorsementIdQuoutaIdComponentType(
            List<TempApplicationPremiumComponent> tempApplicationPremiumComponents,
            List<PayerPaymentComponentDTO> payerPaymentsComponents, string componentType)
        {
            ApplicationPremiumComponent appPremiumComponent = new ApplicationPremiumComponent()
            {
                ComponentTinyDescription = componentType,
                Amount = 0,
                LocalAmount = 0,
                MainAmount = 0,
                MainLocalAmount = 0
            };

            if (tempApplicationPremiumComponents == null || tempApplicationPremiumComponents.Count == 0)
                return appPremiumComponent;

            tempApplicationPremiumComponents = tempApplicationPremiumComponents.Where(x => x.ComponentTinyDescription == componentType).ToList();

            // Se calcula el monto inicial
            if (payerPaymentsComponents != null && payerPaymentsComponents.Count > 0)
            {
                foreach (TempApplicationPremiumComponent tempApplicationPremiumComponent in tempApplicationPremiumComponents)
                {
                    appPremiumComponent.Amount += payerPaymentsComponents.Where(x => x.ComponentId == tempApplicationPremiumComponent.ComponentCode).Sum(x => x.Amount);
                    appPremiumComponent.LocalAmount += payerPaymentsComponents.Where(x => x.ComponentId == tempApplicationPremiumComponent.ComponentCode).Sum(x => x.LocalAmount);
                    appPremiumComponent.MainAmount += payerPaymentsComponents.Where(x => x.ComponentId == tempApplicationPremiumComponent.ComponentCode).Sum(x => x.MainAmount);
                    appPremiumComponent.MainLocalAmount += payerPaymentsComponents.Where(x => x.ComponentId == tempApplicationPremiumComponent.ComponentCode).Sum(x => x.MainLocalAmount);
                }
            }
            return appPremiumComponent;
        }

        public int GetAccountingAccountIdByBankIdCurrencyId(int bankId, int currencyId)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.BankAccountCompany.Properties.BankCode, bankId);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.BankAccountCompany.Properties.CurrencyCode, currencyId);

            List<ACCOUNTINGEN.BankAccountCompany> entityBanksAccountCompanies = DataFacadeManager.GetObjects(
                    typeof(ACCOUNTINGEN.BankAccountCompany), criteriaBuilder.GetPredicate()).
                    Cast<ACCOUNTINGEN.BankAccountCompany>().ToList();

            if (entityBanksAccountCompanies != null && entityBanksAccountCompanies.Count > 0)
            {
                return Convert.ToInt32(entityBanksAccountCompanies.First().AccountingAccountId);
            }
            return -1;
        }

        public int GetAccountingAccountIdByBankIdCurrencyIdAccountNumber(int bankId, int currencyId, string accountNumber)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.BankAccountCompany.Properties.BankCode, bankId);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.BankAccountCompany.Properties.CurrencyCode, currencyId);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.BankAccountCompany.Properties.AccountNumber, accountNumber);

            List<ACCOUNTINGEN.BankAccountCompany> entityBanksAccountCompanies = DataFacadeManager.GetObjects(
                    typeof(ACCOUNTINGEN.BankAccountCompany), criteriaBuilder.GetPredicate()).
                    Cast<ACCOUNTINGEN.BankAccountCompany>().ToList();

            if (entityBanksAccountCompanies != null && entityBanksAccountCompanies.Count > 0)
            {
                return Convert.ToInt32(entityBanksAccountCompanies.First().AccountingAccountId);
            }
            return -1;
        }

        public bool ReverseApplicationPremiumByCollectPaymentId(int collectPaymentId)
        {
            return true;
        }

        public List<ApplicationPremium> GetApplicationPremiumsByApplicationId(int applicationId)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.ApplicationPremium.Properties.AppCode, applicationId);

            List<ACCOUNTINGEN.ApplicationPremium> entityApplicationPremiums = DataFacadeManager.GetObjects(
                    typeof(ACCOUNTINGEN.ApplicationPremium), criteriaBuilder.GetPredicate()).
                    Cast<ACCOUNTINGEN.ApplicationPremium>().ToList();

            return ModelAssembler.CreateApplicationPremiums(entityApplicationPremiums);
        }

        public Models.Imputations.Application GetApplicationByApplicationId(int applicationId)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Application.Properties.ApplicationCode, applicationId);

            List<ACCOUNTINGEN.Application> entityApplication = DataFacadeManager.GetObjects(
                    typeof(ACCOUNTINGEN.Application), criteriaBuilder.GetPredicate()).
                    Cast<ACCOUNTINGEN.Application>().ToList();

            Models.Imputations.Application application = null;
            if (entityApplication != null && entityApplication.Count > 0)
            {
                application = ModelAssembler.CreateApplication(entityApplication[0]);
            }
            return application;
        }

        public bool ValidateTempApplication(int tempApplicationId)
        {
            var debitsAndCredits = GetDebitsAndCreditsByTempApplicationId(tempApplicationId);
            // Controla que s󬯠se genera la aplicaci󮠰ermanente si el ingreso estᠢalanceado
            return (Math.Abs(debitsAndCredits.Credits) - Math.Abs(debitsAndCredits.Debits) == 0);
        }

        public bool ValidateTempApplicationByTotal(int tempApplicationId, decimal total)
        {
            var debitsAndCredits = GetDebitsAndCreditsByTempApplicationId(tempApplicationId);
            return (debitsAndCredits.Credits - debitsAndCredits.Debits == total);
        }

        public Models.Application.DebitsandCredits GetDebitsAndCreditsByTempApplicationId(int tempApplicationId)
        {
            List<ReversionPremium> tempReversions = new List<ReversionPremium>();
            TempApplicationBusiness tempApplicationBusiness = new TempApplicationBusiness();
            List<ApplicationAccounting> applicationsAccountings = GetTempAccountingTransactionItemByTempApplicationId(tempApplicationId);
            TempApplicationPremiumDAO tempApplicationPremiumDAO = new TempApplicationPremiumDAO();
            List<ApplicationPremium> applicationPremiums =
                ModelAssembler.CreateTempApplicationPremiums(tempApplicationPremiumDAO.GetTempApplicationPremiumsByTempApplicationId(tempApplicationId));
            tempReversions = TempApplicationPremiumReversionDAO.GetTempAppRevPremiumBytempAppId(tempApplicationId);
            decimal totalCredit = 0;
            decimal totalDebit = 0;
            int decimalPlaces = 2;
            if (applicationsAccountings != null && applicationsAccountings.Any())
            {
                totalCredit = Math.Round(applicationsAccountings.Where(a => a.AccountingNature == (int)AccountingNature.Credit).Sum(s => Math.Round(s.LocalAmount.Value, decimalPlaces)), decimalPlaces);
                totalDebit = Math.Round(applicationsAccountings.Where(a => a.AccountingNature == (int)AccountingNature.Debit).Sum(s => Math.Round(s.LocalAmount.Value, decimalPlaces)), decimalPlaces);
            }
            if (applicationPremiums != null && applicationPremiums.Any())
            {
                totalCredit += applicationPremiums.Where(a => a.LocalAmount >= 0).Sum(m => m.LocalAmount);
                totalDebit += Math.Round(Math.Abs(applicationPremiums.Where(a => a.LocalAmount < 0).Sum(m => Math.Round(m.LocalAmount, decimalPlaces))), decimalPlaces);
                List<decimal> tempCommisions = new List<decimal>();
                applicationPremiums.Select(m => m.Id).ToList().ForEach(s =>
                {
                    tempCommisions.AddRange(tempApplicationBusiness.GetApplicationPremiumCommisionsByTempAppPremiumId(s).Select(u => Math.Round(u.LocalAmount, decimalPlaces)).ToList());
                });
                totalDebit += Math.Round(Math.Abs(tempCommisions.Where(a => a >= 0).Sum(m => Math.Round(m, decimalPlaces))), decimalPlaces);
                totalCredit += Math.Round(Math.Abs(tempCommisions.Where(a => a < 0).Sum(m => Math.Round(m, decimalPlaces))), decimalPlaces);
            }
            if (tempReversions != null && tempReversions.Any())
            {
                totalDebit += Math.Round(Math.Abs(tempReversions.Where(a => a.Premium < 0).Sum(m => Math.Round(m.Premium, decimalPlaces))), decimalPlaces);
                totalCredit += Math.Round(tempReversions.Where(a => a.PremiumCommision < 0).Sum(m => Math.Round(m.PremiumCommision, decimalPlaces)), decimalPlaces);
            }
            // Controla que sisee genera la aplicación permanente si el ingreso está balanceado

            return new Models.Application.DebitsandCredits()
            {
                Debits = totalDebit,
                Credits = totalCredit
            };
        }

        public List<PremiumSearchPolicyDTO> GetPaymentQuotas(SearchPolicyPaymentDTO searchPolicyPayment)
        {
            List<object> premiumReceivableSearchPoliciesResponses = new List<object>();
            if (searchPolicyPayment.BranchId == null)
                searchPolicyPayment.BranchId = "";

            if (searchPolicyPayment.PrefixId == null)
                searchPolicyPayment.PrefixId = "";

            if (searchPolicyPayment.InsuredId == null)
                searchPolicyPayment.InsuredId = "";

            if (searchPolicyPayment.PayerId == null)
                searchPolicyPayment.PayerId = "";

            if (searchPolicyPayment.AgentId == null)
                searchPolicyPayment.AgentId = "";

            if (searchPolicyPayment.GroupId == null)
                searchPolicyPayment.GroupId = "";

            if (searchPolicyPayment.PolicyId == null)
                searchPolicyPayment.PolicyId = "";

            if (searchPolicyPayment.PolicyDocumentNumber == null)
                searchPolicyPayment.PolicyDocumentNumber = "";

            if (searchPolicyPayment.SalesTicket == null)
                searchPolicyPayment.SalesTicket = "";

            if (searchPolicyPayment.EndorsementId == null)
                searchPolicyPayment.EndorsementId = "";

            if (searchPolicyPayment.DateFrom == null)
                searchPolicyPayment.DateFrom = "";

            if (searchPolicyPayment.DateTo == null)
                searchPolicyPayment.DateTo = "";

            if (searchPolicyPayment.PageSize == null)
                searchPolicyPayment.PageSize = "";

            if (searchPolicyPayment.PageIndex == null)
                searchPolicyPayment.PageIndex = "";

            if (searchPolicyPayment.InsuredDocumentNumber == null)
                searchPolicyPayment.InsuredDocumentNumber = "";

            if (searchPolicyPayment.EndorsementDocumentNumber == null)
                searchPolicyPayment.EndorsementDocumentNumber = "";


            List<PremiumSearchPolicyDTO> premiums = DelegateService.underwritingIntegrationService.GetPremiuPaymSearchPolicies(searchPolicyPayment);
            if (premiums != null && premiums.Count > 0)
            {
                List<ApplicationPremium> appplicationPremiums = GetApplicationPremiums(premiums);

                premiums.ForEach(x =>
                {
                    var appPremiums = appplicationPremiums.Where(y => y.EndorsementId == x.EndorsementId && x.PaymentNumber == y.PaymentNumber);
                    x.Amount -= appPremiums.Sum(z => z.Amount);
                    x.TotalPremium -= appPremiums.Sum(z => z.LocalAmount);
                });
            }
            return premiums;
        }

        private List<ApplicationPremium> GetApplicationPremiums(List<PremiumSearchPolicyDTO> premiums)
        {
            ApplicationPremiumItemDAO applicationPremiumItemDAO = new ApplicationPremiumItemDAO();
            return applicationPremiumItemDAO.GetApplicationsByFilter(premiums);
        }

        public List<PayerPaymentComponentDTO> GetPayerPaymentComponentsByEndorsementIdQuotaNumber(int endorsementId, int quotaNumber)
        {
            TempApplicationPremiumDAO tempApplicationPremiumDAO = new TempApplicationPremiumDAO();
            var applicationComponents = tempApplicationPremiumDAO.
                GetPayerPaymentComponentsByEndorsementIdQuotaNumber(endorsementId, quotaNumber);
            var components = DelegateService.underwritingIntegrationService.GetPayerPaymetComponentsByEndorsementIdQuota(endorsementId, quotaNumber);

            bool premiums = false;
            bool taxes = false;
            bool expenses = false;
            components.ForEach(x =>
            {
                if (x.TinyDescription == "P" && !premiums)
                {
                    premiums = true;
                    var filter = applicationComponents.Where(y => y.ComponentTinyDescription == "P");
                    x.Amount -= filter.Sum(z => z.Amount);
                    x.LocalAmount -= filter.Sum(z => z.LocalAmount);
                }
                else if (x.TinyDescription == "I" && !taxes)
                {
                    taxes = true;
                    var filter = applicationComponents.Where(y => y.ComponentTinyDescription == "I");
                    x.Amount -= filter.Sum(z => z.Amount);
                    x.LocalAmount -= filter.Sum(z => z.LocalAmount);
                }
                else if (x.TinyDescription == "G" && !expenses)
                {
                    expenses = true;
                    var filter = applicationComponents.Where(y => y.ComponentTinyDescription == "G");
                    x.Amount -= filter.Sum(z => z.Amount);
                    x.LocalAmount -= filter.Sum(z => z.LocalAmount);
                }
            });
            return components;
        }

        public List<PayerPaymentComponentLBSBDTO> GetPayerPaymentComponentsLSBSByPayerPaymentIdEndorsementIdQuotaNumber(int payerPaymentId, int endorsementId, int quotaNumber)
        {
            var components = DelegateService.integrationUnderwritingService.GetPayerPaymetComponentsLBSB(payerPaymentId);
            TempApplicationPremiumDAO tempApplicationPremiumDAO = new TempApplicationPremiumDAO();
            var applicationComponentsLBSB = tempApplicationPremiumDAO.GetPayerPaymentComponentsLSBSByEndorsementIdQuotaNumbers(endorsementId, quotaNumber);

            List<int> componentsIds = new List<int>();
            components.ForEach(x =>
            {
                if (!componentsIds.Contains(x.ComponentId))
                {
                    componentsIds.Add(x.ComponentId);

                    var group = applicationComponentsLBSB.Where(y => y.ApplicationComponentId == x.ComponentId
                        && x.SubLineBusiness == y.SubLineBussinesId
                        && x.LineBusiness == y.LineBussinesId);

                    x.Amount -= group.Sum(z => z.Amount);
                    x.LocalAmount -= group.Sum(z => z.LocalAmount);
                    x.MainAmount -= group.Sum(z => z.MainAmount);
                    x.MainLocalAmount -= group.Sum(z => z.MainLocalAmount);
                }
            });
            return components;
        }

        public Models.Imputations.Application GetTemporalApplicationByEndorsementIdPaymentNumber(int tempApplicationId, int endorsementId, int paymentNumber)
        {
            Models.Imputations.Application tempApplication = new Models.Imputations.Application();

            TempApplicationPremiumDAO tempApplicationPremiumDAO = new TempApplicationPremiumDAO();
            ApplicationPremium tempApplicationPremium = tempApplicationPremiumDAO.
                GetTemporalApplicationPremiumByEndorsementIdPaymentNumber(endorsementId, paymentNumber, tempApplicationId);

            if (tempApplicationPremium != null)
            {
                tempApplication = new TempApplicationDAO().GetTempApplicationByTempApplicationId(tempApplicationPremium.ApplicationId);
            }
            return tempApplication;
        }

        public int CheckoutAnalysisCodeByAnalysisConceptKeyIdKeyValue(int AnalysisConceptKeyId, string KeyValue)
        {
            ApplicationAccountingAnalysisDAO applicationAccountingAnalysisDAO = new ApplicationAccountingAnalysisDAO();
            return applicationAccountingAnalysisDAO.CheckoutAnalysisCodeByAnalysisConceptKeyIdKeyValue(AnalysisConceptKeyId, KeyValue);
        }

        ///<summary>
        /// GetTempPremiumReceivableItemByTempImputationId
        /// Trae un item de prima por cobrar
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <returns>List<PremiumReceivableItemDTO/></returns>
        public List<ApplicationAccounting> GetApplicationAccountsByApplicationId(int applicationId)
        {
            ApplicationAccountingItemDAO applicationAccountingItemDAO = new ApplicationAccountingItemDAO();
            return applicationAccountingItemDAO.GetApplicationAccountsByApplicationId(applicationId);
        }

        public Models.Imputations.Application GetApplicationByModuleIdSourceId(int moduleId, int sourceId)
        {
            ApplicationDAO applicationDAO = new ApplicationDAO();
            return applicationDAO.GetApplicationByModuleIdSourceId(moduleId, sourceId);
        }

        public Models.Imputations.Application SaveApplication(Models.Imputations.Application application)
        {
            ApplicationDAO applicationDAO = new ApplicationDAO();
            return applicationDAO.SaveApplication(application);
        }

        public bool ReverseApplication(Models.Imputations.Application application, Models.Imputations.Application newApplication)
        {
            try
            {
                decimal localAmountDifference = 0;
                int individualId = 0;
                var currentDate = DateTime.Now;

                List<Models.Imputations.ApplicationAccounting> accoutingMovements = GetApplicationAccountsByApplicationId(application.Id);
                List<Models.Imputations.ApplicationPremium> applicationPremiums = GetApplicationPremiumsByApplicationId(application.Id);

                if (applicationPremiums != null && applicationPremiums.Count > 0)
                {
                    ApplicationPremiumItemDAO applicationPremiumDAO = new ApplicationPremiumItemDAO();
                    PolicyComponentDistributionDAO policyComponentDistributionDAO = new PolicyComponentDistributionDAO();
                    ApplicationPremiumComponentDAO applicationPremiumComponentDAO = new ApplicationPremiumComponentDAO();
                    ApplicationPremiumCommisionDAO applicationPremiumCommisionDAO = new ApplicationPremiumCommisionDAO();

                    applicationPremiums.ForEach(premium =>
                    {
                        List<ApplicationPremiumCommision> commissions = applicationPremiumCommisionDAO.GetApplicationPremiumCommissByAppPremiumId(premium.Id);
                        var components = applicationPremiumComponentDAO.GetApplicationPremiumComponentsByAppPremium(premium.Id);

                        premium.Amount *= -1;
                        premium.LocalAmount *= -1;
                        premium.MainAmount *= -1;
                        premium.MainLocalAmount *= -1;
                        premium.DiscountCommission *= -1;
                        localAmountDifference += premium.LocalAmount;

                        if (individualId == 0)
                            individualId = premium.PayerId;

                        premium.Id = 0;
                        premium.RegisterDate = currentDate;
                        premium.ApplicationId = newApplication.Id;
                        premium.AccountingDate = newApplication.AccountingDate;
                        var newPremium = applicationPremiumDAO.SaveApplicationPremium(premium);
                        if (newPremium.Id > 0 && commissions != null && commissions.Count > 0)
                        {
                            commissions.ForEach(commission =>
                            {
                                commission.Amount *= -1;
                                commission.LocalAmount *= -1;
                                commission.Id = 0;
                                commission.ApplicationPremiumId = newPremium.Id;
                                localAmountDifference += commission.LocalAmount;
                            });
                            applicationPremiumCommisionDAO.CreateApplicationPremiumCommisions(commissions);
                        }
                        if (newPremium.Id > 0 && components != null && components.Count > 0)
                        {
                            components.ForEach(component =>
                            {
                                var lbsbComponents = policyComponentDistributionDAO.GetApplicationPremiumComponentsLBSBByAppPremiumComponent(component.AppComponentId);

                                component.Amount *= -1;
                                component.LocalAmount *= -1;
                                component.MainAmount *= -1;
                                component.MainLocalAmount *= -1;
                                component.AppComponentId = 0;
                                component.PremiumId = newPremium.Id;

                                var newComponent = policyComponentDistributionDAO.saveApplicationPremiumComponent(component);
                                if (newComponent.AppComponentId > 0 && lbsbComponents != null && lbsbComponents.Count > 0)
                                {
                                    lbsbComponents.ForEach(lsbsComponent =>
                                    {
                                        lsbsComponent.Amount *= -1;
                                        lsbsComponent.LocalAmount *= -1;
                                        lsbsComponent.MainAmount *= -1;
                                        lsbsComponent.MainLocalAmount *= -1;
                                        lsbsComponent.ApplicationComponenLSBSId = 0;
                                        lsbsComponent.ApplicationComponentId = newComponent.AppComponentId;

                                        policyComponentDistributionDAO.saveApplicationPremiumComponentLBSB(lsbsComponent);
                                    });
                                }
                            });
                        }

                        var payment = DelegateService.integrationUnderwritingService.GetPayerPaymet(premium.EndorsementId, premium.PaymentNumber);
                        if (payment != null && payment.PayerPaymentId > 0)
                        {
                            payment.PaymentState = 1;
                            DelegateService.underwritingIntegrationService.UpdateStatusPayerPayment(payment);
                        }
                    });
                }

                if (accoutingMovements != null && accoutingMovements.Count > 0)
                {
                    accoutingMovements.ForEach(movement =>
                    {
                        if (movement.AccountingNature == Convert.ToInt32(AccountingNature.Credit))
                        {
                            localAmountDifference -= movement.LocalAmount.Value;
                            movement.AccountingNature = Convert.ToInt32(AccountingNature.Debit);
                        }
                        else
                        {
                            localAmountDifference += movement.LocalAmount.Value;
                            movement.AccountingNature = Convert.ToInt32(AccountingNature.Credit);
                        }

                        if (individualId == 0)
                            individualId = movement.Beneficiary.IndividualId;

                        movement.Id = 0;
                        movement.ApplicationId = newApplication.Id;
                        SaveApplicationAccounting(movement);

                    });
                }

                if (localAmountDifference != 0 && application.ModuleId == Convert.ToInt32(ApplicationTypes.Collect))
                {
                    int currencyId = 0;
                    int accoutingConceptId = 0;
                    int accoutingApplicationId = 0;
                    string bridgeAccountNumber = CommonBusiness.GetTextParameter(Enums.AccountingKeys.ACC_BRIDGE_COLLECT_NUMBER);
                    if (String.IsNullOrEmpty(bridgeAccountNumber))
                    {
                        throw new ArgumentNullException();
                    }

                    var accounts = DelegateService.accountingApplicationService.GetAccountingAccountByNumber(bridgeAccountNumber, application.BranchId);
                    if (accounts != null && accounts.Count > 0)
                    {
                        accoutingApplicationId = accounts[0].AccountingAccountId;
                        AccountingAccountBusiness accountingAccountBusiness = new AccountingAccountBusiness();
                        accoutingConceptId = accountingAccountBusiness.GetConceptIdByAccoutingAccountId(accoutingApplicationId);
                    }
                    else
                    {
                        throw new ArgumentNullException();
                    }
                    try
                    {
                        currencyId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.PAYM_PAYMENT_CURRENCY));
                    }
                    catch (Exception)
                    {

                    }

                    ApplicationAccounting bridgeAccount = new ApplicationAccounting()
                    {
                        ApplicationId = newApplication.Id,
                        AccountingNature = Convert.ToInt32((localAmountDifference > 0) ? AccountingNature.Debit : AccountingNature.Credit),
                        Beneficiary = new Individual()
                        {
                            IndividualId = individualId
                        },
                        ExchangeRate = new ExchangeRate()
                        {
                            SellAmount = 1
                        },
                        LocalAmount = new Amount()
                        {
                            Value = Math.Abs(localAmountDifference)
                        },
                        Amount = new Amount()
                        {
                            Value = Math.Abs(localAmountDifference)
                        },
                        Description = newApplication.Description,
                        Branch = new Branch()
                        {
                            Id = application.BranchId
                        },
                        CurrencyId = currencyId,
                        SalePoint = new SalePoint
                        {
                            Id = 0
                        },
                        AccountingConcept = new Models.Imputations.AccountingConcept()
                        {
                            Id = Convert.ToString(accoutingConceptId)
                        },
                        ApplicationAccountingId = accoutingApplicationId,
                        AccountingCostCenters = new List<ApplicationAccountingCostCenter>(),
                        AccountingAnalysisCodes = new List<ApplicationAccountingAnalysis>()
                    };

                    SaveApplicationAccounting(bridgeAccount);
                }

                return true;
            }
            catch (Exception ex)
            {

            }
            return false;
        }

        private ApplicationAccounting SaveApplicationAccounting(ApplicationAccounting applicationAccounting)
        {
            ApplicationAccountingItemDAO applicationAccountingItemDAO = new ApplicationAccountingItemDAO();
            ApplicationAccountingAnalysisDAO applicationAccountingAnalysisDAO = new ApplicationAccountingAnalysisDAO();
            ApplicationAccountingCostCenterDAO applicationAccountingCostCenterDAO = new ApplicationAccountingCostCenterDAO();

            var newApplicationAccounting = applicationAccountingItemDAO.SaveApplicationAccounting(applicationAccounting);

            if (newApplicationAccounting.Id > 0)
            {
                //grabación de analisis.
                if (applicationAccounting.AccountingAnalysisCodes != null
                    && applicationAccounting.AccountingAnalysisCodes.Count > 0)
                {
                    foreach (ApplicationAccountingAnalysis accountingAnalysis in applicationAccounting.AccountingAnalysisCodes)
                    {
                        accountingAnalysis.Id = 0;
                        applicationAccountingAnalysisDAO.SaveAccountingAnalysisCode(accountingAnalysis, newApplicationAccounting.Id);
                    }
                }

                if (applicationAccounting.AccountingCostCenters != null
                    && applicationAccounting.AccountingCostCenters.Count > 0)
                {
                    foreach (ApplicationAccountingCostCenter accountingCostCenter in applicationAccounting.AccountingCostCenters)
                    {
                        accountingCostCenter.Id = 0;
                        applicationAccountingCostCenterDAO.SaveApplicationAccountingCostCenter(accountingCostCenter, newApplicationAccounting.Id);
                    }
                }
            }
            return newApplicationAccounting;
        }

        public Message ReverseApplication(int sourceId, int moduleId, int userId)
        {
            Message message = new Message()
            {
                Success = false
            };
            Collect collect = null;

            if (moduleId == Convert.ToInt32(ApplicationTypes.Collect))
            {
                CollectBusiness collectBusiness = new CollectBusiness();
                collect = collectBusiness.GetCollectByCollectId(sourceId);

                if (collect.Status == Convert.ToInt32(CollectStatus.Canceled))
                {
                    message.Info = Resources.Resources.CollectAlreadyCanceled;
                    return message;
                }
                else if (collect.Status == Convert.ToInt32(CollectStatus.Canceled))
                {
                    message.Info = Resources.Resources.CollectAlreadyReversed;
                    return message;
                }

                // Es una anulación
                else if (collect.Status == Convert.ToInt32(CollectStatus.Active)
                    || collect.Status == Convert.ToInt32(CollectStatus.PartiallyApplied))
                {
                    message = collectBusiness.CanCancelCollect(collect);
                    if (!message.Success)
                        return message;

                    var parameters = new JournalEntryReversionParameters()
                    {
                        TechnicalTransaction = collect.Transaction.TechnicalTransaction,
                        BranchId = collect.Branch.Id,
                        ModuleId = moduleId,
                        UserId = userId
                    };
                    message = ReverseJournalEntry(parameters);
                    if (message.Success)
                    {
                        if (!collectBusiness.UpdateCollectStatus(sourceId, Convert.ToInt32(CollectStatus.Canceled)))
                        {
                            message.Info = ((message.Info != null) ? message.Info : "") + Resources.Resources.ErrorCouldNotUpdateSourceStatus;
                        }
                        TempApplicationBusiness tempApplicationBusiness = new TempApplicationBusiness();
                        var tempAppId = tempApplicationBusiness.GetTempApplicationBySourceId(sourceId, moduleId);
                        if (tempAppId > 0)
                        {
                            IncomeBusiness incomeBusiness = new IncomeBusiness();
                            incomeBusiness.DeleteTempApplicationByTempAplicationIdModuleId(tempAppId, moduleId);
                        }
                    }
                    return message;
                }
            }
            Models.Imputations.Application application = GetApplicationByModuleIdSourceId(moduleId, sourceId);
            Models.Imputations.Application newApplication = new Models.Imputations.Application();
            JournalEntry newJournalEntry = new JournalEntry();

            if (application == null || application.Id == 0)
            {
                message.Info = Resources.Resources.ErrorApplicationNotFound;
                return message;
            }

            bool isReversed = false;

            using (Context.Current)
            {
                Transaction transaction = new Transaction();

                try
                {
                    if (moduleId == Convert.ToInt32(ApplicationTypes.Collect))
                    {
                        if (application.BranchId == 0 && collect != null && collect.Branch != null)
                            application.BranchId = collect.Branch.Id;
                        if (application.IndividualId == 0 && collect != null && collect.Payer != null)
                            application.IndividualId = collect.Payer.IndividualId;
                    }
                    else if (moduleId == Convert.ToInt32(ApplicationTypes.JournalEntry))
                    {
                        if (application.BranchId == 0 || application.IndividualId == 0)
                        {
                            var journalEntryDAO = new JournalEntryDAO();
                            var journalOrigin = journalEntryDAO.GetJournalEntryById(sourceId);
                            if (journalOrigin != null)
                            {
                                if (journalOrigin.Branch != null)
                                    application.BranchId = journalOrigin.Branch.Id;
                                if (journalOrigin.Payer != null)
                                    application.IndividualId = journalOrigin.Payer.IndividualId;
                            }
                        }
                    }

                    DateTime accountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ApplicationTypes.JournalEntry), DateTime.Now);
                    TechnicalTransactionBusiness technicalTransactionBusiness = new TechnicalTransactionBusiness();
                    var newTechnicalTransction = technicalTransactionBusiness.GetTechnicalTransaction(application.BranchId);
                    var description = String.Format(Resources.Resources.JournalEntryReversionComment, application.TechnicalTransaction);
                    // TODO
                    // Consultar company
                    // Consultar persontype
                    // Consultar salepoint

                    // crear el newJournalEntry
                    newJournalEntry = new JournalEntry()
                    {
                        AccountingDate = accountingDate,
                        Branch = new Branch() { Id = application.BranchId },
                        Comments = description,
                        Description = description,
                        IsTemporal = false,
                        Payer = new Individual() { IndividualId = application.IndividualId },
                        Status = Convert.ToInt32(JournalEntryStatus.Applied),
                        Transaction = new Models.Collect.Transaction() { TechnicalTransaction = newTechnicalTransction },
                        Company = new Company() { IndividualId = 0 },
                        SalePoint = new SalePoint() { Id = 0 },
                        PersonType = new PersonType() { Id = 0 }
                    };

                    JournalEntryBusiness journalEntryBusiness = new JournalEntryBusiness();
                    newJournalEntry = journalEntryBusiness.SaveJournalEntry(newJournalEntry);

                    if (newJournalEntry != null && newJournalEntry.Id > 0)
                    {
                        newApplication = new Models.Imputations.Application()
                        {
                            Id = 0,
                            UserId = userId,
                            RegisterDate = DateTime.Now,
                            AccountingDate = accountingDate,
                            ModuleId = Convert.ToInt32(ApplicationTypes.JournalEntry),
                            TechnicalTransaction = newTechnicalTransction,
                            SourceId = newJournalEntry.Id,
                            BranchId = application.BranchId,
                            IndividualId = application.IndividualId,
                            Description = description
                        };
                        newApplication = SaveApplication(newApplication);
                        if (newApplication != null && newApplication.Id > 0)
                        {
                            isReversed = ReverseApplication(application, newApplication);
                            if (!isReversed)
                            {
                                message.Info = Resources.Resources.ErrorCouldNotReverseApplication;
                            }
                        }
                        else
                        {
                            message.Info = Resources.Resources.ErrorCouldNotCreateApplication;
                        }

                        if (isReversed)
                        {
                            // Todas la imputaciones menos Asientos de Diario
                            if (moduleId == Convert.ToInt32(ApplicationTypes.Collect))
                            {
                                // Realizo la actualizacíon del estado del recibo a anulado.
                                CollectBusiness collectBusiness = new CollectBusiness();
                                isReversed = collectBusiness.UpdateCollectStatus(sourceId, Convert.ToInt32(CollectStatus.Reversed));
                                if (!isReversed)
                                {
                                    message.Info = Resources.Resources.ErrorCouldNotUpdateSourceStatus;
                                }
                            }
                            else if (moduleId == Convert.ToInt32(ApplicationTypes.JournalEntry))
                            {
                                // Realizo la actualizacíon del estado del recibo a anulado.
                                isReversed = journalEntryBusiness.UpdateJournalEntryStatus(sourceId, Convert.ToInt32(JournalEntryStatus.Canceled));
                                if (!isReversed)
                                {
                                    message.Info = Resources.Resources.ErrorCouldNotUpdateSourceStatus;
                                }
                            }
                        }
                    }
                    else
                    {
                        message.Info = Resources.Resources.ErrorSavingJournalEntry;
                    }

                    message.Success = isReversed;

                    if (!message.Success)
                    {
                        transaction.Dispose();
                    }
                    else
                    {
                        try
                        {
                            if (!transaction.IsCompleted)
                                transaction.Complete();
                            if (transaction != null)
                                transaction.Dispose();
                        }
                        catch (Exception)
                        {
                            // TODO: La transacción debe hacer commit, pero el dispose genera error y no permite continuar
                        }
                        message.Code = SaveApplicationJournalEntry(newApplication.Id, newApplication.BranchId, 0, userId, newApplication.ModuleId);

                        if (newApplication.Id > 0)
                            message.SourceCode = newApplication.Id;

                        if (message.Code > 0)
                        {
                            message.Code = newTechnicalTransction;
                            message.Success = true;
                            message.GeneralLedgerSuccess = true;
                            message.Info = Resources.Resources.IntegrationSuccessMessage + " " + newApplication.Id;

                        }
                        else if (message.Code == 0)
                        {
                            message.Success = true;
                            message.Info = Resources.Resources.AccountingIntegrationUnbalanceEntry;
                        }
                        else if (message.Code == -1)
                        {
                            message.Info = Resources.Resources.ErrorApplicationNotFound;
                        }
                        else if (message.Code == -2)
                        {
                            message.Info = Resources.Resources.EntryRecordingError;
                        }
                    }
                }
                catch (BusinessException ex)
                {
                    transaction.Dispose();
                    throw new BusinessException(Resources.Resources.BusinessException);
                }
                return message;
            }
        }

        private int SaveApplicationJournalEntry(int applicationId, int brachId, int salePointId, int userId, int moduleId)
        {
            Models.Imputations.Application application = GetApplicationByApplicationId(applicationId);
            if (application != null)
            {
                DTOs.AccountingParametersDTO accountingParameters = new DTOs.AccountingParametersDTO();
                accountingParameters.AccountingDate = application.AccountingDate;
                accountingParameters.BranchId = brachId;
                accountingParameters.SalePointId = salePointId;
                accountingParameters.Application = application.ToDTO().ToLedgerDTO();
                accountingParameters.UserId = userId;
                accountingParameters.Description = application.Description;
                accountingParameters.Application.BridgeAccountingId =
                    CommonBusiness.GetIntParameter(Enums.AccountingKeys.ACC_BRIDGE_APPLICATION);

                if (application.ModuleId == Convert.ToInt32(ApplicationTypes.Collect))
                    accountingParameters.Application.BridgeAccountingId =
                        CommonBusiness.GetIntParameter(Enums.AccountingKeys.ACC_BRIDGE_COLLECT);

                accountingParameters.ApplicationItems = new List<DTOs.Imputations.ApplicationJournalEntryDTO>();
                // Consultar los movimentos contables
                var premium = GetApplicationPremiumsByApplicationId(accountingParameters.Application.Id).ToDTOs();
                accountingParameters.ApplicationItems.AddRange(DelegateService.accountingApplicationService.GetApplicationAccountsByApplicationId(accountingParameters.Application.Id).ToJournalDTOs());

                accountingParameters.ApplicationItems.AddRange(premium.ToJournalDTOs(moduleId));
                foreach (DTOs.Imputations.ApplicationPremiumDTO applicationPremiumDTO in premium)
                {
                    accountingParameters.ApplicationItems.AddRange(DelegateService.accountingApplicationService.
                        GetApplicationPremiumCommisionsByApplicationPremiumId(applicationPremiumDTO.Id).ToJournalDTOs(moduleId));
                }
                if (application.ModuleId != moduleId)
                {
                    if (moduleId == Convert.ToInt32(ApplicationTypes.Collect))
                        accountingParameters.OriginalGeneralLedger = new DTOs.OriginalGeneralLedgerDTO()
                        {
                            ModuleId = moduleId,
                            BridgeAccountingId = CommonBusiness.GetIntParameter(Enums.AccountingKeys.ACC_BRIDGE_COLLECT),
                            CodePackageRule = Convert.ToString(CommonBusiness.GetIntParameter(Enums.AccountingKeys.ACC_RULE_PACKAGE_COLLECT))
                        };
                }
                string parameters = Newtonsoft.Json.JsonConvert.SerializeObject(accountingParameters);
                return DelegateService.accountingGeneralLedgerApplicationService.SaveApplicationJournalEntry(parameters);
            }
            return -1;
        }

        public List<ApplicationPremium> GetApplicationPremiumsByEndorsementId(int endorsementId)
        {
            ApplicationPremiumItemDAO applicationPremiumItemDAO = new ApplicationPremiumItemDAO();
            return applicationPremiumItemDAO.GetApplicationPremiumsByEndorsementId(endorsementId);
        }

        public Message ReverseJournalEntry(JournalEntryReversionParameters parameters)
        {
            Message message = new Message()
            {
                Success = false
            };

            if (parameters.TechnicalTransaction > 0)
            {
                if (parameters.BranchId > 0)
                {
                    TechnicalTransactionBusiness technicalTransactionBusiness = new TechnicalTransactionBusiness();
                    parameters.NewTechnicalTransaction = technicalTransactionBusiness.GetTechnicalTransaction(parameters.BranchId);
                    parameters.AccountingDate = DelegateService.commonService.GetModuleDateIssue(parameters.ModuleId, DateTime.Now);
                    string strParameters = Newtonsoft.Json.JsonConvert.SerializeObject(parameters);
                    int newJournalEntryId = DelegateService.accountingGeneralLedgerApplicationService.ReverseJournalEntry(strParameters);
                    if (newJournalEntryId > 0)
                    {
                        message.Success = true;
                        message.GeneralLedgerSuccess = true;
                        message.Code = parameters.NewTechnicalTransaction;
                        message.SourceCode = newJournalEntryId;
                        message.Info = Resources.Resources.IntegrationSuccessMessage + " " + parameters.NewTechnicalTransaction;
                    }
                    else
                        message.Info = Resources.Resources.EntryRecordingError;
                }
                else
                    message.Info = Resources.Resources.InvalidBranchId;
            }
            else
                message.Info = Resources.Resources.InvalidTechnicalTransaction;
            return message;
        }
    }
}