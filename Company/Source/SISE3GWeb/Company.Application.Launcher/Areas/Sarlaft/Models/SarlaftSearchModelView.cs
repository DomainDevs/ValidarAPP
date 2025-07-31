using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Sarlaft.Models
{
    public class SarlaftSearchModelView
    {
        /// <summary>
        /// Numero documento
        /// </summary>
        public int DocumentNumber { get; set; }

        /// <summary>
        /// Tipo documento
        /// </summary>
        public int DocumentTypeId { get; set; }
        /// <summary>
        /// Descripcion de Actividad economica
        /// </summary>
        public int EconomicActivityDesc { get; set; }
        /// <summary>
        /// Tipo de actividad economica
        /// </summary>
        public int EconomicActivityId { get; set; }
        /// <summary>
        /// Individual ID
        /// </summary>
        public int IndividualId { get; set; }
        /// <summary>
        /// Ramo comercial
        /// </summary>
        public int Name { get; set; }
        /// <summary>
        /// Ramo comercial
        /// </summary>
        public int PersonType { get; set; }
    }
}