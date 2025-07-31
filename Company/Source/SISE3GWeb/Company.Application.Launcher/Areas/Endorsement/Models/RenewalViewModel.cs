using System;
using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models
{
    public class RenewalViewModel : EndorsementViewModel
    {

        /// <summary>
        /// Obtiene o sete el tipo de endoso
        /// </summary>
        public int EndorsementsType { get; set; }

        /// <summary>
        /// Vigencia Final Modificada
        /// </summary>
        public DateTime ModifyFrom { get; set; }

        /// <summary>
        /// Vigencia Final Modificada
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorCurrentTo")]
        public DateTime? ModifyTo { get; set; }

        /// <summary>
        /// tipo de renovacion
        /// </summary>
        public bool IsUnderIdenticalConditions { get; set; }

    }
}