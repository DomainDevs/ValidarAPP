// -----------------------------------------------------------------------
// <copyright file="CoverageGroupController.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Gina Gómez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;    
    using Sistran.Core.Application.ModelServices.Models.Param;
    using Sistran.Core.Application.ModelServices.Models.Underwriting;
    using Sistran.Core.Application.ModelServices.Models.UnderwritingParam;
    using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
    using Sistran.Core.Framework.UIF.Web.Models;
    using Sistran.Core.Framework.UIF.Web.Services;
    using ENUMUTIL = Sistran.Core.Services.UtilitiesServices.Enums;
    using ENTEN = Sistran.Core.Application.EntityServices.Enums;
    using ENUMSM = Sistran.Core.Application.ModelServices.Enums;
    using Sistran.Core.Application.EntityServices.Models;

    /// <summary>
    /// Clase grupo de cobertura
    /// </summary>
    public class CoverageGroupController : Controller
    {
        /// <summary>
        /// Parametros de entidad 
        /// </summary>
        private PostEntity postEntity = new PostEntity { EntityType = "Sistran.Core.Application.Product.Entities.CoverGroupRiskType", KeyType = KeyType.IdentityByKey };

        /// <summary>
        /// Listado CoverageGroupViewModel
        /// </summary>
        private List<CoverageGroupViewModel> groupModel = new List<CoverageGroupViewModel>();

        /// <summary>
        /// Listado CoveredRiskTypeViewModel
        /// </summary>
        private List<CoveredRiskTypeViewModel> coveredRiskTypeModel = new List<CoveredRiskTypeViewModel>();

        /// <summary>
        /// Modelo CoveredRiskTypesServiceModel
        /// </summary>
        private CoveredRiskTypesServiceModel coveredRiskTypesServiceModel = new CoveredRiskTypesServiceModel();

        /// <summary>
        /// Vista principal
        /// </summary>
        /// <returns>Retorna la vista principal</returns>
        public ActionResult CoverageGroup()
        {
            return this.View();
        }

        /// <summary>
        /// Obtiene toda la información de la base de datos
        /// </summary>
        /// <returns>Listado de amparos</returns>
        public ActionResult GetCoverageGroupAll()
        {
            try
            {
                this.GetCoverageGroups();
                return new UifJsonResult(true, this.groupModel);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetGroupCoverages);
            }
        }

        /// <summary>
        /// Obtiene los grupos de cobertura
        /// </summary>
        public void GetCoverageGroups()
        {
            if (this.groupModel.Count == 0)
            {
                this.postEntity.Fields = null;
                List<PostEntity> entity = DelegateService.EntityServices.GetEntities(this.postEntity);
                this.GetCoveredRiskTypes();
                this.groupModel = ModelAssembler.CreateCoverageGroup(entity, this.coveredRiskTypeModel).OrderBy(x => x.Description).ToList();
            }
        }

        /// <summary>
        /// Listado de tipos de cobertura
        /// </summary>
        /// <returns>Modelo CoveredRiskTypesServiceModel</returns>
        [HttpGet]
        public ActionResult GetCoveredRiskType()
        {
            this.coveredRiskTypesServiceModel = this.GetCoveredRiskTypes();
            if (this.coveredRiskTypesServiceModel.ErrorTypeService == ENUMSM.ErrorTypeService.Ok)
            {
                return new UifJsonResult(true, this.coveredRiskTypeModel.OrderBy(x => x.ShortDescription).ToList());
            }
            else
            {
                return new UifJsonResult(false, new { this.coveredRiskTypesServiceModel.ErrorTypeService, this.coveredRiskTypesServiceModel.ErrorDescription });
            }
        }

        /// <summary>
        /// Genera Archivo excel
        /// </summary>
        /// <returns>Archivo de excel</returns>      
        public ActionResult GenerateFileToExport()
        {
            try
            {
                this.GetCoverageGroups();
                List<CoverageGroupRiskTypeServiceModel> coverageGroupRiskTypeServiceModel = ModelAssembler.CreateCoverageGroups(this.groupModel);
                ExcelFileServiceModel excelFileServiceModel = DelegateService.UnderwritingParamServiceWeb.GenerateFileToCoverageGroup(coverageGroupRiskTypeServiceModel, App_GlobalResources.Language.LabelGroupCoverages);
                if (excelFileServiceModel.ErrorTypeService == ENUMSM.ErrorTypeService.Ok)
                {
                    var urlFile = excelFileServiceModel.FileData;
                    return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorFileNotFound);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
            }
        }

        /// <summary>
        /// Metodo ejecutar las operaciones del CRUD
        /// </summary>
        /// <param name="coverageGroups">Listado CoverageGroupViewModel</param>
        /// <returns>Ejecuta operaciones de CRUD en base de datos</returns>
        [HttpPost]
        public ActionResult ExecuteOperation(List<CoverageGroupViewModel> coverageGroups)
        {
            string messageErrors = string.Empty;
            int add = 0;
            int edit = 0;
            int delete = 0;

            foreach (CoverageGroupViewModel item in coverageGroups)
            {
                List<Field> fields = new List<Field>();
                try
                {
                    if ((ENUMUTIL.ParametrizationStatus)item.Status == ENUMUTIL.ParametrizationStatus.Delete)
                    {
                        fields.Add(new Field { Name = "CoveredRiskTypeCode", Value = item.CoveredRiskTypeCode.ToString() });
                        fields.Add(new Field { Name = "IdCoverGroupRisk", Value = item.IdCoverGroupRisk.ToString() });
                        fields.Add(new Field { Name = "CoverageGroupCode", Value = item.CoverageGroupCode.ToString() });
                        this.postEntity.Fields = fields;
                        DelegateService.EntityServices.Delete(this.postEntity);
                        delete += 1;
                    }
                    else
                    {
                        this.AssignPostEntity(item);
                        if (this.postEntity.Status == ENTEN.StatusTypeService.Create)
                        {
                            this.postEntity = DelegateService.EntityServices.Create(this.postEntity);
                            Field field = this.postEntity.Fields.First(x => x.Name == "IdCoverGroupRisk");
                            if (field.Value != "0")
                            {
                                add += 1;
                            }
                        }
                        else if (this.postEntity.Status == ENTEN.StatusTypeService.Update)
                        {
                            DelegateService.EntityServices.Update(this.postEntity);
                            edit += 1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    messageErrors = messageErrors + " " + string.Format(ex.Message + " " + item.Description + "<br/>");
                }
            }

            this.groupModel = new List<CoverageGroupViewModel>();
            this.GetCoverageGroups();

            string message = string.Empty;
            if (add > 0)
            {
                message += string.Format(App_GlobalResources.Language.ReturnSaveAdded, add);
            }

            if (edit > 0)
            {
                if (messageErrors != string.Empty)
                {
                    messageErrors += " ";
                }

                message += string.Format(App_GlobalResources.Language.ReturnSaveEdited, edit);
            }

            if (delete > 0)
            {
                if (messageErrors != string.Empty)
                {
                    messageErrors += " ";
                }

                message += string.Format(App_GlobalResources.Language.ReturnSaveDeleted, delete);
            }

            if (messageErrors != string.Empty)
            {
                if (messageErrors != string.Empty)
                {
                    messageErrors += " ";
                }

                message += messageErrors;
            }

            var result = this.groupModel.OrderBy(x => x.Description).ToList();
            return new UifJsonResult(true, new { message = message, data = result });
        }

        /// <summary>
        /// Asigna los campos a la entidad
        /// </summary>
        /// <param name="item">modelo DetailViewModel</param>
        private void AssignPostEntity(CoverageGroupViewModel item)
        {
            List<Field> fields = new List<Field>();
            if (item.CoveredRiskTypeCode != 0)
            {
                fields.Add(new Field { Name = "CoveredRiskTypeCode", Value = item.CoveredRiskTypeCode.ToString(), IsKeyForOtherColumn = true });
            }

            if (item.IdCoverGroupRisk != 0)
            {
                fields.Add(new Field { Name = "IdCoverGroupRisk", Value = item.IdCoverGroupRisk.ToString() });
            }

            fields.Add(new Field { Name = "CoverageGroupCode", Value = item.CoverageGroupCode.ToString(), IsConsecutiveByKeyOtherColumn = true });
            fields.Add(new Field { Name = "Description", Value = item.Description.ToString() });
            fields.Add(new Field { Name = "SmallDescription", Value = item.SmallDescription.ToString() });
            fields.Add(new Field { Name = "Enabled", Value = item.Enabled.ToString(), Type = new FieldType { Name = "System.Boolean" } });

            this.postEntity.Fields = fields;
            this.postEntity.Status = item.Status;
        }

        /// <summary>
        /// Obtiene todos los grupos de conbertura
        /// </summary>
        /// <returns>Modelo CoveredRiskTypesServiceModel</returns>
        private CoveredRiskTypesServiceModel GetCoveredRiskTypes()
        {
            if (this.coveredRiskTypeModel.Count == 0)
            {
                this.coveredRiskTypesServiceModel = DelegateService.UnderwritingParamServiceWeb.GetCoveredRiskTypes();
                this.coveredRiskTypeModel = ModelAssembler.GetCoveredRiskTypes(this.coveredRiskTypesServiceModel);
            }

            return this.coveredRiskTypesServiceModel;
        }
    }
}