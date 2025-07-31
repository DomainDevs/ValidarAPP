using Sistran.Company.Application.MassiveTPLCancellationService.EEProvider.DAOs;
using Sistran.Company.Application.MassiveTPLCancellationServices.EEProvider.Resources;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.MassiveUnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Configuration;
using System.Collections.Generic;

namespace Sistran.Company.Application.MassiveTPLCancellationServices.EEProvider
{
    /// <summary>
    /// Cancelacion masiva
    /// </summary>
    /// <seealso cref="Sistran.Company.Application.MassiveTPLCancellationService.IMassiveTPLCancellationService" />
    public class TPLCancellationServiceEEProvider : IMassiveTPLCancellationServices
    {
        public TPLCancellationServiceEEProvider()
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
            MassiveTPLCancellationDAO massiveCancellationDAO = new MassiveTPLCancellationDAO();
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
            MassiveTPLCancellationDAO massiveCancellationDAO = new MassiveTPLCancellationDAO();
            return massiveCancellationDAO.GenerateReportByMassiveLoadIdByMassiveLoadStatus(MassiveLoadId, massiveLoadStatus);
        }
    }
}
