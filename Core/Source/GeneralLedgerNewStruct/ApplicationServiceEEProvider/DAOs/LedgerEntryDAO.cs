//Sistran Core
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.GeneralLedgerServices.DTOs;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Assemblers;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using GENERALLEDGEREN = Sistran.Core.Application.GeneralLedger.Entities;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Enums;
using Sistran.Core.Application.Utilities.Helper;

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.DAOs
{
    internal class LedgerEntryDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;
        readonly AccountingAccountDAO _accountingAccountDAO = new AccountingAccountDAO();
        readonly AccountingMovementTypeDAO _accountingMovementTypeDAO = new AccountingMovementTypeDAO();
        readonly AnalysisCodeDAO _analysisCodeDAO = new AnalysisCodeDAO();
        readonly AnalysisConceptDAO _analysisConceptDAO = new AnalysisConceptDAO();
        readonly CostCenterDAO _costCenterDAO = new CostCenterDAO();
        readonly DestinationDAO _destinationDAO = new DestinationDAO();
        readonly CostCenterEntryDAO _costCenterEntryDAO = new CostCenterEntryDAO();
        readonly AnalysisDAO _analysisDAO = new AnalysisDAO();
        readonly PostDatedDAO _postDatedDAO = new PostDatedDAO();
        readonly EntryNumberDAO _entryNumberDAO = new EntryNumberDAO();

        #endregion

        #region Public Methods

        #region Save

        /// <summary>
        /// SaveLedgerEntry
        /// </summary>
        /// <param name="ledgerEntry"></param>
        /// <returns></returns>
        public int SaveLedgerEntry(LedgerEntry ledgerEntry)
        {
            try
            {
                // Convertir de model a entity
                GENERALLEDGEREN.LedgerEntry ledgerEntryEntity = EntityAssembler.CreateLedgerEntry(ledgerEntry);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(ledgerEntryEntity);

                // Return del model
                return ledgerEntryEntity.LedgerEntryId;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// SaveLedgerEntryItem
        /// </summary>
        /// <param name="ledgerEntryItem"></param>
        /// <param name="ledgerEntryId"></param>
        /// <param name="isJournalEntry"></param>
        /// <returns></returns>
        public LedgerEntryItem SaveLedgerEntryItem(LedgerEntryItem ledgerEntryItem, int ledgerEntryId, bool isJournalEntry)
        {
            try
            {
                // Convertir de model a entity
                GENERALLEDGEREN.LedgerEntryItem ledgerEntryItemEntity = EntityAssembler.CreateLedgerEntryItem(ledgerEntryItem, ledgerEntryId, isJournalEntry);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(ledgerEntryItemEntity);

                // Return del model
                return ModelAssembler.CreateLedgerEntryItem(ledgerEntryItemEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// SaveLedgerEntryTransaction
        /// </summary>
        /// <param name="ledgerEntry"></param>
        /// <param name="isJournalEntry"></param>
        /// <returns></returns>
        public LedgerEntry SaveLedgerEntryTransaction(LedgerEntry ledgerEntry, bool isJournalEntry= false)
        {            
            try
            {
                // Se obtiene el número de asiento
                EntryNumber entryNumber = new EntryNumber();
                entryNumber.AccountingMovementType = new AccountingMovementType()
                {
                    AccountingMovementTypeId = ledgerEntry.AccountingMovementType.AccountingMovementTypeId
                };
                entryNumber.Date = ledgerEntry.AccountingDate;
                entryNumber.EntryDestination = new EntryDestination()
                {
                    DestinationId = ledgerEntry.EntryDestination.DestinationId
                };
                entryNumber.Id = 0;
                entryNumber.IsJournalEntry = isJournalEntry;
                entryNumber.Number = 0;
                entryNumber.Year = ledgerEntry.AccountingDate.Year;

                entryNumber = _entryNumberDAO.GetLastEntryNumber(entryNumber);

                ledgerEntry.EntryNumber = Convert.ToInt32(entryNumber.Number);
                int ledgerEntryId = SaveLedgerEntry(ledgerEntry);
                ledgerEntry.Id = ledgerEntryId;

                foreach (LedgerEntryItem ledgerEntryItem in ledgerEntry.LedgerEntryItems)
                {
                    // Se graba el asiento
                    LedgerEntryItem newLedgerEntryItem = ledgerEntryItem;
                    newLedgerEntryItem.Id = 0; //es un nuevo registro.
                    newLedgerEntryItem = SaveLedgerEntryItem(newLedgerEntryItem, ledgerEntryId, isJournalEntry);

                    SaveLedgerEntryItemGroup(ledgerEntryItem, newLedgerEntryItem.Id, isJournalEntry);
                }

                // Se actualiza el número de asiento
                EntryNumber newEntryNumber = _entryNumberDAO.GetEntryNumber(Convert.ToInt32(entryNumber.Id));
                newEntryNumber.Number = newEntryNumber.Number + 1;
                _entryNumberDAO.UpdateEntryNumber(newEntryNumber);
				
            }
            catch (BusinessException ex)
            {

                if (ex.Message.Contains("BusinessException"))
                {
                    throw new BusinessException(EnumHelper.GetEnumParameterValue<GeneralLederKeys>(GeneralLederKeys.GL_BUSINESS_EXCEPTION_MSJ).ToString());
                }

                throw new BusinessException(EnumHelper.GetEnumParameterValue<GeneralLederKeys>(GeneralLederKeys.GL_UNHANDLED_EXCEPTION_MSJ).ToString());
            }

            return ledgerEntry;
        }
        
        #endregion

        #region Update

        /// <summary>
        /// UpdateLedgerEntry
        /// </summary>
        /// <param name="ledgerEntry"></param>
        /// <returns></returns>
        public int UpdateLedgerEntry(LedgerEntry ledgerEntry)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.LedgerEntry.CreatePrimaryKey(ledgerEntry.Id);

                // Encuentra el objeto en referencia a la llave primaria
                GENERALLEDGEREN.LedgerEntry ledgerEntryEntity = (GENERALLEDGEREN.LedgerEntry)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                ledgerEntryEntity.AccountingCompanyId = ledgerEntry.AccountingCompany.AccountingCompanyId;
                ledgerEntryEntity.AccountingDate = ledgerEntry.AccountingDate;
                ledgerEntryEntity.AccountingModuleId = ledgerEntry.ModuleDateId;
                ledgerEntryEntity.AccountingMovementTypeId = ledgerEntry.AccountingMovementType.AccountingMovementTypeId;
                ledgerEntryEntity.BranchCode = ledgerEntry.Branch.Id;
                ledgerEntryEntity.Description = ledgerEntry.Description;
                ledgerEntryEntity.EntryDestinationId = ledgerEntry.EntryDestination.DestinationId;
                ledgerEntryEntity.EntryNumber = ledgerEntry.EntryNumber;
                ledgerEntryEntity.RegisterDate = ledgerEntry.RegisterDate;
                ledgerEntryEntity.SalePointCode = ledgerEntry.SalePoint.Id;
                ledgerEntryEntity.UserCode = ledgerEntry.UserId;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(ledgerEntryEntity);

                // Return del model
                return ledgerEntryEntity.LedgerEntryId;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Delete

        /// <summary>
        /// DeleteLedgerEntry
        /// </summary>
        /// <param name="ledgerEntryId"></param>
        public void DeleteLedgerEntry(int ledgerEntryId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.LedgerEntry.CreatePrimaryKey(ledgerEntryId);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.LedgerEntry ledgerEntryEntity = (GENERALLEDGEREN.LedgerEntry)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(ledgerEntryEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteLedgerEntryItem
        /// </summary>
        /// <param name="ledgerEntryItemId"></param>
        public void DeleteLedgerEntryItem(int ledgerEntryItemId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.LedgerEntryItem.CreatePrimaryKey(ledgerEntryItemId);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.LedgerEntryItem ledgerEntryItemEntity = (GENERALLEDGEREN.LedgerEntryItem)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(ledgerEntryItemEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Get

        /// <summary>
        /// GetLedgerEntry
        /// </summary>
        /// <param name="ledgerEntryId"></param>
        /// <returns></returns>
        public LedgerEntry GetLedgerEntry(int ledgerEntryId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.LedgerEntry.CreatePrimaryKey(ledgerEntryId);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.LedgerEntry ledgerEntryEntity = (GENERALLEDGEREN.LedgerEntry)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreateLedgerEntry(ledgerEntryEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetLedgerEntry
        /// </summary>
        /// <param name="ledgerEntryId"></param>
        /// <returns></returns>
        public LedgerEntry GetLedgerEntryById(int ledgerEntryId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.LedgerEntry.CreatePrimaryKey(ledgerEntryId);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.LedgerEntry ledgerEntryEntity = (GENERALLEDGEREN.LedgerEntry)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                LedgerEntry ledgerEntry = new LedgerEntry();
                ledgerEntry.AccountingCompany = new AccountingCompany() { AccountingCompanyId = ledgerEntryEntity.AccountingCompanyId };
                ledgerEntry.AccountingDate = Convert.ToDateTime(ledgerEntryEntity.AccountingDate);
                ledgerEntry.AccountingMovementType = new AccountingMovementType() { AccountingMovementTypeId = ledgerEntryEntity.AccountingMovementTypeId };
                ledgerEntry.Branch = new Branch() { Id = ledgerEntryEntity.BranchCode };
                ledgerEntry.Description = ledgerEntryEntity.Description;
                ledgerEntry.EntryDestination = new EntryDestination() { DestinationId = ledgerEntryEntity.EntryDestinationId };
                ledgerEntry.EntryNumber = Convert.ToInt32(ledgerEntryEntity.EntryNumber);
                ledgerEntry.Id = ledgerEntryEntity.LedgerEntryId;
                ledgerEntry.LedgerEntryItems = new List<LedgerEntryItem>();
                ledgerEntry.ModuleDateId = ledgerEntryEntity.AccountingModuleId;
                ledgerEntry.RegisterDate = ledgerEntryEntity.RegisterDate;
                ledgerEntry.SalePoint = new SalePoint() { Id = Convert.ToInt32(ledgerEntryEntity.SalePointCode) };
                ledgerEntry.Status = 1;
                ledgerEntry.UserId = ledgerEntryEntity.UserCode;

                //Se obtiene el detalle
                List<LedgerEntryItem> ledgerEntryItems = new List<LedgerEntryItem>();

                var criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.LedgerEntryItem.Properties.LedgerEntryId, ledgerEntryEntity.LedgerEntryId);

                BusinessCollection businessCollectionItems = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.LedgerEntryItem), criteriaBuilder.GetPredicate()));
                foreach (GENERALLEDGEREN.LedgerEntryItem ledgerEntryItemEntity in businessCollectionItems.OfType<GENERALLEDGEREN.LedgerEntryItem>())
                {
                    LedgerEntryItem ledgerEntryItem = new LedgerEntryItem();
                    ledgerEntryItem.AccountingAccount = new AccountingAccount()
                    {
                        AccountingAccountId = Convert.ToInt32(ledgerEntryItemEntity.AccountingAccountId),
                        Description = _accountingAccountDAO.GetAccountingAccount(Convert.ToInt32(ledgerEntryItemEntity.AccountingAccountId)).Description,
                        Number = _accountingAccountDAO.GetAccountingAccount(Convert.ToInt32(ledgerEntryItemEntity.AccountingAccountId)).Number
                    };
                    ledgerEntryItem.AccountingNature = ledgerEntryItemEntity.AccountingNature == 1 ? AccountingNatures.Credit : AccountingNatures.Debit;
                    ledgerEntryItem.Amount = new Amount()
                    {
                        Currency = new Currency() { Id = ledgerEntryItemEntity.CurrencyCode, Description = "" },
                        
                        Value = ledgerEntryItemEntity.AmountValue
                    };
                    ledgerEntryItem.Analysis = new List<Analysis>();
                    ledgerEntryItem.ExchangeRate = new ExchangeRate()
                    {
                        SellAmount = ledgerEntryItemEntity.ExchangeRate
                    };
                    ledgerEntryItem.LocalAmount = new Amount() { Value = ledgerEntryItemEntity.AmountLocalValue };

                    List<Analysis> analysisItems = new List<Analysis>();
                    criteriaBuilder = new ObjectCriteriaBuilder();
                    criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AnalysisEntryItem.Properties.EntryItemId, ledgerEntryItemEntity.LedgerEntryItemId).And();
                    criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AnalysisEntryItem.Properties.IsJournalEntry, ledgerEntryItemEntity.IsJournalEntry);

                    BusinessCollection businessCollectionAnalysis = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.AnalysisEntryItem), criteriaBuilder.GetPredicate()));

                    foreach (GENERALLEDGEREN.AnalysisEntryItem analysisEntryItemEntity in businessCollectionAnalysis.OfType<GENERALLEDGEREN.AnalysisEntryItem>())
                    {
                        analysisItems.Add(new Analysis()
                        {
                            AnalysisConcept = new AnalysisConcept()
                            {
                                AnalysisCode = new AnalysisCode()
                                {
                                    AnalysisCodeId = Convert.ToInt32(analysisEntryItemEntity.AnalysisId),
                                    Description = _analysisCodeDAO.GetAnalysisCode(Convert.ToInt32(analysisEntryItemEntity.AnalysisId)).Description
                                },
                                AnalysisConceptId = Convert.ToInt32(analysisEntryItemEntity.AnalysisConceptId),
                                Description = _analysisConceptDAO.GetAnalysisConcept(Convert.ToInt32(analysisEntryItemEntity.AnalysisConceptId)).Description
                            },
                            AnalysisId = Convert.ToInt32(analysisEntryItemEntity.AnalysisId),
                            Description = _analysisCodeDAO.GetAnalysisCode(Convert.ToInt32(analysisEntryItemEntity.AnalysisId)).Description,
                            Key = analysisEntryItemEntity.ConceptKey
                        });
                    }
                    ledgerEntryItem.Analysis = analysisItems;

                    ledgerEntryItem.ReconciliationMovementType = new ReconciliationMovementType() { Id = Convert.ToInt32(ledgerEntryItemEntity.BankReconciliationId) };
                    ledgerEntryItem.CostCenters = new List<CostCenter>();

                    List<CostCenter> costCenters = new List<CostCenter>();
                    criteriaBuilder = new ObjectCriteriaBuilder();
                    criteriaBuilder.PropertyEquals(GENERALLEDGEREN.CostCenterEntryItem.Properties.EntryItemId, ledgerEntryItemEntity.LedgerEntryItemId).And();
                    criteriaBuilder.PropertyEquals(GENERALLEDGEREN.CostCenterEntryItem.Properties.IsJournalEntry, ledgerEntryItemEntity.IsJournalEntry);

                    BusinessCollection businessCollectionCostCenter = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.CostCenterEntryItem), criteriaBuilder.GetPredicate()));

                    foreach (GENERALLEDGEREN.CostCenterEntryItem costCenterEntryItemEntity in businessCollectionCostCenter.OfType<GENERALLEDGEREN.CostCenterEntryItem>())
                    {
                        costCenters.Add(new CostCenter()
                        {
                            CostCenterId = Convert.ToInt32(costCenterEntryItemEntity.CostCenterId),
                            Description = _costCenterDAO.GetCostCenter(new CostCenter() { CostCenterId = Convert.ToInt32(costCenterEntryItemEntity.CostCenterId) }).Description,
                            PercentageAmount = Convert.ToDecimal(costCenterEntryItemEntity.CostCenterPercentage)
                        });
                    }
                    ledgerEntryItem.CostCenters = costCenters;

                    ledgerEntryItem.Currency = new Currency() { Id = ledgerEntryItemEntity.CurrencyCode };
                    ledgerEntryItem.Description = ledgerEntryItemEntity.Description;
                    ledgerEntryItem.EntryType = new EntryType();
                    ledgerEntryItem.Id = ledgerEntryItemEntity.LedgerEntryItemId;
                    ledgerEntryItem.Individual = new Individual() { IndividualId = Convert.ToInt32(ledgerEntryItemEntity.IndividualId) };
                    ledgerEntryItem.PostDated = new List<PostDated>();

                    List<PostDated> postDates = new List<PostDated>();
                    criteriaBuilder = new ObjectCriteriaBuilder();
                    criteriaBuilder.PropertyEquals(GENERALLEDGEREN.PostdatedEntryItem.Properties.EntryItemId, ledgerEntryItemEntity.LedgerEntryItemId).And();
                    criteriaBuilder.PropertyEquals(GENERALLEDGEREN.PostdatedEntryItem.Properties.IsJournalEntry, ledgerEntryItemEntity.IsJournalEntry);

                    BusinessCollection businessCollectionPostdated = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.PostdatedEntryItem), criteriaBuilder.GetPredicate()));

                    foreach (GENERALLEDGEREN.PostdatedEntryItem postdatedEntryItemEntity in businessCollectionPostdated.OfType<GENERALLEDGEREN.PostdatedEntryItem>())
                    {
                        postDates.Add(new PostDated()
                        {
                            Amount = new Amount()
                            {
                                Currency = new Currency() { Id = Convert.ToInt32(postdatedEntryItemEntity.CurrencyCode) },
                                Value = Convert.ToDecimal(postdatedEntryItemEntity.AmountValue),
                            },
                            DocumentNumber = Convert.ToInt32(postdatedEntryItemEntity.DocumentNumber),
                            ExchangeRate = new ExchangeRate()
                            {
                                SellAmount = Convert.ToDecimal(postdatedEntryItemEntity.ExchangeRate) 
                            },
                            LocalAmount = new Amount() { Value = Convert.ToDecimal(postdatedEntryItemEntity.AmountLocalValue) },
                            PostDatedId = Convert.ToInt32(postdatedEntryItemEntity.PostdatedEntryItemId),
                            PostDateType = Convert.ToInt32(postdatedEntryItemEntity.PostdatedType) == 1 ? Enums.PostDateTypes.Check : Enums.PostDateTypes.Credit
                        });
                    }
                    ledgerEntryItem.PostDated = postDates;

                    ledgerEntryItem.Receipt = new Receipt()
                    {
                        Date = ledgerEntryItemEntity.ReceiptDate,
                        Number = ledgerEntryItemEntity.ReceiptNumber
                    };

                    ledgerEntryItems.Add(ledgerEntryItem);
                }

                ledgerEntry.LedgerEntryItems = ledgerEntryItems;

                // Return del model
                return ledgerEntry;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetLedgerEntries
        /// </summary>
        /// <param name="ledgerEntryId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="branchId"></param>
        /// <param name="destinationId"></param>
        /// <param name="accountingMovementTypeId"></param>
        /// <returns></returns>
        public List<LedgerEntry> GetLedgerEntries(int ledgerEntryId, DateTime dateFrom, DateTime dateTo, int branchId, int destinationId, int accountingMovementTypeId)
        {
            try
            {
                var ledgerEntries = new List<LedgerEntry>();
                var criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.LedgerEntry.Properties.EntryNumber, ledgerEntryId).And();
                criteriaBuilder.Property(GENERALLEDGEREN.LedgerEntry.Properties.AccountingDate);
                criteriaBuilder.GreaterEqual();
                criteriaBuilder.Constant(dateFrom);
                criteriaBuilder.And();
                criteriaBuilder.Property(GENERALLEDGEREN.LedgerEntry.Properties.AccountingDate);
                criteriaBuilder.LessEqual();
                criteriaBuilder.Constant(dateTo);

                if (accountingMovementTypeId > 0)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(GENERALLEDGEREN.LedgerEntry.Properties.AccountingMovementTypeId, accountingMovementTypeId);
                }
                if (branchId > 0)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(GENERALLEDGEREN.LedgerEntry.Properties.BranchCode, branchId);
                }
                if (destinationId > 0)
                {
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(GENERALLEDGEREN.LedgerEntry.Properties.EntryDestinationId, destinationId);
                }
                
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.LedgerEntry), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    foreach (GENERALLEDGEREN.LedgerEntry ledgerEntryEntity in businessCollection.OfType<GENERALLEDGEREN.LedgerEntry>())
                    {
                        LedgerEntry ledgerEntry = GetLedgerEntryById(ledgerEntryEntity.LedgerEntryId);                        
                        ledgerEntries.Add(ledgerEntry);
                    }
                }

                return ledgerEntries;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// SaveLedgerEntryItemGroup
        /// </summary>
        /// <param name="ledgerEntryItem"></param>
        /// <param name="ledgerEntryItemId"></param>
        /// <param name="isJournalEntry"></param>
        private void SaveLedgerEntryItemGroup(LedgerEntryItem ledgerEntryItem, int ledgerEntryItemId, bool isJournalEntry)
        {
            // Se graba centro de costos asociados al movimiento.
            if (ledgerEntryItem.CostCenters != null)
            {
                foreach (CostCenter costCenter in ledgerEntryItem.CostCenters)
                {
                    _costCenterEntryDAO.SaveCostCenterEntry(costCenter, ledgerEntryItemId, isJournalEntry);
                }
            }

            // Grabación de análisis de movimientos
            if (ledgerEntryItem.Analysis != null)
            {
                foreach (Analysis analysisItem in ledgerEntryItem.Analysis)
                {
                    int correlativeNumber = 0;

                    correlativeNumber = GetCorrelativeNumber(analysisItem.AnalysisConcept.AnalysisCode.AnalysisCodeId,
                                                             analysisItem.AnalysisConcept.AnalysisConceptId, analysisItem.Key) + 1;
                    analysisItem.AnalysisId = 0; // Id autonumérico
                    _analysisDAO.SaveAnalysis(analysisItem, ledgerEntryItemId, correlativeNumber, isJournalEntry);
                }
            }

            // Grabación de Postfechados
            if (ledgerEntryItem.PostDated != null)
            {
                foreach (PostDated postDatedItem in ledgerEntryItem.PostDated)
                {
                    _postDatedDAO.SavePostDated(postDatedItem, ledgerEntryItemId, isJournalEntry);
                }
            }
        }

        /// <summary>
        /// GetEntryStatus
        /// Obtiene el estado de un movimiento
        /// </summary>
        /// <param name="ledgerEntryId"></param>
        /// <param name="isJournalEntry"></param>
        /// <returns></returns>
        private int GetLedgerEntryStatus(int ledgerEntryId, bool isJournalEntry)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

            criteriaBuilder.PropertyEquals(GENERALLEDGEREN.EntryRevertion.Properties.EntrySourceId, ledgerEntryId);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(GENERALLEDGEREN.EntryRevertion.Properties.IsJournalEntry, isJournalEntry);

            BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.EntryRevertion), criteriaBuilder.GetPredicate()));

            return businessCollection.Count > 0 ? Convert.ToInt32(AccountingEntryStatus.Reverted) : Convert.ToInt32(AccountingEntryStatus.Active);
        }

        /// <summary>
        /// GetCorrelativeNumber
        /// Obtiene el número correlativo para la tabla Entry Análisis 
        /// </summary>
        /// <returns>int</returns>
        private int GetCorrelativeNumber(int analysisCodeId, int analysisConceptId, string key)
        {
            int correlativeNumber = 0;

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

            criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AnalysisEntryItem.Properties.AnalysisId, analysisCodeId);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AnalysisEntryItem.Properties.AnalysisConceptId, analysisConceptId);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AnalysisEntryItem.Properties.ConceptKey, key);

            BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.AnalysisEntryItem), criteriaBuilder.GetPredicate()));

            if (businessCollection.Count > 0)
            {
                var maxNumber = (from GENERALLEDGEREN.AnalysisEntryItem entryAnalisis in businessCollection select entryAnalisis.CorrelativeNumber).Max();
                correlativeNumber = Convert.ToInt32(maxNumber);
            }
            else
            {
                correlativeNumber = 1;
            }

            return correlativeNumber;
        }

        #endregion

    }
}
