using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Sistran.Core.Framework.UIF.Web.Filters;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Application.SecurityServices.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Application.SecurityServices.Enums;
using System.Collections.Generic;
using Sistran.Core.Framework.UIF.Web.Assemblers;
using Sistran.Core.Application.UnderwritingServices.Models;
using System.Threading.Tasks;
using MlUser = Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Framework.UIF.Web.Managers;
using Sistran.Core.Application.UniqueUserServices.Models;
using System.Collections.Concurrent;
using System.Linq;
using Sistran.Core.Framework.UIF2.Security;
using Sistran.Core.Framework.UIF.Web.Helpers;
using System.Diagnostics;

namespace Sistran.Core.Framework.UIF.Web.Controllers
{
    [Authorize]
    [ExcludeFilter(typeof(SessionExpireFilterAttribute))]
    public class AccountController : Controller
    {
        /// <summary>
        /// Sesión de usuarios
        /// </summary>
        private static List<UniqueUserSession> usersSession;

        private ISecurityManager securityManager = new SecurityManager();
        /// <summary>
        /// Obtiene o establece la sesion del usuario
        /// </summary>    
        public static List<UniqueUserSession> UsersSession
        {
            get
            {
                if (usersSession == null)
                {
                    usersSession = new List<UniqueUserSession>();
                }

                return usersSession;
            }

            set
            {
                usersSession = value;
            }
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (returnUrl == null)
            {
                ViewBag.ReturnUrl = returnUrl;
                return RedirectToAction("Vehicle", "Quotation");
            }
            else
            {
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }
        }

        /// <summary>
        /// Establecer usuario autenticado de sesión
        /// </summary>
        /// <param name="userLogin">Modelo usuario</param>
        private void SetSessionAuthenticatedUser(LoginModel sessionModel)
        {
            sessionModel.InSession = true;
            sessionModel.Conections = new ConcurrentBag<string>();
            sessionModel.Id = this.securityManager.GetUserId(sessionModel.UserName);//Lista de enteros, sólo trae los id de grupos
            sessionModel.UserGroup = DelegateService.uniqueUserService.GetUsersGroupByUserId(sessionModel.Id);
            IssuanceAgency issuanceAgency = DelegateService.underwritingService.GetIssuanceAgencyByUserId(sessionModel.Id);
            var UniqueUserSession = new MlUser.UniqueUserSession
            {
                AccountName = sessionModel.UserName,
                RegistrationDate = DateTime.Now,
                LastUpdateDate = DateTime.Now,
                ExpirationDate = DateTime.Now.AddDays(1),
                UserId = sessionModel.Id,
                AgencyId = issuanceAgency == null ? 0 : issuanceAgency.Id,
                AgentId = issuanceAgency == null ? 0 : issuanceAgency.Agent.IndividualId,
                BranchId = issuanceAgency == null ? 0 : issuanceAgency.Branch.Id
            };
            UniqueUserSession uniqueUserSession = DelegateService.uniqueUserService.TryInitSession(UniqueUserSession);
            UniqueUserSession userSession = UsersSession?.Where(t => t != null && t.AccountName == uniqueUserSession.AccountName).FirstOrDefault();
            if (userSession != null)
            {
                UsersSession.Remove(userSession);
                UsersSession.Add(uniqueUserSession);
            }
            else
            {
                UsersSession.Add(uniqueUserSession);
            }
            this.SetAuthCookie(sessionModel.UserName);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            model.UserName = model.UserName.ToUpper();
            var isInSession = false;
            UniqueUserSession uniqueUserSession = new UniqueUserSession();
            int userName = 0;
            string SessionID = System.Web.HttpContext.Current.Session.SessionID.ToString();
            if (Int32.TryParse(model.UserName, out userName))
            {
                ModelState.AddModelError(string.Empty, App_GlobalResources.Language.LBL_LOGIN_MESSAGE_3G);
                return this.View(model);
            }
            else
            {
                uniqueUserSession = DelegateService.uniqueUserService.GetUserInSession(model.UserName);
                if (uniqueUserSession != null && uniqueUserSession.ExpirationDate > DateTime.Now)
                {
                    isInSession = true;
                }
            }
            try
            {
                if (isInSession)
                {
                    ModelState.AddModelError(string.Empty, App_GlobalResources.Language.UserHasActiveSesion);
                    return this.View(model);
                }
                else
                {
                    AuthenticationResult result = await Task.Run(() => DelegateService.AuthenticationProviders.AutenthicateR2(model.UserName, model.Password, null, SessionID));
                    if (result != null)
                    {
                        if (ModelState.IsValid && result.isAuthenticated)
                        {
                            this.SetSessionAuthenticatedUser(model);
                            return Redirect(returnUrl, result);
                        }

                        switch (result.Result)
                        {
                            case AuthenticationEnum.isInvalidPassword:
                                this.ModelState.AddModelError(string.Empty, App_GlobalResources.Language.UserOrPasswordFailed);
                                break;
                            case AuthenticationEnum.isInvalidPasswordWithData:
                                this.ModelState.AddModelError(string.Empty, string.Format(App_GlobalResources.Language.PasswordLockWarning, result.data[0], result.data[1]));
                                break;
                            case AuthenticationEnum.isUserBlocked:
                                this.ModelState.AddModelError(string.Empty, App_GlobalResources.Language.UserBlocledContact);
                                break;
                            case AuthenticationEnum.isUserBlockedWithTime:
                                this.ModelState.AddModelError(string.Empty, string.Format(App_GlobalResources.Language.PasswordLocked, result.data[0]));
                                break;
                            case AuthenticationEnum.IsUserExpired:
                                this.ModelState.AddModelError(string.Empty, string.Format(App_GlobalResources.Language.UserError));
                                break;
                            case AuthenticationEnum.isPasswordExpired:
                                return this.RedirectToAction("Main", "Common/Main", new { type = 9, cause = "expired" });
                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("ErrorAutentication", "LogOn:" + ex.GetBaseException().Message);
                if (App_GlobalResources.Language.ErrorUserNotFound.Contains(ex.Message))
                    ModelState.AddModelError(string.Empty, App_GlobalResources.Language.ErrorUserNotFound);

                else
                    ModelState.AddModelError(string.Empty, App_GlobalResources.Language.MSG_GENERIC_WS);
            }
            return this.View(model);
        }

        private void SetAuthCookie(string userName)
        {
            HttpCookie cookie = FormsAuthentication.GetAuthCookie(userName, true);
            FormsAuthenticationTicket tkt = FormsAuthentication.Decrypt(cookie.Value);

            string userData = this.securityManager.GetUserId(userName).ToString();
            tkt = new FormsAuthenticationTicket(tkt.Version, userName, tkt.IssueDate, tkt.Expiration, true, userData);

            string enc = FormsAuthentication.Encrypt(tkt);
            cookie.Value = enc;
            Response.Cookies.Add(cookie);
        }

        public ActionResult LogOff()
        {
            try
            {
                DelegateService.uniqueUserService.DeletetUserSession(SessionHelper.GetUserId(false));
                UniqueUserSession uniqueUserSession = UsersSession?.Where(x => x != null && x.UserId == SessionHelper.GetUserId(false)).FirstOrDefault();
                if (uniqueUserSession != null)
                {
                    UsersSession.Remove(uniqueUserSession);
                }
                Session.Clear();
                Session.Abandon();
                Session.RemoveAll();
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
                Response.Cache.SetNoStore();
                FormsAuthentication.SignOut();
                Session["USER_ACCESS_PERMISSIONS_KEY"] = null;
                return this.RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("ErrorAutentication", "LogOff:" + ex.GetBaseException().Message);
                DelegateService.uniqueUserService.DeletetUserSession(SessionHelper.GetUserId(false));
                return this.RedirectToAction("Login", "Account");
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Vehicle", "Quotation");
            }
        }


        public static int GetUserId()
        {
            try
            {
                if (HttpContextManager.Current?.User != null && HttpContextManager.Current.User.Identity is FormsIdentity)
                {
                    FormsAuthenticationTicket ticket = ((FormsIdentity)HttpContextManager.Current.User.Identity).Ticket;

                    if (ticket != null)
                    {
                        return Convert.ToInt32(ticket.UserData);
                    }
                    else
                    {
                        throw new Exception("Error de Autenticación");
                    }
                }
                else
                {
                    throw new Exception("Error de Autenticación");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error de Autenticación", ex);
            }
        }

        private ActionResult Redirect(string returnUrl, AuthenticationResult result)
        {
            if (result.Result == AuthenticationEnum.isPasswordExpired)
            {
                return this.RedirectToAction("Main", "Common/Main", new { type = 9, cause = "expired" });
            }

            if (result.Result == AuthenticationEnum.MustChangePasssword)
            {
                return this.RedirectToAction("Main", "Common/Main", new { type = 9, cause = "MustChange" });
            }

            if (result.Result == AuthenticationEnum.isPasswordNearToExpire)
            {
                return this.RedirectToAction("Index", "Home", new { daysToExpite = result.data[0] });
            }

            return this.RedirectToLocal(returnUrl);
        }
    }
}
