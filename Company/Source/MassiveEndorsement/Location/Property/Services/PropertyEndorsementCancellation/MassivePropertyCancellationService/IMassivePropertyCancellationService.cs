using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.MassiveUnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.MassivePropertyCancellationService
{
    /// <summary>
    /// Interfaz cancelacion masiva Property Hogar Multiriesgo
    /// </summary>
    [ServiceContract]
    public interface IMassivePropertyCancellationService
    {
        /// <summary>
        /// Crear temporal de cancelacion Multiriesgo
        /// </summary>
        /// <param name="risks">Lista de Riesgos</param>
        /// <param name="massiveLoadProcess">Proceso Masivo.</param>
        /// <returns>
        /// Errores
        /// </returns>
        [OperationContract]
        List<AuthorizationRequest> CreateTemporalEndorsementCancellation(MassiveLoad massiveLoad, CompanyPolicy companyPolicy, List<Risk> risks, MassiveCancellationRow massiveCancellationRow);

        /// <summary>
        /// Generar Reporte de Cancelacion Masiva Multiriesgo Por identifcador y estado del cargue
        /// </summary>
        /// <param name="MassiveLoadId">The massive load identifier.</param>
        /// <param name="massiveLoadStatus">The massive load status.</param>
        /// <returns>File</returns>
        [OperationContract]
        string GenerateReportByMassiveLoadIdByMassiveLoadStatus(int MassiveLoadId, MassiveLoadStatus massiveLoadStatus);
    }
}
