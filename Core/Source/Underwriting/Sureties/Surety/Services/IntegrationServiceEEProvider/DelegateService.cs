using Sistran.Core.Framework.SAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Core.Application.Sureties.SuretyServices;
using Sistran.Core.Application.Sureties.JudicialSuretyServices;
namespace Sistran.Core.Integration.SuretyServices.EEProvider
{
    public class DelegateService
    {
        internal static ISuretyServiceCore suretyService = ServiceProvider.Instance.getServiceManager().GetService<ISuretyServiceCore>();
        internal static IJudicialSuretyCore judicialSuretyService = ServiceProvider.Instance.getServiceManager().GetService<IJudicialSuretyCore>();

    }
}
