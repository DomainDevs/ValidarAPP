using Sistran.Company.Application.MassiveVehicleCancellationService.EEProvider.DAOs;
using Sistran.Company.Application.MassiveVehicleCancellationService.EEProvider.Resources;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.MassiveUnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Configuration;
using System.Collections.Generic;

namespace Sistran.Company.Application.MassiveVehicleCancellationService.EEProvider
{
    /// <summary>
    /// Cancelacion masiva
    /// </summary>
    /// <seealso cref="Sistran.Company.Application.MassiveVehicleCancellationService.IMassiveVehicleCancellationService" />
    public class VehicleCancellationServiceEEProvider : IMassiveVehicleCancellationService
    {
        public VehicleCancellationServiceEEProvider()
        {
            Errors.Culture = KeySettings.ServiceResourceCulture;
        }

        /// <summary>
        /// Crear temporal de cancelacion Autos
        /// </summary>
        /// <param name="risks">Lista de Riesgos</param>
        /// <param name="massiveLoadProcess">Proceso Masivo.</param>
        /// <returns>
        /// Errores
        /// </returns>
        public List<AuthorizationRequest> CreateTemporalEndorsementCancellation(MassiveLoad massiveLoad, CompanyPolicy companyPolicy, List<CompanyRisk> risks, MassiveCancellationRow massiveCancellationRow)
        {
            MassiveCancellationDAO massiveCancellationDAO = new MassiveCancellationDAO();
            return massiveCancellationDAO.CreateTemporalEndorsementCancellation(massiveLoad, companyPolicy, risks, massiveCancellationRow);
        }

        /// <summary>
        /// Generar Reporte de Cancelacion Masiva Autos Por identificador y estado del cargue
        /// </summary>
        /// <param name="MassiveLoadId">The massive load identifier.</param>
        /// <param name="massiveLoadStatus">The massive load status.</param>
        /// <returns>
        /// File
        /// </returns>
        public string GenerateReportByMassiveLoadIdByMassiveLoadStatus(int MassiveLoadId, MassiveLoadStatus massiveLoadStatus)
        {
            MassiveCancellationDAO massiveCancellationDAO = new MassiveCancellationDAO();
            return massiveCancellationDAO.GenerateReportByMassiveLoadIdByMassiveLoadStatus(MassiveLoadId, massiveLoadStatus);
        }
    }
}
