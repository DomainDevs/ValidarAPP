//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Models.AutomaticDebits;
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;

using System;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class BankNetworkStatusDAO
    {
        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        /// <summary>
        /// SaveBankNetworkStatus
        /// Guarda un nuevo rejistro en la tabla.
        /// </summary>
        /// <param name="bankNetworkStatus"></param>
        /// <returns>BankNetworkStatus</returns>
        public BankNetworkStatus SaveBankNetworkStatus(BankNetworkStatus bankNetworkStatus)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.BankNetworkStatus bankNetworkStatusEntity = EntityAssembler.CreateRejectionBank(bankNetworkStatus);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(bankNetworkStatusEntity);

                return ModelAssembler.CreateBankNetworkStatus(bankNetworkStatusEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateBankNetworkStatus
        /// Actualiza un rejistro de la tabla.
        /// </summary>
        /// <param name="bankNetworkStatus"></param>
        /// <returns>BankNetworkStatus</returns>
        public BankNetworkStatus UpdateBankNetworkStatus(BankNetworkStatus bankNetworkStatus)
        {
            try
            {
                PrimaryKey primaryKey = ACCOUNTINGEN.BankNetworkStatus.CreatePrimaryKey(bankNetworkStatus.BankNetwork.Id, bankNetworkStatus.AcceptedCouponStatus[0].Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.BankNetworkStatus bankNetworkStatusEntity = (ACCOUNTINGEN.BankNetworkStatus)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                bankNetworkStatusEntity.AppliedDefault = bankNetworkStatus.AcceptedCouponStatus[0].SmallDescription;
                bankNetworkStatusEntity.RejectionDefault = bankNetworkStatus.RejectedCouponStatus[0].SmallDescription;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(bankNetworkStatusEntity);

                return ModelAssembler.CreateBankNetworkStatus(bankNetworkStatusEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetBankNetworkStatus
        /// Obtiene un rejistro de la tabla usando su primary Key.
        /// </summary>
        /// <param name="bankNetworkStatus"></param>
        /// <returns>BankNetworkStatus</returns>
        public BankNetworkStatus GetBankNetworkStatus(BankNetworkStatus bankNetworkStatus)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.BankNetworkStatus.CreatePrimaryKey(bankNetworkStatus.BankNetwork.Id, bankNetworkStatus.AcceptedCouponStatus[0].Id);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.BankNetworkStatus bankNetworkStatusEntity = (ACCOUNTINGEN.BankNetworkStatus)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                return ModelAssembler.CreateBankNetworkStatus(bankNetworkStatusEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteBankNetworkStatus
        /// </summary>
        /// <param name="bankNetworkStatus"></param>
        /// <returns>bool</returns>
        public bool DeleteBankNetworkStatus(BankNetworkStatus bankNetworkStatus)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.BankNetworkStatus.CreatePrimaryKey(bankNetworkStatus.BankNetwork.Id, bankNetworkStatus.AcceptedCouponStatus[0].Id);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.BankNetworkStatus rejectionBankEntity = (ACCOUNTINGEN.BankNetworkStatus)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(rejectionBankEntity);

                return true;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

    }
}
