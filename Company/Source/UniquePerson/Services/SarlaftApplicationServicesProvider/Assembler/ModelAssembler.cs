using AutoMapper;
//using Sistran.Core.Application.CommonService.Models.Base;
//using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using Sistran.Company.Application.SarlaftApplicationServices;
using Sistran.Company.Application.SarlaftApplicationServices.DTO;
using Sistran.Company.Application.SarlaftBusinessServices.Models;



namespace Sistran.Company.Application.SarlaftApplicationServicesProvider.Assemblers
{
    /// <summary>
    /// Convertir de Entities a Modelos
    /// </summary>
    public class ModelAssembler
    {
        public static CompanyUser CreateUser(UserDTO userDTO)
        {
            return new CompanyUser
            {
                UserId = userDTO.UserId,
                Name = userDTO.Name,
                BranchId = userDTO.BranchId,
                FormNum = userDTO.FormNum,
                
            };

        }

        #region Sarlaft
        /// <summary>
        /// Se encarga de realizar el mapeo entro los DTOS a las modelos de Negocio.
        /// </summary>
        /// <param name="person"></param>
        /// <returns>Retorna la informacion para visualizarla.</returns>
        /// 
        public static CompanyPerson CreatePerson(PersonDTO personDTO)
        {

            return new CompanyPerson
            {
                IndividualId = personDTO.IndividualId,
                Name = personDTO.Name,
                DocumentType = personDTO.DocumentTypeId,
                DocumentNumber = personDTO.DocumentNumber,
                EconomicActivity = new CompanyEconomicActivity
                {
                    Id = personDTO.EconomicActivityId
                }
            };
        }
        public static CompanyCompany CreateCompany(PersonDTO personDTO)
        {

            return new CompanyCompany
            {
                IndividualId = personDTO.IndividualId,
                FullName = personDTO.Name,
                DocumentType = personDTO.DocumentTypeId,
                DocumentNumber = personDTO.DocumentNumber,
                EconomicActivity = new CompanyEconomicActivity
                {
                    Id = personDTO.EconomicActivityId
                }
            };
        }
        public static CompanyIndividualSarlaft CreateSarlaft(SarlaftDTO sarlaftDTO)
        {

            return new CompanyIndividualSarlaft
            {
                Id = sarlaftDTO.Id,
                IndividualId = sarlaftDTO.IndividualId,
                FormNum = sarlaftDTO.FormNum,
                Year = sarlaftDTO.Year,
                RegistrationDate = sarlaftDTO.RegistrationDate,
                AuthorizedBy = sarlaftDTO.AuthorizedBy,
                FillingDate = sarlaftDTO.FillingDate,
                CheckDate = sarlaftDTO.CheckDate,
                VerifyingEmployee = sarlaftDTO.VerifyingEmployee,
                InterviewDate = sarlaftDTO.InterviewDate,
                InterviewerName = sarlaftDTO.InterviewerName,
                InternationalOperations = sarlaftDTO.InternationalOperations,
                InterviewPlace = sarlaftDTO.InterviewPlace,
                BranchId = sarlaftDTO.BranchId,
                EconomicActivity = new CompanyEconomicActivity {
                    Id = sarlaftDTO.EconomicActivityId
                },
                SecondEconomicActivity = new CompanyEconomicActivity {
                    Id = sarlaftDTO.SecondEconomicActivityId
                },
                InterviewResultId = sarlaftDTO.InterviewResultId,
                PendingEvent = sarlaftDTO.PendingEvent,
                UserId = sarlaftDTO.UserId,
                UserName = sarlaftDTO.UserName,
                BranchName = sarlaftDTO.BranchName,
                DocumentNumber = sarlaftDTO.DocumentNumber,
                DocumentType = sarlaftDTO.TypeDocument,
                PersonType = sarlaftDTO.TypePerson,
                FullName = sarlaftDTO.Name
            };
        }

        public static CompanyFinancialSarlaft CreateFinancialSarlaft(FinancialSarlaftDTO financialSarlaftDTO)
        {

            return new CompanyFinancialSarlaft
            {
                SarlaftId = financialSarlaftDTO.SarlaftId,
                IncomeAmount = financialSarlaftDTO.IncomeAmount,
                ExpenseAmount = financialSarlaftDTO.ExpenseAmount,
                ExtraIncomeAmount = financialSarlaftDTO.ExtraIncomeAmount,
                AssetsAmount = financialSarlaftDTO.AssetsAmount,
                LiabilitiesAmount = financialSarlaftDTO.LiabilitiesAmount,
                Description = financialSarlaftDTO.Description
            };
        }

        public static CompanyTmpSarlaftOperation CreateTmpSarlaftOperation(TmpSarlaftOperationDTO tmpSarlaftOperationDTO)
        {
            return new CompanyTmpSarlaftOperation
            {
                OperationId = tmpSarlaftOperationDTO.OperationId,
                IndividualId = tmpSarlaftOperationDTO.IndividualId,
                SarlaftId = tmpSarlaftOperationDTO.SarlaftId,
                Operation = tmpSarlaftOperationDTO.Operation,
                Proccess = tmpSarlaftOperationDTO.Proccess,
                TypeProccess = tmpSarlaftOperationDTO.TypeProccess,
                FunctionId = tmpSarlaftOperationDTO.FunctionId
            };
        }
        #endregion

        #region Link
        public static CompanyRelationShip CreateRelationShip(RelationShipDTO relationShipDTO)
        {
            return new CompanyRelationShip
            {
                Id = relationShipDTO.Id,
                Description = relationShipDTO.Description
            };

        }

        public static CompanyIndvidualLink CreateIndividualLink(LinkDTO linkDTO)
        {
            return new CompanyIndvidualLink
            {
                IndividualId = linkDTO.Id,
                LinkType = new CompanyLinkType
                {
                    Id = linkDTO.LinkTypeCode
                },
                RelationshipSarlaft = new CompanyRelationShip
                {
                    Id = linkDTO.RelationShipCode
                },
                Description = linkDTO.Description,
                SarlaftId = linkDTO.SarlaftId
            };

        }

        public static List<CompanyIndvidualLink> CreateIndividualLinks(List<LinkDTO> linkDTOs)
        {
            List<CompanyIndvidualLink> companyIndvidualLinks = new List<CompanyIndvidualLink>();
            foreach (LinkDTO linkDTO in linkDTOs)
            {
                companyIndvidualLinks.Add(CreateIndividualLink(linkDTO));
            }

            return companyIndvidualLinks;
        }
        #endregion

        #region LegalRepresentative
        public static CompanyLegalRepresentative CreateLegalRepresentative(LegalRepresentativeDTO legalRepresentative)
        {
            return new CompanyLegalRepresentative
            {
                IndividualId = legalRepresentative.IndividualId,
                LegalRepresentativeName = legalRepresentative.LegalRepresentativeName,
                ExpeditionDate = legalRepresentative.ExpeditionDate,
                ExpeditionPlace = legalRepresentative.ExpeditionPlace,
                BirthDate = legalRepresentative.BirthDate,
                BirthPlace = legalRepresentative.BirthPlace,
                City = legalRepresentative.City,
                Phone = legalRepresentative.Phone,
                JobTitle = legalRepresentative.JobTitle,
                CellPhone = legalRepresentative.CellPhone,
                Email = legalRepresentative.Email != null ? legalRepresentative.Email : string.Empty,
                Address = legalRepresentative.Address,
                CountryId = legalRepresentative.CountryId,
                StateId = legalRepresentative.StateId,
                CityId = legalRepresentative.CityId,
                Nationality = legalRepresentative.Nationality,
                IdCardNo = legalRepresentative.IdCardNo,
                AuthorizationAmount = legalRepresentative.AuthorizationAmount,
                Description = legalRepresentative.Description,
                CurrencyId = legalRepresentative.CurrencyId,
                IdCardTypeCode = legalRepresentative.IdCardTypeCode,
                IsMain = legalRepresentative.IsMain,
                NationalityType = legalRepresentative.NationalityType,
                NationalityOtherType= legalRepresentative.NationalityOtherType,
                LegalRepresentType = legalRepresentative.LegalRepresentType,
                SarlaftId = legalRepresentative.SarlaftId
            };
        }

        public static List<CompanyLegalRepresentative> CreateLegalRepresentatives(List<LegalRepresentativeDTO> legalRepresentative)
        {
            List<CompanyLegalRepresentative> CompanyLegalRepresentative = new List<CompanyLegalRepresentative>();

            foreach (var legalRepresentatives in legalRepresentative)
            {
                CompanyLegalRepresentative.Add(CreateLegalRepresentative(legalRepresentatives));
            }

            return CompanyLegalRepresentative;
        }
        #endregion

        #region Partners

        public static CompanyIndividualPartner CreatePartner(PartnersDTO partnerDTO)
        {
            return new CompanyIndividualPartner
            {
                Id = partnerDTO.Id,
                DocumentTypeId = partnerDTO.DocumentTypeId,
                IdCardNumero = partnerDTO.IdCardNumero,
                IndividualId = partnerDTO.IndividualId,
                Active = partnerDTO.Active,
                TradeName = partnerDTO.TradeName,
                Participation = partnerDTO.Participation,
                FinalBeneficiary = ModelAssembler.CreateFinalBeneficiarys(partnerDTO?.FinalBeneficiary),
                CoIndividualPartner = partnerDTO.CoPartners ==null ?  new CompanyCoIndividualPartner(): new CompanyCoIndividualPartner {
                    IdCardNumero = partnerDTO.CoPartners.IdCardNumero,
                    DocumentTypeId = partnerDTO.CoPartners.DocumentTypeId,
                    Participation = partnerDTO.CoPartners.Participation,
                    Occupation= partnerDTO.CoPartners.Occupation,
                    IdProfileCd= partnerDTO.CoPartners.IdProfileCd,
                    Nationality = partnerDTO.CoPartners.Nationality,
                    SocietyHolder= partnerDTO.CoPartners.SocietyHolder,
                    SocietyName= partnerDTO.CoPartners.SocietyName,
                    Constitutionyear= partnerDTO.CoPartners.Constitutionyear,
                    Address= partnerDTO.CoPartners.Address,
                    Phone= partnerDTO.CoPartners.Phone,
                    IdCompanyTypeCd= partnerDTO.CoPartners.IdCompanyTypeCd


                },
                SarlaftId = partnerDTO.SarlaftId
            };
        }

        public static CompanyFinalBeneficiary CreateFinalBeneficiary(FinalBeneficiaryDTO finalBeneficiary)
        {
            return new CompanyFinalBeneficiary
            {
                IdCardNumero = finalBeneficiary.IdCardNumero,
                TradeName = finalBeneficiary.TradeName,
                DocumentTypeId = finalBeneficiary.DocumentTypeId,
                Id = finalBeneficiary.Id,
                IndividualId = finalBeneficiary.IndividualId,
                SarlaftId = finalBeneficiary.SarlaftId
            };

        }

        public static List<CompanyFinalBeneficiary> CreateFinalBeneficiarys(List<FinalBeneficiaryDTO> finalBeneficiarys)
        {
            List<CompanyFinalBeneficiary> companyfinalBeneficiarys = new List<CompanyFinalBeneficiary>();
            if(finalBeneficiarys!= null )
            foreach (FinalBeneficiaryDTO finalBeneficiaryDTO in finalBeneficiarys)
            {
                companyfinalBeneficiarys.Add(ModelAssembler.CreateFinalBeneficiary(finalBeneficiaryDTO));
            }

            return companyfinalBeneficiarys;

        }

        public static List<CompanyIndividualPartner> CreatePartners(List<PartnersDTO> partnerDTOs)
        {
            List<CompanyIndividualPartner> companyIndividualPartners = new List<CompanyIndividualPartner>();
            foreach (PartnersDTO partnerDTO in partnerDTOs)
            {
                companyIndividualPartners.Add(CreatePartner(partnerDTO));
            }

            return companyIndividualPartners;
        }
        #endregion

        #region International Operations

        public static CompanySarlaftOperation CreateInternationalOperation(InternationalOperationDTO internationalOperationDTO)
        {
            return new CompanySarlaftOperation
            {
                Id = internationalOperationDTO.Id,
                SarlaftId = internationalOperationDTO.SarlaftId,
                ProductNum = internationalOperationDTO.ProductNum,
                ProductAmt = internationalOperationDTO.ProductAmt,
                Entity = internationalOperationDTO.Entity,
                CompanyOperationType = new CompanyOperationType
                {
                    Id = internationalOperationDTO.OperationTypeId
                },
                CompanyProductType = new CompanyProductType
                {
                    Id = internationalOperationDTO.ProductTypeId
                },
                CurrencyId = internationalOperationDTO.CurrencyId,
                CountryId = internationalOperationDTO.CountryId,
                CityId = internationalOperationDTO.CityId,
                StateId = internationalOperationDTO.StateId
            };
        }

        public static List<CompanySarlaftOperation> CreateInternationalOperations(List<InternationalOperationDTO> internationalOperationDTOs)
        {
            List<CompanySarlaftOperation> companySarlaftOperations = new List<CompanySarlaftOperation>();
            foreach (InternationalOperationDTO internationalOperationDTO in internationalOperationDTOs)
            {
                companySarlaftOperations.Add(CreateInternationalOperation(internationalOperationDTO));
            }

            return companySarlaftOperations;
        }

        #endregion

        #region Peps
        public static CompanyIndvidualPeps CreatePeps(PepsDTO PepsDTO)
        {
            return new CompanyIndvidualPeps
            {
                IndividualId = PepsDTO.Individual_Id,
                Exposed = PepsDTO.Exposed,
                TradeName = PepsDTO.Trade_Name,
                UnlinkedDATE = PepsDTO.Unlinked_DATE,

                Category = PepsDTO.Category,
                Link = PepsDTO.Link,
                Affinity = PepsDTO.Affinity,
                Unlinked = PepsDTO.Unlinked,

                Entity = PepsDTO.Entity,
                Observations = PepsDTO.Observations,
                JobOffice=PepsDTO.JobOffice,
                SarlaftId = PepsDTO.SarlaftId
            };
        }
        #endregion


        #region CoSarlaft
        public static CompanyCoSarlaft CreateCoSarlaft(CoSarlaftDTO coSarlaft)
        {
            return new CompanyCoSarlaft
            {

                SarlaftId = coSarlaft.sarlaftid,
                IndividualId = coSarlaft.individualid,
                Email = coSarlaft.email,
                Heritage = coSarlaft.heritage,
                IdCompanyTypeCode = coSarlaft.idCompanyTypeCode,
                CityCode = coSarlaft.cityCode,
                StateCode = coSarlaft.stateCode,
                CountryCode = coSarlaft.countryCode,
                OppositorTypeCode= coSarlaft.OppositorTypeCode,
                PersonTypeCode= coSarlaft.PersonTypeCode,
                SocietyTypeCode =coSarlaft.SocietyTypeCode,
                NationalityCode= coSarlaft.NationalityCode,
                NationalityOtherCode= coSarlaft.NationalityOtherCode,
                Phone= coSarlaft.Phone,
                ExonerationTypeCode= coSarlaft.ExonerationTypeCode,
                MainAddressNatural = coSarlaft.MainAddressNatural


            };
        }
        #endregion


        public static Sistran.Company.Application.UniquePersonServices.V1.Models.CompanySarlaftExoneration CreateExoneration(SarlaftExonerationtDTO exoneration)
        {
            return new Sistran.Company.Application.UniquePersonServices.V1.Models.CompanySarlaftExoneration
            {
                
                 ExonerationType = new UniquePersonServices.V1.Models.CompanyExonerationType 
                 {
                     Id = (int)exoneration.ExonerationType,
                     IndividualTypeCode = (int)exoneration.ExonerationType
                 } ,
                  Id =    exoneration.IndividualId,
                  IsExonerated= exoneration.IsExonerated,
                  EnteredDate = exoneration.RegistrationDate

            };
        }


        public static SarlaftExonerationtDTO CreateExoneration( Sistran.Company.Application.UniquePersonServices.V1.Models.CompanySarlaftExoneration exoneration)
        {
            return new SarlaftExonerationtDTO
            {

                ExonerationType= exoneration.ExonerationType.IndividualTypeCode,
                IndividualId = exoneration.Id,
                IsExonerated = exoneration.IsExonerated
            };
        }

    }
}
