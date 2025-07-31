using Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Models;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Application.CommonService.Models.Base;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.ModelServices.Models.Param;
using ENUMSM = Sistran.Core.Application.ModelServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using System.IO;

namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Controllers
{
    [Authorize]
    public class SearchUnderwritingController : Controller
    {
        private Helpers.PostEntity entityPrefix = new Helpers.PostEntity() { EntityType = "Sistran.Core.Application.Common.Entities.Prefix" };
        private Helpers.PostEntity entityBranch = new Helpers.PostEntity() { EntityType = "Sistran.Core.Application.Common.Entities.Branch" };

        public ActionResult SubscriptionSearch()
        {
            return View();
        }

        public ActionResult GetSelectTypes()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "Cotización", Value = "1" });
            list.Add(new SelectListItem { Text = "Póliza", Value = "2" });
            list.Add(new SelectListItem { Text = "Temporario", Value = "3" });
            return new UifSelectResult(list);
        }

        /// <summary>
        /// Cargar Ramo
        /// </summary>
        /// <returns></returns>
        public ActionResult GetPrefixes()
        {
            try
            {
                return new UifJsonResult(true,
                ModelAssembler.CreatePrefixes(
                    ModelAssembler.DynamicToDictionaryList(
                        entityPrefix.CRUDCliente.Find(entityPrefix.EntityType, null, null))).OrderBy(x => x.DescriptionLong).ToList());
            }
            catch (Exception ex)
            {
                return new UifJsonResult(true, null);
            }
        }

        /// <summary>
        /// Cargar Sucursal
        /// </summary>
        /// <returns></returns>
        public ActionResult GetBranchs()
        {
            try
            {
                return new UifJsonResult(true,
                    ModelAssembler.CreateBranchs(
                        ModelAssembler.DynamicToDictionaryList(
                            entityBranch.CRUDCliente.Find(entityBranch.EntityType, null, null))).OrderBy(x => x.DescriptionLong).ToList());
            }
            catch (Exception ex)
            {
                return new UifJsonResult(true, null);
            }
        }

        public UifJsonResult GetUserAgenciesByAgentIdDescription(int agentId, string description)
        {
            try
            {
                List<Application.UniqueUserServices.Models.UserAgency> agencies = DelegateService.uniqueUserService.GetCompanyUserAgenciesByAgentIdDescription(agentId, description, SessionHelper.GetUserId());
                return new UifJsonResult(true, agencies);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        public ActionResult GetPrefixesByAgentIdAgents(int agentId)
        {
            List<BasePrefix> prefixes = DelegateService.uniquePersonServiceV1.GetPrefixesByAgentId(agentId);

            if (prefixes.Count > 0)
            {
                return new UifJsonResult(true, prefixes.OrderBy(x => x.Description).ToList());
            }
            else
            {
                return new UifJsonResult(false, App_GlobalResources.Language.MessageAgentWithoutPrefix);
            }
        }
        public ActionResult GetUserAgenciesByAgentId(int agentId)
        {
            List<Sistran.Core.Application.UniqueUserServices.Models.UserAgency> agencies = DelegateService.uniqueUserService.GetAgenciesByAgentIdUserId(agentId, SessionHelper.GetUserId());
            return new UifJsonResult(true, agencies.OrderBy(x => x.FullName).ToList());
        }
        public ActionResult CreateNewVersionQuotation(int operationId)
        {
            try
            {
                Policy policy = DelegateService.quotationService.CreateNewVersionQuotation(operationId);

                if (policy != null)
                {
                    return new UifJsonResult(true, policy);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorQuotationNoExist);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateNewVersionQuotation);
            }
        }


        public UifJsonResult GetUsersByDescription(string description)
        {
            try
            {
                var users = DelegateService.uniqueUserService.GetUsersByAccountName(description, 0, 0);

                if (users != null && users.Any())
                {
                    return new UifJsonResult(true, users);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchUser);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchUser);
            }
        }

        public UifJsonResult GetSearchHoldersByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType, TemporalType temporalType)
        {
            try
            {
                List<Holder> holders = DelegateService.underwritingService.GetHoldersByDocument(description, customerType);
                if (holders != null && holders.Any())
                {
                    return new UifJsonResult(true, holders);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorHolderNotFoundInsured);
                }


            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchPolicyholder);
            }
        }

        public UifJsonResult GetHoldersByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType, TemporalType temporalType)
        {
            try
            {
                var holders = DelegateService.underwritingService.GetCompanyHoldersByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType, temporalType);

                if (holders != null && holders.Any())
                {
                    return new UifJsonResult(true, holders);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorHolderNotFound);
                }
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchPolicyholder);
            }
        }

        public UifJsonResult SearchQuotations(SubscriptionSearchViewModel subscriptionSearchViewModel)
        {
            List<QuotationSearchModelsView> listQuotationSearchModelsView = ModelAssembler.CreateQuotationSearchModelsView(DelegateService.underwritingService.SearchQuotations(ModelAssembler.CreateCompanySubscriptionSearch(subscriptionSearchViewModel)));
            return new UifJsonResult(true, listQuotationSearchModelsView);
        }

        public UifJsonResult SearchPolicies(SubscriptionSearchViewModel subscriptionSearchViewModel)
        {
            List<PolicySearchModelsView> listPolicySearchModelsView = ModelAssembler.CreatePolicySearchModelsView(DelegateService.underwritingService.SearchPolicies(ModelAssembler.CreateCompanySubscriptionSearch(subscriptionSearchViewModel)));
            return new UifJsonResult(true, listPolicySearchModelsView);
        }

        public UifJsonResult SearchTemporaries(SubscriptionSearchViewModel subscriptionSearchViewModel)
        {
            List<TemporalSearchModelsView> listTemporalSearchModelsView = ModelAssembler.CreateTemporalSearchModelsView(DelegateService.underwritingService.SearchTemporals(ModelAssembler.CreateCompanySubscriptionSearch(subscriptionSearchViewModel)));
            return new UifJsonResult(true, listTemporalSearchModelsView);
        }

        /// <summary>
        /// Genera archivo excel ramo técnico
        /// </summary>
        /// <returns>Ruta del archivo</returns>
        public ActionResult GenerateFileToExport(int searchType, SubscriptionSearchViewModel subscriptionSearchViewModel)
        {
            try
            {
                ExcelFileServiceModel excelFileServiceModel = new ExcelFileServiceModel();

                switch (searchType)
                {
                    case 1:
                        excelFileServiceModel = DelegateService.underwritingService.GenerateQuotations("Detalle de Consulta de Cotización", ModelAssembler.CreateCompanySubscriptionSearch(subscriptionSearchViewModel));
                        break;
                    case 2:
                        excelFileServiceModel = DelegateService.underwritingService.GeneratePolicies("Detalle de Consulta de Pólizas", ModelAssembler.CreateCompanySubscriptionSearch(subscriptionSearchViewModel));
                        break;
                    default:
                        break;
                }

                if (excelFileServiceModel.ErrorTypeService == ENUMSM.ErrorTypeService.Ok)
                {
                    var urlFile = excelFileServiceModel.FileData;
                    var filenamefromPath = urlFile.Split(new char[] { '\\' }).Last();
                    //return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile);
                    return new UifJsonResult(true, new { Url = this.Url.Action("ShowExcelFile", "SearchUnderwriting") + "?url=" + urlFile, FileName = filenamefromPath });
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


        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public FileResult ShowExcelFile(string url)
        {
            var pathToTheFile = url;
            var fileStream = new FileStream(pathToTheFile, FileMode.Open, FileAccess.Read);
            return new FileStreamResult(fileStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }
}