using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Imputations
{
    /// <summary>
    /// PremiumsReceivable:  Primas por cobrar
    /// </summary>
    [DataContract]
    public class PremiumReceivableTransactionItemDTO
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Policy:Poliza que se carga al item de imputación
        /// </summary>        
        [DataMember]
        public PolicyDTO Policy { get; set; }
        
        /// <summary>
        /// DeductCommission: comisión descontada
        /// </summary>
        [DataMember]
        public AmountDTO DeductCommission { get; set; }

            /// <summary>
            /// NoExpenses: tiene o no gastos
            /// </summary>
            [DataMember]
            public bool NoExpenses { get; set; }


    }
}
