using System;
using models = Sistran.Core.Application.UniquePersonService.V1.Models;
using modelsCommon = Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.Common.Entities;
using COMMEN = Sistran.Core.Application.Common.Entities;
using taxEntity = Sistran.Core.Application.Tax.Entities;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System.Collections.Generic;
using Sistran.Core.Application.Product.Entities;
using Sistran.Core.Application.UniquePersonService.V1.Enums;
using ENTV1 = Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Framework.Rules;
using Sistran.Core.Application.Utilities.RulesEngine;
using TAXEN = Sistran.Core.Application.Tax.Entities;
using INTEN = Sistran.Core.Application.Integration.Entities;
using Sistran.Core.Application.UniquePersonService.V1.DAOs;
using System.Linq;

namespace Sistran.Core.Application.UniquePersonService.V1.Assemblers
{
    public static class EntityAssembler
    {
        /// <summary>
        /// Dromero - 30/03/2015 
        /// Convierte un objeto de tipo Models.MassiveLoadFields a Entidad.MassiveLoadFields.
        /// </summary>
        /// <param name="massiveLoadFields">Objeto del tipo Models.MassiveLoadFields</param>
        /// <returns>Retorna un objeto del tipo Entidad.MassiveLoadFields</returns>
        public static IndividualPartner IndividualPartnerFields(models.Partner partNer)
        {

            return new IndividualPartner(partNer.PartnerId, partNer.IdentificationDocument.Number, partNer.IndividualId, partNer.IdentificationDocument.DocumentType.Id)
            {
                IdCardNo = partNer.IdentificationDocument.Number,
                IdCardTypeCode = partNer.IdentificationDocument.DocumentType.Id,
                TradeName = partNer.TradeName,
                Active = partNer.Active,
                IndividualId = partNer.IndividualId,
                PartnerId = partNer.PartnerId

            };
        }

        #region LegalRepresent

        public static IndividualLegalRepresent CreateLegalRepresent(models.LegalRepresentative legalRepresent)
        {
            return new IndividualLegalRepresent(legalRepresent.Id)
            {

                LegalRepresentativeName = legalRepresent.FullName,
                ExpeditionDate = legalRepresent.ExpeditionDate,
                ExpeditionPlace = legalRepresent.ExpeditionPlace,
                BirthDate = legalRepresent.BirthDate,
                BirthPlace = legalRepresent.BirthPlace,
                Nationality = legalRepresent.Nationality,
                City = legalRepresent.City.Description,
                Phone = String.IsNullOrEmpty(legalRepresent.Phone) ? null : (long?)Convert.ToInt64(legalRepresent.Phone),
                JobTitle = legalRepresent.JobTitle,
                CellPhone = String.IsNullOrEmpty(legalRepresent.CellPhone) ? null : (long?)Convert.ToInt64(legalRepresent.CellPhone),
                Email = legalRepresent.Email,
                Address = legalRepresent.Address,
                IdCardNo = legalRepresent.IdentificationDocument.Number,
                AuthorizationAmount = legalRepresent.AuthorizationAmount.Value,
                Description = legalRepresent.Description,
                CurrencyCode = legalRepresent.AuthorizationAmount.Currency.Id,
                IdCardTypeCode = legalRepresent.IdentificationDocument.DocumentType.Id,
                CountryCode = legalRepresent.City.State.Country.Id,
                StateCode = legalRepresent.City.State.Id,
                CityCode = legalRepresent.City.Id
            };

        }

        #endregion

        #region Phone

        public static Phone CreatePhone(models.Phone phone, int individualId)
        {
            return new Phone
            {
                IndividualId = individualId,
                PhoneTypeCode = phone.PhoneType.Id,
                PhoneNumber = Convert.ToInt64(phone.Description),
                IsMain = phone.IsMain,
                Extension = phone.Extension,
                CountryCode = phone.CountryCode,
                CityCode = phone.CityCode,
                ScheduleAvailability = phone.ScheduleAvailability
            };
        }

        #endregion

        #region Address

        public static Address CreateAddress(models.Address address)
        {
            return new Address
            {
                AddressTypeCode = address.AddressType.Id,
                IsMailingAddress = address.IsPrincipal,
                StreetTypeCode = 1,
                Street = address.Description,
                CityCode = address.City.Id,
                StateCode = address.City.State.Id,
                CountryCode = address.City.State.Country.Id,
            };

        }

        #endregion

        #region PersonJob
        public static PersonJob CreatePersonJob(models.LabourPerson personJob, int individualId)
        {
            int? def = null;
            return new PersonJob(individualId)
            {
                OccupationCode = personJob.Occupation.Id,
                IncomeLevelCode = personJob.IncomeLevel?.Id,
                CompanyName = personJob.CompanyName,
                JobSector = personJob.JobSector,
                Position = personJob.Position,
                Contact = personJob.Contact,
                CompanyPhone = personJob.CompanyPhone?.Id,
                SpecialityCode = personJob.Speciality?.Id,
                OtherOccupationCode = personJob.OtherOccupation?.Id
            };
        }

        #endregion

        #region Email

        public static Email CreateEmail(models.Email email)
        {
            return new Email
            {
                EmailTypeCode = email.EmailType.Id,
                Address = email.Description,
                IsMailingAddress = email.IsPrincipal
            };
        }

        #endregion

        #region Agent

        public static Agent CreateAgent(models.Agent agent)
        {
            int AgentTypeCod = 0;
            int? AgentDeclinedTypeCode;
            int? AgentSalesChannel = 0;
            int? AgentGroupCode = 0;
            int? AgentExecutive = 0;

            if (agent.AgentDeclinedType != null)
            {
                AgentDeclinedTypeCode = String.IsNullOrEmpty(agent.AgentDeclinedType.Id.ToString()) ? null : (int?)agent.AgentDeclinedType.Id;
            }
            else
            {
                AgentDeclinedTypeCode = null;
            }
            if (agent.AgentType != null)
            {
                AgentTypeCod = agent.AgentType.Id;
            }
            if (agent.GroupAgent != null)
            {
                AgentGroupCode = String.IsNullOrEmpty(agent.GroupAgent.Id.ToString()) ? null : (int?)agent.GroupAgent.Id;
            }
            if (agent.SalesChannel != null)
            {
                AgentSalesChannel = String.IsNullOrEmpty(agent.SalesChannel.Id.ToString()) ? null : (int?)agent.SalesChannel.Id;
            }
            if (agent.EmployeePerson != null)
            {
                AgentExecutive = String.IsNullOrEmpty(agent.EmployeePerson.Id.ToString()) ? null : (int?)agent.EmployeePerson.Id;
            }
            else
            {
                AgentExecutive = null;
            }

            return new Agent(agent.IndividualId)
            {
                AgentTypeCode = AgentTypeCod,
                CheckPayableTo = agent.FullName,
                AgentDeclinedTypeCode = AgentDeclinedTypeCode,
                EnteredDate = agent.DateCurrent,
                DeclinedDate = agent.DateDeclined,
                Annotations = agent.Annotations,
                ModifyDate = agent.DateModification,
                AgentGroupCode = AgentGroupCode,
                SalesChannelCode = AgentSalesChannel,
                Locker = agent.Locker,
                AccExecutiveIndId = AgentExecutive,
                CommissionDiscountAgreement = Convert.ToInt32(agent.CommissionDiscountAgreement)
            };
        }



        #endregion

        #region Person

        public static Person CreatePerson(Models.Person person)
        {
            return new Person
            {
                IndividualTypeCode = (int)person.IndividualType,
                EconomicActivityCode = person.EconomicActivity.Id,
                Gender = person.Gender,
                BirthDate = person.BirthDate == null ? default(DateTime) : (DateTime)person.BirthDate,
                BirthPlace = person.BirthPlace,
                Children = person.Children,
                Name = person.FullName,
                IdCardTypeCode = person.IdentificationDocument.DocumentType.Id,
                IdCardNo = person.IdentificationDocument.Number,
                Surname = person.SurName,
                MotherLastName = person.SecondSurName,
                SpouseName = person.SpouseName,
                MaritalStatusCode = person.MaritalStatus.Id,
                EducativeLevelCode = person.EducativeLevel != null ? person.EducativeLevel.Id != 0 ? person.EducativeLevel.Id : (int?)null : (int?)null,
                HouseTypeCode = person.HouseType != null ? person.HouseType.Id != 0 ? person.HouseType.Id : (int?)null : (int?)null,
                SocialLayerCode = person.SocialLayer != null ? person.SocialLayer.Id != 0 ? person.SocialLayer.Id : (int?)null : (int?)null,
                CheckPayable = person.CheckPayable,
                //todo ricardo
                DataProtection = person?.DataProtection ?? false
            };
        }

        internal static UserAssignedConsortium CreateUserAssignedConsortium(models.UserAssignedConsortium userAssignedConsortium)
        {
            return new UserAssignedConsortium(userAssignedConsortium.UserAssignedConsortiumId)
            {
                UserId = userAssignedConsortium.UserId,
                NitAssociationType = userAssignedConsortium.NitAssignedConsortium
            };
        }



        #endregion

        #region Company

        public static Company CreateCompany(Models.Company company)
        {
            return new Company
            {
                IndividualTypeCode = (int)company.IndividualType,
                EconomicActivityCode = company.EconomicActivity.Id,
                TradeName = company.FullName,
                TributaryIdTypeCode = company.IdentificationDocument.DocumentType.Id,
                TributaryIdNo = company.IdentificationDocument.Number,
                CountryCode = company.CountryId,
                CompanyTypeCode = company.CompanyType.Id,
                CheckPayable = company.CheckPayable
            };
        }

        public static CoCompany CreateCoCompany(Models.Company company)
        {
            return new CoCompany(company.IndividualId)
            {
                VerifyDigit = company.VerifyDigit,
                AssociationTypeCode = company.AssociationType.Id,
                NitAssociationType = company.IdentificationDocument.NitAssociationType
            };
        }


        #endregion

        #region Insured
        public static Insured CreateInsured(Models.Insured insured)
        {

            return new Insured()
            {
                IndividualId = insured.IndividualId,
                CheckPayableTo = insured.FullName,
                BranchCode = insured.Branch.Id,
                InsDeclinedTypeCode = insured.DeclinedType?.Id,
                Annotations = insured.Annotations,
                InsSegmentCode = insured.Segment.Id,
                InsProfileCode = insured.Profile.Id,
                IsMailAddress = (insured.IsMailAddress == true) ? 1 : 0,
                IsSms = (insured.IsSMS == true) ? 1 : 0,
                ReferredBy = insured.ReferedBy,
                EnteredDate = insured.EnteredDate,
                DeclinedDate = insured.DeclinedDate,
                ModifyDate = insured.ModifyDate,
                ElectronicBiller = insured.ElectronicBiller,
                RegimeType = insured.RegimeType,
            };
        }

        #endregion

        #region InsuredAgent
        public static InsuredAgent CreateInsuredAgent(int insuredId, Models.Agency agency)
        {
            return new InsuredAgent(insuredId, agency.Agent.IndividualId, agency.Id)
            {
                IsMain = agency.IsPrincipal
            };
        }

        #endregion

        #region PaymentMethodAccount
        public static PaymentMethodAccount CreatePaymentMethodAccount(Models.PaymentAccount paymentMethodAccount, int IndividualId, int paymenId)
        {
            return new PaymentMethodAccount(IndividualId, paymenId)
            {
                AccountNumber = paymentMethodAccount.Number,
                BankCode = paymentMethodAccount.BankBranch.Bank.Id,
                CurrencyCode = (paymentMethodAccount.Currency == null) ? 0 : paymentMethodAccount.Currency.Id,
                PaymentAccountTypeCode = paymentMethodAccount.Type.Id,
                IndividualId = IndividualId,
                PaymentId = paymenId,
                BankBranchNumber = paymentMethodAccount.BankBranch.Id
            };
        }

        #endregion

        #region ProspectNatural
        public static Prospect CreateProspectNatural(Models.ProspectNatural prospectNatural)
        {
            return new Prospect
            {

                IndividualTypeCode = prospectNatural.IndividualTyepCode,
                CountryCode = prospectNatural.CountryCode,
                StateCode = prospectNatural.StateCode,
                Surname = prospectNatural.Surname,
                Name = prospectNatural.Name,
                Gender = prospectNatural.Gender,
                MaritalStatusCode = prospectNatural.MaritalStatus,
                BirthDate = prospectNatural.BirthDate,
                CityCode = prospectNatural.CityCode,
                IdCardTypeCode = prospectNatural.IdCardTypeCode,//para valores null de ciudad
                IdCardNo = prospectNatural.IdCardNo,
                AddressTypeCode = prospectNatural.AddressType,
                EmailAddress = prospectNatural.EmailAddress,
                PhoneNumber = prospectNatural.PhoneNumber,
                Street = prospectNatural.Street,
                MotherLastName = prospectNatural.MotherLastName,
                AdditionalInfo = prospectNatural.AdditionalInfo,
                ProspectId = prospectNatural.ProspectCode,
                TributaryIdNo = prospectNatural.TributaryIdNumber,
                TributaryIdTypeCode = prospectNatural.TributaryIdTypeCode,
                TradeName = prospectNatural.TradeName
            };
        }
        #endregion

        #region Prospecto Juridico
        public static Prospect CreateProspectLegal(Models.ProspectNatural prospectlegal)
        {
            return new Prospect
            {
                IndividualTypeCode = prospectlegal.IndividualTyepCode,
                TradeName = prospectlegal.Name,
                TributaryIdTypeCode = prospectlegal.TributaryIdTypeCode,
                TributaryIdNo = prospectlegal.TributaryIdNumber,
                CityCode = prospectlegal.CityCode,
                StateCode = prospectlegal.StateCode,
                CountryCode = prospectlegal.CountryCode,
                CompanyTypeCode = prospectlegal.CompanyTypeCode,
                Street = prospectlegal.Street,
                AddressTypeCode = prospectlegal.AddressType,
                PhoneNumber = prospectlegal.PhoneNumber,
                EmailAddress = prospectlegal.EmailAddress,
                ProspectId = prospectlegal.ProspectCode,
                AdditionalInfo = prospectlegal.AdditionalInfo
            };
        }
        #endregion

        #region CoCompany
        public static CoCompany CreateCoCompany(Models.CompanyExtended coCompany, int individualId)
        {
            return new CoCompany(individualId)
            {
                VerifyDigit = coCompany.VerifyDigit,
                AssociationTypeCode = coCompany.AssociationType.Id
            };
        }
        #endregion

        #region  PersonIndividualType
        public static PersonIndividualType CreatePersonIndividualType(Models.PersonType personIndividualType, int individualId)
        {
            return new PersonIndividualType(individualId)
            {
                PersonTypeCode = personIndividualType.Id
            };

        }
        #endregion

        #region CoCompanyName
        public static CoCompanyName CreateCoCompanyName(Models.CompanyName companyName)
        {
            CoCompanyName companyNameEntity = new CoCompanyName(companyName.IndividualId, companyName.NameNum)
            {
                TradeName = companyName.TradeName,
                IsMain = companyName.IsMain,
                AddressDataCode = companyName.Address.Id,
                NameNum = companyName.NameNum,
                Enabled = companyName.Enabled,
            };

            if (companyName.Phone != null && companyName.Phone.Id > 0)
            {
                companyNameEntity.PhoneDataCode = companyName.Phone.Id;
            }

            if (companyName.Email != null && companyName.Email.Id > 0)
            {
                companyNameEntity.EmailDataCode = companyName.Email.Id;
            }

            return companyNameEntity;
        }
        #endregion

        #region PersonAccountBank
        public static PersonAccountBank CreatePersonAccountBank(Models.BankTransfers companyBankTransfers)
        {
            return new PersonAccountBank(companyBankTransfers.Id, companyBankTransfers.Individual)
            {
                IndividualId = companyBankTransfers.Individual,
                AccountBankCode = companyBankTransfers.Id,
                AccountTypeCode = companyBankTransfers.AccountType.Code,
                Active = companyBankTransfers.ActiveAccount,
                BankBranch = companyBankTransfers.BankBranch,
                BankCode = companyBankTransfers.Bank.Id,
                BankIntermediary = companyBankTransfers.IntermediaryBank,
                BankSquare = companyBankTransfers.BankSquare,
                CurrencyCode = companyBankTransfers.Currency.Id,
                DefaultAccount = companyBankTransfers.DefaultAccount,
                Number = companyBankTransfers.AccountNumber,
                Beneficiary = companyBankTransfers.PaymentBeneficiary,
                InscriptionDate = companyBankTransfers.InscriptionDate,
            };
        }


        public static List<PersonAccountBank> CreatePersonAccountBank(List<Models.BankTransfers> companyBankTransfers)
        {
            List<PersonAccountBank> personAccountBanks = new List<PersonAccountBank>();
            foreach (Models.BankTransfers bank in companyBankTransfers)
            {
                personAccountBanks.Add(CreatePersonAccountBank(bank));
            }
            return personAccountBanks;
        }
        #endregion PersonAccountBank


        #region ElectronicBilling
        public static InsuredFiscalResponsibility CreateInsuredFiscalResponsibility(Models.InsuredFiscalResponsibility companyInsuredFiscalResponsibility)
        {
            return new InsuredFiscalResponsibility(companyInsuredFiscalResponsibility.Id, companyInsuredFiscalResponsibility.IndividualId)
            {
                Id = companyInsuredFiscalResponsibility.Id,
                IndividualId = companyInsuredFiscalResponsibility.IndividualId,
                InsuredCode = companyInsuredFiscalResponsibility.InsuredId,
                FiscalResponsibilityId = companyInsuredFiscalResponsibility.FiscalResponsabilityId
            };
        }


        public static List<InsuredFiscalResponsibility> CreateListInsuredFiscalResponsibility(List<Models.InsuredFiscalResponsibility> companyInsuredFiscalResponsibility)
        {
            List<InsuredFiscalResponsibility> fiscalResponsibilities = new List<InsuredFiscalResponsibility>();
            foreach (Models.InsuredFiscalResponsibility fiscal in companyInsuredFiscalResponsibility)
            {
                fiscalResponsibilities.Add(CreateInsuredFiscalResponsibility(fiscal));
            }
            return fiscalResponsibilities;
        }
        #endregion ElectronicBilling


        #region Agency

        public static AgentAgency CreateAgency(models.Agency agency, int individualId)
        {
            int? AgentDeclinedTypeId = null;
            if (agency.AgentDeclinedType != null)
            {
                AgentDeclinedTypeId = agency.AgentDeclinedType.Id;
            }
            var agencyEntity = new AgentAgency(individualId)
            {
                AgentTypeCode = agency.AgentType.Id,
                AgentDeclinedTypeCode = AgentDeclinedTypeId,
                DeclinedDate = agency?.DateDeclined ?? null,
                BranchCode = agency.Branch.Id,
                Description = agency.FullName,
                AgentCode = agency.Code,
                Annotations = agency?.Annotations ?? null
            };
            if (agency.Id > 0)
                agencyEntity.AgentAgencyId = agency.Id;
            else
            {
                AgencyDAO agencyInd = new AgencyDAO();
                List<Models.Agency> exist = agencyInd.GetAgencyByIndividualId(individualId);
                if (exist != null && exist.Count > 0)
                {
                    var max = exist.Select(x => x.Id).Max();
                    var result = max + 1;
                    agencyEntity.AgentAgencyId = result;
                }
                else
                {
                    agencyEntity.AgentAgencyId = 1;
                }
            }
            return agencyEntity;
        }


        public static List<AgentAgency> CreateAgency(List<models.Agency> agency, int individualId)
        {
            List<AgentAgency> agentAgencies = new List<AgentAgency>();
            foreach (var item in agency)
            {
                agentAgencies.Add(CreateAgency(item, individualId));
            }
            return agentAgencies;
        }


        #endregion

        #region AgentPrefix

        public static AgentPrefix CreateAgentPrefix(modelsCommon.Base.BasePrefix agentPrefix, int IndividualId)
        {
            return new AgentPrefix(agentPrefix.Id, IndividualId);
        }

        #endregion

        #region InsuredConcept
        public static InsuredConcept CreateInsuredConcept(Models.InsuredConcept insuredConcept)
        {
            return new InsuredConcept(insuredConcept.InsuredCode)
            {
                IsInsured = insuredConcept.IsInsured,
                IsHolder = insuredConcept.IsHolder,
                IsBeneficiary = insuredConcept.IsBeneficiary,
                IsConsortium = insuredConcept.IsConsortium == null ? false : insuredConcept.IsConsortium,
                IsPayer = insuredConcept.IsPayer,
                IsRepresentative = insuredConcept.IsRepresentative == null ? false : insuredConcept.IsRepresentative,
                IsSurety = insuredConcept.IsSurety == null ? false : insuredConcept.IsSurety
            };
        }

        #endregion

        #region PaymentMethod

        public static IndividualPaymentMethod CreatePaymentMethod(Models.IndividualPaymentMethod individualPaymentMethod, int individualId)
        {
            return new IndividualPaymentMethod()
            {
                PaymentId = Convert.ToInt32(individualPaymentMethod.Id),
                PaymentMethodCode = individualPaymentMethod.Method.Id,
                IndividualId = individualId,
                RoleCode = (int)RolesType.Insured,
                Enabled = true
            };
        }

        #endregion

        #region InsuredGuaranteeLog

        public static InsuredGuaranteeLog CreateInsuredGuaranteeLog(Models.InsuredGuaranteeLog insuredGuaranteeLog)
        {
            return new InsuredGuaranteeLog(insuredGuaranteeLog.IndividualId, insuredGuaranteeLog.GuaranteeId, insuredGuaranteeLog.GuaranteeStatusCode, insuredGuaranteeLog.UserId, DateTime.Now)
            {
                Description = insuredGuaranteeLog.Description
            };
        }

        #endregion

        #region InsuredGuarantee

        public static InsuredGuarantee CreateInsuredGuarantee(Models.InsuredGuarantee insuredGuarantee)
        {
            return new InsuredGuarantee(insuredGuarantee.IndividualId, insuredGuarantee.Id)
            {

                CountryCode = insuredGuarantee.City?.State?.Country?.Id,
                CityCode = insuredGuarantee.City?.Id,
                StateCode = insuredGuarantee.City?.State?.Id,
                CurrencyCode = insuredGuarantee.Currency?.Id,
                Address = insuredGuarantee.Address,
                Apostille = false,
                BranchCode = insuredGuarantee.Branch.Id,
                PromissoryNoteTypeCode = insuredGuarantee?.PromissoryNoteType?.Id,
                Description = insuredGuarantee.Description,
                DocumentValueAmount = insuredGuarantee.DocumentValueAmount,
                GuaranteeCode = insuredGuarantee.Code,
                GuaranteeId = insuredGuarantee.Id,
                ClosedInd = insuredGuarantee.IsCloseInd,
                RegistrationDate = insuredGuarantee.RegistrationDate,
                SignatoriesNum = insuredGuarantee.SignatoriesNumber,
                LastChangeDate = insuredGuarantee.LastChangeDate,
                GuaranteeStatusCode = insuredGuarantee.Status.Id,
                AppraisalDate = insuredGuarantee.AppraisalDate,
                ExpertName = insuredGuarantee.ExpertName,
                InsuranceValueAmount = insuredGuarantee.InsuranceAmount,
                GuaranteePolicyNumber = insuredGuarantee.PolicyNumber,
                AppraisalAmount = insuredGuarantee.AppraisalAmount,
                MeasurementTypeCode = insuredGuarantee?.MeasurementType?.Id,
                InsuranceCompanyId = Convert.ToInt16(insuredGuarantee.InsuranceCompany?.Id),
                InsuranceCompany = insuredGuarantee.InsuranceCompany?.Description,
                AssetTypeCode = insuredGuarantee.AssetTypeCode,
                IssuerName = insuredGuarantee.IssuerName,
                DocumentNumber = insuredGuarantee.DocumentNumber,
                ExpDate = insuredGuarantee.ExpirationDate,
                LineBusinessCode = insuredGuarantee.BusinessLineCode,
                RegistrationNumber = insuredGuarantee.RegistrationNumber,
                LicensePlate = insuredGuarantee.LicensePlate,
                EngineSerNro = insuredGuarantee.EngineNro,
                ChassisSerNo = insuredGuarantee.ChassisNro,
                GuaranteeDescriptionOthers = insuredGuarantee.DescriptionOthers,
                MeasureAreaQuantity = insuredGuarantee.MeasureArea,





            };
        }

        public static InsuredGuarantee CreateInsuredGuaranteeMortgage(models.InsuredGuaranteeMortgage insuredGuaranteeMortgage)
        {
            return new InsuredGuarantee(insuredGuaranteeMortgage.Id, insuredGuaranteeMortgage.Guarantee.Id)
            {

                GuaranteeId = insuredGuaranteeMortgage.Id,
                IndividualId = insuredGuaranteeMortgage.IndividualId,
                Description = insuredGuaranteeMortgage.Description,

                Apostille = insuredGuaranteeMortgage.Guarantee.HasApostille,
                AppraisalAmount = insuredGuaranteeMortgage.AppraisalAmount,
                AppraisalDate = insuredGuaranteeMortgage.AppraisalDate,
                ExpertName = insuredGuaranteeMortgage.ExpertName,
                AssetTypeCode = insuredGuaranteeMortgage.AssetType.Code,
                RegistrationNumber = insuredGuaranteeMortgage.RegistrationNumber,
                InsuranceValueAmount = Convert.ToDecimal(insuredGuaranteeMortgage.InsuranceValueAmount),
                MeasureAreaQuantity = insuredGuaranteeMortgage.MeasureAreaQuantity,
                BuiltAreaQuantity = insuredGuaranteeMortgage.BuiltAreaQuantity,
                MeasurementTypeCode = insuredGuaranteeMortgage.MeasurementType.Id,
                InsuranceCompanyId = Convert.ToInt16(insuredGuaranteeMortgage.InsuranceCompanyId),
                InsuranceCompany = insuredGuaranteeMortgage.InsuranceCompany,
                GuaranteePolicyNumber = insuredGuaranteeMortgage.PolicyNumber,
                SignatoriesNum = insuredGuaranteeMortgage.SignatoriesNumber,

                Address = insuredGuaranteeMortgage.Address,
                GuaranteeCode = insuredGuaranteeMortgage.Guarantee.Id,
                BranchCode = insuredGuaranteeMortgage.Branch.Id,
                CountryCode = insuredGuaranteeMortgage.City.State.Country.Id,
                CityCode = insuredGuaranteeMortgage.City.Id,
                StateCode = insuredGuaranteeMortgage.City.State.Id,
                CurrencyCode = insuredGuaranteeMortgage.Currency.Id,
                ClosedInd = insuredGuaranteeMortgage.IsCloseInd,
                RegistrationDate = insuredGuaranteeMortgage.RegistrationDate,
                LastChangeDate = insuredGuaranteeMortgage.LastChangeDate,
                GuaranteeStatusCode = insuredGuaranteeMortgage.Status.Id,

            };
        }

        public static InsuredGuarantee CreateInsuredGuaranteePledge(models.InsuredGuaranteePledge insuredGuaranteePledge)
        {
            return new InsuredGuarantee(insuredGuaranteePledge.Id, insuredGuaranteePledge.Id)
            {

                GuaranteeId = insuredGuaranteePledge.Id,
                Description = insuredGuaranteePledge.Description,
                IndividualId = insuredGuaranteePledge.IndividualId,

                Address = insuredGuaranteePledge.Address,
                GuaranteeCode = insuredGuaranteePledge.Guarantee.Id,
                BranchCode = insuredGuaranteePledge.Branch.Id,
                CityCode = insuredGuaranteePledge.City.Id,
                StateCode = insuredGuaranteePledge.City.State.Id,
                CountryCode = insuredGuaranteePledge.City.State.Country.Id,
                CurrencyCode = insuredGuaranteePledge.Currency.Id,
                ClosedInd = insuredGuaranteePledge.IsCloseInd,
                RegistrationDate = insuredGuaranteePledge.RegistrationDate,
                LastChangeDate = insuredGuaranteePledge.LastChangeDate,
                GuaranteeStatusCode = insuredGuaranteePledge.Status.Id,
                Apostille = insuredGuaranteePledge.Guarantee.HasApostille,

                AppraisalAmount = insuredGuaranteePledge.AppraisalAmount,
                AppraisalDate = insuredGuaranteePledge.AppraisalDate,
                LicensePlate = insuredGuaranteePledge.LicensePlate,
                EngineSerNro = insuredGuaranteePledge.EngineNumer,
                ChassisSerNo = insuredGuaranteePledge.ChassisNumer,
                InsuranceCompany = insuredGuaranteePledge.InsuranceCompany,
                InsuranceCompanyId = Convert.ToInt16(insuredGuaranteePledge.InsuranceCompanyId),
                GuaranteePolicyNumber = insuredGuaranteePledge.PolicyNumber,
                InsuranceValueAmount = insuredGuaranteePledge.InsuranceValueAmount,
            };
        }

        public static InsuredGuarantee CreateInsuredGuaranteePromissoryNote(models.InsuredGuaranteePromissoryNote insuredGuaranteePromissoryNote)
        {
            return new InsuredGuarantee(insuredGuaranteePromissoryNote.Id, insuredGuaranteePromissoryNote.Id)
            {

                GuaranteeId = insuredGuaranteePromissoryNote.Id,
                Description = insuredGuaranteePromissoryNote.Description,
                IndividualId = insuredGuaranteePromissoryNote.IndividualId,

                Address = insuredGuaranteePromissoryNote.Address,
                GuaranteeCode = insuredGuaranteePromissoryNote.Guarantee.Id,
                BranchCode = insuredGuaranteePromissoryNote.Branch.Id,
                CityCode = insuredGuaranteePromissoryNote.City.Id,
                StateCode = insuredGuaranteePromissoryNote.City.State.Id,
                CountryCode = insuredGuaranteePromissoryNote.City.State.Country.Id,
                CurrencyCode = insuredGuaranteePromissoryNote.Currency.Id,
                ClosedInd = insuredGuaranteePromissoryNote.IsCloseInd,
                RegistrationDate = insuredGuaranteePromissoryNote.RegistrationDate,
                LastChangeDate = insuredGuaranteePromissoryNote.LastChangeDate,
                GuaranteeStatusCode = insuredGuaranteePromissoryNote.Status.Id,
                Apostille = insuredGuaranteePromissoryNote.Guarantee.HasApostille,

                DocumentNumber = insuredGuaranteePromissoryNote.DocumentNumber,
                DocumentValueAmount = insuredGuaranteePromissoryNote.DocumentValueAmount,
                ExpDate = insuredGuaranteePromissoryNote.ExtDate,
                PromissoryNoteTypeCode = insuredGuaranteePromissoryNote.PromissoryNoteType.Id,
                SignatoriesNum = insuredGuaranteePromissoryNote.SignatoriesNumber,
                ConstitutionDate = insuredGuaranteePromissoryNote.ConstitutionDate,

            };
        }

        public static InsuredGuarantee CreateInsuredGuaranteeFixedTermDeposit(models.InsuredGuaranteeFixedTermDeposit insuredGuaranteeFixed)
        {
            return new InsuredGuarantee(insuredGuaranteeFixed.Id, insuredGuaranteeFixed.Id)
            {

                GuaranteeId = insuredGuaranteeFixed.Id,
                Description = insuredGuaranteeFixed.Description,
                IndividualId = insuredGuaranteeFixed.IndividualId,

                GuaranteeCode = insuredGuaranteeFixed.Guarantee.Id,
                BranchCode = insuredGuaranteeFixed.Branch.Id,
                CityCode = insuredGuaranteeFixed.City.Id,
                StateCode = insuredGuaranteeFixed.City.State.Id,
                CountryCode = insuredGuaranteeFixed.City.State.Country.Id,
                CurrencyCode = insuredGuaranteeFixed.Currency.Id,
                ClosedInd = insuredGuaranteeFixed.IsCloseInd,
                RegistrationDate = insuredGuaranteeFixed.RegistrationDate,
                LastChangeDate = insuredGuaranteeFixed.LastChangeDate,
                GuaranteeStatusCode = insuredGuaranteeFixed.Status.Id,
                Apostille = insuredGuaranteeFixed.Guarantee.HasApostille,

                DocumentNumber = insuredGuaranteeFixed.DocumentNumber,
                DocumentValueAmount = insuredGuaranteeFixed.DocumentValueAmount,
                ExpDate = insuredGuaranteeFixed.ExtDate,
                IssuerName = insuredGuaranteeFixed.IssuerName,
                ConstitutionDate = insuredGuaranteeFixed.ConstitutionDate,


            };
        }


        internal static InsuredGuarantee CreateInsuredGuaranteeOthers(models.InsuredGuaranteeOthers insuredGuaranteeOthers)
        {
            return new InsuredGuarantee(insuredGuaranteeOthers.IndividualId, insuredGuaranteeOthers.Id)
            {

                GuaranteeId = insuredGuaranteeOthers.Id,
                GuaranteeDescriptionOthers = insuredGuaranteeOthers.DescriptionOthers,
                IndividualId = insuredGuaranteeOthers.IndividualId,

                GuaranteeCode = insuredGuaranteeOthers.Guarantee.Id,
                BranchCode = insuredGuaranteeOthers.Branch.Id,
                ClosedInd = insuredGuaranteeOthers.IsCloseInd,
                RegistrationDate = insuredGuaranteeOthers.RegistrationDate,
                LastChangeDate = insuredGuaranteeOthers.LastChangeDate,
                GuaranteeStatusCode = insuredGuaranteeOthers.Status.Id,
                Apostille = insuredGuaranteeOthers.Guarantee.HasApostille,

            };
        }



        #endregion

        #region Guarantee

        public static Guarantee CreateGuarantee(Models.Guarantee guarantee)
        {
            return new Guarantee(guarantee.Id)
            {
                Apostille = guarantee.HasApostille,
                GuaranteeCode = guarantee.Id,
                GuaranteeTypeCode = guarantee.Type.Id,
                Description = guarantee.Description,
                PromissoryNoteTypeInd = guarantee.HasPromissoryNote
            };
        }

        #endregion

        #region Guarantor
        public static List<Guarantor> CreateGuarantors(List<models.Guarantor> guarantors)
        {
            List<Guarantor> Entitiesguarantors = new List<Guarantor>();
            foreach (models.Guarantor item in guarantors)
            {
                Entitiesguarantors.Add(CreateGuarantor(item));
            }
            return Entitiesguarantors;
        }


        public static Guarantor CreateGuarantor(Models.Guarantor guarantor)
        {
            return new Guarantor(guarantor.IndividualId, guarantor.GuaranteeId, guarantor.GuarantorId)
            {
                Adrress = guarantor.Adrress,
                IdCardNo = guarantor.CardNro,
                CityText = guarantor.CityText,
                GuaranteeId = guarantor.GuaranteeId,
                GuarantorId = guarantor.GuarantorId,
                IndividualId = guarantor.IndividualId,
                GuarantorName = guarantor.Name,
                PhoneNumber = guarantor.PhoneNumber,
                TradeName = guarantor.TradeName,
                TributaryIdNo = guarantor.TributaryIdNo
            };
        }

        #endregion

        #region InsuredGuaranteePrefix
        public static InsuredGuaranteePrefix CreateInsuredGuaranteePrefix(Models.InsuredGuaranteePrefix insuredGuaranteePrefix)
        {
            return new InsuredGuaranteePrefix(
                insuredGuaranteePrefix.IndividualId,
                insuredGuaranteePrefix.GuaranteeId,
                insuredGuaranteePrefix.PrefixCode)
            {
            };
        }
        #endregion

        #region InsuredGuaranteeDocumentation
        public static InsuredGuaranteeDocumentation CreateInsuredGuaranteeDocumentation(Models.InsuredGuaranteeDocumentation insuredGuaranteeDocumentation)
        {
            return new InsuredGuaranteeDocumentation(
                insuredGuaranteeDocumentation.IndividualId,
                insuredGuaranteeDocumentation.GuaranteeId,
                insuredGuaranteeDocumentation.GuaranteeCode,
                insuredGuaranteeDocumentation.DocumentCode)
            {
            };
        }
        #endregion

        #region Prospect

        public static Prospect CreateProspect(Models.Prospect prospect)
        {
            Prospect entityProspect = new Prospect()
            {
                IndividualTypeCode = (int)prospect.IndividualType,
                Street = prospect.CompanyName.Address.Description,
                PhoneNumber = Convert.ToInt64(prospect.CompanyName.Phone.Description),
                EmailAddress = prospect.CompanyName.Email.Description
            };

            if (prospect.IndividualType == IndividualType.Person)
            {
                entityProspect.Surname = prospect.Surname;
                entityProspect.MotherLastName = prospect.SecondSurname;
                entityProspect.Name = prospect.Name;
                entityProspect.Gender = prospect.Gender;
                entityProspect.BirthDate = prospect.BirthDate;
                entityProspect.IdCardNo = prospect.IdentificationDocument.Number;
                entityProspect.MaritalStatusCode = prospect.MaritalStatus;
                if (prospect.IdentificationDocument.DocumentType.Id > 0)
                {
                    entityProspect.IdCardTypeCode = prospect.IdentificationDocument.DocumentType.Id;
                }
            }
            else if (prospect.IndividualType == IndividualType.Company)
            {
                entityProspect.TradeName = prospect.TradeName;
                entityProspect.TributaryIdNo = prospect.IdentificationDocument.Number;
                if (prospect.IdentificationDocument.DocumentType.Id > 0)
                {
                    entityProspect.TributaryIdTypeCode = prospect.IdentificationDocument.DocumentType.Id;
                }
            }
            if (prospect.CompanyName != null && prospect.CompanyName.Address != null && prospect.CompanyName.Address.City != null)
            {
                if (prospect.CompanyName.Address.City.Id > 0)
                {
                    entityProspect.CityCode = prospect.CompanyName.Address.City.Id;
                    if (prospect.CompanyName.Address.City.State != null && prospect.CompanyName.Address.City.State.Id > 0)
                    {
                        entityProspect.StateCode = prospect.CompanyName.Address.City.State.Id;
                        if (prospect.CompanyName.Address.City.State.Country != null && prospect.CompanyName.Address.City.State.Country.Id > 0)
                        {
                            entityProspect.CountryCode = prospect.CompanyName.Address.City.State.Country.Id;
                        }
                    }
                }
            }
            if (prospect.CompanyName != null && prospect.CompanyName.Address != null && prospect.CompanyName.Address.AddressType != null)
            {
                entityProspect.AddressTypeCode = prospect.CompanyName.Address.AddressType.Id;
            }

            return entityProspect;
        }

        #endregion

        #region OperatingQuota

        public static OperatingQuota CreateOperatingQuota(Models.OperatingQuota operatingQuota)
        {
            return new OperatingQuota(operatingQuota.IndividualId, operatingQuota.LineBusinessId, operatingQuota.CurrencyId)
            {
                OperatingQuotaAmount = operatingQuota.Amount,
                CurrentTo = operatingQuota.CurrentTo
            };
        }

        #endregion

        #region Supplier
        public static Supplier CreateSupplier(Models.Supplier supplier)
        {
            var supplierEntity = new Supplier()
            {
                SupplierProfileCode = supplier.Profile.Id,
                SupplierTypeCode = supplier.Type.Id,
                EnteredDate = supplier.EnteredDate,
                DeclinedDate = supplier.DeclinedDate,
                DeclinedReason = supplier.DeclinedReason,
                Enabled = supplier.Enabled == null ? false : supplier.Enabled,
                CheckPayableTo = supplier.CheckPayableTo,
                OrderCheck = supplier.OrderCheck == null ? false : supplier.OrderCheck,
                IndividualId = supplier.IndividualId,
                Name = supplier.Name,
                ModificationDate = supplier.ModificationDate,
                Observation = supplier.Observation
            };
            if (supplier.Id > 0)
            {
                supplierEntity.SupplierCode = supplier.Id;
            }

            if (supplier.PaymentAccountType != null)
            {
                supplierEntity.PaymentAccountTypeCode = supplier.PaymentAccountType.Id;
            }

            if (supplier.DeclinedType != null)
            {
                supplierEntity.SupplierDeclinedTypeCode = supplier.DeclinedType.Id;
            }

            return supplierEntity;
        }

        public static ProviderSpeciality CreateProviderSpeciality(Models.ProviderSpeciality providerSpeciality)
        {
            return new ProviderSpeciality
            {
                SpecialityCode = providerSpeciality.SpecialityId,
                ProviderCode = providerSpeciality.ProviderId
            };
        }

        public static ProviderPaymentConcept CreateProviderPaymentConcept(Models.SupplierPaymentConcept providerPaymentConcept)
        {
            return new ProviderPaymentConcept
            {
                PaymentConceptCode = providerPaymentConcept.PaymentConcept.Id,
                ProviderCode = providerPaymentConcept.ProviderId
            };
        }


        public static SupplierGroupSupplier CreateSupplierAccountingConcept(Models.SupplierGroupSupplier supplierGroupSupplier)
        {
            return new SupplierGroupSupplier()
            {
                SupplierCode = supplierGroupSupplier.SupplierCd,
                GroupSupplierCode = supplierGroupSupplier.GroupSupplierCd
            };
        }

        public static SupplierAccountingConcept CreateSupplierAccountingConcept(Models.SupplierAccountingConcept supplierAccountingConcept)
        {
            return new SupplierAccountingConcept()
            {
                AccountingConceptCode = supplierAccountingConcept.AccountingConcept.Id,
                SupplierCode = supplierAccountingConcept.Supplier.Id
            };
        }





        #endregion

        #region Tax
        public static TAXEN.IndividualTax CreateIndividualTax(models.IndividualTax individualTax)
        {
            return new TAXEN.IndividualTax()
            {
                TaxRateCode = individualTax.TaxRate.Id,
                IndividualId = individualTax.IndividualId,
                RoleCode = individualTax.Role.Id
            };
        }

        public static taxEntity.IndividualTaxExemption CreateIndividualTaxExemption(Models.IndividualTaxExeption individualTaxExeption)
        {
            DateTime? Datefrom = individualTaxExeption.Datefrom;
            return new taxEntity.IndividualTaxExemption()
            {


                TaxCode = individualTaxExeption.TaxCode,
                StateCode = individualTaxExeption.StateCode.Id,
                TaxCategoryCode = individualTaxExeption.TaxCategory.Id,
                CurrentFrom = Datefrom == DateTime.MinValue ? null : Datefrom,
                IndividualId = individualTaxExeption.IndividualId,
                ExemptionPercentage = individualTaxExeption.ExtentPercentage,
                HasFullRetention = individualTaxExeption.TotalRetention,
                CurrentTo = individualTaxExeption.DateUntil == DateTime.MinValue ? null : individualTaxExeption.DateUntil,
                CountryCode = individualTaxExeption.CountryCode,
                BulletinDate = individualTaxExeption.OfficialBulletinDate,
                ResolutionNumber = individualTaxExeption.ResolutionNumber




            };
        }

        #endregion

        #region IndividualRelationApp
        public static IndividualRelationApp CreateIndividualRelationApp(Models.IndividualRelationApp individualRelationApp)
        {
            return new IndividualRelationApp()
            {
                AgentAgencyId = individualRelationApp.Agency.Id,
                ChildIndividualId = individualRelationApp.ChildIndividual.IndividualId,
                RelationTypeCode = individualRelationApp.RelationTypeCd,
                ParentIndividualId = individualRelationApp.ParentIndividualId
            };
        }

        #endregion

        #region ReInsurer
        public static Reinsurer CreateReinsurer(models.ReInsurer reinsurer)
        {
            return new Reinsurer(reinsurer.IndividualId)
            {
                Annotations = reinsurer.Annotations,
                DeclinedDate = reinsurer.DeclinedDate,
                DeclinedTypeCode = reinsurer.DeclaredTypeCD,
                EnteredDate = reinsurer.EnteredDate,
                IndividualId = reinsurer.IndividualId,
                ModifyDate = reinsurer.ModifyDate
            };
        }

        #endregion

        #region AgentCommission

        public static ENTV1.AgencyCommissRate CreateAgentCommission(models.Commission commissionAgent, int individualId)
        {
            //int AgentAgencyId = 0;
            int PrefixId = 0;
            int LineBusinessId = 0;
            int SubLineBusinessId = 0;
            if (commissionAgent.Prefix != null)
            {
                PrefixId = commissionAgent.Prefix.Id;
            }
            if (commissionAgent.LineBusiness != null)
            {
                LineBusinessId = commissionAgent.LineBusiness.Id;
            }
            if (commissionAgent.SubLineBusiness != null)
            {
                SubLineBusinessId = commissionAgent.SubLineBusiness.Id;
            }
            return new ENTV1.AgencyCommissRate()
            {
                IndividualId = individualId,
                AgentAgencyId = commissionAgent.AgentAgencyId,
                PrefixCode = PrefixId,
                LineBusinessCode = LineBusinessId,
                SubLineBusinessCode = SubLineBusinessId,
                StCommissPercentage = commissionAgent.PercentageCommission,
                AdditCommissPercentage = commissionAgent.PercentageAdditional,

            };
        }
        #endregion

        #region "PersonInterest"

        public static PersonInterestGroup CreatePersonInterestGroup(Models.PersonInterestGroup personInterestGroup)
        {
            return new PersonInterestGroup(personInterestGroup.IndividualId, personInterestGroup.InterestGroupTypeId)
            {
                IndividualId = personInterestGroup.IndividualId,
                InterestGroupTypeCode = personInterestGroup.InterestGroupTypeId
            };
        }
        #endregion

        #region Consortium
        public static CoConsortium CreateConsortium(models.Consortium consortium)
        {
            return new CoConsortium(consortium.InsuredCode, consortium.IndividualId)
            {
                InsuredCode = consortium.InsuredCode,
                ConsortiumId = consortium.ConsortiumId,
                Enabled = consortium.Enabled,
                IsMain = consortium.Ismain,
                ParticipationRate = consortium.ParticipationRate,
                StartDate = consortium.StartDate
            };
        }

        public static List<CoConsortium> CreateConsortiums(List<models.Consortium> consortium)
        {
            var result = new List<CoConsortium>();
            foreach (var item in consortium)
            {
                result.Add(EntityAssembler.CreateConsortium(item));
            }
            return result;
        }
        #endregion

        #region CompanyCoInsured
        /// <summary>
        /// Se encarga de Mappear del Modelo cargado Hasta la entidad 
        /// </summary>
        /// <param name="model">Datos del Modelo.</param>
        /// <returns></returns>
        public static CoInsuranceCompany CreateCompanyCoInsured(models.CompanyCoInsured model)
        {
            return new CoInsuranceCompany
            {
                AddressTypeCode = model.AddressTypeCode,
                Annotations = model.Annotations,
                CityCode = model.CityCode,
                CountryCode = model.CountryCode,
                Description = model.Description,
                EnsureInd = model.EnsureInd,
                EnteredDate = model.EnteredDate,
                InsuranceCompanyId = model.InsuraceCompanyId,
                ModifyDate = model.ModifyDate,
                PhoneNumber = model.PhoneNumber,
                PhoneTypeCode = model.PhoneTypeCode,
                Street = model.Street,
                TributaryIdNo = model.TributaryIdNo,
                ComDeclinedTypeCode = model.ComDeclinedTypeCode,
                DeclinedDate = model.DeclinedDate,
                IndividualId = model.IndividualId,
                StateCode = model.StateCode,
                IvaTypeCode = (decimal)(model?.IvaTypeCode ?? 0)
            };
        }

        internal static Role CreateRole(models.Role role)
        {
            return new Role(role.Id)
            {
                RoleCode = role.Id,
                Description = role.Description
            };
        }

        internal static PaymentAccountType CreatePaymentAccountType(models.PaymentAccountType paymentAccountType)
        {
            return new PaymentAccountType(paymentAccountType.Id)
            {
                Description = paymentAccountType.Description,
                PaymentAccountTypeCode = paymentAccountType.Id
            };
        }
        #endregion

        /// <summary>
        /// Convierte modelo a entidad del servicio
        /// </summary>
        /// <param name="DocumentsTypeRange"></param>
        /// <returns></returns>
        public static DocumentsTypeRange CreateDocumentTypeRange(models.DocumentTypeRange documentTypeRange)
        {
            return new DocumentsTypeRange(documentTypeRange.Id)
            {
                DocumentsTypeRangeCode = documentTypeRange.Id,
                IdCardTypeCode = documentTypeRange.CardTypeCode.Id,
                Gender = documentTypeRange.Gender,
                IdCardNoFrom = documentTypeRange.CardNumberFrom,
                IdCardNoTo = documentTypeRange.CardNumberTo
            };
        }
        #region InsuredSegment
        /// <summary>
        /// Mapear Modelo InsuredSegment
        /// </summary>
        /// <param name="insuredProfile">Modelo InsuredSegment</param>
        /// <returns>Entidad InsuredSegment</returns>
        public static InsuredSegment CreateInsuredSegment(Models.InsuredSegmentV1 insuredProfile)
        {
            return new InsuredSegment(insuredProfile.Id)
            {
                SmallDescription = insuredProfile.ShortDescription,
                Description = insuredProfile.LongDescription,
                InsSegmentCode = insuredProfile.Id
            };
        }
        #endregion
        public static CoProspect CreateCoProspect(int ProspectId, int VerifyDigit = -1)
        {
            if (VerifyDigit == -1)
            {
                return new CoProspect(ProspectId)
                {
                    ProspectId = ProspectId,
                    VerifyDigit = null
                };

            }
            else
            {
                return new CoProspect(ProspectId)
                {
                    ProspectId = ProspectId,
                    VerifyDigit = VerifyDigit
                };
            }
        }

        public static COMMEN.StatusRoute CreateStatusRoute(int AssignedGuaranteeStatusCd, int GuaranteeStatusCd)
        {
            return new COMMEN.StatusRoute(GuaranteeStatusCd, AssignedGuaranteeStatusCd);
        }

        #region InsuredProfile
        /// <summary>
        /// Mapear el Modelo InsuredProfile a la entidad InsuredProfile
        /// </summary>
        /// <param name="insuredProfile">Modelo InsuredProfile</param>
        /// <returns>Entidad InsuredProfile</returns>
        public static InsuredProfile CreateInsuredProfile(Models.InsuredProfile insuredProfile)
        {
            return new InsuredProfile(insuredProfile.Id)
            {
                SmallDescription = insuredProfile.SmallDescription,
                Description = insuredProfile.Description,
                InsProfileCode = insuredProfile.Id
            };
        }
        #endregion InsuredProfile

        #region PersonFiscalResponsibility
        public static FiscalResponsibility CreatePersonFiscalResponsibility(Models.FiscalResponsibility fiscalResponsibility)
        {
            return new FiscalResponsibility(fiscalResponsibility.Id)
            {
                Id = fiscalResponsibility.Id,
                Code = fiscalResponsibility.Code,
                Description = fiscalResponsibility.Description

            };
        }


        public static List<FiscalResponsibility> CreateListPersonFiscalResponsibility(List<Models.FiscalResponsibility> fiscalResponsibility)
        {
            List<FiscalResponsibility> personFiscalResponsibility = new List<FiscalResponsibility>();
            foreach (Models.FiscalResponsibility fiscal in fiscalResponsibility)
            {
                personFiscalResponsibility.Add(CreatePersonFiscalResponsibility(fiscal));
            }
            return personFiscalResponsibility;
        }
        #endregion PersonFiscalResponsibility

        /// <summary>
        /// Convierte modelo a entidad del servicio
        /// </summary>
        /// <param name="DocumentsTypeRange"></param>
        /// <returns></returns>
        public static EconomicGroup CreateEconomicGroup(models.EconomicGroup economicGroup)
        {
            return new EconomicGroup(economicGroup.EconomicGroupId)
            {
                //EconomicGroupId = economicGroup.EconomicGroupId > 0 ? economicGroup.EconomicGroupId : 0,
                EconomicGroupName = economicGroup.EconomicGroupName,
                TributaryIdTypeCode = economicGroup.TributaryIdType,
                TrubutaryIdNo = economicGroup.TributaryIdNo,
                VerifyDigit = economicGroup.VerifyDigit,
                EnteredDate = economicGroup.EnteredDate,
                OperatingQuotaAmount = economicGroup.OperationQuoteAmount,
                EnabledInd = economicGroup.Enabled,
                DeclinedDate = economicGroup.DeclinedDate
            };
        }

        /// <summary>
        /// Convierte modelo a entidad del servicio
        /// </summary>
        /// <param name="DocumentsTypeRange"></param>
        /// <returns></returns>
        public static EconomicGroupDetail CreateEconomicGroupDetail(models.EconomicGroupDetail economicGroupDetail)
        {
            return new EconomicGroupDetail(economicGroupDetail.EconomicGroupId, economicGroupDetail.EconomicGroupId)
            {
                EconomicGroupId = economicGroupDetail.EconomicGroupId,
                IndividualId = economicGroupDetail.IndividualId,
                EnabledInd = economicGroupDetail.Enabled,
                DeclinedDate = economicGroupDetail.DeclinedDate
            };
        }

        #region AuthorizationPolicies

        public static TmpPersonOperation CreatePersonOperation(Models.PersonOperation personOperation)
        {
            return new TmpPersonOperation()
            {
                IndividualId = personOperation.IndividualId,
                FunctionId = personOperation.FunctionId,
                Proccess = personOperation.Process,
                TypeProccess = personOperation.ProcessType,
                Operation = personOperation.Operation
            };
        }
        #endregion AuthorizationPolicies

        #region Punto de Control (Integracion)
        public static INTEN.UpPersonAccountBankControl CreatePersonAccountBankControl(Models.PersonAccountBankControl personAccountBankControl)
        {
            return new INTEN.UpPersonAccountBankControl()
            {
                IndividualId = personAccountBankControl.IndividualId,
                Action = personAccountBankControl.Action
            };
        }

        public static INTEN.UpIndividualControl CreateIndividualControl(Models.IndividualControl individualControl)
        {
            return new INTEN.UpIndividualControl(individualControl.Id)
            {
                IndividualId = individualControl.IndividualId,
                Action = individualControl.Action
            };
        }

        public static INTEN.UpInsuredControl CreateInsuredControl(Models.InsuredControl insuredControl)
        {
            return new INTEN.UpInsuredControl(insuredControl.Id)
            {
                IndividualId = insuredControl.IndividualId,
                InsuredCode = insuredControl.InsuredCode,
                Action = insuredControl.Action
            };
        }
        #endregion
    }
}
