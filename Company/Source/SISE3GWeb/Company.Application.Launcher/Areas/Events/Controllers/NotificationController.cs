using Sistran.Core.Application.EventsServices.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Events.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using Sistran.Core.Services.UtilitiesServices.Models;

namespace Sistran.Core.Framework.UIF.Web.Areas.Events.Controllers
{
    public class NotificationController : Controller
    {
        //#region partialView
        /// <summary>
        /// obtiene los eventos que fueron ejecutados en la pantalla
        /// </summary>
        /// <param name="IdModule">id del modulo</param>
        /// <param name="IdSubmodule">id del submodulo</param>
        /// <param name="ObjectName">nombre de la pantalla</param>
        /// <param name="IdTemp">id del temporal</param>
        /// <param name="key_1"></param>
        /// <param name="key_2"></param>
        /// <param name="key_3"></param>
        /// <param name="key_4"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetGeneratedEvents(EventsCriteria eventsCriteria)
        {
            eventsCriteria.IdUser = SessionHelper.GetUserId();

            var events = DelegateService.eventsService.GetEventsNotificationByEventsCriteria(eventsCriteria);
            List<NotificationEventModelView> model = NotificationEventModelView.CreateNofificationModelView(events);

            return Json(model.OrderBy(x => x.EnabledStop).ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// obtine la vista parcial de los eventos que se lanzaron 
        /// </summary>
        /// <param name="IdModule">id del modulo</param>
        /// <param name="IdSubmodule">id del submodulo</param>
        /// <param name="IdTemp">id del temporal</param>
        /// <param name="Type">individual,collective</param>
        /// <returns></returns>
        public ActionResult GetEventsSummary(int IdModule, int IdSubModule, string IdTemp, string Type, List<EventDelegationResult> delegationResult)
        {
            if (delegationResult == null)
            {
                delegationResult = DelegateService.eventsService.GetPolicyEventsByIdModuleIdSubModuleIdUserIdTemp(IdModule, IdSubModule, SessionHelper.GetUserId(), IdTemp);
            }
            else
            {
                this.TempData["delegationResult"] = delegationResult;
            }

            if (delegationResult.Any(x => x.IsNotification == false))
            {
                return new UifJsonResult(false, App_GlobalResources.Language.WarningRestrictiveEvents);
            }
            if (delegationResult.Count != 0)
            {
                this.ViewBag.IdModule = IdModule;
                this.ViewBag.IdSubModule = IdSubModule;
                this.ViewBag.IdTemp = IdTemp;
                this.ViewBag.idJson = IdTemp;
                this.ViewBag.Type = Type;

                return this.PartialView("~/Areas/Events/Views/Notification/EventsSummary.cshtml");
            }
            else
            {
                return new UifJsonResult(true, App_GlobalResources.Language.NoEventsLaunched);
            }
        }
        //#endregion

        //#region JsonResult

        ///// <summary>
        ///// obtine el resumen de los eventos que se lanzaron 
        ///// </summary>
        ///// <param name="IdModule">id del modulo</param>
        ///// <param name="IdSubmodule">id del submodulo</param>
        ///// <param name="IdTemp">id del temporal</param>
        ///// <returns></returns>
        public JsonResult GetEventsSForAutorize(int IdUser, int IdModule, int IdSubModule, string IdTemp, string type)
        {

            if (IdUser == 0)
            {
                IdUser = SessionHelper.GetUserId();
            }

            List<EventDelegationResult> list = this.TempData["delegationResult"] as List<EventDelegationResult>;
            if (list == null)
            {
                list = DelegateService.eventsService.GetPolicyEventsByIdModuleIdSubModuleIdUserIdTemp(IdModule, IdSubModule, IdUser, IdTemp);
            }

            List<DelegationResultModelView> model = DelegationResultModelView.CreateDelegationResultModelView(list);
            return this.Json(model, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// obtiene los usuarios correspondientes al evento 
        /// </summary>
        /// <param name="IdEvent">id evento</param>
        /// <param name="IdGroup">id grupo</param>
        /// <param name="IdDelegation">id delegacion</param>
        /// <returns></returns>
        [HttpPost]
      public JsonResult GetUsersByDelegation(int IdEvent, int IdGroup, int IdDelegation)
      {
         var IdHierarchy = DelegateService.eventsService.GetDelegationsByIdGroupIdEvent(IdGroup, IdEvent).Where(x => x.DelegationId == IdDelegation).FirstOrDefault().HierarchyId;
         var list = DelegateService.eventsService.GetDelegationUsersByIdGroupIdEventIdHierarchy(IdGroup, IdEvent, IdHierarchy).OrderBy(x => x.UserName).ToList();

         return Json(list, JsonRequestBehavior.AllowGet);
      }

      /// <summary>
      /// crea las autorizaciones de los eventos del temporal
      /// </summary>
      /// <param name="delegation">lista de los eventos a autorizar</param>
      /// <param name="IdTemp">id del temporal</param>
      /// <returns></returns>
      [HttpPost]
      public JsonResult SaveAuthorizationEvent(List<DelegationResultModelView> delegation)
      {
         var list = new List<EventDelegationResult>();
         foreach (var item in delegation)
         {
            list.Add(DelegationResultModelView.CreateDelegationResultModel(item));
         }

         DelegateService.eventsService.CreateEventAuthorizationByIdTempIduser(list, SessionHelper.GetUserId());
         Thread hiloEmail = new Thread(() => CreateEmail(list));

         hiloEmail.Start();
         return new UifJsonResult(true, App_GlobalResources.Language.AuthorizationsEventsSent);
      }

      ///// <summary>
      ///// obtiene los eventos pendientes de autorizacion para el usuario
      ///// </summary>
      ///// <returns></returns>
      //[HttpPost]
      //public ActionResult GetCountEvents()
      //{
      //    var today = DateTime.Now.ToString("dd/MM/yyyy");
      //    try
      //    {
      //        var list = DelegateService.eventsService.GetCountEventsByIdUser(SessionHelper.GetUserId());
      //        var eventsCount = new CountEventModelView();

      //        eventsCount.EventsLowerDay =
      //            list.Where(x => DateTime.ParseExact(x.StrEventDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) >= DateTime.ParseExact(today, "dd/MM/yyyy", CultureInfo.InvariantCulture).AddDays(-1)).Count();
      //        eventsCount.EventsBetweenDay =
      //            list.Where(x => DateTime.ParseExact(x.StrEventDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) >= DateTime.ParseExact(today, "dd/MM/yyyy", CultureInfo.InvariantCulture).AddDays(-4)).Count() - eventsCount.EventsLowerDay;
      //        eventsCount.EventHigherDay =
      //            list.Where(x => DateTime.ParseExact(x.StrEventDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) < DateTime.ParseExact(today, "dd/MM/yyyy", CultureInfo.InvariantCulture).AddDays(-4)).Count();

      //        return new UifJsonResult(true, eventsCount);
      //    }
      //    catch (Exception)
      //    {

      //        return new UifJsonResult(false, App_GlobalResources.Language.ServiceError);
      //    }

      //}
      //#endregion

      /// <summary>
      /// crea lso diferenes correos electronicos a enviar, y realiza el envio
      /// </summary>
      /// <param name="list">lsita de eventos </param>
      /// <param name="IdTemp">id del temporal</param>
      private void CreateEmail(List<EventDelegationResult> list)
      {
         List<EmailCriteriaModelView> ListEmail = new List<EmailCriteriaModelView>();

         foreach (EventDelegationResult item in list)
         {
            var EmailAuth = ListEmail.Where(x => x.userId == item.IdAuthorizer.ToString() && x.Type == "auth").FirstOrDefault();
            var EmailNotif = ListEmail.Where(x => x.userId == item.IdNotifier.ToString() && x.Type == "notif").FirstOrDefault();

            var evento = DelegateService.eventsService.GetEventByIdEventGroupIdEvent(item.GroupEventId, item.EventId);
            var content = App_GlobalResources.Language.ContentEmailEvent;

            content = content.Replace("@@EVENT_NAME", evento.Description + " (" + item.ResultId.Split('-').Count() + ")");
            content = content.Replace("@@EVENT_ERROR", evento.DescriptionErrorMessage);
            content = content.Replace("@@EVENT_MESSAGE", item.ReasonRequest);

            if (EmailAuth == null)
            {
               var address = DelegateService.eventsService.GetEmailByIdUser(item.IdAuthorizer);
               if (address != "")
               {
                  var header = App_GlobalResources.Language.HeaderEmailEvent;

                  EmailCriteriaModelView email = new EmailCriteriaModelView();
                  email.Type = "auth";
                  email.userId = item.IdAuthorizer.ToString();
                  email.count = item.ResultId.Split('-').Count();
                  email.Subject = App_GlobalResources.Language.SubjectAutorization + " " + item.IdTemporal;
                  email.Addressed = new List<string> { address };

                  header = header.Replace("@@HEADER_MAIL", email.Subject);
                  header = header.Replace("@@USER_NAME", User.Identity.Name);
                  header = header.Replace("@@DATE", DateTime.Now.ToString("dd/MM/yyyy"));

                  email.Message = header + content;

                  ListEmail.Add(email);
               }
            }
            else
            {
               ListEmail.Remove(EmailAuth);
               EmailAuth.count += item.ResultId.Split('-').Count();
               EmailAuth.Message += content;
               ListEmail.Add(EmailAuth);
            }

            if (EmailNotif == null)
            {
               var address = DelegateService.eventsService.GetEmailByIdUser(item.IdNotifier);
               if (address != "")
               {
                  var header = App_GlobalResources.Language.HeaderEmailEvent;

                  EmailCriteriaModelView email = new EmailCriteriaModelView();
                  email.Type = "notif";
                  email.userId = item.IdNotifier.ToString();
                  email.count = item.ResultId.Split('-').Count();
                  email.Subject = App_GlobalResources.Language.SubjectNotification + " " + item.IdTemporal;
                  email.Addressed = new List<string> { address };

                  header = header.Replace("@@HEADER_MAIL", email.Subject);
                  header = header.Replace("@@USER_NAME", User.Identity.Name);
                  header = header.Replace("@@DATE", DateTime.Now.ToString("dd/MM/yyyy"));

                  email.Message = header + content;

                  ListEmail.Add(email);
               }
            }
            else
            {
               ListEmail.Remove(EmailNotif);
               EmailNotif.count += item.ResultId.Split('-').Count();
               EmailNotif.Message += content;
               ListEmail.Add(EmailNotif);
            }
         }

         foreach (var email in ListEmail)
         {
            EmailCriteria emailSend = new EmailCriteria();
            emailSend.Subject = email.Subject;
            emailSend.Message = email.Message.Replace("@@COUNT_EVENT", email.count.ToString());
            emailSend.Addressed = email.Addressed;
            DelegateService.AuthorizationPoliciesService.SendEmail(emailSend);
         }
      }
   }
}