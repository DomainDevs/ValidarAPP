// -----------------------------------------------------------------------
// <copyright file="FinancialPlanDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.DAOs
{
    using Sistran.Core.Application.CommonService.Models;
    using Sistran.Co.Application.Data;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Assemblers;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Entities.Views;
    using Sistran.Core.Application.UnderwritingParamService.Models;
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.Assemblers;
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.DAF.Engine;
    using Sistran.Core.Framework.Queries;
    using Sistran.Core.Services.UtilitiesServices.Enums;
    using Sistran.Core.Services.UtilitiesServices.Models;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using COMMEN = Sistran.Core.Application.Common.Entities;
    using ENUMUT = Sistran.Core.Application.Utilities.Enums;
    using PRODEN = Sistran.Core.Application.Product.Entities;
    using QUOEN = Sistran.Core.Application.Quotation.Entities;
    using UTMO = Sistran.Core.Application.Utilities.Error;

    /// <summary>
    /// Clase para FinancialPlanDAO
    /// </summary>
    public class FinancialPlanDAO
    {
        /// <summary>
        /// Genera archivo excel de plan financiero
        /// </summary>
        /// <param name="financialPlans">listado de plan financiero a exportar</param>
        /// <param name="fileName">Nombre de archivo</param>
        /// <returns>lista de FinancialPlan - MOD-B</returns>
        public UTMO.Result<string, UTMO.ErrorModel> GenerateFileToFinancialPlan(List<ParamFinancialPlanComponent> financialPlans, string fileName)
        {
            List<string> listErrors = new List<string>();
            try
            {
                FileProcessValue fileProcessValue = new FileProcessValue()
                {
                    Key1 = (int)FileProcessType.ParametrizationFinancialPlan
                };
                FileDAO fileDAO = new FileDAO();
                File file = fileDAO.GetFileByFileProcessValue(fileProcessValue);

                if (file != null && file.IsEnabled)
                {
                    file.Name = fileName;
                    List<Row> rows = new List<Row>();

                    foreach (ParamFinancialPlanComponent item in financialPlans)
                    {
                        foreach (ParamFirstPayComponent itemp in item.ParamFirstPayComponent)
                        {
                            var fields = file.Templates[0].Rows[0].Fields.Select(x => new Field
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

                            fields[0].Value = item.ParamFinancialPlan.ParametrizationPaymentPlan.Description.ToString();
                            fields[1].Value = item.ParamFinancialPlan.ParamPaymentMethod.Description.ToString();
                            fields[2].Value = item.ParamFinancialPlan.ParamCurrency.Description.ToString();

                            fields[3].Value = itemp.Description;
                            rows.Add(new Row
                            {
                                Fields = fields
                            });
                        }
                    }

                    file.Templates[0].Rows = rows;
                    file.Name = fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy");

                    var result = fileDAO.GenerateFile(file);
                    return new UTMO.ResultValue<string, UTMO.ErrorModel>(result);
                }
                else
                {
                    listErrors.Add(Resources.Errors.ErrorDownloadingExcel);
                    return new UTMO.ResultError<string, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, new System.ArgumentException(Resources.Errors.ErrorDownloadingExcel, "original")));
                }
            }
            catch (System.Exception ex)
            {
                listErrors.Add(Resources.Errors.ErrorDownloadingExcel);
                return new UTMO.ResultError<string, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Obtener listado de plan financiero
        /// </summary>        
        /// <returns>listado de plan financiero</returns>
        public UTMO.Result<List<ParamFinancialPlanComponent>, UTMO.ErrorModel> GetParamFinancialPlans()
        {
            try
            {
                ParamFinancialPlanView view = new ParamFinancialPlanView();
                ViewBuilder builder = new ViewBuilder("ParamFinancialPlanView");
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
                List<ParamFinancialPlanComponent> paramFinancialPlans = ModelAssembler.CreateParamFinancialPlans(view.FinancialPlan);
                List<ParamFirstPayComponent> paramFirtsComponent = new List<ParamFirstPayComponent>();
                paramFirtsComponent = ModelAssembler.CreateFirstPayComponents(view.PayComponents);

                foreach (var item in paramFinancialPlans)
                {
                    item.ParamFirstPayComponent = paramFirtsComponent.Where(b => b.IdFinancialPlan == item.ParamFinancialPlan.Id).ToList();
                    for (int i = 0; i < item.ParamFirstPayComponent.Count; i++)
                    {
                        item.ParamFirstPayComponent[i].Description = view.Components.Cast<QUOEN.Component>().ToList().Where(b => b.ComponentCode == item.ParamFirstPayComponent[i].IdComponent).FirstOrDefault().SmallDescription;
                    }

                    paramFinancialPlans.AsParallel().ForAll(b => b.ParamFinancialPlan.ParametrizationPaymentPlan.Description = view.PaymentPlan.Cast<PRODEN.PaymentSchedule>().FirstOrDefault(c => c.PaymentScheduleId == b.ParamFinancialPlan.ParametrizationPaymentPlan.Id).Description);
                    paramFinancialPlans.AsParallel().ForAll(b => b.ParamFinancialPlan.ParamPaymentMethod.Description = view.PaymentMethod.Cast<COMMEN.PaymentMethod>().FirstOrDefault(c => c.PaymentMethodCode == b.ParamFinancialPlan.ParamPaymentMethod.Id).Description);
                    paramFinancialPlans.AsParallel().ForAll(b => b.ParamFinancialPlan.ParamCurrency.Description = view.Currency.Cast<COMMEN.Currency>().FirstOrDefault(c => c.CurrencyCode == b.ParamFinancialPlan.ParamCurrency.Id).Description);
                }

                return new UTMO.ResultValue<List<ParamFinancialPlanComponent>, UTMO.ErrorModel>(paramFinancialPlans);
            }
            catch (System.Exception ex)
            {
                return new UTMO.ResultError<List<ParamFinancialPlanComponent>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.ErrorGetFinancialPlan }, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Metodo para crear plan financiero
        /// </summary>
        /// <param name="parametrizationFinancialPlan">Recibe nuevo plan financiero</param>
        /// <returns>Retorna plan financiero creado</returns>
        public UTMO.Result<ParamFinancialPlanComponent, UTMO.ErrorModel> CreateParametrizationFinancialPlan(ParamFinancialPlanComponent parametrizationFinancialPlan)
        {
            try
            {
                PRODEN.FinancialPlan financialPlanEntity = EntityAssembler.CreateFinancialPlan(parametrizationFinancialPlan);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(financialPlanEntity);
                ParamFinancialPlanComponent parametrizationClauseResult = ModelAssembler.CreateParamFinancialPlan(financialPlanEntity);
                parametrizationFinancialPlan.ParamFinancialPlan.Id = parametrizationClauseResult.ParamFinancialPlan.Id;
                return new UTMO.ResultValue<ParamFinancialPlanComponent, UTMO.ErrorModel>(parametrizationFinancialPlan);
            }
            catch (System.Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(Resources.Errors.FailedCreatingFinancialPlanBD);
                return new UTMO.ResultError<ParamFinancialPlanComponent, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Actualizacion plan financiero
        /// </summary>
        /// <param name="paramFinancialPlan">plan financiero a actualizar</param>
        /// <returns>plan financiero actualizado</returns>
        public UTMO.Result<ParamFinancialPlanComponent, UTMO.ErrorModel> UpdateParamCoverage(ParamFinancialPlanComponent paramFinancialPlan)
        {
            try
            {
                PrimaryKey key = PRODEN.FinancialPlan.CreatePrimaryKey(paramFinancialPlan.ParamFinancialPlan.Id);
                PRODEN.FinancialPlan financialPlanEntity = (PRODEN.FinancialPlan)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                financialPlanEntity.PaymentMethodCode = paramFinancialPlan.ParamFinancialPlan.ParamPaymentMethod.Id;
                financialPlanEntity.CurrencyCode = paramFinancialPlan.ParamFinancialPlan.ParamCurrency.Id;
                financialPlanEntity.PaymentScheduleId = paramFinancialPlan.ParamFinancialPlan.ParametrizationPaymentPlan.Id;
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(financialPlanEntity);
                ParamFinancialPlanComponent paramFinancialPlanResult = ModelAssembler.CreateParamFinancialPlan(financialPlanEntity);
                paramFinancialPlan.ParamFinancialPlan.Id = paramFinancialPlanResult.ParamFinancialPlan.Id;
                return new UTMO.ResultValue<ParamFinancialPlanComponent, UTMO.ErrorModel>(paramFinancialPlan);
            }
            catch (System.Exception ex)
            {
                return new UTMO.ResultError<ParamFinancialPlanComponent, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.FailedUpdateFinancialPlanBD }, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Eliminacion plan financiero
        /// </summary>
        /// <param name="paramFinancialPlan">cobertura a eliminar</param>
        /// <param name="isPrincipal">eliminacion de padres e hijos</param>
        /// <returns>cobertura eliminada</returns>
        public UTMO.Result<ParamFinancialPlanComponent, UTMO.ErrorModel> DeleteParamFinancialPlan(ParamFinancialPlanComponent paramFinancialPlan, bool isPrincipal)
        {
            try
            {
                DataTable result;
                NameValue[] parameters = new NameValue[2];
                parameters[0] = new NameValue("@FINACIALPLAN_ID", paramFinancialPlan.ParamFinancialPlan.Id);
                parameters[1] = new NameValue("@PRINCIPAL", isPrincipal);
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("[PROD].[DELETE_FINANCIALPLAN_FIRSTPAYMENT_PARAMETRIZATION]", parameters);
                }

                if (((int)result.Rows[0][0]) == 1)
                {
                    paramFinancialPlan.ParamFirstPayComponent = null;
                    return new UTMO.ResultValue<ParamFinancialPlanComponent, UTMO.ErrorModel>(paramFinancialPlan);
                }

                if (((int)result.Rows[0][0]) == -1)
                {
                    return new UTMO.ResultError<ParamFinancialPlanComponent, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.FailedDeletingFinancialPlan }, ENUMUT.ErrorType.TechnicalFault, null));
                }

                return new UTMO.ResultError<ParamFinancialPlanComponent, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.FailedDeletingFinancialPlan }, ENUMUT.ErrorType.TechnicalFault, null));
            }
            catch (System.Exception ex)
            {
                return new UTMO.ResultError<ParamFinancialPlanComponent, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.FailedDeletingFinancialPlan }, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Obtiene planes financieros por items
        /// </summary>
        /// <param name="idPaymentPlan">plan de pago</param>
        /// <param name="idPaymentMethod">metodo de pago</param>
        /// <param name="idCurrency">tipo de moneda</param>
        /// <returns>listado de planes financieros</returns>
        public UTMO.Result<List<ParamFinancialPlanComponent>, UTMO.ErrorModel> GetFinancialPlanForItems(int idPaymentPlan, int idPaymentMethod, int idCurrency)
        {
            try
            {
                ParamFinancialPlanView view = new ParamFinancialPlanView();
                ViewBuilder builder = new ViewBuilder("ParamFinancialPlanView");

                List<ParamFinancialPlanComponent> paramFinancialPlan = new List<ParamFinancialPlanComponent>();
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(PRODEN.FinancialPlan.Properties.PaymentScheduleId, typeof(PRODEN.FinancialPlan).Name);
                filter.Equal();
                filter.Constant(idPaymentPlan);
                filter.And();
                filter.Property(PRODEN.FinancialPlan.Properties.PaymentMethodCode, typeof(PRODEN.FinancialPlan).Name);
                filter.Equal();
                filter.Constant(idPaymentMethod);
                filter.And();
                filter.Property(PRODEN.FinancialPlan.Properties.CurrencyCode, typeof(PRODEN.FinancialPlan).Name);
                filter.Equal();
                filter.Constant(idCurrency);
                builder.Filter = filter.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

                List<ParamFinancialPlanComponent> paramFinancialPlans = ModelAssembler.CreateParamFinancialPlans(view.FinancialPlan);
                List<ParamFirstPayComponent> paramFirtsComponent = new List<ParamFirstPayComponent>();
                paramFirtsComponent = ModelAssembler.CreateFirstPayComponents(view.PayComponents);

                foreach (var item in paramFinancialPlans)
                {
                    item.ParamFirstPayComponent = paramFirtsComponent.Where(b => b.IdFinancialPlan == item.ParamFinancialPlan.Id).ToList();
                    for (int i = 0; i < item.ParamFirstPayComponent.Count; i++)
                    {
                        item.ParamFirstPayComponent[i].Description = view.Components.Cast<QUOEN.Component>().ToList().Where(b => b.ComponentCode == item.ParamFirstPayComponent[i].IdComponent).FirstOrDefault().SmallDescription;
                    }

                    paramFinancialPlans.AsParallel().ForAll(b => b.ParamFinancialPlan.ParametrizationPaymentPlan.Description = view.PaymentPlan.Cast<PRODEN.PaymentSchedule>().FirstOrDefault(c => c.PaymentScheduleId == b.ParamFinancialPlan.ParametrizationPaymentPlan.Id).Description);
                    paramFinancialPlans.AsParallel().ForAll(b => b.ParamFinancialPlan.ParamPaymentMethod.Description = view.PaymentMethod.Cast<COMMEN.PaymentMethod>().FirstOrDefault(c => c.PaymentMethodCode == b.ParamFinancialPlan.ParamPaymentMethod.Id).Description);
                    paramFinancialPlans.AsParallel().ForAll(b => b.ParamFinancialPlan.ParamCurrency.Description = view.Currency.Cast<COMMEN.Currency>().FirstOrDefault(c => c.CurrencyCode == b.ParamFinancialPlan.ParamCurrency.Id).Description);
                }

                return new UTMO.ResultValue<List<ParamFinancialPlanComponent>, UTMO.ErrorModel>(paramFinancialPlans);
            }
            catch (System.Exception ex)
            {
                return new UTMO.ResultError<List<ParamFinancialPlanComponent>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.ErrorGetFinancialPlan }, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }
    }
}
