using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    [DataContract]
    public class IndividualType 
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
