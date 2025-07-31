// -----------------------------------------------------------------------
// <copyright file="FinancialPlanViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Sistran.Core.Application.ModelServices.Enums;

    /// <summary>
    /// ModelView plan financiero
    /// </summary>
    public class FinancialPlanViewModel
    {
        /// <summary>
        /// Obtiene o establece Id plan financiero
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece Id plan de pago
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "FieldRequired")]
        public int IdPaymentPlan { get; set; }

        /// <summary>
        /// Obtiene o establece Id medio de pago
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "FieldRequired")]
        public int IdPaymentMethod { get; set; }

        /// <summary>
        /// Obtiene o establece Id moneda
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "FieldRequired")]
        public int IdCurrency { get; set; }

        /// <summary>
        /// Obtiene o establece valor cuota minima
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "FieldRequired")]
        public int MinimunQuota { get; set; }

        /// <summary>
        /// Obtiene o establece Nombre componente
        /// </summary>
        public string ComponentName { get; set; }

        public List<PartialsInformationViewModel> ListComponent {get;set;}

        /// <summary>
        /// Obtiene o establece estado
        /// </summary>
        public StatusTypeService StatusTypeService { get; set; }
    }
}