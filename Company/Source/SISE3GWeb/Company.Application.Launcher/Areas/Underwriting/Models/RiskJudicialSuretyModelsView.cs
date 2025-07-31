using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Models
{
    public class RiskJudicialSuretyModelsView
    {
        /// <summary>
        /// Id de Temporal
        /// </summary>
        public int TemporalId { get; set; }


        /// <summary>
        /// Identificador del riesgo de caucion Judicial
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Id de Riesgo
        /// </summary>
        public int? RiskId { get; set; }

        /// <summary>
        /// Documento asegurado
        /// </summary>        
        [Display(Name = "LabelPrincipalInsured", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string InsuredDocumentNumber { get; set; }

        /// <summary>
        /// Id detalle asegurado
        /// </summary>        
        [Display(Name = "LabelPrincipalInsured", ResourceType = typeof(App_GlobalResources.Language))]
        public int? InsuredDetailId { get; set; }

        /// <summary>
        /// Tipo de cliente Tomador
        /// </summary>        
        [Display(Name = "LabelPrincipalInsured", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int InsuredCustomerType { get; set; }

        /// <summary>
        /// Id asegurado
        /// </summary>        
        [Display(Name = "LabelPrincipalInsured", ResourceType = typeof(App_GlobalResources.Language))]
        //[Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int? InsuredId { get; set; }

        /// <summary>
        /// Descripcion del asegurado
        /// </summary>
        [Display(Name = "InsuredName", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string InsuredName { get; set; }

        /// <summary>
        /// Direccion asegurado
        /// </summary>
        [Display(Name = "LabelPrincipalInsured", ResourceType = typeof(App_GlobalResources.Language))]
        //[Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int InsuredAddressId { get; set; }

        /// <summary>
        /// Telefono asegurado
        /// </summary>
        public int? InsuredPhoneId { get; set; }

        /// <summary>
        /// Correo asegurado
        /// </summary>
        public int? InsuredEmailId { get; set; }

        /// <summary>
        /// Fecha de Nacimiento del Asegurado
        /// </summary>
        public DateTime? InsuredBirthDate { get; set; }

        /// <summary>
        /// Genero del Asegurado
        /// </summary>
        public string InsuredGender { get; set; }

        /// <summary>
        /// Identificador de asegurador actua como
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int IdInsuredActsAs { get; set; }

        /// <summary>
        /// Identificador de Tomador actua como
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int IdHolderActAs { get; set; }

        /// <summary>
        /// Identificador del articulo
        /// </summary>
        [DataMember]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int IdArticle { get; set; }

        /// <summary>
        /// Nombre del articulo
        /// </summary>
        [Display(Name = "LabelArticle", ResourceType = typeof(App_GlobalResources.Language))]
        //[Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string ArticleName { get; set; }

        /// <summary>
        /// Identificador de Tipo de Juzgado
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int IdTypeOfCourt { get; set; }

        /// <summary>
        /// Nombre de Tipo de Juzgado
        /// </summary>
        [Display(Name = "LabelTypeOfCourt", ResourceType = typeof(App_GlobalResources.Language))]
        //[Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string TypeOfCourtName { get; set; }

        public int CountryId { get; set; }

        /// <summary>
        /// Identificador del departamento
        /// </summary>
        [Display(Name = "LabelDepartment", ResourceType = typeof(App_GlobalResources.Language))]
        public int IdDepartment { get; set; }

        /// <summary>
        /// Nombre del Departamento 
        /// </summary>
        [Display(Name = "LabelDepartment", ResourceType = typeof(App_GlobalResources.Language))]
        //[Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string DepartmentName { get; set; }

        /// <summary>
        /// Identificador de municipio
        /// </summary>
        [Display(Name = "LabelCity", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int IdMunicipality { get; set; }

        /// <summary>
        /// Nombre del municipio
        /// </summary>
        [Display(Name = "LabelCity", ResourceType = typeof(App_GlobalResources.Language))]
        //[Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string MunicipalityName { get; set; }

        /// <summary>
        /// Proceso y/o Radicado
        /// </summary>
        [Display(Name = "ProcessAndOrFiled", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(25)]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string ProcessAndOrFiled { get; set; }

        /// <summary>
        /// Valor asegurado
        /// </summary>
        [Display(Name = "LabelInsuredValue", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        [RegularExpression(@"^\$?\d+((\.\d+)*)+(\,\d{1,2})?$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        [StringLength(18)]
        public string InsuredValue { get; set; }

        /// <summary>
        /// Valor asegurado
        /// </summary>
        [Display(Name = "LabelTotalSumInsured", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public string AmountInsured { get; set; }

        /// <summary>
        /// Grupo de coberturas
        /// </summary>
        [Display(Name = "LabelGroupCoverage", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int GroupCoverage { get; set; }

        /// <summary>
        /// Titulo pantalla
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Id de Riesgo
        /// </summary>
        public int? OriginalRiskId { get; set; }

        public AdditionalDataJudicialSuretyModelsView Attorney { get; set; }
       
        ///// <summary>
        ///// Datos del apoderado
        ///// </summary>
        //public string AttorneyName { get; set; }

        ///// <summary>
        ///// tipo de identificacion del apoderado
        ///// </summary>
        //public int DocumenTypeAttorney { get; set; }

        ///// <summary>
        ///// Numero de documento de apoderado
        ///// </summary>
        //public int DocumentNumberAttorney { get; set; }

        ///// <summary>
        ///// Numero de la tarjeta profesional
        ///// </summary>
        //public string CardProfessionalNumber { get; set; }

        ///// <summary>
        ///// Nombre de asegurado a imprimir
        ///// </summary>
        //public string InsuredPrintName { get; set; }

        /// <summary>
        /// Valor de la Prima
        /// </summary>
        public decimal Premium { get; set; }

        /// <summary>
        /// 100 % Retención
        /// </summary>
        public bool IsRetention { get; set; }

        /// <summary>
        /// Obtiene o setea el tipo de documento de persona
        /// </summary>
        public int InsuredDocumentTypeId { get; set; }

        /// <summary>
        /// Obtiene o setea el tipo de 
        /// </summary>
        public int InsuredIndividualTypeId { get; set; }

        public int RiskActivityId { get; set; }


        public int InsuredAssociationType { get; set; }
    }
}