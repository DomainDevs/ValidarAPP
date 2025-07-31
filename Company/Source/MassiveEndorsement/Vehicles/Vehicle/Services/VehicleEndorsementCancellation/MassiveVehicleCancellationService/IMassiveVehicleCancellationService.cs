using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.MassiveUnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.MassiveVehicleCancellationService
{
    /// <summary>
    /// Interfaz cancelacion masiva autos
    /// </summary>
    [ServiceContract]
    public interface IMassiveVehicleCancellationService
    {
        /// <summary>
        /// Crear temporal de cancelacion Autos
        /// </summary>
        /// <param name="risks">Lista de Riesgos</param>
        /// <param name="massiveLoadProcess">Proceso Masivo.</param>
        /// <returns>
        /// Errores
        /// </returns>
        [OperationContract]
        List<AuthorizationRequest> CreateTemporalEndorsementCancellation(MassiveLoad massiveLoad,CompanyPolicy companyPolicy, List<CompanyRisk> risks, MassiveCancellationRow massiveCancellationRow);

        /// <summary>
        /// Generar Reporte de Cancelacion Masiva Autos Por identificador y estado del cargue
        /// </summary>
        /// <param name="MassiveLoadId">The massive load identifier.</param>
        /// <param name="massiveLoadStatus">The massive load status.</param>
        /// <returns>
        /// File
        /// </returns>
        string GenerateReportByMassiveLoadIdByMassiveLoadStatus(int MassiveLoadId, MassiveLoadStatus massiveLoadStatus);
    }
}
