using Sistran.Co.Application.Data;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ENUMUT = Sistran.Core.Application.Utilities.Enums;
using UTILMO = Sistran.Core.Services.UtilitiesServices.Models;
using UTMO = Sistran.Core.Application.Utilities.Error;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider.DAOs
{
    public class CompanyProductionReportDAO
    {
        /// <summary>
        /// Metodo para actualizar clausulas
        /// </summary>
        /// <param name="ProductionReport">Recibe reporte de producción</param>
        /// <returns>Retorna clausulas actualizadas</returns>
        public UTMO.Result<string, UTMO.ErrorModel> GenerateProductionReport(CompanyProductionReport CompanyProductionReport)
        {
            List<string> errorModelListDescription = new List<string>();
            try
            {
                List<CompanyProductionReport> listCompanyProductionReport = new List<CompanyProductionReport>();
                DataTable result;
                NameValue[] parameters = new NameValue[7];

                parameters[0] = new NameValue("@BRANCH_CD", CompanyProductionReport.BranchId);
                parameters[1] = new NameValue("@PREFIX_CD", CompanyProductionReport.PrefixId);
                parameters[2] = new NameValue("@AGENT_CD", CompanyProductionReport.AgentId);
                parameters[3] = new NameValue("@PRODUCT_CD", CompanyProductionReport.ProductId);
                parameters[4] = new NameValue("@CURRENT_FROM", CompanyProductionReport.InputFromDateTime);
                parameters[5] = new NameValue("@CURRENT_TO", CompanyProductionReport.InputToDateTime);
                parameters[6] = new NameValue("@USER_ID", CompanyProductionReport.UserId);

                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("REPORT.CO_GET_PRODUCTION_REPORT", parameters);
                }
                if (result != null)
                {
                    foreach (DataRow row in result.Rows)
                    {
                        string varPrime;
                        string varInsuredValue;
                        CompanyProductionReport ProductionReport = new CompanyProductionReport();

                        ProductionReport.BranchId = row[0].ToString() == "" ? 0 : Convert.ToInt32(row[0].ToString());
                        ProductionReport.BranchDescription = row[1].ToString();
                        ProductionReport.SalePointDescription = row[2].ToString();
                        ProductionReport.AgentId = row[3].ToString() == "" ? 0 : Convert.ToInt32(row[3].ToString());
                        ProductionReport.PolicyId = row[4].ToString() == "" ? 0 : Convert.ToInt32(row[4].ToString());
                        ProductionReport.EndorsementId = row[5].ToString() == "" ? 0 : Convert.ToInt32(row[5].ToString());
                        ProductionReport.GroupEndorsement = row[6].ToString();
                        ProductionReport.Plate = row[7].ToString();
                        ProductionReport.CoveragePlan = row[8].ToString();
                        ProductionReport.FasecoldaCode = row[9].ToString() == "" ? 0 : Convert.ToInt32(row[9].ToString());
                        ProductionReport.Comision = row[10].ToString() == "" ? 0 : Convert.ToDecimal(row[10].ToString());
                        if (row[11].ToString() != "")
                        {
                            varInsuredValue = row[11].ToString();
                            string[] splitInsuredValue = varInsuredValue.Split('.');
                            decimal endInsuredValue = decimal.Parse(splitInsuredValue[0]);
                            ProductionReport.InsuredValue = endInsuredValue.ToString("N");
                        }
                        else
                        {
                            ProductionReport.InsuredValue = "0";
                        }
                        if (row[12].ToString() != "")
                        {
                            varPrime = row[12].ToString();
                            string[] splitvarPrime = varPrime.Split('.');
                            decimal endPrime = decimal.Parse(splitvarPrime[0]);
                            ProductionReport.Prime = endPrime.ToString("N");
                        }
                        else
                        {
                            ProductionReport.Prime = "0";
                        }
                        ProductionReport.DateExpedition = row[13].ToString();
                        ProductionReport.DateFrom = row[14].ToString();
                        ProductionReport.DateTo = row[15].ToString();

                        listCompanyProductionReport.Add(ProductionReport);
                    }
                }

                FileDAO fileDAO = new FileDAO();
                FileProcessValue fileProcessValue = new FileProcessValue()
                {
                    Key1 = (int)FileProcessType.ParametrizationProductionReport
                };

                UTILMO.File file = fileDAO.GetFileByFileProcessValue(fileProcessValue);

                if (file != null && file.IsEnabled)
                {
                    file.Name = "ReporteProducción";
                    List<UTILMO.Row> rows = new List<UTILMO.Row>();

                    foreach (CompanyProductionReport companyProductionReport in listCompanyProductionReport)
                    {
                        List<Field> fields = file.Templates[0].Rows[0].Fields.Select(x => new UTILMO.Field
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

                        fields[0].Value = companyProductionReport.BranchId.ToString();
                        fields[1].Value = companyProductionReport.BranchDescription;
                        fields[2].Value = companyProductionReport.SalePointDescription;
                        fields[3].Value = companyProductionReport.AgentId.ToString();
                        fields[4].Value = companyProductionReport.PolicyId.ToString();
                        fields[5].Value = companyProductionReport.EndorsementId.ToString();
                        fields[6].Value = companyProductionReport.GroupEndorsement;
                        fields[7].Value = companyProductionReport.Plate;
                        fields[8].Value = companyProductionReport.CoveragePlan;
                        fields[9].Value = companyProductionReport.FasecoldaCode.ToString();
                        fields[10].Value = companyProductionReport.Comision.ToString();
                        fields[11].Value = companyProductionReport.InsuredValue.ToString();
                        fields[12].Value = companyProductionReport.Prime.ToString();
                        fields[13].Value = companyProductionReport.DateExpedition;
                        fields[14].Value = companyProductionReport.DateFrom;
                        fields[15].Value = companyProductionReport.DateTo;

                        rows.Add(new UTILMO.Row
                        {
                            Fields = fields
                        });
                    }
                    file.Templates[0].Rows = rows;
                    file.Name = string.Format(file.Name + "_" + DateTime.Now.ToString("dd_MM_yyyy"));
                    return new UTMO.ResultValue<string, UTMO.ErrorModel>(fileDAO.GenerateFile(file));
                }
                else
                {
                    return new UTMO.ResultValue<string, UTMO.ErrorModel>(string.Empty);
                }
            }
            catch (System.Exception ex)
            {
                if (ex.Message== "La secuencia no contiene elementos" || ex.Message == "Sequence contains no elements")
                {
                    errorModelListDescription.Add("Resources.Errors.ErrorNoDataFound");
                    return new UTMO.ResultError<string, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(errorModelListDescription, ENUMUT.ErrorType.NotFound, ex));
                }
                else
                {
                    errorModelListDescription.Add("Resources.Errors.ErrorExecuteQuery");
                    return new UTMO.ResultError<string, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(errorModelListDescription, ENUMUT.ErrorType.TechnicalFault, ex));
                }
            }
        }
    }
}
