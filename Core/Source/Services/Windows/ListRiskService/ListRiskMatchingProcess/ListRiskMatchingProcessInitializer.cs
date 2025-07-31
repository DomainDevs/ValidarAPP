using ListRiskMatchingProcess.DAO_s;
using ListRiskMatchingProcess.Enum;
using ListRiskMatchingProcess.Helper;
using ListRiskMatchingProcess.Model;
using Newtonsoft.Json;
using Sistran.Core.Application.UniquePersonV1.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ListRiskMatchingProcess
{
    public class ListRiskMatchingProcessInitializer
    {
        private PeopleProcessModel _SystemPeople { get; set; }
        private PeopleProcessModel _MatchedPeople { get; set; }
        private MoveMatchPerson _MoveMatchPerson { get; set; }
        private CompanyListRiskModel _RiskList { get; set; }
        private int _processId { get; set; }
        private static List<int> _currentRequestProcessing { get; set; }
        private static bool _processing { get; set; }
        public int _currentProcessId { get; private set; }
        public string _listRiskType { get; set; }
        private static IEnumerable<ViewListRiskPerson> _entityViewListRisk { get; set; }
        private static Dictionary<string, int> _umbral { get; set; }

        private MatchingProcessDAO matchingProcessDAO;

        public ListRiskMatchingProcessInitializer()
        {
            _umbral = new Dictionary<string, int>();

            _currentRequestProcessing = new List<int>();
            matchingProcessDAO = new MatchingProcessDAO();
            _SystemPeople = new PeopleProcessModel
            {
                Company = new List<CompanyModel>(),
                People = new List<PersonModel>(),
            };
            _MatchedPeople = new PeopleProcessModel
            {
                Company = new List<CompanyModel>(),
                People = new List<PersonModel>(),
            };
            _MoveMatchPerson = new MoveMatchPerson
            {
                Policies = new List<PolicyModel>(),
                Claims = new List<ClaimModel>(),
                Payments = new List<PaymentModel>(),
                Reinsurances = new ReinsuranceModel()
            };
            _RiskList = new CompanyListRiskModel();
        }

        #region Process
        public async void GenerateProcess(int processFrequency, int processNodes, int processRetries, int? requestId, bool isMasive, string searchValue, string listType)
        {

            while (processRetries > 0)
            {
                try
                {

                    string fileName = "";
                    _processing = true;
                    _processId = 0;

                    MatchModel matchModel = new MatchModel();
                    matchModel.RequestId = requestId == null ? 0 : requestId;
                    matchModel.StatusId = (int)ProcessStatusEnum.Cargando;
                    matchingProcessDAO.UpdateProcessRequest(matchModel, fileName);
                    List<MatchModel> matchModels = new List<MatchModel>();
                    GetRiskListView();

                    var tasks = new List<Task>()
                            {
                                GetSystemPeople(),
                                GetSystemPolicies(),
                                GetSystemClaims(),
                                GetSystemPayments(),
                                GetSystemReinsuranceIssues(),
                                GetSystemReinsuranceClaims(),
                                GetSystemReinsurancePayments()
                            };

                    await Task.WhenAll(tasks).ConfigureAwait(false);

                    if (isMasive)
                    {
                        if (listType == RiskListConstants.ONU)
                        {
                            _entityViewListRisk = _entityViewListRisk.Where(x => x.RiskListType == "ONU").ToList();
                        }
                        await PeopleValidationsAsync(_SystemPeople);
                    }
                    else
                    {
                        List<PersonModel> personModels = _SystemPeople.People.FindAll(x => x.DocumentNumber == searchValue || x.Name.Contains(searchValue) || x.SecondSurName.Contains(searchValue) || x.SurName.Contains(searchValue));
                        List<CompanyModel> companyModels = _SystemPeople.Company.FindAll(x => x.DocumentNumber == searchValue || x.TradeName.Contains(searchValue));

                        PeopleProcessModel peopleProcessModel = new PeopleProcessModel()
                        {
                            People = personModels,
                            Company = companyModels
                        };

                        await PeopleValidationsAsync(peopleProcessModel);
                    }

                    if (_MatchedPeople.People.Count > 0 || _MatchedPeople.Company.Count > 0)
                    {
                        _processId = matchingProcessDAO.CreateAsyncProcess("Proceso Validacion de Coincidencias", _MatchedPeople.Company.Count + _MatchedPeople.People.Count);
                        matchingProcessDAO.RegisterMatchingprocessLog(_processId, ProcessStatusEnum.Cargado, string.Empty);
                        _currentProcessId = _processId;

                        await Task.Run(() => (from PersonModel person in _MatchedPeople.People select person).AsParallel().WithDegreeOfParallelism(processNodes).ForAll(RegisterCreationPerson));
                        await Task.Run(() => (from CompanyModel company in _MatchedPeople.Company select company).AsParallel().WithDegreeOfParallelism(processNodes).ForAll(RegisterCreationCompany));

                        await Task.Run(() => (from PersonModel person in _MatchedPeople.People select person).AsParallel().WithDegreeOfParallelism(processNodes).ForAll(ValidatePoliciesPerson));
                        await Task.Run(() => (from PersonModel person in _MatchedPeople.People select person).AsParallel().WithDegreeOfParallelism(processNodes).ForAll(ValidateClaimsPerson));
                        await Task.Run(() => (from PersonModel person in _MatchedPeople.People select person).AsParallel().WithDegreeOfParallelism(processNodes).ForAll(ValidatePaymentsPerson));

                        await Task.Run(() => (from CompanyModel company in _MatchedPeople.Company select company).AsParallel().WithDegreeOfParallelism(processNodes).ForAll(ValidatePoliciesCompany));
                        await Task.Run(() => (from CompanyModel company in _MatchedPeople.Company select company).AsParallel().WithDegreeOfParallelism(processNodes).ForAll(ValidateClaimsCompany));
                        await Task.Run(() => (from CompanyModel company in _MatchedPeople.Company select company).AsParallel().WithDegreeOfParallelism(processNodes).ForAll(ValidatePaymentsCompany));

                        matchingProcessDAO.RegisterMatchingprocessLog(_processId, ProcessStatusEnum.Procesado, string.Empty);
                        matchModels = matchingProcessDAO.GetListRiskMatchResult(_processId);
                        fileName = matchingProcessDAO.GenerateMatchReport(_processId, matchModels, ConfigurationManager.AppSettings["ExternalFolderFiles"]);
                        matchModel.StatusId = (int)ProcessStatusEnum.Procesado;
                    }
                    else
                    {
                        matchModel.StatusId = (int)ProcessStatusEnum.SinCoincidencias;
                    }


                    matchModel.ProcessId = _processId;
                    matchingProcessDAO.UpdateProcessRequest(matchModel, fileName);

                    _currentRequestProcessing.Remove(matchModel.RequestId.Value);

                    if (listType == RiskListConstants.ONU)
                    {
                        if (matchModels.Count > 0)
                        {
                            string messageSuccessBody = "Finalizó exitosamente el proceso automatico de coincidencias para la lista ONU a las " + DateTime.Now + ". Id del proceso: " + matchModel.RequestId.Value + " con " + matchModels.Count +
                                " posibles coincidencias";
                            SendMailNotification(fileName, messageSuccessBody);
                        }
                        else
                        {
                            string messageSuccessBody = "Finalizó exitosamente el proceso automatico de coincidencias para la lista ONU a las " + DateTime.Now + ", donde no se encontraron coincidencias." +
                                " Id del proceso: " + matchModel.RequestId.Value;
                            SendMailNotification("", messageSuccessBody);
                        }
                    }

                    _processing = false;

                    break;
                }
                catch (Exception ex)
                {
                    _processing = false;
                    processRetries--;
                }

            }

        }

        private static void SendMailNotification(string fileName, string messageBody)
        {
            MatchingProcessDAO matchingProcessDAO = new MatchingProcessDAO();
            string filePath = string.Empty;
            if (!string.IsNullOrEmpty(fileName))
            {
                filePath = ConfigurationManager.AppSettings["ExternalFolderFiles"] + "\\" + fileName + ".xlsx";
            }
            string email = ConfigurationManager.AppSettings["EMailTo"];
            string SmtpServer = ConfigurationManager.AppSettings.Get("SMTPServer");
            int SMTPServerPort = Convert.ToInt32(ConfigurationManager.AppSettings.Get("SMTPServerPort"));
            string strUserName = ConfigurationManager.AppSettings.Get("EmailFrom");
            string strSenderName = "ONU List risk service";
            string strPassword = ConfigurationManager.AppSettings.Get("EMailPass");
            string strEmailFrom = ConfigurationManager.AppSettings.Get("EmailFrom");
            bool ews = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("EWS"));

            matchingProcessDAO.SendMailNotification(ews, SmtpServer, SMTPServerPort, filePath, email, strUserName, strSenderName, strPassword,
            strEmailFrom, messageBody);
        }


        public void CheckProcessRequest(int processFrequency, int processNodes, int processRetries)
        {
            MatchingProcessDAO matchingProcessDAO = new MatchingProcessDAO();
            while (true)
            {
                List<PendingProcessModel> pendingProcessModels = matchingProcessDAO.GetProcessRequest();
                if (pendingProcessModels.Count > 0)
                {
                    foreach (PendingProcessModel pendingProcess in pendingProcessModels)
                    {
                        if (!_processing)
                        {
                            _processing = true;
                            _currentRequestProcessing.Add(pendingProcess.Id);
                            Task.Run(() => GenerateProcess(processFrequency, processNodes, processRetries, pendingProcess.Id, pendingProcess.IsMasive, pendingProcess.SearchValue, pendingProcess.listType));
                        }
                    }
                }
                Thread.Sleep(1000);
            }
        }


        public async Task PeopleValidationsAsync(PeopleProcessModel systemPeople)
        {
            PeopleProcessModel matchingProcess = new PeopleProcessModel();
            _MatchedPeople.Company = new List<CompanyModel>();
            _MatchedPeople.People = new List<PersonModel>();
            try
            {
                await Task.Run(() => (from CompanyModel c in systemPeople.Company select c).AsParallel().WithDegreeOfParallelism(10).ForAll(CompanyValidation));
                await Task.Run(() => (from PersonModel p in systemPeople.People select p).AsParallel().WithDegreeOfParallelism(10).ForAll(PersonValidation));
            }
            catch (Exception ex)
            {
                matchingProcessDAO.RegisterMatchingprocessLog(_processId, ProcessStatusEnum.ConErrores, ex.Message);
                throw;
            }
        }

        public int personCount { get; set; }
        public void PersonValidation(PersonModel person)
        {

            if (person.DocumentNumber != "" && person.DocumentNumber != "N/A")
            {
                string fullName = person.Name + " " + person.SurName + " " + person.SecondSurName;
                IEnumerable<RiskListMatch> riskListMatches = ValidateListRiskPerson(person.DocumentNumber, fullName, null);

                if (riskListMatches.Any())
                {
                    foreach (RiskListMatch match in riskListMatches)
                    {
                        string personString = JsonConvert.SerializeObject(person);
                        PersonModel personMatch = new PersonModel();
                        personMatch = JsonConvert.DeserializeObject<PersonModel>(personString);
                        personMatch.ListDescription = match.listType;
                        lock (_MatchedPeople.People)
                        {
                            if (!_MatchedPeople.People.Any(x => x.DocumentNumber == personMatch.DocumentNumber && x.ListDescription == match.listType))
                            {
                                _MatchedPeople.People.Add(personMatch);
                            }
                        }

                    }
                }
            }
            //personCount++;
        }

        public int companyCount { get; set; }
        public void CompanyValidation(CompanyModel company)
        {

            if (company.DocumentNumber != "" && company.DocumentNumber != "N/A")
            {
                IEnumerable<RiskListMatch> riskListMatches = ValidateListRiskPerson(company.DocumentNumber, company.TradeName, null);

                if (riskListMatches.Any())
                {
                    foreach (RiskListMatch match in riskListMatches)
                    {
                        string companyString = JsonConvert.SerializeObject(company);
                        CompanyModel companyMatch = new CompanyModel();
                        companyMatch = JsonConvert.DeserializeObject<CompanyModel>(companyString);
                        companyMatch.ListDescription = match.listType;
                        lock (_MatchedPeople.Company)
                        {
                            if (!_MatchedPeople.Company.Any(x => x.DocumentNumber == companyMatch.DocumentNumber && x.ListDescription == match.listType))
                            {
                                _MatchedPeople.Company.Add(companyMatch);
                            }
                        }

                    }
                }
            }
            //companyCount++;
        }


        public string[] CompanyConcatName(string[] fullName)
        {
            string pattern = @"(\b(([Ss])\W?([Aa])\W?([Ss])?)\b|(\b([Ss])([Aa])([Ss])?)\b)|(\b(([Dd])\s*([Ee]))\b)|(\b(([Ll])\s*([Aa]))\b)|(\b(([Ee])\s*([Nn]))\b)";
            Regex rg = new Regex(pattern);

            List<string> fullNameConcat = fullName.ToList();

            int originalLength = fullNameConcat.Count;

            for (int i = 0; i < fullNameConcat.Count; i++)
            {
                if (i > originalLength)
                {
                    break;
                }
                if (fullNameConcat[i].Length == 1)
                {
                    if (i - 1 > -1)
                    {
                        if (fullNameConcat[i - 1].Length == 1)
                        {
                            fullNameConcat[i - 1] = fullNameConcat[i - 1] + ' ' + fullNameConcat[i];
                        }
                        else
                        {
                            fullNameConcat.Add(fullNameConcat[i - 1] + ' ' + fullNameConcat[i]);
                        }
                    }
                    if (i + 1 <= fullNameConcat.Count)
                    {
                        if (fullNameConcat[i + 1].Length == 1)
                        {
                            fullNameConcat[i + 1] = fullNameConcat[i] + ' ' + fullNameConcat[i + 1];
                        }
                        else
                        {
                            fullNameConcat.Add(fullNameConcat[i] + ' ' + fullNameConcat[i + 1]);
                        }

                    }
                    fullNameConcat.Remove(fullNameConcat[i]);
                }
            }

            fullNameConcat = fullNameConcat.Where(x => !rg.Match(x).Success).ToList();

            return fullNameConcat.ToArray();
        }
        public IEnumerable<RiskListMatch> ValidateListRiskPerson(string documentNumber, string originalName, int? riskListType)
        {
            documentNumber = documentNumber.Trim();
            ValidateListRiskPersonHelper validateHelper = new ValidateListRiskPersonHelper(_entityViewListRisk, documentNumber, originalName, riskListType, _umbral);

            return validateHelper.ExecuteFuzzyMatching().ToList();
        }

        #endregion

        #region Querys

        public void GetRiskListView()
        {
            MatchingProcessDAO matchingProcessDAO = new MatchingProcessDAO();

            if (_umbral.Count == 0)
            {
                _umbral.Add("ONU", matchingProcessDAO.GetListRiskUmbral("RISK_LIST_UMBRAL_ONU"));
                _umbral.Add("OFAC", matchingProcessDAO.GetListRiskUmbral("RISK_LIST_UMBRAL_OFAC"));
                _umbral.Add("PROPIA", matchingProcessDAO.GetListRiskUmbral("RISK_LIST_UMBRAL_OWN"));
            }
            else
            {
                int onuCurrentValue = 0;
                _umbral.TryGetValue("ONU", out onuCurrentValue);
                if (onuCurrentValue != matchingProcessDAO.GetListRiskUmbral("RISK_LIST_UMBRAL_ONU"))
                {
                    _umbral.Remove("ONU");
                    _umbral.Add("ONU", matchingProcessDAO.GetListRiskUmbral("RISK_LIST_UMBRAL_ONU"));
                }

                int ofacCurrentValue = 0;
                _umbral.TryGetValue("OFAC", out ofacCurrentValue);
                if (ofacCurrentValue != matchingProcessDAO.GetListRiskUmbral("RISK_LIST_UMBRAL_OFAC"))
                {
                    _umbral.Remove("OFAC");
                    _umbral.Add("OFAC", matchingProcessDAO.GetListRiskUmbral("RISK_LIST_UMBRAL_OFAC"));
                }

                int propiaCurrentValue = 0;
                _umbral.TryGetValue("PROPIA", out propiaCurrentValue);
                if (propiaCurrentValue != matchingProcessDAO.GetListRiskUmbral("RISK_LIST_UMBRAL_OWN"))
                {
                    _umbral.Remove("PROPIA");
                    _umbral.Add("PROPIA", matchingProcessDAO.GetListRiskUmbral("RISK_LIST_UMBRAL_OWN"));
                }
            }
            _entityViewListRisk = matchingProcessDAO.GetListRiskView();

        }

        public Task GetSystemPeople()
        {
            return Task.Run(() =>
            {
                PeopleProcessModel matchingProcess = matchingProcessDAO.GetSystemPeople();
                _SystemPeople.People = matchingProcess.People;
                _SystemPeople.Company = matchingProcess.Company;
                return matchingProcess;
            });

        }

        public Task GetSystemPolicies()
        {
            return Task.Run(() =>
            {
                _MoveMatchPerson.Policies = matchingProcessDAO.GetSystemPolicies();
                return _SystemPeople;
            });
        }

        public Task GetSystemClaims()
        {
            return Task.Run(() =>
            {
                _MoveMatchPerson.Claims = matchingProcessDAO.GetSystemClaims();
                return _SystemPeople;
            });

        }

        public Task GetSystemPayments()
        {
            return Task.Run(() =>
            {
                _MoveMatchPerson.Payments = matchingProcessDAO.GetSystemPayment();
                return _SystemPeople;
            });
        }

        public Task GetSystemReinsuranceIssues()
        {
            return Task.Run(() =>
            {
                _MoveMatchPerson.Reinsurances.ReinsurancePolicies = matchingProcessDAO.GetSystemReinsuranceIssues();
                return _SystemPeople;
            });
        }

        public Task GetSystemReinsuranceClaims()
        {
            return Task.Run(() =>
            {
                _MoveMatchPerson.Reinsurances.ReinsuranceClaims = matchingProcessDAO.GetSystemReinsuranceClaims();
                return _SystemPeople;
            });
        }

        public Task GetSystemReinsurancePayments()
        {
            return Task.Run(() =>
            {
                _MoveMatchPerson.Reinsurances.ReinsurancePayments = matchingProcessDAO.GetSystemReinsurancePayments();
                return _SystemPeople;
            });
        }

        public Task GetSystemRiskLists()
        {
            return Task.Run(() =>
            {
                _RiskList = matchingProcessDAO.GetAssignedListMantenance(null, null);
                return _SystemPeople;
            });
        }

        #endregion

        #region Validations

        public void RegisterCreationPerson(PersonModel personModel)
        {
            matchingProcessDAO.SaveListRiskMatchPerson(personModel, _processId, Enum.ProcessType.PersonCreation, Enum.PersonRoleEnum.PersonCreation, personModel.DocumentNumber.ToString(), DateTime.Now, DateTime.Now, DateTime.Now);
        }
        public void RegisterCreationCompany(CompanyModel companyModel)
        {
            matchingProcessDAO.SaveListRiskMatchCompany(companyModel, _processId, Enum.ProcessType.PersonCreation, Enum.PersonRoleEnum.PersonCreation, companyModel.DocumentNumber.ToString(), DateTime.Now, DateTime.Now, DateTime.Now);
        }

        public void ValidatePoliciesPerson(PersonModel personModel)
        {
            MatchingProcessDAO matchingProcessDAO = new MatchingProcessDAO();
            int individualId = personModel.IndividualId;
            IEnumerable<PolicyModel> personHolderPoliciesMatchModels = _MoveMatchPerson.Policies.Where(x => x.PolicyHolderId == individualId || x.InsuredId == individualId || x.BeneficiaryId == individualId || x.AgentId == individualId);

            if (personHolderPoliciesMatchModels.Any())
            {
                foreach (PolicyModel policyModel in personHolderPoliciesMatchModels)
                {
                    PersonRoleEnum roleId = PersonRoleEnum.PolicyHolder;
                    if (policyModel.InsuredId == individualId)
                    {
                        roleId = PersonRoleEnum.PolicyInsured;
                    }
                    else if (policyModel.AgentId == individualId)
                    {
                        roleId = PersonRoleEnum.PolicyAgent;
                    }
                    else if (policyModel.PolicyHolderId == individualId)
                    {
                        roleId = PersonRoleEnum.PolicyHolder;
                    }
                    else if (policyModel.BeneficiaryId == individualId)
                    {
                        roleId = PersonRoleEnum.PolicyBeneficiary;
                    }

                    matchingProcessDAO.SaveListRiskMatchPerson(personModel, _processId, Enum.ProcessType.Policy, roleId, policyModel.DocumentNumber.ToString(), policyModel.IssueDate, policyModel.CurrentFrom, policyModel.CurrentTo);

                    IEnumerable<ReinsurancePolicyModel> reinsurancePolicyMatchModels = _MoveMatchPerson.Reinsurances.ReinsurancePolicies.Where(x => x.PoliciyId == policyModel.PolicyId);
                    if (reinsurancePolicyMatchModels.Any())
                    {
                        matchingProcessDAO.SaveListRiskMatchPerson(personModel, _processId, Enum.ProcessType.Policy, Enum.PersonRoleEnum.Reinsured, policyModel.DocumentNumber.ToString(), policyModel.IssueDate, policyModel.CurrentFrom, policyModel.CurrentTo);
                    }

                }
            }
        }

        public void ValidatePoliciesCompany(CompanyModel companyModel)
        {

            int individualId = companyModel.IndividualId;
            IEnumerable<PolicyModel> personHolderPoliciesMatchModels = _MoveMatchPerson.Policies.Where(x => x.PolicyHolderId == individualId);

            if (personHolderPoliciesMatchModels.Any())
            {
                foreach (PolicyModel policyModel in personHolderPoliciesMatchModels)
                {
                    PersonRoleEnum roleId = PersonRoleEnum.PolicyHolder;

                    if (policyModel.InsuredId == individualId)
                    {
                        roleId = PersonRoleEnum.PolicyInsured;
                    }
                    else if (policyModel.AgentId == individualId)
                    {
                        roleId = PersonRoleEnum.PolicyAgent;
                    }
                    else if (policyModel.PolicyHolderId == individualId)
                    {
                        roleId = PersonRoleEnum.PolicyHolder;
                    }
                    else if (policyModel.BeneficiaryId == individualId)
                    {
                        roleId = PersonRoleEnum.PolicyBeneficiary;
                    }

                    matchingProcessDAO.SaveListRiskMatchCompany(companyModel, _processId, Enum.ProcessType.Policy, roleId, policyModel.DocumentNumber.ToString(), policyModel.IssueDate, policyModel.CurrentFrom, policyModel.CurrentTo);

                    IEnumerable<ReinsurancePolicyModel> reinsurancePolicyMatchModels = _MoveMatchPerson.Reinsurances.ReinsurancePolicies.Where(x => x.PoliciyId == policyModel.PolicyId);
                    if (reinsurancePolicyMatchModels.Any())
                    {
                        matchingProcessDAO.SaveListRiskMatchCompany(companyModel, _processId, Enum.ProcessType.Policy, Enum.PersonRoleEnum.Reinsured, policyModel.DocumentNumber.ToString(), policyModel.IssueDate, policyModel.CurrentFrom, policyModel.CurrentTo);
                    }

                }
            }
        }

        public void ValidateClaimsPerson(PersonModel personModel)
        {

            int individualId = personModel.IndividualId;
            IEnumerable<ClaimModel> claimMatchModels = _MoveMatchPerson.Claims.Where(x => x.IndividualId == individualId);

            if (claimMatchModels.Any())
            {
                //Registro persona con siniestros

                foreach (ClaimModel claimModel in claimMatchModels)
                {
                    matchingProcessDAO.SaveListRiskMatchPerson(personModel, _processId, Enum.ProcessType.Claim, Enum.PersonRoleEnum.Beneficiary, claimModel.ClaimNumber.ToString(), claimModel.RegistrationDate, claimModel.RegistrationDate, claimModel.RegistrationDate);

                    IEnumerable<ReinsuranceClaimModel> reinsuranceClaimMatchModels = _MoveMatchPerson.Reinsurances.ReinsuranceClaims.Where(x => x.ClaimId == claimModel.ClaimCd);
                    if (reinsuranceClaimMatchModels.Any())
                    {
                        matchingProcessDAO.SaveListRiskMatchPerson(personModel, _processId, Enum.ProcessType.Claim, Enum.PersonRoleEnum.Reinsured, claimModel.ClaimNumber.ToString(), claimModel.RegistrationDate, claimModel.RegistrationDate, claimModel.RegistrationDate);
                    }
                }
            }
        }

        public void ValidateClaimsCompany(CompanyModel companyModel)
        {

            int individualId = companyModel.IndividualId;
            IEnumerable<ClaimModel> claimMatchModels = _MoveMatchPerson.Claims.Where(x => x.IndividualId == individualId);

            if (claimMatchModels.Any())
            {
                foreach (ClaimModel claimModel in claimMatchModels)
                {
                    matchingProcessDAO.SaveListRiskMatchCompany(companyModel, _processId, Enum.ProcessType.Payment, Enum.PersonRoleEnum.Beneficiary, claimModel.ClaimNumber.ToString(), claimModel.RegistrationDate, claimModel.RegistrationDate, claimModel.RegistrationDate);

                    IEnumerable<ReinsuranceClaimModel> ReinsuranceClaimMatchModels = _MoveMatchPerson.Reinsurances.ReinsuranceClaims.Where(x => x.ClaimId == claimModel.ClaimCd);
                    if (ReinsuranceClaimMatchModels.Any())
                    {
                        matchingProcessDAO.SaveListRiskMatchCompany(companyModel, _processId, Enum.ProcessType.Payment, Enum.PersonRoleEnum.Beneficiary, claimModel.ClaimNumber.ToString(), claimModel.RegistrationDate, claimModel.RegistrationDate, claimModel.RegistrationDate);
                    }
                }
            }
        }

        public void ValidatePaymentsPerson(PersonModel personModel)
        {
            int individualId = personModel.IndividualId;
            IEnumerable<PaymentModel> paymentMatchModels = _MoveMatchPerson.Payments.Where(x => x.IndividualId == individualId);

            if (paymentMatchModels.Any())
            {
                foreach (PaymentModel paymentModel in paymentMatchModels)
                {
                    matchingProcessDAO.SaveListRiskMatchPerson(personModel, _processId, Enum.ProcessType.Payment, Enum.PersonRoleEnum.Beneficiary, paymentModel.PaymentRequestNumber.ToString(), paymentModel.PaymentDate, paymentModel.PaymentDate, paymentModel.PaymentDate);

                    IEnumerable<ReinsurancePaymentModel> ReinsurancePaymentMatchModels = _MoveMatchPerson.Reinsurances.ReinsurancePayments.Where(x => x.PaymentRequestId == paymentModel.PaymentRequestId);
                    if (ReinsurancePaymentMatchModels.Any())
                    {
                        //Registro persona con pago reasegurado
                        matchingProcessDAO.SaveListRiskMatchPerson(personModel, _processId, Enum.ProcessType.Payment, Enum.PersonRoleEnum.Reinsured, paymentModel.PaymentRequestNumber.ToString(), paymentModel.PaymentDate, paymentModel.PaymentDate, paymentModel.PaymentDate);
                    }
                }
            }
        }

        public void ValidatePaymentsCompany(CompanyModel companyModel)
        {

            IEnumerable<PaymentModel> paymentMatchModels = _MoveMatchPerson.Payments.Where(x => x.IndividualId == companyModel.IndividualId);

            if (paymentMatchModels.Any())
            {
                foreach (PaymentModel paymentModel in paymentMatchModels)
                {
                    matchingProcessDAO.SaveListRiskMatchCompany(companyModel, _processId, Enum.ProcessType.Payment, Enum.PersonRoleEnum.Beneficiary, paymentModel.PaymentRequestNumber.ToString(), paymentModel.PaymentDate, paymentModel.PaymentDate, paymentModel.PaymentDate);

                    IEnumerable<ReinsurancePaymentModel> ReinsurancePaymentMatchModels = _MoveMatchPerson.Reinsurances.ReinsurancePayments.Where(x => x.PaymentRequestId == paymentModel.PaymentRequestId);

                    if (ReinsurancePaymentMatchModels.Any())
                    {
                        matchingProcessDAO.SaveListRiskMatchCompany(companyModel, _processId, Enum.ProcessType.Payment, Enum.PersonRoleEnum.Reinsured, paymentModel.PaymentRequestNumber.ToString(), paymentModel.PaymentDate, paymentModel.PaymentDate, paymentModel.PaymentDate);
                    }
                }
            }
        }

        #endregion
    }
}