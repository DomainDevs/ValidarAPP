using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Models
{
    public class ErrorInfoModel
    {
        public HandleErrorInfo HandleErrorInfo { get; set; }
        public string Url { get; set; }
    }
}