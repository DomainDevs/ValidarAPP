using Sistran.Core.Application.EventsServices.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Events.Models
{
    public class NotificationEventModelView
    {
        [Key]
        public int ResultId { set; get; }
        public int Count { get; set; }
        public int RecordId { set; get; }
        public int EventId { set; get; }
        public string DescriptionError { set; get; }
        public bool EnabledStop { set; get; }

        internal static List<NotificationEventModelView> CreateNofificationModelView(List<EventNotification> events)
        {
            var model = new List<NotificationEventModelView>();

            foreach (var item in events)
            {
                model.Add(new NotificationEventModelView
                {
                    Count = item.Count,
                    EventId = item.EventId,
                    RecordId = item.RecordId,
                    ResultId = item.ResultId,
                    DescriptionError = item.DescriptionError,
                    EnabledStop = item.EnabledStop
                });
            }

            return model.OrderByDescending(x => x.EnabledStop).ToList();
        }
    }
}