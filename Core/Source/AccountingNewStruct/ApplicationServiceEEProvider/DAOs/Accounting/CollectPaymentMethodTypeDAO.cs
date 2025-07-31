//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
   public class CollectPaymentMethodTypeDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        /// <summary>
        /// SaveCollectPaymentMethodType
        /// </summary>
        /// <param name="id"></param>
        /// <param name="methodType"></param>
        /// <param name="enabledTicket"></param>
        /// <param name="enabledBilling"></param>
        /// <param name="enabledPaymentOrder"></param>
        /// <param name="enabledPaymentRequest"></param>
        /// <returns>SaveCollectPaymentMethodType</returns>
        public int SaveCollectPaymentMethodType(int id, int methodType, int enabledTicket, int enabledBilling,
                                             int enabledPaymentOrder, int enabledPaymentRequest)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.PaymentType collectPaymentMethodTypeEntity = EntityAssembler.CreateCollectPaymentMethodType(id,
                    methodType, enabledTicket, enabledBilling, enabledPaymentOrder, enabledPaymentRequest);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(collectPaymentMethodTypeEntity);

                // Return del model
                return collectPaymentMethodTypeEntity.PaymentTypeCode;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateCollectPaymentMethodType
        /// </summary>
        /// <param name="id"></param>
        /// <param name="methodType"></param>
        /// <param name="enabledTicket"></param>
        /// <param name="enabledBilling"></param>
        /// <param name="enabledPaymentOrder"></param>
        /// <param name="enabledPaymentRequest"></param>
        /// <returns></returns>
        public int UpdateCollectPaymentMethodType(int id, int methodType, int? enabledTicket,
                                               int? enabledBilling, int? enabledPaymentOrder, int? enabledPaymentRequest)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.PaymentType.CreatePrimaryKey(id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.PaymentType collectPaymentMethodTypeEntity = (ACCOUNTINGEN.PaymentType)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                collectPaymentMethodTypeEntity.EnabledTicket = enabledTicket;
                collectPaymentMethodTypeEntity.EnabledBilling = enabledBilling;
                collectPaymentMethodTypeEntity.EnabledPaymentOrder = enabledPaymentOrder;
                collectPaymentMethodTypeEntity.EnabledPaymentRequest = enabledPaymentRequest;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(collectPaymentMethodTypeEntity);

                // Return del model
                return collectPaymentMethodTypeEntity.PaymentTypeCode;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteCollectPaymentMethodType
        /// </summary>
        /// <param name="id"></param>
        public void DeleteCollectPaymentMethodType(int id)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.PaymentType.CreatePrimaryKey(id);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.PaymentType collectPaymentMethodTypeEntity = (ACCOUNTINGEN.PaymentType)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(collectPaymentMethodTypeEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}
