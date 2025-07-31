using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.QuotationServices.Models
{
    [DataContract]
    public class CondTextLevelModel
    {
        [DataMember]
        public int  ConditionTextIdCod { get; set; }
        [DataMember]
        public int CondTextLevelId { get; set; }
        [DataMember]
        public int? ConditionLevelId { get; set; }
        [DataMember]
        public bool IsAutomatic { get; set; }
        [DataMember]
        public ConditionTextModel condition { get; set; }
    }
}
