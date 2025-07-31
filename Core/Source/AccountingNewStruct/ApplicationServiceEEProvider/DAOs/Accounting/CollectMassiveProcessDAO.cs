//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.AccountingServices.DTOs.Search;
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using System;


namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
   public class CollectMassiveProcessDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        /// <summary>
        /// SaveCollectMassiveProcess
        /// </summary>
        /// <param name="collectMassiveProcess"></param>
        /// <returns>CollectMassiveProcessDTO</returns>
        public CollectMassiveProcessDTO SaveCollectMassiveProcess(CollectMassiveProcessDTO collectMassiveProcess)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.CollectMassiveProcess collectMassiveProcessEntity = EntityAssembler.CreateBillMassiveProcess(collectMassiveProcess);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(collectMassiveProcessEntity);

                // Return del model
                return ModelAssembler.CreateCollectMassiveProcess(collectMassiveProcessEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateCollectMassiveProcess
        /// </summary>
        /// <param name="collectMassiveProcess"></param>
        /// <returns>CollectMassiveProcessDTO</returns>
        public CollectMassiveProcessDTO UpdateCollectMassiveProcess(CollectMassiveProcessDTO collectMassiveProcess)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.CollectMassiveProcess.CreatePrimaryKey(collectMassiveProcess.CollectMassiveProcessId);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.CollectMassiveProcess collectMassiveProcessEntity = (ACCOUNTINGEN.CollectMassiveProcess)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                DateTime? endDate;

                if (collectMassiveProcess.EndDate == Convert.ToDateTime("01/01/0001 0:00:00"))
                {
                    endDate = null;
                }
                else
                {
                    endDate = collectMassiveProcess.EndDate;
                }

                collectMassiveProcessEntity.BeginDate = collectMassiveProcess.BeginDate;
                collectMassiveProcessEntity.EndDate = endDate;
                collectMassiveProcessEntity.UserId = collectMassiveProcess.UserId;
                collectMassiveProcessEntity.Status = collectMassiveProcess.Status;
                collectMassiveProcessEntity.TotalRecords = collectMassiveProcess.TotalRecords;
                collectMassiveProcessEntity.FailedRecords = collectMassiveProcess.FailedRecords;
                collectMassiveProcessEntity.SuccessfulRecords = collectMassiveProcess.SuccessfulRecords;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(collectMassiveProcessEntity);

                // Return del model
                return ModelAssembler.CreateCollectMassiveProcess(collectMassiveProcessEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}
