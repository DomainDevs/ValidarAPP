using Sistran.Core.Application.AccountingServices.EEProvider.Models.Underwriting.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Underwriting
{
    [DataContract]
    public class IssuanceCompanyName : BaseIssuanceCompanyName
    {
        [DataMember]
        public IssuanceAddress Address { get; set; }

        [DataMember]
        public IssuancePhone Phone { get; set; }

        [DataMember]
        public IssuanceEmail Email { get; set; }
    }
}