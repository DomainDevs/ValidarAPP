using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    /// <summary>
    /// Modelo de Ramos Reaseguros
    /// </summary>
    [DataContract]
    public class ReinsurancePrefixDTO
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Ramo
        /// </summary>
        [DataMember]
        public PrefixDTO Prefix { get; set; }

        /// <summary>
        /// Ramo Cumulo
        /// </summary>
        [DataMember]
        public PrefixDTO PrefixCumulus { get; set; }

        /// <summary>
        /// Tipo de Ejercicio
        /// </summary>
        [DataMember]
        public int ExerciseType { get; set; }

        /// <summary>
        /// Localizacion
        /// </summary>
        [DataMember]
        public bool IsLocation { get; set; }
    }
}
