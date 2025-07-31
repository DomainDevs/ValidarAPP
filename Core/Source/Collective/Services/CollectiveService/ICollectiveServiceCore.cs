using System.Collections.Generic;
using Sistran.Core.Application.CollectiveServices.Models;
using System.ServiceModel;
using Sistran.Core.Application.CollectiveServices.Enums;
using Sistran.Core.Application.CommonService.Enums;

namespace Sistran.Core.Application.CollectiveServices
{
    using MassiveServices.Models;
    using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
    using Sistran.Core.Services.UtilitiesServices.Enums;
    using UnderwritingServices.Models;

    [ServiceContract]
    [XmlSerializerFormat]
    public interface ICollectiveServiceCore
    {

        [OperationContract]
        CollectiveEmission CreateCollectiveEmission(CollectiveEmission collectiveEmission);

        [OperationContract]
        CollectiveEmissionRow CreateCollectiveEmissionRow(CollectiveEmissionRow collectiveEmissionRow);

        [OperationContract]
        CollectiveEmission UpdatCollectiveEmission(CollectiveEmission collectiveEmission);

        [OperationContract]
        CollectiveEmissionRow UpdateCollectiveEmissionRow(CollectiveEmissionRow collectiveEmissionRow);

        /// <summary>
        /// Actualiza los temporales a excluir
        /// </summary>
        /// <param name="massiveLoadId">Id del cargue</param>
        /// <param name="temps">Lista de temporales</param>
        /// <param name="userName">Usuario Actual</param>
        /// <param name="deleteTemporal">Si debe eliminar el temporal</param>
        [OperationContract]
        CollectiveEmission ExcludeCollectiveEmissionRowsTemporals(int massiveLoadId, List<int> temps, string userName, bool deleteTemporal);

        [OperationContract]
        CollectiveEmission GetCollectiveEmissionByMassiveLoadId(int massiveLoadId, bool withRows, bool? withErrors = null, bool? withEvents = null);

        [OperationContract]
        List<CollectiveEmissionRow> GetCollectiveEmissionRowByMassiveLoadId(int collectiveLoadId, CollectiveLoadProcessStatus collectiveLoadProcessStatus);

        // <summary>
        // Genera el archivo de error del proceso de emisión colectiva
        // </summary>
        // <param name = "massiveLoadId" ></ param >
        // < returns ></ returns >
        [OperationContract]
        string GenerateFileErrorsCollective(int massiveLoadId, FileProcessType fileProcessType);

        //// <summary>
        //// Genera el archivo de error del proceso de renovación colectiva
        //// </summary>
        //// <param name = "massiveLoadId" ></ param >
        //// < returns ></ returns >
        //[OperationContract]
        //string GenerateFileErrorsCollectiveRenewal(int massiveLoadId);

        ///// <summary>
        ///// Genera el archivo de error del proceso de inclusión
        ///// </summary>
        ///// <param name="massiveLoadProccessId"></param>
        ///// <returns></returns>
        //[OperationContract]
        //string GenerateFileErrorsCollectiveInclusion(int massiveLoadId);

        ///// <summary>
        ///// Genera el archivo de error del proceso de exclusión
        ///// </summary>
        ///// <param name="massiveLoadProccessId"></param>
        ///// <returns></returns>
        //[OperationContract]
        //string GenerateFileErrorsCollectiveExclusion(int massiveLoadId);

        [OperationContract]
        List<AuthorizationRequest> ValidateAuthorizationPoliciesPolicy(List<PoliciesAut> policiesAuthorization, MassiveLoad massiveLoad, int policyId);

        [OperationContract]
        List<AuthorizationRequest> ValidateAuthorizationPoliciesRisk(List<PoliciesAut> policiesAuthorization, MassiveLoad massiveLoad, int policyId, int riskId);

        [OperationContract]
        List<CollectiveEmissionRow> GetCollectiveEmissionRowByMassiveLoadIdCollectiveLoadProcessStatus(int collectiveLoadId, CollectiveLoadProcessStatus? collectiveLoadProcessStatus, bool? withError, bool? withEvent);

        [OperationContract]
        List<CollectiveEmission> GetCollectiveEmissionsByTempId(int tempId, bool withRows, bool withEvents);

        [OperationContract]
        CollectiveEmissionRow GetCollectiveEmissionRowById(int id);

        [OperationContract]
        void UpdateCollectiveLoadAuthorization(int loadId, int temporalId, List<int> risks);
    }
}
