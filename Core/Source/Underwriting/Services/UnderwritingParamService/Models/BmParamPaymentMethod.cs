namespace Sistran.Core.Application.UnderwritingParamService.Models
{
    using Sistran.Core.Application.UnderwritingParamService.Models.Base;
    using Sistran.Core.Application.UnderwritingParamService.Resources;
    using Sistran.Core.Application.Utilities.Error;
    using System.Collections.Generic;

    public class BmParamPaymentMethod: BaseBmParamPaymentMethod
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly BmParamPaymentMethodType paymentMethod;
        
        /// <summary>
        /// 
        /// </summary>
        public BmParamPaymentMethodType PaymentMethod
        {
            get
            {
                return this.paymentMethod;
            }
        }


        private BmParamPaymentMethod(int id, string description, string tinyDescription, string smallDescription, BmParamPaymentMethodType paymentMethod) :
            base(id,description,tinyDescription,smallDescription)
        {

            this.paymentMethod = paymentMethod;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        /// <param name="tinyDescription"></param>
        /// <param name="paymentMethod"></param>
        /// <returns></returns>
        public static Result<BmParamPaymentMethod, ErrorModel> GetParamPaymentMethod(int id, string description, string tinyDescription, string smallDescription, BmParamPaymentMethodType paymentMethod)
        {
            return new ResultValue<BmParamPaymentMethod, ErrorModel>(new BmParamPaymentMethod(id, description, tinyDescription, smallDescription, paymentMethod));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        /// <param name="tinyDescription"></param>
        /// <param name="paymentMethod"></param>
        /// <returns></returns>
        public static Result<BmParamPaymentMethod, ErrorModel> CreateParamPaymentMethod(int id, string description, string tinyDescription, string smallDescription, BmParamPaymentMethodType paymentMethod)
        {
            List<string> error = new List<string>();

            if (string.IsNullOrEmpty(description))
            {
                error.Add(Errors.PaymentMethodModelDescNullError);
            }

            if (description.Length > 50)
            {
                error.Add(Errors.PaymentMethodModelDescError);
            }
            if (smallDescription.Length > 30)
            {
                error.Add(Errors.PaymentMethodModelSDescError);
            }

            if (string.IsNullOrEmpty(smallDescription))
            {
                error.Add(Errors.PaymentMethodModelSDescNullError);
            }

            if (error.Count > 0)
            {
                return new ResultError<BmParamPaymentMethod, ErrorModel>(ErrorModel.CreateErrorModel(error, Utilities.Enums.ErrorType.BusinessFault, null));
            }

            return new ResultValue<BmParamPaymentMethod, ErrorModel>(new BmParamPaymentMethod(id, description, tinyDescription, smallDescription, paymentMethod));
        }
    }
}
