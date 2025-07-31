using System.Runtime.Serialization;

namespace Sistran.Company.ExternalPrinterServices.Models
{
    [DataContract]
    public class GenerateWSPolicyPrinterResponse
    {
        [DataMember]
        public int BranchId { get; set; }
        [DataMember]
        public int PrefixNum { get; set; }
        [DataMember]
        public int DocumentNumber { get; set; }
        [DataMember]
        public int EndorsementId { get; set; }
        [DataMember]
        public int emailUser { get; set; }
        [DataMember]
        public int YearEndorsement { get; set; }
        [DataMember]
        public int AllInformation { get; set; }
        [DataMember]
        public int FrontPage { get; set; }
        [DataMember]
        public int PaymentFormat { get; set; }
        [DataMember]
        public int AllInsured { get; set; }
        [DataMember]
        public int Payment { get; set; }
        [DataMember]
        public int Coverages { get; set; }
        [DataMember]
        public int Annexes { get; set; }
        [DataMember]
        public int Fee { get; set; }
        [DataMember]
        public int Clauses { get; set; }
        [DataMember]
        public int Certificate { get; set; }
        [DataMember]
        public int AllCertificate { get; set; }
        [DataMember]
        public int CertificateSince { get; set; }
        [DataMember]
        public int CertificateUntil { get; set; }
        [DataMember]
        public int LetterInstruction { get; set; }
        [DataMember]
        public int InsuranceObject { get; set; }
        [DataMember]
        public int printBinary { get; set; }
        [DataMember]
        public string ProcessMessage { get; set; }
    }
}
