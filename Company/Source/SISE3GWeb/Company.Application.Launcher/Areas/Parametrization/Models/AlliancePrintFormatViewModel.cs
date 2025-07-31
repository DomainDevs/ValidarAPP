// -----------------------------------------------------------------------
// <copyright file="InsuredProfileViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Eduardo Rodríguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Modelo de las propiedades del Formato de impresión de aliado.
    /// </summary>    
    public class AlliancePrintFormatViewModel
    {
        /// <summary>
        /// Obtiene o establece el identificador del formato de impresión.
        /// </summary>          
        [Range(0, 32767, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorIdInsuredProfile")]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece el código del ramo.
        /// </summary>  
        [Display(Name = "LabelAlliancePrintFormatPrefix", ResourceType = typeof(App_GlobalResources.Language))]
        [Range(0, 255, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorIdAlliancePrintFormatPrefixCd")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int PrefixCd { get; set; }

        /// <summary>
        /// Obtiene o establece el código tipo de endoso.
        /// </summary>        
        [Display(Name = "LabelAlliancePrintFormatEndorsementType", ResourceType = typeof(App_GlobalResources.Language))]
        [Range(0, 255, ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorIdAlliancePrintFormatEndoTypeCd")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int EndoTypeCd { get; set; }

        /// <summary>
        /// Obtiene o establece el Nombre formato de impresión.
        /// </summary>     
        [Display(Name = "LabelAlliancePrintFormatCode", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(30)]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string Format { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si se encuentra habilitado el formato de impresión.
        /// </summary>        
        public bool Enable { get; set; }
    }
}