
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using System.Collections.Generic;
using System.Linq;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.UniquePersonParamService.Models;
using Sistran.Core.Application.ModelServices.Models.AuthorizationPolicies;
using Sistran.Core.Application.ModelServices.Enums;
using Sistran.Core.Framework.UIF.Web.Areas.ParamAuthorizationPolicies.Models;
using Sistran.Company.Application.ModelServices.Models.AuthorizationPolicies;
using Sistran.Company.Application.ModelServices.Models.Param;
using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;

namespace Sistran.Core.Framework.UIF.Web.Areas.ParamAuthorizationPolicies.Models
{
    public class ModelAssembler
    {
        #region SubLinesBusiness
        public static List<SubLineBusiness> CreateSubLineBusiness(List<TechnicalSubBranchViewModel> sublinebusinessView)
        {
            List<SubLineBusiness> subLineBusiness = new List<SubLineBusiness>();
            foreach (TechnicalSubBranchViewModel sublineBusiness in sublinebusinessView)
            {
                SubLineBusiness sublinebusines = new SubLineBusiness();
                sublinebusines.Description = sublineBusiness.Description;
                sublinebusines.Id = sublineBusiness.Id;
                sublinebusines.SmallDescription = sublineBusiness.SmallDescription;
                sublinebusines.LineBusinessId = sublineBusiness.LineBusinessId;
                sublinebusines.Status = sublineBusiness.Status;
                subLineBusiness.Add(sublinebusines);
            }
            return subLineBusiness;
        }

        public static List<TechnicalSubBranchViewModel> CreateSubLineBusiness(List<SubLineBusiness> subLineBusiness)
        {
            return subLineBusiness.Select(p => new TechnicalSubBranchViewModel()
            {
                Description = p.Description,
                SmallDescription = p.SmallDescription,
                LineBusinessId = p.LineBusinessId,
                Id = p.Id,
                Status = p.Status
            }).ToList();
        }
        #endregion

        #region Amparos
        public static TechnicalBranchProtectionViewModel CreateTechicalBranchProtection(PerilLineBusiness perilLineBusines)
        {
            TechnicalBranchProtectionViewModel returnViewModel = new TechnicalBranchProtectionViewModel()
            {
                Id = perilLineBusines.IdLineBusiness
            };
            if (perilLineBusines.PerilAssign != null)
                returnViewModel.PerilAssign = perilLineBusines.PerilAssign.Select(p => CreateProtection(p)).ToList();
            if (perilLineBusines.PerilNotAssign != null)
                returnViewModel.PerilNotAssign = perilLineBusines.PerilNotAssign.Select(p => CreateProtection(p)).ToList();
            return returnViewModel;
        }

        public static Peril CreateProtection(ProtectionViewModel protectionModel)
        {
            return new Peril()
            {
                Description = protectionModel.DescriptionLong,
                SmallDescription = protectionModel.DescriptionShort,                
                Id = protectionModel.Id
            };
        }

        public static ProtectionViewModel CreateProtection(Peril protectionModel)
        {
            return new ProtectionViewModel()
            {
                DescriptionLong = protectionModel.Description,
                DescriptionShort = protectionModel.SmallDescription,
                Id = protectionModel.Id
            };
        }

        public static List<ProtectionViewModel> CreateProtections(List<Peril> protections)
        {
            return protections.Select(p => new ProtectionViewModel()
            {
                DescriptionLong = p.Description,
                DescriptionShort = p.SmallDescription,
                Id = p.Id
            }).ToList();
        }
        public static List<Peril> CreateProtections(List<ProtectionViewModel> protections)
        {
            if (protections == null)
                return null;
            return protections.Select(p => new Peril()
            {
                Description = p.DescriptionLong,
                SmallDescription = p.DescriptionShort,
                Id = p.Id
            }).ToList();
        }
        #endregion

        #region InsurencesObjects

        public static InsurencesObjectsViewModel CreateInsurencesObjects(CompanyInsuredObject insurencesObjects)
        {
            return new InsurencesObjectsViewModel()
            {
                Id = insurencesObjects.Id,
                Description = insurencesObjects.Description,
                SmallDescription = insurencesObjects.SmallDescription,
                IsDeraclarative = insurencesObjects.IsDeclarative
            };
        }

        public static List<CompanyInsuredObject> CreateInsurencesObjects(List<InsurencesObjectsViewModel> insurencesObjects)
        {
            List<CompanyInsuredObject> lstObjects = new List<CompanyInsuredObject>();
            if (insurencesObjects != null)
            {
                foreach (var insurancesObject in insurencesObjects)
                {
                    lstObjects.Add(new CompanyInsuredObject()
                    {
                        Id = insurancesObject.Id,
                        Description = insurancesObject.Description,
                        IsDeclarative = insurancesObject.IsDeraclarative,
                        SmallDescription = insurancesObject.SmallDescription
                    });
                }
            }

            return lstObjects;
        }

        #endregion

        #region Branchs

        public static List<BranchViewModel> GetBranchs(List<Branch> Branchs)
        {
            List<BranchViewModel> branchModelView = new List<BranchViewModel>();
            foreach (Branch model in Branchs)
            {
                BranchViewModel BranchModel = new BranchViewModel();
                BranchModel.LongDescription = model.Description;
                BranchModel.ShortDescription = model.SmallDescription;
                BranchModel.Id = model.Id;

                branchModelView.Add(BranchModel);
            }

            return branchModelView;
        }

        public static List<Branch> CreateBranchs(List<BranchViewModel> Branchs)
        {
            if (Branchs == null)
                return null;
            List<Branch> branchModelView = new List<Branch>();
            foreach (BranchViewModel model in Branchs)
            {
                Branch BranchModel = new Branch();
                BranchModel.Description = model.LongDescription;
                BranchModel.SmallDescription = model.ShortDescription;
                BranchModel.Id = model.Id;

                branchModelView.Add(BranchModel);
            }

            return branchModelView;
        }

        #endregion

        #region Prefix
        public static List<Prefix> CreatePrefix(List<BusinessBranchViewModel> prefixView)
        {
            List<Prefix> prefrixes = new List<Prefix>();
            foreach (BusinessBranchViewModel prefixItem in prefixView)
            {
                Prefix prefix = new Prefix();
                prefix.Description = prefixItem.Description;
                prefix.Id = prefixItem.IdPrefix;
                prefix.SmallDescription = prefixItem.SmallDescription;
                prefix.PrefixType = new Application.CommonService.Models.PrefixType { Id = prefixItem.PrefixTypeCode };
                prefix.TinyDescription = prefixItem.TinyDescription;
                prefix.LineBusinessId = prefixItem.LineBusinessId;
             //   prefix.PrefixLineBusiness = prefixItem.PrefixLineBusiness;
                prefrixes.Add(prefix);
            }
            return prefrixes;
        }

        public static List<BusinessBranchViewModel> CreatePrefixes(List<Prefix> prefix)
        {
            return prefix.Select(p => new BusinessBranchViewModel()
            {
                Description = p.Description,
                SmallDescription = p.SmallDescription,
                LineBusinessId = p.LineBusinessId,
                IdPrefix = p.Id,
                TinyDescription = p.TinyDescription,
                PrefixTypeCode = p.PrefixType.Id
            }).ToList();
        }
        #endregion
        #region Coverages

        //public static CoverageParametrization CreateCoverage(CoverageViewModel model)
        //{
        //    return new CoverageParametrization
        //    {
        //        Id = model.Id,
        //        Description = model.Description,
        //        IsPrimary = model.IsPrimary,
        //        LineBusinessId = model.LineBusinessId,
        //        SubLineBusinessId = model.SubLineBusinessId,
        //        PerilId = model.PerilId,
        //        InsuredObjectId = model.InsuredObjectId,
        //        CompositionTypeId = model.CompositionTypeId,
        //        Clauses = CreatePartialCoverageParametrization(model.Clauses),
        //        Deductibles = CreatePartialCoverageParametrization(model.Deductibles),
        //        DetailTypes = CreatePartialCoverageParametrization(model.DetailTypes)
        //    };
        //}

        //public static PartialsCoverageParametrization CreatePartialCoverageParametrization(PartialsViewModel partialViewModel)
        //{
        //    if (partialViewModel == null)
        //    {
        //        partialViewModel = new PartialsViewModel();
        //    }
        //    return new PartialsCoverageParametrization
        //    {
        //        Created = partialViewModel.Created.Select(CratePartialInformation).ToList(),
        //        Updated = partialViewModel.Updated.Select(CratePartialInformation).ToList(),
        //        Deleted = partialViewModel.Deleted.Select(CratePartialInformation).ToList()
        //    };
        //}

        //public static PartialInformation CratePartialInformation(PartialsInformationViewModel partialInformationViewModel)
        //{
        //    return new PartialInformation
        //    {
        //        Id = partialInformationViewModel.Id,
        //        Required = partialInformationViewModel.Required
        //    };
        //}

        #endregion

        #region Expenses

        public static List<ExpenseViewModel> GetExpenses(List<Expense> listExpenses)
        {
            if (listExpenses != null)
            {
                List<ExpenseViewModel> ExpensesViewModel = new List<ExpenseViewModel>();
                foreach (Expense model in listExpenses)
                {
                    ExpenseViewModel ExpensesModel = new ExpenseViewModel();

                    ExpensesModel.id = model.id;
                    ExpensesModel.Description = model.Description;
                    ExpensesModel.Abbreviation = model.Abbreviation;
                    ExpensesModel.InitiallyIncluded = model.InitiallyIncluded;
                    ExpensesModel.Mandatory = model.Mandatory;
                    ExpensesModel.Rate = model.Rate;
                    ExpensesModel.RateType = model.RateType;
                    ExpensesModel.RuleSet = model.RuleSet;
                    ExpensesModel.RuleSetName = model.RuleSetName;
                    ExpensesModel.ComponentClass = model.ComponentClass;
                    ExpensesModel.ComponentType = model.ComponentType;

                    ExpensesViewModel.Add(ExpensesModel);
                }
                return ExpensesViewModel;
            }
            return null;
        }

        public static List<Expense> CreateExpenses(List<ExpenseViewModel> listExpense)
        {
            if (listExpense != null)
            {
                List<Expense> Expenses = new List<Expense>();
                foreach (ExpenseViewModel model in listExpense)
                {
                    Expense ExpenseModel = new Expense();
                    
                    ExpenseModel.Description = model.Description;
                    ExpenseModel.Abbreviation = model.Abbreviation;
                    ExpenseModel.id = model.id;
                    ExpenseModel.InitiallyIncluded = model.InitiallyIncluded;
                    ExpenseModel.Mandatory = model.Mandatory;
                    ExpenseModel.Rate = model.Rate;
                    ExpenseModel.RateType = model.RateType;
                    ExpenseModel.RuleSet = model.RuleSet;
                    ExpenseModel.ComponentClass = model.ComponentClass;
                    ExpenseModel.ComponentType = model.ComponentType;

                    Expenses.Add(ExpenseModel);
                }
                return Expenses;
            }
            return null;
        }
        #endregion

        #region ALLIANCES
        /// <summary>
        /// Tranforma los aliados.
        /// </summary>
        /// <param name="listAlliance">Lista de aliados (Modelos de negocio)</param>
        /// <returns>Lista de aliados (Modelos de vista)</returns>
        public static List<AllianceViewModel> CreateAlliances(List<Alliance> listAlliance)
        {
            return listAlliance.Select(l => new AllianceViewModel()
            {
                Description = l.Description,
                AlliedCode = l.AllianceId,
                IsFine = l.IsFine,
                IsScore = l.IsScore
            }).ToList();
        }

        /// <summary>
        /// Tranforma los aliados.
        /// </summary>
        /// <param name="listAlliance">Lista de aliados (Modelos de negocio)</param>
        /// <returns>Lista de aliados (Modelos de vista)</returns>
        public static List<Alliance> CreateAlliances(List<AllianceViewModel> listAlliance)
        {
            return listAlliance.Select(l => new Alliance()
            {
                Description = l.Description,
                AllianceId = l.AlliedCode,
                IsFine = l.IsFine,
                IsScore = l.IsScore,
                Status = l.Status
            }).ToList();
        }

        /// <summary>
        /// Transformador de negocio a modelo de vista
        /// </summary>
        /// <param name="branchAlliance">Lista de sucursales</param>
        /// <returns>Lista de sucursales para vista</returns>
        public static List<BranchAllianceViewModel> CreateBrachAlliances(List<BranchAlliance> branchAlliance)
        {
            return branchAlliance.Select(l => new BranchAllianceViewModel()
            {
                BranchDescription = l.BranchDescription,
                AllianceId = l.AllianceId,
                AllianceName = l.AllianceDescription,
                BranchId = l.BranchId,
                CityCD = l.CityCD,
                CityName = l.CityName,
                StateCD = l.StateCD,
                StateName = l.StateName,
                CountryCD = l.CountryCD,
                CountryName = l.CountryName
            }).ToList();
        }

        /// <summary>
        /// Transformador de negocio a modelo de vista
        /// </summary>
        /// <param name="branchAlliance">Lista de sucursales</param>
        /// <returns>Lista de sucursales para vista</returns>
        public static List<BranchAlliance> CreateBrachAlliances(List<BranchAllianceViewModel> branchAlliance)
        {
            return branchAlliance.Select(l => new BranchAlliance()
            {
                BranchDescription = l.BranchDescription,
                AllianceId = l.AllianceId,
                BranchId = l.BranchId,
                CityCD = l.CityCD,
                CityName = l.CityName,
                StateCD = l.StateCD,
                StateName = l.StateName,
                CountryCD = l.CountryCD,
                CountryName = l.CountryName,
                Status = l.Status,
                SalesPointsAlliance = CreateSalesPoints(l)
            }).ToList();
        }

        /// <summary>
        /// Convierte los puntos de vetna para la vista
        /// </summary>
        /// <param name="branch">Modelo de suscursal</param>
        /// <returns>Lista de puntos de venta</returns>
        public static List<AllianceBranchSalePonit> CreateSalesPoints(BranchAllianceViewModel branch)
        {
            return branch.SalesPoints.Select(l => new AllianceBranchSalePonit()
            {
                SalePointId = l.SalePointId,
                SalePointDescription = l.SalePointDescription,
                Status = l.Status,
                AllianceId = branch.AllianceId,
                BranchId = branch.BranchId
            }).ToList();
        }



        private static List<AllianceBranchSalePonit> CreateSalesPoints(List<AllianceSalesPointsViewModel> salesPoints)
        {
            return salesPoints.Select(l => new AllianceBranchSalePonit()
            {
                SalePointId = l.SalePointId,
                SalePointDescription = l.SalePointDescription,
                Status = l.Status
            }).ToList();
        }

        /// <summary>
        /// Tranformador de negocio a modelos de vista
        /// </summary>
        /// <param name="salesPoints">Puntos de venta</param>
        /// <returns>Puntos de venta para vista</returns>
        public static List<AllianceSalesPointsViewModel> CreateSalesPoints(List<AllianceBranchSalePonit> salesPoints)
        {
            return salesPoints.Select(l => new AllianceSalesPointsViewModel()
            {
                SalePointDescription = l.SalePointDescription,
                SalePointId = l.SalePointId
            }).ToList();
        }




        #endregion

        #region Delegation
        public static List<HierarchyAssociationServiceModel> CreateDelegations(List<DelegationViewModel> delegations)
        {

            List<HierarchyAssociationServiceModel> delegationServiceModel = new List<HierarchyAssociationServiceModel>();
            foreach (var item in delegations)

            {
                delegationServiceModel.Add(CreateDelegation(item));
            }
            return delegationServiceModel;

        }
        private static HierarchyAssociationServiceModel CreateDelegation(DelegationViewModel item) => new HierarchyAssociationServiceModel

        {
            Description = item.Description,
            HierarchyServiceQueryModel = new HierarchyServiceQueryModel { Id = item.Hierarchy },
            ModuleServiceQueryModel = new ModuleServiceQueryModel { Id = item.Module },
            SubModuleServicesQueryModel = new SubModuleServicesQueryModel { Id = item.SubModule },
            StatusTypeService = (StatusTypeService)item.StatusTypeService,
            IsEnabled = item.IsEnabled,
            IsExclusionary = item.IsExclusionary

        };
        #endregion

        #region BaseRejectionCauses

        public static List<RejectionCauseServiceModel> CreateRejectionCauses(List<BaseRejectionCausesViewModel> baseRejectionCausesViewModel)
        {

            List<RejectionCauseServiceModel> rejectionCauseServiceModel = new List<RejectionCauseServiceModel>();
            foreach (var item in baseRejectionCausesViewModel)

            {
                rejectionCauseServiceModel.Add(CreateRejectionCause(item));
            }
            return rejectionCauseServiceModel;

        }

        private static RejectionCauseServiceModel CreateRejectionCause(BaseRejectionCausesViewModel item) => new RejectionCauseServiceModel

        {
           id= item.Id,
           description = item.Description,
           GroupPolicies = new GenericModelServicesQueryModel { id = item.GroupPolicies.Id, description=item.GroupPolicies.Description},
           StatusTypeService = item.StatusTypeService
           
        };
        #endregion
    }
}