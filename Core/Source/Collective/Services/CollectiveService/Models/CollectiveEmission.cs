using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveServices.Models;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Core.Application.CollectiveServices.Models
{
    [DataContract]
    public class CollectiveEmission : MassiveLoad
    {
        [DataMember]
        public int TemporalId { get; set; }
        [DataMember]
        public bool IsAutomatic { get; set; }
        [DataMember]
        public bool HasEvents { get; set; }
        [DataMember]
        public Prefix Prefix { get; set; }
        [DataMember]
        public Branch Branch { get; set; }

        [DataMember]
        public IssuanceAgency Agency { get; set; }

        [DataMember]
        public ProductServices.Models.Product Product { get; set; }
        [DataMember]
        public decimal? DocumentNumber { get; set; }
        [DataMember]
        public decimal? Premium { get; set; }
        [DataMember]
        public decimal? Commiss { get; set; }
        [DataMember]
        public List<CollectiveEmissionRow> Rows { get; set; }

        [DataMember]
        public int? EndorsementNumber { get; set; }
        [DataMember]
        public CoveredRiskType? CoveredRiskType { get; set; }
        [DataMember]
        public int? EndorsementId { get; set; }
    }
}