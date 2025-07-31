using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;



namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{

    [DataContract]
    public class CurrencyDiferenceDTO 
    {
        [DataMember]
        public int CurrencyId { get; set; }
        [DataMember]
        public double MaximumDiference { get; set; }
        [DataMember]
        public double PercentageDiference { get; set; }

    }
}

