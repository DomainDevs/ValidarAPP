using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Person.Models
{
    /// <summary>
    /// Modelo de la Información de Persona
    /// </summary
    public class PersonBasicViewModel
    {
        /// <summary>
        /// Obtiene o establece el Id de la Persona
        /// </summary>
        public int IndividualId { get; set; }

        /// <summary>
        /// Obtiene o establece el Tipo de Documento
        /// </summary>   
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.LanguagePerson), ErrorMessageResourceName = "ErrorDocument")]
        [Display(ResourceType = typeof(App_GlobalResources.LanguagePerson), Name = "LabelTypeDocument")]
        public int DocumentType { get; set; }


        /// <summary>
        /// Obtiene o establece el Numero de Documento
        /// </summary>   
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.LanguagePerson), ErrorMessageResourceName = "ErrorDocument")]
        [MaxLength(14)]
        [Display(ResourceType = typeof(App_GlobalResources.LanguagePerson), Name = "LabelDocumentNumber")]
        public string DocumentNumber { get; set; }

        /// <summary>
        /// Obtiene o establece el Código SBS
        /// </summary>   
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.LanguagePerson), ErrorMessageResourceName = "ErrorDocument")]
        [MaxLength(11)]
        [Display(ResourceType = typeof(App_GlobalResources.LanguagePerson), Name = "codePerson")]
        public int SbsCode { get; set; }

        /// <summary>
        /// Obtiene o establece el Código de Persona
        /// </summary>   
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.LanguagePerson), ErrorMessageResourceName = "ErrorDocument")]
        [MaxLength(14)]
        [Display(ResourceType = typeof(App_GlobalResources.LanguagePerson), Name = "codePerson")]
        public int PersonCode { get; set; }

        /// <summary>
        /// Obtiene o establece el Primer Apellido
        /// </summary>   
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.LanguagePerson), ErrorMessageResourceName = "ErrorDocument")]
        [MaxLength(20)]
        [Display(ResourceType = typeof(App_GlobalResources.LanguagePerson), Name = "Surname")]
        public string FirstName { get; set; }

        /// <summary>
        /// Obtiene o establece el Segundo Apellido
        /// </summary>   
        [MaxLength(20)]
        [Display(ResourceType = typeof(App_GlobalResources.LanguagePerson), Name = "Secondsurname")]
        public string LastName { get; set; }

        /// <summary>
        /// Obtiene o establece el Nombre
        /// </summary>   
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.LanguagePerson), ErrorMessageResourceName = "ErrorDocument")]
        [MaxLength(20)]
        [Display(ResourceType = typeof(App_GlobalResources.LanguagePerson), Name = "Names")]
        public string Name { get; set; }


        /// <summary>
        /// Obtiene o establece el Genero
        /// </summary>   
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.LanguagePerson), ErrorMessageResourceName = "ErrorDocument")]
        [Display(ResourceType = typeof(App_GlobalResources.LanguagePerson), Name = "LabelGender")]
        public int Gender { get; set; }

        /// <summary>
        /// Obtiene o establece el Estado Civil
        /// </summary>   
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.LanguagePerson), ErrorMessageResourceName = "ErrorDocument")]
        [Display(ResourceType = typeof(App_GlobalResources.LanguagePerson), Name = "Maritalstatus")]
        public int MaritalStatus { get; set; }

        /// <summary>
        /// Obtiene o establece la Fecha de Nacimiento
        /// </summary>   
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.LanguagePerson), ErrorMessageResourceName = "ErrorDocument")]
        [Display(ResourceType = typeof(App_GlobalResources.LanguagePerson), Name = "Birthdate")]
        public DateTime Birthdate { get; set; }

        /// <summary>
        /// Obtiene o establece la Edad
        /// </summary>   
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.LanguagePerson), ErrorMessageResourceName = "ErrorDocument")]
        [MaxLength(20)]
        [Display(ResourceType = typeof(App_GlobalResources.LanguagePerson), Name = "age")]
        public int Age { get; set; }


        /// <summary>
        /// Obtiene o establece Lugar de Nacimiento
        /// </summary>   
        [MaxLength(20)]
        [Display(ResourceType = typeof(App_GlobalResources.LanguagePerson), Name = "Birthplace")]
        public string BirthPlace { get; set; }

        /// <summary>
        /// Obtiene o establece la Fecha última actualización
        /// </summary>   
        //[Display(ResourceType = typeof(App_GlobalResources.Language), Name = "LastUpdate")]
        public DateTime LastUpdate { get; set; }

        /// <summary>
        /// Obtiene o establece Lugar de Nacimiento
        /// </summary>   
        [Display(ResourceType = typeof(App_GlobalResources.LanguagePerson), Name = "UpdateBy")]
        public string UpdateBy { get; set; }
    }


    /// <summary>
    /// Modelo de la Información Basica de Compañia
    /// </summary
    public class CompanyBasicViewModel
    {
        /// <summary>
        /// Obtiene o establece el Id de la Compañia
        /// </summary>
        public int IndividualId { get; set; }

        /// <summary>
        /// Obtiene o establece el Tipo de Documento
        /// </summary>   
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.LanguagePerson), ErrorMessageResourceName = "ErrorDocument")]
        [Display(ResourceType = typeof(App_GlobalResources.LanguagePerson), Name = "LabelTypeDocument")]
        public int DocumentTypeCompany { get; set; }


        /// <summary>
        /// Obtiene o establece el Numero de Documento
        /// </summary>   
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.LanguagePerson), ErrorMessageResourceName = "ErrorDocument")]
        [MaxLength(14)]
        [Display(ResourceType = typeof(App_GlobalResources.LanguagePerson), Name = "LabelDocumentNumber")]
        public string DocumentNumberCompany { get; set; }

        /// <summary>
        /// Obtiene o establece el digito de Documento de la Compañia
        /// </summary>
        [MaxLength(1)]
        public string CompanyDigit { get; set; }
        
        /// <summary>
        /// Obtiene o establece el Código de la Compañia
        /// </summary>   
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.LanguagePerson), ErrorMessageResourceName = "ErrorDocument")]
        [MaxLength(14)]
        [Display(ResourceType = typeof(App_GlobalResources.LanguagePerson), Name = "codePerson")]
        public int CompanyCode { get; set; }

        /// <summary>
        /// Obtiene o establece la Razon Social
        /// </summary>   
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.LanguagePerson), ErrorMessageResourceName = "ErrorDocument")]
        [MaxLength(120)]
        [Display(ResourceType = typeof(App_GlobalResources.LanguagePerson), Name = "TradeName")]
        public string TradeName { get; set; }


        /// <summary>
        /// Obtiene o establece el Tipo de Asociación
        /// </summary>   
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.LanguagePerson), ErrorMessageResourceName = "ErrorDocument")]
        [Display(ResourceType = typeof(App_GlobalResources.LanguagePerson), Name = "LabelTypePartnership")]
        public int TypePartnership { get; set; }

        /// <summary>
        /// Obtiene o establece Tipo de Empresa
        /// </summary>   
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.LanguagePerson), ErrorMessageResourceName = "ErrorDocument")]
        [Display(ResourceType = typeof(App_GlobalResources.LanguagePerson), Name = "CompanyType")]
        public int CompanyTypePartnership { get; set; }

        /// <summary>
        /// Obtiene o establece el Pais de la Compañia
        /// </summary>   
        [Display(ResourceType = typeof(App_GlobalResources.LanguagePerson), Name = "LabelCountryOrigin")]
        public int Country { get; set; }

        /// <summary>
        /// Obtiene o establece la Fecha última actualización
        /// </summary>   
        [Display(ResourceType = typeof(App_GlobalResources.LanguagePerson), Name = "LastUpdate")]
        public DateTime LastUpdateCompany { get; set; }

        /// <summary>
        /// Obtiene o establece Lugar de Nacimiento
        /// </summary>   
        [Display(ResourceType = typeof(App_GlobalResources.LanguagePerson), Name = "UpdateBy")]
        public string UpdateByCompany { get; set; }
    }

    /// <summary>
    /// Modelo de las propiedades de Busqueda Avanzada de Informacion basica Persona
    /// </summary>    
    public class BasicInformationAdvancedSearchViewModel
    {
        /// <summary>
        /// Obtiene o establece el Tipo de Documento
        /// </summary>   
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.LanguagePerson), ErrorMessageResourceName = "ErrorDocument")]
        [Display(ResourceType = typeof(App_GlobalResources.LanguagePerson), Name = "LabelTypeDocument")]
        public int DocumentTypeSearch { get; set; }



        /// <summary>
        /// Obtiene o establece el Tipo de Persona
        /// </summary>   
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.LanguagePerson), ErrorMessageResourceName = "ErrorDocument")]
        [Display(ResourceType = typeof(App_GlobalResources.LanguagePerson), Name = "PersonType")]
        public int PersonTypeSearch { get; set; }


        /// <summary>
        /// Obtiene o establece el Código de Persona
        /// </summary>   
        [Display(ResourceType = typeof(App_GlobalResources.LanguagePerson), Name = "codePerson")]
        public int? PersonCodeSearch { get; set; }

        /// <summary>
        /// Obtiene o establece el Primer Apellido
        /// </summary>   
        [MaxLength(20)]
        [Display(ResourceType = typeof(App_GlobalResources.LanguagePerson), Name = "Surname")]
        public string FirstNameSearch { get; set; }

        /// <summary>
        /// Obtiene o establece el Segundo Apellido
        /// </summary>   
        [MaxLength(20)]
        [Display(ResourceType = typeof(App_GlobalResources.LanguagePerson), Name = "Secondsurname")]
        public string LastNameSearch { get; set; }

        /// <summary>
        /// Obtiene o establece el Nombre
        /// </summary>   
        [MaxLength(30)]
        [Display(ResourceType = typeof(App_GlobalResources.LanguagePerson), Name = "Names")]
        public string NameSearch { get; set; }

        /// <summary>
        /// Obtiene o establece el Numero de Documento
        /// </summary>   
        [MaxLength(14)]
        [Display(ResourceType = typeof(App_GlobalResources.LanguagePerson), Name = "LabelDocumentNumber")]
        public string DocumentNumberSearch { get; set; }



        /// <summary>
        /// Obtiene o establece la Razon Social
        /// </summary>   
        [MaxLength(120)]
        [Display(ResourceType = typeof(App_GlobalResources.LanguagePerson), Name = "TradeName")]
        public string TradeName { get; set; }

    }
}