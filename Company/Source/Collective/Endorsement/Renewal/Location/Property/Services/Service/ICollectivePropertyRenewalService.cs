using Sistran.Core.Application.CollectiveServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Sistran.Core.Application.MassiveServices.Models;

namespace Sistran.Company.Application.Location.CollectivePropertyRenewalService
{
    [ServiceContract]
    public interface ICollectivePropertyRenewalService
    {
        [OperationContract]
        CollectiveEmission CreateCollectiveRenewal(CollectiveEmission collectiveEmission);

        [OperationContract]
        void QuotateCollectiveRenewal(MassiveLoad massiveLoad);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collectiveRenewal"></param>
        /// <returns></returns>
        [OperationContract]
        MassiveLoad IssuanceCollectiveRenewal(MassiveLoad collectiveRenewal);
    }
}
