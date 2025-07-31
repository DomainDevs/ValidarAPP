using Sistran.Core.Application.CommonService.Models;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingClosingServices.EEProvider.Models.GeneralLedger.AccountingRules
{
    [DataContract]
    public class Parameter
    {
        /// <summary>
        ///     Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        ///     Id
        /// </summary>
        [DataMember]
        public int Order { get; set; }

        /// <summary>
        ///     Descripción
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        ///     Valor del parametro
        /// </summary>
        [DataMember]
        public string Value { get; set; }

        /// <summary>
        ///     DataType : Se debe convertir en un modelo dentro de Core.Param
        /// </summary>
        [DataMember]
        public string DataType { get; set; }

        /// <summary>
        ///     Modulo
        /// </summary>
        [DataMember]
        public int ModuleDateId { get; set; }
    }
}