using Sistran.Core.Application.MassiveServices.Models;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.CollectiveServices.Models.Base
{
    [DataContract]
    public class BaseCollectiveEmission : MassiveLoad
    {
        [DataMember]
        public int TemporalId { get; set; }
        [DataMember]
        public bool IsAutomatic { get; set; }
        [DataMember]
        public bool? HasEvents { get; set; }
        [DataMember]
        public decimal? DocumentNumber { get; set; }
        [DataMember]
        public decimal? Premium { get; set; }
        [DataMember]
        public decimal? Commiss { get; set; }
        [DataMember]
        public int? EndorsementNumber { get; set; }
        [DataMember]
        public int? EndorsementId { get; set; }
    }
}
