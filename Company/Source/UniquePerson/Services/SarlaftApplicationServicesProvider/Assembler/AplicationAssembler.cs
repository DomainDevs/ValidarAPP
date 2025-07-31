using System.Collections.Generic;
using AutoMapper;
using System;
using Sistran.Company.Application.SarlaftApplicationServices;
using Sistran.Company.Application.SarlaftBusinessServices.Models;
using Sistran.Company.Application.SarlaftApplicationServices.DTO;
using Sistran.Company.Application.SarlaftBusinessServices.Enum;
using System.Linq;

namespace Sistran.Company.Application.SarlaftApplicationServicesProvider.Assemblers
{
    /// <summary>
    /// Convertir de DTOs a Modelos
    /// </summary>
    public class AplicationAssembler
    {

        #region Sarlaft

        public static UserDTO CreateUser(CompanyUser user)
        {
            return new UserDTO
            {
                UserId = user.UserId,
                Name = user.Name,
                BranchId = user.BranchId,
                FormNum = user.FormNum
            };

        }
        public static PersonDTO CreatePerson(CompanyPerson person)
        {
            return new PersonDTO
            {
                IndividualId = person.IndividualId,
                Name = person.Name,
                DocumentTypeId = person.DocumentType,
                DocumentNumber = person.DocumentNumber,
                EconomicActivityId = person.EconomicActivity.Id,
                EconomicActivityDesc = person.EconomicActivity.Description,
                PersonType = person.PersonType,
                AssociationType = person.AssociationType

            };

        }
        public static PersonDTO CreatePerson(CompanyCompany company)
        {
            return new PersonDTO
            {
                IndividualId = company.IndividualId,
                Name = company.FullName,
                DocumentTypeId = company.DocumentType,
                DocumentNumber = company.DocumentNumber,
                EconomicActivityId = company.EconomicActivity.Id,
                EconomicActivityDesc = company.EconomicActivity.Description,
                PersonType = company.PersonType,
                AssociationType = company.AssociationType

            };

        }
        public static SarlaftDTO CreateIndividualSarlaft(CompanyIndividualSarlaft sarlaft)
        {
            return new SarlaftDTO
            {
                Id = sarlaft.Id,
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
                BranchId = sarlaft.BranchId,
                EconomicActivityId = sarlaft.EconomicActivity.Id,
                EconomicActivityDesc = sarlaft.EconomicActivity.Description,
                SecondEconomicActivityId = sarlaft.SecondEconomicActivity.Id,
                SecondEconomicActivityDesc = sarlaft.SecondEconomicActivity.Description,
                InterviewResultId = sarlaft.InterviewResultId,
                PendingEvent = sarlaft.PendingEvent,
                UserId = sarlaft.UserId,
                UserName = sarlaft.UserName,
                BranchName = sarlaft.BranchName,
                YearParameter = sarlaft.YearParameter,
                DocumentNumber = sarlaft.DocumentNumber,
                TypeDocument = sarlaft.DocumentType,
                TypePerson = sarlaft.PersonType
            };

        }

        public static List<SarlaftDTO> CreateIndividualsSarlaft(List<CompanyIndividualSarlaft> Persons)
        {
            List<SarlaftDTO> sarlaftsDTO = new List<SarlaftDTO>();

            foreach (CompanyIndividualSarlaft companyPerson in Persons)
            {
                sarlaftsDTO.Add(CreateIndividualSarlaft(companyPerson));
            }

            return sarlaftsDTO;
        }

        public static FinancialSarlaftDTO CreatefinancialSarlaft(CompanyFinancialSarlaft financialSarlaft)
        {

            return new FinancialSarlaftDTO
            {
                SarlaftId = financialSarlaft.SarlaftId,
                IncomeAmount = financialSarlaft.IncomeAmount,
                ExpenseAmount = financialSarlaft.ExpenseAmount,
                ExtraIncomeAmount = financialSarlaft.ExtraIncomeAmount,
                AssetsAmount = financialSarlaft.AssetsAmount,
                LiabilitiesAmount = financialSarlaft.LiabilitiesAmount,
                Description = financialSarlaft.Description
            };
        }

        public static EconomicActivityDTO CreateEconomicActivity(CompanyEconomicActivity companyEconomicActivity)
        {
            return new EconomicActivityDTO
            {
                Id = companyEconomicActivity.Id,
                Description = companyEconomicActivity.Description,
                SmallDescription = companyEconomicActivity.SmallDescription
            };
        }

        public static List<EconomicActivityDTO> CreateEconomicActivities(List<CompanyEconomicActivity> companyEconomicActivities)
        {
            List<EconomicActivityDTO> economicActivityDTOs = new List<EconomicActivityDTO>();

            foreach (CompanyEconomicActivity companyEconomicActivity in companyEconomicActivities)
            {
                economicActivityDTOs.Add(CreateEconomicActivity(companyEconomicActivity));
            }

            return economicActivityDTOs;
        }

        public static TmpSarlaftOperationDTO CreateTmpSarlaftOperation(CompanyTmpSarlaftOperation companyTmpSarlaftOperation)
        {
            return new TmpSarlaftOperationDTO
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

        public static LinkDTO CreateLink(CompanyIndvidualLink companyIndividualLink)
        {
            return new LinkDTO
            {
                Id = companyIndividualLink.IndividualId,
                Description = companyIndividualLink.Description,
                LinkTypeCode = companyIndividualLink.LinkType.Id,
                RelationShipCode = companyIndividualLink.RelationshipSarlaft.Id,
                Status = (int)SarlaftStatus.Original

            };
        }

        public static List<LinkDTO> CreateLinks(List<CompanyIndvidualLink> companyIndividualLinks)
        {
            List<LinkDTO> linksDTOs = new List<LinkDTO>();
            foreach (CompanyIndvidualLink companyIndividualLink in companyIndividualLinks)
            {
                linksDTOs.Add(CreateLink(companyIndividualLink));
            }

            return linksDTOs;
        }

        public static RelationShipDTO CreateRelationShip(CompanyRelationShip companyRelationShip)
        {
            return new RelationShipDTO
            {
                Id = companyRelationShip.Id,
                Description = companyRelationShip.Description

            };
        }

        public static List<RelationShipDTO> CreateRelationShips(List<CompanyRelationShip> companyRelationShips)
        {
            List<RelationShipDTO> RelationShipDTOs = new List<RelationShipDTO>();
            foreach (CompanyRelationShip companyRelationShip in companyRelationShips)
            {
                RelationShipDTOs.Add(CreateRelationShip(companyRelationShip));
            }

            return RelationShipDTOs;
        }

        #endregion Links

        #region LegalRepresentative
        public static LegalRepresentativeDTO CreateLegalRepresentative(CompanyLegalRepresentative companyLegalRepresentative)
        {
            LegalRepresentativeDTO legalRepresentativeDTO = new LegalRepresentativeDTO();
            if (companyLegalRepresentative != null)
            {
                return new LegalRepresentativeDTO
                {
                    IndividualId = companyLegalRepresentative.IndividualId,
                    LegalRepresentativeName = companyLegalRepresentative.LegalRepresentativeName,
                    ExpeditionDate = companyLegalRepresentative.ExpeditionDate,
                    ExpeditionPlace = companyLegalRepresentative.ExpeditionPlace,
                    BirthDate = companyLegalRepresentative.BirthDate,
                    BirthPlace = companyLegalRepresentative.BirthPlace,
                    City = companyLegalRepresentative.City,
                    Phone = companyLegalRepresentative.Phone??0,
                    JobTitle = companyLegalRepresentative.JobTitle,
                    CellPhone = companyLegalRepresentative.CellPhone??0,
                    Email = companyLegalRepresentative.Email != null ? companyLegalRepresentative.Email : string.Empty,
                    Address = companyLegalRepresentative.Address,
                    CountryId = companyLegalRepresentative.CountryId,
                    StateId = companyLegalRepresentative.StateId,
                    CityId = companyLegalRepresentative.CityId,
                    Nationality = companyLegalRepresentative.Nationality,
                    IdCardNo = companyLegalRepresentative.IdCardNo,
                    AuthorizationAmount = companyLegalRepresentative.AuthorizationAmount,
                    Description = companyLegalRepresentative.Description,
                    CurrencyId = companyLegalRepresentative.CurrencyId,
                    IdCardTypeCode = companyLegalRepresentative.IdCardTypeCode,
                    Status = (int)SarlaftStatus.Original,
                    IsMain = companyLegalRepresentative.IsMain,
                    NationalityType = companyLegalRepresentative.NationalityType,
                    NationalityOtherType = companyLegalRepresentative.NationalityOtherType,
                    LegalRepresentType = companyLegalRepresentative.LegalRepresentType
                };
            }
            else
                return legalRepresentativeDTO;
        }

        public static List<LegalRepresentativeDTO> CreateLegalRepresentatives(List<CompanyLegalRepresentative> LegalRepresentatives)
        {
            List<LegalRepresentativeDTO> LegalRepresentativesDTO = new List<LegalRepresentativeDTO>();

            foreach (CompanyLegalRepresentative companyLegal in LegalRepresentatives)
            {
                LegalRepresentativesDTO.Add(CreateLegalRepresentative(companyLegal));
            }

            return LegalRepresentativesDTO;
        }

        public static SarlaftExonerationtDTO CreateSarlaftExonerationModel(CompanySarlaftExoneration sarlaft)
        {
            return new SarlaftExonerationtDTO
            {
                IndividualId = sarlaft.IndividualId,
                ExonerationType = (int)sarlaft.ExonerationType,
                IsExonerated = sarlaft.IsExonerated,
                RegistrationDate = (DateTime)sarlaft.RegistrationDate,
                RoleId = sarlaft.RoleId
            };
        }

        public static List<SarlaftExonerationtDTO> CreateSarlaftExoneration(List<CompanySarlaftExoneration> sarlaft)
        {
            var result = new List<SarlaftExonerationtDTO>();
            foreach (var item in sarlaft)
            {
                result.Add(CreateSarlaftExonerationModel(item));
            }
            return result;
        }

        #endregion

        #region Partners

        public static PartnersDTO CreatePartner(CompanyIndividualPartner companyPartner)
        {
            return new PartnersDTO
            {
                Id = companyPartner.Id,
                IdCardNumero = companyPartner.IdCardNumero,
                IndividualId = companyPartner.IndividualId,
                DocumentTypeId = companyPartner.DocumentTypeId,
                TradeName = companyPartner.TradeName,
                Active = companyPartner.Active,
                Status = (int)SarlaftStatus.Original,
                Participation = companyPartner.Participation,
                FinalBeneficiary = AplicationAssembler.CreateFinalBeneficiarys(companyPartner.FinalBeneficiary),
                CoPartners = companyPartner.CoIndividualPartner ==null ? new CoPartnersDTO() : new CoPartnersDTO {
                    Id =companyPartner.CoIndividualPartner.Id,
                    IndividualId = companyPartner.CoIndividualPartner.IndividualId,
                    IdCardNumero = companyPartner.CoIndividualPartner.IdCardNumero,
                    DocumentTypeId = companyPartner.CoIndividualPartner.DocumentTypeId
                    
                }
            };
        }

        public static List<PartnersDTO> CreatePartners(List<CompanyIndividualPartner> companyPartners)
        {
            List<PartnersDTO> partnersDTO = new List<PartnersDTO>();
            foreach (CompanyIndividualPartner companyPartner in companyPartners)
            {
                partnersDTO.Add(CreatePartner(companyPartner));
            }

            return partnersDTO;
        }

        public static FinalBeneficiaryDTO CreateFinalBeneficiary(CompanyFinalBeneficiary finalBeneficiary)
        {
            return new FinalBeneficiaryDTO
            {
                Id = finalBeneficiary.Id,
                IdCardNumero = finalBeneficiary.IdCardNumero,
                TradeName = finalBeneficiary.TradeName,
                DocumentTypeId = finalBeneficiary.DocumentTypeId,
                IndividualId = finalBeneficiary.IndividualId
            };

        }

        public static List<FinalBeneficiaryDTO> CreateFinalBeneficiarys(List<CompanyFinalBeneficiary> finalBeneficiarys)
        {
            List<FinalBeneficiaryDTO> companyfinalBeneficiarys = new List<FinalBeneficiaryDTO>();
            if (finalBeneficiarys != null)
                foreach (CompanyFinalBeneficiary finalBeneficiaryDTO in finalBeneficiarys)
                {
                    companyfinalBeneficiarys.Add(AplicationAssembler.CreateFinalBeneficiary(finalBeneficiaryDTO));
                }

            return companyfinalBeneficiarys;

        }

        #endregion

        #region Insured

        //public static InsuredDeclinedTypeDTO CreateInsuredDeclinedType(CompanyInsuredDeclinedType companyInsuredDeclinedType)
        //{
        //    var result = new InsuredDeclinedTypeDTO();
        //    result.Id = companyInsuredDeclinedType.Id;
        //    result.Description = companyInsuredDeclinedType.Description;
        //    result.SmallDescription = companyInsuredDeclinedType.SmallDescription;
        //    return result;
        //}

        //public static List<InsuredDeclinedTypeDTO> CreateInsuredDeclinedTypes(List<CompanyInsuredDeclinedType> companyInsuredDeclinedType)
        //{
        //    var insuredDeclinedTypeDTO = new List<InsuredDeclinedTypeDTO>();
        //    foreach (CompanyInsuredDeclinedType insuredDeclinedType in companyInsuredDeclinedType)
        //    {
        //        insuredDeclinedTypeDTO.Add(CreateInsuredDeclinedType(insuredDeclinedType));
        //    }
        //    return insuredDeclinedTypeDTO;
        //}

        //public static InsuredSegmentDTO CreateInsuredSegment(CompanyInsuredSegment companyInsuredSegment)
        //{
        //    var result = new InsuredSegmentDTO();
        //    result.IndividualId = companyInsuredSegment.Id;
        //    result.Description = companyInsuredSegment.Description;
        //    return result;
        //}

        //public static List<InsuredSegmentDTO> CreateInsuredSegments(List<CompanyInsuredSegment> companyInsuredSegment)
        //{
        //    var insuredSegmentDTO = new List<InsuredSegmentDTO>();
        //    foreach (CompanyInsuredSegment insuredSegment in companyInsuredSegment)
        //    {
        //        insuredSegmentDTO.Add(CreateInsuredSegment(insuredSegment));
        //    }
        //    return insuredSegmentDTO;
        //}

        //public static InsuredProfileDTO CreateInsuredProfile(CompanyInsuredProfile companyInsuredProfile)
        //{
        //    var result = new InsuredProfileDTO();
        //    result.IndividualId = companyInsuredProfile.Id;
        //    result.Description = companyInsuredProfile.Description;
        //    return result;
        //}

        //public static List<InsuredProfileDTO> CreateInsuredProfiles(List<CompanyInsuredProfile> companyInsuredProfile)
        //{
        //    var insuredProfileDTO = new List<InsuredProfileDTO>();
        //    foreach (CompanyInsuredProfile insuredProfile in companyInsuredProfile)
        //    {
        //        insuredProfileDTO.Add(CreateInsuredProfile(insuredProfile));
        //    }
        //    return insuredProfileDTO;
        //}

        #endregion

        #region International Operations 

        public static InternationalOperationDTO CreateInternationalOperation(CompanySarlaftOperation companySarlaftOperation)
        {
            return new InternationalOperationDTO
            {
                Id = companySarlaftOperation.Id,
                SarlaftId = companySarlaftOperation.SarlaftId,
                ProductNum = companySarlaftOperation.ProductNum,
                ProductAmt = companySarlaftOperation.ProductAmt,
                Entity = companySarlaftOperation.Entity,
                OperationTypeId = companySarlaftOperation.CompanyOperationType.Id,
                OperationDescription = companySarlaftOperation.CompanyOperationType.Description,
                ProductTypeId = companySarlaftOperation.CompanyProductType.Id,
                ProductDescription = companySarlaftOperation.CompanyProductType.Description,
                CurrencyId = companySarlaftOperation.CurrencyId,
                CountryId = companySarlaftOperation.CountryId,
                CityId = companySarlaftOperation.CityId,
                StateId = companySarlaftOperation.StateId,
                Status = Convert.ToInt32(SarlaftStatus.Original)
            };
        }

        public static List<InternationalOperationDTO> CreateInternationalOperations(List<CompanySarlaftOperation> companySarlaftOperations)
        {
            List<InternationalOperationDTO> internationalOperationDTOs = new List<InternationalOperationDTO>();
            foreach (CompanySarlaftOperation companySarlaftOperation in companySarlaftOperations)
            {
                internationalOperationDTOs.Add(CreateInternationalOperation(companySarlaftOperation));
            }

            return internationalOperationDTOs;
        }
        #endregion

        #region peps
        public static PepsDTO CreatePeps(CompanyIndvidualPeps companyIndividualPeps)
        {
            return new PepsDTO
            {
                Individual_Id = companyIndividualPeps.IndividualId,
                Exposed = companyIndividualPeps.Exposed,
                Trade_Name = companyIndividualPeps.TradeName,
                Unlinked_DATE = companyIndividualPeps.UnlinkedDATE,

                Category = companyIndividualPeps.Category,
                Link = companyIndividualPeps.Link,
                Affinity = companyIndividualPeps.Affinity,
                Unlinked = companyIndividualPeps.Unlinked,

                Entity = companyIndividualPeps.Entity,
                Observations = companyIndividualPeps.Observations,
                JobOffice = companyIndividualPeps.JobOffice
            };
        }
        #endregion


        #region CoSarlaft
        public static CoSarlaftDTO CreateCoSarlaft(CompanyCoSarlaft companyCoSarlaft)
        {
            return new CoSarlaftDTO
            {
                sarlaftid = companyCoSarlaft.SarlaftId,
                individualid = companyCoSarlaft.IndividualId,
                email = companyCoSarlaft.Email,
                heritage = companyCoSarlaft.Heritage,
                idCompanyTypeCode = companyCoSarlaft.IdCompanyTypeCode,
                cityCode = companyCoSarlaft.CityCode,
                stateCode = companyCoSarlaft.StateCode,
                countryCode = companyCoSarlaft.CountryCode,
                OppositorTypeCode = companyCoSarlaft.OppositorTypeCode,
                PersonTypeCode = companyCoSarlaft.PersonTypeCode,
                SocietyTypeCode = companyCoSarlaft.SocietyTypeCode,
                NationalityCode = companyCoSarlaft.NationalityCode,
                NationalityOtherCode = companyCoSarlaft.NationalityOtherCode,
                Phone = companyCoSarlaft.Phone,
                ExonerationTypeCode = companyCoSarlaft.ExonerationTypeCode,
                MainAddressNatural = companyCoSarlaft.MainAddressNatural

            };
        }
        #endregion

        #region Combos
        public static List<RolDTO> CreateeRoles(List<CompanyRole> Roles)
        {

            List<RolDTO> rolesDTO = new List<RolDTO>();
            RolDTO NodoDto;
            foreach (CompanyRole nodo in Roles)
            {
                NodoDto = new RolDTO
                {
                    RoleCd = nodo.RoleCd,
                    Description= nodo.Description
                    

                };
                rolesDTO.Add(NodoDto);
            }
            return rolesDTO;
        }

        public static List<EntityDTO> CreateEntity(List<CompanyEntity> Entity)
        {

            List<EntityDTO> EntityDTO = new List<EntityDTO>();
            EntityDTO NodoDto;
            foreach (CompanyEntity nodo in Entity)
            {
                NodoDto = new EntityDTO
                {
                    Id = nodo.Id,
                    Description = nodo.Description
                    


                };
                EntityDTO.Add(NodoDto);
            }
            return EntityDTO;
        }

       
        #endregion 

    }
}