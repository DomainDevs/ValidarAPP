using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    using ModelServices.Models.Param;

    [DataContract]
    public class RangeEntityValue: ParametricServiceModel
    {
        /// <summary>
        /// Codigo de Entidad tipo Rango
        /// </summary>
        [DataMember]
        public int RangeEntityCode { get; set; }        

        /// <summary>
        /// Codigo de Entidad valores de Rango
        /// </summary>        
        [DataMember]
        public int RangeValueCode { get; set; }

        /// <summary>
        /// Desde el Valor
        /// </summary>        
        [DataMember]
        public int FromValue { get; set; }

        /// <summary>
        /// Hasta el Valor
        /// </summary>        
        [DataMember]
        public int ToValue { get; set; }
    }
}
