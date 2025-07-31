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

using System;
using System.Collections.Generic;

#endregion Using

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.DAOs
{
    public class AnalysisConceptDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Save

        /// <summary>
        /// SaveAnalysisConcept
        /// </summary>
        /// <param name="analysisConcept"></param>
        /// <returns></returns>
        public AnalysisConcept SaveAnalysisConcept(AnalysisConcept analysisConcept)
        {
            try
            {
                // Convertir de model a entity
                GENERALLEDGEREN.AnalysisConcept analysisConceptEntity = EntityAssembler.CreateAnalysisConcept(analysisConcept);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(analysisConceptEntity);

                // Return del model
                return ModelAssembler.CreateAnalysisConcept(analysisConceptEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// UpdateAnalysisConcept
        /// </summary>
        /// <param name="analysisConcept"></param>
        /// <returns></returns>
        public AnalysisConcept UpdateAnalysisConcept(AnalysisConcept analysisConcept)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.AnalysisConcept.CreatePrimaryKey(analysisConcept.AnalysisConceptId);

                // Encuentra el objeto en referencia a la llave primaria
                GENERALLEDGEREN.AnalysisConcept analysisConceptEntity = (GENERALLEDGEREN.AnalysisConcept)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                analysisConceptEntity.Description = analysisConcept.Description;
                analysisConceptEntity.AnalysisTreatmentId = analysisConcept.AnalysisTreatment.AnalysisTreatmentId;
               
                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(analysisConceptEntity);

                // Return del model
                return ModelAssembler.CreateAnalysisConcept(analysisConceptEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Delete

        /// <summary>
        /// DeleteAnalysisConcept
        /// </summary>
        /// <param name="analysisConceptId"></param>
        public void DeleteAnalysisConcept(int analysisConceptId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.AnalysisConcept.CreatePrimaryKey(analysisConceptId);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.AnalysisConcept analysisConceptEntity = (GENERALLEDGEREN.AnalysisConcept)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(analysisConceptEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Get

        /// <summary>
        /// GetAnalysisConcept
        /// </summary>
        /// <param name="analysisConceptId"></param>
        /// <returns></returns>
        public AnalysisConcept GetAnalysisConcept(int analysisConceptId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.AnalysisConcept.CreatePrimaryKey(analysisConceptId);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.AnalysisConcept analysisConceptEntity = (GENERALLEDGEREN.AnalysisConcept)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreateAnalysisConcept(analysisConceptEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetAnalysisConcepts
        /// </summary>
        /// <returns></returns>
        public List<AnalysisConcept> GetAnalysisConcepts()
        {
            try
            {
                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.AnalysisConcept)));

                // Return como Lista
                return ModelAssembler.CreateAnalysisConcepts(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion
    }
}
