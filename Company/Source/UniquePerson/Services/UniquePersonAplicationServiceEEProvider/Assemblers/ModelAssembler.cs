using AutoMapper;
using Sistran.Company.Application.UniquePersonAplicationServices.DTOs;
using Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Core.Application.CommonService.Models.Base;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using MOCOUP = Sistran.Core.Application.UniquePersonService.V1.Models;
using COMP = Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Core.Application.Utilities.Cache;
using System.Linq;
using COCOM = Sistran.Core.Application.CommonService.Models;
using modelsTax = Sistran.Core.Application.TaxServices.Models;
using CONS = Sistran.Company.Application.UniquePersonAplicationServices.DTOs;
using CONSINT = Sistran.Core.Integration.OperationQuotaServices.DTOs.Consortium;
using Sistran.Core.Integration.OperationQuotaServices.Enums;

namespace Sistran.Company.Application.UniquePersonAplicationServices.EEProvider.Assemblers
{
    /// <summary>
    /// Convertir de Entities a Modelos
    /// </summary>
    public class ModelAssembler
    {
        #region Person
        /// <summary>
        /// Crear una Persona
        /// </summary>
        /// <param name="person">The person.</param>
        /// <returns></returns>
        public static CompanyPerson CreatePerson(PersonDTO person)
        {
            var result = new CompanyPerson();
            result.IndividualId = person.Id;
            result.BirthDate = person.BirthDate;
            result.BirthPlace = person.BirthPlace;
            result.IdentificationDocument = new CompanyIdentificationDocument { DocumentType = new CompanyDocumentType { Id = person.DocumentTypeId }, Number = person.Document };
            result.Exoneration = person.ExonerationTypeCode == null ? null : new CompanySarlaftExoneration
            {
                IsExonerated = person.ExonerationTypeCode > 0,
                ExonerationType = new CompanyExonerationType
                {
                    Id = (int)person.ExonerationTypeCode
                }
            };
            result.EconomicActivity = new CompanyEconomicActivity { Id = person.EconomicActivityId };
            result.Gender = person.Gender;
            result.MaritalStatus = new CompanyMaritalStatus { Id = person.MaritalStatusId };
            result.SurName = person.Surname;
            result.FullName = person.Names;
            result.SecondSurName = person.SecondSurname;
            result.CheckPayable = person.CheckPayable;
            result.UserId = person.UserId;
            result.DataProtection = person.DataProtection;
            result.Insured = new CompanyInsured { ElectronicBiller = person.ElectronicBiller };
            return result;
        }

        #endregion

        #region company
        public static UniquePersonServices.V1.Models.CompanyCompany CreateCompany(CompanyDTO company)
        {
            var result = new UniquePersonServices.V1.Models.CompanyCompany();

            result.IdentificationDocument = new CompanyIdentificationDocument()
            {
                Number = company.Document,
                DocumentType = new CompanyDocumentType()
                {
                    Id = company.DocumentTypeId
                },
                NitAssociationType = company.NitAssociationType
            };
            result.IndividualId = company.Id;
            result.FullName = company.BusinessName;
            result.AssociationType = new CompanyAssociationType { Id = company.AssociationTypeId };
            result.CompanyType = new CompanyCompanyType { Id = company.CompanyTypeId };
            result.CountryId = company.CountryOriginId;
            result.EconomicActivity = new CompanyEconomicActivity { Id = company.EconomicActivityId };
            result.VerifyDigit = company.VerifyDigit;
            result.Exoneration = company.ExonerationTypeCode == null ? null : new CompanySarlaftExoneration
            {
                IsExonerated = company.ExonerationTypeCode > 0,
                ExonerationType = new CompanyExonerationType
                {
                    Id = (int)company.ExonerationTypeCode
                }
            };
            result.Consortiums = company.ConsortiumMembers?.Select(CreateConsortium)?.ToList();
            result.Insured = company.Insured != null ? CreateInsured(company.Insured) : null;
            result.CheckPayable = company.CheckPayable;
            result.UserId = company.UserId;
            //**chequea a nombre de
            // company.CheckPayable 
            //tipo de direccion no esta
            //direccion completa
            //company.Addresses.Description

            //result.Name = company.BusinessName;
            //result.CompanyExtended = new CompanyExtended { AssociationType = new AssociationType { Id = company.AssociationTypeId } };

            //result.CheckPayable = company.CheckPayable;
            return result;
        }
        public static List<UniquePersonServices.V1.Models.CompanyCompany> CreateCompanys(List<CompanyDTO> companys)
        {
            var companysDTO = new List<UniquePersonServices.V1.Models.CompanyCompany>();
            foreach (CompanyDTO company in companys)
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
        public static CompanyAddress CreateAdddress(AddressDTO address)
        {
            var result = new CompanyAddress()
            {
                Id = address.Id,
                AddressType = new CompanyAddressType() { Id = address.AddressTypeId },
                City = new Core.Application.CommonService.Models.City() { Id = address.CityId, State = new Core.Application.CommonService.Models.State() { Id = address.StateId, Country = new Core.Application.CommonService.Models.Country { Id = address.CountryId } } },
                Description = address.Description,
                IsPrincipal = address.IsPrincipal,
            };
            return result;
        }
        public static List<CompanyAddress> CreateAdddresses(List<AddressDTO> addresses)
        {
            var result = new List<CompanyAddress>();
            foreach (var address in addresses)
            {
                result.Add(ModelAssembler.CreateAdddress(address));
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
        public static CompanyEmail CreateEmail(EmailDTO email)
        {
            var result = new CompanyEmail()
            {
                Id = email.Id,
                EmailType = new CompanyEmailType() { Id = email.EmailTypeId },
                Description = email.Description,
                IsPrincipal = email.IsPrincipal,
            };
            return result;
        }

        public static List<CompanyEmail> CreateEmails(List<EmailDTO> Emails)
        {
            var result = new List<CompanyEmail>();
            foreach (var Email in Emails)
            {
                result.Add(ModelAssembler.CreateEmail(Email));
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
        public static CompanyPhone CreatePhone(PhoneDTO phone)
        {
            var result = new CompanyPhone()
            {
                Id = phone.Id,
                PhoneType = new CompanyPhoneType() { Id = phone.PhoneTypeId, Description = phone.PhoneTypeDescription },
                Description = phone.Description,
                IsMain = phone.IsPrincipal,
                CityCode = phone.CityCode,
                CountryCode = phone.CountryCode,
                Extension = phone.Extension,
                ScheduleAvailability = phone.ScheduleAvailability
            };
            return result;
        }

        public static List<CompanyPhone> CreatePhones(List<PhoneDTO> Phones)
        {
            var result = new List<CompanyPhone>();
            foreach (var Phone in Phones)
            {
                result.Add(ModelAssembler.CreatePhone(Phone));
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
        public static CompanyInsured CreateInsured(InsuredDTO insured)
        {
            CompanyInsured companyInsured = new CompanyInsured()
            {
                InsuredCode = insured.Id,
                IndividualId = insured.IndividualId,
                Agency = insured.AgencyId == null ? null : new CompanyAgency { Id = (int)insured.AgencyId, Agent = insured.AgentId == null ? null : new CompanyAgent { IndividualId = (int)insured.AgentId } },
                Annotations = insured.Annotations,
                EnteredDate = insured.EnteredDate,
                UpdateDate = insured.UpdateDate,
                ModifyDate = insured.ModifyDate,
                DeclinedDate = insured.DeclinedDate,
                DeclinedType = insured.InsDeclinesTypeId == null ? null : new CompanyInsuredDeclinedType { Id = (int)insured.InsDeclinesTypeId },
                Concept = new CompanyInsuredConcept { IsBeneficiary = insured.IsBeneficiary, IsHolder = insured.IsHolder, IsPayer = insured.IsPayer, IsInsured = insured.IsInsured, },
                Profile = insured.InsProfileId == null ? null : new CompanyInsuredProfile { Id = (int)insured.InsProfileId },
                Branch = insured.BranchId == null ? null : new CommonServices.Models.CompanyBranch { Id = Convert.ToInt32(insured.BranchId) },
                Segment = insured.InsSegmentId == null ? null : new CompanyInsuredSegment { Id = Convert.ToInt32(insured.InsSegmentId) },
                IsSMS = insured.IsSms,
                IsMailAddress = insured.IsMailAddress,
                UserId = insured.UserId,
                ElectronicBiller = insured.ElectronicBiller,
                RegimeType = insured.RegimeType

            };
            return companyInsured;
        }

        #endregion


        #region Supplier

        public static CompanySupplier CreateSupplier(ProviderDTO supplier)
        {
            var result = new CompanySupplier();

            result.EnteredDate = supplier.CreationDate;
            result.DeclinedDate = supplier.DeclinationDate;
            result.Id = supplier.Id;
            result.IndividualId = supplier.IndividualId;
            result.ModificationDate = supplier.ModificationDate;
            result.Observation = supplier.Observation;
            result.DeclinedType = (supplier.ProviderDeclinedTypeId) == null ? null : new CompanySupplierDeclinedType { Id = (int)supplier.ProviderDeclinedTypeId };
            result.Type = new CompanySupplierType { Id = supplier.ProviderTypeId };
            result.Profile = new CompanySupplierProfile { Id = supplier.SupplierProfileId };

            result.AccountingConcepts = CreateAccountingConcepts(supplier.ProviderPaymentConcepts);
            result.GroupSupplier = CreateGroupsSupplier(supplier.GroupSupplier);

            return result;
        }


        public static CompanyAccountingConcept CreateAccountingConcept(ProviderPaymentConceptDTO providerPaymentConcept)
        {
            var result = new CompanyAccountingConcept();

            result.Id = providerPaymentConcept.PaymentConceptId;
            result.Description = providerPaymentConcept.Description;

            return result;
        }

        public static CompanyGroupSupplier CreateGroupSupplier(GroupSupplierDTO groupSupplierDTO)
        {
            var result = new CompanyGroupSupplier();

            result.Id = groupSupplierDTO.Id;
            result.Description = groupSupplierDTO.Description;

            return result;
        }

        public static List<CompanyAccountingConcept> CreateAccountingConcepts(List<ProviderPaymentConceptDTO> ProviderPaymentConcept)
        {
            var result = new List<CompanyAccountingConcept>();
            if (ProviderPaymentConcept != null && ProviderPaymentConcept.Count > 0)
            {
                foreach (var providerPaymentConcept in ProviderPaymentConcept)
                {
                    result.Add(ModelAssembler.CreateAccountingConcept(providerPaymentConcept));
                }
            }
            return result;
        }

        public static List<CompanyGroupSupplier> CreateGroupsSupplier(List<GroupSupplierDTO> groupSupplierDTOs)
        {
            var result = new List<CompanyGroupSupplier>();
            if (groupSupplierDTOs != null && groupSupplierDTOs.Count > 0)
            {
                foreach (var groupSupplier in groupSupplierDTOs)
                {
                    result.Add(ModelAssembler.CreateGroupSupplier(groupSupplier));
                }
            }
            return result;
        }



        #endregion Supplier

        #region CoCompanyName v1

        public static CompanyCoCompanyName CreateCoCompanyName(CompanyNameDTO companyNameDTO)
        {
            var result = new CompanyCoCompanyName();

            if (companyNameDTO != null)
            {
                result.IndividualId = companyNameDTO.IndividualId;
                result.NameNum = companyNameDTO.NameNum;
                result.TradeName = companyNameDTO.TradeName;
                result.IsMain = companyNameDTO.IsMain;

                result.Phone = new CompanyPhone() { Id = companyNameDTO.PhoneID };
                result.Address = new CompanyAddress() { Id = companyNameDTO.AddressID };
                result.Email = new CompanyEmail() { Id = companyNameDTO.EmailID };
            }

            return result;
        }

        public static List<CompanyCoCompanyName> CreateCoCompanyNames(List<CompanyNameDTO> companyNameDTOs)
        {
            var result = new List<CompanyCoCompanyName>();
            foreach (var companyNameDTO in companyNameDTOs)
            {
                result.Add(ModelAssembler.CreateCoCompanyName(companyNameDTO));
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
        public static Supplier CreateProvider(ProviderDTO provider)
        {
            var result = new Supplier();

            //result.CreationDate = provider.CreationDate;
            //result.DeclinationDate = provider.DeclinationDate;
            //result.Id = provider.Id;
            //result.IndividualId = provider.IndividualId;
            //result.ModificationDate = provider.ModificationDate;
            //result.Observation = provider.Observation;
            ////result.OriginTypeId = provider.OriginTypeId;
            //result.ProviderDeclinedTypeId = provider.ProviderDeclinedTypeId;
            //result.ProviderPaymentConcept = CreateProviderPaymentConcepts(provider.ProviderPaymentConcepts);
            //result.ProviderSpeciality = CreateProviderSpecialities(provider.ProviderSpecialities);
            // result.ProviderTypeId = provider.ProviderTypeId;

            return result;
        }

        /// <summary>
        /// Crear un ProviderPaymentConcept
        /// </summary>
        /// <param name="paymentConcept">The insured.</param>
        /// <returns></returns>
        //public static ProviderPaymentConcept CreateProviderPaymentConcept(ProviderPaymentConceptDTO paymentConcept)
        //{
        //    var result = new ProviderPaymentConcept();

        //    result.Id = paymentConcept.Id;
        //    result.PaymentConcept = new Core.Application.CommonService.Models.PaymentConcept { Id = paymentConcept.PaymentConceptId };
        //    return result;
        //}

        /// <summary>
        /// Crear una lista de ProviderPaymentConcept
        /// </summary>
        /// <param name="paymentConcepts">The insured.</param>
        /// <returns></returns>
        //public static List<ProviderPaymentConcept> CreateProviderPaymentConcepts(List<ProviderPaymentConceptDTO> paymentConcepts)
        //{
        //    var result = new List<ProviderPaymentConcept>();
        //    foreach (ProviderPaymentConceptDTO providerPaymentConceptDTO in paymentConcepts)
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
        public static ProviderSpeciality CreateProviderSpeciality(ProviderSpecialityDTO providerSpeciality)
        {
            var result = new ProviderSpeciality();

            result.Id = providerSpeciality.Id;
            result.SpecialityId = providerSpeciality.SpecialityId;
            return result;
        }

        /// <summary>
        /// Crear una lista de ProviderPaymentConcept
        /// </summary>
        /// <param name="providerSpecialities">The insured.</param>
        /// <returns></returns>
        public static List<ProviderSpeciality> CreateProviderSpecialities(List<ProviderSpecialityDTO> providerSpecialities)
        {
            var result = new List<ProviderSpeciality>();
            foreach (ProviderSpecialityDTO providerSpecialitie in providerSpecialities)
            {
                result.Add(CreateProviderSpeciality(providerSpecialitie));
            }
            return result;
        }

        #endregion

        #region Agent
        /// <summary>
        /// Crear un agente
        /// </summary>
        /// <param name="agent">The insured.</param>
        /// <returns></returns>
        public static CompanyAgent CreateAgent(AgentDTO agent)
        {
            var result = new CompanyAgent();
            if (agent.AgentDeclinedTypeId > 0)
            {
                result.AgentDeclinedType = new CompanyAgentDeclinedType { Id = agent.AgentDeclinedTypeId };
            }
            result.AgentType = new CompanyAgentType { Id = agent.AgentTypeId };
            result.Annotations = agent.Annotations;
            result.DateCurrent = agent.DateCurrent;
            result.DateDeclined = agent.DateDeclined;
            result.DateModification = agent.DateModification;
            result.IndividualId = agent.IndividualId;
            result.GroupAgent = new CompanyGroupAgent { Id = agent.IdGroup };
            result.SalesChannel = new CompanySalesChannel { Id = agent.IdChanel };
            result.FullName = agent.FullNName;
            result.Locker = agent.Locker;
            result.CommissionDiscountAgreement = agent.CommissionDiscountAgreement;
            if (agent.EmployeePerson != null)
            {
                result.EmployeePerson = new CompanyEmployeePerson { Id = agent.EmployeePerson.Id };
            }

            return result;
        }

        public static List<CompanyComissionAgent> CreateComissionAgents(List<ComissionAgentDTO> comissionAgents)
        {
            List<CompanyComissionAgent> commissionAgents = new List<CompanyComissionAgent>();
            foreach (ComissionAgentDTO comissionAgentDTO in comissionAgents)
            {
                commissionAgents.Add(CreateCommissionAgent(comissionAgentDTO));
            }
            return commissionAgents;
        }

        public static CompanyComissionAgent CreateCommissionAgent(ComissionAgentDTO comissionAgentDTO)
        {
            return new CompanyComissionAgent
            {
                AgentAgencyId = comissionAgentDTO.AgencyId,
                DateCommission = comissionAgentDTO.DateCommission,
                Id = comissionAgentDTO.Id,
                LineBusiness = new CommonServices.Models.CompanyLineBusiness { Id = comissionAgentDTO.LineBusinessId },
                Prefix = new CommonServices.Models.CompanyPrefix { Id = comissionAgentDTO.PrefixId },
                SubLineBusiness = new CommonServices.Models.CompanySubLineBusiness { Id = comissionAgentDTO.SubLineBusinessId },
                PercentageAdditional = comissionAgentDTO.PercentageAdditional,
                PercentageCommission = comissionAgentDTO.PercentageCommission,
                IndividualId = comissionAgentDTO.IndividualId,
                //Prefix = new BasePrefix { Id = comissionAgentDTO.PrefixId},
                //SubLineBusiness = new BaseSubLineBusiness { Id = comissionAgentDTO.SubLineBusinessId},               
                //Prefix = new BasePrefix { Id = comissionAgentDTO.PrefixId },
                //SubLineBusiness = new BaseSubLineBusiness { Id = comissionAgentDTO.SubLineBusinessId },
            };
        }

        public static CompanyAgency CreateAgency(AgencyDTO agency)
        {
            var result = new CompanyAgency();
            if (agency.AgentDeclinedTypeId > 0)
            {
                result.AgentDeclinedType = new CompanyAgentDeclinedType { Id = agency.AgentDeclinedTypeId };
            }

            result.Annotations = agency.Annotations;
            result.Branch = new CommonServices.Models.CompanyBranch { Id = agency.BranchId };
            result.Code = agency.Code;
            result.DateDeclined = agency.DateDeclined;
            result.FullName = agency.FullName;
            result.Id = agency.Id;
            result.AgentType = new CompanyAgentType { Id = agency.AgentId };

            return result;
        }

        internal static List<CONSINT.ConsortiumEventDTO> CreateConsortiumEvents(List<ConsortiumEventDTO> consortiumEventDTOs)
        {
            List<CONSINT.ConsortiumEventDTO> eventDTOs = new List<CONSINT.ConsortiumEventDTO>();

            foreach (ConsortiumEventDTO consortiumEventDTO in consortiumEventDTOs)
            {
                eventDTOs.Add(CreateConsortiumEvent(consortiumEventDTO));
            }
            return eventDTOs;
        }

        internal static CONSINT.ConsortiumEventDTO CreateConsortiumEvent(CONS.ConsortiumEventDTO consortiumEventDTO)
        {
            if (consortiumEventDTO == null)
            {
                return null;
            }
            return new CONSINT.ConsortiumEventDTO
            {
                ConsortiumEventEventType = Convert.ToInt32(consortiumEventDTO.ConsortiumEventEventType),
                consortiumDTO = new CONSINT.ConsortiumDTO
                {
                    AssociationType = Convert.ToInt32(consortiumEventDTO.consortiumDTO?.AssociationType),
                    AssociationTypeDesc = Convert.ToString(consortiumEventDTO.consortiumDTO?.AssociationTypeDesc),
                    ConsortiumName = Convert.ToString(consortiumEventDTO.consortiumDTO?.ConsortiumName),
                    ConsotiumId = Convert.ToInt32(consortiumEventDTO.IndividualConsortiumID),
                    UpdateDate = Convert.ToDateTime(consortiumEventDTO.consortiumDTO?.UpdateDate)
                },
                ConsortiumpartnersDTO = new CONSINT.ConsortiumpartnersDTO
                {
                    ConsortiumId = Convert.ToInt32(consortiumEventDTO.IndividualConsortiumID),
                    IndividualConsortiumId = Convert.ToInt32(consortiumEventDTO.IndividualConsortiumID),
                    IndividualPartnerId = Convert.ToInt32(consortiumEventDTO.ConsortiumpartnersDTO?.IndividualPartnerId),
                    ParticipationRate = Convert.ToDecimal(consortiumEventDTO.ConsortiumpartnersDTO?.ParticipationRate),
                    PartnerName = Convert.ToString(consortiumEventDTO.ConsortiumpartnersDTO?.PartnerName),
                    InitDate = Convert.ToDateTime(consortiumEventDTO.ConsortiumpartnersDTO?.InitDate),
                    EndDate = Convert.ToDateTime(consortiumEventDTO.ConsortiumpartnersDTO?.EndDate),
                    Enabled = Convert.ToBoolean(consortiumEventDTO.ConsortiumpartnersDTO?.Enabled)
                },
                ConsortiumEventID = Convert.ToInt32(consortiumEventDTO.ConsortiumEventID),
                IndividualId = Convert.ToInt32(consortiumEventDTO.IndividualId),
                IssueDate = Convert.ToDateTime(consortiumEventDTO.IssueDate),
                IndividualConsortiumID = Convert.ToInt32(consortiumEventDTO.IndividualConsortiumID),
                payload = Convert.ToString(consortiumEventDTO.payload)
            };
        }

        public static List<CompanyAgency> CreateAgencies(List<AgencyDTO> agencies)
        {
            var result = new List<CompanyAgency>();

            foreach (var agency in agencies)
            {
                result.Add(CreateAgency(agency));
            }
            return result;
        }

        public static AgentAgency CreateAgentAgency(AgentAgencyDTO agentAgency)
        {
            var result = new AgentAgency();
            result.AgencyAgencyId = agentAgency.AgencyAgencyId;
            result.AllianceId = agentAgency.AllianceId;
            result.IndividualId = agentAgency.IndividualId;
            result.IsSpecialImpression = agentAgency.IsSpecialImpression;
            result.Status = agentAgency.Status;
            return result;
        }

        public static List<AgentAgency> CreateAgentAgencies(List<AgentAgencyDTO> agentAgencies)
        {
            var result = new List<AgentAgency>();

            foreach (var agentAgency in agentAgencies)
            {
                result.Add(CreateAgentAgency(agentAgency));
            }
            return result;
        }

        public static CompanyPrefixs CreatePrefix(PrefixDTO prefix)
        {
            var result = new CompanyPrefixs();
            result.Description = prefix.Description;
            result.Id = prefix.Id;
            return result;
        }

        public static List<CompanyPrefixs> CreatePrefixes(List<PrefixDTO> prefixes)
        {
            var result = new List<CompanyPrefixs>();

            foreach (var prefix in prefixes)
            {
                result.Add(CreatePrefix(prefix));
            }
            return result;
        }
        #endregion

        #region SarlaftV1
        #region individualSarlaft v1
        public static IndividualSarlaft CreateIndividualSarlaft(IndividualSarlaftDTO individualSarlaftDto)
        {
            IndividualSarlaft individualSarlaf = new IndividualSarlaft();
            individualSarlaf.BranchCode = individualSarlaftDto.BranchCode;
            individualSarlaf.FormNum = individualSarlaftDto.FormNum;
            individualSarlaf.Id = individualSarlaftDto.Id;
            individualSarlaf.IndividualId = individualSarlaftDto.IndividualId;
            individualSarlaf.InternationalOperations = individualSarlaftDto.InternationalOperations;
            individualSarlaf.InterviewResultCode = individualSarlaftDto.InterviewResultCode;
            individualSarlaf.InterviewerName = individualSarlaftDto.InterviewerName;
            individualSarlaf.InterviewerPlace = individualSarlaftDto.InterviewerPlace;
            individualSarlaf.PendingEvents = individualSarlaftDto.PendingEvents;
            individualSarlaf.RegistrationDate = individualSarlaftDto.RegistrationDate;
            individualSarlaf.VerifyingEmployee = individualSarlaftDto.VerifyingEmployee;
            individualSarlaf.Year = individualSarlaftDto.Year;
            individualSarlaf.EconomicActivity = new EconomicActivity { Id = individualSarlaftDto.ActivityEconomic };
            individualSarlaf.AuthorizedBy = individualSarlaftDto.AuthorizedBy;
            individualSarlaf.UserId = individualSarlaftDto.UserId;
            individualSarlaf.FinancialSarlaft = new FinancialSarlaf
            {
                AssetsAmount = individualSarlaftDto.finacialSarlaft.AssetsAmount,
                ExpenseAmount = individualSarlaftDto.finacialSarlaft.ExpenseAmount,
                ExtraIncomeAmount = individualSarlaftDto.finacialSarlaft.ExtraIncomeAmount,
                IncomeAmount = individualSarlaftDto.finacialSarlaft.IncomeAmount,
                IsForeignTransaction = individualSarlaftDto.finacialSarlaft.IsForeignTransaction,
                ForeignTransactionAmount = individualSarlaftDto.finacialSarlaft.ForeignTransactionAmount,
                LiabilitiesAmount = individualSarlaftDto.finacialSarlaft.LiabilitiesAmount,
                SarlaftId = individualSarlaftDto.finacialSarlaft.SarlaftId,
                Description = individualSarlaftDto.finacialSarlaft.Description,
            };

            return individualSarlaf;
        }
        public static List<IndividualSarlaft> CreateIndividualSarlafts(List<IndividualSarlaftDTO> individualSarlaftDto)
        {
            List<IndividualSarlaft> individualSarlaft = new List<IndividualSarlaft>();
            foreach (var item in individualSarlaftDto)
            {
                individualSarlaft.Add(CreateIndividualSarlaft(item));
            }

            return individualSarlaft;
        }
        #endregion
        #endregion

        #region ReInsurer

        public static CompanyReInsurer CreateReInsurer(ReInsurerDTO reinsurer)
        {
            var imapper = CreateMapReInsurer();
            return imapper.Map<ReInsurerDTO, CompanyReInsurer>(reinsurer);



        }
        #endregion

        #region AutoMapper
        #region ReInsurer
        public static IMapper CreateMapReInsurer()
        {
            var config = MapperCache.GetMapper<ReInsurerDTO, CompanyReInsurer>(cfg =>
            {
                cfg.CreateMap<ReInsurerDTO, CompanyReInsurer>();
            });

            return config;
        }

        public static ReInsurerDTO CreateReInsurer(CompanyReInsurer companyReInsurer)
        {
            return new ReInsurerDTO()
            {
                Id = companyReInsurer.ReinsuredCD,
                IndividualId = companyReInsurer.IndividualId,
                ModifyDate = companyReInsurer.ModifyDate,
                DeclinedDate = companyReInsurer.DeclinedDate,
                EnteredDate = companyReInsurer.EnteredDate,
                DeclaredTypeCD = companyReInsurer.DeclaredTypeCD,
                Annotations = companyReInsurer.Annotations
            };

        }

        #endregion ReInsurer

        #endregion AutoMapper

        #region Partner
        /// <summary>
        /// Crear un Asociado
        /// </summary>
        /// <param name="partner">The pal.</param>
        /// <returns></returns>
        public static CompanyPartner CreatePartner(PartnerDTO partner)
        {

            var objs = new CompanyPartner();
            objs.Active = partner.Active;
            objs.IdentificationDocument = new CompanyIdentificationDocument { Number = Convert.ToString(partner.IdentificationDocumentNumber) };
            objs.IdentificationDocument.DocumentType = new CompanyDocumentType { Id = (partner.DocumentTypeId) };
            objs.IndividualId = partner.IndividualId;
            objs.PartnerId = partner.PartnerId;
            objs.TradeName = partner.TradeName;
            return objs;
        }

        public static List<CompanyPartner> CreatePartners(List<PartnerDTO> partners)
        {
            var objs = new List<CompanyPartner>();
            foreach (var partner in partners)
            {
                objs.Add(ModelAssembler.CreatePartner(partner));
            }
            return objs;
        }

        #endregion

        #region CompanyName
        /// <summary>
        /// Crea razon social
        /// </summary>
        /// <param name="cpoName"></param>
        /// <returns></returns>
        public static CompanyName CreateCompanyName(CompanyNameDTO cpoName)
        {
            var objs = new CompanyName();
            objs.Address.Id = cpoName.AddressID;
            objs.Email.Id = cpoName.EmailID;
            objs.IndividualId = cpoName.IndividualId;
            objs.IsMain = cpoName.IsMain;
            objs.Phone.Id = cpoName.PhoneID;
            objs.NameNum = cpoName.NameNum;
            objs.TradeName = cpoName.TradeName;
            objs.Enabled = cpoName.Enabled;
            return objs;
        }

        public static CompanyName CreateCompanyName(int individualID, int addressesId, int emailsId, int phonesId, CompanyDTO company)
        {
            var objs = new CompanyName();
            objs.Address = new Address() { Id = addressesId };
            objs.Email = new Email() { Id = emailsId };
            objs.IndividualId = individualID;
            objs.IsMain = true;
            objs.Phone = new Phone() { Id = phonesId };
            objs.NameNum = 1;
            objs.TradeName = company.BusinessName;
            objs.Enabled = true;
            return objs;
        }


        /// <summary>
        /// Crea mas de una razon social
        /// </summary>
        /// <param name="cpoNames"></param>
        /// <returns></returns>
        public static List<CompanyName> CreateCompaniesName(List<CompanyNameDTO> cpoNames)
        {
            var objs = new List<CompanyName>();
            foreach (var cpoName in cpoNames)
            {
                objs.Add(ModelAssembler.CreateCompanyName(cpoName));
            }
            return objs;
        }
        #endregion

        #region SarlaftPerson
        /// <summary>
        /// Crear datos de SARLAFT
        /// </summary>
        /// <param name="fSarlaft">The SARLAFT.</param>
        /// <returns></returns>
        public static FinancialSarlaf CreateFinancialSarlaft(SarlaftDTO fSarlaft)
        {
            var objs = new FinancialSarlaf()
            {
                SarlaftId = fSarlaft.SarlaftId,
                AssetsAmount = fSarlaft.AssetsAmount,
                Description = fSarlaft.Description,
                ExpenseAmount = fSarlaft.ExpenseAmount,
                ExtraIncomeAmount = fSarlaft.ExtraIncomeAmount,
                ForeignTransactionAmount = fSarlaft.ForeignTransactionAmount,
                IsForeignTransaction = fSarlaft.IsForeignTransaction,
                IncomeAmount = fSarlaft.IncomeAmount,
                LiabilitiesAmount = fSarlaft.LiabilitiesAmount
            };
            return objs;
        }

        public static List<CompanyPartner> CreateFinancialSarlafts(List<PartnerDTO> partners)
        {
            var objs = new List<CompanyPartner>();
            foreach (var partner in partners)
            {
                objs.Add(ModelAssembler.CreatePartner(partner));
            }
            return objs;
        }
        #endregion

        #region PersonLabour
        /// <summary>
        /// Se encarga de realizar el mapeo entro los DTOS a las modelos de Negocio.
        /// </summary>
        /// <param name="laborPerson">Pasa uno a uno la Informacion que se envia.</param>
        /// <returns>Retorna la informacion para guardarla.</returns>
        public static CompanyLabourPerson CreateInformationLabourPerson(PersonInformationAndLabourDTO personInformationAndLabour)
        {
            return new CompanyLabourPerson
            {
                Id = personInformationAndLabour.IndividualId,
                IndividualId = personInformationAndLabour.IndividualId,
                PersonType = personInformationAndLabour.PersonType == null ? null : new CompanyPersonType { Id = (int)personInformationAndLabour.PersonType },
                CustomerType = CustomerType.Individual,
                IncomeLevel = personInformationAndLabour.IncomeLevel == null ? null : new CompanyIncomeLevel { Id = (int)personInformationAndLabour.IncomeLevel },
                CompanyName = personInformationAndLabour.CompanyName,
                Children = personInformationAndLabour.Children,
                EducativeLevel = personInformationAndLabour.EducativeLevel == null ? null : new CompanyEducativeLevel { Id = (int)personInformationAndLabour.EducativeLevel },
                SocialLayer = personInformationAndLabour.SocialLayer == null ? null : new CompanySocialLayer { Id = (int)personInformationAndLabour.SocialLayer },
                SpouseName = personInformationAndLabour.SpouseName,
                Occupation = personInformationAndLabour.Occupation == null ? null : new CompanyOccupation { Id = (int)personInformationAndLabour.Occupation },
                Position = personInformationAndLabour.Position,
                Contact = personInformationAndLabour.Contact,
                OtherOccupation = personInformationAndLabour.OtherOccupation == null ? null : new CompanyOccupation { Id = (int)personInformationAndLabour.OtherOccupation },
                CompanyPhone = personInformationAndLabour.CompanyPhone == null ? null : new CompanyPhone { Id = (int)personInformationAndLabour.CompanyPhone },
                Speciality = personInformationAndLabour.Speciality == null ? null : new CompanySpeciality { Id = (int)personInformationAndLabour.Speciality },
                HouseType = personInformationAndLabour.HouseType == null ? null : new CompanyHouseType { Id = (int)personInformationAndLabour.HouseType },
                JobSector = personInformationAndLabour.JobSector,
                BirthCountryId = personInformationAndLabour.BirthCountryId,
                PersonInterestGroup = CreatePersonInterestGroups(personInformationAndLabour.PersonInterestGroup, personInformationAndLabour.IndividualId)
            };
        }

        public static List<CompanyPersonInterestGroup> CreatePersonInterestGroups(List<PersonInterestGroupDTO> groups, int individualid)
        {
            var gruposDTO = new List<UniquePersonServices.V1.Models.CompanyPersonInterestGroup>();
            foreach (PersonInterestGroupDTO field in groups)
            {
                gruposDTO.Add(CreatePersonInterestGroup(field, individualid));
            }
            return gruposDTO;
        }

        public static CompanyPersonInterestGroup CreatePersonInterestGroup(PersonInterestGroupDTO personinterestgroup, int individualid)
        {
            var result = new UniquePersonServices.V1.Models.CompanyPersonInterestGroup();
            result.InterestGroupTypeId = personinterestgroup.InterestGroupTypeId;
            result.IndividualId = individualid;
            return result;
        }
        #endregion PersonLabour

        #region OperatingQuota

        /// <summary>
        /// Convierte una lista de objetos OperatingQuotaDTO a OperatingQuota
        /// </summary>
        /// <param name="ListOperatingQuotaDTOs"></param>
        /// <returns></returns>
        public static List<CompanyOperatingQuota> CreateOperatingQuotas(List<OperatingQuotaDTO> ListOperatingQuotaDTOs)
        {
            var result = new List<CompanyOperatingQuota>();
            foreach (OperatingQuotaDTO operatingQuotaDTO in ListOperatingQuotaDTOs)
            {
                result.Add(CreateOperatingQuota(operatingQuotaDTO));

            }
            return result;
        }

        /// <summary>
        /// Convierte un objeto OperatingQuotaDTO a OperatingQuota
        /// </summary>
        /// <param name="operatingQuotaDTO"></param>
        /// <returns></returns>
        public static CompanyOperatingQuota CreateOperatingQuota(OperatingQuotaDTO operatingQuotaDTO)
        {
            var result = new CompanyOperatingQuota();
            result.IndividualId = operatingQuotaDTO.IndividualId;
            result.LineBusinessId = operatingQuotaDTO.LineBusinessId;
            result.Amount = operatingQuotaDTO.AmountValue;
            result.CurrencyId = operatingQuotaDTO.CurrencyId;
            result.CurrentTo = operatingQuotaDTO.CurrentTo;
            return result;
        }

        #endregion

        #region PaymentMethodAccount
        //public static MOCOUP. CreatePaymentMethodAccount(PaymentMethodAccountDTO paymentMethodAccountDTO)
        //{

        //    return new PaymentMethodAccount
        //    {
        //        AccountNumber = paymentMethodAccountDTO.AccountNumber,
        //        AccountType = new PaymentAccountType { Id = paymentMethodAccountDTO.AccountType },
        //        PaymentMethod = new Sistran.Core.Application.CommonService.Models.PaymentMethod { Id = paymentMethodAccountDTO.MethodPayment },
        //        Bank = new Sistran.Core.Application.CommonService.Models.Bank { Id = paymentMethodAccountDTO.Bank, /*BranchOffice = paymentMethodAccountDTO.Office*/ },
        //    };
        //}
        //public static List<PaymentMethodAccount> CreatePaymentMethodAccount(List<PaymentMethodAccountDTO> paymentMethodAccountDTO)
        //{
        //    var result = new List<PaymentMethodAccount>();
        //    foreach (var item in paymentMethodAccountDTO)
        //    {
        //        result.Add(CreatePaymentMethodAccount(item));
        //    }
        //    return result;
        //}

        #endregion

        #region IndividualTaxExeption

        /// <summary>
        /// Crear impuesto individual
        /// </summary>
        /// <param name="listIndividualTaxExeptionDTO"></param>
        /// <returns></returns>
        public static List<CompanyIndividualTax> CreateIndividualTaxes(List<IndividualTaxExeptionDTO> listIndividualTaxExeptionDTO)
        {
            var result = new List<CompanyIndividualTax>();
            foreach (IndividualTaxExeptionDTO individualTax in listIndividualTaxExeptionDTO)
            {
                result.Add(CreateIndividualTax(individualTax));

            }
            return result;
        }

        /// <summary>
        /// Convierte un objeto IndividualTaxExeptionDTO a IndividualTaxExeption
        /// </summary>
        /// <param name="individualTaxExeptionDTO"></param>
        /// <returns></returns>
        public static CompanyIndividualTax CreateIndividualTax(IndividualTaxExeptionDTO individualTaxExeptionDTO)
        {
            var result = new CompanyIndividualTax();
            result = new CompanyIndividualTax()
            {
                IndividualId = individualTaxExeptionDTO.IndividualId,
                taxRate = new modelsTax.TaxRate
                {
                    Id = individualTaxExeptionDTO.TaxRateId,
                    Tax = new modelsTax.Tax
                    {
                        Id = individualTaxExeptionDTO.TaxId,
                        Description = individualTaxExeptionDTO.TaxDescription
                    },
                    TaxCondition = new modelsTax.TaxCondition
                    {
                        Id = individualTaxExeptionDTO.TaxCondition,
                    },
                    TaxCategory = new modelsTax.TaxCategory
                    {
                        Id = individualTaxExeptionDTO.TaxCategoryId,
                    }
                },
                IndividualTaxExeption = new CompanyIndividualTaxExeption()
                {
                    IndividualTaxExemptionId = individualTaxExeptionDTO.IndividualTaxExemptionId
                },
                Role = new Role
                {
                    Id = individualTaxExeptionDTO.RoleId
                }

            };

            result.IndividualId = individualTaxExeptionDTO.IndividualId;
            result.IndividualTaxExeption.IndividualTaxExemptionId = individualTaxExeptionDTO.IndividualTaxExemptionId;
            result.Id = individualTaxExeptionDTO.Id;

            return result;
        }


        public static CompanyIndividualTaxExeption CreateIndividualTaxExeption(IndividualTaxExeptionDTO individualTaxExeptionDTO)
        {
            var result = new CompanyIndividualTaxExeption();

            result.IndividualTaxExemptionId = individualTaxExeptionDTO.IndividualTaxExemptionId;
            result.ExtentPercentage = individualTaxExeptionDTO.ExtentPercentage;
            result.Datefrom = individualTaxExeptionDTO.Datefrom;
            result.DateUntil = individualTaxExeptionDTO.DateUntil;
            result.StateCode = new Core.Application.CommonService.Models.State { Id = individualTaxExeptionDTO.StateCode };
            result.CountryCode = individualTaxExeptionDTO.CountryId;
            result.TaxCategory = new CompanyTaxCategory { Id = individualTaxExeptionDTO.TaxCategoryId };
            result.OfficialBulletinDate = individualTaxExeptionDTO.OfficialBulletinDate;
            result.ResolutionNumber = individualTaxExeptionDTO.ResolutionNumber;
            result.TotalRetention = individualTaxExeptionDTO.TotalRetention;
            result.IndividualId = individualTaxExeptionDTO.IndividualId;
            result.TaxCode = individualTaxExeptionDTO.TaxId;
            result.IndividualId = individualTaxExeptionDTO.IndividualId;
            result.RateTaxId = individualTaxExeptionDTO.TaxRateId;
            individualTaxExeptionDTO.status = ModelServices.Enums.StatusTypeService.Original;
            return result;
        }
        public static List<CompanyIndividualTaxExeption> CreateIndividualTaxesExeptions(List<IndividualTaxExeptionDTO> listIndividualTaxExeptionDTO)
        {
            var result = new List<CompanyIndividualTaxExeption>();
            foreach (IndividualTaxExeptionDTO individualTax in listIndividualTaxExeptionDTO)
            {
                result.Add(CreateIndividualTaxExeption(individualTax));

            }
            return result;
        }



        #endregion

        #region ProspectPersonNatural
        public static CompanyProspectNatural CreateProspectNatural(ProspectPersonNaturalDTO prospectPersonNaturalDTO)
        {
            return new CompanyProspectNatural
            {
                ProspectCode = prospectPersonNaturalDTO.ProspectCode,
                AdditionalInfo = prospectPersonNaturalDTO.AdditionaInformation,
                BirthDate = prospectPersonNaturalDTO.BirthDate,
                City = new CompanyCity
                {
                    Id = Convert.ToInt32(prospectPersonNaturalDTO.City.Id),
                    Description = prospectPersonNaturalDTO.City.Description,
                    State = new CompanyState()
                    {
                        Id = Convert.ToInt32(prospectPersonNaturalDTO.State.Id),
                        Description = prospectPersonNaturalDTO.State.Description,
                        Country = new CompanyCountry()
                        {
                            Id = Convert.ToInt32(prospectPersonNaturalDTO.Country.Id),
                            Description = prospectPersonNaturalDTO.Country.Description,
                        }
                    },
                    DANECode = prospectPersonNaturalDTO.DANECode
                },

                CityCode = prospectPersonNaturalDTO.City.Id,
                CountryCode = prospectPersonNaturalDTO.Country.Id,
                StateCode = prospectPersonNaturalDTO.State.Id,
                IndividualTyepCode = prospectPersonNaturalDTO.IndividualTypePerson,

                EmailAddress = prospectPersonNaturalDTO.EmailAddres,
                Gender = prospectPersonNaturalDTO.Gender,

                IdCardNo = prospectPersonNaturalDTO.Card.Description,
                IdCardTypeCode = prospectPersonNaturalDTO.Card.Id,


                IndividualTypePerson = prospectPersonNaturalDTO.IndividualTypePerson,


                AddressType = prospectPersonNaturalDTO.Address.Id,
                Street = prospectPersonNaturalDTO.Address.Description,

                MaritalStatus = prospectPersonNaturalDTO.MartialStatus,
                MotherLastName = prospectPersonNaturalDTO.MotherLastName,
                Name = prospectPersonNaturalDTO.Name,
                PhoneNumber = prospectPersonNaturalDTO.PhoneNumber,
                Surname = prospectPersonNaturalDTO.SurName
            };
        }

        #endregion

        #region LegalRepresentative
        /// <summary>
        /// Convierte un objeto LegalRepresentativeDTO a LegalRepresentative
        /// </summary>
        /// <param name="legalRepresentativeDTO"></param>
        /// <returns></returns>
        internal static CompanyLegalRepresentative CreateLegalRepresentative(LegalRepresentativeDTO legalRepresentativeDTO)
        {
            return new CompanyLegalRepresentative
            {
                Address = legalRepresentativeDTO.Address,
                AuthorizationAmount = new CompanyAmount
                {
                    Currency = new CompanyCurrency { Id = legalRepresentativeDTO.CurrencyId },
                    Value = legalRepresentativeDTO.Value
                },
                BirthDate = legalRepresentativeDTO.BirthDate,
                BirthPlace = legalRepresentativeDTO.BirthPlace,
                CellPhone = legalRepresentativeDTO.CellPhone,
                Description = legalRepresentativeDTO.Description,
                Email = legalRepresentativeDTO.Email,
                ExpeditionDate = legalRepresentativeDTO.ExpeditionDate,
                ExpeditionPlace = legalRepresentativeDTO.ExpeditionPlace,
                FullName = legalRepresentativeDTO.FullName,
                Id = legalRepresentativeDTO.individualId,
                IdentificationDocument = new CompanyIdentificationDocument
                {
                    DocumentType = new CompanyDocumentType { Id = legalRepresentativeDTO.DocumentTypeId },
                    Number = legalRepresentativeDTO.NumberDocument,
                    ExpeditionDate = legalRepresentativeDTO.ExpeditionDate
                },
                JobTitle = legalRepresentativeDTO.JobTitle,
                Nationality = legalRepresentativeDTO.Nationality,
                Phone = legalRepresentativeDTO.Phone,
                City = new CompanyCity()
                {
                    Id = Convert.ToInt32(legalRepresentativeDTO.City.Id),
                    Description = legalRepresentativeDTO.City.Description,
                    State = new CompanyState()
                    {
                        Id = Convert.ToInt32(legalRepresentativeDTO.State.Id),
                        Description = legalRepresentativeDTO.Description,
                        Country = new CompanyCountry()
                        {
                            Id = Convert.ToInt32(legalRepresentativeDTO.Country.Id),
                            Description = legalRepresentativeDTO.Description
                        }
                    },
                    DANECode = legalRepresentativeDTO.DANECode
                },

            };
        }
        #endregion

        #region Consortium
        public static CompanyConsortium CreateConsortium(ConsorciatedDTO model)
        {
            CompanyConsortium companyConsortium = new CompanyConsortium();


            companyConsortium.InsuredCode = model.InsuredCode;
            companyConsortium.IndividualId = model.IndividualId;
            companyConsortium.ConsortiumId = model.ConsortiumId;
            companyConsortium.IsMain = model.IsMain;
            companyConsortium.ParticipationRate = model.ParticipationRate;
            companyConsortium.StartDate = DateTime.Now;
            companyConsortium.Enabled = model.Enabled;
            companyConsortium.FullName = model.FullName;
            companyConsortium.IdentificationDocument = new IdentificationDocument { Number = model.PersonIdentificationNumber };

            if (model.Person != null)
            {
                companyConsortium.Person = new CompanyPerson
                {
                    IdentificationDocument = new CompanyIdentificationDocument { Number = model.PersonIdentificationNumber },
                };
            }
            if (model.Company != null)
            {
                companyConsortium.Company = new CompanyCompany()
                {
                    IdentificationDocument = new CompanyIdentificationDocument { Number = model.PersonIdentificationNumber },
                };
            }

            return companyConsortium;
        }
        public static List<CompanyConsortium> CreateConsortiums(List<ConsorciatedDTO> model)
        {
            var result = new List<CompanyConsortium>();
            foreach (ConsorciatedDTO item in model)
            {
                result.Add(ModelAssembler.CreateConsortium(item));
            }
            return result;
        }
        #endregion

        #region ProspectLegal

        /// <summary>
        /// Mapea ProspectLegalDTO a ProspectNatural
        /// </summary>
        /// <param name="prospectLegalDTO"></param>
        /// <returns></returns>
        internal static CompanyProspectNatural CreateProspectLegalModel(ProspectLegalDTO prospectLegalDTO)
        {


            return new CompanyProspectNatural()
            {
                ProspectCode = prospectLegalDTO.ProspectCode,
                AdditionalInfo = prospectLegalDTO.AdditionaInformation,
                City = new CompanyCity
                {
                    Id = Convert.ToInt32(prospectLegalDTO.City.Id),
                    Description = prospectLegalDTO.City.Description,
                    State = new CompanyState()
                    {
                        Id = Convert.ToInt32(prospectLegalDTO.State.Id),
                        Description = prospectLegalDTO.State.Description,
                        Country = new CompanyCountry()
                        {
                            Id = Convert.ToInt32(prospectLegalDTO.Country.Id),
                            Description = prospectLegalDTO.Country.Description,
                        }
                    },
                    DANECode = prospectLegalDTO.DANECode
                },

                CityCode = prospectLegalDTO.City.Id,
                CountryCode = prospectLegalDTO.Country.Id,
                StateCode = prospectLegalDTO.State.Id,
                IndividualTyepCode = prospectLegalDTO.IndividualTypePerson,

                AddressType = prospectLegalDTO.Address.Id,
                Street = prospectLegalDTO.Address.Description,

                EmailAddress = prospectLegalDTO.EmailAddres,
                Name = prospectLegalDTO.Name,
                PhoneNumber = prospectLegalDTO.PhoneNumber,
                TributaryIdNumber = prospectLegalDTO.TributaryIdNumber,
                TributaryIdTypeCode = prospectLegalDTO.TributaryIdTypeCode,
                TradeName = prospectLegalDTO.TradeName

            };
        }

        internal static CompanyProspectNatural CreateProspectLegal(ProspectLegalDTO prospectLegalDTO)
        {


            return new CompanyProspectNatural()
            {
                ProspectCode = prospectLegalDTO.ProspectCode,
                IndividualTyepCode = prospectLegalDTO.IndividualTypePerson,
                Name = prospectLegalDTO.Name,
                TributaryIdNumber = prospectLegalDTO.TributaryIdNumber,
                TributaryIdTypeCode = prospectLegalDTO.TributaryIdTypeCode,
                TradeName = prospectLegalDTO.TradeName

            };
        }

        internal static CompanyProspectNatural CreateProspectNaturalModel(ProspectPersonNaturalDTO prospectDTO)
        {


            return new CompanyProspectNatural()
            {
                ProspectCode = prospectDTO.ProspectCode,
                IndividualTyepCode = prospectDTO.IndividualTypePerson,
                Name = prospectDTO.Name,
                IdCardNo = prospectDTO.IdCardNo,
                IdCardTypeCode = prospectDTO.IdCardTypeCode

            };
        }


        #endregion

        #region CompanyCoInsured
        public static MOCOUP.CompanyCoInsured CreateCompanyCoInsured(CompanyCoInsuredDTO companyCoInsuredDTO)
        {
            return new MOCOUP.CompanyCoInsured
            {
                AddressTypeCode = companyCoInsuredDTO.AddressTypeCode,
                Annotations = companyCoInsuredDTO.Annotations,
                CityCode = companyCoInsuredDTO.CityCode,
                CountryCode = companyCoInsuredDTO.CountryCode,
                Description = companyCoInsuredDTO.Description,
                EnsureInd = companyCoInsuredDTO.EnsureInd,
                EnteredDate = companyCoInsuredDTO.EnteredDate,
                InsuraceCompanyId = companyCoInsuredDTO.InsuraceCompanyId,
                ModifyDate = companyCoInsuredDTO.ModifyDate,
                PhoneNumber = companyCoInsuredDTO.PhoneNumber,
                PhoneTypeCode = companyCoInsuredDTO.PhoneTypeCode,
                StateCode = companyCoInsuredDTO.StateCode,
                Street = companyCoInsuredDTO.Street,
                TributaryIdNo = companyCoInsuredDTO.TributaryIdNo,
                ComDeclinedTypeCode = companyCoInsuredDTO.ComDeclinedTypeCode,
                DeclinedDate = companyCoInsuredDTO.DeclinedDate,
            };
        }
        #endregion

        #region InsuredGuarantee
        /// <summary>
        /// Crea  contragarantia de model a dto
        /// </summary>
        /// <param name="guarantee"></param>
        /// <returns></returns>
        public static MOCOUP.InsuredGuarantee CreateInsuredGuarantee(GuaranteeDTO guarantee)
        {
            return new MOCOUP.InsuredGuarantee()
            {
                //Id = guarantee.InsuredGuaranteeId,
                //IndividualId = guarantee.IndividualId,
                //Address = guarantee.Address,
                //GuaranteeStatus = new GuaranteeStatus { Code = guarantee.StatusCode },
                //AppraisalAmount = guarantee.AppraisalAmount,
                //BuiltArea = guarantee.BuiltArea,
                //DocumentValueAmount = guarantee.DocumentValueAmount,
                //MeasureArea = guarantee.MeasureArea,
                //RegistrationDate = guarantee.RegistrationDate,
                //Description = guarantee.Description,
                //IsCloseInd = guarantee.IsCloseInd,
                //AppraisalDate = guarantee.AppraisalDate,
                //ExpertName = guarantee.ExpertName,
                //InsuranceAmount = guarantee.InsuranceAmount,
                //PolicyNumber = guarantee.PolicyNumber,
                //DocumentNumber = guarantee.DocumentNumber,
                //ExpirationDate = guarantee.ExpirationDate,
                //RegistrationNumber = guarantee.RegistrationNumber,
                //LicensePlate = guarantee.LicensePlate,
                //EngineNro = guarantee.EngineNro,
                //ChassisNro = guarantee.ChassisNro,
                //SignatoriesNumber = guarantee.SignatoriesNumber,
                //Country = guarantee.CountryID != null ? new Core.Application.CommonService.Models.Country { Id = (int)guarantee.CountryID } : null,
                //City = guarantee.CityId != null ? new Core.Application.CommonService.Models.City { Id = (int)guarantee.CityId } : null,
                //State = guarantee.StateId != null ? new Core.Application.CommonService.Models.State { Id = (int)guarantee.StateId } : null,
                //Branch = guarantee.BranchId != null ? new Core.Application.CommonService.Models.Branch { Id = (int)guarantee.BranchId } : null,
                //PromissoryNoteType = guarantee.PromissoryNoteTypeId != null ? new PromissoryNoteType { Code = (int)guarantee.PromissoryNoteTypeId } : null,
                //MeasurementType = guarantee.MeasurementTypeId != null ? new MeasurementType { Code = (int)guarantee.MeasurementTypeId } : null,
                //Currency = guarantee.CurrencyId != null ? new Core.Application.CommonService.Models.Currency { Id = (int)guarantee.CurrencyId } : null,
                //InsuranceCompanyId = guarantee.InsuranceCompanyId,
                //AssetTypeCode = guarantee.AssetTypeCode,
                //Code = guarantee.GuaranteeCode,
                //Guarantors = CreateGuarantors(guarantee.Guarantors),
                //listDocumentation = CreateInsuredGuaranteeDocumentation(guarantee.listDocumentation),
                //listPrefix = CreateInsuredGuaranteePrefixies(guarantee.listPrefix),
                //InsuredGuaranteeLog = CreateInsuredGuaranteeLog(guarantee.InsuredGuaranteeLog),
            };
        }



        /// <summary>
        /// crea mas de una contragarantia de model a dto
        /// </summary>
        /// <param name="partners"></param>
        /// <returns></returns>
        public static List<MOCOUP.InsuredGuarantee> CreateInsuredGuaranties(List<GuaranteeDTO> insuredGuarantee)
        {
            var objs = new List<MOCOUP.InsuredGuarantee>();
            foreach (var insGuartee in insuredGuarantee)
            {
                objs.Add(ModelAssembler.CreateInsuredGuarantee(insGuartee));
            }
            return objs;
        }

        /// <summary>
        /// Crea  contragarantia de model a dto
        /// </summary>
        /// <param name="guarantee"></param>
        /// <returns></returns>
        public static MOCOUP.Guarantee CreateGuarantee(GuaranteeDTO guarantee)
        {
            return new MOCOUP.Guarantee()
            {
                //GuaranteeType = new GuaranteeType { Code = guarantee.GuaranteeTypeId },
                //InsuredGuarantee = CreateInsuredGuarantee(guarantee),
            };
        }

        /// <summary>
        /// crea mas de una contragarantia de model a dto
        /// </summary>
        /// <param name="partners"></param>
        /// <returns></returns>
        public static List<MOCOUP.Guarantee> CreateGuaranties(List<GuaranteeDTO> insuredGuarantee)
        {
            var objs = new List<MOCOUP.Guarantee>();
            foreach (var insGuartee in insuredGuarantee)
            {
                objs.Add(ModelAssembler.CreateGuarantee(insGuartee));
            }
            return objs;
        }

        #region Guarantor
        /// <summary>
        /// Crea  Contragarante
        /// </summary>
        /// <param name="guarantor"></param>
        /// <returns></returns>
        public static CompanyGuarantor CreateGuarantor(GuarantorDTO guarantor)
        {
            return new CompanyGuarantor()
            {
                GuarantorId = guarantor.GuarantorId,
                GuaranteeId = guarantor.GuaranteeId,
                IndividualId = guarantor.IndividualId,
                Adrress = guarantor.Adrress,
                CardNro = guarantor.CardNro,
                CityText = guarantor.CityText,
                TributaryIdNo = guarantor.TributaryIdNo,
                Name = guarantor.Name,
                TradeName = guarantor.TradeName,
                PhoneNumber = guarantor.PhoneNumber,
            };

        }

        /// <summary>
        /// Crea  Contragarantes
        /// </summary>
        /// <param name="guarantors"></param>
        /// <returns></returns>
        public static List<CompanyGuarantor> CreateGuarantors(List<GuarantorDTO> guarantors)
        {
            var objs = new List<CompanyGuarantor>();
            foreach (var guarantor in guarantors)
            {
                objs.Add(CreateGuarantor(guarantor));
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
        public static CompanyInsuredGuaranteeLog CreateInsuredGuaranteeLog(InsuredGuaranteeLogDTO insuredGuaranteeLog)
        {
            return new CompanyInsuredGuaranteeLog()
            {
                IndividualId = insuredGuaranteeLog.IndividualId,
                Description = insuredGuaranteeLog.Description,
                GuaranteeId = insuredGuaranteeLog.GuaranteeId,
                GuaranteeStatusCode = insuredGuaranteeLog.GuaranteeStatusCode,
                LogDate = Convert.ToDateTime(insuredGuaranteeLog.LogDate),
                UserId = insuredGuaranteeLog.UserId,
                UserName = insuredGuaranteeLog.UserName,
            };

        }


        #endregion

        #region InsuredGuaranteeDocumentation
        /// <summary>
        /// Crea  Contragarante
        /// </summary>
        /// <param name="guarantor"></param>
        /// <returns></returns>
        public static COMP.CompanyInsuredGuaranteeDocumentation CreateInsuredGuaranteeDocument(InsuredGuaranteeDocumentationDTO insuredGuaranteeDocumentation)
        {
            return new COMP.CompanyInsuredGuaranteeDocumentation()
            {
                DocumentCode = insuredGuaranteeDocumentation.DocumentCode,
                GuaranteeCode = insuredGuaranteeDocumentation.GuaranteeCode,
                GuaranteeId = insuredGuaranteeDocumentation.GuaranteeId,
                IndividualId = insuredGuaranteeDocumentation.IndividualId
            };

        }

        /// <summary>
        /// Crea  Contragarantes
        /// </summary>
        /// <param name="guarantors"></param>
        /// <returns></returns>
        public static List<COMP.CompanyInsuredGuaranteeDocumentation> CreateInsuredGuaranteeDocumentation(List<InsuredGuaranteeDocumentationDTO> insuredGuaranteeDocumentation)
        {
            var objs = new List<COMP.CompanyInsuredGuaranteeDocumentation>();
            foreach (var insuredGuaranteeDocument in insuredGuaranteeDocumentation)
            {
                objs.Add(ModelAssembler.CreateInsuredGuaranteeDocument(insuredGuaranteeDocument));
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
        public static CompanyInsuredGuaranteePrefix CreateInsuredGuaranteePrefix(InsuredGuaranteePrefixDTO insuredGuaranteePrefix)
        {
            return new CompanyInsuredGuaranteePrefix()
            {
                GuaranteeId = insuredGuaranteePrefix.GuaranteeId,
                IndividualId = insuredGuaranteePrefix.IndividualId,
                PrefixCode = insuredGuaranteePrefix.PrefixCode
            };

        }
        public static List<CompanyInsuredGuaranteePrefix> CreateInsuredGuaranteePrefixes(List<InsuredGuaranteePrefixDTO> insuredGuaranteePrefix)
        {
            List<CompanyInsuredGuaranteePrefix> companyInsuredGuaranteePrefixes = new List<CompanyInsuredGuaranteePrefix>();
            foreach (InsuredGuaranteePrefixDTO item in insuredGuaranteePrefix)
            {
                companyInsuredGuaranteePrefixes.Add(CreateInsuredGuaranteePrefix(item));
            }
            return companyInsuredGuaranteePrefixes;
        }

        /// <summary>
        /// Crea  guarantors
        /// </summary>
        /// <param name="guarantors"></param>
        /// <returns></returns>
        public static List<MOCOUP.InsuredGuaranteePrefix> CreateInsuredGuaranteePrefixies(List<InsuredGuaranteePrefixDTO> insuredGuaranteePrefixies)
        {
            var objs = new List<MOCOUP.InsuredGuaranteePrefix>();
            foreach (var insuredGuaranteePrefix in insuredGuaranteePrefixies)
            {
                objs.Add(ModelAssembler.CreateInsuredGuaranteePrefix(insuredGuaranteePrefix));
            }
            return objs;
        }
        #endregion

        #endregion

        #region SarlafatPersonNatural
        public static IndividualSarlaft CreateSarlaftPErsonNatural(IndividualSarlaftDTO individualSarlaft)
        {
            return new IndividualSarlaft
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
                AuthorizedBy = individualSarlaft.AuthorizedBy,
                UserId = individualSarlaft.UserId,
                FinancialSarlaft = new FinancialSarlaf
                {
                    AssetsAmount = individualSarlaft.finacialSarlaft.AssetsAmount,
                    ExpenseAmount = individualSarlaft.finacialSarlaft.ExpenseAmount,
                    ExtraIncomeAmount = individualSarlaft.finacialSarlaft.ExtraIncomeAmount,
                    IncomeAmount = individualSarlaft.finacialSarlaft.IncomeAmount,
                    IsForeignTransaction = individualSarlaft.finacialSarlaft.IsForeignTransaction,
                    ForeignTransactionAmount = individualSarlaft.finacialSarlaft.ForeignTransactionAmount,
                    LiabilitiesAmount = individualSarlaft.finacialSarlaft.LiabilitiesAmount,
                    SarlaftId = individualSarlaft.finacialSarlaft.SarlaftId,
                    Description = individualSarlaft.finacialSarlaft.Description,
                }
            };
        }

        public static List<IndividualSarlaft> CreateSarlaftPErsonNaturals(List<IndividualSarlaftDTO> individualSarlaft)
        {
            var IndividualSarlaftDTO = new List<IndividualSarlaft>();
            foreach (var item in individualSarlaft)
            {
                IndividualSarlaftDTO.Add(CreateSarlaftPErsonNatural(item));
            }
            return IndividualSarlaftDTO;
        }




        #endregion

        #region PaymentMehtod
        internal static List<CompanyIndividualPaymentMethod> CreateIndividualpaymentMethods(List<IndividualPaymentMethodDTO> individualpaymentMethodDTO)
        {
            List<CompanyIndividualPaymentMethod> individualPaymentMethods = new List<CompanyIndividualPaymentMethod>();

            foreach (IndividualPaymentMethodDTO item in individualpaymentMethodDTO)
            {
                individualPaymentMethods.Add(CreateIndividualpaymentMethod(item));
            }

            return individualPaymentMethods;
        }

        internal static CompanyIndividualPaymentMethod CreateIndividualpaymentMethod(IndividualPaymentMethodDTO individualpaymentMethodDTO)
        {
            return new CompanyIndividualPaymentMethod()
            {
                Id = individualpaymentMethodDTO.Id,
                Account = individualpaymentMethodDTO.Account == null ? null : CreatePaymentMethodAccount(individualpaymentMethodDTO.Account),
                Method = new CompanyPaymentMethod()
                {
                    Id = Convert.ToInt32(individualpaymentMethodDTO.Method.Id),
                    Description = individualpaymentMethodDTO.Method.Description
                }
            };
        }

        internal static CompanyPaymentAccount CreatePaymentMethodAccount(PaymentAccountDTO paymentAccountDTO)
        {
            return new CompanyPaymentAccount()
            {
                Number = Convert.ToDecimal(paymentAccountDTO.Number),
                Type = paymentAccountDTO.Type == null ? null : new CompanyPaymentAccountType()
                {
                    Id = Convert.ToInt32(paymentAccountDTO.Type.Id),
                    Description = paymentAccountDTO.Type.Description
                },
                //Currency = new Core.Application.CommonService.Models.Currency()
                //{
                //    Id = Convert.ToInt32(paymentAccountDTO.Currency.Id),
                //    Description = paymentAccountDTO.Currency.Description
                //},
                BankBranch = paymentAccountDTO.BankBranch == null ? null : new CompanyBankBranch()
                {
                    Id = Convert.ToInt32(paymentAccountDTO.BankBranch.Id),
                    Description = paymentAccountDTO.BankBranch.Description,
                    Bank = new CompanyBank()
                    {
                        Id = Convert.ToInt32(paymentAccountDTO.Bank?.Id ?? 0),
                        Description = paymentAccountDTO.Bank?.Description
                    }
                }
            };
        }

        #endregion

        #region insuredGuarantee

        internal static CompanyInsuredGuaranteeMortgage CreateInsuredGuaranteeMortgage(InsuredGuaranteeMortgageDTO insuredGuaranteeMortgage)
        {

            return new CompanyInsuredGuaranteeMortgage()
            {
                Id = insuredGuaranteeMortgage.Id,
                IndividualId = insuredGuaranteeMortgage.IndividualId,
                Address = insuredGuaranteeMortgage.Address,
                AppraisalAmount = insuredGuaranteeMortgage.AppraisalAmount,
                AppraisalDate = insuredGuaranteeMortgage.AppraisalDate,
                AssetType = new CompanyAssetType()
                {
                    Code = Convert.ToInt32(insuredGuaranteeMortgage.AssetType.Id),
                    Description = insuredGuaranteeMortgage.Description
                },
                Branch = new Core.Application.CommonService.Models.Branch()
                {
                    Id = Convert.ToInt32(insuredGuaranteeMortgage.Branch.Id),
                    Description = insuredGuaranteeMortgage.Branch.Description
                },
                BuiltAreaQuantity = insuredGuaranteeMortgage.BuiltAreaQuantity,
                City = new Core.Application.CommonService.Models.City()
                {
                    Id = Convert.ToInt32(insuredGuaranteeMortgage.City.Id),
                    Description = insuredGuaranteeMortgage.City.Description,
                    State = new Core.Application.CommonService.Models.State()
                    {
                        Id = Convert.ToInt32(insuredGuaranteeMortgage.State.Id),
                        Description = insuredGuaranteeMortgage.State.Description,
                        Country = new Core.Application.CommonService.Models.Country()
                        {
                            Id = Convert.ToInt32(insuredGuaranteeMortgage.Country.Id),
                            Description = insuredGuaranteeMortgage.Country.Description
                        }
                    },
                },
                ClosedInd = insuredGuaranteeMortgage.ClosedInd,
                Currency = new Core.Application.CommonService.Models.Currency()
                {
                    Id = Convert.ToInt32(insuredGuaranteeMortgage.Currency.Id),
                    Description = insuredGuaranteeMortgage.Currency.Description
                },
                Description = insuredGuaranteeMortgage.Description,
                ExpertName = insuredGuaranteeMortgage.ExpertName,
                Guarantee = new Guarantee()
                {
                    Id = insuredGuaranteeMortgage.Guarantee.Id,
                    Description = insuredGuaranteeMortgage.Guarantee.Description,
                    HasApostille = insuredGuaranteeMortgage.Guarantee.HasApostille,
                    HasPromissoryNote = insuredGuaranteeMortgage.Guarantee.HasPromissoryNote,
                    Type = new GuaranteeType()
                    {
                        Id = Convert.ToInt32(insuredGuaranteeMortgage.Guarantee.GuaranteeType.Id),
                        Description = insuredGuaranteeMortgage.Guarantee.GuaranteeType.Description
                    }
                },
                InsuranceCompany = insuredGuaranteeMortgage.InsuranceCompany.Description,
                InsuranceCompanyId = Convert.ToInt32(insuredGuaranteeMortgage.InsuranceCompany.Id),
                LastChangeDate = insuredGuaranteeMortgage.LastChangeDate,
                InsuranceValueAmount = insuredGuaranteeMortgage.InsuranceValueAmount,
                MeasureAreaQuantity = insuredGuaranteeMortgage.MeasureAreaQuantity,
                MeasurementType = new CompanyMeasurementType()
                {
                    Id = Convert.ToInt32(insuredGuaranteeMortgage.MeasurementType.Id),
                    Description = insuredGuaranteeMortgage.MeasurementType.Description
                },
                PolicyNumber = insuredGuaranteeMortgage.PolicyNumber,
                RegistrationNumber = insuredGuaranteeMortgage.RegistrationNumber,
                Status = new GuaranteeStatus()
                {
                    Id = Convert.ToInt32(insuredGuaranteeMortgage.Status.Id),
                    Description = insuredGuaranteeMortgage.Status.Description
                },
                RegistrationDate = insuredGuaranteeMortgage.RegistrationDate
            };
        }

        internal static CompanyInsuredGuaranteePromissoryNote CreateInsuredGuaranteePromissoryNote(InsuredGuaranteePromissoryNoteDTO insuredGuaranteePromissoryNote)
        {
            return new CompanyInsuredGuaranteePromissoryNote()
            {
                Id = insuredGuaranteePromissoryNote.Id,
                Branch = new Core.Application.CommonService.Models.Branch()
                {
                    Id = Convert.ToInt32(insuredGuaranteePromissoryNote.Branch.Id),
                    Description = insuredGuaranteePromissoryNote.Branch.Description
                },
                Description = insuredGuaranteePromissoryNote.Description,
                City = new Core.Application.CommonService.Models.City()
                {
                    Id = Convert.ToInt32(insuredGuaranteePromissoryNote.City.Id),
                    Description = insuredGuaranteePromissoryNote.City.Description,
                    State = new Core.Application.CommonService.Models.State()
                    {
                        Id = Convert.ToInt32(insuredGuaranteePromissoryNote.State.Id),
                        Description = insuredGuaranteePromissoryNote.State.Description,
                        Country = new Core.Application.CommonService.Models.Country()
                        {
                            Id = Convert.ToInt32(insuredGuaranteePromissoryNote.Country.Id),
                            Description = insuredGuaranteePromissoryNote.Country.Description
                        }
                    },
                },
                ClosedInd = insuredGuaranteePromissoryNote.ClosedInd,
                ConstitutionDate = insuredGuaranteePromissoryNote.ConstitutionDate,
                Currency = new Core.Application.CommonService.Models.Currency()
                {
                    Id = Convert.ToInt32(insuredGuaranteePromissoryNote.Currency.Id),
                    Description = insuredGuaranteePromissoryNote.Currency.Description
                },
                DocumentNumber = insuredGuaranteePromissoryNote.DocumentNumber,
                DocumentValueAmount = insuredGuaranteePromissoryNote.DocumentValueAmount,
                ExtDate = insuredGuaranteePromissoryNote.ExtDate,
                Guarantee = new Guarantee()
                {
                    Id = insuredGuaranteePromissoryNote.Guarantee.Id,
                    Description = insuredGuaranteePromissoryNote.Guarantee.Description,
                    HasApostille = insuredGuaranteePromissoryNote.Guarantee.HasApostille,
                    HasPromissoryNote = insuredGuaranteePromissoryNote.Guarantee.HasPromissoryNote,
                    Type = new GuaranteeType()
                    {
                        Id = Convert.ToInt32(insuredGuaranteePromissoryNote.Guarantee.GuaranteeType.Id),
                        Description = insuredGuaranteePromissoryNote.Guarantee.GuaranteeType.Description
                    }
                },
                IndividualId = insuredGuaranteePromissoryNote.IndividualId,
                LastChangeDate = insuredGuaranteePromissoryNote.LastChangeDate,
                PromissoryNoteType = new CompanyPromissoryNoteType()
                {
                    Id = Convert.ToInt32(insuredGuaranteePromissoryNote.PromissoryNoteType.Id),
                    Description = insuredGuaranteePromissoryNote.PromissoryNoteType.Description
                },
                RegistrationDate = insuredGuaranteePromissoryNote.RegistrationDate,
                SignatoriesNumber = insuredGuaranteePromissoryNote.SignatoriesNumber,
                Status = new GuaranteeStatus()
                {
                    Id = Convert.ToInt32(insuredGuaranteePromissoryNote.Status.Id),
                    Description = insuredGuaranteePromissoryNote.Status.Description
                }
            };
        }

        internal static CompanyInsuredGuaranteePledge CreateInsuredGuaranteePledge(InsuredGuaranteePledgeDTO insuredGuaranteePledge)
        {
            return new CompanyInsuredGuaranteePledge()
            {
                AppraisalAmount = insuredGuaranteePledge.AppraisalAmount,
                AppraisalDate = insuredGuaranteePledge.AppraisalDate,
                Branch = new Core.Application.CommonService.Models.Branch()
                {
                    Id = Convert.ToInt32(insuredGuaranteePledge.Branch.Id),
                    Description = insuredGuaranteePledge.Branch.Description
                },
                ChassisNumer = insuredGuaranteePledge.ChassisNumer,
                City = new Core.Application.CommonService.Models.City()
                {
                    Id = Convert.ToInt32(insuredGuaranteePledge.City.Id),
                    Description = insuredGuaranteePledge.City.Description,
                    State = new Core.Application.CommonService.Models.State()
                    {
                        Id = Convert.ToInt32(insuredGuaranteePledge.State.Id),
                        Description = insuredGuaranteePledge.State.Description,
                        Country = new Core.Application.CommonService.Models.Country()
                        {
                            Id = Convert.ToInt32(insuredGuaranteePledge.Country.Id),
                            Description = insuredGuaranteePledge.Country.Description
                        }
                    },

                },
                ClosedInd = insuredGuaranteePledge.ClosedInd,
                Currency = new Core.Application.CommonService.Models.Currency()
                {
                    Id = Convert.ToInt32(insuredGuaranteePledge.Currency.Id),
                    Description = insuredGuaranteePledge.Currency.Description
                },
                Description = insuredGuaranteePledge.Description,
                Id = insuredGuaranteePledge.Id,
                EngineNumer = insuredGuaranteePledge.EngineNumer,
                Guarantee = new Guarantee()
                {
                    Id = insuredGuaranteePledge.Guarantee.Id,
                    Description = insuredGuaranteePledge.Guarantee.Description,
                    HasApostille = insuredGuaranteePledge.Guarantee.HasApostille,
                    HasPromissoryNote = insuredGuaranteePledge.Guarantee.HasPromissoryNote,
                    Type = new GuaranteeType()
                    {
                        Id = Convert.ToInt32(insuredGuaranteePledge.Guarantee.GuaranteeType.Id),
                        Description = insuredGuaranteePledge.Guarantee.GuaranteeType.Description
                    }
                },
                IndividualId = insuredGuaranteePledge.IndividualId,
                InsuranceCompany = insuredGuaranteePledge.InsuranceCompany.Description,
                InsuranceCompanyId = Convert.ToInt32(insuredGuaranteePledge.InsuranceCompany.Id),
                InsuranceValueAmount = insuredGuaranteePledge.InsuranceValueAmount,
                LastChangeDate = insuredGuaranteePledge.LastChangeDate,
                LicensePlate = insuredGuaranteePledge.LicensePlate,
                PolicyNumber = insuredGuaranteePledge.PolicyNumber,
                RegistrationDate = insuredGuaranteePledge.RegistrationDate,
                Status = new GuaranteeStatus()
                {
                    Id = Convert.ToInt32(insuredGuaranteePledge.Status.Id),
                    Description = insuredGuaranteePledge.Status.Description
                }
            };
        }

        internal static CompanyInsuredGuaranteeFixedTermDeposit CreateInsuredGuaranteeFixedTermDeposit(InsuredGuaranteeFixedTermDepositDTO guaranteeFixedTermDeposit)
        {
            return new CompanyInsuredGuaranteeFixedTermDeposit()
            {
                Id = guaranteeFixedTermDeposit.Id,
                Branch = new Core.Application.CommonService.Models.Branch()
                {
                    Id = Convert.ToInt32(guaranteeFixedTermDeposit.Branch.Id),
                    Description = guaranteeFixedTermDeposit.Branch.Description
                },
                Description = guaranteeFixedTermDeposit.Description,
                City = new Core.Application.CommonService.Models.City()
                {
                    Id = Convert.ToInt32(guaranteeFixedTermDeposit.City.Id),
                    Description = guaranteeFixedTermDeposit.City.Description,
                    State = new Core.Application.CommonService.Models.State()
                    {
                        Id = Convert.ToInt32(guaranteeFixedTermDeposit.State.Id),
                        Description = guaranteeFixedTermDeposit.State.Description,
                        Country = new Core.Application.CommonService.Models.Country()
                        {
                            Id = Convert.ToInt32(guaranteeFixedTermDeposit.Country.Id),
                            Description = guaranteeFixedTermDeposit.Country.Description
                        }
                    },
                },
                ClosedInd = guaranteeFixedTermDeposit.ClosedInd,
                ConstitutionDate = guaranteeFixedTermDeposit.ConstitutionDate,
                Currency = new Core.Application.CommonService.Models.Currency()
                {
                    Id = Convert.ToInt32(guaranteeFixedTermDeposit.Currency.Id),
                    Description = guaranteeFixedTermDeposit.Currency.Description
                },
                DocumentNumber = guaranteeFixedTermDeposit.DocumentNumber,
                DocumentValueAmount = guaranteeFixedTermDeposit.DocumentValueAmount,
                ExtDate = guaranteeFixedTermDeposit.ExtDate,
                Guarantee = new Guarantee()
                {
                    Id = guaranteeFixedTermDeposit.Guarantee.Id,
                    Description = guaranteeFixedTermDeposit.Guarantee.Description,
                    HasApostille = guaranteeFixedTermDeposit.Guarantee.HasApostille,
                    HasPromissoryNote = guaranteeFixedTermDeposit.Guarantee.HasPromissoryNote,
                    Type = new GuaranteeType()
                    {
                        Id = Convert.ToInt32(guaranteeFixedTermDeposit.Guarantee.GuaranteeType.Id),
                        Description = guaranteeFixedTermDeposit.Guarantee.GuaranteeType.Description
                    }
                },
                IndividualId = guaranteeFixedTermDeposit.IndividualId,
                LastChangeDate = guaranteeFixedTermDeposit.LastChangeDate,
                RegistrationDate = guaranteeFixedTermDeposit.RegistrationDate,
                Status = new GuaranteeStatus()
                {
                    Id = Convert.ToInt32(guaranteeFixedTermDeposit.Status.Id),
                    Description = guaranteeFixedTermDeposit.Status.Description
                },
                IssuerName = guaranteeFixedTermDeposit.IssuerName
            };
        }

        internal static CompanyInsuredGuaranteeOthers CreateInsuredGuaranteeOthers(InsuredGuaranteeOthersDTO guaranteePromissoryOthers)
        {
            return new CompanyInsuredGuaranteeOthers()
            {
                Branch = new Core.Application.CommonService.Models.Branch()
                {
                    Id = Convert.ToInt32(guaranteePromissoryOthers.Branch.Id),
                    Description = guaranteePromissoryOthers.Branch.Description
                },
                ClosedInd = guaranteePromissoryOthers.ClosedInd,
                DescriptionOthers = guaranteePromissoryOthers.DescriptionOthers,
                Id = guaranteePromissoryOthers.Id,
                Guarantee = new Guarantee()
                {
                    Id = guaranteePromissoryOthers.Guarantee.Id,
                    Description = guaranteePromissoryOthers.Guarantee.Description,
                    HasApostille = guaranteePromissoryOthers.Guarantee.HasApostille,
                    HasPromissoryNote = guaranteePromissoryOthers.Guarantee.HasPromissoryNote,
                    Type = new GuaranteeType()
                    {
                        Id = Convert.ToInt32(guaranteePromissoryOthers.Guarantee.GuaranteeType.Id),
                        Description = guaranteePromissoryOthers.Guarantee.GuaranteeType.Description
                    }
                },
                IndividualId = guaranteePromissoryOthers.IndividualId,
                RegistrationDate = guaranteePromissoryOthers.RegistrationDate,
                LastChangeDate = guaranteePromissoryOthers.LastChangeDate,
                Status = new GuaranteeStatus()
                {
                    Id = Convert.ToInt32(guaranteePromissoryOthers.Status.Id),
                    Description = guaranteePromissoryOthers.Status.Description
                }
            };
        }

        #endregion

        #region CreateCompanyCoInsured
        public static COMP.CompanyCoInsured CreateCompanyCoInsureds(CompanyCoInsuredDTO companyCoInsuredDTO)
        {
            return new COMP.CompanyCoInsured
            {
                AddressTypeCode = companyCoInsuredDTO.AddressTypeCode,
                Annotations = companyCoInsuredDTO.Annotations,
                CityCode = companyCoInsuredDTO.CityCode,
                CountryCode = companyCoInsuredDTO.CountryCode,
                Description = companyCoInsuredDTO.Description,
                EnsureInd = companyCoInsuredDTO.EnsureInd,
                EnteredDate = companyCoInsuredDTO.EnteredDate,
                InsuraceCompanyId = companyCoInsuredDTO.InsuraceCompanyId,
                ModifyDate = companyCoInsuredDTO.ModifyDate,
                PhoneNumber = companyCoInsuredDTO.PhoneNumber,
                PhoneTypeCode = companyCoInsuredDTO.PhoneTypeCode,
                StateCode = companyCoInsuredDTO.StateCode,
                Street = companyCoInsuredDTO.Street,
                TributaryIdNo = companyCoInsuredDTO.TributaryIdNo,
                ComDeclinedTypeCode = companyCoInsuredDTO.ComDeclinedTypeCode,
                DeclinedDate = companyCoInsuredDTO.DeclinedDate,
                IndividualId = companyCoInsuredDTO.IndividualId,
                IvaTypeCode = companyCoInsuredDTO.IvaTypeCode

            };
        }

        #endregion

        #region ThirdPerson
        public static CompanyThird CreateThird(ThirdPartyDTO supplier)
        {
            var result = new CompanyThird();

            result.EnteredDate = supplier.CreationDate;
            result.DeclinedDate = supplier.DeclinationDate;
            result.Id = supplier.Id;
            result.IndividualId = supplier.IndividualId;
            result.ModificationDate = supplier.ModificationDate;
            result.Annotation = supplier.Annotation;
            result.DeclinedTypeId = supplier.DeclinedTypeId;



            return result;
        }
        #endregion

        #region Employee
        public static CompanyEmployee CreateEmployee(EmployeeDTO employee)
        {
            var result = new CompanyEmployee();

            result.BranchId = employee.BranchId;
            result.EgressDate = employee.EgressDate;
            result.EntryDate = employee.EntryDate;
            result.FileNumber = employee.FileNumber;
            result.IndividualId = employee.IndividualId;
            result.Annotation = employee.Annotation;
            result.ModificationDate = employee.ModificationDate;
            result.DeclinedTypeId = employee.DeclinedTypeId;
            result.Annotation = employee.Annotation;
            return result;
        }
        #endregion
        #region BusinessName
        /// <summary>
        /// Crear razon social
        /// </summary>
        /// <param name="listBusinessNameDTO"></param>
        /// <returns></returns>
        public static List<CompanyName> CreateBusinessNames(List<CompanyNameDTO> listBusinessNameDTO)
        {
            var result = new List<CompanyName>();
            foreach (CompanyNameDTO businessName in listBusinessNameDTO)
            {
                result.Add(CreateBusinessName(businessName));

            }
            return result;
        }

        /// <summary>
        /// Convierte un objeto CompanyNameDTO a CompanyCoCompanyName
        /// </summary>
        /// <param name="businessNameDTO"></param>
        /// <returns></returns>
        public static CompanyName CreateBusinessName(CompanyNameDTO businessNameDTO)
        {
            var result = new CompanyName();
            result = new CompanyName()
            {
                IndividualId = businessNameDTO.IndividualId,
                NameNum = businessNameDTO.NameNum,
                TradeName = businessNameDTO.TradeName,
                IsMain = businessNameDTO.IsMain,
                Enabled = businessNameDTO.Enabled,

                Phone = new Phone()
                {
                    Id = businessNameDTO.PhoneID
                },
                Address = new Address()
                {
                    Id = businessNameDTO.AddressID
                },
                Email = new Email()
                {
                    Id = businessNameDTO.EmailID
                }


            };

            return result;
        }


        #endregion

        #region BankTransfers

        /// <summary>
        /// Crear Transferencia Bancaria
        /// </summary>
        /// <param name="listBankTransfersDTO"></param>
        /// <returns></returns>
        public static List<BankTransfers> CreateBankTransfers(List<BankTransfersDTO> listBankTransfersDTO)
        {
            var result = new List<BankTransfers>();
            foreach (BankTransfersDTO bankTransfers in listBankTransfersDTO)
            {
                result.Add(CreateBankTransfers(bankTransfers));

            }
            return result;
        }


        /// <summary>
        /// Convierte un objeto CompanyNameDTO a CompanyCoCompanyName
        /// </summary>
        /// <param name="businessNameDTO"></param>
        /// <returns></returns>
        public static BankTransfers CreateBankTransfers(BankTransfersDTO bankTransfersDTO)
        {
            var result = new BankTransfers();
            result = new BankTransfers()
            {
                Individual = bankTransfersDTO.IndividualId,
                AccountNumber = bankTransfersDTO.AccountNumber,
                AccountType = new MOCOUP.AccountType {  Code= bankTransfersDTO.AccountTypeId},
                ActiveAccount = bankTransfersDTO.ActiveAccount,
                Bank = new COCOM.Bank { Id = bankTransfersDTO.BankId, Description = bankTransfersDTO.BankDescription },
                BankBranch = bankTransfersDTO.BankBranch,
                BankSquare = bankTransfersDTO.BankSquare,
                Currency = new COCOM.Currency { Id = bankTransfersDTO.CurrencyId, Description = bankTransfersDTO.CurrencyDescription },
                DefaultAccount = bankTransfersDTO.DefaultAccount,
                IntermediaryBank = bankTransfersDTO.IntermediaryBank,
                PaymentBeneficiary = bankTransfersDTO.PaymentBeneficiary,
                Id = bankTransfersDTO.Id,
                InscriptionDate = bankTransfersDTO.InscriptionDate
            };

            return result;
        }

        #endregion BankTransfers



        #region ElectronicBilling

        /// <summary>
        /// Crear Lista de Responsabilidades fiscales
        /// </summary>
        /// <param name="listBankTransfersDTO"></param>
        /// <returns></returns>
        public static List<InsuredFiscalResponsibility> CreateListInsuredFiscalResponsibility(List<InsuredFiscalResponsibilityDTO> listInsuredFiscalResponsibilityDTO)
        {
            var result = new List<InsuredFiscalResponsibility>();
            foreach (InsuredFiscalResponsibilityDTO fiscalResponsibilityDTO in listInsuredFiscalResponsibilityDTO)
            {
                result.Add(CreateInsuredFiscalResponsibility(fiscalResponsibilityDTO));

            }
            return result;
        }


        /// <summary>
        /// Convierte un objeto InsuredFiscalResponsibilityDTO a InsuredFiscalResponsibility
        /// </summary>
        /// <param name="businessNameDTO"></param>
        /// <returns></returns>
        public static InsuredFiscalResponsibility CreateInsuredFiscalResponsibility(InsuredFiscalResponsibilityDTO insuredFiscalDTO)
        {
            var result = new InsuredFiscalResponsibility();
            result = new InsuredFiscalResponsibility()
            {
                Id = insuredFiscalDTO.Id,
                IndividualId = insuredFiscalDTO.IndividualId,
                InsuredId = insuredFiscalDTO.InsuredCode,
                FiscalResponsabilityId = insuredFiscalDTO.FiscalResponsibilityId,
                FiscalResponsabilityDescription = insuredFiscalDTO.FiscalResponsibilityDescription,
                Code = insuredFiscalDTO.Code
            };
            return result;
        }

        /// <summary>
        /// Convierte un objeto InsuredFiscalResponsibilityDTO a CompanyInsuredFiscalResponsibility
        /// </summary>
        /// <param name="businessNameDTO"></param>
        /// <returns></returns>
        public static CompanyInsuredFiscalResponsibility CreateCompanyInsuredFiscalResponsibility(InsuredFiscalResponsibilityDTO insuredFiscalDTO)
        {
            var result = new CompanyInsuredFiscalResponsibility();
            result = new CompanyInsuredFiscalResponsibility()
            {
                Id = insuredFiscalDTO.Id,
                IndividualId = insuredFiscalDTO.IndividualId,
                InsuredId = insuredFiscalDTO.InsuredCode,
                FiscalResponsabilityId = insuredFiscalDTO.FiscalResponsibilityId,
                FiscalResponsabilityDescription = insuredFiscalDTO.FiscalResponsibilityDescription,
                Code = insuredFiscalDTO.Code
            };
            return result;
        }

        #endregion ElectronicBilling

        #region Politicas
        /// <summary>
        /// Crear persona 
        /// </summary>
        /// <param name="person">The person.</param>
        /// <returns></returns>
        public static CompanyPersonOperation CreatePersonOperation(PersonOperationDTO person)
        {
            var result = new CompanyPersonOperation();
            result.OperationId = person.OperationId;
            result.IndividualId = person.IndividualId;
            result.Operation = person.Operation;
            result.Process = person.Proccess;
            result.ProcessType = person.ProcessType;
            result.FunctionId = person.FunctionId;

            return result;
        }

        #endregion Politicas
    }
}
