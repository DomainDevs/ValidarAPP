using Sistran.Core.Application.UnderwritingServices.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Sistran.Core.Framework.UIF.Web.Areas.Collective.Models
{
    public class CollectiveModelView
    {
        /// <summary>
        /// Proceso
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nombre Cargue
        /// </summary>
        [Required]
        [Display(Name = "LabelNameLoad", ResourceType = typeof(App_GlobalResources.Language))]
        [StringLength(60, MinimumLength = 3)]
        public string LoadName { get; set; }

        /// <summary>
        /// IdTypeLoad
        /// </summary>
        [Required]
        public int LoadTypeId { get; set; }

        /// <summary>
        /// IdBranch
        /// </summary>
        [Required]
        public int BranchId { get; set; }

        /// <summary>
        /// SalesPoint
        /// </summary>
        public int SalesPoint { get; set; }

        /// <summary>
        /// Codigo del Ramo Comercial
        /// </summary>
        [Required]
        public int PrefixId { get; set; }

        /// <summary>
        /// Producto
        /// </summary>
        [Required]
        public int ProductId { get; set; }

        /// <summary>
        /// Codigo Agente
        /// </summary>
        [Required]
        public int AgentId { get; set; }

        /// <summary>
        /// Codigo Agencia
        /// </summary>
        [Required]
        public int AgencyId { get; set; }


        /// <summary>
        /// IdRequest
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// Tipo de negocio
        /// </summary>        
        public BusinessType BusinessType { get; set; }


        /// <summary>
        /// Tipo de Endoso
        /// </summary>             
        public string EndorsementType { get; set; }


        /// <summary>
        /// Id Tipo de Endoso
        /// </summary>             
        public string EndorsementTypeId { get; set; }

        /// <summary>
        /// Id Temporal
        /// </summary>             
        public string TempId { get; set; }

        /// <summary>
        /// Id JsonId
        /// </summary>             
        public string JsonId { get; set; }


        /// <summary>
        /// policyId
        /// </summary>
        public int PolicyId { get; set; }

        /// <summary>
        /// endorsementId
        /// </summary>
        public int EndorsementId { get; set; }

        /// <summary>
        /// policyNum
        /// </summary>
        public int PolicyNum { get; set; }

        public List<int> CollectiveLoads { get; set; }

        public bool IsAutomatic { get; set; }
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
        /// Id de Usuario
        /// </summary>
        public int? UserId { get; set; }

    }
}