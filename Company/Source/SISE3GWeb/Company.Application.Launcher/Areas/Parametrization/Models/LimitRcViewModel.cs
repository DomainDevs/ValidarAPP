// -----------------------------------------------------------------------
// <copyright file="LimitRcModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Jeison Rodriguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    using Sistran.Core.Application.EntityServices.Enums;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    /// <summary>
    /// Clase con el modelo (MVC) de limite rc
    /// </summary>
    public class LimitRcViewModel
    {
        /// <summary>
        /// Obtiene o establece codigo del limite rc
        /// </summary>
        public int LimitRcCd { get; set; }

        /// <summary>
        /// Obtiene o establece limite uno
        /// </summary>
        [Display(Name = "Limit1", ResourceType = typeof(App_GlobalResources.Language))]
        [DataMember]
        public Decimal Limit1 { get; set; }

        /// <summary>
        /// Obtiene o establece limite dos
        /// </summary>
        [Display(Name = "Limit2", ResourceType = typeof(App_GlobalResources.Language))]
        [DataMember]
        public Decimal Limit2 { get; set; }

        /// <summary>
        /// Obtiene o establece limite tres
        /// </summary>
        [DataMember]
        public Decimal Limit3 { get; set; }

        /// <summary>
        /// Obtiene o establece limite unico
        /// </summary>
        [Display(Name = "LimitUnique", ResourceType = typeof(App_GlobalResources.Language))]
        [DataMember]
        public Decimal LimitUnique { get; set; }

        /// <summary>
        /// Obtiene o establece descripcion de limite
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///  Obtiene o establece el Estado de item - (CRUD)
        /// </summary>
        public StatusTypeService StatusTypeService { get; set; }

    }
}