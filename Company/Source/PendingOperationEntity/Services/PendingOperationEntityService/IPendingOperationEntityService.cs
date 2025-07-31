using System.Collections.Generic;
using System.ServiceModel;
using System;
using Sistran.Core.Services.UtilitiesServices.Models;

namespace Sistran.Company.Application.PendingOperationEntityService
{
    [ServiceContract]
    [XmlSerializerFormat]
    public interface IPendingOperationEntityService
    {
        [OperationContract]
        /// <summary>
        /// Guardar JSON
        /// </summary>
        /// <param name="pendingOperation">Datos operacion</param>
        /// <returns>Modelo PendingOperation</returns>
        PendingOperation CreatePendingOperation(PendingOperation pendingOperation);

        [OperationContract]
        /// <summary>
        /// Eliminar JSON
        /// </summary>
        /// <param name="id">Identificador</param>
        /// <returns>Eliminado Si/No</returns>
        bool DeletePendingOperation(int id);

        [OperationContract]
        /// <summary>
        /// Obtener JSON hijo
        /// </summary>
        /// <param name="id">Identificador</param>
        /// <param name="parentId">Id padre</param>
        /// <returns>Modelo PendingOperation</returns>
        bool DeletePendingOperationsByParentId(int parentId);

        [OperationContract]
        /// <summary>
        /// Obtener JSON
        /// </summary>
        /// <param name="id">Identificador</param>
        /// <returns>Modelo PendingOperation</returns>
        PendingOperation GetPendingOperationById(int id);

        [OperationContract]
        /// <summary>
        /// Obtener JSON hijo
        /// </summary>
        /// <param name="id">Identificador</param>
        /// <param name="parentId">Id padre</param>
        /// <returns>Modelo PendingOperation</returns>
        PendingOperation GetPendingOperationByIdParentId(int id, int parentId);

        [OperationContract]
        /// <summary>
        /// Obtener lista de JSON
        /// </summary>
        /// <param name="parentId">Id padre</param>
        /// <returns>Lista de JSONs</returns>
        List<PendingOperation> GetPendingOperationsByParentId(int parentId);

        [OperationContract]
        /// <summary>
        /// Actualizar JSON
        /// </summary>
        /// <param name="pendingOperation">Datos operacion</param>
        /// <returns>Modelo PendingOperation</returns>
        PendingOperation UpdatePendingOperation(PendingOperation pendingOperation);

        [OperationContract]
        String GetPolicyByEndorsementDocumentNumber(int endorsementId, decimal documentNumber);

        [OperationContract]
        void RecordEndorsementOperation(int endorsementId, int pendingOperationId);

        [OperationContract]
        List<String> GetRiskByEndorsementDocumentNumber(int endorsementId);

        [OperationContract]
        string GetCompanyPolicyJsonByEndorsementId(int endorsementId);

    }
}