using Sistran.Core.Framework.UIF.Web.Managers;
using System;
using System.Web.Security;
using System.Configuration;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF.Web.App_GlobalResources;

namespace Sistran.Core.Framework.UIF.Web.Helpers
{
    public class SessionHelper
    {
        public static bool ExistUserInSession()
        {
            return HttpContextManager.Current.User.Identity.IsAuthenticated;
        }

        public static int GetUserId(bool thrwo = true)
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
                        if (thrwo)
                            throw new Exception("Error de Autenticación");
                        else
                            return -1;
                    }
                }
                else
                {
                    if (thrwo)
                        throw new Exception("Error de Autenticación");
                    else
                        return -1;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(Language.ErrorUserAutenthicate);
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