using Sistran.Company.Application.SarlaftBusinessServices.Models;
using Sistran.Company.Application.UniquePerson.Entities;
using System;
using Sistran.Core.Application.Utilities.RulesEngine;
using Sistran.Core.Framework.Rules;
using UPENV1 = Sistran.Core.Application.UniquePersonV1.Entities;
using UUSM = Sistran.Core.Application.UniqueUserServices.Models;

namespace Sistran.Company.Application.SarlaftBusinessServicesProvider.Assemblers
{
    /// <summary>
    /// Convercion Emsamblados
    /// </summary>
    public static class EntityAssembler
    {


        #region Facades
        public static void CreateFacadeGeneralSarlaft(Facade facade, CompanyFinancialSarlaft companyFinancialSarlaft, CompanyCoSarlaft coSarlaft, CompanyIndividualSarlaft companyIndividualSarlaft)
        {
            facade.SetConcept(RuleConceptPolicies.UserId, companyIndividualSarlaft.UserId);

            facade.SetConcept(RuleConceptGeneralSarlaft.IndividualIdSarlaft, companyIndividualSarlaft.IndividualId);
            facade.SetConcept(RuleConceptGeneralSarlaft.DocumentNumberSarlaft, companyIndividualSarlaft.DocumentNumber);
            facade.SetConcept(RuleConceptGeneralSarlaft.DocumentTypeSarlaft, companyIndividualSarlaft.DocumentType);
            facade.SetConcept(RuleConceptGeneralSarlaft.NamesBusinessNameSarlaft, companyIndividualSarlaft.FullName);
            facade.SetConcept(RuleConceptGeneralSarlaft.CountrySarlaft, coSarlaft.CountryCode);
            facade.SetConcept(RuleConceptGeneralSarlaft.StateSarlaft, coSarlaft.StateCode);
            facade.SetConcept(RuleConceptGeneralSarlaft.CitySarlaft, coSarlaft.CityCode);
        }

        public static void CreateFacadeLinks(Facade facade, CompanyIndvidualLink companyIndvidualLink)
        {

        }

        public static void CreateFacadeInternationalOperations(Facade facade, CompanySarlaftOperation operation)
        {
            facade.SetConcept(RuleConceptInternationalOperations.CityInternationalOperations, operation.CityId);
            facade.SetConcept(RuleConceptInternationalOperations.CountryInternationalOperations, operation.CountryId);
            facade.SetConcept(RuleConceptInternationalOperations.StateInternationalOperations, operation.StateId);
        }

        public static void CreateFacadeLegalRepresentative(Facade facade, CompanyLegalRepresentative legalRepresentative)
        {
            facade.SetConcept(RuleConceptLegalRepresentative.CityLegalRepresentative, legalRepresentative.CityId);
            facade.SetConcept(RuleConceptLegalRepresentative.CountryLegalRepresentative, legalRepresentative.CountryId);
            facade.SetConcept(RuleConceptLegalRepresentative.StateLegalRepresentative, legalRepresentative.StateId);
            facade.SetConcept(RuleConceptLegalRepresentative.NameLegal, legalRepresentative.LegalRepresentativeName);
            facade.SetConcept(RuleConceptLegalRepresentative.DocumentNumberLegal, legalRepresentative.IdCardNo);
            facade.SetConcept(RuleConceptLegalRepresentative.DocumentTypeLegal, legalRepresentative.IdCardTypeCode);
        }

        public static void CreateFacadePartners(Facade facade, CompanyIndividualPartner partner)
        {
            facade.SetConcept(RuleConceptPartners.IndividualIdShareholders, partner.IndividualId);
            facade.SetConcept(RuleConceptPartners.DocumentNumberShareholders, partner.IdCardNumero);
            facade.SetConcept(RuleConceptPartners.NameShareholders, partner.TradeName);
            facade.SetConcept(RuleConceptPartners.DocumentTypePartner, partner.DocumentTypeId);
        }

        public static void CreateFacadeBeneficiaries(Facade facade, CompanyFinalBeneficiary beneficiary)
        {
            facade.SetConcept(RuleConceptBeneficiaries.DocumentNumberBeneficiary, beneficiary?.IdCardNumero);
            facade.SetConcept(RuleConceptBeneficiaries.NameBeneficiary, beneficiary?.TradeName);
            facade.SetConcept(RuleConceptBeneficiaries.DocumentTypeBeneficiary, beneficiary?.DocumentTypeId);
        }
        #endregion Facades


        #region Sarlaft
        public static IndividualSarlaft CreateIndividualSarlaft(CompanyIndividualSarlaft sarlaft)
        {
            return new IndividualSarlaft(sarlaft.Id)
            {
                SarlaftId = sarlaft.Id,
                IndividualId = sarlaft.IndividualId,
                FormNum = sarlaft.FormNum,
                Year = sarlaft.Year,
                RegistrationDate = sarlaft.RegistrationDate,
                AuthorizedBy = sarlaft.AuthorizedBy,
                FillingDate = sarlaft.FillingDate,
                CheckDate = sarlaft.CheckDate,
                VerifyingEmployee = sarlaft.VerifyingEmployee,
                InterviewDate = sarlaft.InterviewDate,
                InterviewerName = sarlaft.InterviewerName,
                InternationalOperations = sarlaft.InternationalOperations,
                InterviewPlace = sarlaft.InterviewPlace,
                BranchCode = sarlaft.BranchId,
                EconomicActivityCode = Convert.ToInt32(sarlaft.EconomicActivity.Id),
                SecondEconomicActivityCode = Convert.ToInt32(sarlaft.SecondEconomicActivity.Id),
                InterviewResultCode = sarlaft.InterviewResultId,
                PendingEvent = sarlaft.PendingEvent,
                UserId = sarlaft.UserId
            };
        }

        public static FinancialSarlaft CreateFinancialSarlaft(CompanyFinancialSarlaft financialSarlaft)
        {
            return new FinancialSarlaft(financialSarlaft.SarlaftId)
            {
                IncomeAmount = financialSarlaft.IncomeAmount,
                ExpenseAmount = financialSarlaft.ExpenseAmount,
                ExtraIncomeAmount = financialSarlaft.ExtraIncomeAmount,
                AssetsAmount = financialSarlaft.AssetsAmount,
                LiabilitiesAmount = financialSarlaft.LiabilitiesAmount,
                Description = financialSarlaft.Description
            };
        }

        public static CoPrvIndividual CreateCoPrvIndividual(int individualId, int? economicActivityId, int? secondEconomicActivityId)
        {
            return new CoPrvIndividual(individualId)
            {
                IndividualId = individualId,
                EconomicActivityCdNew = economicActivityId,
                SecondEconomicActivityCdNew = secondEconomicActivityId
            };
        }

        public static UPENV1.TmpSarlaftOperation CreateTmpSarlaftOperation(CompanyTmpSarlaftOperation companyTmpSarlaftOperation)
        {
            return new UPENV1.TmpSarlaftOperation
            {
                OperationId = companyTmpSarlaftOperation.OperationId,
                IndividualId = companyTmpSarlaftOperation.IndividualId,
                SarlaftId = companyTmpSarlaftOperation.SarlaftId,
                Operation = companyTmpSarlaftOperation.Operation,
                Proccess = companyTmpSarlaftOperation.Proccess,
                TypeProccess = companyTmpSarlaftOperation.TypeProccess,
                FunctionId = companyTmpSarlaftOperation.FunctionId
            };
        }

        #endregion

        #region Links
        public static IndividualLink CreateIndvidualLink(CompanyIndvidualLink indvidualLink)
        {
            return new IndividualLink(indvidualLink.IndividualId, indvidualLink.LinkType.Id, indvidualLink.RelationshipSarlaft.Id, indvidualLink.SarlaftId)
            {
                IndividualId = indvidualLink.IndividualId,
                LinkTypeCode = indvidualLink.LinkType.Id,
                RelationshipSarlaftCode = indvidualLink.RelationshipSarlaft.Id,
                Description = indvidualLink.Description,
                SarlaftId = indvidualLink.SarlaftId
            };
        }

        #endregion

        #region LegalRepresentative
        public static IndividualLegalRepresent CreateLegalRepresentative(CompanyLegalRepresentative companyLegal)
        {
            return new IndividualLegalRepresent(companyLegal.IndividualId, companyLegal.SarlaftId)
            {
                IndividualId = companyLegal.IndividualId,
                Phone = companyLegal.Phone,
                IdCardTypeCode = companyLegal?.IdCardTypeCode ?? 0,
                CurrencyCode = companyLegal.CurrencyId,
                Description = companyLegal.Description,
                AuthorizationAmount = companyLegal.AuthorizationAmount,
                IdCardNo = companyLegal?.IdCardNo ?? "",
                Address = companyLegal?.Address ?? "",
                Email = companyLegal?.Email ?? "",
                CellPhone = companyLegal.CellPhone,
                JobTitle = companyLegal.JobTitle,
                City = companyLegal?.City ?? "",
                CountryCode = companyLegal.CountryId == 0 ? (int?)null : companyLegal.CountryId,
                Nationality = companyLegal?.Nationality ?? "",
                BirthPlace = companyLegal?.BirthPlace ?? "",
                BirthDate = companyLegal.BirthDate <= DateTime.MinValue ? (DateTime?)null : companyLegal.BirthDate,
                ExpeditionPlace = companyLegal?.ExpeditionPlace ?? "",
                ExpeditionDate = companyLegal.ExpeditionDate <= DateTime.MinValue ? (DateTime?)null : companyLegal.ExpeditionDate,
                LegalRepresentativeName = companyLegal.LegalRepresentativeName,
                CityCode = companyLegal.CityId == 0 ? (int?)null : companyLegal.CityId,
                StateCode = companyLegal.StateId == 0 ? (int?)null : companyLegal.StateId,
                NationalityTypeCode = companyLegal.NationalityType,
                NationalityOtherTypeCode = companyLegal.NationalityOtherType,
                LegalRepresentTypeCode = companyLegal.LegalRepresentType,
                IsMain = companyLegal?.IsMain ?? true,
                SarlaftId = companyLegal.SarlaftId
            };
        }

        public static IndividualSubstituteLegalRepresent CreateSubstituteLegalRepresentative(CompanyLegalRepresentative companyLegal)
        {
            return new IndividualSubstituteLegalRepresent(companyLegal.IndividualId)
            {
                IndividualId = companyLegal.IndividualId,
                Phone = companyLegal.Phone,
                IdCardTypeCode = companyLegal.IdCardTypeCode,
                CurrencyCode = companyLegal.CurrencyId,
                Description = companyLegal.Description,
                AuthorizationAmount = companyLegal.AuthorizationAmount,
                IdCardNo = companyLegal.IdCardNo,
                Address = companyLegal.Address,
                Email = companyLegal.Email,
                CellPhone = companyLegal.CellPhone,
                JobTitle = companyLegal.JobTitle,
                City = companyLegal.City,
                CountryCode = companyLegal.CountryId,
                Nationality = companyLegal.Nationality,
                BirthPlace = companyLegal.BirthPlace,
                BirthDate = companyLegal.BirthDate,
                ExpeditionPlace = companyLegal.ExpeditionPlace,
                ExpeditionDate = companyLegal.ExpeditionDate,
                LegalRepresentativeName = companyLegal.LegalRepresentativeName,
                CityCode = companyLegal.CityId,
                StateCode = companyLegal.StateId,
                LegalRepresentCode = companyLegal.IndividualId
            };
        }
        #endregion

        #region Partners
        public static IndividualPartner CreatePartner(CompanyIndividualPartner partner)
        {
            return new IndividualPartner(partner.Id, partner.IdCardNumero, partner.IndividualId, partner.DocumentTypeId, partner.SarlaftId)
            {
                PartnerId = partner.Id,
                IdCardTypeCode = partner.DocumentTypeId,
                IdCardNo = partner.IdCardNumero,
                IndividualId = partner.IndividualId,
                TradeName = partner.TradeName,
                Active = partner.Active,
                Participation = partner.Participation,
                SarlaftId = partner.SarlaftId

            };
        }

        public static CoIndividualPartner CreateCoPartner(CompanyCoIndividualPartner partner)
        {
            return new CoIndividualPartner()
            {  
                PartnerId = partner.Id,
                IdCardTypeCode = partner.DocumentTypeId,
                IdCardNo = partner.IdCardNumero,
                IndividualId = partner.IndividualId
                
            };
        }

        public static CoBeneficiaryPartner CreateBeneficiaryPartner(CompanyFinalBeneficiary beneficiarypartner)
        {
            return new CoBeneficiaryPartner(beneficiarypartner.Id, beneficiarypartner.IdCardNumero, beneficiarypartner.IndividualId, beneficiarypartner.DocumentTypeId, beneficiarypartner.SarlaftId)
            {
                TradeName = beneficiarypartner.TradeName

            };
        }
        #endregion

        #region International Operations

        public static SarlaftOperation CreateInternationalOperation(CompanySarlaftOperation sarlaftOperation)
        {
            return new SarlaftOperation(sarlaftOperation.Id, sarlaftOperation.SarlaftId)
            {
                SarlaftId = sarlaftOperation.SarlaftId,
                ProductNum = sarlaftOperation.ProductNum,
                ProductAmount = sarlaftOperation.ProductAmt,
                Entity = sarlaftOperation.Entity,
                OperationTypeCode = sarlaftOperation.CompanyOperationType.Id,
                ProductTypeCode = sarlaftOperation.CompanyProductType.Id,
                CurrencyCode = sarlaftOperation.CurrencyId,
                CountryCode = sarlaftOperation.CountryId,
                CityCode = sarlaftOperation.CityId,
                StateCode = sarlaftOperation.StateId

            };
        }

        #endregion

        #region Peps
        public static IndividualPeps CreatePeps(CompanyIndvidualPeps peps)
        {
            return new IndividualPeps(peps.IndividualId, peps.SarlaftId)
            {
                 
                 
                Entity = peps.Entity,
                IdUnlinkedTypeCode = peps.Unlinked,
                IdAffinityTypeCode = peps.Affinity,
                IdLinkTypeCode= peps.Link,
                IdCategoryTypeCode = peps.Category,
                UnlinkedDate = peps.UnlinkedDATE,
                Exposed = peps.Exposed,
                Observations = peps.Observations,
                TradeName = peps.TradeName,
                JobOffice = peps.JobOffice,
                SarlaftId = peps.SarlaftId


            };
        }
        #endregion

        #region CoSarlaft
        public static CoIndividualSarlaft CreateCoSarlaft(CompanyCoSarlaft coSarlaft)
        {
            return new CoIndividualSarlaft(coSarlaft.SarlaftId, coSarlaft.IndividualId)
            {

                SarlaftId = coSarlaft.SarlaftId,
                IndividualId = coSarlaft.IndividualId,
                Email = coSarlaft.Email,
                Heritage = coSarlaft.Heritage,
                IdCompanyTypeCode = coSarlaft.IdCompanyTypeCode,
                CityCode = coSarlaft.CityCode,
                StateCode = coSarlaft.StateCode,
                CountryCode = coSarlaft.CountryCode,
                
                OppositorCode = coSarlaft.OppositorTypeCode,
                PersonCode = coSarlaft.PersonTypeCode,
                SocietyCode = coSarlaft.SocietyTypeCode,
                NationalityCode = coSarlaft.NationalityCode,
                NationalityOtherCode = coSarlaft.NationalityOtherCode,
                Phone = coSarlaft.Phone,
                ExonerationCode = coSarlaft.ExonerationTypeCode,
                MainAddress  = coSarlaft.MainAddressNatural



            };
        }
        #endregion

        public static System.Collections.Generic.List<string> CreateInterviewManager(System.Collections.Generic.List<UUSM.User> InterviewManager)
        {
            System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
            foreach (UUSM.User item in InterviewManager)
            {
                list.Add(item.AccountName);
            }
            return list; 
        }

        public static System.Collections.Generic.List<string> CreateInterviewManagerSarlaft(System.Collections.Generic.List<UUSM.User> InterviewManager)
        {
            System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
            foreach (UUSM.User item in InterviewManager)
            {
                if(item.Name != null && item.Name.Length >0 )
                list.Add(item.AccountName +" - " + item.Name);
                else
                list.Add(item.AccountName );
            }
            return list;
        }
    }
}