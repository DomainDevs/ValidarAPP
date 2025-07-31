using System.ServiceModel;
using Sistran.Core.Application.ModificationEndorsement;

namespace Sistran.Core.Application.VehicleEndorsementModificationService
{
    [ServiceContract]
    public interface IVehicleModificationService : IModificationEndorsement
    {

    }
}
