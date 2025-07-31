namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.DAOs
{

    using Sistran.Core.Application.Parameters.Entities;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Assemblers;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Entities.Views;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Resources;
    using Sistran.Core.Application.UnderwritingParamService.Models;
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.Assemblers;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Application.Utilities.Enums;
    using Sistran.Core.Application.Utilities.Error;
    using Sistran.Core.Application.UtilitiesServicesEEProvider.DAOs;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.DAF.Engine;
    using Sistran.Core.Framework.Queries;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using COMMEN = Sistran.Core.Application.Common.Entities;
    using ENUMUT = Sistran.Core.Application.Utilities.Enums;
    using UTILEN = Sistran.Core.Services.UtilitiesServices.Enums;
    using UTILMO = Sistran.Core.Services.UtilitiesServices.Models;
    using UTMO = Sistran.Core.Application.Utilities.Error;
    public class PaymentMethodDAO
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramPaymentMethod"></param>
        /// <returns></returns>
        public Result<Models.BmParamPaymentMethod, ErrorModel> CreateParamPaymentMethod(Models.BmParamPaymentMethod paramPaymentMethod)
        {

            try
            {
                int id = (GetParamPaymentMethods() as ResultValue<List<BmParamPaymentMethod>, ErrorModel>).Value.Max(p => p.Id) + 1;
                BmParamPaymentMethod item = (BmParamPaymentMethod.GetParamPaymentMethod(id, paramPaymentMethod.Description, paramPaymentMethod.TinyDescription, paramPaymentMethod.SmallDescription, paramPaymentMethod.PaymentMethod)as ResultValue<BmParamPaymentMethod, ErrorModel>).Value;
                PaymentMethod paymentMethod = EntityAssembler.CreatePaymentMethod(item);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(paymentMethod);
                Result<Models.BmParamPaymentMethod, ErrorModel> paymentMethodTypeResult = ModelAssembler.CreateParamPaymentMethod(paymentMethod);
                return paymentMethodTypeResult;
            }
            catch (Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(Errors.FailedCreatingPaymentMethodErrorBD);
                return new ResultError<BmParamPaymentMethod, ErrorModel>(ErrorModel.CreateErrorModel(listErrors, ErrorType.TechnicalFault, ex));
            }
               
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramPaymentMethod"></param>
        /// <returns></returns>
        public Result<Models.BmParamPaymentMethod, ErrorModel> UpdateParamPaymentMethod(Models.BmParamPaymentMethod paramPaymentMethod)
        {

            try
            {
                PrimaryKey primaryKey = PaymentMethod.CreatePrimaryKey(paramPaymentMethod.Id);
                PaymentMethod paymentMethod = (PaymentMethod)(DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey));
                paymentMethod.Description = paramPaymentMethod.Description;
                paymentMethod.SmallDescription = paramPaymentMethod.SmallDescription;
                paymentMethod.TinyDescription = paramPaymentMethod.TinyDescription;
                paymentMethod.PaymentMethodTypeCode = paramPaymentMethod.PaymentMethod.Id;
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(paymentMethod);
                return new ResultValue<BmParamPaymentMethod, ErrorModel>(paramPaymentMethod);
            }
            catch (Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(Resources.Errors.FailedUpdatingPaymentMethodErrorBD);
                return new ResultError<BmParamPaymentMethod, ErrorModel>(ErrorModel.CreateErrorModel(listErrors, ErrorType.TechnicalFault, ex));

            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramPaymentMethod"></param>
        /// <returns></returns>
        public Result<Models.BmParamPaymentMethod, ErrorModel> DeleteParamPaymentMethod(Models.BmParamPaymentMethod paramPaymentMethod)
        {
            try
             {
                PrimaryKey primaryKey = PaymentMethod.CreatePrimaryKey(paramPaymentMethod.Id);
                PaymentMethod paymentMethod = (PaymentMethod)(DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey));
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(paymentMethod);
                return new ResultValue<BmParamPaymentMethod, ErrorModel>(paramPaymentMethod);
            }
             catch (Exception ex)
             {
                 List<string> listErrors = new List<string>();
                 listErrors.Add(Resources.Errors.FailedDeletePaymentMethodErrorBD);
                 return new ResultError<BmParamPaymentMethod, ErrorModel>(ErrorModel.CreateErrorModel(listErrors, ErrorType.TechnicalFault, ex));
             
             }
                
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Result<List<BmParamPaymentMethod>, ErrorModel> GetParamPaymentMethods()
        {

             List<string> errorModel = new List<string>();                
             try
             {
                 BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(PaymentMethod)));
                 Result<List<BmParamPaymentMethod>, ErrorModel> lstParamPaymentMethod = ModelAssembler.CreateParamPaymentMethods(businessCollection);

                 if(lstParamPaymentMethod is ResultError<List<BmParamPaymentMethod>, ErrorModel>)
                 {
                     return lstParamPaymentMethod;
                 }
                 else
                 {
                     List<BmParamPaymentMethod> resultValue = (lstParamPaymentMethod as ResultValue<List<BmParamPaymentMethod>, ErrorModel>).Value;

                     if(resultValue.Count == 0)
                     {
                         errorModel.Add(Errors.FailedGetPaymentMethodErrorBD);
                         return new ResultError<List<BmParamPaymentMethod>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.NotFound, null));
                     }
                     else
                     {
                         return lstParamPaymentMethod;
                     }
                 }
             }
             catch(Exception ex)
             {
                 errorModel.Add(Errors.FailedGetResultPaymentMethodErrorBD);
                 return new ResultError<List<BmParamPaymentMethod>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.TechnicalFault, ex));
             }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="description"></param>
        /// <param name="code"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public Result<List<BmParamPaymentMethod>, ErrorModel> GetParamPaymentMethodByDescription(string description)
        {
            List<string> errorModel = new List<string>();
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                if (description != String.Empty)
                {
                    filter.Property(PaymentMethod.Properties.Description, typeof(PaymentMethod).Name);
                    filter.Like();
                    filter.Constant("%" + description + "%");
                }
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(PaymentMethod), filter.GetPredicate()));
                Result<List<BmParamPaymentMethod>, ErrorModel> lstParamPaymentMethod = ModelAssembler.CreateParamPaymentMethods(businessCollection);
                if (lstParamPaymentMethod is ResultError<List<BmParamPaymentMethod>, ErrorModel>)
                {
                    return lstParamPaymentMethod;
                }
                else
                {
                    List<BmParamPaymentMethod> resultValue = (lstParamPaymentMethod as ResultValue<List<BmParamPaymentMethod>, ErrorModel>).Value;

                    if (resultValue.Count == 0)
                    {
                        List<BmParamPaymentMethod> listFilter = new List<BmParamPaymentMethod>();
                        ResultValue<List<BmParamPaymentMethod>, ErrorModel> listResult = (this.GetParamPaymentMethods() as ResultValue<List<BmParamPaymentMethod>, ErrorModel>);
                        listFilter = listResult.Value.Where(x => x.SmallDescription.Contains(description) || x.TinyDescription.Contains(description) || x.PaymentMethod.Description.Contains(description)).ToList();
                        ResultValue<List<BmParamPaymentMethod>, ErrorModel> resultFilter = new ResultValue<List<BmParamPaymentMethod>, ErrorModel>(listFilter);


                        if(listFilter.Count == 0)
                        {
                            errorModel.Add(Errors.FailedGetPaymentMethodErrorBD);
                            return new ResultError<List<BmParamPaymentMethod>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.NotFound, null));
                        }  
                        else
                        {
                            return resultFilter;
                        }
                    }
                    else
                    {
                        return lstParamPaymentMethod;
                    }
                }
            }
            catch (Exception ex)
            {
                errorModel.Add(Errors.FailedGetResultPaymentMethodErrorBD);
                return new ResultError<List<BmParamPaymentMethod>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.TechnicalFault, ex));
            }
        }

        public Result<string, ErrorModel> GenerateFileToPaymentMethod(List<BmParamPaymentMethod> lstPaymentMethod, string fileName)
        {
            try
            {
                FileDAO commonFileDAO = new FileDAO();
                UTILMO.FileProcessValue fileProcessValue = new UTILMO.FileProcessValue();
                fileProcessValue.Key1 = (int)UTILEN.FileProcessType.ParametrizationPaymentMethod;
                UTILMO.File file = commonFileDAO.GetFileByFileProcessValue(fileProcessValue);
                string url = String.Empty;
                if (file.IsEnabled)
                {
                    file.Name = fileName;
                    List<UTILMO.Row> rows = new List<UTILMO.Row>();
                    foreach (BmParamPaymentMethod paymentMethod in lstPaymentMethod)
                    {
                        var fields = file.Templates[0].Rows[0].Fields.Select(x => new UTILMO.Field
                        {
                            ColumnSpan = x.ColumnSpan,
                            Description = x.Description,
                            FieldType = x.FieldType,
                            Id = x.Id,
                            IsEnabled = x.IsEnabled,
                            IsMandatory = x.IsMandatory,
                            Order = x.Order,
                            RowPosition = x.RowPosition,
                            SmallDescription = x.SmallDescription
                        }).ToList();
                        fields[0].Value = paymentMethod.Description;
                        fields[1].Value = paymentMethod.SmallDescription;
                        fields[2].Value = paymentMethod.PaymentMethod.Description;
                        rows.Add(new UTILMO.Row
                        {
                            Fields = fields
                        });
                    }
                    file.Templates[0].Rows = rows;
                    file.Name = fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy");
                    url = commonFileDAO.GenerateFile(file);
                }
                return new ResultValue<string, ErrorModel>(url);
            }
            catch (Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(Errors.FailedCreatingPaymentMethodErrorBD);
                return new ResultError<string, ErrorModel>(ErrorModel.CreateErrorModel(listErrors, ErrorType.TechnicalFault, ex));
            }
        }
        
        /// <summary>
        /// The GetPaymentMethod
        /// </summary>
        /// <returns>The <see cref="UTMO.Result{List{ParamPaymentMethod}, UTMO.ErrorModel}"/></returns>
        public UTMO.Result<List<ParamPaymentMethod>, UTMO.ErrorModel> GetPaymentMethod()
        {
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.PaymentMethod)));
                List<ParamPaymentMethod> paymentMethod = ModelAssembler.CreatePaymentsMethod(businessCollection);
                return new UTMO.ResultValue<List<ParamPaymentMethod>, UTMO.ErrorModel>(paymentMethod);
            }
            catch (System.Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add("Error Creando");
                return new UTMO.ResultError<List<ParamPaymentMethod>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// The GetPaymentMethodForId
        /// </summary>
        /// <param name="idMethod">The <see cref="int"/></param>
        /// <returns>The <see cref="UTMO.Result{List{ParamPaymentMethod}, UTMO.ErrorModel}"/></returns>
        public UTMO.Result<List<ParamPaymentMethod>, UTMO.ErrorModel> GetPaymentMethodForId(int idMethod)
        {
            try
            {
                ParamPaymentMethodView view = new ParamPaymentMethodView();
                ViewBuilder builder = new ViewBuilder("ParamPaymentMethodView");

                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

                filter.Property(COMMEN.PaymentMethod.Properties.PaymentMethodCode, typeof(COMMEN.PaymentMethod).Name);
                filter.Like();
                filter.Constant(idMethod);

                builder.Filter = filter.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
                List<ParamPaymentMethod> parametrizationTextClause = ModelAssembler.CreatePaymentsMethod(view.PaymentMethod);
                return new UTMO.ResultValue<List<ParamPaymentMethod>, UTMO.ErrorModel>(parametrizationTextClause);
            }
            catch (System.Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add("Error Creando");
                return new UTMO.ResultError<List<ParamPaymentMethod>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }
    }
}
