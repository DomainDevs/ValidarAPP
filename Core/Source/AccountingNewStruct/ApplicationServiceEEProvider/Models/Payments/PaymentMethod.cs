using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Payments
{
    

    /// <summary>
    ///     Metodo de Pago
    /// </summary>
    [DataContract]
    public class PaymentMethod
    {
       
        /// <summary>
        ///     Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        ///     Descripcion
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        ///     Ingresos
        /// </summary>
        [DataMember]
        public bool IsIncome { get; set; }

        /// <summary>
        ///     Egresos
        /// </summary>
        [DataMember]
        public bool IsOutcome { get; set; }
    }
}
