using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingClosingServices.EEProvider.Models.GeneralLedger.AccountingRules
{
    [DataContract]
    public class AccountingAccountMask
    {
        /// <summary>
        ///     Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Mask: Cadena de Formato del campo
        /// </summary>        
        [DataMember]
        public string Mask { get; set; }

        /// <summary>
        /// Start: Posicion Inicial 
        /// </summary>        
        [DataMember]
        public int Start { get; set; }

        /// <summary>
        ///     Parameter : Valor del parametro de ingreso
        /// </summary>
        [DataMember]
        public Parameter Parameter { get; set; }
    }
}