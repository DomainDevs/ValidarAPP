using Sistran.Company.Application.UploadFileServices.Models;
using System.Collections.Generic;
using System.ServiceModel;


namespace Sistran.Company.Application.UploadFileServices
{
    [ServiceContract]
    public interface IUploadFileService
    {
        /// <summary>
        /// Inicia el proceso de Excel
        /// Obtiene los valores y Guarda en la Base de Datos.
        /// </summary>
        /// <param name="massiveLoadProcess">Información del proceso a iniciar.</param>
        /// <param name="userName">Nombre de usuario.</param>
        /// <param name="fieldSet">Id del ramo.</param>
        /// <returns>Modelo MassiveLoadError si se presenta un error.</returns>
        [OperationContract]
        List<MassiveLoadVehicleReception> InitExcelProcess(MassiveLoadProcess massiveLoadProcess, string userName, int fieldSetId, int tempId);
    }
}
