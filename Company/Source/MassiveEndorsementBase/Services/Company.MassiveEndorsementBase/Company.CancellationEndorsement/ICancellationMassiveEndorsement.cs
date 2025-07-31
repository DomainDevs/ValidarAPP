using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniqueUserServices.Models;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.CancellationMsvEndorsementServices
{
    [ServiceContract]
    public interface ICancellationMassiveEndorsementServices
    {
        /// <summary>
        /// Tarifar Cancelación de Póliza
        /// </summary>
        /// <param name="policy">Póliza</param>
        /// <param name="cancellationFactor">Factor de Cancelación</param>
        /// <returns>Riesgos</returns>
        [OperationContract]
        List<CompanyRisk> QuotateCancellation(CompanyPolicy policy, int cancellationFactor);

        /// <summary>
        /// Cancelacion masiva por Identificador
        /// </summary>
        /// <param name="massiveLoadId">The massive load identifier.</param>
        /// <returns></returns>
        [OperationContract]
        MassiveLoad CancellationMassiveByMassiveLoadId(MassiveLoad massiveLoad);

        /// <summary>
        /// Crear Cancelacion masiva
        /// </summary>
        /// <param name="massiveLoad">The massive load.</param>
        /// <returns></returns>
        [OperationContract]
        MassiveLoad CreateMassiveLoad(MassiveLoad massiveLoad);

        /// <summary>
        /// Emision Cancelacion Masiva
        /// </summary>
        /// <param name="massiveLoadId">identificador del Cargue masiva</param>
        /// <returns></returns>
        [OperationContract]
        MassiveLoad CreateIssuePolicy(MassiveLoad massiveLoad);


        /// <summary>
        /// Generar Reporte de Cancelacion Masiva por Identificador y estado del Cargue
        /// </summary>
        /// <param name="MassiveLoadId">The massive load identifier.</param>
        /// <param name="massiveLoadStatus">The massive load status.</param>
        /// <returns></returns>
        [OperationContract]
        string GenerateReportByMassiveLoadIdByMassiveLoadStatus(int MassiveLoadId, MassiveLoadStatus massiveLoadStatus);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="massiveLoadId"></param>
        /// <param name="rangeFrom"></param>
        /// <param name="rangeTo"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [OperationContract]
        string PrintCancellationMassive(int massiveLoadId, int rangeFrom, int rangeTo, User user, bool checkIssuedDetail);
    }
}
