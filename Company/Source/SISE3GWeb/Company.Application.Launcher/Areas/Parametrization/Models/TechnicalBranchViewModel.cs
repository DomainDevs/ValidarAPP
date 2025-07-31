using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    /// <summary>
    /// Modelo del Ramo Técnico
    /// </summary>
    public class TechnicalBranchViewModel
    {
        /// <summary>
        /// Identificador del Ramo Técnico
        /// </summary>
        [Range(0, 999, ErrorMessage = "Debe ser mayor a 0 y menor a 1000 ")]
        [RegularExpression(@"^[0-9]+$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Descripcion del largo del Ramo tecnico
        /// </summary>
        [Display(Name = "LabelDescriptionLong", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(50)]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [DataMember]
        public string LongDescription{ get; set; }

        /// <summary>
        /// Descripcioon corta del ramo tecnico
        /// </summary>
        [Display(Name = "LabelDescriptionShort", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(15)]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [DataMember]
        public string ShortDescription { get; set; }

        /// <summary>
        /// Abreviatura
        /// </summary>
        [Display(Name = "LabelAbbreviation", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(3)]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorSpecialCharacters")]
        [DataMember]
        public string TyniDescription { get; set; }

        /// <summary>
        /// Tipo de Riesgo cubierto
        /// </summary>
        [DataMember]
        public int RiskTypeId { get; set; }

        /// <summary>
        /// Nombre de ramo Tecnico
        /// </summary>
        [Display(Name = "PlaceHolderTechnicalBranchName", ResourceType = typeof(App_GlobalResources.Language))]
        [MaxLength(150)]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string TechnicalBranchName { get; set; }

        /// <summary>
        /// Nombre del tipo de riesgo
        /// </summary>
        [Display(Name = "PlaceHolderTechnicalBranchName", ResourceType = typeof(App_GlobalResources.Language))]
        [MaxLength(150)]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string DescriptionRiskType { get; set; }

        /// <summary>
        /// geto o set 
        /// </summary>
        [DataMember]
        public int CreatedLineBussinesId { get; set; }


        /// <summary>
        /// atributo de linea de ramo tecnico
        /// </summary>
        [DataMember]
        private string _reportLineBusinessCode = null;

        /// <summary>
        /// enabled
        /// </summary>
        [DataMember]
        public bool Enabled { get; set; }

        /// <summary>
        /// Nombre de la clausula del ramo tecnico
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Id de la clausula del ramo tecnico
        /// </summary>
        [DataMember]
        public int IdClause { get; set; }


        /// <summary>
        /// Lista de clausulas para el ramo tecnico
        /// </summary>
        [DataMember]
        public List<ClausesByLineBusiness> ListClausesByTechnicalBranch { get; set; }

        /// <summary>
        /// Lista de Tipos de riesgo para el ramo tecnico
        /// </summary>
        [DataMember]
        public List<LineBusinessByCoveredRiskType> ListLineBusinessCoveredrisktype { get; set; }

        /// <summary>
        /// Lista de los Id de los objeto del seguro que se asocian al ramo técnico.
        /// </summary>
        [DataMember]
        public List<int> ListIsnsuranceObjects { get; set; }

        /// <summary>
        /// Descripcion de ramo tecnico
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Tipo de riesgo asociado al ramo tecnico
        /// </summary>
        [DataMember]
        public int IdLineBusinessbyRiskType { get; set; }

        /// <summary>
        /// Lista de AMparos del ramo tecnico
        /// </summary>
        [DataMember]
        public List<ProtectionViewModel> ProtectionAssigned { get; set; }
    }
}