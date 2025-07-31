// -----------------------------------------------------------------------
// <copyright file="VehicleTypeDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Stiveen Niño</author>
// -----------------------------------------------------------------------
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
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.DAF.Engine;
    using Sistran.Core.Framework.Queries;
    using Sistran.Core.Framework.Transactions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EntRules = Sistran.Core.Application.Script.Entities;
    using ENUM = Sistran.Core.Application.UnderwritingParamService.Enums;
    using ENUMUT = Sistran.Core.Application.Utilities.Enums;
    using QUOEN = Sistran.Core.Application.Quotation.Entities;
    using UTILEN = Sistran.Core.Services.UtilitiesServices.Enums;
    using UTILMO = Sistran.Core.Services.UtilitiesServices.Models;
    using UTMO = Sistran.Core.Application.Utilities.Error;


    /// <summary>
    /// Dao para Componentes 
    /// </summary>
    public class ExpenseDAO
    {
        /// <summary>
        /// Lista los componente
        /// </summary>
        /// <returns> retorna el modelo de componentes </returns>       
        public UTMO.Result<List<ParamExpense>, UTMO.ErrorModel> GetExpense()
        {
            try
            {
                List<ParamExpense> surchangeComponent = new List<ParamExpense>();

                var viewBuilder = new ViewBuilder("ExpenseView");

                ExpenseView expenseView = new ExpenseView();
                DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, expenseView);
                foreach (QUOEN.Component componentQuotation in expenseView.Components)
                {
                    QUOEN.ExpenseComponent expenseComponent = expenseView.ExpenseComponents.Cast<QUOEN.ExpenseComponent>().First(x => x.ComponentCode == componentQuotation.ComponentCode);
                    EntRules.RuleSet ruleSet = expenseView.RulesSet.Cast<EntRules.RuleSet>().First();
                    if (expenseComponent.RuleSetId != null)
                    {
                        ruleSet = expenseView.RulesSet.Cast<EntRules.RuleSet>().First(r => r.RuleSetId == expenseComponent.RuleSetId);
                    }                      
                    
                    
                    RateType rateType = expenseView.RateTypes.Cast<RateType>().First(r => r.RateTypeCode == expenseComponent.RateTypeCode);
                    Result<ParamExpense, ErrorModel> resultParamExpense = ModelAssembler.CreateParamExpenses(componentQuotation, expenseComponent, ruleSet, rateType);
                    surchangeComponent.Add((resultParamExpense as UTMO.ResultValue<ParamExpense, UTMO.ErrorModel>).Value);

                }

                return new UTMO.ResultValue<List<ParamExpense>, UTMO.ErrorModel>(surchangeComponent);
            }
            catch (System.Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(Resources.Errors.FailedUpdatingDiscountErrorBD);
                return new UTMO.ResultError<List<ParamExpense>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

               /// </summary>
        /// <param name="paramExpense"></param>
        /// <returns></returns>
        public Result<ParamExpense, ErrorModel> CreateExpenseComponent(ResultValue<Models.ParamExpense, ErrorModel> paramExpense)
        {
            using (Transaction transaction = new Transaction())
            {
                try
                {                    
                    BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(QUOEN.Component)));
                    var enumerable = businessCollection.Select(x => (QUOEN.Component)x);
                    int id = enumerable.Max(p => p.ComponentCode) + 1;
                    Result<ParamExpense, ErrorModel> item = ParamExpense.CreateParamExpense(id, paramExpense.Value.SmallDescription, paramExpense.Value.TinyDescripcion,
                        ENUM.ComponentClass.EXPENSES, ENUM.ComponnetType.EXPENSES, paramExpense.Value.Rate, paramExpense.Value.IsMandatory,
                        paramExpense.Value.IsInitially, paramExpense.Value.ParamRuleSet, paramExpense.Value.ParamRateType);   
                    if(item is ResultError<ParamExpense, ErrorModel>)
                    {
                        transaction.Dispose();
                        List<string> listErrors = new List<string>();
                        listErrors.Add(Resources.Errors.FailedCreatingExpenseErrorBD);
                        return new ResultError<ParamExpense, ErrorModel>(ErrorModel.CreateErrorModel(listErrors, ErrorType.TechnicalFault, null));
                    }
                    else
                    {

                        QUOEN.Component component = EntityAssembler.CreateComponentExpense(((ResultValue<ParamExpense, ErrorModel>)item).Value);
                        DataFacadeManager.Instance.GetDataFacade().InsertObject(component);


                        QUOEN.ExpenseComponent expenseComponent = EntityAssembler.CreateExpense(((ResultValue<ParamExpense, ErrorModel>)item).Value);
                        DataFacadeManager.Instance.GetDataFacade().InsertObject(expenseComponent);

                        Result<ParamExpense, ErrorModel> resultParamExpense = ModelAssembler.CreateParamExpense(component, expenseComponent);
                        transaction.Complete();
                        return resultParamExpense;
                    }
                    
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    List<string> listErrors = new List<string>();
                    listErrors.Add(Resources.Errors.FailedCreatingExpenseErrorBD);
                    return new ResultError<ParamExpense, ErrorModel>(ErrorModel.CreateErrorModel(listErrors, ErrorType.TechnicalFault, ex));
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramExpense"></param>
        /// <returns></returns>
        public Result<ParamExpense, ErrorModel> UpdateExpenseComponent(ResultValue<Models.ParamExpense, ErrorModel> paramExpense)
        {
            using (Transaction transaction = new Transaction())
            {
                try
                {
                    PrimaryKey primaryKeyComponent = QUOEN.Component.CreatePrimaryKey(paramExpense.Value.Id);
                    ////////
                    QUOEN.Component Component = (QUOEN.Component)(DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKeyComponent));
                    Component.SmallDescription = paramExpense.Value.SmallDescription;
                    Component.TinyDescription = paramExpense.Value.TinyDescripcion;
                    Component.ComponentClassCode = (int)ENUM.ComponentClass.EXPENSES;
                    Component.ComponentTypeCode = (int)ENUM.ComponnetType.EXPENSES;
                    DataFacadeManager.Instance.GetDataFacade().UpdateObject(Component);
                    /////////
                    PrimaryKey primaryKeyExpense = QUOEN.ExpenseComponent.CreatePrimaryKey(paramExpense.Value.Id);
                    //////
                    QUOEN.ExpenseComponent Expense = (QUOEN.ExpenseComponent)(DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKeyExpense));
                    Expense.Rate = paramExpense.Value.Rate;
                    Expense.IsMandatory = paramExpense.Value.IsMandatory;
                    Expense.IsInitially = paramExpense.Value.IsInitially;
                    if (paramExpense.Value.ParamRuleSet!=null)
                    {
                        Expense.RuleSetId = paramExpense.Value.ParamRuleSet.Id;
                    }
                    Expense.RateTypeCode = paramExpense.Value.ParamRateType.Id;
                    DataFacadeManager.Instance.GetDataFacade().UpdateObject(Expense);
                    ////
                    transaction.Complete();
                    Result<ParamExpense, ErrorModel> result = ModelAssembler.CreateParamExpense(Component, Expense);
                    ResultValue<ParamExpense, ErrorModel> paramExpenseResult = ((ResultValue<ParamExpense, ErrorModel>)result);
                    return paramExpenseResult;
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    List<string> listErrors = new List<string>();
                    listErrors.Add(Resources.Errors.FailedUpdatingExpenseErrorBD);
                    return new ResultError<ParamExpense, ErrorModel>(ErrorModel.CreateErrorModel(listErrors, ErrorType.TechnicalFault, ex));

                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramPaymentMethod"></param>
        /// <returns></returns>
        public Result<ParamExpense, ErrorModel> DeleteExpenseComponent(ResultValue<Models.ParamExpense, ErrorModel> paramExpense)
        {
            using (Transaction transaction = new Transaction())
            {
                try
                {

                    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                    filter.Property(QUOEN.ExpenseComponent.Properties.ComponentCode, typeof(QUOEN.ExpenseComponent).Name);
                    filter.Equal();
                    filter.Constant(paramExpense.Value.Id);

                    BusinessCollection businessCollection1 = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(QUOEN.ExpenseComponent), filter.GetPredicate()));
                    QUOEN.ExpenseComponent expenseComponent = new QUOEN.ExpenseComponent(paramExpense.Value.Id);
                    foreach (QUOEN.ExpenseComponent item in businessCollection1)
                    {
                        DataFacadeManager.Instance.GetDataFacade().DeleteObject(item);
                    }                    
                    ObjectCriteriaBuilder filter1 = new ObjectCriteriaBuilder();
                    filter1.Property(QUOEN.Component.Properties.ComponentCode, typeof(QUOEN.Component).Name);
                    filter1.Equal();
                    filter1.Constant(paramExpense.Value.Id);

                    BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(QUOEN.Component), filter1.GetPredicate()));
                    QUOEN.Component component = new QUOEN.Component(paramExpense.Value.Id);
                    foreach (QUOEN.Component item in businessCollection)
                    {
                        item.ComponentClassCode = (int)ENUM.ComponentClass.EXPENSES;
                        item.ComponentTypeCode = (int)ENUM.ComponnetType.EXPENSES;
                        DataFacadeManager.Instance.GetDataFacade().DeleteObject(item);
                    }
                    transaction.Complete();


                    Result<ParamExpense, ErrorModel> result = ModelAssembler.CreateParamExpense(component, expenseComponent);
                    ResultValue<ParamExpense, ErrorModel> paramExpenseResult = ((ResultValue<ParamExpense, ErrorModel>)result);
                    return paramExpenseResult;
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    List<string> listErrors = new List<string>();
                    listErrors.Add($"({paramExpense.Value.SmallDescription}) {Resources.Errors.ItemInUse}"); ;

                    //listErrors.Add(Resources.Errors.FailedDeletingExpenseErrorBD);
                    return new ResultError<ParamExpense, ErrorModel>(ErrorModel.CreateErrorModel(listErrors, ErrorType.TechnicalFault, ex));

                }
            }

        }

        /// <summary>
        /// Obtener Objetos de gastos de suscripcion
        /// </summary>
        /// <param name="description">descripcion de objetos del seguro</param>
        /// <returns>Objetos Del Seguro</returns>
        public UTMO.Result<List<ParamExpense>, UTMO.ErrorModel> GetExpenseByDescripcion(string descripcion)
        {
            try
            {
                List<ParamExpense> surchangeComponent = new List<ParamExpense>();

                var viewBuilder = new ViewBuilder("ExpenseView");

                ExpenseView expenseView = new ExpenseView();
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(QUOEN.Component.Properties.SmallDescription, typeof(QUOEN.Component).Name);
                filter.Like();
                filter.Constant("%" + descripcion + "%");
                viewBuilder.Filter = filter.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, expenseView);

                foreach (QUOEN.ExpenseComponent componentQuotation in expenseView.ExpenseComponents)
                {
                    QUOEN.ExpenseComponent expenseComponent = expenseView.ExpenseComponents.Cast<QUOEN.ExpenseComponent>().First(x => x.ComponentCode == componentQuotation.ComponentCode);
                    QUOEN.Component component = expenseView.Components.Cast<QUOEN.Component>().First(x => x.ComponentCode == componentQuotation.ComponentCode);
                    Result<ParamExpense, ErrorModel> resultParamExpense = ModelAssembler.CreateParamExpense(component, expenseComponent);
                    surchangeComponent.Add((resultParamExpense as UTMO.ResultValue<ParamExpense, UTMO.ErrorModel>).Value);

                }

                return new UTMO.ResultValue<List<ParamExpense>, UTMO.ErrorModel>(surchangeComponent);
            }
            catch (System.Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(Resources.Errors.FailedUpdatingDiscountErrorBD);
                return new UTMO.ResultError<List<ParamExpense>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Genera el archivo
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <returns>Ruta de archivo</returns>
        public Result<string, ErrorModel> GenerateFileToExpense(string fileName)
        {
            try
            {
                UTILMO.FileProcessValue fileProcessValue = new UTILMO.FileProcessValue();
                fileProcessValue.Key1 = (int)UTILEN.FileProcessType.ParametrizationExpenseComponent;

                UTILMO.File file = DelegateService.utilitiesServiceCore.GetFileByFileProcessValue(fileProcessValue);
                if (file == null)
                {
                    return new ResultError<string, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { Errors.ErrorNotExistsTemplate }, Utilities.Enums.ErrorType.BusinessFault, null));
                }

                if (!file.IsEnabled)
                {
                    return new ResultError<string, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { Errors.ErrorTempleteIsNotEnable }, Utilities.Enums.ErrorType.BusinessFault, null));
                }

                file.Name = fileName;
                List<UTILMO.Row> rows = new List<UTILMO.Row>();

                Result<List<ParamExpense>, ErrorModel> paramExpenseResult = this.GetExpense();

                if (paramExpenseResult is ResultError<List<Models.ParamExpense>, ErrorModel>)
                {
                    return new ResultError<string, ErrorModel>(((ResultError<List<Models.ParamExpense>, ErrorModel>)paramExpenseResult).Message);
                }
                else
                {
                    List<Models.ParamExpense> expenseComponent = ((ResultValue<List<Models.ParamExpense>, ErrorModel>)paramExpenseResult).Value;
                    foreach (Models.ParamExpense expense in expenseComponent)
                    {
                        List<UTILMO.Field> fields = file.Templates[0].Rows[0].Fields.Select(p => new UTILMO.Field()
                        {
                            ColumnSpan = p.ColumnSpan,
                            Description = p.Description,
                            FieldType = p.FieldType,
                            Id = p.Id,
                            IsEnabled = p.IsEnabled,
                            IsMandatory = p.IsMandatory,
                            Order = p.Order,
                            RowPosition = p.RowPosition,
                            SmallDescription = p.SmallDescription
                        }).ToList();

                        if (fields.Count < 5)
                        {
                            return new ResultError<string, ErrorModel>(ErrorModel.CreateErrorModel(new List<string> { Errors.ErrorTemplateColumnsNotEqual }, Utilities.Enums.ErrorType.BusinessFault, null));
                        }
                        

                        fields[0].Value = expense.SmallDescription;
                        fields[1].Value = expense.TinyDescripcion;
                        fields[2].Value = expense.IsMandatory.ToString();
                        fields[3].Value = expense.IsInitially.ToString();
                        fields[4].Value = expense.Rate.ToString();

                        rows.Add(new UTILMO.Row
                        {
                            Fields = fields
                        });
                    }

                    file.Templates[0].Rows = rows;
                    file.Name = fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy");

                    string generateFile = DelegateService.utilitiesServiceCore.GenerateFile(file);
                    return new ResultValue<string, ErrorModel>(generateFile);
                }
            }
            catch (Exception ex)
            {
                return new ResultError<string, ErrorModel>(ErrorModel.CreateErrorModel(new List<string> { Errors.ErrorGenerateFile }, Utilities.Enums.ErrorType.TechnicalFault, ex));
            }
        }
    }
}
