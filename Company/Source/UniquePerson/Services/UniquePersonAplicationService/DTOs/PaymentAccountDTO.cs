using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonAplicationServices.DTOs
{
    [DataContract]
    public class PaymentAccountDTO
    {

        [DataMember]
        public string Number { get; set; }

        [DataMember]
        public SelectDTO BankBranch { get; set; }

        [DataMember]
        public SelectDTO Currency { get; set; }

        [DataMember]
        public SelectDTO Bank { get; set; }

        [DataMember]
        public SelectDTO Type { get; set; }

    }
}
