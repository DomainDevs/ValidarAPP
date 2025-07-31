#region Using

//Sistran Core
using Sistran.Core.Application.GeneralLedgerServices.DTOs;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Assemblers;
using GENERALLEDGEREN = Sistran.Core.Application.GeneralLedger.Entities;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;

using System;
using System.Collections.Generic;

#endregion Using

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.DAOs
{
    internal class EntryMassiveLoadLogDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Save

        /// <summary>
        /// SaveEntryMassiveLoadLog
        /// </summary>
        /// <param name="massiveEntryLogDTO"></param>
        /// <returns></returns>
        public MassiveEntryLogDTO SaveEntryMassiveLoadLog(MassiveEntryLogDTO massiveEntryLogDTO)
        {
            try
            {
                // Convertir de model a entity
                GENERALLEDGEREN.EntryMassiveLoadLog entryMassiveLoadLogEntity = EntityAssembler.CreateEntryMassiveLoadLog(massiveEntryLogDTO);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(entryMassiveLoadLogEntity);

                // Return del model
                return ModelAssembler.CreateEntryMassiveLoadLog(entryMassiveLoadLogEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// UpdateEntryMassiveLoadLog
        /// </summary>
        /// <param name="massiveEntryLog"></param>
        /// <returns></returns>
        public MassiveEntryLogDTO UpdateEntryMassiveLoadLog(MassiveEntryLogDTO massiveEntryLog)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.EntryMassiveLoadLog.CreatePrimaryKey(massiveEntryLog.Id);

                // Encuentra el objeto en referencia a la llave primaria
                GENERALLEDGEREN.EntryMassiveLoadLog entryMassiveLoadLogEntity = (GENERALLEDGEREN.EntryMassiveLoadLog)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                entryMassiveLoadLogEntity.ProcessDate = massiveEntryLog.ProcessDate;
                entryMassiveLoadLogEntity.OperationDate = massiveEntryLog.OperationDate;
                entryMassiveLoadLogEntity.RowNumber = massiveEntryLog.RowNumber;
                entryMassiveLoadLogEntity.Success = Convert.ToInt32(massiveEntryLog.Success);
                entryMassiveLoadLogEntity.ErrorDescription = massiveEntryLog.ErrorDescription;
                entryMassiveLoadLogEntity.Enabled = Convert.ToInt32(massiveEntryLog.Enabled);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(entryMassiveLoadLogEntity);

                // Return del model
                return ModelAssembler.CreateEntryMassiveLoadLog(entryMassiveLoadLogEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Get

        /// <summary>
        /// GetEntryMassiveLoadLog
        /// </summary>
        /// <param name="massiveEntryLog"></param>
        /// <returns></returns>
        public MassiveEntryLogDTO GetEntryMassiveLoadLog(MassiveEntryLogDTO massiveEntryLog)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.EntryMassiveLoadLog.CreatePrimaryKey(massiveEntryLog.Id);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.EntryMassiveLoadLog entryMassiveLoadLogEntity = (GENERALLEDGEREN.EntryMassiveLoadLog)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreateEntryMassiveLoadLog(entryMassiveLoadLogEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetEntryMassiveLoadLogs
        /// </summary>
        /// <returns></returns>
        public List<MassiveEntryLogDTO> GetEntryMassiveLoadLogs()
        {
            try
            {
                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.EntryMassiveLoadLog)));

                // Return como Lista
                return ModelAssembler.CreateEntryMassiveLoadLogs(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Get

    }
}
