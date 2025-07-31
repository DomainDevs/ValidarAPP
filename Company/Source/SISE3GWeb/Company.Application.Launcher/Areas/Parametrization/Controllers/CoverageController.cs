
using System;
using System.Web.Mvc;
using AutoMapper;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.ModelServices.Models.Param;
using Sistran.Core.Application.ModelServices.Models.UnderwritingParam;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
using COMMO = Sistran.Company.Application.CommonServices.Models;
using ENUMO = Sistran.Core.Application.ModelServices.Enums;
using MOS = Sistran.Core.Application.UnderwritingServices.Models;
using PARUPSM = Sistran.Core.Application.ModelServices.Models.UnderwritingParam;
using PARUSM = Sistran.Core.Application.ModelServices.Models.Underwriting;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Company.Application.Quotation.Entities;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    /// <summary>
    /// Control de coberturas
    /// </summary>
    public class CoverageController : Controller
    {
        /// <summary>
        /// Vista principal de coberturas
        /// </summary>
        /// <returns>vista de cobertura</returns>
        public ActionResult Coverage()
        {
            return this.View();
        }

        /// <summary>
        /// Vista de busqueda avanzada
        /// </summary>
        /// <returns>vista parcial de busqueda avanzada</returns>
        public ActionResult AdvancedSearch()
        {
            return this.PartialView();
        }

        /// <summary>
        /// Vista de Homologacion 2G
        /// </summary>
        /// <returns>Vista parcial de homologacion 2gs</returns>
        public ActionResult Coverage2G()
        {
            return this.PartialView();
        }

        /// <summary>
        /// Vista principal de Impresiones de coberturas
        /// </summary>
        /// <returns>vista de impresiones de coberturas</returns>
        public ActionResult Prints()
        {
            return this.PartialView();
        }

        #region Consultas
        /// <summary>
        /// Consulta ramo tecnico
        /// </summary>
        /// <returns>Listado de ramos tecnico</returns>
        public ActionResult GetLineBusiness()
        {
            LinesBusinessServiceQueryModel linesBusinessServiceQueryModel = DelegateService.UnderwritingParamServiceWeb.GetLinesBusiness();
            if (linesBusinessServiceQueryModel.ErrorTypeService == ENUMO.ErrorTypeService.Ok)
            {
                return new UifJsonResult(true, linesBusinessServiceQueryModel.LineBusinessServiceModel.OrderBy(x => x.Description).ToList());
            }
            else
            {
                return new UifJsonResult(false, new { linesBusinessServiceQueryModel.ErrorTypeService, linesBusinessServiceQueryModel.ErrorDescription });
            }
        }

        /// <summary>
        /// Consulta subramos tecnicos por id de ramo tecnico
        /// </summary>
        /// <param name="lineBusinessId">Id de ramo tecnico</param>
        /// <param name="selectedId">valor por defecto</param>
        /// <returns>Subramos tecnicos por id de reamo tecnico</returns>
        public ActionResult GetSubLineBusinessByLineBusinessId(int lineBusinessId, int? selectedId)
        {
            PARUSM.SubLinesBusinessServiceQueryModel subLinesBusinessServiceQueryModel = DelegateService.UnderwritingParamServiceWeb.GetSubLinesBusinessByLineBusinessId(lineBusinessId);
            if (subLinesBusinessServiceQueryModel.ErrorTypeService == ENUMO.ErrorTypeService.Ok)
            {
                return new UifJsonResult(true, new { items = subLinesBusinessServiceQueryModel.SubLineBusinessServiceModel.OrderBy(x => x.Description).ToList(), selectedId = selectedId });
            }
            else
            {
                return new UifJsonResult(false, new { subLinesBusinessServiceQueryModel.ErrorTypeService, subLinesBusinessServiceQueryModel.ErrorDescription });
            }
        }

        /// <summary>
        /// Consulta amparos por id de ramo tecnico
        /// </summary>
        /// <param name="lineBusinessId">Id de ramo tecnico</param>
        /// <param name="selectedId">Id por default</param>
        /// <returns>Amparos por id de ramo tecnico</returns>
        public ActionResult GetPerilsByLineBusinessId(int lineBusinessId, int? selectedId)
        {
            PARUSM.PerilsServiceQueryModel perilsServiceQueryModel = DelegateService.UnderwritingParamServiceWeb.GetPerilsServiceQueryModelByLineBusinessId(lineBusinessId);
            if (perilsServiceQueryModel.ErrorTypeService == ENUMO.ErrorTypeService.Ok)
            {
                perilsServiceQueryModel.PerilServiceQueryModels = perilsServiceQueryModel.PerilServiceQueryModels.OrderBy(x => x.Description).ToList();
                return new UifJsonResult(true, new { items = perilsServiceQueryModel.PerilServiceQueryModels, selectedId = selectedId });
            }
            else
            {
                return new UifJsonResult(false, new { perilsServiceQueryModel.ErrorTypeService, perilsServiceQueryModel.ErrorDescription });
            }
        }

        /// <summary>
        /// Consulta objetos del seguro relacionos con el id de ramo tecnico
        /// </summary>
        /// <param name="lineBusinessId">Id de ramo tecnico</param>
        /// <param name="selectedId">Id por default</param>
        /// <returns>Lista de objetos del seguro</returns>
        public ActionResult GetInsuredObjectsByLineBusinessId(int lineBusinessId, int? selectedId)
        {
            PARUSM.InsuredObjectsServiceQueryModel insuredObjectsServiceQueryModel = DelegateService.UnderwritingParamServiceWeb.GetInsuredObjectsServiceQueryModel(lineBusinessId);
            if (insuredObjectsServiceQueryModel.ErrorTypeService == ENUMO.ErrorTypeService.Ok)
            {
                return new UifJsonResult(true, new { items = insuredObjectsServiceQueryModel.InsuredObjectServiceQueryModels.OrderBy(x => x.Description).ToList(), selectedId = selectedId });
            }
            else
            {
                return new UifJsonResult(false, new { insuredObjectsServiceQueryModel.ErrorTypeService, insuredObjectsServiceQueryModel.ErrorDescription });
            }
        }

        ///pendiente
        /// <summary>
        /// Consulta los niveles de influencia
        /// </summary>
        /// <returns>Listado de niveles de influencia</returns>
        public ActionResult GetCompositionTypes()
        {
            //PARUSM.CompositionTypesServiceQueryModel compositionTypesServiceQueryModel = null;//DelegateService.UnderwritingParamServiceWeb.GetCompositionTypesServiceQueryModel();
            PARUSM.CompositionTypesServiceQueryModel compositionTypesServiceQueryModel = DelegateService.UnderwritingParamServiceWeb.GetCompositionTypes();
            if (compositionTypesServiceQueryModel.ErrorTypeService == ENUMO.ErrorTypeService.Ok)
            {
                return new UifJsonResult(true, compositionTypesServiceQueryModel.CompositionTypeServiceQueryModels.OrderBy(x => x.Description).ToList());
            }
            else
            {
                return new UifJsonResult(false, new { compositionTypesServiceQueryModel.ErrorTypeService, compositionTypesServiceQueryModel.ErrorDescription });
            }
        }

        /// <summary>
        /// Consulta de clausulas
        /// </summary>
        /// <returns>listado de clausulas</returns>
        public ActionResult GetClauses()
        {
            PARUPSM.ClausesServiceQueryModel clauses = DelegateService.UnderwritingParamServiceWeb.GetClausesSQByConditionLevelType(ConditionLevelType.Coverage);
            if (clauses.ErrorTypeService == ENUMO.ErrorTypeService.Ok)
            {
                return new UifJsonResult(true, clauses.ClauseServiceModels.OrderBy(x => x.Description).ToList());
            }
            else
            {
                return new UifJsonResult(false, new { clauses.ErrorTypeService, clauses.ErrorDescription });
            }
        }

        /// <summary>
        /// Consulta de deducibles
        /// </summary>
        /// <returns>listado de deducibles</returns>
        public ActionResult GetDeductibles()
        {
            PARUPSM.DeductiblesServiceQueryModel deductibles = DelegateService.UnderwritingParamServiceWeb.GetDeductiblesSQM();
            if (deductibles.ErrorTypeService == ENUMO.ErrorTypeService.Ok)
            {
                return new UifJsonResult(true, deductibles.DeductibleServiceQueryModels.OrderBy(x => x.Id).ToList());
            }
            else
            {
                return new UifJsonResult(false, new { deductibles.ErrorTypeService, deductibles.ErrorDescription });
            }
        }
        ///pendiente
        /// <summary>
        /// Consulta de tipos de detalle
        /// </summary>
        /// <returns>listado de tipos de detalle</returns>
        public ActionResult GetDetailTypes()
        {
            //PARUPSM.DetailTypesServiceQueryModel detailTypes = null; //DelegateService.UnderwritingParamServiceWeb.GetDetailTypeSQM();
            PARUPSM.DetailTypesServiceQueryModel detailTypes = DelegateService.UnderwritingParamServiceWeb.GetDetailTypes();
            if (detailTypes.ErrorTypeService == ENUMO.ErrorTypeService.Ok)
            {
                return new UifJsonResult(true, detailTypes.DetailTypeServiceQueryModel.OrderBy(x => x.Description).ToList());
            }
            else
            {
                return new UifJsonResult(false, new { detailTypes.ErrorTypeService, detailTypes.ErrorDescription });
            }
        }
        #endregion

        #region Consulta Cobertura
        /// <summary>
        /// Consulta por filtros descripcion y ramo tecnico
        /// </summary>
        /// <param name="description">descripcion de cobertura</param>
        /// <param name="technicalBranchId">id de ramo tecnico</param>
        /// <returns>listado de coberturas</returns>
        public ActionResult GetCoverageSQMByDescriptionTechnicalBranchId(string description, int? technicalBranchId)
        {
            PARUPSM.CoveragesServiceModel coverage = DelegateService.UnderwritingParamServiceWeb.GetCoveragesSMByDescriptionTechnicalBranchId(description, technicalBranchId);
           
            if (coverage.ErrorTypeService == ENUMO.ErrorTypeService.Ok)
            {
                return new UifJsonResult(true, coverage.CoverageServiceModels.OrderBy(x => x.Description).ToList());
            }
            else
            {
                return new UifJsonResult(false, new { coverage.ErrorTypeService, coverage.ErrorDescription });
            }
        }

        public JsonResult GetPrvCoverage(int coverageId, int coverageNum) 
        {          
            try
            {
                var prvCoverage = DelegateService.underwritingService.GetPrvCoverageByIdAndNum(coverageId, coverageNum);
                if (prvCoverage != null)
                {
                    var prvCoverageView = new PrvCoverageViewModel
                    {
                        CoverageId = prvCoverage.CoverageId,
                        CoverageNum = prvCoverage.CoverageNum,
                        IsPost = prvCoverage.IsPost,
                        BeginDate = prvCoverage.BeginDate
                    };
                    return new UifJsonResult(true, prvCoverageView);
                }
               
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetDataPrvCoverage);
            }
            catch(Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }
        
        #endregion

        #region Consulta avanzada de cobertura
        /// <summary>
        /// Consulta de busqueda avanzada
        /// </summary>
        /// <param name="coverageServiceModel">parametros de filtro</param>
        /// <returns>listado de coberturas</returns>
        public ActionResult GetCoverageSMBySearchAdv(PARUPSM.CoverageServiceModel coverageServiceModel)
        {
            coverageServiceModel.StatusTypeService = ENUMO.StatusTypeService.Original;
            PARUPSM.CoveragesServiceModel coverage = DelegateService.UnderwritingParamServiceWeb.GetCoverageSMBySearchAdv(coverageServiceModel);

            if (coverage.ErrorTypeService == ENUMO.ErrorTypeService.Ok)
            {
                return new UifJsonResult(true, coverage.CoverageServiceModels.OrderBy(x => x.Description).ToList());
            }
            else
            {
                return new UifJsonResult(false, new { coverage.ErrorTypeService, coverage.ErrorDescription });
            }
        }
        #endregion

        #region export excel de coberturas
        /// <summary>
        /// Genera archivo excel de plan de pago
        /// </summary>
        /// <returns>Arhivo de excel de plan de pago</returns>
        public ActionResult GenerateFileToExport()
        {
            try
            {
                ExcelFileServiceModel excelFileServiceModel = DelegateService.UnderwritingParamServiceWeb.GenerateFileToCoverage(App_GlobalResources.Language.FileNameCoverage);
                if (excelFileServiceModel.ErrorTypeService == ENUMO.ErrorTypeService.Ok)
                {
                    var urlFile = excelFileServiceModel.FileData;
                    return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorFileNotFound);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
            }
        }
        #endregion

        #region CRUD Coberturas
        /// <summary>
        /// Crud de coberturas
        /// </summary>
        /// <param name="coverage">cobertura a modificar</param>
        /// <returns>Cobertura a afectar</returns>
        public ActionResult ExecuteOperations(CoverageViewModel coverage, PrvCoverageViewModel prvCoverageData)
        {
            try
            {
                CoverageServiceModel coverageServiceModel = ModelAssembler.CreateCoverage(coverage);
                PARUPSM.CoverageServiceModel result = DelegateService.UnderwritingParamServiceWeb.ExecuteOperationCoverage(coverageServiceModel);
                if (result.ErrorServiceModel.ErrorTypeService != ENUMO.ErrorTypeService.Ok)
                {
                    return new UifJsonResult(false, result.ErrorServiceModel.ErrorDescription);
                }
                else
                {
                    if (coverage.Status == ENUMO.StatusTypeService.Create || coverage.Status == ENUMO.StatusTypeService.Update)
                    {
                        var prvCoverage = new CompanyPrvCoverage
                        {
                            CoverageId = result.Id,
                            CoverageNum = coverage.PerilId,
                            IsPost = prvCoverageData != null && prvCoverageData.IsPost,
                            BeginDate = prvCoverageData?.BeginDate
                        };
                        var resultPrv = (coverage.Status == ENUMO.StatusTypeService.Create) ? DelegateService.underwritingService.CreatePrvCoverage(prvCoverage) : DelegateService.underwritingService.UpdatePrvCoverage(prvCoverage);
                        return new UifJsonResult(true, coverage.Status);
                    }
                    return new UifJsonResult(true, coverage.Status);
                }
              
            }
            catch(Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }            
        }
        #endregion

        public ActionResult GetCoverages2G()
        {
            Coverages2GServiceModel coverages = DelegateService.UnderwritingParamServiceWeb.GetCoverages2GByVehicleInsuredObject();
            if (coverages.ErrorTypeService == ENUMO.ErrorTypeService.Ok)
            {
                return new UifJsonResult(true, ModelAssembler.CreateCoverage2G(coverages.CoverageServiceModels).OrderBy(p => p.Description));
            }
            else
            {
                return new UifJsonResult(false, string.Join("<br/>", coverages.ErrorDescription));
            }
        }

        public ActionResult GetInsuredObjectVehicle()
        {
            List<Object> insuredObjectVehicles = new List<object>();
            insuredObjectVehicles.Add(new { Id = 3 });
            return new UifJsonResult(true, insuredObjectVehicles);
        }

    }
}