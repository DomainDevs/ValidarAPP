using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.UniqueUser.Models
{
    public class ProfileAccessView
    {
        ///// <summary>
        ///// Id profile
        ///// </summary>
        //public int Id { get; set; }

        /// <summary>
        /// Id del acceso
        /// </summary>
        public int AccessId { get; set; }

        /// <summary>
        /// Si está asignado o no
        /// </summary>
        public bool Assigned { get; set; }

        /// <summary>
        /// Tipo
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Descripción
        /// </summary>
        [MaxLength(30)]
        public string Description { get; set; }

        public string Status { get; set; }


        /// <summary>
        /// Id del objeto a modificar  para opcion permisos (profileaccesspermissions)
        /// </summary>
        public int AccessObjectId { get; set; }
    }
}