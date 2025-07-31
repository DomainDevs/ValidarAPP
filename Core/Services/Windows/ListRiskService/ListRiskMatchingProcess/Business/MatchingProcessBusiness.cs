using ListRiskMatchingProcess.Enum;
using ListRiskMatchingProcess.Model;
using Newtonsoft.Json;
using Sistran.Co.Application.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListRiskMatchingProcess.Business
{
    public class MatchingProcessBusiness
    {

        public MatchingProcessModel GetSystemPeople()
        {
            DataTable result;
            MatchingProcessModel matchingProcessModel = new MatchingProcessModel();
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
                        CompanyModel companyCompany = new CompanyModel
                        {
                            IndividualId = item.ItemArray[0] != DBNull.Value ? (int)item.ItemArray[0] : 0,
                            DocumentTypeId = item.ItemArray[2] != DBNull.Value ? Convert.ToInt16(item.ItemArray[2]) : 0,
                            DocumentType = item.ItemArray[3] != DBNull.Value ? (string)item.ItemArray[3] : "",
                            DocumentNumber = item.ItemArray[4] != DBNull.Value ? (string)item.ItemArray[4] : "",
                            TradeName = item.ItemArray[5] != DBNull.Value ? (string)item.ItemArray[5] : ""
                        };

                        matchingProcessModel.Company.Add(companyCompany);
                    }
                    else if(Convert.ToInt16(item.ItemArray[1]) == (int)IndividualType.Person)
                    {
                        PersonModel companyPerson = new PersonModel
                        {
                            IndividualId = item.ItemArray[0] != DBNull.Value ? (int)item.ItemArray[0] : 0,
                            DocumentTypeId = item.ItemArray[6] != DBNull.Value ? Convert.ToInt16(item.ItemArray[6]) : 0,
                            DocumentType = item.ItemArray[7] != DBNull.Value ? (string)item.ItemArray[7] : "",
                            DocumentNumber = item.ItemArray[8] != DBNull.Value ? (string)item.ItemArray[8] : "",
                            SurName = item.ItemArray[9] != DBNull.Value ? (string)item.ItemArray[9] : "",
                            SecondSurName = item.ItemArray[10] != DBNull.Value ? (string)item.ItemArray[10] : "",
                            Name = item.ItemArray[11] != DBNull.Value ? (string)item.ItemArray[11] : ""
                        };


                        matchingProcessModel.People.Add(companyPerson);
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
                        PolicyHolderId = item.ItemArray[4] != DBNull.Value ? (int)item.ItemArray[4] : 0,
                        EndorsementId = item.ItemArray[5] != DBNull.Value ? (int)item.ItemArray[5] : 0,
                        RiskId = item.ItemArray[6] != DBNull.Value ? (int)item.ItemArray[6] : 0,
                        RiskStatusId = item.ItemArray[7] != DBNull.Value ? Convert.ToInt16(item.ItemArray[7]) : 0
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
                        EstimationTypeCd = item.ItemArray[9] != DBNull.Value ? (int)item.ItemArray[9] : 0,
                        EstimationTypeStatusCd = item.ItemArray[10] != DBNull.Value ? (int)item.ItemArray[10] : 0,
                        EstimationTypeStatusReasonCd = item.ItemArray[11] != DBNull.Value ? (int)item.ItemArray[11] : 0,
                        EstimationAmount = item.ItemArray[12] != DBNull.Value ? (decimal)item.ItemArray[12] : 0,
                        EstimateAmountAccumulate = item.ItemArray[13] != DBNull.Value ? (decimal)item.ItemArray[13] : 0
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
                        IndividualId = item.ItemArray[4] != DBNull.Value ? (int)item.ItemArray[4] : 0,
                        PaymentMethodId = item.ItemArray[5] != DBNull.Value ? (int)item.ItemArray[5] : 0,
                        TotalAmount = item.ItemArray[6] != DBNull.Value ? (decimal)item.ItemArray[6] : 0,
                        PaymentRequestNumber = item.ItemArray[7] != DBNull.Value ? (int)item.ItemArray[7] : 0,
                        Description = item.ItemArray[8] != DBNull.Value ? (string)item.ItemArray[8] : "",
                        PaymentMovementTypeId = item.ItemArray[9] != DBNull.Value ? (int)item.ItemArray[9] : 0,
                        PrefixId = item.ItemArray[10] != DBNull.Value ? (int)item.ItemArray[10] : 0,
                        ClaimId = item.ItemArray[11] != DBNull.Value ? (int)item.ItemArray[11] : 0,
                        PaymentVoucherId = item.ItemArray[12] != DBNull.Value ? (int)item.ItemArray[12] : 0,
                        PaymentVoucherNumber = item.ItemArray[13] != DBNull.Value ? (string)item.ItemArray[13] : ""
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
                        DocumentNumber = item.ItemArray[6] != DBNull.Value ? (int)item.ItemArray[6] : 0,
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
                    //FALTA AGREGAR LISTA ONU
                    //else if ((int)item.ItemArray[1] == (int)RiskListTypeEnum.ONU)
                    //{
                    //    CompanyListRisk listRisk = JsonConvert.DeserializeObject<CompanyListRisk>((string)item.ItemArray[0]);
                    //}
                }
            }
            return resultOperation;
        }
    }
}
