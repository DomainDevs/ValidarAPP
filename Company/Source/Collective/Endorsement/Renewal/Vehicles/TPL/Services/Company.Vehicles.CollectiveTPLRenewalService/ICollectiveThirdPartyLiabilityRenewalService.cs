using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.MassiveServices.Models;
using System.ServiceModel;

namespace Sistran.Company.Application.Vehicles.CollectiveTPLRenewalService
{
    [ServiceContract]
    public interface ICollectiveThirdPartyLiabilityRenewalService
    {
        [OperationContract]
        CollectiveEmission CreateCollectiveRenewal(CollectiveEmission collectiveLoad);

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