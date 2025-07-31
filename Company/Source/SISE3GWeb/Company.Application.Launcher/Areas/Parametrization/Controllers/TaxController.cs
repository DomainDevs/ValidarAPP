using Sistran.Company.Application.UnderwritingParamApplicationService.DTOs;
using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
using Sistran.Core.Framework.UIF.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Company.Application.Utilities.Enums;
using System.Data.Linq.SqlClient;
using System.Text.RegularExpressions;
using Sistran.Company.Application.Utilities.DTO;
using Sistran.Core.Application.ModelServices.Models.Param;
using ENUMSM = Sistran.Core.Application.ModelServices.Enums;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    /// <summary>
    /// Controlador de la vista de creación y modificación de Impuestos.
    /// Módulo de parametrización.
    /// </summary>
    public class TaxController : Controller
    {

        #region properties

        #region Tax Selects/Combos
        private Helpers.PostEntity entityRateType = new Helpers.PostEntity() { EntityType = "Sistran.Core.Application.Parameters.Entities.RateType" };
        private Helpers.PostEntity entityBaseConditionTax = new Helpers.PostEntity() { EntityType = "Sistran.Core.Application.Tax.Entities.Tax" };
        private Helpers.PostEntity entityBaseTaxWithHolding = new Helpers.PostEntity() { EntityType = "Sistran.Core.Application.Tax.Entities.Tax" };
        private Helpers.PostEntity entityRole = new Helpers.PostEntity() { EntityType = "Sistran.Core.Application.Parameters.Entities.Role" };
        private Helpers.PostEntity entityFeesApplies = new Helpers.PostEntity() { EntityType = "Sistran.Core.Application.Tax.Entities.TaxAttributeType" };

        List<GenericViewModel> li_rateTypes = new List<GenericViewModel>();
        List<GenericViewModel> li_BaseConditionsTax = new List<GenericViewModel>();
        List<GenericViewModel> li_BaseTaxWithHolding = new List<GenericViewModel>();
        List<GenericViewModel> li_roles = new List<GenericViewModel>();
        List<GenericViewModel> li_feesApplies = new List<GenericViewModel>();
        List<GenericViewModel> li_FeesAppliesUpdated = new List<GenericViewModel>();
        #endregion

        #region TaxRates Selects/Combos
        private Helpers.PostEntity entityTaxCondition = new Helpers.PostEntity() { EntityType = "Sistran.Core.Application.Tax.Entities.TaxCondition" };
        private Helpers.PostEntity entityTaxCategory = new Helpers.PostEntity() { EntityType = "Sistran.Core.Application.Tax.Entities.TaxCategory" };
        private Helpers.PostEntity entityCity = new Helpers.PostEntity() { EntityType = "Sistran.Core.Application.Common.Entities.City" };
        private Helpers.PostEntity entityState = new Helpers.PostEntity() { EntityType = "Sistran.Core.Application.Common.Entities.State" };
        private Helpers.PostEntity entityCountry = new Helpers.PostEntity() { EntityType = "Sistran.Core.Application.Common.Entities.Country" };
        private Helpers.PostEntity entityLineBusiness = new Helpers.PostEntity() { EntityType = "Sistran.Core.Application.Common.Entities.LineBusiness" };
        private Helpers.PostEntity entityBranch = new Helpers.PostEntity() { EntityType = "Sistran.Core.Application.Common.Entities.Branch" };
        private Helpers.PostEntity entityEconomicActivityTax = new Helpers.PostEntity() { EntityType = "Sistran.Core.Application.Tax.Entities.EconomicActivityTax" };

        List<GenericViewModel> li_TaxConditions = new List<GenericViewModel>();
        List<GenericViewModel> li_TaxCategories = new List<GenericViewModel>();
        List<GenericViewModel> li_Cities = new List<GenericViewModel>();
        List<GenericViewModel> li_States = new List<GenericViewModel>();
        List<GenericViewModel> li_Countries = new List<GenericViewModel>();
        List<GenericViewModel> li_LinesBusiness = new List<GenericViewModel>();
        List<GenericViewModel> li_Branches = new List<GenericViewModel>();
        List<GenericViewModel> li_EconomicActivities = new List<GenericViewModel>();
        #endregion

        #endregion


        #region views
        public ActionResult Tax()
        {
            return View();
        }
        public ActionResult TaxSearch()
        {
            return PartialView();
        }

        public ActionResult RateTax()
        {
            //ViewBag.taxViewModel = taxViewModel;
            return PartialView();
        }
        public ActionResult RateTaxAdvancedSearch()
        {
            return PartialView();
        }
        public ActionResult CategoryTax()
        {
            return PartialView();
        }
        public ActionResult ConditionTax()
        {
            return PartialView();
        }
        #endregion



        #region publicMethods

        #region Tax Methods
        public ActionResult GetRateTypeTax()
        {
            try
            {
                this.GetListRateTypesTax();
                return new UifJsonResult(true, this.li_rateTypes);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGettingRateTypeData);
            }
        }

        public ActionResult GetRateTypeAdditionalTax()
        {
            try
            {
                this.GetListRateTypesTax();
                return new UifJsonResult(true, this.li_rateTypes);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGettingRateTypeData);
            }
        }

        public ActionResult GetBaseConditionTax()
        {
            try
            {
                this.GetListBaseConditionsTax();
                return new UifJsonResult(true, this.li_BaseConditionsTax);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGettingBaseConditionTaxData);
            }
        }

        public ActionResult GetBaseTaxWithHolding()
        {
            try
            {
                this.GetListBaseTaxWithHoldings();
                return new UifJsonResult(true, this.li_BaseTaxWithHolding);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGettingBaseTaxWithHolding);
            }
        }

        public ActionResult GetCoveragesByLinesBusinessId(int lineBusinessId)
        {
            try
            {
               return new UifJsonResult(true, DelegateService.underwritingService.GetCoveragesByLineBusinessId(lineBusinessId));
            }
            catch
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetCoveragesByLineBusinessIdSubLineBusinessId); 
            }
            
        }

        public ActionResult GetRoles()
        {
            try
            {
                this.GetListRoles();
                return new UifJsonResult(true, this.li_roles);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGettingRole);
            }
        }


        public ActionResult GetFeesApplies()
        {
            try
            {
                this.GetListFeesApplies();
                return new UifJsonResult(true, this.li_FeesAppliesUpdated);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGettingFeesApplies);
            }
        }

        public ActionResult GetDefaultCountry()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.commonService.GetParameterByDescription("DefaultCountryId").NumberParameter);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetParameters);
            }
        }

        public ActionResult SaveTax(TaxViewModel taxViewModel)
        {
            TaxDTO taxDTO = new TaxDTO();
            try
            {
                taxDTO = ModelAssembler.MappTaxApplication(taxViewModel);
                if (taxDTO.Id > 0)
                {
                    taxDTO = DelegateService.CompanyUnderwritingParamApplicationService.UpdateApplicationTax(taxDTO);
                }
                else
                {
                    taxDTO = DelegateService.CompanyUnderwritingParamApplicationService.CreateApplicationTax(taxDTO);
                }

                if (taxDTO.errorDTO.ErrorType == ErrorType.Ok)
                {
                    return new UifJsonResult(true, taxDTO);
                }
                else
                {
                    return new UifJsonResult(true, App_GlobalResources.Language.RecordError);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.RecordError);
            }
        }

        public JsonResult GetTaxByDescription(string Description)
        {
            try
            {
                TaxQueryDTO taxQueryDTO = new TaxQueryDTO();
                taxQueryDTO = DelegateService.CompanyUnderwritingParamApplicationService.GetApplicationTaxByDescription(Description);
                return new UifJsonResult(true, taxQueryDTO.TaxDTOlist);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearch);
            }

        }


        public JsonResult GetTaxByIdAndDescription(int taxId, string taxDescription)
        {
            try
            {
                TaxQueryDTO taxQueryDTO = new TaxQueryDTO();
                taxQueryDTO = DelegateService.CompanyUnderwritingParamApplicationService.GetApplicationTaxByIdAndDescription(taxId, taxDescription);
                return new UifJsonResult(true, taxQueryDTO.TaxDTOlist);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearch);
            }

        }

        /// <summary>
        /// Genera archivo excel impuesto
        /// </summary>
        /// <returns>Ruta del archivo</returns>
        public ActionResult GenerateFileToExport(int taxId)
        {
            try
            {
                ExcelFileServiceModel excelFileServiceModel = DelegateService.underwritingService.GenerateTaxFileReport(taxId);
                if (excelFileServiceModel.ErrorTypeService == ENUMSM.ErrorTypeService.Ok)
                {
                    var urlFile = excelFileServiceModel.FileData;

                    if (!string.IsNullOrEmpty(urlFile))
                    {
                        return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile);
                    }

                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorThereIsNoDataToExport);
                }
                
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorThereIsNoDataToExport);                
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
            }
        }
        #endregion

        #region TaxRate Methods
        public ActionResult GetTaxConditions()
        {
            try
            {
                this.GetListTaxConditions();
                return new UifJsonResult(true, this.li_TaxConditions);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGettingTaxConditionsData);
            }
        }

        public ActionResult GetTaxCategories()
        {
            try
            {
                this.GetListTaxCategories();
                return new UifJsonResult(true, this.li_TaxCategories);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGettingTaxCategoriesData);
            }
        }

        public ActionResult GetCitiesByCountryIdStateId(int countryId, int stateId)
        {
            try
            {
                this.GetListCitiesByCountryIdStateId(countryId, stateId);
                return new UifJsonResult(true, this.li_Cities);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGettingCitiesData);
            }

        }

        public ActionResult GetCities()
        {
            try
            {
                this.GetListCities();
                return new UifJsonResult(true, this.li_Cities);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGettingCitiesData);
            }
        }

        public ActionResult GetStates()
        {
            try
            {
                this.GetListStates(null);
                return new UifJsonResult(true, this.li_States);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGettingStatesData);
            }
        }

        public ActionResult GetStatesByCountry(int CountryId)
        {
            try
            {
                this.GetListStates(CountryId);
                return new UifJsonResult(true, this.li_States);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGettingStatesData);
            }
        }

     
        public ActionResult GetCountries()
        {
            try
            {
                this.GetListCountries();
                return new UifJsonResult(true, this.li_Countries);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGettingCountriesData);
            }
        }

        public ActionResult GetLinesBusiness()
        {
            try
            {
                this.GetListLinesBusiness();
                return new UifJsonResult(true, this.li_LinesBusiness);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGettingLinesBusinessData);
            }
        }

        public ActionResult GetBranches()
        {
            try
            {
                this.GetListBranches();
                return new UifJsonResult(true, this.li_Branches);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGettingBranchesData);
            }
        }

        public ActionResult GetEconomicActivities(string query)
        {
            try
            {
                this.GetListEconomicActivities(query);
                return Json(this.li_EconomicActivities, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(App_GlobalResources.Language.ErrorGettingEconomicActivitiesData, JsonRequestBehavior.DenyGet);
            }
        }

        public ActionResult SaveTaxRate(RateTaxViewModel taxRateViewModel)
        {
            TaxRateDTO taxRateDTO = new TaxRateDTO();
            try
            {
                taxRateDTO = ModelAssembler.MappTaxRateApplication(taxRateViewModel);
                if (taxRateDTO.Id > 0)
                {
                    taxRateDTO = DelegateService.CompanyUnderwritingParamApplicationService.UpdateApplicationTaxRate(taxRateDTO);
                }
                else
                {
                    taxRateDTO = DelegateService.CompanyUnderwritingParamApplicationService.CreateApplicationTaxRate(taxRateDTO);
                }

                if (taxRateDTO.errorDTO.ErrorType == ErrorType.Ok)
                {
                    return new UifJsonResult(true, taxRateDTO);
                }
                else
                {
                    return new UifJsonResult(true, App_GlobalResources.Language.RecordError);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorTaxRate);
            }
        }

        

        #endregion

        #region TaxCategory Methods

        public ActionResult GetCategoriesByTaxId(int taxId)
        {
            try
            {
                TaxCategoryQueryDTO taxCategoryQueryDTO = new TaxCategoryQueryDTO();

                taxCategoryQueryDTO = DelegateService.CompanyUnderwritingParamApplicationService.GetApplicationTaxCategoriesByTaxId(taxId);
                List<CategoryTaxViewModel> categoryTaxViewModel = ModelAssembler.MappTaxCategoryDTOListToTaxCategoryViewModelList(taxCategoryQueryDTO.TaxCategoryDTOlist);

                return new UifJsonResult(true, categoryTaxViewModel);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorTaxCategory);
            }
        }

        public ActionResult SaveTaxCategory(List<CategoryTaxViewModel> categoryTaxViewModelList)
        {
            int taxId = categoryTaxViewModelList.Select(t => t.IdTax).First();
            
            List<TaxCategoryDTO> taxCategoryDTOList = new List<TaxCategoryDTO>();
            List<TaxCategoryDTO> taxCategoryDTOListReturned = new List<TaxCategoryDTO>();
            try
            {
                taxCategoryDTOList = ModelAssembler.MappTaxCategoryListApplication(categoryTaxViewModelList);
                if (taxCategoryDTOList.Count() > 0) {

                    foreach (TaxCategoryDTO taxCategoryDTO in taxCategoryDTOList)
                    {
                        TaxCategoryDTO TaxCategoryDTOReturned = new TaxCategoryDTO();
                        if (taxCategoryDTO.Id > 0)
                        {
                            TaxCategoryDTOReturned = DelegateService.CompanyUnderwritingParamApplicationService.UpdateApplicationTaxCategory(taxCategoryDTO);
                        }
                        else
                        {
                            TaxCategoryDTOReturned = DelegateService.CompanyUnderwritingParamApplicationService.CreateApplicationTaxCategory(taxCategoryDTO);
                        }

                        taxCategoryDTOListReturned.Add(TaxCategoryDTOReturned);

                    }

                    if (taxCategoryDTOListReturned.Where(t => t.errorDTO.ErrorType == ErrorType.Ok).ToList().Count > 0)
                    {
                        return new UifJsonResult(true, taxCategoryDTOListReturned);
                    }
                    else
                    {
                        return new UifJsonResult(true, App_GlobalResources.Language.RecordError);
                    }

                }
                else
                {
                    return new UifJsonResult(true, App_GlobalResources.Language.ErrorEmpty);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorTaxCategory);
            }
        }

        public ActionResult DeleteSelectedTaxCategory(int categoryId, int taxId)
        {
            try
            {
                bool TaxCategoryDeleted = DeleteTaxCategories(categoryId, taxId);
                if (TaxCategoryDeleted)
                {
                    return new UifJsonResult(true, TaxCategoryDeleted);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.TaxCategoryCannotBeDeleted);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.TaxCategoryCannotBeDeleted);
            }

        }
        #endregion

        #region TaxCondition Methods

        public ActionResult GetConditionsByTaxId(int taxId)
        {
            try
            {
                TaxConditionQueryDTO taxConditionQueryDTO = new TaxConditionQueryDTO();

                taxConditionQueryDTO = DelegateService.CompanyUnderwritingParamApplicationService.GetApplicationTaxConditionsByTaxId(taxId);
                List<ConditionTaxViewModel> conditionTaxViewModel = ModelAssembler.MappTaxConditionDTOListToTaxConditionViewModelList(taxConditionQueryDTO.TaxConditionDTOlist);

                return new UifJsonResult(true, conditionTaxViewModel);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorTaxCondition);
            }
        }

        public ActionResult SaveTaxCondition(List<ConditionTaxViewModel> conditionTaxViewModelList)
        {
            int taxId = conditionTaxViewModelList.Select(t => t.IdTax).First();

            List<TaxConditionDTO> taxConditionDTOList = new List<TaxConditionDTO>();
            List<TaxConditionDTO> taxConditionDTOListReturned = new List<TaxConditionDTO>();
            try
            {
                taxConditionDTOList = ModelAssembler.MappTaxConditionListApplication(conditionTaxViewModelList);
                if (taxConditionDTOList.Count() > 0)
                {

                    foreach (TaxConditionDTO taxConditionDTO in taxConditionDTOList)
                    {
                        TaxConditionDTO taxConditionDTOReturned = new TaxConditionDTO();
                        if (taxConditionDTO.Id > 0)
                        {
                            taxConditionDTOReturned = DelegateService.CompanyUnderwritingParamApplicationService.UpdateApplicationTaxCondition(taxConditionDTO);
                        }
                        else
                        {
                            taxConditionDTOReturned = DelegateService.CompanyUnderwritingParamApplicationService.CreateApplicationTaxCondition(taxConditionDTO);
                        }

                        taxConditionDTOListReturned.Add(taxConditionDTOReturned);

                    }

                    if (taxConditionDTOListReturned.Where(t => t.errorDTO.ErrorType == ErrorType.Ok).ToList().Count > 0)
                    {
                        return new UifJsonResult(true, taxConditionDTOListReturned);
                    }
                    else
                    {
                        return new UifJsonResult(true, App_GlobalResources.Language.RecordError);
                    }

                }
                else
                {
                    return new UifJsonResult(true, App_GlobalResources.Language.ErrorEmpty);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorTaxCondition);
            }
        }

        public ActionResult DeleteSelectedTaxCondition(int conditionId, int taxId)
        {
            try
            {
                bool TaxConditionDeleted = DeleteTaxConditions(conditionId, taxId);
                if (TaxConditionDeleted)
                {
                    return new UifJsonResult(true, TaxConditionDeleted);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.TaxConditionCannotBeDeleted);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.TaxConditionCannotBeDeleted);
            }

        }
        #endregion


        #endregion



        #region privateMethods

        #region Tax Methods
        /// <summary>
        /// Carga el listado de tipos de aviso: tabla PARAM.RATE_TYPE
        /// </summary>
        private void GetListRateTypesTax()
        {
            if (this.li_rateTypes.Count == 0)
            {
                this.li_rateTypes = ModelAssembler.CreateRateTypes(
                    ModelAssembler.DynamicToDictionaryList(
                        this.entityRateType.CRUDCliente.Find(
                            this.entityRateType.EntityType, null, null
                            )
                        )
                    ).OrderBy(x => x.Id).ToList();
            }
        }

        /// <summary>
        /// Carga el listado de tipos de aviso: tabla TAX.TAX
        /// </summary>
        private void GetListBaseConditionsTax()
        {
            if (this.li_BaseConditionsTax.Count == 0)
            {
                this.li_BaseConditionsTax = ModelAssembler.CreateBaseConditionsTax(
                    ModelAssembler.DynamicToDictionaryList(
                        this.entityBaseConditionTax.CRUDCliente.Find(
                            this.entityBaseConditionTax.EntityType, null, null
                            )
                        )
                    ).OrderBy(x => x.DescriptionShort).ToList();
            }
        }


        /// <summary>
        /// Carga el listado de tipos de aviso: tabla TAX.TAX
        /// </summary>
        private void GetListBaseTaxWithHoldings()
        {
            if (this.li_BaseTaxWithHolding.Count == 0)
            {
                this.li_BaseTaxWithHolding = ModelAssembler.CreateBaseTaxWithHolding(
                    ModelAssembler.DynamicToDictionaryList(
                        this.entityBaseTaxWithHolding.CRUDCliente.Find(
                            this.entityBaseTaxWithHolding.EntityType, null, null
                            )
                        )
                    ).OrderBy(x => x.DescriptionShort).ToList();
            }
        }


        /// <summary>
        /// Carga el listado de tipos de aviso: tabla PARAM.ROLE
        /// </summary>
        private void GetListRoles()
        {
            if (this.li_roles.Count == 0)
            {
                this.li_roles = ModelAssembler.CreateRole(
                    ModelAssembler.DynamicToDictionaryList(
                        this.entityRole.CRUDCliente.Find(
                            this.entityRole.EntityType, null, null
                            )
                        )
                    ).OrderBy(x => x.Id).ToList();
            }
        }

        /// <summary>
        /// Carga el listado de tipos de aviso: tabla QUO.COMPONENT
        /// </summary>
        private void GetListFeesApplies()
        {
            if (this.li_feesApplies.Count == 0)
            {
                this.li_feesApplies = ModelAssembler.CreateFeeApply(
                    ModelAssembler.DynamicToDictionaryList(
                        this.entityFeesApplies.CRUDCliente.Find(
                            this.entityFeesApplies.EntityType, null, null
                            )
                        )
                    ).OrderBy(x => x.Id).ToList();
            }


            foreach (GenericViewModel feesApplies_item in li_feesApplies)
            {
                switch (feesApplies_item.DescriptionLong)
                {
                    case "STATE_CODE":
                        feesApplies_item.DescriptionLong = App_GlobalResources.Language.TitleState;
                        li_FeesAppliesUpdated.Add(feesApplies_item);
                        break;
                    case "ECONOMIC_ACTIVITY_CODE":
                        feesApplies_item.DescriptionLong = App_GlobalResources.Language.EconomicActivity;
                        li_FeesAppliesUpdated.Add(feesApplies_item);
                        break;
                    case "BRANCH_CODE":
                        feesApplies_item.DescriptionLong = App_GlobalResources.Language.Branch;
                        li_FeesAppliesUpdated.Add(feesApplies_item);
                        break;
                    case "LINE_BUSINESS_CODE":
                        feesApplies_item.DescriptionLong = App_GlobalResources.Language.Prefix;
                        li_FeesAppliesUpdated.Add(feesApplies_item);
                        break;
                    case "COUNTRY_CODE":
                        feesApplies_item.DescriptionLong = App_GlobalResources.Language.Country;
                        li_FeesAppliesUpdated.Add(feesApplies_item);
                        break;
                    case "COVERAGE_ID":
                        feesApplies_item.DescriptionLong = App_GlobalResources.Language.Coverage;
                        li_FeesAppliesUpdated.Add(feesApplies_item);
                        break;
                    case "CITY_CODE":
                        feesApplies_item.DescriptionLong = App_GlobalResources.Language.City;
                        li_FeesAppliesUpdated.Add(feesApplies_item);
                        break;

                }
            }
        }

        #endregion

        #region TaxRate Methods
        /// <summary>
        /// Carga el listado de condiciones impositivas: tabla TAX.TAX_CONDITION
        /// </summary>
        private void GetListTaxConditions()
        {
            if (this.li_TaxConditions.Count == 0)
            {
                this.li_TaxConditions = ModelAssembler.CreateTaxConditions(
                    ModelAssembler.DynamicToDictionaryList(
                        this.entityTaxCondition.CRUDCliente.Find(
                            this.entityTaxCondition.EntityType, null, null
                            )
                        )
                    ).OrderBy(x => x.Id).ToList();

            }
        }

        /// <summary>
        /// Carga el listado de categorias: tabla TAX.TAX_CATEGORY
        /// </summary>
        private void GetListTaxCategories()
        {
            if (this.li_TaxCategories.Count == 0)
            {
                this.li_TaxCategories = ModelAssembler.CreateTaxCategories(
                    ModelAssembler.DynamicToDictionaryList(
                        this.entityTaxCategory.CRUDCliente.Find(
                            this.entityTaxCategory.EntityType, null, null
                            )
                        )
                    ).OrderBy(x => x.Id).ToList();

            }
        }
        

        /// <summary>
        /// Carga el listado de municipios: tabla COMM.CITY
        /// </summary>
        private void GetListCities()
        {
            if (this.li_Cities.Count == 0)
            {
                this.li_Cities = ModelAssembler.CreateCities(
                    ModelAssembler.DynamicToDictionaryList(
                        this.entityCity.CRUDCliente.Find(
                            this.entityCity.EntityType, null, null
                            )
                        )
                    ).OrderBy(x => x.Id).ToList();

            }
        }

        private void GetListCitiesByCountryIdStateId(int countryId, int stateId)
        {
            if (this.li_Cities.Count == 0)
            {
                var cities = DelegateService.commonService.GetCitiesByCountryIdStateId(countryId, stateId);
                foreach (Application.CommonService.Models.City city in cities)
                {
                    var genericViewModel = new GenericViewModel()
                    {
                        Id = city.State.Country.Id,
                        DescriptionLong = city.Description,
                        DescriptionShort = city.SmallDescription,
                        IdC = city.Id,
                        IdD = city.State.Id
                    };
                    li_Cities.Add(genericViewModel);
                }     
                   
            }
        }

        /// <summary>
        /// Carga el listado de municipios: tabla COMM.STATE
        /// </summary>
        private void GetListStates(int? CountryId)
        {
            if (this.li_States.Count == 0)
            {
                this.li_States = ModelAssembler.CreateStates(
                    ModelAssembler.DynamicToDictionaryList(
                        this.entityState.CRUDCliente.Find(
                            this.entityState.EntityType, null, null
                            )
                        )
                    ).OrderBy(x => x.Id).ToList();
            }
            if(CountryId != null)
            {
                li_States = li_States.Where(c => c.IdC == CountryId).ToList();
            }
        }

        /// <summary>
        /// Carga el listado de municipios: tabla COMM.COUNTRY
        /// </summary>
        private void GetListCountries()
        {
            if (this.li_Countries.Count == 0)
            {
                this.li_Countries = ModelAssembler.CreateCountries(
                    ModelAssembler.DynamicToDictionaryList(
                        this.entityCountry.CRUDCliente.Find(
                            this.entityCountry.EntityType, null, null
                            )
                        )
                    ).OrderBy(x => x.Id).ToList();

            }
        }

        /// <summary>
        /// Carga el listado de sucursales: tabla COMM.LINE_BUSINESS
        /// </summary>
        private void GetListLinesBusiness()
        {
            if (this.li_LinesBusiness.Count == 0)
            {
                this.li_LinesBusiness = ModelAssembler.CreateLinesBusiness(
                    ModelAssembler.DynamicToDictionaryList(
                        this.entityLineBusiness.CRUDCliente.Find(
                            this.entityLineBusiness.EntityType, null, null
                            )
                        )
                    ).OrderBy(x => x.Id).ToList();

            }
        }

        /// <summary>
        /// Carga el listado de ramo tecnico: tabla COMM.BRANCHES
        /// </summary>
        private void GetListBranches()
        {
            if (this.li_Branches.Count == 0)
            {
                this.li_Branches = ModelAssembler.CreateBranches(
                    ModelAssembler.DynamicToDictionaryList(
                        this.entityBranch.CRUDCliente.Find(
                            this.entityBranch.EntityType, null, null
                            )
                        )
                    ).OrderBy(x => x.Id).ToList();

            }
        }

        /// <summary>
        /// Carga el listado de actividad economica: tabla TAX.ECONOMIC_ACTIVITY_TAX
        /// </summary>
        private void GetListEconomicActivities(string query)
        {
            if (this.li_EconomicActivities.Count == 0)
            {
                this.li_EconomicActivities = ModelAssembler.CreateEconomicActivitiesTax(
                    ModelAssembler.DynamicToDictionaryList(
                        this.entityEconomicActivityTax.CRUDCliente.Find(
                            this.entityEconomicActivityTax.EntityType, null, null
                            )
                        )
                    ).OrderBy(x => x.Id).ToList();

                if (query != null) {
                    int numberQuery;
                    bool isNumeric = int.TryParse(query, out numberQuery);
                    if (isNumeric)
                    {
                        this.li_EconomicActivities = this.li_EconomicActivities.Where(d => d.Id == numberQuery).ToList();
                    }
                    else
                    {
                        this.li_EconomicActivities = this.li_EconomicActivities.Where(d => Regex.IsMatch(d.DescriptionLong.ToLower(), ".*" + query.ToLower() + ".*")).ToList();
                    }
                }
            }
        }
        #endregion

        #region TaxCategory Methods

        private bool DeleteTaxCategories(int categoryId, int taxId)
        {
            return DelegateService.CompanyUnderwritingParamApplicationService.DeleteApplicationTaxCategoriesByTaxId(categoryId, taxId);
        }

        #endregion

        #region TaxCondition Methods

        private bool DeleteTaxConditions(int conditionId, int taxId)
        {
            return DelegateService.CompanyUnderwritingParamApplicationService.DeleteApplicationTaxConditionsByTaxId(conditionId, taxId);
        }

        #endregion

        #endregion
    }
}
