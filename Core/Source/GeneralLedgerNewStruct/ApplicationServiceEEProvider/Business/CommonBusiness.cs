using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Enums;
using System;

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.Business
{
    public class CommonBusiness
    {
        public static int GetIntParameter(GeneralLederKeys generalLederKeys)
        {
            try
            {
                return Convert.ToInt32(DelegateService.commonService.
                    GetParameterByDescription(generalLederKeys.ToString()).NumberParameter);
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public static string GetStringParameter(GeneralLederKeys generalLederKeys)
        {
            try
            {
                return Convert.ToString(DelegateService.commonService.
                    GetParameterByDescription(generalLederKeys.ToString()).NumberParameter);
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string GetTextParameter(GeneralLederKeys generalLederKeys)
        {
            try
            {
                return Convert.ToString(DelegateService.commonService.
                    GetParameterByDescription(generalLederKeys.ToString()).TextParameter);
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static decimal GetFloatParameter(GeneralLederKeys generalLederKeys)
        {
            try
            {
                return Convert.ToDecimal(DelegateService.commonService.
                    GetParameterByDescription(generalLederKeys.ToString()).AmountParameter);
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public static bool GetBooleanParameter(GeneralLederKeys generalLederKeys)
        {
            try
            {
                return Convert.ToBoolean(DelegateService.commonService.
                    GetParameterByDescription(generalLederKeys.ToString()).BoolParameter);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
