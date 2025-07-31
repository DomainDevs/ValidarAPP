namespace Sistran.Core.Application.UnderwritingParamService.Models
{
    using Sistran.Core.Application.UnderwritingParamService.Models.Base;
    using Sistran.Core.Application.Utilities.Error;
    using System.Collections.Generic;
    public class BmParamPaymentMethodType: BaseBmParamPaymentMethodType
    {
       

        /// <summary>
        /// 
        /// </summary>
        private BmParamPaymentMethodType(int id, string description):
            base(id, description)
        {
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Result<BmParamPaymentMethodType, ErrorModel> GetParamPayMethodType(int id, string description)
        {
            return new ResultValue<BmParamPaymentMethodType, ErrorModel>(new BmParamPaymentMethodType(id, description));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Result<BmParamPaymentMethodType, ErrorModel> CreateParamPayMethodType(int id, string description)
        {
            List<string> error = new List<string>();

            if (error.Count > 0)
            {
                return new ResultError<BmParamPaymentMethodType, ErrorModel>(ErrorModel.CreateErrorModel(error, Utilities.Enums.ErrorType.BusinessFault, null));
            }

            return new ResultValue<BmParamPaymentMethodType, ErrorModel>(new BmParamPaymentMethodType(id, description));
        }

    }
}
