using System.Runtime.Serialization;

namespace Sistran.Company.Application.MassiveVehicleParamBusinessService.Model
{
    [DataContract]
    public class CompanyFasecoldaPrice
    {
        [DataMember]
        public string Codigo { get; set; }

        [DataMember]
        public string Modelo { get; set; }

        [DataMember]
        public decimal Valor { get; set; }
    }
}
