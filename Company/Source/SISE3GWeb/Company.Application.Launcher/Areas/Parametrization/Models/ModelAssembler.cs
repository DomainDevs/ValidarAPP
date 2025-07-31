// -----------------------------------------------------------------------
// <copyright file="ModelAssembler.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Desconocido</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    using Application.ModelServices.Enums;
    using Application.ModelServices.Models.CommonParam;
    using Application.ModelServices.Models.UnderwritingParam;
    using AutoMapper;
    using Newtonsoft.Json.Linq;
    using Sistran.Company.Application.ModelServices.Models.AuthorizationPolicies;
    using Sistran.Company.Application.ModelServices.Models.Param;
    using Sistran.Company.Application.ParametrizationAplicationServices.DTO;
    using Sistran.Company.Application.UnderwritingParamApplicationService.DTOs;
    using Sistran.Company.Application.UnderwritingServices.Models;
    using Sistran.Company.Application.UniquePersonParamService.Models;
    using Sistran.Core.Application.CommonService.Models;
    using Sistran.Core.Application.ModelServices.Models.AuthorizationPolicies;
    using Sistran.Core.Application.ModelServices.Models.Common;
    using Sistran.Core.Application.ModelServices.Models.Printing;
    using Sistran.Core.Application.ModelServices.Models.Underwriting;
    using Sistran.Core.Application.ModelServices.Models.UniquePerson;
    using Sistran.Core.Application.ModelServices.Models.VehicleParam;
    using Sistran.Core.Application.UnderwritingServices.Models;
    using Sistran.Core.Application.Utilities.Cache;
    using Sistran.Core.Framework.UIF.Web.Areas.ListRiskPerson.Models;
    using Sistran.Core.Framework.UIF.Web.Areas.ParamAuthorizationPolicies.Models;
    using Sistran.Core.Framework.UIF.Web.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using CoreModV1 = Sistran.Core.Application.UniquePersonService.V1.Models;
    using ENUMSM = Sistran.Core.Application.ModelServices.Enums;
    using MOS = Core.Application.ModelServices.Models.Underwriting;
    using serviceModelsParamCore = Sistran.Core.Application.ModelServices.Models.Param;

    /// <summary>
    /// Clase para mapear las entidades con los modelos. 
    /// </summary>
    public class ModelAssembler
    {
        
        #region SubLineBusiness

        public static List<SubLineBranchServiceModel> CreateSubLinesBusinessParametrization(List<SubLineBusinessViewModel> models)
        {
            List<SubLineBranchServiceModel> subLineBusiness = new List<SubLineBranchServiceModel>();

            foreach (SubLineBusinessViewModel item in models)
            {
                SubLineBranchServiceModel subLine = new SubLineBranchServiceModel
                {
                    Id = item.Id,
                    Description = item.Description,
                    SmallDescription = item.SmallDescription,
                    LineBusinessQuery = new LineBusinessServiceQueryModel()
                    {
                        Description = item.LineBusinessDescription,
                        Id = item.LineBusinessId,
                    },
                    StatusTypeService = Application.ModelServices.Enums.StatusTypeService.Original,
                    ErrorServiceModel = new Application.ModelServices.Models.Param.ErrorServiceModel { ErrorTypeService = Application.ModelServices.Enums.ErrorTypeService.BusinessFault }
                };
                subLineBusiness.Add(subLine);
            }

            return subLineBusiness;
        }
        public static List<SubLineBusinessViewModel> CreateSubLineBusinessParametrization(List<SubLineBranchServiceModel> subLines)
        {
            List<SubLineBusinessViewModel> subLineBusinessModel = new List<SubLineBusinessViewModel>();
            foreach (SubLineBranchServiceModel item in subLines)
            {
                SubLineBusinessViewModel subLineBusiness = new SubLineBusinessViewModel
                {
                    Id = item.Id,
                    Description = item.Description,
                    SmallDescription = item.SmallDescription,
                    LineBusinessId = item.LineBusinessQuery.Id,
                    LineBusinessDescription = item.LineBusinessQuery.Description

                };

                subLineBusinessModel.Add(subLineBusiness);
            }
            return subLineBusinessModel;
        }

        public static List<BranchServiceModel> CreateCoBranches(List<CoBranchViewModel> branch)
        {
            List<BranchServiceModel> branchServiceModel = new List<BranchServiceModel>();

            foreach (var item in branch)
            {
                branchServiceModel.Add(CreateCobranch(item));
            }

            return branchServiceModel;
        }

        private static BranchServiceModel CreateCobranch(CoBranchViewModel item) => new BranchServiceModel
        {
            Id = item.Id,
            Address = item.Adress,
            PhoneNumber = item.PhoneNumbre,
            AddressType = new AddressTypeServiceQueryModel { Id = item.AddressType },
            City = new CityServiceRelationModel { Id = item.City },
            Country = new CountryServiceQueryModel { Id = item.Country },
            State = new StateServiceQueryModel { Id = item.State },
            PhoneType = new PhoneTypeServiceQueryModel { Id = item.PhoneType },
            Branch = new BranchServiceQueryModel { Id = item.BranchId, Description = item.Description, SmallDescription = item.SmallDescription },
            IsIssue = item.IsIssue,
            StatusTypeService = (StatusTypeService)item.Status



        };








        #endregion


        #region SubLinesBusiness


        /// <summary>
        /// Método para convertir lista de objetos tipo TechnicalSubBranchViewModel a tipo SubLineBusiness
        /// </summary>
        /// <param name="sublinebusinessView">Listado de objetos tipo TechnicalSubBranchViewModel</param>
        /// <returns>Listado de SubLineBusiness</returns>        
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

        /// <summary>
        /// Método para convertir lista de objetos tipo SubLineBusiness a tipo TechnicalSubBranchViewModel
        /// </summary>
        /// <param name="subLineBusiness">Listado de objetos tipo SubLineBusiness</param>
        /// <returns>Listado de objetos de tipo TechnicalSubBranchViewModel</returns>        
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
        /// <summary>
        /// Método para mapear objetos de tipo PerilLineBusiness a tipo TechnicalBranchProtectionViewModel
        /// </summary>
        /// <param name="perilLineBusines">Objetos tipo PerilLineBusiness</param>
        /// <returns>Objeto mapeado de tipo TechnicalBranchProtectionViewModel</returns>
        public static TechnicalBranchProtectionViewModel CreateTechicalBranchProtection(PerilLineBusiness perilLineBusines)
        {
            TechnicalBranchProtectionViewModel returnViewModel = new TechnicalBranchProtectionViewModel()
            {
                Id = perilLineBusines.IdLineBusiness
            };

            if (perilLineBusines.PerilAssign != null)
            {
                returnViewModel.PerilAssign = perilLineBusines.PerilAssign.Select(p => CreateProtection(p)).ToList();
            }

            if (perilLineBusines.PerilNotAssign != null)
            {
                returnViewModel.PerilNotAssign = perilLineBusines.PerilNotAssign.Select(p => CreateProtection(p)).ToList();
            }

            return returnViewModel;
        }

        /// <summary>
        ///  Metodo para mapear un objeto ProtectionViewModel a Peril.
        /// </summary>
        /// <param name="protectionModel">Objeto de tipo ProtectionViewModel.</param>
        /// <returns>Objeto de tipo Peril.</returns>
        public static Peril CreateProtection(ProtectionViewModel protectionModel)
        {
            return new Peril()
            {
                Description = protectionModel.DescriptionLong,
                SmallDescription = protectionModel.DescriptionShort,
                Id = protectionModel.Id
            };
        }

        /// <summary>
        /// Método para mapear un objeto de tipo Peril a ProtectionViewModel.
        /// </summary>
        /// <param name="protectionModel">objeto de tipo Peril.</param>
        /// <returns>objeto de tipo ProtectionViewModel.</returns>
        public static ProtectionViewModel CreateProtection(Peril protectionModel)
        {
            return new ProtectionViewModel()
            {
                DescriptionLong = protectionModel.Description,
                DescriptionShort = protectionModel.SmallDescription,
                Id = protectionModel.Id
            };
        }

        /// <summary>
        /// Método para mapear una lista de ojetos de tipo Peril a un lista de objeos de tipo ProtectionViewModel.
        /// </summary>
        /// <param name="protections">lista de ojetos de tipo Peril.</param>
        /// <returns>lista de ojetos de tipo ProtectionViewModel.</returns>
        public static List<ProtectionViewModel> CreateProtections(List<Peril> protections)
        {
            return protections.Select(p => new ProtectionViewModel()
            {
                DescriptionLong = p.Description,
                DescriptionShort = p.SmallDescription,
                Id = p.Id
            }).ToList();
        }
        public static List<PerilServiceModel> CreateProtections(List<ProtectionViewModel> protections)
        {
            if (protections == null)
                return null;
            return protections.Select(p => new PerilServiceModel()
            {
                Description = p.DescriptionLong,
                SmallDescription = p.DescriptionShort,
                Id = p.Id,
                ErrorServiceModel = new Application.ModelServices.Models.Param.ErrorServiceModel { ErrorTypeService = Application.ModelServices.Enums.ErrorTypeService.Ok },
                StatusTypeService = Application.ModelServices.Enums.StatusTypeService.Original
            }).ToList();
        }
        public static List<ProtectionViewModel> CreateProtections(List<Application.EntityServices.Models.PostEntity> postEntities)
        {
            return postEntities.Select(p => new ProtectionViewModel()
            {
                Id = int.Parse(p.Fields.First(y => y.Name == "PerilCode").Value),
                DescriptionLong = p.Fields.First(y => y.Name == "Description").Value,
                DescriptionShort = p.Fields.First(y => y.Name == "SmallDescription").Value,
                Status = Application.EntityServices.Enums.StatusTypeService.Original
            }).ToList();
        }

        public static List<InsurencesObjectsViewModel> CreateInsurencesObjects(List<Application.EntityServices.Models.PostEntity> postEntities)
        {
            return postEntities.Select(p => new InsurencesObjectsViewModel()
            {
                Id = int.Parse(p.Fields.First(y => y.Name == "InsuredObjectCode").Value),
                Description = p.Fields.First(y => y.Name == "Description").Value,
                SmallDescription = p.Fields.First(y => y.Name == "SmallDescription").Value
            }).ToList();
        }
        /// <summary>
        /// Método para mapear una lista de objetos de tipo ProtectionViewModel a objetos de tipo Peril.
        /// </summary>
        /// <param name="protections">Lista de objetos de tipo ProtectionViewModel.</param>
        /// <returns>Lista de objetos de tipo Peril.</returns>
        public static List<Peril> CreateProtectionsPeril(List<ProtectionViewModel> protections)
        {
            if (protections == null)
            {
                return null;
            }

            return protections.Select(p => new Peril()
            {
                Description = p.DescriptionLong,
                SmallDescription = p.DescriptionShort,
                Id = p.Id
            }).ToList();
        }
        #endregion

        #region InsurencesObjects
        /// <summary>
        /// Método para mapear un objeto de tipo CompanyInsuredObject a InsurencesObjectsViewModel.
        /// </summary>
        /// <param name="insurencesObjects">objeto de tipo CompanyInsuredObject.</param>
        /// <returns>objeto de tipo InsurencesObjectsViewModel.</returns>
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

        /// <summary>
        /// Método para mapear una lista de objetos de tipo InsurencesObjectsViewModel a CompanyInsuredObject.
        /// </summary>
        /// <param name="insurencesObjects">lista de objetos de tipo InsurencesObjectsViewModel.</param>
        /// <returns>lista de objetos de tipo CompanyInsuredObject.</returns>
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

        public static List<InsurencesObjectsViewModel> CreateInsuredObjectViewModel(List<InsuredObjectServiceModel> insuredObjectServiceModel)
        {
            List<InsurencesObjectsViewModel> discountsModel = new List<InsurencesObjectsViewModel>();
            foreach (InsuredObjectServiceModel item in insuredObjectServiceModel)
            {
                InsurencesObjectsViewModel deductible = new InsurencesObjectsViewModel
                {
                    IsDeraclarative = item.IsDeclarative,
                    Id = item.Id,
                    Description = item.Description,
                    SmallDescription = item.SmallDescription,
                    Status = (int)item.StatusTypeService
                };

                discountsModel.Add(deductible);
            }
            return discountsModel;
        }
        public static List<InsuredObjectServiceModel> CreateInsuredObjectServiceModel(List<InsurencesObjectsViewModel> models)
        {
            List<InsuredObjectServiceModel> discount = new List<InsuredObjectServiceModel>();

            foreach (InsurencesObjectsViewModel item in models)
            {
                InsuredObjectServiceModel deductible = new InsuredObjectServiceModel
                {
                    Id = item.Id,
                    Description = item.Description,
                    SmallDescription = item.SmallDescription,
                    IsDeclarative = item.IsDeraclarative,
                    StatusTypeService = (StatusTypeService)item.Status,
                    ErrorServiceModel = new Application.ModelServices.Models.Param.ErrorServiceModel { ErrorTypeService = Application.ModelServices.Enums.ErrorTypeService.BusinessFault }
                };
                discount.Add(deductible);
            }
            return discount;
        }

        #endregion

        #region Branchs
        /// <summary>
        /// Convierte de modelo de servicio a view model    
        /// </summary>
        /// <param name="branchServiceQueryModel"></param>
        /// <returns>lista de BranchViewModel</returns>
        public static List<BranchViewModel> CreateBranchesViewModel(List<BranchServiceQueryModel> branchServiceQueryModel)
        {
            List<BranchViewModel> branchViewModel = new List<Models.BranchViewModel>();
            foreach (var item in branchServiceQueryModel)
            {
                branchViewModel.Add(CreateBranchViewModel(item));
            }
            return branchViewModel;
        }

        /// <summary>
        /// Convierte de modelo de servicio a view model 
        /// </summary>
        /// <param name="branchServiceQueryModel"></param>
        /// <returns>retorna modelo de vista de sucursales</returns>
        private static BranchViewModel CreateBranchViewModel(BranchServiceQueryModel branchServiceQueryModel) => new BranchViewModel
        {
            Id = branchServiceQueryModel.Id,
            LongDescription = branchServiceQueryModel.Description,
            ShortDescription = branchServiceQueryModel.SmallDescription,
            Is_issue = branchServiceQueryModel.Is_issue
        };

        /// <summary>
        /// Convierte de view model a modelo de servicio 
        /// </summary>
        /// <param name="branch"></param>
        /// <returns> lista de service model</returns>
        public static List<BranchServiceQueryModel> CreateBranchesServiceModel(List<BranchViewModel> branch)
        {
            List<BranchServiceQueryModel> branchServiceQueryModel = new List<BranchServiceQueryModel>();
            foreach (var item in branch)
            {
                branchServiceQueryModel.Add(CreateBranchServiceModel(item));
            }

            return branchServiceQueryModel;
        }

        /// <summary>
        /// Convierte de view model a modelo de servicio 
        /// </summary>
        /// <param name="branchViewModel"></param>
        /// <returns> service model</returns>
        private static BranchServiceQueryModel CreateBranchServiceModel(BranchViewModel branchViewModel) => new BranchServiceQueryModel
        {
            Description = branchViewModel.LongDescription,
            Id = branchViewModel.Id,
            SmallDescription = branchViewModel.ShortDescription,
            Is_issue = branchViewModel.Is_issue
        };


        /// <summary>
        /// Método para mapear una lista de objetos de tipo Branch a BranchViewModel.
        /// </summary>
        /// <param name="branchs">lista de objetos de tipo Branch.</param>
        /// <returns>lista de objetos de tipo BranchViewModel.</returns>
        public static List<BranchViewModel> GetBranchs(List<Branch> branchs)
        {
            List<BranchViewModel> branchModelView = new List<BranchViewModel>();
            foreach (Branch model in branchs)
            {
                BranchViewModel branchModel = new BranchViewModel();
                branchModel.LongDescription = model.Description;
                branchModel.ShortDescription = model.SmallDescription;
                branchModel.Id = model.Id;

                branchModelView.Add(branchModel);
            }

            return branchModelView;
        }

        /// <summary>
        /// Método para mapear una lista de objetos de tipo BranchViewModel a Branch.
        /// </summary>
        /// <param name="branchs">lista de objetos de tipo BranchViewModel.</param>
        /// <returns>lista de objetos de tipo Branch.</returns>
        public static List<Branch> CreateBranchs(List<BranchViewModel> branchs)
        {
            if (branchs == null)
            {

                return null;
            }

            List<Branch> branchModelView = new List<Branch>();
            foreach (BranchViewModel model in branchs)
            {
                Branch branchModel = new Branch();
                branchModel.Description = model.LongDescription;
                branchModel.SmallDescription = model.ShortDescription;
                branchModel.Id = model.Id;

                branchModelView.Add(branchModel);
            }

            return branchModelView;
        }

        #endregion

        #region Prefix
        /// <summary>
        /// Método para mapear una lista de objetos de tipo BusinessBranchViewModel a Prefix.
        /// </summary>
        /// <param name="prefixView">lista de objetos de tipo BusinessBranchViewModel.</param>
        /// <returns>lista de objetos de tipo Prefix.</returns>
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
                prefrixes.Add(prefix);
            }

            return prefrixes;
        }

        /// <summary>
        /// Método para mapear una lista de objetos de tipo Prefix a BusinessBranchViewModel.
        /// </summary>
        /// <param name="prefix">Lista de objetos de tipo Prefix.</param>
        /// <returns>Lista de objetos de tipo BusinessBranchViewModel.</returns>
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

        #region Expenses


        /// <summary>
        /// Método para mapear una lista de objetos de tipo Expense a ExpenseViewModel.
        /// </summary>
        /// <param name="listExpenses">Lista de objetos de tipo Expense.</param>
        /// <returns>Lista de objetos de tipo ExpenseViewModel.</returns>
        public static List<ExpenseViewModel> GetExpenses(List<Expense> listExpenses)
        {
            if (listExpenses != null)
            {
                List<ExpenseViewModel> expensesViewModel = new List<ExpenseViewModel>();
                foreach (Expense model in listExpenses)
                {
                    ExpenseViewModel expensesModel = new ExpenseViewModel();

                    expensesModel.id = model.id;
                    expensesModel.Description = model.Description;
                    expensesModel.Abbreviation = model.Abbreviation;
                    expensesModel.InitiallyIncluded = model.InitiallyIncluded;
                    expensesModel.Mandatory = model.Mandatory;
                    expensesModel.Rate = model.Rate;
                    expensesModel.RateType = model.RateType;
                    expensesModel.RuleSet = model.RuleSet;
                    expensesModel.RuleSetName = model.RuleSetName;
                    expensesModel.ComponentClass = model.ComponentClass;
                    expensesModel.ComponentType = model.ComponentType;

                    expensesViewModel.Add(expensesModel);
                }

                return expensesViewModel;
            }

            return null;
        }

        /// <summary>
        /// Método para mapear una lista de objetos de tipo ExpenseViewModel a Expense.
        /// </summary>
        /// <param name="listExpense">Lista de objetos de tipo ExpenseViewModel.</param>
        /// <returns>Lista de objetos de tipo Expense.</returns>
        public static List<Expense> CreateExpenses(List<ExpenseViewModel> listExpense)
        {
            if (listExpense != null)
            {
                List<Expense> expenses = new List<Expense>();
                foreach (ExpenseViewModel model in listExpense)
                {
                    Expense expenseModel = new Expense();

                    expenseModel.Description = model.Description;
                    expenseModel.Abbreviation = model.Abbreviation;

                    expenseModel.id = model.id;
                    expenseModel.InitiallyIncluded = model.InitiallyIncluded;
                    expenseModel.Mandatory = model.Mandatory;

                    expenseModel.Rate = model.Rate;
                    expenseModel.RateType = model.RateType;
                    expenseModel.RuleSet = model.RuleSet;
                    expenseModel.ComponentClass = model.ComponentClass;
                    expenseModel.ComponentType = model.ComponentType;

                    expenses.Add(expenseModel);
                }

                return expenses;
            }

            return null;
        }
        #endregion

        #region InsuredProfiles
        /// <summary>
        /// Método para mapear una lista de objetos de tipo InsuredProfile a InsuredProfileViewModel.
        /// </summary>
        /// <param name="insuredProfiles">lista de objetos de tipo InsuredProfile.</param>
        /// <returns>lista de objetos de tipo InsuredProfileViewModel.</returns>
        public static List<InsuredProfileViewModel> GetInsuredProfiles(List<CoreModV1.InsuredProfile> insuredProfiles)
        {
            List<InsuredProfileViewModel> insuredProfileModelView = new List<InsuredProfileViewModel>();
            foreach (CoreModV1.InsuredProfile model in insuredProfiles)
            {
                InsuredProfileViewModel insuredProfileModel = new InsuredProfileViewModel();
                insuredProfileModel.LongDescription = model.Description;
                insuredProfileModel.ShortDescription = model.SmallDescription;
                insuredProfileModel.Id = model.Id;

                insuredProfileModelView.Add(insuredProfileModel);
            }

            return insuredProfileModelView;
        }

        /// <summary>
        /// Método para mapear una lista de objetos de tipo InsuredProfileViewModel a InsuredProfile.
        /// </summary>
        /// <param name="insuredProfilesViewModels">lista de objetos de tipo InsuredProfileViewModel.</param>
        /// <returns>lista de objetos de tipo InsuredProfile.</returns>
        public static List<CoreModV1.InsuredProfile> CreateInsuredProfiles(List<InsuredProfileViewModel> insuredProfilesViewModels)
        {
            if (insuredProfilesViewModels == null)
            {
                return null;
            }

            List<CoreModV1.InsuredProfile> insuredProfileModelView = new List<CoreModV1.InsuredProfile>();
            foreach (InsuredProfileViewModel model in insuredProfilesViewModels)
            {
                CoreModV1.InsuredProfile insuredProfileModel = new CoreModV1.InsuredProfile();
                insuredProfileModel.Description = model.LongDescription;
                insuredProfileModel.SmallDescription = model.ShortDescription;
                insuredProfileModel.Id = model.Id;

                insuredProfileModelView.Add(insuredProfileModel);
            }

            return insuredProfileModelView;
        }

        #endregion

        #region InsuredSegments
        /// <summary>
        /// Método para mapear una lista de objetos de tipo InsuredSegment a InsuredSegmentViewModel.
        /// </summary>
        /// <param name="insuredSegments">lista de objetos de tipo InsuredSegment.</param>
        /// <returns>lista de objetos de tipo InsuredSegmentViewModel.</returns>
        public static List<InsuredSegmentViewModel> GetInsuredSegments(List<CoreModV1.InsuredSegmentV1> insuredSegments)
        {
            List<InsuredSegmentViewModel> insuredSegmentModelView = new List<InsuredSegmentViewModel>();
            foreach (CoreModV1.InsuredSegmentV1 model in insuredSegments)
            {
                InsuredSegmentViewModel insuredSegmentModel = new InsuredSegmentViewModel();
                insuredSegmentModel.LongDescription = model.LongDescription;
                insuredSegmentModel.ShortDescription = model.ShortDescription;
                insuredSegmentModel.Id = model.Id;

                insuredSegmentModelView.Add(insuredSegmentModel);
            }

            return insuredSegmentModelView;
        }

        /// <summary>
        /// Método para mapear una lista de objetos de tipo InsuredSegmentViewModel a InsuredSegment.
        /// </summary>
        /// <param name="insuredSegmentsViewModels">lista de objetos de tipo InsuredSegmentViewModel.</param>
        /// <returns>lista de objetos de tipo InsuredSegment.</returns>
        public static List<CoreModV1.InsuredSegmentV1> CreateInsuredSegments(List<InsuredSegmentViewModel> insuredSegmentsViewModels)
        {
            if (insuredSegmentsViewModels == null)
            {
                return null;
            }

            List<CoreModV1.InsuredSegmentV1> insuredSegmentModelView = new List<CoreModV1.InsuredSegmentV1>();
            foreach (InsuredSegmentViewModel model in insuredSegmentsViewModels)
            {
                CoreModV1.InsuredSegmentV1 insuredSegmentModel = new CoreModV1.InsuredSegmentV1();
                insuredSegmentModel.LongDescription = model.LongDescription;
                insuredSegmentModel.ShortDescription = model.ShortDescription;
                insuredSegmentModel.Id = model.Id;

                insuredSegmentModelView.Add(insuredSegmentModel);
            }

            return insuredSegmentModelView;
        }
        #endregion

        #region Deductible
        public static List<DeductibleServiceModel> CreateDeductibles(List<DeductibleViewModel> models)
        {
            List<DeductibleServiceModel> deductibles = new List<DeductibleServiceModel>();

            foreach (DeductibleViewModel item in models)
            {
                DeductibleServiceModel deductible = new DeductibleServiceModel
                {
                    DeductibleSubject = new DeductibleSubjectServiceQueryModel
                    {
                        Id = item.ApplyOnId,
                        Description = item.DeductibleSubjectDescription
                    },
                    MaxDeductibleSubject = new DeductibleSubjectServiceQueryModel
                    {
                        Id = (int)item.ApplyOnMaxId,
                        Description = item.ApplyOnMaxDescription
                    },
                    MaxDeductValue = item.Max == "" ? 0 : Convert.ToDecimal(item.Max),
                    MaxDeductibleUnit = new DeductibleUnitServiceQueryModel
                    {
                        Id = (int)item.UnitMaxId.Value,
                        Description = item.UnitMaxDescription
                    },
                    MinDeductibleSubject = new DeductibleSubjectServiceQueryModel
                    {
                        Id = (int)item.ApplyOnMinId,
                        Description = item.ApplyOnMinDescription
                    },
                    MinDeductibleUnit = new DeductibleUnitServiceQueryModel
                    {
                        Id = item.UnitMinId,
                        Description = item.UnitMinDescription
                    },
                    MinDeductValue = item.Min == "" ? 0 : Convert.ToDecimal(item.Min),
                    Currency = new CurrencyServiceQueryModel
                    {
                        Id = (int)item.CurrencyId.Value,
                        Description = item.CurrencyDescription
                    },
                    Id = item.DeductibleId,
                    LineBusiness = new LineBusinessServiceQueryModel
                    {
                        Id = item.LineBusinessId,
                        Description = item.LineDescription
                    },
                    RateType = (Application.ModelServices.Enums.RateType)item.Type,
                    Rate = item.Rate == "" ? Convert.ToDecimal(0) : Convert.ToDecimal(item.Rate),
                    Description = item.TotalDescription,
                    DeductibleUnit = new DeductibleUnitServiceQueryModel
                    {
                        Id = item.UnitId,
                        Description = item.UnitDescription
                    },
                    DeductValue = Convert.ToDecimal(item.Value),
                    StatusTypeService = Application.ModelServices.Enums.StatusTypeService.Original,
                    ErrorServiceModel = new Application.ModelServices.Models.Param.ErrorServiceModel { ErrorTypeService = Application.ModelServices.Enums.ErrorTypeService.BusinessFault }
                };
                deductibles.Add(deductible);
            }
            return deductibles;
        }
        public static List<DeductibleViewModel> CreateDeductible(List<DeductibleServiceModel> deductibles)
        {
            List<DeductibleViewModel> deductiblesModel = new List<DeductibleViewModel>();
            foreach (DeductibleServiceModel item in deductibles)
            {
                DeductibleViewModel deductible = new DeductibleViewModel
                {
                    ApplyOnId = item.DeductibleSubject.Id,
                    ApplyOnMaxId = item.MaxDeductibleSubject.Id,
                    ApplyOnMinId = item.MinDeductibleSubject.Id,
                    CurrencyDescription = item.Currency.Description,
                    CurrencyId = item.Currency.Id,
                    DeductibleId = item.Id,
                    LineBusinessId = item.LineBusiness.Id,
                    LineDescription = item.LineBusiness.Description,
                    Max = item.MaxDeductValue.ToString(),
                    Min = item.MinDeductValue.ToString(),
                    Rate = item.Rate.ToString(),
                    TotalDescription = item.Description,
                    Type = (int)item.RateType,
                    UnitId = item.DeductibleUnit.Id,
                    UnitMaxId = item.MaxDeductibleUnit.Id,
                    UnitMinId = item.MinDeductibleUnit.Id,
                    Value = item.DeductValue.ToString(),
                    ApplyOnMinDescription = item.MinDeductibleSubject.Description,
                    ApplyOnMaxDescription = item.MaxDeductibleSubject.Description,
                    DeductibleSubjectDescription = item.DeductibleSubject.Description,
                    UnitDescription = item.DeductibleUnit.Description,
                    UnitMinDescription = item.MinDeductibleUnit.Description,
                    UnitMaxDescription = item.MaxDeductibleUnit.Description,
                    TypeDescription = Helpers.EnumsHelper.GetItemName<RateType>(item.RateType),
                    Status = Application.EntityServices.Enums.StatusTypeService.Original
                };

                deductiblesModel.Add(deductible);
            }
            return deductiblesModel;
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

        #region ServiceQuotationSource

        ///// <summary>
        ///// Asembler para canales model service
        ///// </summary>
        ///// <param name="serviceQuotationSources">Una lista de canal model service</param>
        ///// <returns>Una lista de ChannelViewModel model</returns>
        //public static List<ChannelViewModel> GetServiceQuotationSource(List<ServiceQuotationSource> serviceQuotationSources)
        //{
        //    List<ChannelViewModel> channelModelView = new List<ChannelViewModel>();
        //    foreach (ServiceQuotationSource model in serviceQuotationSources)
        //    {
        //        ChannelViewModel channelViewModel = new ChannelViewModel();
        //        channelViewModel.Id = model.Id;
        //        channelViewModel.IsFine = model.IsFine;
        //        channelViewModel.IsScore = model.IsScore;
        //        channelViewModel.Comments = model.Comments;
        //        channelViewModel.IsEnabled = model.IsEnabled;
        //        channelViewModel.Description = model.Description;
        //        channelViewModel.DetailDescription = model.DetailDescription;

        //        channelModelView.Add(channelViewModel);
        //    }

        //    return channelModelView;
        //}

        ///// <summary>
        ///// Asembler para canales model
        ///// </summary>
        ///// <param name="channelModelView">Una lista de ChannelViewModel model</param>
        ///// <returns>Una lista de canal model service</returns>
        //public static List<ServiceQuotationSource> CreateServiceQuotationSource(List<ChannelViewModel> channelModelView)
        //{
        //    if (channelModelView == null)
        //        return null;
        //    List<ServiceQuotationSource> serviceQuotationSources = new List<ServiceQuotationSource>();
        //    foreach (ChannelViewModel model in channelModelView)
        //    {
        //        ServiceQuotationSource channelViewModel = new ServiceQuotationSource();
        //        channelViewModel.Id = model.Id;
        //        channelViewModel.IsFine = model.IsFine;
        //        channelViewModel.IsScore = model.IsScore;
        //        channelViewModel.Comments = model.Comments;
        //        channelViewModel.IsEnabled = model.IsEnabled;
        //        channelViewModel.Description = model.Description;
        //        channelViewModel.DetailDescription = model.DetailDescription;
        //        channelViewModel.ValuesDefault = model.ValuesDefault;
        //        serviceQuotationSources.Add(channelViewModel);
        //    }

        //    return serviceQuotationSources;
        //}

        #endregion

        #region CoveredRiskType
        /// <summary>
        /// Método para mapear una lista de objetos de tipo CoveredRiskType a CoveredRiskTypeViewModel.
        /// </summary>
        /// <param name="coveredRiskType">lista de objetos de tipo de riesgo cubierto.</param>
        /// <returns>lista de objetos de tipo CoveredRiskTypeViewModel.</returns>
        public static List<CoveredRiskTypeViewModel> GetCoveredRiskTypes(CoveredRiskTypesServiceModel coveredRiskTypeServiceModel)
        {
            List<CoveredRiskTypeViewModel> coveredRiskTypeModelView = new List<CoveredRiskTypeViewModel>();
            foreach (CoveredRiskTypeServiceModel mCoveredRiskTypeServiceModel in coveredRiskTypeServiceModel.CoveredRiskTypeServiceModel)
            {
                CoveredRiskTypeViewModel vmCoveredRiskTypeViewModel = new CoveredRiskTypeViewModel();
                vmCoveredRiskTypeViewModel.ShortDescription = mCoveredRiskTypeServiceModel.SmallDescription;
                vmCoveredRiskTypeViewModel.Id = mCoveredRiskTypeServiceModel.Id;

                coveredRiskTypeModelView.Add(vmCoveredRiskTypeViewModel);
            }
            return coveredRiskTypeModelView;
        }
        #endregion

        #region Parameter

        /// <summary>
        /// Asembler para parametros Service Model
        /// </summary>
        /// <param name="parameterServiceModel">lista de modelo ParametroViewModel</param>
        /// <returns>Lista de modelo ParametroViewModel</returns>
        public static List<ParametroViewModel> GetParameter(List<ParameterServiceModel> parameterServiceModel)
        {
            List<ParametroViewModel> parametroViewModel = new List<ParametroViewModel>();
            foreach (ParameterServiceModel model in parameterServiceModel)
            {
                ParametroViewModel parametroVMo = new ParametroViewModel();
                parametroVMo.ParameterId = model.ParameterId;
                parametroVMo.Description = model.Description;
                parametroVMo.Value = model.Value;

                parametroViewModel.Add(parametroVMo);
            }

            return parametroViewModel;
        }

        /// <summary>
        /// Asembler para parametros Service Model
        /// </summary>
        /// <param name="parametroViewModel">Lista de modelo ParametroViewModel</param>
        /// <returns>Lista de modelo ParametroViewModel</returns>
        public static List<ParameterServiceModel> CreateParameter(List<ParametroViewModel> parametroViewModel)
        {
            if (parametroViewModel == null)
                return null;
            List<ParameterServiceModel> parameterServiceModel = new List<ParameterServiceModel>();
            foreach (ParametroViewModel model in parametroViewModel)
            {
                ParameterServiceModel parameterSM = new ParameterServiceModel();
                parameterSM.ParameterId = model.ParameterId;
                parameterSM.Description = model.Description;
                parameterSM.Value = model.Value;

                parameterServiceModel.Add(parameterSM);
            }

            return parameterServiceModel;
        }

        #endregion

        #region Surcharge

        /// <summary>
        /// Método para mapear un objeto tipo SurchargeViewModel a SurchargeServiceModel.
        /// </summary>
        /// <param name="surcharge">Objeto tipo SurchargeServiceModel.</param>
        /// <returns>Objeto tipo SurchargeViewModel.</returns>
        public static List<SurchargeViewModel> CreateSurcharge(List<SurchargeServiceModel> surcharge)
        {
            List<SurchargeViewModel> surchargesModel = new List<SurchargeViewModel>();
            foreach (SurchargeServiceModel item in surcharge)
            {
                SurchargeViewModel deductible = new SurchargeViewModel
                {
                    TinyDescription = item.TinyDescription,
                    Type = (int)item.Type,
                    Rate = item.Rate.ToString(),
                    Id = item.Id,
                    Description = item.Description,
                    StatusTypeService = item.StatusTypeService,
                };

                surchargesModel.Add(deductible);
            }
            return surchargesModel;
        }

        /// <summary>
        /// Método para mapear un objeto tipo SurchargeViewModel a SurchargeServiceModel.
        /// </summary>
        /// <param name="models">Objeto tipo SurchargeServiceModel.</param>
        /// <returns>Objeto tipo SurchargeViewModel.</returns>
        public static List<SurchargeServiceModel> CreateSurcharges(List<SurchargeViewModel> models)
        {
            List<SurchargeServiceModel> surcharge = new List<SurchargeServiceModel>();

            foreach (SurchargeViewModel item in models)
            {
                SurchargeServiceModel surchargeService = new SurchargeServiceModel
                {

                    Id = item.Id,
                    Type = (Application.ModelServices.Enums.RateType)item.Type,
                    Rate = item.Rate == "" ? Convert.ToDecimal(0) : Convert.ToDecimal(item.Rate),
                    Description = item.Description,
                    TinyDescription = item.TinyDescription,
                    StatusTypeService = item.StatusTypeService,
                    ErrorServiceModel = new Application.ModelServices.Models.Param.ErrorServiceModel { ErrorTypeService = Application.ModelServices.Enums.ErrorTypeService.BusinessFault }
                };
                surcharge.Add(surchargeService);
            }
            return surcharge;
        }
        #endregion

        #region Discount

        /// <summary>
        /// Método para mapear un objeto tipo DiscountViewModel a DiscountServiceModel.
        /// </summary>
        /// <param name="discount">Objeto tipo DiscountViewModel.</param>
        /// <returns>Objeto tipo DiscountServiceModel.</returns>
        public static List<DiscountViewModel> CreateDiscount(List<DiscountServiceModel> discount)
        {
            List<DiscountViewModel> discountsModel = new List<DiscountViewModel>();
            foreach (DiscountServiceModel item in discount)
            {
                DiscountViewModel deductible = new DiscountViewModel
                {
                    TinyDescription = item.TinyDescription,
                    Type = (int)item.Type,
                    Rate = item.Rate.ToString(),
                    Id = item.Id,
                    Description = item.Description,
                    StatusTypeService = item.StatusTypeService
                };

                discountsModel.Add(deductible);
            }
            return discountsModel;
        }

        /// <summary>
        /// Método para mapear un objeto tipo DiscountViewModel a DiscountServiceModel.
        /// </summary>
        /// <param name="models">Objeto tipo DiscountViewModel.</param>
        /// <returns>Objeto tipo DiscountServiceModel.</returns>
        public static List<DiscountServiceModel> CreateDiscounts(List<DiscountViewModel> models)
        {
            List<DiscountServiceModel> discount = new List<DiscountServiceModel>();

            foreach (DiscountViewModel item in models)
            {
                DiscountServiceModel deductible = new DiscountServiceModel
                {

                    Id = item.Id,
                    Type = (Application.ModelServices.Enums.RateType)item.Type,
                    Rate = item.Rate == "" ? Convert.ToDecimal(0) : Convert.ToDecimal(item.Rate),
                    Description = item.Description,
                    TinyDescription = item.TinyDescription,
                    StatusTypeService = item.StatusTypeService,
                    ErrorServiceModel = new Application.ModelServices.Models.Param.ErrorServiceModel { ErrorTypeService = Application.ModelServices.Enums.ErrorTypeService.BusinessFault }
                };
                discount.Add(deductible);
            }
            return discount;
        }
        #endregion

        #region Detail
        /// <summary>
        /// Tranformador modelo servicio a modelos de vista
        /// </summary>
        /// <param name="deductibles">Listado de DetailServiceModel</param>
        /// <returns>Listado de DetailViewModel</returns>
        public static List<DetailViewModel> CreateDetailsViewModel(List<DetailServiceModel> deductibles)
        {
            List<DetailViewModel> detailsViewModel = new List<DetailViewModel>();
            foreach (DetailServiceModel item in deductibles)
            {
                var imapper = CreateMapDetail();
                DetailViewModel detailViewModel = imapper.Map<DetailServiceModel, DetailViewModel>(item);
                if (item.Enabled)
                {
                    detailViewModel.EnabledDescription = App_GlobalResources.Language.LabelIf;
                }
                else
                {
                    detailViewModel.EnabledDescription = App_GlobalResources.Language.LabelNot;
                }
                detailViewModel.TypeDescription = item.DetailType.Description;
                if (item.RateType != null && item.RateType != 0)
                {
                    detailViewModel.RateTypeId = (int)item.RateType;
                    detailViewModel.RateTypeDescription = item.RateType == null ? "" : EnumsHelper.GetItemName<RateType>(item.RateType);
                }
                detailViewModel.Status = Application.EntityServices.Enums.StatusTypeService.Original;
                detailsViewModel.Add(detailViewModel);
            }

            return detailsViewModel;
        }
        #endregion

        #region ExponseComponent

        public static List<ExpenseViewModel> CreateExponseViewModel(ExpensesServiceModel Exponse)
        {
            List<ExpenseViewModel> listViewModel = new List<ExpenseViewModel>();
            foreach (ExpenseServiceModel viewModel in Exponse.ComponentServiceModel)
            {
                listViewModel.Add(CreateExponseViewModel(viewModel));
            }
            return listViewModel;
        }
        /// <summary>
        /// Crea el modelo de vista
        /// </summary>
        /// <param name="vehicleType">Tipo de vehiculo</param>
        /// <returns>Modelo de vista</returns>
        public static ExpenseViewModel CreateExponseViewModel(ExpenseServiceModel Exponse)
        {
            ExpenseViewModel viewModel = new ExpenseViewModel()
            {
                id = Exponse.Id,
                Description = Exponse.SmallDescription,
                Abbreviation = Exponse.TinyDescripcion,
                Mandatory = Exponse.IsMandatory,
                InitiallyIncluded = Exponse.IsInitially,
                ComponentClass = (int)Exponse.ComponentClass,
                ComponentType = (int)Exponse.ComponentType,
                Rate = Exponse.Rate,
                RuleSet = Exponse.RuleSetServiceQueryModel.Id,
                RuleSetName = Exponse.RuleSetServiceQueryModel.description,
                RateType = Exponse.RateTypeServiceQueryModel.Id,
                RateTypeName = Exponse.RateTypeServiceQueryModel.description,
                StatusTypeService = (int)Exponse.StatusTypeService
            };
            return viewModel;
        }

        /// <summary>
        /// Construye el modelo de servicio
        /// </summary>
        /// <param name="vehicleTypeViewModels">Modelo vista</param>
        /// <returns>Modelo de servicio</returns>
        public static List<ExpenseServiceModel> CreateExpensesServiceModel(List<ExpenseViewModel> expenseServiceModel)
        {
            List<ExpenseServiceModel> viewModels = new List<ExpenseServiceModel>();
            foreach (ExpenseViewModel item in expenseServiceModel)
            {
                viewModels.Add(CreateExpenseServiceModel(item));
            }
            return viewModels;
        }

        /// <summary>
        /// Construye el modelo de servicio
        /// </summary>
        /// <param name="vehicleTypeViewModel">Modelo vista</param>
        /// <returns>Modelo de servicio</returns>
        public static ExpenseServiceModel CreateExpenseServiceModel(ExpenseViewModel expenseViewModel)
        {

            ExpenseServiceModel serviceModel = new ExpenseServiceModel()
            {
                Id = expenseViewModel.id,
                SmallDescription = expenseViewModel.Description,
                TinyDescripcion = expenseViewModel.Abbreviation,
                IsMandatory = expenseViewModel.Mandatory,
                IsInitially = expenseViewModel.InitiallyIncluded,
                ComponentClass = ComponentClass.EXPENSES,
                ComponentType = ComponnetType.EXPENSES,
                Rate = (int)expenseViewModel.Rate,
                RuleSetServiceQueryModel = new RuleSetServiceQueryModel() { Id = expenseViewModel.RuleSet, description = expenseViewModel.RuleSetName },
                RateTypeServiceQueryModel = new RateTypeServiceQueryModel() { Id = expenseViewModel.RateType, description = expenseViewModel.RateTypeName },
                StatusTypeService = (StatusTypeService)expenseViewModel.StatusTypeService
            };
            return serviceModel;

        }

        #endregion

        #region SalePoint

        /// <summary>
        /// convierte lista de modelo de servicio en lista de modelo de la vista
        /// </summary>
        /// <param name="salePoint">modelo de la vista </param>
        /// <returns>lista de modelo de servicio</returns>
        public static List<SalePointServiceModel> CreateSalePointesServiceModel(List<SalePointViewModel> salePoint)
        {
            List<SalePointServiceModel> salePointServiceModel = new List<SalePointServiceModel>();

            foreach (var item in salePoint)
            {
                salePointServiceModel.Add(CreateSalePointServiceModel(item));
            }

            return salePointServiceModel;
        }


        public static List<SalePointViewModel> CreateSalePointesViewModel(List<SalePointServiceModel> salePointServiceModel)
        {
            List<SalePointViewModel> salePointViewModel = new List<SalePointViewModel>();
            foreach (var item in salePointServiceModel)
            {
                salePointViewModel.Add(CreateSalePointViewModel(item));
            }

            return salePointViewModel;
        }

        private static SalePointViewModel CreateSalePointViewModel(SalePointServiceModel item) => new SalePointViewModel
        {
            Branch = item.Branch.Description,
            BranchId = item.Branch.Id,
            Description = item.Description,
            Id = item.Id,
            SmallDescription = item.SmallDescription,
            Enabled = item.Enabled
        };

        /// <summary>
        /// convierte modelo de servicio a modelo de la vista
        /// </summary>
        /// <param name="item">modelo de la vista </param>
        /// <returns>modelo de servicio</returns>
        private static SalePointServiceModel CreateSalePointServiceModel(SalePointViewModel item) => new SalePointServiceModel
        {
            Description = item.Description,
            Id = item.Id,
            Branch = new BranchServiceQueryModel
            {
                Description = item.Branch,
                Id = item.BranchId
            },
            SmallDescription = item.SmallDescription,
            Enabled = item.Enabled
        };










        #endregion

        #region VehicleType
        /// <summary>
        /// Crea el modelo de vista
        /// </summary>
        /// <param name="vehicleTypes">Tipo de vehiculo</param>
        /// <returns>Listado de modelo de vista</returns>
        public static List<VehicleTypeViewModel> CreateVehicleTypeViewModel(VehicleTypesServiceModel vehicleTypes)
        {
            List<VehicleTypeViewModel> listViewModel = new List<VehicleTypeViewModel>();
            foreach (VehicleTypeServiceModel viewModel in vehicleTypes.VehicleTypeServiceModel)
            {
                listViewModel.Add(CreateVehicleTypeViewModel(viewModel));
            }
            return listViewModel;
        }

        /// <summary>
        /// Crea el modelo de vista
        /// </summary>
        /// <param name="vehicleType">Tipo de vehiculo</param>
        /// <returns>Modelo de vista</returns>
        public static VehicleTypeViewModel CreateVehicleTypeViewModel(VehicleTypeServiceModel vehicleType)
        {
            VehicleTypeViewModel viewModel = new VehicleTypeViewModel()
            {
                Description = vehicleType.Description,
                IsActive = vehicleType.IsEnable,
                IsTruck = vehicleType.IsTruck,
                ShortDescription = vehicleType.SmallDescription,
                TypeCode = vehicleType.Id,
                State = (int)vehicleType.StatusTypeService,

            };
            viewModel.VehicleBodies = new List<int>();
            viewModel.VehicleBodies.AddRange(vehicleType.VehicleBodyServiceQueryModel.Select(p => p.Id));
            return viewModel;
        }

        /// <summary>
        /// Construye el modelo de servicio
        /// </summary>
        /// <param name="vehicleTypeViewModels">Modelo vista</param>
        /// <returns>Modelo de servicio</returns>
        public static List<VehicleTypeServiceModel> CreateVehicleTypeServiceModel(List<VehicleTypeViewModel> vehicleTypeViewModels)
        {
            List<VehicleTypeServiceModel> viewModels = new List<VehicleTypeServiceModel>();
            foreach (VehicleTypeViewModel item in vehicleTypeViewModels)
            {
                viewModels.Add(CreateVehicleTypeServiceModel(item));
            }
            return viewModels;
        }

        /// <summary>
        /// Construye el modelo de servicio
        /// </summary>
        /// <param name="vehicleTypeViewModel">Modelo vista</param>
        /// <returns>Modelo de servicio</returns>
        public static VehicleTypeServiceModel CreateVehicleTypeServiceModel(VehicleTypeViewModel vehicleTypeViewModel)
        {
            VehicleTypeServiceModel serviceModel = new VehicleTypeServiceModel()
            {
                Description = vehicleTypeViewModel.Description,
                Id = vehicleTypeViewModel.TypeCode ?? 0,
                IsEnable = vehicleTypeViewModel.IsActive,
                IsTruck = vehicleTypeViewModel.IsTruck,
                SmallDescription = vehicleTypeViewModel.ShortDescription,
                StatusTypeService = (Application.ModelServices.Enums.StatusTypeService)vehicleTypeViewModel.State
            };
            if (vehicleTypeViewModel.VehicleBodies != null)
            {
                serviceModel.VehicleBodyServiceQueryModel = vehicleTypeViewModel.VehicleBodies.Select(p => new VehicleBodyServiceQueryModel()
                {
                    Id = p,
                    StatusTypeService = Application.ModelServices.Enums.StatusTypeService.Create
                }).ToList();
            }
            return serviceModel;
        }
        #endregion

        #region VehicleType-Previsora

        /// <summary>
        /// Crea el modelo de vista
        /// </summary>
        /// <param name="vehicleTypesDTO">Tipo de vehiculo</param>
        /// <returns>Listado de modelo de vista</returns>
        public static List<VehicleTypeViewModel> CreateVehicleTypeViewModelDTO(List<VehicleTypeDTO> vehicleTypes)
        {
            List<VehicleTypeViewModel> listViewModel = new List<VehicleTypeViewModel>();
            foreach (var viewModel in vehicleTypes)
            {
                listViewModel.Add(CreateVehicleTypeViewModelDTO(viewModel));
            }
            return listViewModel;
        }

        /// <summary>
        /// Crea el modelo de vista
        /// </summary>
        /// <param name="vehicleType">Tipo de vehiculo</param>
        /// <returns>Modelo de vista</returns>
        public static VehicleTypeViewModel CreateVehicleTypeViewModelDTO(VehicleTypeDTO vehicleType)
        {
            VehicleTypeViewModel viewModel = new VehicleTypeViewModel()
            {
                Description = vehicleType.Description,
                IsActive = vehicleType.IsActive,
                IsTruck = vehicleType.IsTruck,
                ShortDescription = vehicleType.SmallDescription,
                TypeCode = vehicleType.Id,
                State = (int)vehicleType.State,

            };
            viewModel.VehicleBodies = new List<int>();
            viewModel.VehicleBodies.AddRange(vehicleType.VehicleBodies.Select(p => p.Id ?? 0));
            return viewModel;
        }

        /// <summary>
        /// Construye el modelo de servicio
        /// </summary>
        /// <param name="vehicleTypeViewModelDTOs">Modelo vista</param>
        /// <returns>Modelo de servicio</returns>
        public static List<VehicleTypeDTO> CreateVehicleTypeServiceModelDTO(List<VehicleTypeViewModel> vehicleTypeViewModels)
        {
            List<VehicleTypeDTO> viewModels = new List<VehicleTypeDTO>();
            foreach (VehicleTypeViewModel item in vehicleTypeViewModels)
            {
                viewModels.Add(CreateVehicleTypeServiceModelDTO(item));
            }
            return viewModels;
        }

        /// <summary>
        /// Construye el modelo de servicio
        /// </summary>
        /// <param name="vehicleTypeViewModelDTOs">Modelo vista</param>
        /// <returns>Modelo de servicio</returns>
        public static VehicleTypeDTO CreateVehicleTypeServiceModelDTO(VehicleTypeViewModel vehicleTypeViewModel)
        {
            VehicleTypeDTO serviceModelDTO = new VehicleTypeDTO()
            {
                Description = vehicleTypeViewModel.Description,
                Id = vehicleTypeViewModel.TypeCode ?? 0,
                IsActive = vehicleTypeViewModel.IsActive,
                IsTruck = vehicleTypeViewModel.IsTruck,
                SmallDescription = vehicleTypeViewModel.ShortDescription,
                State = (int)(Application.ModelServices.Enums.StatusTypeService)vehicleTypeViewModel.State
            };
            if (vehicleTypeViewModel.VehicleBodies != null)
            {
                serviceModelDTO.VehicleBodies = vehicleTypeViewModel.VehicleBodies.Select(p => new VehicleBodyDTO()
                {
                    Id = p,
                    State = (int)Application.ModelServices.Enums.StatusTypeService.Create
                }).ToList();
            }
            return serviceModelDTO;
        }

        #endregion

        #region AlliancePrintFormat
        /// <summary>
        /// Método para mapear una lista de objetos de tipo alliancePrintFormatServiceModel a AlliancePrintFormatViewModel.
        /// </summary>
        /// <param name="alliancePrintFormatServiceModel">Modelo de formato de impresón de aliado.</param>
        /// <returns>lista de objetos de tipo AlliancePrintFormatViewModel.</returns>
        public static List<AlliancePrintFormatViewModel> GetAlliancePrintFormats(CptAlliancePrintFormatsServiceModel alliancePrintFormatServiceModel)
        {
            List<AlliancePrintFormatViewModel> alliancePrintFormatViewModelList = new List<AlliancePrintFormatViewModel>();
            foreach (CptAlliancePrintFormatServiceModel serviceModelCptAlliancePrintFormat in alliancePrintFormatServiceModel.CptAlliancePrintFormatServiceModel)
            {
                AlliancePrintFormatViewModel modeloViewCoveredRiskTypeViewModel = new AlliancePrintFormatViewModel();
                modeloViewCoveredRiskTypeViewModel.Id = serviceModelCptAlliancePrintFormat.Id;
                modeloViewCoveredRiskTypeViewModel.PrefixCd = serviceModelCptAlliancePrintFormat.PrefixServiceQueryModel.PrefixCode;
                modeloViewCoveredRiskTypeViewModel.EndoTypeCd = serviceModelCptAlliancePrintFormat.EndorsementTypeServiceQueryModel.Id;
                modeloViewCoveredRiskTypeViewModel.Format = serviceModelCptAlliancePrintFormat.Format;
                modeloViewCoveredRiskTypeViewModel.Enable = serviceModelCptAlliancePrintFormat.Enable;

                alliancePrintFormatViewModelList.Add(modeloViewCoveredRiskTypeViewModel);
            }
            return alliancePrintFormatViewModelList;
        }

        /// <summary>
        /// Método para mapear una lista de objetos de tipo AlliancePrintFormatViewModel a InsuredProfile.
        /// </summary>
        /// <param name="alliancePrintFormatViewModels">lista de objetos de tipo AlliancePrintFormatViewModel.</param>
        /// <returns>lista de objetos de tipo InsuredProfile.</returns>
        public static List<CptAlliancePrintFormatServiceModel> MappAlliancePrintFormatsToSave(List<AlliancePrintFormatViewModel> alliancePrintFormatViewModels, StatusTypeService statusTypeService)
        {
            if (alliancePrintFormatViewModels == null)
            {
                return null;
            }
            List<CptAlliancePrintFormatServiceModel> listCptAlliancePrintFormatServiceModel = new List<CptAlliancePrintFormatServiceModel>();
            foreach (AlliancePrintFormatViewModel model in alliancePrintFormatViewModels)
            {
                CptAlliancePrintFormatServiceModel cptAlliancePrintFormatServiceModel = new CptAlliancePrintFormatServiceModel();
                serviceModelsParamCore.ParametricServiceModel parametricServiceModel = new serviceModelsParamCore.ParametricServiceModel();
                serviceModelsParamCore.ErrorServiceModel errorServiceModel = new serviceModelsParamCore.ErrorServiceModel();
                PrefixServiceQueryModel prefixServiceQueryModel = new PrefixServiceQueryModel();
                EndorsementTypeServiceQueryModel endorsementTypeServiceQueryModel = new EndorsementTypeServiceQueryModel();



                cptAlliancePrintFormatServiceModel.Id = model.Id;

                prefixServiceQueryModel.PrefixCode = model.PrefixCd;
                cptAlliancePrintFormatServiceModel.PrefixServiceQueryModel = prefixServiceQueryModel;

                endorsementTypeServiceQueryModel.Id = model.EndoTypeCd;
                cptAlliancePrintFormatServiceModel.EndorsementTypeServiceQueryModel = endorsementTypeServiceQueryModel;

                cptAlliancePrintFormatServiceModel.Format = model.Format;
                cptAlliancePrintFormatServiceModel.Enable = model.Enable;

                errorServiceModel.ErrorDescription = new List<string>();
                errorServiceModel.ErrorTypeService = ErrorTypeService.Ok;

                parametricServiceModel.ErrorServiceModel = errorServiceModel;
                parametricServiceModel.StatusTypeService = statusTypeService;
                cptAlliancePrintFormatServiceModel.ParametricServiceModel = parametricServiceModel;

                listCptAlliancePrintFormatServiceModel.Add(cptAlliancePrintFormatServiceModel);
            }

            return listCptAlliancePrintFormatServiceModel;
        }

        #region Vehicle


        /// <summary>
        /// Método para mapear una lista de objetos de tipo Make a MakeViewModel.
        /// </summary>
        /// <param name="makesServiceModel"></param>
        /// <returns></returns>
        public static List<MakeViewModel> GetMakes(MakesServiceModel makesServiceModel)
        {
            List<MakeViewModel> makeViewModel = new List<MakeViewModel>();
            foreach (MakeServiceModel mMakeServiceModel in makesServiceModel.ListMakesServiceModel)
            {
                MakeViewModel vmMakeViewModel = new MakeViewModel();
                vmMakeViewModel.Description = mMakeServiceModel.Description;
                vmMakeViewModel.Id = mMakeServiceModel.Id;

                makeViewModel.Add(vmMakeViewModel);
            }
            return makeViewModel;
        }

        /// <summary>
        /// Método para mapear una lista de objetos de tipo Model a ModelViewModel.
        /// </summary>
        /// <param name="modelsServiceModel"></param>
        /// <returns></returns>
        public static List<ModelViewModel> GetModels(ModelsServiceModel modelsServiceModel)
        {
            List<ModelViewModel> modelViewModel = new List<ModelViewModel>();
            foreach (ModelServiceModel mModelServiceModel in modelsServiceModel.ListModelServiceModel)
            {
                ModelViewModel vmModelViewModel = new ModelViewModel();
                vmModelViewModel.Description = mModelServiceModel.Description;
                vmModelViewModel.Id = mModelServiceModel.Id;

                modelViewModel.Add(vmModelViewModel);
            }
            return modelViewModel;
        }

        public static List<VersionViewModel> GetVersions(VersionsServiceModel versionsServiceModel)
        {
            List<VersionViewModel> versionViewModel = new List<VersionViewModel>();
            foreach (VersionServiceModel mVersionServiceModel in versionsServiceModel.ListVersionServiceModel)
            {
                VersionViewModel vmVersionViewModel = new VersionViewModel();
                vmVersionViewModel.Description = mVersionServiceModel.Description;
                vmVersionViewModel.Id = mVersionServiceModel.Id;

                versionViewModel.Add(vmVersionViewModel);
            }
            return versionViewModel;
        }

        public static List<FasecoldaViewModel> GetVersionVehicleFasecolda(VersionVehicleFasecoldasServiceModel fasecoldasServiceModel)
        {
            List<FasecoldaViewModel> fasecoldaViewModel = new List<FasecoldaViewModel>();
            foreach (VersionVehicleFasecoldaServiceModel mVersionVehicleFasecoldaServiceModel in fasecoldasServiceModel.ListVersionVehicleFasecoldaModelService)
            {
                FasecoldaViewModel vmVersionVehicleFasecoldaViewModel = new FasecoldaViewModel();
                vmVersionVehicleFasecoldaViewModel.versionVehicle = new VersionViewModel();
                vmVersionVehicleFasecoldaViewModel.modelVehicle = new ModelViewModel();
                vmVersionVehicleFasecoldaViewModel.makeVehicle = new MakeViewModel();
                vmVersionVehicleFasecoldaViewModel.versionVehicle.Id = mVersionVehicleFasecoldaServiceModel.VersionId;
                //vmVersionVehicleFasecoldaViewModel.versionVehicle.Description = mVersionVehicleFasecoldaServiceModel.Version.Description;
                vmVersionVehicleFasecoldaViewModel.modelVehicle.Id = mVersionVehicleFasecoldaServiceModel.ModelId;
                //vmVersionVehicleFasecoldaViewModel.modelVehicle.Description = mVersionVehicleFasecoldaServiceModel.Model.Description;
                vmVersionVehicleFasecoldaViewModel.makeVehicle.Id = mVersionVehicleFasecoldaServiceModel.MakeId;
                //vmVersionVehicleFasecoldaViewModel.makeVehicle.Description = mVersionVehicleFasecoldaServiceModel.Make.Description;
                vmVersionVehicleFasecoldaViewModel.ModelVehicleCode = mVersionVehicleFasecoldaServiceModel.FasecoldaModelId;
                vmVersionVehicleFasecoldaViewModel.MakeVehicleCode = mVersionVehicleFasecoldaServiceModel.FasecoldaMakeId;



                fasecoldaViewModel.Add(vmVersionVehicleFasecoldaViewModel);
            }
            return fasecoldaViewModel;
        }

        public static List<FasecoldaViewModel> GetAllVersionVehicleFasecolda(FasecoldasServiceModel fasecoldasServiceModel)
        {
            List<FasecoldaViewModel> fasecoldaViewModel = new List<FasecoldaViewModel>();
            foreach (FasecoldaServiceModel mFasecoldaServiceModel in fasecoldasServiceModel.ListFasecoldaModelService)
            {
                FasecoldaViewModel vmVersionVehicleFasecoldaViewModel = new FasecoldaViewModel();
                vmVersionVehicleFasecoldaViewModel.versionVehicle = new VersionViewModel();
                vmVersionVehicleFasecoldaViewModel.modelVehicle = new ModelViewModel();
                vmVersionVehicleFasecoldaViewModel.makeVehicle = new MakeViewModel();
                vmVersionVehicleFasecoldaViewModel.versionVehicle.Id = mFasecoldaServiceModel.Version.Id;
                vmVersionVehicleFasecoldaViewModel.versionVehicle.Description = mFasecoldaServiceModel.Version.Description;
                vmVersionVehicleFasecoldaViewModel.modelVehicle.Id = mFasecoldaServiceModel.Model.Id;
                vmVersionVehicleFasecoldaViewModel.modelVehicle.Description = mFasecoldaServiceModel.Model.Description;
                vmVersionVehicleFasecoldaViewModel.makeVehicle.Id = mFasecoldaServiceModel.Make.Id;
                vmVersionVehicleFasecoldaViewModel.makeVehicle.Description = mFasecoldaServiceModel.Make.Description;
                vmVersionVehicleFasecoldaViewModel.ModelVehicleCode = mFasecoldaServiceModel.FasecoldaModelId;
                vmVersionVehicleFasecoldaViewModel.MakeVehicleCode = mFasecoldaServiceModel.FasecoldaMakeId;

                fasecoldaViewModel.Add(vmVersionVehicleFasecoldaViewModel);
            }
            return fasecoldaViewModel;
        }

        ///// <summary>
        ///// Método para mapear una lista de objetos de tipo AlliancePrintFormatViewModel a InsuredProfile.
        ///// </summary>
        ///// <param name="alliancePrintFormatViewModels">lista de objetos de tipo AlliancePrintFormatViewModel.</param>
        ///// <returns>lista de objetos de tipo InsuredProfile.</returns>


        /// <summary>
        /// Método para mapear una lista de objetos de tipo VersionVehicleFasecoldaServiceModel a FasecoldaViewModel.
        /// </summary>
        /// <param name="fasecoldaViewModels"></param>
        /// <param name="statusTypeService"></param>
        /// <returns></returns>
        public static List<VersionVehicleFasecoldaServiceModel> MappVersionVehicleFasecoldaServiceModelToSave(List<FasecoldaViewModel> fasecoldaViewModels, StatusTypeService statusTypeService)
        {
            if (fasecoldaViewModels == null)
            {
                return null;
            }

            List<VersionVehicleFasecoldaServiceModel> listVersionVehicleFasecoldaServiceModel = new List<VersionVehicleFasecoldaServiceModel>();
            foreach (FasecoldaViewModel model in fasecoldaViewModels)
            {
                VersionVehicleFasecoldaServiceModel versionVehicleFasecoldaServiceModel = new VersionVehicleFasecoldaServiceModel();
                ParametricServiceModel parametricServiceModel = new ParametricServiceModel();
                //PrefixServiceQueryModel prefixServiceQueryModel = new PrefixServiceQueryModel();
                //EndorsementTypeServiceQueryModel endorsementTypeServiceQueryModel = new EndorsementTypeServiceQueryModel();

                versionVehicleFasecoldaServiceModel.FasecoldaMakeId = model.MakeVehicleCode;
                versionVehicleFasecoldaServiceModel.FasecoldaModelId = model.ModelVehicleCode;
                versionVehicleFasecoldaServiceModel.MakeId = model.makeVehicle.Id;
                versionVehicleFasecoldaServiceModel.ModelId = model.modelVehicle.Id;
                versionVehicleFasecoldaServiceModel.VersionId = model.versionVehicle.Id;



                //parametricServiceModel.StatusTypeService = statusTypeService;

                //parametricServiceModel. = ErrorTypeService.Ok;

                //versionVehicleFasecoldaServiceModel.ParametricServiceModel = parametricServiceModel;



                listVersionVehicleFasecoldaServiceModel.Add(versionVehicleFasecoldaServiceModel);
            }

            return listVersionVehicleFasecoldaServiceModel;
        }


        /// <summary>
        /// Método para mapear una lista de objetos de tipo AlliancePrintFormatViewModel a InsuredProfile.
        /// </summary>
        /// <param name="alliancePrintFormatViewModels">lista de objetos de tipo AlliancePrintFormatViewModel.</param>
        /// <returns>lista de objetos de tipo InsuredProfile.</returns>
        public static List<CptAlliancePrintFormatServiceModel> MappAllAlliancePrintFormatsToSave(List<CptAlliancePrintFormatServiceModel> listAdded, List<CptAlliancePrintFormatServiceModel> listModified, List<CptAlliancePrintFormatServiceModel> listDeleted)
        {
            List<CptAlliancePrintFormatServiceModel> listToPersist = new List<CptAlliancePrintFormatServiceModel>();



            if (listAdded != null)
            {

                foreach (CptAlliancePrintFormatServiceModel item in listAdded)
                {
                    listToPersist.Add(item);
                }
            }
            if (listModified != null)
            {
                foreach (CptAlliancePrintFormatServiceModel item in listModified)
                {
                    listToPersist.Add(item);
                }
            }

            if (listDeleted != null)
            {
                foreach (CptAlliancePrintFormatServiceModel item in listDeleted)
                {
                    listToPersist.Add(item);
                }
            }
            return listToPersist;
        }





        public static List<VersionVehicleFasecoldaServiceModel> MappAllFasecoldaToSave(List<VersionVehicleFasecoldaServiceModel> listAdded, List<VersionVehicleFasecoldaServiceModel> listModified, List<VersionVehicleFasecoldaServiceModel> listDeleted)
        {
            List<VersionVehicleFasecoldaServiceModel> listToPersist = new List<VersionVehicleFasecoldaServiceModel>();
            if (listAdded != null)
            {
                foreach (VersionVehicleFasecoldaServiceModel item in listAdded)
                {
                    listToPersist.Add(item);
                }
            }
            if (listModified != null)
            {

                foreach (VersionVehicleFasecoldaServiceModel item in listModified)
                {
                    listToPersist.Add(item);
                }
            }

            if (listDeleted != null)
            {

                foreach (VersionVehicleFasecoldaServiceModel item in listDeleted)
                {
                    listToPersist.Add(item);
                }
            }
            return listToPersist;
        }
        #endregion
        #region CreateCoverageGroup
        public static List<CoverageGroupViewModel> CreateCoverageGroup(List<Application.EntityServices.Models.PostEntity> postEntities, List<CoveredRiskTypeViewModel> coveredRiskTypeViewModel)
        {
            return postEntities.Select(p => new CoverageGroupViewModel()
            {
                IdCoverGroupRisk = int.Parse(p.Fields.First(y => y.Name == "IdCoverGroupRisk").Value),
                CoverageGroupCode = int.Parse(p.Fields.First(y => y.Name == "CoverageGroupCode").Value),
                CoveredRiskTypeCode = int.Parse(p.Fields.First(y => y.Name == "CoveredRiskTypeCode").Value),
                Description = p.Fields.First(y => y.Name == "Description").Value,
                RiskTypeCoverageDescription = coveredRiskTypeViewModel.First(x => x.Id == int.Parse(p.Fields.First(y => y.Name == "CoveredRiskTypeCode").Value)).ShortDescription,
                SmallDescription = p.Fields.First(y => y.Name == "SmallDescription").Value,
                Enabled = bool.Parse(p.Fields.First(y => y.Name == "Enabled").Value),
                EnabledDescription = bool.Parse(p.Fields.First(y => y.Name == "Enabled").Value) == true ? "" : App_GlobalResources.Language.LabelIf,
                Status = Application.EntityServices.Enums.StatusTypeService.Original
            }).ToList();
        }

        public static List<CoverageGroupRiskTypeServiceModel> CreateCoverageGroups(List<CoverageGroupViewModel> coverageGroups)
        {
            if (coverageGroups == null)
                return null;
            return coverageGroups.Select(p => new CoverageGroupRiskTypeServiceModel()
            {
                Description = p.Description,
                SmallDescription = p.SmallDescription,
                CoverageRiskType = new CoveredRiskTypeServiceModel { Id = p.CoveredRiskTypeCode, SmallDescription = p.RiskTypeCoverageDescription },
                Enabled = p.Enabled,
                ErrorServiceModel = new Application.ModelServices.Models.Param.ErrorServiceModel { ErrorTypeService = Application.ModelServices.Enums.ErrorTypeService.Ok },
                StatusTypeService = Application.ModelServices.Enums.StatusTypeService.Original
            }).ToList();
        }
        #endregion
        #region Coberturas

        public static CoverageServiceModel CreateCoverage(CoverageViewModel coverageViewModel)
        {
            CoverageServiceModel coverageServiceModel = new CoverageServiceModel()
            {
                Id = coverageViewModel.Id,
                Description = coverageViewModel.Description,
                CompositionTypeId = coverageViewModel.CompositionTypeId,
                IsPrincipal = coverageViewModel.IsPrimary,
                StatusTypeService = coverageViewModel.Status
            };
            coverageServiceModel.LineBusiness = new LineBusinessServiceQueryModel()
            {
                Id = coverageViewModel.LineBusinessId
            };
            coverageServiceModel.SubLineBusiness = new SubLineBusinessServiceQueryModel()
            {
                Id = coverageViewModel.SubLineBusinessId
            };
            coverageServiceModel.Peril = new PerilServiceQueryModel()
            {
                Id = coverageViewModel.PerilId
            };
            coverageServiceModel.InsuredObject = new Application.ModelServices.Models.UnderwritingParam.InsuredObjectServiceQueryModel()
            {
                Id = coverageViewModel.InsuredObjectId
            };


            coverageServiceModel.StatusTypeService = coverageViewModel.Status;
            coverageServiceModel.CoCoverageServiceModel = new CoCoverageServiceModel()
            {
                Description = coverageViewModel.ImpressionName,
                IsAccMinPremium = coverageViewModel.IsAccMinPremium,
                IsAssistance = coverageViewModel.IsAssistance,
                IsImpression = coverageViewModel.IsImpression,
                StatusTypeService = Application.ModelServices.Enums.StatusTypeService.Original,
                ImpressionValue = coverageViewModel.ImpressionValue,
                IsSeriousOffer = coverageViewModel.IsSeriousOffer
            };

            coverageServiceModel.ClausesServiceQueryModel = new ClausesServiceQueryModel() { ClauseServiceModels = new List<ClauseServiceQueryModel>() };
            coverageServiceModel.DeductiblesServiceQueryModel = new DeductiblesServiceQueryModel() { DeductibleServiceQueryModels = new List<DeductibleServiceQueryModel>() };
            coverageServiceModel.DetailTypesServiceQueryModel = new DetailTypesServiceQueryModel() { DetailTypeServiceQueryModel = new List<DetailTypeServiceQueryModel>() };

            foreach (var item in coverageViewModel.Clauses ?? new List<PartialsInformationViewModel>())
            {
                coverageServiceModel.ClausesServiceQueryModel.ClauseServiceModels.Add(new ClauseServiceQueryModel
                {
                    Id = item.Id,
                    IsMandatory = item.IsMandatory
                });
            }

            foreach (var item in coverageViewModel.Deductibles ?? new List<PartialsInformationViewModel>())
            {
                coverageServiceModel.DeductiblesServiceQueryModel.DeductibleServiceQueryModels.Add(new DeductibleServiceQueryModel
                {
                    Id = item.Id,
                    IsMandatory = item.IsMandatory
                });
            }


            foreach (var item in coverageViewModel.DetailTypes ?? new List<PartialsInformationViewModel>())
            {
                coverageServiceModel.DetailTypesServiceQueryModel.DetailTypeServiceQueryModel.Add(new DetailTypeServiceQueryModel
                {
                    Id = item.Id,
                    IsMandatory = item.IsMandatory
                });
            }

            if (coverageViewModel.Homologation2G != null)
            {
                coverageServiceModel.Homologation2G = new Coverage2GServiceModel()
                {
                    Id = coverageViewModel.Homologation2G.CoverageId2G,
                    InsuredObjectId = coverageViewModel.Homologation2G.InsuredObject2G,
                    LineBusinessId = coverageViewModel.Homologation2G.LineBusiness2G,
                    SubLineBusinessId = coverageViewModel.Homologation2G.SubLineBusiness2G,
                    StatusTypeService = (StatusTypeService)coverageViewModel.Homologation2G.Status
                };
            }
            if (coverageViewModel.CoCoverageServiceModels != null)
            {
                foreach (var coverage in coverageViewModel.CoCoverageServiceModels)
                {
                    if (coverageServiceModel.CoCoverageServiceModels == null)
                    {
                        coverageServiceModel.CoCoverageServiceModels = new List<CoCoverageServiceModel>();
                    }
                    coverageServiceModel.CoCoverageServiceModels.Add(new CoCoverageServiceModel()
                    {
                        Id = coverage.Id,
                        Description = coverage.ImpressionNamePrint,
                        ImpressionValue = coverage.ImpressionValuePrint,
                        IsAccMinPremium = coverage.IsAccMinPremium,
                        IsAssistance = coverage.IsAssistance,
                        IsImpression = coverage.IsImpression,
                        StatusTypeService = (StatusTypeService)coverage.Status
                    });
                }
            }

            return coverageServiceModel;
        }
        #endregion

        /// <summary>
        /// Convierte modelos de servicio en modelos de vista.
        /// </summary>
        /// <param name="businessServiceModel">Lista de modelos de servicio.</param>
        /// <returns></returns>
        public static List<BusinessConfigurationViewModel> GetBusinessConfiguration(List<BusinessServiceModel> businessServiceModel)
        {
            List<BusinessConfigurationViewModel> listBusinessConfigurationViewModel = new List<BusinessConfigurationViewModel>();
            foreach (BusinessServiceModel itemBusinessServiceModel in businessServiceModel)
            {
                BusinessConfigurationViewModel businessConfigurationViewModel = new BusinessConfigurationViewModel();
                PrefixBusiness prefixBusiness = new PrefixBusiness();
                businessConfigurationViewModel.BusinessId = itemBusinessServiceModel.BusinessId;
                businessConfigurationViewModel.Description = itemBusinessServiceModel.Description;
                businessConfigurationViewModel.IsEnabled = itemBusinessServiceModel.IsEnabled;
                prefixBusiness.PrefixCode = itemBusinessServiceModel.PrefixCode.PrefixCode;
                prefixBusiness.PrefixDescription = itemBusinessServiceModel.PrefixCode.PrefixDescription;
                prefixBusiness.PrefixSmallDescription = itemBusinessServiceModel.PrefixCode.PrefixSmallDescription;
                businessConfigurationViewModel.PrefixCode = prefixBusiness;
                businessConfigurationViewModel.ListBusinessConfigurationQueryViewModel = new List<BusinessConfigurationQueryViewModel>();
                foreach (BusinessConfigurationServiceModel itemBusinessConfigurationServiceModel in itemBusinessServiceModel.BusinessConfiguration)
                {
                    BusinessConfigurationQueryViewModel businessConfigurationQueryViewModel = new BusinessConfigurationQueryViewModel();
                    businessConfigurationQueryViewModel.BusinessConfigurationId = itemBusinessConfigurationServiceModel.BusinessConfigurationId;
                    businessConfigurationQueryViewModel.BusinessId = itemBusinessConfigurationServiceModel.BusinessId;
                    if (itemBusinessConfigurationServiceModel.Request != null)
                    {
                        RequestBusinessViewModel requestBusinessViewModel = new RequestBusinessViewModel();
                        requestBusinessViewModel.RequestEndorsementId = itemBusinessConfigurationServiceModel.Request.RequestEndorsementId;
                        requestBusinessViewModel.RequestId = itemBusinessConfigurationServiceModel.Request.RequestId;
                        requestBusinessViewModel.ProductId = itemBusinessConfigurationServiceModel.Request.ProductId;
                        requestBusinessViewModel.PrefixCode = itemBusinessConfigurationServiceModel.Request.PrefixCode;
                        businessConfigurationQueryViewModel.Request = requestBusinessViewModel;
                    }
                    ProductBusinessViewModel productBusinessViewModel = new ProductBusinessViewModel();
                    productBusinessViewModel.ProductId = itemBusinessConfigurationServiceModel.Product.ProductId;
                    productBusinessViewModel.ProductDescription = itemBusinessConfigurationServiceModel.Product.ProductDescription;
                    productBusinessViewModel.ProductSmallDescription = itemBusinessConfigurationServiceModel.Product.ProductSmallDescription;
                    productBusinessViewModel.ActiveProduct = itemBusinessConfigurationServiceModel.Product.ActiveProduct;
                    businessConfigurationQueryViewModel.Product = productBusinessViewModel;
                    GroupCoverageBusinessViewModel groupCoverageBusinessViewModel = new GroupCoverageBusinessViewModel();
                    groupCoverageBusinessViewModel.GroupCoverageId = itemBusinessConfigurationServiceModel.GroupCoverage.GroupCoverageId;
                    groupCoverageBusinessViewModel.GroupCoverageSmallDescription = itemBusinessConfigurationServiceModel.GroupCoverage.GroupCoverageSmallDescription;
                    businessConfigurationQueryViewModel.GroupCoverage = groupCoverageBusinessViewModel;
                    AssistanceTypeViewModel assistanceTypeViewModel = new AssistanceTypeViewModel();
                    assistanceTypeViewModel.AssistanceCode = itemBusinessConfigurationServiceModel.Assistance.AssistanceCode;
                    assistanceTypeViewModel.AssistanceDescription = itemBusinessConfigurationServiceModel.Assistance.AssistanceDescription;
                    businessConfigurationQueryViewModel.Assistance = assistanceTypeViewModel;
                    businessConfigurationQueryViewModel.ProductIdResponse = itemBusinessConfigurationServiceModel.ProductIdResponse;
                    businessConfigurationQueryViewModel.StatusType = itemBusinessConfigurationServiceModel.StatusTypeService;
                    businessConfigurationViewModel.ListBusinessConfigurationQueryViewModel.Add(businessConfigurationQueryViewModel);
                }
                businessConfigurationViewModel.StatusType = itemBusinessServiceModel.StatusTypeService;
                listBusinessConfigurationViewModel.Add(businessConfigurationViewModel);
            }
            return listBusinessConfigurationViewModel;
        }

        /// <summary>
        /// Convierte modelos de vista en modelos de servicio.
        /// </summary>
        /// <param name="businessServiceModel">Lista de modelos de vista.</param>
        /// <returns></returns>
        public static List<BusinessServiceModel> CreateBusinessConfiguration(List<BusinessConfigurationViewModel> businessServiceModel)
        {
            List<BusinessServiceModel> listBusinessConfigurationViewModel = new List<BusinessServiceModel>();
            foreach (BusinessConfigurationViewModel itemBusinessServiceModel in businessServiceModel)
            {
                BusinessServiceModel businessConfigurationViewModel = new BusinessServiceModel();
                businessConfigurationViewModel.BusinessId = itemBusinessServiceModel.BusinessId;
                businessConfigurationViewModel.Description = itemBusinessServiceModel.Description;
                businessConfigurationViewModel.IsEnabled = itemBusinessServiceModel.IsEnabled;
                PrefixServiceQueryModel prefixServiceQueryModel = new PrefixServiceQueryModel();
                prefixServiceQueryModel.PrefixCode = itemBusinessServiceModel.PrefixCode.PrefixCode;
                prefixServiceQueryModel.PrefixDescription = itemBusinessServiceModel.PrefixCode.PrefixDescription;
                prefixServiceQueryModel.PrefixSmallDescription = itemBusinessServiceModel.PrefixCode.PrefixSmallDescription;
                businessConfigurationViewModel.PrefixCode = prefixServiceQueryModel;
                if (itemBusinessServiceModel.ListBusinessConfigurationQueryViewModel != null)
                {
                    List<BusinessConfigurationServiceModel> listBusinessConfigurationServiceModel = new List<BusinessConfigurationServiceModel>();
                    foreach (BusinessConfigurationQueryViewModel itemBusinessConfigurationServiceModel in itemBusinessServiceModel.ListBusinessConfigurationQueryViewModel)
                    {
                        BusinessConfigurationServiceModel businessConfigurationQueryViewModel = new BusinessConfigurationServiceModel();
                        businessConfigurationQueryViewModel.BusinessConfigurationId = itemBusinessConfigurationServiceModel.BusinessConfigurationId;
                        businessConfigurationQueryViewModel.BusinessId = itemBusinessConfigurationServiceModel.BusinessId;
                        if (itemBusinessConfigurationServiceModel.Request != null)
                        {
                            RequestEndorsementServiceQueryModel requestEndorsementServiceQueryModel = new RequestEndorsementServiceQueryModel();
                            requestEndorsementServiceQueryModel.RequestEndorsementId = itemBusinessConfigurationServiceModel.Request.RequestEndorsementId;
                            requestEndorsementServiceQueryModel.RequestId = itemBusinessConfigurationServiceModel.Request.RequestId;
                            requestEndorsementServiceQueryModel.ProductId = itemBusinessConfigurationServiceModel.Request.ProductId;
                            requestEndorsementServiceQueryModel.PrefixCode = itemBusinessConfigurationServiceModel.Request.PrefixCode;
                            businessConfigurationQueryViewModel.Request = requestEndorsementServiceQueryModel;
                        }
                        ProductServiceQueryModel productServiceQueryModel = new ProductServiceQueryModel();
                        productServiceQueryModel.ProductId = itemBusinessConfigurationServiceModel.Product.ProductId;
                        productServiceQueryModel.ProductDescription = itemBusinessConfigurationServiceModel.Product.ProductDescription;
                        productServiceQueryModel.ProductSmallDescription = itemBusinessConfigurationServiceModel.Product.ProductSmallDescription;
                        productServiceQueryModel.ActiveProduct = itemBusinessConfigurationServiceModel.Product.ActiveProduct;
                        businessConfigurationQueryViewModel.Product = productServiceQueryModel;
                        GroupCoverageServiceQueryModel groupCoverageServiceQueryModel = new GroupCoverageServiceQueryModel();
                        groupCoverageServiceQueryModel.GroupCoverageId = itemBusinessConfigurationServiceModel.GroupCoverage.GroupCoverageId;
                        groupCoverageServiceQueryModel.GroupCoverageSmallDescription = itemBusinessConfigurationServiceModel.GroupCoverage.GroupCoverageSmallDescription;
                        businessConfigurationQueryViewModel.GroupCoverage = groupCoverageServiceQueryModel;
                        AssistanceTypeServiceQueryModel assistanceTypeServiceQueryModel = new AssistanceTypeServiceQueryModel();
                        assistanceTypeServiceQueryModel.AssistanceCode = itemBusinessConfigurationServiceModel.Assistance.AssistanceCode;
                        assistanceTypeServiceQueryModel.AssistanceDescription = itemBusinessConfigurationServiceModel.Assistance.AssistanceDescription;
                        businessConfigurationQueryViewModel.Assistance = assistanceTypeServiceQueryModel;
                        businessConfigurationQueryViewModel.ProductIdResponse = itemBusinessConfigurationServiceModel.ProductIdResponse;
                        businessConfigurationQueryViewModel.StatusTypeService = itemBusinessConfigurationServiceModel.StatusType;
                        listBusinessConfigurationServiceModel.Add(businessConfigurationQueryViewModel);


                    }
                    businessConfigurationViewModel.BusinessConfiguration = listBusinessConfigurationServiceModel;
                }
                businessConfigurationViewModel.StatusTypeService = itemBusinessServiceModel.StatusType;
                listBusinessConfigurationViewModel.Add(businessConfigurationViewModel);
            }
            return listBusinessConfigurationViewModel;
        }
        #region Assistance Type

        //public static List<AssistanceTypeModel> GetAssistanceType(List<AssistanceType> Assistance)
        //{
        //    List<AssistanceTypeModel> assistanceTypeModel = new List<AssistanceTypeModel>();
        //    foreach (AssistanceType model in Assistance)
        //    {
        //        AssistanceTypeModel AssistanceTypeModel = new AssistanceTypeModel();
        //        AssistanceTypeModel.AssistanceCode = model.AssistanceCode;
        //        AssistanceTypeModel.Description = model.Description;
        //        AssistanceTypeModel.PrefixCode = model.PrefixCode;
        //        AssistanceTypeModel.Enabled = model.Enabled;
        //        AssistanceTypeModel.Status = model.Status;
        //        assistanceTypeModel.Add(AssistanceTypeModel);
        //    }

        //    return assistanceTypeModel;
        //}

        //public static List<AssistanceType> CreateAssistanceType(List<AssistanceTypeModel> Assistance)
        //{
        //    if (Assistance == null)
        //        return null;
        //    List<AssistanceType> assistanceType = new List<AssistanceType>();
        //    foreach (AssistanceTypeModel model in Assistance)
        //    {
        //        AssistanceType AssistanceType = new AssistanceType();
        //        AssistanceType.AssistanceCode = model.AssistanceCode;
        //        AssistanceType.Description = model.Description;
        //        AssistanceType.PrefixCode = model.PrefixCode;
        //        AssistanceType.Enabled = model.Enabled;
        //        AssistanceType.Prefix = model.Prefix;
        //        AssistanceType.Status = model.Status;

        //        assistanceType.Add(AssistanceType);
        //    }

        //    return assistanceType;
        //}

        #endregion

        //#region Assistance Text

        //public static List<AssistanceTextModel> GetAssistanceText(List<AssistanceText> Assistance)
        //{
        //    List<AssistanceTextModel> assistanceTextModel = new List<AssistanceTextModel>();
        //    foreach (AssistanceText model in Assistance)
        //    {
        //        AssistanceTextModel AssistanceTextModel = new AssistanceTextModel();
        //        AssistanceTextModel.AssistanceTextId = model.AssistanceTextId;
        //        AssistanceTextModel.ClauseCd3G = model.ClauseCd3G;
        //        AssistanceTextModel.ClauseCd2G = model.ClauseCd2G;
        //        AssistanceTextModel.PrefixCd = model.PrefixCd;
        //        AssistanceTextModel.AssistanceCd = model.AssistanceCd;
        //        AssistanceTextModel.Text = model.Text;
        //        AssistanceTextModel.Enable = model.Enable;

        //        assistanceTextModel.Add(AssistanceTextModel);
        //    }

        //    return assistanceTextModel;
        //}

        //public static List<AssistanceText> CreateAssistanceText(List<AssistanceTextModel> Assistance)
        //{
        //    if (Assistance == null)
        //        return null;
        //    List<AssistanceText> assistanceText = new List<AssistanceText>();
        //    foreach (AssistanceTextModel model in Assistance)
        //    {
        //        AssistanceText AssistanceText = new AssistanceText();
        //        AssistanceText.AssistanceTextId = model.AssistanceTextId;
        //        AssistanceText.ClauseCd3G = model.ClauseCd3G;
        //        AssistanceText.ClauseCd2G = model.ClauseCd2G;
        //        AssistanceText.PrefixCd = model.PrefixCd;
        //        AssistanceText.AssistanceCd = model.AssistanceCd;
        //        AssistanceText.Text = model.Text;
        //        AssistanceText.Enable = model.Enable;

        //        assistanceText.Add(AssistanceText);
        //    }

        //    return assistanceText;
        //}

        //#endregion

        #region Product 2G

        public static List<Product2GModel> GetProduct2g(List<Product2GModel> product2GMod)
        {
            List<Product2GModel> ltsProduct2GModel = new List<Product2GModel>();
            foreach (Product2GModel model in product2GMod)
            {
                Product2GModel product2GModel = new Product2GModel();
                product2GModel.ProductId = model.ProductId;
                product2GModel.PrefixCode = model.PrefixCode;
                product2GModel.Description = model.Description;

                ltsProduct2GModel.Add(product2GModel);
            }

            return ltsProduct2GModel;
        }

        public static List<Product2GModel> CreateProduct2g(List<Product2GModel> product2G)
        {
            if (product2G == null)
                return null;
            List<Product2GModel> LtsProduct2GMod = new List<Product2GModel>();
            foreach (Product2GModel model in product2G)
            {
                Product2GModel product2GMod = new Product2GModel();
                product2GMod.ProductId = model.ProductId;
                product2GMod.PrefixCode = model.PrefixCode;
                product2GMod.Description = model.Description;
                product2GMod.Status = model.Status;
                product2GMod.Prefix = model.Prefix;

                LtsProduct2GMod.Add(product2GMod);
            }

            return LtsProduct2GMod;
        }

        #endregion



        /// <summary>
        /// Construye el modelo de servicio
        /// </summary>
        /// <param name="fasecoldaViewModel">Modelo vista</param>
        /// <returns>Modelo de servicio</returns>
        public static List<VersionVehicleFasecoldaServiceModel> CreateVehicleFasecoldaServiceModel(List<FasecoldaViewModel> fasecoldaViewModel)
        {
            List<VersionVehicleFasecoldaServiceModel> viewModels = new List<VersionVehicleFasecoldaServiceModel>();
            foreach (FasecoldaViewModel item in fasecoldaViewModel)
            {
                viewModels.Add(CreateFasecoldaServiceModel(item));
            }
            return viewModels;
        }

        /// <summary>
        /// Construye el modelo de servicio
        /// </summary>
        /// <param name="fasecoldaViewModel">Modelo vista</param>
        /// <returns>Modelo de servicio</returns>
        public static VersionVehicleFasecoldaServiceModel CreateFasecoldaServiceModel(FasecoldaViewModel fasecoldaViewModel)
        {
            VersionVehicleFasecoldaServiceModel serviceModel = new VersionVehicleFasecoldaServiceModel();
            ParametricServiceModel parametricServiceModel = new ParametricServiceModel();







            serviceModel.FasecoldaMakeId = fasecoldaViewModel.MakeVehicleCode;
            serviceModel.FasecoldaModelId = fasecoldaViewModel.ModelVehicleCode;
            serviceModel.MakeId = fasecoldaViewModel.makeVehicle.Id;
            serviceModel.ModelId = fasecoldaViewModel.modelVehicle.Id;
            serviceModel.VersionId = fasecoldaViewModel.versionVehicle.Id;
            serviceModel.StatusTypeService = (Application.ModelServices.Enums.StatusTypeService)fasecoldaViewModel.State;

            //Application.ModelServices.Enums.StatusTypeService.Create;

            //if (fasecoldaViewModel.VehicleBodies != null)

            //{

            //    serviceModel.VehicleBodyServiceQueryModel = fasecoldaViewModel.VehicleBodies.Select(p => new VehicleBodyServiceQueryModel()

            //    {

            //        Id = p,

            //        StatusTypeService = Application.ModelServices.Enums.StatusTypeService.Create

            //    }).ToList();

            //}

            return serviceModel;
        }


        #endregion

        #region BusinessLineServiceModel
        /// <summary>
        /// Convierte View model a modelo de servicio de ramo técnico
        /// </summary>
        /// <param name="linebusinesModel">Ramo técnico</param>
        /// <param name="listInsuranceObjects">Objetos del seguro asignados</param>
        /// <param name="protectionAssigned">Amparos asignados</param>
        /// <param name="clausesAssigned">Cláusulas asignadas</param>
        /// <returns>Ramo técnico - MOD-S</returns>
        public static LineBusinessServiceModel CreateLineBusinessServiceModel(LineBusinessViewModel linebusinesModel)
        {
            List<CoveredRiskTypeServiceRelationModel> coveredRiskTypes = new List<CoveredRiskTypeServiceRelationModel>();

            if (linebusinesModel.CoveredRiskTypes.Count > 0)
            {
                foreach (var coveredRiskType in linebusinesModel.CoveredRiskTypes)
                {
                    CoveredRiskTypeServiceRelationModel newCoveredRiskType = new CoveredRiskTypeServiceRelationModel()
                    {
                        Id = coveredRiskType,
                        SmallDescription = " "
                    };
                    newCoveredRiskType.StatusTypeService = StatusTypeService.Original;
                    coveredRiskTypes.Add(newCoveredRiskType);
                }
            }

            LineBusinessServiceModel parametrizationLineBusiness = new LineBusinessServiceModel()
            {
                Id = linebusinesModel.Id,
                Description = linebusinesModel.LongDescription,
                SmallDescription = linebusinesModel.ShortDescription,
                TinyDescription = linebusinesModel.TyniDescription,
                CoveredRiskTypeServiceModel = coveredRiskTypes
            };

            return parametrizationLineBusiness;
        }

        public static List<InsuredObjectServiceModel> CreateInsuranceObject(List<InsurencesObjectsViewModel> insuranceObjects)
        {
            List<InsuredObjectServiceModel> listInsuredObject = new List<InsuredObjectServiceModel>();
            if (insuranceObjects.Count > 0)
            {
                foreach (InsurencesObjectsViewModel insuranceObject in insuranceObjects)
                {
                    InsuredObjectServiceModel insuredObjectServiceModel = new InsuredObjectServiceModel()
                    {
                        Id = insuranceObject.Id
                    };
                    insuredObjectServiceModel.StatusTypeService = StatusTypeService.Original;
                    listInsuredObject.Add(insuredObjectServiceModel);
                }
            }

            return listInsuredObject;
        }

        public static List<ClauseLevelServiceModel> CreateClause(List<LineBusinessClauseViewModel> clauses)
        {
            List<ClauseLevelServiceModel> listClause = new List<ClauseLevelServiceModel>();
            if (clauses.Count > 0)
            {
                foreach (LineBusinessClauseViewModel clause in clauses)
                {
                    ClauseLevelServiceModel newClause = new ClauseLevelServiceModel()
                    {
                        ClauseId = clause.Id,
                        IsMandatory = clause.Required
                    };
                    newClause.StatusTypeService = StatusTypeService.Original;
                    listClause.Add(newClause);
                }
            }

            return listClause;
        }

        public static List<PerilServiceModel> CreatePeril(List<ProtectionViewModel> perils)
        {
            List<PerilServiceModel> listPeril = new List<PerilServiceModel>();
            if (perils.Count > 0)
            {
                foreach (ProtectionViewModel protection in perils)
                {
                    PerilServiceModel peril = new PerilServiceModel()
                    {
                        Id = protection.Id
                    };
                    peril.StatusTypeService = StatusTypeService.Original;
                    listPeril.Add(peril);
                }
            }

            return listPeril;
        }
        #endregion

        #region VehicleBody
        /// <summary>
        /// Crea el modelo de vista
        /// </summary>
        /// <param name="vehicleBodies">Carrocería de vehículo</param>
        /// <returns>Listado de modelo de vista</returns>
        public static List<VehicleBodyViewModel> CreateVehicleBodyViewModel(VehicleBodiesServiceModel vehicleBodies)
        {
            List<VehicleBodyViewModel> listViewModel = new List<VehicleBodyViewModel>();
            foreach (VehicleBodyServiceModel viewModel in vehicleBodies.VehicleBodyServiceModel)
            {
                listViewModel.Add(CreateVehicleBodyViewModel(viewModel));
            }
            return listViewModel;
        }

        /// <summary>
        /// Crea el modelo de vista
        /// </summary>
        /// <param name="vehicleBody">Carrocería de vehículo</param>
        /// <returns>Modelo de vista</returns>
        public static VehicleBodyViewModel CreateVehicleBodyViewModel(VehicleBodyServiceModel vehicleBody)
        {
            VehicleBodyViewModel viewModel = new VehicleBodyViewModel()
            {
                //Description = vehicleBody.Description,
                //IsActive = vehicleBody.IsEnable,
                //IsTruck = vehicleBody.IsTruck,
                ShortDescription = vehicleBody.SmallDescription,
                BodyCode = vehicleBody.Id,
                State = (int)vehicleBody.StatusTypeService
            };
            viewModel.VehicleUses = new List<int>();
            viewModel.VehicleUses.AddRange(vehicleBody.VehicleUseServiceQueryModel.Select(p => p.Id));
            return viewModel;
        }

        /// <summary>
        /// Construye el modelo de servicio
        /// </summary>
        /// <param name="vehicleBodyViewModels">Modelo vista</param>
        /// <returns>Modelo de servicio</returns>
        public static List<VehicleBodyServiceModel> CreateVehicleBodyServiceModel(List<VehicleBodyViewModel> vehicleBodyViewModels)
        {
            List<VehicleBodyServiceModel> viewModels = new List<VehicleBodyServiceModel>();
            foreach (VehicleBodyViewModel item in vehicleBodyViewModels)
            {
                viewModels.Add(CreateVehicleBodyServiceModel(item));
            }
            return viewModels;
        }

        /// <summary>
        /// Construye el modelo de servicio
        /// </summary>
        /// <param name="vehicleBodyViewModel">Modelo vista</param>
        /// <returns>Modelo de servicio</returns>
        public static VehicleBodyServiceModel CreateVehicleBodyServiceModel(VehicleBodyViewModel vehicleBodyViewModel)
        {
            VehicleBodyServiceModel serviceModel = new VehicleBodyServiceModel()
            {
                //Description = vehicleBodyViewModel.Description,
                Id = vehicleBodyViewModel.BodyCode ?? 0,
                //IsEnable = vehicleBodyViewModel.IsActive,
                //IsTruck = vehicleBodyViewModel.IsTruck,
                SmallDescription = vehicleBodyViewModel.ShortDescription,
                StatusTypeService = (Application.ModelServices.Enums.StatusTypeService)vehicleBodyViewModel.State,
            };
            if (vehicleBodyViewModel.VehicleUses != null)
            {
                serviceModel.VehicleUseServiceQueryModel = vehicleBodyViewModel.VehicleUses.Select(p => new VehicleUseServiceQueryModel()
                {
                    Id = p,
                    StatusTypeService = Application.ModelServices.Enums.StatusTypeService.Create
                }).ToList();
            }
            return serviceModel;
        }



        #endregion

        #region Coverage2G
        public static List<CoverageHomologation2GViewModel> CreateCoverage2G(List<Coverage2GServiceModel> serviceModel)
        {
            List<CoverageHomologation2GViewModel> coverages = new List<CoverageHomologation2GViewModel>();
            foreach (Coverage2GServiceModel item in serviceModel)
            {
                coverages.Add(CreateCoverage2G(item));
            }
            return coverages;
        }

        public static CoverageHomologation2GViewModel CreateCoverage2G(Coverage2GServiceModel serviceModel)
        {
            return new CoverageHomologation2GViewModel()
            {
                CoverageId2G = serviceModel.Id,
                InsuredObject2G = serviceModel.InsuredObjectId,
                Description = serviceModel.Description,
                LineBusiness2G = serviceModel.LineBusinessId,
                SubLineBusiness2G = serviceModel.SubLineBusinessId
            };
        }
        #endregion

        #region FinancialPlan
        public static FinancialPlanServiceModel CreateFinancialPlan(FinancialPlanViewModel financialPlanViewModel)
        {
            var imapper = CreateMapFinancialPlan();
            FinancialPlanServiceModel financialPlanServiceModel = imapper.Map<FinancialPlanViewModel, FinancialPlanServiceModel>(financialPlanViewModel);
            //financialPlanServiceModel.Id = financialPlanViewModel.Id;
            financialPlanServiceModel.MinQuota = 0;
            financialPlanServiceModel.PaymentPlanServiceQueryModel = new PaymentPlanServiceQueryModel()
            {
                Id = financialPlanViewModel.IdPaymentPlan
            };
            financialPlanServiceModel.PaymentMethodServiceQueryModel = new PaymentMethodServiceQueryModel()
            {
                Id = financialPlanViewModel.IdPaymentMethod
            };
            financialPlanServiceModel.CurrencyServiceQueryModel = new CurrencyServiceQueryModel()
            {
                Id = financialPlanViewModel.IdCurrency
            };

            financialPlanServiceModel.StatusTypeService = financialPlanViewModel.StatusTypeService;

            financialPlanServiceModel.FirstPayComponentsServiceModel = new List<FirstPayComponentServiceModel>();

            foreach (var item in financialPlanViewModel.ListComponent ?? new List<PartialsInformationViewModel>())
            {
                financialPlanServiceModel.FirstPayComponentsServiceModel.Add(new FirstPayComponentServiceModel
                {
                    ComponentId = item.Id
                });
            }
            return financialPlanServiceModel;
        }


        #endregion

        #region Limit Rc


        /// <summary>
        /// 
        /// </summary>
        /// <param name="limitRcServiceModel"></param>
        /// <returns></returns>
        public static List<LimitRcViewModel> CreateLimitRc(List<LimitRcServiceModel> limitRcServiceModel)
        {
            List<LimitRcViewModel> limitRcModel = new List<LimitRcViewModel>();
            foreach (LimitRcServiceModel item in limitRcServiceModel)
            {
                LimitRcViewModel limitRc = new LimitRcViewModel
                {
                    LimitRcCd = item.LimitRcCd,
                    Limit1 = item.Limit1,
                    Limit2 = item.Limit2,
                    Limit3 = item.Limit3,
                    LimitUnique = item.LimitUnique,
                    Description = item.Description
                };

                limitRcModel.Add(limitRc);
            }
            return limitRcModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public static List<LimitRcServiceModel> CreateLimitsRc(List<LimitRcViewModel> models)
        {
            List<LimitRcServiceModel> limitRcServiceModel = new List<LimitRcServiceModel>();

            foreach (LimitRcViewModel item in models)
            {
                LimitRcServiceModel limitRc = new LimitRcServiceModel
                {
                    LimitRcCd = item.LimitRcCd,
                    Limit1 = item.Limit1,
                    Limit2 = item.Limit2,
                    Limit3 = item.Limit3,
                    LimitUnique = item.LimitUnique,
                    Description = item.Description,
                    StatusTypeService = Application.ModelServices.Enums.StatusTypeService.Original,
                    ErrorServiceModel = new Application.ModelServices.Models.Param.ErrorServiceModel { ErrorTypeService = Application.ModelServices.Enums.ErrorTypeService.BusinessFault }
                };
                limitRcServiceModel.Add(limitRc);
            }
            return limitRcServiceModel;
        }

        #endregion

        #region VehiculeVersion
        public static List<VehicleVersionViewModel> CreateVehicleVersionsServiceModel(VehicleVersionsServiceModel VehicleVersionsServiceModel)
        {
            List<VehicleVersionViewModel> Result = new List<VehicleVersionViewModel>();
            foreach (var item in VehicleVersionsServiceModel.VehicleVersionServiceModel)
            {
                Result.Add(CreateVehicleVersionViewModel(item));
            }
            return Result;
        }
        public static VehicleVersionViewModel CreateVehicleVersionViewModel(VehicleVersionServiceModel VehicleVersionsServiceModel)
        {
            return new VehicleVersionViewModel()
            {
                Currency = VehicleVersionsServiceModel.CurrencyServiceQueryModel == null ? null : (int?)VehicleVersionsServiceModel.CurrencyServiceQueryModel.Id,
                Description = VehicleVersionsServiceModel.Description,
                Id = VehicleVersionsServiceModel.Id,
                DescriptionMake = VehicleVersionsServiceModel.VehicleMakeServiceQueryModel.Description,
                DescriptionModel = VehicleVersionsServiceModel.VehicleModelServiceQueryModel.Description,
                DoorQuantity = VehicleVersionsServiceModel.DoorQuantity,
                EngineQuantity = VehicleVersionsServiceModel.EngineQuantity,
                HorsePower = VehicleVersionsServiceModel.HorsePower,
                IsImported = VehicleVersionsServiceModel.IsImported,
                LastModel = VehicleVersionsServiceModel.LastModel,
                MaxSpeedQuantity = VehicleVersionsServiceModel.MaxSpeedQuantity,
                PassengerQuantity = VehicleVersionsServiceModel.PassengerQuantity,
                Price = VehicleVersionsServiceModel.Price,
                TonsQuantity = VehicleVersionsServiceModel.TonsQuantity,
                VehicleBodyServiceQueryModel = VehicleVersionsServiceModel.VehicleBodyServiceQueryModel.Id,
                VehicleFuelServiceQueryModel = VehicleVersionsServiceModel.VehicleFuelServiceQueryModel == null ? null : (int?)VehicleVersionsServiceModel.VehicleFuelServiceQueryModel.Id,
                VehicleMakeServiceQueryModel = VehicleVersionsServiceModel.VehicleMakeServiceQueryModel.Id,
                VehicleModelServiceQueryModel = VehicleVersionsServiceModel.VehicleModelServiceQueryModel.Id,
                VehicleTransmissionTypeServiceQueryModel = VehicleVersionsServiceModel.VehicleTransmissionTypeServiceQueryModel == null ? null : (int?)VehicleVersionsServiceModel.VehicleTransmissionTypeServiceQueryModel.Id,
                Weight = VehicleVersionsServiceModel.Weight,
                VehicleTypeServiceQueryModel = VehicleVersionsServiceModel.VehicleTypeServiceQueryModel.Id
            };
        }
        public static VehicleVersionServiceModel CreateVehicleVersion(VehicleVersionViewModel VehicleVersionViewModel)
        {
            VehicleVersionServiceModel VehicleVersionServiceModel = new VehicleVersionServiceModel();
            VehicleVersionServiceModel.ErrorServiceModel = new serviceModelsParamCore.ErrorServiceModel() { ErrorDescription = new List<string>(), ErrorTypeService = ErrorTypeService.Ok };
            VehicleVersionServiceModel.Id = VehicleVersionViewModel.Id;
            VehicleVersionServiceModel.IsImported = VehicleVersionViewModel.IsImported;
            VehicleVersionServiceModel.LastModel = VehicleVersionViewModel.LastModel;
            VehicleVersionServiceModel.HorsePower = VehicleVersionViewModel.HorsePower;
            VehicleVersionServiceModel.Description = VehicleVersionViewModel.Description;
            VehicleVersionServiceModel.DoorQuantity = VehicleVersionViewModel.DoorQuantity;
            VehicleVersionServiceModel.EngineQuantity = VehicleVersionViewModel.EngineQuantity;
            VehicleVersionServiceModel.Weight = VehicleVersionViewModel.Weight;
            VehicleVersionServiceModel.PassengerQuantity = VehicleVersionViewModel.PassengerQuantity;
            if (VehicleVersionViewModel.Currency != null)
            {
                VehicleVersionServiceModel.CurrencyServiceQueryModel = new CurrencyServiceQueryModel() { Id = VehicleVersionViewModel.Currency.Value };
            }
            else
            {
                VehicleVersionServiceModel.CurrencyServiceQueryModel = new CurrencyServiceQueryModel();
            }
            VehicleVersionServiceModel.VehicleMakeServiceQueryModel = new VehicleMakeServiceQueryModel() { Id = VehicleVersionViewModel.VehicleMakeServiceQueryModel };
            VehicleVersionServiceModel.VehicleModelServiceQueryModel = new VehicleModelServiceQueryModel() { Id = VehicleVersionViewModel.VehicleModelServiceQueryModel };
            VehicleVersionServiceModel.VehicleBodyServiceQueryModel = new VehicleBodyServiceQueryModel() { StatusTypeService = StatusTypeService.Original, Id = VehicleVersionViewModel.VehicleBodyServiceQueryModel };
            VehicleVersionServiceModel.VehicleTypeServiceQueryModel = new VehicleTypeServiceQueryModel() { Id = VehicleVersionViewModel.VehicleTypeServiceQueryModel };
            VehicleVersionServiceModel.StatusTypeService = (StatusTypeService)VehicleVersionViewModel.StatusTypeService;
            if (VehicleVersionViewModel.VehicleTransmissionTypeServiceQueryModel != null)
            {
                VehicleVersionServiceModel.VehicleTransmissionTypeServiceQueryModel = new VehicleTransmissionTypeServiceQueryModel() { Id = (int)VehicleVersionViewModel.VehicleTransmissionTypeServiceQueryModel };
            }
            if (VehicleVersionViewModel.VehicleFuelServiceQueryModel != null)
            {
                VehicleVersionServiceModel.VehicleFuelServiceQueryModel = new VehicleFuelServiceQueryModel() { Id = (int)VehicleVersionViewModel.VehicleFuelServiceQueryModel };
            }
            VehicleVersionServiceModel.TonsQuantity = VehicleVersionViewModel.TonsQuantity;
            VehicleVersionServiceModel.Price = VehicleVersionViewModel.Price;
            VehicleVersionServiceModel.MaxSpeedQuantity = VehicleVersionViewModel.MaxSpeedQuantity;
            return VehicleVersionServiceModel;
        }
        #endregion

        #region Makemodel
        public static List<VehicleModelViewModel> CreateVehicleServiceModel(VehicleModelsServiceModel vehicleModelsServiceModel)
        {
            List<VehicleModelViewModel> Result = new List<VehicleModelViewModel>();
            if (vehicleModelsServiceModel.VehicleModelServiceModel != null)
            {
                foreach (var item in vehicleModelsServiceModel.VehicleModelServiceModel)
                {
                    Result.Add(CreateVehicleModelViewModel(item));
                }
            }
            return Result;
        }

        public static VehicleModelViewModel CreateVehicleModelViewModel(VehicleModelServiceModel vehicleModelViewModel)
        {
            return new VehicleModelViewModel()
            {
                Id = vehicleModelViewModel.Id,
                DescriptionModel = vehicleModelViewModel.Description == null ? string.Empty : vehicleModelViewModel.Description,
                SmallDescriptionModel = vehicleModelViewModel.SmallDescription == null ? string.Empty : vehicleModelViewModel.SmallDescription,
                MakeId_Id = vehicleModelViewModel.VehicelMakeServiceQueryModel.Id,
                VehicleMake = new VehicleMakeViewModel()
                {
                    id = vehicleModelViewModel.VehicelMakeServiceQueryModel.Id,
                    description = vehicleModelViewModel.VehicelMakeServiceQueryModel.Description == null ? string.Empty : vehicleModelViewModel.VehicelMakeServiceQueryModel.Description
                }
            };

        }

        public static VehicleModelServiceModel CreateVehicleModel(VehicleModelViewModel vehicleModelViewModel)
        {
            VehicleModelServiceModel vehicleModelServiceModel = new VehicleModelServiceModel();
            vehicleModelServiceModel.ErrorServiceModel = new serviceModelsParamCore.ErrorServiceModel() { ErrorDescription = new List<string>(), ErrorTypeService = ErrorTypeService.Ok };
            vehicleModelServiceModel.Id = vehicleModelViewModel.Id;
            vehicleModelServiceModel.Description = vehicleModelViewModel.DescriptionModel;
            vehicleModelServiceModel.SmallDescription = vehicleModelViewModel.SmallDescriptionModel;
            vehicleModelServiceModel.VehicelMakeServiceQueryModel = new Application.ModelServices.Models.VehicelMakeServiceQueryModel();
            vehicleModelServiceModel.VehicelMakeServiceQueryModel = new Application.ModelServices.Models.VehicelMakeServiceQueryModel()
            {



                Id = vehicleModelViewModel.MakeId_Id





            };
            vehicleModelServiceModel.StatusTypeService = vehicleModelViewModel.StatusTypeService;
            return vehicleModelServiceModel;
        }
        #endregion

        #region Technical Plan        
        public static List<CoverageViewModel> CreateCoverages(List<CoverageServiceModel> coverages)
        {
            List<CoverageViewModel> returnCoverages = new List<CoverageViewModel>();
            foreach (var coverage in coverages)
            {
                returnCoverages.Add(CreateCoverage(coverage));
            }
            return returnCoverages;
        }

        public static List<TechnicalPlanAllyCoverageViewModel> CreateAllyCoverages(List<AllyCoverageServiceModel> coverages)
        {
            List<TechnicalPlanAllyCoverageViewModel> returnCoverages = new List<TechnicalPlanAllyCoverageViewModel>();
            foreach (var coverage in coverages)
            {
                returnCoverages.Add(CreateAllyCoverage(coverage));
            }
            return returnCoverages;
        }

        public static TechnicalPlanAllyCoverageViewModel CreateAllyCoverage(AllyCoverageServiceModel coverageServiceModel)
        {
            return new TechnicalPlanAllyCoverageViewModel()
            {
                Id = coverageServiceModel.Id,
                Description = coverageServiceModel.Description,
                CoveragePercentage = coverageServiceModel.AlliedCoveragePercentage
            };
        }

        public static CoverageViewModel CreateCoverage(CoverageServiceModel coverageServiceModel)
        {
            var imapper = CreateMapCoverage();
            CoverageViewModel coverageViewModel = imapper.Map<CoverageServiceModel, CoverageViewModel>(coverageServiceModel);

            coverageViewModel.IsPrimary = coverageServiceModel.IsPrincipal;
            coverageViewModel.LineBusinessId = coverageServiceModel.LineBusiness.Id;
            coverageViewModel.SubLineBusinessId = coverageServiceModel.SubLineBusiness.Id;
            coverageViewModel.PerilId = coverageServiceModel.Peril.Id;
            coverageViewModel.InsuredObjectId = coverageServiceModel.InsuredObject.Id;
            coverageViewModel.Status = coverageServiceModel.StatusTypeService;
            if (coverageServiceModel.CoCoverageServiceModel != null)
            {
                coverageViewModel.ImpressionName = coverageServiceModel.CoCoverageServiceModel.Description == null ? "" : coverageServiceModel.CoCoverageServiceModel.Description;
                coverageViewModel.IsAccMinPremium = coverageServiceModel.CoCoverageServiceModel.IsAccMinPremium;
                coverageViewModel.IsAssistance = coverageServiceModel.CoCoverageServiceModel.IsAssistance;
                coverageViewModel.IsImpression = coverageServiceModel.CoCoverageServiceModel.IsImpression;
                coverageViewModel.ImpressionValue = coverageServiceModel.CoCoverageServiceModel.ImpressionValue == null ? "" : coverageServiceModel.CoCoverageServiceModel.ImpressionValue;
            }

            coverageViewModel.Clauses = new List<PartialsInformationViewModel>();
            coverageViewModel.Deductibles = new List<PartialsInformationViewModel>();
            coverageViewModel.DetailTypes = new List<PartialsInformationViewModel>();

            if (coverageServiceModel.ClausesServiceQueryModel == null)
            {
                coverageServiceModel.ClausesServiceQueryModel = new ClausesServiceQueryModel();
                coverageServiceModel.ClausesServiceQueryModel.ClauseServiceModels = new List<ClauseServiceQueryModel>();
            }
            foreach (var item in coverageServiceModel.ClausesServiceQueryModel.ClauseServiceModels)
            {
                coverageViewModel.Clauses.Add(new PartialsInformationViewModel()
                {
                    Id = item.Id,
                    IsMandatory = item.IsMandatory
                });
            }

            if (coverageServiceModel.DeductiblesServiceQueryModel == null)
            {
                coverageServiceModel.DeductiblesServiceQueryModel = new DeductiblesServiceQueryModel();
                coverageServiceModel.DeductiblesServiceQueryModel.DeductibleServiceQueryModels = new List<DeductibleServiceQueryModel>();
            }
            foreach (var item in coverageServiceModel.DeductiblesServiceQueryModel.DeductibleServiceQueryModels)
            {
                coverageViewModel.Deductibles.Add(new PartialsInformationViewModel()
                {
                    Id = item.Id,
                    IsMandatory = item.IsMandatory
                });
            }

            if (coverageServiceModel.DetailTypesServiceQueryModel == null)
            {
                coverageServiceModel.DetailTypesServiceQueryModel = new DetailTypesServiceQueryModel();
                coverageServiceModel.DetailTypesServiceQueryModel.DetailTypeServiceQueryModel = new List<DetailTypeServiceQueryModel>();
            }
            foreach (var item in coverageServiceModel.DetailTypesServiceQueryModel.DetailTypeServiceQueryModel ?? new List<DetailTypeServiceQueryModel>())
            {
                coverageViewModel.DetailTypes.Add(new PartialsInformationViewModel()
                {
                    Id = item.Id,
                    IsMandatory = item.IsMandatory
                });
            }
            return coverageViewModel;
        }

        public static TechnicalPlanServiceModel CreateTechnicalPlan(TechnicalPlanViewModel technicalPlanViewModel)
        {
            TechnicalPlanServiceModel returnData = new TechnicalPlanServiceModel();

            returnData.Id = technicalPlanViewModel.Id;
            returnData.Description = technicalPlanViewModel.Description;
            returnData.SmallDescription = technicalPlanViewModel.ShortDescription;
            returnData.CoveredRiskType = new CoveredRiskTypeServiceQueryModel()
            {
                Id = technicalPlanViewModel.RiskTypeId,
                SmallDescription = technicalPlanViewModel.RiskTypeSmallDescription
            };
            if (technicalPlanViewModel.CurrentFrom <= System.DateTime.MinValue)
            {
                returnData.CurrentFrom = System.DateTime.Now;
            }
            else
            {
                returnData.CurrentFrom = technicalPlanViewModel.CurrentFrom;
            }
            returnData.CurrentTo = null;
            returnData.StatusTypeService = (StatusTypeService)technicalPlanViewModel.Status;
            returnData.TechnicalPlanCoverages = new List<TechnicalPlanCoverageServiceRelationModel>();

            if (technicalPlanViewModel.Coverages != null)
            {
                foreach (var coverage in technicalPlanViewModel.Coverages)
                {
                    CoverageServiceQueryModel coverageObjModel = new CoverageServiceQueryModel()
                    {
                        Id = coverage.CoverageId,
                        Description = coverage.CoverageDescription
                    };
                    Application.ModelServices.Models.UnderwritingParam.InsuredObjectServiceQueryModel insuredObjModel = new Application.ModelServices.Models.UnderwritingParam.InsuredObjectServiceQueryModel()
                    {
                        Id = coverage.InsuredObjectId,
                        Description = coverage.InsuredObjectDescription
                    };
                    CoverageServiceQueryModel principalCoverageObjModel = null;
                    if (coverage.PrincipalCoverageId > 0)
                    {
                        principalCoverageObjModel = new CoverageServiceQueryModel()
                        {
                            Id = coverage.PrincipalCoverageId
                        };
                    }
                    List<AllyCoverageServiceModel> alliedCoverages = new List<AllyCoverageServiceModel>();
                    if (coverage.AllyCoverages != null)
                    {
                        foreach (var allyCoverage in coverage.AllyCoverages)
                        {
                            alliedCoverages.Add(new AllyCoverageServiceModel()
                            {
                                Id = allyCoverage.Id,
                                Description = allyCoverage.Description,
                                AlliedCoveragePercentage = allyCoverage.CoveragePercentage,
                                StatusTypeService = (StatusTypeService)allyCoverage.Status
                            });
                        }
                    }
                    TechnicalPlanCoverageServiceRelationModel coverageModel = new TechnicalPlanCoverageServiceRelationModel()
                    {
                        Coverage = coverageObjModel,
                        InsuredObject = insuredObjModel,
                        PrincipalCoverage = principalCoverageObjModel,
                        CoveragePercentage = coverage.CoveragePercentage,
                        StatusTypeService = (StatusTypeService)coverage.Status,
                        AlliedCoverages = alliedCoverages
                    };
                    returnData.TechnicalPlanCoverages.Add(coverageModel);
                }
            }
            return returnData;
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
            id = item.Id,
            description = item.Description,
            GroupPolicies = new GenericModelServicesQueryModel { id = item.GroupPolicies.Id }
        };
        #endregion

        #region AllyCoverage
        public static List<AllyCoverageViewModel> CreateAllyCoverage(List<Dictionary<string, string>> results)
        {
            return results.Select(p => new AllyCoverageViewModel()
            {
                AllyCoverageId = int.Parse(p.First(y => y.Key == "AllyCoverageId").Value),
                CoverageId = int.Parse(p.First(y => y.Key == "CoverageId").Value),
                CoveragePct = Convert.ToDecimal(p.First(y => y.Key == "CoveragePercentage").Value.Replace(",", ".")
                    .ToString(
                        new NumberFormatInfo { NumberDecimalSeparator = "." }), CultureInfo.InvariantCulture)
            }).ToList();
        }

        public static List<CoverageQueryViewModel> CreateAllyCoveragePrincipal(List<Dictionary<string, string>> results)
        {
            
            return results.Select(p => new CoverageQueryViewModel()
            {
                Id = int.Parse(p.First(y => y.Key == "CoverageId").Value),
                PerilId = int.Parse(p.First(x => x.Key == "PerilCode").Value),
                SubLineBusinessId = int.Parse(p.First(x => x.Key == "SubLineBusinessCode").Value),
                LineBusinessId = int.Parse(p.First(x => x.Key == "LineBusinessCode").Value),
                InsuredObjectId = int.Parse(p.First(x => x.Key == "InsuredObjectId").Value),
                PrintDescription = p.First(x => x.Key == "PrintDescription").Value.ToString(),
                IsPrimary = bool.Parse(p.First(x => x.Key == "IsPrimary").Value),
                ExpirationDate = string.IsNullOrEmpty(p.First(x => x.Key == "ExpirationDate").Value) ? new DateTime?() : Convert.ToDateTime(p.First(x => x.Key == "ExpirationDate").Value.Split(' ')[0]),
                CompositionTypeId = string.IsNullOrEmpty(p.First(x => x.Key == "CompositionTypeCode").Value) ? new int?() : int.Parse(p.First(x => x.Key == "CompositionTypeCode").Value), //new int?(int.Parse(p.First(x => x.Key == "CompositionTypeCode").Value)) ?? 0;
                RuleSetId = string.IsNullOrEmpty(p.First(x => x.Key == "RuleSetId").Value) ? new int?() : int.Parse(p.First(x => x.Key == "RuleSetId").Value)

            }).ToList();
            
        }

        /// <summary>
        /// MappAllyCoverageApplication. metodo que mapea los datos de AllyCoverageViewModel a AllyCoverageDTO para crud
        /// </summary>
        /// <param name="coverageViewModel"></param>
        /// <returns></returns>
        public static AllyCoverageDTO MappAllyCoverageApplication(AllyCoverageViewModel coverageViewModel)
        {
            AllyCoverageDTO ally_coverageDTO = new AllyCoverageDTO();
            ally_coverageDTO.CoverageId = coverageViewModel.CoverageId;
            ally_coverageDTO.AllyCoverageId = coverageViewModel.AllyCoverageId;//new CoverageDTO { Id = coverageViewModel.Coverage.Id, Description = coverageViewModel.Coverage.Description };
            ally_coverageDTO.CoveragePct = int.Parse(coverageViewModel.CoveragePct.ToString());//new PrefixDTO { Id = coverageViewModel.Prefix.Id, Description = coverageViewModel.Prefix.Description };

            return ally_coverageDTO;
        }

        /// <summary>
        /// MappAllyCoverageApplication. metodo que mapea los datos de AllyCoverageViewModel a AllyCoverageDTO para crud
        /// </summary>
        /// <param name="coverageViewModel"></param>
        /// <returns></returns>
        public static List<QueryAllyCoverageDTO> MappQueryAllyCoverageApplication(List<AllyCoverageViewModel> coverageViewModel)
        {
            var ally_coverageDTO = new List<QueryAllyCoverageDTO>();
            coverageViewModel.ForEach((x) =>
            {
                ally_coverageDTO.Add(new QueryAllyCoverageDTO {
                    CoverageId = MappingObjectQueyCoverage(x.CoverageId_object),
                    AllyCoverageId = MappingObjectQueyCoverage(x.AllyCoverageId_object),
                    CoveragePct = x.CoveragePct
                });
            });
            //ally_coverageDTO.CoverageId = coverageViewModel.CoverageId;
            //ally_coverageDTO.AllyCoverageId = coverageViewModel.AllyCoverageId;//new CoverageDTO { Id = coverageViewModel.Coverage.Id, Description = coverageViewModel.Coverage.Description };
            //ally_coverageDTO.CoveragePct = int.Parse(coverageViewModel.CoveragePct.ToString());//new PrefixDTO { Id = coverageViewModel.Prefix.Id, Description = coverageViewModel.Prefix.Description };

            return ally_coverageDTO;
        }

        private static QueryCoverageDTO MappingObjectQueyCoverage(CoverageQueryViewModel allyCoverageId_object)
        {
            return new QueryCoverageDTO
            {
                Id = allyCoverageId_object.Id,
                PerilId = allyCoverageId_object.PerilId,
                SubLineBusinessId = allyCoverageId_object.SubLineBusinessId,
                LineBusinessId = allyCoverageId_object.LineBusinessId,
                InsuredObjectId = allyCoverageId_object.InsuredObjectId,
                PrintDescription = allyCoverageId_object.PrintDescription,
                IsPrimary = allyCoverageId_object.IsPrimary,
                ExpirationDate = allyCoverageId_object.ExpirationDate,
                CompositionTypeId = allyCoverageId_object.CompositionTypeId,
                RuleSetId = allyCoverageId_object.RuleSetId
            };
        }

        #endregion

        #region BillingPeriod
        /// <summary>
        /// Método para mapear una lista de objetos de tipo BillingPeriod a BillingPeriodViewModel.
        /// </summary>
        /// <param name="BillingPeriodServiceModel">lista de objetos de tipo Periodos de facturación.</param>
        /// <returns>lista de objetos de tipo BillingPeriodViewModelViewModel.</returns>
        public static List<BillingPeriodViewModel> GetBillingPeriod(BillingPeriodQueryDTO BillingPeriodServiceModel)
        {
            List<BillingPeriodViewModel> BillingPeriodModelView = new List<BillingPeriodViewModel>();

            foreach (BillingPeriodDTO mBillingPeriodServiceModel in BillingPeriodServiceModel.BillingPeriodQueryDTOs)
            {
                BillingPeriodViewModel vmBillingPeriodViewModel = new BillingPeriodViewModel();
                vmBillingPeriodViewModel.DESCRIPTION = mBillingPeriodServiceModel.Description;
                vmBillingPeriodViewModel.BILLING_PERIOD_CD = mBillingPeriodServiceModel.BILLING_PERIOD_CD;

                BillingPeriodModelView.Add(vmBillingPeriodViewModel);
            }
            return BillingPeriodModelView;
        }

        #endregion

        #region BusinessType
        /// <summary>
        /// Método para mapear una lista de objetos de tipo BillingPeriod a BillingPeriodViewModel.
        /// </summary>
        /// <param name="BusinessTypeServiceModel">lista de objetos de tipo Periodos de facturación.</param>
        /// <returns>lista de objetos de tipo BusinessTypeViewModelViewModel.</returns>
        public static List<BusinessTypeViewModel> GetBusinessType(BusinessTypeQueryDTO BusinessTypeServiceModel)
        {
            List<BusinessTypeViewModel> BusinessTypeModelView = new List<BusinessTypeViewModel>();

            foreach (BusinessTypeDTO mBusinessTypeServiceModel in BusinessTypeServiceModel.BusinessTypeQueryDTOs)
            {
                BusinessTypeViewModel vmBusinessTypeViewModel = new BusinessTypeViewModel();
                vmBusinessTypeViewModel.SMALL_DESCRIPTION = mBusinessTypeServiceModel.SMALL_DESCRIPTION;
                vmBusinessTypeViewModel.BUSINESS_TYPE_CD = mBusinessTypeServiceModel.BUSINESS_TYPE_CD;

                BusinessTypeModelView.Add(vmBusinessTypeViewModel);
            }
            return BusinessTypeModelView;
        }

        #endregion

        #region MinPremiunRelation
        /// <summary>
        /// Carga con combo generico para paises  
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        public static List<GenericViewModel> CreateEntities(List<Dictionary<string, string>> results, string entity)
        {
            if (entity.Contains(".Product"))
                return CreateProductEntity(results);

            var result = results.Select(p => new GenericViewModel()
            {
                Id = int.Parse(p.First(y => y.Key.Contains("Code")).Value),
                DescriptionLong = p.First(y => y.Key == "Description").Value,
                DescriptionShort = (p.Keys.Contains("SmallDescription") ? p.First(y => y.Key == "SmallDescription").Value : p.First(y => y.Key == "Description").Value)
            }).ToList();
            return result;
        }

        public static List<GenericViewModel> CreateProductEntity(List<Dictionary<string, string>> results) 
        {
            var result = results.Select(p => new GenericViewModel()
            {
                Id = int.Parse(p.First(y => y.Key.Contains("ProductId")).Value),
                DescriptionLong = p.First(y => y.Key == "Description").Value,
                DescriptionShort = (p.Keys.Contains("SmallDescription") ? p.First(y => y.Key == "SmallDescription").Value : p.First(y => y.Key == "Description").Value)
            }).ToList();
            return result;
        }
        public static MinPremiunRelationDTO MappMinPremiunRelationityViewModelToApplication(MinPremiunRelationViewModel viewModel)
        {
            var dto = new MinPremiunRelationDTO();
            if (viewModel.Prefix != null)
            {
                dto.Prefix = new PrefixDTO { Id = viewModel.Prefix.Id, Description = viewModel.Prefix.Description };
            }
            if (viewModel.Branch != null)
            {
                dto.Branch = new BranchDTO { Id = viewModel.Branch.Id, Description = viewModel.Branch.LongDescription };
            }
            if (viewModel.EndorsementType != null)
            {//TODO: Prepara nombre de EndosermentType po EndorsementType en el DTO
                dto.EndorsementType = new EndorsementTypeDTO { Id = viewModel.EndorsementType.Id, Description = viewModel.EndorsementType.Description };
            }
            if (viewModel.Currency != null)
            {
                dto.Currency = new CurrencyDTO { Id = viewModel.Currency.Id, Description = viewModel.Currency.Description };
            }
            if (viewModel.Product != null)
            {
                dto.Product = new ProductDTO { Id = viewModel.Product.Id, Description = viewModel.Product.Description };
            }
            if (viewModel.Clave != null)
            {
                dto.MinPremiunRange = new MinPremiunRangeDTO { Id = viewModel.Clave.Id, Description = viewModel.Clave.Description };
                //if (dto.Prefix.Description.Equals("CUMPLIMIENTO"))
                //{
                //    dto.MinPremiunRange = new MinPremiunRangeDTO { Id = viewModel.Clave.Id, Description = viewModel.Clave.Description };
                //}
                //else
                //{
                //    dto.MinPremiunRange = null;
                //}

                if (dto.Prefix.Description.Contains("AUTOMOVILES"))
                {
                    dto.GroupCoverage = new GroupCoverageDTO { Id = viewModel.Clave.Id, Description = viewModel.Clave.Description };
                }
                else
                {
                    dto.GroupCoverage = null;
                }
            }
            dto.RiskMinPremiun = viewModel.MiniumPremiunValue;
            dto.SubMinPremiun = viewModel.MiniumSubValue;
            dto.Id = viewModel.Id;

            return dto;
        }
        public static MinPremiunRelationViewModel MappMinPremiunRelationityDtoToViewModel(MinPremiunRelationDTO dto)
        {
            var viewModel = new MinPremiunRelationViewModel();
            if (dto.Prefix != null)
            {
                viewModel.Prefix = new PrefixViewModel { Id = dto.Prefix.Id, Description = dto.Prefix.Description };
            }
            if (dto.Branch != null)
            {
                viewModel.Branch = new BranchViewModel { Id = dto.Branch.Id, LongDescription = dto.Branch.Description };
                if (viewModel.Branch.Id == 0)
                {
                    viewModel.Branch.LongDescription = "Todas las sucursales";
                }
            }
            //TODO: Prepara nombre de EndosermentType po EndorsementType en el DTO
            if (dto.EndorsementType != null)
            {
                viewModel.EndorsementType = new EndoTypeViewModel { Id = dto.EndorsementType.Id, Description = dto.EndorsementType.Description };
            }
            if (dto.Currency != null)
            {
                viewModel.Currency = new CurrencyViewModel { Id = dto.Currency.Id, Description = dto.Currency.Description };
            }
            if (dto.Product != null)
            {
                viewModel.Product = new ProductViewModel { Id = dto.Product.Id, Description = dto.Product.Description };
            }/*
            if (dto.Clave != null)
            {                
                viewModel.GroupCoverage = new GroupCoverageDTO { Id = viewModel.Clave.Id, Description = viewModel.Clave.Description };
                viewModel.MinPremiunRange = new MinPremiunRangeDTO { Id = viewModel.Clave.Id, Description = viewModel.Clave.Description };
            }*/
            if(dto.MinPremiunRange != null)
            {
                viewModel.Clave = new ClaveViewModel { Id = dto.MinPremiunRange.Id, Description = dto.MinPremiunRange.Description };
            }

            viewModel.MiniumPremiunValue = dto.RiskMinPremiun;
            viewModel.MiniumSubValue = dto.SubMinPremiun;
            viewModel.Id = dto.Id;

            return viewModel;
        }

        #endregion

        #region cities
        /// <summary>
        /// Metodo para mapear de entity a modelo generico para carga de combos de estados: tabla comm.city
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        public static List<GenericViewModel> CreateCities(List<Dictionary<string, string>> results)
        {
            return results.Select(p => new GenericViewModel()
            {
                Id = int.Parse(p.First(y => y.Key == "CountryCode").Value),
                //IdC = int.Parse(p.First(y => y.Key == "CityCode").Value),
                //IdD = int.Parse(p.First(y => y.Key == "StateCode").Value),
                DescriptionLong = p.First(y => y.Key == "Description").Value,
                DescriptionShort = p.First(y => y.Key == "SmallDescription").Value
            }).ToList();
        }

        /// <summary>
        /// Metodo para mapear de entity a modelo generico para carga de combos de estados: tabla comm.States
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        public static List<GenericViewModel> CreateStates(List<Dictionary<string, string>> results)
        {
            return results.Select(p => new GenericViewModel()
            {
                Id = int.Parse(p.First(y => y.Key == "StateCode").Value),
                DescriptionLong = p.First(y => y.Key == "Description").Value,
                DescriptionShort = p.First(y => y.Key == "SmallDescription").Value,
                IdC = int.Parse(p.First(y => y.Key == "CountryCode").Value)

            }).ToList();
        }


        /// <summary>
        /// Metodo para mapear de entity a modelo generico para carga de combos de estados: tabla comm.States
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        public static List<GenericViewModel> CreateCountries(List<Dictionary<string, string>> results)
        {
            return results.Select(p => new GenericViewModel()
            {
                Id = int.Parse(p.First(y => y.Key == "CountryCode").Value),
                DescriptionLong = p.First(y => y.Key == "Description").Value,
            }).ToList();
        }

        /// <summary>
        /// Metodo para mapear de VM a DTO. cityViewModel a CityDTO
        /// </summary>
        /// <param name="cityViewModel"></param>
        /// <returns></returns>
        public static CityDTO MappCityVMApplication(CityViewModel cityViewModel)
        {
            CityDTO cityDTO = new CityDTO();
            cityDTO.Id = cityViewModel.Id;
            cityDTO.Country = new CountryDTO { Id = cityViewModel.Country.Id, Description = cityViewModel.Country.Description };
            cityDTO.State = new StateDTO { Id = cityViewModel.State.Id, Description = cityViewModel.State.Description };
            cityDTO.Description = cityViewModel.Description;
            cityDTO.SmallDescription = cityViewModel.SmallDescription;
            return cityDTO;
        }


        #endregion

        #region "ConditionText"
        /// <summary>
        /// Metodo para mapear de entity a modelo generico para carga de combos de estados: tabla comm.country
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        public static List<GenericViewModel> CreateConditionLevels(List<Dictionary<string, string>> results)
        {
            return results.Select(p => new GenericViewModel()
            {
                Id = int.Parse(p.First(y => y.Key == "ConditionLevelCode").Value),
                DescriptionLong = p.First(y => y.Key == "SmallDescription").Value,
                DescriptionShort = p.First(y => y.Key == "SmallDescription").Value

            }).ToList();
        }
        /// <summary>
        /// Metodo para mapear de entity a modelo generico para carga de combos de estados: tabla comm.country
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        public static List<GenericViewModel> CreatePrefixes(List<Dictionary<string, string>> results)
        {
            return results.Select(p => new GenericViewModel()
            {
                Id = int.Parse(p.First(y => y.Key == "PrefixCode").Value),
                DescriptionLong = p.First(y => y.Key == "SmallDescription").Value,
                DescriptionShort = p.First(y => y.Key == "Description").Value

            }).ToList();
        }
        /// <summary>
        /// Metodo para mapear de entity a modelo generico para carga de combos de estados: tabla comm.country
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        public static List<GenericViewModel> CreateLineBusiness(List<Dictionary<string, string>> results)
        {
            return results.Select(p => new GenericViewModel()
            {
                Id = int.Parse(p.First(y => y.Key == "LineBusinessCode").Value),
                DescriptionLong = p.First(y => y.Key == "Description").Value,
                DescriptionShort = p.First(y => y.Key == "SmallDescription").Value
            }).ToList();
        }
         /// <summary>
        /// Metodo para mapear de entity a modelo generico para carga de combos de estados: tabla comm.country
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        public static List<GenericViewModel> CreateCoveredRiskType(List<Dictionary<string, string>> results)
        {
            return results.Select(p => new GenericViewModel()
            {
                Id = int.Parse(p.First(y => y.Key == "CoveredRiskTypeCode").Value),
                DescriptionLong = p.First(y => y.Key == "SmallDescription").Value,
                DescriptionShort = p.First(y => y.Key == "SmallDescription").Value

            }).ToList();
        }

        /// <summary>
        /// Metodo para mapear de entity a modelo generico para carga de combos de estados: tabla coverage
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        public static List<GenericViewModel> CreateCoverage(List<Dictionary<string, string>> results)
        {
            return results.Select(p => new GenericViewModel()
            {
                Id = int.Parse(p.First(y => y.Key == "CoverageId").Value),
                DescriptionLong = p.First(y => y.Key == "PrintDescription").Value,
                DescriptionShort = p.First(y => y.Key == "PrintDescription").Value

            }).ToList();
        }
        public static ConditionTextDTO MappConditionTextVMApplication(ConditionTextViewModel conditionTextViewModel)
        {
            ConditionTextDTO conditionTextDTO = new ConditionTextDTO();
            conditionTextDTO.Id = conditionTextViewModel.Id;
            conditionTextDTO.ConditionTextLevel = new ConditionTextLevelDTO { Id = conditionTextViewModel.ConditionTextLevel.Id, Description = conditionTextViewModel.ConditionTextLevel.Description??"" };
            conditionTextDTO.ConditionTextLevelType = new ConditionTextLevelTypeDTO { Id = conditionTextViewModel.ConditionTextLevelType.Id, Description = conditionTextViewModel.ConditionTextLevelType.Description??"" };
            conditionTextDTO.Title = conditionTextViewModel.Title;
            conditionTextDTO.Body = conditionTextViewModel.Body;
            return conditionTextDTO;
        }
        #endregion

        #region CrudServices
        public static List<Dictionary<string, string>> DynamicToDictionaryList(IEnumerable<dynamic> Items)
        {
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
            foreach (var item in Items)
            {
                result.Add(DynamicToDictionary(item));
            }
            return result;
        }
        public static List<Dictionary<string, string>> DynamicToDictionaryList(IEnumerable<dynamic> Items, Dictionary<string, string> filters)
        {
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
            foreach (var item in Items)
            {
                Dictionary<string, string> subItem = DynamicToDictionary(item);
                if (subItem.Keys.Contains(filters.Keys.ElementAt(0)) && subItem[filters.Keys.ElementAt(0)].Equals(filters.Values.ElementAt(0)))
                {
                    result.Add(subItem);
                }
            }
            return result;
        }
        public static Dictionary<string, string> DynamicToDictionary(dynamic item)
        {
            return ((IEnumerable<KeyValuePair<string, JToken>>)item).ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToString()); ;
        }
        #endregion

        #region CoCoverage
        
        /// <summary>
        /// MappCoCoverageVMApplication. metodo que mapea los datos de CoverageValueViewModel a CoCoverageValueDTO para crud de pesos de cobertura
        /// </summary>
        /// <param name="coverageViewModel"></param>
        /// <returns></returns>
         public static CoCoverageValueDTO MappCoCoverageVMApplication(CoverageValueViewModel coverageViewModel)
        {
            CoCoverageValueDTO coCoverageValueDTO = new CoCoverageValueDTO();
            coCoverageValueDTO.Porcentage= coverageViewModel.Porcentage;
            coCoverageValueDTO.Coverage = new CoverageDTO { Id= coverageViewModel.Coverage.Id, Description= coverageViewModel.Coverage.Description };
            coCoverageValueDTO.Prefix = new PrefixDTO { Id= coverageViewModel.Prefix.Id, Description= coverageViewModel.Prefix.Description };
          
            return coCoverageValueDTO;
        }
        #endregion

        #region Taxes


        #region Tax Assembler Methods

        /// <summary>
        /// Metodo para mapear de entity a modelo generico para carga de combos de tipos de tasa: PARAM.RATE_TYPE
        /// </summary>
        /// <param name="results"></param>
        /// <returns>GenericViewModel</returns>
        public static List<GenericViewModel> CreateRateTypes(List<Dictionary<string, string>> results)
        {
            return results.Select(p => new GenericViewModel()
            {
                Id = int.Parse(p.First(y => y.Key == "RateTypeCode").Value),
                DescriptionLong = p.First(y => y.Key == "Description").Value,
                DescriptionShort = p.First(y => y.Key == "SmallDescription").Value
            }).ToList();
        }

        /// <summary>
        /// Metodo para mapear de entity a modelo generico para carga de combos de condicion: TAX.TAX
        /// </summary>
        /// <param name="results"></param>
        /// <returns>GenericViewModel</returns>
        public static List<GenericViewModel> CreateBaseConditionsTax(List<Dictionary<string, string>> results)
        {
            return results.Select(p => new GenericViewModel()
            {
                Id = int.Parse(p.First(y => y.Key == "TaxCode").Value),
                DescriptionLong = p.First(y => y.Key == "Description").Value,
                DescriptionShort = p.First(y => y.Key == "SmallDescription").Value
            }).ToList();
        }

        /// <summary>
        /// Metodo para mapear de entity a modelo generico para carga de combos de retencion: TAX.TAX
        /// </summary>
        /// <param name="results"></param>
        /// <returns>GenericViewModel</returns>
        public static List<GenericViewModel> CreateBaseTaxWithHolding(List<Dictionary<string, string>> results)
        {
            return results.Select(p => new GenericViewModel()
            {
                Id = int.Parse(p.First(y => y.Key == "TaxCode").Value),
                DescriptionLong = p.First(y => y.Key == "Description").Value,
                DescriptionShort = p.First(y => y.Key == "SmallDescription").Value
            }).ToList();
        }

        /// <summary>
        /// Metodo para mapear de entity a modelo generico para carga de combos de rol: PARAM.ROLE
        /// </summary>
        /// <param name="results"></param>
        /// <returns>GenericViewModel</returns>
        public static List<GenericViewModel> CreateRole(List<Dictionary<string, string>> results)
        {
            return results.Select(p => new GenericViewModel()
            {
                Id = int.Parse(p.First(y => y.Key == "RoleCode").Value),
                DescriptionLong = p.First(y => y.Key == "Description").Value
            }).ToList();
        }

        /// <summary>
        /// Metodo para mapear de entity a modelo generico para carga de combos de rol: QUO.COMPONENT
        /// </summary>
        /// <param name="results"></param>
        /// <returns>GenericViewModel</returns>
        public static List<GenericViewModel> CreateFeeApply(List<Dictionary<string, string>> results)
        {
            return results.Select(p => new GenericViewModel()
            {
                Id = int.Parse(p.First(y => y.Key == "TaxAttributeTypeCode").Value),
                DescriptionLong = p.First(y => y.Key == "Description").Value
            }).ToList();
        }

        /// <summary>
        /// MappTaxApplication. metodo que mapea los datos de TaxViewModel a TaxDTO para crud
        /// </summary>
        /// <param name="TaxViewModel"></param>
        /// <returns>MappTaxApplication</returns>
        public static TaxDTO MappTaxApplication(TaxViewModel taxViewModel)
        {
            TaxDTO taxDTO = new TaxDTO()
            {
                Id = taxViewModel.Id,
                Description = taxViewModel.Description,
                TinyDescription = taxViewModel.Abbreviation,
                CurrentFrom = taxViewModel.CurrentFrom,
                RateType = new RateTypeDTO
                {
                    Id = taxViewModel.RateTypeTax,
                    Description = "",
                },
                AdditionalRateType = new RateTypeDTO
                {
                    Id = taxViewModel.RateTypeAdditionalTax,
                    Description = "",
                },
                BaseConditionTax = new BaseTaxDTO
                {
                    Id = taxViewModel.ConditionBaseTax,
                    Description = "",
                },
                IsEarned = taxViewModel.AccrualCalculation,
                IsSurPlus = taxViewModel.MinimumBaseCalculation,
                IsAdditionalSurPlus = taxViewModel.RateAdditionalCalculation,
                Enabled = taxViewModel.Enabled,
                IsRetention = taxViewModel.Retention,
                RetentionTax = new BaseTaxDTO
                {
                    Id = taxViewModel.BaseTaxWithholding,
                    Description = "",
                },
                TaxAttributes = taxViewModel.FeesApplies.Select(x => new TaxAttributeDTO() { Id = x, Description = "" }).ToList(),
                TaxRoles = taxViewModel.Roles.Select(x => new TaxRoleDTO() { Id = x, Description = "" }).ToList(),
            };
            return taxDTO;
        }

        #endregion


        #region TaxRate Assembler Methods

        /// <summary>
        /// Metodo para mapear de entity a modelo generico para carga de combos de condicion impositiva: TAX.TAX_CONDITION
        /// </summary>
        /// <param name="results"></param>
        /// <returns>GenericViewModel</returns>
        public static List<GenericViewModel> CreateTaxConditions(List<Dictionary<string, string>> results)
        {
            return results.Select(p => new GenericViewModel()
            {
                Id = int.Parse(p.First(y => y.Key == "TaxConditionCode").Value),
                IdC = int.Parse(p.First(y => y.Key == "TaxCode").Value),
                DescriptionLong = p.First(y => y.Key == "Description").Value
            }).ToList();
        }

        /// <summary>
        /// Metodo para mapear de entity a modelo generico para carga de combos de categoria: TAX.TAX_CATEGORY
        /// </summary>
        /// <param name="results"></param>
        /// <returns>GenericViewModel</returns>
        public static List<GenericViewModel> CreateTaxCategories(List<Dictionary<string, string>> results)
        {
            return results.Select(p => new GenericViewModel()
            {
                Id = int.Parse(p.First(y => y.Key == "TaxCategoryCode").Value),
                IdC = int.Parse(p.First(y => y.Key == "TaxCode").Value),
                DescriptionLong = p.First(y => y.Key == "Description").Value
            }).ToList();
        }

        /// <summary>
        /// Metodo para mapear de entity a modelo generico para carga de combos de sucursal: COMM.LINE_BUSINESS
        /// </summary>
        /// <param name="results"></param>
        /// <returns>GenericViewModel</returns>
        public static List<GenericViewModel> CreateLinesBusiness(List<Dictionary<string, string>> results)
        {
            return results.Select(p => new GenericViewModel()
            {
                Id = int.Parse(p.First(y => y.Key == "LineBusinessCode").Value),
                DescriptionLong = p.First(y => y.Key == "Description").Value
            }).ToList();
        }

        /// <summary>
        /// Metodo para mapear de entity a modelo generico para carga de combos de ramo técnico: COMM.BRANCH
        /// </summary>
        /// <param name="results"></param>
        /// <returns>GenericViewModel</returns>
        public static List<GenericViewModel> CreateBranches(List<Dictionary<string, string>> results)
        {
            return results.Select(p => new GenericViewModel()
            {
                Id = int.Parse(p.First(y => y.Key == "BranchCode").Value),
                DescriptionLong = p.First(y => y.Key == "Description").Value
            }).ToList();
        }

        /// <summary>
        /// Metodo para mapear de entity a modelo generico para carga de combos de actividad ecnomica: TAX.ECONOMIC_ACTIVITY_TAX
        /// </summary>
        /// <param name="results"></param>
        /// <returns>GenericViewModel</returns>
        public static List<GenericViewModel> CreateEconomicActivitiesTax(List<Dictionary<string, string>> results)
        {
            return results.Select(p => new GenericViewModel()
            {
                Id = int.Parse(p.First(y => y.Key == "EconomicActivityTaxId").Value),
                DescriptionLong = p.First(y => y.Key == "Description").Value
            }).ToList();
        }


        /// <summary>
        /// MappTaxRateApplication. metodo que mapea los datos de RateTaxViewModel a TaxRateDTO para crud
        /// </summary>
        /// <param name="RateTaxViewModel"></param>
        /// <returns>MappTaxRateApplication</returns>
        public static TaxRateDTO MappTaxRateApplication(RateTaxViewModel taxRateViewModel)
        {
            TaxRateDTO taxRateDTO = new TaxRateDTO()
            {
                Id = taxRateViewModel.IdRate,
                IdTax = taxRateViewModel.IdTax,
                Coverage = new CoverageDTO
                {
                    Id = taxRateViewModel.Coverage
                },
                TaxCondition = new TaxConditionDTO
                {
                    Id = taxRateViewModel.TaxCondition
                },
                TaxCategory = new TaxCategoryDTO
                {
                    Id = taxRateViewModel.TaxCategory
                },
                LineBusiness = new LineBusinnessDTO
                {
                    Id = taxRateViewModel.LineBusiness
                },
                TaxState = new TaxStateDTO
                {
                    IdCountry = taxRateViewModel.Country,
                    IdState = taxRateViewModel.State,
                    IdCity = taxRateViewModel.City
                },
                EconomicActivity = new EconomicActivityDTO
                {
                    Id = taxRateViewModel.EconomicActivityId,
                    Description = taxRateViewModel.EconomicActivity
                },
                Branch = new BranchDTO
                {
                    Id = taxRateViewModel.TechnicalBranch
                },
                TaxPeriodRate = new TaxPeriodRateDTO
                {
                    CurrentFrom = taxRateViewModel.CurrentFrom,
                    Rate = taxRateViewModel.Rate,
                    AdditionalRate = taxRateViewModel.RateAdditional,
                    BaseTaxAdditional = taxRateViewModel.BasicTaxIncludedAdditionalBase,
                    MinBaseAMT = taxRateViewModel.MinimumTaxableBase,
                    MinAdditionalBaseAMT = taxRateViewModel.MinimumAdditionalTaxableBase,
                    MinTaxAMT = taxRateViewModel.Minimum,
                    MinAdditionalTaxAMT = taxRateViewModel.AdditionalMinimum
                }
            };
            return taxRateDTO;
        }

        #endregion


        #region TaxCategory Assembler Methods

        /// <summary>
        /// MappTaxCategoryApplication. metodo que mapea los datos de CategoryTaxViewModel a TaxCategoryDTO para crud
        /// </summary>
        /// <param name="CategoryTaxViewModel"></param>
        /// <returns>taxCategoryDTO</returns>
        public static TaxCategoryDTO MappTaxCategoryApplication(CategoryTaxViewModel categoryTaxViewModel)
        {
            TaxCategoryDTO taxCategoryDTO = new TaxCategoryDTO()
            {
                Id = categoryTaxViewModel.IdCategory,
                IdTax = categoryTaxViewModel.IdTax,
                Description = categoryTaxViewModel.DescriptionCategory
            };
            return taxCategoryDTO;
        }


        /// <summary>
        /// MappTaxCategoryListApplication. metodo que mapea una lista con los datos de una lista de CategoryTaxViewModel a una lista TaxCategoryDTO para crud
        /// </summary>
        /// <param name="categoryTaxViewModelList"></param>
        /// <returns>TaxCategoryDTOList</returns>
        public static List<TaxCategoryDTO> MappTaxCategoryListApplication(List<CategoryTaxViewModel> categoryTaxViewModelList)
        {
            List<TaxCategoryDTO> TaxCategoryDTOList = new List<TaxCategoryDTO>();
            foreach(CategoryTaxViewModel item in categoryTaxViewModelList)
            {
                TaxCategoryDTO TaxCategoryDTO = new TaxCategoryDTO();
                TaxCategoryDTO = MappTaxCategoryApplication(item);
                TaxCategoryDTOList.Add(TaxCategoryDTO);
            }
            return TaxCategoryDTOList;
        }


        /// <summary>
        /// MappTaxCategoryDTOToTaxCategoryViewModel. metodo que mapea un TaxCategoryDTO a un CategoryTaxViewModel
        /// </summary>
        /// <param name="TaxCategoryDTO"></param>
        /// <returns>CategoryTaxViewModel</returns>
        public static CategoryTaxViewModel MappTaxCategoryDTOToTaxCategoryViewModel(TaxCategoryDTO taxCategoryDTO)
        {
            CategoryTaxViewModel categoryTaxViewModel = new CategoryTaxViewModel()
            {
                IdCategory = taxCategoryDTO.Id,
                IdTax = taxCategoryDTO.IdTax,
                DescriptionCategory = taxCategoryDTO.Description
            };
            return categoryTaxViewModel;
        }


        /// <summary>
        /// MappTaxCategoryDTOListToTaxCategoryViewModelList. metodo que mapea una lista con los datos de TaxCategoryDTO a una lista CategoryTaxViewModel para crud
        /// </summary>
        /// <param name="TaxCategoryDTO"></param>
        /// <returns>CategoryTaxViewModelList</returns>
        public static List<CategoryTaxViewModel> MappTaxCategoryDTOListToTaxCategoryViewModelList(List<TaxCategoryDTO> taxCategoryDTOList)
        {
            List<CategoryTaxViewModel> categoryTaxViewModelList = new List<CategoryTaxViewModel>();
            foreach (TaxCategoryDTO item in taxCategoryDTOList)
            {
                CategoryTaxViewModel categoryTaxViewModel = new CategoryTaxViewModel();
                categoryTaxViewModel = MappTaxCategoryDTOToTaxCategoryViewModel(item);
                categoryTaxViewModelList.Add(categoryTaxViewModel);
            }
            return categoryTaxViewModelList;
        }

        #endregion


        #region TaxCondition Assembler Methods

        /// <summary>
        /// MappTaxConditionApplication. metodo que mapea los datos de ConditionTaxViewModel a TaxConditionDTO para crud
        /// </summary>
        /// <param name="ConditionTaxViewModel"></param>
        /// <returns>taxConditionDTO</returns>
        public static TaxConditionDTO MappTaxConditionApplication(ConditionTaxViewModel conditionTaxViewModel)
        {
            TaxConditionDTO taxConditionDTO = new TaxConditionDTO()
            {
                Id = conditionTaxViewModel.IdCondition,
                IdTax = conditionTaxViewModel.IdTax,
                Description = conditionTaxViewModel.DescriptionCondition,
                HasNationalRate = conditionTaxViewModel.NationalRate,
                IsIndependent = conditionTaxViewModel.Independent,
                IsDefault = conditionTaxViewModel.Enabled
            };
            return taxConditionDTO;
        }


        /// <summary>
        /// MappTaxConditionListApplication. metodo que mapea una lista con los datos de una lista de ConditionTaxViewModel a una lista TaxConditionDTO para crud
        /// </summary>
        /// <param name="conditionTaxViewModel"></param>
        /// <returns>TaxCategoryDTOList</returns>
        public static List<TaxConditionDTO> MappTaxConditionListApplication(List<ConditionTaxViewModel> conditionTaxViewModelList)
        {
            List<TaxConditionDTO> TaxConditionDTOList = new List<TaxConditionDTO>();
            foreach (ConditionTaxViewModel item in conditionTaxViewModelList)
            {
                TaxConditionDTO taxConditionDTO = new TaxConditionDTO();
                taxConditionDTO = MappTaxConditionApplication(item);
                TaxConditionDTOList.Add(taxConditionDTO);
            }
            return TaxConditionDTOList;
        }


        /// <summary>
        /// MappTaxCategoryDTOToTaxCategoryViewModel. metodo que mapea un TaxConditionDTO a un ConditionTaxViewModel para crud
        /// </summary>
        /// <param name="TaxCategoryDTO"></param>
        /// <returns>CondtionTaxViewModel</returns>
        public static ConditionTaxViewModel MappTaxConditionDTOToTaxConditionViewModel(TaxConditionDTO taxConditionDTO)
        {
            ConditionTaxViewModel conditionTaxViewModel = new ConditionTaxViewModel()
            {
                IdCondition = taxConditionDTO.Id,
                IdTax = taxConditionDTO.IdTax,
                DescriptionCondition = taxConditionDTO.Description,
                NationalRate = taxConditionDTO.HasNationalRate,
                Independent = taxConditionDTO.IsIndependent,
                Enabled = taxConditionDTO.IsDefault
            };
            return conditionTaxViewModel;
        }


        /// <summary>
        /// MappTaxConditionDTOListToTaxConditionViewModelList. metodo que mapea una lista TaxConditionDTO a una lista ConditionTaxViewModel  para crud
        /// </summary>
        /// <param name="TaxCategoryDTOList"></param>
        /// <returns>CategoryTaxViewModelList</returns>
        public static List<ConditionTaxViewModel> MappTaxConditionDTOListToTaxConditionViewModelList(List<TaxConditionDTO> taxConditionDTOList)
        {
            List<ConditionTaxViewModel> conditionTaxViewModelList = new List<ConditionTaxViewModel>();
            foreach (TaxConditionDTO item in taxConditionDTOList)
            {
                ConditionTaxViewModel conditionTaxViewModel = new ConditionTaxViewModel();
                conditionTaxViewModel = MappTaxConditionDTOToTaxConditionViewModel(item);
                conditionTaxViewModelList.Add(conditionTaxViewModel);
            }
            return conditionTaxViewModelList;
        }

        #endregion

        #endregion

        #region Sarlaft
        /// <summary>
        /// Metodo para mapear de entity a modelo generico para carga de combos
        /// </summary>
        /// <param name="results"></param>
        /// <returns>GenericViewModel</returns>
        public static List<GenericViewModel> CreateSignatureBranch(List<Dictionary<string, string>> results)
        {
            return results.Select(p => new GenericViewModel()
            {
                Id = int.Parse(p.First(y => y.Key == "BranchCode").Value),
                DescriptionLong = p.First(y => y.Key == "Description").Value,
                DescriptionShort = p.First(y => y.Key == "SmallDescription").Value
            }).ToList();
        }

        public static List<GenericViewModel> CreateInterviewResult(List<Dictionary<string, string>> results)
        {
            return results.Select(p => new GenericViewModel()
            {
                Id = int.Parse(p.First(y => y.Key == "InterviewResultCode").Value),
                DescriptionLong = p.First(y => y.Key == "Description").Value,
                DescriptionShort = p.First(y => y.Key == "SmallDescription").Value
            }).ToList();
        }

        public static List<GenericViewModel> CreateLinkType(List<Dictionary<string, string>> results)
        {
            return results.Select(p => new GenericViewModel()
            {
                Id = int.Parse(p.First(y => y.Key == "LinkTypeCode").Value),
                DescriptionLong = p.First(y => y.Key == "Description").Value,
                DescriptionShort = p.First(y => y.Key == "SmallDescription").Value
            }).ToList();
        }
        

        /// <summary>
        /// Metodo para mapear de entity a modelo generico para carga de combos
        /// </summary>
        /// <param name="results"></param>
        /// <returns>GenericViewModel</returns>
        public static List<GenericViewModel> CreateCountry(List<Dictionary<string, string>> results)
        {
            return results.Select(p => new GenericViewModel()
            {
                Id = int.Parse(p.First(y => y.Key == "CountryCode").Value),
                DescriptionLong = p.First(y => y.Key == "Description").Value,
                DescriptionShort = p.First(y => y.Key == "SmallDescription").Value
            }).ToList();
        }

        /// <summary>
        /// Metodo para mapear de entity a modelo generico para carga de combos
        /// </summary>
        /// <param name="results"></param>
        /// <returns>GenericViewModel</returns>
        public static List<GenericViewModel> CreateState(List<Dictionary<string, string>> results)
        {
            return results.Select(p => new GenericViewModel()
            {
                Id = int.Parse(p.First(y => y.Key == "StateCode").Value),
                DescriptionLong = p.First(y => y.Key == "Description").Value,
                DescriptionShort = p.First(y => y.Key == "SmallDescription").Value,
                IdC = int.Parse(p.First(y => y.Key == "CountryCode").Value)
            }).ToList();
        }

        /// <summary>
        /// Metodo para mapear de entity a modelo generico para carga de combos
        /// </summary>
        /// <param name="results"></param>
        /// <returns>GenericViewModel</returns>
        public static List<GenericViewModel> CreateCity(List<Dictionary<string, string>> results)
        {
            return results.Select(p => new GenericViewModel()
            {
                Id = int.Parse(p.First(y => y.Key == "CityCode").Value),
                DescriptionLong = p.First(y => y.Key == "Description").Value,
                DescriptionShort = p.First(y => y.Key == "SmallDescription").Value,
                IdC = int.Parse(p.First(y => y.Key == "CountryCode").Value),
                IdD = int.Parse(p.First(y => y.Key == "StateCode").Value)
            }).ToList();
        }
        public static List<GenericViewModel> CreateOperationType(List<Dictionary<string, string>> results)
        {
            return results.Select(p => new GenericViewModel()
            {
                Id = int.Parse(p.First(y => y.Key == "OperationTypeCode").Value),
                DescriptionShort = p.First(y => y.Key == "SmallDescription").Value
            }).ToList();
        }
        public static List<GenericViewModel> CreateProductType(List<Dictionary<string, string>> results)
        {
            return results.Select(p => new GenericViewModel()
            {
                Id = int.Parse(p.First(y => y.Key == "ProductTypeCode").Value),
                DescriptionLong = p.First(y => y.Key == "Description").Value,
                DescriptionShort = p.First(y => y.Key == "SmallDescription").Value,
            }).ToList();
        }
        public static List<GenericViewModel> CreateCurrency(List<Dictionary<string, string>> results)
        {
            return results.Select(p => new GenericViewModel()
            {
                Id = int.Parse(p.First(y => y.Key == "CurrencyCode").Value),
                DescriptionLong = p.First(y => y.Key == "Description").Value,
                DescriptionShort = p.First(y => y.Key == "SmallDescription").Value,
            }).ToList();
        }

        public static List<GenericViewModel> CreateDocumentType(List<Dictionary<string, string>> results)
        {
            return results.Select(p => new GenericViewModel()
            {
                Id = int.Parse(p.First(y => y.Key == "IdDocumentType").Value),
                DescriptionLong = p.First(y => y.Key == "Description").Value,
                DescriptionShort = p.First(y => y.Key == "SmallDescription").Value,
                IsAlphanumeric = bool.Parse(p.First(y => y.Key == "IsAlphanumeric").Value)

            }).ToList();
        }

        #endregion

        #region Automapper
        #region Detail
        public static IMapper CreateMapDetail()
        {
            var config = MapperCache.GetMapper<DetailServiceModel, DetailViewModel>(cfg =>
            {
                cfg.CreateMap<DetailServiceModel, DetailViewModel>();
            });
            return config;
        }
        #endregion

        #region FinancialPlan
        public static IMapper CreateMapFinancialPlan()
        {
            var config = MapperCache.GetMapper<FinancialPlanViewModel, FinancialPlanServiceModel>(cfg =>
            {
                cfg.CreateMap<FinancialPlanViewModel, FinancialPlanServiceModel>();
            });
            return config;
        }
        #endregion

        #region FinancialPlan
        public static IMapper CreateMapCoverage()
        {
            var config = MapperCache.GetMapper<CoverageServiceModel, CoverageViewModel>(cfg =>
            {
                cfg.CreateMap<CoverageServiceModel, CoverageViewModel>();
            });
            return config;
        }
        #endregion

        #region Clause
        public static IMapper CreateMapClause()
        {
            var config = MapperCache.GetMapper<MOS.ClauseServiceModel, ClauseViewModel>(cfg =>
            {
                cfg.CreateMap<MOS.ClauseServiceModel, ClauseViewModel>();
            });
            return config;
        }

        public static IMapper CreateMapClauseView()
        {
            var config = MapperCache.GetMapper<ClauseViewModel, ClauseServiceModel>(cfg =>
            {
                cfg.CreateMap<ClauseViewModel, ClauseServiceModel>()
                .ForMember(x => x.ConditionLevelServiceQueryModel, z => z.MapFrom(y => new ConditionLevelServiceModel { Id = y.Level }))
                .ForMember(x => x.ClauseLevelServiceModel, z => z.MapFrom(y => y.CommercialBranch != null ?
                  new ClauseLevelServiceModel { ConditionLevelId = y.CommercialBranch, IsMandatory = y.Required, StatusTypeService = ENUMSM.StatusTypeService.Original } :
                  y.Coverage != null ? new ClauseLevelServiceModel { ConditionLevelId = y.Coverage, IsMandatory = y.Required, StatusTypeService = ENUMSM.StatusTypeService.Original } :
                  y.CoveredRisk != null ? new ClauseLevelServiceModel { ConditionLevelId = y.CoveredRisk, IsMandatory = y.Required, StatusTypeService = ENUMSM.StatusTypeService.Original } :
                  y.LineBusiness != null ? new ClauseLevelServiceModel { ConditionLevelId = y.LineBusiness, IsMandatory = y.Required, StatusTypeService = ENUMSM.StatusTypeService.Original } :
                  y.Level == 1 ? new ClauseLevelServiceModel { ConditionLevelId = null, IsMandatory = y.Required, StatusTypeService = ENUMSM.StatusTypeService.Original } :
                  new ClauseLevelServiceModel { ConditionLevelId = y.Level, IsMandatory = y.Required, StatusTypeService = ENUMSM.StatusTypeService.Original }));
            });
            return config;
        }
        #endregion

        #region Infringement
        public static IMapper CreateMapInfringement()
        {
            var config = MapperCache.GetMapper<InfringementServiceModel, InfringementViewModel>(cfg =>
            {
                cfg.CreateMap<InfringementServiceModel, InfringementViewModel>();
            });
            return config;
        }
        public static IMapper CreateMapInfringementView()
        {
            var config = MapperCache.GetMapper<InfringementViewModel, InfringementServiceModel>(cfg =>
            {
                cfg.CreateMap<InfringementViewModel, InfringementServiceModel>();
            });
            return config;
        }
        public static IMapper CreateMapInfringementGroup()
        {
            var config = MapperCache.GetMapper<InfringementGroupServiceModel, InfringementGroupViewModel>(cfg =>
            {
                cfg.CreateMap<InfringementGroupServiceModel, InfringementGroupViewModel>();
            });
            return config;
        }
        public static IMapper CreateMapInfringementGroupView()
        {
            var config = MapperCache.GetMapper<InfringementGroupViewModel, InfringementGroupServiceModel>(cfg =>
            {
                cfg.CreateMap<InfringementGroupViewModel, InfringementGroupServiceModel>();
            });
            return config;
        }
        public static IMapper CreateMapInfringementState()
        {
            var config = MapperCache.GetMapper<InfringementStateServiceModel, InfringementStateViewModel>(cfg =>
            {
                cfg.CreateMap<InfringementStateServiceModel, InfringementStateViewModel>();
            });
            return config;
        }
        public static IMapper CreateMapInfringementStateView()
        {
            var config = MapperCache.GetMapper<InfringementStateViewModel, InfringementStateServiceModel>(cfg =>
            {
                cfg.CreateMap<InfringementStateViewModel, InfringementStateServiceModel>();
            });
            return config;
        }
        #endregion

        #region Payment
        public static IMapper CreateMapPaymentMethodType()
        {
            var config = MapperCache.GetMapper<PaymentMethodTypeServiceQueryModel, PaymentMethodTypeViewModel>(cfg =>
            {
                cfg.CreateMap<PaymentMethodTypeServiceQueryModel, PaymentMethodTypeViewModel>();
            });
            return config;
        }
        public static IMapper CreateMapPaymentPlan()
        {
            var config = MapperCache.GetMapper<PaymentPlanServiceModel, PaymentPlanViewModel>(cfg =>
            {
                cfg.CreateMap<PaymentPlanServiceModel, PaymentPlanViewModel>();
            });
            return config;
        }
        public static IMapper CreateMapPaymentPlanView()
        {
            var config = MapperCache.GetMapper<PaymentPlanViewModel, PaymentPlanServiceModel>(cfg =>
            {
                cfg.CreateMap<PaymentPlanViewModel, PaymentPlanServiceModel>();
            });
            return config;
        }
        #endregion

        #region RatingZone
        public static IMapper CreateMapRatingZone()
        {
            var config = MapperCache.GetMapper<RatingZoneServiceModel, RatingZoneViewModel>(cfg =>
            {
                cfg.CreateMap<RatingZoneServiceModel, RatingZoneViewModel>()
                    .ForMember(x => x.PrefixCode, x => x.MapFrom(y => y.Prefix.PrefixCode))
                    .ForMember(x => x.PrefixDescription, x => x.MapFrom(y => y.Prefix.PrefixDescription));
            });
            return config;
        }
        public static IMapper CreateMapRatingZoneView()
        {
            var config = MapperCache.GetMapper<RatingZoneViewModel, RatingZoneServiceModel>(cfg =>
            {
                cfg.CreateMap<RatingZoneViewModel, RatingZoneServiceModel>();
            });
            return config;
        }
        #endregion

        #region Worker
        public static IMapper CreateMapWorkerType()
        {
            var config = MapperCache.GetMapper<WorkerTypeServiceModel, WorkerTypeViewModel>(cfg =>
            {
                cfg.CreateMap<WorkerTypeServiceModel, WorkerTypeViewModel>();
            });
            return config;
        }
        public static IMapper CreateMapWorkerTypeView()
        {
            var config = MapperCache.GetMapper<WorkerTypeViewModel, WorkerTypeServiceModel>(cfg =>
            {
                cfg.CreateMap<WorkerTypeViewModel, WorkerTypeServiceModel>();
            });
            return config;
        }
        #endregion

        #endregion

        #region ListRiskPerson
        public static List<SelectModel> CreatePersonSearchType(List<Dictionary<string, string>> results)
        {
            return results.Select(p => new SelectModel()
            {
                Id = int.Parse(p.First(y => y.Key == "RISK_LIST_CD").Value),
                Description = p.First(y => y.Key == "DESCRIPTION").Value
            }).ToList();
        }
        #endregion


    }
} 