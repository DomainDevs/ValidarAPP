using Sistran.Company.Application.MassivePropertyCancellationService.EEProvider.DAOs;
using Sistran.Company.Application.MassivePropertyCancellationService.EEProvider.Resources;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.MassiveUnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Configuration;
using System.Collections.Generic;

namespace Sistran.Company.Application.MassivePropertyCancellationService.EEProvider
{
    /// <summary>
    /// Cancelacion masiva Property
    /// </summary>
    /// <seealso cref="Sistran.Company.Application.MassivePropertyCancellationService.IMassiveVehicleCancellationService" />
    public class MassivePropertyCancellationServiceEEProvider : IMassivePropertyCancellationService
    {
        public MassivePropertyCancellationServiceEEProvider()
        {
            Errors.Culture = KeySettings.ServiceResourceCulture;
        }

        /// <summary>
        /// Crear temporal de cancelacion Ubicacion
        /// </summary>
        /// <param name="risks">Lista de Riesgos</param>
        /// <param name="massiveLoadProcess">Proceso Masivo.</param>
        /// <returns>
        /// Errores
        /// </returns>
        public List<AuthorizationRequest> CreateTemporalEndorsementCancellation(MassiveLoad massiveLoad, CompanyPolicy companyPolicy, List<Risk> risks, MassiveCancellationRow massiveCancellationRow)
        {
            MassiveCancellationDAO massiveCancellationDAO = new MassiveCancellationDAO();
            return massiveCancellationDAO.CreateTemporalEndorsementCancellation(massiveLoad, companyPolicy, risks, massiveCancellationRow);
        }

        /// <summary>
        /// Generar Reporte de Cancelacion Masiva Multiriesgo Por identificador y estado del cargue
        /// </summary>
        /// <param name="MassiveLoadId">The massive load identifier.</param>
        /// <param name="massiveLoadStatus">The massive load status.</param>
        /// <returns>
        /// string
        /// </returns>
        public string GenerateReportByMassiveLoadIdByMassiveLoadStatus(int MassiveLoadId, MassiveLoadStatus massiveLoadStatus)
        {
            MassiveCancellationDAO massiveCancellationDAO = new MassiveCancellationDAO();
           return  massiveCancellationDAO.GenerateReportByMassiveLoadIdByMassiveLoadStatus(MassiveLoadId, massiveLoadStatus);
        }
    }
}
