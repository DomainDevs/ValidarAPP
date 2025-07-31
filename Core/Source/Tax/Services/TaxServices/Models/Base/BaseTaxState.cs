using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.TaxServices.Models.Base
{
    [DataContract]
    public class BaseTaxState
    {
        [DataMember]
        public int? IdState { get; set; }

        [DataMember]
        public int? IdCity { get; set; }

        [DataMember]
        public int? IdCountry{ get; set; }

        [DataMember]
        public string DescriptionState { get; set; }

        [DataMember]
        public string DescriptionCity { get; set; }

        [DataMember]
        public string DescriptionCountry { get; set; }

    }
}
