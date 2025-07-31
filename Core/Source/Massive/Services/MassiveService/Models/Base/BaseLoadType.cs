using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.MassiveServices.Models.Base
{
    [DataContract]
    public class BaseLoadType : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Descripción
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Abreviatura
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }

        /// <summary>
        /// Descripción del Tipo de Proceso
        /// </summary>
        [DataMember]
        public string ProcessTypeDescription { get; set; }
    }
}
