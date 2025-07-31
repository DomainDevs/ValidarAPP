//Sistran Core
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Assemblers;
using GeneralLedgerModels = Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models;
using GENERALLEDGEREN = Sistran.Core.Application.GeneralLedger.Entities;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;

using System;
using System.Collections.Generic;

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.DAOs
{
    internal class AnalysisDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Save

        /// <summary>
        /// SaveAnalysis
        /// </summary>
        /// <param name="analysis"></param>
        /// <param name="entryItemId"></param>
        /// <param name="correlativeNumber"></param>
        /// <param name="isJournalEntry"></param>
        /// <returns></returns>
        public GeneralLedgerModels.Analysis SaveAnalysis(GeneralLedgerModels.Analysis analysis, int entryItemId, int correlativeNumber, bool isJournalEntry)
        {
            try
            {
                // Convertir de model a entity
                GENERALLEDGEREN.AnalysisEntryItem analysisEntryItemEntity = EntityAssembler.CreateAnalysisEntryItem(analysis, entryItemId, correlativeNumber, isJournalEntry);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(analysisEntryItemEntity);

                // Return del model
                return ModelAssembler.CreateAnalysis(analysisEntryItemEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// UpdateAnalysis
        /// </summary>
        /// <param name="analysis"></param>
        /// <param name="entryItemId"></param>
        /// <param name="correlativeNumber"></param>
        /// <param name="isJournalEntry"></param>
        public void UpdateAnalysis(GeneralLedgerModels.Analysis analysis, int entryItemId, int correlativeNumber, bool isJournalEntry)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.AnalysisEntryItem.CreatePrimaryKey(analysis.AnalysisId);

                // Encuentra el objeto en referencia a la llave primaria
                GENERALLEDGEREN.AnalysisEntryItem analysisEntryItemEntity = (GENERALLEDGEREN.AnalysisEntryItem)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                analysisEntryItemEntity.AnalysisId = analysis.AnalysisConcept.AnalysisCode.AnalysisCodeId;
                analysisEntryItemEntity.AnalysisConceptId = analysis.AnalysisConcept.AnalysisConceptId;
                analysisEntryItemEntity.EntryItemId = entryItemId;
                analysisEntryItemEntity.ConceptKey = analysis.Key;
                analysisEntryItemEntity.CorrelativeNumber = correlativeNumber;
                analysisEntryItemEntity.IsJournalEntry = isJournalEntry;
                analysisEntryItemEntity.Description = analysis.Description;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(analysisEntryItemEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        } 

        #endregion

        #region Delete

        /// <summary>
        /// DeleteAnalysis
        /// </summary>
        /// <param name="analysisId"></param>
        public void DeleteAnalysis(int analysisId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.AnalysisEntryItem.CreatePrimaryKey(analysisId);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.AnalysisEntryItem analysisEntryItemEntity = (GENERALLEDGEREN.AnalysisEntryItem)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(analysisEntryItemEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        } 

        #endregion

        #region Get

        /// <summary>
        /// GetAnalysis
        /// </summary>
        /// <param name="analysisId"></param>
        /// <returns></returns>
        public GeneralLedgerModels.Analysis GetAnalysis(int analysisId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.AnalysisEntryItem.CreatePrimaryKey(analysisId);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.AnalysisEntryItem analysisEntryItemEntity = (GENERALLEDGEREN.AnalysisEntryItem)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreateAnalysis(analysisEntryItemEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetAnalyses
        /// </summary>
        /// <returns></returns>
        public List<GeneralLedgerModels.Analysis> GetAnalyses()
        {
            try
            {
                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.AnalysisEntryItem)));

                // Return como Lista
                return ModelAssembler.CreateAnalyses(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
 
        #endregion
    }
}
