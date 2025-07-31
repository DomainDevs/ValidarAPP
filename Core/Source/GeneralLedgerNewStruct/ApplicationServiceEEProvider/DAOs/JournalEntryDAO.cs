//Sistran Core
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Assemblers;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Views;
using Sistran.Core.Application.Utilities.DataFacade;
//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using GENERALLEDGEREN = Sistran.Core.Application.GeneralLedger.Entities;
using GLEN = Sistran.Core.Application.GeneralLedger.Entities;

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.DAOs
{
    public class JournalEntryDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion Instance variables

        #region Save

        /// <summary>
        /// SaveJournalEntry
        /// </summary>
        /// <param name="journalEntry"></param>
        /// <returns></returns>
        public JournalEntry SaveJournalEntry(JournalEntry journalEntry)
        {
            try
            {
                // Convertir de model a entity
                GENERALLEDGEREN.JournalEntry journalEntryEntity = EntityAssembler.CreateJournalEntry(journalEntry);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(journalEntryEntity);

                return ModelAssembler.CreateJournalEntry(journalEntryEntity);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion Save

        #region Update

        /// <summary>
        /// UpdateJournalEntry
        /// </summary>
        /// <param name="journalEntry"></param>
        /// <returns></returns>
        public JournalEntry UpdateJournalEntry(JournalEntry journalEntry)
        {
            try
            {
                int? accountingCompanyId = 0;

                int? salePointId = 0;

                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.JournalEntry.CreatePrimaryKey(journalEntry.Id);

                // Encuentra el objeto en referencia a la llave primaria
                GENERALLEDGEREN.JournalEntry journalEntryEntity = (GENERALLEDGEREN.JournalEntry)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));


                if (journalEntryEntity.AccountingCompanyId == null)
                {
                    accountingCompanyId = null;
                }
                else
                {
                    accountingCompanyId = journalEntryEntity.AccountingCompanyId;
                }


                if (journalEntryEntity.SalePointCode == null)
                {
                    salePointId = null;
                }
                else
                {
                    salePointId = journalEntryEntity.SalePointCode;
                }


                journalEntryEntity.AccountingCompanyId = accountingCompanyId;
                journalEntryEntity.AccountingModuleId = journalEntry.ModuleDateId;
                journalEntryEntity.BranchCode = journalEntry.Branch.Id;
                journalEntryEntity.SalePointCode = salePointId;
                journalEntryEntity.TechnicalTransaction = journalEntry.TechnicalTransaction;
                journalEntryEntity.Description = journalEntry.Description;
                journalEntryEntity.AccountingDate = journalEntry.AccountingDate;
                journalEntryEntity.RegisterDate = journalEntry.RegisterDate;
                journalEntryEntity.UserCode = journalEntry.UserId;
                journalEntryEntity.Status = journalEntry.Status;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(journalEntryEntity);

                return ModelAssembler.CreateJournalEntry(journalEntryEntity);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion Update

        #region Delete

        /// <summary>
        /// DeleteJournalEntry
        /// </summary>
        /// <param name="journalEntry"></param>
        public void DeleteJournalEntry(JournalEntry journalEntry)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.JournalEntry.CreatePrimaryKey(journalEntry.Id);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.JournalEntry journalEntryEntity = (GENERALLEDGEREN.JournalEntry)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(journalEntryEntity);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion Delete

        #region Get

        /// <summary>
        /// GetJournalEntry
        /// </summary>
        /// <param name="journalEntry"></param>
        /// <returns></returns>
        public JournalEntry GetJournalEntry(JournalEntry journalEntry)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.JournalEntry.CreatePrimaryKey(journalEntry.Id);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.JournalEntry journalEntryEntity = (GENERALLEDGEREN.JournalEntry)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreateJournalEntry(journalEntryEntity);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// Get Journal Entry by id
        /// </summary>
        /// <param name="journalEntryId">Identifier for journal entry</param>
        /// <returns>Journal entry</returns>
        public JournalEntry GetJournalEntryByJournaEntryId(int journalEntryId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.JournalEntry.CreatePrimaryKey(journalEntryId);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.JournalEntry entityJournalEntry = (GENERALLEDGEREN.JournalEntry)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreateJournalEntry(entityJournalEntry);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// GetJournalEntry
        /// </summary>
        /// <param name="journalEntry"></param>
        /// <returns></returns>
        public JournalEntry GetJournalEntryByTechnicalTransaction(int technicalTransaction)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(GENERALLEDGEREN.JournalEntry.Properties.TechnicalTransaction, typeof(GENERALLEDGEREN.JournalEntry).Name);
                filter.Equal();
                filter.Constant(technicalTransaction);

                JournalEntry journalEntryModel = new JournalEntry();
                // Realizar las operaciones con los entities utilizando DAF
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.JournalEntry), filter.GetPredicate()));
                //se llena los movimientos del asiento de diario
                if (businessCollection.Any())
                {
                    foreach (GENERALLEDGEREN.JournalEntry journalEntry in businessCollection.OfType<GENERALLEDGEREN.JournalEntry>())
                    {
                        return ModelAssembler.CreateJournalEntry(journalEntry);
                    }
                }
                return journalEntryModel;
                // Return del model

            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        public JournalEntry GetJournalEntryDetailByTechnicalTransaction(int technicalTransaction)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(GLEN.JournalEntry.Properties.TechnicalTransaction, typeof(GLEN.JournalEntry).Name, technicalTransaction);
                
                JournalEntryView journalEntryView = new JournalEntryView();
                ViewBuilder viewBuilder = new ViewBuilder("JournalEntryView");
                viewBuilder.Filter = filter.GetPredicate();

                DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, journalEntryView);

                if (journalEntryView.JournalEntries.Any())
                {
                    JournalEntry journalEntry = new JournalEntry();

                    GLEN.JournalEntry entityJournalEntry = journalEntryView.JournalEntries.Cast<GLEN.JournalEntry>().First();

                    journalEntry = ModelAssembler.CreateJournalEntry(entityJournalEntry);

                    if (journalEntryView.JournalEntryItems.Any())
                    {
                        journalEntry.JournalEntryItems = ModelAssembler.CreateJournalEntryItems(journalEntryView.JournalEntryItems);
                        journalEntry.JournalEntryItems.ForEach(item =>
                        {
                            item.AccountingAccount = ModelAssembler.CreateAccountingAccount(journalEntryView.AccountingAccounts.Cast<GLEN.AccountingAccount>().First(x => x.AccountingAccountId == item.AccountingAccount.AccountingAccountId));
                        });
                    }

                    return journalEntry;
                }

                return null;
            }
            catch (Exception)
            {
                throw new BusinessException(Resources.Resources.ErrorGetJournalEntry);
            }
        }

        /// <summary>
        /// GetJournalEntryItemsByTechnicalTransaction
        /// </summary>
        /// <returns></returns>
        public List<JournalEntry> GetJournalEntryItemsByTechnicalTransaction(int technicalTransaction)
        {
            try
            {

                List<JournalEntry> journalEntries = new List<JournalEntry>();
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.Property(GENERALLEDGEREN.JournalEntry.Properties.TechnicalTransaction, typeof(GENERALLEDGEREN.JournalEntry).Name);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(technicalTransaction);
                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.JournalEntry), criteriaBuilder.GetPredicate()));

                foreach (GENERALLEDGEREN.JournalEntry journalEntryEntity in businessCollection.OfType<GENERALLEDGEREN.JournalEntry>())
                {
                    JournalEntry journalEntry = ModelAssembler.CreateJournalEntry(journalEntryEntity);
                    List<JournalEntryItem> journalEntryItems = new List<JournalEntryItem>();

                    criteriaBuilder = new ObjectCriteriaBuilder();
                    criteriaBuilder.PropertyEquals(GENERALLEDGEREN.JournalEntryItem.Properties.JournalEntryId, journalEntryEntity.JournalEntryId);
                    // Asignamos BusinessCollection a una Lista
                    BusinessCollection businessCollectionItems = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.JournalEntryItem), criteriaBuilder.GetPredicate()));

                    foreach (GENERALLEDGEREN.JournalEntryItem journalEntryItemEntity in businessCollectionItems.OfType<GENERALLEDGEREN.JournalEntryItem>())
                    {
                        journalEntryItems.Add(ModelAssembler.CreateJournalEntryItem(journalEntryItemEntity));
                    }
                    journalEntry.JournalEntryItems = journalEntryItems;

                    journalEntries.Add(journalEntry);
                }

                // Return como Lista
                return journalEntries;
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// GetJournalEntries
        /// </summary>
        /// <returns></returns>
        public List<JournalEntry> GetJournalEntries()
        {
            try
            {
                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.JournalEntry)));

                // Return como Lista
                return ModelAssembler.CreateJournalEntries(businessCollection);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// GetJournalEntriesByRangeDate
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<JournalEntry> GetJournalEntriesByRangeDate(DateTime startDate, DateTime endDate)
        {
            try
            {
                List<JournalEntry> journalEntries = new List<JournalEntry>();
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.Property(GENERALLEDGEREN.JournalEntry.Properties.AccountingDate);
                criteriaBuilder.GreaterEqual();
                criteriaBuilder.Constant(startDate);
                criteriaBuilder.And();
                criteriaBuilder.Property(GENERALLEDGEREN.JournalEntry.Properties.AccountingDate);
                criteriaBuilder.LessEqual();
                criteriaBuilder.Constant(endDate);

                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.JournalEntry), criteriaBuilder.GetPredicate()));

                foreach (GENERALLEDGEREN.JournalEntry journalEntryEntity in businessCollection.OfType<GENERALLEDGEREN.JournalEntry>())
                {
                    JournalEntry journalEntry = ModelAssembler.CreateJournalEntry(journalEntryEntity);
                    List<JournalEntryItem> journalEntryItems = new List<JournalEntryItem>();

                    criteriaBuilder = new ObjectCriteriaBuilder();
                    criteriaBuilder.PropertyEquals(GENERALLEDGEREN.JournalEntryItem.Properties.JournalEntryId, journalEntryEntity.JournalEntryId);
                    // Asignamos BusinessCollection a una Lista
                    BusinessCollection businessCollectionItems = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.JournalEntryItem), criteriaBuilder.GetPredicate()));

                    foreach (GENERALLEDGEREN.JournalEntryItem journalEntryItemEntity in businessCollectionItems.OfType<GENERALLEDGEREN.JournalEntryItem>())
                    {
                        journalEntryItems.Add(ModelAssembler.CreateJournalEntryItem(journalEntryItemEntity));
                    }
                    journalEntry.JournalEntryItems = journalEntryItems;

                    journalEntries.Add(journalEntry);
                }

                // Return como Lista
                return ModelAssembler.CreateJournalEntries(businessCollection);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }
        #endregion Get

        /// <summary>
        /// GetJournalEntry
        /// </summary>
        /// <param name="journalEntry"></param>
        /// <returns></returns>
        public bool UpdateJournalEntryStatusByTechnicalTransction(int technicalTransaction, int status)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(GENERALLEDGEREN.JournalEntry.Properties.TechnicalTransaction, typeof(GENERALLEDGEREN.JournalEntry).Name);
                filter.Equal();
                filter.Constant(technicalTransaction);
                
                BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(GENERALLEDGEREN.JournalEntry), filter.GetPredicate());
                if (businessCollection.Any())
                {
                    GENERALLEDGEREN.JournalEntry entityJournalEntry = (GENERALLEDGEREN.JournalEntry)businessCollection.First();
                    entityJournalEntry.Status = status;
                    DataFacadeManager.Update(entityJournalEntry);
                }
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
            return false;
        }
    }
}
