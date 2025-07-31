using System.Collections.Generic;
using System.ServiceModel;
using Sistran.Core.Application.MassiveRenewalServices.Enums;
using Sistran.Core.Application.MassiveRenewalServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.MassiveUnderwritingServices.Enums;

namespace Sistran.Core.Application.MassiveRenewalServices
{
    [ServiceContract]
    [XmlSerializerFormat]
    public interface IMassiveRenewalServiceCore
    {
        /// <summary>
        /// Obtener Pólizas Por Fecha De Vencimiento
        /// </summary>
        /// <param name="policy">Parametros De Busqueda</param>
        /// <returns>Lista De Pólizas</returns>
        [OperationContract]
        List<Policy> GetPoliciesByDueDate(Policy policy);

        /// <summary>
        /// Crea una nueva renovación masiva
        /// </summary>
        /// <param name="massiveRenewal"></param>
        /// <returns></returns>
        [OperationContract]
        MassiveRenewal CreateMassiveRenewal(MassiveRenewal massiveRenewal);

        /// <summary>
        /// Actualiza una renovación masiva
        /// </summary>
        /// <param name="massiveRenewal"></param>
        /// <returns></returns>
        [OperationContract]
        MassiveRenewal UpdateMassiveRenewal(MassiveRenewal massiveRenewal);

        /// <summary>
        /// Crea una nueva registro de una renovación masiva
        /// </summary>
        /// <param name="massiveRenewalRow"></param>
        /// <returns></returns>
        [OperationContract]
        MassiveRenewalRow CreateMassiveRenewalRow(MassiveRenewalRow massiveRenewalRow);

        /// <summary>
        /// Actualiza un registro de renovación masiva
        /// </summary>
        /// <param name="massiveRenewalRow"></param>
        /// <returns></returns>
        [OperationContract]
        MassiveRenewalRow UpdateMassiveRenewalRow(MassiveRenewalRow massiveRenewalRow);

        /// <summary>
        /// Excluye y elimina los temporales del cargue
        /// </summary>
        /// <param name="massiveRenewalId">Id del Cargue</param>
        /// <param name="temps">Lista de temporales</param>
        /// <param name="userName">Nombre del Usuario</param>
        [OperationContract]
        bool ExcludeMassiveRenewalRowsTemporals(int massiveRenewalId, List<int> temps, string userName);
        
        // <summary>
        // Genera el archivo de error del proceso de renovación masiva
        // </summary>
        // <param name = "massiveLoadProccessId" ></ param >
        // < returns ></ returns >
        [OperationContract]
        string GenerateFileErrorsMassiveRenewal(int massiveLoadId);

        /// <summary>
        /// Recupera el massive renew de acuerdo al id del massive load
        /// </summary>
        /// <param name="massiveLoadId"></param>
        /// <returns></returns>
        [OperationContract]
        MassiveRenewal GetMassiveRenewalByMassiveRenewalId(int massiveLoadId, bool listRowsQuery, bool? withError, bool? withEvent);

        [OperationContract]
        List<MassiveRenewalRow> GetMassiveLoadProcessByMassiveRenewalProcessId(int massiveRenewalId, MassiveLoadProcessStatus massiveLoadProcessStatus);

        [OperationContract]
        List<MassiveRenewalRow> GetMassiveRenewalRowsByMassiveLoadIdMassiveLoadProcessStatus(int massiveLoadProcessId, MassiveLoadProcessStatus? massiveLoadProcessStatus, bool? withError, bool? withEvent);

    }
}