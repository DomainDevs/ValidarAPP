using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.SecurityServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Massive.Models
{
    public class MassiveProcessViewModel
    {
        /// <summary>
        /// Identificador
        /// </summary>
        public int? LoadId { get; set; }

        /// <summary>
        /// Tipo de Proceso
        /// </summary>
        public ProcessTypes ProcessType { get; set; }

        /// <summary>
        /// Descripcion del Tipo de Proceso
        /// </summary>
        public string ProcessTypeDescription { get; set; }

        /// <summary>
        /// Estado
        /// </summary>
        public MassiveLoadStatus Status { get; set; }

        /// <summary>
        /// Descripción Estado
        /// </summary>
        public string StatusDescription { get; set; }

        /// <summary>
        /// Descripción del Cargue
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Fecha Desde 
        /// </summary>     
        [Display(Name = "LabelFrom", ResourceType = typeof(App_GlobalResources.Language))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime CurrentFrom { get; set; }

        /// <summary>
        /// Fecha Hasta 
        /// </summary>  
        [Display(Name = "LabelTo", ResourceType = typeof(App_GlobalResources.Language))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime CurrentTo { get; set; }

        /// <summary>
        /// Id Usuario
        /// </summary>
        public User User { get; set; }

    }
}