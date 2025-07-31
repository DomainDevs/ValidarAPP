// -----------------------------------------------------------------------
// <copyright file="ScoreTypeDocViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Framework.UIF.Web.Areas.Person.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Modelo de vista para el tipo documento datacrédito
    /// </summary>
    public class ScoreTypeDocViewModel
    {
        #region ScoreTypeDoc
        /// <summary>
        /// Obtiene o establece Id del tipo documento datacrédito
        /// </summary>
        [Display(Name = "LabelCodeScoreTypeDoc", ResourceType = typeof(App_GlobalResources.LanguagePerson))]
        [Range(0, 99, ErrorMessage = "El código data crédito debe estar entre 0 y 99")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.LanguagePerson), ErrorMessageResourceName = "ErrorDocument")]
        public int IdCardTypeScore { get; set; }

        /// <summary>
        /// Obtiene o establece Descripción larga del tipo documento datacrédito
        /// </summary>  
        [Display(Name = "LabelDescription", ResourceType = typeof(App_GlobalResources.LanguagePerson))]
        [StringLength(50, ErrorMessageResourceType = typeof(App_GlobalResources.LanguagePerson), ErrorMessageResourceName = "ErrorMaxLengthCharacter")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.LanguagePerson), ErrorMessageResourceName = "ErrorDocument")]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece Descripción corta del tipo documento datacrédito
        /// </summary>
        [Display(Name = "LabelDescriptionShort", ResourceType = typeof(App_GlobalResources.LanguagePerson))]
        [StringLength(50, ErrorMessageResourceType = typeof(App_GlobalResources.LanguagePerson), ErrorMessageResourceName = "ErrorMaxLengthCharacter")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.LanguagePerson), ErrorMessageResourceName = "ErrorDocument")]
        public string SmallDescription { get; set; }
        #endregion

        #region Score3gTypeDoc
        /// <summary>
        /// Obtiene o establece Id de la asociación entre tipo documento datacrédito y tipo documento SISE 3g
        /// </summary>
        public int IdScore3g { get; set; }

        /// <summary>
        /// Obtiene o establece Id del tipo documento SISE 3g asociado
        /// </summary>
        public int IdCardTypeCode { get; set; }
        #endregion
    }
}