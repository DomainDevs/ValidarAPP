using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PAREN = Sistran.Core.Application.Parameters.Entities;
using QUOEN = Sistran.Core.Application.Quotation.Entities;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider.ModelsMapper
{
    public class CompanySurchargeComponentMapper
    {
        public QUOEN.Component component { get; set; }
        public QUOEN.SurchargeComponent surchargeComponent { get; set; }
        public PAREN.RateType rateType { get; set; }
    }
}
