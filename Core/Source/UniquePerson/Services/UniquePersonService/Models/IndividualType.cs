using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonServiceIndividual.Models

{
    [DataContract]
    public class IndividualType : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }
        /// <summary>
        /// Tipo de Persona
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        /// <summary>
        /// Abreviatura de tipo de Persona
        /// </summary>
        public string SmallDescription { get; set; }

    }
}
