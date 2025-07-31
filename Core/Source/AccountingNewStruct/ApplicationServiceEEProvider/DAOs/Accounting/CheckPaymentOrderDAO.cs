using System;

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
    public class CheckPaymentOrderDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        /// <summary>
        /// SaveCheckPaymentOrder
        /// </summary>
        /// <param name="checkPaymentOrder"></param>
        /// <returns>CheckBookControl</returns>
        public CheckPaymentOrder SaveCheckPaymentOrder(CheckPaymentOrder checkPaymentOrder)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.CheckPaymentOrder checkPaymentOrderEntity = EntityAssembler.CreateCheckPaymentOrder(checkPaymentOrder);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(checkPaymentOrderEntity);

                // Return del model
                return ModelAssembler.CreateCheckPaymentOrder(checkPaymentOrderEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateCheckPaymentOrder
        /// </summary>
        /// <param name="checkPaymentOrder"></param>
        /// <returns>CheckBookControl</returns>
        public CheckPaymentOrder UpdateCheckPaymentOrder(CheckPaymentOrder checkPaymentOrder)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.CheckPaymentOrder.CreatePrimaryKey(checkPaymentOrder.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.CheckPaymentOrder checkPaymentOrderEntity = (ACCOUNTINGEN.CheckPaymentOrder)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                checkPaymentOrderEntity.AccountBankCode = checkPaymentOrder.BankAccountCompany.Id;
                checkPaymentOrderEntity.CheckNumber = Convert.ToInt32(checkPaymentOrder.CheckNumber);
                checkPaymentOrderEntity.IsCheckPrinted = checkPaymentOrder.IsCheckPrinted;
                checkPaymentOrderEntity.PrintedUserId = checkPaymentOrder.PrintedUser;
                checkPaymentOrderEntity.PrintedDate = checkPaymentOrder.PrintedDate;
                checkPaymentOrderEntity.DeliveryDate = checkPaymentOrder.DeliveryDate;
                checkPaymentOrderEntity.PersonTypeCode = checkPaymentOrder.PersonType.Id;
                checkPaymentOrderEntity.IndividualId = checkPaymentOrder.Delivery.IndividualId;
                checkPaymentOrderEntity.Status = checkPaymentOrder.Status;
                checkPaymentOrderEntity.CourierName = checkPaymentOrder.CourierName;
                checkPaymentOrderEntity.CancellationDate = checkPaymentOrder.CancellationDate;
                checkPaymentOrderEntity.CancellationUserId = checkPaymentOrder.CancellationUser;
                checkPaymentOrderEntity.RefundDate = checkPaymentOrder.RefundDate;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(checkPaymentOrderEntity);

                // Return del model
                return ModelAssembler.CreateCheckPaymentOrder(checkPaymentOrderEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetCheckPaymentOrder
        /// </summary>
        /// <param name="checkPaymentOrder"></param>
        /// <returns>CheckPaymentOrder</returns>
        public CheckPaymentOrder GetCheckPaymentOrder(CheckPaymentOrder checkPaymentOrder)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.CheckPaymentOrder.CreatePrimaryKey(checkPaymentOrder.Id);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.CheckPaymentOrder checkPaymentOrderEntity = (ACCOUNTINGEN.CheckPaymentOrder)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreateCheckPaymentOrder(checkPaymentOrderEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}
