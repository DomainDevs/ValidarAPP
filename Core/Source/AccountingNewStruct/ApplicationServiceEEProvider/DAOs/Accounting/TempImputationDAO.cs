using System;
using System.Collections.Generic;
using Sistran.Core.Framework.Queries;
using System.Linq;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class TempImputationDAO
    {
        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #region TempImputationDAO

        /// <summary>
        /// SaveTempImputation
        /// </summary>
        /// <param name="imputation"></param>
        /// <param name="sourceCode"></param>
        /// <returns>Imputation</returns>
        public Imputation SaveTempImputation(Imputation imputation, int sourceCode)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.TempImputation imputationEntity = EntityAssembler.CreateTempImputation(imputation, sourceCode);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(imputationEntity);

                // Return del model
                return ModelAssembler.CreateTempImputation(imputationEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateTempImputation
        /// </summary>
        /// <param name="imputation"></param>
        /// <param name="sourceCode"></param>
        /// <returns>Imputation</returns>
        public Imputation UpdateTempImputation(Imputation imputation, int sourceCode)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.TempImputation.CreatePrimaryKey(imputation.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.TempImputation imputationEntity = (ACCOUNTINGEN.TempImputation)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                if (!imputation.IsTemporal)
                {
                    imputationEntity.ImputationTypeCode = Convert.ToInt32(imputation.ImputationType);
                    imputationEntity.RegisterDate = imputation.Date;
                    imputationEntity.UserId = imputation.UserId;
                }
                else
                {
                    imputationEntity.SourceCode = sourceCode;
                    imputationEntity.IsRealSource = imputation.IsTemporal; 
                }

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(imputationEntity);
                
                // Return del model
                return ModelAssembler.CreateTempImputation(imputationEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteTempImputation
        /// </summary>
        /// <param name="imputationId"></param>
        /// <returns>bool</returns>
        public bool DeleteTempImputation(int imputationId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.TempImputation.CreatePrimaryKey(imputationId);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.TempImputation imputationEntity = (ACCOUNTINGEN.TempImputation)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                if (imputationEntity != null)
                {
                    // Realizar las operaciones con los entities utilizando DAF
                    _dataFacadeManager.GetDataFacade().DeleteObject(imputationEntity);
                }

                return true;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetTempImputation
        /// </summary>
        /// <param name="tempImputation"></param>
        /// <returns>Imputation</returns>
        public Imputation GetTempImputation(Imputation tempImputation)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.TempImputation.CreatePrimaryKey(tempImputation.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.TempImputation tempImputationEntity = (ACCOUNTINGEN.TempImputation)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                if (tempImputationEntity !=null)
                {
                    // Return del model
                    return ModelAssembler.CreateTempImputation(tempImputationEntity);
                }

                tempImputation.Id = 0;
                return tempImputation;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion
    }
}
