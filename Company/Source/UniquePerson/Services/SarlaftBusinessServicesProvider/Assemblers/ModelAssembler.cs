using Sistran.Company.Application.SarlaftBusinessServices.Models;
using Sistran.Core.Application.UniquePerson.Entities;
using COMMEN = Sistran.Core.Application.Common.Entities;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;
using System.Linq;
using UPCEN = Sistran.Company.Application.UniquePerson.Entities;
using System;
using UPENV1 = Sistran.Core.Application.UniquePersonV1.Entities;
using UCCEN = Sistran.Core.Application.UniquePerson.Entities;
using UUSM = Sistran.Core.Application.UniqueUserServices.Models;
using AutoMapper;
using UUENT = Sistran.Core.Application.UniqueUser.Entities;
using UPEN = Sistran.Core.Application.UniquePersonV1.Entities;
namespace Sistran.Company.Application.SarlaftBusinessServicesProvider.Assemblers
{
    /// <summary>
    /// Convertir de Entities a Modelos
    /// </summary>
    public class ModelAssembler
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelAssembler"/> class.
        /// </summary>
        protected ModelAssembler()
        { }

        #region MapperSarlaft
        public static CompanyPerson CreatePerson(Core.Application.UniquePerson.Entities.Person person)
        {
            return new CompanyPerson
            {
                IndividualId = person.IndividualId,
                EconomicActivity = new CompanyEconomicActivity
                {
                    Id = person.EconomicActivityCode
                },
                DocumentType = person.IdCardTypeCode,
                DocumentNumber = person.IdCardNo
            };
        }
        public static CompanyCompany CreateCompany(Core.Application.UniquePerson.Entities.Company company)
        {
            return new CompanyCompany
            {
                IndividualId = company.IndividualId,
                EconomicActivity = new CompanyEconomicActivity
                {
                    Id = company.EconomicActivityCode
                },
                DocumentType = company.TributaryIdTypeCode,
                DocumentNumber = company.TributaryIdNo,
                FullName = company.TradeName
            };
        }

        public static List<CompanyIndividualSarlaft> CreateSarlafts(List<UPCEN.IndividualSarlaft> sarlaft)
        {
            var result = new List<CompanyIndividualSarlaft>();
            foreach (var item in sarlaft)
            {
                result.Add(CreateSarlaft(item));
            }
            return result;
        }
        public static CompanyIndividualSarlaft CreateSarlaft(UPCEN.IndividualSarlaft sarlaft)
        {
            return new CompanyIndividualSarlaft
            {
                Id = sarlaft.SarlaftId,
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
                UserId = (int) (sarlaft?.UserId ?? 0),
                BranchId = sarlaft.BranchCode,
                EconomicActivity = new CompanyEconomicActivity
                {
                    Id = sarlaft.EconomicActivityCode
                },
                SecondEconomicActivity = new CompanyEconomicActivity
                {
                    Id = sarlaft.SecondEconomicActivityCode
                },
                InterviewResultId = sarlaft.InterviewResultCode,
                PendingEvent = sarlaft.PendingEvent
            };
        }

        public static CompanyFinancialSarlaft CreateFinancialSarlaft(UPCEN.FinancialSarlaft financialSarlaft)
        {
            return new CompanyFinancialSarlaft
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

        /// <summary>
        /// Creates the sarlaft exoneration.
        /// </summary>
        /// <param name="sarlaftExoneration">The sarlaft exoneration.</param>
        /// <returns></returns>
        public static CompanySarlaftExoneration CreateSarlaftExoneration(IndividualSarlaftExoneration sarlaftExoneration)
        {
            return new CompanySarlaftExoneration
            {
                IndividualId = sarlaftExoneration.IndividualId,
                RegistrationDate = (System.DateTime)sarlaftExoneration.RegistrationDate,
                UserId = sarlaftExoneration.UserId,
                IsExonerated = sarlaftExoneration.IsExonerated,
                ExonerationType = 1
            };
        }

        /// <summary>
        /// Creates the sarlaft exonerations.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<CompanySarlaftExoneration> CreateSarlaftExonerations(BusinessCollection businessCollection)
        {
            List<CompanySarlaftExoneration> sarlaftExonerations = new List<CompanySarlaftExoneration>();

            foreach (IndividualSarlaftExoneration field in businessCollection)
            {
                sarlaftExonerations.Add(ModelAssembler.CreateSarlaftExoneration(field));
            }

            return sarlaftExonerations;
        }

        public static CompanyEconomicActivity CreateCompanyEconomicActivity(COMMEN.EconomicActivity entityEconomicActivity)
        {
            return new CompanyEconomicActivity
            {
                Id = entityEconomicActivity.EconomicActivityCode,
                Description = entityEconomicActivity.EconomicActivityCode + " - " + entityEconomicActivity.Description,
                SmallDescription = entityEconomicActivity.SmallDescription
            };
        }

        public static CompanyEconomicActivity CreateCompanyCoEconomicActivity(COMMEN.EconomicActivity entityEconomicActivity)
        {
            return new CompanyEconomicActivity
            {
                Id = entityEconomicActivity.EconomicActivityCode,
                Description = entityEconomicActivity.EconomicActivityCode + " - " + entityEconomicActivity.Description,
                SmallDescription = entityEconomicActivity.SmallDescription
            };
        }

        public static List<CompanyEconomicActivity> CreateCompanyEconomicActivities(BusinessCollection businessCollection)
        {
            List<CompanyEconomicActivity> companyEconomicActivities = new List<CompanyEconomicActivity>();
            foreach (COMMEN.EconomicActivity entityEconomicActivity in businessCollection)
            {
                companyEconomicActivities.Add(CreateCompanyEconomicActivity(entityEconomicActivity));
            }
            return companyEconomicActivities;
        }

        public static List<CompanyEconomicActivity> CreateCompanyCoEconomicActivities(BusinessCollection businessCollection)
        {
            List<CompanyEconomicActivity> companyEconomicActivities = new List<CompanyEconomicActivity>();
            foreach (COMMEN.EconomicActivity entityEconomicActivity in businessCollection)
            {
                companyEconomicActivities.Add(CreateCompanyCoEconomicActivity(entityEconomicActivity));
            }
            return companyEconomicActivities;
        }

        public static CompanyTmpSarlaftOperation CreateCompanyTmpSarlaftOperation(UPENV1.TmpSarlaftOperation entityTmpSarlaftOperation)
        {
            return new CompanyTmpSarlaftOperation
            {
                OperationId = entityTmpSarlaftOperation.OperationId,
                IndividualId = entityTmpSarlaftOperation.IndividualId.GetValueOrDefault(),
                SarlaftId = entityTmpSarlaftOperation.SarlaftId.GetValueOrDefault(),
                Operation = entityTmpSarlaftOperation.Operation,
                Proccess = entityTmpSarlaftOperation.Proccess,
                TypeProccess = entityTmpSarlaftOperation.TypeProccess,
                FunctionId = entityTmpSarlaftOperation.FunctionId
            };
        }
        #endregion

        #region Links
        public static CompanyRelationShip CreateRelationShip(UPCEN.RelationshipSarlaft entityCoRelationShip)
        {
            return new CompanyRelationShip
            {
                Id = entityCoRelationShip.RelationshipSarlaftCode,
                Description = entityCoRelationShip.Description
            };

        }

        public static List<CompanyRelationShip> CreateRelationShips(BusinessCollection businessCollection)
        {
            List<CompanyRelationShip> companyRelationShip = new List<CompanyRelationShip>();
            foreach (UPCEN.RelationshipSarlaft entityCoRelationShip in businessCollection)
            {
                companyRelationShip.Add(CreateRelationShip(entityCoRelationShip));
            }

            return companyRelationShip;
        }

        public static CompanyIndvidualLink CreateIndividualLink(UPCEN.IndividualLink entityCoindividualLink)
        {
            return new CompanyIndvidualLink
            {
                IndividualId = entityCoindividualLink.IndividualId,
                LinkType = new CompanyLinkType
                {
                    Id = entityCoindividualLink.LinkTypeCode
                },
                RelationshipSarlaft = new CompanyRelationShip
                {
                    Id =
                entityCoindividualLink.RelationshipSarlaftCode
                },
                Description = entityCoindividualLink.Description
            };

        }

        public static List<CompanyIndvidualLink> CreateIndividualLinks(BusinessCollection businessCollection)
        {
            List<CompanyIndvidualLink> companyIndividualLink = new List<CompanyIndvidualLink>();
            foreach (UPCEN.IndividualLink entityCoIndividualLink in businessCollection)
            {
                companyIndividualLink.Add(CreateIndividualLink(entityCoIndividualLink));
            }

            return companyIndividualLink;
        }

        #endregion

        #region LegalRepresentative
        public static CompanyLegalRepresentative CreateLegalRepresent(UPCEN.IndividualLegalRepresent entityLegalRepresent)
        {
            return new CompanyLegalRepresentative
            {
                IndividualId = entityLegalRepresent.IndividualId,
                LegalRepresentativeName = entityLegalRepresent.LegalRepresentativeName,
                ExpeditionDate = entityLegalRepresent?.ExpeditionDate ?? DateTime.MinValue,
                ExpeditionPlace = entityLegalRepresent.ExpeditionPlace,
                BirthDate = entityLegalRepresent?.BirthDate ?? DateTime.MinValue,
                BirthPlace = entityLegalRepresent.BirthPlace,
                City = entityLegalRepresent.City,
                Phone = entityLegalRepresent.Phone??0,
                JobTitle = entityLegalRepresent.JobTitle,
                CellPhone = entityLegalRepresent.CellPhone??0,
                Email = (String.IsNullOrEmpty(entityLegalRepresent.Email))?"":entityLegalRepresent.Email,
                Address = entityLegalRepresent.Address,
                CountryId = entityLegalRepresent?.CountryCode ?? 0,
                StateId = entityLegalRepresent?.StateCode ?? 0,
                CityId = entityLegalRepresent?.CityCode ?? 0,
                Nationality = entityLegalRepresent.Nationality,
                IdCardNo = entityLegalRepresent.IdCardNo,
                AuthorizationAmount = entityLegalRepresent.AuthorizationAmount,
                Description = entityLegalRepresent.Description,
                CurrencyId = entityLegalRepresent.CurrencyCode,
                IdCardTypeCode = entityLegalRepresent.IdCardTypeCode,
                IsMain = entityLegalRepresent?.IsMain ?? true,
                NationalityType = entityLegalRepresent?.NationalityTypeCode ?? 0,
                NationalityOtherType = entityLegalRepresent?.NationalityOtherTypeCode  ?? 0,
                LegalRepresentType = entityLegalRepresent?.LegalRepresentTypeCode ?? 1
            };
        }

        public static CompanyLegalRepresentative CreateLegalRepresent(UPCEN.IndividualSubstituteLegalRepresent entityLegalRepresent)
        {
            return new CompanyLegalRepresentative
            {
                IndividualId = entityLegalRepresent.IndividualId,
                LegalRepresentativeName = entityLegalRepresent.LegalRepresentativeName,
                ExpeditionDate = entityLegalRepresent.ExpeditionDate,
                ExpeditionPlace = entityLegalRepresent.ExpeditionPlace,
                BirthDate = entityLegalRepresent.BirthDate,
                BirthPlace = entityLegalRepresent.BirthPlace,
                City = entityLegalRepresent.City,
                Phone = ((int?)entityLegalRepresent.Phone) ?? 0,
                JobTitle = entityLegalRepresent.JobTitle,
                CellPhone = (int?)entityLegalRepresent.CellPhone ?? 0,
                Email = (String.IsNullOrEmpty(entityLegalRepresent.Email)) ? "" : entityLegalRepresent.Email,
                Address = entityLegalRepresent.Address,
                CountryId = entityLegalRepresent.CountryCode,
                StateId = entityLegalRepresent.StateCode,
                CityId = entityLegalRepresent.CityCode,
                Nationality = entityLegalRepresent.Nationality,
                IdCardNo = entityLegalRepresent.IdCardNo,
                AuthorizationAmount = entityLegalRepresent.AuthorizationAmount,
                Description = entityLegalRepresent.Description,
                CurrencyId = entityLegalRepresent.CurrencyCode,
                IdCardTypeCode = entityLegalRepresent.IdCardTypeCode,
                IsMain = false
            };
        }

        public static List<CompanyLegalRepresentative> CreateLegalRepresents(BusinessCollection businessCollection)
        {
            List<CompanyLegalRepresentative> legalRepresentatives = new List<CompanyLegalRepresentative>();
            foreach (UPCEN.IndividualLegalRepresent field in businessCollection)
            {
                legalRepresentatives.Add(ModelAssembler.CreateLegalRepresent(field));
            }
            return legalRepresentatives;
        }

        public static List<CompanyLegalRepresentative> CreateSubstituteLegalRepresents(BusinessCollection businessCollection)
        {
            List<CompanyLegalRepresentative> legalRepresentatives = new List<CompanyLegalRepresentative>();
            foreach (UPCEN.IndividualSubstituteLegalRepresent field in businessCollection)
            {
                legalRepresentatives.Add(ModelAssembler.CreateLegalRepresent(field));
            }
            return legalRepresentatives;
        }
        #endregion

        #region Partners
        public static CompanyIndividualPartner CreateIndividualPartner(UPCEN.IndividualPartner entityPartner)
        {
            return new CompanyIndividualPartner
            {

                Id = entityPartner.PartnerId,
                IdCardNumero = entityPartner.IdCardNo,
                IndividualId = entityPartner.IndividualId,
                DocumentTypeId = entityPartner.IdCardTypeCode,
                TradeName = entityPartner.TradeName,
                Active = entityPartner.Active,
                Participation = entityPartner?.Participation ?? 0,
                SarlaftId = entityPartner.SarlaftId
            };
        }

        public static CompanyFinalBeneficiary CreateBeneficiaryPartner(UPCEN.CoBeneficiaryPartner entityBeneficiaryPartner)
        {
            return new CompanyFinalBeneficiary
            {

                Id = entityBeneficiaryPartner.PartnerId,
                IdCardNumero = entityBeneficiaryPartner.IdCardNo,
                IndividualId = entityBeneficiaryPartner.IndividualId,
                DocumentTypeId = entityBeneficiaryPartner.IdCardTypeCode,
                TradeName = entityBeneficiaryPartner.TradeName,
                SarlaftId = entityBeneficiaryPartner.SarlaftId
            };
        }

        public static CompanyCoIndividualPartner CreateCoIndividualPartner(UPCEN.CoIndividualPartner entityPartner)
        {
            return new CompanyCoIndividualPartner
            {

                Id = entityPartner.PartnerId,
                IdCardNumero = entityPartner.IdCardNo,
                IndividualId = entityPartner.IndividualId,
                DocumentTypeId = entityPartner.IdCardTypeCode
           
            };
        }

        public static List<CompanyFinalBeneficiary> CreateBeneficiaryPartner(BusinessCollection businessCollection)
        {
            List<CompanyFinalBeneficiary> partners = new List<CompanyFinalBeneficiary>();

            foreach (UPCEN.CoBeneficiaryPartner entityPartner in businessCollection)
            {
                partners.Add(CreateBeneficiaryPartner(entityPartner));
            }

            return partners;
        }

        public static List<CompanyIndividualPartner> CreateIndividualPartners(BusinessCollection businessCollection)
        {
            List<CompanyIndividualPartner> partners = new List<CompanyIndividualPartner>();

            foreach (UPCEN.IndividualPartner entityPartner in businessCollection)
            {
                partners.Add(CreateIndividualPartner(entityPartner));
            }

            return partners;
        }
        #endregion

        #region International Operations
        public static CompanySarlaftOperation CreateCompanySarlaftOperation(UPCEN.SarlaftOperation entitySarlaftOperation)
        {
            return new CompanySarlaftOperation
            {
                Id = entitySarlaftOperation.SarlaftOperationId,
                SarlaftId = entitySarlaftOperation.SarlaftId,
                ProductNum = entitySarlaftOperation.ProductNum,
                ProductAmt = entitySarlaftOperation.ProductAmount,
                Entity = entitySarlaftOperation.Entity,
                CompanyOperationType = new CompanyOperationType
                {
                    Id = entitySarlaftOperation.OperationTypeCode
                },
                CompanyProductType = new CompanyProductType
                {
                    Id = entitySarlaftOperation.ProductTypeCode
                },
                CurrencyId = entitySarlaftOperation.CurrencyCode,
                CountryId = entitySarlaftOperation.CountryCode,
                CityId = entitySarlaftOperation.CityCode,
                StateId = entitySarlaftOperation.StateCode
            };

        }

        public static List<CompanySarlaftOperation> CreateCompanySarlaftOperations(BusinessCollection businessCollection)
        {
            List<CompanySarlaftOperation> companySarlaftOperations = new List<CompanySarlaftOperation>();
            foreach (UPCEN.SarlaftOperation entitySarlaftOperation in businessCollection)
            {
                companySarlaftOperations.Add(CreateCompanySarlaftOperation(entitySarlaftOperation));
            }
            return companySarlaftOperations;
        }

        #endregion

        #region Peps


        public static CompanyIndvidualPeps CreateIndividualPeps(UPCEN.IndividualPeps entityPeps)
        {
            return new CompanyIndvidualPeps
            {
                IndividualId = entityPeps.IndividualId,
                Entity = entityPeps.Entity,
                Unlinked = entityPeps.IdUnlinkedTypeCode,


                 Affinity = entityPeps.IdAffinityTypeCode,


                Link = entityPeps.IdLinkTypeCode,


                Category = entityPeps.IdCategoryTypeCode,


                UnlinkedDATE = entityPeps.UnlinkedDate,


                Exposed = entityPeps.Exposed,

                Observations = entityPeps.Observations,

                TradeName = entityPeps.TradeName,
                JobOffice = entityPeps.JobOffice


            };
        }

        public static CompanyIndvidualPeps CreateIndividualPeps(BusinessCollection businessCollection)
        {
            CompanyIndvidualPeps indvidualPeps = new CompanyIndvidualPeps();

            foreach (UPCEN.IndividualPeps entityPeps in businessCollection)
            {
                indvidualPeps= CreateIndividualPeps(entityPeps);
            }

            return indvidualPeps;
        }

        #endregion
        #region CoSarlaft


        public static CompanyCoSarlaft CreateCoSarlaft(UPCEN.CoIndividualSarlaft entityCoSarlaft)
        {
            return new CompanyCoSarlaft
            {
                SarlaftId= entityCoSarlaft.SarlaftId,
                IndividualId = entityCoSarlaft.IndividualId,
                Email = entityCoSarlaft.Email,
                Heritage = entityCoSarlaft.Heritage,
                IdCompanyTypeCode = entityCoSarlaft.IdCompanyTypeCode,
                CityCode = entityCoSarlaft.CityCode,
                StateCode = entityCoSarlaft.StateCode,
                CountryCode = entityCoSarlaft.CountryCode,

                OppositorTypeCode = entityCoSarlaft.OppositorCode,
                PersonTypeCode = entityCoSarlaft.PersonCode,
                SocietyTypeCode = entityCoSarlaft.SocietyCode,
                NationalityCode = entityCoSarlaft.NationalityCode,
                NationalityOtherCode = entityCoSarlaft.NationalityOtherCode,
                Phone = entityCoSarlaft.Phone,
                ExonerationTypeCode = entityCoSarlaft.ExonerationCode,
                MainAddressNatural = entityCoSarlaft.MainAddress


            };
        }

        public static CompanyCoSarlaft CreateCoSarlaft(BusinessCollection businessCollection)
        {
            CompanyCoSarlaft indvidualCoSarlaft = new CompanyCoSarlaft();

            foreach (UPCEN.CoIndividualSarlaft entityCoSarlaft in businessCollection)
            {
                indvidualCoSarlaft = CreateCoSarlaft(entityCoSarlaft);
            }

            return indvidualCoSarlaft;
        }



        #endregion

        public static CompanyRole CreateRole(UCCEN.Role entityRole)
        {
            return new CompanyRole
            {
                RoleCd = entityRole.RoleCode,
                Description = entityRole.Description


            };
        }

        public static List<CompanyRole> CreateRoles(BusinessCollection businessCollection)
        {
            List<CompanyRole> Roles = new List<CompanyRole>();
            foreach (UCCEN.Role entityRoles in businessCollection)
            {
                Roles.Add(CreateRole(entityRoles));
            }
            return Roles;
        }


        public static List<CompanyEntity> CreateEntity(BusinessCollection businessCollection)
        {
            List<CompanyEntity> Entitys = new List<CompanyEntity>();
            foreach (Object entity in businessCollection)
            {
                Entitys.Add(CreateEntity(entity));
            }
            return Entitys;
        }

        public static CompanyEntity CreateEntity(Object entity)
        {
            return new CompanyEntity
            {
                Id = (int)(entity.GetType().GetProperties()[0]).GetValue(entity),
                Description = (entity.GetType().GetProperties()[1]).GetValue(entity).ToString()


            };
        }

        /// <summary>
        /// Get list of Models.UniqueUsers
        /// </summary>
        /// <param name="businessCollection">lista de objetos.</param>
        /// <returns>list of Models.User</returns>
        public static List<UUSM.User> CreateUniqueUsers(Sistran.Core.Framework.DAF.BusinessCollection businessCollection, Sistran.Core.Framework.DAF.BusinessCollection person)
        {
            if (businessCollection != null && businessCollection.Count > 0)
            {
                List<UUSM.User> uniqueUsers = new List<UUSM.User>();
                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<UUENT.UniqueUsers, UUSM.User>()
                    .ForMember(x => x.CreationDate, opts => opts.MapFrom(y => y.CreatedDate))
                    .ForMember(x => x.LastModificationDate, opts => opts.MapFrom(y => y.ModifiedDate))
                    .ForMember(x => x.LastModificationDate, opts => opts.MapFrom(y => y.ModifiedDate));
                    cfg.CreateMap<List<UUENT.UniqueUsers>, List<UUSM.User>>()
                   .ConvertUsing(ss => ss.Select(bs => Mapper.Map<UUENT.UniqueUsers, UUSM.User>(bs)).ToList());
                });
                uniqueUsers = Mapper.Map<List<UUENT.UniqueUsers>, List<UUSM.User>>(businessCollection.Cast<UUENT.UniqueUsers>().ToList());
                var persons = person.Cast<Sistran.Core.Application.UniquePerson.Entities.Person>().ToList();
                uniqueUsers.AsParallel().ForAll(x => x.Name = persons?.FirstOrDefault(y => y.IndividualId == x.PersonId)?.Name + " " + persons?.FirstOrDefault(y => y.IndividualId == x.PersonId)?.Surname);
                Mapper.Reset();
                return uniqueUsers;
            }
            else
            {
                return null;
            }
        }

        public static List<UUSM.User> CreateUserSarlaft(List<UUSM.User> listUsers ,Sistran.Core.Framework.DAF.BusinessCollection businessCollection)
        {
            if (businessCollection != null && businessCollection.Count > 0)
            {
                UUSM.User uniqueUsers;

            
                foreach (UPEN.Insured entity in businessCollection)
                {
                    uniqueUsers = new UUSM.User
                    {
                        AccountName = entity.CheckPayableTo,
                        PersonId = entity.IndividualId
                    };

                    listUsers.Add(uniqueUsers);


                }
            
            }
            
                return listUsers;
            
        }




    }
}
