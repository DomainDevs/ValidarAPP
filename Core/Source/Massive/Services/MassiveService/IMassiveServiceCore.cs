using System;
using System.Collections.Generic;
using System.ServiceModel;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.PrintingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;

namespace Sistran.Core.Application.MassiveServices
{
    [ServiceContract]
    public interface IMassiveServiceCore
    {
        /// <summary>
        /// Obtener Tipos de Cargue Por Tipo de Proceso
        /// </summary>
        /// <param name="massiveProcessType">Tipo de Proceso</param>
        /// <returns>Tipos de Cargue</returns>
        [OperationContract]
        List<LoadType> GetLoadTypesByMassiveProcessType(MassiveProcessType massiveProcessType);

        /// <summary>
        /// Crear Cargue
        /// </summary>
        /// <param name="massiveLoad">Cargue</param>
        /// <returns>Cargue</returns>
        [OperationContract]
        MassiveLoad CreateMassiveLoad(MassiveLoad massiveLoad);

        /// <summary>
        /// Actualizar Cargue
        /// </summary>
        /// <param name="massiveLoad">Cargue</param>
        /// <returns>Cargue</returns>
        [OperationContract]
        MassiveLoad UpdateMassiveLoad(MassiveLoad massiveLoad);

        /// <summary>
        /// Obtener Cargue Por Identificador
        /// </summary>
        /// <param name="massiveLoadId">Id Cargue</param>
        /// <returns>Cargue</returns>
        [OperationContract]
        MassiveLoad GetMassiveLoadByMassiveLoadId(int massiveLoadId);

        /// <summary>
        /// Obtener Cargues Por Descripción
        /// </summary>
        /// <param name="description">Descripción</param>
        /// <returns>Cargues</returns>
        [OperationContract]
        List<MassiveLoad> GetMassiveLoadsByDescription(string description);

        /// <summary>
        /// Obtener Cargues Por Fecha, Tipo de Proceso, Tipo de Cargue, Descripción, Usuario
        /// </summary>
        /// <param name="rangeFrom">Fecha Desde</param>
        /// <param name="rangeTo">Fecha Hasta</param>
        /// <param name="massiveLoad">Cargue</param>
        /// <returns>Cargues</returns>
        [OperationContract]
        List<MassiveLoad> GetMassiveLoadsByRangeFromRangeToMassiveLoad(DateTime rangeFrom, DateTime rangeTo, MassiveLoad massiveLoad);

        /// <summary>
        /// Crea log de impresion
        /// </summary>
        /// <param name="printLog"></param>
        /// <returns></returns>
        [OperationContract]
        Printing CreatePrinting(Printing printing);

        /// <summary>
        /// Log de impresion
        /// </summary>
        /// <param name="printLog"></param>
        [OperationContract]
        void CreatePrintLog(PrintingLog printingLog);

        /// <summary>
        /// Elimina el proceso del cargue
        /// </summary>
        /// <param name="massiveLoad"></param>
        [OperationContract]
        string DeleteProcess(MassiveLoad massiveLoad);
        /// <summary>
        /// Actializacion de log de impresion
        /// </summary>
        /// <param name="printLog"></param>
        [OperationContract]
        Printing UpdatePrinting(Printing printing);

        /// <summary>
        /// Trae el log de impresion por id de cargue
        /// </summary>
        /// <param name="massiveLoadId"></param>
        /// <returns></returns>
        [OperationContract]
        Printing GetPrinting(int massiveLoadId, int userId);

        [OperationContract]
        string GenerateMassiveProcessReport(List<MassiveLoad> massiveLoads);

        [OperationContract]
        List<AuthorizationRequest> GetAuthorizationPolicies(Risk risk, MassiveLoad massiveLoad);

        [OperationContract]
        List<AuthorizationRequest> ValidateAuthorizationPolicies(List<PoliciesAut> infringementPolicies, MassiveLoad massiveLoad, int temporalId);

        [OperationContract]
        Printing GetPrintingByPrintingId(int printingId);
        [OperationContract]
        string UpdateMassiveLoadAuthorization(string massiveLoadId, List<string> temporalId);
    }
}