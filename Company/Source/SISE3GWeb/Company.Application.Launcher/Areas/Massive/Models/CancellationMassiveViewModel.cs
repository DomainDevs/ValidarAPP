using Sistran.Core.Application.UnderwritingServices.Enums;
using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Massive.Models
{
    public class CancellationMassiveViewModel
    {
        /// <summary>
        /// Identificador
        /// </summary>
        public int? LoadId { get; set; }

        /// <summary>
        /// Nombre Cargue
        /// </summary>      
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
        /// Id de Usuario
        /// </summary>
        public int? UserId { get; set; }

    }
}