using Sistran.Company.Application.MassiveLiabilityCancellationService.EEProvider.DAOs;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveUnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using System.Collections.Generic;

namespace Sistran.Company.Application.MassiveLiabilityCancellationService.EEProvider
{
    /// <summary>
    /// Cancelacion masiva Liability
    /// </summary>
    /// <seealso cref="Sistran.Company.Application.MassiveLiabilityCancellationService.IMassiveVehicleCancellationService" />
    public class MassiveLiabilityCancellationServiceEEProvider : IMassiveLiabilityCancellationService
    {

        /// <summary>
        /// Crear temporal de cancelacion Ubicacion
        /// </summary>
        /// <param name="risks">Lista de Riesgos</param>
        /// <param name="massiveLoadProcess">Proceso Masivo.</param>
        /// <returns>
        /// Errores
        /// </returns>
        public void CreateTemporalEndorsementCancellation(CompanyPolicy companyPolicy, List<Risk> risks, MassiveCancellationRow massiveCancellationRow)
        {
            MassiveCancellationDAO massiveCancellationDAO = new MassiveCancellationDAO();
            massiveCancellationDAO.CreateTemporalEndorsementCancellation(companyPolicy, risks, massiveCancellationRow);
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
