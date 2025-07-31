using Sistran.Core.Services.UtilitiesServices.Enums;
using System.Runtime.Serialization;

namespace Sistran.Core.Services.UtilitiesServices.Models
{
    /// <summary>
    /// Modelo Generico para todos los modelos de los ABM 
    /// </summary>
    [DataContract]
    public class Parametrization
    {

        [DataMember]
        public ParametrizationStatus Status { get; set; }

    }
}
