    using ListRiskMatchingProcess.Business;
using ListRiskMatchingProcess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ListRiskMatchingProcess
{
    public class ListRiskMatchingProcessInitializer
    {
        private MatchingProcessModel MatchingProcessModel { get; set; }
        private MatchingProcessModel MatchedProcessModel { get; set; }

        private MatchingProcessBusiness matchingProcessBusiness;

        public ListRiskMatchingProcessInitializer()
        {
            matchingProcessBusiness = new MatchingProcessBusiness();
            MatchingProcessModel = new MatchingProcessModel
            {
                Company = new List<CompanyModel>(),
                People = new List<PersonModel>(),
                Reinsurances = new ReinsuranceModel()
            };
            MatchedProcessModel = new MatchingProcessModel
            {
                Company = new List<CompanyModel>(),
                People = new List<PersonModel>(),
                Reinsurances = new ReinsuranceModel()
            };
        }

        #region Process
        public async void GenerateProcess(int processFrequency, int processNodes, int processRetries)
        {
            TimeSpan interval = new TimeSpan(processFrequency, 0, 0);
            while (true)
            {
                var tasks = new List<Task>()
                {
                    GetSystemPeople(),
                    GetSystemPolicies(),
                    GetSystemClaims(),
                    GetSystemPayments(),
                    GetSystemReinsuranceIssues(),
                    GetSystemReinsuranceClaims(),
                    GetSystemReinsurancePayments(),
                    GetSystemRiskLists()
                };

                await Task.WhenAll(tasks).ConfigureAwait(false);

                MatchingProcessModel.People
                    .AsParallel()
                    .WithDegreeOfParallelism(10)
                    .ForAll(f => ValidatePersonMovements(f));

                Thread.Sleep(interval);
            }

        }

        public Task GetSystemPeople()
        {
            return Task.Run(() =>
            {
                MatchingProcessModel matchingProcess = matchingProcessBusiness.GetSystemPeople();
                MatchingProcessModel.People = matchingProcess.People;
                MatchingProcessModel.Company = matchingProcess.Company;
                return matchingProcess;
            });
            
        }


        public Task GetSystemPolicies()
        {
            return Task.Run(() =>
            {
                MatchingProcessModel.Policies = matchingProcessBusiness.GetSystemPolicies();
                return MatchingProcessModel;
            });
        }

        public Task GetSystemClaims()
        {
            return Task.Run(() =>
            {
                MatchingProcessModel.Claims = matchingProcessBusiness.GetSystemClaims();
                return MatchingProcessModel;
            });

        }

        public Task GetSystemPayments()
        {
            return Task.Run(() =>
            {
                MatchingProcessModel.Payments = matchingProcessBusiness.GetSystemPayment();
                return MatchingProcessModel;
            });
        }

        public Task GetSystemReinsuranceIssues()
        {
            return Task.Run(() =>
            {
                MatchingProcessModel.Reinsurances.ReinsurancePolicies = matchingProcessBusiness.GetSystemReinsuranceIssues();
                return MatchingProcessModel;
            });
        }

        public Task GetSystemReinsuranceClaims()
        {
            return Task.Run(() =>
            {
                MatchingProcessModel.Reinsurances.ReinsuranceClaims = matchingProcessBusiness.GetSystemReinsuranceClaims();
                return MatchingProcessModel;
            });
        }

        public Task GetSystemReinsurancePayments()
        {
            return Task.Run(() =>
            {
                MatchingProcessModel.Reinsurances.ReinsurancePayments = matchingProcessBusiness.GetSystemReinsurancePayments();
                return MatchingProcessModel;
            });
        }

        public Task GetSystemRiskLists()
        {
            return Task.Run(() =>
            {
                MatchingProcessModel.RiskList = matchingProcessBusiness.GetAssignedListMantenance(null, null);
                return MatchingProcessModel;
            });
        }

        public void ValidatePersonMovements(PersonModel personModel)
        {
            List<CompanyListRisk> personOwnRiskListMatch = MatchingProcessModel.RiskList.CompanyRiskListOwn.FindAll(x => x.DocumentNumber == personModel.DocumentNumber).ToList();
            List<CompanyListRiskOfac> personOfacRiskListMatch = MatchingProcessModel.RiskList.CompanyRiskListOfac.FindAll(x => x.EntNum == Convert.ToInt32(personModel.DocumentNumber)).ToList();
            
            if (personOwnRiskListMatch != null && personOwnRiskListMatch.Count > 0)
            {
                List<ReinsurancePolicyModel> ReinsurancePolicyMatchModels = new List<ReinsurancePolicyModel>();
                List<ReinsuranceClaimModel> ReinsuranceClaimMatchModels = new List<ReinsuranceClaimModel>();
                List<ReinsurancePaymentModel> ReinsurancePaymentMatchModels = new List<ReinsurancePaymentModel>();
                
                List<PolicyModel> personHolderPoliciesMatchModels = MatchingProcessModel.Policies.FindAll(x => x.PolicyHolderId == personModel.IndividualId);

                MatchedProcessModel.People.Add(personModel);

                foreach (PolicyModel policyModel in personHolderPoliciesMatchModels)
                {
                    ReinsurancePolicyMatchModels = MatchingProcessModel.Reinsurances.ReinsurancePolicies.FindAll(x => x.PoliciyId == policyModel.PolicyId);
                    foreach (ReinsurancePolicyModel reinsurancePolicyModel in ReinsurancePolicyMatchModels)
                    {
                        ReinsurancePolicyMatchModels.Add(reinsurancePolicyModel);
                    }
                    MatchedProcessModel.Policies.Add(policyModel);
                }

                List<ClaimModel> claimMatchModels = MatchingProcessModel.Claims.FindAll(x => x.IndividualId == personModel.IndividualId);

                foreach (ClaimModel item in claimMatchModels)
                {
                    ReinsuranceClaimMatchModels = MatchingProcessModel.Reinsurances.ReinsuranceClaims.FindAll(x => x.ClaimId == item.ClaimCd);
                }

                List<PaymentModel> paymentMatchModels = MatchingProcessModel.Payments.FindAll(x => x.IndividualId == personModel.IndividualId);

                foreach (PaymentModel item in paymentMatchModels)
                {
                    ReinsurancePaymentMatchModels = MatchingProcessModel.Reinsurances.ReinsurancePayments.FindAll(x => x.PaymentRequestId == item.PaymentRequestId);
                }
            }
        }


        #endregion
    }
}
 