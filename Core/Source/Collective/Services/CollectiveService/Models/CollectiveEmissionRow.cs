using Sistran.Core.Application.CollectiveServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.CollectiveServices.Models
{
    [DataContract]
    public class CollectiveEmissionRow
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int MassiveLoadId { get; set; }

        [DataMember]
        public int RowNumber { get; set; }

        [DataMember]
        public Risk Risk { get; set; }

        [DataMember]
        public CollectiveLoadProcessStatus Status { get; set; }

        [DataMember]
        public bool HasError { get; set; }

        [DataMember]
        public bool HasEvents { get; set; }

        [DataMember]
        public string Observations { get; set; }

        [DataMember]
        public string SerializedRow { get; set; }

        [DataMember]
        public decimal? Premium { get; set; }
    }
}
