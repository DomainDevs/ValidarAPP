using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.V1.Models.Base
{
    [DataContract]
    public class BaseExonerationType : BaseGeneric
    {

        [DataMember]
        public int IndividualTypeCode { get; set; }

        [DataMember]
        public bool Enabled { get; set; }

        [DataMember]
        public int RoleId { get; set; }
    }
}
