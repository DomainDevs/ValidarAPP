// -----------------------------------------------------------------------
// <copyright file="SubLineBusinessController.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    using Sistran.Core.Application.EntityServices.Enums;
    using Sistran.Core.Application.ModelServices.Models.Param;
    using Sistran.Core.Application.ModelServices.Models.UnderwritingParam;
    using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
    using Sistran.Core.Framework.UIF.Web.Models;
    using Sistran.Core.Framework.UIF.Web.Services;
    using Sistran.Core.Services.UtilitiesServices.Models;
    using ModelModelServiceCore= Sistran.Core.Application.ModelServices.Models.Param;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using ENUMUTIL = Sistran.Core.Services.UtilitiesServices.Enums;
    using ENMO = Sistran.Core.Application.EntityServices.Models;
    using ENUMSM = Sistran.Core.Application.ModelServices.Enums;

    /// <summary>
    /// Controlador para SubRamo Tecnico
    /// </summary>
    public class SubLineBusinessController : Controller
    {
        /// <summary>
        /// Parametros de la entidad
        /// </summary>
        private ENMO.PostEntity postEntity = new ENMO.PostEntity { EntityType = "Sistran.Core.Application.Common.Entities.SubLineBusiness", KeyType = ENMO.KeyType.Autonumber };

        /// <summary>
        /// Modelo de servicio
        /// </summary>
        private List<SubLineBranchServiceModel> list = new List<SubLineBranchServiceModel>();

        /// <summary>
        /// Modelo vista
        /// </summary>
        private List<SubLineBusinessViewModel> subBranch = new List<SubLineBusinessViewModel>();

        /// <summary>
        /// Obtiene vista principal
        /// </summary>
        /// <returns>Retorna vista SubRamo Tecnico</returns>
        public ActionResult SubLineBusiness()
        {
            return this.View();
        }

        /// <summary>
        /// Obtiene vista busqueda avanzada
        /// </summary>
        /// <returns>Retorna busqueda avanzada</returns>
        public PartialViewResult SubLineBusinessAdvancedSearch()
        {
            return this.PartialView();
        }

        /// <summary>
        /// Obtiene ramo técnico
        /// </summary>
        /// <returns>Retorna Ramo Tecnico</returns>
        public ActionResult GetsLinesBusiness()
        {
            LinesBusinessServiceQueryModel linesBusinessServiceQueryModel = DelegateService.UnderwritingParamServiceWeb.GetLinesBusiness();
            if (linesBusinessServiceQueryModel.ErrorTypeService == ENUMSM.ErrorTypeService.Ok)
            {
                return new UifJsonResult(true, linesBusinessServiceQueryModel.LineBusinessServiceModel.OrderBy(x => x.Description).ToList());
            }
            else
            {
                return new UifJsonResult(false, new { linesBusinessServiceQueryModel.ErrorTypeService, linesBusinessServiceQueryModel.ErrorDescription });
            }
        }

        /// <summary>
        /// Obtiene subramo técnico
        /// </summary>
        /// <returns>retorna listado de subRamo Tecnico</returns>
        public ActionResult GetSubLinesBusinessById()
        {
            SubLineBranchsServiceModel subLinesBusinessServiceQueryModel = DelegateService.UnderwritingParamServiceWeb.GetSubLinesBusiness();
            if (subLinesBusinessServiceQueryModel.ErrorTypeService == ENUMSM.ErrorTypeService.Ok)
            {
                this.subBranch = ModelAssembler.CreateSubLineBusinessParametrization(subLinesBusinessServiceQueryModel.SubLineBranchService);
                return new UifJsonResult(true, this.subBranch.OrderBy(x => x.LineBusinessDescription).ThenBy(x => x.Description).ToList());
            }
            else
            {
                return new UifJsonResult(false, new { subLinesBusinessServiceQueryModel.ErrorTypeService, subLinesBusinessServiceQueryModel.ErrorDescription });
            }
        }

        /// <summary>
        /// Obtiene subRam Tecnico por descripcion
        /// </summary>
        /// <param name="description">Recibe description</param>
        /// <returns>Retorna listado de subRamo Tecnico por descripcion</returns>
        public ActionResult GetListSubLineBusinessByName(string description)
        {
            try
            {
                SubLineBranchsServiceModel parametrizationSubLineBusiness = DelegateService.UnderwritingParamServiceWeb.GetSubLineBusinessByName(description);

                if (parametrizationSubLineBusiness.SubLineBranchService.Count != 0)
                {
                    return new UifJsonResult(true, parametrizationSubLineBusiness);
                }

                return new UifJsonResult(false, App_GlobalResources.Language.MessageNotFoundCountries);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchCountry);
            }
        }

        /// <summary>
        /// Búsquedad avanzada, filtra ramo técnico y subramo técnico 
        /// </summary>
        /// <param name="subLineBusinessView">model vista</param>
        /// <returns>Retorna listado</returns>
        public ActionResult GetSubLineBusinessAdvancedSearch(SubLineBusinessViewModel subLineBusinessView)
        {
            try
            {
                List<SubLineBranchServiceModel> subLineBusinessSearch = new List<SubLineBranchServiceModel>();
                this.GetListSubLinesBusiness();
                foreach (var item in this.list)
                {
                    if (subLineBusinessView != null)
                    {
                        if (subLineBusinessView.LineBusinessId > 0)
                        {
                            if (item.LineBusinessQuery.Id == subLineBusinessView.LineBusinessId)
                            {
                                subLineBusinessSearch.Add(item);
                            }
                        }

                        if (subLineBusinessView.Description != null && subLineBusinessView.Description != " ")
                        {
                            if (item.Description.Contains(subLineBusinessView.Description))
                            {
                                subLineBusinessSearch.Add(item);
                            }
                        }

                        if (subLineBusinessView.Description == "" && subLineBusinessView.LineBusinessDescription != " -Seleccione un item - ")
                        {
                            return new UifJsonResult(false, App_GlobalResources.Language.EnterSomeData);
                        }
                    }
                }

                return new UifJsonResult(true, subLineBusinessSearch.OrderBy(x => x.Id).ToList());
            }

            catch (System.Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetSubLineBusiness);
            }
        }

        /// <summary>
        /// Genera archivo excel subramo técnico
        /// </summary>
        /// <returns>Retorna archivo excel</returns>
        public ActionResult GenerateFileToExport()
        {
            try
            {
                SubLineBranchsServiceModel subLineBusinessServiceModel = DelegateService.UnderwritingParamServiceWeb.GetSubLinesBusiness();
                ExcelFileServiceModel excelFileServiceModel = DelegateService.UnderwritingParamServiceWeb.GenerateFileToSubLineBusiness(subLineBusinessServiceModel.SubLineBranchService, App_GlobalResources.Language.LabelSubLineBusiness);
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
        /// Metodo para CRUD Sub Ramo Tecnico
        /// </summary>
        /// <param name="subLineBusiness">Recibe lista de SubRamo Tecnico</param>
        /// <returns>Retorna lista</returns>
        [HttpPost]
        public ActionResult Save(List<SubLineBusinessViewModel> subLineBusiness)
        {
            string messageErrors = string.Empty;
            ModelModelServiceCore.ParametrizationResult parametrizationResult = new ModelModelServiceCore.ParametrizationResult();

            List<ENMO.Field> fields = new List<ENMO.Field>();
            foreach (SubLineBusinessViewModel item in subLineBusiness)
            {
                try
                {
                    if ((ENUMUTIL.ParametrizationStatus)item.StatusTypeService == ENUMUTIL.ParametrizationStatus.Delete)
                    {
                        fields.Add(new ENMO.Field { Name = "SubLineBusinessCode", Value = item.Id.ToString(), IsConsecutiveByKeyOtherColumn = true });
                        fields.Add(new ENMO.Field { Name = "LineBusinessCode", Value = Convert.ToString((int)item.LineBusinessId), IsKeyForOtherColumn = true, Type = new ENMO.FieldType { Name = "System.Int32" } });
                        this.postEntity.Fields = fields;
                        DelegateService.EntityServices.Delete(this.postEntity);
                        parametrizationResult.TotalDeleted++;
                    }
                    else
                    {
                        this.AssignPostEntity(item);
                        if (this.postEntity.Status == StatusTypeService.Create)
                        {
                            this.postEntity = DelegateService.EntityServices.Create(this.postEntity);
                            ENMO.Field field = this.postEntity.Fields.First(x => x.Name == "SubLineBusinessCode");
                            if (field.Value != "0")
                            {
                                parametrizationResult.TotalAdded++;
                            }
                        }
                        else if (this.postEntity.Status == StatusTypeService.Update)
                        {
                            DelegateService.EntityServices.Update(this.postEntity);
                            parametrizationResult.TotalModified++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    messageErrors = messageErrors + " " + string.Format(ex.Message + " " + item.Description + "<br/>");
                }


                string errores = string.Empty;
                foreach (var itemError in messageErrors)
                {
                    errores += itemError;
                }

                parametrizationResult.Message += errores;
            }
                this.subBranch = new List<SubLineBusinessViewModel>();
                this.GetListSubLinesBusinessById();
 

            

        
            var result = this.subBranch.OrderBy(x => x.LineBusinessDescription).ThenBy(x => x.Description).ToList();
            return new UifJsonResult(true, new { message = parametrizationResult, data = result });
            //return new UifJsonResult(true, parametrizationResult);
        }

        /// <summary>
        /// Obtiene lista de subramo técnico
        /// </summary>
        private void GetListSubLinesBusiness()
        {
            if (this.list.Count == 0)
            {
                this.list = DelegateService.UnderwritingParamServiceWeb.GetSubLinesBusiness().SubLineBranchService.OrderBy(x => x.Description).ToList();
            }
        }

        /// <summary>
        /// Asigna los campos a la entidad
        /// </summary>
        /// <param name="item">modelo TechnicalSubBranchViewModel</param>
        private void AssignPostEntity(SubLineBusinessViewModel item)
        {
            List<ENMO.Field> fields = new List<ENMO.Field>();
            fields.Add(new ENMO.Field { Name = "SubLineBusinessCode", Value = Convert.ToString((int)item.Id), IsConsecutiveByKeyOtherColumn = true, Type = new ENMO.FieldType { Name = "System.Int32" } });
            fields.Add(new ENMO.Field { Name = "LineBusinessCode", Value = Convert.ToString((int)item.LineBusinessId), IsKeyForOtherColumn = true, Type = new ENMO.FieldType { Name = "System.Int32" } });
            fields.Add(new ENMO.Field { Name = "Description", Value = item.Description.ToString() });
            fields.Add(new ENMO.Field { Name = "SmallDescription", Value = item.SmallDescription.ToString() });
            fields.Add(new ENMO.Field { Name = "Required", Value = "false", Type = new ENMO.FieldType { Name = "System.Boolean" } });
            this.postEntity.Fields = fields;
            this.postEntity.EntityType = "Sistran.Core.Application.Common.Entities.SubLineBusiness";
            this.postEntity.KeyType = ENMO.KeyType.IdentityByKey;
            this.postEntity.Status = item.StatusTypeService;
        }

        /// <summary>
        /// Lista de SubRamos Tecnicos
        /// </summary>
        /// <returns>Retorna listado de subRamo Tecnico</returns>
        private List<SubLineBusinessViewModel> GetListSubLinesBusinessById()
        {
            if (this.subBranch.Count == 0)
            {
                SubLineBranchsServiceModel subLineBusinessServiceModel = DelegateService.UnderwritingParamServiceWeb.GetSubLinesBusiness();
                if (subLineBusinessServiceModel.ErrorTypeService == ENUMSM.ErrorTypeService.Ok)
                {
                    this.subBranch = ModelAssembler.CreateSubLineBusinessParametrization(subLineBusinessServiceModel.SubLineBranchService);
                    return this.subBranch.OrderBy(x => x.LineBusinessDescription).ThenBy(x => x.Description).ToList();
                }
                else
                {
                    return null;
                }
            }

            return this.subBranch;
        }
    }
}