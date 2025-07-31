using System;
using System.Collections.Generic;
using Sistran.Core.Application.RulesScriptsServices.Models;
using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.RulesScripts.Models
{
    using Application.ModelServices.Enums;

    public class ListEntityViewModel
    {
        public int ListEntityCode { get; set; }

        [Display(Name = "Description", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "FieldRequired")]
        [StringLength(50)]
        public string Description { get; set; }

        public int ListValueAt { get; set; }

        public List<ListEntityValue> ListEntityValue { get; set; }

        public StatusTypeService StatusTypeService { get; set; }

        public static ListEntityViewModel ListEntityToViewModel(ListEntity listEntity)
        {
            return new ListEntityViewModel
            {
                ListEntityCode = listEntity.ListEntityCode,
                Description = listEntity.Description,
                ListValueAt = listEntity.ListEntityAt,
                ListEntityValue = listEntity.ListEntityValue ?? new List<ListEntityValue>(),
                StatusTypeService = listEntity.StatusTypeService
            };
        }
    }
}
