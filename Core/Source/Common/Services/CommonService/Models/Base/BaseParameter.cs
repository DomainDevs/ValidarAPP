using Sistran.Core.Application.Extensions;
using System;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.CommonService.Models.Base
{
    [DataContract]
    public class BaseParameter : Extension
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public bool? BoolParameter { get; set; }
        [DataMember]
        public DateTime? DateParameter { get; set; }
        [DataMember]
        public int? NumberParameter { get; set; }
        [DataMember]
        public string TextParameter { get; set; }
        [DataMember]
        public decimal? PercentageParameter { get; set; }
        [DataMember]
        public decimal? AmountParameter { get; set; }
        [DataMember]
        public object Value { get; set; }
    }
}
