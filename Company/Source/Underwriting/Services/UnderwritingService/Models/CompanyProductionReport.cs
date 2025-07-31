using Sistran.Core.Application.CommonService.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Company.Application.UnderwritingServices.Models
{
    [DataContract]
    public class CompanyProductionReport
    {
        /// <summary>
        /// Id Sucursal
        /// </summary>
        [DataMember]
        public int BranchId { get; set; }

        /// <summary>
        /// Descripción de Sucursal
        /// /// </summary>
        [DataMember]
        public string BranchDescription { get; set; }

        /// <summary>
        /// Descripcion de punto de venta
        /// </summary>
        [DataMember]
        public string SalePointDescription { get; set; }

        /// <summary>
        /// Id del agente
        /// </summary>
        [DataMember]
        public int AgentId { get; set; }

        /// <summary>
        /// Poliza Id
        /// </summary>
        [DataMember]
        public int PolicyId { get; set; }

        /// <summary>
        /// Endosos Id
        /// </summary>
        [DataMember]
        public int EndorsementId { get; set; }

        /// <summary>
        /// Grupo de endosos
        /// </summary>
        [DataMember]
        public string GroupEndorsement { get; set; }

        /// <summary>
        /// Placa de vehiculo
        /// </summary>
        [DataMember]
        public string Plate { get; set; }

        /// <summary>
        /// Plan de Cobertura
        /// </summary>
        [DataMember]
        public string CoveragePlan { get; set; }

        /// <summary>
        /// Código de FaseColda
        /// </summary>
        [DataMember]
        public int FasecoldaCode { get; set; }

        /// <summary>
        /// Comision
        /// </summary>
        [DataMember]
        public decimal Comision { get; set; }

        /// <summary>
        /// Valor Asegurado
        /// </summary>
        [DataMember]
        public string InsuredValue { get; set; }

        /// <summary>
        /// Prima
        /// </summary>
        [DataMember]
        public string Prime { get; set; }

        /// <summary>
        /// Fecha de Expedición
        /// </summary>
        [DataMember]
        public string DateExpedition { get; set; }

        /// <summary>
        /// Fecha desde
        /// </summary>
        [DataMember]
        public string DateFrom { get; set; }

        /// <summary>
        /// Fecha hasta
        /// </summary>
        [DataMember]
        public string DateTo { get; set; }

        /// <summary>
        /// Id Sucursal
        /// </summary>
        [DataMember]
        public int  PrefixId { get; set; }

        /// <summary>
        /// Id Sucursal
        /// </summary>
        [DataMember]
        public int ProductId { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public DateTime InputFromDateTime { get; set; }
        [DataMember]
        public DateTime InputToDateTime { get; set; }

    }
}
