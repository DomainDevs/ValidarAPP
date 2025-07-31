// -----------------------------------------------------------------------
// <copyright file="QuotationNumberDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Manuel Méndez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.DAOs
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Sistran.Co.Application.Data;
    using Sistran.Core.Application.CommonService.Models;
    using UnderwritingParamServices.EEProvider.Assemblers;    
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Entities.Views;
    using Sistran.Core.Application.UnderwritingParamService.Models;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Framework.DAF.Engine;
    using Sistran.Core.Framework.Queries;
    using COMMEN = Sistran.Core.Application.Common.Entities;    
    using ENUMUT = Sistran.Core.Application.Utilities.Enums;
    using QUOEN = Sistran.Core.Application.Quotation.Entities;
    using Resources = Sistran.Core.Application.UnderwritingParamService.EEProvider.Resources;
    using UTMO = Sistran.Core.Application.Utilities.Error;
    using Sistran.Core.Services.UtilitiesServices.Models;
    using Sistran.Core.Services.UtilitiesServices.Enums;

    /// <summary>
    /// Acceso a DB de número de cotización
    /// </summary>
    public class QuotationNumberDAO
    {
        /// <summary>
        /// Consulta los números de cotización por sucursal
        /// </summary>
        /// <param name="branchId">Id de sucrusal</param>
        /// <returns>Lista de números de cotización</returns>
        public UTMO.Result<List<ParametrizationQuotationNumber>, UTMO.ErrorModel> GetParametrizationQuotationNumbersByBranchId(int branchId)
        {
            try
            {
                List<ParametrizationQuotationNumber> listParametrizationQuotationNumber = new List<ParametrizationQuotationNumber>();
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(QUOEN.QuotationNumber.Properties.BranchCode, typeof(QUOEN.QuotationNumber).Name);
                filter.Equal();
                filter.Constant(branchId);

                ParametrizationQuotationNumberView view = new ParametrizationQuotationNumberView();
                ViewBuilder builder = new ViewBuilder("ParametrizationQuotationNumberView");
                builder.Filter = filter.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

                if (view.QuotationNumbers.Count > 0)
                {
                    foreach (QUOEN.QuotationNumber quotationNumberEntity in view.QuotationNumbers.Cast<QUOEN.QuotationNumber>())
                    {
                        COMMEN.Branch branchEntity = view.Branchs.Cast<COMMEN.Branch>().First(x => x.BranchCode == quotationNumberEntity.BranchCode);
                        COMMEN.Prefix prefixEntity = view.Prefixes.Cast<COMMEN.Prefix>().First(x => x.PrefixCode == quotationNumberEntity.PrefixCode);
                        List<QUOEN.Quotation> quotationEntity = view.Quotations.Cast<QUOEN.Quotation>().Where(x => x.DocumentNum == quotationNumberEntity.QuotNumber).ToList();
                        listParametrizationQuotationNumber.Add(new ParametrizationQuotationNumber()
                        {
                            HasQuotation = quotationEntity.Count > 0,
                            QuotNumber = quotationNumberEntity.QuotNumber,
                            Branch = ModelAssembler.CreateParamBranch(branchEntity),
                            Prefix = ModelAssembler.CreateParamPrefix(prefixEntity)
                        });
                    }
                }

                return new UTMO.ResultValue<List<ParametrizationQuotationNumber>, UTMO.ErrorModel>(listParametrizationQuotationNumber);
            }
            catch (System.Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(Resources.Errors.ErrorGetQuotationNumer);
                return new UTMO.ResultError<List<ParametrizationQuotationNumber>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Consulta los números de cotización por sucursal y ramo
        /// </summary>
        /// <param name="branchId">Id de sucrusal</param>
        /// <param name="prefixId">Id de ramo</param>
        /// <returns>Lista de números de cotización</returns>
        public UTMO.Result<List<ParametrizationQuotationNumber>, UTMO.ErrorModel> GetParametrizationQuotationNumbersByBranchIdPrefixId(int branchId, int prefixId)
        {
            try
            {
                List<ParametrizationQuotationNumber> listParametrizationQuotationNumber = new List<ParametrizationQuotationNumber>();
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(QUOEN.QuotationNumber.Properties.BranchCode, typeof(QUOEN.QuotationNumber).Name);
                filter.Equal();
                filter.Constant(branchId);
                filter.And();
                filter.Property(QUOEN.QuotationNumber.Properties.PrefixCode, typeof(QUOEN.QuotationNumber).Name);
                filter.Equal();
                filter.Constant(prefixId);

                ParametrizationQuotationNumberView view = new ParametrizationQuotationNumberView();
                ViewBuilder builder = new ViewBuilder("ParametrizationQuotationNumberView");
                builder.Filter = filter.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

                if (view.QuotationNumbers.Count > 0)
                {
                    foreach (QUOEN.QuotationNumber quotationNumberEntity in view.QuotationNumbers.Cast<QUOEN.QuotationNumber>())
                    {
                        COMMEN.Branch branchEntity = view.Branchs.Cast<COMMEN.Branch>().First(x => x.BranchCode == quotationNumberEntity.BranchCode);
                        COMMEN.Prefix prefixEntity = view.Prefixes.Cast<COMMEN.Prefix>().First(x => x.PrefixCode == quotationNumberEntity.PrefixCode);
                        List<QUOEN.Quotation> quotationEntity = view.Quotations.Cast<QUOEN.Quotation>().Where(x => x.DocumentNum == quotationNumberEntity.QuotNumber).ToList();
                        listParametrizationQuotationNumber.Add(new ParametrizationQuotationNumber()
                        {
                            HasQuotation = quotationEntity.Count > 0,
                            QuotNumber = quotationNumberEntity.QuotNumber,
                            Branch = ModelAssembler.CreateParamBranch(branchEntity),
                            Prefix = ModelAssembler.CreateParamPrefix(prefixEntity)
                        });
                    }
                }

                return new UTMO.ResultValue<List<ParametrizationQuotationNumber>, UTMO.ErrorModel>(listParametrizationQuotationNumber);
            }
            catch (System.Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(Resources.Errors.ErrorGetQuotationNumer);
                return new UTMO.ResultError<List<ParametrizationQuotationNumber>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }
        
        /// <summary>
        /// Generar archivo excel de números de cotización
        /// </summary>
        /// <param name="fileName">Nombre de archivo</param>
        /// <returns>Archivo de excel</returns>
        public UTMO.Result<string, UTMO.ErrorModel> GenerateFileToQuotationNumber(string fileName)
        {
            List<string> listErrors = new List<string>();
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

                ParametrizationQuotationNumberView view = new ParametrizationQuotationNumberView();
                ViewBuilder builder = new ViewBuilder("ParametrizationQuotationNumberView");
                builder.Filter = filter.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

                FileProcessValue fileProcessValue = new FileProcessValue()
                {
                    Key1 = (int)FileProcessType.ParametrizationQuotationNumber
                };
                FileDAO fileDAO = new FileDAO();
                File file = fileDAO.GetFileByFileProcessValue(fileProcessValue);

                if (file != null && file.IsEnabled)
                {
                    file.Name = fileName;
                    List<Row> rows = new List<Row>();

                    foreach (QUOEN.QuotationNumber quotationNumberEntity in view.QuotationNumbers.Cast<QUOEN.QuotationNumber>())
                    {
                        COMMEN.Branch branchEntity = view.Branchs.Cast<COMMEN.Branch>().First(x => x.BranchCode == quotationNumberEntity.BranchCode);
                        COMMEN.Prefix prefixEntity = view.Prefixes.Cast<COMMEN.Prefix>().First(x => x.PrefixCode == quotationNumberEntity.PrefixCode);
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

                        fields[0].Value = branchEntity.BranchCode.ToString();
                        fields[1].Value = branchEntity.Description;
                        fields[2].Value = prefixEntity.PrefixCode.ToString();
                        fields[3].Value = prefixEntity.Description;
                        fields[4].Value = quotationNumberEntity.QuotNumber.ToString();

                        rows.Add(new Row
                        {
                            Fields = fields
                        });
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
                return new UTMO.ResultError<string, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, new System.ArgumentException(Resources.Errors.ErrorDownloadingExcel, "original")));
            }
        }

        /// <summary>
        /// Validación de numeración de cotización
        /// </summary>
        /// <param name="branchId">Codigo de sucursal</param>
        /// <param name="prefixId">Codigo de ramo</param>
        /// <returns>valor 1: tiene dependencias, 0: no tiene dependencias</returns> 
        public int ValidateQuotationNumber(int branchId, int prefixId)
        {
            DataTable result;
            NameValue[] parameters = new NameValue[2];
            parameters[0] = new NameValue("@BRANCH_ID", branchId);
            parameters[1] = new NameValue("@PREFIX_ID", prefixId);
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("QUO.VALIDATE_QUOTATION_NUMBER_PARAMETRIZATION", parameters);
            }

            return (int)result.Rows[0][0];
        }
    }
}
