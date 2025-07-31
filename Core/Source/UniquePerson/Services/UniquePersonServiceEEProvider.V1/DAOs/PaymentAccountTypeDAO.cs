using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sistran.Core.Application.UniquePersonService.V1.DAOs
{
    /// <summary>
    /// Tipos de Cuenta
    /// </summary>
    public class PaymentAccountTypeDAO
    {
        /// <summary>
        /// Gets the payment types.
        /// </summary>
        /// <returns></returns>
        public List<Models.PaymentAccountType> GetPaymentTypes()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(PaymentAccountType)));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetPaymentTypes");
            return ModelAssembler.CreateAccountTypes(businessCollection);
        }
    }
}
