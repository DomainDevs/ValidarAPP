using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Filters;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Application.SecurityServices.Enums;
using Sistran.Core.Application.SecurityServices.Models;
using Sistran.Core.Framework.UIF.Web.App_GlobalResources;

namespace Sistran.Core.Framework.UIF.Web.Controllers
{
    [Authorize]
    [ExcludeFilter(typeof(SessionExpireFilterAttribute))]
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (returnUrl == null)
            {
                ViewBag.ReturnUrl = returnUrl;
                return RedirectToAction("Quotation", "Quotation");
            }
            else
            {
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            AuthenticationResult result = DelegateService.authenticationService.Autenthicate(model.UserName, model.Password, null);
            
            if (result != null)
            {
                if (ModelState.IsValid && result.isAuthenticated)
                {
                    SetAuthCookie(model.UserName, result.UserId.ToString());
                    return RedirectToLocal(returnUrl);
                }

                switch (result.Result)
                {
                    case AuthenticationEnum.isInvalidPassword:
                        ModelState.AddModelError("", Language.MessageUserPasswordNotMatch);
                        break;
                    case AuthenticationEnum.isUserBlocked:
                        ModelState.AddModelError("", Language.MessageUserBlocked);
                        break;
                }
            }

            return View(model);
        }

        private void SetAuthCookie(string userName, string userId)
        {
            HttpCookie cookie = FormsAuthentication.GetAuthCookie(userName, false);
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
            ticket = new FormsAuthenticationTicket(ticket.Version, userName, ticket.IssueDate, ticket.Expiration, false, userId);

            HttpCookie cookieLogin = new HttpCookie("InfoLogin");
            cookieLogin.Values["UserName"] = userName;
            cookieLogin.Values["UserId"] = userId;
            Response.Cookies.Add(cookieLogin);

            string encrypt = FormsAuthentication.Encrypt(ticket);
            cookie.Value = encrypt;

            Response.Cookies.Add(cookie);
        }

        public ActionResult LogOff()
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
            FormsAuthentication.SignOut();

            return RedirectToAction("Login", "Account");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Quotation", "Quotation");
            }
        }
    }
}