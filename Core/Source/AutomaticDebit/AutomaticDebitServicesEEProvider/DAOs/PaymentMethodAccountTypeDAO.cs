//Sistran Core
using Sistran.Core.Application.AccountingServices.DTOs.BankAccounts;
using Sistran.Core.Application.AccountingServices.DTOs.Payments;
using Sistran.Core.Application.AutomaticDebitServices.EEProvider.Assemblers;
using Sistran.Core.Application.AutomaticDebitServices.Models;

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

namespace Sistran.Core.Application.AutomaticDebitServices.EEProvider.DAOs
{
    public class PaymentMethodAccountTypeDAO
    {
        #region Instance Variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Public Methods

        #region Save

        /// <summary>
        /// SavePaymentMethodAccountType
        /// </summary>
        /// <param name="paymentMethodAccountType"></param>
        /// <returns></returns>
        public PaymentMethodAccountType SavePaymentMethodAccountType(PaymentMethodAccountType paymentMethodAccountType)
        {
            PaymentMethodAccountType newPaymentMethodAccountType = new PaymentMethodAccountType();
            try
            {
                ObjectCriteriaBuilder paymentMethodFilter = new ObjectCriteriaBuilder();
                paymentMethodFilter.PropertyEquals(Entities.PaymentMethodAccountType.Properties.PaymentMethodCode, paymentMethodAccountType.PaymentMethod.Id);
                paymentMethodFilter.And();
                paymentMethodFilter.PropertyEquals(Entities.PaymentMethodAccountType.Properties.AccountTypeCode, paymentMethodAccountType.BankAccountType.Id);

                BusinessCollection paymentMethodCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(Entities.PaymentMethodAccountType), paymentMethodFilter.GetPredicate()));

                if (paymentMethodCollection.Count == 0)
                {

                    //convertir de model a entity
                    Entities.PaymentMethodAccountType paymentMethodAccountTypeEntity = EntityAssembler.CreatePaymentMethodAccountType(paymentMethodAccountType);

                    //realizar las operaciones con los entities utilizando DAF
                    _dataFacadeManager.GetDataFacade().InsertObject(paymentMethodAccountTypeEntity);

                    //return del model
                    newPaymentMethodAccountType = ModelAssembler.CreatePaymentMethodAccountType(paymentMethodAccountTypeEntity);
                }
                else
                {
                    newPaymentMethodAccountType.Id = -1;
                    newPaymentMethodAccountType.BankAccountType = new BankAccountTypeDTO();
                    newPaymentMethodAccountType.BankAccountType.Description = "Error en Grabación";
                }

                return newPaymentMethodAccountType;
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// UpdatePaymentMethodAccountType
        /// </summary>
        /// <param name="paymentMethodAccountType"></param>
        /// <returns></returns>
        public PaymentMethodAccountType UpdatePaymentMethodAccountType(PaymentMethodAccountType paymentMethodAccountType)
        {
            try
            {
                //Crea la Primary key con el codigo de la entidad
                PrimaryKey primaryKey = Entities.PaymentMethodAccountType.CreatePrimaryKey(paymentMethodAccountType.PaymentMethod.Id, paymentMethodAccountType.BankAccountType.Id);

                // Encuentra el objeto en referencia a la llave primaria
                Entities.PaymentMethodAccountType paymentMethodAccountTypeEntity = (Entities.PaymentMethodAccountType)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

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

        #endregion

        #region Delete

        /// <summary>
        /// DeletePaymentMethodAccountType
        /// </summary>
        /// <param name="paymentMethodAccountType"></param>
        /// <returns></returns>
        public bool DeletePaymentMethodAccountType(PaymentMethodAccountType paymentMethodAccountType)
        {
            try
            {
                PrimaryKey primaryKey = Entities.PaymentMethodAccountType.CreatePrimaryKey(paymentMethodAccountType.PaymentMethod.Id, paymentMethodAccountType.BankAccountType.Id);

                // Encuentra el objeto en referencia a la llave primaria
                Entities.PaymentMethodAccountType paymentMethodAccountTypeEntity = (Entities.PaymentMethodAccountType)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                //realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(paymentMethodAccountTypeEntity);
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
        /// GetPaymentMethodAccountTypes
        /// </summary>
        /// <returns></returns>
        public List<PaymentMethodAccountType> GetPaymentMethodAccountTypes()
        {
            int rows;
            List<PaymentMethodAccountType> paymentMethodAccountTypes = new List<PaymentMethodAccountType>();

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.Property(Entities.PaymentMethodAccountType.Properties.PaymentMethodCode);
            criteriaBuilder.GreaterEqual();
            criteriaBuilder.Constant(0);

            UIView accountTypeRelationships = _dataFacadeManager.GetDataFacade().GetView("AccountTypeRelationshipPaymentMethodView", criteriaBuilder.GetPredicate(), null, 0, -1, null, false, out rows);

            foreach (DataRow dataRow in accountTypeRelationships)
            {
                PaymentMethodAccountType newPaymentMethodAccountType = new PaymentMethodAccountType();

                newPaymentMethodAccountType.BankAccountType = new BankAccountTypeDTO()
                {
                    Id = Convert.ToInt32(dataRow["AccountTypeCode"]),
                    Description = dataRow["AccountTypeDescription"].ToString()
                };
                newPaymentMethodAccountType.Id = 0;
                newPaymentMethodAccountType.PaymentMethod = new PaymentMethodDTO() { Id = Convert.ToInt32(dataRow["PaymentMethodCode"]), Description = dataRow["PaymentMethodDescription"].ToString() };
                newPaymentMethodAccountType.SmallDescriptionDebit = dataRow["DebitCode"].ToString();

                paymentMethodAccountTypes.Add(newPaymentMethodAccountType);
            }

            return paymentMethodAccountTypes;
        }

        #endregion

        #endregion

    }
}
