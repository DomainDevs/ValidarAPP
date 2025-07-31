using System;
using System.Collections.Generic;
using Sistran.Core.Application.GeneralLedgerServices.DTOs;
using GENERALLEDGEREN = Sistran.Core.Application.GeneralLedger.Entities;

//Sistran Core
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Assemblers;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using System.Linq;

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.DAOs
{
    public class JournalEntryItemDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion Instance variables

        #region Save

        /// <summary>
        /// SaveJournalEntryItem
        /// </summary>
        /// <param name="journalEntryItem"></param>
        /// <param name="journalEntryId"></param>
        /// <returns>JournalEntryItem</returns>
        public JournalEntryItem SaveJournalEntryItem(JournalEntryItem journalEntryItem, int journalEntryId)
        {
            try
            {
                // Convertir de model a entity
                GENERALLEDGEREN.JournalEntryItem journalEntryItemEntity = EntityAssembler.CreateJournalEntryItem(journalEntryItem, journalEntryId);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(journalEntryItemEntity);

                return ModelAssembler.CreateJournalEntryItem(journalEntryItemEntity);
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
        /// <param name="journalEntryItem"></param>
        /// <param name="journalEntryId"></param>
        /// <returns></returns>
        public JournalEntryItem UpdateJournalEntryItem(JournalEntryItem journalEntryItem, int journalEntryId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.JournalEntry.CreatePrimaryKey(journalEntryItem.Id);

                // Encuentra el objeto en referencia a la llave primaria
                GENERALLEDGEREN.JournalEntryItem journalEntryItemEntity = (GENERALLEDGEREN.JournalEntryItem)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                journalEntryItemEntity.JournalEntryId = journalEntryId;
                journalEntryItemEntity.CurrencyCode = journalEntryItem.Currency.Id;
                journalEntryItemEntity.AccountingAccountId = journalEntryItem.AccountingAccount.AccountingAccountId;
                journalEntryItemEntity.BankReconciliationId = journalEntryItem.ReconciliationMovementType.Id;
                journalEntryItemEntity.ReceiptNumber = journalEntryItem.Receipt.Number;
                journalEntryItemEntity.ReceiptDate = journalEntryItem.Receipt.Date;
                journalEntryItemEntity.AccountingNature = Convert.ToInt32(journalEntryItem.AccountingNature);
                journalEntryItemEntity.Description = journalEntryItem.Description;
                journalEntryItemEntity.Amount = journalEntryItem.Amount.Value;
                journalEntryItemEntity.LocalAmount = journalEntryItem.LocalAmount.Value;
                journalEntryItemEntity.ExchangeRate = journalEntryItem.ExchangeRate.SellAmount;
                journalEntryItemEntity.IndividualId = journalEntryItem.Individual.IndividualId;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(journalEntryItemEntity);

                return ModelAssembler.CreateJournalEntryItem(journalEntryItemEntity);
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
        /// <param name="journalEntryItem"></param>
        public void DeleteJournalEntryItem(JournalEntryItem journalEntryItem)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.JournalEntryItem.CreatePrimaryKey(journalEntryItem.Id);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.JournalEntryItem journalEntryItemEntity = (GENERALLEDGEREN.JournalEntryItem)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(journalEntryItemEntity);
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
        /// <param name="journalEntryItem"></param>
        /// <returns>JournalEntryItem</returns>
        public JournalEntryItem GetJournalEntryItem(JournalEntryItem journalEntryItem)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.JournalEntryItem.CreatePrimaryKey(journalEntryItem.Id);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.JournalEntryItem journalEntryItemEntity = (GENERALLEDGEREN.JournalEntryItem)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreateJournalEntryItem(journalEntryItemEntity);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// GetJournalEntryItems
        /// </summary>
        /// <returns></returns>
        public List<JournalEntryItem> GetJournalEntryItems()
        {
            try
            {
                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.JournalEntryItem)));

                // Return como Lista
                return ModelAssembler.CreateJournalEntryItems(businessCollection);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// GetJournalEntryItemsByTechnicalTransaction
        /// </summary>
        /// <returns></returns>
        public JournalEntry GetJournalEntryItemsByTechnicalTransaction(int technicalTransaction)
        {
            try
            {

                //List<JournalEntry> journalEntries = new List<JournalEntry>();
                JournalEntry journalEntry = null;
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.Property(GENERALLEDGEREN.JournalEntry.Properties.TechnicalTransaction, typeof(GENERALLEDGEREN.JournalEntry).Name);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(technicalTransaction);
                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.JournalEntry), criteriaBuilder.GetPredicate()));

                foreach (GENERALLEDGEREN.JournalEntry journalEntryEntity in businessCollection.OfType<GENERALLEDGEREN.JournalEntry>())
                {
                    journalEntry = ModelAssembler.CreateJournalEntry(journalEntryEntity);
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

                    //journalEntries.Add(journalEntry);
                }

                // Return como Lista
                //return ModelAssembler.CreateJournalEntrie(journalEntry);
                return journalEntry;
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// GetJournalEntryItemsByTechnicalTransaction
        /// </summary>
        /// <returns></returns>
        public List<JournalEntryItem> GetJournalEntryItemsBySourceCode(int sourceCode)
        {
            try
            {
                List<JournalEntryItem> journalEntryItems = new List<JournalEntryItem>();
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.JournalEntryItem.Properties.SourceCode, sourceCode);
                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollectionItems = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.JournalEntryItem), criteriaBuilder.GetPredicate()));

                foreach (GENERALLEDGEREN.JournalEntryItem journalEntryItemEntity in businessCollectionItems.OfType<GENERALLEDGEREN.JournalEntryItem>())
                {
                    journalEntryItems.Add(ModelAssembler.CreateJournalEntryItem(journalEntryItemEntity));
                }

                return journalEntryItems;
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion Get
    }
}
