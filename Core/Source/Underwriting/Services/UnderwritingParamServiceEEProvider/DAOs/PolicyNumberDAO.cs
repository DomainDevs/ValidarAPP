// -----------------------------------------------------------------------
// <copyright file="PolicyNumberDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Oscar Camacho</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.DAOs
{
    using Sistran.Co.Application.Data;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Entities.Views;
    using Sistran.Core.Application.UnderwritingParamService.Models;
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Framework.DAF.Engine;
    using Sistran.Core.Framework.Queries;
    using Sistran.Core.Services.UtilitiesServices.Enums;
    using Sistran.Core.Services.UtilitiesServices.Models;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using UnderwritingParamServices.EEProvider.Assemblers;
    using COMMEN = Sistran.Core.Application.Common.Entities;
    using ENUMUT = Sistran.Core.Application.Utilities.Enums;
    using ISSEN = Sistran.Core.Application.Issuance.Entities;
    using Resources = Sistran.Core.Application.UnderwritingParamService.EEProvider.Resources;
    using UTMO = Sistran.Core.Application.Utilities.Error;

    /// <summary>
    /// Acceso a DB de número de póliza
    /// </summary>
    public class PolicyNumberDAO
    {
        /// <summary>
        /// Consulta los números de póliza por sucursal
        /// </summary>
        /// <param name="branchId">Id de sucrusal</param>
        /// <returns>Lista de números de póliza</returns>
        public UTMO.Result<List<ParamPolicyNumber>, UTMO.ErrorModel> GetParamPolicyNumbersByBranchId(int branchId)
        {
            try
            {
                List<ParamPolicyNumber> listParamPolicyNumber = new List<ParamPolicyNumber>();
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(ISSEN.PolicyNumber.Properties.BranchCode, typeof(ISSEN.PolicyNumber).Name);
                filter.Equal();
                filter.Constant(branchId);

                ParamPolicyNumberView view = new ParamPolicyNumberView();
                ViewBuilder builder = new ViewBuilder("ParamPolicyNumberView");
                builder.Filter = filter.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

                if (view.PolicyNumbers.Count > 0)
                {
                    foreach (ISSEN.PolicyNumber policyNumberEntity in view.PolicyNumbers.Cast<ISSEN.PolicyNumber>())
                    {
                        COMMEN.Branch branchEntity = view.Branchs.Cast<COMMEN.Branch>().First(x => x.BranchCode == policyNumberEntity.BranchCode);
                        COMMEN.Prefix prefixEntity = view.Prefixes.Cast<COMMEN.Prefix>().First(x => x.PrefixCode == policyNumberEntity.PrefixCode);
                        List<ISSEN.Policy> policyEntity = view.Policys.Cast<ISSEN.Policy>().Where(x => x.DocumentNumber == policyNumberEntity.LastPolicyNum).ToList();

                        listParamPolicyNumber.Add(new ParamPolicyNumber()
                        {
                            HasPolicy = policyEntity.Count > 0,
                            PolicyNumber = (int)policyNumberEntity.LastPolicyNum,
                            Branch = ModelAssembler.CreateParamBranch(branchEntity),
                            Prefix = ModelAssembler.CreateParamPrefix(prefixEntity)
                        });
                    }
                }

                return new UTMO.ResultValue<List<ParamPolicyNumber>, UTMO.ErrorModel>(listParamPolicyNumber);
            }
            catch (System.Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(Resources.Errors.ErrorGetPolicyNumer);
                return new UTMO.ResultError<List<ParamPolicyNumber>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Consulta los números de póliza por sucursal y ramo
        /// </summary>
        /// <param name="branchId">Id de sucrusal</param>
        /// <param name="prefixId">Id de ramo</param>
        /// <returns>Lista de números de póliza</returns>
        public UTMO.Result<List<ParamPolicyNumber>, UTMO.ErrorModel> GetParamPolicyNumbersByBranchIdPrefixId(int branchId, int prefixId)
        {
            try
            {
                List<ParamPolicyNumber> listParamPolicyNumber = new List<ParamPolicyNumber>();
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(ISSEN.PolicyNumber.Properties.BranchCode, typeof(ISSEN.PolicyNumber).Name);
                filter.Equal();
                filter.Constant(branchId);
                filter.And();
                filter.Property(ISSEN.PolicyNumber.Properties.PrefixCode, typeof(ISSEN.PolicyNumber).Name);
                filter.Equal();
                filter.Constant(prefixId);

                ParamPolicyNumberView view = new ParamPolicyNumberView();
                ViewBuilder builder = new ViewBuilder("ParamPolicyNumberView");
                builder.Filter = filter.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

                if (view.PolicyNumbers.Count > 0)
                {
                    foreach (ISSEN.PolicyNumber policyNumberEntity in view.PolicyNumbers.Cast<ISSEN.PolicyNumber>())
                    {
                        COMMEN.Branch branchEntity = view.Branchs.Cast<COMMEN.Branch>().First(x => x.BranchCode == policyNumberEntity.BranchCode);
                        COMMEN.Prefix prefixEntity = view.Prefixes.Cast<COMMEN.Prefix>().First(x => x.PrefixCode == policyNumberEntity.PrefixCode);
                        List<ISSEN.Policy> policyEntity = view.Policys.Cast<ISSEN.Policy>().Where(x => x.DocumentNumber == policyNumberEntity.LastPolicyNum).ToList();

                        listParamPolicyNumber.Add(new ParamPolicyNumber()
                        {
                            HasPolicy = policyEntity.Count > 0,
                            PolicyNumber = (decimal)policyNumberEntity.LastPolicyNum,
                            LastPolicyDate = policyNumberEntity.LastPolicyDate,
                            Branch = ModelAssembler.CreateParamBranch(branchEntity),
                            Prefix = ModelAssembler.CreateParamPrefix(prefixEntity)
                        });
                    }
                }

                return new UTMO.ResultValue<List<ParamPolicyNumber>, UTMO.ErrorModel>(listParamPolicyNumber);
            }
            catch (System.Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(Resources.Errors.ErrorGetPolicyNumer);
                return new UTMO.ResultError<List<ParamPolicyNumber>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }
        
        /// <summary>
        /// Generar archivo excel de números de póliza
        /// </summary>
        /// <param name="fileName">Nombre de archivo</param>
        /// <returns>Archivo de excel</returns>
        public UTMO.Result<string, UTMO.ErrorModel> GenerateFileToPolicyNumber(string fileName)
        {
            List<string> listErrors = new List<string>();
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

                ParamPolicyNumberView view = new ParamPolicyNumberView();
                ViewBuilder builder = new ViewBuilder("ParamPolicyNumberView");
                builder.Filter = filter.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

                FileProcessValue fileProcessValue = new FileProcessValue()
                {
                    Key1 = (int)FileProcessType.ParametrizationPolicyNumber
                };
                FileDAO fileDAO = new FileDAO();
                File file = fileDAO.GetFileByFileProcessValue(fileProcessValue);

                if (file != null && file.IsEnabled)
                {
                    file.Name = fileName;
                    List<Row> rows = new List<Row>();

                    foreach (ISSEN.PolicyNumber policyNumberEntity in view.PolicyNumbers.Cast<ISSEN.PolicyNumber>())
                    {
                        COMMEN.Branch branchEntity = view.Branchs.Cast<COMMEN.Branch>().First(x => x.BranchCode == policyNumberEntity.BranchCode);
                        COMMEN.Prefix prefixEntity = view.Prefixes.Cast<COMMEN.Prefix>().First(x => x.PrefixCode == policyNumberEntity.PrefixCode);
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
                        fields[4].Value = policyNumberEntity.LastPolicyNum.ToString();
                        fields[5].Value = policyNumberEntity.LastPolicyDate.ToShortDateString();

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
        /// Validación de numeración de póliza
        /// </summary>
        /// <param name="branchId">Codigo de sucursal</param>
        /// <param name="prefixId">Codigo de ramo</param>
        /// <returns>valor 1: tiene dependencias, 0: no tiene dependencias</returns> 
        public int ValidatePolicyNumber(int branchId, int prefixId)
        {
            DataTable result;
            NameValue[] parameters = new NameValue[2];
            parameters[0] = new NameValue("@BRANCH_ID", branchId);
            parameters[1] = new NameValue("@PREFIX_ID", prefixId);
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("ISS.VALIDATE_ISSUANCE_NUMBER_PARAMETRIZATION", parameters);
            }

            return (int)result.Rows[0][0];
        }
    }
}
