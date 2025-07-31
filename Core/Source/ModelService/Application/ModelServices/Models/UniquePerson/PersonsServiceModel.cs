namespace Sistran.Core.Application.ModelServices.Models
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Contiene las propiedades de personas
    /// </summary>
    [DataContract]
    public class PersonsServiceModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece una lista de PersonServiceModel
        /// </summary>
        [DataMember]
        public List<PersonServiceModel> PersonServiceModel { get; set; }

    }
}
