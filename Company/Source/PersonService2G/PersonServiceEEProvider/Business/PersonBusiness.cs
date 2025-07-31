using PersonService;
using PersonService.Model;
using PersonServiceEEProvider.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonServiceEEProvider.Business
{
    internal class PersonBusiness : IPersonService
    {
        public List<Person> GetPerson2gByDocumentNumber(string documentNumber, bool company)
        {
            PersonDAO personDAO = new PersonDAO();
            return personDAO.GetPerson2gByDocumentNumber(documentNumber, company);
        }


        public List<Person> GetPerson2gByPersonId(int personId, bool company)
        {
            PersonDAO personDAO = new PersonDAO();
            return personDAO.GetPerson2gByPersonId(personId, company);

        }

        public List<Company> GetCompany2gByCompanyId(int companyId, bool company)
        {
            PersonDAO personDAO = new PersonDAO();
            return personDAO.GetCompany2gByCompanyId(companyId, company);

        }

        public Provider GetProvider2g(int personId)
        {
            PersonDAO personDAO = new PersonDAO();
            return personDAO.GetProvider2g(personId);
        }

        public List<BankTransfer> GetBankTransfer2g(int transferPersonId)
        {
            PersonDAO personDAO = new PersonDAO();
            return personDAO.GetBankTransfer2g(transferPersonId);
        }

        public List<Tax> GetTax2g(int taxPersonId)
        {
            PersonDAO personDAO = new PersonDAO();
            return personDAO.GetTax2g(taxPersonId);
        }
    }
}
