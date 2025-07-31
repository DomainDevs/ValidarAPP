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
    internal class EntryMassiveLoadDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Save

        /// <summary>
        /// SaveEntryMassiveLoad
        /// </summary>
        /// <param name="massiveEntryDTO"></param>
        /// <returns></returns>
        public MassiveEntryDTO SaveEntryMassiveLoad(MassiveEntryDTO massiveEntryDTO)
        {
            try
            {
                // Convertir de model a entity
                GENERALLEDGEREN.EntryMassiveLoad entryMassiveLoadEntity = EntityAssembler.CreateEntryMassiveLoad(massiveEntryDTO);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(entryMassiveLoadEntity);

                // Return del model
                return ModelAssembler.CreateEntryMassiveLoad(entryMassiveLoadEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Delete

        /// <summary>
        /// DeleteEntryMassiveLoad
        /// </summary>
        /// <param name="entryMassiveLoadId"></param>
        /// <returns></returns>
        public bool DeleteEntryMassiveLoad(int entryMassiveLoadId)
        {
            bool isDeleted = false;

            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.EntryMassiveLoad.CreatePrimaryKey(entryMassiveLoadId);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.EntryMassiveLoad entryMassiveLoadEntity = (GENERALLEDGEREN.EntryMassiveLoad)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(entryMassiveLoadEntity);

                isDeleted = true;
            }
            catch
            {
                isDeleted = false;
            }

            return isDeleted;
        }

        #endregion Delete

        #region Get

        /// <summary>
        /// GetEntryMassiveLoads
        /// </summary>
        /// <returns></returns>
        public List<MassiveEntryDTO> GetEntryMassiveLoads()
        {
            try
            {
                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.EntryMassiveLoad)));

                // Return como Lista
                return ModelAssembler.CreateEntryMassiveLoads(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Get
    }
}
