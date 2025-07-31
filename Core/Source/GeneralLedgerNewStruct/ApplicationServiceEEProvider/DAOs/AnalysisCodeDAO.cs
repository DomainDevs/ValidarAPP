#region Using

//Sistran Core
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Assemblers;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;

using System;
using System.Collections.Generic;
using GENERALLEDGEREN = Sistran.Core.Application.GeneralLedger.Entities;

#endregion

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.DAOs
{
    internal class AnalysisCodeDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Save

        /// <summary>
        /// SaveAnalysisCode
        /// </summary>
        /// <param name="analysisCode"></param>
        /// <returns></returns>
        public AnalysisCode SaveAnalysisCode(AnalysisCode analysisCode)
        {
            try
            {
                // Convertir de model a entity
                GENERALLEDGEREN.Analysis analysisEntity = EntityAssembler.CreateAnalysis(analysisCode);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(analysisEntity);

                // Return del model
                return ModelAssembler.CreateAnalysisCode(analysisEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
        
        #endregion

        #region Update

        /// <summary>
        /// UpdateAnalysisCode
        /// </summary>
        /// <param name="analysisCode"></param>
        /// <returns></returns>
        public AnalysisCode UpdateAnalysisCode(AnalysisCode analysisCode)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.Analysis.CreatePrimaryKey(analysisCode.AnalysisCodeId);

                // Encuentra el objeto en referencia a la llave primaria
                GENERALLEDGEREN.Analysis analysisEntity = (GENERALLEDGEREN.Analysis)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                analysisEntity.Description = analysisCode.Description;
                analysisEntity.AccountingNature = Convert.ToInt32(analysisCode.AccountingNature);
                analysisEntity.RequireOrigin = Convert.ToBoolean(analysisCode.CheckModuleType);
                analysisEntity.ControlBalance = Convert.ToBoolean(analysisCode.CheckBalance);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(analysisEntity);

                // Return del model
                return ModelAssembler.CreateAnalysisCode(analysisEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
         
        #endregion

        #region Delete

        /// <summary>
        /// DeleteAnalysisCode
        /// </summary>
        /// <param name="analysisId"></param>
        public void DeleteAnalysisCode(int analysisId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.Analysis.CreatePrimaryKey(analysisId);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.Analysis analysisEntity = (GENERALLEDGEREN.Analysis)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(analysisEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
         
        #endregion

        #region Get

        /// <summary>
        /// GetAnalysisCode
        /// </summary>
        /// <param name="analysisCodeId"></param>
        /// <returns></returns>
        public AnalysisCode GetAnalysisCode(int analysisCodeId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.Analysis.CreatePrimaryKey(analysisCodeId);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.Analysis analysisEntity = (GENERALLEDGEREN.Analysis)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreateAnalysisCode(analysisEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetAnalysisCodes
        /// </summary>
        /// <returns></returns>
        public List<AnalysisCode> GetAnalysisCodes()
        {
            try
            {
                // Asignamos BusinessCollection a una Lista
                var businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.Analysis)));
                
                // Return  como Lista
                return ModelAssembler.CreateAnalysisCodes(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
 
        #endregion
    }
}
