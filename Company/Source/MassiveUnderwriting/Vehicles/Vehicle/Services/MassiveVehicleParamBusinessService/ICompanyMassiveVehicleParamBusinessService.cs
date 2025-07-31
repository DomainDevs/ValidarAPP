using System.ServiceModel;
using System.Collections.Generic;
using Sistran.Company.Application.MassiveVehicleParamBusinessService.Model;

namespace Sistran.Company.Application.MassiveVehicleParamBusinessService
{
    [ServiceContract]
    public interface ICompanyMassiveVehicleParamBusinessService
    {
        /// <summary>
        /// Crea un registro de valor de fasecolda
        /// </summary>
        /// <param name="businessCollection"></param>
        [OperationContract]
        void CreateFasecoldaValue(string businessCollection);
        /// <summary>
        /// Crea un registro de codigo fasecolda
        /// </summary>
        /// <param name="businessCollection"></param>
        [OperationContract]
        void CreateFasecoldaCode(string businessCollection);
        /// <summary>
        /// Retorna el string de la ubicación del archivo para ser descargado
        /// </summary>
        /// <param name="loadProcessId"></param>
        /// <returns></returns>
        [OperationContract]
        string GetErrorExcelProcessVehicleFasecolda(int loadProcessId);
        /// <summary>
        /// Metodo que retorna el estado del proceso del cargue fasecolda
        /// </summary>
        /// <param name="loadProcessId"></param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyProcessFasecoldaMassiveLoad> GetProcessMassiveVehiclefasecolda(int loadProcessId);
        /// <summary>
        /// Genera el proceso de la data (Copiar de temporales a Reales)
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyProcessFasecolda GenerateProcessMassiveVehicleFasecolda(int processId);
        /// <summary>
        /// Crea la carga masiva de los archivos seleccionados
        /// </summary>
        /// <param name="processFasecolda"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyProcessFasecolda GenerateLoadMassiveVehicleFasecolda(CompanyProcessFasecolda processFasecolda);
    }
}
