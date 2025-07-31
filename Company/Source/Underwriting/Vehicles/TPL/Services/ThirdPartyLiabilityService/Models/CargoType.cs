using System.Runtime.Serialization;

namespace Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.Models
{
    [DataContract]
    public class CargoType
    {
            /// <summary>
            /// Identificador del tipo de mercancía
            /// </summary>
            [DataMember]
            public int Id { get; set; }

            /// <summary>
            /// Descripción del tipo de mercancía
            /// </summary>
            [DataMember]
            public string Description { get; set; }

          
    }
  
}
