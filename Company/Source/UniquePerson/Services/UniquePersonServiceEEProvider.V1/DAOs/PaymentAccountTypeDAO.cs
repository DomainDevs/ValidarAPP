using Sistran.Company.Application.UniquePersonAplicationServices.EEProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using UPENT = Sistran.Core.Application.UniquePersonV1.Entities;

namespace Sistran.Company.Application.UniquePersonServices.V1.EEProvider.DAOs
{
    public class PaymentAccountTypeDAO
    {
        /// <summary>
        /// Busca datos de una persona
        /// </summary>
        /// <param name="individualId">IndividualId</param>
        /// <returns></returns>
        public List<Models.CompanyPaymentAccountType> getCompanyPaymentAccountTypes()
        {
            List<Models.CompanyPaymentAccountType> companyPaymentAccountType = new List<Models.CompanyPaymentAccountType>();
           var businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UPENT.PaymentAccountType)));
            if (businessCollection != null && businessCollection.Count > 0)
            {
               foreach(UPENT.PaymentAccountType item in businessCollection)
                {
                    companyPaymentAccountType.Add(new Models.CompanyPaymentAccountType() { 
                        Description = item.Description,
                        Id = item.PaymentAccountTypeCode
                        });

                }
                return companyPaymentAccountType;
            }

            return null;
        }
    }
}
