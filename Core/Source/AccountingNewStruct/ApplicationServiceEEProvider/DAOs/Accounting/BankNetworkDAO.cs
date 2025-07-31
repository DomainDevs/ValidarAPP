//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Debit = Sistran.Core.Application.AccountingServices.EEProvider.Models.AutomaticDebits;

//Sistran FKW
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;

using System;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class BankNetworkDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion Instance variables

        #region Public Methods

        /// <summary>
        /// SaveBankNetwork
        /// Graba los parametros de redes
        /// </summary>
        /// <param name="bankNetwork"></param>
        /// <returns>BankNetwork</returns>
        public Debit.BankNetwork SaveBankNetwork(Debit.BankNetwork bankNetwork)
        {
            try
            { 
                // Convertir de model a entity
                ACCOUNTINGEN.BankNetwork bankNetworkEntities = EntityAssembler.CreateNetwork(bankNetwork);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(bankNetworkEntities);

                // Return del model
                return ModelAssembler.CreateNetwork(bankNetworkEntities);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateBankNetwork
        /// </summary>
        /// <param name="bankNetwork"></param>
        /// <returns>BankNetwork</returns>
        public Debit.BankNetwork UpdateBankNetwork(Debit.BankNetwork bankNetwork)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.BankNetwork.CreatePrimaryKey(bankNetwork.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.BankNetwork actionBankNetWorkEntity = (ACCOUNTINGEN.BankNetwork)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                actionBankNetWorkEntity.Description = bankNetwork.Description;
                actionBankNetWorkEntity.Commission = bankNetwork.Commission.Value > 0;
                actionBankNetWorkEntity.Tax = bankNetwork.HasTax;
                actionBankNetWorkEntity.MaximumCoupon = bankNetwork.RetriesNumber;
                actionBankNetWorkEntity.TypePercentageCommission = bankNetwork.TaxCategory.Id;
                actionBankNetWorkEntity.CommissionRate = 0;
                actionBankNetWorkEntity.CommissionAmount = bankNetwork.Commission.Value;
                actionBankNetWorkEntity.Prenotification = bankNetwork.RequiresNotification;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(actionBankNetWorkEntity);

                // Return del model
                return ModelAssembler.CreateNetwork(actionBankNetWorkEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteBankNetWork
        /// </summary>
        /// <param name="bankNetwork"></param>
        public void DeleteBankNetWork(Debit.BankNetwork bankNetwork)
        {
            try
            {
                PrimaryKey primaryKey = ACCOUNTINGEN.BankNetwork.CreatePrimaryKey(bankNetwork.Id);
                ACCOUNTINGEN.BankNetwork actionNetWorkEntity = (ACCOUNTINGEN.BankNetwork)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(actionNetWorkEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetBankNetworkById
        /// Obtiene un rejistro de la tabla usando su primary Key.
        /// </summary>
        /// <param name="bankNetworkId"></param>
        /// <returns>BankNetwork</returns>
        public Debit.BankNetwork GetBankNetworkById(int bankNetworkId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.BankNetwork.CreatePrimaryKey(bankNetworkId);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.BankNetwork actionBankNetWorkEntity = (ACCOUNTINGEN.BankNetwork)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                return ModelAssembler.CreateNetwork(actionBankNetWorkEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Public Methods
    }
}
