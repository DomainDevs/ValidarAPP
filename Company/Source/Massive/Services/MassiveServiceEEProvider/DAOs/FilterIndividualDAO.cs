using Sistran.Core.Application.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Company.Application.Utilities.Constants;
using System.Collections.Concurrent;
using System.Data;
using Sistran.Company.Application.MassiveServices.EEProvider;
using Sistran.Co.Application.Data;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Company.Application.MassiveServices.Models;
using Sistran.Company.Application.MassiveServices.EEProvider.DAOs;
using COUNMO = Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Company.Application.MassiveServices.EEProvider.Business;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Company.Application.MassiveServices.EEProvider.Resources;
using Sistran.Core.Framework;
using Sistran.Core.Services.UtilitiesServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;

namespace Sistran.Company.Application.Massive.EEProvider.DAOs
{
    public class FilterIndividualDAO
    {
        public List<FilterIndividual> GetFilterIndividualsForCollective(Row policyRow, List<File> riskFiles, int userId, string policyNumberPropertyName, string prefixIdPropertyname, int branchId)
        {
            List<FilterIndividual> filterIndividuals = new List<FilterIndividual>();
            List<FilterIndividual> filtercConcurrentIndividuals = new List<FilterIndividual>();
            if (policyRow.HasError)
            {
                return filterIndividuals;
            }

            if (policyRow.Fields.Any(x => x.PropertyName == policyNumberPropertyName))
            {
                filterIndividuals = GetDataFilterIndividualRenewalFromPolicyRow(policyRow, prefixIdPropertyname, policyNumberPropertyName);
            }
            else
            {
                filterIndividuals.Add(GetIndividualHolder(policyRow));
            }
            if (riskFiles == null)
            {
                return filterIndividuals;
            }
            ConcurrentBag<FilterIndividual> concurrentIndividuals = new ConcurrentBag<FilterIndividual>();
            var exceptions = new ConcurrentQueue<Exception>();
            ParallelHelper.ForEach(riskFiles, file =>
            {
                try
                {
                    bool hasError = file.Templates.Any(t => t.Rows.Any(r => r.HasError));
                    if (!hasError)
                    {
                        Template beneficiariesTemplate = file.Templates.Find(x => x.PropertyName == TemplatePropertyName.AdditionalBeneficiaries);
                        CreateAdditionalBeneficiaries(policyRow, beneficiariesTemplate, concurrentIndividuals);
                        Template riskDetailTemplate = file.Templates.Find(x => x.PropertyName == TemplatePropertyName.RiskDetail);
                        if (riskDetailTemplate != null)
                        {
                            CreateDetailRiskIndividual(policyRow, riskDetailTemplate, concurrentIndividuals);
                        }
                    }
                }
                catch (Exception exception)
                {
                    exceptions.Enqueue(exception);
                }
            });

            filtercConcurrentIndividuals = ValidateIndividuals(concurrentIndividuals.ToList());
            if (filtercConcurrentIndividuals.Any())
            {
                filterIndividuals.AddRange(CreateFilterIndividual(userId, branchId, GetDataFilterIndividual(filtercConcurrentIndividuals.ToList())));
            }

            filterIndividuals = ValidateIndividuals(filterIndividuals);
            return filterIndividuals;
        }

        public List<FilterIndividual> GetFilterIndividuals(int userId, List<File> files, string templatePropertyName, int branchId)
        {
            ConcurrentBag<FilterIndividual> individuals = new ConcurrentBag<FilterIndividual>();
            var exceptions = new ConcurrentQueue<Exception>();
            ParallelHelper.ForEach(files, file =>
            {
                bool hasError = file.Templates.Any(t => t.Rows.Any(r => r.HasError));

                if (!hasError)
                {
                    Row row = file.Templates.FirstOrDefault(x => x.PropertyName == templatePropertyName).Rows.FirstOrDefault();

                    DateTime policyCurrentFrom = (DateTime)DelegateService.utilitiesService.GetValueByField<DateTime>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCurrentFrom));
                    int fileBranchId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId));
                    if (fileBranchId != 0)
                    {
                        branchId = fileBranchId;
                    }

                    FilterIndividual insured = new FilterIndividual();
                    FilterIndividual beneficiary = new FilterIndividual();
                    Template beneficiariesTemplate = file.Templates.Find(x => x.PropertyName == TemplatePropertyName.AdditionalBeneficiaries);
                    bool isColective = file.Templates.Any(x => x.PropertyName == TemplatePropertyName.RiskDetail);
                    bool withRequestGrouping = row.Fields.Any(y => y.PropertyName == FieldPropertyName.RequestGroup);
                    if (withRequestGrouping)
                    {
                        int billingGroup = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BillingGroup));
                        string requestGroup = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RequestGroup));
                        CompanyRequestDAO companyRequestDAO = new CompanyRequestDAO();
                        CompanyRequest companyRequest = companyRequestDAO.GetCompanyRequestsByBillingGroupIdDescriptionRequestNumber(billingGroup, requestGroup, null).First();
                        CompanyRequestEndorsement companyRequestEndorsement = companyRequestDAO.GetCompanyRequestEndorsmentPolicyWithRequest(policyCurrentFrom, companyRequest);
                        branchId = companyRequest.Branch.Id;
                        if (ExistsHolderInExcel(row))
                        {
                            individuals.Add(GetIndividualHolder(row, companyRequestEndorsement));
                        }
                        else
                        {
                            individuals.Add(GetIndividualInsuredFromRequest(row, companyRequest));

                        }

                        insured = GetIndividualInsured(row, null, companyRequestEndorsement);
                        beneficiary = GetIndividualBeneficiary(row, row, companyRequestEndorsement);
                        CreateAdditionalBeneficiaries(row, beneficiariesTemplate, individuals, companyRequestEndorsement);
                    }
                    else
                    {
                        if (isColective)
                        {
                            Row rowDetail = file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.RiskDetail).Rows.FirstOrDefault();
                            insured = GetIndividualInsured(row, rowDetail);
                            beneficiary = GetIndividualBeneficiary(row, rowDetail);
                        }
                        else
                        {
                            insured = GetIndividualInsured(row);
                            beneficiary = GetIndividualBeneficiary(row, row);
                        }
                        individuals.Add(GetIndividualHolder(row));
                        CreateAdditionalBeneficiaries(row, beneficiariesTemplate, individuals);
                    }

                    if (insured != null)
                    {
                        individuals.Add(insured);
                    }

                    if (beneficiary != null)
                    {
                        individuals.Add(beneficiary);
                    }

                }
            });

            if (individuals.Any())
            {
                List<FilterIndividual> listIndividuals = ValidateIndividuals(individuals.ToList());
                return CreateFilterIndividual(userId, branchId, GetDataFilterIndividual(listIndividuals));
            }

            return individuals.ToList();
        }

        private List<FilterIndividual> ValidateIndividuals(List<FilterIndividual> individuals)
        {
            ConcurrentDictionary<string, FilterIndividual> individualDictionary = new ConcurrentDictionary<string, FilterIndividual>();

            if (!individuals.Any())
            {
                return individuals;
            }

            ParallelHelper.ForEach(individuals, individual =>
            {
                ValidateIndividual(individualDictionary, individual);
            });

            return new List<FilterIndividual>(individualDictionary.Values);
        }

        #region Construccion de modelos
        private FilterIndividual GetIndividualHolder(Row row, CompanyRequestEndorsement companyRequestEndorsement = null)
        {

            FilterIndividual filterMassiveIndividual = new FilterIndividual();
            int insuredCode = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderInsuredCode));
            bool isCommercialClient = (bool)DelegateService.utilitiesService.GetValueByField<bool>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderCommercialAgreement));
            bool isMailAddress = (bool)DelegateService.utilitiesService.GetValueByField<bool>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderSendingMail));
            bool isSms = (bool)DelegateService.utilitiesService.GetValueByField<bool>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderSMSSending));
            int agentCode, agentTypeCode;

            if (companyRequestEndorsement == null)
            {
                agentCode = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentCode));
                agentTypeCode = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentType));
            }
            else
            {
                agentCode = companyRequestEndorsement.Agencies.First(a => a.IsPrincipal).Id;
                agentTypeCode = companyRequestEndorsement.Agencies.First(a => a.IsPrincipal).AgentType.Id;
            }

            if (insuredCode > 0)
            {
                filterMassiveIndividual.InsuredCode = insuredCode;
            }
            else
            {
                filterMassiveIndividual.IndividualType = BusinessIndividual.FindIndividualTypeByRow(row, FieldPropertyName.HolderIndividualType, FieldPropertyName.HolderPersonDocumentNumber, FieldPropertyName.HolderCompanyDocumentNumber);
                switch (filterMassiveIndividual.IndividualType)
                {
                    case IndividualType.Company:
                        filterMassiveIndividual.Company = CreateIndividualHolderCompany(row);

                        break;
                    case IndividualType.Person:
                        filterMassiveIndividual.Person = CreateIndividualHolderPerson(row);
                        break;
                    default:
                        break;
                }

                filterMassiveIndividual.AgentCode = agentCode;
                filterMassiveIndividual.AgentTypeCode = agentTypeCode;
            }

            return filterMassiveIndividual;


        }

        public static bool ExistsHolderInExcel(Row row)
        {
            int individualId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderInsuredCode));
            string holderDocumentNumber = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderCompanyDocumentNumber));
            if (string.IsNullOrEmpty(holderDocumentNumber))
                holderDocumentNumber = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPersonDocumentNumber));

            return (individualId > 0 || !string.IsNullOrEmpty(holderDocumentNumber));
        }

        private FilterIndividual GetIndividualInsuredFromRequest(Row row, CompanyRequest companyRequest)
        {
            FilterIndividual filterIndividual = new FilterIndividual();

            try
            {
                DateTime policyCurrentFrom = (DateTime)DelegateService.utilitiesService.GetValueByField<DateTime>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCurrentFrom));
                CompanyRequestEndorsement companyRequestEndorsement = companyRequest.CompanyRequestEndorsements.Last();

                COUNMO.Holder holder = DelegateService.underwritingService.GetHoldersByDescriptionInsuredSearchTypeCustomerType(companyRequestEndorsement.Holder.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).First();
                //holder.CompanyName = DelegateService.underwritingService.GetNotificationAddressesByIndividualId(holder.IndividualId, ENUMUP.CustomerType.Individual).First();
                //if (holder.CompanyName.Address == null)
                //{
                //    List<CompanyName> listCompanyName = new List<CompanyName>();
                //    listCompanyName = DelegateService.uniquePersonService.CompanyGetNotificationAddressesByIndividualId(holder.IndividualId, ENUMUP.CustomerType.Individual).ToList();
                //    holder.CompanyName.Address = listCompanyName.FirstOrDefault().Address;
                //    holder.CompanyName.Email = listCompanyName.FirstOrDefault().Email;
                //    holder.CompanyName.Phone = listCompanyName.FirstOrDefault().Phone;
                //}
                //filterIndividual = new FilterIndividual()
                //{
                //    IndividualType = holder.IndividualType,
                //    InsuredCode = holder.InsuredId,
                //    RequestGroup = companyRequest.Id.ToString(),
                //    BillingGroup = companyRequest.BillingGroup.Id,
                //    AgentCode = companyRequestEndorsement.Agencies.First(a => a.IsPrincipal).Id,
                //    AgentTypeCode = companyRequestEndorsement.Agencies.First(a => a.IsPrincipal).AgentType.Id
                //};

                //switch (holder.IndividualType)
                //{
                //    case IndividualType.Person:
                //        filterIndividual.Person = CreateIndividualHolderPerson(holder);
                //        break;
                //    case IndividualType.Company:
                //        filterIndividual.Company = CreateIndividualHolderCompany(holder);
                //        break;
                //    case IndividualType.Naturalprospectus:
                //        filterIndividual.Person = CreateIndividualHolderPerson(holder);
                //        break;
                //    case IndividualType.Legalprospectus:
                //        filterIndividual.Company = CreateIndividualHolderCompany(holder);
                //        break;
                //}
            }
            catch (Exception ex)
            {
                filterIndividual.Error = ex.Message;
            }

            return filterIndividual;
        }

        //private MassivePerson CreateIndividualHolderPerson(COUNMO.Holder holder)
        //{
        //    return new MassivePerson()
        //    {
        //        IndividualId = holder.IndividualId,
        //        //IndividualType = holder.IndividualType,
        //    };
        //}


        private MassiveCompany CreateIndividualHolderCompany(COUNMO.Holder holder)
        {
            return new MassiveCompany()
            {
                IndividualId = holder.IndividualId,
                //IndividualType = holder.IndividualType,
            };
        }

        private FilterIndividual GetIndividualInsured(Row policyRow, Row riskRow = null, CompanyRequestEndorsement companyRequestEndorsement = null)
        {
            if (riskRow == null)
            {
                riskRow = policyRow;
            }

            FilterIndividual filterMassiveIndividual = new FilterIndividual();
            filterMassiveIndividual.IsInsured = true;
            bool insuredEqualHolder = (bool)DelegateService.utilitiesService.GetValueByField<bool>(riskRow.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.InsuredEqualHolder));
            int agentCode, agentTypeCode;

            if (companyRequestEndorsement == null)
            {
                agentCode = (int)DelegateService.utilitiesService.GetValueByField<int>(policyRow.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentCode));
                agentTypeCode = (int)DelegateService.utilitiesService.GetValueByField<int>(policyRow.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentType));
            }
            else
            {
                agentCode = companyRequestEndorsement.Agencies.First(a => a.IsPrincipal).Id;
                agentTypeCode = companyRequestEndorsement.Agencies.First(a => a.IsPrincipal).AgentType.Id;
            }

            if (!insuredEqualHolder)
            {
                int insuredCode = (int)DelegateService.utilitiesService.GetValueByField<int>(riskRow.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredCode));
                if (insuredCode > 0)
                {
                    filterMassiveIndividual.InsuredCode = insuredCode;
                }
                else
                {
                    IndividualType individualTypes = BusinessIndividual.FindIndividualTypeByRow(riskRow, FieldPropertyName.InsuredIndividualType, FieldPropertyName.InsuredPersonDocumentNumber, FieldPropertyName.InsuredCompanyDocumentNumber);
                    filterMassiveIndividual.IndividualType = individualTypes;
                    switch (individualTypes)
                    {
                        case IndividualType.Company:
                            filterMassiveIndividual.Company = CreateIndividualInsuredCompany(riskRow);
                            break;
                        case IndividualType.Person:
                            filterMassiveIndividual.Person = CreateIndividualInsuredPerson(riskRow);
                            break;
                        default:
                            return null;
                    }

                    filterMassiveIndividual.AgentCode = agentCode;
                    filterMassiveIndividual.AgentTypeCode = agentTypeCode;
                }

                return filterMassiveIndividual;
            }
            else
            {
                return null;
            }
        }

        private FilterIndividual GetIndividualBeneficiary(Row row, Row benRow, CompanyRequestEndorsement companyRequestEndorsement = null)
        {
            FilterIndividual filterMassiveIndividual = new FilterIndividual();

            bool beneficiaryEqualInsured = (bool)DelegateService.utilitiesService.GetValueByField<bool>(benRow.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.BeneficiaryEqualInsured));

            if (companyRequestEndorsement == null)
            {
                filterMassiveIndividual.AgentCode = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentCode));
                filterMassiveIndividual.AgentTypeCode = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentType));
            }
            else
            {
                filterMassiveIndividual.AgentCode = companyRequestEndorsement.Agencies.First(a => a.IsPrincipal).Id;
                filterMassiveIndividual.AgentTypeCode = companyRequestEndorsement.Agencies.First(a => a.IsPrincipal).AgentType.Id;
            }

            if (!beneficiaryEqualInsured)
            {
                int insuredCode = (int)DelegateService.utilitiesService.GetValueByField<int>(benRow.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryInsuredCode));

                if (insuredCode > 0)
                {
                    filterMassiveIndividual.InsuredCode = insuredCode;
                }
                else
                {
                    IndividualType individualType = BusinessIndividual.FindIndividualTypeByRow(benRow, FieldPropertyName.BeneficiaryIndividualType, FieldPropertyName.BeneficiaryPersonDocumentNumber, FieldPropertyName.BeneficiaryCompanyDocumentNumber);
                    filterMassiveIndividual.IndividualType = individualType;
                    
                    switch (individualType)
                    {
                        case IndividualType.Company:
                            filterMassiveIndividual.Company = CreateIndividualBeneficiaryCompany(benRow);
                            break;
                        case IndividualType.Person:
                            filterMassiveIndividual.Person = CreateIndividualBeneficaryPerson(benRow);
                            break;
                        default:
                            return null;
                    }
                }

                return filterMassiveIndividual;
            }
            else
            {
                return null;
            }
        }

        private MassivePerson CreateIndividualHolderPerson(Row row)
        {
            MassivePerson person = new MassivePerson
            {
                IndividualType = IndividualType.Person,
                CustomerType = CustomerType.Individual,
                DocumentTypeId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPersonDocumentType)),
                DocumentNumber = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPersonDocumentNumber))
            };
            
            return person;
        }

        private MassiveCompany CreateIndividualHolderCompany(Row row)
        {
            MassiveCompany company = new MassiveCompany
            {
                IndividualType = IndividualType.Company,
                CustomerType = CustomerType.Individual,
                DocumentTypeId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderCompanyDocumentType)),
                DocumentNumber = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderCompanyDocumentNumber))
            };
            
            return company;
        }

        private MassivePerson CreateIndividualInsuredPerson(Row row)
        {
            MassivePerson person = new MassivePerson
            {
                IndividualType = IndividualType.Person,
                CustomerType = CustomerType.Individual,
                DocumentTypeId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPersonDocumentType)),
                DocumentNumber = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPersonDocumentNumber))
            };

            return person;
        }

        private MassiveCompany CreateIndividualInsuredCompany(Row row)
        {
            MassiveCompany company = new MassiveCompany
            {
                IndividualType = IndividualType.Company,
                CustomerType = CustomerType.Individual,
                DocumentTypeId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredCompanyDocumentType)),
                DocumentNumber = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredCompanyDocumentNumber))
            };

            return company;
        }

        private MassivePerson CreateIndividualBeneficaryPerson(Row row)
        {
            MassivePerson person = new MassivePerson
            {
                IndividualType = IndividualType.Person,
                CustomerType = CustomerType.Individual,
                DocumentTypeId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPersonDocumentType)),
                DocumentNumber = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPersonDocumentNumber))
            };

            return person;
        }

        private MassiveCompany CreateIndividualBeneficiaryCompany(Row row)
        {
            MassiveCompany company = new MassiveCompany
            {
                IndividualType = IndividualType.Company,
                CustomerType = CustomerType.Individual,
                DocumentTypeId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryCompanyDocumentType)),
                DocumentNumber = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryCompanyDocumentNumber))
            };

            return company;
        }

        private void CreateAdditionalBeneficiaries(Row row, Template beneficiariesTemplate, ConcurrentBag<FilterIndividual> individuals, CompanyRequestEndorsement companyRequestEndorsement = null)
        {
            if (beneficiariesTemplate != null)
            {
                foreach (Row benRow in beneficiariesTemplate.Rows)
                {
                    FilterIndividual beneficiary = GetIndividualBeneficiary(row, benRow, companyRequestEndorsement);
                    if (beneficiary != null)
                    {
                        individuals.Add(beneficiary);
                    }
                }
            }
        }
        
        private void CreateDetailRiskIndividual(Row row, Template riskDetailTemplate, ConcurrentBag<FilterIndividual> individuals)
        {

            foreach (Row riskRow in riskDetailTemplate.Rows)
            {
                FilterIndividual insured = GetIndividualInsured(riskRow);
                if (insured != null)
                {
                    individuals.Add(insured);
                }

                FilterIndividual beneficiary = GetIndividualBeneficiary(row, riskRow);
                if (beneficiary != null)
                {
                    individuals.Add(beneficiary);
                }
            }


        }

        private MassiveCompany CreateCompanyByDataRow(DataRow row)
        {
            MassiveCompany company = new MassiveCompany();

            company.IndividualId = GetRowValue<int>(row["INDIVIDUAL_ID"]);
            company.IndividualType = IndividualType.Company;
            company.CustomerType = CustomerType.Individual;
            company.IndividualId = GetRowValue<int>(row["INDIVIDUAL_ID"]);
            company.FullName = row["TRADE_NAME"].ToString();
            company.EconomicActivityId = GetRowValue<int>(row["ECONOMIC_ACTIVITY_CD"]);
            company.DocumentTypeId = GetRowValue<int>(row["TRIBUTARY_ID_TYPE_CD"]);
            company.DocumentNumber = row["TRIBUTARY_ID_NO"].ToString();
            company.CompanyTypeId = GetRowValue<int>(row["COMPANY_TYPE_CD"]);
            company.CountryOrigin = new Country
            {
                Id = GetRowValue<int>(row["COUNTRY_CD"]),
                Description = row["DESCRIPTION_COUNTRY"].ToString()
            };
            company.PaymentId = 1;
            company.PaymentMethodId = GetRowValue<int>(row["PAYMENT_METHOD_CD"]);

            company.AddressId = GetRowValue<int>(row["DATA_ID_ADDRESS"]);
            company.AddressTypeId = GetRowValue<int>(row["ADDRESS_TYPE_CD"]);
            company.AddressDescription = row["STREET"].ToString();
            company.AddressCity = new City()
            {
                Id = GetRowValue<int>(row["CITY_CD"]),
                Description = row["DESCRIPTION_CITY"].ToString(),
                State = new State()
                {
                    Id = GetRowValue<int>(row["STATE_CD"]),
                    Description = row["DESCRIPTION_STATE"].ToString(),
                    Country = new Country
                    {
                        Id = GetRowValue<int>(row["COUNTRY_CD"]),
                        Description = row["DESCRIPTION_COUNTRY"].ToString()
                    }
                }
            };

            company.PhoneId = GetRowValue<int>(row["DATA_ID_PHONE"]);
            company.PhoneTypeId = 1;
            company.PhoneDescription = row["PHONE_NUMBER"].ToString();

            company.EmailId = GetRowValue<int>(row["DATA_ID_EMAIL"]);
            company.EmailTypeId = GetRowValue<int>(row["EMAIL_TYPE_CD"]);
            company.EmailDescription = row["ADDRESS_MAIL"].ToString();

            return company;
        }

        private MassivePerson CreatePersonByDataRow(DataRow row)
        {
            MassivePerson person = new MassivePerson
            {
                IndividualType = IndividualType.Person,
                CustomerType = CustomerType.Individual,
                IndividualId = GetRowValue<int>(row["INDIVIDUAL_ID"]),
                Name = row["NAME"].ToString(),
                Surname = row["SURNAME"].ToString(),
                SecondSurname = row["SURNAME2"].ToString(),
                EducativeLevelId = 1,
            HouseTypeId = 1,
                SocialLayerId = 1,
                LaborPersonId = 1,
                
        };
            person.FullName = StringHelper.ConcatenateString(person.Name, " ", person.Surname, " ", person.SecondSurname);//pendiente definicion de este campo
            person.EconomicActivityId = GetRowValue<int>(row["ECONOMIC_ACTIVITY_CD"]);

            if (person.IndividualType == IndividualType.Person)
            {
                person.DocumentTypeId = GetRowValue<int>(row["INDIVIDUAL_TYPE_CD"]);
                person.DocumentNumber = row["ID_CARD_NO"].ToString();
            }
            if (person.IndividualType == IndividualType.Company)
            {
                person.DocumentTypeId = GetRowValue<int>(row["TRIBUTARY_ID_TYPE_CD"]);
                person.DocumentNumber = row["ID_CARD_NO"].ToString();
            }
            person.Gender = row["GENDER"].ToString();
            person.MaritalStatusId = GetRowValue<int>(row["MARITAL_STATUS_CD"]);
            person.BirthDate = GetRowValue<DateTime>(row["BIRTH_DATE"]);

            person.PaymentId = 1;
            person.PaymentMethodId = GetRowValue<int>(row["PAYMENT_METHOD_CD"]);
            
            person.AddressId = GetRowValue<int>(row["DATA_ID_ADDRESS"]);
            person.AddressTypeId = GetRowValue<int>(row["ADDRESS_TYPE_CD"]);
            person.AddressDescription = row["STREET"].ToString();
            person.AddressCity = new City()
            {
                Id = GetRowValue<int>(row["CITY_CD"]),
                Description = row["DESCRIPTION_CITY"].ToString(),
                State = new State()
                {
                    Id = GetRowValue<int>(row["STATE_CD"]),
                    Description = row["DESCRIPTION_STATE"].ToString(),
                    Country = new Country
                    {
                        Id = GetRowValue<int>(row["COUNTRY_CD"]),
                        Description = row["DESCRIPTION_COUNTRY"].ToString()
                    }
                }
            };
            person.PhoneTypeId = GetRowValue<int>(row["DATA_ID_PHONE"]);
            person.IndividualId = GetRowValue<int>(row["INDIVIDUAL_ID"]);

            return person;
        }

        private void UpdateFilterIndividualCompanyByDataRow(FilterIndividual filter, DataRow row)
        {
            filter.Company.IndividualId = GetRowValue<int>(row["INDIVIDUAL_ID"]);
            filter.InsuredCode = GetRowValue<int>(row["INSURED_CD"]);
            filter.Company.AddressId = GetRowValue<int>(row["ADDRESS_ID"]);
            filter.Company.PhoneId = GetRowValue<int>(row["PHONE_ID"]);
            filter.Company.EmailId = GetRowValue<int>(row["EMAIL_ID"]);
        }
        #endregion

        #region Llamadas Store Procedure
        public List<FilterIndividual> GetDataFilterIndividual(List<FilterIndividual> filterIndividuals)
        {
            var parameters = new NameValue[3];

            DataTable dtIndividuals = new DataTable("PARAM_INSURED_CD");
            dtIndividuals.Columns.Add("INDIVIDUAL_TYPE_CD", typeof(int));
            dtIndividuals.Columns.Add("INSURED_CD", typeof(int));
            dtIndividuals.Columns.Add("DOCUMENT_TYPE_CD", typeof(int));
            dtIndividuals.Columns.Add("DOCUMENT_NUM", typeof(string));
            
            foreach (FilterIndividual item in filterIndividuals)
            {
                
               
                DataRow dataRow = dtIndividuals.NewRow();
                dataRow["INSURED_CD"] = DBNull.Value;
                dataRow["INDIVIDUAL_TYPE_CD"] = item.IndividualType;
                dataRow["DOCUMENT_TYPE_CD"] = DBNull.Value;
                dataRow["DOCUMENT_NUM"] = DBNull.Value; ;
                if (item.InsuredCode.HasValue && item.InsuredCode > 0)
                {
                    dataRow["INSURED_CD"] = item.InsuredCode;
                }
                else
                {
                    if (item.Person != null && item.IndividualType == IndividualType.Person)
                    {
                        dataRow["DOCUMENT_TYPE_CD"] = item.Person.DocumentTypeId;
                        dataRow["DOCUMENT_NUM"] = item.Person.DocumentNumber;
                    }
                    else
                    {
                        if (item.Company != null && item.IndividualType == IndividualType.Company)
                        {
                            dataRow["DOCUMENT_TYPE_CD"] = item.Company.DocumentTypeId;
                            dataRow["DOCUMENT_NUM"] = item.Company.DocumentNumber;
                        }
                    }
                }
                dtIndividuals.Rows.Add(dataRow);

            }
            parameters[0] = new NameValue("@VALIDATE_PARAM_INSURED_CD", dtIndividuals);
            parameters[1] = new NameValue("@LINK_SERVER", DBNull.Value);
            parameters[2] = new NameValue("@LINK_DATABASE", DBNull.Value);

            if (Settings.UseReplicatedDatabase())
            {
                parameters[1] = new NameValue("@LINK_SERVER", Settings.LinkServer());
                parameters[2] = new NameValue("@LINK_DATABASE", Settings.LinkDatabase());
            }

            DataTable result = null;
            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                result = pdb.ExecuteSPDataTable("MSV.MASSIVE_GET_INDIVIDUAL", parameters);
            }

            if (result != null && result.Rows.Count > 0)
            {
                foreach (DataRow row in result.Rows)
                {
                    IndividualType individualType = (IndividualType)(GetRowValue<int>(row["INDIVIDUAL_TYPE_CD"]));
                    int insuredCode = GetRowValue<int>(row["INSURED_CD"]);
                    bool isNew = GetRowValue<bool>(row["IS_NEW"]);
                    string error = row["ERROR"].ToString();
                    if (!string.IsNullOrEmpty(error))
                    {
                        error = error.Trim();
                    }
                    
                    bool isInsured = GetRowValue<bool>(row["IS_INSURED"]);
                    FilterIndividual filter = null;

                    switch (individualType)
                    {
                        case IndividualType.Person:
                            MassivePerson person = CreatePersonByDataRow(row);
                            int documentTypeId = GetRowValue<int>(row["ID_CARD_TYPE_CD"]);
                            string documentNumber = row["ID_CARD_NO"].ToString();

                            if (insuredCode > 0)
                            {
                                filter = filterIndividuals.AsParallel().FirstOrDefault(u => u.InsuredCode == insuredCode);
                                if (filter != null)//en caso de que la persona este como codigo asegurado y tambien la esten buscando por documento y tipo de documento
                                {
                                    FilterIndividual requestFilter = filterIndividuals.FirstOrDefault(u => u.Person != null && u.Person.DocumentTypeId == documentTypeId &&
                                                                      u.Person.DocumentNumber.Trim() == documentNumber.Trim());
                                   
                                    filterIndividuals.RemoveAll(u => u.Person != null && u.Person.DocumentTypeId == documentTypeId &&
                                                                      u.Person.DocumentNumber.Trim() == documentNumber.Trim() && !u.InsuredCode.HasValue);

                                }
                                if (filter == null)// caso en que la persona exista y la ingresen por documento
                                {
                                    filter = filterIndividuals.AsParallel().FirstOrDefault(u => u.Person != null && u.Person.DocumentTypeId == documentTypeId &&
                                                                     u.Person.DocumentNumber.Trim() == documentNumber.Trim());
                                }

                                if (filter == null)
                                {
                                    if (string.IsNullOrEmpty(error))
                                        throw new ValidationException(MassiveServices.EEProvider.Resources.Errors.ErrorCreatePersonInsuredNotExists);
                                }
                                else
                                {
                                    filter.Person = person;
                                    filter.InsuredCode = insuredCode;
                                }
                            }
                            else
                            {

                                filter = filterIndividuals.AsParallel().FirstOrDefault(u => u.Person != null && u.Person.DocumentTypeId == documentTypeId &&
                                                                 u.Person.DocumentNumber == documentNumber);

                                filter.Person = person;
                                filter.InsuredCode = insuredCode;
                                
                            }

                            break;
                        case IndividualType.Company:
                            MassiveCompany company = CreateCompanyByDataRow(row);
                            documentTypeId = GetRowValue<int>(row["TRIBUTARY_ID_TYPE_CD"]);
                            documentNumber = row["TRIBUTARY_ID_NO"].ToString();

                            if (insuredCode > 0)
                            {
                                filter = filterIndividuals.AsParallel().FirstOrDefault(u => u.InsuredCode == insuredCode);
                                if (filter != null)//en caso de que la persona este como codigo asegurado y tambien la esten buscando por documento y tipo de documento
                                {
                                    FilterIndividual requestFilter = filterIndividuals.FirstOrDefault(u => u.Company != null && u.Company.DocumentTypeId == documentTypeId &&
                                                                 u.Company.DocumentNumber.Trim() == documentNumber.Trim());
                                    

                                    filterIndividuals.RemoveAll(u => u.Company != null && u.Company.DocumentTypeId == documentTypeId &&
                                                                 u.Company.DocumentNumber.Trim() == documentNumber.Trim() && !u.InsuredCode.HasValue);
                                }
                                if (filter == null)// caso en que la persona exista y la ingresen por documento
                                {
                                    filter = filterIndividuals.AsParallel().FirstOrDefault(u => u.Company != null && u.Company.DocumentTypeId == documentTypeId &&
                                                                 u.Company.DocumentNumber.Trim() == documentNumber.Trim());
                                }
                                if (filter == null)
                                {
                                    if (string.IsNullOrEmpty(error))
                                        throw new ValidationException(MassiveServices.EEProvider.Resources.Errors.ErrorCreatePersonInsuredNotExists);
                                }
                                else
                                {
                                    filter.Company = company;
                                    filter.InsuredCode = insuredCode;
                                }
                            }
                            else
                            {
                                filter = filterIndividuals.AsParallel().FirstOrDefault(u => u.Company != null && u.Company.DocumentTypeId == documentTypeId &&
                                                                 u.Company.DocumentNumber == documentNumber);
                                filter.Company = company;
                                filter.InsuredCode = insuredCode;
                            }
                            break;
                        default:
                            if (insuredCode > 0)
                            {
                                filter = filterIndividuals.AsParallel().FirstOrDefault(p => p.InsuredCode == insuredCode);
                                break;
                            }
                            else
                            {
                                throw new ValidationException(Errors.ErrorCreatePersonInsuredNotExists);
                            }
                    }

                    if (filter != null)
                    {
                        filter.Error = error;
                        filter.IndividualType = individualType;
                        filter.IsCLintonList = GetRowValue<bool>(row["IS_CLINTON"]);
                        filter.SarlaftError = row["SARLAFT_ERROR"].ToString();
                    }
                }
            }

            return filterIndividuals.ToList();
        }

        public List<FilterIndividual> GetDataFilterIndividualRenewalFromPolicyRow(Row policyRow, string prefixIdPropertyname, string policyNumberPropertyName)
        {
            DataTable dtIndividualsRenewal = CreateIndividualRenewalDataTable();
            AddDataRowFromRenewalRow(policyRow, dtIndividualsRenewal, prefixIdPropertyname, policyNumberPropertyName);
            return GetFilterIndividualsByIndividualRenewal(dtIndividualsRenewal);
        }

        private void AddDataRowFromRenewalRow(Row renewalRow, DataTable individualRenewal, string prefixIdPropertyname, string policyNumberPropertyName)
        {
            DataRow dataRow = individualRenewal.NewRow();
            dataRow["BRANCH_CD"] = (int)DelegateService.utilitiesService.GetValueByField<int>(renewalRow.Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId));
            dataRow["PREFIX_CD"] = (int)DelegateService.utilitiesService.GetValueByField<int>(renewalRow.Fields.Find(x => x.PropertyName == prefixIdPropertyname));
            dataRow["DOCUMENT_NUM"] = (decimal)DelegateService.utilitiesService.GetValueByField<decimal>(renewalRow.Fields.Find(x => x.PropertyName == policyNumberPropertyName));
            individualRenewal.Rows.Add(dataRow);
        }

        private DataTable CreateIndividualRenewalDataTable()
        {
            DataTable dtIndividualsRenewal = new DataTable("PARAM_POLICY_DATA");
            dtIndividualsRenewal.Columns.Add("BRANCH_CD", typeof(int));
            dtIndividualsRenewal.Columns.Add("PREFIX_CD", typeof(int));
            dtIndividualsRenewal.Columns.Add("DOCUMENT_NUM", typeof(decimal));
            return dtIndividualsRenewal;
        }

        private List<FilterIndividual> MapFilterIndividualFromDataTableResult(DataTable dataTableResult)
        {
            var filterIndividuals = new List<FilterIndividual>();
            if (dataTableResult == null || dataTableResult.Rows.Count == 0)
            {
                return filterIndividuals;
            }
            //Parallel.ForEach(result.Rows.Cast<DataRow>(), (row) => {
            foreach (DataRow row in dataTableResult.Rows)
            {
                FilterIndividual filter = new FilterIndividual();

                IndividualType individualType = (IndividualType)(GetRowValue<int>(row["INDIVIDUAL_TYPE_CD"]));
                int insuredCode = GetRowValue<int>(row["INSURED_CD"]);
                string error = row["ERROR"].ToString();
                if (!string.IsNullOrEmpty(error))
                {
                    error = error.Trim();
                }

                bool isInsured = GetRowValue<bool>(row["IS_INSURED"]);
                if (insuredCode <= 0)
                {
                    filter.Error += Errors.IndividualNotHaveInsuredCode;
                }
                //Si ya existe el individuo en la lista
                if (filterIndividuals.Count(p => p.InsuredCode == insuredCode) > 0)
                {
                    continue;
                }

                filter.Error += error;
                filter.IndividualType = individualType;
                filter.IsCLintonList = GetRowValue<bool>(row["IS_CLINTON"]);
                filter.InsuredCode = insuredCode;
                filter.IsInsured = isInsured;
                filter.SarlaftError = row["SARLAFT_ERROR"].ToString();

                filter.DeclinedDate = GetRowValue<DateTime>(row["DECLINED_DATE"]) == DateTime.MinValue ? (DateTime?)null: GetRowValue<DateTime>(row["DECLINED_DATE"]);
                
                switch (individualType)
                {
                    case IndividualType.Person:
                        filter.Person = CreatePersonByDataRow(row);
                        break;
                    case IndividualType.Company:
                        filter.Company = CreateCompanyByDataRow(row);
                        break;

                }
                filterIndividuals.Add(filter);

            }//);

            return filterIndividuals;
        }

        public List<FilterIndividual> GetDataFilterIndividualRenewal(List<File> files, string templatePropertyName, string policyNumberPropertyName, string prefixIdPropertyname)
        {
            DataTable dtIndividualsRenewal = CreateIndividualRenewalDataTable();
            var exceptions = new ConcurrentQueue<Exception>();
            foreach (File file in files)
            {
                try
                {
                    bool hasError = file.Templates.Any(t => t.Rows.Any(r => r.HasError));

                    if (!hasError)
                    {
                        Row row = file.Templates.FirstOrDefault(x => x.PropertyName == templatePropertyName)?.Rows.FirstOrDefault();
                        AddDataRowFromRenewalRow(row, dtIndividualsRenewal, prefixIdPropertyname, policyNumberPropertyName);
                    }

                }
                catch (Exception ex)
                {
                    Row row = file.Templates.FirstOrDefault(x => x.PropertyName == templatePropertyName)?.Rows.FirstOrDefault();
                    row.HasError = true;
                    row.ErrorDescription = ex.Message;
                }

            }
            return GetFilterIndividualsByIndividualRenewal(dtIndividualsRenewal);
        }

        private List<FilterIndividual> GetFilterIndividualsByIndividualRenewal(DataTable individualRenewal)
        {
            var parameters = new NameValue[1];
            parameters[0] = new NameValue("@PARAM_POLICY_DATA", individualRenewal);
            DataTable result;
            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                result = pdb.ExecuteSPDataTable("MSV.MASSIVE_RENEWAL_GET_INDIVIDUAL", parameters);
            }
            List<FilterIndividual> filterIndividuals = MapFilterIndividualFromDataTableResult(result);
            return filterIndividuals;
        }

        public List<FilterIndividual> GetDataFilterIndividualRenewal(List<File> files, string templatePropertyName)
        {
            return GetDataFilterIndividualRenewal(files, templatePropertyName, FieldPropertyName.PolicyNumberRenewal, FieldPropertyName.PrefixId);
        }

        private List<FilterIndividual> CreateFilterIndividual(int userId, int branchId, List<FilterIndividual> individuals)
        {
            if (individuals.Count(u => u.InsuredCode == 0 && string.IsNullOrEmpty(u.Error)) > 0)
            {
                var parameters = new NameValue[1];
                DataTable dtIndividuals = new DataTable("CREATE_INDIVIDUAL");
                #region UP.INSURED
                dtIndividuals.Columns.Add("BRANCH_CD", typeof(int));
                dtIndividuals.Columns.Add("INS_PROFILE_CD", typeof(int));
                dtIndividuals.Columns.Add("INS_SEGMENT_CD", typeof(int));
                dtIndividuals.Columns.Add("CHECK_PAYABLE_TO", typeof(string));
                dtIndividuals.Columns.Add("IS_COMMERCIAL_CLIENT", typeof(int));
                dtIndividuals.Columns.Add("IS_SMS", typeof(int));
                dtIndividuals.Columns.Add("IS_MAIL_ADDRESS", typeof(int));
                #endregion

                #region UP.INDIVIDUAL
                dtIndividuals.Columns.Add("INDIVIDUAL_TYPE_CD", typeof(int));
                dtIndividuals.Columns.Add("ECONOMIC_ACTIVITY_CD", typeof(int));
                dtIndividuals.Columns.Add("USER_ID", typeof(int));
                #endregion

                #region UP.COMPANY
                dtIndividuals.Columns.Add("TRADE_NAME", typeof(string));
                dtIndividuals.Columns.Add("TRIBUTARY_ID_TYPE_CD", typeof(int));
                dtIndividuals.Columns.Add("TRIBUTARY_ID_NO", typeof(string));
                dtIndividuals.Columns.Add("COUNTRY_CD", typeof(int));
                dtIndividuals.Columns.Add("COMPANY_TYPE_CD", typeof(int));
                dtIndividuals.Columns.Add("VERIFY_DIGIT", typeof(int));
                dtIndividuals.Columns.Add("ASSOCIATION_TYPE_CD", typeof(int));
                #endregion

                #region UP.PERSON
                dtIndividuals.Columns.Add("SURNAME", typeof(string));
                dtIndividuals.Columns.Add("NAME", typeof(string));
                dtIndividuals.Columns.Add("GENDER", typeof(string));
                dtIndividuals.Columns.Add("ID_CARD_TYPE_CD", typeof(int));
                dtIndividuals.Columns.Add("ID_CARD_NO", typeof(string));
                dtIndividuals.Columns.Add("MARITAL_STATUS_CD", typeof(int));
                dtIndividuals.Columns.Add("BIRTH_DATE", typeof(DateTime));
                dtIndividuals.Columns.Add("CHILDREN", typeof(int));
                dtIndividuals.Columns.Add("MOTHER_LAST_NAME", typeof(string));
                dtIndividuals.Columns.Add("BIRTH_COUNTRY_CD", typeof(int));
                dtIndividuals.Columns.Add("PERSON_TYPE_CD", typeof(int));
                #endregion

                #region UP.ADDRESS
                dtIndividuals.Columns.Add("ADDRESS_TYPE_CD", typeof(int));
                dtIndividuals.Columns.Add("IS_MAILING_ADDRESS", typeof(int));
                dtIndividuals.Columns.Add("STREET_TYPE_CD", typeof(int));
                dtIndividuals.Columns.Add("STREET", typeof(string));
                dtIndividuals.Columns.Add("CITY_CD", typeof(int));
                dtIndividuals.Columns.Add("STATE_CD", typeof(int));
                #endregion

                #region UP.PHONE
                dtIndividuals.Columns.Add("PHONE_TYPE_CD", typeof(int));
                dtIndividuals.Columns.Add("PHONE_NUMBER", typeof(long));
                dtIndividuals.Columns.Add("CELL_PHONE_NUMBER", typeof(long));

                #endregion

                #region .INDIVIDUAL_PAYMENT_METHOD
                dtIndividuals.Columns.Add("PAYMENT_METHOD_CD", typeof(int));
                dtIndividuals.Columns.Add("ROLE_CD", typeof(int));
                #endregion

                #region UP.EMAIL
                dtIndividuals.Columns.Add("ADDRESS", typeof(string));
                dtIndividuals.Columns.Add("EMAIL_TYPE_CD", typeof(int));
                #endregion

                #region UP.AGENT
                dtIndividuals.Columns.Add("AGENT_CD", typeof(string));
                dtIndividuals.Columns.Add("AGENT_TYPE_CD", typeof(int));
                #endregion

                foreach (FilterIndividual item in individuals.Where(u => u.InsuredCode == 0 && string.IsNullOrEmpty(u.Error)))
                {
                    if (ValidateDataIndividual(item))
                    {
                        DataRow dataRow = dtIndividuals.NewRow();
                        // dataRow["INDIVIDUAL_TYPE_CD"] = item.IndividualType;
                        dataRow["BRANCH_CD"] = branchId;// por definir
                        dataRow["USER_ID"] = userId;//pendiente
                        dataRow["INS_PROFILE_CD"] = 1;
                        dataRow["INS_SEGMENT_CD"] = 1;
                        dataRow["ROLE_CD"] = 1;
                        dataRow["AGENT_CD"] = item.AgentCode.HasValue ? item.AgentCode.Value : 0;
                        dataRow["AGENT_TYPE_CD"] = item.AgentTypeCode.HasValue ? item.AgentTypeCode.Value : 0;

                        //if (item.IndividualType == IndividualType.Person)
                        //{
                        //    dataRow["ECONOMIC_ACTIVITY_CD"] = item.Person.EconomicActivity.Id;
                        //    //dataRow["CHECK_PAYABLE_TO"] = StringHelper.ConcatenateString(item.Person.Surname, " " , item.Person.MotherLastName, " ", item.Person.Name);
                        //    dataRow["CHECK_PAYABLE_TO"] = item.Person.Names.ToUpper();
                        //    dataRow["SURNAME"] = item.Person.Surname.ToUpper();
                        //    dataRow["NAME"] = item.Person.Name.ToUpper();
                        //    dataRow["GENDER"] = item.Person.Gender.ToUpper();
                        //    dataRow["ID_CARD_TYPE_CD"] = item.Person.IdentificationDocument.IssuanceDocumentType.Id;
                        //    dataRow["ID_CARD_NO"] = item.Person.IdentificationDocument.Number;
                        //    dataRow["MARITAL_STATUS_CD"] = item.Person.MaritalStatus.Id;
                        //    dataRow["BIRTH_DATE"] = item.Person.BirthDate;

                        //    if (item.Person.Children.HasValue)
                        //    {
                        //        dataRow["CHILDREN"] = item.Person.Children.Value;
                        //    }
                        //    else
                        //    {
                        //        dataRow["CHILDREN"] = 0;
                        //    }
                        //    dataRow["MOTHER_LAST_NAME"] = item.Person.MotherLastName.ToUpper();
                        //    if (item.Person.BirthPlace != null)
                        //    {
                        //        dataRow["BIRTH_COUNTRY_CD"] = item.Person.BirthPlace;
                        //    }
                        //    else
                        //    {
                        //        dataRow["BIRTH_COUNTRY_CD"] = DBNull.Value;
                        //    }
                        //    if (item.Person.PersonType != null)
                        //    {
                        //        dataRow["PERSON_TYPE_CD"] = item.Person.PersonType;
                        //    }
                        //    else
                        //    {
                        //        dataRow["PERSON_TYPE_CD"] = DBNull.Value;
                        //    }

                        //    dataRow["ADDRESS_TYPE_CD"] = item.Person.Addresses[0].AddressType.Id;
                        //    dataRow["IS_MAILING_ADDRESS"] = true;
                        //    if (item.Person.Addresses[0].AddressZone > 0)
                        //    {
                        //        dataRow["STREET_TYPE_CD"] = item.Person.Addresses[0].AddressZone;
                        //    }
                        //    else
                        //    {
                        //        dataRow["STREET_TYPE_CD"] = 1;
                        //    }
                        //    dataRow["STREET"] = item.Person.Addresses[0].Description.ToUpper();
                        //    dataRow["CITY_CD"] = item.Person.Addresses[0].City.Id;
                        //    dataRow["STATE_CD"] = item.Person.Addresses[0].City.State.Id;
                        //    dataRow["COUNTRY_CD"] = item.Person.Addresses[0].City.State.Country.Id;

                        //    dataRow["PHONE_TYPE_CD"] = item.Person.Phones[0].PhoneType.Id;
                        //    dataRow["PHONE_NUMBER"] = item.Person.Phones[0].Description;
                        //    if (item.Person.Phones.Count > 1)
                        //    {
                        //        if (!string.IsNullOrEmpty(item.Person.Phones[1].Description))
                        //            dataRow["CELL_PHONE_NUMBER"] = item.Person.Phones[1].Description;
                        //    }
                        //    dataRow["ADDRESS"] = item.Person.Emails[0].Description.ToUpper();
                        //    if (item.Person.Emails[0].EmailType.Id > 0)
                        //    {
                        //        dataRow["EMAIL_TYPE_CD"] = item.Person.Emails[0].EmailType.Id;
                        //    }
                        //    else
                        //    {
                        //        dataRow["EMAIL_TYPE_CD"] = 13;
                        //    }


                        //    //if (item.Person.PaymentMthodAccount.Any())
                        //    //{
                        //    //    dataRow["PAYMENT_METHOD_CD"] = item.Person.PaymentMthodAccount[0].PaymentMethod.Id;
                        //    //}
                        //    //else
                        //    //{
                        //    dataRow["PAYMENT_METHOD_CD"] = 1;
                        //    //}
                        //}
                        //else
                        //{
                        //    dataRow["ECONOMIC_ACTIVITY_CD"] = item.Company.EconomicActivity.Id;
                        //    dataRow["CHECK_PAYABLE_TO"] = item.Company.Name.ToUpper();
                        //    dataRow["TRADE_NAME"] = item.Company.Name.ToUpper();
                        //    dataRow["TRIBUTARY_ID_TYPE_CD"] = item.Company.IdentificationDocument.IssuanceDocumentType.Id;
                        //    dataRow["TRIBUTARY_ID_NO"] = item.Company.IdentificationDocument.Number;
                        //    dataRow["COUNTRY_CD"] = item.Company.CountryOrigin.Id;
                        //    dataRow["COMPANY_TYPE_CD"] = item.Company.CompanyType.Id;
                        //    dataRow["VERIFY_DIGIT"] = IndividualHelper.CalculateDigitVerify(item.Company.IdentificationDocument.Number);
                        //    dataRow["ASSOCIATION_TYPE_CD"] = 1;

                        //    dataRow["ADDRESS_TYPE_CD"] = item.Company.Addresses[0].AddressType.Id;
                        //    dataRow["IS_MAILING_ADDRESS"] = true;
                        //    if (item.Company.Addresses[0].AddressZone > 0)
                        //    {
                        //        dataRow["STREET_TYPE_CD"] = item.Company.Addresses[0].AddressZone;
                        //    }
                        //    else
                        //    {
                        //        dataRow["STREET_TYPE_CD"] = 1;
                        //    }
                        //    dataRow["STREET"] = item.Company.Addresses[0].Description.ToUpper();
                        //    dataRow["CITY_CD"] = item.Company.Addresses[0].City.Id;
                        //    dataRow["STATE_CD"] = item.Company.Addresses[0].City.State.Id;


                        //    dataRow["PHONE_TYPE_CD"] = item.Company.Phones[0].PhoneType.Id;
                        //    dataRow["PHONE_NUMBER"] = item.Company.Phones[0].Description;

                        //    if (item.Company.Phones.Count > 1)
                        //    {
                        //        if (!string.IsNullOrEmpty(item.Company.Phones[1].Description))
                        //            dataRow["CELL_PHONE_NUMBER"] = item.Company.Phones[1].Description;
                        //    }

                        //    dataRow["ADDRESS"] = item.Company.Emails[0].Description.ToUpper();
                        //    if (item.Company.Emails[0].EmailType.Id > 0)
                        //    {
                        //        dataRow["EMAIL_TYPE_CD"] = item.Company.Emails[0].EmailType.Id;
                        //    }
                        //    else
                        //    {
                        //        dataRow["EMAIL_TYPE_CD"] = 13;
                        //    }


                        //    //if (item.Company.PaymentMethodAccount.Any())
                        //    //{
                        //    //    dataRow["PAYMENT_METHOD_CD"] = item.Company.PaymentMethodAccount[0].PaymentMethod.Id;
                        //    //}
                        //    //else
                        //    //{
                        //    dataRow["PAYMENT_METHOD_CD"] = 1;
                        //    //}

                        //}
                        dtIndividuals.Rows.Add(dataRow);
                    }
                }
                if (dtIndividuals.Rows.Count > 0)
                {
                    parameters[0] = new NameValue("@CREATE_INDIVIDUAL", dtIndividuals);
                    DataTable result = null;

                    using (DynamicDataAccess pdb = new DynamicDataAccess())
                    {
                        result = pdb.ExecuteSPDataTable("UP.CREATE_INDIVIDUAL_MASSIVE", parameters);
                    }

                    if (result.Columns.Contains("ErrorMessage") && result.Rows.Count > 0)
                    {
                        throw new ValidationException(result.Rows[0]["ErrorMessage"].ToString());
                    }

                    foreach (DataRow row in result.Rows)
                    {
                        string error = row["ERROR"].ToString();
                        IndividualType individualType = (IndividualType)(GetRowValue<int>(row["INDIVIDUAL_TYPE_CD"]));
                        FilterIndividual filter = null;
                        int? insuredCode = GetRowValue<int>(row["INSURED_CD"]);

                        switch (individualType)
                        {
                            case IndividualType.Person:
                                {
                                    int documentTypeId = GetRowValue<int>(row["DOCUMENT_TYPE"]);
                                    string documentNumber = row["DOCUMENT_NUMBER"].ToString();
                                    filter = individuals.AsParallel().FirstOrDefault(u => u.Person != null && u.Person.DocumentTypeId == documentTypeId &&
                                                                        u.Person.DocumentNumber == documentNumber);
                                    filter.Error = error;
                                    filter.InsuredCode = insuredCode;
                                    //if (string.IsNullOrEmpty(filter.Error))
                                    //{
                                    //    UpdateFilterIndividualPersonByDataRow(filter, row);
                                    //}

                                }
                                break;
                            case IndividualType.Company:
                                {
                                    int documentTypeId = GetRowValue<int>(row["DOCUMENT_TYPE"]);
                                    string documentNumber = row["DOCUMENT_NUMBER"].ToString();

                                    filter = individuals.AsParallel().FirstOrDefault(u => u.Company != null && u.Company.DocumentTypeId == documentTypeId &&
                                                                        u.Company.DocumentNumber == documentNumber);
                                    filter.Error = error;
                                    filter.InsuredCode = insuredCode;
                                    if (string.IsNullOrEmpty(filter.Error))
                                    {
                                        UpdateFilterIndividualCompanyByDataRow(filter, row);
                                    }


                                }
                                break;
                        }
                        filter.Error = error;

                    }

                }
            }
            return individuals;

        }
        #endregion

        #region Validaciones
        private void ValidateIndividual(ConcurrentDictionary<string, FilterIndividual> individualDictionary, FilterIndividual individual)
        {
            var result = individual.GetHash();
            
            if (result == "Error")
            {
                throw new ValidationException(individual.Error);
            }

            individualDictionary.TryAdd(result, individual);
        }

        private bool ValidateDataIndividual(FilterIndividual filterIndividual)
        {
            switch (filterIndividual.IndividualType)
            {
                case IndividualType.Person:
                    if (filterIndividual.Person.EconomicActivityId == 0)
                    {
                        filterIndividual.Error += Errors.ErrorCreatePersonEconomicActivity;
                    }
                    if (filterIndividual.Person.Surname == "")
                    {
                        filterIndividual.Error += Errors.ErrorCreatePersonSurname;
                    }
                    if (filterIndividual.Person.FullName == "")
                    {
                        filterIndividual.Error += Errors.ErrorCreatePersonName;
                    }
                    if (filterIndividual.Person.Gender == "")
                    {
                        filterIndividual.Error += Errors.ErrorCreatePersonGender;
                    }
                    else
                    {
                        if (filterIndividual.Person.Gender.ToUpper() != "F" && filterIndividual.Person.Gender.ToUpper() != "M")
                        {
                            filterIndividual.Error += Errors.ErrorCreatePersonGender;
                        }
                    }
                    if (filterIndividual.Person.DocumentTypeId == 0)
                    {
                        filterIndividual.Error += Errors.ErrorCreatePersonDocumentType;
                    }
                    if (filterIndividual.Person.DocumentNumber == "" || filterIndividual.Person.DocumentNumber == "0")
                    {
                        filterIndividual.Error += Errors.ErrorCreatePersonDocumentNumber;
                    }
                    if (filterIndividual.Person.MaritalStatusId == 0)
                    {
                        filterIndividual.Error += Errors.ErrorCreatePersonMaritalStatus;
                    }
                    if (filterIndividual.Person.BirthDate == null)
                    {
                        filterIndividual.Error += Errors.ErrorCreatePersonBirthDate;
                    }
                    //if (filterIndividual.Person.MotherLastName == "")
                    //{
                    //    filterIndividual.Error += Errors.ErrorCreatePersonMotherLastName;
                    //}
                    if (filterIndividual.Person.AddressTypeId == 0)
                    {
                        filterIndividual.Error += Errors.ErrorCreatePersonAddressType;
                    }
                    if (filterIndividual.Person.AddressDescription == "")
                    {
                        filterIndividual.Error += Errors.ErrorCreatePersonAddress;
                    }
                    if (filterIndividual.Person.AddressCity.Id == 0)
                    {
                        filterIndividual.Error += Errors.ErrorCreatePersonCity;
                    }
                    if (filterIndividual.Person.AddressCity.State.Id == 0)
                    {
                        filterIndividual.Error += Errors.ErrorCreatePersonState;
                    }
                    if (filterIndividual.Person.AddressCity.State.Country.Id == 0)
                    {
                        filterIndividual.Error += Errors.ErrorCreatePersonCountry;
                    }
                    if (filterIndividual.Person.PhoneTypeId == 0)
                    {
                        filterIndividual.Error += Errors.ErrorCreatePersonPhoneType;
                    }
                    if (filterIndividual.Person.PhoneDescription == "")
                    {
                        filterIndividual.Error += Errors.ErrorCreatePersonPhone;
                    }
                    break;

                case IndividualType.Company:
                    if (filterIndividual.Company.EconomicActivityId == 0)
                    {
                        filterIndividual.Company.EconomicActivityId = 10;
                    }
                    if (filterIndividual.Company.FullName == "")
                    {
                        filterIndividual.Error += Errors.ErrorCreateCompanyName;
                    }
                    if (filterIndividual.Company.DocumentTypeId == 0)
                    {
                        filterIndividual.Error += Errors.ErrorCreateCompanyDocumentType;
                    }
                    if (filterIndividual.Company.DocumentNumber == "")
                    {
                        filterIndividual.Error += Errors.ErrorCreateCompanyDocumentNumber;
                    }
                    if (filterIndividual.Company.CountryOrigin.Id == 0)
                    {
                        filterIndividual.Error += Errors.ErrorCreateCompanyCountryOrigin;
                    }
                    if (filterIndividual.Company.CompanyTypeId == 0)
                    {
                        filterIndividual.Error += Errors.ErrorCreateCompanyCompanyType;
                    }
                    if (filterIndividual.Company.AddressTypeId == 0)
                    {
                        filterIndividual.Error += Errors.ErrorCreateCompanyAddressType;
                    }
                    if (filterIndividual.Company.AddressDescription == "")
                    {
                        filterIndividual.Error += Errors.ErrorCreateCompanyAddress;
                    }
                    if (filterIndividual.Company.AddressCity.Id == 0)
                    {
                        filterIndividual.Error += Errors.ErrorCreateCompanyCity;
                    }
                    if (filterIndividual.Company.AddressCity.State.Id == 0)
                    {
                        filterIndividual.Error += Errors.ErrorCreateCompanyState;
                    }
                    if (filterIndividual.Company.PhoneTypeId == 0)
                    {
                        filterIndividual.Error += Errors.ErrorCreateCompanyPhoneType;
                    }
                    if (filterIndividual.Company.PhoneDescription == "")
                    {
                        filterIndividual.Error += Errors.ErrorCreateCompanyPhone;
                    }
                    break;
                default:
                    filterIndividual.Error += Errors.ErrorIndividualType;
                    break;
            }

            if (filterIndividual.Error != "" && filterIndividual.Error != null)
                return false;
            return true;
        }
        #endregion

        private T GetRowValue<T>(object rowValue)
        {
            if (rowValue == null || rowValue == DBNull.Value)
            {
                return default(T);
            }
            else
            {
                return (T)Convert.ChangeType(rowValue, typeof(T));
            }
        }
    }
}