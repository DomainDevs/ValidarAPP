using Sistran.Company.Application.ExternalProxyServices.Models;
//using Sistran.Company.Application.ModelServices.Models.Param;
using Sistran.Company.Application.ProductServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.CommonService.Models.Base;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ENUMSM = Sistran.Core.Application.ModelServices.Enums;
using Sistran.Company.Application.ModelServices.Enums;
using Sistran.Core.Application.ModelServices.Models.Param;
using System.Globalization;
using System.IO;

namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Controllers
{
    public class ProductionReportController : Controller
    {
        // GET: Underwriting/ProductionReport
        public ActionResult ProductionReport()
        {
            return View();
        }

        public UifJsonResult GetUserAgenciesByAgentIdDescription(int agentId, string description)
        {
            try
            {
                List<Sistran.Core.Application.UniqueUserServices.Models.UserAgency> agencies = DelegateService.uniqueUserService.GetCompanyUserAgenciesByAgentIdDescription(agentId, description, SessionHelper.GetUserId());
                return new UifJsonResult(true, agencies);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        public ActionResult GetBranches()
        {
            List<Branch> branches = DelegateService.uniqueUserService.GetBranchesByUserId(SessionHelper.GetUserId());
            return new UifJsonResult(true, branches.OrderBy(x => x.Description).ToList());
        }

        public ActionResult GetSalePointsByBranchId(int branchId)
        {
            List<SalePoint> salePoints = DelegateService.commonService.GetSalePointByBranchId(branchId, true);
            return new UifJsonResult(true, salePoints.OrderBy(x => x.Description).ToList());
        }

        public ActionResult GetPrefixes()
        {
            List<Prefix> prefixes = DelegateService.commonService.GetPrefixes();
            return new UifJsonResult(true, prefixes.OrderBy(x => x.Description).ToList());
        }

        public ActionResult GetProductsByAgentIdPrefixId(int agentId, int prefixId, bool isCollective)
        {
            try
            {
                List<CompanyProduct> products = DelegateService.underwritingService.GetCompanyProductsByAgentIdPrefixId(agentId, prefixId, isCollective);
                return new UifJsonResult(true, products);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.MessageAgentWithoutProduct);
            }

        }

        public ActionResult GetUserAgenciesByAgentId(int agentId)
        {
            List<Sistran.Core.Application.UniqueUserServices.Models.UserAgency> agencies = DelegateService.uniqueUserService.GetAgenciesByAgentIdUserId(agentId, SessionHelper.GetUserId());
            return new UifJsonResult(true, agencies.OrderBy(x => x.FullName).ToList());
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

        public UifJsonResult ExistProductAgentByAgentIdPrefixIdProductId(int agentId, int prefixId, int productId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.productService.ExistProductAgentByAgentIdPrefixIdProductId(agentId, prefixId, productId));
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                Response.StatusDescription = App_GlobalResources.Language.ErrorSearchProductAgent;
                return new UifJsonResult(false, ex.Message);
            }
        }

        public ActionResult GetAgencyByAgentIdAgencyId(int agentId, int agencyId)
        {
            try
            {
                Agency agency = DelegateService.uniquePersonServiceV1.GetAgencyByAgentIdAgentAgencyId(agentId, agencyId);

                if (agency != null)
                {
                    if (agency.DateDeclined > DateTime.MinValue)
                    {
                        return new UifJsonResult(false, App_GlobalResources.Language.ErrorIntermediaryDischarged);
                    }
                    else
                    {
                        return new UifJsonResult(true, agency);
                    }
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorIntermediaryNoExist);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorValidIntermediary);
            }
        }

        public ActionResult GetFileProductionReport(int agentId, int branchId, int prefixId, int productId, string inputFrom, string inputTo)
        {
            try
            {
                DateTime inputFromDateTime = DateTime.Parse(inputFrom).AddMinutes(1);
                DateTime inputToDateTime = DateTime.Parse(inputTo).AddHours(23).AddMinutes(59);
               
                int userId = SessionHelper.GetUserId();
                ProductionReportViewModel productionReportViewModel = new ProductionReportViewModel();
                productionReportViewModel.AgentId = agentId;
                productionReportViewModel.BranchId = branchId;
                
                productionReportViewModel.ProductId = productId;
                productionReportViewModel.PrefixId = prefixId;
                productionReportViewModel.InputFromDateTime = inputFromDateTime;
                productionReportViewModel.InputToDateTime = inputToDateTime;
                productionReportViewModel.UserId = userId;

                ExcelFileServiceModel excelFileServiceModel = DelegateService.underwritingService.GeneratePoductionReportServiceModel(ModelAssembler.MappCreateProductionReports(productionReportViewModel));
                if (excelFileServiceModel.ErrorTypeService == ENUMSM.ErrorTypeService.Ok)
                {
                    var urlFile = excelFileServiceModel.FileData;
                    var filenamefromPath = urlFile.Split(new char[] { '\\' }).Last();
                    //return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile);
                    return new UifJsonResult(true, new { Url = this.Url.Action("ShowExcelFile", "ProductionReport") + "?url=" + urlFile, FileName = filenamefromPath });
                }
                else if (excelFileServiceModel.ErrorTypeService == ENUMSM.ErrorTypeService.NotFound)
                {
                    var urlFile = excelFileServiceModel.FileData;
                    return new UifJsonResult(false, App_GlobalResources.Language.ReportEmpty);
                }
                else
                    return new UifJsonResult(false, App_GlobalResources.Language.ReportEmpty);
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