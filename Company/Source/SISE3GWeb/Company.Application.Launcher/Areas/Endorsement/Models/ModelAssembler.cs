using System;
using AutoMapper;
using Sistran.Company.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.Utilities.Cache;
namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models
{
    using Sistran.Company.Application.Adjustment.DTO;
    using Sistran.Company.Application.CommonServices.Models;
    using Sistran.Company.Application.UnderwritingServices;
    using Sistran.Company.Application.UnderwritingServices.Models;
    using Sistran.Core.Application.CommonService.Models;
    using Sistran.Core.Application.UnderwritingServices.Enums;
    using Sistran.Core.Application.UnderwritingServices.Models;
    using Sistran.Core.Framework.UIF.Web.Helpers;
    using CRM = Sistran.Company.Application.UnderwritingServices.Models;
    using RM = Sistran.Core.Application.UnderwritingServices.Models;
    public class ModelAssembler
    {
        #region Modelos Company


        /// <summary>
        /// Creates the company risk.
        /// </summary>
        /// <param name="risk">The risk.</param>
        /// <returns></returns>
        public static CompanyEndorsement CreateCompanyEndorsement(ModificationViewModel modificationViewModel)
        {
            var imapper = CreateMappCompanyEndorsementModification();
            var p = imapper.Map<ModificationViewModel, CompanyEndorsement>(modificationViewModel);

            CompanyEndorsement companyEndorsement = new CompanyEndorsement();
            companyEndorsement = p;
            companyEndorsement.IssueDate = modificationViewModel.IssueDate.ToDate(DateHelper.FormatFullDate);
            companyEndorsement.UserId = SessionHelper.GetUserId();
            companyEndorsement.Text.TextBody = companyEndorsement.Text.TextBody == "" ? " " : companyEndorsement.Text.TextBody;
            return companyEndorsement;
        }

        /// <summary>
        /// Creates the company policy.
        /// </summary>
        /// <param name="ModificationViewModel">modificationViewModel.</param>
        /// <returns></returns>
        public static CompanyPolicy CreateCompanyPolicy(ModificationViewModel modificationViewModel)
        {
            var imapper = CreateMappCompanyEndorsementModification();
            var p = imapper.Map<ModificationViewModel, CompanyEndorsement>(modificationViewModel);

            CompanyPolicy companyPolicy = new CompanyPolicy();
            companyPolicy.Endorsement = p;
            companyPolicy.Clauses = modificationViewModel.Clauses;
            companyPolicy.IssueDate = modificationViewModel.IssueDate.ToDate(DateHelper.FormatFullDate);
            companyPolicy.UserId = SessionHelper.GetUserId();
            return companyPolicy;
        }



        public static CompanyPolicy CreateCompanyEndorsement(ExtensionViewModel extensionViewModel)
        {

            var imapper = CreateMappCompanyEndorsementExtension();
            var p = imapper.Map<ExtensionViewModel, CompanyEndorsement>(extensionViewModel);
            CompanyPolicy companyPolicy = new CompanyPolicy();
            companyPolicy.Endorsement = p;
            companyPolicy.IssueDate = Convert.ToDateTime(extensionViewModel.IssueDate);
            companyPolicy.UserId = SessionHelper.GetUserId();
            return companyPolicy;
        }
        public static CompanyEndorsement CreateCompanyEndorsement(CancellationViewModel cancellationViewModel)
        {
            var imapper = CreateMappCompanyEndorsementCancellation();
            var p = imapper.Map<CancellationViewModel, CompanyEndorsement>(cancellationViewModel);

            CompanyEndorsement companyEndorsement = new CompanyEndorsement();
            companyEndorsement = p;
            companyEndorsement.IssueDate = cancellationViewModel.IssueDate.ToDate(DateHelper.FormatFullDate);
            companyEndorsement.UserId = SessionHelper.GetUserId();
            return companyEndorsement;
        }

        public static CompanyEndorsement CreateCompanyEndorsement(ReversionViewModel reversionViewModel)
        {
            var immap = CreateMappCompanyEndorsementreversion();
            var p = immap.Map<ReversionViewModel, CompanyEndorsement>(reversionViewModel);

            CompanyEndorsement companyEndorsement = new CompanyEndorsement();
            companyEndorsement = p;
            companyEndorsement.IssueDate = Convert.ToDateTime(Convert.ToDateTime(reversionViewModel.IssueDate).ToString("dd/MM/yyyy") + " " + DateTime.Now.ToString("HH:mm:ss"));
            companyEndorsement.UserId = SessionHelper.GetUserId();
            return companyEndorsement;
        }

        public static CompanyPolicy CreateCompanyEndorsement(ChangeAgentViewModel changeAgentViewModel)
        {
            var imapper = CreateMappCompanyEndorsementChangeAgent();
            var companyEndorsement = imapper.Map<ChangeAgentViewModel, CompanyEndorsement>(changeAgentViewModel);
            CompanyPolicy companyPolicy = new CompanyPolicy();
            companyPolicy.CurrentFrom = Convert.ToDateTime(changeAgentViewModel.CurrentFrom);
            companyPolicy.CurrentTo = Convert.ToDateTime(changeAgentViewModel.CurrentTo);
            companyPolicy.Endorsement = companyEndorsement;
            companyPolicy.Agencies = changeAgentViewModel.Agencies;
            companyPolicy.Id = changeAgentViewModel.Id;
            companyPolicy.UserId = SessionHelper.GetUserId();
            companyPolicy.Endorsement.UserId = companyPolicy.UserId;
            companyPolicy.Text = new CompanyText()
            {
                TextBody = changeAgentViewModel.Text
            };
            return companyPolicy;
        }

        public static CompanyPolicy CreateCompanyEndorsement(ChangeCoinsuranceViewModel changeCoinsuranceViewModel)
        {
            var imapper = CreateMappCompanyEndorsementChangeCoinsurance();
            var companyEndorsement = imapper.Map<ChangeCoinsuranceViewModel, CompanyEndorsement>(changeCoinsuranceViewModel);
            CompanyPolicy companyPolicy = new CompanyPolicy();
            companyPolicy.CurrentFrom = Convert.ToDateTime(changeCoinsuranceViewModel.CurrentFrom);
            companyPolicy.CurrentTo = Convert.ToDateTime(changeCoinsuranceViewModel.CurrentTo);
            companyPolicy.Endorsement = companyEndorsement;
            companyPolicy.Endorsement.CurrentFrom = Convert.ToDateTime(changeCoinsuranceViewModel.ChangeCoinsuranceFrom);
            companyPolicy.CoInsuranceCompanies = changeCoinsuranceViewModel.companyIssuanceCoInsuranceCompanies;
            companyPolicy.Agencies = changeCoinsuranceViewModel.Agencies;
            companyPolicy.Id = changeCoinsuranceViewModel.Id;
            companyPolicy.UserId = SessionHelper.GetUserId();
            companyPolicy.Endorsement.UserId = companyPolicy.UserId;
            companyPolicy.Text = new CompanyText()
            {
                TextBody = changeCoinsuranceViewModel.Text
            };
            return companyPolicy;
        }

        public static CompanyChangeConsolidation CreateCompanyEndorsement(ChangeConsolidationViewModel ChangeConsolidationViewModel)
        {
            var imapper = CreateMappCompanyEndorsementChangeConsolidation();
            var companyEndorsement = imapper.Map<ChangeConsolidationViewModel, CompanyEndorsement>(ChangeConsolidationViewModel);
            CompanyChangeConsolidation companyChangeConsolidation = new CompanyChangeConsolidation();
            companyChangeConsolidation.CurrentFrom = Convert.ToDateTime(ChangeConsolidationViewModel.CurrentFrom);
            companyChangeConsolidation.CurrentTo = Convert.ToDateTime(ChangeConsolidationViewModel.CurrentTo);
            companyChangeConsolidation.Endorsement = companyEndorsement;
            companyChangeConsolidation.Endorsement.CurrentFrom = Convert.ToDateTime(ChangeConsolidationViewModel.ChangeConsolidationFrom);
            companyChangeConsolidation.Id = ChangeConsolidationViewModel.Id;
            companyChangeConsolidation.UserId = SessionHelper.GetUserId();
            companyChangeConsolidation.Endorsement.UserId = companyChangeConsolidation.UserId;
            companyChangeConsolidation.Text = new CompanyText()
            {
                TextBody = ChangeConsolidationViewModel.Text
            };
            companyChangeConsolidation.companyContract = ChangeConsolidationViewModel.companyContract;
            companyChangeConsolidation.companyContract.CustomerType = Sistran.Core.Services.UtilitiesServices.Enums.CustomerType.Individual;
            companyChangeConsolidation.companyContract.IndividualType = Core.Services.UtilitiesServices.Enums.IndividualType.Person;
            companyChangeConsolidation.companyContract.Name = ChangeConsolidationViewModel.companyContract.Name;
            companyChangeConsolidation.companyContract.IndividualId = ChangeConsolidationViewModel.companyContract.IndividualId;
            return companyChangeConsolidation;

        }

        internal static CompanyChangePolicyHolder CreateCompanyEndorsement(ChangePolicyHolderViewModel changePolicyHolderViewModel)
        {
            var imapper = CreateMappCompanyEndorsementChangePolicyHolder();
            var companyEndorsement = imapper.Map<ChangePolicyHolderViewModel, CompanyEndorsement>(changePolicyHolderViewModel);
            CompanyChangePolicyHolder companyChangePolicyHolder = new CompanyChangePolicyHolder();
            companyChangePolicyHolder.CurrentFrom = Convert.ToDateTime(changePolicyHolderViewModel.CurrentFrom);
            companyChangePolicyHolder.CurrentTo = Convert.ToDateTime(changePolicyHolderViewModel.CurrentTo);
            companyChangePolicyHolder.Endorsement = companyEndorsement;
            companyChangePolicyHolder.Endorsement.CurrentFrom = Convert.ToDateTime(changePolicyHolderViewModel.ChangePolicyHolderFrom);
            companyChangePolicyHolder.Id = changePolicyHolderViewModel.Id;
            companyChangePolicyHolder.UserId = SessionHelper.GetUserId();
            companyChangePolicyHolder.Endorsement.UserId = companyChangePolicyHolder.UserId;
            companyChangePolicyHolder.Text = new CompanyText()
            {
                TextBody = changePolicyHolderViewModel.Text
            };
            companyChangePolicyHolder.holder = changePolicyHolderViewModel.holder;
            companyChangePolicyHolder.companyContract = changePolicyHolderViewModel.companyContract;
            if (companyChangePolicyHolder.companyContract != null)
            {
                companyChangePolicyHolder.companyContract = changePolicyHolderViewModel.companyContract;
                companyChangePolicyHolder.companyContract.CustomerType = Sistran.Core.Services.UtilitiesServices.Enums.CustomerType.Individual;
                companyChangePolicyHolder.companyContract.IndividualType = Core.Services.UtilitiesServices.Enums.IndividualType.Person;
                companyChangePolicyHolder.companyContract.Name = changePolicyHolderViewModel.companyContract.Name;
                companyChangePolicyHolder.companyContract.IndividualId = changePolicyHolderViewModel.companyContract.IndividualId;
            }
            return companyChangePolicyHolder;
        }

        public static CompanyPolicy CreateCompanyEndorsement(ChangeTermViewModel changeTermModel)
        {
            var immaper = CreateMappCompanyEndorsementChangeTerm();
            var companyEndorsement = immaper.Map<ChangeTermViewModel, CompanyEndorsement>(changeTermModel);
            CompanyPolicy companyPolicy = new CompanyPolicy();
            companyPolicy.CurrentFrom = Convert.ToDateTime(changeTermModel.CurrentFrom);
            companyPolicy.CurrentTo = Convert.ToDateTime(changeTermModel.CurrentTo);
            if (changeTermModel.Text != null)
            {
                companyPolicy.Text = new CompanyText()
                {
                    TextBody = Convert.ToString(changeTermModel.Text.ToString())
                };
            }
            companyPolicy.Endorsement = companyEndorsement;
            companyPolicy.UserId = SessionHelper.GetUserId();
            companyPolicy.Endorsement.UserId = SessionHelper.GetUserId();
            return companyPolicy;
        }


        /// <summary>
        /// Creates the company policy by Renewal.
        /// </summary>
        /// <param name="RenewalViewModel">renewalViewModel</param>
        /// <returns></returns>
        public static CompanyPolicy CreateCompanyPolicyByRenewal(RenewalViewModel renewalViewModel)
        {
            var imapper = CreateMappCompanyEndorsementRenewal();
            var p = imapper.Map<RenewalViewModel, CompanyEndorsement>(renewalViewModel);

            CompanyPolicy companyPolicy = new CompanyPolicy();
            companyPolicy.Endorsement = p;
            companyPolicy.CurrentFrom = Convert.ToDateTime(renewalViewModel.ModifyFrom);
            companyPolicy.CurrentTo = Convert.ToDateTime(renewalViewModel.ModifyTo);
            companyPolicy.Endorsement.IsUnderIdenticalConditions = renewalViewModel.IsUnderIdenticalConditions;
            companyPolicy.Endorsement.CurrentFrom = Convert.ToDateTime(renewalViewModel.ModifyFrom);
            companyPolicy.Endorsement.CurrentTo = Convert.ToDateTime(renewalViewModel.ModifyTo);
            companyPolicy.IssueDate = renewalViewModel.IssueDate.ToDate(DateHelper.FormatFullDate);
            companyPolicy.UserId = SessionHelper.GetUserId();
            return companyPolicy;
        }

        /// <summary>
        /// Creates the EventAuthorization by CompanyPolicy
        /// </summary>
        /// <param name="policy"></param>
        /// <returns></returns>
        public static EventAuthorization CreateCompanyEventAuthorizationEndoso(CompanyPolicy policy)
        {
            EventAuthorization Event = new EventAuthorization();
            try
            {
                Event.OPERATION1_ID = policy.Endorsement.TicketNumber.ToString();
                Event.OPERATION2_ID = policy.Endorsement.Id.ToString();
                Event.AUTHO_USER_ID = SessionHelper.GetUserId();
                Event.EVENT_ID = (int)EventTypes.Endorsement;
            }
            catch (Exception ex)
            {

            }
            return Event;
        }

        #endregion
        #region automapper
        public static IMapper CreateMappCompanyEndorsementreversion()
        {
            var config = MapperCache.GetMapper<ReversionViewModel, CompanyEndorsement>(cfg =>
            {
                cfg.CreateMap<ReversionViewModel, CompanyEndorsement>()
            .ForMember(dest => dest.Text, y => y.ResolveUsing<TextReversionResolver>())
            .ForMember(dest => dest.CurrentFrom, opt => opt.MapFrom(src => Convert.ToDateTime(src.CurrentFrom)))
            .ForMember(dest => dest.CurrentTo, opt => opt.MapFrom(src => Convert.ToDateTime(src.CurrentTo)))
            .ForMember(dest => dest.EndorsementType, opt => opt.MapFrom(src => (EndorsementType?)src.EndorsementType))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.EndorsementId));
            });
            return config;
        }
        public static IMapper CreateMappCompanyEndorsementExtension()
        {
            var config = MapperCache.GetMapper<ExtensionViewModel, CompanyEndorsement>(cfg =>
            {
                cfg.CreateMap<ExtensionViewModel, CompanyEndorsement>()
            .ForMember(dest => dest.Text, y => y.ResolveUsing<TextValueExtensionResolver>())
            .ForMember(dest => dest.CurrentFrom, opt => opt.MapFrom(src => Convert.ToDateTime(src.CurrentFrom)))
            .ForMember(dest => dest.CurrentTo, opt => opt.MapFrom(src => Convert.ToDateTime(src.CurrentTo)))
            .ForMember(dest => dest.EndorsementType, opt => opt.MapFrom(src => (EndorsementType?)src.EndorsementType))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.EndorsementId));
            });
            return config;

        }
        public static IMapper CreateMappCompanyEndorsementChangeTerm()
        {
            var config = MapperCache.GetMapper<ChangeTermViewModel, CompanyEndorsement>(cfg =>
            {
                cfg.CreateMap<ChangeTermViewModel, CompanyEndorsement>()
             .ForMember(dest => dest.Text, y => y.ResolveUsing<TextChangeTermResolver>())
             .ForMember(dest => dest.CurrentFrom, opt => opt.MapFrom(src => Convert.ToDateTime(src.EndorsementFrom)))
             .ForMember(dest => dest.CurrentTo, opt => opt.MapFrom(src => Convert.ToDateTime(src.EndorsementTo)))
             .ForMember(dest => dest.EndorsementType, opt => opt.MapFrom(src => (EndorsementType?)src.EndorsementType))
             .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.EndorsementId))
             .ForMember(dest => dest.EndorsementDays, opt => opt.MapFrom(src => src.Days));
            });
            return config;

        }
        public static IMapper CreateMapCompanyEndorsement()
        {
            var config = MapperCache.GetMapper<RM.Endorsement, CompanyEndorsement>(cfg =>
            {
                cfg.CreateMap<RM.Endorsement, CompanyEndorsement>();
                cfg.CreateMap<RM.Text, CompanyText>();
            });
            return config;

        }
        public static IMapper CreateMappCompanyEndorsementModification()
        {
            var config = MapperCache.GetMapper<ModificationViewModel, CompanyEndorsement>(cfg =>
            {
                cfg.CreateMap<ModificationViewModel, CompanyEndorsement>()
            .ForMember(dest => dest.Text, opt => opt.ResolveUsing<TextValueResolver>())
            .ForMember(dest => dest.CurrentFrom, opt => opt.MapFrom(src => Convert.ToDateTime(src.CurrentFrom)))
            .ForMember(dest => dest.CurrentTo, opt => opt.MapFrom(src => Convert.ToDateTime(src.CurrentTo)))
            .ForMember(dest => dest.EndorsementType, opt => opt.MapFrom(src => (EndorsementType?)src.EndorsementType))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.EndorsementId));
            });
            return config;
        }
        public static IMapper CreateMappCompanyEndorsementRenewal()
        {
            var config = MapperCache.GetMapper<RenewalViewModel, CompanyEndorsement>(cfg =>
            {
                cfg.CreateMap<RenewalViewModel, CompanyEndorsement>()
            .ForMember(dest => dest.Text, y => y.ResolveUsing<TextValueRenewalResolver>())
            .ForMember(dest => dest.CurrentFrom, opt => opt.MapFrom(src => Convert.ToDateTime(src.ModifyFrom)))
            .ForMember(dest => dest.CurrentTo, opt => opt.MapFrom(src => Convert.ToDateTime(src.ModifyTo)))
            .ForMember(dest => dest.EndorsementType, opt => opt.MapFrom(src => (EndorsementType?)src.EndorsementType))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.EndorsementId));
            });
            return config;
        }
        public static IMapper CreateMappCompanyEndorsementChangeAgent()
        {
            var config = MapperCache.GetMapper<ChangeAgentViewModel, CompanyEndorsement>(cfg =>
            {
                cfg.CreateMap<ChangeAgentViewModel, CompanyEndorsement>()
               .ForMember(dest => dest.Text, y => y.ResolveUsing<TextChangeAgentResolver>())
               .ForMember(dest => dest.CurrentFrom, opt => opt.MapFrom(src => Convert.ToDateTime(src.EndorsementFrom)))
               .ForMember(dest => dest.CurrentTo, opt => opt.MapFrom(src => Convert.ToDateTime(src.EndorsementTo)))
               .ForMember(dest => dest.EndorsementType, opt => opt.MapFrom(src => (EndorsementType?)src.EndorsementType))
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.EndorsementId))
               .ForMember(dest => dest.EndorsementDays, opt => opt.MapFrom(src => src.Days));
            });
            return config;

        }

        public static IMapper CreateMappCompanyEndorsementChangeCoinsurance()
        {
            var config = MapperCache.GetMapper<ChangeCoinsuranceViewModel, CompanyEndorsement>(cfg =>
            {
                cfg.CreateMap<ChangeCoinsuranceViewModel, CompanyEndorsement>()
               .ForMember(dest => dest.Text, y => y.ResolveUsing<TextChangeCoinsuranceResolver>())
               .ForMember(dest => dest.CurrentFrom, opt => opt.MapFrom(src => Convert.ToDateTime(src.EndorsementFrom)))
               .ForMember(dest => dest.CurrentTo, opt => opt.MapFrom(src => Convert.ToDateTime(src.EndorsementTo)))
               .ForMember(dest => dest.EndorsementType, opt => opt.MapFrom(src => (EndorsementType?)src.EndorsementType))
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.EndorsementId))
               .ForMember(dest => dest.EndorsementDays, opt => opt.MapFrom(src => src.Days));
            });
            return config;

        }

        public static IMapper CreateMappCompanyEndorsementChangePolicyHolder()
        {
            var config = MapperCache.GetMapper<ChangePolicyHolderViewModel, CompanyEndorsement>(cfg =>
            {
                cfg.CreateMap<ChangePolicyHolderViewModel, CompanyEndorsement>()
               .ForMember(dest => dest.Text, y => y.ResolveUsing<TextChangePolicyHolderResolver>())
               .ForMember(dest => dest.CurrentFrom, opt => opt.MapFrom(src => Convert.ToDateTime(src.EndorsementFrom)))
               .ForMember(dest => dest.CurrentTo, opt => opt.MapFrom(src => Convert.ToDateTime(src.EndorsementTo)))
               .ForMember(dest => dest.EndorsementType, opt => opt.MapFrom(src => (EndorsementType?)src.EndorsementType))
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.EndorsementId))
               .ForMember(dest => dest.EndorsementDays, opt => opt.MapFrom(src => src.Days));
            });
            return config;

        }

        public static IMapper CreateMappCompanyEndorsementChangeConsolidation()
        {
            var config = MapperCache.GetMapper<ChangeConsolidationViewModel, CompanyEndorsement>(cfg =>
            {
                cfg.CreateMap<ChangeConsolidationViewModel, CompanyEndorsement>()
               .ForMember(dest => dest.Text, y => y.ResolveUsing<TextChangeConsolidationResolver>())
               .ForMember(dest => dest.CurrentFrom, opt => opt.MapFrom(src => Convert.ToDateTime(src.EndorsementFrom)))
               .ForMember(dest => dest.CurrentTo, opt => opt.MapFrom(src => Convert.ToDateTime(src.EndorsementTo)))
               .ForMember(dest => dest.EndorsementType, opt => opt.MapFrom(src => (EndorsementType.ChangeCoinsuranceEndorsement)))
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.EndorsementId))
               .ForMember(dest => dest.EndorsementDays, opt => opt.MapFrom(src => src.Days));
            });
            return config;

        }


        public static IMapper CreateMappCompanyEndorsementCancellation()
        {
            var config = MapperCache.GetMapper<CancellationViewModel, CompanyEndorsement>(cfg =>
            {
                cfg.CreateMap<CancellationViewModel, CompanyEndorsement>()
            .ForMember(dest => dest.Text, y => y.ResolveUsing<TextCancelationResolver>())
            .ForMember(dest => dest.CurrentFrom, opt => opt.MapFrom(src => Convert.ToDateTime(src.CurrentFrom)))
            .ForMember(dest => dest.CurrentTo, opt => opt.MapFrom(src => Convert.ToDateTime(src.CurrentTo)))
            .ForMember(dest => dest.CancelationCurrentFrom, opt => opt.MapFrom(src => src.EndorsementFrom))
            .ForMember(dest => dest.CancelationCurrentTo, opt => opt.MapFrom(src => src.EndorsementTo))
            .ForMember(dest => dest.EndorsementType, opt => opt.MapFrom(src => (EndorsementType?)src.EndorsementType))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.EndorsementId));
                //  .ForMember(dest => dest.CancellationTypeId, opt => opt.MapFrom(src => src.));
            });
            return config;
        }

        public static IMapper CreateMappCompanyEndorsementPolicyDescriptionBranch()
        {
            var config = MapperCache.GetMapper<Branch, CompanyBranch>(cfg =>
            {
                cfg.CreateMap<Branch, CompanyBranch>();
                cfg.CreateMap<SalePoint, CompanySalesPoint>();

            });
            return config;
        }
        public static IMapper CreateMappCompanyEndorsementPolicyDescriptionPrefix()
        {
            var config = MapperCache.GetMapper<Prefix, CompanyPrefix>(cfg =>
            {
                cfg.CreateMap<Prefix, CompanyPrefix>();
                cfg.CreateMap<PrefixType, CompanyPrefixType>();
                cfg.CreateMap<LineBusiness, CompanyLineBusiness>();

            });
            return config;
        }

        #region trasportes
        public static IMapper CreateMapBranch()
        {
            var config = MapperCache.GetMapper<Application.CommonService.Models.Branch, CompanyBranch>(cfg =>
            {

                cfg.CreateMap<Application.CommonService.Models.Branch, CompanyBranch>();
                cfg.CreateMap<Application.CommonService.Models.SalePoint, CompanySalesPoint>();
            });
            return config;
        }
        public static IMapper CreateMapPrefix()
        {
            var config = MapperCache.GetMapper<Application.CommonService.Models.Prefix, CompanyPrefix>(cfg =>
            {

                cfg.CreateMap<Application.CommonService.Models.Prefix, CompanyPrefix>();
                cfg.CreateMap<PrefixType, CompanyPrefixType>();
                cfg.CreateMap<Application.CommonService.Models.LineBusiness, CompanyLineBusiness>();
            });
            return config;
        }


        #endregion
        #endregion

        #region Adjustment Endorsment
        public static EndorsementDTO CreateEndorsementDTO(Endorsement endorsement)
        {

            if (endorsement == null)
            {
                return null;
            }

            EndorsementDTO endorsementDTO = new EndorsementDTO()
            {
                CurrentFrom = endorsement.CurrentFrom,
                CurrentTo = endorsement.CurrentTo,
                EndorsementType = endorsement.EndorsementType,
                IdEndorsement = endorsement.Id,
                IsCurrent = endorsement.IsCurrent,
                Number = endorsement.Number,
                PolicyNumber = endorsement.PolicyId,
                TemporalId = endorsement.TemporalId
            };

            return endorsementDTO;

        }


        #endregion



    }

    /// <summary>
    /// Textos
    /// </summary>
    /// <seealso cref="AutoMapper.ValueResolver{Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models.ModificationViewModel, Sistran.Company.Application.UnderwritingServices.Models.CompanyText}" />
    public class TextValueResolver : IValueResolver<ModificationViewModel, CompanyEndorsement, CRM.CompanyText>

    {
        public CRM.CompanyText Resolve(ModificationViewModel source, CompanyEndorsement destination, CRM.CompanyText member, ResolutionContext context)
        {
            return new CRM.CompanyText
            {
                TextBody = source.Text,
                Observations = source.Observations
            };
        }

        //protected override CRM.CompanyText ResolveCore(ModificationViewModel source)
        //{

        //    return new CRM.CompanyText
        //    {
        //        TextBody = source.Text,
        //        Observations = source.Observations
        //    };
        //}
    }
    /// <summary>
    /// Textos Endorsement
    /// </summary>
    /// <seealso cref="AutoMapper.ValueResolver{Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models.ExtensionViewModel, Sistran.Company.Application.UnderwritingServices.Models.CompanyText}" />
    public class TextValueExtensionResolver : IValueResolver<ExtensionViewModel, CompanyEndorsement, CRM.CompanyText>

    {
        public CRM.CompanyText Resolve(ExtensionViewModel source, CompanyEndorsement destination, CRM.CompanyText member, ResolutionContext context)
        {
            return new CRM.CompanyText
            {
                TextBody = source.Text,
                Observations = source.Observations
            };
        }
        //protected override CRM.CompanyText ResolveCore(ExtensionViewModel source)
        //{

        //    return new CRM.CompanyText
        //    {
        //        TextBody = source.Text,
        //        Observations = source.Observations
        //    };
        //}
    }

    public class TextCancelationResolver : IValueResolver<CancellationViewModel, CompanyEndorsement, CRM.CompanyText>

    {
        public CRM.CompanyText Resolve(CancellationViewModel source, CompanyEndorsement destination, CRM.CompanyText member, ResolutionContext context)
        {
            return new CRM.CompanyText
            {
                TextBody = source.Text,
                Observations = source.Observations
            };
        }
        //protected override CRM.CompanyText ResolveCore(CancellationViewModel source)
        //{

        //    return new CRM.CompanyText
        //    {
        //        TextBody = source.Text,
        //        Observations = source.Observations
        //    };
        //}
    }


    public class TextReversionResolver : IValueResolver<ReversionViewModel, CompanyEndorsement, CRM.CompanyText>

    {
        public CRM.CompanyText Resolve(ReversionViewModel source, CompanyEndorsement destination, CRM.CompanyText member, ResolutionContext context)
        {
            return new CRM.CompanyText
            {
                TextBody = source.Text,
                Observations = source.Observations
            };
        }
        //protected override CRM.CompanyText ResolveCore(ReversionViewModel source)
        //{

        //    return new CRM.CompanyText
        //    {
        //        TextBody = source.Text,
        //        Observations = source.Observations
        //    };
        //}
    }

    /// <summary>
    /// Textos Renewal
    /// </summary>
    /// <seealso cref="AutoMapper.ValueResolver{Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models.ExtensionViewModel, Sistran.Company.Application.UnderwritingServices.Models.CompanyText}" />

    public class TextValueRenewalResolver : IValueResolver<RenewalViewModel, CompanyEndorsement, CRM.CompanyText>

    {
        public CRM.CompanyText Resolve(RenewalViewModel source, CompanyEndorsement destination, CRM.CompanyText member, ResolutionContext context)
        {
            return new CRM.CompanyText
            {
                TextBody = source.Text,
                Observations = source.Observations
            };
        }
        //protected override CRM.CompanyText ResolveCore(RenewalViewModel source)
        //{

        //    return new CRM.CompanyText
        //    {
        //        TextBody = source.Text,
        //        Observations = source.Observations
        //    };
        //}
    }


    public class TextChangeAgentResolver : IValueResolver<ChangeAgentViewModel, CompanyEndorsement, CRM.CompanyText>

    {
        public CRM.CompanyText Resolve(ChangeAgentViewModel source, CompanyEndorsement destination, CRM.CompanyText member, ResolutionContext context)
        {
            return new CRM.CompanyText
            {
                TextBody = source.Text,
                Observations = source.Observations
            };
        }
        //protected override CRM.CompanyText ResolveCore(ChangeAgentViewModel source)
        //{

        //    return new CRM.CompanyText
        //    {
        //        TextBody = source.Text,
        //        Observations = source.Observations
        //    };
        //}
    }

    public class TextChangeCoinsuranceResolver : IValueResolver<ChangeCoinsuranceViewModel, CompanyEndorsement, CRM.CompanyText>

    {
        public CRM.CompanyText Resolve(ChangeCoinsuranceViewModel source, CompanyEndorsement destination, CRM.CompanyText member, ResolutionContext context)
        {
            return new CRM.CompanyText
            {
                TextBody = source.Text,
                Observations = source.Observations
            };
        }
        //protected override CRM.CompanyText ResolveCore(ChangeAgentViewModel source)
        //{

        //    return new CRM.CompanyText
        //    {
        //        TextBody = source.Text,
        //        Observations = source.Observations
        //    };
        //}
    }
    public class TextChangeTermResolver : IValueResolver<ChangeTermViewModel, CompanyEndorsement, CRM.CompanyText>

    {
        public CRM.CompanyText Resolve(ChangeTermViewModel source, CompanyEndorsement destination, CRM.CompanyText member, ResolutionContext context)
        {
            return new CRM.CompanyText
            {
                TextBody = source.Text,
                Observations = source.Observations
            };
        }
        //protected override CRM.CompanyText ResolveCore(ChangeTermViewModel source)
        //{

        //    return new CRM.CompanyText
        //    {
        //        TextBody = source.Text,
        //        Observations = source.Observations
        //    };
        //}
    }

    public class TextChangeConsolidationResolver : IValueResolver<ChangeConsolidationViewModel, CompanyEndorsement, CRM.CompanyText>

    {
        public CRM.CompanyText Resolve(ChangeConsolidationViewModel source, CompanyEndorsement destination, CRM.CompanyText member, ResolutionContext context)
        {
            return new CRM.CompanyText
            {
                TextBody = source.Text,
                Observations = source.Observations
            };
        }
    }

    public class TextChangePolicyHolderResolver : IValueResolver<ChangePolicyHolderViewModel, CompanyEndorsement, CRM.CompanyText>

    {
        public CRM.CompanyText Resolve(ChangePolicyHolderViewModel source, CompanyEndorsement destination, CRM.CompanyText member, ResolutionContext context)
        {
            return new CRM.CompanyText
            {
                TextBody = source.Text,
                Observations = source.Observations
            };
        }
    }

    /// <summary>
    /// Fecha
    /// </summary>
    /// <seealso cref="AutoMapper.ITypeConverter{System.String, System.DateTime}" />
    public class StringToDateTimeConverter : ITypeConverter<string, DateTime>
    {
        public DateTime Convert(string source, DateTime destination, ResolutionContext context)
        {
            object objDateTime = source;
            DateTime dateTime;

            if (objDateTime == null)
            {
                return default(DateTime);
            }

            if (DateTime.TryParse(objDateTime.ToString(), out dateTime))
            {
                return dateTime;
            }

            return default(DateTime);
        }
    }

}