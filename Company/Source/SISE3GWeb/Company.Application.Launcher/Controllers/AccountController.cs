// -----------------------------------------------------------------------
// <copyright file="AccountController.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Desconocido</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Controllers
{
    using Sistran.Core.Application.SecurityServices.Enums;
    using Sistran.Core.Application.SecurityServices.Models;
    using Sistran.Core.Application.UniqueUserServices.Models;
    using Sistran.Core.Framework.UIF.Web.Autentication;
    using Sistran.Core.Framework.UIF.Web.Helpers;
    using Sistran.Core.Framework.UIF.Web.Models;
    using Sistran.Core.Framework.UIF.Web.Resources;
    using Sistran.Core.Framework.UIF.Web.Services;
    using Sistran.Core.Framework.UIF2.Security;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using static Sistran.Core.Application.UniqueUserServices.Enums.UniqueUserTypes;
    using Enum = Sistran.Core.Framework.UIF.Web.Helpers.Enums;
    using MlUser = Sistran.Core.Application.UniqueUserServices.Models;
    using Sistran.Core.Framework.UIF.Web.Areas.Person.Enums;
    using System.Net.Http;
    using System.Configuration;
    using Newtonsoft.Json;
    using System.Web.UI.WebControls;
    using System.IdentityModel.Tokens.Jwt;
    using System.Text;

    /// <summary>
    /// Controlador cuenta usuario
    /// </summary>
    [Authorize]
    public class AccountController : Controller
    {
        /// <summary>
        /// security Manager
        /// </summary>
        private ISecurityManager securityManager = new SecurityManager();
        /// <summary>
        /// Obtener tipos de documentos de Colpatria
        /// </summary>
        /// <returns>retorna tipos de documentos de Colpatria</returns>
        [HttpGet]
        [AllowAnonymous]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult Login(string returnUrl)
        {
            if (Request.IsAuthenticated)
            {
                if (Request.Cookies?[FormsAuthentication.FormsCookieName]?.Value != null)
                {
                    string accountName = FormsAuthentication.Decrypt(Request.Cookies?[FormsAuthentication.FormsCookieName]?.Value)?.Name;
                    UniqueUserSession userSession = new UniqueUserSession();
                    var UniqueUserSession = new MlUser.UniqueUserSession
                    {
                        UserId = this.securityManager.GetUserId(accountName),
                        AccountName = accountName,
                        RegistrationDate = DateTime.Now,
                        LastUpdateDate = DateTime.Now,
                        ExpirationDate = DateTime.Now.AddDays(1),
                        SessionId = Session.SessionID
                    };
                    userSession = UsersSessionHelper.UsersSession?.Where(t => t != null && t.AccountName == accountName).FirstOrDefault();
                    if (userSession == null)
                    {
                        userSession = DelegateService.uniqueUserService.GetUserInSession(accountName);
                        if (userSession == null)
                        {
                            userSession = DelegateService.uniqueUserService.TryInitSession(UniqueUserSession);
                        }
                        if (userSession != null)
                            UsersSessionHelper.Add(userSession);
                    }
                    else
                    {
                        userSession = DelegateService.uniqueUserService.TryInitSession(UniqueUserSession);
                        if (userSession != null)
                        {
                            UniqueUserSession userSessionItem = UsersSessionHelper.UsersSession.Where(t => t != null && t.AccountName == accountName).FirstOrDefault();
                            if (userSessionItem != null)
                                UsersSessionHelper.Remove(userSessionItem);
                            UsersSessionHelper.Add(userSession);
                        }
                    }
                }
                return this.RedirectToAction("Index", "Home");
            }
            else
            {
                return this.View();
            }
        }

        /// <summary>
        /// Login usuario
        /// </summary>
        /// <param name="model">modelo usuario</param>
        /// <param name="returnUrl">ulr a retornar</param>
        /// <returns>redirecciona al home</returns>
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
                            SetIdentityServer(model);
                            return Redirect(returnUrl, result);
                        }

                        switch (result.Result)
                        {
                            case AuthenticationEnum.IsProfileDisabled:
                                this.ModelState.AddModelError(String.Empty, App_GlobalResources.Language.IsProfileDisabled);
                                break;
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
                            case AuthenticationEnum.IsUserWithoutProfiles:
                                this.ModelState.AddModelError(string.Empty, string.Format(App_GlobalResources.Language.UserWithoutProfiles));
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
                    ModelState.AddModelError(string.Empty, Global.MsgUserDisabled);
            }
            return this.View(model);
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

        /// <summary>
        /// Desconectarse, cerrar session
        /// </summary>
        /// <returns>redirecciona a la ruta</returns>
        public ActionResult LogOff()
        {
            try
            {
                DelegateService.uniquePersonServiceCore.DeleteUserAssignedConsortium((int)parameterType.FutureSociety, SessionHelper.GetUserId());
                DelegateService.uniqueUserService.DeletetUserSession(SessionHelper.GetUserId(false));
                UniqueUserSession uniqueUserSession = UsersSessionHelper.UsersSession?.Where(x => x != null && x.UserId == SessionHelper.GetUserId(false)).FirstOrDefault();
                if (uniqueUserSession != null)
                {
                    UsersSessionHelper.Remove(uniqueUserSession);
                }
                Session.Clear();
                Session.Abandon();
                Session.RemoveAll();
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
                Response.Cache.SetNoStore();
                FormsAuthentication.SignOut();
                Session["USER_ACCESS_PERMISSIONS_KEY"] = null;

                ClearJwtCookie();
                return this.RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("ErrorAutentication", "LogOff:" + ex.GetBaseException().Message);
                DelegateService.uniqueUserService.DeletetUserSession(SessionHelper.GetUserId(false));
                return this.RedirectToAction("Login", "Account");
            }

        }

        public ActionResult AuthenticationQsise()
        {
            string AuthenticationQsise = ConfigurationManager.AppSettings["SiseConsultas"];

            return this.Redirect(AuthenticationQsise);
        }
        /// <summary>
        /// cambio de contraseña
        /// </summary>
        /// <returns>vista model</returns>
        public ActionResult ChangePassword()
        {
            RegisterModel model = new RegisterModel();
            return this.View(model);
        }
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }
        /// <summary>
        /// Guardar contraseña
        /// </summary>
        /// <param name="model">modelo de usuario</param>
        /// <returns>respuesta de metodo</returns>
        [HttpPost]
        public ActionResult SavePassword(RegisterModel model)
        {
            List<string> errorString = new List<string>();

            if (model.OldPassword == model.Password)
            {
                errorString.Add(App_GlobalResources.Language.NewPasswordCantBeOldPassword);
                return new UifJsonResult(false, errorString);
            }

            Dictionary<int, int> errors = new Dictionary<int, int>();
            bool hasChanged = DelegateService.uniqueUserService.ChangePassword(true, SessionHelper.GetUserName(), model.OldPassword, model.Password, out errors);

            if (hasChanged)
            {
                return new UifJsonResult(true, null);
            }
            else
            {
                foreach (KeyValuePair<int, int> error in errors)
                {
                    errorString.Add(string.Format(this.GetChangePasswordResource(error.Key), error.Value));
                }

                return new UifJsonResult(false, errorString);
            }
        }

        /// <summary>
        /// Obtener acceso a los botones por ruta
        /// </summary>
        /// <param name="path">Ruta boton</param>
        /// <returns>accede a los botones por ruta</returns>
        public ActionResult GetAccessButtonsByPath(string path)
        {
            try
            {
                List<MlUser.AccessObject> accessButtons = new List<MlUser.AccessObject>();
                List<MlUser.AccessObject> currentButtons = new List<MlUser.AccessObject>();
                HttpCookie cookieLogin = Request.Cookies.Get("InfoLogin");
                if (cookieLogin != null)
                {
                    if (cookieLogin.Values["Buttons"] != null)
                    {
                        accessButtons = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MlUser.AccessObject>>(cookieLogin.Values["Buttons"]).ToList();
                        if (accessButtons.Count > 0)
                        {
                            currentButtons = accessButtons.Where(x => path.Contains(x.Url)).ToList();
                        }
                    }
                }

                return new UifJsonResult(true, currentButtons);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.ToString());
            }
        }
        /// <summary>
        /// redirecciona a la pagina donde se muestran los errores
        /// </summary>
        /// <param name="returnURL">Url error</param>
        /// <returns>retorna a la pagina donde se muestran los errores</returns>
        [AllowAnonymous]
        public ActionResult Error(string returnURL)
        {
            ErrorInfoModel model = new ErrorInfoModel()
            {
                Url = returnURL
            };
            return this.View("~/Views/Shared/Error.cshtml", model);
        }

        /// <summary>
        /// Obtener botones por nombre de usuario
        /// </summary>
        /// <returns>retorna botones por nombre de usuario</returns>
        public ActionResult GetButtonsByUserName()
        {
            try
            {
                ////Se cargan los botones bloqueados para el usuario
                List<MlUser.AccessObject> accessButtons = new List<MlUser.AccessObject>();
                accessButtons = DelegateService.uniqueUserService.GetButtonsByUserName(SessionHelper.GetUserName());
                return new UifJsonResult(true, accessButtons);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
            }
        }


        /// <summary>
        /// Mapeo de tipos de identificacion de axa , a los tipos que tiene el servicio de axa
        /// </summary>
        /// <param name="identificationType">tipo de identificacion</param>
        /// <returns>Retorna el enum correspondiente al tipo de documento</returns>
        private static autenticacionReqBODYIdentificacionTipoIdentificacion GetIdentificationType(string identificationType)
        {
            switch (identificationType)
            {
                case "CC":
                    return autenticacionReqBODYIdentificacionTipoIdentificacion.CC;
                case "CE":
                    return autenticacionReqBODYIdentificacionTipoIdentificacion.CE;
                case "NIT":
                    return autenticacionReqBODYIdentificacionTipoIdentificacion.NIT;
                case "PA":
                    return autenticacionReqBODYIdentificacionTipoIdentificacion.PA;
                case "RC":
                    return autenticacionReqBODYIdentificacionTipoIdentificacion.RC;
                case "TI":
                    return autenticacionReqBODYIdentificacionTipoIdentificacion.TI;
                default:
                    return autenticacionReqBODYIdentificacionTipoIdentificacion.CC;
            }
        }

        /// <summary>
        /// Obtiene el ID del TipoDeDocumento de Identificación        
        /// </summary>
        /// <param name="nemonic">Abreviación del documento de Identificación</param>
        /// <returns>Id del Tipo de Documento de Identificación</returns>
        private Enum.DocumentType GetDocumentID(string nemonic)
        {
            switch (nemonic)
            {
                case "CC":
                    return Enum.DocumentType.Cc;
                case "CE":
                    return Enum.DocumentType.Ce;
                case "TI":
                    return Enum.DocumentType.Ti;
                case "PS":
                    return Enum.DocumentType.Ps;
                case "TSS":
                    return Enum.DocumentType.Tss;
                case "SEN":
                    return Enum.DocumentType.Sen;
                case "FDI":
                    return Enum.DocumentType.Fdi;
                case "RC":
                    return Enum.DocumentType.Rc;
                case "SD":
                    return Enum.DocumentType.Sd;
                case "SOC":
                    return Enum.DocumentType.Soc;
                default:
                    return Enum.DocumentType.Default;
            }
        }

        #region private methods
        /// <summary>
        /// Obtener recurso de cambio de contraseña
        /// </summary>
        /// <param name="messageEnum">enum del mesaje</param>
        /// <returns>retorna el mensaje</returns>
        private string GetChangePasswordResource(int messageEnum)
        {
            switch (messageEnum)
            {
                case (int)ChangePasswordErrors.CantHaveSecuence:
                    return App_GlobalResources.Language.PasswordCantHaveSecuence;
                case (int)ChangePasswordErrors.ContainsUserName:
                    return App_GlobalResources.Language.PasswordContainsUserName;
                case (int)ChangePasswordErrors.FirstMustNonNumber:
                    return App_GlobalResources.Language.PasswordFirstMustNonNumber;
                case (int)ChangePasswordErrors.LastMustNotNumber:
                    return App_GlobalResources.Language.PasswordLastMustNotNumber;
                case (int)ChangePasswordErrors.minLenght:
                    return App_GlobalResources.Language.PasswordminLenght;
                case (int)ChangePasswordErrors.MustHaveLower:
                    return App_GlobalResources.Language.PasswordMustHaveLower;
                case (int)ChangePasswordErrors.MustHaveNumber:
                    return App_GlobalResources.Language.PasswordMustHaveNumber;
                case (int)ChangePasswordErrors.MustHaveSpecial:
                    return App_GlobalResources.Language.PasswordMustHaveSpecial;
                case (int)ChangePasswordErrors.MustHaveUpper:
                    return App_GlobalResources.Language.PasswordMustHaveUpper;
                case (int)ChangePasswordErrors.SimilarTohistorical:
                    return App_GlobalResources.Language.PasswordSimilarTohistorical;
                case (int)ChangePasswordErrors.OldPassswordDoesntMatch:
                    return App_GlobalResources.Language.OldPassswordDoesntMatch;
                default:
                    return string.Empty;
            }
        }
        #endregion

        /// <summary>
        /// Establecer cookies de autenticación
        /// </summary>
        /// <param name="userName">Nombre de usuario</param>
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

        #region Helpers
        /// <summary>
        /// Redirigir a Local
        /// </summary>
        /// <param name="returnUrl"> url a redireccionar</param>
        /// <returns>Redirecciona a la ruta enviada</returns>
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return this.Redirect(returnUrl);
            }
            else
            {
                return this.RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Obtener texto Captcha
        /// </summary>
        /// <param name="cantCharMin">Cantidad Minima de caracteres</param>
        /// <param name="cantCharMax">Cantidad Maxima de caracteres</param>
        /// <returns>retorna la cadena con el capcha</returns>
        private string GetCaptchaText(int cantCharMin, int cantCharMax)
        {
            Random numRandom = new Random();
            string cad = string.Empty;
            int cantChar = numRandom.Next(cantCharMin, cantCharMax + 1);
            for (int i = 0; i < cantChar; i++)
            {
                int typeChar = numRandom.Next(0, 3);
                switch (typeChar)
                {
                    case 0:
                        cad = cad + Convert.ToChar(numRandom.Next(48, 58));
                        break;
                    case 1:
                        cad = cad + Convert.ToChar(numRandom.Next(65, 91));
                        break;
                    case 2:
                        cad = cad + Convert.ToChar(numRandom.Next(97, 123));
                        break;
                    default:
                        break;
                }
            }

            this.Session["Captcha"] = cad;
            return cad;
        }

        /// <summary>
        /// Establecer usuario autenticado de sesión
        /// </summary>
        /// <param name="userLogin">Modelo usuario</param>
        private void SetSessionAuthenticatedUser(LoginModel userLogin)
        {
            userLogin.InSession = true;
            userLogin.Conections = new ConcurrentBag<string>();
            userLogin.Id = this.securityManager.GetUserId(userLogin.UserName);//Lista de enteros, sólo trae los id de grupos
            userLogin.UserGroup = DelegateService.uniqueUserService.GetUsersGroupByUserId(userLogin.Id);
            var UniqueUserSession = new MlUser.UniqueUserSession
            {
                AccountName = userLogin.UserName,
                RegistrationDate = DateTime.Now,
                LastUpdateDate = DateTime.Now,
                ExpirationDate = DateTime.Now.AddDays(1),
                UserId = userLogin.Id,
                SessionId = Session.SessionID
            };
            UniqueUserSession uniqueUserSession = DelegateService.uniqueUserService.TryInitSession(UniqueUserSession);
            UniqueUserSession userSession = UsersSessionHelper.UsersSession?.Where(t => t != null && t.AccountName == uniqueUserSession.AccountName).FirstOrDefault();
            if (userSession != null)
            {
                UsersSessionHelper.Remove(userSession);
                UsersSessionHelper.Add(uniqueUserSession);
            }
            else
            {
                UsersSessionHelper.Add(uniqueUserSession);
            }
            this.SetAuthCookie(userLogin.UserName);
        }
        #endregion

        /// <summary>
        /// Clase interna para los tipos de documento de axa
        /// </summary>
        internal class CardType
        {
            /// <summary>
            /// Obtiene o establece el id
            /// </summary>
            public int Id { get; set; }

            /// <summary>
            /// Obtiene o establece el tipo de documento
            /// </summary>
            public string Type { get; set; }

            /// <summary>
            /// Obtiene o establece la descripcion del tipo de documento
            /// </summary>
            public string Description { get; set; }
        }
        public void UpdateDateSession()
        {
            DateTime updateDate = DateTime.Now.AddMinutes(Convert.ToInt32(DelegateService.commonService.GetKeyApplication("SessionWarning")));
            UniqueUserSession userSession = UsersSessionHelper.UsersSession.Where(t => t != null && t.UserId == SessionHelper.GetUserId(false)).FirstOrDefault();
            if (userSession != null)
            {
                UsersSessionHelper.Remove(userSession);
                userSession.ExpirationDate = updateDate;
                userSession.LastUpdateDate = updateDate;
                UsersSessionHelper.Add(userSession);
            }
            else
            {
                userSession = new UniqueUserSession();
                userSession.UserId = SessionHelper.GetUserId(false);
                userSession.AccountName = SessionHelper.GetUserName(false);
                userSession.RegistrationDate = DateTime.Now;
                userSession.ExpirationDate = updateDate;
                userSession.LastUpdateDate = updateDate;
                userSession.SessionId = Session.SessionID;
                UsersSessionHelper.Add(userSession);
            }
        }
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
        }

        /// <summary>
        /// Realiza el seteo del token de autenticacion para api de consultas - bi
        /// La idea es que la autenticacion de sise genere y guarde el token y la aplicacion de
        /// consultas verifica que si existe el token y se autentique automaticamente
        /// </summary>
        /// <param name="model"></param>
        private void SetIdentityServer(LoginModel model)
        {
            try
            {

            
                bool cookieExists = ValidateCookie(model.UserName); //HttpContext.Current.Request.Cookies["JwtToken"] != null;

                if (!cookieExists)
                {

                    var nvc = new List<KeyValuePair<string, string>>();
                    nvc.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
                    nvc.Add(new KeyValuePair<string, string>("client_id", ConfigurationManager.AppSettings["IdentityServerClientId"]));
                    nvc.Add(new KeyValuePair<string, string>("client_secret", ConfigurationManager.AppSettings["IdentityServerClientSecret"]));
                    nvc.Add(new KeyValuePair<string, string>("scope", "api"));
                    nvc.Add(new KeyValuePair<string, string>("username", model.UserName));
                    nvc.Add(new KeyValuePair<string, string>("password", model.Password));

                    HttpClient httpClient = new HttpClient();

                    var req = new HttpRequestMessage(HttpMethod.Post, ConfigurationManager.AppSettings["IdentityServerTokenEndpoint"]) { Content = new FormUrlEncodedContent(nvc) };
                    var sendRequestTask = httpClient.SendAsync(req);
                    sendRequestTask.Wait();
                    var res = sendRequestTask.Result;

                    var readContentTask = res.Content.ReadAsStringAsync();
                    readContentTask.Wait();
                    var resContent = readContentTask.Result;

                    dynamic content = JsonConvert.DeserializeObject<dynamic>(resContent);

                    var jwt = new JwtSecurityToken(content.access_token.ToString());

                    //Esto se hace en el metodo donde se invoca
                    // SetAuthCookie(model.UserName);

                    //HttpCookie authCookie = FormsAuthentication.GetAuthCookie(model.UserName, true);

                    HttpCookie JwtTokenCookie = new HttpCookie("JwtToken");
                    JwtTokenCookie.HttpOnly = false;

                    JwtTokenCookie.Value = content.access_token.ToString();
                    JwtTokenCookie.Expires = DateTime.Now.AddDays(1);


                    Response.Cookies.Add(JwtTokenCookie);


                    JwtTokenCookie = new HttpCookie("JwtTokenN");
                    JwtTokenCookie.HttpOnly = false;
                    byte[] userCookie = Encoding.UTF8.GetBytes(model.UserName);
                    byte[] userCookieEncry = MachineKey.Protect(userCookie);
                    JwtTokenCookie.Value = HttpServerUtility.UrlTokenEncode(userCookieEncry);

                    JwtTokenCookie.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(JwtTokenCookie);

                    JwtTokenCookie = new HttpCookie("JwtTokenNP");
                    JwtTokenCookie.HttpOnly = false;
                    userCookie = Encoding.UTF8.GetBytes(model.Password);
                    userCookieEncry = MachineKey.Protect(userCookie);
                    JwtTokenCookie.Value = HttpServerUtility.UrlTokenEncode(userCookieEncry);

                    JwtTokenCookie.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(JwtTokenCookie);

                }
            }
            catch (Exception ex)
            {
                
            }
        }

        private bool ValidateCookie(string userName)
        {
            bool cookieExists = Request.Cookies?["JwtTokenN"]?.Value != null;
            if (cookieExists)
            {
                string cookieValue = Request.Cookies["JwtTokenN"].Value.ToString();
                byte[] value = HttpServerUtility.UrlTokenDecode(cookieValue);
                byte[] userCookieDecry = MachineKey.Unprotect(value);
                string result = Encoding.UTF8.GetString(userCookieDecry);
                if (string.Compare(result, userName) != 0)
                {
                    return false;
                }
             
            }
            return cookieExists;
        }
        private void ClearJwtCookie()
        {
            if (Request.Cookies?["JwtTokenN"]?.Value != null)
            {
                var temp = new HttpCookie("JwtTokenN");
                temp.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(temp);
            }

            if (Request.Cookies?["JwtTokenNP"]?.Value != null)
            {
                var temp = new HttpCookie("JwtTokenNP");
                temp.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(temp);
                Response.Cookies.Add(temp);
            }

            if (Request.Cookies?["JwtToken"]?.Value != null)
            {
          
                var temp = new HttpCookie("JwtToken");
                temp.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(temp);
            }


        }
    }
}
