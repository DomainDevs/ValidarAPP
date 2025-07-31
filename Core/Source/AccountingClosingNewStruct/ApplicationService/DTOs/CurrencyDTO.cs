using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingClosingServices.DTOs
{
    [DataContract]
    public class CurrencyDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Description { get; set; }
    }
}
