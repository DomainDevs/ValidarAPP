using Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Core.Application.UniquePersonService.Assemblers;
using Sistran.Core.Application.UniquePersonService.Enums;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using modelsCommon = Sistran.Core.Application.CommonService.Models;

namespace Sistran.Core.Application.UniquePersonService.DAOs
{
    /// <summary>
    /// Cuentas bancarias
    /// </summary>
    public class PaymentMethodAccountDAO
    {
        /// <summary>
        /// Buscar la informacio de madios de pagos 
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public List<Models.PaymentMethodAccount> GetPaymentMethodAccountByIndividualId(int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PaymentMethodAccount.Properties.IndividualId, typeof(PaymentMethodAccount).Name);
            filter.Equal();
            filter.Constant(individualId);
            BusinessCollection businessCollection = new BusinessCollection();
            businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(PaymentMethodAccount), filter.GetPredicate()));

            List<Models.PaymentMethodAccount> paymentMethodAccounts = ModelAssembler.CreatePaymentMethodAccounts(businessCollection);
            businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(IndividualPaymentMethod), filter.GetPredicate()));

            List<IndividualPaymentMethod> IndividualPaymentMethod = businessCollection.Cast<IndividualPaymentMethod>().ToList();
            if (IndividualPaymentMethod != null && IndividualPaymentMethod.Count > 0)
            {
                paymentMethodAccounts.Add(new Models.PaymentMethodAccount { Id = IndividualPaymentMethod.Where(x => x.PaymentMethodCode == (int)PaymentMethodType.Cash).FirstOrDefault().PaymentId, PaymentMethod = new modelsCommon.PaymentMethod { Id = IndividualPaymentMethod.Where(x => x.PaymentMethodCode == (int)PaymentMethodType.Cash).FirstOrDefault().PaymentMethodCode } });
            }
            businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(PaymentAccountType)));
            List<Models.PaymentAccountType> paymentAccountTypes = ModelAssembler.CreateAccountTypes(businessCollection);

            List<modelsCommon.PaymentMethod> paymentMethods = DelegateService.commonServiceCore.GetPaymentMethods();

            foreach (Models.PaymentMethodAccount item in paymentMethodAccounts)
            {
                if (IndividualPaymentMethod.Count > 0)
                {
                    item.PaymentMethod.Id = IndividualPaymentMethod.Where(x => x.PaymentId == item.Id).FirstOrDefault().PaymentMethodCode;
                }
                if (item.Bank != null)
                {
                    item.Bank.Description = DelegateService.commonServiceCore.GetBanksByBankId(item.Bank.Id).Description;
                }
                if (item.AccountType != null)
                {
                    item.AccountType.Description = paymentAccountTypes.First(x => x.Id == item.AccountType.Id).Description;
                }
                item.PaymentMethod.Description = paymentMethods.First(x => x.Id == item.PaymentMethod.Id).Description;
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetPaymentMethodAccountByIndividualId");
            return paymentMethodAccounts;
        }

        /// <summary>
        /// Gets the payment method account by individual identifier by payment metod by payment account.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public PaymentMethodAccount GetPaymentMethodAccountByIndividualIdByPaymentMetodByPaymentAccount(ObjectCriteriaBuilder filter)
        {
            PaymentMethodAccount paymentMethodAccount = null;
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                paymentMethodAccount = (PaymentMethodAccount)daf.List(typeof(PaymentMethodAccount), filter.GetPredicate()).FirstOrDefault();
            }
            return paymentMethodAccount;
        }

        /// <summary>
        /// Creates the payment method account.
        /// </summary>
        /// <param name="paymentMethodAccount">The payment method account.</param>
        /// <param name="IndividualId">The individual identifier.</param>
        /// <returns></returns>
        public Models.PaymentMethodAccount CreatePaymentMethodAccount(Models.PaymentMethodAccount paymentMethodAccount, int IndividualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PaymentMethodAccount.Properties.IndividualId, typeof(PaymentMethodAccount).Name);
            filter.Equal();
            filter.Constant(IndividualId);
            filter.And();
            filter.Property(PaymentMethodAccount.Properties.PaymentId, typeof(PaymentMethodAccount).Name);
            filter.Equal();
            filter.Constant(paymentMethodAccount.Id);
            filter.And();
            filter.Property(PaymentMethodAccount.Properties.PaymentAccountTypeCode, typeof(PaymentMethodAccount).Name);
            filter.Equal();
            filter.Constant(paymentMethodAccount.AccountType.Id);
            filter.And();
            filter.Property(PaymentMethodAccount.Properties.BankCode, typeof(PaymentMethodAccount).Name);
            filter.Equal();
            filter.Constant(paymentMethodAccount.Bank.Id);
            PaymentMethodAccount payment = GetPaymentMethodAccountByIndividualIdByPaymentMetodByPaymentAccount(filter);
            if (payment == null)
            {
                PaymentMethodAccount paymentMethodAccountEntity = EntityAssembler.CreatePaymentMethodAccount(paymentMethodAccount, IndividualId);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(paymentMethodAccountEntity);
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.CreatePaymentMethodAccount");
                return ModelAssembler.CreatePaymentMethodAccount(paymentMethodAccountEntity);
            }
            else
            {
                payment.AccountNumber = paymentMethodAccount.AccountNumber;
                payment.BankCode = paymentMethodAccount.Bank.Id;
                payment.PaymentAccountTypeCode = paymentMethodAccount.AccountType.Id;
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(payment);
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.CreatePaymentMethodAccount");
                return ModelAssembler.CreatePaymentMethodAccount(payment);
            }
        }

        /// <summary>
        /// UpdatePaymentMethodAccount
        /// </summary>
        /// <param name="paymentMethodAccount"></param>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public Models.PaymentMethodAccount UpdatePaymentMethodAccount(Models.PaymentMethodAccount paymentMethodAccount, int individualId)
        {
            //validar que no exista
            ObjectCriteriaBuilder filterDocument = new ObjectCriteriaBuilder();
            filterDocument.Property(PaymentMethodAccount.Properties.IndividualId, typeof(PaymentAccountType).Name);
            filterDocument.Equal();
            filterDocument.Constant(individualId);
            filterDocument.And();
            filterDocument.Property(PaymentMethodAccount.Properties.PaymentId, typeof(PaymentAccountType).Name);
            filterDocument.Equal();
            filterDocument.Constant(paymentMethodAccount.Id);
            PaymentMethodAccount paymentMethodAccountEntityUpdate = null;
            paymentMethodAccountEntityUpdate = (PaymentMethodAccount)DataFacadeManager.Instance.GetDataFacade().List(typeof(PaymentMethodAccount), filterDocument.GetPredicate()).FirstOrDefault();
            if (paymentMethodAccountEntityUpdate != null)
            {
                paymentMethodAccountEntityUpdate.AccountNumber = paymentMethodAccount.AccountNumber;
                paymentMethodAccountEntityUpdate.BankCode = paymentMethodAccount.Bank.Id;
                paymentMethodAccountEntityUpdate.BankBranchNumber = paymentMethodAccount.Bank.BankBranches == null ? null : (int?)paymentMethodAccount.Bank.BankBranches[0].Id;
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(paymentMethodAccountEntityUpdate);
                return ModelAssembler.CreatePaymentMethodAccount(paymentMethodAccountEntityUpdate);
            }

            return CreatePaymentMethodAccount(paymentMethodAccount, individualId);


        }
    }
}
