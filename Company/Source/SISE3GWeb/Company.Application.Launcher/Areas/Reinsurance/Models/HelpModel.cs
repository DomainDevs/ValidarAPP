using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Reinsurance.Models
{
    public class HelpModel
    {
        /// <summary>
        /// Identificativo único de la lista de posiblbes opciones de búsqueda de ayuda
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Texto por el que se hará la búsqueda
        /// </summary>
        public string HelpDescription { get; set; }

        /// <summary>
        /// Nombre del método de que levanta la vista solicitada
        /// </summary>
        public string Url { get; set; }
         
    }
}