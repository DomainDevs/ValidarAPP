using Sistran.Core.Framework.UIF.Web.Models;

namespace Sistran.Core.Framework.UIF.Web.Assemblers
{
    public class ModelAssembler
    {
        internal static SessionModel CreateSessionModel(LoginModel loginModel)
        {
            return new SessionModel
            {
                UserName = loginModel.UserName
            };
        }
    }
}