using Sistran.Core.Application.AccountingServices.DTOs.Imputations;
using Sistran.Core.Application.AccountingServices.Enums;
using Sistran.Core.Application.WrapperAccountingServiceEEProvide;
using System;

namespace Sistran.Core.Application.WrapperAccountingServiceEEProvider.DAOs
{
    public class ApplicationDAO
    {
        /// <summary>
        /// Creates the temporary application.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="accountingDate">The accounting date.</param>
        /// <returns></returns>
        internal static int CreateTempApplication(int userId,DateTime accountingDate)
        {
            ApplicationDTO application = new ApplicationDTO();
            application.Id = 0;
            application.IsTemporal = true;
            application.RegisterDate = DateTime.Now;
            application.ModuleId = (int)ApplicationTypes.Collect;
            application.UserId = userId;
            application.AccountingDate = accountingDate;
            application.SourceId = 0;
            // Graba la cabecera de imputación
            application = DelegateService.accountingApplicationService.SaveTempApplication(application, 0);
            return application.Id;
        }
    }
}
