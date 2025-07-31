using System.Collections.Generic;
using System.Runtime.Serialization;
using Sistran.Core.Services.UtilitiesServices.Models;
using Sistran.Core.Application.UniqueUserServices.Models;

namespace Sistran.Company.Application.UniquePersonListRiskBusinessService.Model
{
    [DataContract]
    public class CompanyListRiskMassiveLoad
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int ProcessId { get; set; }

        [DataMember]
        public int TotalRows { get; set; }

        [DataMember]
        public int ListRiskId { get; set; }

        [DataMember]
        public string ListRiskDescription { get; set; }

        [DataMember]
        public User User { get; set; }

        [DataMember]
        public File File { get; set; }

        [DataMember]
        public int EnableProcessing { get; set; }

        [DataMember]
        public List<CompanyListRiskRow> Rows { get; set; }
    }
}
