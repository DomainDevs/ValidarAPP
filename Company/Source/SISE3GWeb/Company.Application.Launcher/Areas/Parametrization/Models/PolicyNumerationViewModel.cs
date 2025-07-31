// -----------------------------------------------------------------------
// <copyright file="PolicyNumerationViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Oscar Camacho</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    using Application.EntityServices.Enums;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo de vista para numeración de póliza
    /// </summary>
    public class PolicyNumerationViewModel
    {
        /// <summary>
        /// Obtiene o establece el Id de la numeración
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece el Id del ramo
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [DataMember]
        public int PrefixId { get; set; }

        /// <summary>
        /// Obtiene o establecela descripción del ramo
        /// </summary>
        public string PrefixDescription { get; set; }

        /// <summary>
        /// Obtiene o establece el número de la última póliza
        /// </summary>      
        [Display(Name = "LastPolicy", ResourceType =typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorPolicyNumberRequired")]
        [RegularExpression(@"^[0-9]+$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]        
        public decimal LastPolicy { get; set; }

        /// <summary>
        /// Obtiene o establece el Id de la sucursal
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [DataMember]
        public int BranchId { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción de la sucursal
        /// </summary>
        public string BranchDescription { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha de la póliza
        /// </summary>
        [Required]
        [StringLength(10, MinimumLength = 9)]
        [Display(Name = "DueDateTo", ResourceType = typeof(App_GlobalResources.Language))]
        public string DueDateTo { get; set; }

        public bool HasPolicy { get; set; }

        /// <summary>
        ///  Obtiene o establece el Estado de item - (CRUD)
        /// </summary>
        public StatusTypeService StatusTypeService { get; set; }
    }
}