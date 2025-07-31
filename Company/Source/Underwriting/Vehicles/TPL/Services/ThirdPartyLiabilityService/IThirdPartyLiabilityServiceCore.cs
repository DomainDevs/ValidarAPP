using Sistran.Core.Application.Vehicles.ThirdPartyLiabilityService.Models;
using System.Collections.Generic;
using System.ServiceModel;
using Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Core.Application.Vehicles.ThirdPartyLiabilityService
{
    [ServiceContract]
    public interface IThirdPartyLiabilityServiceCore : Sistran.Core.Application.Vehicles.IVehicles
    {
        /// <summary>
        /// Obtener trayectos habilitados
        /// </summary>
        /// <returns>Lista de de trayectos</returns>
        [OperationContract]
        List<Shuttle> GetShuttlesEnabled();

        /// <summary>
        /// Obtener deducibles por producto
        /// </summary>
        /// <param name="productId">Id producto</param>
        /// <returns>Lista de deducibles</returns>
        [OperationContract]
        List<Deductible> GetDeductiblesByProductId(int productId);
    }
}
