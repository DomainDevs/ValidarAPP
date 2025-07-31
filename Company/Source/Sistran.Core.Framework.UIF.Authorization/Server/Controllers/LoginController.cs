using Sistran.Core.Framework.UIF.Authorization.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using Sistran.Core.Framework.UIF.Authorization.Server.Enum;
using Sistran.Core.Application.SecurityServices.Models;
using Sistran.Core.Framework.UIF.Authorization.Server.Services;
using System.Threading;
using Sistran.Core.Framework.UIF.Authorization.Server.Security;

namespace Sistran.Core.Framework.UIF.Authorization.Server.Controllers
{
    [SistranAuthorizationFilter]
    public class LoginController : ApiController
    {

        /// <summary>
        /// Obtiene o establece la sesion del usuario
        /// </summary>    
        //public static List<LoginModel> UsersSession
        //{
        //    get
        //    {
        //        if (usersSession == null)
        //        {
        //            usersSession = new List<LoginModel>();
        //        }

        //        return usersSession;
        //    }

        //    set
        //    {
        //        usersSession = value;
        //    }
        //}

        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "Respuesta del Back" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "Respuesta con parametro";
        }

        // POST api/values
        public IEnumerable<string> Post([FromBody]LoginModel value)
        {

            try
            {
                AuthenticationResult result = DelegateService.AuthenticationProviders.AutenthicateR2(value.UserName, value.Password, null, "42a5ozsfv2v3231fed0wz1mx");

                if (result != null)
                {

                    if (ModelState.IsValid && result.isAuthenticated)
                    {

                        return new string[] { "Usurio existente" };
                    }

                    switch ((int)result.Result)
                    {
                        case (int)AuthenticationEnum.isInvalidPassword:
                            this.ModelState.AddModelError(string.Empty, Resources.Language.UserOrPasswordFailed);
                            break;
                        case (int)AuthenticationEnum.isInvalidPasswordWithData:
                            this.ModelState.AddModelError(string.Empty, string.Format(Resources.Language.PasswordLockWarning, result.data[0], result.data[1]));
                            break;
                        case (int)AuthenticationEnum.isUserBlocked:
                            this.ModelState.AddModelError(string.Empty, Resources.Language.UserBlocledContact);
                            break;
                        case (int)AuthenticationEnum.isUserBlockedWithTime:
                            this.ModelState.AddModelError(string.Empty, string.Format(Resources.Language.PasswordLocked, result.data[0]));
                            break;
                        case (int)AuthenticationEnum.IsUserExpired:
                            this.ModelState.AddModelError(string.Empty, string.Format(Resources.Language.UserError));
                            break;
                        case (int)AuthenticationEnum.isPasswordExpired:
                            this.ModelState.AddModelError(string.Empty, "Su clave ha expirado");
                            break;
                        default:
                            break;
                    }
                }

                return new string[] { ModelState.Values.ToArray()[0].Errors[0].ErrorMessage };
            }
            catch (Exception ex) {
                return new string[] { "error" };
            }
        }
    }
}
