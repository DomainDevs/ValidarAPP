using System.ServiceModel;
using Sistran.Core.Application.ModificationEndorsement;

namespace Sistran.Core.Application.AircraftEndorsementModificationService
{
    [ServiceContract]
    public interface IAircraftModificationService : IModificationEndorsement
    {

    }
}
