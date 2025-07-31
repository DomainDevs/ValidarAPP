// -----------------------------------------------------------------------
// <copyright file="PaymentPlanDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.DAOs
{
    using Sistran.Co.Application.Data;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Assemblers;
    using Sistran.Core.Application.UnderwritingParamService.Models;
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.Assemblers;
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs;
    using Sistran.Core.Application.UnderwritingServices.Enums;
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
    using ENUMUT = Sistran.Core.Application.Utilities.Enums;
    using PRODEN = Sistran.Core.Application.Product.Entities;
    using UPENTV = Sistran.Core.Application.UnderwritingParamService.EEProvider.Entities.Views;
    using UTMO = Sistran.Core.Application.Utilities.Error;
    using Sistran.Core.Application.CommonService.Models;

    /// <summary>
    /// Acceso a DB de Plan de Pago
    /// </summary>
    public class PaymentPlanDAO
    {
        /// <summary>
        /// Aceso a DB para consultar listado de plan de pagos
        /// </summary>
        /// <returns>Listado Result consulta en DB de Planes de pago</returns>
        public UTMO.Result<List<ParametrizationPaymentPlan>, UTMO.ErrorModel> GetParametrizationPaymentPlans()
        {
            try
            {
                UPENTV.PaymentScheduleDistributionView view = new UPENTV.PaymentScheduleDistributionView();
                ViewBuilder builder = new ViewBuilder("PaymentScheduleDistributionView");
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
                List<ParametrizationPaymentPlan> parametrizationPaymentPlans = ModelAssembler.CreateParametrizationPaymentPlans(view.PaymentSchedules);
                for (int i = 0; i < parametrizationPaymentPlans.Count(); i++)
                {
                    List<PRODEN.PaymentDistribution> paymentDistributions = view.PaymentDistributions.Cast<PRODEN.PaymentDistribution>().Where(b => b.PaymentScheduleId == parametrizationPaymentPlans[i].Id).ToList();
                    foreach (PRODEN.PaymentDistribution item in paymentDistributions)
                    {
                        List<PRODEN.CoPaymentDistributionComponent> paymentDistributionsComponent = view.PaymentDistributionComponents.Cast<PRODEN.CoPaymentDistributionComponent>().Where(b => b.PaymentScheduleId == parametrizationPaymentPlans[i].Id && b.PaymentNumber == item.PaymentNumber)?.ToList();
                        if (paymentDistributionsComponent.Count > 0 && paymentDistributionsComponent != null)
                        {
                            List<ParametrizacionQuotaTypeComponent> parametrizacionQuotaTypeComponents = new List<ParametrizacionQuotaTypeComponent>();
                            foreach (PRODEN.CoPaymentDistributionComponent itemPaymentDistributionComponent in paymentDistributionsComponent)
                            {
                                parametrizacionQuotaTypeComponents.Add(ModelAssembler.CreateParametrizationQuotaComponent(itemPaymentDistributionComponent));
                            }
                            parametrizationPaymentPlans[i].ParametrizationQuotas.Add(ModelAssembler.CreateParametrizationQuotaAndDistributionComponent(item, parametrizacionQuotaTypeComponents));
                        }
                        else
                        {
                            parametrizationPaymentPlans[i].ParametrizationQuotas.Add(ModelAssembler.CreateParametrizationQuota(item));
                        }
                    }
                }

                return new UTMO.ResultValue<List<ParametrizationPaymentPlan>, UTMO.ErrorModel>(parametrizationPaymentPlans);
            }
            catch (System.Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(Resources.Errors.ErrorGetParametrizationPaymentPlanErrorBD);
                return new UTMO.ResultError<List<ParametrizationPaymentPlan>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }


        /// <summary>
        /// Aceso a DB para consultar listado de plan de pagos asociados a la description
        /// </summary>
        /// <param name="description">descripción del plan de pago a consultar</param>
        /// <returns>Listado Result consulta en DB de Planes de pago</returns>
        public UTMO.Result<List<ParametrizationPaymentPlan>, UTMO.ErrorModel> GetPaymentPlansByDescription(string description)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(PRODEN.PaymentSchedule.Properties.Description, typeof(PRODEN.PaymentSchedule).Name);
                filter.Like();
                filter.Constant("%" + description + "%");
                UPENTV.PaymentScheduleDistributionView view = new UPENTV.PaymentScheduleDistributionView();
                ViewBuilder builder = new ViewBuilder("PaymentScheduleDistributionView");
                builder.Filter = filter.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
                List<ParametrizationPaymentPlan> parametrizationPaymentPlans = ModelAssembler.CreateParametrizationPaymentPlans(view.PaymentSchedules);
                for (int i = 0; i < parametrizationPaymentPlans.Count(); i++)
                {
                    List<PRODEN.PaymentDistribution> paymentDistributions = view.PaymentDistributions.Cast<PRODEN.PaymentDistribution>().Where(b => b.PaymentScheduleId == parametrizationPaymentPlans[i].Id).ToList();
                    foreach (var item in paymentDistributions)
                    {
                        parametrizationPaymentPlans[i].ParametrizationQuotas.Add(ModelAssembler.CreateParametrizationQuota(item));
                    }
                }

                return new UTMO.ResultValue<List<ParametrizationPaymentPlan>, UTMO.ErrorModel>(parametrizationPaymentPlans);
            }
            catch (System.Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(Resources.Errors.ErrorGetParametrizationPaymentPlanErrorBD);
                return new UTMO.ResultError<List<ParametrizationPaymentPlan>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Acceso a DB para creacion de plan de pago
        /// </summary>
        /// <param name="parametrizationPaymentPlan">Plan de Pago MOD-B</param>
        /// <returns>Modelo Result con resultado de la creacion del plan de pago</returns>
        public UTMO.Result<ParametrizationPaymentPlan, UTMO.ErrorModel> CreateParametrizationPaymentPlan(ParametrizationPaymentPlan parametrizationPaymentPlan)
        {

            try
            {

                PRODEN.PaymentSchedule paymentScheduleEntity = EntityAssembler.CreatePaymentSchedule(parametrizationPaymentPlan);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(paymentScheduleEntity);
                ParametrizationPaymentPlan parametrizationPaymentPlanResult = ModelAssembler.CreateParametrizationPaymentPlan(paymentScheduleEntity);
                return new UTMO.ResultValue<ParametrizationPaymentPlan, UTMO.ErrorModel>(parametrizationPaymentPlanResult);
            }
            catch (System.Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(Resources.Errors.FailedCreatingPaymentPlanErrorBD);
                return new UTMO.ResultError<ParametrizationPaymentPlan, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Acceso a DB para actualización de plan de pago
        /// </summary>
        /// <param name="parametrizationPaymentPlan">Plan de pago MOD-B</param>
        /// <returns>Modelo Result con resultado de operacion de actualizacion</returns>
        public UTMO.Result<ParametrizationPaymentPlan, UTMO.ErrorModel> UpdateParametrizationPaymentPlan(ParametrizationPaymentPlan parametrizationPaymentPlan)
        {
            try
            {
                PrimaryKey key = PRODEN.PaymentSchedule.CreatePrimaryKey(parametrizationPaymentPlan.Id);
                PRODEN.PaymentSchedule paymentScheduleEntity = (PRODEN.PaymentSchedule)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                paymentScheduleEntity.Description = parametrizationPaymentPlan.Description;
                paymentScheduleEntity.SmallDescription = parametrizationPaymentPlan.SmallDescription;
                paymentScheduleEntity.IsGreaterDate = parametrizationPaymentPlan.IsGreaterDate;
                paymentScheduleEntity.IsIssueDate = parametrizationPaymentPlan.IsIssueDate;
                paymentScheduleEntity.FirstPayQuantity = parametrizationPaymentPlan.FirstPayQuantity;
                paymentScheduleEntity.PaymentQuantity = parametrizationPaymentPlan.Quantity;
                paymentScheduleEntity.GapUnitCode = parametrizationPaymentPlan.GapUnit;
                paymentScheduleEntity.GapQuantity = parametrizationPaymentPlan.GapQuantity;
                paymentScheduleEntity.LastPayQuantity = parametrizationPaymentPlan.LastPayQuantity;
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(paymentScheduleEntity);
                ParametrizationPaymentPlan parametrizationPaymentPlanResult = ModelAssembler.CreateParametrizationPaymentPlan(paymentScheduleEntity);
                return new UTMO.ResultValue<ParametrizationPaymentPlan, UTMO.ErrorModel>(parametrizationPaymentPlanResult);
            }
            catch (System.Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(Resources.Errors.FailedUpdatingPaymentPlanErrorBD);
                return new UTMO.ResultError<ParametrizationPaymentPlan, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Acceso a DB para la eliminacion de plan de pago
        /// </summary>
        /// <param name="parametrizationPaymentPlan">plan de pago MOD-B</param>
        /// <param name="isPrincipal">eliminacion Plan de pago y cuotas</param>
        /// <returns>Modelo Result con eliminacion de plan de pago</returns>
        public UTMO.Result<ParametrizationPaymentPlan, UTMO.ErrorModel> DeleteParametrizationPaymentPlan(ParametrizationPaymentPlan parametrizationPaymentPlan, bool isPrincipal)
        {
            List<string> listErrors = new List<string>();
            try
            {
                DataTable result;
                NameValue[] parameters = new NameValue[2];
                parameters[0] = new NameValue("@PAYMENT_SCHEDULE_ID", parametrizationPaymentPlan.Id);
                parameters[1] = new NameValue("@PRINCIPAL", isPrincipal);
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("PROD.DELETE_PAYMENT_SCHEDULE_PARAMETRIZATION", parameters);
                }

                if (((int)result.Rows[0][0]) == 1)
                {
                    parametrizationPaymentPlan.ParametrizationQuotas = new List<ParametrizationQuota>();
                    return new UTMO.ResultValue<ParametrizationPaymentPlan, UTMO.ErrorModel>(parametrizationPaymentPlan);
                }

                if (((int)result.Rows[0][0]) == -1)
                {
                    listErrors.Add(Resources.Errors.NoDeletePaymentPlanUse);
                    return new UTMO.ResultError<ParametrizationPaymentPlan, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, null));
                }
                listErrors.Add(Resources.Errors.FailedDeletingPaymentPlanErrorBD);
                return new UTMO.ResultError<ParametrizationPaymentPlan, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, null));
            }
            catch (System.Exception ex)
            {
                listErrors.Add(Resources.Errors.FailedDeletingPaymentPlanErrorBD);
                return new UTMO.ResultError<ParametrizationPaymentPlan, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Genera archivo excel de plan de pagos
        /// </summary>
        /// <param name="paymentPlans">listado de plan de pago a exportar</param>
        /// <param name="fileName">Nombre de archivo</param>
        /// <returns>lista de payment plan - MOD-B</returns>
        public UTMO.Result<string, UTMO.ErrorModel> GenerateFileToPaymentPlan(List<ParametrizationPaymentPlan> paymentPlans, string fileName)
        {
            List<string> listErrors = new List<string>();
            try
            {
                FileProcessValue fileProcessValue = new FileProcessValue()
                {
                    Key1 = (int)FileProcessType.ParametrizationPaymentPlan
                };
                FileDAO fileDAO = new FileDAO();
                File file = fileDAO.GetFileByFileProcessValue(fileProcessValue);

                if (file != null && file.IsEnabled)
                {
                    file.Name = fileName;
                    List<Row> rowsPaymentPlan = new List<Row>();
                    List<Row> rowsDistributionQuota = new List<Row>();

                    foreach (ParametrizationPaymentPlan item in paymentPlans)
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

                        fields[0].Value = item.Id.ToString();
                        fields[1].Value = item.Description.ToString();
                        fields[2].Value = item.SmallDescription.ToString();
                        if (!item.IsGreaterDate && item.IsIssueDate)
                        {
                            fields[3].Value = Resources.Errors.LabelDateofIssue;
                        }
                        else if (!item.IsGreaterDate && !item.IsIssueDate)
                        {
                            fields[3].Value = Resources.Errors.LabelStartDate;
                        }
                        else if (item.IsGreaterDate && !item.IsIssueDate)
                        {
                            fields[3].Value = Resources.Errors.LabelGreaterofBoth;
                        }

                        fields[4].Value = item.FirstPayQuantity.ToString();
                        fields[5].Value = item.LastPayQuantity.ToString();
                        if (item.GapUnit.ToString() == ((int)PaymentCalculationType.Day).ToString())
                        {
                            fields[6].Value = Resources.Errors.LabelDay;
                        }
                        else if (item.GapUnit.ToString() == ((int)PaymentCalculationType.Month).ToString())
                        {
                            fields[6].Value = Resources.Errors.LabelMonth;
                        }
                        else if (item.GapUnit.ToString() == ((int)PaymentCalculationType.Fortnight).ToString())
                        {
                            fields[6].Value = Resources.Errors.LabelFortnight;
                        }
                        fields[7].Value = item.Quantity.ToString();
                        rowsPaymentPlan.Add(new Row
                        {
                            Fields = fields
                        });


                        if (file.Templates.Count > 1)
                        {
                            if (item.ParametrizationQuotas != null)
                            {
                                foreach (var distributionQuota in item.ParametrizationQuotas)
                                {
                                    var fieldsDistribution = file.Templates[1].Rows[0].Fields.Select(x => new Field
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

                                    fieldsDistribution[0].Value = item.Id.ToString();
                                    fieldsDistribution[1].Value = item.Description.ToString();
                                    fieldsDistribution[2].Value = distributionQuota.Number.ToString();
                                    fieldsDistribution[3].Value = distributionQuota.GapQuantity.ToString();
                                    fieldsDistribution[4].Value = distributionQuota.Percentage.ToString();
                                    rowsDistributionQuota.Add(new Row
                                    {
                                        Fields = fieldsDistribution
                                    });
                                }

                            }
                        }

                    }
                    file.Templates[0].Rows = rowsPaymentPlan;
                    if (file.Templates.Count > 1 && rowsDistributionQuota.Count > 0)
                    {
                        file.Templates[1].Rows = rowsDistributionQuota;
                    }
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
    }
}
