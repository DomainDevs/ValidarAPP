using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.UniquePersonService.V1.Enums;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using modelsCommon = Sistran.Core.Application.CommonService.Models;

namespace Sistran.Core.Application.UniquePersonService.V1.DAOs
{
    /// <summary>
    /// Cuentas bancarias
    /// </summary>
    public class PaymentMethodAccountDAO
    {

        //        /// <summary>
        //        /// Buscar la informacio de madios de pagos 
        //        /// </summary>
        //        /// <param name="individualId"></param>
        //        /// <returns></returns>
        //        public List<Models.PaymentAccount> GetPaymentMethodAccountByIndividualId(int individualId)
        //        {
        //            Stopwatch stopWatch = new Stopwatch();
        //            stopWatch.Start();

        //            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
        //            filter.Property(PaymentMethodAccount.Properties.IndividualId, typeof(PaymentMethodAccount).Name);
        //            filter.Equal();
        //            filter.Constant(individualId);
        //            BusinessCollection businessCollection = new BusinessCollection();
        //            using (var daf = DataFacadeManager.Instance.GetDataFacade())
        //            {
        //                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(PaymentMethodAccount), filter.GetPredicate()));
        //            }

        //            List<Models.PaymentAccount> paymentMethodAccounts = ModelAssembler.CreatePaymentMethodAccounts(businessCollection);
        //            using (var daf = DataFacadeManager.Instance.GetDataFacade())
        //            {
        //                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(IndividualPaymentMethod), filter.GetPredicate()));
        //            }
        //            List<IndividualPaymentMethod> IndividualPaymentMethod = businessCollection.Cast<IndividualPaymentMethod>().ToList();
        //            if (IndividualPaymentMethod != null && IndividualPaymentMethod.Count > 0)
        //            {
        //                paymentMethodAccounts.Add(new Models.PaymentAccount { Id = IndividualPaymentMethod.Where(x => x.PaymentMethodCode == (int)PaymentMethodType.Cash).FirstOrDefault().PaymentId, PaymentMethod = new modelsCommon.PaymentMethod { Id = IndividualPaymentMethod.Where(x => x.PaymentMethodCode == (int)PaymentMethodType.Cash).FirstOrDefault().PaymentMethodCode } });
        //            }
        //            using (var daf = DataFacadeManager.Instance.GetDataFacade())
        //            {
        //                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(PaymentAccountType)));
        //            }
        //            List<Models.PaymentAccountType> paymentAccountTypes = ModelAssembler.CreateAccountTypes(businessCollection);

        //            List<modelsCommon.PaymentMethod> paymentMethods = DelegateService.commonServiceCore.GetPaymentMethods();

        //            foreach (Models.PaymentAccount item in paymentMethodAccounts)
        //            {
        //                if (IndividualPaymentMethod.Count > 0)
        //                {
        //                    item.PaymentMethod.Id = IndividualPaymentMethod.Where(x => x.PaymentId == item.Id).FirstOrDefault().PaymentMethodCode;
        //                }
        //                if (item.Bank != null)
        //                {
        //                    item.Bank.Description = DelegateService.commonServiceCore.GetBanksByBankId(item.Bank.Id).Description;
        //                }
        //                if (item.AccountType != null)
        //                {
        //                    item.AccountType.Description = paymentAccountTypes.First(x => x.Id == item.AccountType.Id).Description;
        //                }
        //                item.PaymentMethod.Description = paymentMethods.First(x => x.Id == item.PaymentMethod.Id).Description;
        //            }

        //            stopWatch.Stop();
        //            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetPaymentMethodAccountByIndividualId");
        //            return paymentMethodAccounts;
        //        }
        //        /// <summary>
        //        /// Gets the payment method account by individual identifier by payment metod by payment account.
        //        /// </summary>
        //        /// <param name="filter">The filter.</param>
        //        /// <returns></returns>
        //        public PaymentMethodAccount GetPaymentMethodAccountByIndividualIdByPaymentMetodByPaymentAccount(ObjectCriteriaBuilder filter)
        //        {
        //            PaymentMethodAccount paymentMethodAccount = null;
        //            using (var daf = DataFacadeManager.Instance.GetDataFacade())
        //            {
        //                paymentMethodAccount = (PaymentMethodAccount)daf.List(typeof(PaymentMethodAccount), filter.GetPredicate()).FirstOrDefault();
        //            }
        //            return paymentMethodAccount;
        //        }
        //        /// <summary>
        //        /// Creates the payment method account.
        //        /// </summary>
        //        /// <param name="paymentMethodAccount">The payment method account.</param>
        //        /// <param name="IndividualId">The individual identifier.</param>
        //        /// <returns></returns>
        //        public Models.PaymentAccount CreatePaymentMethodAccount(Models.PaymentAccount paymentMethodAccount, int IndividualId)
        //        {
        //            Stopwatch stopWatch = new Stopwatch();
        //            stopWatch.Start();
        //            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
        //            filter.Property(PaymentMethodAccount.Properties.IndividualId, typeof(PaymentMethodAccount).Name);
        //            filter.Equal();
        //            filter.Constant(IndividualId);
        //            filter.And();
        //            filter.Property(PaymentMethodAccount.Properties.PaymentId, typeof(PaymentMethodAccount).Name);
        //            filter.Equal();
        //            filter.Constant(paymentMethodAccount.Id);
        //            filter.And();
        //            filter.Property(PaymentMethodAccount.Properties.PaymentAccountTypeCode, typeof(PaymentMethodAccount).Name);
        //            filter.Equal();
        //            filter.Constant(paymentMethodAccount.AccountType.Id);
        //            filter.And();
        //            filter.Property(PaymentMethodAccount.Properties.BankCode, typeof(PaymentMethodAccount).Name);
        //            filter.Equal();
        //            filter.Constant(paymentMethodAccount.Bank.Id);
        //            PaymentMethodAccount payment = GetPaymentMethodAccountByIndividualIdByPaymentMetodByPaymentAccount(filter);
        //            if (payment == null)
        //            {
        //                PaymentMethodAccount paymentMethodAccountEntity = EntityAssembler.CreatePaymentMethodAccount(paymentMethodAccount, IndividualId);
        //                using (var daf = DataFacadeManager.Instance.GetDataFacade())
        //                {
        //                    daf.InsertObject(paymentMethodAccountEntity);
        //                }
        //                stopWatch.Stop();
        //                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.CreatePaymentMethodAccount");
        //                return ModelAssembler.CreatePaymentMethodAccount(paymentMethodAccountEntity);
        //            }
        //            else
        //            {
        //                payment.AccountNumber = paymentMethodAccount.AccountNumber;
        //                payment.BankCode = paymentMethodAccount.Bank.Id;
        //                payment.PaymentAccountTypeCode = paymentMethodAccount.AccountType.Id;
        //                using (var daf = DataFacadeManager.Instance.GetDataFacade())
        //                {
        //                    daf.UpdateObject(payment);
        //                }
        //                stopWatch.Stop();
        //                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.CreatePaymentMethodAccount");
        //                return ModelAssembler.CreatePaymentMethodAccount(payment);
        //            }
        //        }

        //        public Models.PaymentAccount UpdatePaymentMethodAccount(Models.PaymentAccount paymentMethodAccount, int individualId)
        //        {
        //            //validar que no exista
        //            ObjectCriteriaBuilder filterDocument = new ObjectCriteriaBuilder();
        //            filterDocument.Property(PaymentMethodAccount.Properties.IndividualId, typeof(PaymentAccountType).Name);
        //            filterDocument.Equal();
        //            filterDocument.Constant(individualId);
        //            filterDocument.And();
        //            filterDocument.Property(PaymentMethodAccount.Properties.PaymentId, typeof(PaymentAccountType).Name);
        //            filterDocument.Equal();
        //            filterDocument.Constant(paymentMethodAccount.Id);
        //            PaymentMethodAccount paymentMethodAccountEntityUpdate = null;
        //            using (var daf = DataFacadeManager.Instance.GetDataFacade())
        //            {
        //                paymentMethodAccountEntityUpdate = (PaymentMethodAccount)daf.List(typeof(PaymentMethodAccount), filterDocument.GetPredicate()).FirstOrDefault();
        //            }
        //            if (paymentMethodAccountEntityUpdate != null)
        //            {
        //                paymentMethodAccountEntityUpdate.AccountNumber = paymentMethodAccount.AccountNumber;
        //                paymentMethodAccountEntityUpdate.BankCode = paymentMethodAccount.Bank.Id;
        //                paymentMethodAccountEntityUpdate.BankBranchNumber = paymentMethodAccount.Bank.BankBranches == null ? null : (int?)paymentMethodAccount.Bank.BankBranches[0].Id;
        //                using (var daf = DataFacadeManager.Instance.GetDataFacade())
        //                {
        //                    daf.UpdateObject(paymentMethodAccountEntityUpdate);
        //                }
        //                return ModelAssembler.CreatePaymentMethodAccount(paymentMethodAccountEntityUpdate);
        //            }

        //            return CreatePaymentMethodAccount(paymentMethodAccount, individualId);


        //        }
    }
}
