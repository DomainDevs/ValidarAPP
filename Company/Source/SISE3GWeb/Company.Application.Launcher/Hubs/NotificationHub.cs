using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Sistran.Core.Application.UniqueUserServices.Enums;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Framework.UIF.Web.Controllers;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Services;

namespace Sistran.Core.Framework.UIF.Web.Hubs
{
    public class NotificationHub : Hub
    {
        internal static void SetStaticNotification(NotificationUser notification)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            var user = UsersSessionHelper.UsersSession.FirstOrDefault(x => x.UserId == notification.UserId);
            if (user != null)
            {
                //Si el enum es de tipo WorkFlow
                //consulte la cantidad por usuario => count
                //context.Clients.Group(user.UserName).NotificateWF(count);
                 context.Clients.Group(user.AccountName).Notificate(notification);
            }
        }

        public void SetNotification(NotificationUser notification)
        {
            DelegateService.uniqueUserService.CreateNotification(notification);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            return Groups.Remove(Context.ConnectionId, Context.User.Identity.Name);
        }

        public override Task OnConnected()
        {
            return Groups.Add(Context.ConnectionId, Context.User.Identity.Name);
        }

        public override Task OnReconnected()
        {
            return Groups.Add(Context.ConnectionId, Context.User.Identity.Name);
        }
    }
}