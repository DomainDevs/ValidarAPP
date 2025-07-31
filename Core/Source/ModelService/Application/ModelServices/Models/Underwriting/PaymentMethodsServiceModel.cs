using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ModelServices.Models.Underwriting
{
    public class PaymentMethodsServiceModel : Param.ErrorServiceModel
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public List<PaymentMethodServiceModel> PaymentMethodServiceModel { get; set; }
    }
}

