using Sistran.Core.Application.AccountingServices.DTOs.Search;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Imputations
{
    /// <summary>
    /// CoInsuranceCheckingAccountTransactionItem:  Transaccion de Cuenta Corriente Coaseguros
    /// </summary>
    [DataContract]
    public class CoInsuranceCheckingAccountTransactionItemDTO : CheckingAccountTransactionDTO
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Holder:  Compania Reaseguradora
        /// </summary>        
        [DataMember]
        public new CompanyDTO Holder { get; set; }

        /// <summary>
        /// CoInsuranceType : Tipo de Coaseguro
        /// </summary>        
        [DataMember]
        public int CoInsuranceType { get; set; }

        /// <summary>
        /// CoInsuranceCheckingAccountTransactionChild: Relacion Cuentas Corrientes CoAseguros
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public List<CoInsuranceCheckingAccountTransactionItemDTO> CoInsuranceCheckingAccountTransactionChild { get; set; }

        /// <summary>
        /// IsAutomatic:  Si se genera de manera automatica el registro de cuenta corriente
        /// </summary>        
        [DataMember]
        public bool IsAutomatic { get; set; }

        /// <summary>
        /// Policy : Poliza, campo para transaccion automatica
        /// </summary>        
        [DataMember]
        public PolicyDTO Policy { get; set; }

        /// <summary>
        /// CliamsId : Numero de Solicitud de Siniestro, campo para transaccion automatica
        /// </summary>        
        [DataMember]
        public int CliamsId { get; set; }

        /// <summary>
        /// PaymentRequestId : Numero de Solicitud de Pago, campo para transaccion automatica
        /// </summary>        
        [DataMember]
        public int PaymentRequestId { get; set; }

        /// <summary>
        /// CoinsuranceCheckingAccountItems: Relación Cuentas Corrientes de reaseguros
        /// </summary>
        [DataMember]
        public List<CoInsuranceCheckingAccountItemDTO> CoinsurancesCheckingAccountItems { get; set; }

        /// <summary>
        /// Prefix : Prefix, Linea de Negocio
        /// </summary>        
        [DataMember]
        public LineBusinessDTO LineBusiness { get; set; }

        /// <summary>
        /// SubPrefix : SubPrefix, SubLinea de Negocio
        /// </summary>        
        [DataMember]
        public SubLineBusinessDTO SubLineBusiness { get; set; }

        /// <summary>
        /// AdministrativeExpenses 
        /// </summary>        
        [DataMember]
        public AmountDTO AdministrativeExpenses { get; set; }
     
        /// <summary>
        /// TaxAdministrativeExpenses 
        /// </summary>        
        [DataMember]
        public AmountDTO TaxAdministrativeExpenses { get; set; }

        /// <summary>
        /// ExtraCommission 
        /// </summary>        
        [DataMember]
        public AmountDTO ExtraCommission { get; set; }

        /// <summary>
        /// OverCommission 
        /// </summary>        
        [DataMember]
        public AmountDTO OverCommission { get; set; }
    }
}
