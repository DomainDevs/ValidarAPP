//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using COMMEN = Sistran.Core.Application.Common.Entities;
using System;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    internal class TempVoucherDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion Instance variables

        #region Public Methods

        /// <summary>
        /// DeleteTempVoucher
        /// </summary>
        /// <param name="voucherId"></param>
        public void DeleteTempVoucher(int voucherId)
        {
            try
            {
                // Se crea el Primary key con el código de la entidad
                PrimaryKey primaryKey = COMMEN.TempVoucher.CreatePrimaryKey(voucherId);

                // Realizar las operaciones con los entities utilizando DAF
                COMMEN.TempVoucher tempVoucherEntity = (COMMEN.TempVoucher)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(tempVoucherEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetTempVoucher
        /// </summary>
        /// <param name="voucher"></param>
        /// <returns>Voucher</returns>
        public Models.AccountsPayables.Voucher GetTempVoucher(Models.AccountsPayables.Voucher voucher)
        {
            try
            {
                PrimaryKey primaryKey = COMMEN.TempVoucher.CreatePrimaryKey(voucher.Id);
                COMMEN.TempVoucher tempVoucherEntity = (COMMEN.TempVoucher)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                return ModelAssembler.CreateTempVoucher(tempVoucherEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Public Methods
    }
}
