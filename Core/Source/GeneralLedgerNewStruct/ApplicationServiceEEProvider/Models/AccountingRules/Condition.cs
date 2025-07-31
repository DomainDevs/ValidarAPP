using System.Runtime.Serialization;

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models.AccountingRules
{
    [DataContract]
    public class Condition
    {
        /// <summary>
        ///     Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        ///     AccountingRule
        /// </summary>
        [DataMember]
        public AccountingRule AccountingRule { get; set; }

        /// <summary>
        ///     Parametro
        /// </summary>
        [DataMember]
        public Parameter Parameter { get; set; }

        /// <summary>
        ///     Operador
        /// </summary>
        [DataMember]
        public string Operator { get; set; } // Se debe convertir en un modelo dentro de Core.Param

        /// <summary>
        ///     Valor
        /// </summary>
        [DataMember]
        public decimal Value { get; set; }

        /// <summary>
        ///     Id Resultado Derecho
        /// </summary>
        [DataMember]
        public int IdRightCondition { get; set; }

        /// <summary>
        ///     Id Resultado Izquiero
        /// </summary>
        [DataMember]
        public int IdLeftCondition { get; set; }

        /// <summary>
        ///     Id del Resultado
        /// </summary>
        [DataMember]
        public int IdResult { get; set; }
    }
}