using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Sistran.Core.Framework.UIF.Web.Models
{
    public class UifJsonResult : JsonResult
    {
        /// <summary>
        /// Create a new instance of Json Result
        /// </summary>
        /// <param name="data">Data Source</param>
        public UifJsonResult(bool success, object data)
        {
            this.Data = new { success = success, result = data };
            this.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        }
    }
}