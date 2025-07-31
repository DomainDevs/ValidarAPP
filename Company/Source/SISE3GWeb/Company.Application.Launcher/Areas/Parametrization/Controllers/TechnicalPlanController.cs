using Sistran.Core.Application.ModelServices.Enums;
using UNDER = Sistran.Core.Application.ModelServices.Models.Underwriting;
using Sistran.Core.Application.ModelServices.Models.UnderwritingParam;
using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.EntityServices.Models;
using Sistran.Core.Application.ModelServices.Models.Param;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    public class TechnicalPlanController : Controller
    {        
        public ActionResult TechnicalPlan()
        {
            return View();
        }

        public ActionResult GetRiskTypes()
        {
            UNDER.CoveredRiskTypesServiceModel riskTypesServiceModel = DelegateService.UnderwritingParamServiceWeb.GetCoveredRiskTypes();
            if (riskTypesServiceModel.ErrorTypeService == ErrorTypeService.Ok)
            {
                List<CoveredRiskTypeViewModel> parametrizationriskTypesVM = new List<CoveredRiskTypeViewModel>();
                foreach (UNDER.CoveredRiskTypeServiceModel mCoveredRiskTypeServiceModel in riskTypesServiceModel.CoveredRiskTypeServiceModel)
                {
                    CoveredRiskTypeViewModel vmCoveredRiskTypeViewModel = new CoveredRiskTypeViewModel();
                    vmCoveredRiskTypeViewModel.ShortDescription = mCoveredRiskTypeServiceModel.SmallDescription;
                    vmCoveredRiskTypeViewModel.Id = mCoveredRiskTypeServiceModel.Id;

                    parametrizationriskTypesVM.Add(vmCoveredRiskTypeViewModel);
                }
                return new UifJsonResult(true, parametrizationriskTypesVM);
            }
            else
            {
                return new UifJsonResult(false, new { riskTypesServiceModel.ErrorTypeService, riskTypesServiceModel.ErrorDescription });
            }
            
        }

        public ActionResult GetInsuredObjects(int coveredRiskType)
        {
            List<InsurencesObjectsViewModel> insurencesObjects = new List<InsurencesObjectsViewModel>();            
            InsurancesObjectsServiceModel InsuredObjectsServiceModel = DelegateService.UnderwritingParamServiceWeb.GetInsuredObjectsByCoveredRiskType(coveredRiskType);

            if (InsuredObjectsServiceModel.ErrorTypeService == ErrorTypeService.Ok)
            {
                insurencesObjects = Models.ModelAssembler.CreateInsuredObjectViewModel(InsuredObjectsServiceModel.InsuredObjectServiceModel);
                return new UifJsonResult(true, insurencesObjects.OrderBy(x => x.Description).ToList());
            }
            else
            {
                return new UifJsonResult(false, new { InsuredObjectsServiceModel.ErrorTypeService, InsuredObjectsServiceModel.ErrorDescription });
            }                                
        }

        public ActionResult GetCoverageByInsuredObjects(int insuredObjectId)
        {
            List<CoverageViewModel> coverages = new List<CoverageViewModel>();
            CoveragesServiceModel coveragesServiceModel = DelegateService.UnderwritingParamServiceWeb.GetCoveragesServiceModelByInsuredObject(insuredObjectId);

            if (coveragesServiceModel.ErrorTypeService == ErrorTypeService.Ok)
            {
                coverages = Models.ModelAssembler.CreateCoverages(coveragesServiceModel.CoverageServiceModels);
                return new UifJsonResult(true, coverages.OrderBy(x => x.Description).ToList());
            }
            else
            {
                return new UifJsonResult(false, new { coveragesServiceModel.ErrorTypeService, coveragesServiceModel.ErrorDescription });
            }            
        }

        public ActionResult GetAlliedCoverages(int coverageId)
        {
            List<TechnicalPlanAllyCoverageViewModel> coverages = new List<TechnicalPlanAllyCoverageViewModel>();
            AllyCoveragesServiceModel coveragesServiceModel = DelegateService.UnderwritingParamServiceWeb.GetCoverageAlliedByCoverageId(coverageId);
            if (coveragesServiceModel.ErrorTypeService == ErrorTypeService.Ok)
            {
                coverages = Models.ModelAssembler.CreateAllyCoverages(coveragesServiceModel.AllyCoverageServiceModels);
                return new UifJsonResult(true, coverages.OrderBy(x => x.Description).ToList());
            }
            else if (coveragesServiceModel.ErrorTypeService == ErrorTypeService.NotFound)
            {
                return new UifJsonResult(true, new List<TechnicalPlanAllyCoverageViewModel>());
            }
            else
            {
                return new UifJsonResult(false, new { coveragesServiceModel.ErrorTypeService, coveragesServiceModel.ErrorDescription });
            }
        }
        
        public ActionResult AdvancedSearchTechnicalPlan(string description, int coveredRiskType)
        {
            List<TechnicalPlanServiceQueryModel> technicalPlans = new List<TechnicalPlanServiceQueryModel>();
            TechnicalPlansServiceQueryModel technicalPlanServiceModel = DelegateService.UnderwritingParamServiceWeb.GetTechnicalPlanByDescriptionOrCoveredRiskType(description, coveredRiskType);

            if (technicalPlanServiceModel.ErrorTypeService == ErrorTypeService.Ok)
            {
                technicalPlans = technicalPlanServiceModel.TechnicalPlanServiceQueryModel;
                return new UifJsonResult(true, technicalPlans.OrderBy(x => x.Description).ToList());
            }
            else if (technicalPlanServiceModel.ErrorTypeService == ErrorTypeService.NotFound)
            {
                return new UifJsonResult(true, new List<TechnicalPlanAllyCoverageViewModel>());
            }
            else
            {
                return new UifJsonResult(false, new { technicalPlanServiceModel.ErrorTypeService, technicalPlanServiceModel.ErrorDescription });
            }            
        }

        public ActionResult GetCoverageByTechnicalPlanId(int technicalPlanId)
        {
            List<TechnicalPlanCoverageServiceRelationModel> technicalPlans = new List<TechnicalPlanCoverageServiceRelationModel>();
            TechnicalPlanCoveragesServiceRelationModel technicalPlanCoverageServiceModel = DelegateService.UnderwritingParamServiceWeb.GetCoveragesByTechnicalPlanId(technicalPlanId);

            if (technicalPlanCoverageServiceModel.ErrorTypeService == ErrorTypeService.Ok)
            {
                technicalPlans = technicalPlanCoverageServiceModel.TechnicalPlanCoverageServiceRelationModel;
                return new UifJsonResult(true, technicalPlans.OrderBy(x => x.Coverage.Id).ToList());
            }
            else if (technicalPlanCoverageServiceModel.ErrorTypeService == ErrorTypeService.NotFound)
            {
                return new UifJsonResult(true, new List<TechnicalPlanCoverageServiceRelationModel>());
            }
            else
            {
                return new UifJsonResult(false, new { technicalPlanCoverageServiceModel.ErrorTypeService, technicalPlanCoverageServiceModel.ErrorDescription });
            }            
        }


        public ActionResult TechnicalPlanAdvSearch()
        {
            return PartialView();
        }
        
        public ActionResult ExecuteOperations(TechnicalPlanViewModel technicalPlan)
        {
            TechnicalPlanServiceModel technicalPlanServiceModel = ModelAssembler.CreateTechnicalPlan(technicalPlan);
            technicalPlanServiceModel = DelegateService.UnderwritingParamServiceWeb.ExecuteOperationTechnicalPlan(technicalPlanServiceModel);
            if (technicalPlanServiceModel.ErrorServiceModel.ErrorTypeService != ErrorTypeService.Ok)
            {
                string errores = string.Empty;
                foreach (var itemError in technicalPlanServiceModel.ErrorServiceModel.ErrorDescription)
                {
                    errores += itemError + "</br>";
                }
                return new UifJsonResult(false, errores);
            }
            else
            {
                return new UifJsonResult(true, technicalPlanServiceModel.StatusTypeService);
            }            
        }

        /// <summary>
        /// Genera archivo excel de plan técnico
        /// </summary>
        /// <returns>Arhivo de excel de técnico</returns>
        public ActionResult GenerateFileToExport()
        {
            try
            {
                TechnicalPlansServiceModel technicalPlansServiceModel = DelegateService.UnderwritingParamServiceWeb.GetTechnicalPlansServiceModel();
                List<TechnicalPlanServiceModel> list = technicalPlansServiceModel.TechnicalPlanServiceModel;
                ExcelFileServiceModel excelFileServiceModel = DelegateService.UnderwritingParamServiceWeb.GenerateFileToTechnicalPlan(list, App_GlobalResources.Language.LabelTechnicalPlan);
                if (excelFileServiceModel.ErrorTypeService == ErrorTypeService.Ok)
                {
                    var urlFile = excelFileServiceModel.FileData;
                    return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorFileNotFound);
                }
            }
            catch (System.Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
            }
        }
    }
}