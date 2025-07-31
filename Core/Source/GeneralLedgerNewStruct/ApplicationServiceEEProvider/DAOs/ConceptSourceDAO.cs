#region Using

//Sistran Core
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models.AccountingConcepts;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Assemblers;
using GENERALLEDGEREN = Sistran.Core.Application.GeneralLedger.Entities;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;

using System;
using System.Collections.Generic;
using System.Linq;

#endregion Using

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.DAOs
{
    public class ConceptSourceDAO
    {
        
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Save

        /// <summary>
        /// SaveConceptSource
        /// </summary>
        /// <param name="conceptSource"></param>
        /// <returns></returns>
        public ConceptSource SaveConceptSource(ConceptSource conceptSource)
        {
            try
            {
                // recuperar todos los registros
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.ConceptSource)));
                
                var conceptSourceId = 0;
                if (businessCollection.Count > 0)
                {
                    var maxNumber = (from GENERALLEDGEREN.ConceptSource conceptSourceEntityFind in businessCollection select conceptSourceEntityFind.ConceptSourceCode).Max();
                    conceptSourceId = Convert.ToInt32(maxNumber);
                    conceptSourceId = conceptSourceId + 1;
                }
                else
                {
                    conceptSourceId = 1;
                }
                conceptSource.Id = conceptSourceId;

                // Convertir de model a entity
                GENERALLEDGEREN.ConceptSource conceptSourceEntity = EntityAssembler.CreateConceptSource(conceptSource);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(conceptSourceEntity);

                // Return del model
                return ModelAssembler.CreateConceptSource(conceptSourceEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// UpdateConceptSource
        /// </summary>
        /// <param name="conceptSource"></param>
        /// <returns></returns>
        public ConceptSource UpdateConceptSource(ConceptSource conceptSource)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.ConceptSource.CreatePrimaryKey(conceptSource.Id);

                // Encuentra el objeto en referencia a la llave primaria
                GENERALLEDGEREN.ConceptSource conceptSourceEntity = (GENERALLEDGEREN.ConceptSource)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));
                conceptSourceEntity.Description = conceptSource.Description;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(conceptSourceEntity);

                // Return del model
                return ModelAssembler.CreateConceptSource(conceptSourceEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Delete

        /// <summary>
        /// DeleteConceptSource
        /// </summary>
        /// <param name="conceptSource"></param>
        /// <returns></returns>
        public bool DeleteConceptSource(ConceptSource conceptSource)
        {
            bool isDeleted = false;

            try
            {   // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.ConceptSource.CreatePrimaryKey(conceptSource.Id);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.ConceptSource conceptSourceEntity = (GENERALLEDGEREN.ConceptSource)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(conceptSourceEntity);

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
        /// GetConceptSource
        /// </summary>
        /// <param name="conceptSource"></param>
        /// <returns></returns>
        public ConceptSource GetConceptSource(ConceptSource conceptSource)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.ConceptSource.CreatePrimaryKey(conceptSource.Id);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.ConceptSource conceptSourceEntity = (GENERALLEDGEREN.ConceptSource)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreateConceptSource(conceptSourceEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetConceptSources
        /// </summary>
        /// <returns></returns>
        public List<ConceptSource> GetConceptSources()
        {
            try
            {
                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.ConceptSource)));

                // Return como Lista
                return ModelAssembler.CreateConceptSources(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion
    }
}
