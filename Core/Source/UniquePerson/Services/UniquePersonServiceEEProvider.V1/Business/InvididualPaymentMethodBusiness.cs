using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.UniquePersonService.V1.Business;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using modelsCommon = Sistran.Core.Application.CommonService.Models;

namespace Sistran.Core.Application.UniquePersonService.V1.DAOs
{
    public class InvididualPaymentMethodBusiness
    {

        /// <summary>
        /// GetIndividualPaymentMethodByindividualId
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns>List<Models.IndividualPaymentMethod> </returns>
        public List<Models.IndividualPaymentMethod> GetIndividualPaymentMethodByindividualId(int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(IndividualPaymentMethod.Properties.IndividualId, typeof(IndividualPaymentMethod).Name);
            filter.Equal();
            filter.Constant(individualId);

            List<Models.IndividualPaymentMethod> individualPaymentMethods;

            // Get IndividualPaymentMethod        
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                BusinessCollection businessCollection = new BusinessCollection(daf.SelectObjects(typeof(IndividualPaymentMethod), filter.GetPredicate()));
                individualPaymentMethods = ModelAssembler.CreatePaymentMethods(businessCollection);
            }

            // Get PaymentAccount
            PaymentAccountBusiness paymentAccountBusiness = new PaymentAccountBusiness();
            List<Models.PaymentAccount> paymentAccounts = paymentAccountBusiness.GetPaymentMethodAccountByIndividualId(individualId);

            // Get PaymentMethod
            List<modelsCommon.PaymentMethod> paymentMethods = DelegateService.commonServiceCore.GetPaymentMethods();


            List<Models.IndividualPaymentMethod> PaymentMethods = new List<Models.IndividualPaymentMethod>();

            foreach (Models.IndividualPaymentMethod item in individualPaymentMethods)
            {
                Models.IndividualPaymentMethod individualPaymentMethod = item;

                if (paymentAccounts.Count > 0)
                {
                    item.Account = paymentAccounts.FirstOrDefault(x => x.Id == item.Id);
                }

                if (individualPaymentMethod.Account?.Type != null)
                {
                    PaymentAccountTypeBusiness paymentAccountTypeBusiness = new PaymentAccountTypeBusiness();
                    individualPaymentMethod.Account.Type = paymentAccountTypeBusiness.GetPaymentAccountTypeByPaymentAccountTypeId(item.Account.Type.Id);
                }

                if (individualPaymentMethod.Account?.BankBranch != null && individualPaymentMethod.Account?.BankBranch?.Bank != null)
                {
                    List<BankBranch> bankBranchs = DelegateService.commonServiceCore.GetBankBranches(individualPaymentMethod.Account.BankBranch.Bank.Id);
                    if (individualPaymentMethod.Account?.BankBranch.Id > 0)
                    {
                        individualPaymentMethod.Account.BankBranch.Description = bankBranchs.First(x => x.Id == individualPaymentMethod.Account?.BankBranch.Id).Description;
                        individualPaymentMethod.Account.BankBranch.Bank.Description = DelegateService.commonServiceCore.GetBanksByBankId(item.Account.BankBranch.Bank.Id).Description;
                    }
                }

                if (individualPaymentMethod.Rol != null)
                {
                    RoleBusiness roleBusiness = new RoleBusiness();
                    individualPaymentMethod.Rol.Id = item.Rol.Id;
                }

                if (individualPaymentMethod.Method != null)
                {
                    if (paymentMethods.First(x => x.Id == item.Method.Id) != null)
                    {
                        individualPaymentMethod.Method.Description = paymentMethods.First(x => x.Id == item.Method.Id).Description;
                        individualPaymentMethod.Method.Id = paymentMethods.First(x => x.Id == item.Method.Id).Id;

                    }
                }
                PaymentMethods.Add(individualPaymentMethod);
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetPaymentMethodAccountByIndividualId");
            return PaymentMethods;
        }

        /// <summary>
        /// CreateIndividualPaymentMethod
        /// </summary>
        /// <param name="individualPaymentMethod"></param>
        /// <param name="individualId"></param>
        /// <returns>IndividualPaymentMethod</returns>
        public Models.IndividualPaymentMethod CreateIndividualPaymentMethod(Models.IndividualPaymentMethod individualPaymentMethod, int individualId)
        {
            IndividualPaymentMethod individualPaymentMethodEntity = EntityAssembler.CreatePaymentMethod(individualPaymentMethod, individualId);
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(IndividualPaymentMethod.Properties.IndividualId);
            filter.Equal();
            filter.Constant(individualId);
            SelectQuery selectQuery = new SelectQuery();
            Function funtion = new Function(FunctionType.Max);
            funtion.AddParameter(new Column(IndividualPaymentMethod.Properties.PaymentId));
            selectQuery.Table = new ClassNameTable(typeof(IndividualPaymentMethod), "PaymentId");
            selectQuery.AddSelectValue(new SelectValue(funtion));
            selectQuery.Where = filter.GetPredicate();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    individualPaymentMethodEntity.PaymentId = (Convert.ToInt32(reader[0]) + 1);
                }
            }
            DataFacadeManager.Insert(individualPaymentMethodEntity);
            return ModelAssembler.CreatePaymentMethod(individualPaymentMethodEntity);
        }

        /// <summary>
        /// UpdateIndividualPaymentMethod
        /// </summary>
        /// <param name="individualPaymentMethod"></param>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public Models.IndividualPaymentMethod UpdateIndividualPaymentMethod(Models.IndividualPaymentMethod individualPaymentMethod, int individualId)
        {
            IndividualPaymentMethod individualPaymentMethodEntity = EntityAssembler.CreatePaymentMethod(individualPaymentMethod, individualId);
            DataFacadeManager.Update(individualPaymentMethodEntity);
            return ModelAssembler.CreatePaymentMethod(individualPaymentMethodEntity);
        }

    }

}
