using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class PaymentRequestNumberDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion Instance variables

        #region Save

        /// <summary>
        /// SavePaymentRequestNumber
        /// </summary>
        /// <param name="paymentRequestNumber"></param>
        /// <returns></returns>
        public Models.AccountsPayables.PaymentRequestNumber SavePaymentRequestNumber(Models.AccountsPayables.PaymentRequestNumber paymentRequestNumber)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.PaymentRequestNumber paymentRequestNumberEntity = EntityAssembler.CreatePaymentRequestNumber(paymentRequestNumber);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(paymentRequestNumberEntity);

                // Return del model
                return ModelAssembler.CreatePaymentRequestNumber(paymentRequestNumberEntity);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion Save

        #region Update

        /// <summary>
        /// UpdatePaymentRequestNumber
        /// </summary>
        /// <param name="paymentRequestNumber"></param>
        /// <returns></returns>
        public Models.AccountsPayables.PaymentRequestNumber UpdatePaymentRequestNumber(Models.AccountsPayables.PaymentRequestNumber paymentRequestNumber)
        {
            try
            {
                // Crea la Primary key con el codigo de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.PaymentRequestNumber.CreatePrimaryKey(paymentRequestNumber.Branch.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.PaymentRequestNumber paymentRequestNumberEntity = (ACCOUNTINGEN.PaymentRequestNumber)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                paymentRequestNumberEntity.Number = paymentRequestNumber.Number;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(paymentRequestNumberEntity);

                // Return del model
                return ModelAssembler.CreatePaymentRequestNumber(paymentRequestNumberEntity);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion Update

        #region Get

        /// <summary>
        /// GetPaymentRequestNumber
        /// </summary>
        /// <param name="paymentRequestNumber"></param>
        /// <returns></returns>
        public Models.AccountsPayables.PaymentRequestNumber GetPaymentRequestNumber(Models.AccountsPayables.PaymentRequestNumber paymentRequestNumber)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.PaymentRequestNumber.CreatePrimaryKey(paymentRequestNumber.Branch.Id);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.PaymentRequestNumber paymentRequestNumberEntity = (ACCOUNTINGEN.PaymentRequestNumber)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                if (paymentRequestNumberEntity != null)
                {
                    // Return del model
                    return ModelAssembler.CreatePaymentRequestNumber(paymentRequestNumberEntity);
                }
                else
                {
                    return null;
                }


            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetPaymentRequestNumbers
        /// </summary>
        /// <returns></returns>
        public List<Models.AccountsPayables.PaymentRequestNumber> GetPaymentRequestNumbers()
        {
            try
            {
                //Se asigna una BussinesCollection a una lista.
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.PaymentRequestNumber)));

                //Se retorna como una lista.
                return ModelAssembler.CreatePaymentRequestNumbers(businessCollection);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion Get
    }
}
