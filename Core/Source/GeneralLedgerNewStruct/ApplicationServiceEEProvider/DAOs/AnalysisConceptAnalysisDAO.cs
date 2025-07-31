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
using Sistran.Core.Framework.Queries;

using System;
using System.Collections.Generic;
using System.Linq;
#endregion Using

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.DAOs
{
    class AnalysisConceptAnalysisDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Save

        /// <summary>
        /// SaveAnalysisConceptAnalysis
        /// </summary>
        /// <param name="analysisId"></param>
        /// <param name="analysisConceptId"></param>
        public void SaveAnalysisConceptAnalysis(int analysisId, int analysisConceptId)
        {
            try
            {
                // Convertir de model a entity
                GENERALLEDGEREN.AnalysisConceptAnalysis analysisConceptAnalysisEntity = EntityAssembler.CreateAnalysisConceptAnalysis(analysisId, analysisConceptId);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(analysisConceptAnalysisEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Save

        #region Update

        /// <summary>
        /// UpdateAnalysisConceptAnalysis
        /// </summary>
        /// <param name="analysisConceptAnalysisId"></param>
        /// <param name="analysisId"></param>
        /// <param name="analysisConceptId"></param>
        public void UpdateAnalysisConceptAnalysis(int analysisConceptAnalysisId, int analysisId, int analysisConceptId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.Analysis.CreatePrimaryKey(analysisConceptAnalysisId);

                // Encuentra el objeto en referencia a la llave primaria
                GENERALLEDGEREN.AnalysisConceptAnalysis analysisConceptAnalysisEntity = (GENERALLEDGEREN.AnalysisConceptAnalysis)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                analysisConceptAnalysisEntity.AnalysisId = analysisId;
                analysisConceptAnalysisEntity.AnalysisConceptId = analysisConceptId;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(analysisConceptAnalysisEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Update

        #region Delete

        /// <summary>
        /// DeleteAnalysisConceptAnalysis
        /// </summary>
        /// <param name="analysisConceptAnalysisId"></param>
        public void DeleteAnalysisConceptAnalysis(int analysisConceptAnalysisId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.AnalysisConceptAnalysis.CreatePrimaryKey(analysisConceptAnalysisId);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.AnalysisConceptAnalysis analysisConceptAnalysisEntity = (GENERALLEDGEREN.AnalysisConceptAnalysis)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(analysisConceptAnalysisEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Delete

        #region Get

        /// <summary>
        /// GetAnalysisConceptAnalysis
        /// </summary>
        /// <param name="analysisConceptAnalysisId"></param>
        /// <returns></returns>
        public List<AnalysisConceptAnalysisDTO> GetAnalysisConceptAnalysis(int analysisConceptAnalysisId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AnalysisConceptAnalysis.Properties.AnalysisConceptAnalysisId, analysisConceptAnalysisId);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(GENERALLEDGEREN.AnalysisConceptAnalysis), criteriaBuilder.GetPredicate()));

                List<AnalysisConceptAnalysisDTO> analysisConcepts = new List<AnalysisConceptAnalysisDTO>();

                foreach (GENERALLEDGEREN.AnalysisConceptAnalysis analysisConceptAnalysisEntity in businessCollection.OfType<GENERALLEDGEREN.AnalysisConceptAnalysis>())
                {
                    AnalysisConceptAnalysisDTO analysisConcept = new AnalysisConceptAnalysisDTO();
                    analysisConcept.AnalysisConceptAnalysisId = analysisConceptAnalysisEntity.AnalysisConceptAnalysisId;
                    analysisConcept.AnalysisConceptId = Convert.ToInt32(analysisConceptAnalysisEntity.AnalysisConceptId);
                    analysisConcept.AnalysisId = Convert.ToInt32(analysisConceptAnalysisEntity.AnalysisId);
                    analysisConcepts.Add(analysisConcept);
                }

                return analysisConcepts;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetAnalysisConceptAnalyses
        /// </summary>
        /// <returns></returns>
        public List<AnalysisConceptAnalysisDTO> GetAnalysisConceptAnalyses()
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.Property(GENERALLEDGEREN.AnalysisConceptAnalysis.Properties.AnalysisConceptAnalysisId);
                criteriaBuilder.GreaterEqual();
                criteriaBuilder.Constant(0);

                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(GENERALLEDGEREN.AnalysisConceptAnalysis), criteriaBuilder.GetPredicate()));

                // Asignamos BusinessCollection a un ArrayList
                List<AnalysisConceptAnalysisDTO> analysisConcepts = new List<AnalysisConceptAnalysisDTO>();

                foreach (GENERALLEDGEREN.AnalysisConceptAnalysis analysisConceptAnalysisEntity in businessCollection.OfType<GENERALLEDGEREN.AnalysisConceptAnalysis>())
                {
                    AnalysisConceptAnalysisDTO analysisConcept = new AnalysisConceptAnalysisDTO();
                    analysisConcept.AnalysisConceptAnalysisId = analysisConceptAnalysisEntity.AnalysisConceptAnalysisId;
                    analysisConcept.AnalysisConceptId = Convert.ToInt32(analysisConceptAnalysisEntity.AnalysisConceptId);
                    analysisConcept.AnalysisId = Convert.ToInt32(analysisConceptAnalysisEntity.AnalysisId);
                    analysisConcepts.Add(analysisConcept);
                }

                return analysisConcepts;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Get
    }
}
