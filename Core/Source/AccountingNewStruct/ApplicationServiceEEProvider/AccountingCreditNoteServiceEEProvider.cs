using Sistran.Core.Application.AccountingServices.Assemblers;
using Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.AccountingServices.DTOs.CreditNotes;
using Sistran.Core.Application.AccountingServices.DTOs.Search;
using Sistran.Core.Application.AccountingServices.EEProvider.DAOs;
using Sistran.Core.Application.Accounting.Entities;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Collect;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.CreditNotes;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Underwriting;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Framework.BAF;
//Sistran FWK
using Sistran.Core.Framework.Contexts;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Views;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CommonModels = Sistran.Core.Application.CommonService.Models;
using SearchDTO = Sistran.Core.Application.AccountingServices.DTOs.Search;
using UTILHELPER = Sistran.Core.Application.Utilities.Helper;
using UniquePersonModels = Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.AccountingServices.Enums;
using Sistran.Core.Application.AccountingServices.EEProvider.Enums;

namespace Sistran.Core.Application.AccountingServices.EEProvider
{
    public class AccountingCreditNoteServiceEEProvider : IAccountingCreditNoteService
    {
        #region Constants
        private const int ModuleId = 2;
        // Tipo de proceso utilizado para notas de crédito
        private const int ProcessTypeId = 1;

        #endregion

        #region Instance Variables

        #region Interfaz

        /// <summary>
        /// Declaración del contexto y del dataFacadeManager
        /// </summary>
        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;
        #endregion Interfaz

        #region DAOs

        readonly LogSpecialProcessDAO _logSpecialProcessDAO = new LogSpecialProcessDAO();
        //readonly TempImputationDAO _tempImputationDAO = new TempImputationDAO();
        readonly DAOs.Accounting.TempApplicationDAO tempApplicationDAO = new DAOs.Accounting.TempApplicationDAO();
        readonly TempPremiumReceivableTransactionDAO _tempPremiumReceivableTransactionDAO = new TempPremiumReceivableTransactionDAO();
        readonly TempPremiumReceivableTransactionItemDAO _tempPremiumReceivableTransactionItemDAO = new TempPremiumReceivableTransactionItemDAO();
        readonly TempJournalEntryDAO _tempJournalEntryDAO = new TempJournalEntryDAO();
        //readonly ImputationDAO _imputationDAO = new ImputationDAO();
        readonly ApplicationDAO applicationDAO = new ApplicationDAO();

        #endregion DAOs

        #endregion Instance Viarables

        #region Public Methods

        #endregion Public Methods

        #region CreditNotes

        /// <summary>
        /// SaveCreditNote
        /// Ejecuta el proceso de cruce de notas de crédito - vs. + y los deja en temporales de primas por cobrar
        /// </summary>
        /// <param name="operationType"></param>
        /// <param name="branch"></param>
        /// <param name="prefix"></param>
        /// <param name="policy"></param>
        /// <param name="insured"></param>
        /// <returns>CreditNote</returns>
        public CreditNoteDTO SaveCreditNote(int operationType, BranchDTO branchDTO,
                                         PrefixDTO prefixDTO, PolicyDTO policyDTO, IndividualDTO insuredDTO)
        {

            CommonModels.Branch branch = branchDTO.ToModel();
            CommonModels.Prefix prefix = prefixDTO.ToModel();
            Policy policy = policyDTO.ToModel();
            Individual insured = insuredDTO.ToModel();
            CreditNote creditNote = new CreditNote();

            creditNote.CreditNoteStatus = CreditNoteStatus.Actived;

            try
            {
                int indexNegative = 0;
                int processId = 0;
                int recordsProcessed = 0;
                decimal endorsementNegativeAmount = 0;
                decimal valueToApplyPositive = 0;
                decimal valueToApplyNegative = 0;
                decimal totalToApplyPositive = 0;
                decimal totalToApplyNegative = 0;


                // Busca todos los endosos negativos de Notas de Crédito 
                List<SearchDTO.PremiumReceivableSearchPolicyDTO> endorsementNegatives = GetCreditNoteAutomaticProcess(operationType,
                                                                        branch.Id, prefix.Id, policy, insured.IndividualId, true);

                // Busca todos los endosos positivos de Notas de Crédito 
                List<SearchDTO.PremiumReceivableSearchPolicyDTO> endorsementPositives = GetCreditNoteAutomaticProcess(operationType,
                                                                        branch.Id, prefix.Id, policy, insured.IndividualId, false);

                Models.Imputations.Application tempImputation;

                // Crea un temporal de asiento contable vacío
                #region JournalEntry

                Models.Imputations.JournalEntry journalEntry = new Models.Imputations.JournalEntry();

                DateTime accountingDate = new AccountingParameterServiceEEProvider().GetAccountingDate(Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_MODULE_DATE_ACCOUNTING)));

                journalEntry.Id = 0;
                journalEntry.AccountingDate = accountingDate;
                journalEntry.Branch = new CommonModels.Branch();
                journalEntry.Branch.Id = branch.Id;
                journalEntry.Comments = "";
                journalEntry.Company = new UniquePersonModels.Company() { IndividualId = 1 }; //DEFAULT EN 3G
                journalEntry.Description = "CRUCE AUTOMATICO NOTAS DE CREDITO";
                journalEntry.Payer = new Individual() { IndividualId = 0, FullName = "" };
                journalEntry.PersonType = new UniquePersonModels.PersonType() { Id = 0 };
                journalEntry.SalePoint = new CommonModels.SalePoint() { Id = 0 };
                journalEntry.Status = 0;

                Models.Imputations.JournalEntry tempJournalEntry = DelegateService.accountingApplicationService.SaveTempJournalEntry(journalEntry.ToDTO()).ToModel();

                #endregion

                //CREA UN TEMPORAL DE IMPUTACION VACIO 
                #region Imputation

                Models.Imputations.Application application = new Models.Imputations.Application();

                int imputationId = 0;
                DateTime registerDate = DateTime.Now;

                application.Id = imputationId;
                application.RegisterDate = registerDate;
                application.ModuleId = (int)AccountingServices.Enums.ApplicationTypes.JournalEntry;
                application.UserId = policy.UserId;

                tempImputation = DelegateService.accountingApplicationService.SaveTempApplication(application.ToDTO(), tempJournalEntry.Id).ToModel();

                #endregion

                //ARMA CABECERA DE LOG
                #region LogHeader

                creditNote.Id = 0;
                creditNote.Date = DateTime.Now;
                creditNote.UserId = policy.UserId;
                creditNote.CreditNoteItems = new List<CreditNoteItem>();
                CreditNoteItem creditNoteItem = new CreditNoteItem();
                creditNoteItem.NegativePolicy = new Policy();
                creditNoteItem.NegativePolicy.Branch = new CommonModels.Branch();
                creditNoteItem.NegativePolicy.Branch.Id = branch.Id;
                creditNote.CreditNoteItems.Add(creditNoteItem);
                creditNote.PositiveAppliedTotal = new CommonModels.Amount();
                creditNote.NegativeAppliedTotal = new CommonModels.Amount();
                creditNote.CreditNoteStatus = CreditNoteStatus.Actived;

                // Graba cabecera log
                processId = _logSpecialProcessDAO.SaveLogSpecialProcess(creditNote,
                                                                        ProcessTypeId,
                                                                        tempImputation.Id);

                #endregion


                if (endorsementNegatives.Count > 0 && endorsementPositives.Count > 0)
                {
                    // Recorre negativos
                    for (indexNegative = 0; indexNegative < endorsementNegatives.Count; indexNegative++)
                    {
                        SearchDTO.PremiumReceivableSearchPolicyDTO premiumReceivableNegative = new SearchDTO.PremiumReceivableSearchPolicyDTO();

                        premiumReceivableNegative.PolicyId = endorsementNegatives[indexNegative].PolicyId;
                        premiumReceivableNegative.EndorsementId = endorsementNegatives[indexNegative].EndorsementId;
                        premiumReceivableNegative.PayerId = endorsementNegatives[indexNegative].PayerId;
                        premiumReceivableNegative.CurrencyId = endorsementNegatives[indexNegative].CurrencyId;
                        premiumReceivableNegative.PaymentNumber = endorsementNegatives[indexNegative].PaymentNumber;
                        premiumReceivableNegative.Amount = endorsementNegatives[indexNegative].Amount;

                        SearchDTO.PremiumReceivableSearchPolicyDTO premiumReceivablePositive = new SearchDTO.PremiumReceivableSearchPolicyDTO();

                        endorsementNegativeAmount = endorsementNegatives[indexNegative].PaymentAmount * -1;

                        valueToApplyPositive = 0;
                        valueToApplyNegative = 0;
                        decimal remaining = 0;

                        int k = 0;
                        // Debe recorrer mientras haya saldo
                        for (k = 0; k < endorsementPositives.Count; k++)
                        {
                            decimal endorsementPositiveAmount = endorsementPositives[k].PaymentAmount;

                            // Procesa solo los de la misma póliza
                            if (endorsementNegatives[indexNegative].PolicyId == endorsementPositives[k].PolicyId)
                            {

                                if (endorsementPositiveAmount > 0)// Descarta los ya comparados
                                {
                                    if (endorsementPositiveAmount > endorsementNegativeAmount)
                                    {
                                        valueToApplyPositive = endorsementNegativeAmount;
                                        valueToApplyNegative = valueToApplyPositive * -1;

                                        remaining = endorsementPositiveAmount - valueToApplyPositive;
                                        endorsementPositives[k].PaymentAmount = endorsementPositives[k].PaymentAmount - valueToApplyPositive;

                                        break;
                                    }
                                    else
                                    {
                                        valueToApplyPositive = endorsementPositives[k].PaymentAmount;
                                        remaining = endorsementNegativeAmount - valueToApplyPositive; // Es el sobrante del negativo
                                        endorsementPositives[k].PaymentAmount = 0;
                                        endorsementNegativeAmount = endorsementNegativeAmount - valueToApplyPositive;
                                        valueToApplyNegative = valueToApplyPositive * -1;


                                        // Graba valor (-)
                                        premiumReceivableNegative.PaidAmount = valueToApplyNegative;

                                        SaveTempPremiumReceivableTransaction(premiumReceivableNegative, tempImputation.Id,
                                                endorsementNegatives[indexNegative].ExchangeRate, policy.UserId);

                                        // Graba valor (+)
                                        // Grabar la parte que alcanza
                                        premiumReceivablePositive.PolicyId = endorsementPositives[k].PolicyId;
                                        premiumReceivablePositive.EndorsementId = endorsementPositives[k].EndorsementId;
                                        premiumReceivablePositive.PayerId = endorsementPositives[k].PayerId;
                                        premiumReceivablePositive.CurrencyId = endorsementPositives[k].CurrencyId;
                                        premiumReceivablePositive.PaymentNumber = endorsementPositives[k].PaymentNumber;
                                        premiumReceivablePositive.Amount = endorsementPositives[k].Amount;
                                        premiumReceivablePositive.PaidAmount = valueToApplyPositive; // Lo que se recibe

                                        SaveTempPremiumReceivableTransaction(premiumReceivablePositive, tempImputation.Id,
                                            endorsementNegatives[indexNegative].ExchangeRate, policy.UserId);

                                        totalToApplyPositive = totalToApplyPositive + valueToApplyPositive;
                                        totalToApplyNegative = totalToApplyNegative + valueToApplyNegative;

                                        if (remaining == 0)
                                        {
                                            break;
                                        }
                                    }
                                }
                            }

                            if (k == endorsementPositives.Count - 1) // Ya no hay + pólizas para comparar
                            {
                                remaining = 0;
                            }
                        }

                        if (remaining > 0)// Sobró del positivo
                        {
                            // Graba valor (-)
                            premiumReceivableNegative.PaidAmount = valueToApplyNegative;

                            SaveTempPremiumReceivableTransaction(premiumReceivableNegative, tempImputation.Id,
                                    endorsementNegatives[indexNegative].ExchangeRate, policy.UserId);

                            // Llamar a función de grabar temporal
                            // Grabar la parte que alcanza
                            premiumReceivablePositive.PolicyId = endorsementPositives[k].PolicyId;
                            premiumReceivablePositive.EndorsementId = endorsementPositives[k].EndorsementId;
                            premiumReceivablePositive.PayerId = endorsementPositives[k].PayerId;
                            premiumReceivablePositive.CurrencyId = endorsementPositives[k].CurrencyId;
                            premiumReceivablePositive.PaymentNumber = endorsementPositives[k].PaymentNumber;
                            premiumReceivablePositive.Amount = endorsementPositives[k].Amount; // Cuota original
                            premiumReceivablePositive.PaidAmount = valueToApplyPositive; // Lo que se recibe

                            // Graba valor (+)
                            SaveTempPremiumReceivableTransaction(premiumReceivablePositive, tempImputation.Id,
                                endorsementNegatives[indexNegative].ExchangeRate, policy.UserId);

                            totalToApplyPositive = totalToApplyPositive + valueToApplyPositive;
                            totalToApplyNegative = totalToApplyNegative + valueToApplyNegative;
                        }

                        recordsProcessed++;

                        UpdateLogSpecialProcess(processId, totalToApplyPositive,
                                                endorsementNegatives[indexNegative].ExchangeRate, recordsProcessed,
                                                endorsementNegatives.Count);
                    }
                }
                else
                {
                    creditNote.Id = processId;
                    creditNote.CreditNoteStatus = CreditNoteStatus.NoData;

                    _logSpecialProcessDAO.UpdateLogSpecialProcess(creditNote, 0, 0, DateTime.Now, 0, 0);
                }
            }
            catch (BusinessException exception)
            {
                creditNote.Id = -1;
                creditNote.CreditNoteItems = new List<CreditNoteItem>() { new CreditNoteItem() { NegativePolicy = new Policy() { TemporalTypeDescription = exception.Message } } };
            }

            return creditNote.ToDTO();
        }

        /// <summary>
        /// GetCreditNotes
        /// Obtiene los procesos de las notas de crédito
        /// </summary>
        /// <returns>List<CreditNote/></returns>
        public List<CreditNoteDTO> GetCreditNotes()
        {
            try
            {
                return _logSpecialProcessDAO.GetLogSpecialProcess().ToDTOs().ToList();
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// GetCreditNote
        /// </summary>
        /// <param name="creditNote"></param>
        /// <returns>CreditNote</returns>
        public CreditNoteDTO GetCreditNote(CreditNoteDTO creditNoteDTO)
        {
            try
            {
                CreditNote creditNote = creditNoteDTO.ToModel();
                bool condition = false;
                UIView processLogCreditNotes;
                // Filtro
                if (creditNote.CreditNoteStatus == CreditNoteStatus.Applied)
                {
                    ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                    criteriaBuilder.PropertyEquals(AppliedCreditNote.Properties.LogSpecialProcessId, creditNote.Id);

                    processLogCreditNotes = _dataFacadeManager.GetDataFacade().GetView("AppliedCreditNoteView",
                                        criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);
                    condition = true;
                }
                else
                {
                    ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                    criteriaBuilder.PropertyEquals(LogProcessCreditNote.Properties.LogSpecialProcessId, creditNote.Id);

                    processLogCreditNotes = _dataFacadeManager.GetDataFacade().GetView("LogProcessCreditNoteView",
                                        criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);
                }

                decimal valuePositive = 0;
                decimal valueNegative = 0;
                int userId = creditNote.UserId;

                creditNote = new CreditNote();
                creditNote.UserId = userId;
                creditNote.CreditNoteItems = new List<CreditNoteItem>();
                creditNote.NegativeAppliedTotal = new CommonModels.Amount();
                creditNote.PositiveAppliedTotal = new CommonModels.Amount();
                creditNote.Date = DateTime.Now;

                CreditNoteStatus creditNoteStatus = CreditNoteStatus.Actived;
                if (condition)
                {
                    creditNoteStatus = CreditNoteStatus.Applied;
                }
                else
                {
                    creditNote.CreditNoteStatus = CreditNoteStatus.Actived;
                }

                foreach (DataRow processLog in processLogCreditNotes.Rows)
                {
                    CreditNoteItem creditNoteItem = new CreditNoteItem();
                    if (condition)
                    {
                        creditNote.Id = Convert.ToInt32(processLog["ImputationCode"]);
                        creditNoteItem.Id = Convert.ToInt32(processLog["PremiumReceivableTransCode"]);
                        creditNote.CreditNoteStatus = CreditNoteStatus.Applied;
                    }
                    else
                    {
                        creditNote.Id = Convert.ToInt32(processLog["TempImputationCode"]);
                        creditNoteItem.Id = Convert.ToInt32(processLog["TempPremiumReceivableTransCode"]);
                        creditNote.CreditNoteStatus = CreditNoteStatus.Actived;
                    }

                    Policy policy = new Policy();

                    creditNote.Id = Convert.ToInt32(processLog["LogSpecialProcessId"]);
                    policy.Branch = new CommonModels.Branch();
                    policy.Branch.Id = Convert.ToInt32(processLog["BranchCode"]);
                    policy.Branch.Description = DelegateService.commonService.GetBranchById(policy.Branch.Id).Description;


                    policy.Prefix = new CommonModels.Prefix()
                    {
                        Id = Convert.ToInt32(processLog["PrefixCode"]),
                        Description = DelegateService.commonService.GetPrefixById(Convert.ToInt32(processLog["PrefixCode"])).Description

                    };

                    policy.Id = Convert.ToInt32(processLog["PolicyId"]);
                    policy.DocumentNumber = Convert.ToInt32(processLog["PolicyNumber"]);
                    policy.Endorsement = new Endorsement()
                    {
                        Id = Convert.ToInt32(processLog["EndorsementId"]),
                        Number = Convert.ToInt32(processLog["EndorsementNumber"])
                    };

                    policy.Endorsement.EndorsementType = Enums.EndorsementType.AdjustmentEndorsement;

                    policy.DefaultBeneficiaries = new List<Beneficiary>()
                    {
                        new Beneficiary()
                        {
                            IndividualId = Convert.ToInt32(processLog["PayerId"]),
                            Name = processLog["PayerName"].ToString(),
                            CustomerType = Services.UtilitiesServices.Enums.CustomerType.Individual
                        }
                    };

                    policy.PayerComponents = new List<PayerComponent>()
                    {
                        new PayerComponent()
                        {
                            Amount =  Convert.ToDecimal(processLog["Amount"]),
                            BaseAmount = Convert.ToDecimal(processLog["IncomeAmount"]),

                            Coverage = new Coverage()
                            {
                                EndorsementType = Enums.EndorsementType.Emission,
                                CoverStatus = Enums.CoverageStatusType.Original,
                                CoverageOriginalStatus = Enums.CoverageStatusType.Original,
                                FirstRiskType = Enums.FirstRiskType.None,
                                RateType = Services.UtilitiesServices.Enums.RateType.Percentage,
                                CalculationType =  Services.UtilitiesServices.Enums.CalculationType.Prorate,
                                InsuredObject = new InsuredObject()
                                {
                                    ParametrizationStatus = Services.UtilitiesServices.Enums.ParametrizationStatus.Original
                                },
                            },
                            Component = new Component()
                            {
                                ComponentType = ComponentType.Premium,
                                ComponentClass = ComponentClassType.HardComponent,
                            }
                        }
                    };
                    policy.ExchangeRate = new CommonModels.ExchangeRate()
                    {
                        BuyAmount = Convert.ToDecimal(processLog["ExchangeRate"]),
                        Currency = new CommonModels.Currency()
                        {
                            Description = processLog["Description"].ToString(),
                            Id = Convert.ToInt32(processLog["CurrencyCode"])
                        }
                    };
                    policy.Agencies = new List<IssuanceAgency>()
                    {
                        new IssuanceAgency()
                        {
                            Agent = new IssuanceAgent()
                            {
                                IndividualId = Convert.ToInt32(processLog["AgentId"]),
                                FullName = processLog["AgentName"].ToString()
                            }
                        }
                    };

                    policy.Holder = new Holder()
                    {
                        IndividualId = Convert.ToInt32(processLog["InsuredId"]),
                        Name = processLog["InsuredName"].ToString(),
                        IndividualType = Services.UtilitiesServices.Enums.IndividualType.Company,
                        CustomerType = Services.UtilitiesServices.Enums.CustomerType.Individual,
                    };

                    creditNoteItem.NegativePolicy = new Policy();
                    creditNoteItem.NegativePolicy = policy;

                    creditNoteItem.PositivePolicy = new Policy();


                    if (Convert.ToDecimal(processLog["IncomeAmount"]) > 0)
                    {
                        valuePositive = valuePositive + Convert.ToDecimal(processLog["IncomeAmount"]);
                    }
                    else
                    {
                        valueNegative = valueNegative + (Convert.ToDecimal(processLog["IncomeAmount"]) * -1);
                    }

                    if (condition)
                    {
                        //Si integración con Contabilidad fue exitosa estos 2 campos deben existir en la consulta
                        creditNote.Date = processLog["ApplyDate"] == DBNull.Value ? new DateTime() : Convert.ToDateTime(processLog["ApplyDate"]);
                        creditNoteItem.PositivePolicy.Id = processLog["AccountingTransaction"] == DBNull.Value ? 0 : Convert.ToInt32(processLog["AccountingTransaction"]);
                    }
                    else
                    {
                        creditNote.Date = Convert.ToDateTime(processLog["ProcessDate"]);
                    }
                    creditNote.CreditNoteItems.Add(creditNoteItem);
                }

                creditNote.NegativeAppliedTotal = new CommonModels.Amount();
                creditNote.NegativeAppliedTotal.Value = valueNegative;

                creditNote.PositiveAppliedTotal = new CommonModels.Amount();
                creditNote.PositiveAppliedTotal.Value = valuePositive;
                creditNote.CreditNoteStatus = creditNoteStatus;

                List<CreditNoteItem> creditNotesItems = (from item in creditNote.CreditNoteItems
                                                         orderby item.NegativePolicy.Id, item.NegativePolicy.DocumentNumber
                                                         select item).ToList();

                creditNote.CreditNoteItems = new List<CreditNoteItem>();
                creditNote.CreditNoteItems = creditNotesItems;

                return creditNote.ToDTO();

            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }


        /// <summary>
        /// GetCreditNoteApplied
        /// </summary>
        /// <param name="creditNote"></param>
        /// <returns>CreditNote</returns>
        public CreditNote GetCreditNoteApplied(CreditNote creditNote)
        {
            try
            {


                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(AppliedCreditNote.Properties.LogSpecialProcessId, creditNote.Id);


                UIView processLogCreditNotes = _dataFacadeManager.GetDataFacade().GetView("AppliedCreditNoteView",
                                    criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);

                decimal valuePositive = 0;
                decimal valueNegative = 0;
                int userId = creditNote.UserId;

                creditNote = new CreditNote();
                creditNote.UserId = userId;
                creditNote.CreditNoteItems = new List<CreditNoteItem>();
                creditNote.NegativeAppliedTotal = new CommonModels.Amount();
                creditNote.PositiveAppliedTotal = new CommonModels.Amount();
                creditNote.Date = DateTime.Now;
                creditNote.CreditNoteStatus = CreditNoteStatus.Applied;
                CreditNoteStatus creditNoteStatus = CreditNoteStatus.Applied;

                if (processLogCreditNotes.Count > 0)
                {
                    foreach (DataRow processLog in processLogCreditNotes.Rows)
                    {
                        creditNote.Id = Convert.ToInt32(processLog["ImputationCode"]);

                        CreditNoteItem creditNoteItem = new CreditNoteItem();

                        creditNoteItem.Id = Convert.ToInt32(processLog["PremiumReceivableTransCode"]);
                        creditNote.CreditNoteStatus = CreditNoteStatus.Actived;

                        Policy policy = new Policy();

                        creditNote.Id = Convert.ToInt32(processLog["LogSpecialProcessId"]);
                        policy.Branch = new CommonModels.Branch();

                        policy.Branch.Id = Convert.ToInt32(processLog["BranchCode"]);
                        policy.Branch.Description = DelegateService.commonService.GetBranchById(policy.Branch.Id).Description;
                        policy.Prefix = new CommonModels.Prefix();
                        policy.Prefix.Id = Convert.ToInt32(processLog["PrefixCode"]);
                        policy.Prefix.Description = DelegateService.commonService.GetPrefixById(policy.Prefix.Id).Description;

                        policy.Id = Convert.ToInt32(processLog["PolicyId"]);
                        policy.DocumentNumber = Convert.ToInt32(processLog["PolicyNumber"]);
                        policy.Endorsement = new Endorsement()
                        {
                            Id = Convert.ToInt32(processLog["EndorsementId"]),
                            Number = Convert.ToInt32(processLog["EndorsementNumber"])
                        };
                        policy.DefaultBeneficiaries = new List<Beneficiary>()
                    {
                        new Beneficiary()
                        {
                            IndividualId = Convert.ToInt32(processLog["PayerId"]),
                            Name = processLog["PayerName"].ToString()
                        }
                    };

                        policy.PayerComponents = new List<PayerComponent>()
                    {
                        new PayerComponent()
                        {
                            Amount = Convert.ToDecimal(processLog["Amount"]),
                            BaseAmount = Convert.ToDecimal(processLog["IncomeAmount"])
                        }
                    };

                        policy.ExchangeRate = new CommonModels.ExchangeRate()
                        {
                            BuyAmount = Convert.ToDecimal(processLog["ExchangeRate"]),
                            Currency = new CommonModels.Currency()
                            {
                                Description = processLog["Description"].ToString(),
                                Id = Convert.ToInt32(processLog["CurrencyCode"])
                            }
                        };

                        policy.Agencies = new List<IssuanceAgency>()
                    {
                        new IssuanceAgency()
                        {
                            Agent = new IssuanceAgent()
                            {
                                IndividualId = Convert.ToInt32(processLog["AgentId"]),
                                FullName = processLog["AgentName"].ToString()
                            }
                        }
                    };

                        policy.Holder = new Holder()
                        {
                            IndividualId = Convert.ToInt32(processLog["InsuredId"]),
                            Name = processLog["InsuredName"].ToString()
                        };

                        creditNoteItem.NegativePolicy = new Policy();
                        creditNoteItem.NegativePolicy = policy;
                        creditNote.CreditNoteItems.Add(creditNoteItem);

                        if (Convert.ToDecimal(processLog["IncomeAmount"]) > 0)
                        {
                            valuePositive = valuePositive + Convert.ToDecimal(processLog["IncomeAmount"]);
                        }
                        else
                        {
                            valueNegative = valueNegative + (Convert.ToDecimal(processLog["IncomeAmount"]) * -1);
                        }

                        creditNote.Date = Convert.ToDateTime(processLog["ProcessDate"]);
                    }

                    creditNote.NegativeAppliedTotal = new CommonModels.Amount();
                    creditNote.NegativeAppliedTotal.Value = valueNegative;

                    creditNote.PositiveAppliedTotal = new CommonModels.Amount();
                    creditNote.PositiveAppliedTotal.Value = valuePositive;
                    creditNote.CreditNoteStatus = creditNoteStatus;

                    List<CreditNoteItem> creditNotesItems = (from item in creditNote.CreditNoteItems
                                                             orderby item.NegativePolicy.Id, item.NegativePolicy.DocumentNumber
                                                             select item).ToList();

                    creditNote.CreditNoteItems = new List<CreditNoteItem>();
                    creditNote.CreditNoteItems = creditNotesItems;
                }

                return creditNote;

            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }


        /// <summary>
        /// UpdateCreditNote
        /// Actualiza la nota de crédito
        /// </summary>
        /// <param name="creditNote"></param>
        /// <returns>CreditNote</returns>
        public CreditNoteDTO UpdateCreditNote(CreditNoteDTO creditNoteDTO)
        {
            CreditNote creditNote = creditNoteDTO.ToModel();
            try
            {

                // Obtener el temporal de imputación
                List<CreditNote> processLogs = _logSpecialProcessDAO.GetLogSpecialProcess();

                List<CreditNote> creditNotes = processLogs.Where(sl => sl.Id == creditNote.Id).ToList();

                int tempImputationId = creditNotes[0].CreditNoteItems[0].NegativePolicy.Id;

                PremiumReceivableTransaction premiumReceivableTransaction;

                // Recupera id de asiento diario
                Models.Imputations.Application tempImputation = new Models.Imputations.Application();
                tempImputation.Id = tempImputationId;
                tempImputation = tempApplicationDAO.GetTempApplication(tempImputation);

                // Proceso de baja: cambia estado cabecera y borra temporales
                if (creditNote.CreditNoteStatus == CreditNoteStatus.Rejected)
                {
                    // Recuperar los temporales de primas por cobrar por la temporal de imputacion
                    premiumReceivableTransaction =
                        _tempPremiumReceivableTransactionDAO.GetTempPremiumRecievableTransactionByTempImputationId(
                                                                  tempImputationId, (int)ImputationItemTypes.PremiumsReceivable);

                    foreach (PremiumReceivableTransactionItem premiumReceivableTransactionItem in
                                                                    premiumReceivableTransaction.PremiumReceivableItems)
                    {
                        _tempPremiumReceivableTransactionItemDAO.DeleteTempPremiumRecievableTransactionItem(
                                                                     premiumReceivableTransactionItem.Id);
                    }

                    if (tempImputation.ApplicationItems != null)
                    {
                        // Borra journal entry
                        _tempJournalEntryDAO.DeleteTempJournalEntry(tempImputation.ApplicationItems[0].Id);
                    }

                    // Borra temp imputation
                    tempApplicationDAO.DeleteTempApplication(tempImputationId);

                    // Cambia estado de la cabecera
                    _logSpecialProcessDAO.UpdateLogSpecialProcess(creditNote, 0, 0, DateTime.Now, 0, 0);

                    creditNote.Id = 0;
                }
                // Eliminar: solo borra temporales seleccionados
                if (creditNote.CreditNoteStatus == CreditNoteStatus.Actived)
                {
                    // Recuperar los temporales de primas por cobrar por la temporal de imputación
                    premiumReceivableTransaction =
                           _tempPremiumReceivableTransactionDAO.GetTempPremiumRecievableTransactionByTempImputationId(
                           tempImputationId, (int)ImputationItemTypes.PremiumsReceivable);

                    List<PremiumReceivableTransactionItem> premiums =
                        premiumReceivableTransaction.PremiumReceivableItems.Where(sl => sl.Id == creditNote.CreditNoteItems[0].Id).ToList();

                    if (premiums.Count > 0)
                    {
                        CommonModels.Amount discountAmount = new CommonModels.Amount() { Value = premiums[0].Policy.PayerComponents[0].BaseAmount };

                        creditNote.NegativeAppliedTotal = new CommonModels.Amount();
                        creditNote.NegativeAppliedTotal = discountAmount;

                        if (creditNote.NegativeAppliedTotal.Value > 0)
                        {
                            // Resta saldo
                            _logSpecialProcessDAO.UpdateLogSpecialProcess(creditNote, 0, 0, DateTime.Now, 0, 0);
                        }

                        _tempPremiumReceivableTransactionItemDAO.DeleteTempPremiumRecievableTransactionItem(creditNote.CreditNoteItems[0].Id);

                        // Recuperar los temporales de primas por cobrar por la temporal de imputación
                        premiumReceivableTransaction = _tempPremiumReceivableTransactionDAO.GetTempPremiumRecievableTransactionByTempImputationId(tempImputationId, (int)ImputationItemTypes.PremiumsReceivable);

                    }

                    // Si selecciona la totalidad de los registros borra también el asiento y la imputación temporales
                    if (premiumReceivableTransaction.PremiumReceivableItems.Count == 0)
                    {
                        if (tempImputation.ApplicationItems != null)
                        {
                            // Borra journal entry
                            _tempJournalEntryDAO.DeleteTempJournalEntry(tempImputation.ApplicationItems[0].Id);
                        }

                        // Borra temp imputation
                        tempApplicationDAO.DeleteTempApplication(tempImputationId);

                        creditNote.CreditNoteStatus = CreditNoteStatus.Rejected;

                        // Cambia estado de la cabecera
                        _logSpecialProcessDAO.UpdateLogSpecialProcess(creditNote, 0, 0, DateTime.Now, 0, 0);
                    }
                    creditNote.Id = 0;
                }
                // Aplicar: 
                if (creditNote.CreditNoteStatus == CreditNoteStatus.Applied)
                {
                    int newJournalEntry;
                    CollectApplication collectImputation = DelegateService.accountingApplicationService.SaveJournalEntryImputation(tempImputation.ApplicationItems[0].Id, tempImputationId, creditNote.UserId).ToModel();

                    newJournalEntry = collectImputation.Transaction.Id;

                    creditNote.CreditNoteStatus = CreditNoteStatus.Applied;

                    int imputationId = 0;

                    using (Context context = new Context())
                    {
                        imputationId = applicationDAO.GetImputationBySourceType(newJournalEntry, (int)ApplicationTypes.JournalEntry);

                        // Cambia estado de la cabecera               
                        //La imputación de Asiento de Diario ya no genera un recibo (Collect), se debe registrar TechnicalTransaction
                        _logSpecialProcessDAO.UpdateLogSpecialProcess(creditNote, 0, 0, DateTime.Now, imputationId, collectImputation.Transaction.TechnicalTransaction);

                        creditNote.Id = newJournalEntry; //este Id es necesario para llamar a la Contabilización

                        CreditNoteItem creditNoteItem = new CreditNoteItem();
                        creditNoteItem.Id = collectImputation.Transaction.TechnicalTransaction;
                        creditNote.CreditNoteItems.Add(creditNoteItem);

                    }
                }
            }

            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }

            return creditNote.ToDTO();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// UpdateLogSpecialProcess
        /// Actualiza el proceso de cruce de notas de crédito
        /// </summary>
        /// <param name="processId"></param>
        /// <param name="valuePos"></param>
        /// <param name="exchangeRate"></param>
        /// <param name="recordsProcessed"></param>
        /// <param name="totalRecords"></param>
        private void UpdateLogSpecialProcess(int processId, decimal valuePos, decimal exchangeRate,
                                             int recordsProcessed, int totalRecords)
        {
            try
            {
                CreditNote creditNote = new CreditNote();

                creditNote.Id = processId;

                creditNote.PositiveAppliedTotal = new CommonModels.Amount();
                creditNote.PositiveAppliedTotal.Value = valuePos;

                creditNote.NegativeAppliedTotal = new CommonModels.Amount();
                creditNote.NegativeAppliedTotal.Value = 0;

                creditNote.CreditNoteStatus = CreditNoteStatus.Actived;

                // Registra proceso del proceso
                if (recordsProcessed == totalRecords)
                {
                    _logSpecialProcessDAO.UpdateLogSpecialProcess(creditNote,
                                              exchangeRate, recordsProcessed, DateTime.Now, 0, 0);
                }
                else
                {
                    _logSpecialProcessDAO.UpdateLogSpecialProcess(creditNote,
                                              exchangeRate, recordsProcessed, null, 0, 0);
                }
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// SaveTempPremiumReceivableTransaction
        /// Graba las primas por cobrar 
        /// </summary>
        /// <param name="premiumReceivable"></param>
        /// <param name="tempImputationId"></param>
        /// <param name="exchangeRate"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private void SaveTempPremiumReceivableTransaction(SearchDTO.PremiumReceivableSearchPolicyDTO premiumReceivable,
                                                          int tempImputationId, decimal exchangeRate, int userId)
        {
            try
            {
                PremiumReceivableTransaction premiumReceivableTransaction = SetPremiumReceivable(premiumReceivable);
                DateTime accountingDate = DelegateService.commonService.GetModuleDateIssue(ModuleId, DateTime.Now);
                DelegateService.accountingApplicationService.SaveTempPremiumRecievableTransaction(premiumReceivableTransaction.ToDTO(),
                                                             tempImputationId, exchangeRate, userId, DateTime.Now, accountingDate);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// SetPremiumReceivable
        /// Setea las primas por cobrar
        /// </summary>
        /// <param name="premiumReceivableSearchPolicyDTO"></param>
        /// <returns>PremiumReceivableTransaction</returns>
        private PremiumReceivableTransaction SetPremiumReceivable(SearchDTO.PremiumReceivableSearchPolicyDTO premiumReceivableSearchPolicyDTO)
        {
            try
            {
                PremiumReceivableTransaction premiumReceivableTransaction = new PremiumReceivableTransaction();

                premiumReceivableTransaction.PremiumReceivableItems = new List<PremiumReceivableTransactionItem>();

                PremiumReceivableTransactionItem premiumReceivableTransactionItem = new PremiumReceivableTransactionItem();

                premiumReceivableTransactionItem.DeductCommission = new CommonModels.Amount() { Value = 0 };

                premiumReceivableTransactionItem.Policy = new Policy();
                premiumReceivableTransactionItem.Policy.Id = premiumReceivableSearchPolicyDTO.PolicyId;
                premiumReceivableTransactionItem.Policy.Endorsement = new Endorsement()
                {
                    Id = premiumReceivableSearchPolicyDTO.EndorsementId
                };
                premiumReceivableTransactionItem.Policy.ExchangeRate = new CommonModels.ExchangeRate()
                {
                    Currency = new CommonModels.Currency() { Id = premiumReceivableSearchPolicyDTO.CurrencyId }
                };

                premiumReceivableTransactionItem.Policy.DefaultBeneficiaries = new List<Beneficiary>()
                {
                    new Beneficiary()
                    {
                        IndividualId = premiumReceivableSearchPolicyDTO.PayerId
                    }
                };

                premiumReceivableTransactionItem.Policy.PayerComponents = new List<PayerComponent>()
                {
                    new PayerComponent()
                    {
                        Amount = premiumReceivableSearchPolicyDTO.Amount,
                        BaseAmount = premiumReceivableSearchPolicyDTO.PaidAmount
                    }
                };

                premiumReceivableTransactionItem.Policy.PaymentPlan = new PaymentPlan()
                {
                    Quotas = new List<Quota>()
                    {
                        new Quota() { Number = premiumReceivableSearchPolicyDTO.PaymentNumber }
                    }
                };

                premiumReceivableTransactionItem.Id = 0;

                premiumReceivableTransaction.PremiumReceivableItems.Add(premiumReceivableTransactionItem);

                return premiumReceivableTransaction;
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        ///<summary>
        /// GetCreditNoteAutomaticProcess
        /// Búsqueda de notas de crédito para el cruce automático
        /// </summary>
        /// <param name="operationType"></param>
        /// <param name="branchId"></param>
        /// <param name="prefixId"></param>
        /// <param name="policy"></param>
        /// <param name="insuredId"></param>
        /// <param name="negative"></param>
        /// <returns>List<PremiumReceivableSearchPolicyDTO/></returns>
        private List<SearchDTO.PremiumReceivableSearchPolicyDTO> GetCreditNoteAutomaticProcess(int operationType, int branchId, int prefixId,
                                                                                     Policy policy, int insuredId, bool negative)
        {
            try
            {
                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(CreditNoteAutomaticProcess.Properties.BusinessTypeCode, operationType);

                if (branchId > 0)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(CreditNoteAutomaticProcess.Properties.BranchCode, branchId);
                }
                if (prefixId > 0)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(CreditNoteAutomaticProcess.Properties.PrefixCode, prefixId);
                }
                if (policy.Id > 0)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(CreditNoteAutomaticProcess.Properties.PolicyId,
                                          policy.Id);
                }
                if (policy.DocumentNumber > 0)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(CreditNoteAutomaticProcess.Properties.PolicyDocumentNumber,
                                          policy.DocumentNumber);
                }
                if (insuredId > 0)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(CreditNoteAutomaticProcess.Properties.InsuredIndividualId,
                                          insuredId);
                }
                if (policy.ExchangeRate.Currency.Id > 0)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(CreditNoteAutomaticProcess.Properties.CurrencyCode,
                                          policy.ExchangeRate.Currency.Id);
                }
                if (negative)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.Property(CreditNoteAutomaticProcess.Properties.PaymentAmount);
                    criteriaBuilder.Less();
                    criteriaBuilder.Constant(0);

                    // 2 Endoso de Modificación   
                    // 3 Cancelacion
                    // 22 Anulación de Anexo

                    criteriaBuilder.And();
                    criteriaBuilder.OpenParenthesis();
                    criteriaBuilder.PropertyEquals(CreditNoteAutomaticProcess.Properties.EndoTypeCode, 2);
                    criteriaBuilder.Or();
                    criteriaBuilder.PropertyEquals(CreditNoteAutomaticProcess.Properties.EndoTypeCode, 3);
                    criteriaBuilder.Or();
                    criteriaBuilder.PropertyEquals(CreditNoteAutomaticProcess.Properties.EndoTypeCode, 22);
                    criteriaBuilder.CloseParenthesis();
                }
                else
                {
                    criteriaBuilder.And();
                    criteriaBuilder.Property(CreditNoteAutomaticProcess.Properties.PaymentAmount);
                    criteriaBuilder.Greater();
                    criteriaBuilder.Constant(0);
                }

                UIView creditNotes = _dataFacadeManager.GetDataFacade().GetView("CreditNoteAutomaticProcessView",
                                    criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);

                List<SearchDTO.PremiumReceivableSearchPolicyDTO> premiumReceivablePolicies = new List<SearchDTO.PremiumReceivableSearchPolicyDTO>();

                foreach (DataRow dataRow in creditNotes)
                {
                    premiumReceivablePolicies.Add(new SearchDTO.PremiumReceivableSearchPolicyDTO()
                    {
                        InsuredId = Convert.ToInt32(dataRow["InsuredIndividualId"]),
                        PolicyId = Convert.ToInt32(dataRow["PolicyId"]),
                        PolicyDocumentNumber = Convert.ToString(dataRow["PolicyDocumentNumber"]),
                        EndorsementId = Convert.ToInt32(dataRow["EndorsementId"]),
                        EndorsementDocumentNumber = dataRow["EndorsementDocumentNumber"] == DBNull.Value ? "" : Convert.ToString(dataRow["EndorsementDocumentNumber"]),
                        PaymentNumber = Convert.ToInt32(dataRow["Quota"]),
                        EndorsementTypeId = Convert.ToInt32(dataRow["EndoTypeCode"]),
                        EndorsementTypeDescription = Convert.ToString(dataRow["EndorsementTypeDescription"]),
                        PaymentAmount = Convert.ToDecimal(dataRow["PaymentAmount"]),
                        Amount = Convert.ToDecimal(dataRow["Amount"]),
                        PaymentExpirationDate = Convert.ToDateTime(dataRow["PaymentExpirationDate"]),
                        BussinessTypeId = Convert.ToInt32(dataRow["BusinessTypeCode"]),
                        BussinessTypeDescription = dataRow["BussinessTypeDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["BussinessTypeDescription"]),
                        BranchId = Convert.ToInt32(dataRow["BranchCode"]),
                        PrefixId = Convert.ToInt32(dataRow["PrefixCode"]),
                        CurrencyId = Convert.ToInt32(dataRow["CurrencyCode"]),
                        ExchangeRate = Convert.ToDecimal(dataRow["ExchangeRate"]),
                        PayerId = Convert.ToInt32(dataRow["PayerIndividualId"])
                    });
                }


                if (negative)
                {
                    // Negativas
                    return new List<SearchDTO.PremiumReceivableSearchPolicyDTO>(from premiums in premiumReceivablePolicies
                                                                                orderby premiums.PolicyId, premiums.PaymentAmount
                                                                                select premiums);
                }
                // Positivas
                return new List<SearchDTO.PremiumReceivableSearchPolicyDTO>(from premiums in premiumReceivablePolicies
                                                                            orderby premiums.PolicyId, premiums.PaymentAmount descending
                                                                            select premiums);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }



        #endregion Private Methods

    }
}
