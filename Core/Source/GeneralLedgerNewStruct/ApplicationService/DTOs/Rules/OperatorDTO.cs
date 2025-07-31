using System.Runtime.Serialization;


namespace Sistran.Core.Application.GeneralLedgerServices.DTOs.Rules
{
    [DataContract]
    public class OperatorDTO 
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string OperationSign { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}