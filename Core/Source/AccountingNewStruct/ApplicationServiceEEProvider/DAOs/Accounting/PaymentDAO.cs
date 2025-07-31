//System
using System;
using System.Collections.Generic;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using PaymentModels = Sistran.Core.Application.AccountingServices.EEProvider.Models.Payments;
using Sistran.Core.Application.Utilities.DataFacade;
using System.Data;
using Sistran.Core.Application.AccountingServices.Enums;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class PaymentDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        /// <summary>
        /// SavePayment
        /// Graba un nuevo payment directamente en el servicio
        /// </summary>
        /// <param name="payment"></param>
        /// <param name="collectId"></param>
        /// <returns>Payment</returns>
        public PaymentModels.Payment SavePayment(PaymentModels.Payment payment, int collectId)
        {
            try
            {
                // convertir de model a entity
                ACCOUNTINGEN.Payment paymentEntity = EntityAssembler.CreatePayment(payment, collectId);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(paymentEntity);

                // Return del model
                return ModelAssembler.CreatePayment(paymentEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdatePayment
        /// Actualiza un registro existente en la tabla PAYMENT
        /// </summary>
        /// <param name="payment"></param>
        /// <param name="collectCode"></param>
        /// <returns>Payment</returns>
        public PaymentModels.Payment UpdatePayment(PaymentModels.Payment payment, int collectCode)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.Payment.CreatePrimaryKey(payment.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.Payment paymentEntity = (ACCOUNTINGEN.Payment)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                paymentEntity.Amount = payment.LocalAmount.Value;
                paymentEntity.CollectCode = collectCode;
                paymentEntity.CurrencyCode = payment.Amount.Currency.Id;
                paymentEntity.ExchangeRate = payment.ExchangeRate.BuyAmount;
                paymentEntity.IncomeAmount = payment.Amount.Value;
                paymentEntity.PaymentMethodTypeCode = payment.PaymentMethod.Id;
                paymentEntity.Status = payment.Status;
                paymentEntity.DatePayment = DateTime.Now;
                paymentEntity.Taxes = payment.Tax;
                paymentEntity.Retentions = payment.Retention;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(paymentEntity);

                // Return del model
                return ModelAssembler.CreatePayment(paymentEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeletePayment
        /// Elimina un registro de la tabla PAYMENT
        /// </summary>
        /// <param name="payment"></param>
        public void DeletePayment(PaymentModels.Payment payment)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.Payment.CreatePrimaryKey(payment.Id);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.Payment paymentEntity = (ACCOUNTINGEN.Payment)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(paymentEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetPayment
        /// Obtiene un registro de la tabla PAYMENT
        /// </summary>
        /// <param name="payment"></param>
        /// <returns>Payment</returns>
        public PaymentModels.Payment GetPayment(PaymentModels.Payment payment)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.Payment.CreatePrimaryKey(payment.Id);

                //realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.Payment paymentEntity = (ACCOUNTINGEN.Payment)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreatePayment(paymentEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetPayments
        /// Obtiene todos los registros de la tabla PAYMENT
        /// </summary>
        /// <returns>List<Payment></returns>
        public List<PaymentModels.Payment> GetPayments()
        {
            try
            {
                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(
                    _dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.Payment)));

                // Return  como Lista
                return ModelAssembler.CreatePayments(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdatePaymentStatusById 
        /// Actualizad el estado de la tabla Payment basado en el Id del Pago
        /// metodo renombrado (anteriormente ChangeStatusById), y reubicado desde PaymentService3G Provider.
        /// </summary>
        /// <param name="paymentId"></param>
        /// <param name="statusId"></param>
        public void UpdatePaymentStatusById(int paymentId, int statusId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.Payment.CreatePrimaryKey(paymentId);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.Payment paymentEntity = (ACCOUNTINGEN.Payment)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                paymentEntity.Status = statusId;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(paymentEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        ///<summary>
        /// GetPaymentsByCollectId
        /// Obtiene los pagos a través del collectId 
        /// </summary>
        /// <param name="collectId"></param>
        /// <returns>List<Payment></returns>
        public List<PaymentModels.Payment> GetPaymentsByCollectId(int collectId)
        {
            try
            {
                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.CollectCode, collectId);

                BusinessCollection businessCollection = new BusinessCollection(
                    _dataFacadeManager.GetDataFacade().SelectObjects
                    (typeof(ACCOUNTINGEN.Payment), criteriaBuilder.GetPredicate()));

                // Retorna como Lista
                return ModelAssembler.CreatePayments(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        ///<summary>
        /// UpdateStatusPayment
        /// </summary>
        /// <param name="methodId"></param>
        /// <param name="status"></param>
        /// <param name="description"></param>
        public void UpdateStatusPayment(int methodId, int status, string description)
        {
            // Crea la Primary key con el código de la entidad
            PrimaryKey primaryKey = ACCOUNTINGEN.StatusPayment.CreatePrimaryKey(methodId, status);

            // Encuentra el objeto en referencia a la llave primaria
            ACCOUNTINGEN.StatusPayment statusPaymentEntity = (ACCOUNTINGEN.StatusPayment)
                (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

            statusPaymentEntity.Description = description;

            // Realizar las operaciones con los entities utilizando DAF
            _dataFacadeManager.GetDataFacade().UpdateObject(statusPaymentEntity);
        }

        ///<summary>
        /// DeleteStatusPayment
        /// </summary>
        /// <param name="methodId"></param>
        /// <param name="status"></param>
        public void DeleteStatusPayment(int methodId, int status)
        {
            // Crea la Primary key con el código de la entidad
            PrimaryKey primaryKey = ACCOUNTINGEN.StatusPayment.CreatePrimaryKey(methodId, status);

            // Encuentra el objeto en referencia a la llave primaria
            ACCOUNTINGEN.StatusPayment statusPaymentEntity = (ACCOUNTINGEN.StatusPayment)
                (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

            // Realizar las operaciones con los entities utilizando DAF
            _dataFacadeManager.GetDataFacade().DeleteObject(statusPaymentEntity);
        }

        /// <summary>
        /// SavePayment
        /// Graba una nueva retención directamente en el servicio
        /// </summary>
        /// <param name="payment"></param>
        /// <param name="collectId"></param>
        /// <returns>Payment</returns>
        public void SaveRetentionHistory(PaymentModels.Payment payment, int collectId)
        {
            try
            {
                PaymentModels.RetentionReceipt retentionReceipt = (PaymentModels.RetentionReceipt)payment;

                // convertir de model a entity
                ACCOUNTINGEN.RetentionHistory retentionHistoryEntity = EntityAssembler.CreateRetentionHistory(retentionReceipt, collectId);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(retentionHistoryEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public List<PaymentModels.Payment> GetCollectPaymentsByCollectId(int collectId)
        {

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.CollectCode, collectId);

            SelectQuery selectQuery = new SelectQuery();
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.Payment.Properties.Amount, "p"), "Amount"));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.Payment.Properties.CollectCode, "p"), "CollectCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.Payment.Properties.Commission, "p"), "Commission"));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.Payment.Properties.CurrencyCode, "p"), "CurrencyCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.Payment.Properties.DatePayment, "p"), "DatePayment"));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.Payment.Properties.DocumentNumber, "p"), "DocumentNumber"));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.Payment.Properties.ExchangeRate, "p"), "ExchangeRate"));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.Payment.Properties.Holder, "p"), "Holder"));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.Payment.Properties.IncomeAmount, "p"), "IncomeAmount"));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.Payment.Properties.PaymentCode, "p"), "PaymentCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.Payment.Properties.PaymentMethodTypeCode, "p"), "PaymentMethodTypeCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.Payment.Properties.Status, "p"), "Status"));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.Payment.Properties.Taxes, "p"), "Taxes"));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.Payment.Properties.ReceivingAccountNumber, "p"), "ReceivingAccountNumber"));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.Payment.Properties.ReceivingBankCode, "p"), "ReceivingBankCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.Payment.Properties.IssuingAccountNumber, "p"), "IssuingAccountNumber"));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.Payment.Properties.IssuingBankCode, "p"), "IssuingBankCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.Payment.Properties.Voucher, "p"), "Voucher"));

            selectQuery.Table = new ClassNameTable(typeof(ACCOUNTINGEN.Payment), "p");
            selectQuery.Where = criteriaBuilder.GetPredicate();

            List<PaymentModels.Payment> payments = new List<PaymentModels.Payment>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    PaymentModels.Payment payment = new PaymentModels.Payment()
                    {
                        Id = Convert.ToInt32(reader["PaymentCode"].ToString()),
                        Amount = new CommonService.Models.Amount
                        {
                            Currency = new CommonService.Models.Currency
                            {
                                Id = Convert.ToInt32(reader["CurrencyCode"].ToString())
                            },
                            Value = Convert.ToDecimal(reader["IncomeAmount"].ToString())
                        },
                        LocalAmount = new CommonService.Models.Amount
                        {
                            Value = Convert.ToDecimal(reader["Amount"].ToString())
                        },
                        CollectId = Convert.ToInt32(reader["CollectCode"].ToString()),
                        DatePayment = Convert.ToDateTime(reader["DatePayment"].ToString()),
                        ExchangeRate = new CommonService.Models.ExchangeRate
                        {
                            SellAmount = Convert.ToDecimal(reader["ExchangeRate"].ToString())
                        },
                        PaymentMethod = new PaymentModels.PaymentMethod
                        {
                            Id = Convert.ToInt32(reader["PaymentMethodTypeCode"].ToString())
                        },
                        Status = Convert.ToInt32(reader["Status"].ToString())
                    };

                    if (reader["DocumentNumber"] != null)
                        payment.DocumentNumber = Convert.ToString(reader["DocumentNumber"].ToString());
                    if (reader["Holder"] != null)
                        payment.Holder = Convert.ToString(reader["Holder"].ToString());
                    if (reader["Voucher"] != null)
                        payment.Voucher = Convert.ToString(reader["Voucher"].ToString());

                    if (payment.PaymentMethod.Id == Convert.ToInt32(PaymentMethods.DepositVoucher))
                    {
                        payment.IssuingAccountNumber = Convert.ToString(reader["ReceivingAccountNumber"].ToString());
                        payment.IssuingBankCode = Convert.ToInt32(reader["ReceivingBankCode"].ToString());
                    }
                    else if (payment.PaymentMethod.Id == Convert.ToInt32(PaymentMethods.ConsignmentByCheck))
                    {
                        payment.IssuingAccountNumber = Convert.ToString(reader["IssuingAccountNumber"].ToString());
                        payment.IssuingBankCode = Convert.ToInt32(reader["IssuingBankCode"].ToString());
                    }
                    payments.Add(payment);
                }
            }
            return payments;
        }
    }
}
