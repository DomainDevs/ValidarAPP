using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.CommonService.Models.Base
{
    [DataContract]
    public class BaseValidation : Extension
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int ValidationId { get; set; }

        [DataMember]
        public int? AdditionalRow { get; set; }

        [DataMember]
        public string FieldDescription { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }
    }
}
