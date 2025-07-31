using System.Collections.Generic;

namespace Sistran.Core.Application.UIF2.Providers
{
    public class SISE3GAuthorizationProvider : IAuthorizationProvider
    {
        SISEServiceDelegateCommon serviceDelegate = new SISEServiceDelegateCommon();

        public IList<ControlSecurity> GetControlsSecurity(string viewID, string userName)
        {
            int auxUserId = UserHelper.GetUserIdLogOn(userName);
            User usrAccess = serviceDelegate.GetUserAccess(auxUserId, viewID);

            List<ControlSecurity> controlList = (from usr in usrAccess.AccessObjects
                                                select new ControlSecurity
                                                {
                                                    ControlID = usr.Url,
                                                    Enabled = usr.Enable,
                                                    Visible = usr.Visible
                                                }).ToList();

            return controlList;
        }

        public IList<Module> GetModules(string userName)
        {
            return new List<Module>();
        }
      

    }
}
