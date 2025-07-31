using System;
using System.Linq;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Contexts;

//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Collect;
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Framework.Queries;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class ImputationDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        /// <summary>
        /// SaveImputation
        /// </summary>
        /// <param name="imputation"></param>
        /// <param name="sourceCode"></param>
        /// <param name="TechnicalTransaction"></param>
        /// <returns>Imputation</returns>
        public Imputation SaveImputation(Imputation imputation, int sourceCode, int TechnicalTransaction)
        {
            try
            {
       
                    // Convertir de model a entity
                    ACCOUNTINGEN.Imputation imputationEntity = EntityAssembler.CreateImputation(imputation, sourceCode, TechnicalTransaction);

                    // Realizar las operaciones con los entities utilizando DAF
                    _dataFacadeManager.GetDataFacade().InsertObject(imputationEntity);

                    // Return del model
                    return ModelAssembler.CreateImputation(imputationEntity);

        


            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetImputation
        /// </summary>
        /// <param name="imputation"></param>
        /// <returns>CollectImputation</returns>
        public CollectImputation GetImputation(Imputation imputation)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.Imputation.CreatePrimaryKey(imputation.Id);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.Imputation imputationEntity = (ACCOUNTINGEN.Imputation)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreateCollectImputation(imputationEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateImputation
        /// </summary>
        /// <param name="CollectImputation"></param>
        /// <returns>CollectImputation</returns>
        public CollectImputation UpdateImputation(CollectImputation CollectImputation)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.Imputation.CreatePrimaryKey(CollectImputation.Imputation.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.Imputation imputationEntity = (ACCOUNTINGEN.Imputation)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                imputationEntity.ImputationTypeCode = Convert.ToInt32(CollectImputation.Imputation.ImputationType);
                imputationEntity.SourceCode = CollectImputation.Collect.Id;
                imputationEntity.RegisterDate = CollectImputation.Imputation.Date;
                imputationEntity.UserId = CollectImputation.Imputation.UserId;
                imputationEntity.TechnicalTransaction = CollectImputation.Transaction.TechnicalTransaction;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(imputationEntity);

                // Return del model
                return ModelAssembler.CreateCollectImputation(imputationEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetImputationBySourceType
        /// Obtiene el Id de imputaciòn dado el source y el tipo
        /// </summary>
        /// <param name="source"></param>
        /// <param name="type"></param>
        /// <returns>int</returns>
        public int GetImputationBySourceType(int source, int type)
        {
            try
            {
                int imputationId = 0;

                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Imputation.Properties.SourceCode, source);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Imputation.Properties.ImputationTypeCode, type);

                BusinessCollection imputationCollection = new BusinessCollection(
                    _dataFacadeManager.GetDataFacade().SelectObjects(typeof(
                    ACCOUNTINGEN.Imputation), criteriaBuilder.GetPredicate()));

                if (imputationCollection.Count > 0)
                {
                    imputationId = Convert.ToInt32(imputationCollection.OfType<ACCOUNTINGEN.Imputation>().First().ImputationCode);
                }

                return imputationId;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}
