using Sistran.Core.Application.CommonServices.EEProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;
using System.Diagnostics;
using COMMEN = Sistran.Core.Application.Common.Entities;
using COMMML = Sistran.Core.Application.CommonService.Models;

namespace Sistran.Core.Application.CommonServices.EEProvider.DAOs
{
    public class PaymentMethodDAO
    {
        public List<COMMML.PaymentMethod> GetPaymentMethods()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.PaymentMethod)));
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.Providers.GetPaymentMethods");
            return ModelAssembler.CreatePaymentMethods(businessCollection);
        }

    }
}
