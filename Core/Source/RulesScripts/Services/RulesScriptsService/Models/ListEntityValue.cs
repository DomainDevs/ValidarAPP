using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    using ModelServices.Models.Param;

    [DataContract]
    public class ListEntityValue: ParametricServiceModel
    {
        /// <summary>
        /// Codigo del Valor de la Lista
        /// </summary>
        [DataMember]
        public int ListValueCode { get; set; }

        /// <summary>
        /// Codigo de Listado de Entidades
        /// </summary>
        [DataMember]
        public int ListEntityCode { get; set; }

        /// <summary>
        /// Valor de la Lista
        /// </summary>
        [DataMember]
        public string ListValue { get; set; }
    }
}
