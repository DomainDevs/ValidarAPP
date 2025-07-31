// -----------------------------------------------------------------------
// <copyright file="DetailDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Gina Gómez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.DAOs
{
    using Sistran.Co.Application.Data;

    using Sistran.Core.Application.UnderwritingParamService.Models;
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.Assemblers;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Application.UtilitiesServicesEEProvider.DAOs;
    using Sistran.Core.Framework.DAF.Engine;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.Queries;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    
    using MODEN = Sistran.Core.Application.ModelServices.Enums;
    using QUOET = Sistran.Core.Application.Quotation.Entities;
    using UNDET = Sistran.Core.Application.UnderwritingParamService.EEProvider.Entities.Views;
    using UTIEN = Sistran.Core.Application.Utilities.Enums;
    using UTILEN = Sistran.Core.Services.UtilitiesServices.Enums;
    using UTILMO = Sistran.Core.Services.UtilitiesServices.Models;
    using UTIMO = Sistran.Core.Application.Utilities.Error;
    /// <summary>
    /// Clase para detalle
    /// </summary>
    public class DetailDAO
    {       
        /// <summary>
        /// Acceso a DB para consultar listado de Detalle
        /// </summary>
        /// <returns>Listado Result consulta en DB</returns>
        public UTIMO.Result<List<ParametrizationDetail>, UTIMO.ErrorModel> GetParametrizationDetails()
        {
            try
            {
                UNDET.DetailTypeView view = new UNDET.DetailTypeView();
                ViewBuilder builder = new ViewBuilder("DetailTypeView");
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

                List<ParametrizationDetail> parametrizationDetail = ModelAssembler.CreateParametrizationDetails(view.Details);

                foreach (ParametrizationDetail item in parametrizationDetail)
                {
                    if (item.DetailType.Id != 0)
                    {
                        item.DetailType.Description = (view.DetailTypes.First(x => ((QUOET.DetailType)x).DetailTypeCode == item.DetailType.Id) as QUOET.DetailType).Description;
                    }
                }

                return new UTIMO.ResultValue<List<ParametrizationDetail>, UTIMO.ErrorModel>(parametrizationDetail);
            }
            catch (System.Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(Resources.Errors.ErrorOccurredIn + " GetParametrizationDetails");
                return new UTIMO.ResultError<List<ParametrizationDetail>, UTIMO.ErrorModel>(UTIMO.ErrorModel.CreateErrorModel(listErrors, UTIEN.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Validación de detaller
        /// </summary>
        /// <param name="detailId">Codigo de detalle</param>
        /// <returns>valor 1: tiene dependencias, 0: no tiene dependencias</returns> 
        public int ValidateDetail(int detailId)
        {
            DataTable result;
            NameValue[] parameters = new NameValue[1];
            parameters[0] = new NameValue("@DETAIL_ID", detailId);
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("QUO.VALIDATE_DETAIL_PARAMETRIZATION", parameters);
            }

            return (int)result.Rows[0][0];
        }
        
        /// <summary>
        /// Genera archivo excel de detalles
        /// </summary>
        /// <param name="details">listado de detalles a exportar</param>
        /// <param name="fileName">Nombre de archivo</param>
        /// <returns>lista de detalles MOD-B</returns>
        public UTIMO.Result<string, UTIMO.ErrorModel> GenerateFileToPaymentPlan(List<ParametrizationDetail> details, string fileName)
        {
            List<string> listErrors = new List<string>();
            listErrors.Add(Resources.Errors.ErrorDownloadingExcel);
            try
            {
                UTILMO.FileProcessValue fileProcessValue = new UTILMO.FileProcessValue()
                {
                    Key1 = (int)UTILEN.FileProcessType.ParametrizationDetail
                };
                FileDAO fileDAO = new FileDAO();
                UTILMO.File file = fileDAO.GetFileByFileProcessValue(fileProcessValue);

                if (file != null && file.IsEnabled)
                {
                    file.Name = fileName;
                    List<UTILMO.Row> rows = new List<UTILMO.Row>();

                    foreach (ParametrizationDetail item in details)
                    {
                        var fields = file.Templates[0].Rows[0].Fields.OrderBy(x => x.Order).Select(x => new UTILMO.Field
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
                        fields[2].Value = item.DetailType.Description.ToString();

                        if (item.Enabled)
                        {
                            fields[3].Value = Resources.Errors.Yes;
                        }
                        else
                        {
                            fields[3].Value = Resources.Errors.No;
                        }

                        if (item.RateType == MODEN.RateType.FixedValue)
                        {
                            fields[4].Value = Resources.Errors.FixedValue;
                        }
                        else if (item.RateType == MODEN.RateType.Percentage)
                        {
                            fields[4].Value = Resources.Errors.Percentage;
                        }
                        else if (item.RateType == MODEN.RateType.Permilage)
                        {
                            fields[4].Value = Resources.Errors.Permilage;
                        }

                        fields[5].Value = item.Rate.ToString();
                        fields[6].Value = item.SublimitAmt.ToString();

                        rows.Add(new UTILMO.Row
                        {
                            Fields = fields
                        });
                    }

                    file.Templates[0].Rows = rows;
                    file.Name = fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy");

                    var result = fileDAO.GenerateFile(file);
                    return new UTIMO.ResultValue<string, UTIMO.ErrorModel>(result);
                }
                else
                {
                    return new UTIMO.ResultError<string, UTIMO.ErrorModel>(UTIMO.ErrorModel.CreateErrorModel(listErrors, UTIEN.ErrorType.TechnicalFault, new System.ArgumentException(Resources.Errors.ErrorDownloadingExcel, "original")));
                }
            }
            catch (System.Exception ex)
            {
                return new UTIMO.ResultError<string, UTIMO.ErrorModel>(UTIMO.ErrorModel.CreateErrorModel(listErrors, UTIEN.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Obtiene los tipos d detalles
        /// REQ_#57
        /// </summary>
        /// <returns>listado de deducibles</returns>
        public UTIMO.Result<List<ParamDetailTypeDesc>, UTIMO.ErrorModel> GetParamDetailDescs()
        {
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(QUOET.DetailType)));
                List<ParamDetailTypeDesc> paramDetailTypeDescs = ModelAssembler.CreateParamDetailTypeDescs(businessCollection);
                return new UTIMO.ResultValue<List<ParamDetailTypeDesc>, UTIMO.ErrorModel>(paramDetailTypeDescs);
            }
            catch (System.Exception ex)
            {
                return new UTIMO.ResultError<List<ParamDetailTypeDesc>, UTIMO.ErrorModel>(UTIMO.ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.ErrorDBGetDetailType }, UTIEN.ErrorType.TechnicalFault, ex));
            }
        }


        /// <summary>
        /// Obtiene listado relacionado de tipos de detlle con Id de cobertura
        /// REQ_#57
        /// </summary>
        /// <param name="coverageId">id de cobertura</param>
        /// <returns>listado de tipos de detlle</returns>
        public UTIMO.Result<List<ParamDetailTypeDesc>, UTIMO.ErrorModel> GetParamDetailTypesDescsByCoverageId(int coverageId)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(QUOET.CoverDetailType.Properties.CoverageId, typeof(QUOET.CoverDetailType).Name);
                filter.Equal();
                filter.Constant(coverageId);
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(QUOET.CoverDetailType), filter.GetPredicate()));
                List<ParamDetailTypeDesc> paramDetailTypeDescs = ModelAssembler.CreateDetailTypesRelation(businessCollection);
                return new UTIMO.ResultValue<List<ParamDetailTypeDesc>, UTIMO.ErrorModel>(paramDetailTypeDescs);
            }
            catch (System.Exception ex)
            {
                return new UTIMO.ResultError<List<ParamDetailTypeDesc>, UTIMO.ErrorModel>(UTIMO.ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.ErrorDBGetDetailType }, UTIEN.ErrorType.TechnicalFault, ex));
            }
        }
    }
}
