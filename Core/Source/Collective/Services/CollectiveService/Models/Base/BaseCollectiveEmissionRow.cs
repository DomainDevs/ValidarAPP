using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.CollectiveServices.Models.Base
{
    [DataContract]
    public class BaseCollectiveEmissionRow : Extension
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int MassiveLoadId { get; set; }

        [DataMember]
        public int RowNumber { get; set; }
        [DataMember]
        public bool? HasError { get; set; }

        [DataMember]
        public bool? HasEvents { get; set; }

        [DataMember]
        public string Observations { get; set; }

        [DataMember]
        public string SerializedRow { get; set; }

        [DataMember]
        public decimal? Premium { get; set; }
    }
}
