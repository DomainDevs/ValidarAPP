// -----------------------------------------------------------------------
// <copyright file="AlliedSalePointsModelsView.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Eduardo Rodríguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Areas.UniqueUser.Models
{
    using System.ComponentModel.DataAnnotations;
    /// <summary>
    /// Modelo de vista para puntos de venta por sucursal de aliado.
    /// </summary>
    public class AlliedSalePointsModelsView
    {
        /// <summary>
        /// Obtiene o establece el identificador del aliado.
        /// </summary>        
        public int AllianceId { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador de la sucursal del aliado.
        /// </summary>        
        public int BranchAllianceId { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador del punto de venta del aliado.
        /// </summary>        
        public int SalePointAllianceId { get; set; }

        /// <summary>
        /// Obtiene o establece el individual id del agente.
        /// </summary>             
        public int IndividualId { get; set; }

        /// <summary>
        /// Obtiene o establece el id del agente.
        /// </summary>        
        public int AgentAgencyId { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si esta o no asignado el punto de venta.
        /// </summary>        
        public bool IsAssign { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre del punto de venta.
        /// </summary>        
        public string SalePointDescription { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre de la sucursal.
        /// </summary>        
        public string BranchDescription { get; set; }

    }
}