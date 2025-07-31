using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.ProductServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.MassiveUnderwritingServices.Models
{
    [DataContract]
    public class MassiveEmission : MassiveLoad
    {
        /// <summary>
        /// Id Grupo De Facturación
        /// </summary>
        [DataMember]
        public int? BillingGroupId { get; set; }

        /// <summary>
        /// Número Solicitud Agrupadora
        /// </summary>
        [DataMember]
        public int? RequestId { get; set; }

        /// <summary>
        /// Número Solicitud Agrupadora
        /// </summary>
        [DataMember]
        public int? RequestNumber { get; set; }

        /// <summary>
        /// Agencia
        /// </summary>
        [DataMember]
        public IssuanceAgency Agency { get; set; }

        /// <summary>
        /// Ramo Comercial
        /// </summary>
        [DataMember]
        public Prefix Prefix { get; set; }

        /// <summary>
        /// Producto
        /// </summary>
        [DataMember]
        public Product Product { get; set; }

        /// <summary>
        /// Sucursal
        /// </summary>
        [DataMember]
        public Branch Branch { get; set; }

        /// <summary>
        /// Tipo De Negocio
        /// </summary>
        [DataMember]
        public BusinessType? BusinessType { get; set; }

        [DataMember]
        public List<MassiveEmissionRow> Rows { get; set; }

        /// <summary>
        /// Id endoso
        /// </summary>
        [DataMember]
        public int EndorsementId { get; set; }

        /// <summary>
        /// Id de CoveredRiskType
        /// </summary>
        [DataMember]
        public CommonService.Enums.CoveredRiskType? CoveredRiskType { get; set; }
    }
}