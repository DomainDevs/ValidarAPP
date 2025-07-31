using Sistran.Core.Application.AccountingServices.EEProvider.Enums;
using System;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Business
{
    public class CommonBusiness
    {
        public static int GetIntParameter(AccountingKeys accountingKeys)
        {
            try
            {
                return Convert.ToInt32(DelegateService.commonService.
                    GetParameterByDescription(accountingKeys.ToString()).NumberParameter);
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public static string GetStringParameter(AccountingKeys accountingKeys)
        {
            try
            {
                return Convert.ToString(DelegateService.commonService.
                    GetParameterByDescription(accountingKeys.ToString()).NumberParameter);
            }
            catch (Exception)
            {
                return "";
            }
        }
        
        public static string GetTextParameter(AccountingKeys accountingKeys)
        {
            try
            {
                return Convert.ToString(DelegateService.commonService.
                    GetParameterByDescription(accountingKeys.ToString()).TextParameter);
            }
            catch (Exception)
            {
                return "";
            }
        }
        
        public static decimal GetFloatParameter(AccountingKeys accountingKeys)
        {
            try
            {
                return Convert.ToDecimal(DelegateService.commonService.
                    GetParameterByDescription(accountingKeys.ToString()).AmountParameter);
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public static bool GetBooleanParameter(AccountingKeys accountingKeys)
        {
            try
            {
                return Convert.ToBoolean(DelegateService.commonService.
                    GetParameterByDescription(accountingKeys.ToString()).BoolParameter);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
