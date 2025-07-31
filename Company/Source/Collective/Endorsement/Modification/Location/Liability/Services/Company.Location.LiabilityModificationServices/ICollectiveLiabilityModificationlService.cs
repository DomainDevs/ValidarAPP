using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.MassiveServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.Location.LiabilityModificationService
{
    [ServiceContract]
    public interface ICollectiveLiabilityModificationlService
    {

        [OperationContract]
        CollectiveEmission CreateCollectiveModification(CollectiveEmission collectiveEmission);


        [OperationContract]
        void QuotateCollectiveInclusion(int collectiveEmissionId);

        [OperationContract]
        void QuotateCollectiveExclusion(int collectiveEmissionId);

        [OperationContract]
        MassiveLoad IssuanceCollectiveEmission(MassiveLoad collectiveLoad);
    }
}
