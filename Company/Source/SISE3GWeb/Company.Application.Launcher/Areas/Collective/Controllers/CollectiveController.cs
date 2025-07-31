using AutoMapper;
using Sistran.Company.Application.CollectiveServices.Models;
using Sistran.Company.Application.Location.PropertyServices.Models;
using Sistran.Company.Application.ProductServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UnderModel = Sistran.Core.Application.UnderwritingServices.Models;
namespace Sistran.Core.Framework.UIF.Web.Areas.Collective.Controllers
{
    using Sistran.Core.Application.Utilities.Cache;
    using Sistran.Core.Framework.UIF.Web.Areas.Collective.Models;
    [Authorize]
    public class CollectiveController : Controller
    {

        #region Collective

        public ActionResult Collective(CollectiveModelView model)
        {
            return View("Collective", model);
        }

        /// <summary>
        /// Obtiene el listado de cargues.
        /// </summary>
        /// <param name="tempId">Identificador del temporal.</param>
        /// <returns></returns>
        public ActionResult GetTemporalByTempId(int tempId)
        {
            try
            {
                CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(tempId, true);

                if (policy != null)
                {

                    if (policy.Endorsement != null)
                    {
                        policy.Endorsement.EndorsementTypeDescription = EnumsHelper.GetItemName<EndorsementType>(policy.Endorsement.EndorsementType);
                    }

                    if (policy.Id != 0)
                    {
                        if (policy.PolicyOrigin == PolicyOrigin.Collective)
                        {
                            var authorizationRequests = DelegateService.AuthorizationPoliciesService.GetAuthorizationRequestsByKey(tempId.ToString());
                            if (authorizationRequests.Count != 0)
                            {
                                policy.InfringementPolicies = new List<PoliciesAut>();
                            }
                            return new UifJsonResult(true, policy);
                        }
                        else
                        {
                            return new UifJsonResult(false, App_GlobalResources.Language.TemporalNotCollective);
                        }

                    }
                    else
                    {
                        return new UifJsonResult(false, App_GlobalResources.Language.InvalidTemporal);
                    }

                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.InvalidTemporal);
                }

            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchTemporary + " " + ex);
            }
        }

        /// <summary>
        /// Redireción pagina importar cargue
        /// </summary>
        public ActionResult RedirectImportProcess(int tempId)
        {
            CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(tempId, true);
            if (policy != null)
            {
                return new UifJsonResult(true, policy);
            }
            return new UifJsonResult(false, App_GlobalResources.Language.ErrorTempNoExist);
        }

        public ActionResult CollectiveLoadsByTempId(int tempId)
        {
            List<CollectiveEmission> collectiveEmissions = DelegateService.collectiveService.GetCollectiveEmissionsByTempId(tempId, true, false);

            if (collectiveEmissions != null)
            {
                Parallel.ForEach(collectiveEmissions, x =>
                {
                    x.WithErrors = x.Rows.Count(y => y.HasError == true);
                    x.TotalRows = x.Rows.Count;
                });
                return new UifJsonResult(true, collectiveEmissions);
            }
            return new UifJsonResult(false, App_GlobalResources.Language.ErrorTempNoExist);
        }
        #endregion

        #region AddMassive


        public ActionResult UploadFile()
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    String urlPath = DelegateService.commonService.GetKeyApplication("SavePathExcel") + @"\" + SessionHelper.GetUserName() + @"\";

                    if (!System.IO.Directory.Exists(urlPath))
                    {
                        System.IO.Directory.CreateDirectory(urlPath);
                    }

                    HttpPostedFileBase httpPostedFileBase = Request.Files[0] as HttpPostedFileBase;
                    string fileName = Guid.NewGuid() + System.IO.Path.GetExtension(httpPostedFileBase.FileName);
                    httpPostedFileBase.SaveAs(urlPath + fileName);

                    if (CopyFile(fileName, urlPath))
                    {
                        return new UifJsonResult(true, fileName);
                    }
                    else
                    {
                        return new UifJsonResult(false, App_GlobalResources.Language.ErrorUploadFile);
                    }
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.MessageNotUploadFile);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorUploadFile);
            }
        }
        private bool CopyFile(string fileName, string urlPath)
        {
            string externalDirectoryPath = "";
            externalDirectoryPath = DelegateService.commonService.GetKeyApplication("ExternalFolderFiles") + @"\" + SessionHelper.GetUserName() + @"\";

            if (!System.IO.Directory.Exists(externalDirectoryPath))
            {
                System.IO.Directory.CreateDirectory(externalDirectoryPath);
            }

            string user = DelegateService.commonService.GetKeyApplication("UserDomain");
            string password = DelegateService.commonService.GetKeyApplication("DomainPassword");
            string domain = DelegateService.commonService.GetKeyApplication("Domain");

            Uri uri = new Uri(externalDirectoryPath, UriKind.Absolute);

            int retries = 0;

            while (retries < 5)
            {
                try
                {
                    using (NetworkConnection networkCon = new NetworkConnection(uri.LocalPath, new NetworkCredential(user, password, domain)))
                    {
                        if (networkCon._resultConnection == 1219 || networkCon._resultConnection == 0)
                        {
                            System.IO.FileInfo fileInfo = new System.IO.FileInfo(urlPath + fileName);
                            fileInfo.CopyTo(externalDirectoryPath.ToString() + fileName);
                            fileInfo.Delete();
                            break;
                        }
                        else
                        {
                            throw new Win32Exception(networkCon._resultConnection);
                        }
                    }
                }
                catch (Exception)
                {
                    retries++;
                    if (retries == 5)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        public ActionResult AddMassive(CollectiveModelView collectiveModelView)
        {
            try
            {
                if (collectiveModelView != null)
                {

                    int loadId = 0;
                    switch (collectiveModelView.LoadTypeId)
                    {
                        case (int)SubMassiveProcessType.CollectiveEmission:
                            loadId = CreateCollectiveEmission(collectiveModelView);
                            break;
                        case (int)SubMassiveProcessType.Inclusion:
                        case (int)SubMassiveProcessType.Exclusion:
                            loadId = CreateCollectiveEndorsement(collectiveModelView);
                            break;
                        case (int)SubMassiveProcessType.CollectiveRenewal:
                            loadId = CreateCollectiveRenewal(collectiveModelView);
                            break;
                        default:
                            break;
                    }

                    if (loadId == 0)
                    {
                        return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateLoad);
                    }
                    else
                    {
                        return new UifJsonResult(true, App_GlobalResources.Language.MessageGeneratedLoadNo + " " + loadId);
                    }

                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.InvalidFileType);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveUploadInformation);
            }
        }

        #endregion
        #region emision
        private int CreateCollectiveEmission(CollectiveModelView collectiveModelView)
        {

            CollectiveEmission collectiveEmission = Sistran.Core.Framework.UIF.Web.Areas.Collective.Models.ModelAssembler.CreateCollectiveEmission(collectiveModelView);

            collectiveEmission.Product = DelegateService.productService.GetProductByProductIdPrefixId(collectiveEmission.Product.Id, collectiveEmission.Prefix.Id);

            switch (collectiveEmission.Product.CoveredRisk.SubCoveredRiskType.Value)
            {
                case SubCoveredRiskType.JudicialSurety:
                    break;
                case SubCoveredRiskType.Liability:
                    collectiveEmission = DelegateService.collectiveService.CreateCollectiveEmission(collectiveEmission);
                    break;
                case SubCoveredRiskType.Property:
                    //collectiveEmission = DelegateService.propertyCollectiveService.CreateCollectiveEmission(collectiveEmission);
                    break;
                case SubCoveredRiskType.Surety:
                    break;
                case SubCoveredRiskType.ThirdPartyLiability:
                    collectiveEmission = DelegateService.collectiveService.CreateCollectiveEmission(collectiveEmission);
                    break;
                case SubCoveredRiskType.Vehicle:
                    collectiveEmission = DelegateService.vehicleCollectiveService.CreateCollectiveLoad(collectiveEmission);

                    break;
            }

            if (collectiveEmission == null || collectiveEmission.HasError)
            {
                return 0;
            }
            else
            {
                return collectiveEmission.Id;
            }
        }
        #endregion

        #region Endorsement
        private int CreateCollectiveEndorsement(CollectiveModelView collectiveModelView)
        {

            CollectiveEmission collectiveEmission = ModelAssembler.CreateCollectiveEmission(collectiveModelView);

            collectiveEmission.Product = DelegateService.productService.GetProductByProductIdPrefixId(collectiveEmission.Product.Id, collectiveEmission.Prefix.Id);

            switch (collectiveEmission.Product.CoveredRisk.SubCoveredRiskType.Value)
            {
                case SubCoveredRiskType.ThirdPartyLiability:
                    break;
                case SubCoveredRiskType.Vehicle:
                    //collectiveEmission = DelegateService.collectiveVehicleModificationService.CreateCollectiveModification(collectiveEmission);
                    break;
                case SubCoveredRiskType.Property:
                    //collectiveEmission = DelegateService.collectivePropertyModificationService.CreateCollectiveModification(collectiveEmission);
                    break;
            }

            if (collectiveEmission == null || collectiveEmission.HasError)
            {
                return 0;
            }
            else
            {
                return collectiveEmission.Id;
            }
        }

        private int CreateCollectiveRenewal(CollectiveModelView collectiveModelView)
        {

            CollectiveEmission collectiveEmission = ModelAssembler.CreateCollectiveEmission(collectiveModelView);

            collectiveEmission.Product = DelegateService.productService.GetProductByProductIdPrefixId(collectiveEmission.Product.Id, collectiveEmission.Prefix.Id);

            switch (collectiveEmission.Product.CoveredRisk.SubCoveredRiskType.Value)
            {
                case SubCoveredRiskType.JudicialSurety:
                    break;
                case SubCoveredRiskType.Liability:
                    break;
                case SubCoveredRiskType.Property:
                    collectiveEmission.CoveredRiskType = CoveredRiskType.Location;
                    //collectiveEmission = DelegateService.collectivePropertyRenewalService.CreateCollectiveRenewal(collectiveEmission);
                    break;
                case SubCoveredRiskType.Surety:
                    break;
                case SubCoveredRiskType.ThirdPartyLiability:
                    break;
                case SubCoveredRiskType.Vehicle:
                    collectiveEmission.CoveredRiskType = CoveredRiskType.Vehicle;
                    //collectiveEmission = DelegateService.collectiveVehicleRenewalService.CreateCollectiveRenewal(collectiveEmission);

                    break;
            }

            if (collectiveEmission == null || collectiveEmission.HasError)
            {
                return 0;
            }
            else
            {
                return collectiveEmission.Id;
            }
        }

        /*public ActionResult EndorsementModification(EndorsementModelView policyModel)
        {
            SearchViewModel searchViewModel = new SearchViewModel();
            if ((EndorsementType)policyModel.EndorsementType != EndorsementType.Cancellation)
            {
                return View("EndorsementModification", policyModel);
            }
            else
            {
                return RedirectToAction("Search", "Search", new { area = "Endorsement" });
            }
        }*/

        public ActionResult GetEndorsementsByFilterPolicy(int branchId, int prefixId, decimal policyNumber)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    List<UnderModel.Endorsement> endorsements = DelegateService.underwritingService.GetEndorsementsByPrefixIdBranchIdPolicyNumber(prefixId, branchId, policyNumber);

                    if (endorsements != null)
                    {
                        return new UifSelectResult(endorsements);
                    }
                    else
                    {
                        return new UifJsonResult(false, App_GlobalResources.Language.ErrorPolicyNoExist);
                    }
                }
                else
                {
                    return new UifJsonResult(false, "Error:GetEndorsementsByFilterPolicy ");
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchPolicy);
            }
        }

        public ActionResult GetSummaryByPrefixIdEndorsementId(int prefixId, int endorsementId)
        {
            try
            {
                UnderModel.Summary summary = null;// DelegateService.printingService.GetSummaryByEndorsementId(endorsementId);

                if (summary != null)
                {
                    return new UifJsonResult(true, summary);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.NoInformationEndorsement);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorInformationEndorsement);
            }
        }
        #endregion

        public ActionResult GetBranches()
        {
            List<Branch> branches = DelegateService.uniqueUserService.GetBranchesByUserId(SessionHelper.GetUserId());
            return new UifSelectResult(branches.OrderBy(x => x.Description));
        }

        public ActionResult GetPrefixes()
        {
            List<Prefix> prefixes = DelegateService.commonService.GetPrefixes();
            return new UifSelectResult(prefixes.OrderBy(x => x.Description));
        }

        public ActionResult ImportMassive(CollectiveModelView collectiveModelView)
        {
            return View("ImportMassive", collectiveModelView);
        }

        #region AuthorizeUserTemporal
        public ActionResult AuthorizeUserTemporal()
        {
            return ValidateLoggedUser();
        }

        private ActionResult ValidateLoggedUser()
        {
            if (Request.IsAuthenticated)
            {
                return View();
            }

            return RedirectToAction("Login", "Account");
        }

        #endregion




        //private CompanyTplRisk GetDataModification(CompanyTplRisk risk, CompanyPolicy policy, CoverageStatusType coverageStatusType)
        //{
        //    if (risk.Risk.Beneficiaries[0].IdentificationDocument == null)
        //    {
        //        List<Beneficiary> beneficiaries = new List<Beneficiary>();

        //        foreach (CompanyBeneficiary item in risk.Risk.Beneficiaries)
        //        {
        //            Beneficiary beneficiary = new Beneficiary();
        //            beneficiary = DelegateService.underwritingService.GetBeneficiariesByDescription(item.IndividualId.ToString(), Sistran.Core.Services.UtilitiesServices.Enums.InsuredSearchType.IndividualId).FirstOrDefault();
        //            item.IdentificationDocument = beneficiary.IdentificationDocument;
        //            item.Name = beneficiary.Name;
        //        }
        //    }

        //    if (risk.Risk.Premium == 0)
        //    {
        //        List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(policy.Product.Id, risk.Risk.GroupCoverage.Id, policy.Prefix.Id);

        //        coverages = coverages.Where(c => (risk.Risk.Coverages.Any(x => x.Id == c.Id))).ToList();

        //        foreach (CompanyCoverage item in coverages)
        //        {
        //            item.RiskCoverageId = risk.Risk.Coverages.First(x => x.Id == item.Id).RiskCoverageId;
        //            item.CoverageOriginalStatus = risk.Risk.Coverages.First(x => x.Id == item.Id).CoverageOriginalStatus;
        //            item.AccumulatedPremiumAmount = risk.Risk.Coverages.First(x => x.Id == item.Id).AccumulatedPremiumAmount;
        //            item.OriginalLimitAmount = risk.Risk.Coverages.First(x => x.Id == item.Id).OriginalLimitAmount;
        //            item.OriginalSubLimitAmount = risk.Risk.Coverages.First(x => x.Id == item.Id).OriginalSubLimitAmount;
        //            item.CoverStatus = coverageStatusType;
        //            item.CoverStatusName = EnumsHelper.GetItemName<CoverageStatusType>(coverageStatusType);
        //            item.EndorsementType = policy.Endorsement.EndorsementType;
        //            item.CurrentFrom = policy.CurrentFrom;
        //            item.CurrentTo = policy.CurrentTo;
        //            item.Rate = risk.Risk.Coverages.First(x => x.Id == item.Id).Rate;
        //            item.RateType = risk.Risk.Coverages.First(x => x.Id == item.Id).RateType;
        //            item.LimitAmount = risk.Risk.Coverages.First(x => x.Id == item.Id).LimitAmount;
        //            item.SubLimitAmount = risk.Risk.Coverages.First(x => x.Id == item.Id).SubLimitAmount;
        //            item.EndorsementLimitAmount = risk.Risk.Coverages.First(x => x.Id == item.Id).EndorsementLimitAmount;
        //            item.EndorsementSublimitAmount = risk.Risk.Coverages.First(x => x.Id == item.Id).EndorsementSublimitAmount;
        //            item.CalculationType = risk.Risk.Coverages.First(x => x.Id == item.Id).CalculationType;
        //            item.Deductible = risk.Risk.Coverages.First(x => x.Id == item.Id).Deductible;
        //            item.DynamicProperties = risk.Risk.Coverages.First(x => x.Id == item.Id).DynamicProperties;
        //            item.Number = risk.Risk.Coverages.First(x => x.Id == item.Id).Number;
        //            //   item.PremiumAmount = risk.Coverages.First(x => x.Id == item.Id).PremiumAmount;
        //        }

        //        risk.Risk.Coverages = coverages;
        //        risk.Risk.AmountInsured = risk.Risk.Coverages.Sum(x => x.LimitAmount);
        //    }

        //    return risk;
        //}

        private CompanyVehicle GetDataModification(CompanyVehicle risk, CompanyPolicy vehiclePolicy, CoverageStatusType coverageStatusType)
        {
            if (risk.Risk.Beneficiaries[0].IdentificationDocument == null)
            {
                List<CompanyBeneficiary> beneficiaries = new List<CompanyBeneficiary>();

                foreach (CompanyBeneficiary item in risk.Risk.Beneficiaries)
                {
                    Beneficiary beneficiary = new Beneficiary();
                    beneficiary = DelegateService.underwritingService.GetBeneficiariesByDescription(item.IndividualId.ToString(), Sistran.Core.Services.UtilitiesServices.Enums.InsuredSearchType.IndividualId).FirstOrDefault();
                    item.IdentificationDocument = beneficiary.IdentificationDocument;
                    item.Name = beneficiary.Name;
                }
            }

            if (risk.Risk.Premium == 0)
            {
                List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(vehiclePolicy.Product.Id, risk.Risk.GroupCoverage.Id, vehiclePolicy.Prefix.Id);

                coverages = coverages.Where(c => (risk.Risk.Coverages.Any(x => x.Id == c.Id))).ToList();

                foreach (CompanyCoverage item in coverages)
                {
                    item.RiskCoverageId = risk.Risk.Coverages.First(x => x.Id == item.Id).RiskCoverageId;
                    item.CoverageOriginalStatus = risk.Risk.Coverages.First(x => x.Id == item.Id).CoverageOriginalStatus;
                    item.AccumulatedPremiumAmount = risk.Risk.Coverages.First(x => x.Id == item.Id).AccumulatedPremiumAmount;
                    item.OriginalLimitAmount = risk.Risk.Coverages.First(x => x.Id == item.Id).OriginalLimitAmount;
                    item.OriginalSubLimitAmount = risk.Risk.Coverages.First(x => x.Id == item.Id).OriginalSubLimitAmount;
                    item.CoverStatus = coverageStatusType;
                    item.CoverStatusName = EnumsHelper.GetItemName<CoverageStatusType>(coverageStatusType);
                    item.EndorsementType = vehiclePolicy.Endorsement.EndorsementType;
                    item.CurrentFrom = vehiclePolicy.CurrentFrom;
                    item.CurrentTo = vehiclePolicy.CurrentTo;
                    item.Rate = risk.Risk.Coverages.First(x => x.Id == item.Id).Rate;
                    item.RateType = risk.Risk.Coverages.First(x => x.Id == item.Id).RateType;
                    item.LimitAmount = risk.Risk.Coverages.First(x => x.Id == item.Id).LimitAmount;
                    item.SubLimitAmount = risk.Risk.Coverages.First(x => x.Id == item.Id).SubLimitAmount;
                    item.EndorsementLimitAmount = risk.Risk.Coverages.First(x => x.Id == item.Id).EndorsementLimitAmount;
                    item.EndorsementSublimitAmount = risk.Risk.Coverages.First(x => x.Id == item.Id).EndorsementSublimitAmount;
                    item.Deductible = risk.Risk.Coverages.First(x => x.Id == item.Id).Deductible;
                    item.DynamicProperties = risk.Risk.Coverages.First(x => x.Id == item.Id).DynamicProperties;
                    item.Number = risk.Risk.Coverages.First(x => x.Id == item.Id).Number;
                    //  item.PremiumAmount = risk.Coverages.First(x => x.Id == item.Id).PremiumAmount;
                }

                risk.Risk.Coverages = coverages;
                risk.Risk.AmountInsured = risk.Risk.Coverages.Sum(x => x.LimitAmount);

            }

            return risk;
        }
        public ActionResult GetLoadTypes(int endorsementType)
        {
            try
            {
                List<LoadType> loadTypes = new List<LoadType> {
                    new LoadType { Id = 3, Description = "COLECTIVA", ProcessType = MassiveProcessType.Emission },
                    new LoadType { Id = 4, Description = "INCLUSION", ProcessType = MassiveProcessType.Modification },
                    new LoadType { Id = 5, Description = "EXCLUSION", ProcessType = MassiveProcessType.Modification },
                    new LoadType { Id = 7, Description = "RENOVACIÓN", ProcessType = MassiveProcessType.Renewal } };
                return new UifJsonResult(true, loadTypes);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryUploadTypes);
            }
        }
        #region tarifacion
        public ActionResult TariffedLoad(CollectiveModelView collectiveViewModel)
        {
            try
            {
                switch (collectiveViewModel.LoadTypeId)
                {
                    case (int)SubMassiveProcessType.CollectiveEmission:
                        QuoteColective(collectiveViewModel);
                        break;
                    case (int)SubMassiveProcessType.Inclusion:
                    case (int)SubMassiveProcessType.Exclusion:
                        ModificationQuotate(collectiveViewModel);
                        break;
                    case (int)SubMassiveProcessType.CollectiveRenewal:
                        QuoteRenewalCollective(collectiveViewModel);
                        break;
                    default:
                        break;
                }
                return new UifJsonResult(true, App_GlobalResources.Language.InitiatedTariff);

            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.InitiatedTariff);
            }
        }
        private void QuoteColective(CollectiveModelView collectiveViewModel)
        {
            CompanyProduct product = DelegateService.productService.GetCompanyProductByProductIdPrefixId(collectiveViewModel.ProductId, collectiveViewModel.PrefixId);

            switch (product.CoveredRisk.SubCoveredRiskType.Value)
            {
                case SubCoveredRiskType.ThirdPartyLiability:
                    //DelegateService.thirdPartyLiabilityCollectiveService.QuotateCollectiveEmission(collectiveViewModel.CollectiveLoads);
                    break;
                case SubCoveredRiskType.Vehicle:
                    DelegateService.vehicleCollectiveService.QuotateCollectiveLoad(collectiveViewModel.CollectiveLoads);
                    break;
                case SubCoveredRiskType.Liability:
                    //DelegateService.liabilityCollectiveService.QuotateCollectiveLoad(collectiveViewModel.CollectiveLoads);
                    break;
                case SubCoveredRiskType.Property:
                    //DelegateService.propertyCollectiveService.QuotateCollectiveEmission(collectiveViewModel.CollectiveLoads);
                    break;
                default:
                    break;
            }
        }
        private void QuoteRenewalCollective(CollectiveModelView collectiveViewModel)
        {
            CompanyProduct product = DelegateService.productService.GetCompanyProductByProductIdPrefixId(collectiveViewModel.ProductId, collectiveViewModel.PrefixId);

            switch (product.CoveredRisk.SubCoveredRiskType.Value)
            {
                case SubCoveredRiskType.ThirdPartyLiability:
                    //DelegateService.thirdPartyLiabilityCollectiveService.QuotateCollectiveEmission(collectiveViewModel.CollectiveLoads);
                    break;
                case SubCoveredRiskType.Vehicle:
                    //DelegateService.collectiveVehicleRenewalService.QuotateCollectiveLoads(collectiveViewModel.CollectiveLoads);
                    break;
                case SubCoveredRiskType.Liability:
                    //DelegateService.liabilityCollectiveService.QuotateCollectiveLoad(collectiveViewModel.CollectiveLoads);
                    break;
                case SubCoveredRiskType.Property:
                    //DelegateService.collectivePropertyRenewalService.QuotateCollectiveLoads(collectiveViewModel.CollectiveLoads);
                    break;
                default:
                    break;
            }
        }
        private ActionResult ModificationQuotate(CollectiveModelView collectiveViewModel)
        {
            CompanyProduct product = DelegateService.productService.GetCompanyProductByProductIdPrefixId(collectiveViewModel.ProductId, collectiveViewModel.PrefixId);

            switch (product.CoveredRisk.SubCoveredRiskType.Value)
            {
                case SubCoveredRiskType.JudicialSurety:
                    break;
                case SubCoveredRiskType.Liability:
                    //massiveLoad = DelegateService.massiveliabilityService.QuotateMassiveLoad(massiveEmission.Id);
                    break;
                case SubCoveredRiskType.Property:
                    if (collectiveViewModel.LoadTypeId == (int)SubMassiveProcessType.Inclusion)
                    {
                        //DelegateService.collectivePropertyModificationService.QuotateCollectiveInclusion(collectiveViewModel.CollectiveLoads);
                    }
                    else if (collectiveViewModel.LoadTypeId == (int)SubMassiveProcessType.Exclusion)
                    {
                        //DelegateService.collectivePropertyModificationService.QuotateCollectiveExclusion(collectiveViewModel.CollectiveLoads);
                    }
                    break;
                case SubCoveredRiskType.Surety:
                    break;
                case SubCoveredRiskType.ThirdPartyLiability:
                    //massiveLoad = DelegateService.massiveThirdPartyLiabilityService.QuotateMassiveLoad(massiveEmission.Id);
                    break;
                case SubCoveredRiskType.Vehicle:
                    if (collectiveViewModel.LoadTypeId == (int)SubMassiveProcessType.Exclusion)
                    {
                        //DelegateService.collectiveVehicleModificationService.QuotateCollectiveExclusion(collectiveViewModel.CollectiveLoads, Convert.ToInt32(collectiveViewModel.TempId));
                    }
                    else
                    {
                        //DelegateService.collectiveVehicleModificationService.QuotateCollectiveInclution(collectiveViewModel.CollectiveLoads);

                    }
                    break;
                default:
                    break;
            }
            return new UifJsonResult(true, App_GlobalResources.Language.InitiatedTariff);
        }
        #endregion
        public ActionResult CreateIssuePolicy(CollectiveModelView collectiveModelView)
        {
            try
            {
                IssuedCollectiveLoad issuedCollectiveLoad = DelegateService.collectiveService.CreateIssuedPolicy(collectiveModelView.CollectiveLoads, int.Parse(collectiveModelView.TempId));

                return new UifJsonResult(true, issuedCollectiveLoad);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.Issue);
            }
        }
        public ActionResult IssuePolicy(CollectiveModelView collectiveModelView)
        {
            try
            {
                string message = String.Empty;
                if ((SubMassiveProcessType)collectiveModelView.LoadTypeId == SubMassiveProcessType.CollectiveEmission)
                {
                    //CompanyPolicy companyPolicy = DelegateService.collectiveService.IssuanceCollectiveEmission(Convert.ToInt32(collectiveModelView.TempId));
                    //if (companyPolicy != null && companyPolicy.DocumentNumber > 0)
                    //{
                    //    message = string.Format(App_GlobalResources.Language.PolicyNumber, companyPolicy.DocumentNumber.ToString());
                    //}
                    //else
                    //{
                    //    return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreatePolicy);
                    //}
                }
                else if ((SubMassiveProcessType)collectiveModelView.LoadTypeId == SubMassiveProcessType.Inclusion ||
                        (SubMassiveProcessType)collectiveModelView.LoadTypeId == SubMassiveProcessType.Exclusion ||
                        (SubMassiveProcessType)collectiveModelView.LoadTypeId == SubMassiveProcessType.CollectiveRenewal)
                {
                    //int documentNumber = DelegateService.collectiveService.IssuanceCollectiveEndorsement(Convert.ToInt32(collectiveModelView.TempId));
                    //message = string.Format("{0} : {1}", App_GlobalResources.Language.Endorsement, documentNumber.ToString());
                }

                return new UifJsonResult(true, message);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }
        #region temporales
        /// <summary>
        /// Genera el temporal.
        /// </summary>        
        /// <returns></returns>            
        public ActionResult SetTemporal(CompanyPolicy temporalModel)
        {
            try
            {
                temporalModel.UserId = SessionHelper.GetUserId();
                CompanyPolicy companyPolicy = new CompanyPolicy();
                companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(temporalModel.Endorsement.Id);
                companyPolicy.UserId = SessionHelper.GetUserId();
                companyPolicy.SubEndorsementType = temporalModel.SubEndorsementType;
                companyPolicy.Endorsement.EndorsementType = EndorsementType.Modification;
                companyPolicy.Endorsement.Text = new CompanyText
                {
                    TextBody = temporalModel.Text.TextBody,
                    Observations = temporalModel.Text.Observations
                };
                companyPolicy.Endorsement.EndorsementTypeDescription = EnumsHelper.GetItemName<EndorsementType>(companyPolicy.Endorsement.EndorsementType);
                companyPolicy.TemporalType = TemporalType.Endorsement;
                companyPolicy.TemporalTypeDescription = EnumsHelper.GetItemName<TemporalType>(companyPolicy.TemporalType);

                CompanyProduct product = DelegateService.productService.GetCompanyProductByProductIdPrefixId(companyPolicy.Product.Id, companyPolicy.Prefix.Id);
                companyPolicy.Product = product;
                companyPolicy.EffectPeriod = DelegateService.commonService.GetExtendedParameterByParameterId(1027).NumberParameter.Value;

                var imapp = ModelAssembler.CreateMappClauses();
                companyPolicy.Clauses = imapp.Map<List<Clause>, List<CompanyClause>>(DelegateService.underwritingService.GetClausesByEmissionLevelConditionLevelId(Core.Application.UnderwritingServices.Enums.EmissionLevel.General, companyPolicy.Prefix.Id).Where(x => x.IsMandatory).ToList());



                /*Actualiza el pending operation de la poliza*/
                companyPolicy.Summary = new CompanySummary
                {
                    RiskCount = 0
                };
                companyPolicy = DelegateService.underwritingService.CreatePolicyTemporal(companyPolicy, true);
                /*switch (product.CoveredRisk.SubCoveredRiskType.Value)
                {
                    case SubCoveredRiskType.ThirdPartyLiability:
                        List<CompanyTplRisk> companyTplRisks = DelegateService.thirdPartyLiabilityService.GetCompanyThirdPartyLiabilitiesByPolicyId(temporalModel.Id);

                        foreach (CompanyTplRisk item in companyTplRisks)
                        {
                            CompanyTplRisk risk = item;
                            risk = GetDataModification(risk, companyPolicy, CoverageStatusType.NotModified);
                            
                            risk.Policy = companyPolicy;
                            DelegateService.thirdPartyLiabilityService.CreateThirdPartyLiabilityTemporal(risk, true);
                        }
                        break;
                    case SubCoveredRiskType.Vehicle:
                        List<CompanyVehicle> companyVehicles = DelegateService.vehicleService.GetCompanyVehiclesByPolicyId(temporalModel.Id);

                        foreach (CompanyVehicle item in companyVehicles)
                        {
                            CompanyVehicle risk = GetDataModification(item, companyPolicy, CoverageStatusType.NotModified);
                            
                            risk.Policy = companyPolicy;
                            DelegateService.vehicleService.CreateVehicleTemporal(risk, true);
                        }
                        break;
                    case SubCoveredRiskType.Liability:

                        break;
                    case SubCoveredRiskType.Property:
                        List<CompanyPropertyRisk> propertyPolicy = DelegateService.propertyService.GetCompanyPropertiesByPolicyId(temporalModel.Id);
                        foreach (CompanyPropertyRisk item in propertyPolicy)
                        {
                            item.Policy = companyPolicy;
                            CompanyPropertyRisk propertyRisk = item;
                            propertyRisk = GetDataModification(propertyRisk, companyPolicy, CoverageStatusType.NotModified);

                            propertyRisk = DelegateService.propertyService.CreatePropertyTemporal(propertyRisk, true);

                        }
                        break;
                    default:
                        break;
                }*/
                return new UifJsonResult(true, companyPolicy);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }
        private CompanyPropertyRisk GetDataModification(CompanyPropertyRisk risk, CompanyPolicy propertyPolicy, CoverageStatusType statusCoverage)
        {
            List<CompanyPropertyRisk> CompanyPropertyRisk = new List<CompanyPropertyRisk>();
            if (risk.Risk.Beneficiaries[0].IdentificationDocument == null)
            {
                foreach (CompanyBeneficiary item in risk.Risk.Beneficiaries)
                {
                    Beneficiary beneficiary = new Beneficiary();
                    beneficiary = DelegateService.underwritingService.GetBeneficiariesByDescription(item.IndividualId.ToString(), Sistran.Core.Services.UtilitiesServices.Enums.InsuredSearchType.IndividualId).FirstOrDefault();
                    item.IdentificationDocument = beneficiary.IdentificationDocument;
                    item.Name = beneficiary.Name;
                }
            }
            if (risk.Risk.RiskActivity != null)
            {
                var config = MapperCache.GetMapper<RiskActivity, CompanyRiskActivity>(cfg =>
                {
                    cfg.CreateMap<RiskActivity, CompanyRiskActivity>();
                });
                risk.Risk.RiskActivity = config.Map<RiskActivity, CompanyRiskActivity>(DelegateService.underwritingService.GetRiskActivityByActivityId(risk.Risk.RiskActivity.Id));
            }
            List<CompanyInsuredObject> insuredObjects = DelegateService.underwritingService.GetCompanyInsuredObjectsByRiskId(risk.Risk.RiskId);
            List<CompanyInsuredObject> originalInsuredObjects = DelegateService.underwritingService.GetCompanyInsuredObjectsByProductIdGroupCoverageId(propertyPolicy.Product.Id, risk.Risk.GroupCoverage.Id, propertyPolicy.Prefix.Id);
            originalInsuredObjects = originalInsuredObjects.Where(c => (insuredObjects.Any(x => x.Id == c.Id))).ToList();
            foreach (CompanyInsuredObject insuredObject in originalInsuredObjects)
            {
                List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByInsuredObjectIdGroupCoverageIdProductId(insuredObject.Id, risk.Risk.GroupCoverage.Id, propertyPolicy.Product.Id);
                List<CompanyCoverage> originalCoverages = coverages.Where(c => (risk.Risk.Coverages.Any(x => x.Id == c.Id))).ToList();
                foreach (CompanyCoverage item in risk.Risk.Coverages)
                {
                    if (item.CoverStatus == null && originalCoverages.Exists(u => u.Id == item.Id))
                    {
                        item.Description = originalCoverages.FirstOrDefault(u => u.Id == item.Id).Description;
                        item.CoverStatus = statusCoverage;
                        item.CoverStatusName = EnumsHelper.GetItemName<CoverageStatusType>(statusCoverage);
                        item.EndorsementType = propertyPolicy.Endorsement.EndorsementType;
                        item.AccumulatedPremiumAmount = 0;
                        item.FlatRatePorcentage = 0;
                        item.SubLineBusiness = coverages.First(x => x.Id == item.Id).SubLineBusiness;
                        item.InsuredObject = insuredObject;
                        item.PremiumAmount = 0;
                        item.InsuredObject.Amount = insuredObjects.FirstOrDefault(u => u.Id == insuredObject.Id).Amount;
                        item.PosRuleSetId = originalCoverages.FirstOrDefault(u => u.Id == item.Id).PosRuleSetId;
                        item.RuleSetId = originalCoverages.FirstOrDefault(u => u.Id == item.Id).RuleSetId;
                        item.IsVisible = coverages.FirstOrDefault(u => u.Id == item.Id).IsVisible;
                        item.IsMandatory = coverages.FirstOrDefault(u => u.Id == item.Id).IsMandatory;
                        item.IsSelected = coverages.FirstOrDefault(u => u.Id == item.Id).IsSelected;
                        item.MainCoverageId = coverages.FirstOrDefault(u => u.Id == item.Id).MainCoverageId;
                        item.CoverageOriginalStatus = risk.Risk.Coverages.First(x => x.Id == item.Id).CoverageOriginalStatus;
                        item.Deductible = risk.Risk.Coverages.First(x => x.Id == item.Id).Deductible;
                        item.DynamicProperties = risk.Risk.Coverages.First(x => x.Id == item.Id).DynamicProperties;
                    }
                }
            }
            risk.Risk.Coverages.ForEach(u => u.CurrentFrom = propertyPolicy.CurrentFrom);
            risk.Risk.Coverages.ForEach(u => u.CurrentTo = propertyPolicy.CurrentTo);
            CompanyPropertyRisk.Add(risk);
            risk = CompanyPropertyRisk[0];
            return risk;
        }
        #endregion

        public ActionResult GetCollectiveEmissionByMassiveLoadId(int massiveLoadId)
        {
            CollectiveEmission collectiveEmission = DelegateService.collectiveService.GetCollectiveEmissionByMassiveLoadId(massiveLoadId, true);
            return new UifJsonResult(true, collectiveEmission);
        }
        /*public SearchViewModel SetSearchViewModel(int temporalId, int branchId, int prefixId,
         string policyNumber, int? endorsementId, string modificationType)
        {
            SearchViewModel searchViewModel = new SearchViewModel();
            searchViewModel.TemporalId = temporalId;
            searchViewModel.BranchId = branchId;
            searchViewModel.PrefixId = prefixId;
            searchViewModel.PolicyNumber = policyNumber;
            searchViewModel.EndorsementId = endorsementId;
            return searchViewModel;
        }*/


        /// <summary>
        /// Genera archivo excel riesgos
        /// </summary>
        /// <returns></returns>   
        public ActionResult GenerateFileToExport(Policy policy, string licensePlate)
        {
            try
            {
                string urlFile = "";
                policy = DelegateService.underwritingService.GetPoliciesByPolicy(policy).FirstOrDefault();
                switch (policy.Product.CoveredRisk.SubCoveredRiskType.Value)
                {
                    case SubCoveredRiskType.Vehicle:
                        //urlFile = DelegateService.vehicleCollectiveService.GenerateFileToVehicles(policy, licensePlate, App_GlobalResources.Language.Vehicle);
                        break;
                    case SubCoveredRiskType.Property:
                        //urlFile = DelegateService.propertyCollectiveService.GenerateFileToLocationRisks(policy, App_GlobalResources.Language.FileNameRisks);
                        break;
                }

                if (string.IsNullOrEmpty(urlFile))
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorFileNotFound);
                }
                else
                {
                    return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
            }

        }

        public ActionResult GenerateFileToCollectiveLoad(CollectiveModelView collectiveModelView)
        {
            try
            {
                string urlFile = null;
                switch ((SubMassiveProcessType)collectiveModelView.LoadTypeId)
                {
                    case SubMassiveProcessType.CollectiveEmission:
                        urlFile = DelegateService.collectiveService.GenerateFileErrorsCollective(collectiveModelView.CollectiveLoads[0], FileProcessType.CollectiveEmission);
                        break;
                    case SubMassiveProcessType.Inclusion:
                        urlFile = DelegateService.collectiveService.GenerateFileErrorsCollective(collectiveModelView.CollectiveLoads[0], FileProcessType.CollectiveInclusion);
                        break;
                    case SubMassiveProcessType.Exclusion:
                        urlFile = DelegateService.collectiveService.GenerateFileErrorsCollective(collectiveModelView.CollectiveLoads[0], FileProcessType.CollectiveExclusion);
                        break;
                    case SubMassiveProcessType.CollectiveRenewal:
                        urlFile = DelegateService.collectiveService.GenerateFileErrorsCollective(collectiveModelView.CollectiveLoads[0], FileProcessType.CollectiveEmission);
                        break;
                    default:
                        break;
                }
                if (string.IsNullOrEmpty(urlFile))
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorFileNotFound);
                }
                else
                {
                    return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
            }
        }

        public ActionResult DeleteProcess(MassiveLoad massiveLoad)
        {
            try
            {
                string result = string.Empty;
                result = DelegateService.massiveService.DeleteProcess(massiveLoad);
                if (result == "")
                {
                    return new UifJsonResult(true, App_GlobalResources.Language.DeleteLoadSuccessfully);
                }
                else
                {
                    return new UifJsonResult(false, result);
                }


            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorDeleteLoad);
            }
        }
        #region
        public ActionResult GenerateReportByMassiveLoadId(int massiveLoadId)
        {
            CollectiveEmission collectiveEmission = DelegateService.collectiveService.GetCollectiveEmissionByMassiveLoadId(massiveLoadId, false);
            collectiveEmission.Product = DelegateService.productService.GetProductByProductIdPrefixId(collectiveEmission.Product.Id, collectiveEmission.Prefix.Id);

            string urlFile = "";
            switch (collectiveEmission.Product.CoveredRisk.SubCoveredRiskType.Value)
            {
                case SubCoveredRiskType.JudicialSurety:
                    break;
                case SubCoveredRiskType.Liability:
                    break;
                case SubCoveredRiskType.Property:
                    //urlFile = DelegateService.propertyCollectiveService.GenerateReportToCollectiveLoad(collectiveEmission, 0);
                    break;
                case SubCoveredRiskType.Surety:
                    break;
                case SubCoveredRiskType.ThirdPartyLiability:
                    break;
                case SubCoveredRiskType.Vehicle:
                    urlFile = DelegateService.vehicleCollectiveService.GenerateReportToCollectiveLoad(collectiveEmission, 0);
                    break;
                default:
                    break;
            }
            if (string.IsNullOrEmpty(urlFile))
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorFileNotFound);
            }
            return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile);
        }
        #endregion
    }
}
