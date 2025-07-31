using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    public class PrvCoverageViewModel
    {
        /// <summary>
        /// Obtiene o establece Id de Cobertura
        /// </summary>
        public int CoverageId { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica número de cobertura
        /// </summary>
        public int CoverageNum { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si es postcontractual
        /// </summary>
        public bool IsPost { get; set; }

        /// <summary>
        /// Obtiene o establece fecha postcontractual
        /// </summary>
        public DateTime? BeginDate { get; set; }
    }
}