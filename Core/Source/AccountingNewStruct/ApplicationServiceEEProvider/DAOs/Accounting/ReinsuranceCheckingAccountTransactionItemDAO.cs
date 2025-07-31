//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    class ReinsuranceCheckingAccountTransactionItemDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        ///<summary>
        /// SaveReinsuranceCheckingAccountTransactionItem
        /// </summary>
        /// <param name="reinsuranceCheckingAccountTransactionItem"></param>
        /// <param name="imputationId"></param>
        /// <param name="tempReinsuranceParentId"></param>
        /// <returns>ReInsuranceCheckingAccountTransactionItem</returns>
        public ReInsuranceCheckingAccountTransactionItem SaveReinsuranceCheckingAccountTransactionItem(ReInsuranceCheckingAccountTransactionItem reinsuranceCheckingAccountTransactionItem, int imputationId, int tempReinsuranceParentId)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.ReinsCheckingAccTrans reinsuranceCheckingAccountEntity = EntityAssembler.CreateReinsuranceCheckingAccount(reinsuranceCheckingAccountTransactionItem, imputationId, tempReinsuranceParentId);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(reinsuranceCheckingAccountEntity);

                // Return del model
                return ModelAssembler.CreateReinsuranceCheckingAccountTransactionItem(reinsuranceCheckingAccountEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        ///<summary>
        /// UpdateReinsuranceCheckingAccountTransactionItem
        /// </summary>
        /// <param name="reinsuranceCheckingAccountTransactionItem"></param>
        /// <param name="reinsuranceParentId"></param>
        /// <returns>ReInsuranceCheckingAccountTransactionItem</returns>
        public ReInsuranceCheckingAccountTransactionItem UpdateReinsuranceCheckingAccountTransactionItem(ReInsuranceCheckingAccountTransactionItem reinsuranceCheckingAccountTransactionItem, int reinsuranceParentId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.ReinsCheckingAccTrans.CreatePrimaryKey(reinsuranceCheckingAccountTransactionItem.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.ReinsCheckingAccTrans reinsuranceCheckingAccountEntity = (ACCOUNTINGEN.ReinsCheckingAccTrans)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                reinsuranceCheckingAccountEntity.ReinsuranceParentCode = reinsuranceParentId;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(reinsuranceCheckingAccountEntity);

                // Return del model
                return ModelAssembler.CreateReinsuranceCheckingAccountTransactionItem(reinsuranceCheckingAccountEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

    }
}
