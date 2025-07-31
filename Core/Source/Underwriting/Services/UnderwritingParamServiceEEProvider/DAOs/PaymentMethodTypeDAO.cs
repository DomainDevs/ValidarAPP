using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.DAOs
{

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Sistran.Core.Application.Common.Entities;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Assemblers;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Entities.Views;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Resources;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Application.Utilities.Error;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.DAF.Engine;
    using Sistran.Core.Framework.Queries;
    using Sistran.Core.Framework.Transactions;
    using COMENUM = Sistran.Core.Application.CommonService.Enums;
    using COMMOD = Sistran.Core.Application.CommonService.Models;
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.Assemblers;
    using Sistran.Core.Application.Parameters.Entities;
    using Sistran.Core.Application.UnderwritingParamService.Models;
    using Sistran.Core.Application.Utilities.Enums;

    public class PaymentMethodTypeDAO
    {

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Result<List<BmParamPaymentMethodType>, ErrorModel> GetParamPaymentMethodTypes()
        {
            using (Transaction transaction = new Transaction())
            {
                List<string> errorModel = new List<string>();
                try
                {
                    BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(PaymentMethodType)));
                    Result<List<BmParamPaymentMethodType>, ErrorModel> lstParamPaymentMethodType = ModelAssembler.CreatePaymentMethodTypes(businessCollection);

                    if (lstParamPaymentMethodType is ResultError<List<BmParamPaymentMethodType>, ErrorModel>)
                    {
                        return lstParamPaymentMethodType;
                    }
                    else
                    {
                        List<BmParamPaymentMethodType> resultValue = (lstParamPaymentMethodType as ResultValue<List<BmParamPaymentMethodType>, ErrorModel>).Value;

                        if (resultValue.Count == 0)
                        {
                            errorModel.Add(Errors.FailedGetPaymentMethodTypeErrorBD);
                            return new ResultError<List<BmParamPaymentMethodType>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.NotFound, null));
                        }
                        else
                        {
                            return lstParamPaymentMethodType;
                        }
                    }
                }
                catch (Exception ex)
                {
                    errorModel.Add(Errors.FailedGetResultPaymentMethodTypeErrorBD);
                    return new ResultError<List<BmParamPaymentMethodType>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.TechnicalFault, ex));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="description"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public Result<List<BmParamPaymentMethodType>, ErrorModel> GetParamPaymentMethodByDescription(string description)
        {
            List<string> errorModel = new List<string>();
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                if (description != String.Empty)
                {
                    filter.Property(PaymentMethodType.Properties.Description, typeof(PaymentMethodType).Name);
                    filter.Like();
                    filter.Constant("%" + description + "%");
                }

                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(PaymentMethodType), filter.GetPredicate()));
                Result<List<BmParamPaymentMethodType>, ErrorModel> lstParamPaymentMethodType = ModelAssembler.CreatePaymentMethodTypes(businessCollection);
                if (lstParamPaymentMethodType is ResultError<List<BmParamPaymentMethodType>, ErrorModel>)
                {
                    return lstParamPaymentMethodType;
                }
                else
                {
                    List<BmParamPaymentMethodType> resultValue = (lstParamPaymentMethodType as ResultValue<List<BmParamPaymentMethodType>, ErrorModel>).Value;

                    if (resultValue.Count == 0)
                    {
                        errorModel.Add(Errors.FailedGetPaymentMethodTypeErrorBD);
                        return new ResultError<List<BmParamPaymentMethodType>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.NotFound, null));
                    }
                    else
                    {
                        return lstParamPaymentMethodType;
                    }
                }
            }
            catch (Exception ex)
            {
                errorModel.Add(Errors.FailedGetResultPaymentMethodTypeErrorBD);
                return new ResultError<List<BmParamPaymentMethodType>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.TechnicalFault, ex));
            }
        }
    }
}
