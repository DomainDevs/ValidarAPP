using CrystalDecisions.CrystalReports.Engine;
using Ionic.Zip;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using Sistran.Co.Application.Data;
using Sistran.Company.Application.Location.LiabilityServices.Models;
using Sistran.Company.Application.Location.PropertyServices.Models;
using Sistran.Company.Application.PrintingServices.Enums;
using Sistran.Company.Application.PrintingServices.Models;
using Sistran.Company.Application.Sureties.JudicialSuretyServices.Models;
using Sistran.Company.Application.Sureties.SuretyServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.Models;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.PrintingServices.Models;
using Sistran.Core.Application.Sureties.SuretyServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Framework;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.PrintingServicesEEProvider.DAOs
{
    public class PrintingDAO
    {
        private static string reportCreatePath = ConfigurationSettings.AppSettings["ReportCreatePath"];
        private static string reportExportPath = ConfigurationSettings.AppSettings["ReportExportPath"];
        private static string reportTemplatePath = ConfigurationSettings.AppSettings["ReportTemplatePath"];
        private static List<CompanyInsuredObject> companyInsuredObject = new List<CompanyInsuredObject>();
        private static List<DocumentType> documentTypes = new List<DocumentType>();
        string LegalRepresentative = string.Empty;
        string FromNumber = string.Empty;

        /// <summary>
        /// Generar reporte de un póliza de individual
        /// </summary>
        /// <param name="filterReport">Filtro</param>
        /// <returns>Ruta Reporte</returns>
        public string GenerateReport(CompanyFilterReport companyFilterReport)
        {
            string userFolderPath = reportCreatePath + companyFilterReport.User.AccountName + @"\";

            if (!Directory.Exists(userFolderPath))
            {
                Directory.CreateDirectory(userFolderPath);
            }

            List<string> reportPaths = new List<string>();

            CompanyPolicy companyPolicy = new CompanyPolicy();

            if (companyFilterReport.EndorsementId == 0)
            {
                companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyFilterReport.TemporalId, false);
            }
            else
            {
                companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(companyFilterReport.EndorsementId);
            }

            if (companyPolicy.Product.CoveredRisk.SubCoveredRiskType != null)
            {
                switch (companyPolicy.Product.CoveredRisk.SubCoveredRiskType)
                {
                    case SubCoveredRiskType.Vehicle:
                        reportPaths = GenerateReportVehicle(companyFilterReport, userFolderPath);
                        break;
                    case SubCoveredRiskType.ThirdPartyLiability:
                        reportPaths = GenerateReportThirdPartyLiability(companyFilterReport, userFolderPath);
                        break;
                    case SubCoveredRiskType.Property:
                        reportPaths = GenerateReportProperty(companyFilterReport, userFolderPath);
                        break;
                    case SubCoveredRiskType.Liability:
                        reportPaths = GenerateReportLiability(companyFilterReport, userFolderPath);
                        break;
                    case SubCoveredRiskType.Surety:
                        reportPaths = GenerateReportSurety(companyFilterReport, userFolderPath);
                        break;
                    case SubCoveredRiskType.JudicialSurety:
                        reportPaths = GenerateReportJudicialSurety(companyFilterReport, userFolderPath);
                        break;
                };
            }
            else
            {
                switch (companyPolicy.Product.CoveredRisk.CoveredRiskType)
                {
                    case CoveredRiskType.Vehicle:
                        reportPaths = GenerateReportVehicle(companyFilterReport, userFolderPath);
                        break;
                    case CoveredRiskType.Location:
                        reportPaths = GenerateReportProperty(companyFilterReport, userFolderPath);
                        break;
                }
            }

            if (reportPaths.Count > 0)
            {
                string reportPath = CreateReportPath(userFolderPath, companyPolicy, "_" + DateTime.Now.ToString("dd-MM-yyyy"), companyPolicy.Id);
                string exportPath = CreateReportPath(reportExportPath, companyPolicy, "_" + DateTime.Now.ToString("dd-MM-yyyy"), companyPolicy.Id);
                Merge(reportPaths, reportPath, exportPath, companyFilterReport.EndorsementId);

                if (reportPaths.Count > 2)
                {
                    reportPaths.Remove(reportPaths[0]);
                    reportPaths.Remove(reportPaths[0]);
                }

                return ConfigurationSettings.AppSettings["TransferProtocol"] + exportPath;
            }
            else
            {
                throw new ValidationException("Error al generar reportes");
            }
        }

        private List<string> GenerateReportThirdPartyLiability(CompanyFilterReport companyFilterReport, string userFolderPath)
        {
            List<string> reportPaths = new List<string>();
            CompanyPolicy companyPolicy = new CompanyPolicy();
            List<CompanyTplRisk> thirdPartyLiabilities = new List<CompanyTplRisk>();

            if (companyFilterReport.EndorsementId == 0)
            {
                companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyFilterReport.TemporalId, false);
                //thirdPartyLiabilities = DelegateService.thirdPartyLiabilityService.GetThirdPartyLiabilitiesByTemporalId(companyFilterReport.TemporalId);
            }
            else
            {
                companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(companyFilterReport.EndorsementId);
                //thirdPartyLiabilities = DelegateService.thirdPartyLiabilityService.GetCompanyThirdPartyLiabilitiesByEndorsementId(companyFilterReport.EndorsementId);
            }

            foreach (CompanyTplRisk thirdPartyLiability in thirdPartyLiabilities)
            {
                using (ReportDocument reportDocument = new ReportDocument())
                {
                    DataSet dsVehicle = new DataSet("DataSetVehicle");

                    dsVehicle.Tables.Add(SetDataBUSINESS_TYPE(companyPolicy));
                    dsVehicle.Tables.Add(SetDataCO_TMP_POLICY(companyPolicy, companyFilterReport.User.AccountName));
                    dsVehicle.Tables.Add(CO_TMP_POLICY_RISK(companyPolicy, thirdPartyLiability.Risk));
                    dsVehicle.Tables.Add(SetDataCO_TMP_POLICY_RISK_COLLECTIVE(thirdPartyLiability.Risk.Coverages[0]));
                    dsVehicle.Tables.Add(SetDataCO_TMP_POLICY_VEHICLE(thirdPartyLiability));
                    dsVehicle.Tables.Add(SetDataCO_TMP_POLICY_RISK_COVERAGE(thirdPartyLiability.Risk.Coverages, 0));
                    dsVehicle.Tables.Add(SetDataCO_TMP_POLICY_COINSURANCE(companyPolicy));
                    dsVehicle.Tables.Add(SetDataCO_POLICY_AGENT(companyPolicy.Agencies));
                    dsVehicle.Tables.Add(SetDataCO_TMP_POLICY_COVER(null));

                    SetAdditionalDataThirdPartyLiability(companyPolicy, thirdPartyLiability, dsVehicle);

                    string reportTemplate = userFolderPath + Guid.NewGuid() + ".pdf";
                    reportPaths.Add(reportTemplate);

                    reportDocument.Load(GetTemplatePathByTemplateName("VehicleCover"));
                    reportDocument.SetDataSource(dsVehicle);

                    reportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, reportTemplate);

                    DataTable dtClauses = SetDataCLAUSES(companyPolicy.Clauses, thirdPartyLiability.Risk.Clauses);

                    if (dtClauses.Rows.Count > 0)
                    {
                        dsVehicle.Tables.Add(dtClauses);

                        // Se agrega la tabla de accesorios debido a que el reporte VehicleCoverAppendix lo requiere  
                        DataTable dtAccesories = DatasetHelper.CreateACCESORY();
                        dsVehicle.Tables.Add(dtAccesories);
                        dsVehicle.Tables.Add(CreateCO_TMP_POLICY_TEXT(companyPolicy));

                        reportTemplate = userFolderPath + Guid.NewGuid() + ".pdf";
                        reportPaths.Add(reportTemplate);

                        reportDocument.Load(GetTemplatePathByTemplateName("VehicleCoverAppendix"));
                        reportDocument.SetDataSource(dsVehicle);

                        reportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, reportTemplate);
                    }
                }
            }

            reportPaths.Add(GenerateReportConvection(companyPolicy, companyFilterReport.User.AccountName, userFolderPath));

            return reportPaths;
        }

        private List<string> GenerateReportJudicialSurety(CompanyFilterReport companyFilterReport, string userFolderPath)
        {
            List<string> reportPaths = new List<string>();
            CompanyPolicy companyPolicy = new CompanyPolicy();
            List<CompanyJudgement> companyJudgements = new List<CompanyJudgement>();

            if (companyFilterReport.EndorsementId == 0)
            {
                companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyFilterReport.TemporalId, false);
                companyJudgements = DelegateService.juditialSuretyService.GetCompanyJudgementsByTemporalId(companyFilterReport.TemporalId);
            }
            else
            {
                companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(companyFilterReport.EndorsementId);
                companyJudgements = DelegateService.juditialSuretyService.GetCompanyJudgementsByEndorsementId(companyFilterReport.EndorsementId);
            }

            foreach (CompanyJudgement companyJudgement in companyJudgements)
            {
                using (ReportDocument reportDocument = new ReportDocument())
                {
                    DataSet dsCompanyJudgement = new DataSet("DataSetJudicialSurety");

                    dsCompanyJudgement.Tables.Add(SetDataJUDGEMENT(companyPolicy, companyJudgement, companyFilterReport.User.AccountName));
                    SetAdditionalDataJudgement(companyPolicy, companyJudgement, dsCompanyJudgement);

                    string reportTemplate = userFolderPath + Guid.NewGuid() + ".pdf";
                    reportPaths.Add(reportTemplate);

                    reportDocument.Load(GetTemplatePathByTemplateName("JudgementCover"));
                    reportDocument.SetDataSource(dsCompanyJudgement);

                    reportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, reportTemplate);

                    DataTable dtClauses = SetDataCLAUSES(companyPolicy.Clauses, companyJudgement.Risk.Clauses);

                    if (dtClauses.Rows.Count > 0)
                    {
                        dsCompanyJudgement.Tables.Add(dtClauses);
                        reportTemplate = userFolderPath + Guid.NewGuid() + ".pdf";
                        reportPaths.Add(reportTemplate);

                        reportDocument.Load(GetTemplatePathByTemplateName("JudgementCoverAppendix"));
                        reportDocument.SetDataSource(dsCompanyJudgement);

                        reportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, reportTemplate);
                    }
                }
            }

            reportPaths.Add(GenerateReportConvection(companyPolicy, companyFilterReport.User.AccountName, userFolderPath));

            return reportPaths;
        }

        private DataTable SetDataCLAUSES(List<CompanyClause> policyClauses, List<CompanyClause> riskClauses)
        {
            DataTable dtClauses = DatasetHelper.CreateCLAUSES();

            if (policyClauses != null)
            {
                foreach (CompanyClause clause in policyClauses)
                {
                    DataRow dataRow = dtClauses.NewRow();

                    dataRow["TITLE"] = clause.Title;
                    dataRow["TEXT"] = clause.Text;

                    dtClauses.Rows.Add(dataRow);
                }
            }

            if (riskClauses != null)
            {
                foreach (CompanyClause clause in riskClauses)
                {
                    DataRow dataRow = dtClauses.NewRow();

                    dataRow["TITLE"] = clause.Title;
                    dataRow["TEXT"] = clause.Text;

                    dtClauses.Rows.Add(dataRow);
                }
            }

            return dtClauses;
        }

        private string GenerateReportConvection(CompanyPolicy companyPolicy, string userName, string userFolderPath)
        {
            string reportTemplate = userFolderPath + Guid.NewGuid() + ".pdf";

            using (ReportDocument reportDocument = new ReportDocument())
            {
                DataSet dsConvection = new DataSet("DataSetConvection");
                dsConvection.Tables.Add(SetDataCONVECTION(companyPolicy, userName));
                dsConvection.Tables.Add(SetDataQUOTAS(companyPolicy.PaymentPlan.Quotas));

                reportDocument.Load(GetTemplatePathByTemplateName("Convection"));
                reportDocument.SetDataSource(dsConvection);

                reportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, reportTemplate);
            }

            return reportTemplate;
        }

        private DataTable SetDataQUOTAS(List<Quota> quotas)
        {
            DataTable dtQuotas = DatasetHelper.CreateQUOTAS();
            if (quotas != null)
                foreach (Quota quota in quotas)
                {
                    DataRow dataRow = dtQuotas.NewRow();

                    dataRow["PAYMENT_DATE"] = quota.ExpirationDate;
                    dataRow["PAYMENT_AMOUNT"] = quota.Amount;

                    dtQuotas.Rows.Add(dataRow);
                }

            return dtQuotas;
        }

        private DataTable SetDataCONVECTION(CompanyPolicy companyPolicy, string userName)
        {
            DataTable dtConvection = DatasetHelper.CreateCONVECTION();
            DataRow dataRow = dtConvection.NewRow();

            dataRow["BRANCH_CD"] = companyPolicy.Branch.Id;
            dataRow["PREFIX_CD"] = companyPolicy.Prefix.Id;
            dataRow["POLICY_NUMBER"] = companyPolicy.DocumentNumber;
            dataRow["TEMPORAL_ID"] = companyPolicy.Id;
            dataRow["ISSUE_DATE_DAY"] = companyPolicy.IssueDate.Day;
            dataRow["ISSUE_DATE_MONTH"] = companyPolicy.IssueDate.Month;
            dataRow["ISSUE_DATE_YEAR"] = companyPolicy.IssueDate.Year;
            dataRow["BRANCH"] = companyPolicy.Branch.SmallDescription;
            dataRow["CURRENCY"] = companyPolicy.ExchangeRate.Currency.Description;
            dataRow["PREMIUM_AMT"] = companyPolicy.Summary == null ? 0 : companyPolicy.Summary.Premium;
            dataRow["EXPENSES"] = companyPolicy.Summary == null ? 0 : companyPolicy.Summary.Expenses;
            dataRow["TAX"] = companyPolicy.Summary == null ? 0 : companyPolicy.Summary.Taxes;
            dataRow["AGREED_PAYMENT_METHOD"] = companyPolicy.PaymentPlan.Description;
            dataRow["USER_NAME"] = userName;
            dataRow["BUSINESS_TYPE_CD"] = companyPolicy.BusinessType;
            dataRow["TEMP_TYPE_CD"] = companyPolicy.TemporalType;

            dtConvection.Rows.Add(dataRow);

            return dtConvection;
        }

        private void SetAdditionalDataJudgement(CompanyPolicy companyPolicy, CompanyJudgement companyJudgement, DataSet dsJudgement)
        {
            NameValue[] parameters = new NameValue[19];
            parameters[0] = new NameValue("@HOLDER_ID", companyPolicy.Holder.IndividualId);
            parameters[1] = new NameValue("@INSURED_ID", companyJudgement.Risk.MainInsured.IndividualId);
            parameters[2] = new NameValue("@BENEFICIARY_ID", companyJudgement.Risk.Beneficiaries[0].IndividualId);
            parameters[3] = new NameValue("@BRANCH_ID", companyPolicy.Branch.Id);
            parameters[4] = new NameValue("@SALE_POINT_ID", companyPolicy.Branch.SalePoints == null ? 0 : companyPolicy.Branch.SalePoints.Count > 0 ? 0 : companyPolicy.Branch.SalePoints[0].Id);
            parameters[5] = new NameValue("@PREFIX_ID", companyPolicy.Prefix.Id);
            parameters[6] = new NameValue("@POLICY_TYPE_ID", companyPolicy.PolicyType.Id);
            parameters[7] = new NameValue("@CURRENCY_ID", companyPolicy.ExchangeRate.Currency.Id);
            parameters[8] = new NameValue("@COURT_ID", companyJudgement.Court.Id);
            parameters[9] = new NameValue("@ARTICLE_ID", companyJudgement.Article.Id);
            parameters[10] = new NameValue("@COUNTRY_ID", companyJudgement.City.State.Country.Id);
            parameters[11] = new NameValue("@STATE_ID", companyJudgement.City.State.Id);
            parameters[12] = new NameValue("@CITY_ID", companyJudgement.City.Id);
            parameters[13] = new NameValue("@ENDORSEMENT_TYPE_ID", companyPolicy.Endorsement.EndorsementType);



            //TABLA COVERAGES
            DataTable dtCoverages = new DataTable("COVERAGE");
            var limitRcId = 0;
            dtCoverages.Columns.Add("COVERAGE_ID", typeof(int));
            dtCoverages.Columns.Add("COVER_STATUS_CD", typeof(int));
            dtCoverages.Columns.Add("ENDORSEMENT_SUBLIMIT_AMT", typeof(decimal));
            dtCoverages.Columns.Add("DEDUCT_ID", typeof(int));
            dtCoverages.Columns.Add("DEDUCT_VALUE", typeof(decimal));
            dtCoverages.Columns.Add("MIN_DEDUCT_VALUE", typeof(decimal));
            dtCoverages.Columns.Add("DEDUCT_UNIT_CD", typeof(int));
            dtCoverages.Columns.Add("DEDUCT_SUBJECT_CD", typeof(int));
            dtCoverages.Columns.Add("MIN_DEDUCT_UNIT_CD", typeof(int));
            dtCoverages.Columns.Add("CURRENT_FROM", typeof(DateTime));
            dtCoverages.Columns.Add("CURRENT_TO", typeof(DateTime));

            foreach (CompanyCoverage coverage in companyJudgement.Risk.Coverages)
            {
                DataRow dataRow = dtCoverages.NewRow();
                dataRow["COVERAGE_ID"] = coverage.Id;
                dataRow["COVER_STATUS_CD"] = coverage.CoverStatus;
                dataRow["ENDORSEMENT_SUBLIMIT_AMT"] = coverage.EndorsementSublimitAmount;

                if (coverage.Deductible != null && coverage.Deductible.Id > 0)
                {
                    dataRow["DEDUCT_ID"] = coverage.Deductible.Id;
                    dataRow["DEDUCT_VALUE"] = coverage.Deductible.DeductValue;
                    dataRow["MIN_DEDUCT_VALUE"] = coverage.Deductible.MinDeductValue;

                    if (coverage.Deductible.DeductibleUnit != null)
                    {
                        dataRow["DEDUCT_UNIT_CD"] = coverage.Deductible.DeductibleUnit.Id;
                    }

                    if (coverage.Deductible.DeductibleSubject != null)
                    {
                        dataRow["DEDUCT_SUBJECT_CD"] = dataRow["DEDUCT_UNIT_CD"] = coverage.Deductible.DeductibleSubject.Id;
                    }

                    if (coverage.Deductible.MinDeductibleUnit != null)
                    {
                        dataRow["MIN_DEDUCT_UNIT_CD"] = coverage.Deductible.MinDeductibleUnit.Id;
                    }
                }

                dtCoverages.Rows.Add(dataRow);
            }
            parameters[14] = new NameValue("@ENDORSEMENT_TYPE_ID_COVERAGE", companyJudgement.Risk.Coverages[0].EndorsementType);
            parameters[15] = new NameValue("@LIMIT_RC_ID", limitRcId);
            parameters[16] = new NameValue("@COVERAGES", dtCoverages);



            //TABLA AGENCY
            DataTable dtAgencies = new DataTable("AGENCY");
            dtAgencies.Columns.Add("AGENT_ID", typeof(int));
            dtAgencies.Columns.Add("AGENT_CODE", typeof(int));
            dtAgencies.Columns.Add("PARTICIPATION", typeof(decimal));
            dtAgencies.Columns.Add("IS_MAIN", typeof(bool));


            foreach (IssuanceAgency agency in companyPolicy.Agencies)
            {
                DataRow dataRow = dtAgencies.NewRow();

                dataRow["AGENT_ID"] = agency.Agent.IndividualId;
                dataRow["AGENT_CODE"] = agency.Code;
                dataRow["PARTICIPATION"] = agency.Participation;
                dataRow["IS_MAIN"] = agency.IsPrincipal;


                dtAgencies.Rows.Add(dataRow);
            }
            parameters[17] = new NameValue("@AGENCIES", dtAgencies);


            //tabla COINSURANCE
            DataTable dtCoinsurance = new DataTable("COINSURANCE");
            dtCoinsurance.Columns.Add("ID", typeof(int));
            dtCoinsurance.Columns.Add("PARTICIPATION", typeof(decimal));


            if (companyPolicy.BusinessType != BusinessType.CompanyPercentage)
            {
                foreach (IssuanceCoInsuranceCompany coinsuranceCompany in companyPolicy.CoInsuranceCompanies)
                {

                    DataRow dataRow = dtCoinsurance.NewRow();
                    dataRow["ID"] = coinsuranceCompany.Id;
                    dataRow["PARTICIPATION"] = coinsuranceCompany.ParticipationPercentage;
                    dtCoinsurance.Rows.Add(dataRow);
                }
            }
            parameters[18] = new NameValue("@INSURANCE", dtCoinsurance);

            DataSet dataSet;

            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                dataSet = pdb.ExecuteSPDataSet("REPORT.GET_ADDITIONAL_DATA_JUDGETMENT", parameters);
            }



            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                dsJudgement.Tables[0].Rows[0]["HOLDER_DOCUMENT_TYPE"] = dataSet.Tables[0].Rows[0][0];
                dsJudgement.Tables[0].Rows[0]["HOLDER_ADDRESS"] = dataSet.Tables[0].Rows[0][1];
                dsJudgement.Tables[0].Rows[0]["HOLDER_PHONE"] = dataSet.Tables[0].Rows[0][2];
                dsJudgement.Tables[0].Rows[0]["INSURED_DOCUMENT_TYPE"] = dataSet.Tables[0].Rows[0][3];
                dsJudgement.Tables[0].Rows[0]["INSURED_ADDRESS"] = dataSet.Tables[0].Rows[0][4];
                dsJudgement.Tables[0].Rows[0]["INSURED_PHONE"] = dataSet.Tables[0].Rows[0][5];
                dsJudgement.Tables[0].Rows[0]["BENEFICIARY_DOCUMENT_TYPE"] = dataSet.Tables[0].Rows[0][6];
                dsJudgement.Tables[0].Rows[0]["BENEFICIARY_ADDRESS"] = dataSet.Tables[0].Rows[0][7];
                dsJudgement.Tables[0].Rows[0]["BENEFICIARY_PHONE"] = dataSet.Tables[0].Rows[0][8];
                dsJudgement.Tables[0].Rows[0]["BRANCH_DESCRIPTION"] = dataSet.Tables[0].Rows[0][9];
                companyPolicy.Branch.SmallDescription = (string)dataSet.Tables[0].Rows[0][9];
                dsJudgement.Tables[0].Rows[0]["BRANCH_CITY_DESCRIPTION"] = dataSet.Tables[0].Rows[0][10];
                dsJudgement.Tables[0].Rows[0]["SALE_POINT_DESCRIPTION"] = dataSet.Tables[0].Rows[0][11];
                dsJudgement.Tables[0].Rows[0]["PREFIX_DESCRIPTION"] = dataSet.Tables[0].Rows[0][12];
                companyPolicy.Prefix.SmallDescription = (string)dataSet.Tables[0].Rows[0][12];
                dsJudgement.Tables[0].Rows[0]["POLICY_TYPE_DESCRIPTION"] = dataSet.Tables[0].Rows[0][13];
                dsJudgement.Tables[0].Rows[0]["CURRENCY_DESCRIPTION"] = dataSet.Tables[0].Rows[0][14];
                companyPolicy.ExchangeRate.Currency.SmallDescription = (string)dataSet.Tables[0].Rows[0][14];
                dsJudgement.Tables[0].Rows[0]["CURRENCY_SYMBOL"] = dataSet.Tables[0].Rows[0][15];
                companyPolicy.ExchangeRate.Currency.TinyDescription = (string)dataSet.Tables[0].Rows[0][15];
                dsJudgement.Tables[0].Rows[0]["COURT_DESCRIPTION"] = dataSet.Tables[0].Rows[0][16];
                dsJudgement.Tables[0].Rows[0]["ARTICLE_DESCRIPTION"] = dataSet.Tables[0].Rows[0][17];
                dsJudgement.Tables[0].Rows[0]["ARTICLE_TEXT"] = dataSet.Tables[0].Rows[0][18];
                dsJudgement.Tables[0].Rows[0]["CITY_DESCRIPTION"] = dataSet.Tables[0].Rows[0][19];
                dsJudgement.Tables[0].Rows[0]["ENDORSEMENT_TYPE_DESCRIPTION"] = dataSet.Tables[0].Rows[0][20];

                //Agregarmos las otras tablas
                dataSet.Tables[1].TableName = "COVERAGE";
                dsJudgement.Tables.Add(dataSet.Tables[1].Copy());

                dataSet.Tables[2].TableName = "AGENCY";
                dsJudgement.Tables.Add(dataSet.Tables[2].Copy());

                dataSet.Tables[3].TableName = "COINSURANCE";
                dsJudgement.Tables.Add(dataSet.Tables[3].Copy());
            }
        }

        private DataTable SetDataCOINSURANCE(CompanyPolicy companyPolicy)
        {
            DataTable dtCoinsurance = new DataTable("COINSURANCE");
            dtCoinsurance.Columns.Add("ID", typeof(int));
            dtCoinsurance.Columns.Add("PARTICIPATION", typeof(decimal));


            if (companyPolicy.BusinessType != BusinessType.CompanyPercentage)
            {
                foreach (IssuanceCoInsuranceCompany coinsuranceCompany in companyPolicy.CoInsuranceCompanies)
                {

                    DataRow dataRow = dtCoinsurance.NewRow();
                    dataRow["ID"] = coinsuranceCompany.Id;
                    dataRow["PARTICIPATION"] = coinsuranceCompany.ParticipationPercentage;

                    dtCoinsurance.Rows.Add(dataRow);
                }

                DataTable dataTable;
                NameValue[] parameters = new NameValue[1];
                parameters[0] = new NameValue("@INSURANCE", dtCoinsurance);
                using (DynamicDataAccess pdb = new DynamicDataAccess())
                {
                    dataTable = pdb.ExecuteSPDataTable("REPORT.GET_PRINT_INSURANCE_COMPANY", parameters);
                }

                dataTable.TableName = "COINSURANCE";
                return dataTable;
            }
            else
            {
                return dtCoinsurance;
            }



        }

        private DataTable SetDataAGENCY(List<Agency> agencies)
        {
            NameValue[] parameters = new NameValue[1];

            DataTable dtAgencies = new DataTable("AGENCY");
            dtAgencies.Columns.Add("AGENT_ID", typeof(int));
            dtAgencies.Columns.Add("AGENT_CODE", typeof(int));
            dtAgencies.Columns.Add("PARTICIPATION", typeof(decimal));
            dtAgencies.Columns.Add("IS_MAIN", typeof(bool));

            foreach (Agency agency in agencies)
            {
                DataRow dataRow = dtAgencies.NewRow();

                dataRow["AGENT_ID"] = agency.Agent.IndividualId;
                dataRow["AGENT_CODE"] = agency.Code;
                dataRow["PARTICIPATION"] = agency.Participation;
                dataRow["IS_MAIN"] = agency.IsPrincipal;

                dtAgencies.Rows.Add(dataRow);
            }

            parameters[0] = new NameValue("@AGENCIES", dtAgencies);

            DataTable dataTable;

            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                dataTable = pdb.ExecuteSPDataTable("REPORT.GET_PRINT_AGENCY", parameters);
            }

            dataTable.TableName = "AGENCY";

            return dataTable;
        }

        private DataTable SetDataCOVERAGE(List<CompanyCoverage> coverages, int limitRcId)
        {
            NameValue[] parameters = new NameValue[3];
            parameters[0] = new NameValue("@ENDORSEMENT_TYPE_ID", coverages[0].EndorsementType);
            parameters[1] = new NameValue("@LIMIT_RC_ID", limitRcId);

            DataTable dtCoverages = new DataTable("COVERAGES");
            dtCoverages.Columns.Add("COVERAGE_ID", typeof(int));
            dtCoverages.Columns.Add("COVER_STATUS_CD", typeof(int));
            dtCoverages.Columns.Add("ENDORSEMENT_SUBLIMIT_AMT", typeof(decimal));
            dtCoverages.Columns.Add("DEDUCT_ID", typeof(int));
            dtCoverages.Columns.Add("DEDUCT_VALUE", typeof(decimal));
            dtCoverages.Columns.Add("MIN_DEDUCT_VALUE", typeof(decimal));
            dtCoverages.Columns.Add("DEDUCT_UNIT_CD", typeof(int));
            dtCoverages.Columns.Add("DEDUCT_SUBJECT_CD", typeof(int));
            dtCoverages.Columns.Add("MIN_DEDUCT_UNIT_CD", typeof(int));

            foreach (CompanyCoverage coverage in coverages)
            {
                DataRow dataRow = dtCoverages.NewRow();
                dataRow["COVERAGE_ID"] = coverage.Id;
                dataRow["COVER_STATUS_CD"] = coverage.CoverStatus;
                dataRow["ENDORSEMENT_SUBLIMIT_AMT"] = coverage.EndorsementSublimitAmount;

                if (coverage.Deductible != null && coverage.Deductible.Id > 0)
                {
                    dataRow["DEDUCT_ID"] = coverage.Deductible.Id;
                    dataRow["DEDUCT_VALUE"] = coverage.Deductible.DeductValue;
                    dataRow["MIN_DEDUCT_VALUE"] = coverage.Deductible.MinDeductValue;

                    if (coverage.Deductible.DeductibleUnit != null)
                    {
                        dataRow["DEDUCT_UNIT_CD"] = coverage.Deductible.DeductibleUnit.Id;
                    }

                    if (coverage.Deductible.DeductibleSubject != null)
                    {
                        dataRow["DEDUCT_SUBJECT_CD"] = dataRow["DEDUCT_UNIT_CD"] = coverage.Deductible.DeductibleSubject.Id;
                    }

                    if (coverage.Deductible.MinDeductibleUnit != null)
                    {
                        dataRow["MIN_DEDUCT_UNIT_CD"] = coverage.Deductible.MinDeductibleUnit.Id;
                    }
                }

                dtCoverages.Rows.Add(dataRow);
            }

            parameters[2] = new NameValue("@COVERAGES", dtCoverages);

            DataTable dataTable;

            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                dataTable = pdb.ExecuteSPDataTable("REPORT.GET_PRINT_COVERAGES", parameters);
            }

            dtCoverages = DatasetHelper.CreateCOVERAGE();

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRowResult in dataTable.Rows)
                {
                    DataRow dataRow = dtCoverages.NewRow();

                    dataRow["NUMBER"] = dataRowResult[0];
                    dataRow["DESCRIPTION"] = dataRowResult[1];
                    dataRow["PREMIUM"] = dataRowResult[2];
                    dataRow["DEDUCTIBLE"] = dataRowResult[3];
                    dataRow["DEDUCTIBLE_VALUE"] = dataRowResult[4];
                    dataRow["DEDUCTIBLE_VALUE_MINIMUM"] = dataRowResult[5];

                    dtCoverages.Rows.Add(dataRow);
                }
            }

            return dtCoverages;
        }

        private DataTable SetDataJUDGEMENT(CompanyPolicy companyPolicy, CompanyJudgement companyJudgement, string userName)
        {
            DataTable dtJudgement = DatasetHelper.CreateJUDICIAL_SURETY();
            DataRow dataRow = dtJudgement.NewRow();

            dataRow["BRANCH_ID"] = companyPolicy.Branch.Id;
            dataRow["PREFIX_ID"] = companyPolicy.Prefix.Id;
            dataRow["PRODUCT_DESCRIPTION"] = companyPolicy.Product.SmallDescription;
            dataRow["EXCHANGE_RATE"] = companyPolicy.ExchangeRate.SellAmount;
            dataRow["TEMPORAL_TYPE_ID"] = companyPolicy.TemporalType;
            if (companyPolicy.Endorsement.QuotationId > 0)
            {
                dataRow["TEMPORAL_ID"] = companyPolicy.Endorsement.QuotationId;
            }
            else
            {
                dataRow["TEMPORAL_ID"] = companyPolicy.Id;
            }
            dataRow["POLICY_NUMBER"] = companyPolicy.DocumentNumber;
            dataRow["ENDORSEMENT_NUMBER"] = companyPolicy.Endorsement.Number;
            dataRow["ISSUE_DATE_DAY"] = companyPolicy.IssueDate.Day;
            dataRow["ISSUE_DATE_MONTH"] = companyPolicy.IssueDate.Month;
            dataRow["ISSUE_DATE_YEAR"] = companyPolicy.IssueDate.Year;
            dataRow["CURRENT_FROM_DAY"] = companyPolicy.CurrentFrom.Day;
            dataRow["CURRENT_FROM_MONTH"] = companyPolicy.CurrentFrom.Month;
            dataRow["CURRENT_FROM_YEAR"] = companyPolicy.CurrentFrom.Year;
            dataRow["CURRENT_FROM_HOUR"] = companyPolicy.CurrentFrom.Hour;
            dataRow["CURRENT_TO_DAY"] = companyPolicy.CurrentTo.Day;
            dataRow["CURRENT_TO_MONTH"] = companyPolicy.CurrentTo.Month;
            dataRow["CURRENT_TO_YEAR"] = companyPolicy.CurrentTo.Year;
            dataRow["CURRENT_TO_HOUR"] = companyPolicy.CurrentTo.Hour;
            dataRow["PAYER_NAME"] = companyPolicy.Holder.Name;
            dataRow["PAYMENT_PLAN_DESCRIPTION"] = companyPolicy.PaymentPlan.Description;
            dataRow["FIRST_PAYMENT_DAY"] = companyPolicy.PaymentPlan.Quotas[0].ExpirationDate.Day;
            dataRow["FIRST_PAYMENT_MONTH"] = companyPolicy.PaymentPlan.Quotas[0].ExpirationDate.Month;
            dataRow["FIRST_PAYMENT_YEAR"] = companyPolicy.PaymentPlan.Quotas[0].ExpirationDate.Year;

            if (companyPolicy.BillingGroup != null)
            {
                dataRow["BILLING_GROUP_DESCRIPTION"] = companyPolicy.BillingGroup.Description;
            }

            dataRow["USER_NAME"] = userName;
            dataRow["LIMIT_AMOUNT"] = companyPolicy.Summary == null ? 0 : companyPolicy.Summary.AmountInsured;
            dataRow["PREMIUM"] = companyPolicy.Summary == null ? 0 : companyPolicy.Summary.Premium;
            dataRow["EXPENSES"] = companyPolicy.Summary == null ? 0 : companyPolicy.Summary.Expenses;
            dataRow["TAXES"] = companyPolicy.Summary.Taxes;
            dataRow["HOLDER_NAME"] = companyPolicy.Holder.Name;
            dataRow["HOLDER_DOCUMENT_NUMBER"] = companyPolicy.Holder.IdentificationDocument.Number;
            dataRow["INSURED_NAME"] = companyJudgement.Risk.MainInsured.Name;
            dataRow["INSURED_DOCUMENT_NUMBER"] = companyJudgement.Risk.MainInsured.IdentificationDocument.Number;
            dataRow["BENEFICIARY_NAME"] = companyJudgement.Risk.Beneficiaries[0].Name;
            dataRow["BENEFICIARY_DOCUMENT_NUMBER"] = companyJudgement.Risk.Beneficiaries[0].IdentificationDocument.Number;

            if (companyPolicy.Text != null && !string.IsNullOrEmpty(companyPolicy.Text.TextBody))
            {
                dataRow["TEXTS"] = companyPolicy.Text.TextBody;
            }

            if (companyJudgement.Risk.Text != null && !string.IsNullOrEmpty(companyJudgement.Risk.Text.TextBody))
            {
                dataRow["TEXTS"] = " " + companyPolicy.Text.TextBody;
            }

            dataRow["COURT_NUMBER"] = companyJudgement.Court.Id;
            dataRow["PROCESS_NUMBER"] = companyJudgement.SettledNumber;

            dtJudgement.Rows.Add(dataRow);

            return dtJudgement;
        }

        /// <summary>
        /// Generar reporte de póliza de Masivos
        /// </summary>
        /// <param name="filterReport">Filtro</param>
        /// <returns>Ruta Reporte</returns>
        public string GenerateReportMassive(List<CompanyFilterReport> companyFilterReports, int massiveLoadId)
        {

            ReportDocument document = new ReportDocument();
            List<string> reportPathsMarge = new List<string>();

            TP.Parallel.ForEach(companyFilterReports, (FilterReport) =>
            {
                reportPathsMarge.Add(GenerateReport(FilterReport));

            });

            string[] NombreArchivos = new string[reportPathsMarge.Count];
            int j = 0;
            foreach (var item in reportPathsMarge)
            {
                NombreArchivos[j++] = item.Substring(5);
            }

            using (ZipFile zip = new ZipFile())
            {
                zip.AddFiles(NombreArchivos, false, "");
                zip.Save(reportExportPath + massiveLoadId + ".zip");
            }
            return ConfigurationSettings.AppSettings["TransferProtocol"] + reportExportPath + massiveLoadId + ".zip";
        }
        private string ReportMassive(CompanyFilterReport companyFilterReport)
        {
            PrintingLog printingLog = new PrintingLog();
            try
            {
                CompanyPolicy companyPolicy = new CompanyPolicy();
                string exportPath = string.Empty;
                string reportPath = string.Empty;
                string path = string.Empty;

                if (companyFilterReport.EndorsementId == 0)
                {
                    PendingOperation pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(companyFilterReport.Risks[0].Policy.Id);
                    companyPolicy = JsonConvert.DeserializeObject<CompanyPolicy>(pendingOperation.Operation);
                    companyPolicy.Id = pendingOperation.Id;
                }
                else
                {
                    companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(companyFilterReport.EndorsementId);
                    companyPolicy.Product = DelegateService.productService.GetCompanyProductByProductIdPrefixId(companyPolicy.Product.Id, companyPolicy.Prefix.Id);

                }

                companyPolicy.DocumentNumber = companyFilterReport.Risks[0].Policy.DocumentNumber;

                string userFolderPath = reportCreatePath + companyFilterReport.User.AccountName + @"\";

                if (!Directory.Exists(userFolderPath))
                    Directory.CreateDirectory(userFolderPath);

                List<string> reportPaths = new List<string>();
                switch (companyPolicy.Product.CoveredRisk.SubCoveredRiskType)
                {
                    case SubCoveredRiskType.Vehicle:
                        reportPaths = GenerateReportVehicle(companyFilterReport, userFolderPath);
                        break;
                    case SubCoveredRiskType.ThirdPartyLiability:
                        reportPaths = GenerateReportThirdPartyLiability(companyFilterReport, userFolderPath);
                        break;
                    case SubCoveredRiskType.Property:
                        reportPaths = GenerateReportProperty(companyFilterReport, userFolderPath);
                        break;
                    case SubCoveredRiskType.Liability:
                        reportPaths = GenerateReportLiability(companyFilterReport, userFolderPath);
                        break;
                    case SubCoveredRiskType.Surety:
                        reportPaths = GenerateReportSurety(companyFilterReport, userFolderPath);
                        break;
                    case SubCoveredRiskType.JudicialSurety:
                        reportPaths = GenerateReportJudicialSurety(companyFilterReport, userFolderPath);
                        break;
                };

                if (reportPaths.Count > 0)
                {
                    reportPath = CreateReportPath(userFolderPath, companyPolicy, "_" + DateTime.Now.ToString("dd-MM-yyyy"), companyFilterReport.Risks[0].Policy.Id);
                    exportPath = CreateReportPath(reportExportPath, companyPolicy, "_" + DateTime.Now.ToString("dd-MM-yyyy"), companyFilterReport.Risks[0].Policy.Id);
                    path = Merge(reportPaths, reportPath, exportPath, companyFilterReport.EndorsementId);
                }

                return path;
            }
            catch (Exception ex)
            {
                printingLog.Description = ex.ToString();
                DelegateService.massiveServiceCore.CreatePrintLog(printingLog);
                throw;
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }

        /// <summary>
        /// Generar reporte de póliza de Colectivas
        /// </summary>
        /// <param name="filterReport">Filtro</param>
        /// <returns>Ruta Reporte</returns>
        public string GenerateReportCollective(List<CompanyFilterReport> companyFilterReports, int massiveLoadId)
        {
            ReportDocument document = new ReportDocument();
            CompanyPolicy companyPolicy = new CompanyPolicy();
            List<string> reportPathsMarge = new List<string>();

            if (companyFilterReports[0].EndorsementId == 0)
            {
                PendingOperation pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(companyFilterReports[0].TemporalId);
                companyPolicy = JsonConvert.DeserializeObject<CompanyPolicy>(pendingOperation.Operation);
                companyPolicy.Id = pendingOperation.Id;
            }
            else
            {
                companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(companyFilterReports[0].EndorsementId);
                companyPolicy.Product = DelegateService.productService.GetCompanyProductByProductIdPrefixId(companyPolicy.Product.Id, companyPolicy.Prefix.Id);
            }
            
            TP.Parallel.ForEach(companyFilterReports, (FilterReport) =>
            {
                reportPathsMarge.Add(ReportCollective(FilterReport, companyPolicy));
            });

            string[] NombreArchivos = new string[reportPathsMarge.Count];
            int j = 0;
            foreach (var item in reportPathsMarge)
            {
                NombreArchivos[j++] = item;
            }

            using (ZipFile zip = new ZipFile())
            {
                zip.AddFiles(NombreArchivos, false, "");
                zip.Save(reportExportPath + massiveLoadId + ".zip");
            }
            DeletePdf(NombreArchivos);
            return ConfigurationSettings.AppSettings["TransferProtocol"] + reportExportPath + massiveLoadId + ".zip";
        }

        private string ReportCollective(CompanyFilterReport companyFilterReport, CompanyPolicy companyPolicy)
        {
            PrintingLog printingLog = new PrintingLog();
            try
            {
                string exportPath = string.Empty;
                string reportPath = string.Empty;
                string path = string.Empty;
                string userFolderPath = reportCreatePath + companyFilterReport.User.AccountName + @"\";

                if (!Directory.Exists(userFolderPath))
                {
                    Directory.CreateDirectory(userFolderPath);
                }

                companyPolicy.DocumentNumber = companyFilterReport.Risks[0].Policy.DocumentNumber;

                List<string> reportPaths = new List<string>();

                switch (companyPolicy.Product.CoveredRisk.SubCoveredRiskType)
                {
                    case SubCoveredRiskType.Vehicle:
                        reportPaths = GenerateReportVehicle(companyFilterReport, userFolderPath);
                        break;
                    case SubCoveredRiskType.ThirdPartyLiability:
                        reportPaths = GenerateReportThirdPartyLiability(companyFilterReport, userFolderPath);
                        break;
                    case SubCoveredRiskType.Property:
                        reportPaths = GenerateReportProperty(companyFilterReport, userFolderPath);
                        break;
                    case SubCoveredRiskType.Liability:
                        reportPaths = GenerateReportLiability(companyFilterReport, userFolderPath);
                        break;
                    case SubCoveredRiskType.Surety:
                        reportPaths = GenerateReportSurety(companyFilterReport, userFolderPath);
                        break;
                    case SubCoveredRiskType.JudicialSurety:
                        reportPaths = GenerateReportJudicialSurety(companyFilterReport, userFolderPath);
                        break;
                };

                if (reportPaths.Count > 0)
                {
                    reportPath = CreateReportPath(userFolderPath, companyPolicy, "_" + DateTime.Now.ToString("dd-MM-yyyy"), companyFilterReport.Risks[0].RiskId);
                    exportPath = CreateReportPath(reportExportPath, companyPolicy, "_" + DateTime.Now.ToString("dd-MM-yyyy"), companyFilterReport.Risks[0].RiskId);

                    path = Merge(reportPaths, reportPath, exportPath, companyFilterReport.EndorsementId);
                }
                return path;
            }
            catch (Exception ex)
            {
                printingLog.Description = ex.ToString();
                DelegateService.massiveServiceCore.CreatePrintLog(printingLog);
                throw new ValidationException(ex.Message, ex);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }

        private CompanyPolicy SetAdditionalDataCompanyPolicy(CompanyPolicy companyPolicy)
        {
            NameValue[] parameters = new NameValue[12];
            parameters[0] = new NameValue("BRANCH_CD", companyPolicy.Branch.Id);
            parameters[1] = new NameValue("CURRENCY_CD", companyPolicy.ExchangeRate.Currency.Id);
            parameters[2] = new NameValue("INDIVIDUAL_ID", companyPolicy.Holder.IndividualId);
            parameters[3] = new NameValue("AGENT_TYPE_CD", companyPolicy.Agencies[0].Id);
            parameters[4] = new NameValue("ENDO_TYPE_CD", companyPolicy.Endorsement.EndorsementType);
            parameters[5] = new NameValue("PREFIX_CD", companyPolicy.Prefix.Id);
            parameters[6] = new NameValue("POLICY_TYPE_CD", companyPolicy.PolicyType.Id);
            parameters[7] = new NameValue("ISSUEDATE", companyPolicy.IssueDate);
            parameters[8] = new NameValue("PRODUCT_ID", companyPolicy.Product.Id);
            parameters[9] = new NameValue("BUSINESS_TYPE_CD", companyPolicy.BusinessType.Value);
            parameters[10] = new NameValue("INDIVIDUAL_ID_AGEN", companyPolicy.Agencies[0].Agent.IndividualId);

            if (companyPolicy.Agencies[0].AgentType != null)
            {
                parameters[11] = new NameValue("AGENT_TYPE_ID", companyPolicy.Agencies[0].AgentType.Id);
            }
            else
            {
                parameters[11] = new NameValue("AGENT_TYPE_ID", DBNull.Value);
            }

            DataTable dataTable;

            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                dataTable = dynamicDataAccess.ExecuteSPDataTable("REPORT.POLICY_DETAIL", parameters);
            }

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                companyPolicy.Branch.Description = (string)dataTable.Rows[0][0];
                companyPolicy.ExchangeRate.Currency.Description = (string)dataTable.Rows[0][1];
                companyPolicy.ExchangeRate.Currency.SmallDescription = (string)dataTable.Rows[0][1];

                if (companyPolicy.Holder.IdentificationDocument.DocumentType == null)
                {
                    companyPolicy.Holder.IdentificationDocument.DocumentType = new IssuanceDocumentType();
                }

                companyPolicy.Holder.IdentificationDocument.DocumentType.SmallDescription = (string)dataTable.Rows[0][2];

                if (dataTable.Rows[0][3].ToString() != "")

                    companyPolicy.Agencies[0].Agent.AgentType = new IssuanceAgentType
                    {
                        Description = (string)dataTable.Rows[0][3]
                    };

                companyPolicy.Endorsement.EndorsementTypeDescription = (string)dataTable.Rows[0][4];
                companyPolicy.Prefix.SmallDescription = (string)dataTable.Rows[0][5];
                companyPolicy.ExchangeRate.Currency.TinyDescription = (string)dataTable.Rows[0][6];
                companyPolicy.PolicyType.Description = (string)dataTable.Rows[0][7];

                if (dataTable.Rows[0][8].ToString() != "")
                {
                    LegalRepresentative = (string)dataTable.Rows[0][8];
                }

                if (dataTable.Rows[0][10].ToString() != "")
                {
                    FromNumber = (string)dataTable.Rows[0][10];
                }

                if (dataTable.Rows[0][11].ToString() != "")
                {
                    companyPolicy.BusinessTypeDescription = (string)dataTable.Rows[0][11];
                }

                if (dataTable.Rows[0][12].ToString() != "")
                    companyPolicy.Agencies[0].Agent.FullName = (string)dataTable.Rows[0][12];

                if (companyPolicy.Agencies[0].Agent.AgentType != null && dataTable.Rows[0][13].ToString() != "")
                {
                    companyPolicy.Agencies[0].Agent.AgentType.SmallDescription = (string)dataTable.Rows[0][13];
                }
            }

            return companyPolicy;
        }

        private CompanyRisk SetAdditionalDataCompanyProperty(CompanyRisk companyPropertyRisk, DataSet dsLocation, CompanyPolicy companyPolicy)
        {
            NameValue[] parameters = new NameValue[8];

            parameters[0] = new NameValue("INDIVIDUAL_ID_INSURED", companyPropertyRisk.MainInsured.IndividualId);
            parameters[1] = new NameValue("INDIVIDUAL_ID_BENEFICIARIE", companyPropertyRisk.Beneficiaries[0].IndividualId);
            parameters[2] = new NameValue("INSURED_OBJECT_ID", companyPropertyRisk.Coverages[0].InsuredObject.Id);
            parameters[3] = new NameValue("BRANCH_ID", companyPolicy.Branch.Id);
            parameters[4] = new NameValue("CURRENCY_ID", companyPolicy.ExchangeRate.Currency.Id);
            parameters[5] = new NameValue("POLICY_TYPE_ID", companyPolicy.PolicyType.Id);
            parameters[6] = new NameValue("PREFIX_ID", companyPolicy.Prefix.Id);
            parameters[7] = new NameValue("ENDORSEMENT_TYPE_ID", companyPolicy.Endorsement.EndorsementType);

            DataTable dataTable;

            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                dataTable = dynamicDataAccess.ExecuteSPDataTable("REPORT.POLICY_LOCATION_DETAIL", parameters);
            }

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                dsLocation.Tables[3].Rows[0]["BENEFICIARY_ADD"] = dataTable.Rows[0][4];
                dsLocation.Tables[0].Rows[0]["POLICY_TYPE"] = dataTable.Rows[0][9];
                dsLocation.Tables[3].Rows[0]["INSURED_ADD"] = dataTable.Rows[0][3];
                dsLocation.Tables[0].Rows[0]["POLICY_HOLDER_ADD"] = dataTable.Rows[0][3];
                dsLocation.Tables[3].Rows[0]["BENEFICIARY_PHONE"] = dataTable.Rows[0][2];
                dsLocation.Tables[3].Rows[0]["INSURED_PHONE"] = dataTable.Rows[0][1];
                dsLocation.Tables[0].Rows[0]["POLICY_HOLDER_PHONE"] = dataTable.Rows[0][1];
                dsLocation.Tables[3].Rows[0]["BENEFICIARY_DOC_TYPE"] = dataTable.Rows[0][5];
                dsLocation.Tables[3].Rows[0]["INSURED_DOC_TYPE"] = dataTable.Rows[0][0];
                dsLocation.Tables[0].Rows[0]["POLICY_HOLDER_DOC_TYPE"] = dataTable.Rows[0][0];
                dsLocation.Tables[0].Rows[0]["BRANCH"] = dataTable.Rows[0][7];
                companyPolicy.Branch.SmallDescription = (string)dataTable.Rows[0][7];
                dsLocation.Tables[0].Rows[0]["CURRENCY_SYMBOL"] = dataTable.Rows[0][8];
                companyPolicy.ExchangeRate.Currency.TinyDescription = (string)dataTable.Rows[0][8];
                dsLocation.Tables[0].Rows[0]["PREFIX"] = dataTable.Rows[0][10];
                dsLocation.Tables[0].Rows[0]["ENDORSEMENT_TYPE_DESC"] = dataTable.Rows[0][11];
                dsLocation.Tables[0].Rows[0]["CURRENCY"] = dataTable.Rows[0][12];
            }

            return companyPropertyRisk;
        }

        private CompanyPolicy SetAdditionalDataCompanySurety(CompanyPolicy companyPolicy, DataSet dsSurety, CompanyRisk companySuretyRisk, CompanyContractor contractor)
        {
            NameValue[] parameters = new NameValue[11];
            parameters[0] = new NameValue("BRANCH_CD", companyPolicy.Branch.Id);
            parameters[1] = new NameValue("CURRENCY_CD", companyPolicy.ExchangeRate.Currency.Id);
            parameters[2] = new NameValue("INDIVIDUAL_ID", companyPolicy.Holder.IndividualId);
            parameters[3] = new NameValue("INDIVIDUAL_ID_INSURED", companySuretyRisk.MainInsured.IndividualId);
            parameters[4] = new NameValue("AGENT_TYPE_CD", companyPolicy.Agencies[0].Id);
            parameters[5] = new NameValue("ENDO_TYPE_CD", companyPolicy.Endorsement.EndorsementType);
            parameters[6] = new NameValue("PREFIX_CD", companyPolicy.Prefix.Id);
            parameters[7] = new NameValue("PRODUCT_TYPE_CD", companyPolicy.Product.Id);
            parameters[8] = new NameValue("INDIVIDUAL_ID_ENTRENCHED", contractor.IndividualId);
            parameters[9] = new NameValue("INDIVIDUAL_ID_BENEFICIARIE", companySuretyRisk.Beneficiaries[0].IndividualId);
            parameters[10] = new NameValue("ENDORSEMENT_TYPE_ID", companyPolicy.Endorsement.EndorsementType);


            DataTable dataTable;

            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                dataTable = dynamicDataAccess.ExecuteSPDataTable("REPORT.POLICY_SURETY_DETAIL", parameters);

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                dsSurety.Tables[0].Rows[0]["POLICY_HOLDER_PHONE"] = dataTable.Rows[0][10];
                dsSurety.Tables[0].Rows[0]["POLICY_TYPE"] = dataTable.Rows[0][6];
                dsSurety.Tables[0].Rows[0]["CURRENCY"] = dataTable.Rows[0][1];
                dsSurety.Tables[0].Rows[0]["POLICY_HOLDER_ADD"] = dataTable.Rows[0][13];
                dsSurety.Tables[1].Rows[0]["ENTRENCHED_PHONE"] = dataTable.Rows[0][9];
                dsSurety.Tables[1].Rows[0]["ENTRENCHED_DOC_TYPE"] = dataTable.Rows[0][18];
                dsSurety.Tables[1].Rows[0]["ENTRENCHED_DOC"] = dataTable.Rows[0][19];
                dsSurety.Tables[1].Rows[0]["INSURED_NAME"] = companySuretyRisk.MainInsured.Name;
                dsSurety.Tables[1].Rows[0]["ENTRENCHED_ADD"] = dataTable.Rows[0][12];
                dsSurety.Tables[1].Rows[0]["ENTRENCHED_NAME"] = dataTable.Rows[0][20];
                dsSurety.Tables[1].Rows[0]["INSURED_ADD"] = dataTable.Rows[0][11];
                dsSurety.Tables[1].Rows[0]["INSURED_DOC_TYPE"] = dataTable.Rows[0][14];
                dsSurety.Tables[1].Rows[0]["INSURED_PHONE"] = dataTable.Rows[0][8];
                dsSurety.Tables[0].Rows[0]["BRANCH"] = dataTable.Rows[0][0];
                dsSurety.Tables[0].Rows[0]["PREFIX"] = dataTable.Rows[0][6];
                dsSurety.Tables[0].Rows[0]["POLICY_HOLDER_DOC_TYPE"] = dataTable.Rows[0][2];
                dsSurety.Tables[0].Rows[0]["ENDORSEMENT_TYPE_DESC"] = dataTable.Rows[0][15];
                companyPolicy.Branch.SmallDescription = (string)dataTable.Rows[0][0];
            }

            return companyPolicy;
        }

        private List<string> GenerateReportVehicle(CompanyFilterReport companyFilterReport, string userFolderPath)
        {
            List<string> reportPaths = new List<string>();
            CompanyPolicy companyPolicy = new CompanyPolicy();
            List<CompanyVehicle> companyVehicles = new List<CompanyVehicle>();

            if (companyFilterReport.EndorsementId == 0)
            {
                companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyFilterReport.TemporalId, false);
                //companyVehicles = DelegateService.vehicleService.GetCompanyVehiclesByTemporalId(companyFilterReport.TemporalId);
            }
            else
            {
                companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(companyFilterReport.EndorsementId);
                //companyVehicles = DelegateService.vehicleService.GetCompanyVehiclesByEndorsementId(companyFilterReport.EndorsementId);
            }

            foreach (CompanyVehicle companyVehicle in companyVehicles)
            {
                using (ReportDocument reportDocument = new ReportDocument())
                {
                    DataSet dsVehicle = new DataSet("DataSetVehicle");

                    dsVehicle.Tables.Add(SetDataBUSINESS_TYPE(companyPolicy));
                    dsVehicle.Tables.Add(SetDataCO_POLICY_REPORT_DETAIL_STRING(companyVehicle.Accesories));
                    dsVehicle.Tables.Add(SetDataCO_TMP_POLICY(companyPolicy, companyFilterReport.User.AccountName));
                    dsVehicle.Tables.Add(SetDataCO_TMP_POLICY_COVER(companyVehicle.Accesories));
                    dsVehicle.Tables.Add(CO_TMP_POLICY_RISK(companyPolicy, companyVehicle.Risk));
                    dsVehicle.Tables.Add(SetDataCO_TMP_POLICY_RISK_COLLECTIVE(companyVehicle.Risk.Coverages[0]));
                    dsVehicle.Tables.Add(SetDataCO_TMP_POLICY_VEHICLE(companyVehicle));
                    dsVehicle.Tables.Add(SetDataCO_TMP_POLICY_RISK_COVERAGE(companyVehicle.Risk.Coverages, companyVehicle.Risk.LimitRc.Id));
                    dsVehicle.Tables.Add(SetDataCO_TMP_POLICY_COINSURANCE(companyPolicy));
                    dsVehicle.Tables.Add(SetDataCO_POLICY_AGENT(companyPolicy.Agencies));

                    SetAdditionalDataVehicle(companyPolicy, companyVehicle, dsVehicle);

                    string reportTemplate = userFolderPath + Guid.NewGuid() + ".pdf";
                    reportPaths.Add(reportTemplate);

                    reportDocument.Load(GetTemplatePathByTemplateName("VehicleCover"));
                    reportDocument.SetDataSource(dsVehicle);

                    reportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, reportTemplate);

                    DataTable dtClauses = SetDataCLAUSES(companyPolicy.Clauses, companyVehicle.Risk.Clauses);
                    DataTable dtAccesories = SetDataACCESORIES(companyVehicle.Accesories);

                    if (dtClauses.Rows.Count > 0 || dtAccesories.Rows.Count > 0)
                    {
                        dsVehicle.Tables.Add(dtClauses);
                        dsVehicle.Tables.Add(dtAccesories);
                        dsVehicle.Tables.Add(CreateCO_TMP_POLICY_TEXT(companyPolicy));

                        reportTemplate = userFolderPath + Guid.NewGuid() + ".pdf";
                        reportPaths.Add(reportTemplate);

                        reportDocument.Load(GetTemplatePathByTemplateName("VehicleCoverAppendix"));
                        reportDocument.SetDataSource(dsVehicle);

                        reportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, reportTemplate);
                    }
                }
            }

            reportPaths.Add(GenerateReportConvection(companyPolicy, companyFilterReport.User.AccountName, userFolderPath));

            return reportPaths;
        }

        private DataTable SetDataACCESORIES(List<CompanyAccessory> accesories)
        {
            DataTable dtAccesories = DatasetHelper.CreateACCESORY();

            if (accesories != null && accesories.Count > 0)
            {
                foreach (CompanyAccessory accessory in accesories.Where(x => !x.IsOriginal))
                {
                    DataRow dataRow = dtAccesories.NewRow();

                    dataRow["TYPE_DESCRIPTION"] = accessory.Make;
                    dataRow["MAKE_DESCRIPTION"] = accessory.Description;
                    dataRow["INSURED_AMOUNT"] = accessory.Amount;

                    dtAccesories.Rows.Add(dataRow);
                }
            }

            return dtAccesories;
        }

        private void SetAdditionalDataVehicle(CompanyPolicy companyPolicy, CompanyVehicle companyVehicle, DataSet dsVehicle)
        {
            NameValue[] parameters = new NameValue[17];
            parameters[0] = new NameValue("@HOLDER_ID", companyPolicy.Holder.IndividualId);
            parameters[1] = new NameValue("@INSURED_ID", companyVehicle.Risk.MainInsured.IndividualId);
            parameters[2] = new NameValue("@BENEFICIARY_ID", companyVehicle.Risk.Beneficiaries[0].IndividualId);
            parameters[3] = new NameValue("@BRANCH_ID", companyPolicy.Branch.Id);

            if (companyPolicy.Branch.SalePoints?.Count > 0 && companyPolicy.Branch.SalePoints?[0]?.Id != null)
            {
                parameters[4] = new NameValue("@SALE_POINT_ID", companyPolicy.Branch.SalePoints[0].Id);
            }
            else
            {
                parameters[4] = new NameValue("@SALE_POINT_ID", DBNull.Value);
            }

            parameters[5] = new NameValue("@PREFIX_ID", companyPolicy.Prefix.Id);
            parameters[6] = new NameValue("@POLICY_TYPE_ID", companyPolicy.PolicyType.Id);
            parameters[7] = new NameValue("@CURRENCY_ID", companyPolicy.ExchangeRate.Currency.Id);
            parameters[8] = new NameValue("@BUSINESS_TYPE_ID", companyPolicy.BusinessType);
            parameters[9] = new NameValue("@VEHICLE_TYPE_ID", companyVehicle.Version.Type.Id);
            parameters[10] = new NameValue("@VEHICLE_COLOR_ID", companyVehicle.Color.Id);
            parameters[11] = new NameValue("@VEHICLE_USE_ID", companyVehicle.Use.Id);
            parameters[12] = new NameValue("@RATING_ZONE_ID", companyVehicle.Risk.RatingZone.Id);
            parameters[13] = new NameValue("@VEHICLE_MAKE_ID", companyVehicle.Make.Id);
            parameters[14] = new NameValue("@VEHICLE_MODEL_ID", companyVehicle.Model.Id);
            parameters[15] = new NameValue("@VEHICLE_VERSION_ID", companyVehicle.Version.Id);
            parameters[16] = new NameValue("@ENDORSEMENT_TYPE_ID", companyPolicy.Endorsement.EndorsementType);

            DataTable dataTable;

            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                dataTable = pdb.ExecuteSPDataTable("REPORT.GET_ADDITIONAL_DATA_VEHICLE", parameters);
            }

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                dsVehicle.Tables[0].Rows[0]["SMALL_DESCRIPTION"] = dataTable.Rows[0][16];
                dsVehicle.Tables[2].Rows[0]["BRANCH"] = dataTable.Rows[0][9];
                companyPolicy.Branch.SmallDescription = (string)dataTable.Rows[0][9];
                dsVehicle.Tables[2].Rows[0]["PREFIX"] = dataTable.Rows[0][12];
                companyPolicy.Prefix.SmallDescription = (string)dataTable.Rows[0][12];
                dsVehicle.Tables[2].Rows[0]["POLICY_TYPE"] = dataTable.Rows[0][13];
                dsVehicle.Tables[2].Rows[0]["CURRENCY"] = dataTable.Rows[0][14];
                companyPolicy.ExchangeRate.Currency.SmallDescription = (string)dataTable.Rows[0][14];
                dsVehicle.Tables[2].Rows[0]["CURRENCY_SYMBOL"] = dataTable.Rows[0][15];
                companyPolicy.ExchangeRate.Currency.TinyDescription = (string)dataTable.Rows[0][15];
                dsVehicle.Tables[2].Rows[0]["POLICY_HOLDER_ADD"] = dataTable.Rows[0][1];
                dsVehicle.Tables[2].Rows[0]["POLICY_HOLDER_DOC_TYPE"] = dataTable.Rows[0][0];
                dsVehicle.Tables[2].Rows[0]["POLICY_HOLDER_PHONE"] = dataTable.Rows[0][2];
                dsVehicle.Tables[2].Rows[0]["ENDORSEMENT_TYPE_DESC"] = dataTable.Rows[0][23];
                dsVehicle.Tables[4].Rows[0]["INSURED_ADD"] = dataTable.Rows[0][4];
                dsVehicle.Tables[4].Rows[0]["INSURED_DOC_TYPE"] = dataTable.Rows[0][3];
                dsVehicle.Tables[4].Rows[0]["INSURED_PHONE"] = dataTable.Rows[0][5];
                dsVehicle.Tables[4].Rows[0]["RATING_ZONE_DESC"] = dataTable.Rows[0][20];
                dsVehicle.Tables[4].Rows[0]["BENEFICIARY_ADD"] = dataTable.Rows[0][7];
                dsVehicle.Tables[4].Rows[0]["BENEFICIARY_DOC_TYPE"] = dataTable.Rows[0][6];
                dsVehicle.Tables[4].Rows[0]["BENEFICIARY_PHONE"] = dataTable.Rows[0][8];
                dsVehicle.Tables[6].Rows[0]["VEHICLE_TYPE"] = dataTable.Rows[0][17];
                dsVehicle.Tables[6].Rows[0]["VEHICLE_COLOR"] = dataTable.Rows[0][18];
                dsVehicle.Tables[6].Rows[0]["VEHICLE_USE"] = dataTable.Rows[0][19];
                dsVehicle.Tables[6].Rows[0]["RATING_ZONE_DESCRIPTION"] = dataTable.Rows[0][20];
                dsVehicle.Tables[6].Rows[0]["VEHICLE_MODEL"] = dataTable.Rows[0][21] + " " + dataTable.Rows[0][22];
                dsVehicle.Tables[6].Rows[0]["VEHICLE_MAKE"] = dataTable.Rows[0][24];
            }
        }

        private void SetAdditionalDataThirdPartyLiability(CompanyPolicy companyPolicy, CompanyTplRisk thirdPartyLiability, DataSet dsVehicle)
        {
            NameValue[] parameters = new NameValue[17];
            parameters[0] = new NameValue("@HOLDER_ID", companyPolicy.Holder.IndividualId);
            parameters[1] = new NameValue("@INSURED_ID", thirdPartyLiability.Risk.MainInsured.IndividualId);
            parameters[2] = new NameValue("@BENEFICIARY_ID", thirdPartyLiability.Risk.Beneficiaries[0].IndividualId);
            parameters[3] = new NameValue("@BRANCH_ID", companyPolicy.Branch.Id);
            parameters[4] = new NameValue("@SALE_POINT_ID", companyPolicy.Branch.SalePoints == null ? 0 : companyPolicy.Branch.SalePoints.Count > 0 ? 0 : companyPolicy.Branch.SalePoints[0].Id);
            parameters[5] = new NameValue("@PREFIX_ID", companyPolicy.Prefix.Id);
            parameters[6] = new NameValue("@POLICY_TYPE_ID", companyPolicy.PolicyType.Id);
            parameters[7] = new NameValue("@CURRENCY_ID", companyPolicy.ExchangeRate.Currency.Id);
            parameters[8] = new NameValue("@BUSINESS_TYPE_ID", companyPolicy.BusinessType);
            parameters[9] = new NameValue("@VEHICLE_TYPE_ID", thirdPartyLiability.Version.Type.Id);
            parameters[10] = new NameValue("@VEHICLE_COLOR_ID", 0);
            parameters[11] = new NameValue("@VEHICLE_USE_ID", 0);
            parameters[12] = new NameValue("@RATING_ZONE_ID", thirdPartyLiability.Risk.RatingZone.Id);
            parameters[13] = new NameValue("@VEHICLE_MAKE_ID", thirdPartyLiability.Make.Id);
            parameters[14] = new NameValue("@VEHICLE_MODEL_ID", thirdPartyLiability.Model.Id);
            parameters[15] = new NameValue("@VEHICLE_VERSION_ID", thirdPartyLiability.Version.Id);
            parameters[16] = new NameValue("@ENDORSEMENT_TYPE_ID", companyPolicy.Endorsement.EndorsementType);

            DataTable dataTable;

            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                dataTable = pdb.ExecuteSPDataTable("REPORT.GET_ADDITIONAL_DATA_VEHICLE", parameters);
            }

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                dsVehicle.Tables[0].Rows[0]["SMALL_DESCRIPTION"] = dataTable.Rows[0][16];
                dsVehicle.Tables[1].Rows[0]["BRANCH"] = dataTable.Rows[0][9];
                companyPolicy.Branch.SmallDescription = (string)dataTable.Rows[0][9];
                dsVehicle.Tables[1].Rows[0]["PREFIX"] = dataTable.Rows[0][12];
                companyPolicy.Prefix.SmallDescription = (string)dataTable.Rows[0][12];
                dsVehicle.Tables[1].Rows[0]["POLICY_TYPE"] = dataTable.Rows[0][13];
                dsVehicle.Tables[1].Rows[0]["CURRENCY"] = dataTable.Rows[0][14];
                companyPolicy.ExchangeRate.Currency.SmallDescription = (string)dataTable.Rows[0][14];
                dsVehicle.Tables[1].Rows[0]["CURRENCY_SYMBOL"] = dataTable.Rows[0][15];
                companyPolicy.ExchangeRate.Currency.TinyDescription = (string)dataTable.Rows[0][15];
                dsVehicle.Tables[1].Rows[0]["POLICY_HOLDER_ADD"] = dataTable.Rows[0][1];
                dsVehicle.Tables[1].Rows[0]["POLICY_HOLDER_DOC_TYPE"] = dataTable.Rows[0][0];
                dsVehicle.Tables[1].Rows[0]["POLICY_HOLDER_PHONE"] = dataTable.Rows[0][2];
                dsVehicle.Tables[1].Rows[0]["ENDORSEMENT_TYPE_DESC"] = dataTable.Rows[0][23];
                dsVehicle.Tables[2].Rows[0]["INSURED_ADD"] = dataTable.Rows[0][4];
                dsVehicle.Tables[2].Rows[0]["INSURED_DOC_TYPE"] = dataTable.Rows[0][3];
                dsVehicle.Tables[2].Rows[0]["INSURED_PHONE"] = dataTable.Rows[0][5];
                dsVehicle.Tables[2].Rows[0]["RATING_ZONE_DESC"] = dataTable.Rows[0][20];
                dsVehicle.Tables[2].Rows[0]["BENEFICIARY_ADD"] = dataTable.Rows[0][7];
                dsVehicle.Tables[2].Rows[0]["BENEFICIARY_DOC_TYPE"] = dataTable.Rows[0][6];
                dsVehicle.Tables[2].Rows[0]["BENEFICIARY_PHONE"] = dataTable.Rows[0][8];
                dsVehicle.Tables[4].Rows[0]["VEHICLE_TYPE"] = dataTable.Rows[0][17];
                dsVehicle.Tables[4].Rows[0]["VEHICLE_COLOR"] = dataTable.Rows[0][18];
                dsVehicle.Tables[4].Rows[0]["VEHICLE_USE"] = dataTable.Rows[0][19];
                dsVehicle.Tables[4].Rows[0]["RATING_ZONE_DESCRIPTION"] = dataTable.Rows[0][20];
                dsVehicle.Tables[4].Rows[0]["VEHICLE_MODEL"] = dataTable.Rows[0][21] + " " + dataTable.Rows[0][22];
            }
        }

        private DataTable SetDataCO_TMP_POLICY_COVER(List<CompanyAccessory> accesories)
        {
            DataTable dtAccesories = DatasetHelper.CreateCO_TMP_POLICY_COVER();

            if (accesories != null && accesories.Count > 0)
            {
                DataRow dataRow = dtAccesories.NewRow();

                dataRow["ACCESORIES_TOTAL"] = accesories.Where(x => !x.IsOriginal).Sum(x => x.Amount);

                dtAccesories.Rows.Add(dataRow);
            }

            return dtAccesories;
        }

        private List<string> GenerateReportProperty(CompanyFilterReport companyFilterReport, string userFolderPath)
        {
            List<string> reportPaths = new List<string>();
            CompanyPolicy companyPolicy = new CompanyPolicy();
            List<CompanyPropertyRisk> companyPropertyRisks = new List<CompanyPropertyRisk>();

            if (companyFilterReport.EndorsementId == 0)
            {
                companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyFilterReport.TemporalId, false);
                companyPropertyRisks = DelegateService.propertyService.GetCompanyPropertiesByTemporalId(companyFilterReport.TemporalId);
            }
            else
            {
                companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(companyFilterReport.EndorsementId);
                companyPropertyRisks = DelegateService.propertyService.GetCompanyPropertiesByEndorsementId(companyFilterReport.EndorsementId);
            }

            foreach (CompanyPropertyRisk companyPropertyRisk in companyPropertyRisks)
            {
                using (ReportDocument reportDocument = new ReportDocument())
                {

                    DataSet dsLocation = new DataSet("DataSetLocation");
                    dsLocation.Tables.Add(CreateCO_TMP_POLICY_COVER(companyPolicy, companyPropertyRisk.Risk, companyFilterReport.User.AccountName));

                    dsLocation.Tables.Add(CreateCO_TMP_POLICY_LOCATION(companyPropertyRisk.Risk));
                    dsLocation.Tables.Add(SetDataCO_POLICY_AGENT(companyPolicy.Agencies));
                    dsLocation.Tables.Add(SetDataCO_TMP_POLICY_RISK(companyPolicy, companyPropertyRisk.Risk));

                    dsLocation.Tables.Add(CreateCO_TMP_POLICY_RISK_BENEFICIARY(companyPropertyRisk.Risk, companyPolicy, companyPropertyRisk.Risk.Beneficiaries));

                    if (companyPropertyRisk.Risk.Coverages != null && companyPropertyRisk.Risk.Coverages.Count > 0)
                    {
                        dsLocation.Tables.Add(CreateCO_TMP_POLICY_RISK_COVERAGE(companyPropertyRisk.Risk.Coverages, companyPolicy));
                    }
                    if (companyPolicy.CoInsuranceCompanies != null && companyPolicy.CoInsuranceCompanies.Count > 0)
                    {
                        dsLocation.Tables.Add(SetDataCO_TMP_POLICY_COINSURANCE(companyPolicy));
                    }

                    dsLocation.Tables.Add(CreateCO_TMP_POLICY_TEXT(companyPolicy));
                    dsLocation.Tables.Add(SetDataBUSINESS_TYPE(companyPolicy));
                    dsLocation.Tables.Add(CreateCO_POLICY_Payer(companyPolicy));
                    dsLocation.Tables.Add(CreateCO_TMP_POLICY(companyPolicy));
                    SetAdditionalDataCompanyProperty(companyPropertyRisk.Risk, dsLocation, companyPolicy);
                    string reportTemplate = userFolderPath + Guid.NewGuid() + ".pdf";
                    reportPaths.Add(reportTemplate);

                    reportDocument.Load(GetTemplatePathByTemplateName("LocationCover"));
                    reportDocument.SetDataSource(dsLocation);

                    reportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, reportTemplate);

                    DataTable dtClauses = SetDataCLAUSES(companyPolicy.Clauses, companyPropertyRisk.Risk.Clauses);
                    if (companyPolicy.Text?.TextBody != null || dtClauses.Rows.Count > 0)
                    {
                        dsLocation.Tables.Add(dtClauses);
                        dsLocation.Tables.Add(CreateCO_TMP_POLICY_CLAUSES(companyPolicy));
                        reportTemplate = userFolderPath + Guid.NewGuid() + ".pdf";
                        reportPaths.Add(reportTemplate);
                        reportDocument.Load(GetTemplatePathByTemplateName("LocationCoverAppendix"));
                        reportDocument.SetDataSource(dsLocation);
                        reportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, reportTemplate);

                    }

                }
            }

            reportPaths.Add(GenerateReportConvection(companyPolicy, companyFilterReport.User.AccountName, userFolderPath));

            return reportPaths;
        }

        private DataTable CreateCO_TMP_POLICY(CompanyPolicy companyPolicy)
        {
            DataTable CO_TMP_POLICY = DatasetHelper.CreateCO_TMP_POLICY();
            DataRow dataRow = CO_TMP_POLICY.NewRow();

            dataRow["BUSINESS_TYPE_DESC"] = companyPolicy.BusinessTypeDescription;
            CO_TMP_POLICY.Rows.Add(dataRow);

            return CO_TMP_POLICY;
        }

        private List<string> GenerateReportLiability(CompanyFilterReport companyFilterReport, string userFolderPath)
        {
            List<string> reportPaths = new List<string>();
            CompanyPolicy companyPolicy = new CompanyPolicy();
            List<CompanyLiabilityRisk> companyLiabilityRisks = new List<CompanyLiabilityRisk>();

            if (companyFilterReport.EndorsementId == 0)
            {
                companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyFilterReport.TemporalId, false);
                companyLiabilityRisks = DelegateService.liabilityService.GetCompanyLiabilitiesByTemporalId(companyFilterReport.TemporalId);
            }
            else
            {
                companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(companyFilterReport.EndorsementId);
                companyLiabilityRisks = DelegateService.liabilityService.GetCompanyLiabilitiesByEndorsementId(companyFilterReport.EndorsementId);
            }

            foreach (CompanyLiabilityRisk companyPropertyRisk in companyLiabilityRisks)
            {
                using (ReportDocument reportDocument = new ReportDocument())
                {
                    DataSet dsLocation = new DataSet("DataSetLocation");

                    dsLocation.Tables.Add(CreateCO_TMP_POLICY_COVER(companyPolicy, companyPropertyRisk.Risk, companyFilterReport.User.AccountName));
                    dsLocation.Tables.Add(CreateCO_TMP_POLICY_LOCATION(companyPropertyRisk.Risk));
                    dsLocation.Tables.Add(SetDataCO_POLICY_AGENT(companyPolicy.Agencies));
                    dsLocation.Tables.Add(SetDataCO_TMP_POLICY_RISK(companyPolicy, companyPropertyRisk.Risk));
                    dsLocation.Tables.Add(CreateCO_TMP_POLICY_RISK_BENEFICIARY(companyPropertyRisk.Risk, companyPolicy, companyPropertyRisk.Risk.Beneficiaries));
                    dsLocation.Tables.Add(CreateCO_TMP_POLICY_RISK_COVERAGE(companyPropertyRisk.Risk.Coverages, companyPolicy));
                    dsLocation.Tables.Add(SetDataCO_TMP_POLICY_COINSURANCE(companyPolicy));
                    dsLocation.Tables.Add(CreateCO_TMP_POLICY_TEXT(companyPolicy));
                    dsLocation.Tables.Add(SetDataBUSINESS_TYPE(companyPolicy));
                    dsLocation.Tables.Add(CreateCO_POLICY_Payer(companyPolicy));

                    dsLocation.Tables.Add(CreateCO_TMP_POLICY(companyPolicy));

                    SetAdditionalDataCompanyProperty(companyPropertyRisk.Risk, dsLocation, companyPolicy);

                    string reportTemplate = userFolderPath + Guid.NewGuid() + ".pdf";
                    reportPaths.Add(reportTemplate);

                    reportDocument.Load(GetTemplatePathByTemplateName("LocationCover"));
                    reportDocument.SetDataSource(dsLocation);

                    reportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, reportTemplate);

                    DataTable dtClauses = SetDataCLAUSES(companyPolicy.Clauses, companyPropertyRisk.Risk.Clauses);
                    if (companyPolicy.Text?.TextBody != null || dtClauses.Rows.Count > 0)
                    {
                        dsLocation.Tables.Add(dtClauses);
                        dsLocation.Tables.Add(CreateCO_TMP_POLICY_CLAUSES(companyPolicy));
                        reportTemplate = userFolderPath + Guid.NewGuid() + ".pdf";
                        reportPaths.Add(reportTemplate);
                        reportDocument.Load(GetTemplatePathByTemplateName("LocationCoverAppendix"));
                        reportDocument.SetDataSource(dsLocation);
                        reportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, reportTemplate);

                    }
                }
            }

            reportPaths.Add(GenerateReportConvection(companyPolicy, companyFilterReport.User.AccountName, userFolderPath));

            return reportPaths;
        }

        private DataTable CreateCO_TMP_POLICY_CLAUSES(CompanyPolicy companyPolicy)
        {
            DataTable CreateCO_TMP_POLICY_CLAUSES = DatasetHelper.CreateCO_TMP_POLICY_CLAUSES();
            DataRow dataRow = CreateCO_TMP_POLICY_CLAUSES.NewRow();

            dataRow["PROCESS_ID"] = 1;
            dataRow["ENDORSEMENT_ID"] = companyPolicy.Endorsement.EndorsementType;
            dataRow["POLICY_ID"] = companyPolicy.Id;

            CreateCO_TMP_POLICY_CLAUSES.Rows.Add(dataRow);

            return CreateCO_TMP_POLICY_CLAUSES;
        }

        private List<string> GenerateReportSurety(CompanyFilterReport companyFilterReport, string userFolderPath)
        {
            List<string> reportPaths = new List<string>();
            CompanyPolicy companyPolicy = new CompanyPolicy();
            List<CompanyContract> companySurety = new List<CompanyContract>();

            if (companyFilterReport.EndorsementId == 0)
            {
                companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyFilterReport.TemporalId, false);
                companySurety = DelegateService.suretyService.GetCompanySuretiesByTemporalId(companyFilterReport.TemporalId);
            }
            else
            {
                companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(companyFilterReport.EndorsementId);
                companySurety = DelegateService.suretyService.GetCompanySuretyByEndorsementId(companyFilterReport.EndorsementId);
            }

            foreach (CompanyContract companySuretyRisk in companySurety)
            {
                using (ReportDocument reportDocument = new ReportDocument())
                {
                    DataSet dsSurety = new DataSet("DataSetSurety");

                    dsSurety.Tables.Add(CreateCO_TMP_POLICY_COVER(companyPolicy, companySuretyRisk.Risk, companyFilterReport.User.AccountName));

                    dsSurety.Tables.Add(SetDataCO_TMP_POLICY_RISK(companyPolicy, companySuretyRisk.Risk));

                    dsSurety.Tables.Add(CreateCO_TMP_POLICY_TEXT(companyPolicy));

                    dsSurety.Tables.Add(SetDataCO_TMP_POLICY(companyPolicy, companyFilterReport.User.AccountName));
                    if (companySuretyRisk != null && companySuretyRisk.Risk.Coverages != null && companySuretyRisk.Risk.Coverages.Count > 0)
                        dsSurety.Tables.Add(SetDataCO_TMP_POLICY_RISK_COVERAGE(companySuretyRisk.Risk.Coverages, 0));

                    if (companyPolicy.CoInsuranceCompanies != null && companyPolicy.CoInsuranceCompanies.Count > 0)
                        dsSurety.Tables.Add(SetDataCO_TMP_POLICY_COINSURANCE(companyPolicy));

                    dsSurety.Tables.Add(SetDataCO_POLICY_AGENT(companyPolicy.Agencies));

                    SetAdditionalDataCompanySurety(companyPolicy, dsSurety, companySuretyRisk.Risk, companySuretyRisk.Contractor);

                    string reportTemplate = userFolderPath + Guid.NewGuid() + ".pdf";
                    reportPaths.Add(reportTemplate);

                    reportDocument.Load(GetTemplatePathByTemplateName("SuretyCover"));
                    reportDocument.SetDataSource(dsSurety);

                    reportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, reportTemplate);
                    DataTable dtClauses = SetDataCLAUSES(companyPolicy.Clauses, companySuretyRisk.Risk.Clauses);
                    if (companyPolicy.Text?.TextBody != null || dtClauses.Rows.Count > 0)
                    {
                        dsSurety.Tables.Add(dtClauses);
                        dsSurety.Tables.Add(CreateCO_TMP_POLICY_RISK_SURETY_CONTRACT(companySuretyRisk.ContractObject));
                        reportTemplate = userFolderPath + Guid.NewGuid() + ".pdf";
                        reportPaths.Add(reportTemplate);
                        reportDocument.Load(GetTemplatePathByTemplateName("SuretyCoverAppendix"));
                        reportDocument.SetDataSource(dsSurety);
                        reportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, reportTemplate);
                    }
                }
            }

            reportPaths.Add(GenerateReportConvection(companyPolicy, companyFilterReport.User.AccountName, userFolderPath));
            return reportPaths;
        }

        private DataTable CreateCO_TMP_POLICY_RISK_SURETY_CONTRACT(CompanyText text)
        {
            DataTable CO_TMP_POLICY_RISK_SURETY_CONTRACT = DatasetHelper.createCO_TMP_POLICY_RISK_SURETY_CONTRACT();
            DataRow dataRow = CO_TMP_POLICY_RISK_SURETY_CONTRACT.NewRow();

            dataRow["PROCESS_ID"] = 1;
            dataRow["OBJECT_CONTRACT"] = text.TextBody;
            CO_TMP_POLICY_RISK_SURETY_CONTRACT.Rows.Add(dataRow);

            return CO_TMP_POLICY_RISK_SURETY_CONTRACT;
        }

        public void WaterMark(string waterMark, string reportPath)
        {
            if ((waterMark != null) && (waterMark.Length > 0))
            {
                byte[] fileBytes = SetWaterMark(reportPath, waterMark);
                FileStream fileStream = new FileStream(reportPath, FileMode.Create);
                fileStream.Write(fileBytes, 0, fileBytes.Length);
                fileStream.Close();
            }
        }

        public Printing CreatePrinting(List<CompanyFilterReport> companyFilterReports, Printing_Type printing_Type, string description, int massiveLoadId)
        {
            Printing printLog = new Printing();
            printLog.PrintingTypeId = Convert.ToInt32(printing_Type);
            printLog.KeyId = massiveLoadId;
            printLog.UrlFile = "";
            printLog.Total = companyFilterReports.Count();
            printLog.BeginDate = DateTime.Now;
            printLog.FinishDate = DateTime.Now;
            printLog.UserId = companyFilterReports[0].User.UserId;
            return DelegateService.massiveServiceCore.CreatePrinting(printLog);
        }

        public void UpdatePrinting(string url, Printing printLog)
        {

            printLog.FinishDate = DateTime.Now;
            printLog.UrlFile = url;
            DelegateService.massiveServiceCore.UpdatePrinting(printLog);
        }

        public PrintingLog CreatePrintingLog(Printing printing)
        {
            PrintingLog printingLog = new PrintingLog();
            printingLog.Id = printing.Id;
            printingLog.Description = "OK";

            return printingLog;
        }

        public void DeletePdf(string[] NombreArchivos)
        {
            foreach (var item in NombreArchivos)
            {
                if (System.IO.File.Exists(item))
                {
                    System.IO.File.Delete(item);
                }
            }
        }

        /// <summary>
        /// Crear ruta del reporte
        /// </summary>
        /// <param name="userFolderPath">Ruta de la carpeta</param>
        /// <param name="filterVehicle">Datos para el nombre</param>
        /// <param name="reportName">Nombre</param>
        /// <returns>Ruta reporte</returns>
        private string CreateReportPath(string userFolderPath, CompanyPolicy companyPolicy, string reportName, int id)
        {
            if (companyPolicy.DocumentNumber > 0)
            {
                return userFolderPath + companyPolicy.DocumentNumber.ToString() + id + companyPolicy.Branch.Id.ToString() + companyPolicy.Prefix.Id.ToString() + companyPolicy.Endorsement.Id.ToString() + reportName + ".pdf";
            }
            else
            {
                return userFolderPath + id + reportName + ".pdf";
            }
        }

        public void DeleteFiles(List<string> reportPaths)
        {
            foreach (string reportPath in reportPaths)
            {
                System.IO.File.Delete(reportPath);
            }

        }
        #region metodos usados

        /// <summary>
        /// Obtener ruta de la plantilla del reporte
        /// </summary>
        /// <param name="templateName">Nombre de la plantilla</param>
        /// <returns>Ruta de la plantilla</returns>
        private string GetTemplatePathByTemplateName(string templateName)
        {
            return reportTemplatePath + ConfigurationSettings.AppSettings[templateName];
        }

        /// <summary>
        /// Unir todos los PDF's generados
        /// </summary>
        /// <param name="reportPaths">Ruta reportes</param>
        /// <param name="pathFile">Ruta reporte unido</param>
        private string Merge(List<string> reportPaths, string createPath, string exportPath, int EndorsementId)
        {
            Document document = new Document();
            PdfCopy pdfCopy = new PdfCopy(document, new FileStream(createPath, FileMode.Create));
            document.Open();
            PdfImportedPage page;
            PdfCopy.PageStamp stamp;

            foreach (string reportPath in reportPaths)
            {
                PdfReader reader = new PdfReader(new RandomAccessFileOrArray(reportPath), null);
                int pages = reader.NumberOfPages;

                for (int i = 1; i <= pages; i++)
                {
                    page = pdfCopy.GetImportedPage(reader, i);
                    stamp = pdfCopy.CreatePageStamp(page);
                    PdfContentByte pdfContentByte = stamp.GetUnderContent();
                    pdfContentByte.SaveState();
                    stamp.AlterContents();
                    pdfCopy.AddPage(page);
                }
            }

            document.Close();
            pdfCopy.Close();
            if (exportPath != "")
            {
                System.IO.File.Copy(createPath, exportPath, true);
                reportPaths.Add(createPath);
            }

            if (EndorsementId == 0)
            {
                WaterMark(ConfigurationSettings.AppSettings["WaterMark"], exportPath);
            }
            return exportPath;
        }

        /// <summary>
        /// Crear marca de agua
        /// </summary>
        /// <param name="pdfName">Nombre del PDF</param>
        /// <param name="stringToWriteToPdf">Ruta reporte a asignar marca</param>
        /// <returns></returns>
        private static byte[] SetWaterMark(String pdfName, String stringToWriteToPdf)
        {
            int red = Convert.ToInt32(ConfigurationSettings.AppSettings["Red"]);
            int green = Convert.ToInt32(ConfigurationSettings.AppSettings["Green"]);
            int blue = Convert.ToInt32(ConfigurationSettings.AppSettings["Blue"]);
            int fontSize = Convert.ToInt32(ConfigurationSettings.AppSettings["FontSize"]);

            PdfReader reader = new PdfReader(pdfName);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfStamper pdfStamper = new PdfStamper(reader, memoryStream);

                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    Rectangle pageSize = reader.GetPageSizeWithRotation(i);
                    PdfContentByte pdfPageContents = pdfStamper.GetUnderContent(i);
                    pdfPageContents.BeginText();
                    BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, Encoding.ASCII.EncodingName, false);
                    pdfPageContents.SetFontAndSize(baseFont, fontSize);
                    pdfPageContents.SetRGBColorFill(red, green, blue);
                    float textAngle = (float)GetHypotenuseAngleInDegreesFrom(pageSize.Height, pageSize.Width);
                    pdfPageContents.ShowTextAligned(PdfContentByte.ALIGN_CENTER, stringToWriteToPdf, pageSize.Width / 2, pageSize.Height / 2, textAngle);
                    pdfPageContents.EndText();
                }

                pdfStamper.FormFlattening = true;
                pdfStamper.Close();
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Obtener angulo de la marca de agua
        /// </summary>
        /// <param name="opposite">angulo X</param>
        /// <param name="adjacent">angulo Y</param>
        /// <returns>Angulo</returns>
        private static double GetHypotenuseAngleInDegreesFrom(double opposite, double adjacent)
        {
            double radians = Math.Atan2(opposite, adjacent);
            double angle = radians * (180 / Math.PI);
            return angle;
        }

        #endregion

        #region LLenado de tablas

        private DataTable SetDataCO_TMP_POLICY_VEHICLE(CompanyVehicle companyVehicle)
        {
            DataTable CO_TMP_POLICY_VEHICLE = DatasetHelper.CreateCO_TMP_POLICY_VEHICLE();
            DataRow dataRow = CO_TMP_POLICY_VEHICLE.NewRow();

            dataRow["RISK_NUM"] = companyVehicle.Risk.Number;
            dataRow["PROCESS_ID"] = 1;
            dataRow["VEHICLE_LICENSE"] = companyVehicle.LicensePlate;
            dataRow["VEHICLE_MAKE"] = companyVehicle.Make.Description;
            dataRow["VEHICLE_YEAR"] = companyVehicle.Year;
            dataRow["VEHICLE_ENGINE_SER"] = companyVehicle.EngineSerial;
            dataRow["VEHICLE_MODEL"] = companyVehicle.Model.Description + " " + companyVehicle.Version.Description;
            dataRow["VEHICLE_FASECOLDA_CD"] = companyVehicle.Fasecolda.Description;
            dataRow["VEHICLE_CHASSIS_SER"] = companyVehicle.ChassisSerial;
            dataRow["IS_NEW"] = companyVehicle.IsNew;

            CO_TMP_POLICY_VEHICLE.Rows.Add(dataRow);

            return CO_TMP_POLICY_VEHICLE;
        }

        private DataTable SetDataCO_TMP_POLICY_VEHICLE(CompanyTplRisk thirdPartyLiability)
        {
            DataTable CO_TMP_POLICY_VEHICLE = DatasetHelper.CreateCO_TMP_POLICY_VEHICLE();
            DataRow dataRow = CO_TMP_POLICY_VEHICLE.NewRow();

            dataRow["RISK_NUM"] = thirdPartyLiability.Risk.Number;
            dataRow["PROCESS_ID"] = 1;
            dataRow["VEHICLE_LICENSE"] = thirdPartyLiability.LicensePlate;
            dataRow["VEHICLE_MAKE"] = thirdPartyLiability.Make.Description;
            dataRow["VEHICLE_YEAR"] = thirdPartyLiability.Year;
            dataRow["VEHICLE_ENGINE_SER"] = thirdPartyLiability.EngineSerial;
            dataRow["VEHICLE_CHASSIS_SER"] = thirdPartyLiability.ChassisSerial;
            dataRow["IS_NEW"] = thirdPartyLiability.IsNew;

            CO_TMP_POLICY_VEHICLE.Rows.Add(dataRow);

            return CO_TMP_POLICY_VEHICLE;
        }

        private DataTable SetDataCO_TMP_POLICY(CompanyPolicy companyPolicy, string userName)
        {
            DataTable CO_TMP_POLICY = DatasetHelper.CreateCO_TMP_POLICY();
            DataRow dataRow = CO_TMP_POLICY.NewRow();

            dataRow["RISK_NUM"] = 1;
            dataRow["POLICY_NUMBER"] = companyPolicy.DocumentNumber;
            dataRow["POLICY_ID"] = companyPolicy.Id;
            dataRow["PROCESS_ID"] = companyPolicy.Id;
            dataRow["BRANCH_CD"] = companyPolicy.Branch.Id;
            dataRow["PREFIX_CD"] = companyPolicy.Prefix.Id;
            dataRow["DOCUMENT_NUM"] = companyPolicy.Endorsement.Number;
            dataRow["EXCHANGE_RATE"] = companyPolicy.ExchangeRate.SellAmount;
            dataRow["ENDORSEMENT_ID"] = companyPolicy.Endorsement.Id;
            dataRow["ISSUE_DATE_DAY"] = companyPolicy.IssueDate.Day;
            dataRow["ISSUE_DATE_MONTH"] = companyPolicy.IssueDate.Month;
            dataRow["ISSUE_DATE_Year"] = companyPolicy.IssueDate.Year;
            dataRow["POLICY_HOLDER_NAME"] = companyPolicy.Holder.Name;
            dataRow["POLICY_HOLDER_DOC"] = companyPolicy.Holder.IdentificationDocument.Number;
            dataRow["CURRENT_FROM_DAY"] = companyPolicy.CurrentFrom.Day;
            dataRow["CURRENT_FROM_MONTH"] = companyPolicy.CurrentFrom.Month;
            dataRow["CURRENT_FROM_Year"] = companyPolicy.CurrentFrom.Year;
            dataRow["CURRENT_FROM_HOUR"] = companyPolicy.CurrentFrom.ToString("HH:mm");
            dataRow["CURRENT_TO_DAY"] = companyPolicy.CurrentTo.Day;
            dataRow["CURRENT_TO_MONTH"] = companyPolicy.CurrentTo.Month;
            dataRow["CURRENT_TO_Year"] = companyPolicy.CurrentTo.Year;
            dataRow["CURRENT_TO_HOUR"] = companyPolicy.CurrentTo.ToString("HH:mm");
            dataRow["DAY_COUNT"] = System.Data.Linq.SqlClient.SqlMethods.DateDiffDay(companyPolicy.CurrentFrom, companyPolicy.CurrentTo);
            dataRow["PAYER_NAME"] = companyPolicy.Holder.Name;
            dataRow["PAYMENT_METHOD"] = companyPolicy.PaymentPlan.Description;
            dataRow["PREMIUM_AMT"] = companyPolicy.Summary == null ? 0 : companyPolicy.Summary.Premium;
            dataRow["LIMIT_AMT"] = companyPolicy.Summary == null ? 0 : companyPolicy.Summary.AmountInsured;
            dataRow["EXPENSES"] = companyPolicy.Summary == null ? 0 : companyPolicy.Summary.Expenses;
            dataRow["TAX"] = companyPolicy.Summary == null ? 0 : companyPolicy.Summary.Taxes;
            dataRow["PAY_EXP_DATE"] = companyPolicy.CurrentFrom;
            dataRow["PRODUCT_DESCRIPTION"] = companyPolicy.Product.Description;
            dataRow["TEMP_TYPE_CD"] = companyPolicy.TemporalType;
            dataRow["PRODUCT_FORM_NUMBER"] = FromNumber;
            dataRow["BUSINESS_TYPE_CD"] = companyPolicy.BusinessType.Value;
            dataRow["USER_NAME"] = userName;
            dataRow["AGREED_PAYMENT_METHOD"] = companyPolicy.PaymentPlan.Description;
            dataRow["CROP_RC_POLICY_NUMBER"] = companyPolicy.Endorsement.QuotationId;

            if (companyPolicy.BillingGroup != null)
            {
                dataRow["BILLING_GROUP"] = "C-" + companyPolicy.BillingGroup.Id.ToString();
            }

            CO_TMP_POLICY.Rows.Add(dataRow);

            return CO_TMP_POLICY;
        }

        private DataTable CreateCO_TMP_POLICY_COVER(CompanyPolicy companyPolicy, CompanyRisk companyRisk, string userName)
        {
            DataTable CO_TMP_POLICY_COVER = DatasetHelper.CreateCO_TMP_POLICY_COVER();
            DataRow dataRow = CO_TMP_POLICY_COVER.NewRow();

            dataRow["RISK_NUM"] = 1;
            dataRow["POLICY_NUMBER"] = companyPolicy.DocumentNumber;
            dataRow["ASSISTANCE"] = 0;
            dataRow["POLICY_ID"] = companyPolicy.Id;
            dataRow["PROCESS_ID"] = companyPolicy.Id;
            dataRow["BRANCH_CD"] = companyPolicy.Branch.Id;
            dataRow["BRANCH"] = companyPolicy.Branch.Description;
            dataRow["PREFIX_CD"] = companyPolicy.Prefix.Id;
            dataRow["PREFIX"] = companyPolicy.Prefix.Description;
            dataRow["POLICY_TYPE"] = companyPolicy.PolicyType.Description;
            dataRow["DOCUMENT_NUM"] = companyPolicy.Endorsement.Number;
            dataRow["EXCHANGE_RATE"] = companyPolicy.ExchangeRate.SellAmount;
            dataRow["ENDORSEMENT_ID"] = companyPolicy.Endorsement.Id;
            dataRow["ISSUE_DATE_DAY"] = companyPolicy.IssueDate.Day;
            dataRow["ISSUE_DATE_MONTH"] = companyPolicy.IssueDate.Month;
            dataRow["ISSUE_DATE_Year"] = companyPolicy.IssueDate.Year;
            dataRow["CURRENCY_SYMBOL"] = companyPolicy.ExchangeRate.Currency.TinyDescription;
            if (companyPolicy.Holder.Name != null)
            {
                dataRow["POLICY_HOLDER_NAME"] = companyPolicy.Holder.Name;
            }
            else
            {
                dataRow["POLICY_HOLDER_NAME"] = "";
            }
            dataRow["POLICY_HOLDER_ADD"] = companyPolicy.Holder.CompanyName.Address?.Description + "," + companyPolicy.Holder.CompanyName.Address.City?.Description + "," + companyPolicy.Holder.CompanyName.Address.City?.State?.Description;
            dataRow["POLICY_HOLDER_DOC_TYPE"] = companyPolicy.Holder.IdentificationDocument.DocumentType?.SmallDescription;
            dataRow["POLICY_HOLDER_DOC"] = companyPolicy.Holder.IdentificationDocument.Number;
            if (companyPolicy.Holder.CompanyName.Phone?.Description != null)
                dataRow["POLICY_HOLDER_PHONE"] = companyPolicy.Holder.CompanyName.Phone.Description;
            dataRow["CURRENT_FROM_DAY"] = companyPolicy.CurrentFrom.Day;
            dataRow["CURRENT_FROM_MONTH"] = companyPolicy.CurrentFrom.Month;
            dataRow["CURRENT_FROM_Year"] = companyPolicy.CurrentFrom.Year;
            dataRow["CURRENT_FROM_HOUR"] = companyPolicy.CurrentFrom.ToString("HH:mm");
            dataRow["CURRENT_TO_DAY"] = companyPolicy.CurrentTo.Day;
            dataRow["CURRENT_TO_MONTH"] = companyPolicy.CurrentTo.Month;
            dataRow["CURRENT_TO_Year"] = companyPolicy.CurrentTo.Year;
            dataRow["CURRENT_TO_HOUR"] = companyPolicy.CurrentTo.ToString("HH:mm");
            dataRow["DAY_COUNT"] = System.Data.Linq.SqlClient.SqlMethods.DateDiffDay(companyPolicy.CurrentFrom, companyPolicy.CurrentTo);
            dataRow["PAYER_NAME"] = companyPolicy.Holder.Name;
            dataRow["PAYMENT_METHOD"] = companyPolicy.PaymentPlan.Description;
            dataRow["PREMIUM_AMT"] = companyPolicy.Summary == null ? 0 : companyPolicy.Summary.Premium; ;
            dataRow["LIMIT_AMT"] = companyPolicy.Summary == null ? 0 : companyPolicy.Summary.AmountInsured;
            dataRow["LIMIT_AMT_RC"] = 0;
            dataRow["EXPENSES"] = companyPolicy.Summary == null ? 0 : companyPolicy.Summary.Expenses;
            dataRow["TAX"] = companyPolicy.Summary.Taxes;
            dataRow["PAY_EXP_DATE"] = companyPolicy.CurrentFrom;
            dataRow["PRODUCT_DESCRIPTION"] = companyPolicy.Product.Description;
            dataRow["TEMP_TYPE_CD"] = companyPolicy.TemporalType;
            dataRow["PRODUCT_FORM_NUMBER"] = FromNumber;
            dataRow["BUSINESS_TYPE_CD"] = companyPolicy.BusinessType.Value;
            dataRow["CROP_RC_POLICY_NUMBER"] = companyPolicy.Endorsement.QuotationId;

            if (companyPolicy.BillingGroup != null)
            {
                dataRow["BILLING_GROUP"] = "C-" + companyPolicy.BillingGroup.Id.ToString();
            }
            if (userName != null)
            {
                dataRow["USER_NAME"] = userName;
            }
            dataRow["AGREED_PAYMENT_METHOD"] = companyPolicy.PaymentPlan.Description;

            CO_TMP_POLICY_COVER.Rows.Add(dataRow);

            return CO_TMP_POLICY_COVER;
        }

        private DataTable SetDataCO_POLICY_AGENT(List<IssuanceAgency> agencies)
        {
            DataTable CO_POLICY_AGENT = DatasetHelper.CreateCO_POLICY_AGENT();

            foreach (IssuanceAgency agency in agencies)
            {
                DataRow dataRow = CO_POLICY_AGENT.NewRow();

                dataRow["PROCESS_ID"] = 1;
                dataRow["AGENT_CODE"] = agency.Code;
                dataRow["AGENT_NAME"] = agency.Agent.FullName;
                dataRow["PARTICIPATION"] = agency.Participation;
                dataRow["IS_MAIN"] = agency.IsPrincipal;

                CO_POLICY_AGENT.Rows.Add(dataRow);
            }

            return CO_POLICY_AGENT;
        }

        private DataTable SetDataCO_TMP_POLICY_RISK_COLLECTIVE(CompanyCoverage coverage)
        {
            DataTable dtCoverage = DatasetHelper.CreateCO_TMP_POLICY_RISK_COLLECTIVE();
            DataRow dataRow = dtCoverage.NewRow();

            dataRow["CURRENT_FROM"] = coverage.CurrentFrom;
            dataRow["CURRENT_TO"] = coverage.CurrentTo;

            dtCoverage.Rows.Add(dataRow);

            return dtCoverage;
        }

        private DataTable SetDataCO_TMP_POLICY_RISK(CompanyPolicy companyPolicy, CompanyRisk risk)
        {
            DataTable CO_TMP_POLICY_RISK = DatasetHelper.CreateCO_TMP_POLICY_RISK();
            DataRow dataRow = CO_TMP_POLICY_RISK.NewRow();

            dataRow["PROCESS_ID"] = 1;
            dataRow["RISK_NUM"] = 1;
            dataRow["INSURED_NAME"] = risk.MainInsured.Name;
            dataRow["INSURED_DOC"] = risk.MainInsured.IdentificationDocument.Number;
            dataRow["INSURED_ID"] = risk.MainInsured.IndividualId;
            dataRow["LIMIT_AMT"] = companyPolicy.Summary == null ? 0 : companyPolicy.Summary.AmountInsured;
            dataRow["PREMIUM_AMT"] = companyPolicy.Summary == null ? 0 : companyPolicy.Summary.Premium;
            dataRow["EXPENSES"] = companyPolicy.Summary == null ? 0 : companyPolicy.Summary.Expenses;
            dataRow["TAX"] = companyPolicy.Summary == null ? 0 : companyPolicy.Summary.Taxes;
            dataRow["BENEFICIARY_NAME"] = risk.Beneficiaries[0].Name;
            dataRow["BENEFICIARY_DOC"] = risk.Beneficiaries[0].IdentificationDocument?.Number;
            dataRow["BENEFICIARY_ID"] = risk.Beneficiaries[0].IndividualId;

            if (risk.Text != null && !string.IsNullOrEmpty(risk.Text.TextBody))
            {
                dataRow["CONDITION_TEXT"] = risk.Text.TextBody;
            }

            CO_TMP_POLICY_RISK.Rows.Add(dataRow);

            return CO_TMP_POLICY_RISK;
        }
        private DataTable CO_TMP_POLICY_RISK(CompanyPolicy companyPolicy, CompanyRisk risk)
        {
            DataTable CO_TMP_POLICY_RISK = DatasetHelper.CreateCO_TMP_POLICY_RISK();
            DataRow dataRow = CO_TMP_POLICY_RISK.NewRow();

            dataRow["PROCESS_ID"] = 1;
            dataRow["RISK_NUM"] = 1;
            dataRow["INSURED_NAME"] = risk.MainInsured.Name;
            dataRow["INSURED_DOC"] = risk.MainInsured.IdentificationDocument.Number;
            dataRow["INSURED_ID"] = risk.MainInsured.IndividualId;
            dataRow["LIMIT_AMT"] = risk.AmountInsured;
            dataRow["PREMIUM_AMT"] = risk.Premium;
            dataRow["EXPENSES"] = companyPolicy.Summary == null ? 0 : companyPolicy.Summary.Expenses;
            dataRow["TAX"] = companyPolicy.Summary == null ? 0 : companyPolicy.Summary.Taxes;
            dataRow["BENEFICIARY_NAME"] = risk.Beneficiaries[0].Name;
            dataRow["BENEFICIARY_DOC"] = risk.Beneficiaries[0].IdentificationDocument?.Number;
            dataRow["BENEFICIARY_ID"] = risk.Beneficiaries[0].IndividualId;

            if (risk.Text != null && !string.IsNullOrEmpty(risk.Text.TextBody))
            {
                dataRow["CONDITION_TEXT"] = risk.Text.TextBody;
            }

            CO_TMP_POLICY_RISK.Rows.Add(dataRow);

            return CO_TMP_POLICY_RISK;
        }

        private DataTable CreateCO_TMP_POLICY_RISK_COVERAGE(List<CompanyCoverage> coverages, CompanyPolicy companyPolicy)
        {
            DataTable CO_TMP_POLICY_RISK_COVERAGE = new DataTable("CO_TMP_POLICY_RISK_COVERAGE");
            CO_TMP_POLICY_RISK_COVERAGE = DatasetHelper.CreateCO_TMP_POLICY_RISK_COVERAGE();

            NameValue[] parameters = new NameValue[3];
            parameters[0] = new NameValue("@ENDORSEMENT_TYPE_ID", coverages[0].EndorsementType);
            parameters[1] = new NameValue("@LIMIT_RC_ID", 0);

            DataTable dtCoverages = new DataTable("COVERAGES");
            dtCoverages.Columns.Add("COVERAGE_ID", typeof(int));
            dtCoverages.Columns.Add("COVER_STATUS_CD", typeof(int));
            dtCoverages.Columns.Add("ENDORSEMENT_SUBLIMIT_AMT", typeof(decimal));
            dtCoverages.Columns.Add("DEDUCT_ID", typeof(int));
            dtCoverages.Columns.Add("DEDUCT_VALUE", typeof(decimal));
            dtCoverages.Columns.Add("MIN_DEDUCT_VALUE", typeof(decimal));
            dtCoverages.Columns.Add("DEDUCT_UNIT_CD", typeof(int));
            dtCoverages.Columns.Add("DEDUCT_SUBJECT_CD", typeof(int));
            dtCoverages.Columns.Add("MIN_DEDUCT_UNIT_CD", typeof(int));
            dtCoverages.Columns.Add("CURRENT_FROM", typeof(DateTime));
            dtCoverages.Columns.Add("CURRENT_TO", typeof(DateTime));

            foreach (CompanyCoverage coverage in coverages)
            {
                DataRow dataRow = dtCoverages.NewRow();
                dataRow["COVERAGE_ID"] = coverage.Id;
                dataRow["COVER_STATUS_CD"] = coverage.CoverStatus;
                dataRow["ENDORSEMENT_SUBLIMIT_AMT"] = coverage.EndorsementSublimitAmount;

                if (coverage.Deductible != null && coverage.Deductible.Id > 0)
                {
                    dataRow["DEDUCT_ID"] = coverage.Deductible.Id;
                    dataRow["DEDUCT_VALUE"] = coverage.Deductible.DeductValue;
                    dataRow["MIN_DEDUCT_VALUE"] = coverage.Deductible.MinDeductValue;

                    if (coverage.Deductible.DeductibleUnit != null)
                    {
                        dataRow["DEDUCT_UNIT_CD"] = coverage.Deductible.DeductibleUnit.Id;
                    }

                    if (coverage.Deductible.DeductibleSubject != null)
                    {
                        dataRow["DEDUCT_SUBJECT_CD"] = dataRow["DEDUCT_UNIT_CD"] = coverage.Deductible.DeductibleSubject.Id;
                    }

                    if (coverage.Deductible.MinDeductibleUnit != null)
                    {
                        dataRow["MIN_DEDUCT_UNIT_CD"] = coverage.Deductible.MinDeductibleUnit.Id;
                    }
                }

                dtCoverages.Rows.Add(dataRow);
            }

            parameters[2] = new NameValue("@COVERAGES", dtCoverages);

            DataTable dataTable;

            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                dataTable = pdb.ExecuteSPDataTable("REPORT.GET_PRINT_COVERAGES", parameters);
            }

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRowResult in dataTable.Rows)
                {
                    DataRow dataRow = CO_TMP_POLICY_RISK_COVERAGE.NewRow();

                    dataRow["PROCESS_ID"] = 1;
                    dataRow["RISK_NUM"] = 1;
                    dataRow["COVERAGE_NUM"] = dataRowResult[0];
                    dataRow["COVERAGE"] = dataRowResult[1];
                    dataRow["COVERAGE_PREMIUM"] = dataRowResult[2];
                    dataRow["COVERAGE_DEDUCT"] = dataRowResult[3];
                    dataRow["DEDUCT_VALUE"] = dataRowResult[4];
                    dataRow["MIN_DEDUCT_VALUE"] = dataRowResult[5];
                    dataRow["INSURED_OBJECT_DESC"] = dataRowResult[6];
                    dataRow["LNB_DESC"] = dataRowResult[7];
                    dataRow["SUB_LINE_BUSINESS_DESC"] = dataRowResult[8];

                    CO_TMP_POLICY_RISK_COVERAGE.Rows.Add(dataRow);
                }
            }

            return CO_TMP_POLICY_RISK_COVERAGE;
        }

        private DataTable SetDataCO_TMP_POLICY_RISK_COVERAGE(List<CompanyCoverage> coverages, int limitRcId)
        {
            DataTable CO_TMP_POLICY_RISK_COVERAGE = new DataTable("CO_TMP_POLICY_RISK_COVERAGE");
            CO_TMP_POLICY_RISK_COVERAGE = DatasetHelper.CreateCO_TMP_POLICY_RISK_COVERAGE();


            NameValue[] parameters = new NameValue[3];
            parameters[0] = new NameValue("@ENDORSEMENT_TYPE_ID", coverages[0].EndorsementType);
            parameters[1] = new NameValue("@LIMIT_RC_ID", limitRcId);


            DataTable dtCoverages = new DataTable("COVERAGES");
            dtCoverages.Columns.Add("COVERAGE_ID", typeof(int));
            dtCoverages.Columns.Add("COVER_STATUS_CD", typeof(int));
            dtCoverages.Columns.Add("ENDORSEMENT_SUBLIMIT_AMT", typeof(decimal));
            dtCoverages.Columns.Add("DEDUCT_ID", typeof(int));
            dtCoverages.Columns.Add("DEDUCT_VALUE", typeof(decimal));
            dtCoverages.Columns.Add("MIN_DEDUCT_VALUE", typeof(decimal));
            dtCoverages.Columns.Add("DEDUCT_UNIT_CD", typeof(int));
            dtCoverages.Columns.Add("DEDUCT_SUBJECT_CD", typeof(int));
            dtCoverages.Columns.Add("MIN_DEDUCT_UNIT_CD", typeof(int));
            dtCoverages.Columns.Add("CURRENT_FROM", typeof(DateTime));
            dtCoverages.Columns.Add("CURRENT_TO", typeof(DateTime));

            foreach (CompanyCoverage coverage in coverages)
            {

                DataRow dataRow = dtCoverages.NewRow();
                dataRow["COVERAGE_ID"] = coverage.Id;
                dataRow["COVER_STATUS_CD"] = coverage.CoverStatus;
                dataRow["ENDORSEMENT_SUBLIMIT_AMT"] = coverage.EndorsementSublimitAmount;
                dataRow["CURRENT_TO"] = coverage.CurrentTo;
                dataRow["CURRENT_FROM"] = coverage.CurrentFrom;

                if (coverage.Deductible != null && coverage.Deductible.Id > 0)
                {
                    dataRow["DEDUCT_ID"] = coverage.Deductible.Id;
                    dataRow["DEDUCT_VALUE"] = coverage.Deductible.DeductValue;
                    dataRow["MIN_DEDUCT_VALUE"] = coverage.Deductible.MinDeductValue;

                    if (coverage.Deductible.DeductibleUnit != null)
                    {
                        dataRow["DEDUCT_UNIT_CD"] = coverage.Deductible.DeductibleUnit.Id;
                    }

                    if (coverage.Deductible.DeductibleSubject != null)
                    {
                        dataRow["DEDUCT_SUBJECT_CD"] = dataRow["DEDUCT_UNIT_CD"] = coverage.Deductible.DeductibleSubject.Id;
                    }

                    if (coverage.Deductible.MinDeductibleUnit != null)
                    {
                        dataRow["MIN_DEDUCT_UNIT_CD"] = coverage.Deductible.MinDeductibleUnit.Id;
                    }
                }

                dtCoverages.Rows.Add(dataRow);
            }

            parameters[2] = new NameValue("@COVERAGES", dtCoverages);

            DataTable dataTable;

            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                dataTable = pdb.ExecuteSPDataTable("REPORT.GET_PRINT_COVERAGES", parameters);
            }

            if (dataTable != null && dataTable.Rows.Count > 0)
            {

                foreach (DataRow dataRowResult in dataTable.Rows)
                {
                    DataRow dataRowCoverage = CO_TMP_POLICY_RISK_COVERAGE.NewRow();
                    dataRowCoverage["PROCESS_ID"] = 1;
                    dataRowCoverage["RISK_NUM"] = 1;
                    dataRowCoverage["COVERAGE_NUM"] = dataRowResult[0];
                    dataRowCoverage["COVERAGE"] = dataRowResult[1];
                    dataRowCoverage["COVERAGE_PREMIUM"] = dataRowResult[2];
                    dataRowCoverage["COVERAGE_DEDUCT"] = dataRowResult[3];
                    dataRowCoverage["DEDUCT_VALUE"] = dataRowResult[4];
                    dataRowCoverage["MIN_DEDUCT_VALUE"] = dataRowResult[5];
                    dataRowCoverage["CURRENT_TO"] = dataRowResult[6];
                    dataRowCoverage["CURRENT_FROM"] = dataRowResult[7];

                    CO_TMP_POLICY_RISK_COVERAGE.Rows.Add(dataRowCoverage);

                }
            }

            return CO_TMP_POLICY_RISK_COVERAGE;
        }

        private DataTable SetDataCO_TMP_POLICY_COINSURANCE(CompanyPolicy companyPolicy)
        {
            DataTable CO_TMP_POLICY_COINSURANCE = DatasetHelper.CreateCO_TMP_POLICY_COINSURANCE();

            if (companyPolicy.BusinessType != BusinessType.CompanyPercentage)
            {
                foreach (IssuanceCoInsuranceCompany item in companyPolicy.CoInsuranceCompanies)
                {
                    DataRow drCoInsurance = CO_TMP_POLICY_COINSURANCE.NewRow();

                    drCoInsurance["PROCESS_ID"] = companyPolicy.Id;
                    drCoInsurance["INSURANCE_COMPANY_ID"] = item.Id;
                    drCoInsurance["INSURANCE_COMPANY_DESC"] = item.Description;
                    drCoInsurance["PART_CIA_PCT"] = item.ParticipationPercentage;
                    drCoInsurance["IS_MAIN_COMPANY"] = true;

                    CO_TMP_POLICY_COINSURANCE.Rows.Add(drCoInsurance);
                }
            }

            return CO_TMP_POLICY_COINSURANCE;
        }

        private DataTable CreateCO_TMP_POLICY_TEXT(CompanyPolicy companyPolicy)
        {
            DataTable CO_TMP_POLICY_TEXT = new DataTable("CO_TMP_POLICY_TEXT");
            CO_TMP_POLICY_TEXT = DatasetHelper.CreateCO_TMP_POLICY_TEXT();

            DataRow drText = CO_TMP_POLICY_TEXT.NewRow();
            if (companyPolicy.Text != null)
            {
                drText["PROCESS_ID"] = companyPolicy.Id;
                drText["COMPLETE_TEXT"] = companyPolicy.Text?.TextBody;
                drText["FIRST_PAGE_TEXT"] = 1;
                drText["SECOND_PAGE_TEXT"] = 1;
            }
            CO_TMP_POLICY_TEXT.Rows.Add(drText);

            return CO_TMP_POLICY_TEXT;
        }

        private DataTable CreateCO_TMP_POLICY_RISK_DETAIL(CompanyVehicle companyVehicle, CompanyPolicy companyPolicy)
        {
            DataTable dtCompanyRiskDatail = DatasetHelper.CreateCO_TMP_POLICY_RISK_DETAIL();

            if (companyVehicle.Accesories != null)
            {
                int process = 0;
                foreach (Accessory accessory in companyVehicle.Accesories)
                {

                    if (process != companyPolicy.Id)
                    {
                        DataRow drRiskDatail = dtCompanyRiskDatail.NewRow();

                        drRiskDatail["PROCESS_ID"] = companyPolicy.Id;
                        drRiskDatail["RISK_NUM"] = 1;
                        drRiskDatail["DETAIL"] = accessory.Description;

                        dtCompanyRiskDatail.Rows.Add(drRiskDatail);
                        process = companyPolicy.Id;
                    }
                }
            }

            return dtCompanyRiskDatail;
        }

        private DataTable SetDataBUSINESS_TYPE(CompanyPolicy companyPolicy)
        {
            DataTable dtBusinessType = DatasetHelper.CreateBUSINESS_TYPE();
            DataRow dataRow = dtBusinessType.NewRow();

            dataRow["BUSINESS_TYPE_CD"] = companyPolicy.BusinessType.Value;
            dtBusinessType.Rows.Add(dataRow);

            return dtBusinessType;
        }

        private DataTable SetDataCO_POLICY_REPORT_DETAIL_STRING(List<CompanyAccessory> accesories)
        {
            DataTable dtAccesories = DatasetHelper.CreateCO_POLICY_REPORT_DETAIL_STRING();

            if (accesories != null && accesories.Count > 0)
            {
                foreach (CompanyAccessory accessory in accesories)
                {
                    DataRow dataRow = dtAccesories.NewRow();

                    dataRow["PROCESS_ID"] = 1;
                    dataRow["RISK_NUM"] = 1;
                    dataRow["DETAIL"] = accessory.Description;

                    dtAccesories.Rows.Add(dataRow);
                }
            }

            return dtAccesories;
        }

        private DataTable CreateCO_POLICY_Payer(CompanyPolicy companyPolicy)
        {
            DataTable CO_POLICY_Payer = new DataTable("CO_POLICY_Payer");
            CO_POLICY_Payer = DatasetHelper.CreateCO_POLICY_PAYER();

            if (companyPolicy.PaymentPlan.Quotas != null && companyPolicy.PaymentPlan.Quotas.Count > 0)
            {
                DataRow dataRow = CO_POLICY_Payer.NewRow();

                dataRow["PROCESS_ID"] = companyPolicy.Id;
                dataRow["PAYMENT_DATE"] = companyPolicy.PaymentPlan.Quotas[0].ExpirationDate;
                dataRow["PAYMENT_AMOUNT"] = companyPolicy.PaymentPlan.Quotas[0].Amount;

                CO_POLICY_Payer.Rows.Add(dataRow);
            }

            return CO_POLICY_Payer;
        }

        private DataTable CreateCLAUSE(List<Clause> clause, CompanyPolicy companyPolicy)
        {
            DataTable CLAUSE = new DataTable("CLAUSE");
            CLAUSE = DatasetHelper.CreateCLAUSE();

            foreach (Clause item in clause)
            {
                DataRow drClauses = CLAUSE.NewRow();

                drClauses["CLAUSE_ID"] = item.Id;
                drClauses["CLAUSE_NAME"] = item.Name;
                drClauses["CLAUSE_TITLE"] = item.Title;
                drClauses["CURRENT_FROM"] = companyPolicy.CurrentFrom;
                drClauses["CURRENT_TO"] = companyPolicy.CurrentTo;
                drClauses["CLAUSE_TEXT"] = item.Text;
                drClauses["CONDITION_LEVEL_CD"] = 1;
                CLAUSE.Rows.Add(drClauses);
            }
            return CLAUSE;
        }

        private DataTable CreateCO_TMP_POLICY_LOCATION(CompanyRisk risk)
        {
            DataTable CO_TMP_POLICY_LOCATION = new DataTable("CO_TMP_POLICY_LOCATION");
            CO_TMP_POLICY_LOCATION = DatasetHelper.CreateCO_TMP_POLICY_LOCATION();

            DataRow drLocation = CO_TMP_POLICY_LOCATION.NewRow();

            drLocation["PROCESS_ID"] = risk.Id;
            drLocation["RISK_NUM"] = 1;
            drLocation["LOCATION_ADDRESS"] = risk.Description;

            CO_TMP_POLICY_LOCATION.Rows.Add(drLocation);

            return CO_TMP_POLICY_LOCATION;
        }

        private DataTable CreateCO_TMP_POLICY_RISK_BENEFICIARY(CompanyRisk companyRisk, CompanyPolicy companyPolicy, List<CompanyBeneficiary> LtsBeneficiary)
        {
            DataTable CO_TMP_POLICY_RISK_BENEFICIARY = new DataTable("CO_TMP_POLICY_RISK_BENEFICIARY");
            CO_TMP_POLICY_RISK_BENEFICIARY = DatasetHelper.CreateCO_TMP_POLICY_RISK_BENEFICIARY();

            if (LtsBeneficiary != null)
            {
                foreach (CompanyBeneficiary beneficiary in LtsBeneficiary)
                {
                    DataRow dataRow = CO_TMP_POLICY_RISK_BENEFICIARY.NewRow();
                    dataRow["PROCESS_ID"] = companyPolicy.Id;
                    dataRow["RISK_NUM"] = 1;
                    dataRow["BENEFICIARY_NAME"] = beneficiary.Name;
                    dataRow["BENEFICIARY_ADD"] = beneficiary.CompanyName.Address.Description + "," + beneficiary.CompanyName.Address.City?.Description + "," + beneficiary.CompanyName.Address.City?.State?.Description;
                    dataRow["BENEFICIARY_DOC_TYPE"] = beneficiary.IdentificationDocument.DocumentType?.Description;
                    dataRow["BENEFICIARY_DOC"] = beneficiary.IdentificationDocument.Number;
                    if (beneficiary.CompanyName.Phone?.Description != null)
                        dataRow["BENEFICIARY_PHONE"] = beneficiary.CompanyName.Phone.Description;
                    dataRow["BENEFICIARY_ID"] = beneficiary.IndividualId;
                    CO_TMP_POLICY_RISK_BENEFICIARY.Rows.Add(dataRow);
                }
            }

            return CO_TMP_POLICY_RISK_BENEFICIARY;
        }

        #endregion
    }
}