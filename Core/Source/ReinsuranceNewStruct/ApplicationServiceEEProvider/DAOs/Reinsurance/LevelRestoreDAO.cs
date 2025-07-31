using System;
using System.Collections.Generic;
//Sistran FWK
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
//Sistran Core
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Assemblers;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance;
using REINSURANCEEN = Sistran.Core.Application.Reinsurance.Entities;
namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.DAOs.Reinsurance
{

    internal class LevelRestoreDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Save

        /// <summary>
        /// SaveLevelRestore
        /// </summary>
        /// <param name="levelRestore"></param>
        public void SaveLevelRestore(LevelRestore levelRestore)
        {
            // Convertir de model a entity
            REINSURANCEEN.LevelRestore entityLevelRestore = EntityAssembler.CreateLevelRestore(levelRestore);
            // Realizar las operaciones con los entities utilizando DAF
            _dataFacadeManager.GetDataFacade().InsertObject(entityLevelRestore);
        }

        #endregion

        #region Update

        /// <summary>
        /// UpdateLevelRestore
        /// </summary>
        /// <param name="levelRestore"></param>
        public void UpdateLevelRestore(LevelRestore levelRestore)
        {

            // Crea la Primary key con el código de la entidad
            PrimaryKey primaryKey = REINSURANCEEN.LevelRestore.CreatePrimaryKey(levelRestore.Id);

            // Encuentra el objeto en referencia a la llave primaria
            REINSURANCEEN.LevelRestore entityLevelRestore = (REINSURANCEEN.LevelRestore)
                (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

            entityLevelRestore.ContractLevelCode = levelRestore.Level.ContractLevelId;
            entityLevelRestore.ReestablishmentNumber = levelRestore.Number;
            entityLevelRestore.ReestablishmentPercentage = Convert.ToDecimal(levelRestore.RestorePercentage);
            entityLevelRestore.PointNoticePercentage = Convert.ToDecimal(levelRestore.NoticePercentage);
            // Realizar las operaciones con los entities utilizando DAF
            _dataFacadeManager.GetDataFacade().UpdateObject(entityLevelRestore);
        }
        #endregion

        #region Delete

        /// <summary>
        /// DeleteLevelRestore
        /// </summary>
        /// <param name="levelRestoreId"></param>
        public void DeleteLevelRestore(int levelRestoreId)
        {

            // Crea la Primary key con el código de la entidad
            PrimaryKey primaryKey = REINSURANCEEN.LevelRestore.CreatePrimaryKey(levelRestoreId);

            // Realizar las operaciones con los entities utilizando DAF
            REINSURANCEEN.LevelRestore entityLevelRestore = (REINSURANCEEN.LevelRestore)
                (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

            // Realizar las operaciones con los entities utilizando DAF
            _dataFacadeManager.GetDataFacade().DeleteObject(entityLevelRestore);
        }

        #endregion

        #region Get

        /// <summary>
        /// GetLevelRestore
        /// </summary>
        /// <param name="levelRestoreId"></param>
        /// <returns>LevelRestore</returns>
        public LevelRestore GetLevelRestore(int levelRestoreId)
        {

            // Crea la Primary key con el código de la entidad
            PrimaryKey primaryKey = REINSURANCEEN.LevelRestore.CreatePrimaryKey(levelRestoreId);

            // Realizar las operaciones con los entities utilizando DAF
            REINSURANCEEN.LevelRestore entityLevelRestore = (REINSURANCEEN.LevelRestore)
                (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

            // Retornar el model
            return ModelAssembler.CreateLevelRestore(entityLevelRestore);

        }

        /// <summary>
        /// GetLevelRestoresByLevelId
        /// </summary>
        /// <param name="levelId"></param>
        /// <returns>List<LevelRestore></returns>
        public List<LevelRestore> GetLevelRestoresByLevelId(int levelId)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(REINSURANCEEN.LevelRestore.Properties.ContractLevelCode, levelId);

            // Asignamos BusinessCollection a una Lista
            BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                                                    typeof(REINSURANCEEN.LevelRestore), criteriaBuilder.GetPredicate()));
            //Return como Lista
            return ModelAssembler.CreateLevelRestores(businessCollection);
        }

        #endregion Get
    }
}