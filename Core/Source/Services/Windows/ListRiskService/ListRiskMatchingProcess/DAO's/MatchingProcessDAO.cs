using ListRiskMatchingProcess.Enum;
using ListRiskMatchingProcess.Model;
using Newtonsoft.Json;
using Sistran.Co.Application.Data;
using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Excel;
using Utilities.Mail;
using IndividualType = ListRiskMatchingProcess.Enum.IndividualType;

namespace ListRiskMatchingProcess.DAO_s
{
    public class MatchingProcessDAO
    {
        #region DATA
        public List<ViewListRiskPerson> GetListRiskView()
        {
            List<ViewListRiskPerson> viewListRiskPeople = new List<ViewListRiskPerson>();
            DataTable result;
            using (DynamicDataAccess pdb = new DynamicDataAccess("SistranEE"))
            {
                result = pdb.ExecuteSPDataTable("UP.GET_VIEW_LIST_RISK_PERSON");
            }
            if (result != null && result.Rows.Count > 0)
            {
                foreach (DataRow item in result.Rows)
                {
                    ViewListRiskPerson ViewListRisk = new ViewListRiskPerson();

                    ViewListRisk.DocumentNumber = item.ItemArray[0] != DBNull.Value ? (string)item.ItemArray[0] : String.Empty;
                    ViewListRisk.FullName = item.ItemArray[1] != DBNull.Value ? (string)item.ItemArray[1] : String.Empty;
                    ViewListRisk.ProcessId = item.ItemArray[2] != DBNull.Value ? (int)item.ItemArray[2] : 0;
                    ViewListRisk.RegistrationDate = item.ItemArray[3] != DBNull.Value ? (DateTime)item.ItemArray[3] : new DateTime();
                    ViewListRisk.RiskListCode = item.ItemArray[4] != DBNull.Value ? (Int16)item.ItemArray[4] : 0;
                    ViewListRisk.Description = item.ItemArray[5] != DBNull.Value ? (string)item.ItemArray[5] : String.Empty;
                    ViewListRisk.RiskListType = item.ItemArray[6] != DBNull.Value ? (string)item.ItemArray[6] : String.Empty;

                    viewListRiskPeople.Add(ViewListRisk);
                }
            }
            return viewListRiskPeople;
        }

        public int GetListRiskUmbral(string listRisk)
        {
            DataTable result;
            int umbral = 60;
            using (DynamicDataAccess pdb = new DynamicDataAccess("SistranEE"))
            {
                result = pdb.ExecuteSPDataTable("UP.GET_LIST_RISK_UMBRAL");
            }
            if (result != null && result.Rows.Count > 0)
            {
                foreach (DataRow item in result.Rows)
                {
                    if ((string)item.ItemArray[1] == listRisk)
                    {
                        umbral = (int)item.ItemArray[4];
                    }
                }
            }
            return umbral;
        }

        public PeopleProcessModel GetSystemPeople()
        {
            DataTable result;
            PeopleProcessModel matchingProcessModel = new PeopleProcessModel();
            matchingProcessModel.Company = new List<CompanyModel>();
            matchingProcessModel.People = new List<PersonModel>();

            using (DynamicDataAccess pdb = new DynamicDataAccess("SistranEE"))
            {
                result = pdb.ExecuteSPDataTable("UP.GET_COMPANY_PERSON");
            }

            if (result != null && result.Rows.Count > 0)
            {
                foreach (DataRow item in result.Rows)
                {
                    if (Convert.ToInt16(item.ItemArray[1]) == (int)IndividualType.Company)
                    {
                        if (item.ItemArray[4] != DBNull.Value && (string)item.ItemArray[4] != "")
                        {
                            CompanyModel companyCompany = new CompanyModel
                            {
                                IndividualId = item.ItemArray[0] != DBNull.Value ? (int)item.ItemArray[0] : 0,
                                DocumentTypeId = item.ItemArray[2] != DBNull.Value ? Convert.ToInt16(item.ItemArray[2]) : 0,
                                DocumentType = item.ItemArray[3] != DBNull.Value && (string)item.ItemArray[3] != "" ? (string)item.ItemArray[3] : "N/A",
                                DocumentNumber = item.ItemArray[4] != DBNull.Value && (string)item.ItemArray[4] != "" ? (string)item.ItemArray[4] : "N/A",
                                TradeName = item.ItemArray[5] != DBNull.Value && (string)item.ItemArray[5] != "" ? (string)item.ItemArray[5] : "N/A"
                            };

                            matchingProcessModel.Company.Add(companyCompany);
                        }
                    }
                    else if (Convert.ToInt16(item.ItemArray[1]) == (int)IndividualType.Person)
                    {
                        if (item.ItemArray[8] != DBNull.Value && (string)item.ItemArray[8] != "")
                        {
                            PersonModel companyPerson = new PersonModel
                            {
                                IndividualId = item.ItemArray[0] != DBNull.Value ? (int)item.ItemArray[0] : 0,
                                DocumentTypeId = item.ItemArray[6] != DBNull.Value ? Convert.ToInt16(item.ItemArray[6]) : 0,
                                DocumentType = item.ItemArray[7] != DBNull.Value && (string)item.ItemArray[7] != "" ? (string)item.ItemArray[7] : "N/A",
                                DocumentNumber = item.ItemArray[8] != DBNull.Value && (string)item.ItemArray[8] != "" ? (string)item.ItemArray[8] : "N/A",
                                SurName = item.ItemArray[9] != DBNull.Value && (string)item.ItemArray[9] != "" ? (string)item.ItemArray[9] : "N/A",
                                SecondSurName = item.ItemArray[10] != DBNull.Value && (string)item.ItemArray[10] != "" ? (string)item.ItemArray[10] : "N/A",
                                Name = item.ItemArray[11] != DBNull.Value && (string)item.ItemArray[11] != "" ? (string)item.ItemArray[11] : "N/A"
                            };

                            matchingProcessModel.People.Add(companyPerson);
                        }
                    }
                }
            }

            return matchingProcessModel;
        }

        public List<PolicyModel> GetSystemPolicies()
        {
            List<PolicyModel> policyModels = new List<PolicyModel>();

            DataTable result;
            using (DynamicDataAccess pdb = new DynamicDataAccess("SistranEE"))
            {
                result = pdb.ExecuteSPDataTable("UP.GET_POLICIES");
            }

            if (result != null && result.Rows.Count > 0)
            {
                foreach (DataRow item in result.Rows)
                {
                    PolicyModel policyModel = new PolicyModel
                    {
                        PolicyId = item.ItemArray[0] != DBNull.Value ? (int)item.ItemArray[0] : 0,
                        DocumentNumber = item.ItemArray[1] != DBNull.Value ? (decimal)item.ItemArray[1] : 0,
                        BranchId = item.ItemArray[2] != DBNull.Value ? Convert.ToInt16(item.ItemArray[2]) : 0,
                        PrefixId = item.ItemArray[3] != DBNull.Value ? Convert.ToInt16(item.ItemArray[3]) : 0,
                        IssueDate = item.ItemArray[4] != DBNull.Value ? (DateTime)item.ItemArray[4] : new DateTime(),
                        PolicyHolderId = item.ItemArray[5] != DBNull.Value ? (int)item.ItemArray[5] : 0,
                        CurrentFrom = item.ItemArray[6] != DBNull.Value ? (DateTime)item.ItemArray[6] : new DateTime(),
                        CurrentTo = item.ItemArray[7] != DBNull.Value ? (DateTime)item.ItemArray[7] : new DateTime(),
                        EndorsementId = item.ItemArray[8] != DBNull.Value ? (int)item.ItemArray[8] : 0,
                        RiskId = item.ItemArray[9] != DBNull.Value ? (int)item.ItemArray[9] : 0,
                        RiskStatusId = item.ItemArray[10] != DBNull.Value ? Convert.ToInt16(item.ItemArray[10]) : 0
                    };

                    policyModels.Add(policyModel);
                }
            }

            return policyModels;
        }

        public List<ClaimModel> GetSystemClaims()
        {
            List<ClaimModel> claimModels = new List<ClaimModel>();

            DataTable result;
            using (DynamicDataAccess pdb = new DynamicDataAccess("SistranEE"))
            {
                result = pdb.ExecuteSPDataTable("UP.GET_CLAIM_ESTIMATION");
            }

            if (result != null && result.Rows.Count > 0)
            {
                foreach (DataRow item in result.Rows)
                {
                    ClaimModel claimModel = new ClaimModel
                    {
                        ClaimCd = item.ItemArray[0] != DBNull.Value ? (int)item.ItemArray[0] : 0,
                        ClaimBranchCd = item.ItemArray[1] != DBNull.Value ? (int)item.ItemArray[1] : 0,
                        PolicyId = item.ItemArray[2] != DBNull.Value ? (int)item.ItemArray[2] : 0,
                        EndorsementId = item.ItemArray[3] != DBNull.Value ? (int)item.ItemArray[3] : 0,
                        IndividualId = item.ItemArray[4] != DBNull.Value ? (int)item.ItemArray[4] : 0,
                        BusinessTypeCd = item.ItemArray[5] != DBNull.Value ? (int)item.ItemArray[5] : 0,
                        ClaimNumber = item.ItemArray[6] != DBNull.Value ? (int)item.ItemArray[6] : 0,
                        PrefixCd = item.ItemArray[7] != DBNull.Value ? (int)item.ItemArray[7] : 0,
                        DocumentNumber = item.ItemArray[8] != DBNull.Value ? (decimal)item.ItemArray[8] : 0,
                        RegistrationDate = item.ItemArray[9] != DBNull.Value ? (DateTime)item.ItemArray[9] : new DateTime(),
                        EstimationTypeCd = item.ItemArray[10] != DBNull.Value ? (int)item.ItemArray[10] : 0,
                        EstimationTypeStatusCd = item.ItemArray[11] != DBNull.Value ? (int)item.ItemArray[11] : 0,
                        EstimationTypeStatusReasonCd = item.ItemArray[12] != DBNull.Value ? (int)item.ItemArray[12] : 0,
                        EstimationAmount = item.ItemArray[13] != DBNull.Value ? (decimal)item.ItemArray[13] : 0,
                        EstimateAmountAccumulate = item.ItemArray[14] != DBNull.Value ? (decimal)item.ItemArray[14] : 0
                    };


                    claimModels.Add(claimModel);
                }
            }

            return claimModels;
        }

        public List<PaymentModel> GetSystemPayment()
        {
            List<PaymentModel> paymentModels = new List<PaymentModel>();

            DataTable result;
            using (DynamicDataAccess pdb = new DynamicDataAccess("SistranEE"))
            {
                result = pdb.ExecuteSPDataTable("UP.GET_PAYMENT_REQUEST");
            }

            if (result != null && result.Rows.Count > 0)
            {
                foreach (DataRow item in result.Rows)
                {
                    PaymentModel paymentModel = new PaymentModel
                    {
                        PaymentRequestId = item.ItemArray[0] != DBNull.Value ? (int)item.ItemArray[0] : 0,
                        PaymentSourceId = item.ItemArray[1] != DBNull.Value ? (int)item.ItemArray[1] : 0,
                        BranchId = item.ItemArray[2] != DBNull.Value ? (int)item.ItemArray[2] : 0,
                        PaymentDate = item.ItemArray[3] != DBNull.Value ? (DateTime)item.ItemArray[3] : new DateTime(),
                        PersonType = item.ItemArray[4] != DBNull.Value ? (int)item.ItemArray[4] : 0,
                        IndividualId = item.ItemArray[5] != DBNull.Value ? (int)item.ItemArray[5] : 0,
                        PaymentMethodId = item.ItemArray[6] != DBNull.Value ? (int)item.ItemArray[6] : 0,
                        TotalAmount = item.ItemArray[7] != DBNull.Value ? (decimal)item.ItemArray[7] : 0,
                        PaymentRequestNumber = item.ItemArray[8] != DBNull.Value ? (int)item.ItemArray[8] : 0,
                        Description = item.ItemArray[9] != DBNull.Value ? (string)item.ItemArray[9] : "",
                        PaymentMovementTypeId = item.ItemArray[10] != DBNull.Value ? (int)item.ItemArray[10] : 0,
                        PrefixId = item.ItemArray[11] != DBNull.Value ? (int)item.ItemArray[11] : 0,
                        ClaimId = item.ItemArray[12] != DBNull.Value ? (int)item.ItemArray[12] : 0,
                        PaymentVoucherId = item.ItemArray[13] != DBNull.Value ? (int)item.ItemArray[13] : 0,
                        PaymentVoucherNumber = item.ItemArray[14] != DBNull.Value ? (string)item.ItemArray[14] : ""
                    };
                }
            }

            return paymentModels;
        }

        public List<ReinsurancePolicyModel> GetSystemReinsuranceIssues()
        {
            List<ReinsurancePolicyModel> reinsurancePolicyModels = new List<ReinsurancePolicyModel>();

            DataTable result;
            using (DynamicDataAccess pdb = new DynamicDataAccess("SistranEE"))
            {
                result = pdb.ExecuteSPDataTable("UP.GET_REINSURANCE_ISSUES");
            }

            if (result != null && result.Rows.Count > 0)
            {
                foreach (DataRow item in result.Rows)
                {
                    ReinsurancePolicyModel reinsurancePolicyModel = new ReinsurancePolicyModel
                    {
                        IssueReinsuranceId = item.ItemArray[0] != DBNull.Value ? (int)item.ItemArray[0] : 0,
                        EndorsementId = item.ItemArray[1] != DBNull.Value ? (int)item.ItemArray[1] : 0,
                        ReinsuranceNumber = item.ItemArray[2] != DBNull.Value ? (int)item.ItemArray[2] : 0,
                        MovementTypeId = item.ItemArray[3] != DBNull.Value ? (int)item.ItemArray[3] : 0,
                        BranchId = item.ItemArray[4] != DBNull.Value ? (int)item.ItemArray[4] : 0,
                        PrefixId = item.ItemArray[5] != DBNull.Value ? (int)item.ItemArray[5] : 0,
                        DocumentNumber = item.ItemArray[6] != DBNull.Value ? Convert.ToInt32(item.ItemArray[6]) : 0,
                        PoliciyId = item.ItemArray[7] != DBNull.Value ? (int)item.ItemArray[7] : 0,
                        InsuredIndividualId = item.ItemArray[8] != DBNull.Value ? (int)item.ItemArray[8] : 0
                    };

                    reinsurancePolicyModels.Add(reinsurancePolicyModel);
                }
            }

            return reinsurancePolicyModels;
        }

        public List<ReinsuranceClaimModel> GetSystemReinsuranceClaims()
        {
            List<ReinsuranceClaimModel> reinsuranceClaimModels = new List<ReinsuranceClaimModel>();

            DataTable result;
            using (DynamicDataAccess pdb = new DynamicDataAccess("SistranEE"))
            {
                result = pdb.ExecuteSPDataTable("UP.GET_REINSURANCE_ISSUES");
            }

            if (result != null && result.Rows.Count > 0)
            {
                foreach (DataRow item in result.Rows)
                {
                    ReinsuranceClaimModel reinsuranceClaimModel = new ReinsuranceClaimModel
                    {
                        ClaimReinsuranceId = item.ItemArray[0] != DBNull.Value ? (int)item.ItemArray[0] : 0,
                        ClaimId = item.ItemArray[1] != DBNull.Value ? (int)item.ItemArray[1] : 0,
                        ClaimModifyId = item.ItemArray[2] != DBNull.Value ? (int)item.ItemArray[2] : 0,
                        ReinsuranceNumber = item.ItemArray[3] != DBNull.Value ? (int)item.ItemArray[3] : 0,
                        MovementTypeId = item.ItemArray[4] != DBNull.Value ? (int)item.ItemArray[4] : 0
                    };
                    reinsuranceClaimModels.Add(reinsuranceClaimModel);
                }
            }

            return reinsuranceClaimModels;
        }

        public List<ReinsurancePaymentModel> GetSystemReinsurancePayments()
        {
            List<ReinsurancePaymentModel> reinsurancePaymentModels = new List<ReinsurancePaymentModel>();

            DataTable result;
            using (DynamicDataAccess pdb = new DynamicDataAccess("SistranEE"))
            {
                result = pdb.ExecuteSPDataTable("UP.GET_REINSURANCE_ISSUES");
            }

            if (result != null && result.Rows.Count > 0)
            {
                foreach (DataRow item in result.Rows)
                {
                    ReinsurancePaymentModel reinsurancePaymentModel = new ReinsurancePaymentModel
                    {
                        PaymentReinsuranceId = item.ItemArray[0] != DBNull.Value ? (int)item.ItemArray[0] : 0,
                        PaymentRequestId = item.ItemArray[1] != DBNull.Value ? (int)item.ItemArray[1] : 0,
                        ReinsuranceNumber = item.ItemArray[2] != DBNull.Value ? (int)item.ItemArray[2] : 0,
                        MovementTypeId = item.ItemArray[3] != DBNull.Value ? (int)item.ItemArray[3] : 0
                    };
                    reinsurancePaymentModels.Add(reinsurancePaymentModel);
                }
            }

            return reinsurancePaymentModels;
        }

        public CompanyListRiskModel GetAssignedListMantenance(string documentNumber, int? listRiskType)
        {
            CompanyListRiskModel resultOperation = new CompanyListRiskModel();
            resultOperation.CompanyRiskListOwn = new List<CompanyListRisk>();
            resultOperation.CompanyRiskListOfac = new List<CompanyListRiskOfac>();
            resultOperation.CompanyRiskListOnu = new List<OnuPerson>();
            DataTable result;


            string riskListTypeDescription = "";

            switch (listRiskType)
            {
                case (int)RiskListTypeEnum.OWN:
                    riskListTypeDescription = RiskListConstants.OWN;
                    break;
                case (int)RiskListTypeEnum.OFAC:
                    riskListTypeDescription = RiskListConstants.OFAC;
                    break;
                case (int)RiskListTypeEnum.ONU:
                    riskListTypeDescription = RiskListConstants.ONU;
                    break;
            }

            NameValue[] parameters = new NameValue[2];

            parameters[0] = new NameValue("@RISK_LIST_TYPE", riskListTypeDescription != null ? riskListTypeDescription : "0");
            parameters[1] = new NameValue("@ID_CARD_NO", documentNumber != null ? documentNumber : "");

            using (DynamicDataAccess pdb = new DynamicDataAccess("SistranEE"))
            {
                result = pdb.ExecuteSPDataTable("UP.GET_RISK_LIST_PERSON", parameters);
            }
            if (result != null && result.Rows.Count > 0)
            {
                foreach (DataRow item in result.Rows)
                {
                    if ((string)item.ItemArray[1] == RiskListConstants.OWN)
                    {
                        CompanyListRisk listRisk = JsonConvert.DeserializeObject<CompanyListRisk>((string)item.ItemArray[0]);
                        listRisk.ListRiskDescription = (string)item.ItemArray[4];
                        listRisk.ListRiskTypeDescription = RiskListConstants.OWN;
                        listRisk.RegistrationDate = (DateTime)item.ItemArray[5];
                        listRisk.ProcessId = (int)item.ItemArray[3];
                        if ((string)item.ItemArray[2] == RiskListConstants.Included)
                        {
                            listRisk.Event = (int)RiskListEventEnum.INCLUDED;
                        }
                        else
                        {
                            listRisk.Event = (int)RiskListEventEnum.EXCLUDED;
                        }
                        resultOperation.CompanyRiskListOwn.Add(listRisk);
                    }
                    else if ((string)item.ItemArray[1] == RiskListConstants.OFAC)
                    {
                        CompanyListRiskOfac listRisk = JsonConvert.DeserializeObject<CompanyListRiskOfac>((string)item.ItemArray[0]);
                        listRisk.ListRiskDescription = (string)item.ItemArray[4];
                        listRisk.ListRiskTypeDescription = RiskListConstants.OFAC;
                        listRisk.ProcessId = (int)item.ItemArray[3];
                        listRisk.RegistrationDate = (DateTime)item.ItemArray[5];
                        if (listRisk.Remarks == null)
                        {
                            listRisk.Remarks = string.Empty;
                        }
                        if ((string)item.ItemArray[2] == RiskListConstants.Included)
                        {
                            listRisk.Event = (int)RiskListEventEnum.INCLUDED;
                        }
                        else
                        {
                            listRisk.Event = (int)RiskListEventEnum.EXCLUDED;
                        }
                        resultOperation.CompanyRiskListOfac.Add(listRisk);
                    }
                    else if ((string)item.ItemArray[1] == RiskListConstants.ONU)
                    {
                        OnuPerson listRisk = JsonConvert.DeserializeObject<OnuPerson>((string)item.ItemArray[0]);
                        listRisk.RegistrationDate = (DateTime)item.ItemArray[5];
                        if ((string)item.ItemArray[2] == RiskListConstants.Included)
                        {
                            listRisk.Event = (int)RiskListEventEnum.INCLUDED;
                        }
                        else
                        {
                            listRisk.Event = (int)RiskListEventEnum.EXCLUDED;
                        }
                        if (listRisk.DataId != 0)
                        {
                            resultOperation.CompanyRiskListOnu.Add(listRisk);
                        }
                    }
                }
            }
            return resultOperation;
        }

        public void SaveListRiskMatchPerson(PersonModel person, int processId, ProcessType processType, PersonRoleEnum personRol, string movementId, DateTime MovementDate, DateTime movementDateFrom, DateTime movementDateTo)
        {
            
            NameValue[] parameters = new NameValue[16];
            parameters[0] = new NameValue("@PROCESS_ID", processId);
            parameters[1] = new NameValue("@DOCUMENT_TYPE", person.DocumentType);
            parameters[2] = new NameValue("@DOCUMENT_NUMBER", person.DocumentNumber);
            parameters[3] = new NameValue("@NAMES", person.Name);
            parameters[4] = new NameValue("@LAST_NAMES", person.SurName);
            parameters[5] = new NameValue("@MOVEMENT", EnumHelper.GetValueMember<DescriptionAttribute, ProcessType>(processType).Description);
            parameters[6] = new NameValue("@ROL", EnumHelper.GetValueMember<DescriptionAttribute, PersonRoleEnum>(personRol).Description);
            parameters[7] = new NameValue("@MOVEMENT_ID", movementId);
            parameters[8] = new NameValue("@MOVEMENT_DATE", MovementDate);
            parameters[9] = new NameValue("@MOVEMENT_FROM", movementDateFrom);
            parameters[10] = new NameValue("@MOVEMENT_TO", movementDateTo);
            parameters[11] = new NameValue("@OFAC_LIST", person.OfacList);
            parameters[12] = new NameValue("@OWN_LIST", person.OwnList);
            parameters[13] = new NameValue("@ONU_LIST", person.OnuList);
            parameters[14] = new NameValue("@IS_PARTIAL_MATCH", person.IsPartialMatch);
            parameters[15] = new NameValue("@LIST_DESCRIPTION", person.ListDescription);

            DataTable result;
            using (DynamicDataAccess pdb = new DynamicDataAccess("SistranEE"))
            {
                result = pdb.ExecuteSPDataTable("UP.SAVE_LIST_RISK_MATCH_PROCESS", parameters);
            }
        }

        public void SaveListRiskMatchCompany(CompanyModel company, int processId, ProcessType processType, PersonRoleEnum personRol, string movementId, DateTime MovementDate, DateTime movementDateFrom, DateTime movementDateTo)
        {

            NameValue[] parameters = new NameValue[16];
            parameters[0] = new NameValue("@PROCESS_ID", processId);
            parameters[1] = new NameValue("@DOCUMENT_TYPE", company.DocumentType);
            parameters[2] = new NameValue("@DOCUMENT_NUMBER", company.DocumentNumber);
            parameters[3] = new NameValue("@NAMES", company.TradeName);
            parameters[4] = new NameValue("@LAST_NAMES", company.TradeName);
            parameters[5] = new NameValue("@MOVEMENT", EnumHelper.GetValueMember<DescriptionAttribute, ProcessType>(processType).Description);
            parameters[6] = new NameValue("@ROL", EnumHelper.GetValueMember<DescriptionAttribute, PersonRoleEnum>(personRol).Description);
            parameters[7] = new NameValue("@MOVEMENT_ID", movementId);
            parameters[8] = new NameValue("@MOVEMENT_DATE", MovementDate);
            parameters[9] = new NameValue("@MOVEMENT_FROM", movementDateFrom);
            parameters[10] = new NameValue("@MOVEMENT_TO", movementDateTo);
            parameters[11] = new NameValue("@OFAC_LIST", company.OfacList);
            parameters[12] = new NameValue("@OWN_LIST", company.OwnList);
            parameters[13] = new NameValue("@ONU_LIST", company.OnuList);
            parameters[14] = new NameValue("@IS_PARTIAL_MATCH", company.IsPartialMatch);
            parameters[15] = new NameValue("@LIST_DESCRIPTION", company.ListDescription);

            DataTable result;
            using (DynamicDataAccess pdb = new DynamicDataAccess("SistranEE"))
            {
                result = pdb.ExecuteSPDataTable("UP.SAVE_LIST_RISK_MATCH_PROCESS", parameters);
            }
        }

        #endregion

        #region PROCESS
        public int CreateAsyncProcess(string processName, int totalRows)
        {
            int processId = 0;

            NameValue[] parameters = new NameValue[4];
            parameters[0] = new NameValue("@FILE_NAME", processName);
            parameters[1] = new NameValue("@LIST_CODE_ID", (int)RiskListTypeEnum.ONU);
            parameters[2] = new NameValue("@RISK_LIST_TYPE_CD", (int)RiskListTypeEnum.ONU);
            parameters[3] = new NameValue("@TOTAL_ROWS", totalRows);


            DataTable result;
            using (DynamicDataAccess pdb = new DynamicDataAccess("SistranEE"))
            {
                result = pdb.ExecuteSPDataTable("UP.CREATE_ASYNC_PROCESS", parameters);
            }
            if (result != null && result.Rows.Count > 0)
            {
                processId = (int)result.Rows[0].ItemArray[0];
            }
            return processId;
        }

        public List<MatchModel> GetListRiskMatchResult(int processId)
        {
            List<MatchModel> matchModels = new List<MatchModel>();

            NameValue[] parameters = new NameValue[1];
            parameters[0] = new NameValue("@PROCESS_ID", processId);

            DataTable result;
            using (DynamicDataAccess pdb = new DynamicDataAccess("SistranEE"))
            {
                result = pdb.ExecuteSPDataTable("UP.GET_LIST_RISK_MATCH_PROCESS", parameters);
            }
            if (result != null && result.Rows.Count > 0)
            {
                foreach (DataRow item in result.Rows)
                {
                    MatchModel match = new MatchModel();

                    match.ProcessId = (int)item.ItemArray[1];
                    match.DocumentType = (string)item.ItemArray[2];
                    match.DocumentNumber = (string)item.ItemArray[3];
                    match.Names = (string)item.ItemArray[4];
                    match.LastNames = (string)item.ItemArray[5];
                    match.Movement = (string)item.ItemArray[6];
                    match.Role = (string)item.ItemArray[7];
                    match.MovementId = (string)item.ItemArray[8];
                    match.MovementDate = (DateTime)item.ItemArray[9];
                    match.MovementFrom = (DateTime)item.ItemArray[10];
                    match.MovementTo = (DateTime)item.ItemArray[11];
                    match.OFACMatch = (bool)item.ItemArray[12];
                    match.OWNMatch = (bool)item.ItemArray[13];
                    match.ONUMatch = (bool)item.ItemArray[14];
                    match.ListDescription = (string)item.ItemArray[16];

                    matchModels.Add(match);
                }
            }
            return matchModels;

        }

        public void SendMailNotification(bool ews, string smtpServer, int smtpPort, string filePath, string email, string strUserName, string strSenderName, string strPassword,
            string strEmailFrom, string messageBody)
        {
            MailService mailService = new MailService();
            if (ews)
            {
                mailService.SendMailEws(smtpServer, smtpPort, email, strUserName, strPassword, strSenderName, messageBody, filePath, string.Empty);
            }
            else
            {
                mailService.SendMail(filePath, email, smtpServer, smtpPort, strUserName, strSenderName, strPassword, strEmailFrom, messageBody);
            }


        }

        public File GetFileIdByFileProcessValue(FileProcessValue fileProcessValue)
        {
            int fileId = 0;
            NameValue[] parameters = new NameValue[5];

            parameters[0] = new NameValue("@KEY_1", fileProcessValue.Key1);
            parameters[1] = new NameValue("@KEY_2", fileProcessValue.Key2);
            parameters[2] = new NameValue("@KEY_3", fileProcessValue.Key3);
            parameters[3] = new NameValue("@KEY_4", fileProcessValue.Key4);
            parameters[4] = new NameValue("@KEY_5", fileProcessValue.Key5);

            DataTable result;
            using (DynamicDataAccess pdb = new DynamicDataAccess("SistranEE"))
            {
                result = pdb.ExecuteSPDataTable("UP.GET_FILE_ID_BY_FILE_PROCESS_VALUE", parameters);
            }
            if (result != null && result.Rows.Count > 0)
            {
                fileId = (int)result.Rows[0].ItemArray[0];
            }
            return this.GetFileByFileId(fileId);
        }

        public File GetFileByFileId(int fileId)
        {
            File file = new File();

            NameValue[] parameters = new NameValue[1];

            parameters[0] = new NameValue("@FILE_ID", fileId);

            DataSet result;
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess("SistranEE"))
            {
                result = dynamicDataAccess.ExecuteSPDataSet("UP.GET_FILE_BY_FILE_ID", parameters);
            }

            if (result != null)
            {
                foreach (DataTable dataTable in result.Tables)
                {

                    foreach (DataRow row in dataTable.Rows)
                    {
                        if (dataTable.Columns.Contains("FILE_ID"))
                        {
                            file.Id = Convert.ToInt32(row["FILE_ID"].ToString() != string.Empty ? row["FILE_ID"] : 0);
                            file.Description = row["FILE_DESCRIPTION"].ToString();
                            file.SmallDescription = row["FILE_SMALL_DESCRIPTION"].ToString();
                            file.Observations = row["FILE_OBSERVATIONS"].ToString();
                            file.FileType = (FileType)Convert.ToInt32(row["FILE_TYPE_ID"].ToString() != string.Empty ? row["FILE_TYPE_ID"] : 0);
                            file.IsEnabled = Convert.ToBoolean(row["IS_ENABLED"].ToString());
                            file.Templates = new List<Template>();
                        }
                        else if (dataTable.Columns.Contains("TEMPLATE_ID"))
                        {
                            file.Templates.Add(new Template()
                            {
                                Id = Convert.ToInt32(row["ID"].ToString() != string.Empty ? row["ID"] : 0),
                                TemplateId = Convert.ToInt32(row["TEMPLATE_ID"].ToString() != string.Empty ? row["TEMPLATE_ID"] : 0),
                                IsMandatory = Convert.ToBoolean(row["IS_MANDATORY"].ToString()),
                                IsEnabled = Convert.ToBoolean(row["IS_ENABLED"].ToString()),
                                Order = Convert.ToInt32(row["ORDERING"].ToString() != string.Empty ? row["ORDERING"] : 0),
                                IsPrincipal = Convert.ToBoolean(row["IS_PRINCIPAL"].ToString()),
                                Description = row["DESCRIPTION"].ToString(),
                                Rows = new List<Row>()
                            });
                        }
                        else if (dataTable.Columns.Contains("FILE_TEMPLATE_ID"))
                        {
                            if (file.Templates.Where(x => x.Id == (int)row["FILE_TEMPLATE_ID"]).FirstOrDefault().Rows.Count == 0)
                            {
                                file.Templates.Where(x => x.Id == (int)row["FILE_TEMPLATE_ID"]).FirstOrDefault().Rows.Add(new Row()
                                {
                                    Fields = new List<Field>()
                                });
                            }

                            Field field = new Field()
                            {
                                Id = Convert.ToInt32(row["FIELD_ID"].ToString() != string.Empty ? row["FIELD_ID"] : 0),
                                Order = Convert.ToInt32(row["ORDERING"].ToString() != string.Empty ? row["ORDERING"] : 0),
                                ColumnSpan = Convert.ToInt32(row["COLUMN_SPAN"].ToString() != string.Empty ? row["COLUMN_SPAN"] : 0),
                                RowPosition = Convert.ToInt32(row["ROW_POSITION"].ToString() != string.Empty ? row["ROW_POSITION"] : 0),
                                IsMandatory = Convert.ToBoolean(row["IS_MANDATORY"].ToString()),
                                IsEnabled = Convert.ToBoolean(row["IS_ENABLED"].ToString()),
                                Description = row["DESCRIPTION"].ToString(),
                                PropertyName = row["PROPERTY_NAME"].ToString(),
                                PropertyLength = row["PROPERTY_LENGTH"].ToString(),
                                FieldType = (FieldType)Convert.ToInt32(row["FIELD_TYPE_ID"].ToString() != string.Empty ? row["FIELD_TYPE_ID"] : 0)
                            };

                            file.Templates.Where(x => x.Id == (int)row["FILE_TEMPLATE_ID"]).FirstOrDefault().Rows.FirstOrDefault().Fields.Add(field);
                        }
                    }

                };

            }

            return file;

        }

        public List<PendingProcessModel> GetProcessRequest()
        {
            List<PendingProcessModel> processRequest = new List<PendingProcessModel>();

            DataTable result;
            using (DynamicDataAccess pdb = new DynamicDataAccess("SistranEE"))
            {
                result = pdb.ExecuteSPDataTable("UP.GET_LIST_RISK_PENDING_PROCESS");
            }
            if (result != null && result.Rows.Count > 0)
            {
                foreach (DataRow objectResult in result.Rows)
                {
                    if ((bool)objectResult.ItemArray[4])
                    {
                        PendingProcessModel pendingProcessModel = new PendingProcessModel();
                        pendingProcessModel.Id = (int)objectResult.ItemArray[0];
                        pendingProcessModel.SearchValue = objectResult.ItemArray[7] != DBNull.Value ? (string)objectResult.ItemArray[7] : "";

                        if ((bool)objectResult.ItemArray[6])
                        {
                            pendingProcessModel.listType = RiskListConstants.ONU;
                        }

                        pendingProcessModel.IsMasive = (bool)objectResult.ItemArray[8];

                        processRequest.Add(pendingProcessModel);
                    }
                }
            }

            return processRequest;
        }

        public void UpdateProcessRequest(MatchModel matchModel, string fileName)
        {
            NameValue[] parameters = new NameValue[4];

            parameters[0] = new NameValue("@REQUEST_ID", matchModel.RequestId);
            parameters[1] = new NameValue("@PROCESS_ID", matchModel.ProcessId <= 0 ? -1 : matchModel.ProcessId);
            parameters[2] = new NameValue("@DESCRIPTION", fileName == string.Empty ? "Sin coincidencias" : fileName);
            parameters[3] = new NameValue("@STATUS", matchModel.StatusId);

            DataTable result;
            using (DynamicDataAccess pdb = new DynamicDataAccess("SistranEE"))
            {
                result = pdb.ExecuteSPDataTable("UP.UPDATE_LIST_RISK_PROCESS", parameters);
            }
        }

        public string GenerateMatchReport(int processId, List<MatchModel> matchModels, string path)
        {
            ExcelService excelService = new ExcelService();
            FileProcessValue fileProcessValue = new FileProcessValue
            {
                Key1 = (int)FileProcessType.MassiveReport,
                Key4 = 11,
                Key5 = 12
            };

            File file = this.GetFileIdByFileProcessValue(fileProcessValue);
            file.Name = "Reporte_De_Coincidencias_" + processId;

            List<Row> movementsRows = new List<Row>();

            List<Row> matchsRows = new List<Row>();

            string serializedMovementsFields = JsonConvert.SerializeObject(file.Templates.Where(x => x.Description == "Listado de Movimientos").FirstOrDefault().Rows.FirstOrDefault().Fields);
            string serializedMatchFields = JsonConvert.SerializeObject(file.Templates.Where(x => x.Description == "Listado de Coincidencias").FirstOrDefault().Rows.FirstOrDefault().Fields);

            foreach (MatchModel match in matchModels)
            {
                List<Field> movementsFields = JsonConvert.DeserializeObject<List<Field>>(serializedMovementsFields);

                List<Field> matchFields = JsonConvert.DeserializeObject<List<Field>>(serializedMatchFields);

               
                if (match.Movement == EnumHelper.GetValueMember<DescriptionAttribute, ProcessType>(ProcessType.PersonCreation).Description)
                {
                    Row matchRow = new Row();

                    int movementsCount = 0;

                    movementsCount = matchModels.Where(x => x.DocumentNumber == match.DocumentNumber && x.Movement != EnumHelper.GetValueMember<DescriptionAttribute, ProcessType>(ProcessType.PersonCreation).Description).Count();

                    matchFields.Find(u => u.PropertyName == "TipoDocumento").Value = match.DocumentType;
                    matchFields.Find(u => u.PropertyName == "NumeroDocumento").Value = match.DocumentNumber;
                    matchFields.Find(u => u.PropertyName == "Nombres").Value = match.Names;
                    matchFields.Find(u => u.PropertyName == "Apellido").Value = match.LastNames;
                    matchFields.Find(u => u.PropertyName == "TipoLista").Value = match.ListDescription;
                    matchFields.Find(u => u.PropertyName == "MovCount").Value = Convert.ToString(movementsCount);

                    matchRow.Fields = new List<Field>(matchFields);
                    matchsRows.Add(matchRow);
                }
                else
                {
                    Row movementRow = new Row();

                    movementsFields.Find(u => u.PropertyName == "IdProceso").Value = match.ProcessId.ToString();
                    movementsFields.Find(u => u.PropertyName == "TipoDocumento").Value = match.DocumentType;
                    movementsFields.Find(u => u.PropertyName == "NumeroDocumento").Value = match.DocumentNumber;
                    movementsFields.Find(u => u.PropertyName == "Nombres").Value = match.Names;
                    movementsFields.Find(u => u.PropertyName == "Apellido").Value = match.LastNames;
                    movementsFields.Find(u => u.PropertyName == "Movimiento").Value = match.Movement;
                    movementsFields.Find(u => u.PropertyName == "NumeroMovimiento").Value = match.MovementId;
                    movementsFields.Find(u => u.PropertyName == "Rol").Value = match.Role;
                    movementsFields.Find(u => u.PropertyName == "FechaMovimiento").Value = match.MovementDate.ToString();
                    movementsFields.Find(u => u.PropertyName == "FechaMovimientoDesde").Value = match.MovementFrom.ToString();
                    movementsFields.Find(u => u.PropertyName == "FechaMovimientoHasta").Value = match.MovementTo.ToString();
                    movementsFields.Find(u => u.PropertyName == "TipoLista").Value = match.ListDescription;

                    movementRow.Fields = new List<Field>(movementsFields);
                    movementsRows.Add(movementRow);
                }
            }

            file.Templates.Where(x => x.Description == "Listado de Coincidencias").FirstOrDefault().Rows.AddRange(matchsRows);

            file.Templates.Where(x => x.Description == "Listado de Movimientos").FirstOrDefault().Rows.AddRange(movementsRows);

            excelService.CreateListRiskReport(file, path);

            return file.Name;
        }

        public void RegisterMatchingprocessLog(int processId, ProcessStatusEnum processStatus, string errorDescription)
        {
            NameValue[] parameters = new NameValue[5];
            parameters[0] = new NameValue("@PROCESS_ID", processId);
            parameters[1] = new NameValue("@STATUS_DESCRIPTION", processStatus);
            parameters[2] = new NameValue("@STATUS_CODE", (int)processStatus);
            parameters[3] = new NameValue("@REGISTRATION_DATE", DateTime.Now);
            parameters[4] = new NameValue("@ERROR_DESCRIPTION", errorDescription);

            DataTable result;
            using (DynamicDataAccess pdb = new DynamicDataAccess("SistranEE"))
            {
                result = pdb.ExecuteSPDataTable("UP.CREATE_LIST_RISK_MATCHING_PROCESS_LOG", parameters);
            }
        }

        #endregion
    }
}
