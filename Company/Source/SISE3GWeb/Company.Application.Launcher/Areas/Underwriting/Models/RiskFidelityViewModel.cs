using Sistran.Core.Application.UnderwritingServices.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Models
{
    public class RiskFidelityViewModel
    {
        /// <summary>
        /// Id de Temporal
        /// </summary>
        public int TemporalId { get; set; }
        
        /// <summary>
        /// Id ramo comercial
        /// </summary>
        public int PrefixId { get; set; }

        /// <summary>
        /// Id producto
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Id tipo de poliza
        /// </summary>
        public int PolicyTypeId { get; set; }

        /// <summary>
        /// Id de Riesgo
        /// </summary>
        public int? RiskId { get; set; }


        /// <summary>
        /// tipo de endoso
        /// </summary>
        public int? EndorsementType { get; set; }
 

        /// <summary>
        /// Id asegurado
        /// </summary>        
        [Display(Name = "LabelPrincipalInsured", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int InsuredId { get; set; }

        /// <summary>
        /// Nombre asegurado
        /// </summary>
        [Display(Name = "LabelPrincipalInsured", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [StringLength(100)]
        public string InsuredName { get; set; }

        /// <summary>
        /// Tipo de cliente Tomador
        /// </summary>        
        [Display(Name = "LabelPrincipalInsured", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int InsuredCustomerType { get; set; }

        /// <summary>
        /// Id detalle asegurado
        /// </summary>        
        [Display(Name = "LabelPrincipalInsured", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int? InsuredDetailId { get; set; }

        /// <summary>
        /// Direccion asegurado
        /// </summary>
        [Display(Name = "LabelPrincipalInsured", ResourceType = typeof(App_GlobalResources.Language))]
        public int InsuredAddressId { get; set; }

        /// <summary>
        /// Fecha de Nacimiento del Asegurado
        /// </summary>
        public DateTime? InsuredBirthDate { get; set; }

        /// <summary>
        /// Genero del Asegurado
        /// </summary>
        public string InsuredGender { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "LabelActivityRisk", ResourceType = typeof(App_GlobalResources.Language))]
        public int? RiskActivityId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "LabelActivityRisk", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocumentControlActivityRisk")]
        public string RiskActivityDescription { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "LabelGroupCoverage", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int GroupCoverage { get; set; }

        public  List<InsuredObject> InsuredObjects { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public bool IsDeclarative { get; set; }

        /// <summary>
        /// Valor asegurado
        /// </summary>
        public string AmountInsured { get; set; }

        /// <summary>
        /// Prima
        public string Premium { get; set; }
        
        /// <summary>
        /// Id tomador
        /// </summary> 
        public int HolderId { get; set; }

        /// <summary>
        /// titulo de riesgo
        /// </summary> 
        public string Title { get; set; }

        /// <summary>
        /// Obtiene o setea el numero de documento
        /// </summary>
        public string InsuredDocumentNumber { get; set; }
        
        // <summary>
        /// Telefono asegurado
        /// </summary>
        public int? InsuredPhoneId { get; set; }

        /// <summary>
        /// Correo asegurado
        /// </summary>
        public int? InsuredEmailId { get; set; }

        /// <summary>
        /// Identificador de la profesión
        /// </summary>
        [Display(Name = "LabelIdOccupation", ResourceType = typeof(App_GlobalResources.Language))]
        public int IdOccupation { get; set; }

        /// <summary>
        /// Descripción de la profesión
        /// </summary>
        public string OccupationDescription { get; set; }

        /// <summary>
        /// Fecha de descubrimiento
        /// </summary>
        [Display(Name = "LabelDiscoveryDate", ResourceType = typeof(App_GlobalResources.Language))]
        public DateTime DiscoveryDate { get; set; }

        /// <summary>
        /// Obtiene o setea el tipo de persona
        /// </summary>
        public int InsuredDocumentTypeId { get; set; }
    }   
}