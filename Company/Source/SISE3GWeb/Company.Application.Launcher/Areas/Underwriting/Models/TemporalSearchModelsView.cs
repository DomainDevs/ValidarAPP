using System;
using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Models
{
    public class TemporalSearchModelsView
    {
        /// <summary>
        /// Identificador Temporario
        /// </summary>        
        public string NumberTemporary { get; set; }

        /// <summary>
        /// Numero de Poliza
        /// </summary>
        public string PolicyNumber { get; set; }

        /// <summary>
        /// Ramo comercial
        /// </summary>
        public string PrefixCommercial { get; set; }

        /// <summary>
        /// Asegurado
        /// </summary>
        public string Insured { get; set; }

        /// <summary>
        /// Sucursal
        /// </summary>
        public string Branch { get; set; }

        /// <summary>
        /// Usuario
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// fecha Consulta
        /// </summary>
        public DateTime ConsultationDate { get; set; }

        /// <summary>
        /// Dias
        /// </summary>
        public int Days { get; set; }

        /// <summary>
        /// Intermediario principal
        /// </summary>
        public string AgentPrincipal { get; set; }

        /// <summary>
        /// Tipo de Transaccion
        /// </summary>
        public string TypeTransaction { get; set; }
    }
}