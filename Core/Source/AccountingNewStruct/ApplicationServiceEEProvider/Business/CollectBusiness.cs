using Sistran.Core.Application.AccountingServices.EEProvider.DAOs;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Collect;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.AccountingServices.Enums;
using Sistran.Core.Framework.Queries;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using Sistran.Core.Framework.DAF;
using System;
using System.Collections.Generic;
using System.Linq;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.BankAccounts;
using Sistran.Core.Application.AccountingServices.EEProvider.Models;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Business
{
    public class CollectBusiness
    {
        #region Daos
        readonly CollectDAO _collectDAO = new CollectDAO();
        readonly JournalEntryDAO _journalEntryDAO = new JournalEntryDAO();
        readonly CollectControlDAO _collectControlDAO = new CollectControlDAO();
        readonly AccountsPayableBusiness _accountsPayableBusiness = new AccountsPayableBusiness();
        #endregion

        public Collect GetCollect(Collect collect)
        {
            CollectControl collectControl = new CollectControl();
            JournalEntry journalEntry = new JournalEntry();
            collect = _collectDAO.GetCollectByCollectId(collect.Id);
            collect.Branch = new Branch();


            //si el recibo es generado desde un asiento diario
            if (collect.CollectType == CollectTypes.DailyAccount)
            {
                journalEntry.Id = collect.Transaction.TechnicalTransaction;
                journalEntry = _journalEntryDAO.GetJournalEntry(journalEntry);
                collect.Branch.Id = journalEntry.Branch.Id;
            }


            //si el recibo es generado desde ingreso de caja
            if (collect.CollectType == CollectTypes.Incoming)
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Collect.Properties.CollectId, collect.Id);


                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.Collect), criteriaBuilder.GetPredicate()));


                if (businessCollection.Count > 0)
                {
                    collectControl.Id = Convert.ToInt32(businessCollection.OfType<ACCOUNTINGEN.Collect>().First().CollectControlCode);
                    collectControl = _collectControlDAO.GetCollectControl(collectControl);
                }


                collect.Branch.Id = collectControl.Branch.Id;
                Person person = DelegateService.personService.GetPersonByIndividualId(collect.Payer.IndividualId);
                Company company = DelegateService.personService.GetCompanyByIndividualId(collect.Payer.IndividualId);
                if (person != null && person.IndividualId == collect.Payer.IndividualId)
                {
                    collect.Payer.Name = person.Name;
                    collect.Payer.IdentificationDocument.Number = person.IdentificationDocument.Number;
                }
                else if (company != null && company.IndividualId == collect.Payer.IndividualId)
                {
                    collect.Payer.Name = person.Name;
                    collect.Payer.IdentificationDocument.Number = person.IdentificationDocument.Number;
                }
                else if (company != null && company.IndividualId == collect.Payer.IndividualId)
                {
                    collect.Payer.Name = company.FullName;
                    collect.Payer.IdentificationDocument.Number = company.IdentificationDocument.Number;
                }
            }
            return collect;
        }

        public Collect GetCollectByTechnicalTransaction(int technicalTransaction)
        {
            CollectControl collectControl = new CollectControl();
            Collect collect = new Collect();
            collect = _collectDAO.GetCollectByTechnicalTransaction(technicalTransaction);
            if (collect != null)
            {
                if (collect.Id > 0)
                {
                    collect.Branch = new Branch();
                    collectControl.Id = collect.CollectControlId;
                    collectControl = _collectControlDAO.GetCollectControl(collectControl);

                    collect.Branch.Id = collectControl.Branch.Id;

                    Individual person = _accountsPayableBusiness.GetIndividualsByIndividualId(collect.Payer.IndividualId);
                    if (person != null)
                    {
                        collect.Payer.Name = person.FullName;
                        collect.Payer.IdentificationDocument.Number = person.IdentificationDocument.Number;
                    }
                }
            }
            return collect;
        }

        public Collect GetCollectByCollectId(int collectId)
        {
            CollectControl collectControl = new CollectControl();
            Collect collect = new Collect();
            collect = _collectDAO.GetCollectByCollectId(collectId);
            if (collect != null)
            {
                if (collect.Id > 0)
                {
                    collect.Branch = new Branch();
                    collectControl.Id = collect.CollectControlId;
                    collectControl = _collectControlDAO.GetCollectControl(collectControl);

                    collect.Branch.Id = collectControl.Branch.Id;

                    Individual person = _accountsPayableBusiness.GetIndividualsByIndividualId(collect.Payer.IndividualId);
                    if (person != null)
                    {
                        collect.Payer.Name = person.FullName;
                        collect.Payer.IdentificationDocument.Number = person.IdentificationDocument.Number;
                    }
                }
            }
            return collect;
        }

        public List<Currency> GetAvaibleCurrencies()
        {
            List<Currency> accountingCurrencies = new List<Currency>();
            List<Currency> currencies = DelegateService.commonService.GetCurrencies();
            BankAccountCompanyDAO bankAccountCompanyDAO = new BankAccountCompanyDAO();
            List<int> currencyCodes = bankAccountCompanyDAO.GetCurrencyCodes();

            currencies.ForEach(x =>
            {
                if (accountingCurrencies.Count(y => y.Id == x.Id) == 0 && currencyCodes.Contains(x.Id))
                    accountingCurrencies.Add(x);
            });

            return accountingCurrencies;
        }

        public List<Bank> GetAvaibleBanksByCurrencyId(int currencyId)
        {
            List<Bank> accountingBanks = new List<Bank>();
            List<Bank> banks = DelegateService.commonService.GetBanks();
            BankAccountCompanyDAO bankAccountCompanyDAO = new BankAccountCompanyDAO();
            List<int> bankCodes = bankAccountCompanyDAO.GetBankCodesByCurrencyId(currencyId);

            banks.ForEach(x =>
            {
                if (accountingBanks.Count(y => y.Id == x.Id) == 0 && bankCodes.Contains(x.Id))
                    accountingBanks.Add(x);
            });

            return accountingBanks;
        }

        public List<BankAccountCompany> GetAvaibleAccountsByCurrencyIdBankId(int currencyId, int bankId)
        {
            BankAccountCompanyDAO bankAccountCompanyDAO = new BankAccountCompanyDAO();
            return bankAccountCompanyDAO.GetBankAccountsByCurrencyIdBankId(currencyId, bankId);
        }

        public bool UpdateCollectStatus(int collectId, int status)
        {
            CollectDAO collectDAO = new CollectDAO();
            return collectDAO.UpdateCollectStatus(collectId, status);
        }

        public Message CanCancelCollect(int collectId)
        {
            return CanCancelCollect(GetCollectByCollectId(collectId));
        }

        public Message CanCancelCollect(Collect collect)
        {
            var message = new Message()
            {
                Success = false
            };

            if (collect == null || collect.Id <= 0)
            {
                message.Info = Resources.Resources.ErrorCollectNotFound;
                return message;
            }
            if (collect.Status == Convert.ToInt32(CollectStatus.Canceled))
            {
                message.Info = Resources.Resources.CollectAlreadyCanceled;
                return message;
            }
            else if (collect.Status == Convert.ToInt32(CollectStatus.Canceled))
            {
                message.Info = Resources.Resources.CollectAlreadyReversed;
                return message;
            }

            var collectControlBusiness = new CollectControlBusiness();
            var collectControl = collectControlBusiness.GetCollectControlById(collect.CollectControlId);
            if (collectControl.Status == Convert.ToInt32(CollectControlStatus.Close))
            {
                message.Info = Resources.Resources.CollectControlIsClosed;
                return message;
            }

            // Validación innecesaria, la validación de boletas internas es más fuerte
            if (true == false)
            {
                var collectPaymentBusiness = new CollectPaymentBusiness();
                var payments = collectPaymentBusiness.GetPaymentsByCollectId(collect.Id);

                if (payments.Any() && payments.Count != payments.Count(payment => payment.PaymentMethod.Id == Convert.ToInt32(PaymentMethods.Cash)))
                {
                    message.Info = Resources.Resources.CollectIsNotJustCash;
                    return message;
                }
            }

            var paymentTicketBusiness = new PaymentTicketBusiness();
            var tickets = paymentTicketBusiness.GetPaymentTicketItemsByCollectId(collect.Id);
            if (tickets.Any() && tickets.Count > 0)
            {
                string strTickets = "";
                tickets.ForEach(ticket => { strTickets = strTickets + ticket.Id + ", "; });
                if (strTickets != "")
                    strTickets = strTickets.Substring(0, strTickets.LastIndexOf(","));

                message.Info = String.Format(Resources.Resources.CollectHasBallotTicketAssociated, strTickets);
                return message;
            }
            message.Success = true;

            return message;
        }

        public bool DeleteTempApplicationByCollectID(int collectId, int status)
        {
            CollectDAO collectDAO = new CollectDAO();
            return collectDAO.UpdateCollectStatus(collectId, status);
        }
    }
}
