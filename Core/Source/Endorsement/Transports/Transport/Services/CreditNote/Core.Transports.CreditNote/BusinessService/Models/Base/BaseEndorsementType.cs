using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Core.Application.Extensions;

namespace Sistran.Core.Application.Transports.Endorsement.CreditNote.BusinessServices.Models.Base
{
    public class BaseEndorsementType: BaseGeneric
    {

        /// <summary>
        /// Hace cálculo
        /// </summary>
        public bool HasQuotation { get; set; }
    }
}
