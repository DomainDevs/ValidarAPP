using System.Collections.Generic;
using System.ServiceModel;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.MassiveRenewalServices;
using Sistran.Core.Application.MassiveRenewalServices.Models;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Services.UtilitiesServices.Models;

namespace Sistran.Company.Application.MassiveRenewalServices
{
    [ServiceContract]
    public interface IMassiveRenewalService : IMassiveRenewalServiceCore
    {
        /// <summary>
        /// Generar Proceso De Renovación Masiva
        /// </summary>
        /// <param name="userId">Id Usuario</param>
        /// <param name="description">Descripción Proceso</param>
        /// <param name="policies">Pólizas a Renovar</param>
        /// <returns>Id Proceso</returns>
        [OperationContract]
        int GenerateProcessRenewal(int userId, string description, List<CompanyPolicy> policies);

        /// <summary>
        /// Generar Archivo Planilla Intermediario
        /// </summary>
        /// <param name="renewalProcesses">Procesos</param>
        /// <returns>Ruta Archivo</returns>
        [OperationContract]
        string GenerateFileToPayrollByAgent(List<MassiveRenewalRow> massiveRenewalRows, string fileName);

        /// <summary>
        /// CreateCompanyPolicy
        /// </summary>
        /// <param name="file"></param>
        /// <param name="templatePropertyName"></param>
        /// <param name="userId"></param>
        /// <param name="selectedPrefixId"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyPolicy CreateCompanyPolicy(File file, string templatePropertyName, int userId, int selectedPrefixId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="massiveLoadId"></param>
        /// <returns></returns>
        [OperationContract]
        MassiveLoad IssuanceMassiveEmission(int massiveLoadId);

        /// <summary>
        /// Generar impresion de renovacion
        /// </summary>
        /// <param name="massiveLoadId"></param>
        /// <param name="rangeFrom"></param>
        /// <param name="rangeTo"></param>
        /// <returns></returns>
        [OperationContract]
        string PrintRenewalLoad(int massiveLoadId, int rangeFrom, int rangeTo, User user, bool checkIssuedDetail);
    

        [OperationContract]
        string GenerateFileErrorsMassiveRenewals(int massiveLoadId);
    }
}