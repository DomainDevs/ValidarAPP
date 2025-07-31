using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.UniqueUser.Models
{
    public class UserProductModelsView
    {
        /// <summary>
        /// Identificador del usuario
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Identificador del producto
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Descripción del producto
        /// </summary>
        public string ProductDescription { get; set; }

        /// <summary>
        /// Identificador del ramo
        /// </summary>
        public int PrefixCode { get; set; }

        /// <summary>
        /// Producto habilitado
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Producto Asignado
        /// </summary>
        public bool Assign { get; set; }

    }
}