#region Using

//Sistran Core
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Assemblers;
using GENERALLEDGEREN = Sistran.Core.Application.GeneralLedger.Entities;
using GeneralLedgerModels = Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;

using System;
using System.Collections.Generic;

#endregion

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.DAOs
{
    internal class DestinationDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Save

        /// <summary>
        /// SaveEntryDestination
        /// </summary>
        /// <param name="entryDestination"></param>
        /// <returns></returns>
        public GeneralLedgerModels.EntryDestination SaveEntryDestination(GeneralLedgerModels.EntryDestination entryDestination)
        {
            try
            {
                // Convertir de model a entity
                var destinationEntity = EntityAssembler.CreateEntryDestination(entryDestination);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(destinationEntity);

                return ModelAssembler.CreateEntryDestination(destinationEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Save

        #region Update

        /// <summary>
        /// UpdateEntryDestination
        /// </summary>
        /// <param name="destination"></param>
        /// <returns></returns>
        public GeneralLedgerModels.EntryDestination UpdateEntryDestination(GeneralLedgerModels.EntryDestination entryDestination)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                var primaryKey = GENERALLEDGEREN.EntryDestination.CreatePrimaryKey(entryDestination.DestinationId);

                // Encuentra el objeto en referencia a la llave primaria
                var destinationEntity = (GENERALLEDGEREN.EntryDestination)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                destinationEntity.Description = entryDestination.Description;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(destinationEntity);

                return ModelAssembler.CreateEntryDestination(destinationEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Delete

        /// <summary>
        /// DeleteEntryDestination
        /// </summary>
        /// <param name="destinationId"></param>
        public void DeleteEntryDestination(int destinationId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                var primaryKey = GENERALLEDGEREN.EntryDestination.CreatePrimaryKey(destinationId);

                // Realizar las operaciones con los entities utilizando DAF
                var destinationEntity = (GENERALLEDGEREN.EntryDestination)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(destinationEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

        }

        #endregion

        #region Get

        /// <summary>
        /// GetEntryDestination
        /// </summary>
        /// <param name="entryDestination"></param>
        /// <returns></returns>
        public GeneralLedgerModels.EntryDestination GetEntryDestination(GeneralLedgerModels.EntryDestination entryDestination)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                var primaryKey = GENERALLEDGEREN.EntryDestination.CreatePrimaryKey(entryDestination.DestinationId);

                // Realizar las operaciones con los entities utilizando DAF
                var entryDetinationEntity = (GENERALLEDGEREN.EntryDestination)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreateEntryDestination(entryDetinationEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetEntryDestinations
        /// </summary>
        /// <returns></returns>
        public List<GeneralLedgerModels.EntryDestination> GetEntryDestinations()
        {
            try
            {
                // Asignamos BusinessCollection a una Lista
                var businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.EntryDestination)));

                // Return como Lista
                return ModelAssembler.CreateEntryDestinations(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

    }
}
