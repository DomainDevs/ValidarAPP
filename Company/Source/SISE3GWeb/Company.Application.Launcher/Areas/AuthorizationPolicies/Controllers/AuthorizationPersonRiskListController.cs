using AUTMO = Sistran.Core.Framework.UIF.Web.Areas.AuthorizationPolicies.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.AuthorizationPolicies.Controllers
{
    using Helpers;
    [Authorize]
    public class AuthorizationPersonRiskListController : Controller
    {
        #region PostEntity

        private Helpers.PostEntity entityDocumentType = new Helpers.PostEntity() { EntityType = "Sistran.Core.Application.UniquePerson.Entities.DocumentType" };

        List<GenericViewModel> li_DocType = new List<GenericViewModel>();

        #endregion

        public ActionResult AuthorizationPersonRiskList()
        {
            return View();
        }

        public UifJsonResult GetSarlaftEventGroup()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.authorizationPoliciesApplicationService.GetSarlaftEventGroups());
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetEventGroup);
            }
        }

        public UifJsonResult GetEventAuthorizationsByUserId(AUTMO.AuthorizationRiskListModelView authorizationRiskListModelView)
        {
            try
            {
                int userId = SessionHelper.GetUserId();
                List<AUTMO.AuthorizationRiskListModelView> listauthorizationRiskListModelViews = AUTMO.ModelAssembler.CreateAuthorizationRiskLists(DelegateService.AuthorizationPoliciesService.SearchAuthorizationRiskList(AUTMO.ModelAssembler.CreateCompanyAuthorizationRiskList(authorizationRiskListModelView), userId));
                return new UifJsonResult(true, listauthorizationRiskListModelViews);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, "Error");
            }
        }

        public UifJsonResult AuthorizeRiskListOperation(AUTMO.AuthorizationRiskListModelView authorizationRiskListModelView)
        {
            try
            {
                AUTMO.AuthorizationRiskListModelView authorizationRiskListModel = AUTMO.ModelAssembler.CreateAuthorizationRiskList(DelegateService.AuthorizationPoliciesService.AuthorizeRiskListOperation(AUTMO.ModelAssembler.CreateCompanyAuthorizationRiskList(authorizationRiskListModelView)));
                return new UifJsonResult(true, authorizationRiskListModel);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, "Error");
            }
        }

        #region CRUDServices
        public ActionResult GetDocumentType()
        {
            try
            {
                this.GetDocumentTypes();
                return new UifJsonResult(true, this.li_DocType);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetDocumentTypes);
            }
        }

        private void GetDocumentTypes()
        {
            if (this.li_DocType.Count == 0)
            {
                this.li_DocType = ModelAssembler.CreateDocumentType(
                    ModelAssembler.DynamicToDictionaryList(
                        this.entityDocumentType.CRUDCliente.Find(
                            this.entityDocumentType.EntityType, null, null
                            )
                        )
                    ).OrderBy(x => x.DescriptionLong).ToList();
            }
        }

        #endregion

    }
}
