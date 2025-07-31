
using System.Runtime.Serialization;
using Sistran.Core.Application.CommonService.Models;

//Sistran
using Sistran.Core.Application.GeneralLedgerServices.DTOs;

namespace Sistran.Core.Application.ReconciliationServices.Models
{
    /// <summary>
    /// BankReconciliationMovementType : Tipo de  Movimiento de Conciliacion del Banco
    /// </summary>
    [DataContract]
    public class BankReconciliationMovementType
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        // <summary>
        /// ReconciliationMovementType 
        /// </summary>        
        [DataMember]
        public ReconciliationMovementTypeDTO ReconciliationMovementType { get; set; }

        /// <summary>
        /// Bank : Banco
        /// </summary>        
        [DataMember]
        public Bank Bank { get; set; }

       
        /// <summary>
        /// SmallDescription: Descripcion reducida
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }

        /// <summary>
        /// VoucherNumber
        /// </summary>
        [DataMember]
        public bool VoucherNumber { get; set; }


        


    }
}
