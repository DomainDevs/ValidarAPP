using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    [DataContract]
    public class InsuredObject
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string SmallDescription { get; set; }

        [DataMember]
        public bool IsDeclarative { get; set; }

        [DataMember]
        public bool? IsMandatory { get; set; }

        [DataMember]
        public decimal Premium { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public bool? IsSelected { get; set; }

        [DataMember]
        public int ParametrizationStatus { get; set; }
    }
}
