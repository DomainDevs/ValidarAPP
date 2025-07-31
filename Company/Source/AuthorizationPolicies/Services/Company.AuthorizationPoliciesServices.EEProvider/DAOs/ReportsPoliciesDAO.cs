using Company.AuthorizationPoliciesServices.EEProvider.Resources;
using Sistran.Co.Application.Data;
using Sistran.Company.Application.AuthorizationPoliciesServices.EEProvider.Assemblers;
using Sistran.Company.Application.AuthorizationPoliciesServices.Models;
using Sistran.Company.Application.Utilities.Enums;
using Sistran.Company.Application.Utilities.Error;
using System;
using System.Collections.Generic;
using System.Data;

namespace Sistran.Company.Application.AuthorizationPoliciesServices.EEProvider.DAOs
{
    public class ReportsPoliciesDAO
    {
        public Result<CompanyReportPolicies, ErrorModel> GetPolicies(CompanyPolicyValid companyPoliciesValid)
        {
            DataTable dtStatus = new DataTable("PARAM_TEMP_STATUS_POLICY");
            dtStatus.Columns.Add("STATUS_ID", typeof(int));
            dtStatus.Columns.Add("DESCRIPTION_STATUS", typeof(string));
            foreach (int item in companyPoliciesValid.Status)
            {
                DataRow StatusValue = dtStatus.NewRow();
                StatusValue["STATUS_ID"] = item;
                dtStatus.Rows.Add(StatusValue);
            }

            NameValue[] parameters = new NameValue[5];

            parameters[0] = new NameValue("@INSERT_TEMP_STATUS_POLICY", dtStatus);
            parameters[1] = new NameValue("@FECHAINI", companyPoliciesValid.dateIni);
            parameters[2] = new NameValue("@FECHAFIN", companyPoliciesValid.dateFin);
            parameters[3] = new NameValue("@PREFIX", companyPoliciesValid.idPrefix);
            parameters[4] = new NameValue("@BRANCH", companyPoliciesValid.idBranch);

            Result<DataSet, ErrorModel> result = GenericExecuteStoredProcedured("[AUTHO].[GET_REPORT_POLICIES]", parameters);
            CompanyReportPolicies companyPolicies = new CompanyReportPolicies();            

            if (result is ResultError<DataSet, ErrorModel>)
            {
                ErrorModel errorModelResult = (result as ResultError<DataSet, ErrorModel>).Message;
                return new ResultError<CompanyReportPolicies, ErrorModel>(ErrorModel.CreateErrorModel(errorModelResult.ErrorDescription, errorModelResult.ErrorType, null));
            }
            else
            {
                companyPolicies = ModelAssembler.CreateCompanyReportPolicies((result as ResultValue<DataSet, ErrorModel>).Value);

                if (companyPolicies.companyPolicies.Count == 0)
                {
                    return new ResultError<CompanyReportPolicies, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { Errors.ScorCreditNotFound }, ErrorType.NotFound, null));
                }
                else
                {
                    return new ResultValue<CompanyReportPolicies, ErrorModel>(companyPolicies);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="NameStoredProcedure"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private Result<DataSet, ErrorModel> GenericExecuteStoredProcedured(string NameStoredProcedure, NameValue[] parameters)
        {
            DataSet dataSet;
            List<string> errorModel = new List<string>();
            try
            {
                using (DynamicDataAccess pdb = new DynamicDataAccess())
                {
                    dataSet = pdb.ExecuteSPDataSet(NameStoredProcedure, parameters);
                }
                return new ResultValue<DataSet, ErrorModel>(dataSet);
            }
            catch (Exception ex)
            {
                errorModel.Add(Errors.ErrorQueryReports + NameStoredProcedure);
                return new ResultError<DataSet, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.TechnicalFault, ex));
            }
        }

        public string GenerateExcelReportAuthorizationPolicies(string fileName, CompanyPolicyValid cvp)
        {
            Result<CompanyReportPolicies, ErrorModel> listreport = GetPolicies(cvp);

            if(listreport is ResultError<CompanyReportPolicies, ErrorModel>)
            {
                return "Error al consultar";
            }
            else
            {
                CompanyReportPolicies report = (listreport as ResultValue<CompanyReportPolicies, ErrorModel>).Value;
                FileDAOs fileDAOs = new FileDAOs();
                return fileDAOs.GenerateFileToReportAuthorizationPolicies(report.companyPolicies, fileName);
            }
        }
    }
}
