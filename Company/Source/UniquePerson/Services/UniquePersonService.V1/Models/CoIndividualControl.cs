using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    [DataContract]
    public class CoIndividualControl
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Identificador de la persona
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// Identificador del periferico
        /// </summary>
        [DataMember]
        public int PerifericoId { get; set; }

        /// <summary>
        /// Identificador Proceso
        /// </summary>
        [DataMember]
        public int ProcessId { get; set; }
    }
}
