using Sistran.Company.Application.ModelServices.Models;
using Sistran.Company.Application.UniquePersonAplicationServices.DTOs;
using System;
using System.Collections.Generic;
using UPV1 = Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using AutoMapper;
using Sistran.Core.Application.Utilities.Cache;
using System.Linq;
using Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Company.Application.CommonServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;

namespace Sistran.Core.Framework.UIF.Web.Areas.Person.Models
{
    public class ModelAssembler
    {

        #region Person y Company DTO's to Company Models
        public static CompanyCompany CreateCompany(CompanyDTO company)
        {
            var result = new CompanyCompany();

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
            result.Consortiums = new List<CompanyConsortium>();
            result.Insured = company.Insured != null ? CreateInsured(company.Insured) : null;
            result.CheckPayable = company.CheckPayable;
            result.UserId = company.UserId;
            result.IndividualType = IndividualType.Company;
            result.CustomerType = CustomerType.Individual;
            return result;
        }

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
                Branch = insured.BranchId == null ? null : new CompanyBranch { Id = Convert.ToInt32(insured.BranchId) },
                Segment = insured.InsSegmentId == null ? null : new CompanyInsuredSegment { Id = Convert.ToInt32(insured.InsSegmentId) },
                IsSMS = insured.IsSms,
                IsMailAddress = insured.IsMailAddress,
                UserId = insured.UserId

            };
            return companyInsured;
        }

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
            companyConsortium.CustomerType = CustomerType.Individual;
            companyConsortium.IndividualType = IndividualType.Company;
            if (model.Person != null)
            {
                companyConsortium.Person = new CompanyPerson
                {
                    IdentificationDocument = new CompanyIdentificationDocument { Number = model.PersonIdentificationNumber },
                    CustomerType = CustomerType.Individual,
                    IndividualType = IndividualType.Person
                };
            }
            if (model.Company != null)
            {
                companyConsortium.Company = new CompanyCompany()
                {
                    IdentificationDocument = new CompanyIdentificationDocument { Number = model.PersonIdentificationNumber },
                    CustomerType = CustomerType.Individual,
                    IndividualType = IndividualType.Company
                };
            }

            return companyConsortium;
        }

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
            result.Name = person.Names;
            result.SecondSurName = person.SecondSurname;
            result.CheckPayable = person.CheckPayable;
            result.UserId = person.UserId;
            result.DataProtection = person.DataProtection;
            result.IndividualType = IndividualType.Person;
            result.CustomerType = CustomerType.Individual;
            return result;
        }

        #endregion




        public static UPV1.CompanyProspectNatural CreateCompanyProspecNatural(ProspectPersonNaturalDTO prospectModel)
        {

            UPV1.CompanyProspectNatural CompanyProspectNatural = new UPV1.CompanyProspectNatural();
            CompanyProspectNatural.AdditionalInfo = prospectModel.AdditionaInformation;
            CompanyProspectNatural.BirthDate = prospectModel.BirthDate;
            CompanyProspectNatural.AddressType = prospectModel.Address.Id;
            CompanyProspectNatural.Street = prospectModel.Address.Description;
            CompanyProspectNatural.CityCode = prospectModel.City.Id;
            CompanyProspectNatural.CountryCode = prospectModel.Country.Id;
            CompanyProspectNatural.StateCode = prospectModel.State.Id;
            if (prospectModel.City.Id != null && prospectModel.City.Id != 0)
            {
                CompanyProspectNatural.City = new UPV1.CompanyCity()
                {
                    Id = (int)prospectModel.City.Id,
                    Description = prospectModel.City.Description,
                    State = new UPV1.CompanyState()
                    {
                        Id = (int)prospectModel.State.Id,
                        Description = prospectModel.State.Description,
                        Country = new UPV1.CompanyCountry()
                        {
                            Id = (int)prospectModel.Country.Id,
                            Description = prospectModel.Country.Description
                        }
                    },
                    DANECode = prospectModel.DANECode,


                };
            }

            CompanyProspectNatural.EmailAddress = prospectModel.EmailAddres;
            CompanyProspectNatural.Gender = prospectModel.Gender;
            CompanyProspectNatural.IdCardTypeCode = prospectModel.Card.Id;
            CompanyProspectNatural.IdCardNo = prospectModel.Card.Description;
            CompanyProspectNatural.MaritalStatus = prospectModel.MartialStatus;
            CompanyProspectNatural.MotherLastName = prospectModel.MotherLastName;
            CompanyProspectNatural.Name = prospectModel.Name;
            CompanyProspectNatural.PhoneNumber = prospectModel.PhoneNumber;
            CompanyProspectNatural.ProspectCode = prospectModel.ProspectCode;
            CompanyProspectNatural.Surname = prospectModel.SurName;
            CompanyProspectNatural.IndividualTypePerson = prospectModel.IndividualTypePerson;
            CompanyProspectNatural.IndividualTyepCode = prospectModel.IndividualTypePerson;



            return CompanyProspectNatural;

        }

        public static ProspectPersonNaturalDTO CreateCompanyProspecNaturalDto(UPV1.CompanyProspectNatural prospectNatural)
        {
            ProspectPersonNaturalDTO prospectPersonNaturalDTO = new ProspectPersonNaturalDTO();
            prospectPersonNaturalDTO.AdditionaInformation = prospectNatural.AdditionalInfo;
            prospectPersonNaturalDTO.BirthDate = prospectNatural.BirthDate;
            prospectPersonNaturalDTO.Address = new SelectDTO()
            {
                Id = prospectNatural.AddressType,
                Description = prospectNatural.Street
            };
            if (prospectPersonNaturalDTO.City != null)
            {
                prospectPersonNaturalDTO.City = new SelectDTO()
                {
                    Id = prospectNatural.City.Id,
                    Description = prospectNatural.City.Description
                };
                prospectPersonNaturalDTO.State = new SelectDTO()
                {
                    Id = prospectNatural.City.State.Id,
                    Description = prospectNatural.City.State.Description
                };
                prospectPersonNaturalDTO.Country = new SelectDTO()
                {
                    Id = prospectNatural.City.State.Country.Id,
                    Description = prospectNatural.City.State.Country.Description
                };

                prospectPersonNaturalDTO.DANECode = prospectNatural.City.DANECode;
            }


            prospectPersonNaturalDTO.EmailAddres = prospectNatural.EmailAddress;
            prospectPersonNaturalDTO.Gender = prospectNatural.Gender;
            prospectPersonNaturalDTO.Card = new SelectDTO()
            {
                Id = prospectNatural.IdCardTypeCode,
                Description = prospectNatural.IdCardNo
            };
            prospectPersonNaturalDTO.MartialStatus = prospectNatural.MaritalStatus;
            prospectPersonNaturalDTO.MotherLastName = prospectNatural.MotherLastName;
            prospectPersonNaturalDTO.Name = prospectNatural.Name;
            prospectPersonNaturalDTO.PhoneNumber = prospectNatural.PhoneNumber;
            prospectPersonNaturalDTO.ProspectCode = prospectNatural.ProspectCode;
            prospectPersonNaturalDTO.SurName = prospectNatural.Surname;
            prospectPersonNaturalDTO.IndividualTypePerson = prospectNatural.IndividualTypePerson;

            return prospectPersonNaturalDTO;
        }

        internal static List<EconomicGroupView> CreateEconomicGroupView(object p)
        {
            throw new NotImplementedException();
        }

        #region CompanyScoreTypeDoc
        public static List<ScoreTypeDocViewModel> GetScoreTypeDocs(List<UPV1.ScoreTypeDoc> scoreTypeDocs)
        {
            List<ScoreTypeDocViewModel> scoreTypeDocViewModel = new List<ScoreTypeDocViewModel>();
            foreach (UPV1.ScoreTypeDoc model in scoreTypeDocs)
            {
                ScoreTypeDocViewModel scoreTypeDocModel = new ScoreTypeDocViewModel();
                scoreTypeDocModel.IdCardTypeScore = model.IdCardTypeScore;
                scoreTypeDocModel.Description = model.Description;
                scoreTypeDocModel.SmallDescription = model.SmallDescription;
                scoreTypeDocModel.IdCardTypeCode = model.IdCardTypeCode;
                scoreTypeDocModel.IdScore3g = model.IdScore3g;

                scoreTypeDocViewModel.Add(scoreTypeDocModel);
            }

            return scoreTypeDocViewModel;
        }

        public static List<UPV1.ScoreTypeDoc> CreateScoreTypeDocs(List<ScoreTypeDocViewModel> scoreTypeDocs)
        {
            if (scoreTypeDocs == null)
                return null;
            List<UPV1.ScoreTypeDoc> scoreTypeDocViewModel = new List<UPV1.ScoreTypeDoc>();
            foreach (ScoreTypeDocViewModel model in scoreTypeDocs)
            {
                UPV1.ScoreTypeDoc scoreTypeDocModel = new UPV1.ScoreTypeDoc();
                scoreTypeDocModel.IdCardTypeScore = model.IdCardTypeScore;
                scoreTypeDocModel.Description = model.Description;
                scoreTypeDocModel.SmallDescription = model.SmallDescription;
                scoreTypeDocModel.IdCardTypeCode = model.IdCardTypeCode;
                scoreTypeDocModel.IdScore3g = model.IdScore3g;

                scoreTypeDocViewModel.Add(scoreTypeDocModel);
            }

            return scoreTypeDocViewModel;
        }
        #endregion
        public static BasicCompanyServiceModel CreateBasicCompany(CompanyBasicViewModel companyBasicViewModel)
        {
            return new BasicCompanyServiceModel()
            {
                CompanyCode = companyBasicViewModel.CompanyCode,
                CompanyDigit = Convert.ToInt16(companyBasicViewModel.CompanyDigit),
                CompanyTypePartnership = companyBasicViewModel.CompanyTypePartnership,
                Country = companyBasicViewModel.Country,
                DocumentNumber = companyBasicViewModel.DocumentNumberCompany,
                DocumentType = companyBasicViewModel.DocumentTypeCompany,
                IndividualId = companyBasicViewModel.IndividualId,
                LastUpdate = companyBasicViewModel.LastUpdateCompany,
                TradeName = companyBasicViewModel.TradeName,
                TypePartnership = companyBasicViewModel.TypePartnership,
                UpdateBy = companyBasicViewModel.UpdateByCompany
            };
        }

        #region CompanyAddressType
        public static List<CompanyAddressTypeViewModel> GetCompanyAddressTypes(List<UPV1.CompanyAddressType> companyAddressTypes)
        {
            List<CompanyAddressTypeViewModel> companyAddressTypeViewModel = new List<CompanyAddressTypeViewModel>();
            foreach (UPV1.CompanyAddressType model in companyAddressTypes)
            {
                CompanyAddressTypeViewModel companyAddressTypeModel = new CompanyAddressTypeViewModel();
                companyAddressTypeModel.AddressTypeCode = model.AddressTypeCode;
                companyAddressTypeModel.SmallDescription = model.SmallDescription;
                companyAddressTypeModel.TinyDescription = model.TinyDescription;
                companyAddressTypeModel.IsElectronicMail = model.IsElectronicMail;
                companyAddressTypeModel.IsForeing = model.IsForeing;
                if (model.IsForeing == true)
                {
                    companyAddressTypeModel.AllowDelete = false;
                }
                else
                {
                    companyAddressTypeModel.AllowDelete = true;
                }
                companyAddressTypeViewModel.Add(companyAddressTypeModel);
            }

            return companyAddressTypeViewModel;
        }

        public static List<UPV1.CompanyAddressType> CreateCompanyAddressTypes(List<CompanyAddressTypeViewModel> companyAddressTypes)
        {
            if (companyAddressTypes == null)
                return null;
            List<UPV1.CompanyAddressType> companyAddressTypeViewModel = new List<UPV1.CompanyAddressType>();
            foreach (CompanyAddressTypeViewModel model in companyAddressTypes)
            {
                UPV1.CompanyAddressType companyAddressTypeModel = new UPV1.CompanyAddressType();
                companyAddressTypeModel.AddressTypeCode = model.AddressTypeCode;
                companyAddressTypeModel.SmallDescription = model.SmallDescription;
                companyAddressTypeModel.TinyDescription = model.TinyDescription;
                companyAddressTypeModel.IsElectronicMail = model.IsElectronicMail;

                companyAddressTypeViewModel.Add(companyAddressTypeModel);
            }

            return companyAddressTypeViewModel;
        }
        #endregion

        #region CompanyPhoneType
        public static List<CompanyPhoneTypeViewModel> GetCompanyPhoneTypes(List<UPV1.CompanyPhoneType> companyPhoneTypes)
        {
            List<CompanyPhoneTypeViewModel> companyPhoneTypeViewModel = new List<CompanyPhoneTypeViewModel>();
            foreach (UPV1.CompanyPhoneType model in companyPhoneTypes)
            {
                CompanyPhoneTypeViewModel companyPhoneTypeModel = new CompanyPhoneTypeViewModel();
                companyPhoneTypeModel.PhoneTypeCode = model.PhoneTypeCode;
                companyPhoneTypeModel.Description = model.Description;
                companyPhoneTypeModel.SmallDescription = model.SmallDescription;
                companyPhoneTypeModel.IsCellphone = model.IsCellphone;
                companyPhoneTypeModel.RegExpression = model.RegExpression;
                companyPhoneTypeModel.ErrorMessage = model.ErrorMessage;
                companyPhoneTypeModel.IsForeing = model.IsForeing;
                if (model.IsForeing == true)
                {
                    companyPhoneTypeModel.AllowDelete = false;
                }
                else
                {
                    companyPhoneTypeModel.AllowDelete = true;
                }

                companyPhoneTypeViewModel.Add(companyPhoneTypeModel);
            }

            return companyPhoneTypeViewModel;
        }

        public static List<UPV1.CompanyPhoneType> CreateCompanyPhoneTypes(List<CompanyPhoneTypeViewModel> companyPhoneTypes)
        {
            if (companyPhoneTypes == null)
                return null;
            List<UPV1.CompanyPhoneType> companyPhoneTypeViewModel = new List<UPV1.CompanyPhoneType>();
            foreach (CompanyPhoneTypeViewModel model in companyPhoneTypes)
            {
                UPV1.CompanyPhoneType companyPhoneTypeModel = new UPV1.CompanyPhoneType();
                companyPhoneTypeModel.PhoneTypeCode = model.PhoneTypeCode;
                companyPhoneTypeModel.Description = model.Description;
                companyPhoneTypeModel.SmallDescription = model.SmallDescription;
                companyPhoneTypeModel.IsCellphone = model.IsCellphone;
                companyPhoneTypeModel.RegExpression = model.RegExpression;
                companyPhoneTypeModel.ErrorMessage = model.ErrorMessage;

                companyPhoneTypeViewModel.Add(companyPhoneTypeModel);
            }

            return companyPhoneTypeViewModel;
        }
        #endregion

        #region EconomicGroup

        public static EconomicGroup CreateEconomicGroup(EconomicGroupView economicGroup)
        {
            var immap = CreateMapEconomicGroup();
            var p = immap.Map<EconomicGroupView, EconomicGroup>(economicGroup);
            EconomicGroup coreEconomicGroup = new EconomicGroup();
            coreEconomicGroup = p;
            return coreEconomicGroup;
        }

        public static IMapper CreateMapEconomicGroup()
        {
            var config = MapperCache.GetMapper<EconomicGroupView, EconomicGroup>(cfg =>
            {
                cfg.CreateMap<EconomicGroupView, EconomicGroup>()
                .ForMember(x => x.EconomicGroupId, o => o.MapFrom(s => s.EconomicGroupId))
                .ForMember(x => x.EconomicGroupName, o => o.MapFrom(s => s.EconomicGroupName))
                .ForMember(x => x.DeclinedDate, o => o.MapFrom(s => s.DeclinedDate))
                .ForMember(x => x.Enabled, o => o.MapFrom(s => s.Enabled))
                .ForMember(x => x.EnteredDate, o => o.MapFrom(s => s.EnteredDate))
                .ForMember(x => x.OperationQuoteAmount, o => o.MapFrom(s => s.OperationQuoteAmount))
                .ForMember(x => x.TributaryIdNo, o => o.MapFrom(s => s.TributaryIdNo))
                .ForMember(x => x.TributaryIdType, o => o.MapFrom(s => s.TributaryIdType));
            });
            return config;
        }

        public static List<EconomicGroupView> CreateEconomicGroupView(List<EconomicGroup> economicGroup)
        {
            List<EconomicGroupView> listResult = new List<EconomicGroupView>();
            economicGroup.ForEach(x =>
            {
                EconomicGroupView record = new EconomicGroupView()
                {
                    EconomicGroupId = x.EconomicGroupId,
                    EconomicGroupName = x.EconomicGroupName,
                    DeclinedDate = x.DeclinedDate,
                    Enabled = x.Enabled,
                    EnteredDate = x.EnteredDate,
                    OperationQuoteAmount = x.OperationQuoteAmount,
                    TributaryIdNo = x.TributaryIdNo,
                    TributaryIdType = x.TributaryIdType,
                    UserId = x.UserId,
                    VerifyDigit = x.VerifyDigit
                };
                listResult.Add(record);
            });

            return listResult;
        }

        public static List<TributaryIdentityType> CreateTributaryType(List<TributaryTypeView> listTributaryTypeView)
        {
            List<TributaryIdentityType> listResult = new List<TributaryIdentityType>();
            if (listTributaryTypeView.Count > 0)
            {

                listTributaryTypeView.ForEach(x =>
                {
                    var immap = CreateTributaryTypes();
                    var p = immap.Map<TributaryTypeView, TributaryIdentityType>(x);
                    TributaryIdentityType coreTributaryType = new TributaryIdentityType();
                    coreTributaryType = p;
                    listResult.Add(coreTributaryType);
                });
            }

            return listResult;
        }

        public static List<TributaryTypeView> CreateTributaryTypeView(List<TributaryIdentityType> listTributaryTypeView)
        {
            List<TributaryTypeView> listResult = new List<TributaryTypeView>();
            if (listTributaryTypeView.Count > 0)
            {

                listTributaryTypeView.ForEach(x =>
                {
                    var immap = CreateTributaryTypes();
                    var p = immap.Map<TributaryIdentityType, TributaryTypeView>(x);
                    TributaryTypeView coreTributaryType = new TributaryTypeView();
                    coreTributaryType = p;
                    listResult.Add(coreTributaryType);
                });
            }

            return listResult;
        }

        public static IMapper CreateTributaryTypes()
        {
            var config = MapperCache.GetMapper<TributaryTypeView, TributaryIdentityType>(cfg =>
            {
                cfg.CreateMap<TributaryTypeView, TributaryIdentityType>()
                .ForMember(x => x.Id, o => o.MapFrom(s => s.Id))
                .ForMember(x => x.Description, o => o.MapFrom(s => s.Description))
                .ForMember(x => x.SmallDescription, o => o.MapFrom(s => s.SmallDescription));
            });
            return config;
        }

        public static List<EconomicGroupDetail> CreateEconomicGroupDetail(List<EconomicGroupDetailView> economicGroup)
        {
            List<EconomicGroupDetail> listResult = new List<EconomicGroupDetail>();
            if (economicGroup != null)
            {
                economicGroup.ForEach(x =>
                {
                    var immap = CreateMapEconomicGroupsDetail();
                    var p = immap.Map<EconomicGroupDetailView, EconomicGroupDetail>(x);
                    EconomicGroupDetail coreEconomicGroup = new EconomicGroupDetail();
                    coreEconomicGroup = p;
                    listResult.Add(coreEconomicGroup);
                });
            }

            return listResult;
        }

        public static List<EconomicGroupDetailView> CreateEconomicGroupDetailView(List<EconomicGroupDetail> economicGroup)
        {
            List<EconomicGroupDetailView> listResult = new List<EconomicGroupDetailView>();
            if (economicGroup != null)
            {
                economicGroup.ForEach(x =>
                {
                    var immap = CreateMapEconomicGroupsDetail();
                    var p = immap.Map<EconomicGroupDetail, EconomicGroupDetailView>(x);
                    EconomicGroupDetailView coreEconomicGroup = new EconomicGroupDetailView();
                    coreEconomicGroup = p;
                    listResult.Add(coreEconomicGroup);
                });
            }

            return listResult;
        }

        public static IMapper CreateMapEconomicGroupsDetail()
        {
            var config = MapperCache.GetMapper<EconomicGroupDetailView, EconomicGroupDetail>(cfg =>
            {
                cfg.CreateMap<EconomicGroupDetailView, EconomicGroupDetail>()
                .ForMember(x => x.EconomicGroupId, o => o.MapFrom(s => s.EconomicGroupId))
                .ForMember(x => x.Enabled, o => o.MapFrom(s => s.Enabled))
                .ForMember(x => x.IndividualId, o => o.MapFrom(s => s.IndividualId));
            });
            return config;
        }

        #endregion
    }
}