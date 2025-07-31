// -----------------------------------------------------------------------
// <copyright file="SalePoint.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Oscar Camacho</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.CommonParamService.Models
{
    /// <summary>
    /// Modelo de punto de venta
    /// </summary>
    public class ParamSalePoint
    {
        /// <summary>
        /// Obtiene o establece el Id 
        /// </summary>
        public int Id { get; set; }       

        /// <summary>
        /// Obtiene o establece la descripción del punto de venta
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción corta del punto de venta
        public string SmallDescription { get; set; }

        /// <summary>
        /// Obtiene o establece el modelo de la sucursal
        /// </summary>
        public ParamBranch Branch { get; set; }

        /// <summary>
        /// Obtiene o establece el estado del punto de venta
        /// </summary>
        public bool Enabled { get; set; }
    }
}
