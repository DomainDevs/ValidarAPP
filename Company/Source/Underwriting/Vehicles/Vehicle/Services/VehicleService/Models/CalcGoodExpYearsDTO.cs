using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Vehicles.VehicleServices.Models
{
    [Serializable]
    [DataContract]
    public class CalcGoodExpYearsDTO
    {
        [DataMember]
        public HistoricoPolizaCexper HistoryPolicy { get; set; }
        [DataMember]
        public int PolicyWithSiniester { get; set; }
        [DataMember]
        public int YearsWithPolicy { get; set; }
    }
}
