using Sistran.Core.Framework.UIF2.Controls.UifTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web
{
    public class BindersConfig
    {
        public static void Register()
        {
            ModelBinders.Binders.Add(typeof(UifTableParam), new UifTableParamBinder());
        }
    }
}