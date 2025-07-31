//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Debit = Sistran.Core.Application.AccountingServices.EEProvider.Models.AutomaticDebits;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using System;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    class PaymentMethodAccountTypeDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        /// <summary>
        /// SavePaymentMethodAccountType
        /// </summary>
        /// <param name="paymentMethodAccountType"></param>
        /// <returns>PaymentMethodAccountType</returns>
        public Debit.PaymentMethodAccountType SavePaymentMethodAccountType(Debit.PaymentMethodAccountType paymentMethodAccountType)
        {
            try
            {
                //convertir de model a entity
                ACCOUNTINGEN.PaymentMethodAccountType paymentMethodAccountTypeEntity = EntityAssembler.CreatePaymentMethodAccountType(paymentMethodAccountType);

                //realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(paymentMethodAccountTypeEntity);

                //return del model
                return ModelAssembler.CreatePaymentMethodAccountType(paymentMethodAccountTypeEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }


        /// <summary>
        /// UpdatePaymentMethodAccountType
        /// </summary>
        /// <param name="paymentMethodAccountType"></param>
        /// <returns>PaymentMethodAccountType</returns>
        public Debit.PaymentMethodAccountType UpdatePaymentMethodAccountType(Debit.PaymentMethodAccountType paymentMethodAccountType)
        {
            try
            {
                //Crea la Primary key con el codigo de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.PaymentMethodAccountType.CreatePrimaryKey(paymentMethodAccountType.PaymentMethod.Id, paymentMethodAccountType.BankAccountType.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.PaymentMethodAccountType paymentMethodAccountTypeEntity = (ACCOUNTINGEN.PaymentMethodAccountType)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                paymentMethodAccountTypeEntity.DebitCode = paymentMethodAccountType.SmallDescriptionDebit;

                //realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(paymentMethodAccountTypeEntity);

                return ModelAssembler.CreatePaymentMethodAccountType(paymentMethodAccountTypeEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeletePaymentMethodAccountType
        /// </summary>
        /// <param name="paymentMethodAccountType"></param>
        public void DeletePaymentMethodAccountType(Debit.PaymentMethodAccountType paymentMethodAccountType)
        {
            try
            {
                PrimaryKey primaryKey = ACCOUNTINGEN.PaymentMethodAccountType.CreatePrimaryKey(paymentMethodAccountType.PaymentMethod.Id, paymentMethodAccountType.BankAccountType.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.PaymentMethodAccountType paymentMethodAccountTypeEntity = (ACCOUNTINGEN.PaymentMethodAccountType)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                //realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(paymentMethodAccountTypeEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}

