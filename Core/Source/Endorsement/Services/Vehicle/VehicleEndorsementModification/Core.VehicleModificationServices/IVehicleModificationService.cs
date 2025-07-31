using Sistran.Core.Application.ModificationEndorsement;
using System.ServiceModel;

namespace Sistran.Core.Application.VehicleEndorsementModificationService
{
    [ServiceContract]
    public interface IVehicleModificationService : IModificationEndorsement
    {
        
    }
}
