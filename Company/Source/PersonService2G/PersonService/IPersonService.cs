using PersonService.Model;
using System.Collections.Generic;
using System.ServiceModel;

namespace PersonService
{   
    [ServiceContract]
    public interface IPersonService
    {   
        [OperationContract]
        List<Person> GetPerson2gByDocumentNumber(string documentNumber, bool company);

        [OperationContract]
        List<Person> GetPerson2gByPersonId(int personId, bool company);

        [OperationContract]
        List<Company> GetCompany2gByCompanyId(int companyId, bool company);

        [OperationContract]
        Provider GetProvider2g(int personId);

        [OperationContract]
        List<BankTransfer> GetBankTransfer2g(int transferPersonId);

        [OperationContract]
        List<Tax> GetTax2g(int taxPersonId);
    }
}
