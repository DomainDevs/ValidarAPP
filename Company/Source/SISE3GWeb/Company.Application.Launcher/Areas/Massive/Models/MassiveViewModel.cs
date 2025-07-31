using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Framework.UIF.Web.Services;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Massive.Models
{
    public class MassiveViewModel
    {
        /// <summary>
        /// Identificador
        /// </summary>
        public int? LoadId { get; set; }

        /// <summary>
        /// Tipo de Proceso
        /// </summary>
        [Required]
        [Display(Name = "LabelProcessType", ResourceType = typeof(App_GlobalResources.Language))]
        public int ProcessTypeId { get; set; }

        /// <summary>
        /// Tipo Cargue
        /// </summary>
        [Required]
        [Display(Name = "LabelTypeLoad", ResourceType = typeof(App_GlobalResources.Language))]
        public int LoadTypeId { get; set; }

        /// <summary>
        /// Id Solicitud Agrupadora
        /// </summary>
        public int? BillingGroupId { get; set; }

        /// <summary>
        /// Id Solicitud Agrupadora
        /// </summary>
        public string BillingGroupDescription { get; set; }

        /// <summary>
        /// Id Solicitud Agrupadora
        /// </summary>
        public int? RequestGroupId { get; set; }

        /// <summary>
        /// Número Solicitud Agrupadora
        /// </summary>
        public int? RequestGroupNumber { get; set; }

        /// <summary>
        /// Id Solicitud Agrupadora
        /// </summary>
        public string RequestGroupDescription { get; set; }

        /// <summary>
        /// MainAgentName
        /// </summary>
        //[Required]
        //[Display(Name = "LabelAgentPrincipal", ResourceType = typeof(App_GlobalResources.Language))]
        public int AgentId { get; set; }

        /// <summary>
        /// MainAgentName
        /// </summary>
        //[Required]
        //[Display(Name = "LabelAgentPrincipal", ResourceType = typeof(App_GlobalResources.Language))]
        public string AgentName { get; set; }

        /// <summary>
        /// Id agente principal
        /// </summary>
        //[Required]
        //[Display(Name = "LabelAgentPrincipal", ResourceType = typeof(App_GlobalResources.Language))]
        public int AgencyId { get; set; }

        /// <summary>
        /// Id Sucursal
        /// </summary>
        //[Required]
        //[Display(Name = "LabelBranch", ResourceType = typeof(App_GlobalResources.Language))]
        public int? BranchId { get; set; }

        /// <summary>
        /// Id Ramo Comercial
        /// </summary>
        [Required]
        [Display(Name = "LabelPrefixCommercial", ResourceType = typeof(App_GlobalResources.Language))]
        public int PrefixId { get; set; }

        /// <summary>
        /// Id Producto
        /// </summary>        
        [Required]
        [Display(Name = "LabelCommercialProduct", ResourceType = typeof(App_GlobalResources.Language))]
        public int ProductId { get; set; }

        /// <summary>
        /// Id Punto De Venta
        /// </summary>
        [Display(Name = "LabelSalesPoint", ResourceType = typeof(App_GlobalResources.Language))]
        public int? SalesPointId { get; set; }

        /// <summary>
        /// Tipo de negocio
        /// </summary>        
        public int? BusinessTypeId { get; set; }

        /// <summary>
        /// Nombre Cargue
        /// </summary>
        [Required]
        [Display(Name = "LabelNameLoad", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(60, MinimumLength = 3)]
        public string LoadName { get; set; }

        /// <summary>
        /// Nombre Archivo
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Estado del cargue
        /// </summary>                     
        public string StateDescription { get; set; }

        /// <summary>
        /// Total De Registros
        /// </summary>
        public int? RecordsTotal { get; set; }

        /// <summary>
        /// Registros Procesados
        /// </summary>
        public int? RecordsProcessed { get; set; }

        /// <summary>
        /// Registros Pendientes
        /// </summary>
        public int? RecordsPendings { get; set; }

        /// <summary>
        /// Registros Con Eventos
        /// </summary>
        public int? RecordsEvents { get; set; }

        /// <summary>
        /// Registros Con Error
        /// </summary>
        public int? RecordsErrors { get; set; }

        /// <summary>
        /// Registros Tarifados
        /// </summary>
        public int? RecordsTariffed { get; set; }

        /// <summary>
        /// Registros por Tarifart
        /// </summary>
        public int? RecordsForRate { get; set; }

        public int? RecordsIssued { get; set; }

        public int? RecordsForIssue { get; set; }

        /// <summary>
        /// Id de Usuario
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// Id del tipo de archivo
        /// </summary>
        public int MassiveFileType { get; set; }

        /// <summary>
        /// Descripcion del tipo de archivo seleccionado
        /// </summary>                     
        public string MassiveFileTypeDescription { get; set; }

        /// <summary>
        /// Registros a Excluir
        /// </summary>
        //[Required]
        //[Display(Name = "LabelPolicyNumber", ResourceType = typeof(App_GlobalResources.Language))]
        //[RegularExpression(@"^[0-9]+$", ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorOnlyNumbers")]
        public string RecordsExclude { get; set; }

    }
}