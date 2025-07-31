using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Sistran.Company.Application.CollectionFormBusinessService.Models;

namespace Sistran.Company.Application.CollectionFormBusinessService
{
    [ServiceContract]
    public interface ICompanyCollectionFormBusinessService
    {
        [OperationContract]
        String[] GenerateCollectionForm(CollectionForm collectionForm);

        [OperationContract]
        ReportPolicy GetPolicybyBranchPrefixDocument(int branchId, int prefixId, int documentNumber);
    }
}
