using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Audit.Models
{
    public class AuditChangeModelView
    {
        /// <summary>
        /// Obtiene o setea Nombre de la propiedad Modificada
        /// </summary>
        /// <value>
        ///  Nombre de la propiedad Modificada
        /// </value>
      
        public string Id { get; set; }

        /// <summary>
        ///Obtiene o setea el Valor Anterior del Objeto
        /// </summary>
        /// <value>
        ///Valor Anterior
        /// </value>
      
        public string ValueBefore { get; set; }

        /// <summary>
        /// Obtiene o setea el Valor Nuevo del Objeto
        /// </summary>
        /// <value>
        /// Valor Nuevo
        /// </value>
   
        public string ValueAfter { get; set; }
        /// <summary>
        ///  Obtiene o establece un valor que indica si esta instancia se serializa
        /// </summary>
        /// <value>
        ///true si esta instancia es serializada; de lo contrario, falso.
        /// </value>

        public bool IsSerialize { get; set; }
    }
}