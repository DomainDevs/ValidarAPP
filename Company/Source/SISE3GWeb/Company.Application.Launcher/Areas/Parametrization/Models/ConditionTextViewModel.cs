
namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    using Sistran.Company.Application.Utilities.DTO;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    public class ConditionTextViewModel
    {
        /// <summary>
        /// Get or sets Id.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorfieldState")]
        public int Id { get; set; }

        /// <summary>
        /// Get or sets Title.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorfieldState")]
        public String Title { get; set; }

        /// <summary>
        /// Get or sets Body.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorfieldState")]
        public String Body { get; set; }

        /// <summary>
        /// Get or sets Objeto ConditionTextLevel.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorfieldState")]
        public ConditionTextLevelViewModel ConditionTextLevel { get; set; }

        /// <summary>
        /// Get or sets Objeto ConditionTextLevel.
        /// </summary>
        public ConditionTextLevelTypeViewModel ConditionTextLevelType { get; set; }

        /// <summary>
        /// Get or sets Objeto Error DTO.
        /// </summary>
        public ErrorDTO ErrorDto { get; set; }
        public class ConditionTextLevelViewModel
        {
            /// <summary>
            /// Get or sets Id.
            /// </summary>
            [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorfieldState")]
            public int Id { get; set; }
            /// <summary>
            /// Get or sets Id.
            /// </summary>
            [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorfieldState")]
            public string Description { get; set; }
        }
        public class ConditionTextLevelTypeViewModel
        {
            /// <summary>
            /// Get or sets Id.
            /// </summary>
            [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorfieldState")]
            public int Id { get; set; }
            /// <summary>
            /// Get or sets Id.
            /// </summary>
            [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorfieldState")]
            public string Description { get; set; }
        }
    }
}