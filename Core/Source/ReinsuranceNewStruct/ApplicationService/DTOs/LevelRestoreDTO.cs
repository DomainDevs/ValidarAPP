using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    /// <summary>
    /// Modelo para Restablecer de Nivel de contrato
    /// </summary>
    [DataContract]
    public class LevelRestoreDTO
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Level
        /// </summary>
        [DataMember]
        public LevelDTO Level { get; set; }

        /// <summary>
        /// Número 
        /// </summary>
        [DataMember]
        public int Number { get; set; }

        /// <summary>
        /// Porcentaje Restablecimiento
        /// </summary>
        [DataMember]
        public decimal RestorePercentage { get; set; }

        /// <summary>
        /// Porcentaje de Aviso
        /// </summary>
        [DataMember]
        public decimal NoticePercentage { get; set; }
    }
}