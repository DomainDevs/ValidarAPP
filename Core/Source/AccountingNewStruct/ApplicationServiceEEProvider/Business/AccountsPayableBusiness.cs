using Sistran.Core.Application.UniquePersonService.V1.Models;
using System.Collections.Generic;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Business
{
    internal class AccountsPayableBusiness
    {
        /// <summary>
        /// GetIndividualsByIndividualId
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public Individual GetIndividualsByIndividualId(int individualId)
        {
            string documentNumber = "";
            string Name = "";
            int documentType = 0;
            Person person = DelegateService.personService.GetPersonByIndividualId(individualId);
            if (person != null && person.IndividualId > 0)
            {
                documentNumber = person.IdentificationDocument.Number;
                Name = person.Name;
                documentType = (int)person.IndividualType;
            }
            else 
            {
                Insured insured = DelegateService.personService.GetInsuredByIndividualId(individualId);
                if (insured != null && insured.IndividualId > 0)
                {
                    documentNumber = insured.IdentificationDocument;
                    Name = insured.FullName;
                    documentType = (int)insured.IndividualType;
                }
                else {
                    Company company = DelegateService.personService.GetCompanyByIndividualId(individualId);
                    if (company != null && company.IndividualId > 0)
                    {
                        documentNumber = company.IdentificationDocument.Number;
                        Name = company.FullName;
                        documentType = (int)company.IndividualType;
                    }
                }
            }


            return new Individual()
            {
                IdentificationDocument = new IdentificationDocument() { Number = documentNumber, DocumentType = new DocumentType() { Id = documentType } },
                FullName = Name,
                IndividualId = individualId
            }; 
        }
    }
}

