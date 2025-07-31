#region Using

//Sistran Core
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Assemblers;
using GENERALLEDGEREN = Sistran.Core.Application.GeneralLedger.Entities;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;

using System;
using System.Collections.Generic;
using System.Linq;

#endregion Using

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.DAOs
{
    class EntryNumberDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Save

        /// <summary>
        /// SaveEntryNumber
        /// </summary>
        /// <param name="entryNumber"></param>
        /// <returns></returns>
        public EntryNumber SaveEntryNumber(EntryNumber entryNumber)
        {
            try
            {
                // Convertir de model a entity
                GENERALLEDGEREN.EntryNumber entryNumberEntity = EntityAssembler.CreateEntryNumber(entryNumber);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(entryNumberEntity);

                // Return del model
                return ModelAssembler.CreateEntryNumber(entryNumberEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// UpdateEntryNumber
        /// </summary>
        /// <param name="entryNumber"></param>
        /// <returns></returns>
        public EntryNumber UpdateEntryNumber(EntryNumber entryNumber)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.EntryNumber.CreatePrimaryKey(entryNumber.Id);

                // Encuentra el objeto en referencia a la llave primaria
                GENERALLEDGEREN.EntryNumber entryNumberEntity = (GENERALLEDGEREN.EntryNumber)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                entryNumberEntity.AccountingMovementTypeId = entryNumber.AccountingMovementType.AccountingMovementTypeId;
                entryNumberEntity.EntryDestinationId = entryNumber.EntryDestination.DestinationId;
                entryNumberEntity.GenerationMonth = entryNumber.Date.Month;
                entryNumberEntity.GenerationYear = entryNumber.Date.Year;
                entryNumberEntity.IsJournalEntry = entryNumber.IsJournalEntry;
                entryNumberEntity.LastGeneratedEntryYear = entryNumber.Year;
                entryNumberEntity.Number = entryNumber.Number;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(entryNumberEntity);

                // Return del model
                return ModelAssembler.CreateEntryNumber(entryNumberEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateEntryNumber
        /// </summary>
        /// <param name="entryNumber"></param>
        /// <returns></returns>
        public int UpdateEntryNumber(Dictionary<string, string> entryNumber)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.EntryNumber.CreatePrimaryKey(Convert.ToInt32(entryNumber["EntryNumberId"]));

                // Encuentra el objeto en referencia a la llave primaria
                GENERALLEDGEREN.EntryNumber entryNumberEntity = (GENERALLEDGEREN.EntryNumber)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                entryNumberEntity.GenerationMonth = Convert.ToInt32(entryNumber["GenerationMonth"]);
                entryNumberEntity.GenerationYear = Convert.ToInt32(entryNumber["GenerationYear"]);
                entryNumberEntity.LastGeneratedEntryYear = Convert.ToInt32(entryNumber["LastGeneratedEntryYear"]);
                entryNumberEntity.Number = Convert.ToInt32(entryNumber["Number"]);
                entryNumberEntity.EntryDestinationId = Convert.ToInt32(entryNumber["EntryDestinationId"]);
                entryNumberEntity.AccountingMovementTypeId = Convert.ToInt32(entryNumber["AccountingMovementTypeId"]);
                entryNumberEntity.IsJournalEntry = Convert.ToBoolean(entryNumber["IsJournalEntry"]);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(entryNumberEntity);

                // Return del model
                return entryNumberEntity.EntryNumberId;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Delete

        /// <summary>
        /// DeleteEntryNumber
        /// </summary>
        /// <param name="entryNumberId"></param>
        /// <returns></returns>
        public bool DeleteEntryNumber(int entryNumberId)
        {
            bool isDeleted = false;

            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.EntryNumber.CreatePrimaryKey(entryNumberId);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.EntryNumber entryNumberEntity = (GENERALLEDGEREN.EntryNumber)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(entryNumberEntity);

                isDeleted = true;
            }
            catch
            {
                isDeleted = false;
            }
            return isDeleted;
        }

        #endregion

        #region Get

        /// <summary>
        /// GetEntryNumber
        /// </summary>
        /// <param name="entryNumberId"></param>
        /// <returns></returns>
        public EntryNumber GetEntryNumber(int entryNumberId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.EntryNumber.CreatePrimaryKey(entryNumberId);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.EntryNumber entryNumberEntity = (GENERALLEDGEREN.EntryNumber)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreateEntryNumber(entryNumberEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetLastEntryNumber
        /// </summary>
        /// <param name="entryNumber"></param>
        /// <returns></returns>
        public EntryNumber GetLastEntryNumber(EntryNumber entryNumber)
        {
            EntryNumber lastEntryNumber = new EntryNumber();

            // Se obtiene el último número por código de sistema, código de destino, código de caja, año del asiento, mes de asiento y tipo de asiento
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

            criteriaBuilder.PropertyEquals(GENERALLEDGEREN.EntryNumber.Properties.AccountingMovementTypeId, entryNumber.AccountingMovementType.AccountingMovementTypeId);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(GENERALLEDGEREN.EntryNumber.Properties.EntryDestinationId, entryNumber.EntryDestination.DestinationId);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(GENERALLEDGEREN.EntryNumber.Properties.GenerationMonth, entryNumber.Date.Month);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(GENERALLEDGEREN.EntryNumber.Properties.GenerationYear, entryNumber.Date.Year);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(GENERALLEDGEREN.EntryNumber.Properties.IsJournalEntry, entryNumber.IsJournalEntry);

            BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.EntryNumber), criteriaBuilder.GetPredicate()));

            //Busca el último numerador  
            if (businessCollection.Count > 0)
            {
                foreach (GENERALLEDGEREN.EntryNumber entryNumberEntity in businessCollection.OfType<GENERALLEDGEREN.EntryNumber>())
                {
                    lastEntryNumber.AccountingMovementType = new AccountingMovementType()
                    {
                        AccountingMovementTypeId = Convert.ToInt32(entryNumberEntity.AccountingMovementTypeId)
                    };
                    lastEntryNumber.Date = new DateTime(Convert.ToInt32(entryNumberEntity.GenerationYear), Convert.ToInt32(entryNumberEntity.GenerationMonth), 1);
                    lastEntryNumber.EntryDestination = new EntryDestination()
                    {
                        DestinationId = Convert.ToInt32(entryNumberEntity.EntryDestinationId)
                    };
                    lastEntryNumber.Id = entryNumberEntity.EntryNumberId;
                    lastEntryNumber.IsJournalEntry = Convert.ToBoolean(entryNumberEntity.IsJournalEntry);
                    lastEntryNumber.Number = Convert.ToInt32(entryNumberEntity.Number);
                    lastEntryNumber.Year = Convert.ToInt32(entryNumberEntity.LastGeneratedEntryYear);
                }
            }
            else
            {
                //Crea un nuevo registro
                entryNumber.Number = 1;
                lastEntryNumber = SaveEntryNumber(entryNumber);
            }

            return lastEntryNumber;
        }

        /// <summary>
        /// GetEntryNumbers
        /// </summary>
        /// <returns></returns>
        public List<EntryNumber> GetEntryNumbers()
        {
            try
            {
                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.EntryNumber)));

                // Return como Lista
                return ModelAssembler.CreateEntryNumbers(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion
    }
}

