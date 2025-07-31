using Sistran.Company.Application.UniquePersonAplicationServices.DTOs;
using Sistran.Company.Application.UniquePersonServices.V1.Models;
using MOCOUP = Sistran.Core.Application.UniquePersonService.V1.Models;
using System.Collections.Generic;
using Sistran.Company.Application.ModelServices.Models.Param;
using Sistran.Company.Application.ModelServices.Enums;
using MOCOV1 = Sistran.Company.Application.UniquePersonServices.V1.Models;
using System;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.UniquePersonService.V1.Enums;
using Sistran.Company.Application.UniquePersonAplicationServices.EEProvider.Resources;
using System.Linq;

namespace Sistran.Company.Application.UniquePersonAplicationServices.EEProvider.Assemblers
{
    /// <summary>
    /// Convertir de DTOs a Modelos
    /// </summary>
    public class AplicationAssembler
    {
        #region person
        public static PersonDTO CreatePerson(CompanyPerson person)
        {
            PersonDTO result = new PersonDTO();
            result.Id = person.IndividualId;
            result.BirthDate = person.BirthDate;
            result.BirthPlace = person.BirthPlace;
            result.Document = person.IdentificationDocument.Number;
            result.DocumentTypeId = person.IdentificationDocument.DocumentType.Id;
            result.ExonerationTypeCode = person.Exoneration?.ExonerationType?.Id;
            result.EconomicActivityId = person.EconomicActivity.Id;
            result.EconomicActivityDescription = person.EconomicActivity.Description;
            result.Gender = person.Gender;
            result.MaritalStatusId = person.MaritalStatus.Id;
            result.Names = person.FullName ?? string.Empty;
            result.SecondSurname = person.SecondSurName ?? string.Empty;
            result.Surname = person.SurName ?? string.Empty;
            result.CheckPayable = person.CheckPayable;
            result.DataProtection = person.DataProtection;
            result.DocumentNumber = person.IdentificationDocument.Number;
            result.ElectronicBiller = person.Insured != null ? person.Insured.ElectronicBiller : true;
            return result;
        }
        public static List<PersonDTO> CreatePersons(List<CompanyPerson> persons)
        {
            var personsDTO = new List<PersonDTO>();
            foreach (CompanyPerson person in persons)
            {
                personsDTO.Add(CreatePerson(person));
            }
            return personsDTO;

        }

        #endregion

        #region company
        public static CompanyDTO CreateCompany(UniquePersonServices.V1.Models.CompanyCompany company)
        {
            var result = new CompanyDTO();
            result.ConsortiumMembers = company.Consortiums != null ? CreateConsortiums(company.Consortiums) : null;
            result.Id = company.IndividualId;
            result.DocumentTypeId = company.IdentificationDocument.DocumentType.Id;
            result.Document = company.IdentificationDocument.Number;
            result.BusinessName = company.FullName ?? string.Empty;
            result.AssociationTypeId = company.AssociationType.Id;
            result.CompanyTypeId = company.CompanyType.Id;
            result.CountryOriginId = company.CountryId;
            result.EconomicActivityId = company.EconomicActivity.Id;
            result.EconomicActivityDescription = company.EconomicActivity.Description;
            result.ExonerationTypeCode = company.Exoneration?.ExonerationType?.Id;
            result.VerifyDigit = company.VerifyDigit;
            result.CheckPayable = company.CheckPayable;
            result.NitAssociationType = company.IdentificationDocument.NitAssociationType;
            return result;
        }

        public static List<CompanyDTO> CreateCompanies(List<CompanyCompany> companys)
        {
            var companysDTO = new List<CompanyDTO>();
            foreach (UniquePersonServices.V1.Models.CompanyCompany company in companys)
            {
                companysDTO.Add(CreateCompany(company));
            }
            return companysDTO;

        }
        #endregion

        #region Address
        /// <summary>
        /// Crear una Persona
        /// </summary>
        /// <param name="Address">The person.</param>
        /// <returns></returns>
        public static AddressDTO CreateAdddress(CompanyAddress address)
        {
            var result = new AddressDTO()
            {
                Id = address.Id,
                AddressTypeId = address.AddressType.Id,
                CityDescription = address.City.Description,
                CityId = address.City.Id,
                StateDescription = address.City.State.Description,
                CountryId = address.City.State.Country.Id,
                CountryDescription = address.City.State.Country.Description,
                StateId = address.City.State.Id,
                Description = address.Description,
                IsPrincipal = address.IsPrincipal,
                AplicationStaus = CommonAplicationServices.Enums.AplicationStaus.Original
            };
            return result;
        }
        public static List<AddressDTO> CreateAdddresses(List<CompanyAddress> addresses)
        {
            var result = new List<AddressDTO>();
            foreach (var address in addresses)
            {
                result.Add(AplicationAssembler.CreateAdddress(address));
            }
            return result;
        }


        #endregion

        #region Email
        /// <summary>
        /// Crear una Persona
        /// </summary>
        /// <param name="Email">The person.</param>
        /// <returns></returns>
        public static EmailDTO CreateEmail(CompanyEmail email)
        {
            var result = new EmailDTO()
            {
                Id = email.Id,
                EmailTypeId = email.EmailType.Id,
                Description = email.Description,
                IsPrincipal = email.IsPrincipal,
                AplicationStaus = CommonAplicationServices.Enums.AplicationStaus.Original
            };
            return result;
        }

        public static List<EmailDTO> CreateEmails(List<CompanyEmail> Emails)
        {
            var result = new List<EmailDTO>();
            foreach (var Email in Emails)
            {
                result.Add(AplicationAssembler.CreateEmail(Email));
            }
            return result;
        }

        #endregion

        #region Phone
        /// <summary>
        /// Crear una Persona
        /// </summary>
        /// <param name="Email">The person.</param>
        /// <returns></returns>
        public static PhoneDTO CreatePhone(CompanyPhone phone)
        {
            var result = new PhoneDTO()
            {
                Id = phone.Id,
                PhoneTypeId = phone.PhoneType.Id,
                PhoneTypeDescription = phone.PhoneType.Description,
                Description = phone.Description,
                IsPrincipal = phone.IsMain,
                ScheduleAvailability = phone.ScheduleAvailability,
                CityCode = phone.CityCode.HasValue ? phone.CityCode.Value : 0,
                CountryCode = phone.CountryCode.HasValue ? phone.CountryCode.Value : 0,
                Extension = phone.Extension.HasValue ? phone.Extension.Value : 0

            };
            return result;
        }

        public static List<PhoneDTO> CreatePhones(List<CompanyPhone> Phones)
        {
            var result = new List<PhoneDTO>();
            foreach (var Phone in Phones)
            {
                result.Add(AplicationAssembler.CreatePhone(Phone));
            }
            return result;
        }
        #endregion

        #region Insured
        /// <summary>
        /// Crear un asegurado
        /// </summary>
        /// <param name="insured">The insured.</param>
        /// <returns></returns>
        public static InsuredDTO CreateInsured(UniquePersonServices.V1.Models.CompanyInsured insured)
        {
            return new InsuredDTO()
            {
                Id = insured.InsuredCode,
                IndividualId = insured.IndividualId,
                AgencyId = insured.Agency?.Id,
                AgentId = insured.Agency?.Agent?.IndividualId,
                IdDescription = insured.Agency?.FullName,
                EnteredDate = insured.EnteredDate,
                DeclinedDate = insured.DeclinedDate,
                ModifyDate = insured.ModifyDate,
                InsDeclinesTypeId = insured.DeclinedType?.Id,
                IsBeneficiary = insured.Concept?.IsBeneficiary,
                IsHolder = insured.Concept?.IsHolder,
                IsInsured = insured.Concept?.IsInsured,
                IsPayer = insured.Concept?.IsPayer,
                InsProfileId = insured.Profile.Id,
                BranchId = insured.Branch.Id,
                InsSegmentId = insured.Segment.Id,
                IsSms = insured.IsSMS,
                IsMailAddress = insured.IsMailAddress,
                Annotations = insured.Annotations,
                ElectronicBiller = insured.ElectronicBiller,
                RegimeType = insured.RegimeType
            };
        }


        public static InsuredDTO CreateInsuredElectronicBilling(UniquePersonServices.V1.Models.CompanyInsured insured)
        {
            return new InsuredDTO()
            {
                Id = insured.InsuredCode,
                IndividualId = insured.IndividualId,
                BranchId = insured.Branch.Id,
                InsProfileId = insured.Profile.Id,
                InsSegmentId = insured.Segment.Id,
                EnteredDate = insured.EnteredDate,
                DeclinedDate = insured.DeclinedDate,
                InsDeclinesTypeId = insured.DeclinedType?.Id,
                ModifyDate = insured.ModifyDate,
                IsSms = insured.IsSMS,
                IsMailAddress = insured.IsMailAddress,
                Annotations = insured.Annotations,
                ElectronicBiller = insured.ElectronicBiller,
                RegimeType = insured.RegimeType
            };
        }

        #endregion


        #region Supplier v1

        /// <summary>
        /// Crear un poveedor
        /// </summary>
        /// <param name="provider">The insured.</param>
        /// <returns></returns>
        public static ProviderDTO CreateSupplier(MOCOV1.CompanySupplier supplier)
        {
            var result = new ProviderDTO();

            if (supplier != null)
            {

                if (supplier.EnteredDate != null)
                {
                    result.CreationDate = (DateTime)supplier.EnteredDate;
                }
                result.DeclinationDate = supplier.DeclinedDate;
                result.Id = supplier.Id;
                result.IndividualId = supplier.IndividualId;
                result.ModificationDate = supplier.ModificationDate;
                result.Observation = supplier.Observation;

                if (supplier.DeclinedType != null)
                {
                    result.ProviderDeclinedTypeId = supplier.DeclinedType.Id;
                }

                if (supplier.Type != null)
                {
                    result.ProviderTypeId = supplier.Type.Id;
                }

                if (supplier.Profile != null)
                {
                    result.SupplierProfileId = supplier.Profile.Id;
                }
                if (supplier.GroupSupplier != null && supplier.GroupSupplier.Count > 0)
                {
                    result.GroupSupplier = supplier.GroupSupplier.Select(x => new GroupSupplierDTO { Id = x.Id }).ToList();
                }

            }


            return result;
        }

        public static SupplierDeclinedTypeDTO CreateSupplierDeclinedType(MOCOV1.CompanySupplierDeclinedType companySupplierType)
        {
            var result = new SupplierDeclinedTypeDTO();
            result.Id = companySupplierType.Id;
            result.Description = companySupplierType.Description;
            result.SmallDescription = companySupplierType.SmallDescription;
            return result;
        }

        public static GroupSupplierDTO CreateGroupSupplier(MOCOV1.CompanyGroupSupplier companyGroupSupplier)
        {
            var result = new GroupSupplierDTO();
            result.Id = companyGroupSupplier.Id;
            result.Description = companyGroupSupplier.Description;
            return result;
        }


        public static List<SupplierDeclinedTypeDTO> CreateSupplierDeclinedTypes(List<MOCOV1.CompanySupplierDeclinedType> companySupplierType)
        {
            var result = new List<SupplierDeclinedTypeDTO>();
            foreach (var supplierType in companySupplierType)
            {
                result.Add(AplicationAssembler.CreateSupplierDeclinedType(supplierType));
            }
            return result;
        }

        public static List<GroupSupplierDTO> CreateGroupsSupplier(List<MOCOV1.CompanyGroupSupplier> companyGroupSuppliers)
        {
            var result = new List<GroupSupplierDTO>();
            foreach (var groupSuppliers in companyGroupSuppliers)
            {
                result.Add(AplicationAssembler.CreateGroupSupplier(groupSuppliers));
            }
            return result;
        }

        public static SupplierTypeDTO CreateSupplierType(MOCOV1.CompanySupplierType companySupplierType)
        {
            var result = new SupplierTypeDTO();
            result.Id = companySupplierType.Id;
            result.Description = companySupplierType.Description;
            result.Enabled = (bool)companySupplierType.Enable;
            return result;
        }


        public static List<SupplierTypeDTO> CreateSupplierTypes(List<MOCOV1.CompanySupplierType> companySupplierType)
        {
            var result = new List<SupplierTypeDTO>();
            foreach (var supplierType in companySupplierType)
            {
                result.Add(AplicationAssembler.CreateSupplierType(supplierType));
            }
            return result;
        }

        public static SupplierProfileDTO CreateSupplierProfile(MOCOV1.CompanySupplierProfile companySupplierProfile)
        {
            var result = new SupplierProfileDTO();
            result.Id = companySupplierProfile.Id;
            result.Description = companySupplierProfile.Description;
            result.Enabled = companySupplierProfile.IsEnabled;
            return result;
        }


        public static List<SupplierProfileDTO> CreateSupplierProfiles(List<MOCOV1.CompanySupplierProfile> companySupplierProfile)
        {
            var result = new List<SupplierProfileDTO>();
            foreach (var supplierProfile in companySupplierProfile)
            {
                result.Add(AplicationAssembler.CreateSupplierProfile(supplierProfile));
            }
            return result;
        }

        public static AccountingConceptDTO CreateAccountingConcept(MOCOV1.CompanyAccountingConcept companyAccountingConcept)
        {
            var result = new AccountingConceptDTO();
            result.Id = companyAccountingConcept.Id;
            result.Description = companyAccountingConcept.Description;
            return result;
        }


        public static List<AccountingConceptDTO> CreateAccountingConcepts(List<MOCOV1.CompanyAccountingConcept> companyAccountingConcept)
        {
            var result = new List<AccountingConceptDTO>();
            foreach (var accountingConcept in companyAccountingConcept)
            {
                result.Add(AplicationAssembler.CreateAccountingConcept(accountingConcept));
            }
            return result;
        }


        public static SupplierAccountingConceptDTO CreateSupplierAccountingConcept(MOCOV1.CompanySupplierAccountingConcept companyAccountingConcept)
        {
            var result = new SupplierAccountingConceptDTO();
            result.Id = companyAccountingConcept.Id;

            if (companyAccountingConcept.Supplier != null)
            {
                result.SupplierId = companyAccountingConcept.Supplier.Id;
            }

            if (companyAccountingConcept.AccountingConcept != null)
            {
                result.AccountingConceptId = companyAccountingConcept.AccountingConcept.Id;
            }

            return result;
        }


        public static List<SupplierAccountingConceptDTO> CreateSupplierAccountingConcepts(List<MOCOV1.CompanySupplierAccountingConcept> companySupplierAccountingConcept)
        {
            var result = new List<SupplierAccountingConceptDTO>();
            foreach (var supplierAccountingConcept in companySupplierAccountingConcept)
            {
                result.Add(AplicationAssembler.CreateSupplierAccountingConcept(supplierAccountingConcept));
            }
            return result;
        }

        #endregion Supplier v1


        #region CoCompanyName v1

        public static CompanyNameDTO CreateCoCompanyName(MOCOV1.CompanyCoCompanyName CompanyName)
        {
            var result = new CompanyNameDTO();

            if (CompanyName != null)
            {
                result.IndividualId = CompanyName.IndividualId;
                result.NameNum = CompanyName.NameNum;
                result.TradeName = CompanyName.TradeName;
                result.IsMain = CompanyName.IsMain;

                if (CompanyName.Phone != null)
                {
                    result.PhoneID = CompanyName.Phone.Id;
                }

                if (CompanyName.Address != null)
                {
                    result.AddressID = CompanyName.Address.Id;
                }

                if (CompanyName.Email != null)
                {
                    result.EmailID = CompanyName.Email.Id;
                }
            }

            return result;
        }

        public static List<CompanyNameDTO> CreateCoCompanyNames(List<MOCOV1.CompanyCoCompanyName> companyCoCompanyName)
        {
            var result = new List<CompanyNameDTO>();
            foreach (var CoCompanyName in companyCoCompanyName)
            {
                result.Add(AplicationAssembler.CreateCoCompanyName(CoCompanyName));
            }
            return result;
        }

        #endregion CoCompanyName v1


        #region Provider
        /// <summary>
        /// Crear un poveedor
        /// </summary>
        /// <param name="provider">The insured.</param>
        /// <returns></returns>
        public static ProviderDTO CreateProvider(MOCOUP.Supplier provider)
        {
            var result = new ProviderDTO();

            //result.CreationDate = provider.CreationDate;
            //result.DeclinationDate = provider.DeclinationDate;
            //result.Id = provider.Id;
            //result.IndividualId = provider.IndividualId;
            //result.ModificationDate = provider.ModificationDate;
            //result.Observation = provider.Observation;
            ////result.OriginTypeId = provider.OriginTypeId;
            //result.ProviderDeclinedTypeId = provider.ProviderDeclinedTypeId;
            //result.ProviderPaymentConcepts = CreateProviderPaymentConcepts(provider.ProviderPaymentConcept);
            //result.ProviderSpecialities = CreateProviderSpecialities(provider.ProviderSpeciality);
            //  result.ProviderTypeId = provider.ProviderTypeId;
            return result;
        }

        /// <summary>
        /// Crear un ProviderPaymentConcept
        /// </summary>
        /// <param name="paymentConcept">The insured.</param>
        /// <returns></returns>
        //public static ProviderPaymentConceptDTO CreateProviderPaymentConcept(MOCOUP.ProviderPaymentConcept paymentConcept)
        //{
        //    var result = new ProviderPaymentConceptDTO();

        //    result.Id = paymentConcept.Id;
        //    result.PaymentConceptId = paymentConcept.PaymentConcept.Id;
        //    return result;
        //}

        /// <summary>
        /// Crear una lista de ProviderPaymentConcept
        /// </summary>
        /// <param name="paymentConcepts">The insured.</param>
        /// <returns></returns>
        //public static List<ProviderPaymentConceptDTO> CreateProviderPaymentConcepts(List<MOCOUP.ProviderPaymentConcept> paymentConcepts)
        //{
        //    var result = new List<ProviderPaymentConceptDTO>();
        //    foreach (MOCOUP.ProviderPaymentConcept providerPaymentConceptDTO in paymentConcepts)
        //    {
        //        result.Add(CreateProviderPaymentConcept(providerPaymentConceptDTO));
        //    }
        //    return result;
        //}

        /// <summary>
        /// Crear un poveedor
        /// </summary>
        /// <param name="providerSpeciality">The insured.</param>
        /// <returns></returns>
        public static ProviderSpecialityDTO CreateProviderSpeciality(MOCOUP.ProviderSpeciality providerSpeciality)
        {
            var result = new ProviderSpecialityDTO();

            result.Id = providerSpeciality.Id;
            result.SpecialityId = providerSpeciality.SpecialityId;
            return result;
        }

        /// <summary>
        /// Crear una lista de ProviderPaymentConcept
        /// </summary>
        /// <param name="providerSpecialities">The insured.</param>
        /// <returns></returns>
        public static List<ProviderSpecialityDTO> CreateProviderSpecialities(List<MOCOUP.ProviderSpeciality> providerSpecialities)
        {
            var result = new List<ProviderSpecialityDTO>();
            foreach (MOCOUP.ProviderSpeciality providerSpecialitie in providerSpecialities)
            {
                result.Add(CreateProviderSpeciality(providerSpecialitie));
            }
            return result;
        }

        #endregion

        #region Agent
        /// <summary>
        /// Crear un poveedor
        /// </summary>
        /// <param name="agent">The insured.</param>
        /// <returns></returns>
        public static AgentDTO CreateAgent(CompanyAgent agent)
        {
            var result = new AgentDTO();
            if (agent.AgentDeclinedType != null)
            {
                result.AgentDeclinedTypeId = agent.AgentDeclinedType.Id;
            }
            if (agent.AgentType != null)
            {
                result.AgentTypeId = agent.AgentType.Id;
            }
            if (agent.SalesChannel != null)
            {
                result.IdChanel = agent.SalesChannel.Id;
            }
            if (agent.GroupAgent != null)
            {
                result.IdGroup = agent.GroupAgent.Id;
            }
            if (agent.EmployeePerson != null)
            {
                result.EmployeePerson = new CompanyEmployePersonDto
                {
                    Id = agent.EmployeePerson.Id,
                    IdCardNo = agent.EmployeePerson.IdCardNo,
                    MotherLastName = agent.EmployeePerson.MotherLastName,
                    Name = agent.EmployeePerson.Name,
                };
            }

            result.Annotations = agent.Annotations;
            result.DateCurrent = agent.DateCurrent;
            result.DateDeclined = agent.DateDeclined;
            result.DateModification = agent.DateModification;
            result.IndividualId = agent.IndividualId;
            result.FullNName = agent.FullName;
            result.Locker = agent.Locker;
            result.CommissionDiscountAgreement = agent.CommissionDiscountAgreement;
            return result;
        }

        public static List<ComissionAgentDTO> CreateComissionAgentsa(List<CompanyComissionAgent> comissionAgent)
        {
            var result = new List<ComissionAgentDTO>();
            foreach (var item in comissionAgent)
            {

                result.Add(CreateComissionAgentDTO(item));
            }
            return result;
        }

        public static List<ComissionAgentDTO> CreateComissionAgents(CompanyComissionAgent comissionAgent)
        {
            var result = new List<ComissionAgentDTO>();
            result.Add(CreateComissionAgentDTO(comissionAgent));

            return result;
        }

        private static ComissionAgentDTO CreateComissionAgentDTO(CompanyComissionAgent item)
        {
            return new ComissionAgentDTO
            {
                DateCommission = item.DateCommission,
                LineBusinessId = item.LineBusiness.Id,
                PercentageAdditional = item.PercentageAdditional,
                PercentageCommission = item.PercentageCommission,
                PrefixId = item.Prefix.Id,
                SubLineBusinessId = item.SubLineBusiness.Id,
                AgencyId = item.AgentAgencyId,
                StatusTypeService = StatusTypeService.Original,
                Id = item.Id,
                prefix = item.Prefix.Description,
                lineBusiness = item.LineBusiness.Description,
                subLineBusiness = item.SubLineBusiness.Description,
                agency = item.agency,
            };
        }

        public static AgencyDTO CreateAgency(CompanyAgency agency)
        {
            var result = new AgencyDTO();
            if (agency.AgentDeclinedType != null)
            {
                result.AgentDeclinedTypeId = agency.AgentDeclinedType.Id;
            }

            result.Annotations = agency.Annotations;
            if (agency.Branch != null)
            {
                result.BranchId = agency.Branch.Id;
            }

            result.Code = agency.Code;
            result.DateDeclined = agency.DateDeclined;
            result.FullName = agency.FullName;
            result.Id = agency.Id;
            result.AgentId = agency.AgentType.Id;
            result.DescriptionBranch = agency.Branch.Description;
            result.IndividualId = agency.Agent.IndividualId;

            return result;
        }

        public static List<AgencyDTO> CreateAgencies(List<CompanyAgency> agencies)
        {
            var result = new List<AgencyDTO>();
            foreach (var agency in agencies)
            {
                result.Add(CreateAgency(agency));
            }
            return result;
        }

        public static List<AgencyDTO> CreateAgencies(CompanyAgency agencies)
        {
            var result = new List<AgencyDTO>();
            result.Add(CreateAgency(agencies));
            return result;
        }

        public static AgentAgencyDTO CreateAgentAgency(MOCOUP.AgentAgency agentAgency)
        {
            var result = new AgentAgencyDTO();
            result.AgencyAgencyId = agentAgency.AgencyAgencyId;
            result.AllianceId = agentAgency.AllianceId;
            result.IndividualId = agentAgency.IndividualId;
            result.IsSpecialImpression = agentAgency.IsSpecialImpression;
            result.Status = agentAgency.Status;
            return result;
        }

        public static List<AgentAgencyDTO> CreateAgentAgencies(List<MOCOUP.AgentAgency> agentAgencies)
        {
            var result = new List<AgentAgencyDTO>();

            foreach (var agentAgency in agentAgencies)
            {
                result.Add(CreateAgentAgency(agentAgency));
            }
            return result;
        }

        public static PrefixDTO CreatePrefix(CompanyPrefixs prefix)
        {
            var result = new PrefixDTO();
            result.Description = prefix.Description;
            result.Id = prefix.Id;
            return result;
        }

        public static List<PrefixDTO> CreatePrefixes(List<CompanyPrefixs> prefixes)
        {
            var result = new List<PrefixDTO>();

            foreach (var prefix in prefixes)
            {
                result.Add(CreatePrefix(prefix));
            }
            return result;
        }

        public static List<PrefixDTO> CreatePrefixes(CompanyPrefixs prefixes)
        {
            var result = new List<PrefixDTO>();
            result.Add(CreatePrefix(prefixes));

            return result;
        }
        #endregion

        #region individualSarlaft v1
        public static IndividualSarlaftDTO CreateIndividualSarlaft(IndividualSarlaft individualSarlaft)
        {
            IndividualSarlaftDTO individualSarlaftDTO = new IndividualSarlaftDTO();
            individualSarlaftDTO.BranchCode = individualSarlaft.BranchCode;
            individualSarlaftDTO.FormNum = individualSarlaft.FormNum;
            individualSarlaftDTO.Id = individualSarlaft.Id;
            individualSarlaftDTO.IndividualId = individualSarlaft.IndividualId;
            individualSarlaftDTO.InternationalOperations = individualSarlaft.InternationalOperations;
            individualSarlaftDTO.InterviewResultCode = individualSarlaft.InterviewResultCode;
            individualSarlaftDTO.InterviewerName = individualSarlaft.InterviewerName;
            individualSarlaftDTO.InterviewerPlace = individualSarlaft.InterviewerPlace;
            individualSarlaftDTO.PendingEvents = individualSarlaft.PendingEvents;
            individualSarlaftDTO.RegistrationDate = individualSarlaft.RegistrationDate;
            individualSarlaftDTO.VerifyingEmployee = individualSarlaft.VerifyingEmployee;
            individualSarlaftDTO.Year = individualSarlaft.Year;
            individualSarlaftDTO.ErrorServiceModel = new ErrorServiceModel { ErrorTypeService = 0, ErrorDescription = null };
            individualSarlaftDTO.StatusTypeService = StatusTypeService.Original;
            individualSarlaftDTO.finacialSarlaft = new SarlaftDTO
            {
                AssetsAmount = individualSarlaft.FinancialSarlaft.AssetsAmount,
                ExpenseAmount = individualSarlaft.FinancialSarlaft.ExpenseAmount,
                ExtraIncomeAmount = individualSarlaft.FinancialSarlaft.ExtraIncomeAmount,
                IncomeAmount = individualSarlaft.FinancialSarlaft.IncomeAmount,
                IsForeignTransaction = individualSarlaft.FinancialSarlaft.IsForeignTransaction,
                ForeignTransactionAmount = individualSarlaft.FinancialSarlaft.ForeignTransactionAmount,
                LiabilitiesAmount = individualSarlaft.FinancialSarlaft.LiabilitiesAmount,
                SarlaftId = individualSarlaft.FinancialSarlaft.SarlaftId,
                Description = individualSarlaft.FinancialSarlaft.Description,
            };

            return individualSarlaftDTO;
        }

        public static List<IndividualSarlaftDTO> CreateIndividualSarlafts(List<IndividualSarlaft> individualSarlaft)
        {
            List<IndividualSarlaftDTO> individualSarlaftDto = new List<IndividualSarlaftDTO>();
            foreach (var item in individualSarlaft)
            {
                individualSarlaftDto.Add(CreateIndividualSarlaft(item));
            }

            return individualSarlaftDto;
        }
        #endregion

        #region ReInsurer
        public static ReInsurerDTO CreateReInsurer(CompanyReInsurer companyReInsurer)
        {
            //var imapper = ModelAssembler.CreateMapReInsurer();
            //return imapper.Map<CompanyReInsurer, ReInsurerDTO>(companyReInsurer);
            return ModelAssembler.CreateReInsurer(companyReInsurer);
        }


        #endregion

        #region Partner
        /// <summary>
        /// Crear un Asociado
        /// </summary>
        /// <param name="partner">The pal.</param>
        /// <returns></returns>
        public static PartnerDTO CreatePartner(CompanyPartner partner)
        {
            var objs = new PartnerDTO()
            {
                Active = partner.Active,
                IndividualId = partner.IndividualId,
                PartnerId = partner.PartnerId,
                IdentificationDocumentNumber = partner.IdentificationDocument.Number,
                DocumentTypeId = partner.IdentificationDocument.DocumentType.Id,
                TradeName = partner.TradeName,
                statusTypeService = StatusTypeService.Original,
                Description = partner.IdentificationDocument.DocumentType.Description,
            };
            return objs;
        }

        /// <summary>
        /// crea mas de un asociado
        /// </summary>
        /// <param name="partners"></param>
        /// <returns></returns>
        public static List<PartnerDTO> CreatePartners(List<CompanyPartner> partners)
        {
            var objs = new List<PartnerDTO>();
            foreach (var partner in partners)
            {
                objs.Add(AplicationAssembler.CreatePartner(partner));
            }
            return objs;
        }
        #endregion

        #region companyName
        /// <summary>
        /// crea razon social
        /// </summary>
        /// <param name="CpoName"></param>
        /// <returns></returns>
        public static CompanyNameDTO CreateCompanyName(MOCOUP.CompanyName CpoName)
        {
            var objs = new CompanyNameDTO()
            {
                AddressID = CpoName.Address.Id,
                EmailID = CpoName.Email.Id,
                IndividualId = CpoName.IndividualId,
                IsMain = CpoName.IsMain,
                PhoneID = CpoName.Phone.Id,
                NameNum = CpoName.NameNum,
                TradeName = CpoName.TradeName
            };
            return objs;
        }

        /// <summary>
        /// Crea mas de una razon social
        /// </summary>
        /// <param name="cpoNames"></param>
        /// <returns></returns>
        public static List<CompanyNameDTO> CreateCompaniesName(List<MOCOUP.CompanyName> cpoNames)
        {
            var objs = new List<CompanyNameDTO>();
            foreach (var cpoName in cpoNames)
            {
                objs.Add(AplicationAssembler.CreateCompanyName(cpoName));
            }
            return objs;
        }
        #endregion

        #region SarlaftPerson
        /// <summary>
        /// Crea sarlaft 
        /// </summary>
        /// <param name="fsarlaft"></param>
        /// <returns></returns>
        public static SarlaftDTO CreateFinancialSarlaft(FinancialSarlaf fsarlaft)
        {
            var objs = new SarlaftDTO()
            {
                SarlaftId = fsarlaft.SarlaftId,
                AssetsAmount = fsarlaft.AssetsAmount,
                Description = fsarlaft.Description,
                ExpenseAmount = fsarlaft.ExpenseAmount,
                ExtraIncomeAmount = fsarlaft.ExtraIncomeAmount,
                ForeignTransactionAmount = fsarlaft.ForeignTransactionAmount,
                IsForeignTransaction = fsarlaft.IsForeignTransaction,
                IncomeAmount = fsarlaft.IncomeAmount,
                LiabilitiesAmount = fsarlaft.LiabilitiesAmount
            };
            return objs;
        }
        /// <summary>
        /// crea mas de un sarlaft
        /// </summary>
        /// <param name="fsarlafts"></param>
        /// <returns></returns>
        public static List<SarlaftDTO> CreateFinancialSarlafts(List<FinancialSarlaf> fsarlafts)
        {
            var objs = new List<SarlaftDTO>();
            foreach (var sarlaft in fsarlafts)
            {
                objs.Add(AplicationAssembler.CreateFinancialSarlaft(sarlaft));
            }
            return objs;
        }

        #endregion

        #region OperatingQuota

        /// <summary>
        /// Convierte una lista de objetos OperatingQuota a OperatingQuotaDTO
        /// </summary>
        /// <param name="ListOperatingQuotaModel"></param>
        /// <returns></returns>
        internal static List<OperatingQuotaDTO> CreateOperatingQuotaDTOs(List<CompanyOperatingQuota> ListOperatingQuotaModel)
        {
            List<OperatingQuotaDTO> operatingQuotaDTOs = new List<OperatingQuotaDTO>();
            foreach (CompanyOperatingQuota OperatingQuota in ListOperatingQuotaModel)
            {
                operatingQuotaDTOs.Add(CreateOperatingQuotaDTO(OperatingQuota));
            }
            return operatingQuotaDTOs;

        }

        /// <summary>
        /// Convierte un objeto OperatingQuota a OperatingQuotaDTO
        /// </summary>
        /// <param name="operatingQuota"></param>
        /// <returns></returns>
        public static OperatingQuotaDTO CreateOperatingQuotaDTO(CompanyOperatingQuota operatingQuota)
        {
            var result = new OperatingQuotaDTO();
            result.IndividualId = operatingQuota.IndividualId;
            result.LineBusinessId = operatingQuota.LineBusinessId;
            result.AmountValue = operatingQuota.Amount;
            result.CurrencyId = operatingQuota.CurrencyId;
            result.CurrentTo = operatingQuota.CurrentTo;
            return result;
        }
        #endregion



        #region InsuredGuarantee
        /// <summary>
        /// Crea  contragarantia de model a dto
        /// </summary>
        /// <param name="insuredGuarantee"></param>
        /// <returns></returns>
        public static GuaranteeDTO CreateGuarantee(MOCOUP.Guarantee guarantee)
        {
            return new GuaranteeDTO()
            {
                //GuaranteeTypeId = guarantee.GuaranteeType.Code,
                //InsuredGuaranteeId = guarantee.InsuredGuarantee.Id,
                //IndividualId = guarantee.InsuredGuarantee.IndividualId,
                //Address = guarantee.InsuredGuarantee.Address,
                //StatusCode = guarantee.InsuredGuarantee.GuaranteeStatus.Code,
                //AppraisalAmount = guarantee.InsuredGuarantee.AppraisalAmount,
                //BuiltArea = guarantee.InsuredGuarantee.BuiltArea,
                //DocumentValueAmount = guarantee.InsuredGuarantee.DocumentValueAmount,
                //MeasureArea = guarantee.InsuredGuarantee.MeasureArea,
                //RegistrationDate = guarantee.InsuredGuarantee.RegistrationDate,
                //Description = guarantee.InsuredGuarantee.Description,
                //IsCloseInd = guarantee.InsuredGuarantee.IsCloseInd,
                //AppraisalDate = guarantee.InsuredGuarantee.AppraisalDate,
                //ExpertName = guarantee.InsuredGuarantee.ExpertName,
                //InsuranceAmount = guarantee.InsuredGuarantee.InsuranceAmount,
                //PolicyNumber = guarantee.InsuredGuarantee.PolicyNumber,
                //DocumentNumber = guarantee.InsuredGuarantee.DocumentNumber,
                //ExpirationDate = guarantee.InsuredGuarantee.ExpirationDate,
                //RegistrationNumber = guarantee.InsuredGuarantee.RegistrationNumber,
                //LicensePlate = guarantee.InsuredGuarantee.LicensePlate,
                //EngineNro = guarantee.InsuredGuarantee.EngineNro,
                //ChassisNro = guarantee.InsuredGuarantee.ChassisNro,
                //SignatoriesNumber = guarantee.InsuredGuarantee.SignatoriesNumber,
                //CountryID = guarantee.InsuredGuarantee.Country != null ? (int?)guarantee.InsuredGuarantee.Country.Id : null,
                //CityId = guarantee.InsuredGuarantee.City != null ? (int?)guarantee.InsuredGuarantee.City.Id : null,
                //StateId = guarantee.InsuredGuarantee.State != null ? (int?)guarantee.InsuredGuarantee.State.Id : null,
                //BranchId = guarantee.InsuredGuarantee.Branch != null ? (int?)guarantee.InsuredGuarantee.Branch.Id : null,
                //PromissoryNoteTypeId = guarantee.InsuredGuarantee.PromissoryNoteType != null ? (int?)guarantee.InsuredGuarantee.PromissoryNoteType.Code : null,
                //MeasurementTypeId = guarantee.InsuredGuarantee.MeasurementType != null ? (int?)guarantee.InsuredGuarantee.MeasurementType.Code : null,
                //CurrencyId = guarantee.InsuredGuarantee.Currency != null ? (int?)guarantee.InsuredGuarantee.Currency.Id : null,
                //InsuranceCompanyId = guarantee.InsuredGuarantee.InsuranceCompanyId,
                //AssetTypeCode = guarantee.InsuredGuarantee.AssetTypeCode,
                //GuaranteeCode = guarantee.InsuredGuarantee.Code,



            };
        }

        /// <summary>
        /// crea mas de una contragarantia de model a dto
        /// </summary>
        /// <param name="partners"></param>
        /// <returns></returns>
        public static List<GuaranteeDTO> CreateGuaranties(List<MOCOUP.Guarantee> insuredGuarantee)
        {
            var objs = new List<GuaranteeDTO>();
            foreach (var insGuartee in insuredGuarantee)
            {
                objs.Add(AplicationAssembler.CreateGuarantee(insGuartee));
            }
            return objs;
        }
        /// <summary>
        /// Crea  contragarantia de model a dto
        /// </summary>
        /// <param name="insuredGuarantee"></param>
        /// <returns></returns>
        public static GuaranteeDTO CreateInsuredGuarantee(MOCOUP.InsuredGuarantee insuredGuarantee)
        {
            return new GuaranteeDTO()
            {
                //InsuredGuaranteeId = insuredGuarantee.Id,
                //IndividualId = insuredGuarantee.IndividualId,
                //Address = insuredGuarantee.Address,
                //StatusCode = insuredGuarantee.GuaranteeStatus.Code,
                //AppraisalAmount = insuredGuarantee.AppraisalAmount,
                //BuiltArea = insuredGuarantee.BuiltArea,
                //DocumentValueAmount = insuredGuarantee.DocumentValueAmount,
                //MeasureArea = insuredGuarantee.MeasureArea,
                //RegistrationDate = insuredGuarantee.RegistrationDate,
                //Description = insuredGuarantee.Description,
                //IsCloseInd = insuredGuarantee.IsCloseInd,
                //AppraisalDate = insuredGuarantee.AppraisalDate,
                //ExpertName = insuredGuarantee.ExpertName,
                //InsuranceAmount = insuredGuarantee.InsuranceAmount,
                //PolicyNumber = insuredGuarantee.PolicyNumber,
                //DocumentNumber = insuredGuarantee.DocumentNumber,
                //ExpirationDate = insuredGuarantee.ExpirationDate,
                //RegistrationNumber = insuredGuarantee.RegistrationNumber,
                //LicensePlate = insuredGuarantee.LicensePlate,
                //EngineNro = insuredGuarantee.EngineNro,
                //ChassisNro = insuredGuarantee.ChassisNro,
                //SignatoriesNumber = insuredGuarantee.SignatoriesNumber,
                //CountryID = insuredGuarantee.Country != null ? (int?)insuredGuarantee.Country.Id : null,
                //CityId = insuredGuarantee.City != null ? (int?)insuredGuarantee.City.Id : null,
                //StateId = insuredGuarantee.State != null ? (int?)insuredGuarantee.State.Id : null,
                //BranchId = insuredGuarantee.Branch != null ? (int?)insuredGuarantee.Branch.Id : null,
                //PromissoryNoteTypeId = insuredGuarantee.PromissoryNoteType != null ? (int?)insuredGuarantee.PromissoryNoteType.Code : null,
                //MeasurementTypeId = insuredGuarantee.MeasurementType != null ? (int?)insuredGuarantee.MeasurementType.Code : null,
                //CurrencyId = insuredGuarantee.Currency != null ? (int?)insuredGuarantee.Currency.Id : null,
                //InsuranceCompanyId = insuredGuarantee.InsuranceCompanyId,
                //AssetTypeCode = insuredGuarantee.AssetTypeCode,
                //GuaranteeCode = insuredGuarantee.Code,

                //Guarantors = CreateGuarantors(insuredGuarantee.Guarantors),
                //listDocumentation = CreateInsuredGuaranteeDocumentation(insuredGuarantee.listDocumentation),
                //listPrefix = CreateInsuredGuaranteePrefixies(insuredGuarantee.listPrefix),
                //InsuredGuaranteeLog = CreateInsuredGuaranteeLog(insuredGuarantee.InsuredGuaranteeLog),

            };
        }

        /// <summary>
        /// crea mas de una contragarantia de model a dto
        /// </summary>
        /// <param name="partners"></param>
        /// <returns></returns>
        public static List<GuaranteeDTO> CreateInsuredGuaranties(List<MOCOUP.InsuredGuarantee> insuredGuarantee)
        {
            var objs = new List<GuaranteeDTO>();
            foreach (var insGuartee in insuredGuarantee)
            {
                objs.Add(AplicationAssembler.CreateInsuredGuarantee(insGuartee));
            }
            return objs;
        }

        #region Guarantor
        /// <summary>
        /// Crea  Contragarante
        /// </summary>
        /// <param name="guarantor"></param>
        /// <returns></returns>
        public static GuarantorDTO CreateGuarantor(CompanyGuarantor guarantor)
        {
            return new GuarantorDTO()
            {
                GuaranteeId = guarantor.GuaranteeId,
                IndividualId = guarantor.IndividualId,
                GuarantorId = guarantor.GuarantorId,
                Adrress = guarantor.Adrress,
                CardNro = guarantor.CardNro,
                CityText = guarantor.CityText,
                TributaryIdNo = guarantor.TributaryIdNo,
                Name = guarantor.Name,
                TradeName = guarantor.TradeName,
                PhoneNumber = guarantor.PhoneNumber,
                ParametrizationStatus = ParametrizationStatus.Original
            };

        }

        /// <summary>
        /// Crea  Contragarantes
        /// </summary>
        /// <param name="guarantors"></param>
        /// <returns></returns>
        public static List<GuarantorDTO> CreateGuarantors(List<CompanyGuarantor> guarantors)
        {
            var objs = new List<GuarantorDTO>();
            foreach (var guarantor in guarantors)
            {
                objs.Add(AplicationAssembler.CreateGuarantor(guarantor));
            }
            return objs;
        }
        #endregion

        #region InsuredGuaranteeLog
        /// <summary>
        /// Crea  log
        /// </summary>
        /// <param name="insuredGuaranteeLog"></param>
        /// <returns></returns>
        internal static InsuredGuaranteeLogDTO CreateInsuredGuaranteeLog(CompanyInsuredGuaranteeLog insuredGuaranteeLog)
        {
            return new InsuredGuaranteeLogDTO()
            {
                IndividualId = insuredGuaranteeLog.IndividualId,
                Description = insuredGuaranteeLog.Description,
                GuaranteeId = insuredGuaranteeLog.GuaranteeId,
                GuaranteeStatusCode = insuredGuaranteeLog.GuaranteeStatusCode,
                LogDate = insuredGuaranteeLog.LogDate.ToString("dd/MM/yyyyy"),
                UserId = insuredGuaranteeLog.UserId,
                UserName = insuredGuaranteeLog.UserName
            };
        }


        internal static List<InsuredGuaranteeLogDTO> CreateInsuredGuaranteeLogs(List<CompanyInsuredGuaranteeLog> insuredGuaranteeLog)
        {
            List<InsuredGuaranteeLogDTO> insuredGuaranteeLogs = new List<InsuredGuaranteeLogDTO>();

            foreach (CompanyInsuredGuaranteeLog item in insuredGuaranteeLog)
            {
                insuredGuaranteeLogs.Add(CreateInsuredGuaranteeLog(item));
            }

            return insuredGuaranteeLogs;

        }
        #endregion

        #region InsuredGuaranteeDocumentation
        /// <summary>
        /// Crea  Contragarante
        /// </summary>
        /// <param name="guarantor"></param>
        /// <returns></returns>
        public static InsuredGuaranteeDocumentationDTO CreateInsuredGuaranteeDocument(MOCOV1.CompanyInsuredGuaranteeDocumentation insuredGuaranteeDocumentation)
        {
            return new InsuredGuaranteeDocumentationDTO()
            {
                DocumentCode = insuredGuaranteeDocumentation.DocumentCode,
                GuaranteeCode = insuredGuaranteeDocumentation.GuaranteeCode,
                GuaranteeId = insuredGuaranteeDocumentation.GuaranteeId,
                IndividualId = insuredGuaranteeDocumentation.IndividualId,
                ParametrizationStatus = ParametrizationStatus.Original
            };

        }

        /// <summary>
        /// Crea  Contragarantes
        /// </summary>
        /// <param name="guarantors"></param>
        /// <returns></returns>
        public static List<InsuredGuaranteeDocumentationDTO> CreateInsuredGuaranteeDocuments(List<MOCOV1.CompanyInsuredGuaranteeDocumentation> insuredGuaranteeDocumentation)
        {
            var objs = new List<InsuredGuaranteeDocumentationDTO>();
            foreach (var insuredGuaranteeDocument in insuredGuaranteeDocumentation)
            {
                objs.Add(AplicationAssembler.CreateInsuredGuaranteeDocument(insuredGuaranteeDocument));
            }
            return objs;
        }
        #endregion

        #region InsuredGuaranteePrefix
        /// <summary>
        /// Crea  Contragarante
        /// </summary>
        /// <param name="guarantor"></param>
        /// <returns></returns>
        public static InsuredGuaranteePrefixDTO CreateInsuredGuaranteePrefix(MOCOUP.InsuredGuaranteePrefix insuredGuaranteePrefix)
        {
            return new InsuredGuaranteePrefixDTO()
            {
                GuaranteeId = insuredGuaranteePrefix.GuaranteeId,
                IndividualId = insuredGuaranteePrefix.IndividualId,
                PrefixCode = insuredGuaranteePrefix.PrefixCode,
                Parameter = ParametrizationStatus.Original
            };

        }

        /// <summary>
        /// Crea  guarantors
        /// </summary>
        /// <param name="guarantors"></param>
        /// <returns></returns>
        public static List<InsuredGuaranteePrefixDTO> CreateInsuredGuaranteePrefixies(List<MOCOUP.InsuredGuaranteePrefix> insuredGuaranteePrefixies)
        {
            var objs = new List<InsuredGuaranteePrefixDTO>();
            foreach (var insuredGuaranteePrefix in insuredGuaranteePrefixies)
            {
                objs.Add(AplicationAssembler.CreateInsuredGuaranteePrefix(insuredGuaranteePrefix));
            }
            return objs;
        }
        #endregion

        #endregion

        #region PaymentMethod
        internal static List<IndividualPaymentMethodDTO> CreateIndividualpaymentMethods(List<CompanyIndividualPaymentMethod> list)
        {
            List<IndividualPaymentMethodDTO> individualPaymentMethodDTOs = new List<IndividualPaymentMethodDTO>();

            foreach (CompanyIndividualPaymentMethod item in list)
            {
                individualPaymentMethodDTOs.Add(CreateIndividualpaymentMethod(item));
            }
            return individualPaymentMethodDTOs;

        }

        internal static IndividualPaymentMethodDTO CreateIndividualpaymentMethod(CompanyIndividualPaymentMethod individualPaymentMethod)
        {
            return new IndividualPaymentMethodDTO()
            {
                Id = individualPaymentMethod.Id,
                Method = new SelectDTO()
                {
                    Id = individualPaymentMethod.Method.Id,
                    Description = individualPaymentMethod.Method.Description
                },
                Account = individualPaymentMethod.Account == null ? null
                    : new PaymentAccountDTO()
                    {
                        Number = individualPaymentMethod.Account.Number.ToString(),
                        BankBranch = individualPaymentMethod.Account.BankBranch == null ? null
                        : new SelectDTO()
                        {
                            Id = individualPaymentMethod.Account.BankBranch.Id,
                            Description = individualPaymentMethod.Account.BankBranch.Description
                        },
                        Bank = individualPaymentMethod.Account.BankBranch?.Bank == null ? null
                        : new SelectDTO()
                        {
                            Id = individualPaymentMethod.Account.BankBranch.Bank.Id,
                            Description = individualPaymentMethod.Account.BankBranch.Bank.Description
                        },

                        Type = individualPaymentMethod.Account.Type == null ? null
                        : new SelectDTO()
                        {
                            Id = individualPaymentMethod.Account.Type.Id,
                            Description = individualPaymentMethod.Account.Type.Description
                        }
                    }
            };
        }

        #endregion

        #region IndividualTaxExeption

        /// <summary>
        /// Convierte una lista de IndividualTaxExeption a IndividualTaxExeptionDTO
        /// </summary>
        /// <param name="listIndividualTaxExeptionDTOModel"></param>
        /// <returns></returns>
        public static List<IndividualTaxExeptionDTO> CreateIndividualTaxExeptionDTOs(List<CompanyIndividualTax> listIndividualTax)
        {
            List<IndividualTaxExeptionDTO> individualTaxExeptionDTOs = new List<IndividualTaxExeptionDTO>();
            foreach (CompanyIndividualTax IndividualTax in listIndividualTax)
            {
                individualTaxExeptionDTOs.Add(CreateIndividualTaxExeptionDTO(IndividualTax, IndividualTax.IndividualTaxExeption));
            }
            return individualTaxExeptionDTOs;
        }

        /// <summary>
        /// Convierte un objeto IndividualTaxExeption a IndividualTaxExeptionDTO
        /// </summary>
        /// <param name="individualTaxExeption"></param>
        /// <returns></returns>
        public static IndividualTaxExeptionDTO CreateIndividualTaxExeptionDTO(CompanyIndividualTax individualTax)
        {
            var result = new IndividualTaxExeptionDTO();
            result.IndividualId = individualTax.IndividualId;
            result.TaxId = individualTax.taxRate.Tax.Id;
            result.TaxCondition = individualTax.taxRate.TaxCondition.Id;
            result.IndividualTaxExemptionId = individualTax.Id;

            return result;

        }
        public static IndividualTaxExeptionDTO CreateIndividualTaxExeptionDTO(CompanyIndividualTax individualTax, CompanyIndividualTaxExeption individualTaxExeption)
        {
            var result = new IndividualTaxExeptionDTO();

            if (individualTaxExeption != null)
            {
                result.IndividualTaxExemptionId = individualTaxExeption.IndividualTaxExemptionId;
                result.TaxCategoryId = individualTaxExeption.TaxCategory?.Id ?? 0;
                result.StateCode = individualTaxExeption.StateCode?.Id ?? 0;
                result.Datefrom = individualTaxExeption.Datefrom;
                result.CountryId = individualTaxExeption.CountryCode;
                result.DateUntil = individualTaxExeption.DateUntil;
                result.OfficialBulletinDate = individualTaxExeption.OfficialBulletinDate;
                result.TotalRetention = individualTaxExeption.TotalRetention;
                result.ExtentPercentage = individualTaxExeption.ExtentPercentage;
                result.ResolutionNumber = individualTaxExeption.ResolutionNumber;
                result.TaxCategoryDescription = individualTaxExeption.TaxCategory?.Description;
                result.StateCodeDescription = individualTaxExeption.StateCode?.Description;
            }

            result.TaxRateId = individualTax.taxRate.Id;
            result.TaxId = individualTax.taxRate.Tax.Id;
            result.TaxDescription = individualTax.taxRate.Tax.Description;
            result.TaxCondition = individualTax.taxRate.TaxCondition?.Id ?? 0;
            result.TaxConditionDescription = individualTax.taxRate.TaxCondition?.Description ?? string.Empty;
            result.Id = individualTax.Id;
            result.IndividualId = individualTax.IndividualId;
            result.status = StatusTypeService.Original;
            result.RoleId = individualTax.Role.Id;
            result.RoleDescription = individualTax.Role.Description;

            return result;

        }

        #endregion

        #region ProspectPersonNatural
        public static ProspectPersonNaturalDTO CreateProspectPersonNatural(CompanyProspectNatural prospectNatural)
        {
            ProspectPersonNaturalDTO prospectNaturalRet = new ProspectPersonNaturalDTO
            {
                AdditionaInformation = prospectNatural.AdditionalInfo == null ? "" : prospectNatural.AdditionalInfo,
                BirthDate = prospectNatural.BirthDate,

                Address = prospectNatural.AddressType == null ? null
                    : new SelectDTO()
                    {
                        Id = prospectNatural.AddressType,
                        Description = prospectNatural.Street
                    },
                City = prospectNatural.City == null ? null
                    : new SelectDTO()
                    {
                        Id = prospectNatural.City?.Id,
                        Description = prospectNatural.City.Description
                    },
                State = prospectNatural.City?.State == null ? null
                    : new SelectDTO()
                    {
                        Id = prospectNatural.City?.State?.Id,
                        Description = prospectNatural.City?.State.Description
                    },
                Country = prospectNatural.City?.State.Country == null ? null
                    : new SelectDTO()
                    {
                        Id = prospectNatural.City?.State.Country?.Id,
                        Description = prospectNatural.City?.State.Country.Description
                    },
                DANECode = prospectNatural.City?.DANECode == null ? null : prospectNatural.City.DANECode,

                EmailAddres = prospectNatural.EmailAddress,
                Gender = prospectNatural.Gender,
                Card = new SelectDTO()
                {
                    Id = prospectNatural.IdCardTypeCode,
                    Description = prospectNatural.IdCardNo
                },
                MartialStatus = prospectNatural.MaritalStatus,
                MotherLastName = prospectNatural.MotherLastName,
                Name = prospectNatural.Name,
                PhoneNumber = prospectNatural.PhoneNumber,
                ProspectCode = prospectNatural.ProspectCode,
                SurName = prospectNatural.Surname,
                IndividualTypePerson = prospectNatural.IndividualTypePerson
            };

            return prospectNaturalRet;
        }

        public static List<ProspectPersonNaturalDTO> CreateProspectPersonNaturals(List<CompanyProspectNatural> prospectNaturala)
        {
            var result = new List<ProspectPersonNaturalDTO>();
            foreach (CompanyProspectNatural item in prospectNaturala)
            {
                result.Add(CreateProspectPersonNatural(item));
            }
            return result;
        }
        public static ProspectPersonNaturalDTO CreateProspectPersonNaturalAdv(CompanyPerson prospectNaturals)
        {
            int identificacion = prospectNaturals.IdentificationDocument.DocumentType.Id;
            int? identificar = identificacion;
            return new ProspectPersonNaturalDTO
            {
                //AddresType = prospectNaturals.Addresses[0].Id,
                //IndividualTypePerson = prospectNaturals.PersonType.PersonTypeCode,
                //SurName = prospectNaturals.Surname,
                //MotherLastName = prospectNaturals.MotherLastName,
                Name = prospectNaturals.Name,
                //IdCarTypeCode = identificar,
                //IdCardNo = prospectNaturals.IdentificationDocument.Number,
                //IndividualId = prospectNaturals.IndividualId
            };
        }

        public static List<ProspectPersonNaturalDTO> CreateProspectPersonsNaturalAdv(List<CompanyPerson> prospectNaturals)
        {
            var result = new List<ProspectPersonNaturalDTO>();
            foreach (var item in prospectNaturals)
            {
                result.Add(AplicationAssembler.CreateProspectPersonNaturalAdv(item));
            }
            return result;
        }
        #endregion

        #region ProspectLegal

        /// <summary>
        /// Crea una lista de ProspectLegalDTO a partir de una lista ProspectNatural
        /// </summary>
        /// <param name="listProspectLegalModel"></param>
        /// <returns></returns>
        public static List<ProspectLegalDTO> CreateProspectLegalDTOs(List<CompanyProspectNatural> listProspectLegalModel)
        {
            List<ProspectLegalDTO> ListProspectLegalDTOs = new List<ProspectLegalDTO>();
            foreach (CompanyProspectNatural item in listProspectLegalModel)
            {
                ListProspectLegalDTOs.Add(CreateProspectLegalDTO(item));
            }
            return ListProspectLegalDTOs;
        }

        /// <summary>
        /// Mapea un ProspectLegalDTO a partir de un ProspectNatural
        /// </summary>
        /// <param name="ProspectLegalModel"></param>
        /// <returns></returns>
        public static ProspectLegalDTO CreateProspectLegalDTO(CompanyProspectNatural ProspectLegalModel)
        {
            ProspectLegalDTO prospectLegalDTO = new ProspectLegalDTO();



            prospectLegalDTO.ProspectCode = ProspectLegalModel.ProspectCode;
            prospectLegalDTO.AdditionaInformation = ProspectLegalModel.AdditionalInfo;
            prospectLegalDTO.Address = new SelectDTO()
            {
                Id = ProspectLegalModel.AddressType,
                Description = ProspectLegalModel.Street
            };
            if (prospectLegalDTO.City != null)
            {
                prospectLegalDTO.City = new SelectDTO()
                {
                    Id = ProspectLegalModel.City?.Id,
                    Description = ProspectLegalModel.City?.Description
                };

            };
            prospectLegalDTO.State = new SelectDTO()
            {
                Id = ProspectLegalModel.City?.State?.Id,
                Description = ProspectLegalModel.City?.State?.Description
            };
            prospectLegalDTO.Country = new SelectDTO()
            {
                Id = ProspectLegalModel.City?.State?.Country.Id,
                Description = ProspectLegalModel.City?.State?.Country?.Description
            };
            prospectLegalDTO.DANECode = ProspectLegalModel.City?.DANECode;
            prospectLegalDTO.EmailAddres = ProspectLegalModel.EmailAddress;
            prospectLegalDTO.Name = ProspectLegalModel.Name;
            prospectLegalDTO.PhoneNumber = ProspectLegalModel.PhoneNumber;
            prospectLegalDTO.IndividualTypePerson = ProspectLegalModel.IndividualTypePerson;
            prospectLegalDTO.TributaryIdNumber = ProspectLegalModel.TributaryIdNumber;
            prospectLegalDTO.TributaryIdTypeCode = Convert.ToInt32(ProspectLegalModel.TributaryIdTypeCode);
            return prospectLegalDTO;
        }


        #endregion

        #region Consortium
        public static ConsorciatedDTO CreateConsortium(CompanyConsortium consortium)
        {
            ConsorciatedDTO companyConsortium = new ConsorciatedDTO();

            companyConsortium.ConsortiumId = consortium.ConsortiumId;
            companyConsortium.Enabled = consortium.Enabled;
            companyConsortium.IndividualId = consortium.IndividualId;
            companyConsortium.InsuredCode = consortium.InsuredCode;
            companyConsortium.IsMain = consortium.IsMain;
            companyConsortium.ParticipationRate = consortium.ParticipationRate;
            companyConsortium.StartDate = consortium.StartDate;
            companyConsortium.FullName = consortium.FullName;
            companyConsortium.CompanyIdentifationNumber = consortium.IdentificationDocument?.Number;
            companyConsortium.PersonIdentificationNumber = consortium.IdentificationDocument?.Number;
            companyConsortium.DocumentType = consortium.IdentificationDocument?.DocumentType?.Id;

            return companyConsortium;
        }

        public static List<ConsorciatedDTO> CreateConsortiums(List<CompanyConsortium> consortiums)
        {
            var result = new List<ConsorciatedDTO>();
            foreach (var item in consortiums)
            {
                result.Add(AplicationAssembler.CreateConsortium(item));
            }
            return result;
        }
        #endregion

        #region LegalRepresentative
        /// <summary>
        /// Convierte un objeto LegalRepresentative a LegalRepresentativeDTO
        /// </summary>
        /// <param name="companyLegalRepresentative"></param>
        /// <returns></returns>
        internal static LegalRepresentativeDTO CreateLegalRepresentativeDTO(CompanyLegalRepresentative companyLegalRepresentative)
        {
            if (companyLegalRepresentative != null)
            {
                return new LegalRepresentativeDTO
                {
                    Address = companyLegalRepresentative.Address,
                    BirthDate = companyLegalRepresentative.BirthDate,
                    BirthPlace = companyLegalRepresentative.BirthPlace,
                    CellPhone = companyLegalRepresentative.CellPhone,
                    City = new SelectDTO() { Id = companyLegalRepresentative.City.Id, Description = companyLegalRepresentative.City.Description },
                    Country = new SelectDTO() { Id = companyLegalRepresentative.City.State.Country.Id, Description = companyLegalRepresentative.City.State.Country.Description },
                    State = new SelectDTO() { Id = companyLegalRepresentative.City.State.Id, Description = companyLegalRepresentative.City.State.Description },
                    CurrencyId = companyLegalRepresentative.AuthorizationAmount.Currency.Id,
                    Description = companyLegalRepresentative.Description,
                    DocumentTypeId = companyLegalRepresentative.IdentificationDocument.DocumentType.Id,
                    Email = companyLegalRepresentative.Email,
                    ExpeditionDate = companyLegalRepresentative.ExpeditionDate,
                    ExpeditionPlace = companyLegalRepresentative.ExpeditionPlace,
                    FullName = companyLegalRepresentative.FullName,
                    individualId = companyLegalRepresentative.Id,
                    JobTitle = companyLegalRepresentative.JobTitle,
                    Nationality = companyLegalRepresentative.Nationality,
                    NumberDocument = companyLegalRepresentative.IdentificationDocument.Number,
                    Phone = companyLegalRepresentative.Phone,
                    Value = companyLegalRepresentative.AuthorizationAmount.Value
                };
            }
            else
            {
                return null;
            }

        }
        #endregion

        #region CompanyCoInsured
        public static CompanyCoInsuredDTO CreateCompanyCoInsured(MOCOUP.CompanyCoInsured companyCoInsured)
        {
            return new CompanyCoInsuredDTO
            {
                AddressTypeCode = companyCoInsured.AddressTypeCode,
                Annotations = companyCoInsured.Annotations,
                CityCode = companyCoInsured.CityCode,
                CountryCode = companyCoInsured.CountryCode,
                Description = companyCoInsured.Description,
                EnsureInd = companyCoInsured.EnsureInd,
                EnteredDate = companyCoInsured.EnteredDate,
                InsuraceCompanyId = companyCoInsured.InsuraceCompanyId,
                ModifyDate = companyCoInsured.ModifyDate,
                PhoneNumber = companyCoInsured.PhoneNumber,
                PhoneTypeCode = companyCoInsured.PhoneTypeCode,
                StateCode = companyCoInsured.StateCode,
                Street = companyCoInsured.Street,
                TributaryIdNo = companyCoInsured.TributaryIdNo,
                ComDeclinedTypeCode = companyCoInsured.ComDeclinedTypeCode,
                DeclinedDate = companyCoInsured.DeclinedDate,

            };
        }
        #endregion

        #region SarlafatPersonNatural
        public static IndividualSarlaftDTO CreateSarlaftPErsonNatural(IndividualSarlaft individualSarlaft)
        {
            return new IndividualSarlaftDTO
            {
                BranchCode = individualSarlaft.BranchCode,
                FormNum = individualSarlaft.FormNum,
                Id = individualSarlaft.Id,
                IndividualId = individualSarlaft.IndividualId,
                InternationalOperations = individualSarlaft.InternationalOperations,
                InterviewResultCode = individualSarlaft.InterviewResultCode,
                InterviewerName = individualSarlaft.InterviewerName,
                InterviewerPlace = individualSarlaft.InterviewerPlace,
                PendingEvents = individualSarlaft.PendingEvents,
                RegistrationDate = individualSarlaft.RegistrationDate,
                VerifyingEmployee = individualSarlaft.VerifyingEmployee,
                Year = individualSarlaft.Year,
                ErrorServiceModel = new ErrorServiceModel { ErrorTypeService = 0, ErrorDescription = null },
                StatusTypeService = StatusTypeService.Original,
                finacialSarlaft = new SarlaftDTO
                {
                    AssetsAmount = individualSarlaft.FinancialSarlaft.AssetsAmount,
                    ExpenseAmount = individualSarlaft.FinancialSarlaft.ExpenseAmount,
                    ExtraIncomeAmount = individualSarlaft.FinancialSarlaft.ExtraIncomeAmount,
                    IncomeAmount = individualSarlaft.FinancialSarlaft.IncomeAmount,
                    IsForeignTransaction = individualSarlaft.FinancialSarlaft.IsForeignTransaction,
                    ForeignTransactionAmount = individualSarlaft.FinancialSarlaft.ForeignTransactionAmount,
                    LiabilitiesAmount = individualSarlaft.FinancialSarlaft.LiabilitiesAmount,
                    SarlaftId = individualSarlaft.FinancialSarlaft.SarlaftId,
                    Description = individualSarlaft.FinancialSarlaft.Description,
                },

            };
        }

        public static List<IndividualSarlaftDTO> CreateSarlaftPErsonNaturals(List<IndividualSarlaft> individualSarlaft)
        {
            var IndividualSarlaftDTO = new List<IndividualSarlaftDTO>();
            foreach (var item in individualSarlaft)
            {
                IndividualSarlaftDTO.Add(CreateSarlaftPErsonNatural(item));
            }
            return IndividualSarlaftDTO;
        }
        #endregion

        #region Individual Role

        public static IndividualRoleDTO CreateIndividualRole(CompanyIndividualRole companyIndividualRole)
        {
            var result = new IndividualRoleDTO();
            result.IndividualId = companyIndividualRole.IndividualId;
            result.RoleId = companyIndividualRole.RoleId;
            return result;
        }

        public static List<IndividualRoleDTO> CreateIndividualRoles(List<CompanyIndividualRole> companyIndividualRole)
        {
            var individualRoleDTO = new List<IndividualRoleDTO>();
            foreach (CompanyIndividualRole individualRole in companyIndividualRole)
            {
                individualRoleDTO.Add(CreateIndividualRole(individualRole));
            }
            return individualRoleDTO;
        }

        #endregion

        #region MaritalStatus

        public static MaritalStatusDTO CreateMaritalStatusDTO(CompanyMaritalStatus companyMaritalStatus)
        {
            var result = new MaritalStatusDTO();
            result.Id = companyMaritalStatus.Id;
            result.Description = companyMaritalStatus.Description;
            result.SmallDescription = companyMaritalStatus.SmallDescription;
            return result;
        }

        public static List<MaritalStatusDTO> CreateMaritalStatusDTOs(List<CompanyMaritalStatus> companyMaritalStatus)
        {
            var maritalStatusDTO = new List<MaritalStatusDTO>();
            foreach (CompanyMaritalStatus maritalStatus in companyMaritalStatus)
            {
                maritalStatusDTO.Add(CreateMaritalStatusDTO(maritalStatus));
            }
            return maritalStatusDTO;
        }

        #endregion

        #region DocumentType

        public static DocumentTypeDTO CreateDocumentType(CompanyDocumentType companyDocumentType)
        {
            var result = new DocumentTypeDTO();
            result.Id = companyDocumentType.Id;
            result.Description = companyDocumentType.Description;
            result.SmallDescription = companyDocumentType.SmallDescription;
            result.IsAlphanumeric = companyDocumentType.IsAlphanumeric;
            return result;
        }

        public static List<DocumentTypeDTO> CreateDocumentTypes(List<CompanyDocumentType> companyDocumentType)
        {
            var documentTypeDTO = new List<DocumentTypeDTO>();
            foreach (CompanyDocumentType documentType in companyDocumentType)
            {
                documentTypeDTO.Add(CreateDocumentType(documentType));
            }
            return documentTypeDTO;
        }

        #endregion

        #region AddressType

        public static AddressTypeDTO CreateAddressType(CompanyAddressType companyAddressType)
        {
            var result = new AddressTypeDTO();
            result.Id = companyAddressType.Id;
            result.Description = companyAddressType.Description;
            result.SmallDescription = companyAddressType.SmallDescription;
            return result;
        }

        public static List<AddressTypeDTO> CreateAddressTypes(List<CompanyAddressType> companyAddressType)
        {
            var addressTypeDTO = new List<AddressTypeDTO>();
            foreach (CompanyAddressType addressType in companyAddressType)
            {
                addressTypeDTO.Add(CreateAddressType(addressType));
            }
            return addressTypeDTO;
        }

        #endregion

        #region PhoneType

        public static PhoneTypeDTO CreatePhoneType(CompanyPhoneType companyPhoneType)
        {
            var result = new PhoneTypeDTO();
            result.Id = companyPhoneType.Id;
            result.Description = companyPhoneType.Description;
            result.SmallDescription = companyPhoneType.SmallDescription;
            return result;
        }

        public static List<PhoneTypeDTO> CreatePhoneTypes(List<CompanyPhoneType> companyPhoneType)
        {
            var phoneTypeDTO = new List<PhoneTypeDTO>();
            foreach (CompanyPhoneType phoneType in companyPhoneType)
            {
                phoneTypeDTO.Add(CreatePhoneType(phoneType));
            }
            return phoneTypeDTO;
        }

        #endregion

        #region EmailType

        public static EmailTypeDTO CreateEmailType(CompanyEmailType companyEmailType)
        {
            var result = new EmailTypeDTO();
            result.Id = companyEmailType.Id;
            result.Description = companyEmailType.Description;
            result.SmallDescription = companyEmailType.SmallDescription;
            return result;
        }

        public static List<EmailTypeDTO> CreateEmailTypes(List<CompanyEmailType> companyEmailType)
        {
            var emailTypeDTO = new List<EmailTypeDTO>();
            foreach (CompanyEmailType emailType in companyEmailType)
            {
                emailTypeDTO.Add(CreateEmailType(emailType));
            }
            return emailTypeDTO;
        }

        #endregion

        #region EconomicActivity

        public static EconomicActivityDTO CreateEconomicActivity(CompanyEconomicActivity companyEconomicActivity)
        {
            var result = new EconomicActivityDTO();
            result.Id = companyEconomicActivity.Id;
            result.Description = companyEconomicActivity.Description;
            result.SmallDescription = companyEconomicActivity.SmallDescription;
            return result;
        }

        public static List<EconomicActivityDTO> CreateEconomicActivities(List<CompanyEconomicActivity> companyEconomicActivity)
        {
            var economicActivityDTO = new List<EconomicActivityDTO>();
            foreach (CompanyEconomicActivity economicActivity in companyEconomicActivity)
            {
                economicActivityDTO.Add(CreateEconomicActivity(economicActivity));
            }
            return economicActivityDTO;
        }

        #endregion

        #region AssociationType

        public static AssociationTypeDTO CreateAssociationType(CompanyAssociationType companyAssociationType)
        {
            var result = new AssociationTypeDTO();
            result.Id = companyAssociationType.Id;
            result.Description = companyAssociationType.Description;
            result.SmallDescription = companyAssociationType.SmallDescription;
            return result;
        }

        public static List<AssociationTypeDTO> CreateAssociationTypes(List<CompanyAssociationType> companyAssociationType)
        {
            var associationTypeDTO = new List<AssociationTypeDTO>();
            foreach (CompanyAssociationType associationType in companyAssociationType)
            {
                associationTypeDTO.Add(CreateAssociationType(associationType));
            }
            return associationTypeDTO;
        }


        #endregion

        #region CompanyType

        public static CompanyTypeDTO CreateCompanyType(CompanyCompanyType companyCompanyType)
        {
            var result = new CompanyTypeDTO();
            result.Id = companyCompanyType.Id;
            result.Description = companyCompanyType.Description;
            result.SmallDescription = companyCompanyType.SmallDescription;
            return result;
        }

        public static List<CompanyTypeDTO> CreateCompanyTypes(List<CompanyCompanyType> companyCompanyType)
        {
            var companyTypeDTO = new List<CompanyTypeDTO>();
            foreach (CompanyCompanyType companyType in companyCompanyType)
            {
                companyTypeDTO.Add(CreateCompanyType(companyType));
            }
            return companyTypeDTO;
        }

        #endregion

        #region CreateCompanyCoInsured
        public static CompanyCoInsuredDTO CreateCompanyCoInsureds(MOCOV1.CompanyCoInsured companyCoInsured)
        {
            return new CompanyCoInsuredDTO
            {
                AddressTypeCode = companyCoInsured.AddressTypeCode,
                Annotations = companyCoInsured.Annotations,
                CityCode = companyCoInsured.CityCode,
                CountryCode = companyCoInsured.CountryCode,
                Description = companyCoInsured.Description,
                EnsureInd = companyCoInsured.EnsureInd,
                EnteredDate = companyCoInsured.EnteredDate,
                InsuraceCompanyId = companyCoInsured.InsuraceCompanyId,
                ModifyDate = companyCoInsured.ModifyDate,
                PhoneNumber = companyCoInsured.PhoneNumber,
                PhoneTypeCode = companyCoInsured.PhoneTypeCode,
                StateCode = companyCoInsured.StateCode,
                Street = companyCoInsured.Street,
                TributaryIdNo = companyCoInsured.TributaryIdNo,
                ComDeclinedTypeCode = companyCoInsured.ComDeclinedTypeCode,
                DeclinedDate = companyCoInsured.DeclinedDate,
                IndividualId = companyCoInsured.IndividualId
            };
        }
        #endregion

        #region LabourPerson
        public static PersonInformationAndLabourDTO CreateLabourPerson(CompanyLabourPerson labourPerson)
        {
            return new PersonInformationAndLabourDTO
            {
                Id = labourPerson.IndividualId,
                IndividualId = labourPerson.IndividualId,
                PersonType = labourPerson.PersonType?.Id,
                IncomeLevel = labourPerson.IncomeLevel?.Id,
                EducativeLevel = labourPerson.EducativeLevel?.Id,
                CompanyName = labourPerson.CompanyName,
                Children = labourPerson.Children,
                SpouseName = labourPerson.SpouseName,
                Occupation = labourPerson.Occupation?.Id,
                Position = labourPerson.Position,
                Contact = labourPerson.Contact,
                OtherOccupation = labourPerson.OtherOccupation?.Id,
                CompanyPhone = labourPerson.CompanyPhone?.Id,
                Speciality = labourPerson.Speciality?.Id,
                HouseType = labourPerson.HouseType?.Id,
                JobSector = labourPerson.JobSector,
                SocialLayer = labourPerson.SocialLayer?.Id,
                BirthCountryId = labourPerson.BirthCountryId,
                PersonInterestGroup = CreatePersonInterestGroups(labourPerson.PersonInterestGroup, labourPerson.IndividualId)
            };
        }


        public static List<PersonInterestGroupDTO> CreatePersonInterestGroups(List<CompanyPersonInterestGroup> groups, int individualid)
        {
            var gruposDTO = new List<PersonInterestGroupDTO>();
            foreach (CompanyPersonInterestGroup field in groups)
            {
                gruposDTO.Add(CreatePersonInterestGroup(field, individualid));
            }
            return gruposDTO;
        }

        public static PersonInterestGroupDTO CreatePersonInterestGroup(CompanyPersonInterestGroup personinterestgroup, int individualid)
        {
            var result = new PersonInterestGroupDTO();
            result.IndividualId = individualid;
            result.InterestGroupTypeId = personinterestgroup.InterestGroupTypeId;
            return result;
        }
        #endregion

        #region Insured

        public static InsuredDeclinedTypeDTO CreateInsuredDeclinedType(CompanyInsuredDeclinedType companyInsuredDeclinedType)
        {
            var result = new InsuredDeclinedTypeDTO();
            result.Id = companyInsuredDeclinedType.Id;
            result.Description = companyInsuredDeclinedType.Description;
            result.SmallDescription = companyInsuredDeclinedType.SmallDescription;
            return result;
        }

        public static List<InsuredDeclinedTypeDTO> CreateInsuredDeclinedTypes(List<CompanyInsuredDeclinedType> companyInsuredDeclinedType)
        {
            var insuredDeclinedTypeDTO = new List<InsuredDeclinedTypeDTO>();
            foreach (CompanyInsuredDeclinedType insuredDeclinedType in companyInsuredDeclinedType)
            {
                insuredDeclinedTypeDTO.Add(CreateInsuredDeclinedType(insuredDeclinedType));
            }
            return insuredDeclinedTypeDTO;
        }

        public static InsuredSegmentDTO CreateInsuredSegment(CompanyInsuredSegment companyInsuredSegment)
        {
            var result = new InsuredSegmentDTO();
            result.IndividualId = companyInsuredSegment.Id;
            result.Description = companyInsuredSegment.Description;
            return result;
        }

        public static List<InsuredSegmentDTO> CreateInsuredSegments(List<CompanyInsuredSegment> companyInsuredSegment)
        {
            var insuredSegmentDTO = new List<InsuredSegmentDTO>();
            foreach (CompanyInsuredSegment insuredSegment in companyInsuredSegment)
            {
                insuredSegmentDTO.Add(CreateInsuredSegment(insuredSegment));
            }
            return insuredSegmentDTO;
        }

        public static InsuredProfileDTO CreateInsuredProfile(CompanyInsuredProfile companyInsuredProfile)
        {
            var result = new InsuredProfileDTO();
            result.IndividualId = companyInsuredProfile.Id;
            result.Description = companyInsuredProfile.Description;
            return result;
        }

        public static List<InsuredProfileDTO> CreateInsuredProfiles(List<CompanyInsuredProfile> companyInsuredProfile)
        {
            var insuredProfileDTO = new List<InsuredProfileDTO>();
            foreach (CompanyInsuredProfile insuredProfile in companyInsuredProfile)
            {
                insuredProfileDTO.Add(CreateInsuredProfile(insuredProfile));
            }
            return insuredProfileDTO;
        }

        #endregion

        #region AgentType

        public static AgentTypeDTO CreateAgentType(CompanyAgentType companyAgentType)
        {
            var result = new AgentTypeDTO();
            result.Id = companyAgentType.Id;
            result.Description = companyAgentType.Description;
            result.SmallDescription = companyAgentType.SmallDescription;
            return result;
        }

        public static List<AgentTypeDTO> CreateAgentTypes(List<CompanyAgentType> companyAgentType)
        {
            var agentTypeDTO = new List<AgentTypeDTO>();
            foreach (CompanyAgentType agentType in companyAgentType)
            {
                agentTypeDTO.Add(CreateAgentType(agentType));
            }
            return agentTypeDTO;
        }

        #endregion

        #region AgentDeclinedType

        public static AgentDeclinedTypeDTO CreateAgentDeclinedType(CompanyAgentDeclinedType companyAgentDeclinedType)
        {
            var result = new AgentDeclinedTypeDTO();
            result.Id = companyAgentDeclinedType.Id;
            result.Description = companyAgentDeclinedType.Description;
            result.SmallDescription = companyAgentDeclinedType.SmallDescription;
            return result;
        }

        public static List<AgentDeclinedTypeDTO> CreateAgentDeclinedTypes(List<CompanyAgentDeclinedType> companyAgentDeclinedType)
        {
            var agentDeclinedTypeDTO = new List<AgentDeclinedTypeDTO>();
            foreach (CompanyAgentDeclinedType agentDeclinedType in companyAgentDeclinedType)
            {
                agentDeclinedTypeDTO.Add(CreateAgentDeclinedType(agentDeclinedType));
            }
            return agentDeclinedTypeDTO;
        }

        #endregion

        #region GroupAgent

        public static GroupAgentDTO CreateGroupAgent(CompanyGroupAgent companyGroupAgent)
        {
            var result = new GroupAgentDTO();
            result.Id = companyGroupAgent.Id;
            result.Description = companyGroupAgent.Description;
            result.SmallDescription = companyGroupAgent.SmallDescription;
            return result;
        }

        public static List<GroupAgentDTO> CreateGroupAgents(List<CompanyGroupAgent> companyGroupAgent)
        {
            var groupAgentDTO = new List<GroupAgentDTO>();
            foreach (CompanyGroupAgent groupAgent in companyGroupAgent)
            {
                groupAgentDTO.Add(CreateGroupAgent(groupAgent));
            }
            return groupAgentDTO;
        }

        #endregion

        #region SalesChannel

        public static SalesChannelDTO CreateSalesChannel(CompanySalesChannel companySalesChannel)
        {
            var result = new SalesChannelDTO();
            result.Id = companySalesChannel.Id;
            result.Description = companySalesChannel.Description;
            result.SmallDescription = companySalesChannel.SmallDescription;
            return result;
        }

        public static List<SalesChannelDTO> CreateSalesChannels(List<CompanySalesChannel> companySalesChannel)
        {
            var salesChannelDTO = new List<SalesChannelDTO>();
            foreach (CompanySalesChannel salesChannel in companySalesChannel)
            {
                salesChannelDTO.Add(CreateSalesChannel(salesChannel));
            }
            return salesChannelDTO;
        }

        #endregion

        #region EmployeePerson

        public static EmployeePersonDTO CreateEmployeePerson(CompanyEmployeePerson companyEmployeePerson)
        {
            var result = new EmployeePersonDTO();
            result.Id = companyEmployeePerson.Id;
            result.Description = companyEmployeePerson.Description;
            result.Name = companyEmployeePerson.Name;
            result.MotherLastName = companyEmployeePerson.MotherLastName;
            result.IdCardNo = companyEmployeePerson.IdCardNo;
            return result;
        }

        public static List<EmployeePersonDTO> CreateEmployeePersons(List<CompanyEmployeePerson> companyEmployeePerson)
        {
            var employeePersonDTO = new List<EmployeePersonDTO>();
            foreach (CompanyEmployeePerson employeePerson in companyEmployeePerson)
            {
                employeePersonDTO.Add(CreateEmployeePerson(employeePerson));
            }
            return employeePersonDTO;
        }

        #endregion

        #region AllOthersDeclinedType

        public static AllOthersDeclinedTypeDTO CreateAllOthersDeclinedType(CompanyAllOthersDeclinedType companyAllOthersDeclinedType)
        {
            var result = new AllOthersDeclinedTypeDTO();
            result.Id = companyAllOthersDeclinedType.Id;
            result.Description = companyAllOthersDeclinedType.Description;
            result.SmallDescription = companyAllOthersDeclinedType.SmallDescription;
            result.RoleCd = companyAllOthersDeclinedType.RoleCd;
            return result;
        }

        public static List<AllOthersDeclinedTypeDTO> CreateAllOthersDeclinedTypes(List<CompanyAllOthersDeclinedType> companyAllOthersDeclinedType)
        {
            var allOthersDeclinedTypeDTO = new List<AllOthersDeclinedTypeDTO>();
            foreach (CompanyAllOthersDeclinedType allOthersDeclinedType in companyAllOthersDeclinedType)
            {
                allOthersDeclinedTypeDTO.Add(CreateAllOthersDeclinedType(allOthersDeclinedType));
            }
            return allOthersDeclinedTypeDTO;
        }

        #endregion

        #region insured Guarantee

        internal static List<GuaranteeInsuredGuaranteeDTO> CreateGuaranteeInsuredGuarantees(List<CompanyGuaranteeInsuredGuarantee> list)
        {
            var GuaranteeInsuredGuaranteeDTOs = new List<GuaranteeInsuredGuaranteeDTO>();
            foreach (CompanyGuaranteeInsuredGuarantee CompanyGuaranteeInsuredGuarantee in list)
            {
                GuaranteeInsuredGuaranteeDTOs.Add(CreateCreateGuaranteeInsuredGuarantee(CompanyGuaranteeInsuredGuarantee));
            }
            return GuaranteeInsuredGuaranteeDTOs;
        }

        private static GuaranteeInsuredGuaranteeDTO CreateCreateGuaranteeInsuredGuarantee(CompanyGuaranteeInsuredGuarantee companyGuaranteeInsuredGuarantee)
        {
            return new GuaranteeInsuredGuaranteeDTO()
            {
                Id = companyGuaranteeInsuredGuarantee.Id,
                Description = companyGuaranteeInsuredGuarantee.Description,
                IndividualId = companyGuaranteeInsuredGuarantee.IndividualId,
                typeId = companyGuaranteeInsuredGuarantee.typeId
            };
        }

        internal static InsuredGuaranteeMortgageDTO CreateInsuredGuaranteeMortgage(CompanyInsuredGuaranteeMortgage companyInsuredGuaranteeMortgage)
        {
            return new InsuredGuaranteeMortgageDTO()
            {
                Id = companyInsuredGuaranteeMortgage.Id,
                IndividualId = companyInsuredGuaranteeMortgage.IndividualId,
                Address = companyInsuredGuaranteeMortgage.Address,
                AppraisalAmount = companyInsuredGuaranteeMortgage.AppraisalAmount,
                AppraisalDate = companyInsuredGuaranteeMortgage.AppraisalDate,
                AssetType = new SelectDTO()
                {
                    Id = companyInsuredGuaranteeMortgage.AssetType.Code,
                    Description = companyInsuredGuaranteeMortgage.AssetType.Description
                },
                Branch = new SelectDTO()
                {
                    Id = companyInsuredGuaranteeMortgage.Branch.Id,
                    Description = companyInsuredGuaranteeMortgage.Branch.Description
                },
                BuiltAreaQuantity = companyInsuredGuaranteeMortgage.BuiltAreaQuantity,
                City = new SelectDTO()
                {
                    Id = companyInsuredGuaranteeMortgage.City.Id,
                    Description = companyInsuredGuaranteeMortgage.City.Description
                },
                State = new SelectDTO()
                {
                    Id = companyInsuredGuaranteeMortgage.City.State.Id,
                    Description = companyInsuredGuaranteeMortgage.City.State.Description
                },
                Country = new SelectDTO()
                {
                    Id = companyInsuredGuaranteeMortgage.City.State.Country.Id,
                    Description = companyInsuredGuaranteeMortgage.City.State.Country.Description
                },
                ClosedInd = companyInsuredGuaranteeMortgage.ClosedInd,
                Currency = new SelectDTO()
                {
                    Id = companyInsuredGuaranteeMortgage.Currency.Id,
                    Description = companyInsuredGuaranteeMortgage.Currency.Description
                },
                Description = companyInsuredGuaranteeMortgage.Description,
                ExpertName = companyInsuredGuaranteeMortgage.ExpertName,
                Guarantee = new GuaranteeDTO()
                {
                    Id = companyInsuredGuaranteeMortgage.Guarantee.Id,
                    Description = companyInsuredGuaranteeMortgage.Guarantee.Description,
                    GuaranteeType = new SelectDTO()
                    {
                        Id = companyInsuredGuaranteeMortgage.Guarantee.Type.Id,
                        Description = companyInsuredGuaranteeMortgage.Guarantee.Type.Description
                    },
                    HasApostille = companyInsuredGuaranteeMortgage.Guarantee.HasApostille,
                    HasPromissoryNote = companyInsuredGuaranteeMortgage.Guarantee.HasPromissoryNote
                },
                InsuranceCompany = new SelectDTO()
                {
                    Id = Convert.ToInt32(companyInsuredGuaranteeMortgage.InsuranceCompanyId),
                    Description = companyInsuredGuaranteeMortgage.InsuranceCompany
                },
                InsuranceValueAmount = companyInsuredGuaranteeMortgage.InsuranceValueAmount,
                LastChangeDate = Convert.ToDateTime(companyInsuredGuaranteeMortgage.LastChangeDate),
                MeasureAreaQuantity = companyInsuredGuaranteeMortgage.MeasureAreaQuantity,
                MeasurementType = new SelectDTO()
                {
                    Id = companyInsuredGuaranteeMortgage.MeasurementType.Id,
                    Description = companyInsuredGuaranteeMortgage.MeasurementType.Description
                },
                PolicyNumber = companyInsuredGuaranteeMortgage.PolicyNumber,
                RegistrationDate = Convert.ToDateTime(companyInsuredGuaranteeMortgage.RegistrationDate),
                RegistrationNumber = companyInsuredGuaranteeMortgage.RegistrationNumber,
                Status = new SelectDTO()
                {
                    Id = companyInsuredGuaranteeMortgage.Status.Id,
                    Description = companyInsuredGuaranteeMortgage.Status.Description
                }


            };
        }

        internal static InsuredGuaranteePledgeDTO CreateInsuredGuaranteePledge(CompanyInsuredGuaranteePledge companyInsuredGuaranteePledge)
        {
            return new InsuredGuaranteePledgeDTO()
            {
                Id = companyInsuredGuaranteePledge.Id,
                IndividualId = companyInsuredGuaranteePledge.IndividualId,

                AppraisalAmount = companyInsuredGuaranteePledge.AppraisalAmount,
                AppraisalDate = companyInsuredGuaranteePledge.AppraisalDate,
                Branch = new SelectDTO()
                {
                    Id = companyInsuredGuaranteePledge.Branch.Id,
                    Description = companyInsuredGuaranteePledge.Branch.Description
                },
                ChassisNumer = companyInsuredGuaranteePledge.ChassisNumer,
                City = new SelectDTO()
                {
                    Id = companyInsuredGuaranteePledge.City.Id,
                    Description = companyInsuredGuaranteePledge.City.Description
                },
                Country = new SelectDTO()
                {
                    Id = companyInsuredGuaranteePledge.City.State.Country.Id,
                    Description = companyInsuredGuaranteePledge.City.State.Country.Description
                },
                ClosedInd = companyInsuredGuaranteePledge.ClosedInd,
                Currency = new SelectDTO()
                {
                    Id = companyInsuredGuaranteePledge.Currency.Id,
                    Description = companyInsuredGuaranteePledge.Currency.Description
                },
                Description = companyInsuredGuaranteePledge.Description,
                EngineNumer = companyInsuredGuaranteePledge.EngineNumer,
                Guarantee = new GuaranteeDTO()
                {
                    Id = companyInsuredGuaranteePledge.Guarantee.Id,
                    Description = companyInsuredGuaranteePledge.Guarantee.Description,
                    GuaranteeType = new SelectDTO()
                    {
                        Id = companyInsuredGuaranteePledge.Guarantee.Type.Id,
                        Description = companyInsuredGuaranteePledge.Guarantee.Type.Description
                    },
                    HasApostille = companyInsuredGuaranteePledge.Guarantee.HasApostille,
                    HasPromissoryNote = companyInsuredGuaranteePledge.Guarantee.HasPromissoryNote
                },
                InsuranceCompany = new SelectDTO()
                {
                    Id = Convert.ToInt32(companyInsuredGuaranteePledge.InsuranceCompanyId),
                    Description = companyInsuredGuaranteePledge.InsuranceCompany
                },
                InsuranceValueAmount = companyInsuredGuaranteePledge.InsuranceValueAmount,
                LastChangeDate = Convert.ToDateTime(companyInsuredGuaranteePledge.LastChangeDate),
                LicensePlate = companyInsuredGuaranteePledge.LicensePlate,
                PolicyNumber = companyInsuredGuaranteePledge.PolicyNumber,
                RegistrationDate = Convert.ToDateTime(companyInsuredGuaranteePledge.RegistrationDate),
                Status = new SelectDTO()
                {
                    Id = companyInsuredGuaranteePledge.Status.Id,
                    Description = companyInsuredGuaranteePledge.Status.Description
                },
                State = new SelectDTO()
                {
                    Id = companyInsuredGuaranteePledge.City.State.Id,
                    Description = companyInsuredGuaranteePledge.City.State.Description
                },
            };
        }

        internal static List<InsuredGuaranteePrefixDTO> CreateInsuredGuaranteePrefixes(List<CompanyInsuredGuaranteePrefix> list)
        {
            List<InsuredGuaranteePrefixDTO> insuredGuaranteePrefixDTOs = new List<InsuredGuaranteePrefixDTO>();

            foreach (CompanyInsuredGuaranteePrefix item in list)
            {
                insuredGuaranteePrefixDTOs.Add(CreateInsuredGuaranteePrefix(item));
            }
            return insuredGuaranteePrefixDTOs;
        }

        internal static InsuredGuaranteePromissoryNoteDTO CreateInsuredGuaranteePromissoryNote(CompanyInsuredGuaranteePromissoryNote companyInsuredGuaranteePromissoryNote)
        {
            return new InsuredGuaranteePromissoryNoteDTO()
            {
                Id = companyInsuredGuaranteePromissoryNote.Id,
                IndividualId = companyInsuredGuaranteePromissoryNote.IndividualId,
                Branch = new SelectDTO()
                {
                    Id = companyInsuredGuaranteePromissoryNote.Branch.Id,
                    Description = companyInsuredGuaranteePromissoryNote.Branch.Description
                },
                City = new SelectDTO()
                {
                    Id = companyInsuredGuaranteePromissoryNote.City.Id,
                    Description = companyInsuredGuaranteePromissoryNote.City.Description
                },
                Country = new SelectDTO()
                {
                    Id = companyInsuredGuaranteePromissoryNote.City.State.Country.Id,
                    Description = companyInsuredGuaranteePromissoryNote.City.State.Country.Description
                },
                ClosedInd = companyInsuredGuaranteePromissoryNote.ClosedInd,
                Currency = new SelectDTO()
                {
                    Id = companyInsuredGuaranteePromissoryNote.Currency.Id,
                    Description = companyInsuredGuaranteePromissoryNote.Currency.Description
                },
                Description = companyInsuredGuaranteePromissoryNote.Description,
                RegistrationDate = Convert.ToDateTime(companyInsuredGuaranteePromissoryNote.RegistrationDate),
                Status = new SelectDTO()
                {
                    Id = companyInsuredGuaranteePromissoryNote.Status.Id,
                    Description = companyInsuredGuaranteePromissoryNote.Status.Description
                },
                State = new SelectDTO()
                {
                    Id = companyInsuredGuaranteePromissoryNote.City.State.Id,
                    Description = companyInsuredGuaranteePromissoryNote.City.State.Description
                },
                LastChangeDate = Convert.ToDateTime(companyInsuredGuaranteePromissoryNote.LastChangeDate),
                Guarantee = new GuaranteeDTO()
                {
                    Id = companyInsuredGuaranteePromissoryNote.Guarantee.Id,
                    Description = companyInsuredGuaranteePromissoryNote.Guarantee.Description,
                    GuaranteeType = new SelectDTO()
                    {
                        Id = companyInsuredGuaranteePromissoryNote.Guarantee.Type.Id,
                        Description = companyInsuredGuaranteePromissoryNote.Guarantee.Type.Description
                    }
                },
                PromissoryNoteType = new SelectDTO()
                {
                    Id = companyInsuredGuaranteePromissoryNote.PromissoryNoteType.Id
                },
                ConstitutionDate = Convert.ToDateTime(companyInsuredGuaranteePromissoryNote.ConstitutionDate),
                DocumentNumber = companyInsuredGuaranteePromissoryNote.DocumentNumber,
                DocumentValueAmount = companyInsuredGuaranteePromissoryNote.DocumentValueAmount,
                ExtDate = Convert.ToDateTime(companyInsuredGuaranteePromissoryNote.ExtDate),
                SignatoriesNumber = companyInsuredGuaranteePromissoryNote.SignatoriesNumber,
            };
        }

        internal static InsuredGuaranteeFixedTermDepositDTO CreateInsuredGuaranteeFixedTermDeposit(CompanyInsuredGuaranteeFixedTermDeposit companyInsuredGuaranteeFixedTermDeposit)
        {
            return new InsuredGuaranteeFixedTermDepositDTO()
            {
                Id = companyInsuredGuaranteeFixedTermDeposit.Id,
                IndividualId = companyInsuredGuaranteeFixedTermDeposit.IndividualId,
                Branch = new SelectDTO()
                {
                    Id = companyInsuredGuaranteeFixedTermDeposit.Branch.Id,
                    Description = companyInsuredGuaranteeFixedTermDeposit.Branch.Description
                },
                City = new SelectDTO()
                {
                    Id = companyInsuredGuaranteeFixedTermDeposit.City.Id,
                    Description = companyInsuredGuaranteeFixedTermDeposit.City.Description
                },
                Country = new SelectDTO()
                {
                    Id = companyInsuredGuaranteeFixedTermDeposit.City.State.Country.Id,
                    Description = companyInsuredGuaranteeFixedTermDeposit.City.State.Country.Description
                },
                ClosedInd = companyInsuredGuaranteeFixedTermDeposit.ClosedInd,
                Currency = new SelectDTO()
                {
                    Id = companyInsuredGuaranteeFixedTermDeposit.Currency.Id,
                    Description = companyInsuredGuaranteeFixedTermDeposit.Currency.Description
                },
                Description = companyInsuredGuaranteeFixedTermDeposit.Description,
                RegistrationDate = Convert.ToDateTime(companyInsuredGuaranteeFixedTermDeposit.RegistrationDate),
                Status = new SelectDTO()
                {
                    Id = companyInsuredGuaranteeFixedTermDeposit.Status.Id,
                    Description = companyInsuredGuaranteeFixedTermDeposit.Status.Description
                },
                State = new SelectDTO()
                {
                    Id = companyInsuredGuaranteeFixedTermDeposit.City.State.Id,
                    Description = companyInsuredGuaranteeFixedTermDeposit.City.State.Description
                },
                LastChangeDate = Convert.ToDateTime(companyInsuredGuaranteeFixedTermDeposit.LastChangeDate),
                Guarantee = new GuaranteeDTO()
                {
                    Id = companyInsuredGuaranteeFixedTermDeposit.Guarantee.Id,
                    Description = companyInsuredGuaranteeFixedTermDeposit.Guarantee.Description,
                    GuaranteeType = new SelectDTO()
                    {
                        Id = companyInsuredGuaranteeFixedTermDeposit.Guarantee.Type.Id,
                        Description = companyInsuredGuaranteeFixedTermDeposit.Guarantee.Type.Description
                    }
                },
                ConstitutionDate = Convert.ToDateTime(companyInsuredGuaranteeFixedTermDeposit.ConstitutionDate),
                DocumentNumber = companyInsuredGuaranteeFixedTermDeposit.DocumentNumber,
                DocumentValueAmount = companyInsuredGuaranteeFixedTermDeposit.DocumentValueAmount,
                ExtDate = Convert.ToDateTime(companyInsuredGuaranteeFixedTermDeposit.ExtDate),
                IssuerName = companyInsuredGuaranteeFixedTermDeposit.IssuerName
            };
        }

        internal static InsuredGuaranteeOthersDTO CreateInsuredGuaranteeOthers(CompanyInsuredGuaranteeOthers companyInsuredGuaranteeOthers)
        {
            return new InsuredGuaranteeOthersDTO()
            {
                Branch = new SelectDTO()
                {
                    Id = companyInsuredGuaranteeOthers.Branch.Id,
                    Description = companyInsuredGuaranteeOthers.Description
                },
                ClosedInd = companyInsuredGuaranteeOthers.ClosedInd,
                DescriptionOthers = companyInsuredGuaranteeOthers.DescriptionOthers,
                Id = companyInsuredGuaranteeOthers.Id,
                Guarantee = new GuaranteeDTO()
                {
                    Id = companyInsuredGuaranteeOthers.Guarantee.Id,
                    Description = companyInsuredGuaranteeOthers.Guarantee.Description,
                    GuaranteeType = new SelectDTO()
                    {
                        Id = companyInsuredGuaranteeOthers.Guarantee.Type.Id,
                        Description = companyInsuredGuaranteeOthers.Guarantee.Type.Description
                    }
                },
                Status = new SelectDTO()
                {
                    Id = companyInsuredGuaranteeOthers.Status.Id,
                    Description = companyInsuredGuaranteeOthers.Status.Description
                },
                IndividualId = companyInsuredGuaranteeOthers.IndividualId,
                LastChangeDate = Convert.ToDateTime(companyInsuredGuaranteeOthers.LastChangeDate),
                RegistrationDate = Convert.ToDateTime(companyInsuredGuaranteeOthers.RegistrationDate)
            };
        }
        #endregion

        #region GuaranteeRequiredDocument

        internal static GuaranteeRequiredDocumentDTO CreateGuaranteeRequiredDocument(CompanyGuaranteeRequiredDocument companyGuaranteeRequiredDocument)
        {
            return new GuaranteeRequiredDocumentDTO()
            {
                GuaranteeCode = companyGuaranteeRequiredDocument.GuaranteeCode,
                DocumentCode = companyGuaranteeRequiredDocument.DocumentCode,
                Description = companyGuaranteeRequiredDocument.Description
            };

        }

        #endregion

        #region Prospect

        public static ProspectLigthQuotationDTO CreateProspectLigthQuotation(CompanyProspectNatural prospectNatural)
        {
            if (prospectNatural == null)
            {
                return null;
            }

            return new ProspectLigthQuotationDTO
            {
                BirthDate = prospectNatural.BirthDate,
                Address = prospectNatural.Street,
                EmailAddress = prospectNatural.EmailAddress,
                Gender = prospectNatural.Gender,
                CardDescription = prospectNatural.IdCardNo,
                CardId = (int)prospectNatural.IdCardTypeCode,
                Name = prospectNatural.Name,
                PhoneNumber = prospectNatural.PhoneNumber?.ToString(),
                ProspectCode = prospectNatural.ProspectCode,
                SurName = prospectNatural.Surname,
                MotherLastName = prospectNatural.MotherLastName,
                IndividualTypePerson = prospectNatural.IndividualTypePerson
            };
        }
        #endregion

        #region Better Performance
        public static List<CurrencyDTO> CreateCurrencies(List<Currency> currencies)
        {
            List<CurrencyDTO> currenciesDTO = new List<CurrencyDTO>();

            if (currencies != null && currencies.Count > 0)
            {
                foreach (var currency in currencies)
                {
                    currenciesDTO.Add(CreateCurrency(currency));
                }
            }
            return currenciesDTO;
        }

        public static CurrencyDTO CreateCurrency(Currency currency)
        {
            if (currency == null)
            {
                return null;
            }

            return new CurrencyDTO
            {
                Id = currency.Id,
                Description = currency.Description,
                SmallDescription = currency.SmallDescription
            };
        }

        public static List<ExonerationTypeDTO> CreateExonerationTypes(List<ExonerationType> exonerationTypes)
        {
            List<ExonerationTypeDTO> exonerationTypesDTO = new List<ExonerationTypeDTO>();
            if (exonerationTypes != null && exonerationTypes.Count > 0)
            {
                foreach (var exonerationType in exonerationTypes)
                {
                    exonerationTypesDTO.Add(CreateExonerationType(exonerationType));
                }
            }
            return exonerationTypesDTO;
        }

        public static ExonerationTypeDTO CreateExonerationType(ExonerationType exonerationType)
        {
            if (exonerationType == null)
            {
                return null;
            }

            return new ExonerationTypeDTO
            {
                Id = exonerationType.Id,
                Description = exonerationType.Description,
                SmallDescription = exonerationType.SmallDescription
            };
        }

        public static List<GenderDTO> CreateGenders()
        {
            List<GenderDTO> genders = new List<GenderDTO>();

            foreach (var item in Enum.GetValues(typeof(GenderType)))
            {
                genders.Add(new GenderDTO()
                {
                    Id = (int)item,
                    Description = Errors.ResourceManager.GetString(item.ToString())
                });
            }
            return genders;
        }


        public static List<AddressTypeDTO> CreateAddressTypesbyEmail(List<GenericModelServicesQueryModel> list)
        {
            List<AddressTypeDTO> adressTypesDTO = new List<AddressTypeDTO>();
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    adressTypesDTO.Add(CreateAddressTypebyEmail(item));
                }
            }
            return adressTypesDTO;
        }

        public static AddressTypeDTO CreateAddressTypebyEmail(GenericModelServicesQueryModel item)
        {
            if (item == null)
            {
                return null;
            }

            return new AddressTypeDTO
            {
                Id = item.id,
                Description = item.description,
                SmallDescription = item.smallDescription
            };
        }
        #endregion

        #region ThirdPerson
        /// <summary>
        /// Crear un tercero
        /// </summary>
        /// <param name="third">The insured.</param>
        /// <returns></returns>
        public static ThirdPartyDTO CreateThird(MOCOV1.CompanyThird third)
        {
            var result = new ThirdPartyDTO();

            if (third != null)
            {

                if (third.EnteredDate != null)
                {
                    result.CreationDate = (DateTime)third.EnteredDate;
                }
                result.DeclinationDate = third.DeclinedDate;
                result.Id = third.Id;
                result.IndividualId = third.IndividualId;
                result.ModificationDate = third.ModificationDate;
                result.Annotation = third.Annotation;
                result.DeclinedTypeId = third.DeclinedTypeId;



            }


            return result;
        }
        #endregion

        #region ThirdPerson
        /// <summary>
        /// Crear un tercero
        /// </summary>
        /// <param name="third">The insured.</param>
        /// <returns></returns>
        public static EmployeeDTO CreateEmployee(MOCOV1.CompanyEmployee employee)
        {
            var result = new EmployeeDTO();

            if (employee != null)
            {
                result.BranchId = employee.BranchId;
                result.IndividualId = employee.IndividualId;
                result.EntryDate = employee.EntryDate;
                result.EgressDate = employee.EgressDate;
                result.Annotation = employee.Annotation;
                result.ModificationDate = employee.ModificationDate;
                result.DeclinedTypeId = employee.DeclinedTypeId;
                result.FileNumber = employee.FileNumber;
            }


            return result;
        }
        #endregion
        #region BusinessName
        public static CompanyNameDTO CreateBusinessNameDTO(CompanyName companyCoCompanyName)
        {
            var result = new CompanyNameDTO();
            result.IndividualId = companyCoCompanyName.IndividualId;
            result.NameNum = companyCoCompanyName.NameNum;
            result.TradeName = companyCoCompanyName.TradeName;
            result.IsMain = companyCoCompanyName.IsMain;
            result.Enabled = companyCoCompanyName.Enabled;
            if (companyCoCompanyName.Phone != null)
            {
                result.PhoneID = companyCoCompanyName.Phone.Id;
            }
            if (companyCoCompanyName.Address != null)
            {
                result.AddressID = companyCoCompanyName.Address.Id;
            }
            if (companyCoCompanyName.Email != null)
            {
                result.EmailID = companyCoCompanyName.Email.Id;
            }

            return result;

        }


        /// <summary>
        /// Convierte una lista de IndividualTaxExeption a IndividualTaxExeptionDTO
        /// </summary>
        /// <param name="listIndividualTaxExeptionDTOModel"></param>
        /// <returns></returns>
        public static List<CompanyNameDTO> CreateIndividualCompanyBusinessDTOs(List<CompanyName> listIndividualCompanyBusiness)
        {
            List<CompanyNameDTO> individualCompanyBusinessDTOs = new List<CompanyNameDTO>();
            foreach (CompanyName IndividualCompanyName in listIndividualCompanyBusiness)
            {
                individualCompanyBusinessDTOs.Add(CreateBusinessNameDTO(IndividualCompanyName));
            }
            return individualCompanyBusinessDTOs;
        }


        #endregion

        #region BankTransfers
        public static BankTransfersDTO CreateBankTransfersDTO(BankTransfers bankTransfers)
        {
            var result = new BankTransfersDTO();

            result.Id = bankTransfers.Id;
            result.IndividualId = bankTransfers.Individual;
            result.IntermediaryBank = bankTransfers.IntermediaryBank;
            result.PaymentBeneficiary = bankTransfers.PaymentBeneficiary;
            result.DefaultAccount = bankTransfers.DefaultAccount;
            result.AccountNumber = bankTransfers.AccountNumber;
            result.AccountTypeId = bankTransfers.AccountType.Code;
            result.ActiveAccount = bankTransfers.ActiveAccount;
            result.BankSquare = bankTransfers.BankSquare;
            result.BankId = bankTransfers.Bank.Id;
            result.BankDescription = bankTransfers.Bank.Description;
            result.BankBranch = bankTransfers.BankBranch;
            result.CurrencyId = bankTransfers.Currency.Id;
            result.CurrencyDescription = bankTransfers.Currency.Description;
            result.InscriptionDate = bankTransfers.InscriptionDate;

            return result;

        }

        public static List<BankTransfersDTO> CreateBankTransfersDTO(List<BankTransfers> listBankTransfers)
        {
            List<BankTransfersDTO> bankTransfers = new List<BankTransfersDTO>();
            foreach (BankTransfers bank in listBankTransfers)
            {
                bankTransfers.Add(CreateBankTransfersDTO(bank));
            }

            return bankTransfers;

        }



        #region ElectronicBilling
        public static InsuredFiscalResponsibilityDTO CreateInsuredFiscalResponsibilityDTO(InsuredFiscalResponsibility fiscalResponsibility)
        {
            var result = new InsuredFiscalResponsibilityDTO();

            result.Id = fiscalResponsibility.Id;
            result.IndividualId = fiscalResponsibility.IndividualId;
            result.InsuredCode = fiscalResponsibility.InsuredId;
            result.FiscalResponsibilityId = fiscalResponsibility.FiscalResponsabilityId;
            result.FiscalResponsibilityDescription = fiscalResponsibility.FiscalResponsabilityDescription;
            result.Code = fiscalResponsibility.Code;
            return result;

        }

        public static List<InsuredFiscalResponsibilityDTO> CreateListInsuredFiscalResponsibilityDTO(List<InsuredFiscalResponsibility> listFiscalResponsibilities)
        {
            List<InsuredFiscalResponsibilityDTO> fiscalResponsibilities = new List<InsuredFiscalResponsibilityDTO>();
            foreach (InsuredFiscalResponsibility fiscal in listFiscalResponsibilities)
            {
                fiscalResponsibilities.Add(CreateInsuredFiscalResponsibilityDTO(fiscal));
            }

            return fiscalResponsibilities;

        }
        #endregion

        /// <summary>
        /// Convierte una lista de BankTRansfers a BankTransfersDTO
        /// </summary>
        /// <returns></returns>
        public static List<BankTransfersDTO> CreateBankTransfersDTOs(List<BankTransfers> listBankTransfers)
        {
            List<BankTransfersDTO> bankTransfersDTOs = new List<BankTransfersDTO>();
            foreach (BankTransfers BankTransfer in listBankTransfers)
            {
                bankTransfersDTOs.Add(CreateBankTransfersDTO(BankTransfer));
            }
            return bankTransfersDTOs;
        }


        #endregion BankTransfers

        #region CompanyFiscalResponsibility
        public static FiscalResponsibilityDTO CreateCompanyFiscalResponsibility(CompanyFiscalResponsibility companyFiscalResponsibility)
        {
            var result = new FiscalResponsibilityDTO();
            result.Id = companyFiscalResponsibility.Id;
            result.Code = companyFiscalResponsibility.Code;
            result.Description = companyFiscalResponsibility.Description;
            return result;
        }

        public static List<FiscalResponsibilityDTO> CreateCompanyFiscalResponsibilities(List<CompanyFiscalResponsibility> companyFiscalResponsibility)
        {
            var fiscalResponsibilityDTO = new List<FiscalResponsibilityDTO>();
            foreach (CompanyFiscalResponsibility fiscalResponsibility in companyFiscalResponsibility)
            {
                fiscalResponsibilityDTO.Add(CreateCompanyFiscalResponsibility(fiscalResponsibility));
            }
            return fiscalResponsibilityDTO;
        }

        #endregion CompanyFiscalResponsibility

        #region Politicas

        public static PersonOperationDTO CreatePersonOperation(CompanyPersonOperation personOperation)
        {
            var result = new PersonOperationDTO();
            result.OperationId = personOperation.OperationId;
            result.IndividualId = personOperation.IndividualId;
            result.Operation = personOperation.Operation;
            result.Proccess = personOperation.Process;
            result.ProcessType = personOperation.ProcessType;
            result.FunctionId = personOperation.FunctionId;

            return result;
        }
        public static List<PersonOperationDTO> CreatePersonOperations(List<CompanyPersonOperation> persons)
        {
            var personOperationsDTO = new List<PersonOperationDTO>();
            foreach (CompanyPersonOperation person in persons)
            {
                personOperationsDTO.Add(CreatePersonOperation(person));
            }
            return personOperationsDTO;

        }
        #endregion Politicas
    }
}