//Sistran Core
using Sistran.Core.Application.AccountingServices.DTOs.BankAccounts;
using Sistran.Core.Application.AccountingServices.DTOs.Payments;
using Sistran.Core.Application.AutomaticDebitServices.EEProvider.Assemblers;
using Sistran.Core.Application.AutomaticDebitServices.Models;
using CommonModels = Sistran.Core.Application.CommonService.Models;

//Sitran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Views;

using System;
using System.Collections.Generic;
using System.Data;
using Sistran.Core.Application.AccountingServices.DTOs;

namespace Sistran.Core.Application.AutomaticDebitServices.EEProvider.DAOs
{
    public class PaymentMethodBankNetworkDAO
    {
        #region Instance Variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Public Methods

        #region Save

        /// <summary>
        /// SavePaymentMethodBankNetwork
        /// </summary>
        /// <param name="paymentMethodBankNetwork"></param>
        /// <returns></returns>
        public PaymentMethodBankNetwork SavePaymentMethodBankNetwork(PaymentMethodBankNetwork paymentMethodBankNetwork)
        {
            try
            {
                // Convertir de model a entity
                Entities.PaymentMethodBankNetwork paymentMethodBankNetworkEntity = EntityAssembler.CreatePaymentMethodBankNetwork(paymentMethodBankNetwork);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(paymentMethodBankNetworkEntity);

                // Return del model
                return ModelAssembler.CreatePaymentMethodBankNetwork(paymentMethodBankNetworkEntity);
            }
            catch (DuplicatedObjectException)
            {
                // Se quiere grabar un registro duplicado de llave primaria
                paymentMethodBankNetwork.Id = -1;

                return paymentMethodBankNetwork;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// UpdatePaymentMethodBankNetwork
        /// </summary>
        /// <param name="paymentMethodBankNetwork"></param>
        /// <returns></returns>
        public PaymentMethodBankNetwork UpdatePaymentMethodBankNetwork(PaymentMethodBankNetwork paymentMethodBankNetwork)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = Entities.PaymentMethodBankNetwork.CreatePrimaryKey(paymentMethodBankNetwork.BankNetwork.Id, paymentMethodBankNetwork.PaymentMethod.Id, paymentMethodBankNetwork.BankAccountCompany.Id);

                // Encuentra el objeto en referencia a la llave primaria
                Entities.PaymentMethodBankNetwork paymentMethodBankNetworkEntity = (Entities.PaymentMethodBankNetwork)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                paymentMethodBankNetworkEntity.AccountBankCode = paymentMethodBankNetwork.BankAccountCompany.Id;
                paymentMethodBankNetworkEntity.Generate = paymentMethodBankNetwork.ToGenerate;
                paymentMethodBankNetworkEntity.Identifier = paymentMethodBankNetwork.BankNetwork.Description;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(paymentMethodBankNetworkEntity);

                // Return del model
                return ModelAssembler.CreatePaymentMethodBankNetwork(paymentMethodBankNetworkEntity);
            }
            catch (DuplicatedObjectException)
            {
                // Se quiere grabar un registro duplicado de llave primaria
                paymentMethodBankNetwork.Id = -1;

                return paymentMethodBankNetwork;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Delete

        /// <summary>
        /// DeletePaymentMethodBankNetwork
        /// </summary>
        /// <param name="paymentMethodBankNetwork"></param>
        /// <returns></returns>
        public bool DeletePaymentMethodBankNetwork(PaymentMethodBankNetwork paymentMethodBankNetwork)
        {
            try
            {
                PrimaryKey primaryKey = Entities.PaymentMethodBankNetwork.CreatePrimaryKey(paymentMethodBankNetwork.BankNetwork.Id, paymentMethodBankNetwork.PaymentMethod.Id, paymentMethodBankNetwork.BankAccountCompany.Id);

                Entities.PaymentMethodBankNetwork actionNetWorkEntity = (Entities.PaymentMethodBankNetwork)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

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
        /// GetPaymentMethodBankNetworks
        /// </summary>
        /// <param name="paymentMethodBankNetwork"></param>
        /// <returns></returns>
        public List<PaymentMethodBankNetwork> GetPaymentMethodBankNetworks(PaymentMethodBankNetwork paymentMethodBankNetwork)
        {
            int rows;
            List<PaymentMethodBankNetwork> paymentMethodBankNetworks = new List<PaymentMethodBankNetwork>();

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(Entities.PaymentMethodBankNetwork.Properties.BankNetworkCode, paymentMethodBankNetwork.BankNetwork.Id);

            UIView paymentMethods = _dataFacadeManager.GetDataFacade().GetView("BankNetworkRelationshipPaymentMethodView", criteriaBuilder.GetPredicate(), null, 0, -1, null, false, out rows);

            if (paymentMethods.Count > 0)
            {
                foreach (DataRow dataRow in paymentMethods.Rows)
                {
                    PaymentMethodBankNetwork newPaymentMethodBankNetwork = new PaymentMethodBankNetwork();
                    BankAccountCompanyDTO bankAccountCompany = new BankAccountCompanyDTO();
                    bankAccountCompany.Id = Convert.ToInt32(dataRow["AccountBankCode"]);
                    bankAccountCompany.BankAccountType = new BankAccountTypeDTO()
                    {
                        Id = Convert.ToInt32(dataRow["AccountTypeCode"]),
                        Description = dataRow["AccountTypeDescription"].ToString()
                    };
                    bankAccountCompany.Bank = new BankDTO()
                    {
                        Id = Convert.ToInt32(dataRow["BankCode"]),
                        Description = dataRow["BankDescription"].ToString()
                    };
                    bankAccountCompany.Number = dataRow["Number"].ToString();

                    newPaymentMethodBankNetwork.BankAccountCompany = bankAccountCompany;
                    newPaymentMethodBankNetwork.BankNetwork = new BankNetwork()
                    {
                        Id = Convert.ToInt32(dataRow["BankNetworkCode"]),
                        Description = dataRow["NetworkDescription"].ToString()
                    };
                    newPaymentMethodBankNetwork.Id = 0;
                    newPaymentMethodBankNetwork.PaymentMethod = new PaymentMethodDTO()
                    {
                        Id = Convert.ToInt32(dataRow["PaymentMethodCode"]),
                        Description = dataRow["PaymentMethodDescription"].ToString()
                    };
                    newPaymentMethodBankNetwork.ToGenerate = Convert.ToBoolean(dataRow["Generate"]);

                    paymentMethodBankNetworks.Add(newPaymentMethodBankNetwork);
                }
            }

            return paymentMethodBankNetworks;
        }

        #endregion

        #endregion

    }
}
