using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs
{
    [DataContract]
    public class PersonIndividualTypeDTO
    {
        [DataMember]
        public int IndividualId { get; set; }
        [DataMember]
        public int PersonTypeCode { get; set; }
    }
}
