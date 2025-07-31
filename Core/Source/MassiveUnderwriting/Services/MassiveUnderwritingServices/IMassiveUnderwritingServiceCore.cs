using System.Collections.Generic;
using System.ServiceModel;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.MassiveUnderwritingServices.Enums;
using Sistran.Core.Application.MassiveUnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Enums;

namespace Sistran.Core.Application.MassiveUnderwritingServices
{
    [ServiceContract]
    [XmlSerializerFormat]
    public interface IMassiveUnderwritingServiceCore
    {
        [OperationContract]
        MassiveEmission CreateMassiveEmission(MassiveEmission massiveEmission);

        [OperationContract]
        MassiveEmission UpdateMassiveEmission(MassiveEmission massiveEmission);

        [OperationContract]
        MassiveEmissionRow CreateMassiveEmissionRow(MassiveEmissionRow massiveEmissionRow);

        [OperationContract]
        MassiveEmissionRow UpdateMassiveEmissionRows(MassiveEmissionRow massiveEmissionRow);

        [OperationContract]
        List<MassiveCancellationRow> GetMassiveCancellationRowsWithErrorsWithEventsByMassiveLoadId(int massiveLoadId, bool? withErrors, bool? withEvents);


        /// <summary>
        /// Obtener Cargue por Id
        /// </summary>
        /// <param name="massiveEmissionId"></param>
        /// <returns>Cargue Masivo</returns>
        [OperationContract]
        MassiveEmission GetMassiveEmissionByMassiveLoadId(int massiveLoadId);

        /// <summary>
        /// Obtener Proceso De Cargue Masivo
        /// </summary>
        /// <param name="massiveLoadProcessId"></param>
        /// <param name="massiveLoadProcessStatus"></param>
        /// <param name="withError"></param>
        /// <param name="withEvent"></param>
        /// <returns>Proceso De Cargue Masivo</returns>
        [OperationContract]
        List<MassiveEmissionRow> GetMassiveEmissionRowsByMassiveLoadIdMassiveLoadProcessStatus(int massiveLoadProcessId, MassiveLoadProcessStatus? massiveLoadProcessStatus, bool? withError, bool? withEvent);
        
        
        ///// <summary>
        ///// Retorna los procesos finalizados
        ///// </summary>
        ///// <param name="massiveLoadId">Proceso De Cargue Masivo</param>
        ///// <returns>Proceso De Cargue Masivo</returns>
        [OperationContract]
        List<MassiveEmissionRow> GetFinalizedMassiveEmissionRowsByMassiveLoadId(int massiveLoadId);

        /// <summary>
        /// Actualiza los temporales a excluir
        /// </summary>
        /// <param name="massiveLoadId">Id del cargue</param>
        /// <param name="temps">Lista de temporales</param>
        /// <param name="userName">Usuario actual</param>
        /// <param name="deleteTemporal">si debe borrar el temporal excluido</param>
        [OperationContract]
        bool ExcludeMassiveEmissionRowsTemporals(int massiveLoadId, List<int> temps, string userName, bool deleteTemporal);


        /// <summary>
        /// Excluye del temporario de cancelacion
        /// </summary>
        /// <param name="massiveLoadId"></param>
        /// <param name="temps"></param>
        /// <param name="userName"></param>
        /// <param name="deleteTemporal"></param>
        /// <returns></returns>
        [OperationContract]
        bool ExcludeMassiveCancellationRowsByTemporals(int massiveLoadId, List<int> temps, string userName, bool deleteTemporal);


        #region CANCELACION
        /// <summary>
        /// Obtener Proceso De Cargue Masivo
        /// </summary>
        /// <param name="massiveLoadProcessId"></param>
        /// <param name="massiveLoadProcessStatus"></param>
        /// <param name="withError"></param>
        /// <param name="withEvent"></param>
        /// <returns>Proceso De Cargue Masivo</returns>
        [OperationContract]
        List<MassiveCancellationRow> GetMassiveCancellationRowsByMassiveLoadIdSubCoveredRiskType(int massiveLoadProcessId,SubCoveredRiskType? subCoveredRiskType, MassiveLoadProcessStatus? massiveLoadProcessStatus, bool? withError, bool? withEvent);

        [OperationContract]
        MassiveCancellationRow CreateMassiveCancellationRow(MassiveCancellationRow massiveCancellationRow);

        [OperationContract]
        MassiveCancellationRow UpdateMassiveCancellationRows(MassiveCancellationRow massiveCancellationRow);

        #endregion

        [OperationContract]
        MassiveEmissionRow GetMassiveEmissionRowByMassiveLoadIdRowNumber(int massiveLoadProcessId, int rowNumber);

    
        
    }
}