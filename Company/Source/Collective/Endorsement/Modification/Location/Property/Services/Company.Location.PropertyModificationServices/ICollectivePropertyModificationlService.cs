using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.MassiveServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.Location.PropertyModificationService
{
    [ServiceContract]
    public interface ICollectivePropertyModificationlService
    {

        [OperationContract]
        CollectiveEmission CreateCollectiveModification(CollectiveEmission collectiveEmission);


        [OperationContract]
        void QuotateCollectiveInclusion(MassiveLoad massiveLoad);

        [OperationContract]
        void QuotateCollectiveExclusion(MassiveLoad massiveLoad);

        [OperationContract]
        MassiveLoad IssuanceCollectiveEmission(MassiveLoad collectiveLoad);
    }
}
