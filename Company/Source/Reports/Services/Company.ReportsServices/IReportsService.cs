

namespace Sistran.Company.Application.ReportsServices
{
    using Sistran.Company.Application.ModelServices.Models.Reports;
    using Sistran.Company.Application.ReportsServices.Models;
    using System.Collections.Generic;
    using System.ServiceModel;

    [ServiceContract]
    public interface IReportsService
    {
        /// <summary>
        /// Consulta el historial de data credito por numero y tipo de documento
        /// </summary>
        /// <param name="idCardTypeCd">tipo de documento</param>
        /// <param name="idCardNo">Numero de documento</param>
        /// <returns>Lista con el histrial de consultas a datacredito</returns>
        [OperationContract]
        ScoreCreditsServiceModel GetScoredCredit(int idCardTypeCd, string idCardNo);

        [OperationContract]
        VehiclesParametersServiceModel GetVehicleParameter(int accionType, int vehicleYearInit, int vehicleYearEnd, string makeId, string modelId);
    }
}
