using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CPEM = Sistran.Core.Application.UniquePersonService.V1.Models;

namespace Sistran.Core.Framework.UIF.Web.Areas.QuotaOperational.Models
{
    public class ModelAssembler
    {
        public static CPEM.OperatingQuota MappQuotaOperation(QuotaOperationalModelView quotaOperationalModelView)
        {
            return new CPEM.OperatingQuota()
            {
                CurrencyId = quotaOperationalModelView.CurrencyId,
                Amount = quotaOperationalModelView.amountValue,                
                IndividualId = quotaOperationalModelView.individualId,
                LineBusinessId = quotaOperationalModelView.lineBusinessId,                
                CurrentTo = quotaOperationalModelView.DateEnd
            };
        }
    }
}