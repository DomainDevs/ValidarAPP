using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Person.Models
{
    public class EmailViewModel
    {

        /// <summary>
        /// Obtiene o setea el Id del telefono
        /// </summary>
        /// <value>
        /// Identificador
        /// </value>
        public int Id { get; set; }


        /// <summary>
        /// obtiene o setea la descripcion telefono
        /// </summary>
        /// <value>
        /// descripcion telefono
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// obtiene o setea tipo telefono
        /// </summary>
        /// <value>
        /// tipo telefono
        /// </value>
        public int EmailTypeId { get; set; }

        /// <summary>
        /// obtiene o setea descripcion tipo direccion
        /// </summary>
        /// <value>
        /// The address type description.
        /// </value>
        public string EmailTypeDescription { get; set; }

    }
}