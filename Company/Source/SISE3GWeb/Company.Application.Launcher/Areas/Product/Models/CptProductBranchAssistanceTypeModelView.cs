using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Product.Models
{
    public class CptProductBranchAssistanceTypeModelView
    {
        /// <summary>
        /// Obtiene o establece el id del producto
        /// </summary>        public int ProductId { get; set; }

        /// <summary>
        /// Obtiene o establece el id del ramo
        /// </summary>        public int PrefixCode { get; set; }

        /// <summary>
        /// Obtiene o establece el id de la asistencia
        /// </summary>        public int Id { get; set; }
    }
}