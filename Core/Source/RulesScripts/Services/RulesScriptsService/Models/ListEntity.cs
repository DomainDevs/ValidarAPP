using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    using ModelServices.Models.Param;

    [DataContract]
    public class ListEntity: ParametricServiceModel
    {
        /// <summary>
        /// Codigo de Listado de Entidades
        /// </summary>
        [DataMember]
        public int ListEntityCode { get; set; }

        /// <summary>
        /// Codigo de Listado de Entidades
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Lista de Valores
        /// </summary>
        [DataMember]
        public int ListEntityAt { get; set; }

        /// <summary>
        /// Lista de Valores
        /// </summary>
        [DataMember]
        public List<ListEntityValue> ListEntityValue { get; set; }
    }
}
