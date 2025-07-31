using System;
using System.Runtime.Serialization;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.MassiveServices.Models;
using System.Collections.Generic;
using Sistran.Core.Application.ProductServices.Models;

namespace Sistran.Core.Application.MassiveRenewalServices.Models
{
    [DataContract]
    public class MassiveRenewal : MassiveLoad
    {
        /// <summary>
        /// Número Solicitud Agrupadora
        /// </summary>
        [DataMember]
        public int? RequestId { get; set; }

        /// <summary>
        /// Agencia
        /// </summary>
        [DataMember]
        public IssuanceAgency Agency { get; set; }

             /// <summary>
        /// Ramo comercial
        /// </summary>
        [DataMember]
        public Prefix Prefix { get; set; }

        /// <summary>
        /// Agencia
        /// </summary>
        [DataMember]
        public Branch Branch { get; set; }

        /// <summary>
        /// Producto
        /// </summary>
        [DataMember]
        public Product Product { get; set; }

        /// <summary>
        /// Tommador
        /// </summary>
        [DataMember]
        public Holder Hoder { get; set; }

        /// <summary>
        /// Inicio
        /// </summary>
        [DataMember]
        public DateTime? CurrentFrom { get; set; }

        /// <summary>
        /// Final
        /// </summary>
        [DataMember]
        public DateTime? CurrentTo { get; set; }

        [DataMember]
        public List<MassiveRenewalRow> Rows { get; set; }

        /// <summary>
        /// Id de CoveredRiskType
        /// </summary>
        [DataMember]
        public CommonService.Enums.CoveredRiskType? CoveredRiskType { get; set; }
    }
}
