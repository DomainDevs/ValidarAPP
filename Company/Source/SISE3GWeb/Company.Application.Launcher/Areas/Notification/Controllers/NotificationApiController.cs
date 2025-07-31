using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Framework.UIF.Web.Hubs;
using System.Web.Http;
using System.Web.Http.Description;

namespace Sistran.Core.Framework.UIF.Web.Areas.Notification.Controllers
{
    public class NotificationApiController : ApiController
    {
        [HttpPost]
        [ResponseType(typeof(string))]
        public IHttpActionResult Post([FromBody] NotificationUser notification)
        {
            if (ModelState.IsValid && notification != null)
            {
                NotificationHub.SetStaticNotification(notification);
                return Ok();
            }
            return BadRequest();
        }
    }
}