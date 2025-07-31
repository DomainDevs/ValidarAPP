using Sistran.Core.Application.AccountingServices.DTOs.Payments;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs
{
    [DataContract]
    public class CollectControlPaymentDTO
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
        public PaymentMethodDTO PaymentMethod { get; set; }

        /// <summary>
        /// PaymentTotalAdmitted: Total de todos los pagos ingresados
        /// </summary>        
        [DataMember]
        public AmountDTO PaymentTotalAdmitted { get; set; }

        /// <summary>
        /// PaymentsTotalReceived: Total de todos los pagos recebidos efectivamente
        /// </summary>        
        [DataMember]
        public AmountDTO PaymentsTotalReceived { get; set; }

        /// <summary>
        /// PaymentsTotalDifference: Difencia Total de todos los pagos
        /// </summary>        
        [DataMember]
        public AmountDTO PaymentsTotalDifference { get; set; }
    }
}

