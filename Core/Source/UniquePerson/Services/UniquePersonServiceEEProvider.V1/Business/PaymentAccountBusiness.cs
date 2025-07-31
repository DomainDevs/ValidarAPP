using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UniquePersonService.V1.Business
{
    public class PaymentAccountBusiness
    {

        /// <summary>
        /// CreatePaymentAccount
        /// </summary>
        /// <param name="individualPaymentMethod"></param>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public Models.PaymentAccount CreatePaymentAccount(Models.PaymentAccount paymentAccount, int paymentId, int individualId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PaymentMethodAccount.Properties.IndividualId, typeof(PaymentMethodAccount).Name);
            filter.Equal();
            filter.Constant(individualId);
            filter.And();
            filter.Property(PaymentMethodAccount.Properties.PaymentId, typeof(PaymentMethodAccount).Name);
            filter.Equal();
            filter.Constant(paymentId);
            filter.And();
            filter.Property(PaymentMethodAccount.Properties.PaymentAccountTypeCode, typeof(PaymentMethodAccount).Name);
            filter.Equal();
            filter.Constant(paymentAccount.Type.Id);
            filter.And();
            filter.Property(PaymentMethodAccount.Properties.BankCode, typeof(PaymentMethodAccount).Name);
            filter.Equal();
            filter.Constant(paymentAccount.BankBranch.Bank.Id);
            PaymentMethodAccount paymentMethodAccountEntity = EntityAssembler.CreatePaymentMethodAccount(paymentAccount, individualId, paymentId);
            DataFacadeManager.Insert(paymentMethodAccountEntity);
            return ModelAssembler.CreatePaymentMethodAccount(paymentMethodAccountEntity);
        }

        /// <summary>
        /// Gets the payment method account by individual identifier by payment metod by payment account.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public Models.PaymentAccount GetPaymentMethodAccountByIndividualIdByPaymentMetodByPaymentAccount(Models.PaymentAccount paymentAccount, int paymentId, int individualId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PaymentMethodAccount.Properties.IndividualId, typeof(PaymentMethodAccount).Name);
            filter.Equal();
            filter.Constant(individualId);
            filter.And();
            filter.Property(PaymentMethodAccount.Properties.PaymentId, typeof(PaymentMethodAccount).Name);
            filter.Equal();
            filter.Constant(paymentId);
            filter.And();
            filter.Property(PaymentMethodAccount.Properties.PaymentAccountTypeCode, typeof(PaymentMethodAccount).Name);
            filter.Equal();
            filter.Constant(paymentAccount.Type?.Id ?? 0);
            filter.And();
            filter.Property(PaymentMethodAccount.Properties.BankCode, typeof(PaymentMethodAccount).Name);
            filter.Equal();
            filter.Constant(paymentAccount.BankBranch?.Bank?.Id ?? 0);

            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                var result = (PaymentMethodAccount)daf.List(typeof(PaymentMethodAccount), filter.GetPredicate()).FirstOrDefault();
                if (result != null)
                {
                    return ModelAssembler.CreatePaymentMethodAccount(result);
                }
                else
                {
                    return null;
                }

            }
        }

        /// <summary>
        /// GetPaymentMethodAccountByIndividualId
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns>List<Models.PaymentAccount></returns>
        public List<Models.PaymentAccount> GetPaymentMethodAccountByIndividualId(int individualId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PaymentMethodAccount.Properties.IndividualId, typeof(PaymentMethodAccount).Name);
            filter.Equal();
            filter.Constant(individualId);

            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                BusinessCollection businessCollection = new BusinessCollection(daf.SelectObjects(typeof(PaymentMethodAccount), filter.GetPredicate()));
                return ModelAssembler.CreatePaymentMethodAccounts(businessCollection);
            }

        }

        /// <summary>
        /// UpdatePaymentMethodAccount
        /// </summary>
        /// <param name="paymentMethodAccount"></param>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public Models.PaymentAccount UpdatePaymentMethodAccount(Models.PaymentAccount paymentAccount, int paymentId, int individualId)
        {
            PaymentMethodAccount paymentMethodAccountEntity = EntityAssembler.CreatePaymentMethodAccount(paymentAccount, individualId, paymentId);
            paymentMethodAccountEntity.AccountNumber = paymentAccount.Number;
            paymentMethodAccountEntity.BankCode = paymentAccount.BankBranch.Bank.Id;
            DataFacadeManager.Update(paymentMethodAccountEntity);
            return ModelAssembler.CreatePaymentMethodAccount(paymentMethodAccountEntity);
        }

    }
}
