using Newtonsoft.Json;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Company.Application.Utilities.Constants;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Company.Application.MassiveServices.EEProvider.DAOs
{
    public class ReportDAO
    {
        #region Reportes

        public CompanyPolicy GetCompanyPolicyByMassiveLoadStatusPolicy(MassiveLoadStatus massiveLoadStatus, Policy policy)
        {
            CompanyPolicy companyPolicy = null;
            switch (massiveLoadStatus)
            {
                case MassiveLoadStatus.Tariffed:
                    PendingOperation pendingOperation = DelegateService.utilitiesService.GetPendingOperationById(policy.Id);
                    companyPolicy = JsonConvert.DeserializeObject<CompanyPolicy>(pendingOperation.Operation);
                    break;
                case MassiveLoadStatus.Issued:
                    companyPolicy = JsonConvert.DeserializeObject<CompanyPolicy>(DelegateService.underwritingService.GetPolicyByEndorsementDocumentNumber(policy.Endorsement.Id, policy.DocumentNumber));
                    if (!Settings.UseReplicatedDatabase())
                    {
                        companyPolicy = JsonConvert.DeserializeObject<CompanyPolicy>(DelegateService.underwritingService.GetPolicyByEndorsementDocumentNumber(policy.Endorsement.Id, policy.DocumentNumber));
                    }
                    else
                    {
                        companyPolicy = JsonConvert.DeserializeObject<CompanyPolicy>(DelegateService.underwritingService.GetPolicyByEndorsementDocumentNumber(policy.Endorsement.Id, policy.DocumentNumber));
                    }
                    companyPolicy.DocumentNumber = policy.DocumentNumber;
                    break;
            }

            return companyPolicy;

        }

        public List<Field> GetFields(string serializeFields, CompanyPolicy companyPolicy)
        {
            List<IssuanceDocumentType> documentTypes = (List<IssuanceDocumentType>)GetCacheList(CacheType.DocumentType, "DocumentTypes");
            List<Branch> branches = (List<Branch>)InProcCache.Get(CacheType.Branch, "Branches");
            List<Field> fields = JsonConvert.DeserializeObject<List<Field>>(serializeFields);


            fields.Find(u => u.PropertyName == FieldPropertyName.PolicyBranchCode).Value = companyPolicy.Branch.Id.ToString();
            fields.Find(u => u.PropertyName == FieldPropertyName.PolicyBranchDescription).Value = branches.FirstOrDefault(u => u.Id == companyPolicy.Branch.Id).Description;
            fields.Find(u => u.PropertyName == FieldPropertyName.PolicyPrefixCode).Value = companyPolicy.Prefix.Id.ToString();
            if (companyPolicy.Branch.SalePoints != null)
            {
                fields.Find(u => u.PropertyName == FieldPropertyName.SalePoint).Value = companyPolicy.Branch.SalePoints[0].Id.ToString();
            }
            fields.Find(u => u.PropertyName == FieldPropertyName.PolicyTemporal).Value = companyPolicy.Id.ToString();
            fields.Find(u => u.PropertyName == FieldPropertyName.PolicyType).Value = companyPolicy.PolicyType.Description ?? "";
            fields.Find(u => u.PropertyName == FieldPropertyName.PolicyBusinessTypeDescription).Value = EnumHelper.GetDescription(companyPolicy.BusinessType);
            fields.Find(u => u.PropertyName == FieldPropertyName.BillingGroup).Value = companyPolicy.BillingGroup != null ? companyPolicy.BillingGroup.Description : "";
            fields.Find(u => u.PropertyName == FieldPropertyName.RequestGroup).Value = companyPolicy.Request != null ? companyPolicy.Request.Id.ToString() : "";
            fields.Find(u => u.PropertyName == FieldPropertyName.PolicyIssueDate).Value = companyPolicy.IssueDate.ToString();
            fields.Find(u => u.PropertyName == FieldPropertyName.PolicyEndorsementTypeDescription).Value = EnumHelper.GetDescription(companyPolicy.Endorsement.EndorsementType);
            if (companyPolicy.DocumentNumber > 0)
            {
                fields.Find(u => u.PropertyName == FieldPropertyName.PolicyNumber).Value = companyPolicy.DocumentNumber.ToString();
            }
            fields.Find(u => u.PropertyName == FieldPropertyName.PolicyCurrentFrom).Value = companyPolicy.CurrentFrom.ToString();
            fields.Find(u => u.PropertyName == FieldPropertyName.PolicyCurrentTo).Value = companyPolicy.CurrentTo.ToString();
            fields.Find(u => u.PropertyName == FieldPropertyName.PolicyNumberDays).Value = companyPolicy.Endorsement.EndorsementDays.ToString();
            fields.Find(u => u.PropertyName == FieldPropertyName.PolicyCurrencyDescription).Value = companyPolicy.ExchangeRate?.Currency?.Description?.ToString();
            fields.Find(u => u.PropertyName == FieldPropertyName.PolicyExchangeRate).Value = companyPolicy.ExchangeRate.SellAmount.ToString();
            fields.Find(u => u.PropertyName == FieldPropertyName.PolicyProductDescription).Value = companyPolicy.Product?.Description;
            //fields.Find(u => u.PropertyName == FieldPropertyName.PolicyClauses).Value = CreateClauses(companyPolicy.Clauses);//pendiente;
            fields.Find(u => u.PropertyName == FieldPropertyName.PolicyUserName).Value = DelegateService.uniqueUserService.GetUserById(companyPolicy.UserId).AccountName;

            fields.Find(u => u.PropertyName == FieldPropertyName.DayPaymentFirstQuota).Value = companyPolicy.PaymentPlan.Quotas.First().ExpirationDate.ToString();
            fields.Find(u => u.PropertyName == FieldPropertyName.MethodOfPaymentDescription).Value = companyPolicy.PaymentPlan.Description ?? "";
            string premiumFormated = companyPolicy.Summary.Premium.ToString("F2");
            fields.Find(u => u.PropertyName == FieldPropertyName.PolicyPremium).Value = premiumFormated;
            string expensesFormated = companyPolicy.Summary.Expenses.ToString("F2");
            fields.Find(u => u.PropertyName == FieldPropertyName.PolicyExpensesWithAssistance).Value = expensesFormated;
            fields.Find(u => u.PropertyName == FieldPropertyName.PolicyVAT).Value = companyPolicy.Summary.Taxes.ToString("F2");
            string fullPremiumFormated = companyPolicy.Summary.FullPremium.ToString("F2");
            fields.Find(u => u.PropertyName == FieldPropertyName.PolicyTotalToPay).Value = fullPremiumFormated;

            fields.Find(u => u.PropertyName == FieldPropertyName.IntermediaryData).Value = CreateAgents(companyPolicy.Agencies);
            fields.Find(u => u.PropertyName == FieldPropertyName.CoinsuranceData).Value = CreateCoinsurance(companyPolicy);

            

            fields.Find(u => u.PropertyName == FieldPropertyName.Observations).Value = "";
            fields = CreateHolder(fields, companyPolicy);
            return fields;


        }

        public string CreateBeneficiaries(List<CompanyBeneficiary> beneficiares)
        {
            List<IssuanceDocumentType> documentTypes = (List<IssuanceDocumentType>)GetCacheList(CacheType.DocumentType, "DocumentTypes");
            List<CompanyBeneficiaryType> beneficiaryTypes = (List<CompanyBeneficiaryType>)GetCacheList(CacheType.BeneficiaryType, "BeneficiaryTypes");
            List<City> city = (List<City>)GetCacheList(CacheType.City, "City");
            List<State> states = (List<State>)GetCacheList(CacheType.State, "State");

            string value = "";
            if (beneficiares != null)
            {
                foreach (CompanyBeneficiary beneficiary in beneficiares)
                {
                    string documentType = documentTypes.FirstOrDefault(u => u.Id == beneficiary.IdentificationDocument?.DocumentType?.Id)?.SmallDescription ?? "";
                    string beneficiaryType = beneficiaryTypes.FirstOrDefault(u => u.Id == beneficiary?.BeneficiaryType?.Id)?.SmallDescription ?? "";
                    value += beneficiary?.Name?.ToUpper() + " | " + documentType + ". " + beneficiary.IdentificationDocument?.Number + " | " + beneficiaryType + "|";
                    if (beneficiary.CompanyName != null)
                    {
                        if (beneficiary.CompanyName.Address != null)
                        {
                            string address = beneficiary.CompanyName.Address != null ? beneficiary.CompanyName.Address.Description.ToUpper() : "";
                            if (beneficiary.CompanyName.Address.City != null && beneficiary.CompanyName.Address.City.State != null)
                            {
                                address += "| " + states.FirstOrDefault(u => u.Id == beneficiary.CompanyName.Address.City.State.Id && u.Country.Id == beneficiary.CompanyName.Address.City.State.Country.Id).Description;
                                address += "| " + city.FirstOrDefault(u => u.Id == beneficiary.CompanyName.Address.City.Id && u.State.Id == beneficiary.CompanyName.Address.City.State.Id).Description;
                            }
                            value += address;
                        }
                        if (beneficiary.CompanyName.Phone != null)
                        {
                            value += " | " + beneficiary.CompanyName.Phone.Description;
                        }

                        value += " | " + beneficiary.Participation;
                    }
                    value += " _ ";
                }
            }
            return value;
        }

        private string CreateAgents(List<IssuanceAgency> agencies)
        {
            string value = "";
            if (agencies != null)
            {
                List<IssuanceAgency> agencyCache = (List<IssuanceAgency>)GetCacheList(CacheType.Agency, "Agency");
                foreach (IssuanceAgency agency in agencies)
                {
                    string nameAgency = agencyCache.FirstOrDefault(u => u.Code == agency?.Code)?.FullName ?? "";
                    value += nameAgency + " | " + agency.Participation + " | " + agency.Commissions[0].Percentage;
                    value += " _ ";
                }
            }
            return value;
        }

        private string CreateCoinsurance(CompanyPolicy companyPolicy)
        {
            string value = "";
            if (companyPolicy.CoInsuranceCompanies != null)
            {
                decimal fullPremium = companyPolicy.Summary.Premium;
                if (companyPolicy.BusinessType.Value == BusinessType.Accepted)
                {
                    fullPremium = companyPolicy.Summary.Premium + companyPolicy.Summary.Expenses;
                }

                foreach (IssuanceCoInsuranceCompany coInsuranceCompany in companyPolicy.CoInsuranceCompanies)
                {
                    if (coInsuranceCompany.Id > 0)
                    {

                        IssuanceCoInsuranceCompany coInsurance = DelegateService.underwritingService.GetCoInsuranceCompanyByCoinsuranceId((int)coInsuranceCompany.Id);
                        if (companyPolicy.BusinessType.Value == BusinessType.Accepted)
                        {
                            if (coInsuranceCompany.ParticipationPercentageOwn != 0)
                            {
                                fullPremium = Math.Round((fullPremium * 100 / coInsuranceCompany.ParticipationPercentageOwn), 2);
                                fullPremium = fullPremium - (companyPolicy.Summary.Premium + companyPolicy.Summary.Expenses);
                            }
                        }

                        else
                        {
                            fullPremium = Math.Round((fullPremium * coInsuranceCompany.ParticipationPercentage / 100), 2);
                        }

                        value += Convert.ToInt16(coInsuranceCompany.Id) + " | " + (coInsurance != null ? coInsurance.Description : "") + " | " + coInsuranceCompany.ParticipationPercentage + " | " + fullPremium + " | " + coInsuranceCompany.ExpensesPercentage;
                        value += " _ ";
                    }

                }
            }
            return value;
        }

        private List<Field> CreateHolder(List<Field> fields, CompanyPolicy companyPolicy)
        {
            List<IssuanceDocumentType> documentTypes = (List<IssuanceDocumentType>)GetCacheList(CacheType.DocumentType, "DocumentTypes");
            List<City> city = (List<City>)GetCacheList(CacheType.City, "City");
            List<State> states = (List<State>)GetCacheList(CacheType.State, "State");

            string address = "";
            if (companyPolicy.Holder.CompanyName != null && companyPolicy.Holder.CompanyName.Address != null)
            {
                address += companyPolicy.Holder.CompanyName.Address.Description.ToUpper() ?? "";
                if (companyPolicy.Holder.CompanyName.Address.City != null && companyPolicy.Holder.CompanyName.Address.City.State != null)
                {
                    address += "| " + states.FirstOrDefault(u => u.Id == companyPolicy.Holder.CompanyName.Address.City.State.Id && u.Country.Id == companyPolicy.Holder.CompanyName.Address.City.State.Country.Id).Description;
                    address += "| " + city.FirstOrDefault(u => u.Id == companyPolicy.Holder.CompanyName.Address.City.Id && u.State.Id == companyPolicy.Holder.CompanyName.Address.City.State.Id).Description;
                }
            }
            else
            {
                address += "Dirección no Encontrada";
            }

            fields.Find(u => u.PropertyName == FieldPropertyName.HolderAddressDescription).Value = address.ToUpper();
            fields.Find(u => u.PropertyName == FieldPropertyName.HolderDescription).Value = companyPolicy.Holder.Name.ToUpper();

            if (companyPolicy.Holder.CompanyName != null && companyPolicy.Holder.CompanyName.Phone != null)
            {
                fields.Find(u => u.PropertyName == FieldPropertyName.HolderPhoneDescription).Value = companyPolicy.Holder.CompanyName.Phone.Description ?? "";
            }
            if (documentTypes != null)
            {
                fields.Find(u => u.PropertyName == FieldPropertyName.HolderDocument).Value = documentTypes.FirstOrDefault(u => u.Id == companyPolicy.Holder.IdentificationDocument.DocumentType.Id).SmallDescription + ". " + companyPolicy.Holder.IdentificationDocument.Number;
            }
            else
            {
                fields.Find(u => u.PropertyName == FieldPropertyName.HolderDocument).Value = "";
            }

            return fields;
        }
        public string CreateClauses(List<CompanyClause> clauses)
        {
            string value = "";
            if (clauses != null)
            {
                List<Clause> clausesCache = (List<Clause>)GetCacheList(CacheType.Clauses, "Clauses");
                foreach (CompanyClause clause in clauses)
                {
                    string nameClause = clausesCache.FirstOrDefault(u => u.Id == clause?.Id)?.Name ?? "";
                    value += nameClause + " | ";
                }
            }
            return value;
        }

        public List<Field> CreateInsured(List<Field> fields, CompanyIssuanceInsured mainInsured)
        {
            List<City> city = (List<City>)GetCacheList(CacheType.City, "City");
            List<State> states = (List<State>)GetCacheList(CacheType.State, "State");

            IssuanceCompanyName companyName = mainInsured?.CompanyName;
            if (mainInsured != null)
            {
                fields.Find(u => u.PropertyName == FieldPropertyName.InsuredDocument).Value = DocumentTypes.DefaultIfEmpty(new IssuanceDocumentType { SmallDescription = "CC" }).First(u => u.Id == mainInsured.IdentificationDocument.DocumentType.Id).SmallDescription + ". " + mainInsured.IdentificationDocument.Number;
                fields.Find(u => u.PropertyName == FieldPropertyName.InsuredDescription).Value = mainInsured.Name.ToUpper();
                fields.Find(u => u.PropertyName == FieldPropertyName.InsuredPhoneDescription).Value = (companyName?.Phone != null) ? companyName.Phone.Description : "";
                if (fields.Find(u => u.PropertyName == FieldPropertyName.InsuredGender) != null && fields.Find(u => u.PropertyName == FieldPropertyName.InsuredAge) != null)
                {
                    if (mainInsured.IndividualType == IndividualType.Person)
                    {
                        fields.Find(u => u.PropertyName == FieldPropertyName.InsuredGender).Value = mainInsured.Gender.ToUpper();
                        int age = DateTime.Now.Year - mainInsured.BirthDate.Value.Year;
                        DateTime birthDate = mainInsured.BirthDate.Value;
                        if (DateTime.Now.Month < birthDate.Month || (DateTime.Now.Month == birthDate.Month && DateTime.Now.Day < birthDate.Day))
                        {
                            age--;
                        }
                        fields.Find(u => u.PropertyName == FieldPropertyName.InsuredAge).Value = age.ToString();
                    }
                }
            }
            string address = companyName.Address.Description.ToUpper() ?? "";
            if (companyName != null)
            {
                if (companyName.Address.City != null && companyName.Address.City.State != null)
                {
                    address += "| " + states.FirstOrDefault(u => u.Id == companyName.Address.City.State.Id && u.Country.Id == companyName.Address.City.State.Country.Id).Description;
                    address += "| " + city.FirstOrDefault(u => u.Id == companyName.Address.City.Id && u.State.Id == companyName.Address.City.State.Id).Description;
                }
            }
            fields.Find(u => u.PropertyName == FieldPropertyName.InsuredAddressDescription).Value = address.ToUpper();
            return fields;
        }

        #endregion


        #region CACHE
        public void LoadReportCacheList()
        {

            var documentTypes = InProcCache.Get(CacheType.DocumentType, "DocumentTypes");
            if (documentTypes == null)
            {
                InProcCache.Add(CacheType.DocumentType, "DocumentTypes", DelegateService.underwritingService.GetDocumentTypes(3));
            }

            var deductibles = InProcCache.Get(CacheType.Deductible, "Deductibles");
            if (deductibles == null)
            {
                InProcCache.Add(CacheType.Deductible, "Deductibles", DelegateService.underwritingService.GetDeductiblesAll());
            }

            var branches = InProcCache.Get(CacheType.Branch, "Branches");
            if (branches == null)
            {
                InProcCache.Add(CacheType.Branch, "Branches", DelegateService.commonService.GetBranches());
            }

            var beneficiaryTypes = InProcCache.Get(CacheType.BeneficiaryType, "BeneficiaryTypes");
            if (beneficiaryTypes == null)
            {
                InProcCache.Add(CacheType.BeneficiaryType, "BeneficiaryTypes", DelegateService.underwritingService.GetCompanyBeneficiaryTypes());
            }

            var clauses = InProcCache.Get(CacheType.Clauses, "Clauses");
            if (clauses == null)
            {
                InProcCache.Add(CacheType.Clauses, "Clauses", DelegateService.underwritingService.GetClauseAll());
            }

            var agency = InProcCache.Get(CacheType.Agency, "Agency");
            if (agency == null)
            {
                InProcCache.Add(CacheType.Agency, "Agency", DelegateService.underwritingService.GetAgencyAll());
            }

            var city = InProcCache.Get(CacheType.City, "City");
            if (city == null)
            {
                InProcCache.Add(CacheType.City, "City", DelegateService.commonService.GetCities());
            }

            var states = InProcCache.Get(CacheType.State, "State");
            if (states == null)
            {
                InProcCache.Add(CacheType.State, "State", DelegateService.commonService.GetStates());
            }
        }

        private List<IssuanceDocumentType> DocumentTypes
        {
            get
            {
                object documentType = InProcCache.Get(CacheType.DocumentType, "DocumentTypes");
                if (documentType == null)
                    return new List<IssuanceDocumentType>();
                return (List<IssuanceDocumentType>)documentType;
            }
        }


        public object GetCacheList(string type, string key)
        {

            return InProcCache.Get(type, key);
        }

        public void ClearCacheList()
        {
            InProcCache.Remove(CacheType.Branch);
            InProcCache.Remove(CacheType.Deductible);
            InProcCache.Remove(CacheType.DocumentType);
            InProcCache.Remove(CacheType.BeneficiaryType);
            InProcCache.Remove(CacheType.Clauses);
            InProcCache.Remove(CacheType.Agency);
            InProcCache.Remove(CacheType.City);
            InProcCache.Remove(CacheType.State);
        }
        #endregion
    }
}
