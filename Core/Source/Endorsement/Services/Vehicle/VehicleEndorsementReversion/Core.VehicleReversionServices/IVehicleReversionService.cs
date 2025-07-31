using Sistran.Core.Application.ReversionEndorsement;
using System.ServiceModel;

namespace Sistran.Core.Application.VehicleEndorsementReversionService
{
    [ServiceContract]
    public interface IVehicleReversionService : IReversionEndorsement
    {
        [OperationContract]
        object GetVehicleEndorsementReversion(int idVehicleEndorsementReversion);

        [OperationContract]
        int ReversionPolicy(int policyId, string conditionText, int endorsementReason, int userId, string annotations);
    }
}
