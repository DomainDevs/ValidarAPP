using AutoMapper;
using Newtonsoft.Json;
using Sistran.Company.Application.CollectiveServices.EEProvider.Assemblers;
using Sistran.Company.Application.CollectiveServices.EEProvider.Resources;
using Sistran.Company.Application.CollectiveServices.Models;
using Sistran.Company.Application.CommonServices.Models;
using Sistran.Company.Application.MassiveServices.Models;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;

using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Company.Application.Utilities.Constants;
using Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.Models;
using Sistran.Core.Application.CollectiveServices.EEProvider.DAOs;
using Sistran.Core.Application.CollectiveServices.Enums;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.ProductServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
//using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnumsCore = Sistran.Core.Application.UnderwritingServices.Enums;
using UPDTOs = Sistran.Company.Application.UniquePersonAplicationServices.DTOs;
using TP = Sistran.Core.Application.Utilities.Utility;
using Sistran.Core.Application.UnderwritingServices.DTOs.Filter;

namespace Sistran.Company.Application.CollectiveServices.EEProvider.DAOs
{
    public class CollectiveLoadPolicyDAO
    {
        string templateName = "";
        private void GetClauses(CompanyPolicy policy)
        {
            IMapper mapper = ModelAssembler.CreateMapCompanyClause();
            List<CompanyClause> clauses = mapper.Map<List<Clause>, List<CompanyClause>>(DelegateService.underwritingServiceCore.GetClausesByEmissionLevelConditionLevelId(EnumsCore.EmissionLevel.General, policy.Prefix.Id));

            if (policy.Clauses != null)
            {
                policy.Clauses = policy.Clauses.Where(x => x.IsMandatory == false).ToList();
            }
            else
            {
                policy.Clauses = new List<CompanyClause>();
            }

            if (clauses.Count > 0)
            {
                policy.Clauses.AddRange(clauses.Where(x => x.IsMandatory == true).ToList());
            }
        }

        public CompanyPolicy CreateCompanyPolicy(CollectiveEmission collectiveEmission, File file, string templatePropertyName, List<FilterIndividual> filtersIndividuals, int riskCount)
        {
            try
            {
                Row row = file.Templates.First(x => x.PropertyName == templatePropertyName).Rows.Where(w => w.HasError == false).FirstOrDefault();
                templateName = file.Templates.First(x => x.PropertyName == templatePropertyName).Description;
                List<CompanySalesPoint> companySalesPoints = new List<CompanySalesPoint>() { new CompanySalesPoint() };
                CompanyPolicy companyPolicy = new CompanyPolicy
                {
                    Endorsement = new CompanyEndorsement
                    {
                        EndorsementType = EndorsementType.Emission,
                        IsMassive = true
                    },
                    TemporalType = TemporalType.Policy,
                    UserId = collectiveEmission.User.UserId,
                    IssueDate = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Now),
                    BeginDate = DateTime.Now,
                    Prefix = new CompanyPrefix
                    {
                        Id = collectiveEmission.Prefix.Id
                    },
                    Branch = new CompanyBranch
                    {
                        Id = collectiveEmission.Branch.Id,
                        SalePoints = companySalesPoints
                    },
                    Summary = new CompanySummary
                    {
                        RiskCount = riskCount
                    },
                    Text = new CompanyText { }
                };

                companyPolicy.Clauses = DelegateService.massiveService.GetClausesObligatory(EmissionLevel.General, companyPolicy.Prefix.Id, null);

                companyPolicy.Endorsement.TicketNumber = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.TicketNumber));
                companyPolicy.Endorsement.TicketDate = (DateTime)DelegateService.utilitiesService.GetValueByField<DateTime>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.TicketDate));
                companyPolicy.Text.TextBody = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyText));
                companyPolicy.Branch.SalePoints[0].Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.SalePoint));
                companyPolicy.EffectPeriod = DelegateService.commonService.GetExtendedParameterByParameterId(1027).NumberParameter.Value;

                companyPolicy.PolicyOrigin = PolicyOrigin.Collective;
                int productId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyProductCode));
                companyPolicy.Product = DelegateService.productService.GetCompanyProductByProductIdPrefixId(productId, companyPolicy.Prefix.Id);
                companyPolicy.CalculateMinPremium = companyPolicy.Product.CalculateMinPremium;
                companyPolicy.SubMassiveProcessType = SubMassiveProcessType.CollectiveEmission;

                List<PaymentPlan> paymentPlans = DelegateService.underwritingService.GetPaymentPlansByProductId(companyPolicy.Product.Id);
                PaymentPlan holderPaymentPlan = null;

                int holderPaymentPlanId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPaymentPlan));
                if (holderPaymentPlanId > 0)
                {
                    holderPaymentPlan = paymentPlans.FirstOrDefault(p => p.Id == holderPaymentPlanId);
                    if (holderPaymentPlan == null)
                    {
                        throw new ValidationException(string.Format(Errors.ErrorPaymentPlanNotParameterized, holderPaymentPlanId));
                    }
                }
                else
                {
                    if (paymentPlans.Exists(u => u.IsDefault))
                    {
                        holderPaymentPlan = paymentPlans.FirstOrDefault(u => u.IsDefault);
                    }
                    else
                    {
                        holderPaymentPlan = paymentPlans.First();
                    }
                }


                companyPolicy.PaymentPlan = new CompanyPaymentPlan
                {
                    Id = holderPaymentPlan.Id,
                    Description = holderPaymentPlan.Description,
                    Quotas = holderPaymentPlan.Quotas
                };

                companyPolicy.Holder = DelegateService.massiveService.CreateHolder(row, filtersIndividuals);
                int holderPaymentMethod = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPaymentMethod));
                if (holderPaymentMethod != 0 && companyPolicy.Holder.PaymentMethod.Id != holderPaymentMethod)
                {
                    throw new ValidationException(string.Format(Errors.ErrorPaymentMethodNotParameterizedHolder, holderPaymentMethod));
                }

                companyPolicy.Product.StandardCommissionPercentage = Convert.ToDecimal(DelegateService.utilitiesService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.StandardCommissionPercentage)));
                companyPolicy.Agencies = new List<IssuanceAgency>();
                companyPolicy.Agencies.Add(CreateAgencyPrincipal(collectiveEmission, productId, companyPolicy.Product.StandardCommissionPercentage.GetValueOrDefault()));

                companyPolicy.BusinessType = (BusinessType)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyBusinessType));
                if (companyPolicy.BusinessType == BusinessType.Accepted)
                {
                    Template coinsuranceAcceptedTemplate = file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.CoinsuranceAccepted);
                    if (coinsuranceAcceptedTemplate == null)
                    {
                        throw new ValidationException(Errors.ErrorCoinsuranceAcceptedTemplateMandatory);
                    }

                }
                if (companyPolicy.BusinessType == BusinessType.Assigned)
                {
                    Template coinsuranceAssignedTemplate = file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.CoinsuranceAssigned);
                    if (coinsuranceAssignedTemplate == null)
                    {
                        throw new ValidationException(Errors.ErrorCoinsuranceAssignedTemplateMandatory);
                    }

                }
                companyPolicy.CoInsuranceCompanies = new List<CompanyIssuanceCoInsuranceCompany>();
                companyPolicy.CurrentFrom = (DateTime)DelegateService.utilitiesService.GetValueByField<DateTime>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCurrentFrom));
                companyPolicy.CurrentTo = (DateTime)DelegateService.utilitiesService.GetValueByField<DateTime>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCurrentTo));
                companyPolicy.ExchangeRate = DelegateService.commonService.GetExchangeRateByRateDateCurrencyId(DateTime.Now, (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCurrency)));

                PolicyType policyType = DelegateService.commonService.GetPolicyTypesByProductId(companyPolicy.Product.Id).FirstOrDefault(x => x.Id == (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(y => y.PropertyName == FieldPropertyName.PolicyType)));
                companyPolicy.PolicyType = new CompanyPolicyType
                {
                    Id = policyType.Id
                };

                switch (companyPolicy.BusinessType)
                {
                    case BusinessType.CompanyPercentage:
                        companyPolicy.CoInsuranceCompanies.Add(new CompanyIssuanceCoInsuranceCompany
                        {
                            ParticipationPercentageOwn = 100
                        });
                        break;
                    case BusinessType.Assigned:
                        Template templateCoinsuranceAssigned = file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.CoinsuranceAssigned);

                        if (templateCoinsuranceAssigned == null)
                        {
                            templateName = "";
                            throw new ValidationException(Errors.ErrorCoinsuranceAssignedTemplateMandatory);
                        }

                        templateName = templateCoinsuranceAssigned.Description;

                        companyPolicy.CoInsuranceCompanies = DelegateService.massiveService.CreateCoInsuranceAssigned(companyPolicy, templateCoinsuranceAssigned);

                        break;
                    case BusinessType.Accepted:
                        Template templateCoinsuranceAccepted = file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.CoinsuranceAccepted);

                        if (templateCoinsuranceAccepted == null)
                        {
                            templateName = "";
                            throw new ValidationException(Errors.ErrorCoinsuranceAcceptedTemplateMandatory);
                        }

                        templateName = templateCoinsuranceAccepted.Description;


                        Template templateCoinsuranceAcceptedAgency = file.Templates.FirstOrDefault(x => x.PropertyName == CompanyTemplatePropertyName.CoinsuranceAcceptedAgency);
                        companyPolicy.CoInsuranceCompanies = DelegateService.massiveService.CreateCoInsuranceAccepted(companyPolicy, file);


                        break;
                }


                Template templateAdditionalIntermediaries = file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.AdditionalIntermediaries);
                if (templateAdditionalIntermediaries != null)
                {
                    string error = string.Empty;
                    List<IssuanceAgency> issuanceAgencies = DelegateService.massiveService.GetAgenciesValidation(file, companyPolicy.Agencies, ref error);
                    if (string.IsNullOrEmpty(error))
                    {
                        companyPolicy.Agencies = issuanceAgencies;
                    }
                    else
                    {
                        throw new ValidationException(error);
                    }
                }



                var correlativePolicyNumber = (decimal)DelegateService.utilitiesService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCoCorrelativePolicyNumber));

                if (correlativePolicyNumber != default(decimal))
                {
                    companyPolicy.CorrelativePolicyNumber = correlativePolicyNumber;
                }

                companyPolicy.Endorsement.EndorsementDays = (companyPolicy.CurrentTo - companyPolicy.CurrentFrom).Days;

                Template templateClauses = file.Templates.Find(p => p.PropertyName == TemplatePropertyName.Clauses);

                if (templateClauses != null)
                {
                    List<ConditionLevel> conditionLevels = DelegateService.underwritingService.GetConditionLevels();
                    templateName = templateClauses.Description;

                    foreach (Row clausesRow in templateClauses.Rows)
                    {
                        int levelCode = (int)DelegateService.utilitiesService.GetValueByField<int>(clausesRow.Fields.Find(p => p.PropertyName == FieldPropertyName.LevelCode));
                        int clauseCode = (int)DelegateService.utilitiesService.GetValueByField<int>(clausesRow.Fields.Find(p => p.PropertyName == FieldPropertyName.ClauseCode));
                        ConditionLevel conditionLevel = conditionLevels.FirstOrDefault(c => c.Id == levelCode);

                        if (conditionLevel.EmissionLevel == EmissionLevel.General)
                        {

                            Clause clause = new Clause();
                            clause = DelegateService.underwritingService.GetClauseByClauseId(clauseCode);

                            if (clause == null)
                            {
                                throw new ValidationException(string.Format(Errors.MessageClauseNotExists, clauseCode));
                            }

                            CompanyClause companyClause = new CompanyClause()
                            {
                                Id = clause.Id,
                                Name = clause.Name,
                                Text = clause.Text,
                                Title = clause.Title,
                                IsMandatory = clause.IsMandatory,
                                ConditionLevel = clause.ConditionLevel
                            };

                            if (companyPolicy.Clauses.FirstOrDefault(c => c.Id == companyClause.Id) == null)
                            {
                                companyPolicy.Clauses.Add(companyClause);
                            }
                        }
                    }
                }

                templateName = "";

                PendingOperation pendingOperation = new PendingOperation
                {
                    Operation = JsonConvert.SerializeObject(companyPolicy),
                    UserId = companyPolicy.UserId,
                    IsMassive = true
                };

                if (Settings.UseReplicatedDatabase())
                {
                    pendingOperation = DelegateService.pendingOperationEntityService.CreatePendingOperation(pendingOperation);
                }
                else
                {
                    pendingOperation = DelegateService.utilitiesService.CreatePendingOperation(pendingOperation);
                }

                companyPolicy.Id = pendingOperation.Id;

                return companyPolicy;
            }
            catch (Exception ex)
            {
                if (string.IsNullOrEmpty(templateName))
                {
                    throw new ValidationException(ex.Message);
                }
                else
                {
                    throw new ValidationException(string.Format(Errors.ErrorInTemplate, templateName, ex.Message));
                }
            }
        }

        /// <summary>
        /// Crear Agencia Principal
        /// </summary>
        /// <param name="collectiveLoad">Cargue</param>
        /// <returns>Agencia</returns>
        private IssuanceAgency CreateAgencyPrincipal(CollectiveEmission collectiveEmission, int productId, decimal standardComission)
        {
            IssuanceAgency agency = DelegateService.underwritingService.GetAgencyByAgentIdAgentAgencyId(collectiveEmission.Agency.Agent.IndividualId, collectiveEmission.Agency.Id);
            ProductAgencyCommiss productAgencyCommiss = DelegateService.productService.GetCommissByAgentIdAgencyIdProductId(agency.Agent.IndividualId, agency.Id, productId);
            agency.IsPrincipal = true;
            agency.Participation = 100;
            agency.Commissions = new List<IssuanceCommission>();

            IssuanceCommission issuanceCommission = new IssuanceCommission();

            if (agency.Agent.IndividualId == agency.Code)
            {
                issuanceCommission.Percentage = 0;
                issuanceCommission.PercentageAdditional = 0;
            }
            else
            {
                if (standardComission > 0)
                {
                    issuanceCommission.Percentage = standardComission;
                }
                else
                {
                    issuanceCommission.Percentage = productAgencyCommiss.CommissPercentage;
                }
                issuanceCommission.PercentageAdditional = productAgencyCommiss.AdditionalCommissionPercentage.GetValueOrDefault();
            }
            agency.Commissions.Add(issuanceCommission);

            return agency;
        }

        private UPDTOs.InsuredDTO CreateInsuredByCompany(UPDTOs.CompanyDTO company, Row row)
        {
            UPDTOs.InsuredDTO insured = new UPDTOs.InsuredDTO
            {
                IndividualId = company.Id,
                EnteredDate = DateTime.Now,
                InsProfileId = 2,
                BranchId = 1,
                ModifyDate = DateTime.Now,

                // Validar propiedades del modelo
                //IsCommercialClient = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderCommercialAgreement)),
                //IsMailAddress = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderSendingMail)),
                //IsSms = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderSMSSending))
            };

            insured = DelegateService.uniquePersonAplicationService.CreateAplicationInsured(insured);
            return insured;
        }

        private Holder CreateHolderByInsuredCompany(UPDTOs.InsuredDTO insured, UPDTOs.CompanyDTO company, UPDTOs.IndividualPaymentMethodDTO paymentMethodDTO)
        {
            Holder holder = new Holder
            {
                IndividualId = company.Id,
                InsuredId = insured.Id,
                //Validar propiedad del modelo
                IdentificationDocument = new IssuanceIdentificationDocument
                {
                    Number = company.Document,
                    DocumentType = new IssuanceDocumentType
                    {
                        Id = company.DocumentTypeId
                    }
                },
                PaymentMethod = new IssuancePaymentMethod
                {
                    Id = paymentMethodDTO.Id,
                    PaymentId = paymentMethodDTO.Id
                },
                CompanyName = new IssuanceCompanyName()
                {
                    Address = new IssuanceAddress
                    {
                        Id = company.Addresses.FirstOrDefault().Id,
                        Description = company.Addresses.FirstOrDefault().Description,
                        City = new City
                        {
                            Id = company.Addresses.FirstOrDefault().CityId,
                            State = new State
                            {
                                Id = company.Addresses.FirstOrDefault().Id,
                                Country = new Country
                                {
                                    Id = company.Addresses.FirstOrDefault().CountryId
                                }
                            }
                        }
                    },
                    Phone = new IssuancePhone
                    {
                        Id = company.Phones.FirstOrDefault().Id,
                        Description = company.Phones.FirstOrDefault().Description
                    },
                    Email = new IssuanceEmail
                    {
                        Id = company.Emails.FirstOrDefault().Id,
                        Description = company.Emails.FirstOrDefault().Description
                    }
                }
            };
            return holder;
        }

        private Holder CreateHolderCompany(Row row)
        {
            UPDTOs.CompanyDTO company = new UPDTOs.CompanyDTO
            {
                BusinessName = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderCompanyTradeName)),
                EconomicActivityId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderEconomicActivity)),
                DocumentTypeId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderCompanyDocumentType)),
                Document = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderCompanyDocumentNumber)),
                CompanyTypeId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderCompanyType)),
                CountryOriginId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressCountry)),
                Addresses = new List<UPDTOs.AddressDTO>(),
                Phones = new List<UPDTOs.PhoneDTO>(),
                Emails = new List<UPDTOs.EmailDTO>()
            };

            company.Addresses.Add(new UPDTOs.AddressDTO()
            {
                AddressTypeId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressType)),
                Description = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressDescription)),
                CityId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressCity)),
                StateId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressState)),
                CountryId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressCountry)),
            });

            company.Phones.Add(new UPDTOs.PhoneDTO()
            {
                PhoneTypeId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPhoneType)),
                Description = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPhoneDescription))
            });

            ///Inicio cargue masivo Company
            company.Phones.Add(new UPDTOs.PhoneDTO()
            {
                PhoneTypeId = 4,
                Description = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPhoneDescription))
            });
            ///Fin cargue masivo Company

            company.Emails.Add(new UPDTOs.EmailDTO()
            {
                EmailTypeId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderEmailType)),
                Description = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderEmailDescription))
            });

            company = DelegateService.uniquePersonAplicationService.CreateAplicationCompany(company);

            var individualPaymentMethodDTO = new UPDTOs.IndividualPaymentMethodDTO
            {

            };

            List<UPDTOs.IndividualPaymentMethodDTO> individualPaymentMethods = new List<UPDTOs.IndividualPaymentMethodDTO>();
            individualPaymentMethods.Add(
                new UPDTOs.IndividualPaymentMethodDTO
                {
                    Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPaymentPlan))
                }
                );
            var paymentMethodAccount = DelegateService.uniquePersonAplicationService.CreateIndividualpaymentMethods(individualPaymentMethods, company.Id);



            var insured = CreateInsuredByCompany(company, row);

            return CreateHolderByInsuredCompany(insured, company, paymentMethodAccount.FirstOrDefault());
        }
        /*
        private void UpdateInsuredByFile(Holder holder, Row row)
        {
            var insured = DelegateService.uniquePersonService.GetInsuredByIndividualId(holder.IndividualId);

            if (holder.IndividualType == IndividualType.Company)
                UpdateCompanyByFile(insured, row);
            else if (holder.IndividualType == IndividualType.Person)
                UpdatePersonByFile(insured, row);
        }

        private void UpdatePersonByFile(Insured insured, Row row)
        {

            UPMO.Person currentPerson = DelegateService.uniquePersonService.GetPersonByDocumentNumber(insured.IdentificationDocument.Number);
            bool personChanged = false;

            string names = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPersonName));
            if (!string.IsNullOrEmpty(names)
                && currentPerson.Names.Trim().ToLower() != names.Trim().ToLower())
            {
                currentPerson.Names = names;
                personChanged = true;
            }

            string surname = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPersonSurname));
            if (!string.IsNullOrEmpty(surname)
                && currentPerson.Surname.Trim().ToLower() != surname.Trim().ToLower())
            {
                currentPerson.Surname = surname;
                personChanged = true;
            }

            string motherLastName = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPersonSecondSurname));
            if (!string.IsNullOrEmpty(motherLastName)
                && currentPerson.MotherLastName.Trim().ToLower() != motherLastName.Trim().ToLower())
            {
                currentPerson.MotherLastName = motherLastName;
                personChanged = true;
            }

            int economicActivityId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderEconomicActivity));
            if (economicActivityId > 0
                && currentPerson.EconomicActivity != null && currentPerson.EconomicActivity.Id != economicActivityId)
            {
                currentPerson.EconomicActivity = new UPMO.EconomicActivity()
                {
                    Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderEconomicActivity))
                };
                personChanged = true;
            }

            string gender = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPersonGender));
            if (!string.IsNullOrEmpty(gender)
                && currentPerson.Gender != gender)
            {
                currentPerson.Gender = gender;
                personChanged = true;
            }

            int maritalStatusId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPersonMaritalStatus));
            if (maritalStatusId > 0
                && currentPerson.MaritalStatus != null && currentPerson.MaritalStatus.Id != maritalStatusId)
            {
                currentPerson.MaritalStatus = new MaritalStatus
                {
                    Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPersonMaritalStatus))
                };
                personChanged = true;
            }

            int paymentMethodId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPaymentPlan));
            if (paymentMethodId > 0
                && currentPerson.PaymentMthodAccount != null
                && !currentPerson.PaymentMthodAccount.Any(p => p.PaymentMethod.Id == paymentMethodId))
            {
                currentPerson.PaymentMthodAccount.Add(new PaymentMethodAccount
                {
                    Id = currentPerson.PaymentMthodAccount.Any(p => p.Id == 1) ? currentPerson.PaymentMthodAccount.Count() + 1 : 1,
                    PaymentMethod = new PaymentMethod
                    {
                        Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPaymentPlan))
                    }
                });
                personChanged = true;
            }

            int addressTypeId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressType));
            string addressDescription = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressDescription));
            int addresssCityId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressCity));
            int addresssStateId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressState));
            int addresssCountryId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressCountry));
            if (addressTypeId > 0
                && !string.IsNullOrEmpty(addressDescription)
                && addresssCityId > 0
                && addresssStateId > 0
                && addresssCountryId > 0
                && !currentPerson.Addresses.Any(a => a.Description.Trim().ToLower() == addressDescription.Trim().ToLower()))
            {
                currentPerson.Addresses.Add(new Address()
                {
                    AddressType = new AddressType()
                    {
                        Id = addressTypeId
                    },
                    Description = addressDescription,
                    City = new City()
                    {
                        Id = addresssCityId,
                        State = new State()
                        {
                            Id = addresssStateId,
                            Country = new Country
                            {
                                Id = addresssCountryId
                            }
                        }
                    }
                });
                personChanged = true;
            }

            int phoneTypeId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPhoneType));
            string phoneDescription = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPhoneDescription));
            if (phoneTypeId > 0 && !string.IsNullOrEmpty(phoneDescription)
                && !currentPerson.Phones.Any(p => p.Description.Trim() == phoneDescription.Trim()))
            {
                currentPerson.Phones.Add(new Phone()
                {
                    PhoneType = new PhoneType()
                    {
                        Id = phoneTypeId
                    },
                    Description = phoneDescription
                });
                personChanged = true;
            }

            int emailType = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderEmailType));
            string emailDescription = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderEmailDescription));

            if (emailType > 0 && !string.IsNullOrEmpty(emailDescription)
                && !currentPerson.Emails.Any(e => e.Description.Trim().ToLower() == emailDescription.Trim().ToLower()))
            {
                currentPerson.Emails.Add(new Email()
                {
                    EmailType = new EmailType()
                    {
                        Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderEmailType))
                    },
                    Description = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderEmailDescription))
                });
                personChanged = true;
            }

            if (personChanged)
                DelegateService.uniquePersonService.UpdatePerson(currentPerson, null);
        }

        private void UpdateCompanyByFile(Insured insured, Row row)
        {
            UniquePersonServices.Models.Company currentCompany = DelegateService.uniquePersonService.GetCompanyByDocumentNumber(insured.IdentificationDocument.Number);
            bool companyChanged = false;

            string name = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderCompanyTradeName));
            if (!string.IsNullOrEmpty(name)
                && currentCompany.Name.Trim().ToLower() != name.Trim().ToLower())
            {
                currentCompany.Name = name;
                companyChanged = true;
            }

            int economicActivityId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderEconomicActivity));
            if (economicActivityId > 0
                && currentCompany.EconomicActivity != null && currentCompany.EconomicActivity.Id != economicActivityId)
            {
                currentCompany.EconomicActivity = new UPMO.EconomicActivity()
                {
                    Id = economicActivityId
                };
                companyChanged = true;
            }

            int companyTypeId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderCompanyType));

            if (companyTypeId > 0
                && currentCompany.CompanyType != null && currentCompany.CompanyType.Id != companyTypeId)
            {
                currentCompany.CompanyType = new CompanyType()
                {
                    Id = companyTypeId
                };
                companyChanged = true;
            }

            int companyCountryOriginId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressCountry));
            if (companyCountryOriginId > 0
                && currentCompany.CountryOrigin != null && currentCompany.CountryOrigin.Id != companyCountryOriginId)
            {
                currentCompany.CountryOrigin = new Country
                {
                    Id = companyCountryOriginId
                };
                companyChanged = true;
            }

            int companyPaymentMethodId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPaymentPlan));
            if (companyPaymentMethodId > 0
                && currentCompany.PaymentMethodAccount != null
                && !currentCompany.PaymentMethodAccount.Any(p => p.PaymentMethod.Id == companyPaymentMethodId))
            {
                currentCompany.PaymentMethodAccount.Add(new PaymentMethodAccount
                {
                    Id = currentCompany.PaymentMethodAccount.Any(p => p.PaymentMethod.Id == 1) ? currentCompany.PaymentMethodAccount.Count() + 1 : 1,
                    PaymentMethod = new PaymentMethod
                    {
                        Id = companyPaymentMethodId
                    }
                });
                companyChanged = true;
            }

            int addressTypeId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressType));
            string addressDescription = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressDescription));
            int addresssCityId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressCity));
            int addresssStateId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressState));
            int addresssCountryId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressCountry));
            if (addressTypeId > 0
                && !string.IsNullOrEmpty(addressDescription)
                && addresssCityId > 0
                && addresssStateId > 0
                && addresssCountryId > 0
                && !currentCompany.Addresses.Any(a => a.Description.Trim().ToLower() == addressDescription.Trim().ToLower()))
            {
                currentCompany.Addresses.Add(new Address()
                {
                    AddressType = new AddressType()
                    {
                        Id = addressTypeId
                    },
                    Description = addressDescription,
                    City = new City()
                    {
                        Id = addresssCityId,
                        State = new State()
                        {
                            Id = addresssStateId,
                            Country = new Country
                            {
                                Id = addresssCountryId
                            }
                        }
                    }
                });
                companyChanged = true;
            }

            int companyPhoneTypeId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPhoneType));
            string companyPhoneDescription = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPhoneDescription));
            if (companyPhoneTypeId > 0 && !string.IsNullOrEmpty(companyPhoneDescription)
                && !currentCompany.Phones.Any(p => p.Description.Trim() == companyPhoneDescription.Trim()))
            {
                currentCompany.Phones.Add(new Phone()
                {
                    PhoneType = new PhoneType()
                    {
                        Id = companyPhoneTypeId
                    },
                    Description = companyPhoneDescription
                });
            }

            string companyCellPhoneDescription = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPhoneDescription));
            if (!string.IsNullOrEmpty(companyCellPhoneDescription)
                && !currentCompany.Phones.Any(p => p.Description.Trim() == companyCellPhoneDescription.Trim()))
            {
                currentCompany.Phones.Add(new Phone()
                {
                    PhoneType = new PhoneType()
                    {
                        Id = 4
                    },
                    Description = companyCellPhoneDescription
                });
            }

            if (companyChanged)
                DelegateService.uniquePersonService.UpdateCompany(currentCompany);
        }

        private Holder CreateHolderPerson(Row row)
        {
            UPMO.Person person = new UPMO.Person
            {
                IndividualType = IndividualType.Person,
                Names = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPersonName)),
                Surname = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPersonSurname)),
                MotherLastName = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPersonSecondSurname)),
                EconomicActivity = new UPMO.EconomicActivity()
                {
                    Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderEconomicActivity))
                },
                IdentificationDocument = new IdentificationDocument()
                {
                    DocumentType = new DocumentType()
                    {
                        Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPersonDocumentType))
                    },
                    Number = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPersonDocumentNumber))
                },
                Gender = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPersonGender)),
                MaritalStatus = new MaritalStatus
                {
                    Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPersonMaritalStatus))
                },
                BirthDate = (DateTime)DelegateService.utilitiesService.GetValueByField<DateTime>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPersonBirthDate)),
                EducativeLevel = new EducativeLevel
                {
                    Id = 1
                },
                HouseType = new HouseType
                {
                    Id = 1
                },
                SocialLayer = new SocialLayer
                {
                    Id = 1
                },
                LaborPerson = new LaborPerson
                {
                    Id = 1,
                    Occupation = new Occupation()
                },
                PaymentMthodAccount = new List<PaymentMethodAccount>(),
                Addresses = new List<Address>(),
                Phones = new List<Phone>(),
                Emails = new List<Email>()
            };

            person.PaymentMthodAccount.Add(new PaymentMethodAccount
            {
                Id = 1,
                PaymentMethod = new PaymentMethod
                {
                    Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPaymentPlan))
                }
            });

            person.Addresses.Add(new Address()
            {
                AddressType = new AddressType()
                {
                    Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressType))
                },
                Description = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressDescription)),
                City = new City()
                {
                    Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressCity)),
                    State = new State()
                    {
                        Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressState)),
                        Country = new Country
                        {
                            Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressCountry))
                        }
                    }
                }
            });

            person.Phones.Add(new Phone()
            {
                PhoneType = new PhoneType()
                {
                    Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPhoneType))
                },
                Description = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPhoneDescription))
            });

            ///Inicio cargue masivo Company
            person.Phones.Add(new Phone()
            {
                PhoneType = new PhoneType()
                {
                    Id = 4
                },
                Description = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPhoneDescription))
            });
            ///Fin cargue masivo Company

            person.Emails.Add(new Email()
            {
                EmailType = new EmailType()
                {
                    Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderEmailType))
                },
                Description = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderEmailDescription))
            });

            person = DelegateService.uniquePersonService.CreatePerson(person, null);

            //CREAR ASEGURADO
            CompanyInsured insured = CreateInsuredByPerson(person, row);

            return CreateHolderByInsuredPerson(insured, person);
        }

        private CompanyInsured CreateInsuredByPerson(UPMO.Person person, Row row)
        {
            CompanyInsured insured = new CompanyInsured
            {
                IndividualId = person.IndividualId,
                IdentificationDocument = person.IdentificationDocument,
                Name = person.Names + " " + person.Surname + " " + person.MotherLastName,
                EnteredDate = DateTime.Now,
                Profile = 2,
                BranchCode = 1,
                //Validar modelo
                //IsCommercialClient = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderCommercialAgreement)),
                //IsMailAddress = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderSendingMail)),
                //IsSms = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderSMSSending))
            };

            insured = DelegateService.uniquePersonService.CreateCompanyInsured(insured);
            return insured;
        }

        private Holder CreateHolderByInsuredPerson(CompanyInsured insured, UPMO.Person person)
        {
            Holder holder = new Holder
            {
                IndividualId = person.IndividualId,
                InsuredId = insured.InsuredId,
                //Validar propiedad del modelo
                IdentificationDocument = new IssuanceIdentificationDocument()
                {
                    DocumentType = new IssuanceDocumentType()
                    {
                        Id = insured.IdentificationDocument.DocumentType.Id
                    },
                    Number = insured.IdentificationDocument.Number
                },
                PaymentMethod = new IssuancePaymentMethod
                {
                    Id = person.PaymentMthodAccount[0].PaymentMethod.Id,
                    PaymentId = person.PaymentMthodAccount[0].Id
                },
                CompanyName = new IssuanceCompanyName()
                {
                    Address = new IssuanceAddress
                    {
                        Id = insured.CompanyName.Address.Id,
                        Description = insured.CompanyName.Address.Description,
                        City = insured.CompanyName.Address.City
                    },
                    Phone = new IssuancePhone
                    {
                        Id = insured.CompanyName.Phone.Id,
                        Description = insured.CompanyName.Phone.Description
                    },
                    Email = new IssuanceEmail
                    {
                        Id = insured.CompanyName.Email.Id,
                        Description = insured.CompanyName.Email.Description
                    }
                }
            };

            return holder;
        }

        */

        private List<IssuanceAgency> CreateAdditionalAgencies(Template template)
        {
            List<IssuanceAgency> agencies = new List<IssuanceAgency>();
            string error = String.Empty;
            try
            {
                if (template != null)
                {
                    templateName = template.Description;

                    foreach (Row row in template.Rows)
                    {
                        int agentCode = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentCode));
                        int agentTypeId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentType));

                        IssuanceAgency agency = DelegateService.underwritingService.GetAgencyByAgentCodeAgentTypeCode(agentCode, agentTypeId);
                        if (agency == null)
                        {
                            error = Errors.ErrorIntermediaryNotExist + agentCode;
                        }
                        agency.Participation = (decimal)DelegateService.utilitiesService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentParticipation));
                        agencies.Add(agency);
                    }
                }
            }
            catch (Exception)
            {

                throw new ValidationException(error);
            }



            return agencies;
        }


        public IssuedCollectiveLoad CreateIssuedPolicy(List<int> collectiveEmissionIds, int tempId)
        {
            IssuedCollectiveLoad issuedCollectiveLoad = new IssuedCollectiveLoad();
            issuedCollectiveLoad.CollectiveEmissions = new List<CollectiveEmission>();
            issuedCollectiveLoad.CollectiveEmissionRows = new List<CollectiveEmissionRow>();
            int errors = 0;

            PendingOperation pendingOperation = DelegateService.utilitiesService.GetPendingOperationById(tempId);
            CompanyPolicy companyPolicy = JsonConvert.DeserializeObject<CompanyPolicy>(pendingOperation.Operation);
            companyPolicy.IsPersisted = true;

            CollectiveEmissionRowDAO collectiveEmissionRowDAO = new CollectiveEmissionRowDAO();
            CollectiveEmissionDAO collectiveEmissionDAO = new CollectiveEmissionDAO();

            List<CompanyRisk> risks = new List<CompanyRisk>();

            foreach (int collectiveEmissionId in collectiveEmissionIds)
            {
                CollectiveEmission collectiveEmission = collectiveEmissionDAO.GetCollectiveEmissionByMassiveLoadIdWithRowsErrors(collectiveEmissionId, false, false, false);

                if (collectiveEmission != null)
                {
                    issuedCollectiveLoad.CollectiveEmissions.Add(collectiveEmission);
                    List<CollectiveEmissionRow> collectiveEmissionRows = collectiveEmissionRowDAO.GetCollectiveEmissionRowByMassiveLoadId(collectiveEmission.Id, CollectiveLoadProcessStatus.Tariff);

                    if (collectiveEmissionRows.Count > 0)
                    {
                        issuedCollectiveLoad.CollectiveEmissionRows.AddRange(collectiveEmissionRows);
                        collectiveEmission.Status = Core.Application.MassiveServices.Enums.MassiveLoadStatus.Issuing;
                        collectiveEmission.TotalRows = collectiveEmissionRows.Count;
                        collectiveEmissionDAO.UpdateCollectiveEmission(collectiveEmission);

                        foreach (CollectiveEmissionRow collectiveEmissionRow in collectiveEmissionRows)
                        {
                            try
                            {

                                pendingOperation = DelegateService.utilitiesService.GetPendingOperationById((int)collectiveEmissionRow.Risk.RiskId);
                                CompanyTplRisk companyTplRisk = new CompanyTplRisk();
                                companyTplRisk = JsonConvert.DeserializeObject<CompanyTplRisk>(pendingOperation.Operation);
                                companyTplRisk.Risk.Id = pendingOperation.Id;
                                companyTplRisk.Risk.IsPersisted = true;
                                risks.Add(companyTplRisk.Risk);
                                collectiveEmissionRow.Risk.Description = companyTplRisk.LicensePlate;
                                collectiveEmissionRow.Risk.RiskId = companyTplRisk.Risk.Id;
                                collectiveEmissionRow.Status = CollectiveLoadProcessStatus.Finalized;
                                collectiveEmissionRow.Premium = companyTplRisk.Risk.Premium;
                                collectiveEmissionRowDAO.UpdateCollectiveEmissionRow(collectiveEmissionRow);
                            }
                            catch (Exception ex)
                            {
                                collectiveEmissionRow.Status = CollectiveLoadProcessStatus.Tariff;
                                collectiveEmissionRow.HasError = true;
                                collectiveEmissionRow.Observations = ex.Message;
                                collectiveEmissionRowDAO.UpdateCollectiveEmissionRow(collectiveEmissionRow);
                                errors++;
                            }
                            finally
                            {
                                DataFacadeManager.Dispose();
                            }
                        }
                    }
                }
            }
            companyPolicy.PayerComponents = DelegateService.underwritingService.CalculatePayerComponentsByCompanyPolicy(companyPolicy, risks);
            companyPolicy.Summary = DelegateService.underwritingService.CalculateSummaryByCompanyPolicy(companyPolicy, risks);
            companyPolicy.PaymentPlan.Quotas = DelegateService.underwritingService.CalculateQuotas(new QuotaFilterDTO { PlanId = companyPolicy.PaymentPlan.Id, CurrentFrom = companyPolicy.CurrentFrom, IssueDate = companyPolicy.IssueDate, ComponentValueDTO = ModelAssembler.CreateCompanyComponentValueDTO(companyPolicy.Summary) });
            companyPolicy.Agencies = DelegateService.underwritingService.CalculateCommissionsByCompanyPolicy(companyPolicy, risks);
            companyPolicy.Summary.Risks = risks;
            companyPolicy = DelegateService.underwritingService.CreatePolicyTemporal(companyPolicy, true);

            pendingOperation = DelegateService.utilitiesService.GetPendingOperationById(tempId);
            pendingOperation.Operation = JsonConvert.SerializeObject(companyPolicy);
            DelegateService.utilitiesService.UpdatePendingOperation(pendingOperation);

            issuedCollectiveLoad.AmmountInsured = companyPolicy.Summary.AmountInsured;
            issuedCollectiveLoad.Premium = companyPolicy.Summary.FullPremium;
            issuedCollectiveLoad.Errors = errors;

            return issuedCollectiveLoad;
        }

        public IssuedCollectiveLoad CreateMassiveIssuedPolicy(int massiveLoadId, int tempId)
        {
            CollectiveEmissionRowDAO collectiveEmissionRowDAO = new CollectiveEmissionRowDAO();
            CollectiveEmissionDAO collectiveEmissionDAO = new CollectiveEmissionDAO();
            CollectiveEmission collectiveEmission = collectiveEmissionDAO.GetCollectiveEmissionByMassiveLoadIdWithRowsErrors(massiveLoadId, false, false, false);

            if (collectiveEmission != null)
            {
                IssuedCollectiveLoad issuedCollectiveLoad = new IssuedCollectiveLoad();
                issuedCollectiveLoad.CollectiveEmissions = new List<CollectiveEmission>();
                issuedCollectiveLoad.CollectiveEmissionRows = new List<CollectiveEmissionRow>();
                int errors = 0;

                PendingOperation pendingOperation = DelegateService.utilitiesService.GetPendingOperationById(collectiveEmission.TemporalId);
                CompanyPolicy companyPolicy = JsonConvert.DeserializeObject<CompanyPolicy>(pendingOperation.Operation);
                companyPolicy.IsPersisted = true;

                List<CompanyRisk> risks = new List<CompanyRisk>();

                issuedCollectiveLoad.CollectiveEmissions.Add(collectiveEmission);

                List<CollectiveEmissionRow> collectiveEmissionRows = collectiveEmissionRowDAO.GetCollectiveEmissionRowByMassiveLoadId(collectiveEmission.Id, CollectiveLoadProcessStatus.Tariff);

                if (collectiveEmissionRows.Count > 0)
                {
                    issuedCollectiveLoad.CollectiveEmissionRows.AddRange(collectiveEmissionRows);

                    collectiveEmission.Status = MassiveLoadStatus.Issuing;
                    collectiveEmission.TotalRows = collectiveEmissionRows.Count;
                    collectiveEmissionDAO.UpdateCollectiveEmission(collectiveEmission);



                    foreach (CollectiveEmissionRow collectiveEmissionRow in collectiveEmissionRows)
                    {
                        try
                        {

                            pendingOperation = DelegateService.utilitiesService.GetPendingOperationById((int)collectiveEmissionRow.Risk.RiskId);

                            CompanyTplRisk companyTplRisk = new CompanyTplRisk();
                            companyTplRisk = JsonConvert.DeserializeObject<CompanyTplRisk>(pendingOperation.Operation);
                            companyTplRisk.Risk.Id = pendingOperation.Id;
                            companyTplRisk.Risk.IsPersisted = true;
                            risks.Add(companyTplRisk.Risk);
                            collectiveEmissionRow.Risk.Description = companyTplRisk.LicensePlate;
                            collectiveEmissionRow.Risk.RiskId = companyTplRisk.Risk.Id;
                            collectiveEmissionRow.Status = CollectiveLoadProcessStatus.Finalized;
                            collectiveEmissionRow.Premium = companyTplRisk.Risk.Premium;
                            collectiveEmissionRowDAO.UpdateCollectiveEmissionRow(collectiveEmissionRow);
                        }
                        catch (Exception ex)
                        {
                            collectiveEmissionRow.Status = CollectiveLoadProcessStatus.Tariff;
                            collectiveEmissionRow.HasError = true;
                            collectiveEmissionRow.Observations = ex.Message;
                            collectiveEmissionRowDAO.UpdateCollectiveEmissionRow(collectiveEmissionRow);
                            errors++;
                        }
                        finally
                        {
                            DataFacadeManager.Dispose();
                        }
                    }
                }


                if (collectiveEmission.IsAutomatic != true)
                {
                    companyPolicy.PayerComponents = DelegateService.underwritingService.CalculatePayerComponentsByCompanyPolicy(companyPolicy, risks);
                    companyPolicy.Summary = DelegateService.underwritingService.CalculateSummaryByCompanyPolicy(companyPolicy, risks);
                    companyPolicy.PaymentPlan.Quotas = DelegateService.underwritingService.CalculateQuotas(new QuotaFilterDTO { PlanId = companyPolicy.PaymentPlan.Id, CurrentFrom = companyPolicy.CurrentFrom, IssueDate = companyPolicy.IssueDate,  ComponentValueDTO= ModelAssembler.CreateCompanyComponentValueDTO(companyPolicy.Summary) });
                    companyPolicy.Agencies = DelegateService.underwritingService.CalculateCommissionsByCompanyPolicy(companyPolicy, risks);
                }
                companyPolicy.Summary.Risks = risks;
                companyPolicy = DelegateService.underwritingService.CreatePolicyTemporal(companyPolicy, true);

                pendingOperation = DelegateService.utilitiesService.GetPendingOperationById(tempId);
                pendingOperation.Operation = JsonConvert.SerializeObject(companyPolicy);
                DelegateService.utilitiesService.UpdatePendingOperation(pendingOperation);
                companyPolicy = DelegateService.underwritingService.CreatePolicyTemporal(companyPolicy, true);

                issuedCollectiveLoad.AmmountInsured = companyPolicy.Summary.AmountInsured;
                issuedCollectiveLoad.Premium = companyPolicy.Summary.FullPremium;
                issuedCollectiveLoad.Errors = errors;

                return issuedCollectiveLoad;
            }
            else
            {
                return null;
            }
        }

        public bool ExcludeCompanyCollectiveEmissionRowsTemporals(int massiveLoadId, List<int> temps, string userName, bool deleteTemporal)
        {
            CollectiveEmission collectiveEmission = DelegateService.collectiveService.ExcludeCollectiveEmissionRowsTemporals(massiveLoadId, temps, userName, deleteTemporal);

            if (collectiveEmission != null)
            {
                PendingOperation pendingOperation = DelegateService.utilitiesService.GetPendingOperationById(collectiveEmission.TemporalId);
                CompanyPolicy companyPolicy = JsonConvert.DeserializeObject<CompanyPolicy>(pendingOperation.Operation);

                List<CompanyRisk> risks = new List<CompanyRisk>();
                foreach (CollectiveEmissionRow collectiveEmissionRow in collectiveEmission.Rows)
                {
                    pendingOperation = DelegateService.utilitiesService.GetPendingOperationById((int)collectiveEmissionRow.Risk.RiskId);
                    CompanyTplRisk risk = new CompanyTplRisk();
                    risk = JsonConvert.DeserializeObject<CompanyTplRisk>(pendingOperation.Operation);
                    risks.Add(risk.Risk);
                }

                //VALIDAR CAMBIO DE METODOS
                companyPolicy.PayerComponents = DelegateService.underwritingService.CalculatePayerComponentsByCompanyPolicy(companyPolicy, risks);
                companyPolicy.Summary = DelegateService.underwritingService.CalculateSummaryByCompanyPolicy(companyPolicy, risks);                
                companyPolicy.PaymentPlan.Quotas = DelegateService.underwritingService.CalculateQuotas(new QuotaFilterDTO { PlanId = companyPolicy.PaymentPlan.Id, CurrentFrom = companyPolicy.CurrentFrom, IssueDate = companyPolicy.IssueDate, ComponentValueDTO= ModelAssembler.CreateCompanyComponentValueDTO(companyPolicy.Summary) });
                companyPolicy.Agencies = DelegateService.underwritingService.CalculateCommissionsByCompanyPolicy(companyPolicy, risks);
                companyPolicy.Summary.Risks = risks;

                pendingOperation = DelegateService.utilitiesService.GetPendingOperationById(collectiveEmission.TemporalId);
                pendingOperation.Operation = JsonConvert.SerializeObject(companyPolicy);
                DelegateService.utilitiesService.UpdatePendingOperation(pendingOperation);
            }
            else
            {
                return false;
            }
            return true;
        }

        public MassiveLoad IssuanceCollectiveEmission(int massiveLoadId)
        {
            MassiveLoad massiveLoad = DelegateService.massiveService.GetMassiveLoadByMassiveLoadId(massiveLoadId);
            CollectiveEmissionDAO collectiveEmissionDAO = new CollectiveEmissionDAO();

            if (massiveLoad != null)
            {
                CollectiveEmission collectiveEmission = collectiveEmissionDAO.GetCollectiveEmissionByMassiveLoadIdWithRowsErrors(massiveLoadId, false, false, false);
                if (!(bool)collectiveEmission.HasEvents && massiveLoad.TotalRows > 0)
                {

                    massiveLoad.Status = MassiveLoadStatus.Issuing;
                    DelegateService.massiveService.UpdateMassiveLoad(massiveLoad);
                    TP.Task.Run(() => ExecuteIssuanceCollectiveEmission(collectiveEmission, massiveLoad));
                }
                else
                {
                    massiveLoad.HasError = (bool)collectiveEmission.HasEvents;
                    massiveLoad.ErrorDescription = "El temporal tiene eventos";
                }
            }
            return massiveLoad;
        }

        private void ExecuteIssuanceCollectiveEmission(CollectiveEmission collectiveEmission, MassiveLoad massiveLoad)
        {
            CollectiveEmissionDAO collectiveEmissionDAO = new CollectiveEmissionDAO();

            try
            {
                collectiveEmission.Description = massiveLoad.Description;
                collectiveEmission.ErrorDescription = massiveLoad.ErrorDescription;
                collectiveEmission.File = massiveLoad.File;
                collectiveEmission.LoadType = massiveLoad.LoadType;
                collectiveEmission.HasError = massiveLoad.HasError;
                collectiveEmission.User = massiveLoad.User;
                collectiveEmission.TotalRows = massiveLoad.TotalRows;
                collectiveEmission.Status = MassiveLoadStatus.Issued;
                collectiveEmissionDAO.UpdateCollectiveEmission(collectiveEmission);

            }
            catch (Exception ex)
            {
                collectiveEmission.ErrorDescription = ex.Message;
                collectiveEmission.HasError = true;
                collectiveEmission.Status = MassiveLoadStatus.Issued;
                collectiveEmissionDAO.UpdateCollectiveEmission(collectiveEmission);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }

        public MassiveLoad IssuanceCollectiveEndorsement(int massiveLoadId)
        {
            MassiveLoad massiveLoad = DelegateService.massiveService.GetMassiveLoadByMassiveLoadId(massiveLoadId);
            CollectiveEmissionDAO collectiveEmissionDAO = new CollectiveEmissionDAO();

            if (massiveLoad != null)
            {
                CollectiveEmission collectiveEmission = collectiveEmissionDAO.GetCollectiveEmissionByMassiveLoadIdWithRowsErrors(massiveLoadId, false, false, false);
                if (!(bool)collectiveEmission.HasEvents && massiveLoad.TotalRows > 0)
                {

                    massiveLoad.Status = MassiveLoadStatus.Issuing;
                    DelegateService.massiveService.UpdateMassiveLoad(massiveLoad);
                    TP.Task.Run(() => ExecuteIssuanceCollectiveEndorsement(collectiveEmission, massiveLoad));
                }
                else
                {
                    massiveLoad.HasError = (bool)collectiveEmission.HasEvents;
                    massiveLoad.ErrorDescription = "ErrorIssue";
                }
            }
            return massiveLoad;
        }

        private void ExecuteIssuanceCollectiveEndorsement(CollectiveEmission collectiveEmission, MassiveLoad massiveLoad)
        {

            CollectiveEmissionDAO collectiveEmissionDAO = new CollectiveEmissionDAO();

            try
            {
                Policy policy = DelegateService.endorsementService.CreateEndorsement(collectiveEmission.TemporalId);

                collectiveEmission.EndorsementNumber = policy.Endorsement.Number;

                collectiveEmission.Description = massiveLoad.Description;
                collectiveEmission.ErrorDescription = massiveLoad.ErrorDescription;
                collectiveEmission.File = massiveLoad.File;
                collectiveEmission.LoadType = massiveLoad.LoadType;
                collectiveEmission.HasError = massiveLoad.HasError;
                collectiveEmission.User = massiveLoad.User;
                collectiveEmission.TotalRows = massiveLoad.TotalRows;
                collectiveEmission.Status = MassiveLoadStatus.Issued;
                collectiveEmissionDAO.UpdateCollectiveEmission(collectiveEmission);
            }
            catch (Exception ex)
            {
                collectiveEmission.ErrorDescription = ex.Message;
                collectiveEmission.HasError = true;
                collectiveEmission.Status = MassiveLoadStatus.Issued;
                collectiveEmissionDAO.UpdateCollectiveEmission(collectiveEmission);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }
    }
}
