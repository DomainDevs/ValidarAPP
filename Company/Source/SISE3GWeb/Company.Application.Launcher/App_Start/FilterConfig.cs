using Sistran.Core.Framework.UIF.Web.Areas.UniqueUser.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF.Web.Controllers;
using System.Globalization;
using Sistran.Core.Framework.UIF.Web.Helpers;
using System.Configuration;

namespace Sistran.Core.Framework.UIF.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());         
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class NoDirectAccessAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                var session = ConfigurationManager.AppSettings["validateSession"].ToString();
                var converSession = Convert.ToBoolean(session);
                if (converSession)
                {
                    if (filterContext.HttpContext.Request.UrlReferrer == null || filterContext.HttpContext.Request.Url.Host != filterContext.HttpContext.Request.UrlReferrer.Host)
                    {
                        AccountController abandon = new AccountController();
                        int userId = SessionHelper.GetUserId();
                        string UserName = SessionHelper.GetUserName();

                        if (!string.IsNullOrEmpty(UserName))
                        {
                            List<UniqueUserModelsView> uniqueUserModelsView = new List<UniqueUserModelsView>();
                            uniqueUserModelsView = ModelAssembler.CreateUsersModelView(DelegateService.uniqueUserService.GetUsersByAccountName(UserName, userId, 0));

                            if (uniqueUserModelsView.Count == 1)
                            {
                                bool accountLogin = uniqueUserModelsView[0].UniqueUsersLogin.MustChangePassword;

                                if (!accountLogin)
                                {
                                    var date1 = DateTime.ParseExact(Convert.ToDateTime(uniqueUserModelsView[0].UniqueUsersLogin.ExpirationDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    var date2 = DateTime.ParseExact(DelegateService.uniqueUserService.GetDate().ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                                    if (date1 <= date2)
                                    {
                                        accountLogin = true;
                                    }
                                }

                                if (accountLogin)
                                {
                                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { area = "", controller = "Account", action = "Login", id = UrlParameter.Optional }));
                                }
                                else
                                {
                                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { area = "", controller = "Home", action = "Index", id = UrlParameter.Optional }));
                                }
                            }
                            else
                            {
                                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { area = "", controller = "Account", action = "Login", id = UrlParameter.Optional }));
                            }
                        }
                        else
                        {
                            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { area = "", controller = "Account", action = "Login", id = UrlParameter.Optional }));
                        }
                    }
                }
            }
            catch
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { area = "", controller = "Account", action = "Login", id = UrlParameter.Optional }));
            }
        }
    }
}