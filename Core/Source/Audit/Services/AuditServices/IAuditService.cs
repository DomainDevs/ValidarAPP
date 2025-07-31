namespace Sistran.Core.Application.AuditServices
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System.Collections.Generic;
    using System.ServiceModel;
    using System.Threading.Tasks;
    using AUDSM = ModelServices.Models.Audit;

    /// <summary>
    /// Auditoria
    /// </summary>
    [ServiceContract]
    public interface IAuditService
    {
        /// <summary>
        /// Gets the audit by object.
        /// </summary>
        /// <param name="auditServiceModel">The audit service model.</param>
        /// <returns>
        /// List<AUDSM.AuditServiceModel>
        /// </returns>
        [OperationContract]
        Task<List<AUDSM.AuditServiceModel>> GetAuditByObject(AUDSM.AuditServiceModel auditServiceModel);

        /// <summary>
        /// Gets the packages.
        /// </summary>
        /// <returns>
        /// PackageServiceModel
        /// </returns>
        [OperationContract]
        Task<List<AUDSM.PackageServiceModel>> GetPackages();

        /// <summary>
        /// Gets the entity by description.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns>List<AUDSM.EntityServiceModel></returns>
        [OperationContract]
        Task<List<AUDSM.EntityServiceModel>> GetEntityByDescription(string description);

        /// <summary>
        /// Gets the entities by description.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        Task<List<AUDSM.EntityServiceModel>> GetEntitiesByDescription(string description, int idPackage);

        /// <summary>
        /// Genera archivo excel Log Auditoria.
        /// </summary>
        /// <param name="auditServiceModel">The audit service model.</param>
        /// <param name="fileName">Nombre del archivo.</param>
        /// <returns>
        /// Path archivo de excel
        /// </returns>
        [OperationContract]
        ExcelFileServiceModel GenerateFileToAudit(AUDSM.AuditServiceModel auditServiceModel, string fileName);

        // <summary>
        /// Loads the data.
        /// </summary>
        /// <param name="body">The body.</param>
        /// </returns>
        [OperationContract]
        void AuditData(object body);
    }
}
