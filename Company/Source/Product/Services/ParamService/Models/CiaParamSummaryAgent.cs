// -----------------------------------------------------------------------
// <copyright file="CiaParamSummaryAgent.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres Gonzalez</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.ProductParamService.Models
{
    /// <summary>
    /// Clase resumen de intermediario 
    /// </summary>
    public class CiaParamSummaryAgent
    {
        /// <summary>
        /// Intermediarios Asignados
        /// </summary>
        public int AssignedAgents { get; set; }

        /// <summary>
        /// Intermediarios Sin Asignar
        /// </summary>
        public int UnassignedAgents { get; set; }

        /// <summary>
        /// Agencias con comisión diferencial 
        /// </summary>
        public int AgentsCommission { get; set; }

        /// <summary>
        /// Agencias con Incentivos por intermediario
        /// </summary>
        public int AgentsIncentives { get; set; }
    }
}
