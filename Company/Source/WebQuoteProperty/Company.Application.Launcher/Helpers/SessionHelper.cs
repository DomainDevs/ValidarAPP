using Sistran.Core.Framework.UIF.Web.Managers;
using System;
using System.Web.Security;
using System.Configuration;
using Sistran.Core.Framework.UIF.Web.Services;

namespace Sistran.Core.Framework.UIF.Web.Helpers
{
    public class SessionHelper
    {
        public static bool ExistUserInSession()
        {
            return HttpContextManager.Current.User.Identity.IsAuthenticated;
        }

        public static int GetUserId()
        {
            try
            {
                if (HttpContextManager.Current != null && HttpContextManager.Current.User != null && HttpContextManager.Current.User.Identity is FormsIdentity)
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
                else if (Convert.ToBoolean(DelegateService.commonService.GetKeyApplication("UnitTest")))
                {
                    return Convert.ToInt32(DelegateService.commonService.GetKeyApplication("UnitTestUserId"));
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

        public static string GetUserName()
        {

            try
            {
                if (HttpContextManager.Current != null && HttpContextManager.Current.User != null && HttpContextManager.Current.User.Identity is FormsIdentity)
                {
                    FormsAuthenticationTicket ticket = ((FormsIdentity)HttpContextManager.Current.User.Identity).Ticket;
                    if (ticket != null)
                    {
                        return ticket.Name.ToUpper();
                    }
                    else
                    {
                        throw new Exception("Error de Autenticación");
                    }
                }
                else if (Convert.ToBoolean(DelegateService.commonService.GetKeyApplication("UnitTest")))
                {
                    return DelegateService.commonService.GetKeyApplication("UnitTestUserName");
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
    }
}