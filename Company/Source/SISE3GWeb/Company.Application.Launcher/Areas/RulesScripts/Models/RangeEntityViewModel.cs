using System;
using System.Collections.Generic;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.RulesScriptsServices.Enums;
using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.RulesScripts.Models
{
    using Application.ModelServices.Enums;

    public class RangeEntityViewModel
    {
        public int RangeEntityCode { get; set; }

        [Display(Name = "Description", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "FieldRequired")]
        [StringLength(50)]
        public string Description { get; set; }

        public int RangeValueAt { get; set; }

        public List<RangeEntityValue> RangeEntityValue { get; set; }

        public StatusTypeService StatusTypeService { get; set; }

        public static RangeEntityViewModel RangeEntityToViewModel(RangeEntity rangeEntity)
        {
            return new RangeEntityViewModel
            {
                RangeEntityCode = rangeEntity.RangeEntityCode,
                Description = rangeEntity.Description,
                RangeValueAt = rangeEntity.RangeValueAt,
                RangeEntityValue = rangeEntity.RangeEntityValue ?? new List<RangeEntityValue>(),
                StatusTypeService = rangeEntity.StatusTypeService
            };
        }
    }
}
