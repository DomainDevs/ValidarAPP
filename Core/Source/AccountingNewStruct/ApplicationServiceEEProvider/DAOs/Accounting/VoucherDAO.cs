using System;
using System.Collections.Generic;

//Sitran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Models.AccountsPayables;
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class VoucherDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion Instance variables

        #region Save

        /// <summary>
        /// SaveVoucher
        /// </summary>
        /// <param name="voucher"></param>
        /// <param name="paymentRequestId"></param>
        /// <returns></returns>
        public Voucher SaveVoucher(Voucher voucher, int paymentRequestId)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.Voucher voucherEntity = EntityAssembler.CreateVoucher(voucher, paymentRequestId);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(voucherEntity);

                // Return del model
                return ModelAssembler.CreateVoucher(voucherEntity);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion Save

        #region Update

        /// <summary>
        /// UpdatePaymentRequest
        /// </summary>
        /// <param name="voucher"></param>
        /// <param name="paymentRequestId"></param>
        /// <returns></returns>
        public Voucher UpdatePaymentRequest(Voucher voucher, int paymentRequestId)
        {
            try
            {
                // Crea la Primary key con el codigo de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.Voucher.CreatePrimaryKey(voucher.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.Voucher voucherEntity = (ACCOUNTINGEN.Voucher)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                DateTime? date;

                if (voucher.Date == Convert.ToDateTime("01/01/0001 0:00:00"))
                {
                    date = null;
                }
                else
                {
                    date = voucher.Date;
                }
                
                voucherEntity.PaymentRequestCode = paymentRequestId;
                voucherEntity.VoucherTypeCode = voucher.Type.Id;
                voucherEntity.Number = voucher.Number;
                voucherEntity.Date = date;
                voucherEntity.CurrencyCode = voucher.Currency.Id;
                voucherEntity.ExchangeRate = voucher.ExchangeRate;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(voucherEntity);

                // Return del model
                return ModelAssembler.CreateVoucher(voucherEntity);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion Update

        #region Delete

        /// <summary>
        /// DeletePaymentRequest
        /// </summary>
        /// <param name="voucher"></param>
        public void DeleteVoucher(Voucher voucher)
        {
            try
            {
                // Crea la Primary key con el codigo de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.Voucher.CreatePrimaryKey(voucher.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.Voucher voucherEntity = (ACCOUNTINGEN.Voucher)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(voucherEntity);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion Delete

        #region Get

        /// <summary>
        /// GetPaymentRequest
        /// </summary>
        /// <param name="voucher"></param>
        /// <returns></returns>
        public Voucher GetVoucher(Voucher voucher)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.Voucher.CreatePrimaryKey(voucher.Id);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.Voucher voucherEntity = (ACCOUNTINGEN.Voucher)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreateVoucher(voucherEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetVouchers
        /// </summary>
        /// <returns></returns>
        public List<Voucher> GetVouchers()
        {
            try
            {
                //Se asigna una BussinesCollection a una lista.
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.Voucher)));

                //Se retorna como una lista.
                return ModelAssembler.CreateVouchers(businessCollection);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion Get
    }
}
