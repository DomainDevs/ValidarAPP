using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Payments;
using Sistran.Core.Application.AccountingServices.Enums;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using System.Linq;
using Sistran.Co.Application.Data;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{

    class PaymentTicketDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        /// <summary>
        /// SavePaymentTicket
        /// Graba un nuevo registro en la tabla ACC.PAYMENT_TICKET
        /// </summary>
        /// <param name="paymentTicket"></param>
        /// <param name="registerDate"></param>
        /// <param name="userId"></param>
        /// <returns>PaymentTicket</returns>
        public PaymentTicket SavePaymentTicket(PaymentTicket paymentTicket, DateTime registerDate, int userId)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.PaymentTicket paymentTicketEntity = EntityAssembler.CreatePaymentTicket(paymentTicket, registerDate, userId);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(paymentTicketEntity);

                // Return del model
                return ModelAssembler.CreatePaymentTicket(paymentTicketEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }


        /// <summary>
        /// UpdatePaymentTicket
        /// Actualiza un registro existente en la tabla ACC.PAYMENT_TICKET
        /// </summary>
        /// <param name="paymentTicket"></param>
        /// <param name="registerDate"></param>
        /// <param name="operationId"></param>
        /// <param name="userId"></param>
        /// <returns>PaymentTicket</returns>
        public PaymentTicket UpdatePaymentTicket(PaymentTicket paymentTicket, DateTime registerDate,
                                                        int userId, int operationId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.PaymentTicket.CreatePrimaryKey(paymentTicket.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.PaymentTicket entityPaymentTicket = (ACCOUNTINGEN.PaymentTicket)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                entityPaymentTicket.BranchCode = paymentTicket.Branch.Id;
                entityPaymentTicket.Amount = paymentTicket.Amount.Value;
                entityPaymentTicket.BankCode = paymentTicket.Bank.Id;
                if (paymentTicket.Commission.Value > 0)
                {
                    entityPaymentTicket.CommissionAmount = paymentTicket.Commission.Value;
                }
                if (operationId == 0)
                {
                    entityPaymentTicket.RegisterDate = registerDate;
                    entityPaymentTicket.UserId = userId;
                }
                entityPaymentTicket.Status = paymentTicket.Status;
                if (paymentTicket.CashAmount.Value > 0)
                {
                    entityPaymentTicket.CashAmount = paymentTicket.CashAmount.Value;
                }
                if (paymentTicket.DisabledDate != null)
                {
                    entityPaymentTicket.DisabledDate = paymentTicket.DisabledDate;
                }
                if (paymentTicket.DisabledUser != null)
                {
                    entityPaymentTicket.DisabledUserId = paymentTicket.DisabledUser;
                }

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(entityPaymentTicket);

                // Return del model
                return ModelAssembler.CreatePaymentTicket(entityPaymentTicket);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// Update Payment Ticket items
        /// Actualiza un registro existente en la tabla ACC.PAYMENT_TICKET
        /// </summary>
        /// <param name="paymentTicket"></param>
        /// <param name="userId"></param>
        /// <returns>PaymentTicket</returns>
        public PaymentTicket UpdatePaymentTicketItems(PaymentTicket paymentTicket, int userId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.PaymentTicket.CreatePrimaryKey(paymentTicket.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.PaymentTicket entityPaymentTicket = (ACCOUNTINGEN.PaymentTicket)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                entityPaymentTicket.BranchCode = paymentTicket.Branch.Id;
                entityPaymentTicket.BankCode = paymentTicket.Bank.Id;
                if (paymentTicket.Commission.Value > 0)
                {
                    entityPaymentTicket.CommissionAmount = paymentTicket.Commission.Value;
                }
                entityPaymentTicket.UserId = userId;
                if (paymentTicket.CashAmount.Value >= 0)
                {
                    entityPaymentTicket.CashAmount = paymentTicket.CashAmount.Value;
                }
                entityPaymentTicket.PaymentMethodTypeCode = paymentTicket.PaymentMethod;
                if (paymentTicket.Payments == null || paymentTicket.Payments.Count == 0)
                {
                    entityPaymentTicket.PaymentMethodTypeCode = Convert.ToInt32(PaymentMethods.Cash);
                }
                UpdatePayments(paymentTicket);

                entityPaymentTicket.Amount = GetPaymentsforPaymentTicketId(entityPaymentTicket.PaymentTicketCode).Sum(x => x.LocalAmount.Value);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(entityPaymentTicket);

                // Return del model
                return ModelAssembler.CreatePaymentTicket(entityPaymentTicket);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        private List<Payment> GetPaymentsforPaymentTicketId(int paymentTicketId)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentTicketItem.Properties.PaymentTicketCode, paymentTicketId);
            criteriaBuilder.And();
            criteriaBuilder.Property(ACCOUNTINGEN.Payment.Properties.PaymentMethodTypeCode);
            criteriaBuilder.In();
            criteriaBuilder.ListValue();
            criteriaBuilder.Constant(Convert.ToInt32(PaymentMethods.CurrentCheck));
            criteriaBuilder.Constant(Convert.ToInt32(PaymentMethods.ConsignmentByCheck));
            criteriaBuilder.EndList();

            SelectQuery selectQuery = new SelectQuery();
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.Payment.Properties.PaymentCode, "p"), "PaymentCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.Payment.Properties.IncomeAmount, "p"), "IncomeAmount"));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.Payment.Properties.PaymentMethodTypeCode, "p"), "PaymentMethodTypeCode"));

            Join join = new Join(new ClassNameTable(typeof(ACCOUNTINGEN.PaymentTicketItem), "pti"), new ClassNameTable(typeof(ACCOUNTINGEN.Payment), "p"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ACCOUNTINGEN.PaymentTicketItem.Properties.PaymentCode, "pti")
                .Equal()
                .Property(ACCOUNTINGEN.Payment.Properties.PaymentCode, "p")
                .GetPredicate());

            selectQuery.Table = join;
            selectQuery.Where = criteriaBuilder.GetPredicate();

            List<Payment> payments = new List<Payment>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    payments.Add(new Payment()
                    {
                        Id = Convert.ToInt32(reader["PaymentCode"].ToString()),
                        LocalAmount = new CommonService.Models.Amount() {
                            Value = Convert.ToDecimal(reader["IncomeAmount"].ToString())
                        }
                    });
                }
            }
            return payments;
        }

        private void UpdatePayments(PaymentTicket paymentTicket)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentTicketItem.Properties.PaymentTicketCode, paymentTicket.Id);
            criteriaBuilder.And();
            criteriaBuilder.Property(ACCOUNTINGEN.Payment.Properties.PaymentMethodTypeCode);
            criteriaBuilder.In();
            criteriaBuilder.ListValue();
            criteriaBuilder.Constant(Convert.ToInt32(PaymentMethods.CurrentCheck));
            criteriaBuilder.Constant(Convert.ToInt32(PaymentMethods.ConsignmentByCheck));
            criteriaBuilder.EndList();
        
            SelectQuery selectQuery = new SelectQuery();
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.Payment.Properties.PaymentCode, "p"), "PaymentCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.PaymentTicketItem.Properties.PaymentTicketItemCode, "pti"), "PaymentTicketItemCode"));

            Join join = new Join(new ClassNameTable(typeof(ACCOUNTINGEN.PaymentTicketItem), "pti"), new ClassNameTable(typeof(ACCOUNTINGEN.Payment), "p"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ACCOUNTINGEN.PaymentTicketItem.Properties.PaymentCode, "pti")
                .Equal()
                .Property(ACCOUNTINGEN.Payment.Properties.PaymentCode, "p")
                .GetPredicate());

            selectQuery.Table = join;
            selectQuery.Where = criteriaBuilder.GetPredicate();

            List<PaymentTicketItem> currentPayments = new List<PaymentTicketItem>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    currentPayments.Add(new PaymentTicketItem()
                    {
                        PaymentId = Convert.ToInt32(reader["PaymentCode"].ToString()),
                        PaymentTicketItemId = Convert.ToInt32(reader["PaymentTicketItemCode"].ToString())
                    });
                }
            }

            PaymentTicketItemDAO paymentTicketItemDAO = new PaymentTicketItemDAO();
            PaymentDAO paymentDAO = new PaymentDAO();
            int newPaymentStatus = Convert.ToInt32(PaymentStatus.Active);
            currentPayments.ForEach(x =>
            {
                if (paymentTicket.Payments.Where(y => y.Id == x.PaymentId).ToList().Count == 0)
                {
                    paymentDAO.UpdatePaymentStatusById(x.PaymentId, newPaymentStatus);
                    paymentTicketItemDAO.DeletePaymentTicketItem(x.PaymentTicketItemId);
                }

            });

            if (paymentTicket.Payments != null && paymentTicket.Payments.Count > 0)
            {
                int releatedPaymentStatus = Convert.ToInt32(PaymentStatus.InternalBallot);
                paymentTicket.Payments.ForEach(x =>
                {
                    if (currentPayments.Where(y => y.PaymentId == x.Id).ToList().Count == 0)
                    {
                        paymentTicketItemDAO.SavePaymentTicketItem(0, x.Id, paymentTicket.Id);
                        paymentDAO.UpdatePaymentStatusById(x.Id, releatedPaymentStatus);
                    }   
                });
            }
        }
               
        /// <summary>
        /// GetPaymentTicket
        /// Obtiene un registro de la tabla ACC.PAYMENT_TICKET
        /// </summary>
        /// <param name="paymentTicket"></param>
        /// <returns>PaymentTicket</returns>
        public PaymentTicket GetPaymentTicket(PaymentTicket paymentTicket)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.PaymentTicket.CreatePrimaryKey(paymentTicket.Id);

                // Realizar las operaciones con los entities utilizando DAF
                ACCOUNTINGEN.PaymentTicket paymentTicketEntity = (ACCOUNTINGEN.PaymentTicket)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Return del model
                return ModelAssembler.CreatePaymentTicket(paymentTicketEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public int GetTechnicalTransactionForPaymentBallotByPaymentCode(int paymentCode)
        {
            int technicalTransaction = 0;
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.Property(ACCOUNTINGEN.PaymentTicketItem.Properties.PaymentCode);
            criteriaBuilder.Equal();
            criteriaBuilder.Constant(paymentCode);

            SelectQuery selectQuery = new SelectQuery();
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.PaymentTicketItem.Properties.PaymentCode, "pti"), "PaymentCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.PaymentBallot.Properties.TechnicalTransaction, "pb"), "TechnicalTransaction"));

            Join join = new Join(new ClassNameTable(typeof(ACCOUNTINGEN.PaymentTicketItem), "pti"), new ClassNameTable(typeof(ACCOUNTINGEN.PaymentTicket), "pt"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ACCOUNTINGEN.PaymentTicketItem.Properties.PaymentTicketCode, "pti")
                .Equal()
                .Property(ACCOUNTINGEN.PaymentTicket.Properties.PaymentTicketCode, "pt")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(ACCOUNTINGEN.PaymentTicketBallot), "ptb"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ACCOUNTINGEN.PaymentTicketBallot.Properties.PaymentTicketCode, "ptb")
                .Equal()
                .Property(ACCOUNTINGEN.PaymentTicket.Properties.PaymentTicketCode, "pt")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(ACCOUNTINGEN.PaymentBallot), "pb"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ACCOUNTINGEN.PaymentBallot.Properties.PaymentBallotCode, "pb")
                .Equal()
                .Property(ACCOUNTINGEN.PaymentTicketBallot.Properties.PaymentBallotCode, "ptb")
                .GetPredicate());
            selectQuery.Table = join;
            selectQuery.Where = criteriaBuilder.GetPredicate();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    technicalTransaction = Convert.ToInt32(reader["TechnicalTransaction"].ToString());
                }
            }
            return technicalTransaction;
        }

        /// <summary>
        /// GetPaymentTickets
        /// Obtiene todos los registros de la tabla PAYMENT_TICKET
        /// </summary>
        /// <returns>List<Models.PaymentTicket/></returns>
        public List<PaymentTicket> GetPaymentTickets()
        {
            try
            {
                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.PaymentTicket)));

                // Return como lista
                return ModelAssembler.CreatePaymentTicket(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public List<PaymentTicket> GetPaymentTicketsByCollectId(int collectId)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.Property(ACCOUNTINGEN.PaymentTicket.Properties.Status, "pt");
            criteriaBuilder.Distinct();
            criteriaBuilder.Constant(Convert.ToInt32(PaymentTicketStatus.Canceled));
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Payment.Properties.CollectCode, collectId);

            SelectQuery selectQuery = new SelectQuery();
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.PaymentTicket.Properties.PaymentTicketCode, "pt"), "PaymentTicketCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.PaymentTicket.Properties.AccountNumber, "pt"), "AccountNumber"));


            Join join = new Join(new ClassNameTable(typeof(ACCOUNTINGEN.PaymentTicket), "pt"), new ClassNameTable(typeof(ACCOUNTINGEN.PaymentTicketItem), "pti"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ACCOUNTINGEN.PaymentTicket.Properties.PaymentTicketCode, "pt")
                .Equal()
                .Property(ACCOUNTINGEN.PaymentTicketItem.Properties.PaymentTicketCode, "pti")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(ACCOUNTINGEN.Payment), "p"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ACCOUNTINGEN.PaymentTicketItem.Properties.PaymentCode, "pti")
                .Equal()
                .Property(ACCOUNTINGEN.Payment.Properties.PaymentCode, "p")
                .GetPredicate());

            selectQuery.Table = join;
            selectQuery.Where = criteriaBuilder.GetPredicate();

            List<PaymentTicket> paymentTickets = new List<PaymentTicket>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    paymentTickets.Add(new PaymentTicket()
                    {
                        Id = Convert.ToInt32(reader["PaymentTicketCode"].ToString()),
                        AccountNumber = Convert.ToString(reader["AccountNumber"]),
                    });
                }
            }
            return paymentTickets;
        }
        
        public List<PaymentTicket> GetPaymentsByPaymentBallotId(int paymentBallotId)
        {

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentTicketBallot.Properties.PaymentBallotCode, paymentBallotId);

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
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.PaymentTicket.Properties.BankCode, "pt"), "BankCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.PaymentTicket.Properties.BranchCode, "pt"), "BranchCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.PaymentTicket.Properties.Status, "pt"), "PaymentTicketStatus"));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.PaymentTicket.Properties.Amount, "pt"), "PaymentTicketAmount"));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.PaymentTicket.Properties.CashAmount, "pt"), "PaymentTicketCashAmount"));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.PaymentTicket.Properties.CurrencyCode, "pt"), "PaymentTicketCurrencyCode"));
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.PaymentTicket.Properties.PaymentTicketCode, "pt"), "PaymentTicketCode"));

            Join join = new Join(new ClassNameTable(typeof(ACCOUNTINGEN.PaymentTicketBallot), "ptb"), new ClassNameTable(typeof(ACCOUNTINGEN.PaymentTicket), "pt"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ACCOUNTINGEN.PaymentTicketBallot.Properties.PaymentTicketCode, "ptb")
                .Equal()
                .Property(ACCOUNTINGEN.PaymentTicket.Properties.PaymentTicketCode, "pt")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(ACCOUNTINGEN.PaymentTicketItem), "pti"), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ACCOUNTINGEN.PaymentTicket.Properties.PaymentTicketCode, "pt")
                .Equal()
                .Property(ACCOUNTINGEN.PaymentTicketItem.Properties.PaymentTicketCode, "pti")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(ACCOUNTINGEN.Payment), "p"), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ACCOUNTINGEN.PaymentTicketItem.Properties.PaymentCode, "pti")
                .Equal()
                .Property(ACCOUNTINGEN.Payment.Properties.PaymentCode, "p")
                .GetPredicate());

            selectQuery.Table = join;
            selectQuery.Where = criteriaBuilder.GetPredicate();

            List<PaymentTicket> paymentTickets = new List<PaymentTicket>();
            int paymentTicketId;
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    paymentTicketId = Convert.ToInt32(reader["PaymentTicketCode"].ToString());
                    if (paymentTickets.Count(x => x.Id == paymentTicketId) == 0)
                    {
                        paymentTickets.Add(new PaymentTicket()
                        {
                            Id = Convert.ToInt32(reader["PaymentTicketCode"].ToString()),
                            Bank = new CommonService.Models.Bank()
                            {
                                Id = Convert.ToInt32(reader["BankCode"].ToString())
                            },
                            Status = Convert.ToInt32(reader["PaymentTicketStatus"].ToString()),
                            Currency = new CommonService.Models.Currency()
                            {
                                Id = Convert.ToInt32(reader["PaymentTicketCurrencyCode"].ToString())
                            },
                            Amount = new CommonService.Models.Amount
                            {
                                Currency = new CommonService.Models.Currency
                                {
                                    Id = Convert.ToInt32(reader["PaymentTicketCurrencyCode"].ToString())
                                },
                                Value = Convert.ToDecimal(reader["PaymentTicketAmount"].ToString())
                            },
                            CashAmount = new CommonService.Models.Amount
                            {
                                Value = Convert.ToDecimal(reader["PaymentTicketCashAmount"].ToString())
                            },
                            Payments = new List<Payment>()
                        });
                    }

                    var paymentTicket = paymentTickets.Where(x => x.Id == paymentTicketId).FirstOrDefault();
                    if (paymentTicket != null && paymentTicket.Id > 0)
                    {
                        if (reader["BranchCode"] != null)
                        {
                            paymentTicket.Branch = new CommonService.Models.Branch()
                            {
                                Id = Convert.ToInt32(reader["BranchCode"].ToString())
                            };
                        }
                        if (reader["PaymentCode"] != null)
                        {
                            Payment payment = new Payment()
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
                                DocumentNumber = Convert.ToString(reader["DocumentNumber"].ToString()),
                                ExchangeRate = new CommonService.Models.ExchangeRate
                                {
                                    SellAmount = Convert.ToDecimal(reader["ExchangeRate"].ToString())
                                },
                                Holder = Convert.ToString(reader["Holder"].ToString()),
                                PaymentMethod = new PaymentMethod
                                {
                                    Id = Convert.ToInt32(reader["PaymentMethodTypeCode"].ToString())
                                },
                                Status = Convert.ToInt32(reader["Status"].ToString())
                            };

                            if (payment.PaymentMethod.Id == Convert.ToInt32(PaymentMethods.DepositVoucher))
                            {
                                payment.IssuingAccountNumber = Convert.ToString(reader["ReceivingAccountNumber"].ToString());
                                payment.IssuingBankCode = Convert.ToInt32(reader["ReceivingBankCode"].ToString());
                            }
                            else if (payment.PaymentMethod.Id == Convert.ToInt32(PaymentMethods.ConsignmentByCheck))
                            {
                                payment.IssuingAccountNumber = Convert.ToString(reader["IssuingBankCode"].ToString());
                                payment.IssuingBankCode = Convert.ToInt32(reader["IssuingAccountNumber"].ToString());
                            }
                            paymentTicket.Payments.Add(payment);
                        }
                    }
                }
            }
            return paymentTickets;
        }
        
        public int GetPaymentTicketSequence()
        {
            DataTable result = null;
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("ACC.GET_PAYMENT_TICKET_CD");
            }

            if (result != null  && result.Rows.Count > 0)
            {
                return Convert.ToInt32(result.Rows[0].ItemArray[0]);
            }
            return -1;
        }
    }
}
