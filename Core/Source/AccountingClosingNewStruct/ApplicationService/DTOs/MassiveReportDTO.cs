using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.AccountingClosingServices.DTOs
{
    [DataContract]
    public class MassiveReportDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public DateTime GenerationDate { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public DateTime StartDate { get; set; }

        [DataMember]
        public DateTime EndDate { get; set; }

        [DataMember]
        public string UrlFile { get; set; }

        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public int ModuleId { get; set; }

        [DataMember]
        public int RecordsNumber { get; set; }

        [DataMember]
        public int RecordsProcessed { get; set; }
    }
}
