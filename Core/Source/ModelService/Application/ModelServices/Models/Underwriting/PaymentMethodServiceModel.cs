using System.Runtime.Serialization;

namespace Sistran.Core.Application.ModelServices.Models.Underwriting
{
    public class PaymentMethodServiceModel : Param.ParametricServiceModel
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string TinyDescription { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public PaymentMethodTypeServiceQueryModel PaymentMethodTypeServiceQueryModel { get; set; }
    }
}
