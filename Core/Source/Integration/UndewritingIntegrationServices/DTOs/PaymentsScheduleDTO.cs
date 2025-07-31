using System.Runtime.Serialization;

namespace Sistran.Core.Integration.UndewritingIntegrationServices.DTOs
{
    [DataContract]
    public class PaymentsScheduleDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public bool IsGreaterDate { get; set; }

        [DataMember]
        public bool IsIssueDate { get; set; }

        [DataMember]
        public int QuotasNumber { get; set; }

        [DataMember]
        public short CalculationUnit { get; set; }

        [DataMember]
        public short CalculationQuantity { get; set; }

    }
}
