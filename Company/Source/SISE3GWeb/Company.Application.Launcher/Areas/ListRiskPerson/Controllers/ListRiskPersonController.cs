using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Sistran.Company.Application.UniquePersonListRiskApplicationServices.DTO;
using Sistran.Company.Application.UniquePersonListRiskApplicationServices.Enum;
using Sistran.Core.Framework.UIF.Web.Areas.ListRiskPerson.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;

namespace Sistran.Core.Framework.UIF.Web.Areas.ListRiskPerson.Controllers
{
    public class ListRiskPersonController : Controller
    {
        private Helpers.PostEntity entityListRisk = new Helpers.PostEntity() { EntityType = "Sistran.Core.Application.UniquePerson.Entities.RiskList" };
        // GET: ListRiskPerson/ListRiskPerson
        public ActionResult ListRiskPerson()
        {
            return View();
        }
        public ActionResult AdvancedSearch()
        {
            return View();
        }

        public JsonResult GetDocumentType()
        {
            try
            {
                var IdentityTypes = DelegateService.uniquePersonAplicationService.GetAplicationDocumentTypes(1);
                return new UifJsonResult(true, IdentityTypes);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, false);
                throw;
            }
        }

        public JsonResult GetListRisk()
        {
            try
            {
                List<ListRiskDTO> listRisk = DelegateService.uniquePersonListRiskApplication.GetListRisk();
                return new UifJsonResult(true, listRisk);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetListRisk);
                throw;
            }
        }

        public JsonResult GetSearchType()
        {
            try
            {
                var result = ModelAssembler.CreatePersonSearchType(
                    ModelAssembler.DynamicToDictionaryList(entityListRisk.CRUDCliente.Find(
                        entityListRisk.EntityType, null, null
                        ))).OrderBy(x => x.Description).ToList();
                return new UifJsonResult(true, result);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ActionResult CreateListRiskPerson(ListRiskPersonDTO listRisk, string documentNumber, int? listRiskType)
        {
            try
            {
                listRisk.CreateListUserId = SessionHelper.GetUserId();
                listRisk.CreateListUserName = SessionHelper.GetUserName();
                switch (listRisk.ListRisk.Id)
                {

                    case 2:
                        listRisk.ListRisk.RiskListType = (int)RiskListTypeEnum.OFAC;
                        listRisk.ListRisk.RiskListTypeDescription = RiskListConstants.OFAC;
                        break;
                    case 3:
                        listRisk.ListRisk.RiskListType = (int)RiskListTypeEnum.ONU;
                        listRisk.ListRisk.RiskListTypeDescription = RiskListConstants.ONU;
                        break;
                    default:
                        listRisk.ListRisk.RiskListType = (int)RiskListTypeEnum.OWN;
                        listRisk.ListRisk.RiskListTypeDescription = RiskListConstants.OWN;
                        break;
                }


                var result = DelegateService.uniquePersonListRiskApplication.CreateListRiskPersonApplication(listRisk);
                if (result != null)
                {
                    return GetAssignedListMantenance(documentNumber, listRiskType);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveListRiskPerson);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public ActionResult GetListRiskPersonByIndexKey(string documentNumber)
        {
            try
            {
                var listResutl = DelegateService.uniquePersonListRiskApplication.GetListRiskPersonByDocumentNumber(documentNumber);
                if (listResutl != null)
                {
                    return new UifJsonResult(true, listResutl);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorPersonInformationNotFound);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPersonInformation);
            }
        }

        public ActionResult GetListRiskPerson(AdvancedSearchViewModel search)
        {
            try
            {
                var listResutl = DelegateService.uniquePersonListRiskApplication.GetListRiskPersonByAdvanceSearch(search.DocumentNumber, search.Name, search.Surname, search.NickName, search.ListRiskId);
                if (listResutl != null)
                {
                    return new UifJsonResult(true, listResutl);
                }
                else
                {
                    return new UifJsonResult(false, false);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public ActionResult UpdateListRiskPerson(ListRiskPersonViewModel listRisk)
        {
            try
            {
                return Json(0);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public ActionResult GetFileToListRiskPerson()
        {
            try
            {
                return Json(0);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public ActionResult GetAssignedListMantenance(string documentNumber, int? listRisk)
        {
            try
            {
                var listResult = DelegateService.uniquePersonListRiskApplication.GetAssignedListMantenance(documentNumber, listRisk);
                if (listResult != null)
                {
                    return new UifJsonResult(true, listResult);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorPersonInformationNotFound);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPersonInformation);
            }
        }

        public ActionResult GetListRiskTypes()
        {
            var riskListType = Enum.GetValues(typeof(RiskListTypeEnum))
                                .Cast<RiskListTypeEnum>()
                                .Select(v => new { Id = (int)v, Description = v.ToString() })
                                .ToList();
            return new UifJsonResult(true, riskListType);

        }

        public void GenerateListRiskRequest()
        {
            DelegateService.uniquePersonListRiskApplication.GenerateListRiskProcessRequest(false, true, false, "", false);
        }
    }
}