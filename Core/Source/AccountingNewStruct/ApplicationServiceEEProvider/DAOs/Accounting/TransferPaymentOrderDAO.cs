//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;

//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Models.AccountsPayables;
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    class TransferPaymentOrderDAO
    {
        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        /// <summary>
        /// SaveTransferPaymentOrder
        /// </summary>
        /// <param name="transferPaymentOrder"></param>
        /// <returns>TransferPaymentOrder</returns>
        public TransferPaymentOrder SaveTransferPaymentOrder(TransferPaymentOrder transferPaymentOrder)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.TransferPaymentOrder transferPaymentOrderEntity = EntityAssembler.CreateTransferPaymentOrder(transferPaymentOrder);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(transferPaymentOrderEntity);

                // Return del model
                return ModelAssembler.CreateTransferPaymentOrder(transferPaymentOrderEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateTransferPaymentOrder
        /// </summary>
        /// <param name="transferPaymentOrder"></param>
        /// <returns>TransferPaymentOrder</returns>
        public TransferPaymentOrder UpdateTransferPaymentOrder(TransferPaymentOrder transferPaymentOrder)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.TransferPaymentOrder.CreatePrimaryKey(transferPaymentOrder.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.TransferPaymentOrder transferPaymentOrderEntity = (ACCOUNTINGEN.TransferPaymentOrder)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                transferPaymentOrderEntity.AccountBankCode = transferPaymentOrder.BankAccountCompany.Id;
                transferPaymentOrderEntity.DeliveryDate = transferPaymentOrder.DeliveryDate;
                transferPaymentOrderEntity.Status = transferPaymentOrder.Status;
                transferPaymentOrderEntity.CancellationDate = transferPaymentOrder.CancellationDate;
                transferPaymentOrderEntity.User = transferPaymentOrder.UserId;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(transferPaymentOrderEntity);

                // Return del model
                return ModelAssembler.CreateTransferPaymentOrder(transferPaymentOrderEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetTransferPaymentOrder
        /// </summary>
        /// <param name="transferPaymentOrder"></param>
        /// <returns>TransferPaymentOrder</returns>
        public TransferPaymentOrder GetTransferPaymentOrder(TransferPaymentOrder transferPaymentOrder)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.TransferPaymentOrder.CreatePrimaryKey(transferPaymentOrder.Id);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.TransferPaymentOrder transferPaymentOrderEntity = (ACCOUNTINGEN.TransferPaymentOrder)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                if (transferPaymentOrderEntity != null)
                {
                    // Return del model
                    return ModelAssembler.CreateTransferPaymentOrder(transferPaymentOrderEntity);
                }

                return null;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}
