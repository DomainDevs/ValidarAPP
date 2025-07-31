using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using CPEM = Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UniquePersonService;
using Sistran.Core.Framework.UIF.Web.Areas.QuotaOperational.Models;
using Sistran.Core.Application.CommonService.Models;

namespace Sistran.Core.Framework.UIF.Web.Areas.QuotaOperational.Controllers
{
    public class QuotaOperationalController : Controller
    {
        // GET: QuotaOperational/QuotaOperational
        public ActionResult QuotaOperational()
        {
            return View();
        }

        public ActionResult GetInsuredsByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType, TemporalType temporalType)
        {
            try
            {
                var insureds = DelegateService.underwritingService.GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType, temporalType);
                return new UifJsonResult(true, insureds);

            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.GetBaseException().Message);
            }
        }


        public ActionResult GetOperatingQuotaByIndividualId(int insuredCode, int individualId)
        {
            try
            {
                List<CPEM.OperatingQuota> listOperatingQuota = new List<CPEM.OperatingQuota>();
                listOperatingQuota = DelegateService.uniquePersonService.GetOperatingQuotaByIndividualId(individualId);
                return new UifJsonResult(true, listOperatingQuota);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetOperatingQuota);
    }
}

public ActionResult DeleteQuotaOperation(QuotaOperational.Models.QuotaOperationalModelView operatingQuotaModelView)
{
    try
    {
        CPEM.OperatingQuota operatingQuota = ModelAssembler.MappQuotaOperation(operatingQuotaModelView);
        if (DelegateService.uniquePersonService.DeleteOperatingQuota(operatingQuota))
        {
            return new UifJsonResult(true, App_GlobalResources.Language.DeleteQuotaOperationalSuccess);
        }
        else
        {
            return new UifJsonResult(false, App_GlobalResources.Language.DeleteQuotaOperationalError);
        }
    }
    catch (Exception ex)
    {
        return new UifJsonResult(false, App_GlobalResources.Language.DeleteQuotaOperationalError);
    }
}

public ActionResult UpdateQuotaOperation(QuotaOperational.Models.QuotaOperationalModelView operatingQuotaModelView)
{
    try
    {
        CPEM.OperatingQuota operatingQuota = ModelAssembler.MappQuotaOperation(operatingQuotaModelView);
        DelegateService.uniquePersonService.UpdateOperatingQuota(operatingQuota);
        return new UifJsonResult(true, App_GlobalResources.Language.UpdateQuotaOperationalSuccess);
    }
    catch (Exception ex)
    {
        return new UifJsonResult(false, App_GlobalResources.Language.UpdateQuotaOperationalError);
    }
}

public ActionResult CreateQuotaOperation(QuotaOperational.Models.QuotaOperationalModelView operatingQuotaModelView)
{
    try
    {
        CPEM.OperatingQuota operatingQuota = ModelAssembler.MappQuotaOperation(operatingQuotaModelView);
        List<CPEM.OperatingQuota> operatingQuotas = new List<CPEM.OperatingQuota>();
        operatingQuotas.Add(operatingQuota);
        DelegateService.uniquePersonService.CreateOperatingQuota(operatingQuotas);
        return new UifJsonResult(true, App_GlobalResources.Language.SaveQuotaOperationalSuccess);
    }
    catch (Exception ex)
    {
        return new UifJsonResult(false, App_GlobalResources.Language.SaveQuotaOperationalError);
    }
}

public ActionResult GetPrefixes()
{
    try
    {
        List<Prefix> prefixes = DelegateService.commonService.GetPrefixes();
        return new UifJsonResult(true, prefixes.OrderBy(x => x.Description).ToList());
    }
    catch (Exception ex)
    {
        throw ex;
    }
}

public ActionResult GetCurrencies()
{
    try
    {
        List<Currency> currencies = DelegateService.commonService.GetCurrencies();
        return new UifJsonResult(true, currencies);
    }
    catch (Exception ex)
    {
        throw ex;
    }
}
    }
}