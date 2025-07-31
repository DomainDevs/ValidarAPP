using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Sistran.Core.Framework.UIF.Web.Helpers;
using System;
using System.Web;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Models
{
    public class UifJsonData : JsonResult
    {
        /// <summary>
        /// Create a new instance of Json Result
        /// </summary>
        /// <param name="data">Data Source</param>
        public UifJsonData(bool success, object data)
        {
            this.Data = new { success = success, result = data };
            this.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            this.MaxJsonLength = Int32.MaxValue;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            HttpResponseBase response = context.HttpContext.Response;

            if (!String.IsNullOrEmpty(ContentType))
            {
                response.ContentType = ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }
            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }
            if (Data != null)
            {
                // Using Json.NET serializer
                var isoConvert = new IsoDateTimeConverter();
                isoConvert.DateTimeFormat = DateHelper.FormatDate;
                response.Write(JsonConvert.SerializeObject(Data, isoConvert));
            }
        }
    }

}