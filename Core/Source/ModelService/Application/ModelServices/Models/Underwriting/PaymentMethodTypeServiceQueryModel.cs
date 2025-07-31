using System.Runtime.Serialization;

namespace Sistran.Core.Application.ModelServices.Models.Underwriting
{
    public class PaymentMethodTypeServiceQueryModel : Param.ErrorServiceModel
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string Description { get; set; }

    }
}
