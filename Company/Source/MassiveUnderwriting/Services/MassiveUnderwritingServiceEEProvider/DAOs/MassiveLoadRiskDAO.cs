using Newtonsoft.Json;
using Sistran.Company.Application.MassiveServices.Models;
using Sistran.Company.Application.MassiveUnderwritingServices.EEProvider.Entities.Views;
using Sistran.Company.Application.MassiveUnderwritingServices.EEProvider.Resources;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.Constants;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.MassiveUnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using APEntity = Sistran.Core.Application.AuthorizationPolicies.Entities;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.MassiveUnderwritingServices.EEProvider.DAOs
{
    public class MassiveLoadRiskDAO
    {
        public GroupCoverage CreateGroupCoverage(Row row, int productId)
        {
            Field field = row.Fields.Find(y => y.PropertyName == FieldPropertyName.RiskGroupCoverage);
            if (field == null)
            {
                throw new ValidationException(Errors.RiskGroupCoverageFieldNotFound);
            }
            List<GroupCoverage> groupCoverages = DelegateService.underwritingService.GetGroupCoverages(productId);
            GroupCoverage groupCoverage;

            if (string.IsNullOrEmpty(field.Value))
            {
                groupCoverage = groupCoverages.OrderBy(x => x.Id).FirstOrDefault();
                if ((groupCoverage) == null)
                {
                    throw new ValidationException(Errors.NoCoverageGroupRelatedToProduct);
                }
                return groupCoverage;
            }
            int riskGroupCoverageId = (int)DelegateService.utilitiesService.GetValueByField<int>(field);
            if ((groupCoverage = groupCoverages.Find(x => x.Id == riskGroupCoverageId)) == null)
            {
                throw new ValidationException(Errors.ErrorInvalidGroupCoverageId);
            }
            return groupCoverage;
        }

        public RatingZone CreateRatingZone(Row row, int prefixId)
        {
            try
            {
                int ratingZoneId = (int)DelegateService.utilitiesService.GetValueByField<int>(
                        row.Fields.First(y => y.PropertyName == FieldPropertyName.RiskRatingZone));
                RatingZone ratingZone = new RatingZone();
                if (ratingZoneId == 0
                    /*|| (ratingZone = DelegateService.commonService.GetRatingZonesByPrefixId(prefixId).Find(x => x.Id == ratingZoneId)) == null*/)
                {
                    throw new ValidationException(Errors.ErrorInvalidRatingZoneId);
                }
                return ratingZone;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(FieldPropertyName.RiskRatingZone + "-" + ex.ToString());
            }
        }
        public List<Beneficiary> CreateAdditionalBeneficiaries(Template beneficiariesTemplate)
        {
            List<Beneficiary> beneficiaries = new List<Beneficiary>();

            if (beneficiariesTemplate != null)
            {
                foreach (Row row in beneficiariesTemplate.Rows)
                {
                    Beneficiary beneficiary = new Beneficiary();
                    CompanyIssuanceInsured insured = new CompanyIssuanceInsured();

                    int insuredCode = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryInsuredCode));

                    if (insuredCode > 0)
                    {
                        //insured = DelegateService.underwritingService.GetInsuredByInsuredCode(insuredCode);

                        if (insured == null)
                        {
                            throw new ValidationException(Errors.ErrorBeneficiaryNotFound + " " + row.Number);
                        }
                        else
                        {
                            if (insured.CompanyName == null || insured.CompanyName.Address == null)
                            {
                                throw new ValidationException(Errors.ErrorBeneficiaryAdditionalAddress);
                            }
                            beneficiary.IndividualId = insured.IndividualId;
                            beneficiary.Name = insured.Name;
                            beneficiary.BeneficiaryType = (BeneficiaryType)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryBeneficiaryType));
                            beneficiary.Participation = (decimal)DelegateService.utilitiesService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryParticipation));
                            if (beneficiary.Participation == 0)
                            {
                                throw new ValidationException(Errors.ErrorBeneficiaryParticipation);
                            }
                        }
                    }
                    else
                    {
                        string documentNumber = string.Empty;

                        IndividualType individualType = (IndividualType)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryIndividualType));

                        switch (individualType)
                        {
                            case IndividualType.Company:
                                documentNumber = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryCompanyDocumentNumber)).ToString();
                                insured = DelegateService.underwritingService.GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(documentNumber, InsuredSearchType.DocumentNumber, CustomerType.Individual);
                                beneficiary = DelegateService.underwritingService.GetBeneficiariesByDescription(insured.IndividualId.ToString(), InsuredSearchType.IndividualId).FirstOrDefault();

                                if (beneficiary == null)
                                {
                                    beneficiary = CreateBeneficiaryCompany(row);
                                }
                                break;
                            case IndividualType.Person:
                                documentNumber = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryCompanyDocumentNumber)).ToString();
                                insured = DelegateService.underwritingService.GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(documentNumber, InsuredSearchType.DocumentNumber, CustomerType.Individual);
                                beneficiary = DelegateService.underwritingService.GetBeneficiariesByDescription(insured.IndividualId.ToString(), InsuredSearchType.IndividualId).FirstOrDefault();

                                if (beneficiary == null)
                                {
                                    beneficiary = CreateBeneficiaryPerson(row);
                                }
                                break;
                            default:
                                throw new ValidationException(Errors.ErrorBeneficiaryIndividualType + " " + row.Number);
                        }

                        beneficiary.BeneficiaryType = (BeneficiaryType)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryBeneficiaryType));
                        beneficiary.Participation = (decimal)DelegateService.utilitiesService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryParticipation));
                        if (beneficiary.Participation == 0)
                        {
                            throw new ValidationException(Errors.ErrorBeneficiaryParticipation);
                        }
                    }

                    beneficiaries.Add(beneficiary);
                }
            }

            return beneficiaries;
        }

        //public Beneficiary CreateBeneficiary(Row row, Insured insured)
        //{
        //    Beneficiary beneficiary = new Beneficiary();

        //    bool beneficiaryEqualInsured = (bool)DelegateService.utilitiesService.GetValueByField<bool>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryEqualInsured));
        //    bool isWithGroupingRequest = !string.IsNullOrEmpty((string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RequestGroup)));

        //    if (beneficiaryEqualInsured)
        //    {
        //        beneficiary.IndividualId = insured.IndividualId;
        //        //beneficiary.IdentificationDocument = insured.IdentificationDocument;
        //        //beneficiary.CompanyName = insured.CompanyName;
        //        beneficiary.Name = insured.Name;
        //    }
        //    else
        //    {
        //        int insuredCode = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryInsuredCode));

        //        if (insuredCode > 0)
        //        {
        //            insured = DelegateService.uniquePersonService.GetInsuredByInsuredCode(insuredCode);

        //            if (insured == null)
        //            {
        //                throw new ValidationException(Errors.ErrorBeneficiaryNotFound + " " + row.Number);
        //            }
        //            else
        //            {
        //                if (insured.CompanyName == null || insured.CompanyName.Address == null)
        //                {
        //                    throw new ValidationException(Errors.ErrorBeneficiaryAddress);
        //                }
        //                beneficiary.IndividualId = insured.IndividualId;
        //                //beneficiary.IdentificationDocument = insured.IdentificationDocument;
        //                //beneficiary.CompanyName = insured.CompanyName;
        //                beneficiary.Name = insured.Name;
        //            }
        //            //if (!isWithGroupingRequest)
        //            //    //UpdateBeneficiaryByFile(insured, row);
        //        }
        //        else
        //        {
        //            string documentNumber = string.Empty;
        //            UPENUM.IndividualType individualType = (UPENUM.IndividualType)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryIndividualType));

        //            switch (individualType)
        //            {
        //                case UPENUM.IndividualType.LegalPerson:
        //                    documentNumber = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryCompanyDocumentNumber)).ToString();
        //                    UniquePersonServices.Models.Company currentCompany = DelegateService.uniquePersonService.GetCompanyByDocumentNumber(documentNumber);
        //                    // Company could be already registered, but HolderInsuredCode was not supplied
        //                    if (currentCompany != null)
        //                    {
        //                        Insured currentInsured = DelegateService.uniquePersonService.GetInsuredByIndividualId(currentCompany.IndividualId);
        //                        if (currentInsured == null)
        //                            currentInsured = CreateInsuredByCompany(currentCompany, row);
        //                        else if (!isWithGroupingRequest)
        //                            //UpdateCompanyBeneficiaryByFile(currentInsured, row);

        //                            beneficiary = DelegateService.underwritingService.GetBeneficiariesByDescription(insured.InsuredId.ToString(), Sistran.Core.Services.UtilitiesServices.Enums.InsuredSearchType.IndividualId).DefaultIfEmpty(null).First();
        //                        if (beneficiary == null)
        //                            beneficiary = CreateBeneficiaryByInsuredCompany(currentCompany, currentInsured);

        //                    }
        //                    else
        //                        beneficiary = CreateBeneficiaryCompany(row);
        //                    break;
        //                case UPENUM.IndividualType.Person:
        //                    documentNumber = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPersonDocumentNumber)).ToString();
        //                    UPMO.Person currentPerson = DelegateService.uniquePersonService.GetPersonByDocumentNumber(documentNumber);
        //                    // Person could be already registered, but HolderInsuredCode was not supplied
        //                    if (currentPerson != null)
        //                    {
        //                        Insured currentInsured = DelegateService.uniquePersonService.GetInsuredByIndividualId(currentPerson.IndividualId);
        //                        if (currentInsured == null)
        //                            currentInsured = CreateInsuredByPerson(currentPerson, row);
        //                        else if (!isWithGroupingRequest)
        //                            //UpdatePersonBeneficiaryByFile(currentInsured, row);

        //                            beneficiary = DelegateService.underwritingService.GetBeneficiariesByDescription(insured.InsuredId.ToString(), Sistran.Core.Services.UtilitiesServices.Enums.InsuredSearchType.IndividualId).DefaultIfEmpty(null).First();
        //                        if (beneficiary == null)
        //                            beneficiary = CreateBeneficiaryByInsuredPerson(currentPerson, currentInsured);

        //                    }
        //                    else
        //                        beneficiary = CreateBeneficiaryPerson(row);
        //                    break;
        //                default:
        //                    throw new ValidationException(StringHelper.ConcatenateString(Errors.ErrorBeneficiaryIndividualType, " ", row.Number.ToString()));
        //            }
        //        }
        //    }

        //    beneficiary.BeneficiaryType = (BeneficiaryType)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryBeneficiaryType));
        //    beneficiary.CustomerType = Sistran.Core.Services.UtilitiesServices.Enums.CustomerType.Individual;
        //    beneficiary.Participation = 100;

        //    return beneficiary;
        //}

        //private void UpdateBeneficiaryByFile(Insured beneficiary, Row row)
        //{
        //    if (beneficiary.IndividualType == UPENUM.IndividualType.LegalPerson)
        //        UpdateCompanyBeneficiaryByFile(beneficiary, row);
        //    else if (beneficiary.IndividualType == UPENUM.IndividualType.Person)
        //        UpdatePersonBeneficiaryByFile(beneficiary, row);
        //}

        //private void UpdatePersonBeneficiaryByFile(Insured insured, Row row)
        //{

        //    UPMO.Person currentPerson = DelegateService.uniquePersonService.GetPersonByDocumentNumber(insured.IdentificationDocument.Number);
        //    bool personChanged = false;

        //    string names = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPersonName));
        //    if (!string.IsNullOrEmpty(names)
        //        && currentPerson.Names.Trim().ToLower() != names.Trim().ToLower())
        //    {
        //        currentPerson.Names = names;
        //        personChanged = true;
        //    }

        //    string surname = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPersonSurname));
        //    if (!string.IsNullOrEmpty(surname)
        //        && currentPerson.Surname.Trim().ToLower() != surname.Trim().ToLower())
        //    {
        //        currentPerson.Surname = surname;
        //        personChanged = true;
        //    }

        //    string motherLastName = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPersonSecondSurname));
        //    if (!string.IsNullOrEmpty(motherLastName)
        //        && currentPerson.MotherLastName.Trim().ToLower() != motherLastName.Trim().ToLower())
        //    {
        //        currentPerson.MotherLastName = motherLastName;
        //        personChanged = true;
        //    }

        //    int economicActivityId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryEconomicActivity));
        //    if (economicActivityId > 0
        //        && currentPerson.EconomicActivity != null && currentPerson.EconomicActivity.Id != economicActivityId)
        //    {
        //        currentPerson.EconomicActivity = new EconomicActivity()
        //        {
        //            Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryEconomicActivity))
        //        };
        //        personChanged = true;
        //    }

        //    string gender = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPersonGender));
        //    if (!string.IsNullOrEmpty(gender)
        //        && currentPerson.Gender != gender)
        //    {
        //        currentPerson.Gender = gender;
        //        personChanged = true;
        //    }

        //    int maritalStatusId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPersonMaritalStatus));
        //    if (maritalStatusId > 0
        //        && currentPerson.MaritalStatus != null && currentPerson.MaritalStatus.Id != maritalStatusId)
        //    {
        //        currentPerson.MaritalStatus = new MaritalStatus
        //        {
        //            Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPersonMaritalStatus))
        //        };
        //        personChanged = true;
        //    }

        //    int addressTypeId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryAddressType));
        //    string addressDescription = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryAddressDescription));
        //    int addresssCityId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryAddressCity));
        //    int addresssStateId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryAddressState));
        //    int addresssCountryId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryAddressCountry));
        //    if (addressTypeId > 0
        //        && !string.IsNullOrEmpty(addressDescription)
        //        && addresssCityId > 0
        //        && addresssStateId > 0
        //        && addresssCountryId > 0
        //        && !currentPerson.Addresses.Any(a => a.Description.Trim().ToLower() == addressDescription.Trim().ToLower()))
        //    {
        //        currentPerson.Addresses.Add(new Address()
        //        {
        //            AddressType = new AddressType()
        //            {
        //                Id = addressTypeId
        //            },
        //            Description = addressDescription,
        //            City = new City()
        //            {
        //                Id = addresssCityId,
        //                State = new State()
        //                {
        //                    Id = addresssStateId,
        //                    Country = new Country
        //                    {
        //                        Id = addresssCountryId
        //                    }
        //                }
        //            }
        //        });
        //        personChanged = true;
        //    }

        //    int phoneTypeId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPhoneType));
        //    string phoneDescription = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPhoneDescription));
        //    if (phoneTypeId > 0 && !string.IsNullOrEmpty(phoneDescription)
        //        && !currentPerson.Phones.Any(p => p.Description.Trim() == phoneDescription.Trim()))
        //    {
        //        currentPerson.Phones.Add(new Phone()
        //        {
        //            PhoneType = new PhoneType()
        //            {
        //                Id = phoneTypeId
        //            },
        //            Description = phoneDescription
        //        });
        //        personChanged = true;
        //    }

        //    int emailType = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryEmailType));
        //    string emailDescription = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryEmailDescription));

        //    if (emailType > 0 && !string.IsNullOrEmpty(emailDescription)
        //        && !currentPerson.Emails.Any(e => e.Description.Trim().ToLower() == emailDescription.Trim().ToLower()))
        //    {
        //        currentPerson.Emails.Add(new Email()
        //        {
        //            EmailType = new EmailType()
        //            {
        //                Id = emailType
        //            },
        //            Description = emailDescription
        //        });
        //        personChanged = true;
        //    }

        //    if (personChanged)
        //        DelegateService.uniquePersonService.UpdatePerson(currentPerson, null);
        //}

        //private void UpdateCompanyBeneficiaryByFile(Insured insured, Row row)
        //{

        //    UniquePersonServices.Models.Company currentCompany = DelegateService.uniquePersonService.GetCompanyByDocumentNumber(insured.IdentificationDocument.Number);
        //    bool companyChanged = false;

        //    string name = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryCompanyTradeName));
        //    if (!string.IsNullOrEmpty(name)
        //        && currentCompany.Name.Trim().ToLower() != name.Trim().ToLower())
        //    {
        //        currentCompany.Name = name;
        //        companyChanged = true;
        //    }

        //    int economicActivityId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryEconomicActivity));
        //    if (economicActivityId > 0
        //        && currentCompany.EconomicActivity != null && currentCompany.EconomicActivity.Id != economicActivityId)
        //    {
        //        currentCompany.EconomicActivity = new EconomicActivity()
        //        {
        //            Id = economicActivityId
        //        };
        //        companyChanged = true;
        //    }

        //    int companyTypeId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryCompanyType));
        //    if (companyTypeId > 0
        //        && currentCompany.CompanyType != null && currentCompany.CompanyType.Id != companyTypeId)
        //    {
        //        currentCompany.CompanyType = new CompanyType()
        //        {
        //            Id = companyTypeId
        //        };
        //        companyChanged = true;
        //    }

        //    int companyCountryOriginId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryAddressCountry));
        //    if (companyCountryOriginId > 0
        //        && currentCompany.CountryOrigin != null && currentCompany.CountryOrigin.Id != companyCountryOriginId)
        //    {
        //        currentCompany.CountryOrigin = new Country
        //        {
        //            Id = companyCountryOriginId
        //        };
        //        companyChanged = true;
        //    }

        //    int addressTypeId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryAddressType));
        //    string addressDescription = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryAddressDescription));
        //    int addresssCityId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryAddressCity));
        //    int addresssStateId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryAddressState));
        //    int addresssCountryId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryAddressCountry));
        //    if (addressTypeId > 0
        //        && !string.IsNullOrEmpty(addressDescription)
        //        && addresssCityId > 0
        //        && addresssStateId > 0
        //        && addresssCountryId > 0
        //        && currentCompany.Addresses != null
        //        && !currentCompany.Addresses.Any(a => a.Description.Trim().ToLower() == addressDescription.Trim().ToLower()))
        //    {
        //        currentCompany.Addresses.Add(new Address()
        //        {
        //            AddressType = new AddressType()
        //            {
        //                Id = addressTypeId
        //            },
        //            Description = addressDescription,
        //            City = new City()
        //            {
        //                Id = addresssCityId,
        //                State = new State()
        //                {
        //                    Id = addresssStateId,
        //                    Country = new Country
        //                    {
        //                        Id = addresssCountryId
        //                    }
        //                }
        //            }
        //        });
        //        companyChanged = true;
        //    }
        //    int companyPhoneTypeId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPhoneType));
        //    string companyPhoneDescription = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPhoneDescription));
        //    if (companyPhoneTypeId > 0 && !string.IsNullOrEmpty(companyPhoneDescription)
        //        && currentCompany.Phones != null
        //        && !currentCompany.Phones.Any(p => p.Description.Trim() == companyPhoneDescription.Trim()))
        //    {
        //        currentCompany.Phones.Add(new Phone()
        //        {
        //            PhoneType = new PhoneType()
        //            {
        //                Id = companyPhoneTypeId
        //            },
        //            Description = companyPhoneDescription
        //        });
        //    }

        //    string companyCellPhoneDescription = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPhoneDescription));
        //    if (!string.IsNullOrEmpty(companyCellPhoneDescription)
        //        && currentCompany.Phones != null
        //        && !currentCompany.Phones.Any(p => p.Description.Trim() == companyCellPhoneDescription.Trim()))
        //    {
        //        currentCompany.Phones.Add(new Phone()
        //        {
        //            PhoneType = new PhoneType()
        //            {
        //                Id = 4
        //            },
        //            Description = companyCellPhoneDescription
        //        });
        //    }

        //    if (companyChanged)
        //        DelegateService.uniquePersonService.UpdateCompany(currentCompany);
        //}

        private Beneficiary CreateBeneficiaryCompany(Row row)
        {
            //UPMO.Company company = new UPMO.Company
            //{
            //    IndividualType = UPEN.IndividualType.LegalPerson,
            //    Name = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryCompanyTradeName)),
            //    EconomicActivity = new EconomicActivity()
            //    {
            //        Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryEconomicActivity))
            //    },
            //    IdentificationDocument = new IdentificationDocument()
            //    {
            //        DocumentType = new DocumentType()
            //        {
            //            Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryCompanyDocumentType))
            //        },
            //        Number = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryCompanyDocumentNumber))
            //    },
            //    CompanyType = new CompanyType()
            //    {
            //        Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryCompanyType))
            //    },
            //    CountryOrigin = new Country
            //    {
            //        Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryAddressCountry))
            //    },
            //    PaymentMethodAccount = new List<PaymentMethodAccount>(),
            //    Addresses = new List<Address>(),
            //    Phones = new List<Phone>(),
            //    Emails = new List<Email>()
            //};

            //company.Addresses.Add(new Address()
            //{
            //    AddressType = new AddressType()
            //    {
            //        Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryAddressType))
            //    },
            //    Description = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryAddressDescription)),
            //    City = new City()
            //    {
            //        Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryAddressCity)),
            //        State = new State()
            //        {
            //            Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryAddressState)),
            //            Country = new Country
            //            {
            //                Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryAddressCountry))
            //            }
            //        }
            //    }
            //});

            //company.Phones.Add(new Phone()
            //{
            //    PhoneType = new PhoneType()
            //    {
            //        Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPhoneType))
            //    },
            //    Description = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPhoneDescription))
            //});

            /////Inicio cargue masivo Company
            //company.Phones.Add(new Phone()
            //{
            //    PhoneType = new PhoneType()
            //    {
            //        Id = 4
            //    },
            //    Description = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.BeneficiaryCellNumber))
            //});
            /////Fin cargue masivo Company

            //company.Emails.Add(new Email()
            //{
            //    EmailType = new EmailType()
            //    {
            //        Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryEmailType))
            //    },
            //    Description = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryEmailDescription))
            //});

            //company = DelegateService.uniquePersonService.CreateCompany(company);
            //Insured insured = CreateInsuredByCompany(company, row);


            return CreateBeneficiaryByInsuredCompany(null, null);
        }

        private Beneficiary CreateBeneficiaryByInsuredCompany(MassiveCompany company, CompanyIssuanceInsured insured)
        {
            Beneficiary beneficiary = new Beneficiary
            {
                /// Validar cambio de modelo
                IndividualId = insured.IndividualId,
                //IdentificationDocument = insured.IdentificationDocument ?? company.IdentificationDocument,
                Name = insured.Name ?? company.FullName,
                //CustomerType = insured.CustomerType,
                CompanyName = new IssuanceCompanyName()

            };

            //if (company.Addresses != null)
            //{
            //    beneficiary.CompanyName.Address = company.Addresses.First();
            //}
            //if (company.Phones != null)
            //{
            //    beneficiary.CompanyName.Phone = company.Phones.First();
            //}
            //if (company.Emails != null)
            //{
            //    beneficiary.CompanyName.Email = company.Emails.First();
            //}
            return beneficiary;
        }

        private Beneficiary CreateBeneficiaryPerson(Row row)
        {
            //UPMO.Person person = new UPMO.Person
            //{
            //    IndividualType = UPEN.IndividualType.Person,
            //    Names = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPersonName)),
            //    Surname = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPersonSurname)),
            //    MotherLastName = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPersonSecondSurname)),
            //    EconomicActivity = new EconomicActivity()
            //    {
            //        Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryEconomicActivity))
            //    },
            //    IdentificationDocument = new IdentificationDocument()
            //    {
            //        DocumentType = new DocumentType()
            //        {
            //            Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPersonDocumentType))
            //        },
            //        Number = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPersonDocumentNumber))
            //    },
            //    Gender = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPersonGender)),
            //    MaritalStatus = new MaritalStatus()
            //    {
            //        Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPersonMaritalStatus))
            //    },
            //    BirthDate = (DateTime)DelegateService.utilitiesService.GetValueByField<DateTime>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPersonBirthDate)),
            //    EducativeLevel = new EducativeLevel
            //    {
            //        Id = 1
            //    },
            //    HouseType = new HouseType
            //    {
            //        Id = 1
            //    },
            //    SocialLayer = new SocialLayer
            //    {
            //        Id = 1
            //    },
            //    LaborPerson = new LaborPerson
            //    {
            //        Id = 1,
            //        Occupation = new Occupation()
            //    },
            //    PaymentMthodAccount = new List<PaymentMethodAccount>(),
            //    Addresses = new List<Address>(),
            //    Phones = new List<Phone>(),
            //    Emails = new List<Email>()
            //};

            //person.Addresses.Add(new Address()
            //{
            //    AddressType = new AddressType()
            //    {
            //        Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryAddressType))
            //    },
            //    Description = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryAddressDescription)),
            //    City = new City()
            //    {
            //        Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryAddressCity)),
            //        State = new State()
            //        {
            //            Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryAddressState)),
            //            Country = new Country
            //            {
            //                Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryAddressCountry))
            //            }
            //        }
            //    }
            //});

            //person.Phones.Add(new Phone()
            //{
            //    PhoneType = new PhoneType()
            //    {
            //        Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPhoneType))
            //    },
            //    Description = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPhoneDescription))
            //});

            /////Inicio cargue masivo Company
            //person.Phones.Add(new Phone()
            //{
            //    PhoneType = new PhoneType()
            //    {
            //        Id = 4
            //    },
            //    Description = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.BeneficiaryCellNumber))
            //});
            /////Fin cargue masivo Company

            //person.Emails.Add(new Email()
            //{
            //    EmailType = new EmailType()
            //    {
            //        Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryEmailType))
            //    },
            //    Description = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryEmailDescription))
            //});

            //person = DelegateService.uniquePersonService.CreatePerson(person, null);

            ////CREAR ASEGURADO
            //Insured insured = CreateInsuredByPerson(person, row);

            //insured = DelegateService.uniquePersonService.CreateInsured(insured);



            return CreateBeneficiaryByInsuredPerson(null, null);
        }

        private Beneficiary CreateBeneficiaryByInsuredPerson(MassivePerson person, CompanyIssuanceInsured insured)
        {
            Beneficiary beneficiary = new Beneficiary
            {
                //Validar cambio de modelo
                IndividualId = insured.IndividualId,
                //IdentificationDocument = insured.IdentificationDocument ?? person.IdentificationDocument,
                Name = insured.Name ?? person.FullName,
                //CustomerType = insured.CustomerType,
                CompanyName = new IssuanceCompanyName()

            };

            //if (person.Addresses != null)
            //{
            //    beneficiary.CompanyName.Address = person.Addresses.First();
            //}
            //if (person.Phones != null)
            //{
            //    beneficiary.CompanyName.Phone = person.Phones.First();
            //}
            //if (person.Emails != null)
            //{
            //    beneficiary.CompanyName.Email = person.Emails.First();
            //}
            return beneficiary;
        }

        public CompanyIssuanceInsured CreateInsured(Row row, Holder holder)
        {
            CompanyIssuanceInsured insured = new CompanyIssuanceInsured();

            bool InsuredEqualHolder = (bool)DelegateService.utilitiesService.GetValueByField<bool>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.InsuredEqualHolder));
            bool isWithGroupingRequest = !string.IsNullOrEmpty((string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RequestGroup)));

            if (InsuredEqualHolder)
            {
                //Validar cambio de modelo
                //insured.CustomerType = holder.CustomerType;
                //insured.IndividualType = holder.IndividualType;
                insured.IndividualId = holder.IndividualId;
                insured.Name = holder.Name;
                //insured.IdentificationDocument = holder.IdentificationDocument;
                //insured.EconomicActivity = holder.EconomicActivity;
                insured.InsuredId = holder.InsuredId;
                //insured.PaymentMethod = holder.PaymentMethod;
                insured.BirthDate = holder.BirthDate;
                insured.Gender = holder.Gender;
                insured.DeclinedDate = holder.DeclinedDate;
                //insured.CompanyName = holder.CompanyName;
            }
            else
            {
                int insuredCode = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredCode));

                if (insuredCode > 0)
                {
                    //insured = DelegateService.uniquePersonService.GetInsuredByInsuredCode(insuredCode);

                    //if (insured != null)
                    //{
                    //    if (insured.DeclinedDate > DateTime.MinValue)
                    //    {
                    //        throw new ValidationException(Errors.ErrorInsuredDeclinedDate + " " + row.Number);
                    //    }
                    //    if (insured.CompanyName == null || insured.CompanyName.Address == null)
                    //    {
                    //        throw new ValidationException(Errors.ErrorInsuredAddress);
                    //    }
                    //    if (insured.IndividualType == UPENUM.IndividualType.Person)
                    //    {
                    //        if (insured.BirthDate == null)
                    //        {
                    //            throw new ValidationException(Errors.ErrorInsuredBirthDate);
                    //        }
                    //    }

                    //}
                    //else
                    //{
                    //    throw new ValidationException(Errors.ErrorInsuredNotFound + " " + row.Number);
                    //}

                    //if (!isWithGroupingRequest)
                    //    //UpdateInsuredByFile(insured, row);

                    return insured;
                }
                else
                {
                    string documentNumber = string.Empty;


                    insured.IndividualType = (IndividualType)(int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredIndividualType));

                    switch (insured.IndividualType)
                    {
                        case IndividualType.Company:
                            documentNumber = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredCompanyDocumentNumber)).ToString();
                            insured = DelegateService.underwritingService.GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(documentNumber, InsuredSearchType.DocumentNumber, CustomerType.Individual);

                            if (insured == null)
                            {
                                insured = CreateInsuredByCompany(null, row);
                            }
                            break;
                        case IndividualType.Person:
                            documentNumber = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPersonDocumentNumber)).ToString();
                            break;
                            //UPMO.Person currentPerson = DelegateService.uniquePersonService.GetPersonByDocumentNumber(documentNumber);
                            // Person could be already registered, but InsuredCode was not supplied
                            //    if (currentPerson != null)
                            //    {
                            //        insured = DelegateService.uniquePersonService.GetInsuredByIndividualId(currentPerson.IndividualId);
                            //        insured.CompanyName = DelegateService.uniquePersonService.GetNotificationAddressesByIndividualId(currentPerson.IndividualId, UPENUM.CustomerType.Individual).First();
                            //        if (insured.Gender == null)
                            //        {
                            //            insured.Gender = currentPerson.Gender;
                            //        }
                            //        if (insured.BirthDate == null)
                            //        {
                            //            insured.BirthDate = currentPerson.BirthDate;
                            //        }
                            //        if (insured == null)
                            //            insured = CreateInsuredByPerson(currentPerson, row);
                            //        //else if (!isWithGroupingRequest)
                            //        //    //UpdatePersonByFile(insured, row);
                            //    }
                            //    else
                            //        insured = CreateInsuredPerson(row);

                            //default:
                            //    throw new ValidationException(Errors.ErrorInsuredIndividualType + " " + row.Number);
                    }
                }
            }

            return insured;
        }

        //private void UpdateInsuredByFile(CompanyIssuanceInsured insured, Row row)
        //{
        //    if (insured.IndividualType == IndividualType.Company)
        //        UpdateCompanyByFile(insured, row);
        //   else if (insured.IndividualType == IndividualType.Person)
        //       UpdatePersonByFile(insured, row);
        //}

        //private void UpdatePersonByFile(CompanyIssuanceInsured insured, Row row)
        //{

        //    UPMO.Person currentPerson = DelegateService.uniquePersonService.GetPersonByDocumentNumber(insured.IdentificationDocument.Number);
        //    bool personChanged = false;

        //    string names = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPersonName));
        //    if (!string.IsNullOrEmpty(names)
        //        && currentPerson.Names.Trim().ToLower() != names.Trim().ToLower())
        //    {
        //        currentPerson.Names = names;
        //        personChanged = true;
        //    }

        //    string surname = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPersonSurname));
        //    if (!string.IsNullOrEmpty(surname)
        //        && currentPerson.Surname.Trim().ToLower() != surname.Trim().ToLower())
        //    {
        //        currentPerson.Surname = surname;
        //        personChanged = true;
        //    }

        //    string motherLastName = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPersonSecondSurname));
        //    if (!string.IsNullOrEmpty(motherLastName)
        //        && currentPerson.MotherLastName.Trim().ToLower() != motherLastName.Trim().ToLower())
        //    {
        //        currentPerson.MotherLastName = motherLastName;
        //        personChanged = true;
        //    }

        //    int economicActivityId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredEconomicActivity));
        //    if (economicActivityId > 0
        //        && currentPerson.EconomicActivity != null && currentPerson.EconomicActivity.Id != economicActivityId)
        //    {
        //        currentPerson.EconomicActivity = new EconomicActivity()
        //        {
        //            Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredEconomicActivity))
        //        };
        //        personChanged = true;
        //    }

        //    string gender = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPersonGender));
        //    if (!string.IsNullOrEmpty(gender)
        //        && currentPerson.Gender != gender)
        //    {
        //        currentPerson.Gender = gender;
        //        personChanged = true;
        //    }

        //    int maritalStatusId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPersonMaritalStatus));
        //    if (maritalStatusId > 0
        //        && currentPerson.MaritalStatus != null && currentPerson.MaritalStatus.Id != maritalStatusId)
        //    {
        //        currentPerson.MaritalStatus = new MaritalStatus
        //        {
        //            Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPersonMaritalStatus))
        //        };
        //        personChanged = true;
        //    }

        //    int addressTypeId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressType));
        //    string addressDescription = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressDescription));
        //    int addresssCityId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressCity));
        //    int addresssStateId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressState));
        //    int addresssCountryId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressCountry));
        //    if (addressTypeId > 0
        //        && !string.IsNullOrEmpty(addressDescription)
        //        && addresssCityId > 0
        //        && addresssStateId > 0
        //        && addresssCountryId > 0
        //        && !currentPerson.Addresses.Any(a => a.Description.Trim().ToLower() == addressDescription.Trim().ToLower()))
        //    {
        //        currentPerson.Addresses.Add(new Address()
        //        {
        //            AddressType = new AddressType()
        //            {
        //                Id = addressTypeId
        //            },
        //            Description = addressDescription,
        //            City = new City()
        //            {
        //                Id = addresssCityId,
        //                State = new State()
        //                {
        //                    Id = addresssStateId,
        //                    Country = new Country
        //                    {
        //                        Id = addresssCountryId
        //                    }
        //                }
        //            }
        //        });
        //        personChanged = true;
        //    }

        //    int phoneTypeId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPhoneType));
        //    string phoneDescription = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPhoneDescription));
        //    if (phoneTypeId > 0 && !string.IsNullOrEmpty(phoneDescription)
        //        && !currentPerson.Phones.Any(p => p.Description.Trim() == phoneDescription.Trim()))
        //    {
        //        currentPerson.Phones.Add(new Phone()
        //        {
        //            PhoneType = new PhoneType()
        //            {
        //                Id = phoneTypeId
        //            },
        //            Description = phoneDescription
        //        });
        //        personChanged = true;
        //    }

        //    int emailType = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredEmailType));
        //    string emailDescription = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredEmailDescription));

        //    if (emailType > 0 && !string.IsNullOrEmpty(emailDescription)
        //        && !currentPerson.Emails.Any(e => e.Description.Trim().ToLower() == emailDescription.Trim().ToLower()))
        //    {
        //        currentPerson.Emails.Add(new Email()
        //        {
        //            EmailType = new EmailType()
        //            {
        //                Id = emailType
        //            },
        //            Description = emailDescription
        //        });
        //        personChanged = true;
        //    }
        //    DateTime birthDate = (DateTime)DelegateService.utilitiesService.GetValueByField<DateTime>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPersonBirthDate));

        //    if (personChanged)
        //        DelegateService.uniquePersonService.UpdatePerson(currentPerson, null);
        //    if (insured.Gender != gender)
        //    {
        //        insured.Gender = gender;
        //    }
        //    if (insured.BirthDate != birthDate)
        //    {
        //        insured.BirthDate = birthDate;
        //    }
        //    //if(insured.CompanyName == null)
        //    //{
        //    //    insured.CompanyName = 
        //    //}
        //}

        //private void UpdateCompanyByFile(CompanyIssuanceInsured insured, Row row)
        //{

        //    UniquePersonServices.Models.Company currentCompany = DelegateService.underwritingService.GetCompanyByDocumentNumber(insured.IdentificationDocument.Number);
        //    bool companyChanged = false;

        //    string name = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredCompanyTradeName));
        //    if (!string.IsNullOrEmpty(name)
        //        && currentCompany.Name.Trim().ToLower() != name.Trim().ToLower())
        //    {
        //        currentCompany.Name = name;
        //        companyChanged = true;
        //    }

        //    int economicActivityId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredEconomicActivity));
        //    if (economicActivityId > 0
        //        && currentCompany.EconomicActivity != null && currentCompany.EconomicActivity.Id != economicActivityId)
        //    {
        //        currentCompany.EconomicActivityId = economicActivityId;
        //        companyChanged = true;
        //    }

        //    int companyTypeId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredCompanyType));
        //    if (companyTypeId > 0
        //        && currentCompany.CompanyType != null && currentCompany.CompanyType.Id != companyTypeId)
        //    {
        //        currentCompany.CompanyTypeId = companyTypeId;
        //        companyChanged = true;
        //    }

        //    int companyCountryOriginId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressCountry));
        //    if (companyCountryOriginId > 0
        //        && currentCompany.CountryOrigin != null && currentCompany.CountryOrigin.Id != companyCountryOriginId)
        //    {
        //        currentCompany.CountryOrigin = new Country
        //        {
        //            Id = companyCountryOriginId
        //        };
        //        companyChanged = true;
        //    }

        //    int addressTypeId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressType));
        //    string addressDescription = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressDescription));
        //    int addresssCityId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressCity));
        //    int addresssStateId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressState));
        //    int addresssCountryId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressCountry));
        //    if (addressTypeId > 0
        //        && !string.IsNullOrEmpty(addressDescription)
        //        && addresssCityId > 0
        //        && addresssStateId > 0
        //        && addresssCountryId > 0
        //        && currentCompany.Addresses != null
        //        && !currentCompany.Addresses.Any(a => a.Description.Trim().ToLower() == addressDescription.Trim().ToLower()))
        //    {
        //        currentCompany.Addresses.Add(new Address()
        //        {
        //            AddressType = new AddressType()
        //            {
        //                Id = addressTypeId
        //            },
        //            Description = addressDescription,
        //            City = new City()
        //            {
        //                Id = addresssCityId,
        //                State = new State()
        //                {
        //                    Id = addresssStateId,
        //                    Country = new Country
        //                    {
        //                        Id = addresssCountryId
        //                    }
        //                }
        //            }
        //        });
        //        companyChanged = true;
        //    }
        //    int companyPhoneTypeId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPhoneType));
        //    string companyPhoneDescription = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPhoneDescription));
        //    if (companyPhoneTypeId > 0 && !string.IsNullOrEmpty(companyPhoneDescription)
        //        && currentCompany.Phones != null
        //        && !currentCompany.Phones.Any(p => p.Description.Trim() == companyPhoneDescription.Trim()))
        //    {
        //        currentCompany.Phones.Add(new Phone()
        //        {
        //            PhoneType = new PhoneType()
        //            {
        //                Id = companyPhoneTypeId
        //            },
        //            Description = companyPhoneDescription
        //        });
        //    }

        //    string companyCellPhoneDescription = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPhoneDescription));
        //    if (!string.IsNullOrEmpty(companyCellPhoneDescription)
        //        && currentCompany.Phones != null
        //        && !currentCompany.Phones.Any(p => p.Description.Trim() == companyCellPhoneDescription.Trim()))
        //    {
        //        currentCompany.Phones.Add(new Phone()
        //        {
        //            PhoneType = new PhoneType()
        //            {
        //                Id = 4
        //            },
        //            Description = companyCellPhoneDescription
        //        });
        //    }

        //    if (companyChanged)
        //        DelegateService.uniquePersonService.UpdateCompany(currentCompany);
        //    //CreateHolderByInsuredCompany(insured, company);
        //}

        private CompanyIssuanceInsured CreateInsuredCompany(Row row)
        {
            MassiveCompany company = new MassiveCompany
            {
                IndividualType = IndividualType.Company,
                FullName = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredCompanyTradeName)),
                EconomicActivityId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredEconomicActivity)),
                DocumentTypeId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredCompanyDocumentType)),
                DocumentNumber = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredCompanyDocumentNumber)),
                CompanyTypeId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredCompanyType)),
                CountryOrigin = new Country
                {
                    Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressCountry)),
                },
                AddressTypeId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressType)),
                AddressDescription = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressDescription)),
                AddressCity = new City()
                {
                    Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressCity)),
                    State = new State()
                    {
                        Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressState)),
                        Country = new Country
                        {
                            Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressCountry))
                        }
                    }

                },
                PhoneId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPhoneType)),
                PhoneDescription = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPhoneDescription)),
                EmailTypeId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredEmailType)),
                EmailDescription = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredEmailDescription))
            };

            CompanyIssuanceInsured insured = CreateInsuredByCompany(company, row);
            CreateHolderByInsuredCompany(insured, company);
            return insured;
        }

        private CompanyIssuanceInsured CreateInsuredByCompany(MassiveCompany company, Row row)
        {
            bool isCommercialClient = (bool)DelegateService.utilitiesService.GetValueByField<bool>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredCommercialAgreement));
            bool isMailAddress = (bool)DelegateService.utilitiesService.GetValueByField<bool>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredSendingMail));
            bool isSms = (bool)DelegateService.utilitiesService.GetValueByField<bool>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredSMSSending));

            CompanyIssuanceInsured insured = new CompanyIssuanceInsured
            {
                IndividualId = company.IndividualId,
                Name = company.FullName,
                EnteredDate = DateTime.Now,
                Profile = 2,
            };
            return insured;
        }

        private Holder CreateHolderByInsuredCompany(CompanyIssuanceInsured insured, MassiveCompany company)
        {
            Holder holder = new Holder
            {
                IndividualId = company.IndividualId,
                //InsuredId = insured.Id,
                CompanyName = new IssuanceCompanyName()

            };

            //if (company.Addresses != null)
            //{
            //    holder.CompanyName.Address = company.Addresses.First();
            //}
            //if (company.Phones != null)
            //{
            //    holder.CompanyName.Phone = company.Phones.First();
            //}
            //if (company.Emails != null)
            //{
            //    holder.CompanyName.Email = company.Emails.First();
            //}
            return holder;
        }

        private CompanyIssuanceInsured CreateInsuredPerson(Row row)
        {
            MassivePerson person = new MassivePerson
            {
                IndividualType = IndividualType.Person,
                FullName = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPersonName)),
                Surname = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPersonSurname)),
                SecondSurname = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPersonSecondSurname)),
                EconomicActivityId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredEconomicActivity)),


                DocumentTypeId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPersonDocumentType)),
                DocumentNumber = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPersonDocumentNumber)),
                Gender = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPersonGender)),
                MaritalStatusId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPersonMaritalStatus)),
                BirthDate = (DateTime)DelegateService.utilitiesService.GetValueByField<DateTime>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPersonBirthDate)),
                EducativeLevelId = 1,
                HouseTypeId = 1,
                SocialLayerId = 1,
                LaborPersonId = 1,
                AddressTypeId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressType)),
                AddressDescription = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressDescription)),
                AddressCity = new City()
                {
                    Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressCity)),
                    State = new State()
                    {
                        Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressState)),
                        Country = new Country
                        {
                            Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressCountry))
                        }
                    }
                },
                PhoneTypeId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPhoneType)),
                PhoneDescription = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPhoneDescription)),
                EmailTypeId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredEmailType)),
                EmailDescription = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredEmailDescription))
            };
            ///Inicio cargue masivo Company
            //person.Phones.Add(new Phone()
            //{
            //    PhoneType = new PhoneType()
            //    {
            //        Id = 4
            //    },
            //    Description = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.InsuredCellNumber))
            //});
            ///Fin cargue masivo Company

            return CreateInsuredByPerson(person, row);
        }

        private CompanyIssuanceInsured CreateInsuredByPerson(MassivePerson person, Row row)
        {
            bool isCommercialClient = (bool)DelegateService.utilitiesService.GetValueByField<bool>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredCommercialAgreement));
            bool isMailAddress = (bool)DelegateService.utilitiesService.GetValueByField<bool>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredSendingMail));
            bool isSms = (bool)DelegateService.utilitiesService.GetValueByField<bool>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredSMSSending));

            CompanyIssuanceInsured insured = new CompanyIssuanceInsured
            {
                IndividualId = person.IndividualId,
                //IdentificationDocument = person.IdentificationDocument,
                Name = person.FullName + " " + person.Surname + " " + person.SecondSurname,
                EnteredDate = DateTime.Now,
                Profile = 2,
                //BranchCode = 1,
                // identificar el cambio del modelo
                //IsCommercialClient = isCommercialClient == true ? 1 : 0,
                //IsMailAddress = isMailAddress,
                //IsSms = isSms == true ? 1 : 0,
            };

            //insured = DelegateService.uniquePersonService.CreateInsured(insured);

            //insured.IdentificationDocument = person.IdentificationDocument;
            //insured.CompanyName = new CompanyName
            //{
            //    Address = person.Addresses.First(),
            //    Phone = person.Phones.First(),
            //    Email = person.Emails.First()
            //};
            //insured.BirthDate = person.BirthDate;
            //insured.Gender = person.Gender;

            return insured;
        }

        public LimitRc CreateLimitRc(Row row, int prefixId, int productId, int policyTypeId)
        {
            Field field = row.Fields.Find(y => y.PropertyName == FieldPropertyName.RiskLimitRc);

            LimitRc limitRc;
            List<LimitRc> limitRC = DelegateService.underwritingService.GetLimitsRcByPrefixIdProductIdPolicyTypeId(prefixId, productId, policyTypeId);
            if (string.IsNullOrEmpty(field.Value))
            {
                if ((limitRc = limitRC.OrderBy(x => x.Id).FirstOrDefault()) == null)
                {
                    throw new ValidationException(Errors.NoLimitRcRelatedToProduct);
                }
                return limitRc;
            }
            int riskLimitRc = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(y => y.PropertyName == FieldPropertyName.RiskLimitRc));
            if ((limitRc = limitRC.Find(x => x.Id == riskLimitRc)) == null)
            {
                throw new ValidationException(Errors.ErrorInvalidLimitRcId);
            }
            return limitRc;
        }

        public List<PendingOperation> GetRiskPendingOperationsByPolicyOperationId(int policyOperationId)
        {
            List<PendingOperation> pendingOperations;
            return pendingOperations = DelegateService.utilitiesService.GetPendingOperationsByParentId(policyOperationId);
        }

        /// <summary>
        /// Genera el archivo de error del proceso de cancelacion masiva
        /// </summary>
        /// <param name="massiveLoadId">The massive load identifier.</param>
        /// <param name="withErrors">if set to <c>true</c> [with errors].</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string GenerateFileErrorsMassiveCancellation(int massiveLoadId)
        {
            object objlock = new object();
            MassiveLoad massiveLoad = DelegateService.massiveService.GetMassiveLoadByMassiveLoadId(massiveLoadId);
            List<MassiveCancellationRow> massiveCancellationRows = DelegateService.massiveUnderwritingService.GetMassiveCancellationRowsWithErrorsWithEventsByMassiveLoadId(massiveLoadId, null, null);
            FileProcessValue fileProcessValue = new FileProcessValue();
            fileProcessValue.Key1 = (int)FileProcessType.CancellationMassive;

            File file = DelegateService.utilitiesService.GetFileByFileProcessValue(fileProcessValue);
            Template template = file.Templates.First(x => x.PropertyName == TemplatePropertyName.MassiveCancellation);
            file.Templates[0].Rows.Last().Fields.Add(new Field
            {
                ColumnSpan = 1,
                FieldType = FieldType.String,
                Description = "Errores",
                IsEnabled = true,
                IsMandatory = false,
                Id = 0,
                Order = file.Templates[0].Rows.Last().Fields.Count(),
                RowPosition = file.Templates[0].Rows.Last().Fields.First().RowPosition
            });

            file.Templates[0].Rows.Last().Fields.Add(new Field
            {
                ColumnSpan = 1,
                FieldType = FieldType.String,
                Description = "Eventos",
                IsEnabled = true,
                IsMandatory = false,
                Id = 0,
                Order = file.Templates[0].Rows.Last().Fields.Count(),
                RowPosition = file.Templates[0].Rows.Last().Fields.First().RowPosition
            });
            massiveCancellationRows = massiveCancellationRows.OrderBy(x => x.RowNumber).ToList();

            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
            CompanyMassiveUnderwritingGetAuthorizations view = new CompanyMassiveUnderwritingGetAuthorizations();
            ViewBuilder builder = new ViewBuilder("GetAuthorizations");

            where.Property(APEntity.AuthorizationRequest.Properties.Key, "AutorizarionRequest").Equal().Constant(massiveLoadId.ToString());
            where.And().Property(APEntity.AuthorizationRequest.Properties.StatusId, "AutorizarionRequest").Distinct().Constant(Core.Application.AuthorizationPoliciesServices.Enums.TypeStatus.Authorized);
            builder.Filter = where.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            List<APEntity.AuthorizationRequest> totalRequests = view.AutorizarionRequest.AsParallel().Cast<APEntity.AuthorizationRequest>().ToList();
            List<APEntity.AuthorizationAnswer> totalAnswers = view.AutorizarionAnswer.AsParallel().Cast<APEntity.AuthorizationAnswer>().ToList();
            List<APEntity.Policies> totalPolicies = view.Policies.AsParallel().Cast<APEntity.Policies>().ToList();
            List<Core.Application.UniqueUser.Entities.UniqueUsers> totalUsers = view.Users.AsParallel().Cast<Core.Application.UniqueUser.Entities.UniqueUsers>().ToList();

            foreach (MassiveCancellationRow proccess in massiveCancellationRows)
            {
                Row rowSerialized = JsonConvert.DeserializeObject<Row>(proccess.SerializedRow);
                ConcurrentBag<string> eventMessaje = new ConcurrentBag<string>();
                template.Rows.Add(rowSerialized);

                file.Templates[0].Rows.Last().Fields.Add(new Field
                {
                    ColumnSpan = 1,
                    FieldType = FieldType.String,
                    Value = proccess.HasError ? proccess.Observations : string.Empty,
                    Description = "Errores",
                    IsEnabled = true,
                    IsMandatory = false,
                    Id = 0,
                    Order = file.Templates[0].Rows.Last().Fields.Count(),
                    RowPosition = file.Templates[0].Rows.Last().Fields.First().RowPosition
                });

                if (proccess.HasEvents && proccess.SerializedRow != null)
                {
                    List<APEntity.AuthorizationRequest> requests = totalRequests.AsParallel().Where(x => !string.IsNullOrEmpty(x.Key2) && x.Key2.Split('|').Where(k2 => k2 == proccess.Risk.Policy.Id.ToString()).Count() > 0).ToList();

                    ParallelHelper.ForEach(requests, request =>
                    {
                        APEntity.Policies policie = totalPolicies.AsParallel().First(x => x.PoliciesId == request.PoliciesId);
                        APEntity.AuthorizationAnswer answer = totalAnswers.AsParallel().First(x => x.AuthorizationRequestId == request.AuthorizationRequestId);
                        Core.Application.UniqueUser.Entities.UniqueUsers user = totalUsers.AsParallel().First(x => x.UserId == answer.UserAnswerId);

                        if (request.StatusId == (int)Core.Application.AuthorizationPoliciesServices.Enums.TypeStatus.Rejected)
                        {
                            eventMessaje.Add(Errors.Rejected + ": " + policie.Message + " (" + Errors.AuthorizingUser + user.AccountName + ")");
                        }
                        else if (request.StatusId == (int)Core.Application.AuthorizationPoliciesServices.Enums.TypeStatus.Pending)
                        {
                            eventMessaje.Add(Errors.Authorization + ": " + policie.Message + " (" + Errors.AuthorizingUser + user.AccountName + ")");
                        }
                    });
                }

                file.Templates[0].Rows.Last().Fields.Add(new Field
                {
                    ColumnSpan = 1,
                    FieldType = FieldType.String,
                    Description = "Eventos",
                    Value = string.Join(" |", eventMessaje),
                    IsEnabled = true,
                    IsMandatory = false,
                    Id = 0,
                    Order = file.Templates[0].Rows.Last().Fields.Count(),
                    RowPosition = file.Templates[0].Rows.Last().Fields.First().RowPosition
                });
            }

            file.Name = "Errores_" + DateTime.Now.ToString("dd_MM_yyyy_ssms");
            return DelegateService.utilitiesService.GenerateFile(file);
        }

        /// <summary>
        /// Genera el archivo de error del proceso de emisión masiva
        /// </summary>
        /// <param name="massiveLoadProccessId"></param>
        /// <returns></returns>
        public string GenerateFileErrorsMassiveEmission(int massiveLoadId)
        {
            MassiveEmission massiveEmission = new MassiveEmission();

            massiveEmission = DelegateService.massiveUnderwritingService.GetMassiveEmissionByMassiveLoadId(massiveLoadId);

            massiveEmission.Rows = DelegateService.massiveUnderwritingService.GetMassiveEmissionRowsByMassiveLoadIdMassiveLoadProcessStatus(massiveLoadId, null, null, null);

            SubCoveredRiskType subCoveredRiskType = DelegateService.commonService.GetSubCoveredRiskTypeByPrefixIdCoveredRiskTypeId(massiveEmission.Prefix.Id, (int)massiveEmission.CoveredRiskType);

            FileProcessValue fileProcessValue = new FileProcessValue();

            switch (subCoveredRiskType)
            {
                case SubCoveredRiskType.Vehicle:

                    fileProcessValue.Key1 = (int)FileProcessType.MassiveEmission;
                    fileProcessValue.Key3 = massiveEmission.LoadType.Id;
                    fileProcessValue.Key4 = massiveEmission.Prefix.Id;
                    fileProcessValue.Key5 = (int)SubCoveredRiskType.Vehicle;

                    break;
                case SubCoveredRiskType.ThirdPartyLiability:

                    fileProcessValue.Key1 = (int)FileProcessType.MassiveEmission;
                    fileProcessValue.Key3 = massiveEmission.LoadType.Id;
                    fileProcessValue.Key4 = massiveEmission.Prefix.Id;
                    fileProcessValue.Key5 = (int)SubCoveredRiskType.ThirdPartyLiability;

                    break;
                case SubCoveredRiskType.Property:

                    fileProcessValue.Key1 = (int)FileProcessType.MassiveEmission;
                    fileProcessValue.Key3 = massiveEmission.LoadType.Id;
                    fileProcessValue.Key4 = massiveEmission.Prefix.Id;
                    fileProcessValue.Key5 = (int)SubCoveredRiskType.Property;

                    break;
                case SubCoveredRiskType.Liability:

                    fileProcessValue.Key1 = (int)FileProcessType.MassiveEmission;
                    fileProcessValue.Key3 = massiveEmission.LoadType.Id;
                    fileProcessValue.Key4 = massiveEmission.Prefix.Id;
                    fileProcessValue.Key5 = (int)SubCoveredRiskType.Liability;

                    break;
            }

            File file = DelegateService.utilitiesService.GetFileByFileProcessValue(fileProcessValue);

            file.Templates[0].Rows.Last().Fields.Add(new Field
            {
                ColumnSpan = 1,
                FieldType = FieldType.String,
                Description = "Errores",
                IsEnabled = true,
                IsMandatory = false,
                Id = 0,
                Order = file.Templates[0].Rows.Last().Fields.Count(),
                RowPosition = file.Templates[0].Rows.Last().Fields.First().RowPosition
            });

            file.Templates[0].Rows.Last().Fields.Add(new Field
            {
                ColumnSpan = 1,
                FieldType = FieldType.String,
                Description = "Eventos",
                IsEnabled = true,
                IsMandatory = false,
                Id = 0,
                Order = file.Templates[0].Rows.Last().Fields.Count(),
                RowPosition = file.Templates[0].Rows.Last().Fields.First().RowPosition
            });
            massiveEmission.Rows = massiveEmission.Rows.OrderBy(x => x.RowNumber).ToList();

            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
            CompanyMassiveUnderwritingGetAuthorizations view = new CompanyMassiveUnderwritingGetAuthorizations();
            ViewBuilder builder = new ViewBuilder("GetAuthorizations");

            where.Property(APEntity.AuthorizationRequest.Properties.Key, "AutorizarionRequest").Equal().Constant(massiveLoadId.ToString());
            where.And().Property(APEntity.AuthorizationRequest.Properties.StatusId, "AutorizarionRequest").Distinct().Constant((int)Core.Application.AuthorizationPoliciesServices.Enums.TypeStatus.Authorized);
            builder.Filter = where.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            List<APEntity.AuthorizationRequest> totalRequests = view.AutorizarionRequest.AsParallel().Cast<APEntity.AuthorizationRequest>().ToList();
            List<APEntity.AuthorizationAnswer> totalAnswers = view.AutorizarionAnswer.AsParallel().Cast<APEntity.AuthorizationAnswer>().ToList();
            List<APEntity.Policies> totalPolicies = view.Policies.AsParallel().Cast<APEntity.Policies>().ToList();
            List<Core.Application.UniqueUser.Entities.UniqueUsers> totalUsers = view.Users.AsParallel().Cast<Core.Application.UniqueUser.Entities.UniqueUsers>().ToList();

            foreach (MassiveEmissionRow proccess in massiveEmission.Rows)
            {
                File fileSerialized = JsonConvert.DeserializeObject<File>(proccess.SerializedRow);
                ConcurrentBag<string> eventMessaje = new ConcurrentBag<string>();
                foreach (Template t in fileSerialized.Templates)
                {
                    FormatRows(t.Rows);
                    file.Templates.Find(x => x.PropertyName == t.PropertyName).Rows.AddRange(t.Rows);
                }

                file.Templates[0].Rows.Last().Fields.Add(new Field
                {
                    ColumnSpan = 1,
                    FieldType = FieldType.String,
                    Value = proccess.Observations,
                    Description = "Errores",
                    IsEnabled = true,
                    IsMandatory = false,
                    Id = 0,
                    Order = file.Templates[0].Rows.Last().Fields.Count(),
                    RowPosition = file.Templates[0].Rows.Last().Fields.First().RowPosition
                });

                if (proccess.HasEvents && proccess.SerializedRow != null)
                {
                    List<APEntity.AuthorizationRequest> requests = totalRequests.AsParallel().Where(x => x.Key2 == proccess.Risk.Policy.Id.ToString()).ToList();

                    ParallelHelper.ForEach(requests, request =>
                    {
                        APEntity.Policies policie = totalPolicies.AsParallel().First(x => x.PoliciesId == request.PoliciesId);
                        APEntity.AuthorizationAnswer answer = totalAnswers.AsParallel().First(x => x.AuthorizationRequestId == request.AuthorizationRequestId);
                        Core.Application.UniqueUser.Entities.UniqueUsers user = totalUsers.AsParallel().First(x => x.UserId == answer.UserAnswerId);

                        if (request.StatusId == (int)Core.Application.AuthorizationPoliciesServices.Enums.TypeStatus.Rejected)
                        {
                            eventMessaje.Add(Errors.Rejected + ": " + policie.Message + " (" + Errors.AuthorizingUser + user.AccountName + ")");
                        }
                        else if (request.StatusId == (int)Core.Application.AuthorizationPoliciesServices.Enums.TypeStatus.Pending)
                        {
                            eventMessaje.Add(Errors.Authorization + ": " + policie.Message + " (" + Errors.AuthorizingUser + user.AccountName + ")");
                        }
                    });
                }

                file.Templates[0].Rows.Last().Fields.Add(new Field
                {
                    ColumnSpan = 1,
                    FieldType = FieldType.String,
                    Description = "Eventos",
                    Value = string.Join(" |", eventMessaje),
                    IsEnabled = true,
                    IsMandatory = false,
                    Id = 0,
                    Order = file.Templates[0].Rows.Last().Fields.Count(),
                    RowPosition = file.Templates[0].Rows.Last().Fields.First().RowPosition
                });
            }

            file.Name = "Errores_" + DateTime.Now.ToString("dd_MM_yyyy_ssms");
            return DelegateService.utilitiesService.GenerateFile(file);
        }

        private void FormatRows(List<Row> rows)
        {
            TP.Parallel.ForEach(rows.AsParallel(),
            (row) =>
            {
                foreach (Field field in row.Fields.Where(u => u.FieldType == FieldType.Boolean))
                {
                    switch (field.Value)
                    {
                        case "True":
                            field.Value = "SI";
                            break;
                        case "False":
                            field.Value = "NO";
                            break;
                    }
                }
            });
        }
    }
}