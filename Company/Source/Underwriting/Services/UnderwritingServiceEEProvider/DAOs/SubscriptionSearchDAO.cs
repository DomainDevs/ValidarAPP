using Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs;
using Sistran.Core.Application.Utilities.Error;
using System.Collections.Generic;
using UTILMO = Sistran.Core.Services.UtilitiesServices.Models;
using System.Linq;
using System;
using System.Diagnostics;
using Sistran.Co.Application.Data;
using System.Data;
using System.Threading.Tasks;
using Sistran.Company.Application.UnderwritingServices.Models;
using UTILEN = Sistran.Core.Services.UtilitiesServices.Enums;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider.DAOs
{
    /// <summary>
    /// Dao para modulo de busqueda
    /// </summary>
    public class SubscriptionSearchDAO
    {
        public List<CompanyQuotationSearch> SearchQuotations(CompanySubscriptionSearch companySubscriptionSearch)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            NameValue[] parameters = new NameValue[11];

            if (companySubscriptionSearch.InsuredId != null)
            {
                parameters[0] = new NameValue("INSURED_ID", companySubscriptionSearch.InsuredId);
            }
            else
            {
                parameters[0] = new NameValue("INSURED_ID", DBNull.Value, DbType.Int32);
            }

            if (companySubscriptionSearch.AgentPrincipalId != null)
            {
                parameters[1] = new NameValue("INTERMEDIARY_ID", companySubscriptionSearch.AgentPrincipalId);
            }
            else
            {
                parameters[1] = new NameValue("INTERMEDIARY_ID", DBNull.Value, DbType.Int32);
            }

            if (companySubscriptionSearch.AgentAgency != null)
            {
                parameters[2] = new NameValue("AGENT_ID", companySubscriptionSearch.AgentAgency);
            }
            else
            {
                parameters[2] = new NameValue("AGENT_ID", DBNull.Value, DbType.Int32);
            }

            if (companySubscriptionSearch.PrefixId != null)
            {
                parameters[3] = new NameValue("PREFIX_ID", companySubscriptionSearch.PrefixId);
            }
            else
            {
                parameters[3] = new NameValue("PREFIX_ID", DBNull.Value, DbType.Int32);
            }

            if (companySubscriptionSearch.QuotationNumber != null)
            {
                parameters[4] = new NameValue("QUOTATION_NUMBER", companySubscriptionSearch.QuotationNumber);
            }
            else
            {
                parameters[4] = new NameValue("QUOTATION_NUMBER", DBNull.Value, DbType.Int32);
            }

            if (companySubscriptionSearch.Version != null)
            {
                parameters[5] = new NameValue("VERSION", companySubscriptionSearch.Version);
            }
            else
            {
                parameters[5] = new NameValue("VERSION", DBNull.Value, DbType.Decimal);
            }

            if (companySubscriptionSearch.IssueDate != null)
            {
                parameters[6] = new NameValue("ISSUE_DATE", companySubscriptionSearch.IssueDate);
            }
            else
            {
                parameters[6] = new NameValue("ISSUE_DATE", DBNull.Value, DbType.DateTime);
            }

            if (companySubscriptionSearch.Plate != null)
            {
                parameters[7] = new NameValue("PLATE", companySubscriptionSearch.Plate);
            }
            else
            {
                parameters[7] = new NameValue("PLATE", DBNull.Value, DbType.String);
            }

            if (companySubscriptionSearch.Engine != null)
            {
                parameters[8] = new NameValue("ENGINE", companySubscriptionSearch.Engine);
            }
            else
            {
                parameters[8] = new NameValue("ENGINE", DBNull.Value, DbType.String);
            }

            if (companySubscriptionSearch.Chassis != null)
            {
                parameters[9] = new NameValue("CHASSIS", companySubscriptionSearch.Chassis);
            }
            else
            {
                parameters[9] = new NameValue("CHASSIS", DBNull.Value, DbType.String);
            }

            if (companySubscriptionSearch.UserId != null)
            {
                parameters[10] = new NameValue("USER_ID", companySubscriptionSearch.UserId);
            }
            else
            {
                parameters[10] = new NameValue("USER_ID", DBNull.Value, DbType.Int32);
            }

            DataTable result;
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("QUO.SEARCH_QUOTATIONS", parameters);
            }

            if (result != null && result.Rows.Count > 0)
            {
                List<CompanyQuotationSearch> listCompanyQuotationSearch = new List<CompanyQuotationSearch>();

                foreach (DataRow dataRow in result.Rows)
                {
                    CompanyQuotationSearch companyQuotationSearch = new CompanyQuotationSearch();
                    companyQuotationSearch.QuotationNumber = Convert.ToInt32(dataRow[0].ToString());
                    companyQuotationSearch.Version = Convert.ToInt32(dataRow[1].ToString());
                    companyQuotationSearch.PrefixCommercial = dataRow[2].ToString();
                    companyQuotationSearch.Insured = dataRow[3].ToString();
                    companyQuotationSearch.Branch = dataRow[4].ToString();
                    companyQuotationSearch.CurrencyIssuance = dataRow[5].ToString();
                    companyQuotationSearch.TotalPremium = dataRow[6].ToString();
                    companyQuotationSearch.User = dataRow[7].ToString();
                    companyQuotationSearch.Date = Convert.ToDateTime(dataRow[8].ToString());
                    companyQuotationSearch.Days = Convert.ToInt32(dataRow[9].ToString());
                    companyQuotationSearch.AgentPrincipal = dataRow[10].ToString();
                    companyQuotationSearch.Product = dataRow[11].ToString();
                    companyQuotationSearch.OperationId = Convert.ToInt32(dataRow[12].ToString());

                    listCompanyQuotationSearch.Add(companyQuotationSearch);
                }
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.SearchQuotations");
                return listCompanyQuotationSearch;
            }
            else
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.SearchQuotations");
                return new List<CompanyQuotationSearch>();
            }

        }

        public List<CompanyPolicySearch> SearchPolicies(CompanySubscriptionSearch companySubscriptionSearch)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            NameValue[] parameters = new NameValue[13];

            if (companySubscriptionSearch.InsuredId != null)
            {
                parameters[0] = new NameValue("INSURED_ID", companySubscriptionSearch.InsuredId);
            }
            else
            {
                parameters[0] = new NameValue("INSURED_ID", DBNull.Value, DbType.Int32);
            }

            if (companySubscriptionSearch.AgentPrincipalId != null)
            {
                parameters[1] = new NameValue("INTERMEDIARY_ID", companySubscriptionSearch.AgentPrincipalId);
            }
            else
            {
                parameters[1] = new NameValue("INTERMEDIARY_ID", DBNull.Value, DbType.Int32);
            }

            if (companySubscriptionSearch.AgentAgency != null)
            {
                parameters[2] = new NameValue("AGENT_ID", companySubscriptionSearch.AgentAgency);
            }
            else
            {
                parameters[2] = new NameValue("AGENT_ID", DBNull.Value, DbType.Int32);
            }

            if (companySubscriptionSearch.PrefixId != null)
            {
                parameters[3] = new NameValue("PREFIX_ID", companySubscriptionSearch.PrefixId);
            }
            else
            {
                parameters[3] = new NameValue("PREFIX_ID", DBNull.Value, DbType.Int32);
            }

            if (companySubscriptionSearch.BranchId != null)
            {
                parameters[4] = new NameValue("BRANCH_ID", companySubscriptionSearch.BranchId);
            }
            else
            {
                parameters[4] = new NameValue("BRANCH_ID", DBNull.Value, DbType.Int32);
            }

            if (companySubscriptionSearch.PolicyNumber != null)
            {
                parameters[5] = new NameValue("POLICY_NUMBER", companySubscriptionSearch.PolicyNumber);
            }
            else
            {
                parameters[5] = new NameValue("POLICY_NUMBER", DBNull.Value, DbType.Decimal);
            }

            if (companySubscriptionSearch.EndorsementId != null)
            {
                parameters[6] = new NameValue("ENDORSEMENT_ID", companySubscriptionSearch.EndorsementId);
            }
            else
            {
                parameters[6] = new NameValue("ENDORSEMENT_ID", DBNull.Value, DbType.Decimal);
            }

            if (companySubscriptionSearch.IssueDate != null)
            {
                parameters[7] = new NameValue("ISSUE_DATE", companySubscriptionSearch.IssueDate);
            }
            else
            {
                parameters[7] = new NameValue("ISSUE_DATE", DBNull.Value, DbType.DateTime);
            }

            if (companySubscriptionSearch.Plate != null)
            {
                parameters[8] = new NameValue("PLATE", companySubscriptionSearch.Plate);
            }
            else
            {
                parameters[8] = new NameValue("PLATE", DBNull.Value, DbType.String);
            }

            if (companySubscriptionSearch.Engine != null)
            {
                parameters[9] = new NameValue("ENGINE", companySubscriptionSearch.Engine);
            }
            else
            {
                parameters[9] = new NameValue("ENGINE", DBNull.Value, DbType.String);
            }

            if (companySubscriptionSearch.Chassis != null)
            {
                parameters[10] = new NameValue("CHASSIS", companySubscriptionSearch.Chassis);
            }
            else
            {
                parameters[10] = new NameValue("CHASSIS", DBNull.Value, DbType.String);
            }

            if (companySubscriptionSearch.UserId != null)
            {
                parameters[11] = new NameValue("USER_ID", companySubscriptionSearch.UserId);
            }
            else
            {
                parameters[11] = new NameValue("USER_ID", DBNull.Value, DbType.Int32);
            }
            if (companySubscriptionSearch.HolderId != null)
            {
                parameters[12] = new NameValue("HOLDER_ID", companySubscriptionSearch.HolderId);
            }
            else
            {
                parameters[12] = new NameValue("HOLDER_ID", DBNull.Value, DbType.Int32);
            }

            DataTable result;

            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("QUO.SEARCH_POLICIES", parameters);
            }

            if (result != null && result.Rows.Count > 0)
            {
                List<CompanyPolicySearch> listCompanyPolicySearch = new List<CompanyPolicySearch>();

                foreach (DataRow dataRow in result.Rows)
                {
                    CompanyPolicySearch companyPolicySearch = new CompanyPolicySearch();
                    companyPolicySearch.PolicyNumber = Convert.ToInt32(dataRow[0].ToString());
                    companyPolicySearch.EndorsementId = Convert.ToInt32(dataRow[1].ToString());
                    companyPolicySearch.PrefixCommercial = dataRow[2].ToString();
                    companyPolicySearch.EndorsementType = dataRow[3].ToString();
                    companyPolicySearch.Insured = dataRow[4].ToString();
                    companyPolicySearch.Branch = dataRow[5].ToString();
                    companyPolicySearch.IssueCurrency = dataRow[6].ToString();
                    companyPolicySearch.TotalPremium = dataRow[7].ToString();
                    companyPolicySearch.User = dataRow[8].ToString();
                    companyPolicySearch.IssueDate = Convert.ToDateTime(dataRow[9].ToString());
                    companyPolicySearch.AgentPrincipal = dataRow[10].ToString();
                    companyPolicySearch.Product = dataRow[11].ToString();

                    listCompanyPolicySearch.Add(companyPolicySearch);
                }
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.SearchPolicies");
                return listCompanyPolicySearch;
            }
            else
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.SearchPolicies");
                return new List<CompanyPolicySearch>();
            }
        }

        public List<CompanyTemporalSearch> SearchTemporals(CompanySubscriptionSearch companySubscriptionSearch)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            NameValue[] parameters = new NameValue[4];

            if (companySubscriptionSearch.InsuredId != null)
            {
                parameters[0] = new NameValue("INSURED_ID", companySubscriptionSearch.InsuredId);
            }
            else
            {
                parameters[0] = new NameValue("INSURED_ID", DBNull.Value, DbType.Int32);
            }

            if (companySubscriptionSearch.TemporaryNumber != null)
            {
                parameters[1] = new NameValue("TEMPORARY_NUMBER", companySubscriptionSearch.TemporaryNumber);
            }
            else
            {
                parameters[1] = new NameValue("TEMPORARY_NUMBER", DBNull.Value, DbType.Int32);
            }

            if (companySubscriptionSearch.UserId != null)
            {
                parameters[2] = new NameValue("USER_ID", companySubscriptionSearch.UserId);
            }
            else
            {
                parameters[2] = new NameValue("USER_ID", DBNull.Value, DbType.Int32);
            }

            if (companySubscriptionSearch.IssueDate != null)
            {
                parameters[3] = new NameValue("ISSUE_DATE", companySubscriptionSearch.IssueDate);
            }
            else
            {
                parameters[3] = new NameValue("ISSUE_DATE", DBNull.Value, DbType.DateTime);
            }

            DataTable result;

            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("QUO.SEARCH_TEMPORALS", parameters);
            }

            if (result != null && result.Rows.Count > 0)
            {
                List<CompanyTemporalSearch> listCompanyTemporalSearch = new List<CompanyTemporalSearch>();

                foreach (DataRow dataRow in result.Rows)
                {
                    CompanyTemporalSearch companyTemporalSearch = new CompanyTemporalSearch();
                    companyTemporalSearch.NumberTemporary = dataRow[0].ToString();
                    companyTemporalSearch.PolicyNumber = dataRow[1].ToString();
                    companyTemporalSearch.PrefixCommercial = dataRow[2].ToString();
                    companyTemporalSearch.Insured = dataRow[3].ToString();
                    companyTemporalSearch.Branch = dataRow[4].ToString();
                    companyTemporalSearch.User = dataRow[5].ToString();
                    companyTemporalSearch.ConsultationDate = Convert.ToDateTime(dataRow[6].ToString());
                    companyTemporalSearch.Days = Convert.ToInt32(dataRow[7].ToString());
                    companyTemporalSearch.AgentPrincipal = dataRow[8].ToString();
                    companyTemporalSearch.TypeTransaction = dataRow[9].ToString();

                    listCompanyTemporalSearch.Add(companyTemporalSearch);
                }
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.SearchTemporals");
                return listCompanyTemporalSearch;
            }
            else
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.SearchTemporals");
                return new List<CompanyTemporalSearch>();
            }

        }

        /// <summary>
        /// Genera el archivo excel al buscar cotizaciones
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <returns>Ruta del archivo</returns>
        public Result<string, ErrorModel> GenerateQuotation(string fileName, CompanySubscriptionSearch companySubscriptionSearch)
        {
            try
            {
                UTILMO.FileProcessValue fileProcessValue = new UTILMO.FileProcessValue();
                fileProcessValue.Key1 = (int)UTILEN.FileProcessType.QuotationSearch;

                FileDAO fileDAO = new FileDAO();
                UTILMO.File file = fileDAO.GetFileByFileProcessValue(fileProcessValue);

                if (file != null && file.IsEnabled)
                {
                    file.Name = fileName;
                    List<UTILMO.Row> rows = new List<UTILMO.Row>();
                    List<CompanyQuotationSearch> listCompanyQuotationSearch = SearchQuotations(companySubscriptionSearch);

                    foreach (CompanyQuotationSearch companyQuotationSearch in listCompanyQuotationSearch)
                    {
                        List<UTILMO.Field> fields = file.Templates[0].Rows[0].Fields.Select(x => new UTILMO.Field
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

                        fields[0].Value = companyQuotationSearch.QuotationNumber.ToString();
                        fields[1].Value = companyQuotationSearch.Version.ToString();
                        fields[2].Value = companyQuotationSearch.PrefixCommercial;
                        fields[3].Value = companyQuotationSearch.Insured;
                        fields[4].Value = companyQuotationSearch.Branch;
                        fields[5].Value = companyQuotationSearch.CurrencyIssuance;
                        fields[6].Value = companyQuotationSearch.TotalPremium;
                        fields[7].Value = companyQuotationSearch.User;
                        fields[8].Value = companyQuotationSearch.Date.ToString();
                        fields[9].Value = companyQuotationSearch.Days.ToString();
                        fields[10].Value = companyQuotationSearch.AgentPrincipal;
                        fields[11].Value = companyQuotationSearch.Product;

                        rows.Add(new UTILMO.Row
                        {
                            Fields = fields
                        });
                    }

                    file.Templates[0].Rows = rows;
                    file.Name = string.Format(fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy"));
                    return new ResultValue<string, ErrorModel>(fileDAO.GenerateFile(file));
                }
                else
                {
                    return new ResultValue<string, ErrorModel>(string.Empty);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Genera el archivo excel al buscar Polizas
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <returns>Ruta del archivo</returns>
        public Result<string, ErrorModel> GeneratePolicy(string fileName, CompanySubscriptionSearch companySubscriptionSearch)
        {
            try
            {
                UTILMO.FileProcessValue fileProcessValue = new UTILMO.FileProcessValue();
                fileProcessValue.Key1 = (int)UTILEN.FileProcessType.PolicySearch;

                FileDAO fileDAO = new FileDAO();
                UTILMO.File file = fileDAO.GetFileByFileProcessValue(fileProcessValue);

                if (file != null && file.IsEnabled)
                {
                    file.Name = fileName;
                    List<UTILMO.Row> rows = new List<UTILMO.Row>();
                    List<CompanyPolicySearch> listCompanyPolicySearch = SearchPolicies(companySubscriptionSearch);

                    foreach (CompanyPolicySearch companyPolicySearch in listCompanyPolicySearch)
                    {
                        List<UTILMO.Field> fields = file.Templates[0].Rows[0].Fields.Select(x => new UTILMO.Field
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

                        fields[0].Value = companyPolicySearch.PolicyNumber.ToString();
                        fields[1].Value = companyPolicySearch.EndorsementId.ToString();
                        fields[2].Value = companyPolicySearch.PrefixCommercial;
                        fields[3].Value = companyPolicySearch.EndorsementType;
                        fields[4].Value = companyPolicySearch.Insured;
                        fields[5].Value = companyPolicySearch.Branch;
                        fields[6].Value = companyPolicySearch.IssueCurrency;
                        fields[7].Value = companyPolicySearch.TotalPremium;
                        fields[8].Value = companyPolicySearch.User;
                        fields[9].Value = companyPolicySearch.IssueDate.ToString();
                        fields[10].Value = companyPolicySearch.AgentPrincipal;
                        fields[11].Value = companyPolicySearch.Product;

                        rows.Add(new UTILMO.Row
                        {
                            Fields = fields
                        });
                    }

                    file.Templates[0].Rows = rows;
                    file.Name = string.Format(fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy"));
                    return new ResultValue<string, ErrorModel>(fileDAO.GenerateFile(file));
                }
                else
                {
                    return new ResultValue<string, ErrorModel>(string.Empty);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
