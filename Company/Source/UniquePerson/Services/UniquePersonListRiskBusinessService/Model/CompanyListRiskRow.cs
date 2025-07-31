
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniquePersonListRiskBusinessService.Model
{
    [DataContract]
    public class CompanyListRiskRow
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int ProcessMassiveLoadId { get; set; }

        [DataMember]
        public int ProcessId { get; set; }

        [DataMember]
        public int RowNumber { get; set; }

        [DataMember]
        public bool HasError { get; set; }

        [DataMember]
        public string IdCardNo { get; set; }

        [DataMember]
        public string Error_Description { get; set; }

        [DataMember]
        public string SerializedRow { get; set; }
    }
}
