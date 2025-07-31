using Sistran.Co.Application.Data;
using Sistran.Core.Application.AccountingServices.DTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs.Accounting
{
    internal class ReversionDAO
    {

        /// <summary>
        /// Converts the tempto real premium reversion.
        /// </summary>
        /// <param name="AppId">The application identifier.</param>
        /// <param name="ids">The ids.</param>
        /// <returns></returns>
        internal static void ConvertTemptoRealPremiumReversion(int AppId, List<int> ids)
        {
            ReversionFilterDTO reversionFilterDTO = null;
            foreach (int value in ids)
            {
                reversionFilterDTO = new ReversionFilterDTO
                {
                    AppId = AppId,
                    Id = value
                };
                ReversionPremium(reversionFilterDTO);
            }
        }
        /// <summary>
        /// Reversion primas Aplicadas
        /// </summary>
        /// <param name="reversionFilterDTO">The reversion filter dto.</param>
        /// <returns></returns>
        internal static bool ReversionPremium(ReversionFilterDTO reversionFilterDTO)
        {
            var parameters = new NameValue[2];

            parameters[0] = new NameValue("APP_ID", reversionFilterDTO.AppId);
            parameters[1] = new NameValue("APP_PREMIUM_REVERSE", reversionFilterDTO.Id);

            DataTable result;
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("ACC.REVERSE_APPLICATION_PREMIUM", parameters);
            }

            if (result != null && result.Rows.Count > 0)
            {
                DataRow dataRow = result.Rows.Cast<DataRow>().FirstOrDefault();
                return Convert.ToBoolean(dataRow[0]);
            }
            else
            {
                return false;
            }
        }

        internal static bool ReversionTempPremium(ReversionFilterDTO reversionFilterDTO)
        {
            var parameters = new NameValue[2];

            parameters[0] = new NameValue("TEMP_ID", reversionFilterDTO.Id);
            parameters[1] = new NameValue("APP_PREMIUM_ID", reversionFilterDTO.AppId);

            DataTable result;
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("ACC.TEMP_APP_REVERSION", parameters);
            }

            if (result != null && result.Rows.Count > 0)
            {
                DataRow dataRow = result.Rows.Cast<DataRow>().FirstOrDefault();
                return Convert.ToBoolean(dataRow[0]);
            }
            else
            {
                return false;
            }
        }
    }
}
