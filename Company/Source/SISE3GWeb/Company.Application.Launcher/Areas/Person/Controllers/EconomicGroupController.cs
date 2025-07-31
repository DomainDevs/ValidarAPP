using Sistran.Core.Framework.UIF.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF.Web.Areas.Person.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Company.Application.UniquePersonAplicationServices.DTOs;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.OperationQuotaServices.DTOs.EconomicGroup;

namespace Sistran.Core.Framework.UIF.Web.Areas.Person.Controllers
{
    public class EconomicGroupController : Controller
    {
        // GET: Parametrization/EconomicGroup
        public ActionResult EconomicGroup()
        {
            return View();
        }

        public ActionResult SaveEconomicGroup(EconomicGroupView economicGroup) {

            try
            {
                foreach (EconomicGroupDetailView economicGroupDetail in economicGroup.EconomicGroupDetail)
                {
                    EconomicGroupDetail economicGroupDetails = new EconomicGroupDetail();
                    economicGroupDetails = DelegateService.uniquePersonServiceCore.GetExistIndividdualByIndividualId(economicGroupDetail.IndividualId);
                    if (economicGroupDetails.EconomicGroupId != 0 && economicGroupDetails.EconomicGroupId != economicGroup.EconomicGroupId && economicGroupDetail.Enabled)
                    {
                        List<EconomicGroupView> economicGroupRepet = ModelAssembler.CreateEconomicGroupView(DelegateService.uniquePersonServiceV1.GetGroupEconomicById(economicGroupDetails.EconomicGroupId));
                        return new UifJsonResult(false, "El participante " + economicGroupDetail.Description + " se encuentra en el grupo económico " + economicGroupRepet.FirstOrDefault().EconomicGroupName);
                    }
                }
                economicGroup.UserId = SessionHelper.GetUserId();
                return new UifJsonResult(true, DelegateService.uniquePersonServiceV1.CreateEconomicGroup(ModelAssembler.CreateEconomicGroup(economicGroup),ModelAssembler.CreateEconomicGroupDetail(economicGroup.EconomicGroupDetail)));
            }
            catch (Exception) {
                return new UifJsonResult(false, "Modulo en estabilización");
            }
        }

        public ActionResult GetEconomicGroup(int id)
        {
            try
            {
                List<EconomicGroupView> economicGroup = ModelAssembler.CreateEconomicGroupView(DelegateService.uniquePersonServiceV1.GetGroupEconomicById(id));
                return new UifJsonResult(true, economicGroup);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, "");
            }
        }

        public ActionResult GetTributaryType()
        {
            try
            {
                List<TributaryTypeView> tributaryType = ModelAssembler.CreateTributaryTypeView(DelegateService.uniquePersonServiceV1.GetTributaryType());
                return new UifJsonResult(true, tributaryType);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, "");
            }
        }

        public ActionResult UpdateEconomicGroup()
        {

            try
            {

                return new UifJsonResult(true, "");

            }
            catch (Exception)
            {


                return new UifJsonResult(true, "");
            }
        }

        public UifJsonResult GetHoldersByDescriptionInsuredSearchTypeCustomerType(string description)
        {
            try
            {
                List<Holder> holders = DelegateService.underwritingService.GetHoldersByDocument(description, CustomerType.Individual);
                if (holders != null && holders.Any())
                {
                    return new UifJsonResult(true, holders);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorHolderNotFound);
                }


            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchPolicyholder);
            }
        }

        public ActionResult GetIndividualDetails(int individualId, int idEconomicGroup)
        {
            try
            {
                EconomicGroupDetail economicGroupDetails = new EconomicGroupDetail();
                economicGroupDetails = DelegateService.uniquePersonServiceCore.GetExistIndividdualByIndividualId(individualId);
                if(economicGroupDetails.EconomicGroupId == 0 || (idEconomicGroup != 0 && economicGroupDetails.EconomicGroupId == idEconomicGroup))
                {
                    List<OperatingQuotaDTO> operatingQuotaDTOs = new List<OperatingQuotaDTO>();
                    operatingQuotaDTOs = DelegateService.uniquePersonAplicationService.GetOperatingQuotaByIndividualId(individualId);
                    if(operatingQuotaDTOs.Count > 0)
                    {
                        return new UifJsonResult(true, operatingQuotaDTOs.OrderBy(x => x.CurrentTo));
                    }

                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorNoQuota);
                }
                else
                {
                    List<EconomicGroupView> economicGroup = ModelAssembler.CreateEconomicGroupView(DelegateService.uniquePersonServiceV1.GetGroupEconomicById(economicGroupDetails.EconomicGroupId));
                    return new UifJsonResult(false, "El participante ya existe en el grupo económico " + economicGroup.FirstOrDefault().EconomicGroupName);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetOperatingQuota);
            }
        }

        public ActionResult GetEconomicGroupByDocument(string groupName, string document) {

            try
            {
                List<EconomicGroupView> economicGroup = ModelAssembler.CreateEconomicGroupView(DelegateService.uniquePersonServiceV1.GetEconomicGroupByDocument(groupName, document));
                return new UifJsonResult(true, economicGroup);
            }
            catch (Exception ex) 
            {
                return new UifJsonResult(false, "");
            }
        }

        public PartialViewResult AdvancedSearch()
        {
            return PartialView();
        }

        public ActionResult GetEconomicGroupAdvancedSearch(EconomicGroup economicGroup)
        {
            try
            {
                List<EconomicGroupView> economicGroups = new List<EconomicGroupView>();
                economicGroups = ModelAssembler.CreateEconomicGroupView(DelegateService.uniquePersonServiceCore.GetEconomicGroupByEconomicGroup(economicGroup));
                return Json(economicGroups, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult GetIndividualDetail(int economicGroupId)
        {
            try
            {
                List<Insured> insured = new List<Insured>();
                insured = DelegateService.uniquePersonServiceCore.GetEconomicGroupInsureds(economicGroupId);
                decimal operatingQuota = new decimal();
                foreach (Insured item in insured)
                {
                    object insuredOQ = item.ExtendedProperties[0].Value;
                    operatingQuota += (decimal)insuredOQ;
                    item.SetExtendedProperty("TotalOperatingQuota", operatingQuota);
                }
                return new UifJsonResult(true, insured);
            }
            catch (Exception ex)
            {

                return new UifJsonResult(false, null);
            }

        }

        public ActionResult GetPrefixes()
        {
            try
            {
                List<Prefix> prefixes = DelegateService.commonService.GetPrefixes();
                return new UifJsonResult(true, prefixes.OrderBy(x => x.Description).ToList());
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetPrefixes);
            }
        }
        
        public UifJsonResult CreateEconomicGroupEvent(EconomicGroupEventDTO economicGroupEventDTO)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.EconomicGroupService.CreateEconomicGroupEvent(economicGroupEventDTO));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorCreateConsortium);
            }
        }

        public UifJsonResult AssigendIndividualToEconomicGroupEvent(List<EconomicGroupEventDTO> economicGroupEventDTOs)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.EconomicGroupService.AssigendIndividualToEconomicGroupEvent(economicGroupEventDTOs));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorCreateConsortium);
            }
        }
    }
}
