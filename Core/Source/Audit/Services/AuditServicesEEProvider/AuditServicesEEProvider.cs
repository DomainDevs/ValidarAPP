using Sistran.Core.Application.AuditServices.DAOs;
using Sistran.Core.Application.AuditServices.EEProvider.Assemblers;
using Sistran.Core.Application.AuditServices.EEProvider.Business;
using Sistran.Core.Application.AuditServices.EEProvider.DAOs;
using Sistran.Core.Application.AuditServices.EEProvider.Resources;
using Sistran.Core.Application.ModelServices.Enums;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using AUDModel = Sistran.Core.Application.AuditServices.Models;
using AUDSM = Sistran.Core.Application.ModelServices.Models.Audit;
using ENUMSM = Sistran.Core.Application.ModelServices.Enums;
using MODSM = Sistran.Core.Application.ModelServices.Models.Param;
using UTMO = Sistran.Core.Application.Utilities.Error;
namespace Sistran.Core.Application.AuditServices.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class AuditServicesEEProvider : IAuditService
    {
        /// <summary>
        /// Gets the audit by object.
        /// </summary>
        /// <param name="auditServiceModel">The audit service model.</param>
        /// <returns></returns>
        public async Task<List<AUDSM.AuditServiceModel>> GetAuditByObject(AUDSM.AuditServiceModel auditServiceModel)
        {
            AuditBusiness auditBusiness = new AuditBusiness();
            var result = auditBusiness.ValidateAudit(auditServiceModel);
            if (result.ErrorTypeService == ErrorTypeService.Ok)
            {
                AUDModel.Audit audit = ServicesModelsAssembler.CreateAudit(auditServiceModel);
                AuditDAO auditDAO = new AuditDAO();
                List<AUDModel.Audit> audits = await auditDAO.GetAuditByObject(audit);
                return ModelsServicesAssembler.CreateAudits(audits);
            }
            else
            {
                return new List<AUDSM.AuditServiceModel> { new AUDSM.AuditServiceModel { ErrorServiceModel = result } };
            }
        }

        public Task<List<AUDSM.EntityServiceModel>> GetEntityByDescription(string description)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<AUDSM.PackageServiceModel>> GetPackages()
        {
            AuditDAO auditDAO = new AuditDAO();
            List<AUDModel.Package> packages = await auditDAO.GetPackages();
            return ModelsServicesAssembler.CreatePackages(packages);
        }
        public async Task<List<AUDSM.EntityServiceModel>> GetEntitiesByDescription(string description, int idPackage)
        {
            if (string.IsNullOrEmpty(description))
            {
                throw new ArgumentException(Errors.ParameterEmpty);
            }
            AuditDAO auditDAO = new AuditDAO();
            List<AUDModel.Entity> Entities = await auditDAO.GetEntitiesByDescription(description, idPackage);
            return ModelsServicesAssembler.CreateEntities(Entities);
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        /// <param name="body">The body.</param>
        public void AuditData(object body)
        {
            LoadAudit.AuditData(body);
        }


        #region Archivo Excel()
        /// <summary>
        /// Generar archivo excel de Auditoria
        /// </summary>
        /// <param name="branchServiceModel">Listado de sucursal</param>
        /// <param name="fileName">Nombre de archivo</param>
        /// <returns>Archivo de excel</returns>
        public MODSM.ExcelFileServiceModel GenerateFileToAudit(AUDSM.AuditServiceModel auditServiceModel, string fileName)
        {
            AuditFileDAO fileDAO = new AuditFileDAO();
            MODSM.ExcelFileServiceModel excelFileServiceModel = new MODSM.ExcelFileServiceModel();
            AUDModel.Audit audit = ServicesModelsAssembler.CreateAudit(auditServiceModel);
            AuditDAO auditDAO = new AuditDAO();
            List<AUDModel.Audit> audits = auditDAO.GetAuditByObject(audit).Result;
            UTMO.Result<string, UTMO.ErrorModel> result = fileDAO.GenerateFileToAudits(audits, fileName);
            if (result is UTMO.ResultError<string, UTMO.ErrorModel>)
            {
                UTMO.ErrorModel errorModelResult = (result as UTMO.ResultError<string, UTMO.ErrorModel>).Message;
                excelFileServiceModel.ErrorTypeService = (ENUMSM.ErrorTypeService)errorModelResult.ErrorType;
                excelFileServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTMO.ResultValue<string, UTMO.ErrorModel>)
            {
                excelFileServiceModel.FileData = (result as UTMO.ResultValue<string, UTMO.ErrorModel>).Value;
            }

            return excelFileServiceModel;
        }
        #endregion

    }
}
