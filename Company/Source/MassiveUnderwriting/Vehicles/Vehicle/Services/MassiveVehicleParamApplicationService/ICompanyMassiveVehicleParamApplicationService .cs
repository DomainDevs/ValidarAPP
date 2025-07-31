using Sistran.Company.Application.Vehicles.MassiveVehicleParamApplicationService.DTOs;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.Vehicles.MassiveVehicleParamApplicationService
{
    [ServiceContract]
    public interface ICompanyMassiveVehicleParamApplicationService
    {
        /// <summary>
        /// Metodo que retorna el estado del proceso del cargue fasecolda
        /// </summary>
        /// <param name="loadProcessId"></param>
        /// <returns></returns>
        [OperationContract]
        List<ProcessFasecoldaMassiveLoadDTO> GetProcessMassiveVehiclefasecolda(int loadProcessId);
        /// <summary>
        /// Retorna el string de la ubicación del archivo para ser descargado
        /// </summary>
        /// <param name="loadProcessId"></param>
        /// <returns></returns>
        [OperationContract]
        string GetErrorExcelProcessVehicleFasecolda(int loadProcessId);
        /// <summary>
        /// Genera el proceso de la data (Copiar de temporales a Reales)
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        [OperationContract]
        ProcessFasecoldaDTO GenerateProccessMassiveVehicleFasecolda(int processId);
        /// <summary>
        /// Crea la carga masiva de los archivos seleccionados
        /// </summary>
        /// <param name="processFasecoldaDTO"></param>
        /// <returns></returns>
        [OperationContract]
        ProcessFasecoldaDTO GenerateLoadMassiveVehicleFasecolda(ProcessFasecoldaDTO processFasecoldaDTO);
    }
}
