// -----------------------------------------------------------------------
// <copyright file="ClausesController.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using AutoMapper;
    using Sistran.Core.Application.ModelServices.Models.Param;
    using Sistran.Core.Application.ModelServices.Models.Underwriting;
    using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
    using Sistran.Core.Framework.UIF.Web.Helpers.Enums;
    using Sistran.Core.Framework.UIF.Web.Models;
    using Sistran.Core.Framework.UIF.Web.Services;
    using ENUMSM = Sistran.Core.Application.ModelServices.Enums;
    using MOS = Core.Application.ModelServices.Models.Underwriting;
    using PARMS = Sistran.Core.Application.ModelServices.Models.Param;

    /// <summary>
    /// Controlador clausulas
    /// </summary>
    public class ClausesController : Controller
    {
        /// <summary>
        /// Vista general
        /// </summary>
        /// <returns>Retorna vista general</returns>
        public ActionResult Clause()
        {
            return View();
        }

        /// <summary>
        /// Vista detalle texto de clausulas
        /// </summary>
        /// <returns>Retorna detalle texto</returns>
        public ActionResult DetailClause()
        {
            return PartialView();
        }

        /// <summary>
        /// Obtiene clausulas
        /// </summary>
        /// <returns>Retorna listado de clausulas</returns>
        public ActionResult GetParametrizationClause()
        {
            MOS.ClausesServiceModel clauseServiceModel = DelegateService.UnderwritingParamServiceWeb.GetClausesServiceModel();
            if (clauseServiceModel.ErrorTypeService == ENUMSM.ErrorTypeService.Ok)
            {
                List<ClauseViewModel> parametrizationClauseMV = new List<ClauseViewModel>();
                var imapperClause = ModelAssembler.CreateMapClause();

                for (int i = 0; i < clauseServiceModel.ClauseServiceModels.Count; i++)
                {
                    parametrizationClauseMV.Add(imapperClause.Map<MOS.ClauseServiceModel, ClauseViewModel>(clauseServiceModel.ClauseServiceModels[i]));
                    parametrizationClauseMV[i].Level = clauseServiceModel.ClauseServiceModels[i].ConditionLevelServiceQueryModel.Id;
                    parametrizationClauseMV[i].ClauseText = clauseServiceModel.ClauseServiceModels[i].ClauseText;
                    parametrizationClauseMV[i].LevelDescription = clauseServiceModel.ClauseServiceModels[i].ConditionLevelServiceQueryModel.Description;
                    parametrizationClauseMV[i].Required = clauseServiceModel.ClauseServiceModels[i].ClauseLevelServiceModel.IsMandatory;

                    if (parametrizationClauseMV[i].Level == (int)Enum.Parse(typeof(ConditionType), "Coverage"))
                    {
                        if (clauseServiceModel.ClauseServiceModels[i].CoverageServiceQueryModel != null)
                        {
                            parametrizationClauseMV[i].Coverage = clauseServiceModel.ClauseServiceModels[i].CoverageServiceQueryModel.Id;
                            parametrizationClauseMV[i].CoverageName = clauseServiceModel.ClauseServiceModels[i].CoverageServiceQueryModel.Description;
                            parametrizationClauseMV[i].ObjectInsurance = clauseServiceModel.ClauseServiceModels[i].CoverageServiceQueryModel.InsuredObjectServiceQueryModel.Description;
                            parametrizationClauseMV[i].Protection = clauseServiceModel.ClauseServiceModels[i].CoverageServiceQueryModel.PerilServiceQueryModel.Description;
                        }
                    }

                    if (parametrizationClauseMV[i].Level == (int)Enum.Parse(typeof(ConditionType), "Prefix"))
                    {
                        if (clauseServiceModel.ClauseServiceModels[i].PrefixServiceQueryModel != null)
                        {
                            parametrizationClauseMV[i].CommercialBranch = clauseServiceModel.ClauseServiceModels[i].PrefixServiceQueryModel.PrefixCode;
                            parametrizationClauseMV[i].CommercialBranchName = clauseServiceModel.ClauseServiceModels[i].PrefixServiceQueryModel.PrefixDescription;
                        }
                    }

                    if (parametrizationClauseMV[i].Level == (int)Enum.Parse(typeof(ConditionType), "TypeRisk"))
                    {
                        if (clauseServiceModel.ClauseServiceModels[i].RiskTypeServiceQueryModel != null)
                        {
                            parametrizationClauseMV[i].CoveredRisk = clauseServiceModel.ClauseServiceModels[i].RiskTypeServiceQueryModel.Id;
                            parametrizationClauseMV[i].RiskTypeName = clauseServiceModel.ClauseServiceModels[i].RiskTypeServiceQueryModel.Description;
                        }
                    }
                    if (parametrizationClauseMV[i].Level == (int)Enum.Parse(typeof(ConditionType), "LineBusiness"))
                    {
                        if (clauseServiceModel.ClauseServiceModels[i].LineBusinessServiceQueryModel != null)
                        {
                            parametrizationClauseMV[i].LineBusiness = clauseServiceModel.ClauseServiceModels[i].LineBusinessServiceQueryModel.Id;
                            parametrizationClauseMV[i].LineBusinessName = clauseServiceModel.ClauseServiceModels[i].LineBusinessServiceQueryModel.Description;
                        }
                    }
                }

                return new UifJsonResult(true, parametrizationClauseMV);
            }
            else
            {
                if(clauseServiceModel.ErrorTypeService != ENUMSM.ErrorTypeService.NotFound) 
                  return new UifJsonResult(false, new { clauseServiceModel.ErrorTypeService, clauseServiceModel.ErrorDescription });
               else
                    return new UifJsonResult(true, new List<ClauseViewModel>());
            }
        }

        /// <summary>
        /// Obtiene nivel
        /// </summary>
        /// <returns>Retorna niveles</returns>
        [HttpGet]
        public ActionResult GetLevels()
        {
            try
            {
                ConditionLevelsServiceModel parametrizationClause = DelegateService.UnderwritingParamServiceWeb.GetClausesLevelsServiceModel();
                return new UifJsonResult(true, parametrizationClause.ConditionLevelsServiceModels.OrderBy(b => b.Description));
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetLineBusiness);
            }
        }

        /// <summary>
        /// Obtiene ramo comercial
        /// </summary>
        /// <returns>Retorna lista de ramos</returns>
        [HttpGet]
        public ActionResult GetCommercialBranch()
        {
            try
            {
                PrefixsServiceQueryModel parametrizationClause = DelegateService.UnderwritingParamServiceWeb.GetCommercialBranch();
                return new UifJsonResult(true, parametrizationClause.PrefixServiceQueryModel.OrderBy(b => b.PrefixDescription));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetLineBusiness);
            }
        }

        /// <summary>
        /// Obtiene tipo de riesgo cubierto
        /// </summary>
        /// <returns>Retorna combo tipo de riesgos</returns>
        [HttpGet]
        public ActionResult GetCoveredRiskType()
        {
            try
            {
                RiskTypesServiceModel parametrizationClause = DelegateService.UnderwritingParamServiceWeb.GetCoveredRiskType();
                return new UifJsonResult(true, parametrizationClause.RiskTypeServiceModels.OrderBy(b => b.Description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetLineBusiness);
            }
        }

        /// <summary>
        /// Obtiene ramo tecnico
        /// </summary>
        /// <returns>Retorna ramo tecnico</returns>
        [HttpGet]
        public ActionResult GetLineBusiness()
        {
            try
            {
                Application.ModelServices.Models.UnderwritingParam.LinesBusinessServiceQueryModel parametrizationLineBussiness = DelegateService.UnderwritingParamServiceWeb.GetLinesBusiness();
                return new UifJsonResult(true, parametrizationLineBussiness.LineBusinessServiceModel.OrderBy(b => b.Description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetLineBusiness);
            }
        }


        /// <summary>
        /// Metodo para consultar Texto precatalogados
        /// </summary>
        /// <param name="description">parametro descripcion</param>
        /// <returns>Retorta texto clausulas</returns>
        public ActionResult GetTextClause(string description)
        {
            try
            {
                TextsServiceModel parametrizationClause = DelegateService.UnderwritingParamServiceWeb.GetTextServiceModel(description);
                if (parametrizationClause.TextServiceModels.Count != 0)
                {
                    return new UifJsonResult(true, parametrizationClause);
                }

                return new UifJsonResult(false, App_GlobalResources.Language.MessageNotFoundCountries);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchCountry);
            }
        }

        /// <summary>
        /// Metodo permite consulta Coberturas
        /// </summary>
        /// <param name="description">Parametro descripcion</param>
        /// <returns>Retorna lista de coberturas por descripcion</returns>
        public ActionResult GetCoverage(string description)
        {
            try
            {
                CoveragesClauseServiceModel coverageParametrization = DelegateService.UnderwritingParamServiceWeb.GetCoverageByName(description);

                if (coverageParametrization.CoverageServiceModels.Count != 0)
                {
                    return new UifJsonResult(true, coverageParametrization);
                }

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorConsultingCoverages);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorConsultingCoverages);
            }
        }

        /// <summary>
        /// Metodo que obtiene lista de clausula
        /// </summary>
        /// <param name="description">Parametro descripcion</param>
        /// <returns>Retorna lista de clausulas</returns>
        public ActionResult GetListClause(string description)
        {
            try
            {
                ClausesServiceModel parametrizationClause = DelegateService.UnderwritingParamServiceWeb.GetClauseByNameAndTitle(description);

                if (parametrizationClause.ClauseServiceModels.Count != 0)
                {
                    return new UifJsonResult(true, parametrizationClause.ClauseServiceModels);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.Error);
                }
                
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.Error);
            }
        }

        /// <summary>
        /// Genera archivo excel de clausulas
        /// </summary>
        /// <returns>Retorna archivo excel</returns>
        public ActionResult GenerateFileToExport()
        {
            try
            {
                ClausesServiceModel clausesServiceModel = DelegateService.UnderwritingParamServiceWeb.GetClausesServiceModel();
                ExcelFileServiceModel excelFileServiceModel = DelegateService.UnderwritingParamServiceWeb.GenerateFileToClause(clausesServiceModel.ClauseServiceModels, App_GlobalResources.Language.LabelClauses);
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
        /// CRUD de clausulas
        /// </summary>
        /// <param name="parametrizationClauseVM">Listado de clausulas VM</param>
        /// <returns>Conteo de CRUD de la operacion</returns>
        public ActionResult CreateParametrizationClause(List<ClauseViewModel> parametrizationClauseVM)
        {
            List<ClauseServiceModel> clauseServiceModel = new List<ClauseServiceModel>();
            var imapperClauseView = ModelAssembler.CreateMapClauseView();

            for (int i = 0; i < parametrizationClauseVM.Count; i++)
            {
                clauseServiceModel.Add(imapperClauseView.Map<ClauseViewModel, ClauseServiceModel>(parametrizationClauseVM[i]));
            }

            List<ClauseServiceModel> clausesServiceModels = DelegateService.UnderwritingParamServiceWeb.ExecuteOperationsClauseServiceModel(clauseServiceModel);
            PARMS.ParametrizationResult parametrizationResult = new PARMS.ParametrizationResult();

            foreach (var item in clausesServiceModels)
            {
                if (item.ErrorServiceModel.ErrorTypeService != ENUMSM.ErrorTypeService.Ok)
                {
                    string statusType = (string)HttpContext.GetGlobalResourceObject("Language", item.StatusTypeService.ToString());
                    string errores = "";
                    foreach (var itemError in item.ErrorServiceModel.ErrorDescription)
                    {
                        errores += itemError;
                    }

                    parametrizationResult.Message += item.Name + " " + errores + "</br>";
                }
                else
                {
                    switch (item.StatusTypeService)
                    {
                        case ENUMSM.StatusTypeService.Create:
                            parametrizationResult.TotalAdded++;
                            break;
                        case ENUMSM.StatusTypeService.Update:
                            parametrizationResult.TotalModified++;
                            break;
                        case ENUMSM.StatusTypeService.Delete:
                            parametrizationResult.TotalDeleted++;
                            break;
                        default:
                            break;
                    }
                }
            }

            return new UifJsonResult(true, parametrizationResult);
        }

    }
}