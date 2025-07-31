using Sistran.Core.Application.Sureties.JudicialSuretyServices.Enums;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Sureties.JudicialSuretyServices.Models.Base
{
    /// <summary>
    /// Caucion Judicial
    /// </summary>
    [DataContract]
    public class BaseJudgement
    {
        /// <summary>
        /// Proceso y/o Radicado
        /// </summary>
        [DataMember]
        public string SettledNumber { get; set; }

        /// <summary>
        /// Valor Asegurado
        /// </summary>
        [DataMember]
        public decimal InsuredValue { get; set; }

        /// <summary>
        /// Tasa
        /// </summary>
        [DataMember]
        public decimal Rate { get; set; }

        /// <summary>
        /// Precio
        /// </summary>
        [DataMember]
        public decimal Price { get; set; }


        /// <summary>
        /// Datos de asegurador actua como
        /// </summary>
        [DataMember]
        public CapacityOf InsuredActAs { get; set; }

        /// <summary>
        /// Datos de Tomador actua como
        /// </summary>
        [DataMember]
        public CapacityOf HolderActAs { get; set; }
    }
}
