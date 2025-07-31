using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Assemblers;
using Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;

namespace Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Business
{
    public class CompanyIndividualPaymentMethodBusiness
    {
        private Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider coreProvider;

        public CompanyIndividualPaymentMethodBusiness()
        {
            coreProvider = new Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider();
        }

        internal List<CompanyIndividualPaymentMethod> GetIndividualPaymentMethods(int individualId)
        {
            var imapper = ModelAssembler.CreateMapperIndividualPaymentMethod();
            var result = coreProvider.GetIndividualPaymentMethods(individualId);
            return imapper.Map<List<IndividualPaymentMethod>, List<CompanyIndividualPaymentMethod>>(result);
        }

        internal List<CompanyIndividualPaymentMethod> CreateIndividualPaymentMethods(List<CompanyIndividualPaymentMethod> companyIndividualPaymentMethods, int individualId)
        {

            List<CompanyIndividualPaymentMethod> list = new List<CompanyIndividualPaymentMethod>();

            var imap = ModelAssembler.CreateMapperIndividualPaymentMethod();

            foreach (var item in companyIndividualPaymentMethods)
            {
                PaymentAccount resultPaymentAccount = null;
                if (item.Account != null)
                {
                    var companyCompanyPaymentAccountCore = imap.Map<CompanyPaymentAccount, PaymentAccount>(item.Account);
                    if(companyCompanyPaymentAccountCore != null)
                    {
                        resultPaymentAccount = coreProvider.GetPaymentMethodAccountByIndividualIdByPaymentMetodByPaymentAccount(companyCompanyPaymentAccountCore, item.Id, individualId);
                    }                    
                }                

                if (resultPaymentAccount == null && item.Id == 0)
                {
                    list.Add(CreateIndividualPaymentMethod(item, individualId));
                }
                else
                {
                    list.Add(UpdateIndividualPaymentMethod(item, individualId));
                }
            }
            return list;
        }

        internal CompanyIndividualPaymentMethod CreateIndividualPaymentMethod(CompanyIndividualPaymentMethod companyIndividualPaymentMethod, int individualId)
        {
            var map = ModelAssembler.CreateMapperIndividualPaymentMethod();
            var companyIndividualPaymentMethodCore = map.Map<CompanyIndividualPaymentMethod, IndividualPaymentMethod>(companyIndividualPaymentMethod);
            var result = coreProvider.CreateIndividualPaymentMethod(companyIndividualPaymentMethodCore, individualId);
            return map.Map<IndividualPaymentMethod, CompanyIndividualPaymentMethod>(result);
        }

        internal CompanyIndividualPaymentMethod UpdateIndividualPaymentMethod(CompanyIndividualPaymentMethod companyIndividualPaymentMethod, int individualId)
        {
            var imapper = ModelAssembler.CreateMapperIndividualPaymentMethod();
            var companyIndividualPaymentMethodCore = imapper.Map<CompanyIndividualPaymentMethod, IndividualPaymentMethod>(companyIndividualPaymentMethod);
            var result = coreProvider.UpdateIndividualPaymentMethod(companyIndividualPaymentMethodCore, individualId);
            return imapper.Map<IndividualPaymentMethod, CompanyIndividualPaymentMethod>(result);
        }


    }
}
