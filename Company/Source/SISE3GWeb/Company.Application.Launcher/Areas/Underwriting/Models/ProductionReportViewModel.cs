using System;
using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Models
{
    public class ProductionReportViewModel
    {
        /// <summary>
        /// Id Sucursal
        /// </summary>
        public int BranchId { get; set; }

        /// <summary>
        /// Descripción de Sucursal
        /// /// </summary>
        public string BranchDescription { get; set; }

        /// <summary>
        /// Id de punto de venta
        /// </summary>
        public int SalePointId { get; set; }

        /// <summary>
        /// Descripcion de punto de venta
        /// </summary>
        public string SalePointDescription { get; set; }

        /// <summary>
        /// Id del agente
        /// </summary>
        public int AgentId { get; set; }

        /// <summary>
        /// Poliza Id
        /// </summary>
        public int PolicyId { get; set; }

        /// <summary>
        /// Endosos Id
        /// </summary>
        public int EndorsementId { get; set; }

        /// <summary>
        /// Grupo de endosos
        /// </summary>
        public string GroupEndorsement { get; set; }

        /// <summary>
        /// Placa de vehiculo
        /// </summary>
        public string Plate { get; set; }

        /// <summary>
        /// Plan de Cobertura
        /// </summary>
        public string CoveragePlan { get; set; }

        /// <summary>
        /// Código de FaseColda
        /// </summary>
        public int FasecoldaCode { get; set; }

        /// <summary>
        /// Comision
        /// </summary>
        public decimal Comision { get; set; }

        /// <summary>
        /// Valor Asegurado
        /// </summary>
        public decimal InsuredValue { get; set; }

        /// <summary>
        /// Prima
        /// </summary>
        public decimal Prime { get; set; }

        /// <summary>
        /// Fecha de Expedición
        /// </summary>
        public string DateExpedition { get; set; }

        /// <summary>
        /// Fecha desde
        /// </summary>
        public string DateFrom { get; set; }

        /// <summary>
        /// Fecha hasta
        /// </summary>
        public string DateTo { get; set; }

        /// <summary>
        /// Id Sucursal
        /// </summary>
        public int PrefixId { get; set; }

        /// <summary>
        /// Id Sucursal
        /// </summary>
        public int ProductId { get; set; }

        public int UserId { get; set; }

        public DateTime InputToDateTime { get; set; }
        public DateTime InputFromDateTime { get; set; }
    }
}