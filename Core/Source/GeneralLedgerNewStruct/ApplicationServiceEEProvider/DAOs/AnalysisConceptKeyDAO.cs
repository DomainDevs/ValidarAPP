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

#endregion Using

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.DAOs
{
    public class AnalysisConceptKeyDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Save
        /// <summary>
        ///  SaveAnalysisConceptKey
        /// </summary>
        /// <param name="analysisConceptKey"></param>
        /// <returns></returns>        
        public AnalysisConceptKey SaveAnalysisConceptKey(AnalysisConceptKey analysisConceptKey)
        {
            try
            {
                // Convertir de model a entity
                GENERALLEDGEREN.AnalysisConceptKey analysisConceptKeyEntity = EntityAssembler.CreateAnalysisConceptKey(analysisConceptKey);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(analysisConceptKeyEntity);

                // Return del model
                return ModelAssembler.CreateAnalysisConceptKey(analysisConceptKeyEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// UpdateAnalysisConceptKey
        /// </summary>
        /// <param name="analysisConceptKey"></param>
        /// <returns></returns>
        public AnalysisConceptKey UpdateAnalysisConceptKey(AnalysisConceptKey analysisConceptKey)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.AnalysisConceptKey.CreatePrimaryKey(analysisConceptKey.Id, analysisConceptKey.AnalysisConcept.AnalysisConceptId);

                // Encuentra el objeto en referencia a la llave primaria
                GENERALLEDGEREN.AnalysisConceptKey analysisConceptKeyEntity = (GENERALLEDGEREN.AnalysisConceptKey)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));
                analysisConceptKeyEntity.Table = analysisConceptKey.TableName;
                analysisConceptKeyEntity.Column = analysisConceptKey.ColumnName;
                analysisConceptKeyEntity.ColumnDescription = analysisConceptKey.ColumnDescription;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(analysisConceptKeyEntity);

                // Return del model
                return ModelAssembler.CreateAnalysisConceptKey(analysisConceptKeyEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Delete
        /// <summary>
        /// DeleteAnalysisConceptKey
        /// </summary>
        /// <param name="analysisConceptKey"></param>
        public void DeleteAnalysisConceptKey(AnalysisConceptKey analysisConceptKey)
        {
            try

            {
                
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.AnalysisConceptKey.CreatePrimaryKey(analysisConceptKey.Id, analysisConceptKey.AnalysisConcept.AnalysisConceptId);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.AnalysisConceptKey analysisConcepKeytEntity = (GENERALLEDGEREN.AnalysisConceptKey)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(analysisConcepKeytEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
        #endregion

        #region Get
        /// <summary>
        /// GetAnalysisConceptKey
        /// </summary>
        /// <param name="analysisConceptKey"></param>
        /// <returns></returns>
        public AnalysisConceptKey GetAnalysisConceptKey(AnalysisConceptKey analysisConceptKey)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.AnalysisConceptKey.CreatePrimaryKey(analysisConceptKey.Id, analysisConceptKey.AnalysisConcept.AnalysisConceptId);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.AnalysisConceptKey analysisConceptKeyEntity = (GENERALLEDGEREN.AnalysisConceptKey)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreateAnalysisConceptKey(analysisConceptKeyEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// List GetAnalysisConceptKeys
        /// </summary>
        /// <returns></returns>
        public List<AnalysisConceptKey> GetAnalysisConceptKeys()
        {
            try
            {
                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.AnalysisConceptKey)));

                // Return como Lista
                return ModelAssembler.CreateAnalysisConceptKeys(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
        
        public List<AnalysisConceptKey> GetAnalysisConceptKeysByAnalysisConcept(AnalysisConcept analysisConcept)
        {
            try
            {
                // AccountingAccountPrefix
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AnalysisConceptKey.Properties.AnalysisConceptId, analysisConcept.AnalysisConceptId);

                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                   typeof(GENERALLEDGEREN.AnalysisConceptKey), criteriaBuilder.GetPredicate()));

                // Return como Lista
                return ModelAssembler.CreateAnalysisConceptKeys(businessCollection);

            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

        }


        #endregion

    }
}
