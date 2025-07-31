using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Enums;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    /// <summary>
    /// Modelo de Ramos Reaseguros
    /// </summary>
    [DataContract]
    public class ReinsurancePrefix
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
        public Prefix Prefix { get; set; }

        /// <summary>
        /// Ramo Cumulo
        /// </summary>
        [DataMember]
        public Prefix PrefixCumulus { get; set; }

        /// <summary>
        /// Tipo de Ejercicio
        /// </summary>
        [DataMember]
        public ExerciseTypes ExerciseType { get; set; }

        /// <summary>
        /// Localizacion
        /// </summary>
        [DataMember]
        public bool IsLocation { get; set; }
    }
}