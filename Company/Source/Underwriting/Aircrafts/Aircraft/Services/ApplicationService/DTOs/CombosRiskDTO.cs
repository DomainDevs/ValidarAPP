using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Aircrafts.AircraftApplicationService.DTOs
{
    /// <summary>
    /// modelo para poliza de Aircrafte
    /// </summary>
    [DataContract]
    public class CombosRiskDTO
    {
        /// <summary>
        /// Marcas del riesgo
        /// </summary>
        [DataMember]
        public List<MakeDTO> Makes { get; set; }

        /// <summary>
        /// Operadores del riesgo
        /// </summary>
        [DataMember]
        public List<OperatorDTO> Operators { get; set; }

        /// <summary>
        /// Registros del riesgo
        /// </summary>
        [DataMember]
        public List<RegisterDTO> Registers { get; set; }

        /// <summary>
        /// Type del riesgo
        /// </summary>
        [DataMember]
        public List<AircraftTypeDTO> Types { get; set; }

        /// <summary>
        /// Usos del riesgo
        /// </summary>
        [DataMember]
        public List<UseDTO> Uses { get; set; }
    }
}
