using System.Runtime.Serialization;

namespace Sistran.Core.Services.UtilitiesServices.Models
{
    /// <summary>
    /// Modelo generico para retornar el resultado de las operaciones de los ABM
    /// </summary>
    [DataContract]
    public class ParametrizationResult
    {
        [DataMember]
        public int TotalAdded { get; set; }

        [DataMember]
        public int TotalModified { get; set; }

        [DataMember]
        public int TotalDeleted { get; set; }

        [DataMember]
        public string Message { get; set; }
    }
}
