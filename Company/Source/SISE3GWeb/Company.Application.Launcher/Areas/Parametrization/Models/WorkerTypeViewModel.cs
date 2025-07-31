using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

// -----------------------------------------------------------------------
// <copyright file="InfringementViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Lina Maria Quintero</author>
// --

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{

    using Sistran.Core.Application.ModelServices.Enums;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class WorkerTypeViewModel
    {
        [Display(Name = "LabelWorkerTypeDescription", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(50, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorMaxLengthCharacter")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string Description { get; set; }

        [Display(Name = "LabelWorkerTypeSmallDescription", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(15, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorMaxLengthCharacter")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string SmallDescription { get; set; }

        public bool IsEnabled { get; set; }

        public int Id { get; set; }

        public StatusTypeService StatusTypeService { get; set; }
    }
}