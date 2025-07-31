//Sistran Core
using Sistran.Core.Application.AutomaticDebitServices.EEProvider.Assemblers;
using Sistran.Core.Application.AutomaticDebitServices.Models;
using Sistran.Core.Application.TaxServices.Models;

//Sitran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Core.Application.AutomaticDebitServices.EEProvider.DAOs
{
    public class BankNetworkDAO
    {
        #region Instance Variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Public Methods

        #region Save

        /// <summary>
        /// SaveBankNetwork
        /// </summary>
        /// <param name="bankNetwork"></param>
        /// <returns></returns>
        public BankNetwork SaveBankNetwork(BankNetwork bankNetwork)
        {
            try
            {
                // Convertir de model a entity
                Entities.BankNetwork bankNetworkEntities = EntityAssembler.CreateNetwork(bankNetwork);

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

        #endregion

        #region Update

        /// <summary>
        /// UpdateBankNetwork
        /// </summary>
        /// <param name="bankNetwork"></param>
        /// <returns></returns>
        public BankNetwork UpdateBankNetwork(BankNetwork bankNetwork)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = Entities.BankNetwork.CreatePrimaryKey(bankNetwork.Id);

                // Encuentra el objeto en referencia a la llave primaria
                Entities.BankNetwork actionBankNetWorkEntity = (Entities.BankNetwork)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

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

        #endregion

        #region Delete

        /// <summary>
        /// DeleteBankNetwork
        /// </summary>
        /// <param name="bankNetwork"></param>
        /// <returns></returns>
        public bool DeleteBankNetwork(BankNetwork bankNetwork)
        {
            try
            {
                PrimaryKey primaryKey = Entities.BankNetwork.CreatePrimaryKey(bankNetwork.Id);
                Entities.BankNetwork actionNetWorkEntity = (Entities.BankNetwork)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(actionNetWorkEntity);

                return true;
            }
            catch (BusinessException exception)
            {
                if (exception.Message == "RELATED_OBJECT")
                {
                    return false;
                }
                throw new BusinessException(exception.Message);
            }
        }

        #endregion

        #region Get

        /// <summary>
        /// GetBankNetworks
        /// </summary>
        /// <returns></returns>
        public List<BankNetwork> GetBankNetworks()
        {
            List<BankNetwork> bankNetworks = new List<BankNetwork>();
            string description = "";
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.Property(Entities.BankNetwork.Properties.BankNetworkId);
            criteriaBuilder.IsNotNull();

            BusinessCollection networkCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(Entities.BankNetwork), criteriaBuilder.GetPredicate()));

            foreach (Entities.BankNetwork bankNetworkEntities in networkCollection.OfType<Entities.BankNetwork>())
            {
                if (bankNetworkEntities.TypePercentageCommission == 1)
                {
                    description = "Importe";
                }
                if (bankNetworkEntities.TypePercentageCommission == 100)
                {
                    description = "Porcentaje";
                }
                if (bankNetworkEntities.TypePercentageCommission == 1000)
                {
                    description = "Por Milaje";
                }

                BankNetwork bankNetwork = new BankNetwork();
                bankNetwork.Commission = new CommonService.Models.Amount() { Value = Convert.ToDecimal(bankNetworkEntities.CommissionAmount) };
                bankNetwork.Description = ReferenceEquals(bankNetworkEntities.Description, DBNull.Value) ? "" : Convert.ToString(bankNetworkEntities.Description);
                bankNetwork.HasTax = Convert.ToInt32(bankNetworkEntities.Tax) != 0;
                bankNetwork.Id = ReferenceEquals(bankNetworkEntities.BankNetworkId, DBNull.Value) ? 0 : Convert.ToInt32(bankNetworkEntities.BankNetworkId);
                bankNetwork.RequiresNotification = Convert.ToInt32(bankNetworkEntities.Prenotification) != 0;
                bankNetwork.RetriesNumber = ReferenceEquals(bankNetworkEntities.MaximumCoupon, DBNull.Value) ? 0 : Convert.ToInt32(bankNetworkEntities.MaximumCoupon);
                bankNetwork.TaxCategory = new TaxCategory()
                {
                    Description = description,
                    Id = ReferenceEquals(bankNetworkEntities.TypePercentageCommission, DBNull.Value) ? 0 : Convert.ToInt32(bankNetworkEntities.TypePercentageCommission),
                };
                bankNetworks.Add(bankNetwork);
            }

            return bankNetworks;
        }

        /// <summary>
        /// GetBankNetworkById
        /// Obtiene un rejistro de la tabla usando su primary Key.
        /// </summary>
        /// <param name="bankNetworkId"></param>
        /// <returns>BankNetwork</returns>
        public BankNetwork GetBankNetworkById(int bankNetworkId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = Entities.BankNetwork.CreatePrimaryKey(bankNetworkId);

                // Realizar las operaciones con los entities utilizando DAF
                Entities.BankNetwork actionBankNetWorkEntity = (Entities.BankNetwork)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                return ModelAssembler.CreateNetwork(actionBankNetWorkEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #endregion

    }
}
