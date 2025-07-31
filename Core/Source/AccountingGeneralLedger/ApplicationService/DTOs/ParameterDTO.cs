using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.AccountingGeneralLedgerServices.DTOs
{
    [DataContract]
    public class ParameterDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int Order { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Value { get; set; }
        [DataMember]
        public string DataType { get; set; }
        [DataMember]
        public int ModuleDateId { get; set; }
    }
}
