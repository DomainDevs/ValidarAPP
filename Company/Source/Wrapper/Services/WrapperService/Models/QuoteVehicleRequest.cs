
using System.Runtime.Serialization;

namespace Sistran.Company.Application.WrapperServices.Models
{
    [DataContract]
    public class QuoteVehicleRequest
    {
        [DataMember]
        public int TemporalId { get; set; }
        [DataMember]
        public int QuotationId { get; set; }
        [DataMember]
        public int PrefixId { get; set; }
        [DataMember]
        public int ProductId { get; set; }
        [DataMember]
        public int GroupCoverageId { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public int CustomerId { get; set; }
        [DataMember]
        public int CustomerTypeId { get; set; }
        [DataMember]
        public string Plate { get; set; }
        [DataMember]
        public string FasecoldaCode { get; set; }
        [DataMember]
        public int MakeId { get; set; }
        [DataMember]
        public int ModelId { get; set; }
        [DataMember]
        public int VersionId { get; set; }
        [DataMember]
        public int TypeId { get; set; }
        [DataMember]
        public int UseId { get; set; }
        [DataMember]
        public int Year { get; set; }
        [DataMember]
        public decimal Price { get; set; }
        [DataMember]
        public decimal Rate { get; set; }
        [DataMember]
        public int RatingZoneId { get; set; }
    }
}