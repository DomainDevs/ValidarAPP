using System;
using System.Collections.Generic;
using System.Linq;
using Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.UniquePerson.Entities;
using UPMOCORE = Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.Utilities.RulesEngine;
using Sistran.Core.Framework.Rules;
using entities = Sistran.Core.Application.UniquePersonV1.Entities;
using UPET = Sistran.Company.Application.UniquePerson.Entities;
using UPETCORE = Sistran.Core.Application.UniquePersonV1.Entities;
using INTEN = Sistran.Core.Application.Integration.Entities;

namespace Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Assemblers
{
    /// <summary>
    /// Convercion Emsamblados
    /// </summary>
    public static class EntityAssembler
    {
        #region Facades
        public static void CreateFacadeGeneralPerson(Facade facade, Models.CompanyPerson person, Models.CompanyCompany company, List<Models.CompanyAddress> addresses)
        {
            if (!string.IsNullOrEmpty(person?.FullName))
            {
                facade.SetConcept(RuleConceptGeneralPerson.IndividualType, (int)Core.Services.UtilitiesServices.Enums.IndividualType.Person);
                facade.SetConcept(RuleConceptPolicies.UserId, person.UserId);
                facade.SetConcept(RuleConceptGeneralPerson.DocumentType, person.IdentificationDocument?.DocumentType?.Id);
                facade.SetConcept(RuleConceptGeneralPerson.IndividualId, person.IndividualId);
                facade.SetConcept(RuleConceptGeneralPerson.DocumentNumber, person.IdentificationDocument?.Number);
                facade.SetConcept(RuleConceptGeneralPerson.Surname, person.SurName);
                facade.SetConcept(RuleConceptGeneralPerson.SecondSurname, person.SecondSurName);
                facade.SetConcept(RuleConceptGeneralPerson.Names, person.FullName);

            }

            if (!string.IsNullOrEmpty(company?.FullName))
            {
                facade.SetConcept(RuleConceptGeneralPerson.IndividualType, (int)Core.Services.UtilitiesServices.Enums.IndividualType.Company);
                facade.SetConcept(RuleConceptPolicies.UserId, company.UserId);
                facade.SetConcept(RuleConceptGeneralPerson.IndividualId, company.IndividualId);
                facade.SetConcept(RuleConceptGeneralPerson.BusinessName, company.FullName);
                facade.SetConcept(RuleConceptGeneralPerson.DocumentNumber, company.IdentificationDocument?.Number);
                facade.SetConcept(RuleConceptGeneralPerson.DocumentType, company.IdentificationDocument?.DocumentType?.Id);
                facade.SetConcept(RuleConceptGeneralPerson.AssociationTypePerson, company.AssociationType.Id);
            }

            Models.CompanyAddress address = addresses?.FirstOrDefault(x => x.IsPrincipal);
            if (address != null)
            {
                facade.SetConcept(RuleConceptGeneralPerson.CountryPerson, address.City?.State?.Country?.Id);
                facade.SetConcept(RuleConceptGeneralPerson.StatePerson, address.City?.State?.Id);
                facade.SetConcept(RuleConceptGeneralPerson.CityPerson, address.City?.Id);
            }
        }

        public static void CreateFacadeInsured(Facade facade, Models.CompanyInsured insured)
        {

        }

        public static void CreateFacadeProvider(Facade facade, Models.CompanySupplier companySupplier)
        {

        }

        public static void CreateFacadeThird(Facade facade, Models.CompanyThird companyThird)
        {

        }

        public static void CreateFacadeAgent(Facade facade, Models.CompanyAgent agent)
        {

        }
        
        public static void CreateFacadePersonalInformation(Facade facade, Models.CompanyLabourPerson labourPerson)
        {

        }

        public static void CreateFacadeEmployee(Facade facade, Models.CompanyEmployee companyEmployee)
        {

        }

        public static void CreateFacadePaymentMethod(Facade facade, Models.CompanyIndividualPaymentMethod companyPa)
        {

        }

        public static void CreateFacadeOperatingQuota(Facade facade, Models.CompanyOperatingQuota operationQuota)
        {

        }

        public static void CreateFacadeBankTransfers(Facade facade, UPMOCORE.BankTransfers bankTransfers)
        {

        }
        
        public static void CreateFacadeTaxes(Facade facade, Models.CompanyIndividualTax individualTax)
        {

        }
        

        public static void CreateFacadeReInsurer(Facade facade, Models.CompanyReInsurer bankTransfers)
        {

        }

        public static void CreateFacadeCoInsured(Facade facade, Models.CompanyCoInsured bankTransfers)
        {

        }

        public static void CreateFacadeBusinessName(Facade facade, UPMOCORE.CompanyName CompanyName)
        {

        }

        public static void CreateFacadeConsortium(Facade facade, Models.CompanyConsortium companyConsortium)
        {
            facade.SetConcept(RuleConceptConsortiates.DocumentNumberConsortium, companyConsortium.IdentificationDocument.Number);
            facade.SetConcept(RuleConceptConsortiates.IndividualIdConsortium, companyConsortium.IndividualId);
            facade.SetConcept(RuleConceptConsortiates.NameConsortium, companyConsortium.FullName);
        }

        public static void CreateFacadeGuarantee(Facade facade, UPMOCORE.Guarantee guarantee)
        {

        }

        public static void CreateFacadeGeneralPersonBasicInfo(Facade facade, Models.CompanyPerson person, Models.CompanyCompany company)
        {
            if (person?.IndividualType == Core.Services.UtilitiesServices.Enums.IndividualType.Person)
            {
                facade.SetConcept(RuleConceptGeneralBasicInfo.IndividualType, (int)Core.Services.UtilitiesServices.Enums.IndividualType.Person);
                facade.SetConcept(RuleConceptPolicies.UserId, person.UserId);
                facade.SetConcept(RuleConceptGeneralBasicInfo.DocumentType, person.IdentificationDocument?.DocumentType?.Id);
                facade.SetConcept(RuleConceptGeneralBasicInfo.IndividualId, person.IndividualId);
                facade.SetConcept(RuleConceptGeneralBasicInfo.DocumentNumber, person.IdentificationDocument?.Number);
                facade.SetConcept(RuleConceptGeneralBasicInfo.Surname, person.SurName);
                facade.SetConcept(RuleConceptGeneralBasicInfo.SecondSurname, person.SecondSurName);
                facade.SetConcept(RuleConceptGeneralBasicInfo.Names, person.FullName);

            }

            if (company?.IndividualType == Core.Services.UtilitiesServices.Enums.IndividualType.Company)
            {
                facade.SetConcept(RuleConceptGeneralBasicInfo.IndividualType, (int)Core.Services.UtilitiesServices.Enums.IndividualType.Company);
                facade.SetConcept(RuleConceptPolicies.UserId, company.UserId);
                facade.SetConcept(RuleConceptGeneralBasicInfo.IndividualId, company.IndividualId);
                facade.SetConcept(RuleConceptGeneralBasicInfo.BusinessName, company.FullName);
                facade.SetConcept(RuleConceptGeneralBasicInfo.DocumentNumber, company.IdentificationDocument?.Number);
                facade.SetConcept(RuleConceptGeneralBasicInfo.DocumentType, company.IdentificationDocument?.DocumentType?.Id);
            }
        }

        #endregion

        #region FinancialSarlaft
        /// <summary>
        /// Crear Sarlaft
        /// </summary>
        /// <param name="sarlaft">The sarlaft.</param>
        /// <returns></returns>
        public static UPET.FinancialSarlaft CreateSarlaft(Models.FinancialSarlaf sarlaft)
        {

            return new UPET.FinancialSarlaft(0)
            {
                SarlaftId = sarlaft.SarlaftId,
                IncomeAmount = sarlaft.IncomeAmount,
                ExpenseAmount = sarlaft.ExpenseAmount,
                ExtraIncomeAmount = sarlaft.ExtraIncomeAmount,
                AssetsAmount = sarlaft.AssetsAmount,
                LiabilitiesAmount = sarlaft.LiabilitiesAmount,
                Description = sarlaft.Description,
                IsForeingTransaction = sarlaft.IsForeignTransaction,
                ForeingTransactionAmount = sarlaft.ForeignTransactionAmount
            };
        }
        #endregion

        #region IndividualSarlaft
        /// <summary>
        /// Creates the individual sarlaft.
        /// </summary>
        /// <param name="individualSarlaft">The individual sarlaft.</param>
        /// <returns></returns>
        public static UPET.IndividualSarlaft CreateIndividualSarlaft(Models.IndividualSarlaft individualSarlaft)
        {
            return new UPET.IndividualSarlaft()
            {

                Year = individualSarlaft.Year,
                AuthorizedBy = individualSarlaft.AuthorizedBy,
                VerifyingEmployee = individualSarlaft.VerifyingEmployee,
                InterviewerName = individualSarlaft.InterviewerName,
                InternationalOperations = Convert.ToBoolean(individualSarlaft.InternationalOperations),
                InterviewPlace = individualSarlaft.InterviewerPlace,
                InterviewResultCode = individualSarlaft.InterviewResultCode,
                PendingEvent = individualSarlaft.PendingEvents,
                RegistrationDate = individualSarlaft.RegistrationDate,
                UserId = individualSarlaft.UserId
            };
        }
        #endregion

        #region IndividualSarlaftV2
        /// <summary>
        /// Creates the individual sarlaft.
        /// </summary>
        /// <param name="individualSarlaft">The individual sarlaft.</param>
        /// <returns></returns>
        public static UPETCORE.IndividualSarlaft CreateIndividualSarlaftV1(Models.IndividualSarlaft individualSarlaft)
        {
            return new UPETCORE.IndividualSarlaft()
            {

                Year = individualSarlaft.Year,
                AuthorizedBy = individualSarlaft.AuthorizedBy,
                VerifyingEmployee = individualSarlaft.VerifyingEmployee,
                InterviewerName = individualSarlaft.InterviewerName,
                InternationalOperations = Convert.ToBoolean(individualSarlaft.InternationalOperations),
                InterviewPlace = individualSarlaft.InterviewerPlace,
                InterviewResultCode = individualSarlaft.InterviewResultCode,
                PendingEvent = individualSarlaft.PendingEvents,
                RegistrationDate = individualSarlaft.RegistrationDate,
                UserId = individualSarlaft.UserId
            };
        }
        #endregion

        #region Sarlaft Exoneration

        /// <summary>
        /// Creates the sarlaft exoneration.
        /// </summary>
        /// <param name="sarlaftExoneration">The sarlaft exoneration.</param>
        /// <returns></returns>
        public static UPET.IndividualSarlaftExoneration CreateSarlaftExoneration(Models.CompanySarlaftExoneration sarlaftExoneration)
        {
            return new UPET.IndividualSarlaftExoneration(sarlaftExoneration.Id)
            {
                IsExonerated = sarlaftExoneration.IsExonerated,
                RegistrationDate = sarlaftExoneration.EnteredDate,
                RoleCode = sarlaftExoneration.RolId,
                UserId = sarlaftExoneration.UserId,
                ExonerationTypeCode = sarlaftExoneration.ExonerationType.Id
            };
        }

        #endregion

        #region CoConsortium
        /// <summary>
        /// Creates the consortium.
        /// </summary>
        /// <param name="coConsortium">The co consortium.</param>
        /// <returns></returns>
        public static UPETCORE.CoConsortium CreateConsortium(Models.CompanyConsortium consortium)
        {
            return new UPETCORE.CoConsortium(consortium.InsuredCode, consortium.IndividualId)
            {
                ConsortiumId = consortium.ConsortiumId,
                IsMain = consortium.IsMain,
                ParticipationRate = consortium.ParticipationRate,
                StartDate = consortium.StartDate,
                Enabled = consortium.Enabled
            };
        }
        #endregion

        #region CoInsurer
        /// <summary>
        /// Creates the coinsurer.
        /// </summary>
        /// <param name="coInsurance">The co coInsurer.</param>
        /// <returns></returns>
        public static void CreateCoInsurer(CoInsuranceCompany coInsurerEntity, Models.CoInsurerCompany coInsurer)
        {
            coInsurerEntity.Description = coInsurer.Description;
            coInsurerEntity.IndividualId = coInsurer.IndividualId;
            coInsurerEntity.EnteredDate = coInsurer.EnteredDate;
            coInsurerEntity.ModifyDate = coInsurer.ModifyDate;
            coInsurerEntity.DeclinedDate = coInsurer.DeclinedDate;
            coInsurerEntity.Annotations = coInsurer.Annotations;

            coInsurerEntity.AddressTypeCode = coInsurer.AddressTypeCode;
            coInsurerEntity.AreaCode = coInsurer.AreaCode ?? 0;
            coInsurerEntity.CityCode = coInsurer.CityCode;
            coInsurerEntity.ColonyCode = coInsurer.ColonyCode ?? 0;
            coInsurerEntity.ComDeclinedTypeCode = coInsurer.ComDeclinedTypeCode;
            coInsurerEntity.CountryCode = coInsurer.CountryCode;
            coInsurerEntity.IvaTypeCode = coInsurer.IvaTypeCode;
            coInsurerEntity.PhoneNumber = coInsurer.PhoneNumber;
            coInsurerEntity.PhoneTypeCode = coInsurer.PhoneTypeCode;
            coInsurerEntity.PilotingSpendAmount = coInsurer.PilotingSpendAmount;
            coInsurerEntity.PostalCode = coInsurer.PostalCode ?? 0;
            coInsurerEntity.SmallDescription = coInsurer.SmallDescription;
            coInsurerEntity.StateCode = coInsurer.StateCode;
            coInsurerEntity.Street = coInsurer.Street;
            coInsurerEntity.TributaryIdNo = coInsurer.TributaryIdNo;
            coInsurerEntity.YearMaxLongQuantity = coInsurer.YearMaxLongQuantity ?? 0;
            coInsurerEntity.YearMaxSignInQuantity = coInsurer.YearMaxSignInQuantity ?? 0;
            coInsurerEntity.YearMinSignInQuantity = coInsurer.YearMinSignInQuantity ?? 0;
        }
        #endregion

        #region SarlaftBranch
        /// <summary>
        /// Creates the sarlaft branch.
        /// </summary>
        /// <param name="sarlaftBranch">The sarlaft branch.</param>
        /// <returns></returns>
        public static UPET.SarlaftYear CreateSarlaftYear(int year)
        {
            return new UPET.SarlaftYear(year)
            {
                FormNum = 1
            };
        }
        #endregion

        #region Company.ScoreTypeDoc

        /// <summary>
        /// Convierte modelo a entidad del servicio
        /// </summary>
        /// <param name="scoreTypeDoc">Modelo del tipo de documento datacrédito</param>
        /// <returns>Entidad del tipo de documento datacrédito</returns>
        public static UPET.CompanyScoreTypeDoc CreateScoreTypeDoc(Models.ScoreTypeDoc scoreTypeDoc)
        {
            return new UPET.CompanyScoreTypeDoc(0)
            {
                IdCardTypeScore = scoreTypeDoc.IdCardTypeScore,
                Description = scoreTypeDoc.Description,
                SmallDescription = scoreTypeDoc.SmallDescription,
            };
        }

        /// <summary>
        /// Convierte modelo a entidad del servicio
        /// </summary>
        /// <param name="scoreTypeDoc">Modelo de la asociación entre tipo de documento datacrédito y tipo documento SISE</param>
        /// <returns>Entidad de la asociación entre tipo de documento datacrédito y tipo documento SISE</returns>
        public static UPET.CompanyScore3gTypeDoc CreateScore3gTypeDoc(Models.ScoreTypeDoc scoreTypeDoc)
        {
            return new UPET.CompanyScore3gTypeDoc(0)
            {
                IdCardTypeScore = scoreTypeDoc.IdCardTypeScore,
                IdScore3g = scoreTypeDoc.IdScore3g,
                IdCardTypeCode = scoreTypeDoc.IdCardTypeCode,
            };
        }
        #endregion

        #region PaymentMethod      

        #endregion

        #region Company.PhoneType
        /// <summary>
        /// Convierte modelo a entidad del servicio
        /// </summary>
        /// <param name="companyPhoneType">Modelo del tipo de teléfono</param>
        /// <returns>Entidad del tipo de teléfono</returns>
        public static entities.CompanyPhoneType CreateCompanyPhoneType(Models.CompanyPhoneType companyPhoneType)
        {
            return new entities.CompanyPhoneType(0)
            {
                PhoneTypeCode = companyPhoneType.PhoneTypeCode,
                Description = companyPhoneType.Description,
                SmallDescription = companyPhoneType.SmallDescription
            };
        }
        #endregion
        #region Company.AddressType
        /// <summary>
        /// Convierte modelo a entidad del servicio
        /// </summary>
        /// <param name="companyAddressType">Modelo del tipo de dirección</param>
        /// <returns>Entidad del tipo de dirección</returns>
        public static entities.CompanyAddressType CreateCompanyAddressType(Models.CompanyAddressType companyAddressType)
        {
            return new entities.CompanyAddressType(0)
            {
                AddressTypeCode = companyAddressType.AddressTypeCode,
                SmallDescription = companyAddressType.SmallDescription,
                TinyDescription = companyAddressType.TinyDescription,
                IsElectronicMail = companyAddressType.IsElectronicMail
            };
        }
        #endregion

        #region ThirdPerson
        public static entities.ThirdParty CreateThird(Models.CompanyThird third)
        {
            ThirdParty thirdEntity = new entities.ThirdParty()
            {

                EnteredDate = third.EnteredDate,
                DeclinedDate = third.DeclinedDate,
                IndividualId = third.IndividualId,
                ModificationDate = third.ModificationDate,
                Annotation = third.Annotation
            };

            if (third.DeclinedTypeId != null)
            {
                thirdEntity.DeclinedTypeCode = third.DeclinedTypeId;
            }

            return thirdEntity;
        }
        #endregion

        #region CiaDocumentsTypeRange
        /// <summary>
        /// CreateCiaDocumentTypeRange
        /// </summary>
        /// <param name="DocumentTypeRange"></param>
        /// <returns></returns>
        public static CiaDocumentsTypeRange CreateCiaDocumentTypeRange(Models.CiaDocumentTypeRange ciaDocumentTypeRange)
        {
            return new CiaDocumentsTypeRange(ciaDocumentTypeRange.DocumentTypeRange)
            {
                IndividualTypeCode = ciaDocumentTypeRange.IndividualTypeId,
                DocumentsTypeRangeCode = ciaDocumentTypeRange.DocumentTypeRange
            };
        }
        #endregion
        #region DocumentsTypeRange
        /// <summary>
        /// Convierte modelo a entidad del servicio
        /// </summary>
        /// <param name="DocumentsTypeRange"></param>
        /// <returns></returns>
        public static DocumentsTypeRange CreateDocumentTypeRange(Models.DocumentTypeRange documentTypeRange)
        {
            return new DocumentsTypeRange(documentTypeRange.Id)
            {
                DocumentsTypeRangeCode = documentTypeRange.Id,
                IdCardTypeCode = documentTypeRange.Id,
                Gender = documentTypeRange.Gender,
                IdCardNoFrom = documentTypeRange.CardNumberFrom,
                IdCardNoTo = documentTypeRange.CardNumberTo
            };
        }
        #endregion

        public static entities.Employee CreateEmployeeEntity(Models.CompanyEmployee employee)
        {
            return new Employee(employee.IndividualId)
            {
                IndividualId = employee.IndividualId,
                BranchCode = employee.BranchId,
                FileNumber = employee.FileNumber,
                EntryDate = employee.EntryDate,
                EgressDate = employee.EgressDate,
                Annotation = employee.Annotation,
                DeclinedTypeCode = employee.DeclinedTypeId,
                ModificationDate = employee.ModificationDate
            };
        }

        public static entities.IndividualRole CreateIndividualRole(Models.CompanyIndividualRole IndividualRole)
        {
            return new IndividualRole(IndividualRole.IndividualId, IndividualRole.RoleId)
            {
                IndividualId = IndividualRole.IndividualId,
                RoleCode = IndividualRole.RoleId
               
            };
        }

        public static INTEN.UpIndividualControl  CreateIndividualControl(Models.IndividualControl individualControl)
        {
            return new INTEN.UpIndividualControl(individualControl.Id)
            {                
                IndividualId = individualControl.IndividualId,
                Action = individualControl.Action
            };
        }

        public static INTEN.UpCoIndividualControl CreateCoIndividualControl(Models.CoIndividualControl CoindividualControl)
        {
            return new INTEN.UpCoIndividualControl(CoindividualControl.Id)
            {
                IndividualId = CoindividualControl.IndividualId,
                PerifericoId = CoindividualControl.PerifericoId
            };
        }

        public static INTEN.UpEmployeeControl CreateEmployeeControl(Models.EmployeeControl EmployeeControl)
        {
            return new INTEN.UpEmployeeControl(EmployeeControl.Id)
            {
                IndividualId = EmployeeControl.IndividualId,
                Action = EmployeeControl.Action
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

        public static INTEN.UpInsuranceCompanyControl CreateCoInsuranceCompanyControl(Models.InsuranceCompanyControl coInsuranceCompanyControl)
        {
            return new INTEN.UpInsuranceCompanyControl(coInsuranceCompanyControl.Id)
            {
                InsuranceCompanyId = coInsuranceCompanyControl.InsuranceCompanyId,                
                Action = coInsuranceCompanyControl.Action
            };
        }

        public static INTEN.UpAgentControl CreateAgentControl(Models.AgentControl agentControl)
        {
            return new INTEN.UpAgentControl()
            {
                IndividualId = agentControl.IndividualId,
                Action = agentControl.Action
            };
        }

        public static INTEN.UpReinsuranceCompanyControl CreateCompanyReinsuranceControl(Models.CompanyReinsuranceControl companyReinsuranceControl)
        {
            return new INTEN.UpReinsuranceCompanyControl()
            {
                IndividualId = companyReinsuranceControl.IndividualId,
                Action = companyReinsuranceControl.Action
            };
        }

        public static INTEN.UpSupplierControl CreateSupplierControl(Models.SupplierControl supplierControl)
        {
            return new INTEN.UpSupplierControl()
            {
                SupplierCode = supplierControl.SupplierCode,
                Action = supplierControl.Action
            };
        }
    }
}