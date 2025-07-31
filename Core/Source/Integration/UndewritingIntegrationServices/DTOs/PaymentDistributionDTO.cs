using System.Runtime.Serialization;

namespace Sistran.Core.Integration.UndewritingIntegrationServices.DTOs
{
    [DataContract]
    public class PaymentDistributionDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public short CalculationUnit { get; set; }

        [DataMember]
        public short CalculationQuantity { get; set; }

        [DataMember]
        public int Number { get; set; }

        [DataMember]
        public decimal Percentage { get; set; }


    }
}
