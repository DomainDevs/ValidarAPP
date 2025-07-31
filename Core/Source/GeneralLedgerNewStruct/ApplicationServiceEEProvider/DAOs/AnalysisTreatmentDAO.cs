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
    public class AnalysisTreatmentDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Save

        /// <summary>
        /// SaveAnalysisTreatment
        /// </summary>
        /// <param name="analysisTreatment"></param>
        /// <returns></returns>
        public AnalysisTreatment SaveAnalysisTreatment(AnalysisTreatment analysisTreatment)
        {
            try
            {
                // Convertir de model a entity
                GENERALLEDGEREN.AnalysisTreatment analysisTreatmentEntity = EntityAssembler.CreateAnalysisTreatment(analysisTreatment);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(analysisTreatmentEntity);

                // Return del model
                return ModelAssembler.CreateAnalysisTreatment(analysisTreatmentEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Save

        #region Update

        /// <summary>
        /// UpdateAnalysisTreatment
        /// </summary>
        /// <param name="analysisTreatment"></param>
        /// <returns></returns>
        public AnalysisTreatment UpdateAnalysisTreatment(AnalysisTreatment analysisTreatment)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.AnalysisTreatment.CreatePrimaryKey(analysisTreatment.AnalysisTreatmentId);

                // Encuentra el objeto en referencia a la llave primaria
                GENERALLEDGEREN.AnalysisTreatment analysisTreatmentEntity = (GENERALLEDGEREN.AnalysisTreatment)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                analysisTreatmentEntity.Description = analysisTreatment.Description;
                
                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(analysisTreatmentEntity);

                // Return del model
                return ModelAssembler.CreateAnalysisTreatment(analysisTreatmentEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Update

        #region Delete

        /// <summary>
        /// DeleteAnalysisTreatment
        /// </summary>
        /// <param name="analysisTreatmentId"></param>
        public void DeleteAnalysisTreatment(int analysisTreatmentId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.AnalysisTreatment.CreatePrimaryKey(analysisTreatmentId);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.AnalysisTreatment analysisTreatmentEntity = (GENERALLEDGEREN.AnalysisTreatment)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(analysisTreatmentEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Delete

        #region Get

        /// <summary>
        /// GetAnalysisTreatment
        /// </summary>
        /// <param name="analysisTreatmentId"></param>
        /// <returns></returns>
        public AnalysisTreatment GetAnalysisTreatment(int analysisTreatmentId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.AnalysisTreatment.CreatePrimaryKey(analysisTreatmentId);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.AnalysisTreatment analysisTreatmentEntity = (GENERALLEDGEREN.AnalysisTreatment)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreateAnalysisTreatment(analysisTreatmentEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetAnalysisTreatments
        /// </summary>
        /// <returns></returns>
        public List<AnalysisTreatment> GetAnalysisTreatments()
        {
            try
            {
                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(GENERALLEDGEREN.AnalysisTreatment)));

                // Return como Lista
                return ModelAssembler.CreateAnalysisTreatments(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Get

    }
}
