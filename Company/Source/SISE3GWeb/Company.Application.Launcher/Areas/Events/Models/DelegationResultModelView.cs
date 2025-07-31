using Sistran.Core.Application.EventsServices.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Events.Models
{
    public class DelegationResultModelView
    {
        public int Count { set; get; }

        public string ResultId { set; get; }

        public int GroupEventId { set; get; }

        public int EventId { set; get; }

        public int DelegationId { set; get; }

        public string Description { set; get; }

        public string DescriptionErrorMessage { set; get; }

        public int? TypeCode { set; get; }

        public string EventDate { set; get; }

        public int ModuleId { get; set; }

        public int SubModuleId { get; set; }

        public string Operation2Id { get; set; }

        [Display(Name = "LabelAutorizador", ResourceType = typeof(App_GlobalResources.Language))]
        public int IdAuthorizer { get; set; }

        [Display(Name = "LabelNotificationFor", ResourceType = typeof(App_GlobalResources.Language))]
        public int IdNotifier { get; set; }

        [Display(Name = "LabelRasonRequest", ResourceType = typeof(App_GlobalResources.Language))]
        [Required]
        public string ReasonRequest { get; set; }

        public string IdTemp { get; set; }

        internal static List<DelegationResultModelView> CreateDelegationResultModelView(List<EventDelegationResult> list)
        {
            var model = new List<DelegationResultModelView>();
            foreach (var item in list)
            {
                model.Add(new DelegationResultModelView
                {
                    Count = item.Count,
                    ResultId = item.ResultId,
                    GroupEventId = item.GroupEventId,
                    EventId = item.EventId,
                    DelegationId = item.DelegationId,
                    Description = item.Description,
                    DescriptionErrorMessage = item.DescriptionErrorMessage,
                    //TypeCode              = item.TypeCode
                    EventDate = item.EventDate.ToString("dd/MM/yyyy"),
                    ModuleId = item.ModuleId,
                    SubModuleId = item.SubModuleId,
                    IdTemp = item.IdTemporal,
                    Operation2Id = item.Operation2Id

                });
            }
            return model;
        }

        internal static EventDelegationResult CreateDelegationResultModel(DelegationResultModelView item)
        {
            return new EventDelegationResult
            {
                IdAuthorizer = item.IdAuthorizer,
                IdNotifier = item.IdNotifier,
                ResultId = item.ResultId,
                GroupEventId = item.GroupEventId,
                EventId = item.EventId,
                DelegationId = item.DelegationId,
                //TypeCode              = item.TypeCode
                ModuleId = item.ModuleId,
                SubModuleId = item.SubModuleId,
                ReasonRequest = item.ReasonRequest,
                IdTemporal = item.IdTemp,
                Operation2Id = item.Operation2Id
            };
        }
    }
}