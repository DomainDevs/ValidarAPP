using System.Runtime.Serialization;

//Sistran
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Payments;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Collect
{
    /// <summary>
    /// CollectControlPayment: Detalle Control de Apertura de Ingreso
    /// </summary>
    /// <returns></returns>
    [DataContract]
    public class CollectControlPayment
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

         /// <summary>
        /// PaymentMethod: Metodo de Pago
        /// </summary>        
        [DataMember]
        public Models.Payments.PaymentMethod PaymentMethod { get; set; }

        /// <summary>
        /// PaymentTotalAdmitted: Total de todos los pagos ingresados
        /// </summary>        
        [DataMember]
        public Amount PaymentTotalAdmitted { get; set; }

        /// <summary>
        /// PaymentsTotalReceived: Total de todos los pagos recebidos efectivamente
        /// </summary>        
        [DataMember]
        public Amount PaymentsTotalReceived { get; set; }

        /// <summary>
        /// PaymentsTotalDifference: Difencia Total de todos los pagos
        /// </summary>        
        [DataMember]
        public Amount PaymentsTotalDifference { get; set; }
    }
}
