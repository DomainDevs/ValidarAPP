// -----------------------------------------------------------------------
// <copyright file="BusinessConfigurationViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Sistran.Core.Application.ModelServices.Enums;

    /// <summary>
    /// Modelo de vista para negocio y acuerdos de negocio
    /// </summary>
    public class BusinessConfigurationViewModel
    {
        /// <summary>
        /// Obtiene o establece el Id del negocio.
        /// </summary>
        [Display(Name = "CodeBusiness", ResourceType = typeof(App_GlobalResources.Language))]
        [Range(1, 9999,
        ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int BusinessId { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción del negocio.
        /// </summary>
        [Display(Name = "LabelDescription", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(50)]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si el negocio está habilitado.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Obtiene o establece el ramo comercial asociado.
        /// </summary>
        public PrefixBusiness PrefixCode { get; set; }

        /// <summary>
        /// Obtiene o establece los acuerdos de negocio asociados.
        /// </summary>
        public List<BusinessConfigurationQueryViewModel> ListBusinessConfigurationQueryViewModel { get; set; }

        /// <summary>
        /// Obtiene o establece el status del negocio.
        /// </summary>
        public StatusTypeService StatusType { get; set; }
    }

    /// <summary>
    /// Modelo de vista ramo comercial.
    /// </summary>
    public class PrefixBusiness
    {
        /// <summary>
        /// Obtiene o establece el Id del ramo comercial.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorPrefix")]
        public int PrefixCode { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción del ramo comrcial.
        /// </summary>
        public string PrefixDescription { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción corta del ramo comercial.
        /// </summary>
        public string PrefixSmallDescription { get; set; }
    }


    /// <summary>
    /// Modelo de acuerdo de negocio.
    /// </summary>
    public class BusinessConfigurationQueryViewModel
    {
        /// <summary>
        /// Obtiene o establece el Id del acuerdo de negocio.
        /// </summary>
        public int BusinessConfigurationId { get; set; }

        /// <summary>
        /// Obtiene o establece el Id del negocio.
        /// </summary>
        public int BusinessId { get; set; }

        /// <summary>
        /// Obtiene o establece la solicitud agrupadora.
        /// </summary>
        public RequestBusinessViewModel Request { get; set; }

        /// <summary>
        /// Obtiene o establece el producto.
        /// </summary>
        public ProductBusinessViewModel Product { get; set; }

        /// <summary>
        /// Obtiene o establece el grupo de coberturas.
        /// </summary>
        public GroupCoverageBusinessViewModel GroupCoverage { get; set; }

        /// <summary>
        /// Obtiene o establece el tipo de asistencia.
        /// </summary>
        public AssistanceTypeViewModel Assistance { get; set; }

        /// <summary>
        /// Obtiene o establece el id producto respuesta.
        /// </summary>
        public string ProductIdResponse { get; set; }

        /// <summary>
        /// Obtiene o establece el status del acuerdo de negocio.
        /// </summary>
        public StatusTypeService StatusType { get; set; }
    }

    /// <summary>
    /// Modelo de solicitud agrupadora.
    /// </summary>
    public class RequestBusinessViewModel
    {
        /// <summary>
        /// Obtiene o establece el Id de la solicitud agrupadora con endoso.
        /// </summary>
        public int RequestEndorsementId { get; set; }

        /// <summary>
        /// Obtiene o establece la solicitud agrupadora.
        /// </summary>
        public int RequestId { get; set; }

        /// <summary>
        /// Obtiene o establece el id producto de la solicitud agrupadora.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Obtiene o establece el id del ramo comercial de la solicitud agrupadora.
        /// </summary>
        public int PrefixCode { get; set; }
    }

    /// <summary>
    /// Modelo de producto.
    /// </summary>
    public class ProductBusinessViewModel
    {
        /// <summary>
        /// Obtiene o establece el Id del producto.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorSelectedProduct")]
        public int ProductId { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción del producto.
        /// </summary>
        public string ProductDescription { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción corta del producto.
        /// </summary>
        public string ProductSmallDescription { get; set; }

        /// <summary>
        /// Obtiene a establece un valor que indica si el producto está activo.
        /// </summary>
        public bool ActiveProduct { get; set; }
    }

    /// <summary>
    /// Modelo de grupo de coberturas.
    /// </summary>
    public class GroupCoverageBusinessViewModel
    {
        /// <summary>
        /// Obtiene o establece el Id del grupo de coberturas.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorSelectCoverage")]
        public int GroupCoverageId { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción corta del grupo de coberturas.
        /// </summary>
        public string GroupCoverageSmallDescription { get; set; }
    }

    /// <summary>
    /// Modelo de tipo de asistencia.
    /// </summary>
    public class AssistanceTypeViewModel
    {
        /// <summary>
        /// Obtiene o establece el Id del tipo de assitencia.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorSelectAssistanceType")]
        public int AssistanceCode { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción del tipo de assitencia.
        /// </summary>
        public string AssistanceDescription { get; set; }
    }
}