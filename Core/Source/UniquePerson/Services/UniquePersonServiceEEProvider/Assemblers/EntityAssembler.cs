using System;
using models = Sistran.Core.Application.UniquePersonService.Models;
using modelsCommon = Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePerson.Entities;
using entitiesUPerson = Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Core.Application.Common.Entities;
using taxEntity = Sistran.Core.Application.Tax.Entities;
using Sistran.Core.Application.Product.Entities;

namespace Sistran.Core.Application.UniquePersonService.Assemblers
{
    public static class EntityAssembler
    {
        /// <summary>
        /// Dromero - 30/03/2015 
        /// Convierte un objeto de tipo Models.MassiveLoadFields a Entidad.MassiveLoadFields
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
                PartnerId= partNer.PartnerId

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
                ScheduleAvailability=phone.ScheduleAvailability,
                Extension=phone.Extension,
                CountryCode=phone.CountryCode,
                CityCode=phone.CityCode,
             
            };
        }

        #endregion

        #region Address

        public static Address CreateAddress(models.Address address)
        {
            int? City;
            int? State;
            int? Country;
            if (address.City.Id == 0)
            {
                City = null;
            }
            else
            {
                City = address.City.Id;
            }

            if (address.City.State.Id == 0)
            {
                State = null;
            }
            else
            {
                State = address.City.State.Id;
            }

            if (address.City.State.Country.Id == 0)
            {
                Country = null;
            }
            else
            {
                Country = address.City.State.Country.Id;
            }


            return new Address
            {
                AddressTypeCode = address.AddressType.Id,
                IsMailingAddress = address.IsMailAddress,
                StreetTypeCode = 1,
                Street = address.Description,
                CityCode = City,
                StateCode = State,
                CountryCode = Country
              
            };

        }

        #endregion

        #region PersonJob
        public static PersonJob CreatePersonJob(models.LaborPerson personJob, int individualId)
        {
            return new PersonJob(individualId)
            {
                OccupationCode = personJob.Occupation.Id,
                IncomeLevelCode = personJob.IncomeLevel.Id != 0 ? personJob.IncomeLevel.Id : null,
                CompanyName = personJob.CompanyName,
                JobSector = personJob.JobSector,
                Position = personJob.Position,
                Contact = personJob.Contact,
                CompanyPhone = personJob.CompanyPhone.Id != 0 ? personJob.CompanyPhone.Id : 0,
                SpecialityCode = personJob.Speciality.Id != 0 ? personJob.Speciality.Id : null,
                OtherOccupationCode = personJob.OtherOccupation.Id != 0 ? (int?)personJob.OtherOccupation.Id : null
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
                IsMailingAddress = email.IsMailingAddress
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
            int? AgentExecutive=0;

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
                Locker =agent.Locker,
                AccExecutiveIndId=AgentExecutive
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
                Name = person.Names,
                IdCardTypeCode = person.IdentificationDocument.DocumentType.Id,
                IdCardNo = person.IdentificationDocument.Number,
                Surname = person.Surname,
                MotherLastName = person.MotherLastName,
                SpouseName = person.SpouseName,
                MaritalStatusCode = person.MaritalStatus.Id,
                EducativeLevelCode = person.EducativeLevel != null ? person.EducativeLevel.Id != 0 ? person.EducativeLevel.Id : (int?)null : (int?)null,
                HouseTypeCode = person.HouseType != null ? person.HouseType.Id != 0 ? person.HouseType.Id : (int?)null : (int?)null,
                SocialLayerCode = person.SocialLayer != null ? person.SocialLayer.Id != 0 ? person.SocialLayer.Id : (int?)null : (int?)null,
                DataProtection = person.DataProtection
            };
        }

        #endregion

        #region Company

        public static Company CreateCompany(Models.Company company)
        {
            models.LegalRepresentative legalRepresentative = new models.LegalRepresentative();
            if (company.LegalRepresentative != null)
            {
                legalRepresentative.ManagerName = company.LegalRepresentative.ManagerName;
                legalRepresentative.GeneralManagerName = company.LegalRepresentative.GeneralManagerName;
                legalRepresentative.ContactName = company.LegalRepresentative.ContactName;
                legalRepresentative.ContactAdditionalInfo = company.LegalRepresentative.ContactAdditionalInfo;
            }

            return new Company
            {
                IndividualTypeCode = (int)company.IndividualType,
                EconomicActivityCode = company.EconomicActivity.Id,
                TradeName = company.Name,
                TributaryIdTypeCode = company.IdentificationDocument.DocumentType.Id,
                TributaryIdNo = company.IdentificationDocument.Number,
                CountryCode = company.CountryOrigin.Id,
                CompanyTypeCode = company.CompanyType.Id,
                ManagerName = legalRepresentative.ManagerName,
                GeneralManagerName = legalRepresentative.GeneralManagerName,
                ContactName = legalRepresentative.ContactName,
                ContactAdditionalInfo = legalRepresentative.ContactAdditionalInfo
            };
        }

        #endregion

        #region Insured
        public static Insured CreateInsured(Models.Insured insured)
        {
            int? insuredMainId;
            if(insured.InsuredMain != null)
            {
                insuredMainId = Convert.ToInt32(insured.InsuredMain);
            }
            else
            {
                insuredMainId = null;
            }
            return new Insured()
            {
                IndividualId = insured.IndividualId,
                CheckPayableTo = insured.Name,
                BranchCode = insured.BranchCode,
                InsDeclinedTypeCode = insured.InsDeclinesType,
                Annotations = insured.Annotations,
                InsSegmentCode = insured.Profile,
                InsProfileCode = insured.Profile,
                MainInsuredIndId = insuredMainId,
                IsCommercialClient = (insured.IsComercialClient == true) ? 1 : 0,
                IsMailAddress = (insured.IsMailAddress == true) ? 1 : 0,
                IsSms = (insured.IsSMS == true) ? 1 : 0,
                ReferredBy = insured.ReferedBy,
                EnteredDate = insured.EnteredDate,
                DeclinedDate = insured.DeclinedDate,                
                ModifyDate = insured.ModifyDate
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
        public static PaymentMethodAccount CreatePaymentMethodAccount(Models.PaymentMethodAccount paymentMethodAccount, int IndividualId)
        {
            return new PaymentMethodAccount(IndividualId, paymentMethodAccount.Id)
            {

                BankCode = paymentMethodAccount.Bank.Id,
                AccountNumber = paymentMethodAccount.AccountNumber,
                PaymentAccountTypeCode = paymentMethodAccount.AccountType.Id,
                BankBranchNumber = paymentMethodAccount.Bank.BankBranches == null ? 0 : paymentMethodAccount.Bank.BankBranches[0].Id

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
                AdditionalInfo = prospectNatural.AdditionalInfo

            };
        }

        public static CoProspect CreateCoProspect( int ProspectId, int VerifyDigit=-1)
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
        public static PersonIndividualType CreatePersonIndividualType(Models.PersonIndividualType personIndividualType)
        {
            return new PersonIndividualType(personIndividualType.IndividualId)
            {
                PersonTypeCode = personIndividualType.PersonTypeCode
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
                AddressDataCode = companyName.Address.Id
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

        #region Agency

        public static AgentAgency CreateAgency(models.Agency agency, int individualId)
        {
            int? AgentDeclinedTypeId = null;
            if (agency.AgentDeclinedType != null)
            {
                AgentDeclinedTypeId = agency.AgentDeclinedType.Id;
            }
            return new AgentAgency(individualId, agency.Id)
            {
                AgentTypeCode = agency.AgentType.Id,
                AgentDeclinedTypeCode = AgentDeclinedTypeId,
                DeclinedDate = agency.DateDeclined,
                AgentAgencyId = agency.Id,
                BranchCode = agency.Branch.Id,
                Description = agency.FullName,
                AgentCode = agency.Code,
                Annotations = agency.Annotations
            };
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
                IsConsortium = insuredConcept.IsConsortium,
                IsPayer = insuredConcept.IsPayer,
                IsRepresentative = insuredConcept.IsRepresentative,
                IsSurety = insuredConcept.IsSurety
            };
        }

        #endregion

        #region PaymentMethod

        public static IndividualPaymentMethod CreatePaymentMethod(Models.IndividualPaymentMethod paymentMethod)
        {
            return new IndividualPaymentMethod(paymentMethod.IndividualId, paymentMethod.PaymentId, paymentMethod.RoleId)
            {
                PaymentId = paymentMethod.PaymentId,
                PaymentMethodCode = paymentMethod.Id,
                Enabled = paymentMethod.Enabled,
                RoleCode = paymentMethod.RoleId
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
                Address = insuredGuarantee.Address,
                GuaranteeCode = insuredGuarantee.Code,
                AppraisalAmount = insuredGuarantee.AppraisalAmount,
                BuiltAreaQuantity = insuredGuarantee.BuiltArea,
                DeedNumber = insuredGuarantee.DeedNumber,
                GuaranteeDescriptionOthers = insuredGuarantee.DescriptionOthers,
                DocumentValueAmount = insuredGuarantee.DocumentValueAmount,
                MeasureAreaQuantity = insuredGuarantee.MeasureArea,
                RegistrationDate = insuredGuarantee.RegistrationDate,
                MortgagerName = insuredGuarantee.MortgagerName,
                DepositEntity = insuredGuarantee.DepositEntity,
                DepositDate = insuredGuarantee.DepositDate,
                Depositor = insuredGuarantee.Depositor,
                Constituent = insuredGuarantee.Constituent,
                Description = insuredGuarantee.Description,
                ClosedInd = insuredGuarantee.IsCloseInd,
                AppraisalDate = insuredGuarantee.AppraisalDate,
                ExpertName = insuredGuarantee.ExpertName,
                InsuranceValueAmount = insuredGuarantee.InsuranceAmount,
                GuaranteePolicyNumber = insuredGuarantee.PolicyNumber,
                Apostille = insuredGuarantee.Apostille,
                IssuerName = insuredGuarantee.IssuerName,
                DocumentNumber = insuredGuarantee.DocumentNumber,
                ExpDate = insuredGuarantee.ExpirationDate,
                LineBusinessCode = insuredGuarantee.BusinessLineCode,
                RegistrationNumber = insuredGuarantee.RegistrationNumber,
                LicensePlate = insuredGuarantee.LicensePlate,
                EngineSerNro = insuredGuarantee.EngineNro,
                ChassisSerNo = insuredGuarantee.ChassisNro,
                SignatoriesNum = insuredGuarantee.SignatoriesNumber,
                CountryCode = insuredGuarantee.Country != null ? (int?)insuredGuarantee.Country.Id : null,
                CityCode = insuredGuarantee.City != null ? (int?)insuredGuarantee.City.Id : null,
                StateCode = insuredGuarantee.State != null ? (int?)insuredGuarantee.State.Id : null,
                GuaranteeStatusCode = insuredGuarantee.GuaranteeStatus != null ? (int?)insuredGuarantee.GuaranteeStatus.Code : null,
                BranchCode = insuredGuarantee.Branch != null ? (int?)insuredGuarantee.Branch.Id : null,
                PromissoryNoteTypeCode = insuredGuarantee.PromissoryNoteType != null ? (int?)insuredGuarantee.PromissoryNoteType.Id : null,
                MeasurementTypeCode = insuredGuarantee.MeasurementType != null ? (int?)insuredGuarantee.MeasurementType.Code : null,
                CurrencyCode = insuredGuarantee.Currency != null ? (int?)insuredGuarantee.Currency.Id : null,
                InsuranceCompanyId = insuredGuarantee.InsuranceCompany != null ? (decimal?)insuredGuarantee.InsuranceCompany.Id : null,
                GuaranteeAmount = insuredGuarantee.GuaranteeAmount,
                LastChangeDate = insuredGuarantee.LastChangeDate,
                VehicleMakeCode = insuredGuarantee.VehicleMake,
                VehicleModelCode = insuredGuarantee.VehicleModel,
                VehicleVersionCode = insuredGuarantee.VehicleVersion,
                AssetTypeCode = insuredGuarantee.AssetTypeCode,
                InsuranceCompany = insuredGuarantee.InsuranceCompany != null ? insuredGuarantee.InsuranceCompany.Description : null,
                RealstateMatriculation = insuredGuarantee.RealstateMatriculation,
                ConstitutionDate = insuredGuarantee.ConstitutionDate
            };
        }

        #endregion

        #region Guarantee

        public static Guarantee CreateGuarantee(Models.Guarantee guarantee)
        {
            return new Guarantee(guarantee.Code)
            {
                Apostille = guarantee.Apostille,
                GuaranteeCode = guarantee.Code,
                GuaranteeTypeCode = guarantee.GuaranteeType.Code,
                Description = guarantee.Description
            };
        }

        #endregion

        #region Guarantor

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

            if (prospect.IndividualType == Enums.IndividualType.Person)
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
            else if (prospect.IndividualType == Enums.IndividualType.LegalPerson)
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
            return new OperatingQuota(operatingQuota.IndividualId, operatingQuota.LineBusiness.Id, operatingQuota.Amount.Currency.Id)
            {
                IndividualId = operatingQuota.IndividualId,
                LineBusinessCode = operatingQuota.LineBusiness.Id,
                CurrencyCode = operatingQuota.Amount.Currency.Id,
                OperatingQuotaAmount = operatingQuota.Amount.Value,
                CurrentTo = operatingQuota.CurrentTo
            };
        }

        #endregion

        #region Provider
        public static Provider CreateProvider(Models.Provider provider)
        {
            return new Provider
            {
                IndividualId = provider.IndividualId,
                ProviderTypeCode = provider.ProviderTypeId,
                OriginTypeCode = provider.OriginTypeId,
                ProviderDeclinedTypeCode = provider.ProviderDeclinedTypeId,
                CreationDate = provider.CreationDate,
                ModificationDate = provider.ModificationDate,
                DeclinationDate = provider.DeclinationDate,
                Observation = provider.Observation,
                SpecialityDefault = provider.SpecialityDefault
            };
        }

        public static ProviderSpeciality CreateProviderSpeciality(Models.ProviderSpeciality providerSpeciality)
        {
            return new ProviderSpeciality
            {
                SpecialityCode = providerSpeciality.SpecialityId,
                ProviderCode = providerSpeciality.ProviderId
            };
        }

        public static ProviderPaymentConcept CreateProviderPaymentConcept(Models.ProviderPaymentConcept providerPaymentConcept)
        {
            return new ProviderPaymentConcept
            {
                PaymentConceptCode = providerPaymentConcept.PaymentConcept.Id,
                ProviderCode = providerPaymentConcept.ProviderId
            };
        }
        #endregion

        #region Tax
        public static IndividualTax CreateIndividualTax(Models.IndividualTax individualTax)
        {
            return new IndividualTax
            {
                TaxCode = individualTax.Tax.Id,
                TaxConditionCode = individualTax.TaxCondition.Id,
                IndividualId = individualTax.IndividualId
            };
        }

        public static taxEntity.IndividualTaxExemption CreateIndividualTaxExemption(Models.IndividualTaxExeption individualTaxExeption)
        {

           return new taxEntity.IndividualTaxExemption()
           {
              
               
                TaxCode = individualTaxExeption.Tax.Id,               
                StateCode = individualTaxExeption.StateCode.Id,
                TaxCategoryCode = individualTaxExeption.TaxCategory.Id,
                CurrentFrom = individualTaxExeption.Datefrom,
                IndividualId = individualTaxExeption.IndividualId,
                ExemptionPercentage = individualTaxExeption.ExtentPercentage,
                HasFullRetention = individualTaxExeption.TotalRetention,               
                CurrentTo = individualTaxExeption.DateUntil,               
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
                ParentIndividualId = individualRelationApp.ParentIndividualId,
                IndividualRelationAppId = individualRelationApp.IndividualRelationAppId
            };
        }

        #endregion

        #region ReInsurer
        public static Reinsurer CreateReinsurer(models.ReInsurer reinsurer)
        {
            return new Reinsurer
            {
                Annotations = reinsurer.Annotations,
                DeclinedDate = reinsurer.DeclinedDate,
                DeclinedTypeCode = reinsurer.DeclaredTypeCD,
                EnteredDate = reinsurer.EnteredDate,
                IndividualId = reinsurer.IndividualId,
                ModifyDate = reinsurer.ModifyDate,
                ReinsurerCode = reinsurer.ReinsuredCD,
                IsActive = reinsurer.IsActive
            };
        }
        #endregion

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
                SmallDescription = insuredProfile.ShortDescription,
                Description = insuredProfile.LongDescription,
                InsProfileCode = insuredProfile.Id
            };
        }
        #endregion

        #region InsuredSegment
        /// <summary>
        /// Mapear Modelo InsuredSegment
        /// </summary>
        /// <param name="insuredProfile">Modelo InsuredSegment</param>
        /// <returns>Entidad InsuredSegment</returns>
        public static InsuredSegment CreateInsuredSegment(Models.InsuredSegment insuredProfile)
        {
            return new InsuredSegment(insuredProfile.Id)
            {
                SmallDescription = insuredProfile.ShortDescription,
                Description = insuredProfile.LongDescription,
                InsSegmentCode = insuredProfile.Id
            };
        }
        #endregion

        #region AgentCommission

        public static AgencyCommissRate CreateAgentCommission(models.CommissionAgent commissionAgent, int individualId)
        {
            int AgentAgencyId = 0;
            int PrefixId = 0;
            int LineBusinessId = 0;
            int SubLineBusinessId= 0;
            if (commissionAgent.Agency != null)
            { 
                AgentAgencyId = commissionAgent.Agency.Id;
            }
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
            return new AgencyCommissRate()
            {
                IndividualId = individualId,
                AgentAgencyId = AgentAgencyId,
                PrefixCode = PrefixId,
                LineBusinessCode = LineBusinessId,
                SubLineBusinessCode = SubLineBusinessId,
                StCommissPercentage = commissionAgent.PercentageCommission,
                AdditCommissPercentage = commissionAgent.PercentageAdditional
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
		
		#region Company.AddressType
        /// <summary>
        /// Convierte modelo a entidad del servicio
        /// </summary>
        /// <param name="companyAddressType">Modelo del tipo de dirección</param>
        /// <returns>Entidad del tipo de dirección</returns>
        public static CompanyAddressType CreateCompanyAddressType(Models.CompanyAddressType companyAddressType)
        {
            return new CompanyAddressType(0)
            {
                AddressTypeCode = companyAddressType.AddressTypeCode,
                SmallDescription = companyAddressType.SmallDescription,
                TinyDescription = companyAddressType.TinyDescription,
                IsElectronicMail = companyAddressType.IsElectronicMail
            };
        }
        #endregion

        #region Company.PhoneType
        /// <summary>
        /// Convierte modelo a entidad del servicio
        /// </summary>
        /// <param name="companyPhoneType">Modelo del tipo de teléfono</param>
        /// <returns>Entidad del tipo de teléfono</returns>
        public static CompanyPhoneType CreateCompanyPhoneType(Models.CompanyPhoneType companyPhoneType)
        {
            return new CompanyPhoneType(0)
            {
                PhoneTypeCode = companyPhoneType.PhoneTypeCode,
                Description = companyPhoneType.Description,
                SmallDescription = companyPhoneType.SmallDescription,
                IsCellphone = companyPhoneType.IsCellphone
            };
        }
        #endregion

        #region DocumentsTypeRange
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
        #endregion

    }
}
