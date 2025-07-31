using Sistran.Co.Previsora.Application.FullServices;
using Sistran.Core.Framework.UIF2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OperatingQuoteWebServices.Delegate
{
    public class DelegateService
    {

            
        internal static IFullServicesSUP fullServicesSupProvider = ServiceManager.Instance.GetService<IFullServicesSUP>();

    }
}