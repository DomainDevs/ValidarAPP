using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.WrapperAccountingService.Enums;
using Sistran.Core.Application.WrapperAccountingServiceEEProvide;
using System;

namespace Sistran.Core.Application.WrapperAccountingServiceEEProvider.DAOs
{
    public class ParametersDAO
    {
        public static int GetBillNumber()
        {
            int BillNumber = (int)DelegateService.commonService.GetParameterByParameterId(Convert.ToInt32(AccountingKey.BillNumber)).NumberParameter;
            DelegateService.commonService.UpdateParameters(new Parameter { Id = Convert.ToInt32(AccountingKey.BillNumber), NumberParameter = BillNumber + 1 });
            return BillNumber;
        }

    }
}
