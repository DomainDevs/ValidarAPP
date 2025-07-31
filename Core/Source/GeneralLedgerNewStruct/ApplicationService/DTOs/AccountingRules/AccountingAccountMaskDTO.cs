using System.Runtime.Serialization;


namespace Sistran.Core.Application.GeneralLedgerServices.DTOs.AccountingRules
{
    [DataContract]
    public class AccountingAccountMaskDTO
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
        public ParameterDTO Parameter { get; set; }
    }
}