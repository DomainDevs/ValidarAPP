using Sistran.Core.Framework.UIF.Web.Models;
using System;
using Sistran.Core.Framework.UIF.Web.Services;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sistran.Core.Framework.UIF.Web.Areas.AuthorizationPolicies.Models;

namespace Sistran.Core.Framework.UIF.Web.Areas.AuthorizationPolicies.Controllers
{
    using Helpers;
    [Authorize]
    public class AuthorizationSarlaftOperationController : Controller
    {
        public ActionResult AuthorizationSarlaftOperation()
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

        public UifJsonResult SearchSuspectOperations(AuthorizationSuspectOperationModelView authorizationSuspectOperationViewModel)
        {
            try
            {
                authorizationSuspectOperationViewModel.AuthoUserId = SessionHelper.GetUserId();
                List<AuthorizationSuspectOperationModelView> listSarlaftSuspectOperations = ModelAssembler.CreateSarlaftSuspectOperations(DelegateService.AuthorizationPoliciesService.SearchSarlaftSuspectOperations(ModelAssembler.CreateSarlaftEventAuthorization(authorizationSuspectOperationViewModel)));
                return new UifJsonResult(true, listSarlaftSuspectOperations);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, "Error");
            }
        }

        public UifJsonResult GetAuthorizationReasons(int eventGroupId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.authorizationPoliciesApplicationService.GetAuthorizationReasons(eventGroupId));
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, "Error");
            }
        }

        public UifJsonResult AuthorizeSuspectOperation(AuthorizationSuspectOperationModelView authorizationSuspectOperationModelView)
        {
            try
            {
                AuthorizationSuspectOperationModelView authorizationSuspectOperationModel = ModelAssembler.CreateSarlaftSuspectOperation(DelegateService.AuthorizationPoliciesService.AuthorizeSuspectOperation(ModelAssembler.CreateSarlaftEventAuthorization(authorizationSuspectOperationModelView)));
                return new UifJsonResult(true, authorizationSuspectOperationModel);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, "Error");
            }
        }

    }
}