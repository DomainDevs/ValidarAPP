using Sistran.Company.Application.ModelServices.Enums;
using Sistran.Company.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MOS = Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    public class GuaranteeStatusRouteViewModel
    {
        /// <summary>
        /// Identificador
        /// </summary>
        public int GuaranteeStatusCd { get; set; }
        /// <summary>
        /// Identificador
        /// </summary>
        public int AssignedGuaranteeStatusCd { get; set; }
    }
}