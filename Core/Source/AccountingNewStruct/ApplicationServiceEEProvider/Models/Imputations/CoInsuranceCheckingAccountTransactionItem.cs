//Sistran
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Underwriting;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Sistran.Core.Application.AccountingServices.Enums;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{
    /// <summary>
    /// CoInsuranceCheckingAccountTransactionItem:  Transaccion de Cuenta Corriente Coaseguros
    /// </summary>
    [DataContract]
    public class CoInsuranceCheckingAccountTransactionItem : CheckingAccountTransaction
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
        public new Company Holder { get; set; }

        /// <summary>
        /// CoInsuranceType : Tipo de Coaseguro
        /// </summary>        
        [DataMember]
        public CoInsuranceTypes CoInsuranceType { get; set; }

        /// <summary>
        /// CoInsuranceCheckingAccountTransactionChild: Relacion Cuentas Corrientes CoAseguros
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public List<CoInsuranceCheckingAccountTransactionItem> CoInsuranceCheckingAccountTransactionChild { get; set; }

        /// <summary>
        /// IsAutomatic:  Si se genera de manera automatica el registro de cuenta corriente
        /// </summary>        
        [DataMember]
        public bool IsAutomatic { get; set; }

        /// <summary>
        /// Policy : Poliza, campo para transaccion automatica
        /// </summary>        
        [DataMember]
        public Policy Policy { get; set; }

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
        public List<CoInsuranceCheckingAccountItem> CoinsurancesCheckingAccountItems { get; set; }

        /// <summary>
        /// Prefix : Prefix, Linea de Negocio
        /// </summary>        
        [DataMember]
        public LineBusiness LineBusiness { get; set; }

        /// <summary>
        /// SubPrefix : SubPrefix, SubLinea de Negocio
        /// </summary>        
        [DataMember]
        public SubLineBusiness SubLineBusiness { get; set; }

        /// <summary>
        /// AdministrativeExpenses 
        /// </summary>        
        [DataMember]
        public Amount AdministrativeExpenses { get; set; }
     
        /// <summary>
        /// TaxAdministrativeExpenses 
        /// </summary>        
        [DataMember]
        public Amount TaxAdministrativeExpenses { get; set; }

        /// <summary>
        /// ExtraCommission 
        /// </summary>        
        [DataMember]
        public Amount ExtraCommission { get; set; }

        /// <summary>
        /// OverCommission 
        /// </summary>        
        [DataMember]
        public Amount OverCommission { get; set; }
    }
}
