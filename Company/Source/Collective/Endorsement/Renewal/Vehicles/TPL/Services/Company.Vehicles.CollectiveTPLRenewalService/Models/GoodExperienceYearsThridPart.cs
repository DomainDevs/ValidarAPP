//using Sistran.Company.Application.CommonServices.ConsumerWebServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.Vehicles.CollectiveTPLRenewalService.Models
{
    [DataContract]
    public class GoodExperienceYearsThridPart
    {
        //[DataMember]
        //public HistoricoPolizaCexper[]
        //HistoricoPolizaCexperCollection
        //{ get; set; }

        //[DataMember]
        //public HistoricoSiniestroCexper[]
        //HistoricoSiniestroCexperCollection
        //{ get; set; }

        [DataMember]
        public int IdCardTypeCode { get; set; }

        [DataMember]
        public string IdCardNo { get; set; }
    }
}
