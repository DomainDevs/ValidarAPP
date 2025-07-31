using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Company.Application.MassiveServices.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.MassiveUnderwritingServices.Enums;
using System.Web;
using System.Net;
using System.ComponentModel;
using Sistran.Core.Application.MassiveServices.Enums;
using MSV = Sistran.Core.Framework.UIF.Web.Areas.Massive.Models;
using Sistran.Core.Application.MassiveUnderwritingServices.Models;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.MassiveRenewalServices.Models;
using Sistran.Core.Application.CollectiveServices.Models;
using UnderModel = Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Company.Application.PrintingServices.Models;
using Sistran.Core.Application.PrintingServices.Models;
using System.Threading.Tasks;
using Sistran.Company.Application.UnderwritingServices.Models;
using Newtonsoft.Json;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Enums;
using System.Diagnostics;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System.Configuration;
using System.IO;
using Sistran.Company.Application.CollectionFormBusinessService.Models;

namespace Sistran.Core.Framework.UIF.Web.Areas.Massive.Controllers
{
    [Authorize]
    public class MassiveController : Controller
    {
        public ActionResult Massive()
        {
            return View();
        }

        public PartialViewResult MassiveAdvancedSearch()
        {
            return PartialView();
        }

        public ActionResult GetProcessTypes()
        {
            try
            {
                return new UifJsonResult(true, EnumsHelper.GetItems<MassiveProcessType>());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryUploadTypes);
            }
        }

        public ActionResult GetLoadTypes(int processTypeId)
        {
            try
            {
                List<LoadType> loadTypes = DelegateService.massiveService.GetLoadTypesByMassiveProcessType((MassiveProcessType)processTypeId);
                return new UifJsonResult(true, loadTypes);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryUploadTypes);
            }
        }

        public ActionResult GetBranches()
        {
            try
            {
                List<Branch> branches = DelegateService.uniqueUserService.GetBranchesByUserId(SessionHelper.GetUserId());
                return new UifJsonResult(true, branches.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGettingBranch);
            }
        }

        public ActionResult GetBusinessTypes()
        {
            try
            {
                return new UifJsonResult(true, EnumsHelper.GetItems<BusinessType>());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetBusinessTypes);
            }
        }

        public ActionResult GetBillingGroupsByDescription(string description)
        {
            try
            {
                List<BillingGroup> billingGroups = DelegateService.underwritingService.GetBillingGroupsByDescription(description);

                if (billingGroups.Count == 0)
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchBillingGroup);
                }
                else
                {
                    return new UifJsonResult(true, billingGroups);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveBillingGroup);
            }
        }

        public ActionResult GetCompanyRequestsByBillingGroupIdDescriptionRequestNumber(int billingGroupId, string description, int? requestNumber)
        {
            try
            {
                List<CompanyRequest> companyRequests = DelegateService.massiveService.GetCompanyRequestsByBillingGroupIdDescriptionRequestNumber(billingGroupId, description, requestNumber);

                if (companyRequests.Count == 0)
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryingGroupRequest);
                }
                else
                {
                    return new UifJsonResult(true, companyRequests);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryingGroupRequest);
            }
        }

        public ActionResult GetAgentByIndividualId(int agentId)
        {
            try
            {
                Agent agent = DelegateService.uniquePersonServiceV1.GetAgentByIndividualId(agentId);

                if (agent == null)
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.MessageNotFoundIntermediaries);
                }
                else
                {
                    return new UifJsonResult(true, agent);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchAgent);
            }
        }

        public ActionResult GetAgenciesByUserIdAgentIdDescription(int agentId, string description)
        {
            try
            {
                List<UserAgency> agencies = DelegateService.uniqueUserService.GetAgenciesByUserIdAgentIdDescription(SessionHelper.GetUserId(), agentId, description);

                if (agencies.Count == 0)
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.MessageNotFoundIntermediaries);
                }
                else if (agencies.Count == 1)
                {
                    if (agencies.Exists(x => x.DateDeclined.GetValueOrDefault() > DateTime.MinValue || x.Agent.DateDeclined.GetValueOrDefault() > DateTime.MinValue))
                    {
                        return new UifJsonResult(false, App_GlobalResources.Language.ErrorIntermediaryDisabled);
                    }
                    else
                    {
                        return new UifJsonResult(true, agencies);
                    }
                }
                else
                {
                    return new UifJsonResult(true, agencies);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchAgent);
            }
        }

        public ActionResult GetAgenciesByAgentIdUserId(int agentId)
        {
            try
            {
                List<UserAgency> agencies = DelegateService.uniqueUserService.GetAgenciesByAgentIdUserId(agentId, SessionHelper.GetUserId());

                if (agencies.Count == 0)
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.IntermediateWithoutAgencies);
                }
                else
                {
                    return new UifJsonResult(true, agencies.OrderBy(x => x.FullName).ToList());
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetAgentAgency);
            }
        }

        public ActionResult GetPrefixesByAgentId(int agentId)
        {
            try
            {
                List<Prefix> prefixes = DelegateService.massiveService.GetPrefixesByAgentId(agentId);

                if (prefixes.Count == 0)
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.MessageNotFoundPrefixes);
                }
                else
                {
                    return new UifJsonResult(true, prefixes.OrderBy(x => x.Description).ToList());
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPrefixes);
            }
        }

        /// <summary>
        /// Obtener los ramos comerciales activos para Masivos
        /// </summary>
        /// <returns></returns>
        public ActionResult GetPrefixesToMassive()
        {
            try
            {
                List<Prefix> prefixes = DelegateService.commonService.GetPrefixes();

                if (prefixes.Count == 0)
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.MessageNotFoundPrefixes);
                }
                else
                {
                    return new UifJsonResult(true, prefixes.OrderBy(x => x.Description).ToList());
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPrefixes);
            }
        }

        /// <summary>
        /// Obtener los tipos de Archivos
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCoveredRiskTypeByLineBusinessId(int lineBusinessId)
        {
            try
            {
                return new UifJsonResult(true, EnumsHelper.GetItems<CoveredRiskType>());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryUploadTypes);
            }
        }

        public ActionResult GetProductsByAgentIdPrefixId(int agentId, int prefixId)
        {
            try
            {
                List<Application.ProductServices.Models.Product> products = DelegateService.massiveService.GetProductsByAgentIdPrefixId(agentId, prefixId);

                if (products.Count == 0)
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.MessageNotFoundProducts);
                }
                else
                {
                    return new UifJsonResult(true, products.OrderBy(x => x.Description).ToList());
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetProducts);
            }
        }

        public ActionResult GetCollectiveProductsByAgentIdPrefixId(int agentId, int prefixId)
        {
            try
            {
                List<Application.ProductServices.Models.Product> products = DelegateService.massiveService.GetCollectiveProductsByAgentIdPrefixId(agentId, prefixId);

                if (products.Count == 0)
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.MessageNotFoundProducts);
                }
                else
                {
                    return new UifJsonResult(true, products.OrderBy(x => x.Description).ToList());
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetProducts);
            }
        }

        public ActionResult GetSalePointsByBranchId(int branchId)
        {
            try
            {
                List<SalePoint> salePoints = DelegateService.uniqueUserService.GetSalePointsByBranchIdUserId(branchId, SessionHelper.GetUserId());

                if (salePoints.Count == 0)
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.MessageNotFoundSalePoints);
                }
                else
                {
                    return new UifJsonResult(true, salePoints.OrderBy(x => x.Description).ToList());
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetSalePoints);
            }
        }

        public ActionResult UploadFile()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorCheckRequiredFields);
                }
                else if (Request.Files.Count > 0)
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
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", ex.ToString(), EventLogEntryType.Error);
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorUploadFile);
            }
        }

        public ActionResult CreateLoad(MSV.MassiveViewModel massiveViewModel)
        {
            try
            {
                if (massiveViewModel.ProcessTypeId == (int)MassiveProcessType.Cancellation)
                {
                    ModelState.Clear();
                }
                if (ModelState.IsValid)
                {
                    MassiveLoad result = new MassiveLoad();
                    switch ((MassiveProcessType)massiveViewModel.ProcessTypeId)
                    {
                        case MassiveProcessType.Emission:
                            if ((SubMassiveProcessType)massiveViewModel.LoadTypeId == SubMassiveProcessType.CollectiveEmission)
                            {
                                result = CreateCollectiveEmission(massiveViewModel);
                            }
                            else
                            {
                                result = CreateMassiveEmission(massiveViewModel);
                            }
                            break;
                        case MassiveProcessType.Modification:
                            result = CreateModification(massiveViewModel);
                            break;
                        case MassiveProcessType.Renewal:
                            switch ((SubMassiveProcessType)massiveViewModel.LoadTypeId)
                            {
                                case SubMassiveProcessType.MassiveRenewal:
                                    result = CreateMassiveRenewal(massiveViewModel);
                                    break;
                                case SubMassiveProcessType.CollectiveRenewal:
                                    result = CreateCollectiveRenewal(massiveViewModel);
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case MassiveProcessType.Cancellation:
                            result = CreateCancellation(massiveViewModel);
                            break;
                        default:
                            break;
                    }
                    if (result.HasError)
                    {
                        return new UifJsonResult(false, ExceptionHelper.GetMessage(result.ErrorDescription));
                    }
                    else
                    {
                        return new UifJsonResult(true, App_GlobalResources.Language.MessageGeneratedLoadNo + " " + result.Id);
                    }
                }
                else
                {
                    return new UifJsonResult(false, ViewModelError.GetMessages(ModelState.Values));
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreatingUpload + " " + ex.Message);
            }
        }

        private MassiveLoad CreateMassiveEmission(MSV.MassiveViewModel massiveViewModel)
        {
            MassiveEmission massiveEmission = MSV.ModelAssembler.CreateMassiveEmission(massiveViewModel);
            var rt = DelegateService.commonService.GetSubCoveredRiskTypeByPrefixIdCoveredRiskTypeId(massiveEmission.Prefix.Id, (int)massiveEmission.CoveredRiskType);

            switch (rt)
            {
                case SubCoveredRiskType.JudicialSurety:
                    break;
                case SubCoveredRiskType.Liability:
                    //massiveEmission = DelegateService.massiveLiabilityService.CreateMassiveEmission(massiveEmission);
                    break;
                case SubCoveredRiskType.Property:
                    //massiveEmission = DelegateService.massivePropertyService.CreateMassiveEmission(massiveEmission);
                    break;
                case SubCoveredRiskType.Surety:
                    break;
                case SubCoveredRiskType.ThirdPartyLiability:
                    massiveEmission = DelegateService.massiveThirdPartyLiabilityService.CreateMassiveEmission(massiveEmission);
                    break;
                case SubCoveredRiskType.Vehicle:
                    massiveEmission = DelegateService.massiveVehicleService.CreateMassiveEmission(massiveEmission);
                    break;
            }

            return massiveEmission;
        }

        public string ValidateMessage(string inputMessage)
        {
            //string messageToSearch= App_GlobalResources.Language.ErrorTemplateLocationIsRequired;

            //bool b = inputMessage.Contains(messageToSearch);
            if (inputMessage.Contains(App_GlobalResources.Language.ErrorTemplateLocationIsRequired))
            {
                return App_GlobalResources.Language.ErrorTemplateDoesNotCorrespondToBranch;
            }
            else if (inputMessage.Contains(App_GlobalResources.Language.ErrorTemplateRiskDetailIsRequired))
            {
                return App_GlobalResources.Language.ErrorTemplateDoesNotCorrespondToBranch;
            }
            else if (inputMessage.Contains(App_GlobalResources.Language.ErrorTemplateEmissionIsRequired))
            {
                return App_GlobalResources.Language.ErrorTemplateDoesNotCorrespondToBranch;
            }
            else
            {
                return string.Empty;
            }
        }

        private MassiveLoad CreateCollectiveEmission(MSV.MassiveViewModel massiveViewModel)
        {
            CollectiveEmission collectiveEmission = MSV.ModelAssembler.CreateCollectiveEmission(massiveViewModel);
            collectiveEmission.IsAutomatic = true;
            collectiveEmission.User = new User
            {
                UserId = SessionHelper.GetUserId(),
                AccountName = SessionHelper.GetUserName(),
                AuthenticationType = Application.UniqueUserServices.Enums.UniqueUserTypes.AuthenticationType.Standard
            };
            SubCoveredRiskType riskType = DelegateService.commonService.GetSubCoveredRiskTypeByPrefixIdCoveredRiskTypeId(collectiveEmission.Prefix.Id, (int)collectiveEmission.CoveredRiskType);
            switch (riskType)
            {
                case SubCoveredRiskType.ThirdPartyLiability:
                    collectiveEmission = DelegateService.thirdPartyLiabilityCollectiveService.CreateCollectiveLoad(collectiveEmission);
                    break;
                case SubCoveredRiskType.Liability:
                    //collectiveEmission = DelegateService.liabilityCollectiveService.CreateCollectiveLoad(collectiveEmission);
                    break;
                case SubCoveredRiskType.Vehicle:
                    collectiveEmission = DelegateService.vehicleCollectiveService.CreateCollectiveLoad(collectiveEmission);
                    break;
                case SubCoveredRiskType.Property:
                    //collectiveEmission = DelegateService.propertyCollectiveService.CreateCollectiveEmission(collectiveEmission);
                    break;
            }

            return collectiveEmission;
        }

        private MassiveLoad CreateModification(MSV.MassiveViewModel massiveViewModel)
        {
            CollectiveEmission collectiveEmission = MSV.ModelAssembler.CreateCollectiveEmission(massiveViewModel);
            collectiveEmission.IsAutomatic = true;
            collectiveEmission.User = new User
            {
                UserId = SessionHelper.GetUserId(),
                AccountName = SessionHelper.GetUserName(),
                AuthenticationType = Application.UniqueUserServices.Enums.UniqueUserTypes.AuthenticationType.Standard
            };

            SubCoveredRiskType riskType = DelegateService.commonService.GetSubCoveredRiskTypeByPrefixIdCoveredRiskTypeId(collectiveEmission.Prefix.Id, (int)collectiveEmission.CoveredRiskType);

            switch (riskType)
            {
                case SubCoveredRiskType.ThirdPartyLiability:
                    collectiveEmission = DelegateService.thirdPartyLiabilityModificationService.CreateCollectiveModification(collectiveEmission);
                    break;
                case SubCoveredRiskType.Vehicle:
                    collectiveEmission = DelegateService.collectiveVehicleModificationService.CreateCollectiveModification(collectiveEmission);
                    break;
                case SubCoveredRiskType.Property:
                    //collectiveEmission = DelegateService.collectivePropertyModificationService.CreateCollectiveModification(collectiveEmission);
                    break;
                case SubCoveredRiskType.Liability:
                    //collectiveEmission = DelegateService.collectiveLiabilityModificationService.CreateCollectiveModification(collectiveEmission);
                    break;
            }

            return collectiveEmission;
        }

        private MassiveLoad CreateMassiveRenewal(MSV.MassiveViewModel massiveViewModel)
        {
            MassiveRenewal massiveRenewal = MSV.ModelAssembler.CreateMassiveRenewal(massiveViewModel);
            var rt = DelegateService.commonService.GetSubCoveredRiskTypeByPrefixIdCoveredRiskTypeId(massiveRenewal.Prefix.Id, (int)massiveRenewal.CoveredRiskType);

            switch (rt)
            {
                case SubCoveredRiskType.JudicialSurety:
                    break;
                case SubCoveredRiskType.Liability:
                    //massiveRenewal = DelegateService.massiveLiabilityService.CreateMassiveRenewal(massiveRenewal);
                    break;
                case SubCoveredRiskType.Property:
                    //massiveRenewal = DelegateService.massivePropertyService.CreateMassiveRenewal(massiveRenewal);
                    break;
                case SubCoveredRiskType.Surety:
                    break;
                case SubCoveredRiskType.ThirdPartyLiability:
                    massiveRenewal = DelegateService.massiveThirdPartyLiabilityService.CreateMassiveRenewal(massiveRenewal);
                    break;
                case SubCoveredRiskType.Vehicle:
                    massiveRenewal = DelegateService.massiveVehicleService.CreateMassiveRenewal(massiveRenewal);
                    break;
            }

            return massiveRenewal;
        }

        private MassiveLoad CreateCollectiveRenewal(MSV.MassiveViewModel massiveViewModel)
        {
            CollectiveEmission collectiveEmission = MSV.ModelAssembler.CreateCollectiveEmission(massiveViewModel);
            collectiveEmission.IsAutomatic = true;
            collectiveEmission.User = new User
            {
                UserId = SessionHelper.GetUserId(),
                AccountName = SessionHelper.GetUserName(),
                AuthenticationType = Application.UniqueUserServices.Enums.UniqueUserTypes.AuthenticationType.Standard
            };

            SubCoveredRiskType riskType = DelegateService.commonService.GetSubCoveredRiskTypeByPrefixIdCoveredRiskTypeId(collectiveEmission.Prefix.Id, (int)collectiveEmission.CoveredRiskType);

            switch (riskType)
            {
                case SubCoveredRiskType.ThirdPartyLiability:
                    collectiveEmission = DelegateService.collectiveThirdPartyLiabilityRenewalService.CreateCollectiveRenewal(collectiveEmission);
                    break;
                case SubCoveredRiskType.Vehicle:
                    collectiveEmission = DelegateService.collectiveVehicleRenewalService.CreateCollectiveRenewal(collectiveEmission);
                    break;
                case SubCoveredRiskType.Property:
                    //collectiveEmission = DelegateService.collectivePropertyRenewalService.CreateCollectiveRenewal(collectiveEmission);
                    break;
                case SubCoveredRiskType.Liability:
                    //collectiveEmission = DelegateService.collectiveLiabilityRenewalService.CreateCollectiveRenewal(collectiveEmission);
                    break;
            }
            return collectiveEmission;
        }

        private MassiveLoad CreateCancellation(MSV.MassiveViewModel massiveViewModel)
        {
            MassiveLoad massiveLoad = MSV.ModelAssembler.CreateCancellationMassiveLoad(massiveViewModel);
            massiveLoad.User = new User
            {
                UserId = SessionHelper.GetUserId(),
                AccountName = SessionHelper.GetUserName(),
                AuthenticationType = Application.UniqueUserServices.Enums.UniqueUserTypes.AuthenticationType.Standard
            };
            massiveLoad.Description = App_GlobalResources.Language.MassiveCancellation;
            massiveLoad = DelegateService.CancellationMassiveEndorsementServices.CreateMassiveLoad(massiveLoad);

            return massiveLoad;

        }

        private MassiveLoad QuotateMassive(MassiveLoad massiveLoad)
        {
            MassiveEmission massiveEmission = DelegateService.massiveUnderwritingService.GetMassiveEmissionByMassiveLoadId(massiveLoad.Id);
            var rt = DelegateService.commonService.GetSubCoveredRiskTypeByPrefixIdCoveredRiskTypeId(massiveEmission.Prefix.Id, (int)massiveEmission.CoveredRiskType);

            switch (rt)
            {
                case SubCoveredRiskType.JudicialSurety:
                    break;
                case SubCoveredRiskType.Liability:
                    //massiveLoad = DelegateService.massiveLiabilityService.QuotateMassiveLoad(massiveEmission.Id);
                    break;
                case SubCoveredRiskType.Property:
                    //massiveLoad = DelegateService.massivePropertyService.QuotateMassiveLoad(massiveLoad);
                    break;
                case SubCoveredRiskType.Surety:
                    break;
                case SubCoveredRiskType.ThirdPartyLiability:
                    massiveLoad = DelegateService.massiveThirdPartyLiabilityService.QuotateMassiveLoad(massiveLoad);
                    break;
                case SubCoveredRiskType.Vehicle:
                    massiveLoad = DelegateService.massiveVehicleService.QuotateMassiveLoad(massiveLoad);
                    break;
                default:
                    break;
            }
            return massiveLoad;
        }

        private MassiveLoad CollectiveQuotate(MassiveLoad massiveLoad)
        {
            CollectiveEmission collectiveEmission = DelegateService.collectiveService.GetCollectiveEmissionByMassiveLoadId(massiveLoad.Id, true);
            var rt = DelegateService.commonService.GetSubCoveredRiskTypeByPrefixIdCoveredRiskTypeId(collectiveEmission.Prefix.Id, (int)collectiveEmission.CoveredRiskType);

            switch (rt)
            {
                case SubCoveredRiskType.ThirdPartyLiability:
                    DelegateService.thirdPartyLiabilityCollectiveService.QuotateMassiveCollectiveEmission(massiveLoad);
                    break;
                case SubCoveredRiskType.Vehicle:
                    DelegateService.vehicleCollectiveService.QuotateMassiveCollectiveEmission(massiveLoad);
                    break;
                case SubCoveredRiskType.Liability:
                    //DelegateService.liabilityCollectiveService.QuotateMassiveCollectiveEmission(massiveLoad.Id);
                    break;
                case SubCoveredRiskType.Property:
                    //DelegateService.propertyCollectiveService.QuotateMassiveCollectiveEmission(massiveLoad);
                    break;
                default:
                    break;
            }
            return massiveLoad;
        }

        private MassiveLoad ModificationQuotate(MassiveLoad collectiveLoad)
        {
            CollectiveEmission collectiveModification = DelegateService.collectiveService.GetCollectiveEmissionByMassiveLoadId(collectiveLoad.Id, false);
            var rt = DelegateService.commonService.GetSubCoveredRiskTypeByPrefixIdCoveredRiskTypeId(collectiveModification.Prefix.Id, (int)collectiveModification.CoveredRiskType);
            switch (rt)
            {
                case SubCoveredRiskType.JudicialSurety:
                    break;
                case SubCoveredRiskType.Liability:
                    if (collectiveLoad.LoadType.Id == (int)SubMassiveProcessType.Inclusion)
                    {
                        //DelegateService.collectiveLiabilityModificationService.QuotateCollectiveInclusion(collectiveLoad.Id);
                    }
                    else if (collectiveLoad.LoadType.Id == (int)SubMassiveProcessType.Exclusion)
                    {
                        //DelegateService.collectiveLiabilityModificationService.QuotateCollectiveExclusion(collectiveLoad.Id);
                    }
                    break;
                case SubCoveredRiskType.Property:
                    if (collectiveLoad.LoadType.Id == (int)SubMassiveProcessType.Inclusion)
                    {
                        //DelegateService.collectivePropertyModificationService.QuotateCollectiveInclusion(collectiveLoad);
                    }
                    else if (collectiveLoad.LoadType.Id == (int)SubMassiveProcessType.Exclusion)
                    {
                        //DelegateService.collectivePropertyModificationService.QuotateCollectiveExclusion(collectiveLoad);
                    }
                    break;
                case SubCoveredRiskType.Surety:
                    break;
                case SubCoveredRiskType.ThirdPartyLiability:
                    if (collectiveLoad.LoadType.Id == (int)SubMassiveProcessType.Exclusion)
                    {
                        DelegateService.thirdPartyLiabilityModificationService.QuotateCollectiveExclusion(collectiveLoad);
                    }
                    else
                    {
                        DelegateService.thirdPartyLiabilityModificationService.QuotateCollectiveIncluition(collectiveLoad);
                    }
                    break;
                case SubCoveredRiskType.Vehicle:
                    if (collectiveLoad.LoadType.Id == (int)SubMassiveProcessType.Exclusion)
                    {
                        DelegateService.collectiveVehicleModificationService.QuotateCollectiveExclusion(collectiveLoad);
                    }
                    else
                    {
                        DelegateService.collectiveVehicleModificationService.QuotateCollectiveIncluition(collectiveLoad);
                    }
                    break;
                default:
                    break;
            }
            return collectiveLoad;
        }

        private MassiveLoad MassiveRenewalQuotate(MassiveLoad massiveLoad)
        {

            MassiveRenewal massiveRenewal = DelegateService.massiveRenewalService.GetMassiveRenewalByMassiveRenewalId(massiveLoad.Id, false, false, false);
            var rt = DelegateService.commonService.GetSubCoveredRiskTypeByPrefixIdCoveredRiskTypeId(massiveRenewal.Prefix.Id, (int)massiveRenewal.CoveredRiskType);

            switch (rt)
            {
                case SubCoveredRiskType.JudicialSurety:
                    break;
                case SubCoveredRiskType.Liability:
                    //massiveLoad = DelegateService.massiveLiabilityService.QuotateMassiveRenewal(massiveRenewal.Id);
                    break;
                case SubCoveredRiskType.Property:
                    //massiveLoad = DelegateService.massivePropertyService.QuotateMassiveRenewal(massiveLoad);
                    break;
                case SubCoveredRiskType.Surety:
                    break;
                case SubCoveredRiskType.ThirdPartyLiability:
                    massiveLoad = DelegateService.massiveThirdPartyLiabilityService.QuotateMassiveRenewal(massiveLoad);
                    break;
                case SubCoveredRiskType.Vehicle:
                    massiveLoad = DelegateService.massiveVehicleService.QuotateMassiveRenewal(massiveLoad);
                    break;
                default:
                    break;
            }
            return massiveLoad;
        }

        private MassiveLoad CollectiveRenewalQuotate(MassiveLoad massiveLoad)
        {
            CollectiveEmission collectiveEmission = DelegateService.collectiveService.GetCollectiveEmissionByMassiveLoadId(massiveLoad.Id, false);
            var rt = DelegateService.commonService.GetSubCoveredRiskTypeByPrefixIdCoveredRiskTypeId(collectiveEmission.Prefix.Id, (int)collectiveEmission.CoveredRiskType);

            switch (rt)
            {
                case SubCoveredRiskType.ThirdPartyLiability:
                    DelegateService.collectiveThirdPartyLiabilityRenewalService.QuotateCollectiveRenewal(massiveLoad);
                    break;
                case SubCoveredRiskType.Vehicle:
                    DelegateService.collectiveVehicleRenewalService.QuotateCollectiveRenewal(massiveLoad);
                    break;
                case SubCoveredRiskType.Liability:
                    //DelegateService.collectiveLiabilityRenewalService.QuotateCollectiveRenewal(massiveLoad.Id);
                    break;
                case SubCoveredRiskType.Property:
                    //DelegateService.collectivePropertyRenewalService.QuotateCollectiveRenewal(massiveLoad);
                    break;
                default:
                    break;
            }
            return massiveLoad;
        }

        private MassiveLoad CancellationQuotate(MassiveLoad massiveLoad)
        {
            massiveLoad = DelegateService.CancellationMassiveEndorsementServices.CancellationMassiveByMassiveLoadId(massiveLoad);
            return massiveLoad;
        }

        public ActionResult GetMassiveLoadsByDescription(string description)
        {
            try
            {
                List<MassiveLoad> massiveLoads = DelegateService.massiveService.GetMassiveLoadsByDescription(description);

                if (massiveLoads == null)
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.NoChargesFound);
                }
                else if (massiveLoads[0].HasError)
                {
                    massiveLoads.ForEach(x => x.StatusDescription = EnumsHelper.GetItemName<MassiveLoadStatus>(x.Status));
                    return new UifJsonResult(false, massiveLoads);
                }
                else
                {
                    if (massiveLoads[0].Status == MassiveLoadStatus.Querying)
                    {
                        DelegateService.massiveService.UpdateMassiveLoadStatusIfComplete(Convert.ToInt32(description));
                    }
                    massiveLoads.ForEach(x => x.StatusDescription = EnumsHelper.GetItemName<MassiveLoadStatus>(x.Status));
                    return new UifJsonResult(true, massiveLoads);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(ex.Message));
            }
        }

        public ActionResult GetMassiveLoadDetailsByMassiveLoad(int massiveLoadId, SubMassiveProcessType processType, int status)
        {
            try
            {
                switch (processType)
                {
                    case SubMassiveProcessType.MassiveEmissionWithRequest:
                    case SubMassiveProcessType.MassiveEmissionWithoutRequest:
                        MassiveEmission massiveEmission = DelegateService.massiveUnderwritingService.GetMassiveEmissionByMassiveLoadId(massiveLoadId);
                     
                        return new UifJsonResult(true, massiveEmission);
                    case SubMassiveProcessType.CollectiveEmission:
                    case SubMassiveProcessType.Inclusion:
                    case SubMassiveProcessType.Exclusion:
                    case SubMassiveProcessType.CollectiveRenewal:
                        return new UifJsonResult(true, DelegateService.collectiveService.GetCollectiveEmissionByMassiveLoadId(massiveLoadId, false)); 
                    case SubMassiveProcessType.MassiveRenewal:
                        return new UifJsonResult(true, DelegateService.massiveRenewalService.GetMassiveRenewalByMassiveRenewalId(massiveLoadId, false, false, false));
                    case SubMassiveProcessType.CancellationMassive:
                        return new UifJsonResult(true, DelegateService.massiveUnderwritingService.GetMassiveEmissionRowsByMassiveLoadIdMassiveLoadProcessStatus(massiveLoadId, null, false, false));
                    default:
                        return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryLoad);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryLoad);
            }
        }

        public ActionResult FindExternalServices(int massiveLoadId, SubMassiveProcessType processType)
        {
            try
            {
                switch (processType)
                {
                    case SubMassiveProcessType.MassiveEmissionWithRequest:
                    case SubMassiveProcessType.MassiveEmissionWithoutRequest:
                        MassiveEmission massiveEmission = DelegateService.massiveUnderwritingService.GetMassiveEmissionByMassiveLoadId(massiveLoadId);
                        var rt = DelegateService.commonService.GetSubCoveredRiskTypeByPrefixIdCoveredRiskTypeId(massiveEmission.Prefix.Id, (int)massiveEmission.CoveredRiskType);
                        switch (rt)
                        {
                            case SubCoveredRiskType.Vehicle:
                                //DelegateService.massiveVehicleService.FindExternalServicesLoad(massiveLoadId);
                                break;
                            case SubCoveredRiskType.Property:
                                //DelegateService.massivePropertyService.FindExternalServicesLoad(massiveLoadId);
                                break;

                        }
                        break;

                    case SubMassiveProcessType.MassiveRenewal:
                        MassiveRenewal massiveRenewal = new MassiveRenewal();// DelegateService.massiveRenewalService.GetMassiveRenewalByMassiveRenewalId(massiveLoadId, false, false, false);
                        var rtr = DelegateService.commonService.GetSubCoveredRiskTypeByPrefixIdCoveredRiskTypeId(massiveRenewal.Prefix.Id, (int)massiveRenewal.CoveredRiskType);
                        switch (rtr)
                        {
                            case SubCoveredRiskType.Vehicle:
                                //DelegateService.massiveVehicleService.FindExternalServicesLoad(massiveLoadId);
                                break;
                            case SubCoveredRiskType.Property:
                                //DelegateService.massivePropertyService.FindExternalServicesLoad(massiveLoadId);
                                break;
                        }
                        break;

                    case SubMassiveProcessType.CollectiveEmission:
                    case SubMassiveProcessType.CollectiveRenewal:
                    case SubMassiveProcessType.Inclusion:
                        CollectiveEmission collectiveEmission = DelegateService.collectiveService.GetCollectiveEmissionByMassiveLoadId(massiveLoadId, false);
                        var rtc = DelegateService.commonService.GetSubCoveredRiskTypeByPrefixIdCoveredRiskTypeId(collectiveEmission.Prefix.Id, (int)collectiveEmission.CoveredRiskType);
                        switch (rtc)
                        {
                            case SubCoveredRiskType.Vehicle:
                                //DelegateService.massiveVehicleService.FindExternalServicesLoad(massiveLoadId);
                                break;
                            case SubCoveredRiskType.Property:
                                //DelegateService.massivePropertyService.FindExternalServicesLoad(massiveLoadId);
                                break;
                        }
                        break;

                }

                return new UifJsonResult(true, App_GlobalResources.Language.ThirdProcessInitiated);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryLoad);
            }
        }

        public ActionResult PrintLoad(int massiveLoadId, int rangeFrom, int rangeTo, SubMassiveProcessType processType, bool checkIssuedDetail)
        {
            try
            {
                string message = string.Empty;



                bool findCancellation = true;

                CollectiveEmission collectiveEmission = DelegateService.collectiveService.GetCollectiveEmissionByMassiveLoadId(massiveLoadId, true);

                if ((massiveLoadId > 0 && rangeFrom > 0 && rangeTo > 0) && (rangeTo >= rangeFrom))
                {

                    User user = new User();
                    user.Name = Helpers.SessionHelper.GetUserName();
                    user.UserId = Helpers.SessionHelper.GetUserId();
                    switch (processType)
                    {
                        case SubMassiveProcessType.MassiveEmissionWithRequest:
                            message = DelegateService.massiveUnderwritingService.PrintMassiveLoad(massiveLoadId, rangeFrom, rangeTo, user, checkIssuedDetail);
                            break;
                        case SubMassiveProcessType.MassiveEmissionWithoutRequest:
                            message = DelegateService.massiveUnderwritingService.PrintMassiveLoad(massiveLoadId, rangeFrom, rangeTo, user, checkIssuedDetail);
                            break;
                        case SubMassiveProcessType.CancellationMassive:
                            //message = DelegateService.CancellationMassiveEndorsementServices.PrintCancellationMassive(massiveLoadId, rangeFrom, rangeTo, user, checkIssuedDetail);
                            break;
                        case SubMassiveProcessType.CollectiveEmission:
                            message = DelegateService.collectiveService.PrintCollectiveLoad(massiveLoadId, rangeFrom, rangeTo, user, checkIssuedDetail);
                            break;
                        case SubMassiveProcessType.Inclusion:
                            message = DelegateService.collectiveService.PrintCollectiveLoad(massiveLoadId, rangeFrom, rangeTo, user, checkIssuedDetail);
                            break;
                        case SubMassiveProcessType.Exclusion:
                            message = DelegateService.collectiveService.PrintCollectiveLoad(massiveLoadId, rangeFrom, rangeTo, user, checkIssuedDetail);
                            break;
                        case SubMassiveProcessType.CollectiveRenewal:
                            message = DelegateService.collectiveService.PrintCollectiveLoad(massiveLoadId, rangeFrom, rangeTo, user, checkIssuedDetail);
                            break;
                        case SubMassiveProcessType.MassiveRenewal:
                            //message = DelegateService.massiveRenewalService.PrintRenewalLoad(massiveLoadId, rangeFrom, rangeTo, user, checkIssuedDetail);
                            break;
                        default:
                            return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryLoad);
                    }
                }
                return new UifJsonResult(true, message);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorPrintingLoad);
            }
        }

        public ActionResult GenerateFileToMassiveLoad(int massiveLoadId, SubMassiveProcessType processType)
        {
            try
            {
                string urlFile = null;
                switch (processType)
                {
                    case SubMassiveProcessType.MassiveEmissionWithRequest:
                    case SubMassiveProcessType.MassiveEmissionWithoutRequest:
                        urlFile = DelegateService.massiveUnderwritingService.GenerateFileErrorsMassiveEmission(massiveLoadId);
                        break;
                    case SubMassiveProcessType.CollectiveEmission:
                        urlFile = DelegateService.collectiveService.GenerateFileErrorsCollective(massiveLoadId, FileProcessType.CollectiveEmission);
                        break;
                    case SubMassiveProcessType.Inclusion:
                        urlFile = DelegateService.collectiveService.GenerateFileErrorsCollective(massiveLoadId, FileProcessType.CollectiveInclusion);
                        break;
                    case SubMassiveProcessType.Exclusion:
                        urlFile = DelegateService.collectiveService.GenerateFileErrorsCollective(massiveLoadId, FileProcessType.CollectiveExclusion);
                        break;
                    case SubMassiveProcessType.MassiveRenewal:
                        urlFile = DelegateService.massiveRenewalService.GenerateFileErrorsMassiveRenewals(massiveLoadId);
                        break;
                    case SubMassiveProcessType.CollectiveRenewal:
                        urlFile = DelegateService.collectiveService.GenerateFileErrorsCollective(massiveLoadId, FileProcessType.CollectiveRenewal);
                        break;
                    case SubMassiveProcessType.CancellationMassive:
                        urlFile = DelegateService.massiveUnderwritingService.GenerateFileErrorsMassiveCancellation(massiveLoadId);
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
                    var filenamefromPath = urlFile.Split(new char[] { '\\' }).Last();
                    return new UifJsonResult(true, new { Url = this.Url.Action("ShowExcelFile", "Massive") + "?url=" + urlFile, FileName = filenamefromPath });
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public FileResult ShowExcelFile(string url)
        {
            var pathToTheFile = url;
            var fileStream = new FileStream(pathToTheFile, FileMode.Open, FileAccess.Read);
            var extention = url.Split(new char[] { '.' }).Last();
            if (extention == "zip")
                return new FileStreamResult(fileStream, "application/zip");
            else if (extention == "csv")
                return new FileStreamResult(fileStream, "text/csv");
            else if (extention == "xlsx")
                return new FileStreamResult(fileStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            else
                return new FileStreamResult(fileStream, "application/pdf");
        }

        public ActionResult GenerateReportToMassiveLoad(int massiveLoadId, int status)
        {
            try
            {
                string urlFile = "";
                MassiveLoad massiveLoad = DelegateService.massiveService.GetMassiveLoadByMassiveLoadId(massiveLoadId);
                SubMassiveProcessType loadType = (SubMassiveProcessType)massiveLoad.LoadType.Id;

                switch (loadType)
                {
                    case SubMassiveProcessType.CollectiveEmission:
                    case SubMassiveProcessType.Exclusion:
                    case SubMassiveProcessType.Inclusion:
                        urlFile = GenerateCollectiveReport(massiveLoadId, status, EndorsementType.Emission);
                        break;
                    case SubMassiveProcessType.CollectiveRenewal:
                        urlFile = GenerateCollectiveReport(massiveLoadId, status, EndorsementType.Renewal);
                        break;
                    case SubMassiveProcessType.MassiveRenewal:
                        urlFile = GenerateMassiveRenewalReport(massiveLoadId, status);
                        break;
                    case SubMassiveProcessType.CancellationMassive:
                        urlFile = DelegateService.CancellationMassiveEndorsementServices.GenerateReportByMassiveLoadIdByMassiveLoadStatus(massiveLoadId, (MassiveLoadStatus) status);
                        break;
                    default:
                        urlFile = GenerateMassiveEmissionReport(massiveLoadId, status);
                        break;
                }
                if (string.IsNullOrEmpty(urlFile))
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorFileNotFound);
                }
                var filenamefromPath = urlFile.Split(new char[] { '\\' }).Last();
                return new UifJsonResult(true, new { Url = this.Url.Action("ShowExcelFile", "Massive") + "?url=" + urlFile, FileName = filenamefromPath });
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
            }
        }

        private string GenerateCollectiveReport(int massiveLoadId, int status, EndorsementType endorsementType)
        {
            string urlFile = "";
            CollectiveEmission collectiveEmission = DelegateService.collectiveService.GetCollectiveEmissionByMassiveLoadId(massiveLoadId, false);
            collectiveEmission.Status = (MassiveLoadStatus)status;
            var rt = DelegateService.commonService.GetSubCoveredRiskTypeByPrefixIdCoveredRiskTypeId(collectiveEmission.Prefix.Id, (int)collectiveEmission.CoveredRiskType);
            switch (rt)
            {
                case SubCoveredRiskType.JudicialSurety:
                    break;
                case SubCoveredRiskType.Liability:
                    //urlFile = DelegateService.liabilityCollectiveService.GenerateReportToCollectiveLoad(collectiveEmission, (int)endorsementType);
                    break;
                case SubCoveredRiskType.Property:
                    //urlFile = DelegateService.propertyCollectiveService.GenerateReportToCollectiveLoad(collectiveEmission, (int)endorsementType);
                    break;
                case SubCoveredRiskType.Surety:
                    break;
                case SubCoveredRiskType.ThirdPartyLiability:
                    urlFile = DelegateService.thirdPartyLiabilityCollectiveService.GenerateReportToCollectiveLoad(collectiveEmission, (int)endorsementType);
                    break;
                case SubCoveredRiskType.Vehicle:
                    urlFile = DelegateService.vehicleCollectiveService.GenerateReportToCollectiveLoad(collectiveEmission, (int)endorsementType);
                    break;
                default:
                    break;
            }
            return urlFile;
        }

        private string GenerateMassiveEmissionReport(int massiveLoadId, int status)
        {
            MassiveEmission massiveEmission = DelegateService.massiveUnderwritingService.GetMassiveEmissionByMassiveLoadId(massiveLoadId);
            var rt = DelegateService.commonService.GetSubCoveredRiskTypeByPrefixIdCoveredRiskTypeId(massiveEmission.Prefix.Id, (int)massiveEmission.CoveredRiskType);
            massiveEmission.Status = (MassiveLoadStatus)status;
            string urlFile = "";
            switch (rt)
            {
                case SubCoveredRiskType.JudicialSurety:
                    break;
                case SubCoveredRiskType.Liability:
                    //urlFile = DelegateService.massiveLiabilityService.GenerateReportToMassiveLoad(massiveEmission);
                    break;
                case SubCoveredRiskType.Property:
                    //urlFile = DelegateService.massivePropertyService.GenerateReportToMassiveLoad(massiveEmission);
                    break;
                case SubCoveredRiskType.Surety:
                    break;
                case SubCoveredRiskType.ThirdPartyLiability:
                    urlFile = DelegateService.massiveThirdPartyLiabilityService.GenerateReportToMassiveLoad(massiveEmission);
                    break;
                case SubCoveredRiskType.Vehicle:
                    urlFile = DelegateService.massiveVehicleService.GenerateReportToMassiveLoad(massiveEmission);
                    break;
                default:
                    break;
            }
            return urlFile;
        }

        private string GenerateMassiveRenewalReport(int massiveLoadId, int status)
        {
            MassiveRenewal massiveRenewal = DelegateService.massiveRenewalService.GetMassiveRenewalByMassiveRenewalId(massiveLoadId, false, false, false);
            var rt = DelegateService.commonService.GetSubCoveredRiskTypeByPrefixIdCoveredRiskTypeId(massiveRenewal.Prefix.Id, (int)massiveRenewal.CoveredRiskType);
            massiveRenewal.Status = (MassiveLoadStatus)status;
            string urlFile = "";
            switch (rt)
            {
                case SubCoveredRiskType.JudicialSurety:
                    break;
                case SubCoveredRiskType.Liability:
                    break;
                case SubCoveredRiskType.Property:
                    //urlFile = DelegateService.massivePropertyService.GenerateReportToMassiveRenewal(massiveRenewal);
                    break;
                case SubCoveredRiskType.Surety:
                    break;
                case SubCoveredRiskType.ThirdPartyLiability:
                    urlFile = DelegateService.massiveThirdPartyLiabilityService.GenerateReportToMassiveRenewal(massiveRenewal);
                    break;
                case SubCoveredRiskType.Vehicle:
                    urlFile = DelegateService.massiveVehicleService.GenerateReportToMassiveRenewal(massiveRenewal);
                    break;
                default:
                    break;
            }
            return urlFile;
        }


        public ActionResult GenerateReportByMassiveLoadIdByStatus(int massiveLoadId, int status)
        {
            try
            {
                string urlFile = "";
                urlFile = DelegateService.CancellationMassiveEndorsementServices.GenerateReportByMassiveLoadIdByMassiveLoadStatus(massiveLoadId, (MassiveLoadStatus)status);

                if (string.IsNullOrEmpty(urlFile))
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorFileNotFound);
                }
                else
                {
                    var filenamefromPath = urlFile.Split(new char[] { '\\' }).Last();
                    return new UifJsonResult(true, new { Url = this.Url.Action("ShowExcelFile", "Massive") + "?url=" + urlFile, FileName = filenamefromPath });
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
            }
        }
        public ActionResult TariffedLoad(int massiveLoadId)

        {
            try
            {
                MassiveLoad massiveLoad = DelegateService.massiveService.GetMassiveLoadByMassiveLoadId(massiveLoadId);
                massiveLoad.User.UserId = SessionHelper.GetUserId();
                massiveLoad.User.AccountName = SessionHelper.GetUserName();

                switch (massiveLoad.LoadType.ProcessType)
                {
                    case MassiveProcessType.Emission:
                        if ((SubMassiveProcessType)massiveLoad.LoadType.Id == SubMassiveProcessType.CollectiveEmission)
                        {
                            massiveLoad = CollectiveQuotate(massiveLoad);
                        }
                        else
                        {
                            massiveLoad = QuotateMassive(massiveLoad);
                        }
                        break;
                    case MassiveProcessType.Modification:
                        massiveLoad = ModificationQuotate(massiveLoad);
                        break;
                    case MassiveProcessType.Renewal:
                        switch ((SubMassiveProcessType)massiveLoad.LoadType.Id)
                        {
                            case SubMassiveProcessType.MassiveRenewal:
                                massiveLoad = MassiveRenewalQuotate(massiveLoad);
                                break;
                            case SubMassiveProcessType.CollectiveRenewal:
                                massiveLoad = CollectiveRenewalQuotate(massiveLoad);
                                break;
                            default:
                                break;
                        }
                        break;
                    case MassiveProcessType.Cancellation:
                        massiveLoad = CancellationQuotate(massiveLoad);
                        break;
                    default:
                        break;
                }
                return new UifJsonResult(true, App_GlobalResources.Language.InitiatedTariff);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorInitiatedTariff);
            }
        }

        public ActionResult ValidateIssuePolicies(int massiveLoadId)
        {
            MassiveLoad massiveLoad = DelegateService.massiveService.GetMassiveLoadByMassiveLoadId(massiveLoadId);

            if ((SubMassiveProcessType) massiveLoad.LoadType.Id == SubMassiveProcessType.CollectiveEmission
                || (SubMassiveProcessType)massiveLoad.LoadType.Id == SubMassiveProcessType.CollectiveRenewal
                || (SubMassiveProcessType)massiveLoad.LoadType.Id == SubMassiveProcessType.Inclusion
                || (SubMassiveProcessType)massiveLoad.LoadType.Id == SubMassiveProcessType.Exclusion
                || massiveLoad.LoadType.ProcessType == MassiveProcessType.Cancellation)
            {
                List<AuthorizationRequest> authorizationRequests = DelegateService.AuthorizationPoliciesService.GetAuthorizationRequestsByKey(massiveLoadId.ToString());
                if (authorizationRequests.Count(x => !x.Key2.Contains("|") && x.Status == TypeStatus.Pending) > 0)
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.PoliciesWithoutAuthorization);
                }

                if (authorizationRequests.Count(x => !x.Key2.Contains("|") && x.Status == TypeStatus.Rejected) > 0)
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.PoliciesRejected);
                }

                if (authorizationRequests.Count(x => x.Key2.Contains("|") && x.Status == TypeStatus.Pending) > 0)
                {
                    // return new UifJsonResult(false, App_GlobalResources.Language.PoliciesWithoutAuthorizationRisks);
                }
                if (authorizationRequests.Count(x => x.Key2.Contains("|") && x.Status == TypeStatus.Rejected) > 0)
                {
                    return new UifJsonResult(true, new { confirmationType = "WithEvent", message = App_GlobalResources.Language.PoliciesWithApprovedRisks });
                }
            }
            return RedirectToAction("IssuePolicy", new { massiveLoadId });
        }

        public ActionResult IssuePolicy(int massiveLoadId)
        {
            MassiveLoad massiveLoad = DelegateService.massiveService.GetMassiveLoadByMassiveLoadId(massiveLoadId);
            massiveLoad.User.UserId = SessionHelper.GetUserId();
            massiveLoad.User.AccountName = SessionHelper.GetUserName();

            switch (massiveLoad.LoadType.ProcessType)
            {
                case MassiveProcessType.Emission:
                case MassiveProcessType.Modification:
                    if ((SubMassiveProcessType)massiveLoad.LoadType.Id == SubMassiveProcessType.CollectiveEmission)
                    {
                        massiveLoad = IssuanceCollectiveEmission(massiveLoad);
                    }
                    else if ((SubMassiveProcessType)massiveLoad.LoadType.Id == SubMassiveProcessType.Inclusion ||
                            (SubMassiveProcessType)massiveLoad.LoadType.Id == SubMassiveProcessType.Exclusion)
                    {
                        massiveLoad = IssuanceCollectiveModificationEmission(massiveLoad);
                    }
                    else
                    {
                        massiveLoad = IssuanceMassiveEmission(massiveLoad);
                    }
                    break;
                //break;
                case MassiveProcessType.Renewal:
                    if ((SubMassiveProcessType)massiveLoad.LoadType.Id == SubMassiveProcessType.CollectiveRenewal)
                    {
                        massiveLoad = IssuanceRenewalCollectiveEmission(massiveLoad);
                    }
                    else
                    {
                        massiveLoad = IssuanceRenewalMassiveEmission(massiveLoad);
                    }
                    break;
                case MassiveProcessType.Cancellation:
                    massiveLoad = DelegateService.CancellationMassiveEndorsementServices.CreateIssuePolicy(massiveLoad);
                    break;
                default:
                    break;
            }
            string statusMessage = String.Empty;
            if (massiveLoad.HasError)
            {
                statusMessage = String.Format("{0}{1}{2}", App_GlobalResources.Language.ErrorIssue,
                    Environment.NewLine,
                    massiveLoad.ErrorDescription);
            }
            else
            {
                statusMessage = App_GlobalResources.Language.InitiatedIssue;
            }
            return new UifJsonResult(true, statusMessage);
        }

        private MassiveLoad IssuanceCollectiveModificationEmission(MassiveLoad massiveLoad)
        {
            CollectiveEmission collectiveEmission = DelegateService.collectiveService.GetCollectiveEmissionByMassiveLoadId(massiveLoad.Id, true);
            if (collectiveEmission?.CoveredRiskType == null)
            {
                massiveLoad.HasError = true;
                return massiveLoad;
            }
            SubCoveredRiskType rt = DelegateService.commonService.GetSubCoveredRiskTypeByPrefixIdCoveredRiskTypeId(collectiveEmission.Prefix.Id, (int)collectiveEmission.CoveredRiskType);
            switch (rt)
            {
                case SubCoveredRiskType.Vehicle:
                    return DelegateService.collectiveVehicleModificationService.IssuanceCollectiveEmission(massiveLoad);
                case SubCoveredRiskType.Property:
                //return DelegateService.collectivePropertyModificationService.IssuanceCollectiveEmission(massiveLoad);
                case SubCoveredRiskType.ThirdPartyLiability:
                    return DelegateService.thirdPartyLiabilityModificationService.IssuanceCollectiveEmission(massiveLoad);
                case SubCoveredRiskType.Liability:
                //return DelegateService.liabilityCollectiveService.IssuanceCollectiveEmission(massiveLoad);
                default:
                    break;
            }
            return massiveLoad;
        }

        private MassiveLoad IssuanceMassiveEmission(MassiveLoad massiveLoad)
        {
            MassiveEmission massiveEmission = DelegateService.massiveUnderwritingService.GetMassiveEmissionByMassiveLoadId(massiveLoad.Id);
            var rt = DelegateService.commonService.GetSubCoveredRiskTypeByPrefixIdCoveredRiskTypeId(massiveEmission.Prefix.Id, (int)massiveEmission.CoveredRiskType);
            switch (rt)
            {
                case SubCoveredRiskType.Vehicle:
                    return DelegateService.massiveVehicleService.IssuanceMassiveEmission(massiveLoad);
                case SubCoveredRiskType.Property:
                //return DelegateService.massivePropertyService.IssuanceMassiveEmission(massiveLoad);
                case SubCoveredRiskType.Liability:
                //return DelegateService.massiveLiabilityService.IssuanceMassiveEmission(massiveLoad);
                case SubCoveredRiskType.ThirdPartyLiability:
                    return DelegateService.massiveThirdPartyLiabilityService.IssuanceMassiveEmission(massiveLoad);
                default:
                    return DelegateService.massiveUnderwritingService.IssuanceMassiveEmission(massiveLoad.Id);

            }

        }

        private MassiveLoad IssuanceRenewalMassiveEmission(MassiveLoad massiveLoad)
        {
            MassiveRenewal massiveRenewal = DelegateService.massiveRenewalService.GetMassiveRenewalByMassiveRenewalId(massiveLoad.Id, false, false, false);
            var rt = DelegateService.commonService.GetSubCoveredRiskTypeByPrefixIdCoveredRiskTypeId(massiveRenewal.Prefix.Id, (int)massiveRenewal.CoveredRiskType);
            switch (rt)
            {
                case SubCoveredRiskType.Vehicle:
                    return DelegateService.massiveVehicleService.IssuanceRenewalMassiveEmission(massiveLoad);
                case SubCoveredRiskType.Property:
                //return DelegateService.massivePropertyService.IssuanceRenewalMassiveEmission(massiveLoad);
                case SubCoveredRiskType.ThirdPartyLiability:
                    return DelegateService.massiveThirdPartyLiabilityService.IssuanceRenewalMassiveEmission(massiveLoad);
                case SubCoveredRiskType.Liability:
                //return DelegateService.massiveLiabilityService.IssuanceRenewalMassiveEmission(massiveLoad);
                default:
                    return DelegateService.massiveUnderwritingService.IssuanceMassiveEmission(massiveLoad.Id);
            }
        }
        private MassiveLoad IssuanceRenewalCollectiveEmission(MassiveLoad massiveLoad)
        {
            CollectiveEmission collectiveEmission = DelegateService.collectiveService.GetCollectiveEmissionByMassiveLoadId(massiveLoad.Id, false);
            SubCoveredRiskType riskType = DelegateService.commonService.GetSubCoveredRiskTypeByPrefixIdCoveredRiskTypeId(collectiveEmission.Prefix.Id, (int)collectiveEmission.CoveredRiskType);
            switch (riskType)
            {
                case SubCoveredRiskType.Vehicle:
                    return DelegateService.collectiveVehicleRenewalService.IssuanceCollectiveRenewal(massiveLoad);
                case SubCoveredRiskType.Property:
                //return DelegateService.collectivePropertyRenewalService.IssuanceCollectiveRenewal(massiveLoad);
                case SubCoveredRiskType.ThirdPartyLiability:
                    return DelegateService.collectiveThirdPartyLiabilityRenewalService.IssuanceCollectiveRenewal(massiveLoad);
                case SubCoveredRiskType.Liability:
                //return DelegateService.collectiveLiabilityRenewalService.IssuanceRenewalMassiveEmission(massiveLoad);
                default:
                    //return DelegateService.massiveUnderwritingService.IssuanceMassiveEmission(massiveLoad.Id);
                    return null;
            }
        }

        private MassiveLoad IssuanceCollectiveEmission(MassiveLoad massiveLoad)
        {
            CollectiveEmission collectiveEmission = DelegateService.collectiveService.GetCollectiveEmissionByMassiveLoadId(massiveLoad.Id, true);
            if (collectiveEmission?.CoveredRiskType == null)
            {
                massiveLoad.HasError = true;
                return massiveLoad;
            }
            SubCoveredRiskType rt = DelegateService.commonService.GetSubCoveredRiskTypeByPrefixIdCoveredRiskTypeId(collectiveEmission.Prefix.Id, (int)collectiveEmission.CoveredRiskType);
            switch (rt)
            {
                case SubCoveredRiskType.Vehicle:
                    return DelegateService.vehicleCollectiveService.IssuanceCollectiveEmission(massiveLoad);
                case SubCoveredRiskType.Property:
                //return DelegateService.propertyCollectiveService.IssuanceCollectiveEmission(massiveLoad);
                case SubCoveredRiskType.ThirdPartyLiability:
                    return DelegateService.thirdPartyLiabilityCollectiveService.IssuanceCollectiveEmission(massiveLoad);
                case SubCoveredRiskType.Liability:
                //return DelegateService.liabilityCollectiveService.IssuanceCollectiveEmission(massiveLoad);
                default:
                    break;
            }
            return massiveLoad;
        }


        public ActionResult ExcludeTemporalsByLoad(int massiveLoadId, List<int> temps)
        {
            temps.RemoveAll(x => x == 0);

            bool response = false;

            if (temps.Count > 0)
            {

                try
                {
                    MassiveLoad massiveLoad = DelegateService.massiveService.GetMassiveLoadByMassiveLoadId(massiveLoadId);
                    switch (massiveLoad.LoadType.ProcessType)
                    {
                        case MassiveProcessType.Emission:
                            if ((SubMassiveProcessType)massiveLoad.LoadType.Id == SubMassiveProcessType.CollectiveEmission)
                            {
                                if (DelegateService.collectiveService.ExcludeCollectiveEmissionRowsTemporals(massiveLoadId, temps.Distinct().ToList(), SessionHelper.GetUserName(), false) != null)
                                {
                                    response = true;
                                }
                            }
                            else
                            {
                                response = DelegateService.massiveUnderwritingService.ExcludeMassiveEmissionRowsTemporals(massiveLoad.Id, temps.Distinct().ToList(), SessionHelper.GetUserName(), false);
                            }
                            break;
                        case MassiveProcessType.Modification:
                            if (DelegateService.collectiveService.ExcludeCollectiveEmissionRowsTemporals(massiveLoadId, temps.Distinct().ToList(), SessionHelper.GetUserName(), false) != null)
                            {
                                response = true;
                            }

                            break;
                        case MassiveProcessType.Renewal:
                            switch ((SubMassiveProcessType)massiveLoad.LoadType.Id)
                            {
                                case SubMassiveProcessType.MassiveRenewal:
                                    response = DelegateService.massiveRenewalService.ExcludeMassiveRenewalRowsTemporals(massiveLoad.Id, temps.Distinct().ToList(), SessionHelper.GetUserName());
                                    break;
                                case SubMassiveProcessType.CollectiveRenewal:
                                    if (DelegateService.collectiveService.ExcludeCollectiveEmissionRowsTemporals(massiveLoadId, temps.Distinct().ToList(), SessionHelper.GetUserName(), true) != null)
                                    {
                                        response = true;
                                    }
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case MassiveProcessType.Cancellation:
                            response = DelegateService.massiveUnderwritingService.ExcludeMassiveCancellationRowsByTemporals(massiveLoad.Id, temps.Distinct().ToList(), SessionHelper.GetUserName(), true);
                            break;
                        default:
                            break;
                    }

                    if (response)
                    {
                        return new UifJsonResult(true, massiveLoad);
                    }
                    else
                    {
                        string temp = string.Join(",", temps);
                        return new UifJsonResult(false, string.Format(App_GlobalResources.Language.ErrorExclusionIdentifiers, temp));
                    }

                }
                catch (Exception)
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorInitiatedExclusion);
                }
            }
            else
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorExclusionIdentifiers);
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
                    //using (NetworkConnection networkCon = new NetworkConnection(uri.LocalPath, new NetworkCredential(user, password, domain)))
                    //{
                    //    if (networkCon._resultConnection == 1219 || networkCon._resultConnection == 0)
                    //    {
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(urlPath + fileName);
                    fileInfo.CopyTo(externalDirectoryPath.ToString() + fileName);
                    fileInfo.Delete();
                    break;
                    //    }
                    //    else
                    //    {
                    //        throw new Win32Exception(networkCon._resultConnection);
                    //    }
                    //}
                }
                catch (Exception ex)
                {
                    retries++;
                    if (retries == 5)
                    {
                        EventLog.WriteEntry("Application", ex.ToString(), EventLogEntryType.Error);
                        return false;
                    }
                }
            }

            return true;
        }

        private CompanyFilterReport GetRisksNums(CompanyFilterReport companyFilterReport)
        {
            List<UnderModel.Risk> riskNums = new List<UnderModel.Risk>();

            companyFilterReport = AddRisks(companyFilterReport, (List<UnderModel.Risk>)riskNums);
            return companyFilterReport;
        }

        private CompanyFilterReport AddRisks(CompanyFilterReport companyFilterReport, List<UnderModel.Risk> risks)
        {
            companyFilterReport.Risks = new List<Risk>();

            foreach (UnderModel.Risk item in risks)
            {
                companyFilterReport.Risks.Add(new Risk { Id = item.Id, Status = item.Status });

            }

            return companyFilterReport;
        }

        public ActionResult DeleteProcess(MassiveLoad massiveLoad)
        {
            try
            {
                string result = string.Empty;
                result = DelegateService.massiveService.DeleteProcess(massiveLoad);
                if (result == "")
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.DeleteLoadSuccessfully);
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

        public ActionResult MassiveLoadHasThirdParty(int massiveLoadId)
        {
            try
            {
                //var result = DelegateService.externalProxyService.MassiveLoadHasThirdParty(massiveLoadId);
                return new UifJsonResult(true, null);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, massiveLoadId);
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public FileResult ShowPdfFile(string url)
        {
            var pathToTheFile = url;
            var fileStream = new FileStream(pathToTheFile, FileMode.Open, FileAccess.Read);
            var extention = url.Split(new char[] { '.' }).Last();
            if (extention == "zip")
                return new FileStreamResult(fileStream, "application/zip");
            else
                return new FileStreamResult(fileStream, "application/pdf");
        }

        public ActionResult GetPrintingByLoadId(int massiveLoadId, int massiveLoadStatus, int massiveLoadType, int RiskSince, int RiskUntil, bool collectFormat, string[] idCuotes)
        {
            try
            {
                CompanyPolicy companyPolicy = new CompanyPolicy();
                List<MassiveCancellationRow> massiveCancellationRowsWithEvents = new List<MassiveCancellationRow>();
                List<MassiveCancellationRow> massiveCancellationRows = new List<MassiveCancellationRow>();
                PolicyInfo PolicyInfoRisks = new PolicyInfo();
                TemporaryInfo temporaryInfo = new TemporaryInfo();
                CommonProperties commonProperties = new CommonProperties();
                bool findCancellation = true;
                int iTotalRisksByUser = (RiskUntil - RiskSince) + 1;
                int itotalRisksInEndorsement = 0;
                List<PrintingInfo> lstPolicyInfo = new List<PrintingInfo>();
                string strPath = string.Empty;
                string sPrintingProcess = string.Empty;
                string strReturnValidate = string.Empty;
                string filenamefromPath = string.Empty;

                switch (massiveLoadType)
                {
                    case 3:
                    case 7:
                    case 8:
                        {
                            if (massiveLoadStatus == 6)
                            {
                                CollectiveEmission collectiveEmission = DelegateService.collectiveService.GetCollectiveEmissionByMassiveLoadId(massiveLoadId, true);

                                if (collectiveEmission != null)
                                {
                                    PolicyInfo policyInfo = new PolicyInfo();

                                    companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId((int)collectiveEmission.EndorsementId);
                                    PolicyInfoRisks = DelegateService.printingService.GetRisksByEndorsementId((int)collectiveEmission.EndorsementId);
                                    findCancellation = false;

                                    policyInfo.BranchId = companyPolicy.Branch.Id;
                                    policyInfo.PolicyNumber = int.Parse(companyPolicy.DocumentNumber.ToString());
                                    policyInfo.EndorsementNum = companyPolicy.Endorsement.Number;
                                    policyInfo.PolicyId = companyPolicy.Endorsement.PolicyId;
                                    policyInfo.EndorsementId = companyPolicy.Endorsement.Id;
                                    policyInfo.TicketNumber = companyPolicy.Endorsement.TicketNumber;
                                    policyInfo.IsCollective = true;
                                    commonProperties.PrefixId = companyPolicy.Prefix.Id;
                                    commonProperties.UserId = SessionHelper.GetUserId();
                                    commonProperties.UserName = SessionHelper.GetUserName();
                                    commonProperties.IsMassive = companyPolicy.PolicyOrigin != PolicyOrigin.Individual ? true : false;

                                    if (RiskSince < PolicyInfoRisks.CommonProperties.RiskSince || RiskUntil > PolicyInfoRisks.CommonProperties.RiskUntil)
                                    {
                                        return new UifJsonResult(false, App_GlobalResources.Language.RankIngressedPrinting + PolicyInfoRisks.CommonProperties.RiskSince + "-" + PolicyInfoRisks.CommonProperties.RiskUntil);
                                    }
                                    else
                                    {
                                        commonProperties.RiskSince = RiskSince;
                                        commonProperties.RiskUntil = RiskUntil;
                                    }

                                    policyInfo.CommonProperties = commonProperties;

                                    lstPolicyInfo.Add(policyInfo);

                                    if (lstPolicyInfo.Count > 0)
                                    {
                                        sPrintingProcess = DelegateService.printingService.GenerateReportMassiveJetForm(lstPolicyInfo, string.Empty, 1, SessionHelper.GetUserId(), lstPolicyInfo.Count, massiveLoadId, collectFormat, idCuotes);
                                    }

                                    return new UifJsonResult(true, new { PrintingProcess = sPrintingProcess });
                                }

                                if (findCancellation)
                                {
                                    massiveCancellationRows = DelegateService.massiveUnderwritingService.GetMassiveCancellationRowsWithErrorsWithEventsByMassiveLoadId(massiveLoadId, false, false);

                                    foreach (var item in massiveCancellationRows)
                                    {
                                        PolicyInfo policyInfo = new PolicyInfo();

                                        companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(item.Risk.Policy.Endorsement.Id);
                                        PolicyInfoRisks = DelegateService.printingService.GetRisksByEndorsementId(item.Risk.Policy.Endorsement.Id);

                                        commonProperties = new CommonProperties
                                        {
                                            PrefixId = companyPolicy.Prefix.Id,
                                            UserId = SessionHelper.GetUserId(),
                                            RiskSince = PolicyInfoRisks.CommonProperties.RiskSince,
                                            RiskUntil = PolicyInfoRisks.CommonProperties.RiskUntil,
                                            UserName = SessionHelper.GetUserName(),
                                            IsMassive = true
                                        };

                                        policyInfo.BranchId = companyPolicy.Branch.Id;
                                        policyInfo.PolicyNumber = int.Parse(companyPolicy.DocumentNumber.ToString());
                                        policyInfo.EndorsementNum = companyPolicy.Endorsement.Number;
                                        policyInfo.PolicyId = companyPolicy.Endorsement.PolicyId;
                                        policyInfo.EndorsementId = companyPolicy.Endorsement.Id;
                                        policyInfo.TicketNumber = companyPolicy.Endorsement.TicketNumber;
                                        policyInfo.CommonProperties = commonProperties;
                                        policyInfo.IsCollective = true;

                                        lstPolicyInfo.Add(policyInfo);
                                    }
                                    if (lstPolicyInfo.Count > 0)
                                    {
                                        sPrintingProcess = DelegateService.printingService.GenerateReportMassiveJetForm(lstPolicyInfo, string.Empty, 1, SessionHelper.GetUserId(), lstPolicyInfo.Count, massiveLoadId, collectFormat, idCuotes);
                                    }
                                    else
                                        return new UifJsonResult(false, "Error en la generacion de pdf");

                                    return new UifJsonResult(true, new { PrintingProcess = sPrintingProcess });
                                }

                                if (string.IsNullOrEmpty(strPath))
                                    return new UifJsonResult(true, new { Url = strPath, FilePathResult = strPath });
                                else
                                    return new UifJsonResult(true, new { PrintingProcess = sPrintingProcess });
                            }
                            else if (massiveLoadStatus == 4)
                            {
                                CollectiveEmission collectiveEmission = DelegateService.collectiveService.GetCollectiveEmissionByMassiveLoadId(massiveLoadId, true);

                                if (collectiveEmission != null)
                                {
                                    temporaryInfo = DelegateService.printingService.GetTemporaryByOpetarionId(collectiveEmission.TemporalId);
                                    temporaryInfo.OperationId = collectiveEmission.TemporalId;
                                    temporaryInfo.IsCollective = true;
                                    findCancellation = false;

                                    temporaryInfo.CommonProperties.UserId = SessionHelper.GetUserId();
                                    temporaryInfo.CommonProperties.UserName = SessionHelper.GetUserName();
                                    temporaryInfo.CommonProperties.IsMassive = true;

                                    if (RiskSince < temporaryInfo.CommonProperties.RiskSince || RiskUntil > temporaryInfo.CommonProperties.RiskUntil)
                                    {
                                        return new UifJsonResult(false, App_GlobalResources.Language.RankIngressedPrinting + temporaryInfo.CommonProperties.RiskSince + "-" + temporaryInfo.CommonProperties.RiskUntil);
                                    }
                                    else
                                    {
                                        temporaryInfo.CommonProperties.RiskSince = RiskSince;
                                        temporaryInfo.CommonProperties.RiskUntil = RiskUntil;
                                    }

                                    lstPolicyInfo.Add(temporaryInfo);

                                    if (lstPolicyInfo.Count > 0)
                                        sPrintingProcess = DelegateService.printingService.GenerateReportMassiveJetForm(lstPolicyInfo, string.Empty, 1, SessionHelper.GetUserId(), lstPolicyInfo.Count, massiveLoadId, collectFormat, idCuotes);
                                    else
                                        return new UifJsonResult(false, "Error en la generacion de pdf");

                                    return new UifJsonResult(true, new { PrintingProcess = sPrintingProcess });
                                }

                                if (findCancellation)
                                {
                                    massiveCancellationRows = DelegateService.massiveUnderwritingService.GetMassiveCancellationRowsWithErrorsWithEventsByMassiveLoadId(massiveLoadId, false, false);

                                    foreach (var item in massiveCancellationRows)
                                    {
                                        temporaryInfo = DelegateService.printingService.GetTemporaryByOpetarionId(item.Risk.Policy.Id);
                                        temporaryInfo.CommonProperties.UserId = SessionHelper.GetUserId();
                                        temporaryInfo.CommonProperties.UserName = SessionHelper.GetUserName();
                                        temporaryInfo.CommonProperties.IsMassive = true;
                                        temporaryInfo.OperationId = item.Risk.Policy.Id;
                                        temporaryInfo.IsCollective = true;
                                        lstPolicyInfo.Add(temporaryInfo);
                                    }

                                    massiveCancellationRowsWithEvents = DelegateService.massiveUnderwritingService.GetMassiveCancellationRowsWithErrorsWithEventsByMassiveLoadId(massiveLoadId, false, true);

                                    foreach (var item in massiveCancellationRowsWithEvents)
                                    {
                                        temporaryInfo = DelegateService.printingService.GetTemporaryByOpetarionId(item.Risk.Policy.Id);
                                        temporaryInfo.CommonProperties.UserId = SessionHelper.GetUserId();
                                        temporaryInfo.CommonProperties.UserName = SessionHelper.GetUserName();
                                        temporaryInfo.CommonProperties.IsMassive = true;
                                        temporaryInfo.OperationId = item.Risk.Policy.Id;
                                        temporaryInfo.IsCollective = true;
                                        lstPolicyInfo.Add(temporaryInfo);
                                    }

                                    if (lstPolicyInfo.Count > 0)
                                        strPath = DelegateService.printingService.GenerateReportMassiveJetForm(lstPolicyInfo, string.Empty, 1, SessionHelper.GetUserId(), lstPolicyInfo.Count, massiveLoadId, collectFormat, idCuotes);
                                    else
                                        return new UifJsonResult(false, "Error en la generacion de pdf");

                                    return new UifJsonResult(true, new { PrintingProcess = strPath });
                                }

                                if (strPath != strReturnValidate)
                                    return new UifJsonResult(true, new { PrintingProcess = strReturnValidate });
                                else
                                    return new UifJsonResult(true, new { Url = strPath, FilePathResult = strPath });
                            }
                            else
                            {
                                return new UifJsonResult(false, "No es posible imprimir en este estado de cargue");
                            }
                        }
                    case 6:
                    case 2:
                        {
                            if (massiveLoadStatus == 6)
                            {
                                List<MassiveEmissionRow> massiveEmissionRows = DelegateService.massiveUnderwritingService.GetMassiveEmissionRowsByMassiveLoadIdMassiveLoadProcessStatus(massiveLoadId, MassiveLoadProcessStatus.Finalized, false, false);

                                if (massiveEmissionRows.Count() > 0)
                                {
                                    foreach (var item in massiveEmissionRows.OrderBy(x => x.RowNumber))
                                    {
                                        PolicyInfo policyInfo = new PolicyInfo();

                                        companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId((int)item.Risk.Policy.Endorsement.Id);
                                        PolicyInfoRisks = DelegateService.printingService.GetRisksByEndorsementId((int)item.Risk.Policy.Endorsement.Id);
                                        commonProperties = new CommonProperties
                                        {
                                            PrefixId = companyPolicy.Prefix.Id,
                                            UserId = SessionHelper.GetUserId(),
                                            RiskSince = PolicyInfoRisks.CommonProperties.RiskSince,
                                            RiskUntil = PolicyInfoRisks.CommonProperties.RiskUntil,
                                            UserName = SessionHelper.GetUserName(),
                                            IsMassive = true
                                        };

                                        policyInfo.BranchId = companyPolicy.Branch.Id;
                                        policyInfo.PolicyNumber = int.Parse(companyPolicy.DocumentNumber.ToString());
                                        policyInfo.EndorsementNum = companyPolicy.Endorsement.Number;
                                        policyInfo.PolicyId = companyPolicy.Endorsement.PolicyId;
                                        policyInfo.EndorsementId = companyPolicy.Endorsement.Id;
                                        policyInfo.TicketNumber = companyPolicy.Endorsement.TicketNumber;
                                        policyInfo.IsCollective = false;
                                        policyInfo.CommonProperties = commonProperties;

                                        if (lstPolicyInfo.Count < iTotalRisksByUser)
                                            lstPolicyInfo.Add(policyInfo);
                                    }
                                }
                                else
                                {
                                    List<MassiveRenewalRow> massiveRenewalRows = DelegateService.massiveRenewalService.GetMassiveRenewalRowsByMassiveLoadIdMassiveLoadProcessStatus(massiveLoadId, MassiveLoadProcessStatus.Finalized, false, false);

                                    if (massiveRenewalRows.Count() > 0)
                                    {
                                        foreach (var item in massiveRenewalRows.OrderBy(x => x.RowNumber))
                                        {
                                            PolicyInfo policyInfo = new PolicyInfo();

                                            companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId((int)item.Risk.Policy.Endorsement.Id);
                                            PolicyInfoRisks = DelegateService.printingService.GetRisksByEndorsementId((int)item.Risk.Policy.Endorsement.Id);
                                            commonProperties = new CommonProperties
                                            {
                                                PrefixId = companyPolicy.Prefix.Id,
                                                UserId = SessionHelper.GetUserId(),
                                                RiskSince = PolicyInfoRisks.CommonProperties.RiskSince,
                                                RiskUntil = PolicyInfoRisks.CommonProperties.RiskUntil,
                                                UserName = SessionHelper.GetUserName(),
                                                IsMassive = true
                                            };

                                            policyInfo.BranchId = companyPolicy.Branch.Id;
                                            policyInfo.PolicyNumber = int.Parse(companyPolicy.DocumentNumber.ToString());
                                            policyInfo.EndorsementNum = companyPolicy.Endorsement.Number;
                                            policyInfo.PolicyId = companyPolicy.Endorsement.PolicyId;
                                            policyInfo.EndorsementId = companyPolicy.Endorsement.Id;
                                            policyInfo.TicketNumber = companyPolicy.Endorsement.TicketNumber;
                                            policyInfo.IsCollective = false;
                                            policyInfo.CommonProperties = commonProperties;

                                            if (lstPolicyInfo.Count < iTotalRisksByUser)
                                                lstPolicyInfo.Add(policyInfo);
                                        }
                                    }
                                }

                                if (lstPolicyInfo.Count > 0)
                                    sPrintingProcess = DelegateService.printingService.GenerateReportMassiveJetForm(lstPolicyInfo, string.Empty, 1, SessionHelper.GetUserId(), lstPolicyInfo.Count, massiveLoadId, collectFormat, idCuotes);
                                else
                                    return new UifJsonResult(false, "Error en la generacion de pdf");

                                return new UifJsonResult(true, new { PrintingProcess = sPrintingProcess });
                            }
                            else if (massiveLoadStatus == 4)
                            {
                                List<MassiveEmissionRow> massiveEmissionRows = DelegateService.massiveUnderwritingService.GetMassiveEmissionRowsByMassiveLoadIdMassiveLoadProcessStatus(massiveLoadId, MassiveLoadProcessStatus.Events, false, false);

                                if (massiveEmissionRows.Count() > 0)
                                {
                                    foreach (var item in massiveEmissionRows.OrderBy(x => x.RowNumber))
                                    {
                                        temporaryInfo = DelegateService.printingService.GetTemporaryByOpetarionId(item.Risk.Policy.Id);
                                        temporaryInfo.CommonProperties.UserId = SessionHelper.GetUserId();
                                        temporaryInfo.CommonProperties.UserName = SessionHelper.GetUserName();
                                        temporaryInfo.CommonProperties.IsMassive = true;
                                        temporaryInfo.CommonProperties.RiskSince = 1;
                                        temporaryInfo.CommonProperties.RiskUntil = 1;
                                        temporaryInfo.OperationId = item.Risk.Policy.Id;
                                        temporaryInfo.IsCollective = false;

                                        if (lstPolicyInfo.Count < iTotalRisksByUser)
                                            lstPolicyInfo.Add(temporaryInfo);
                                    }
                                }

                                List<MassiveEmissionRow> massiveEmissionRowsWithEvents = DelegateService.massiveUnderwritingService.GetMassiveEmissionRowsByMassiveLoadIdMassiveLoadProcessStatus(massiveLoadId, MassiveLoadProcessStatus.Events, false, true);

                                if (massiveEmissionRowsWithEvents.Count() > 0)
                                {
                                    foreach (var item in massiveEmissionRowsWithEvents.OrderBy(x => x.RowNumber))
                                    {
                                        temporaryInfo = DelegateService.printingService.GetTemporaryByOpetarionId(item.Risk.Policy.Id);
                                        temporaryInfo.CommonProperties.UserId = SessionHelper.GetUserId();
                                        temporaryInfo.CommonProperties.UserName = SessionHelper.GetUserName();
                                        temporaryInfo.CommonProperties.IsMassive = true;
                                        temporaryInfo.CommonProperties.RiskSince = 1;
                                        temporaryInfo.CommonProperties.RiskUntil = 1;
                                        temporaryInfo.OperationId = item.Risk.Policy.Id;
                                        temporaryInfo.IsCollective = false;

                                        if (lstPolicyInfo.Count < iTotalRisksByUser)
                                            lstPolicyInfo.Add(temporaryInfo);
                                    }
                                }

                                else
                                {
                                    List<MassiveRenewalRow> massiveRenewalRows = DelegateService.massiveRenewalService.GetMassiveRenewalRowsByMassiveLoadIdMassiveLoadProcessStatus(massiveLoadId, MassiveLoadProcessStatus.Events, false, false);

                                    if (massiveRenewalRows.Count() > 0)
                                    {
                                        foreach (var item in massiveRenewalRows.OrderBy(x => x.RowNumber))
                                        {
                                            temporaryInfo = DelegateService.printingService.GetTemporaryByOpetarionId(item.Risk.Policy.Id);
                                            temporaryInfo.CommonProperties.UserId = SessionHelper.GetUserId();
                                            temporaryInfo.CommonProperties.UserName = SessionHelper.GetUserName();
                                            temporaryInfo.CommonProperties.IsMassive = true;
                                            temporaryInfo.CommonProperties.RiskSince = 1;
                                            temporaryInfo.CommonProperties.RiskUntil = 1;
                                            temporaryInfo.OperationId = item.Risk.Policy.Id;
                                            temporaryInfo.IsCollective = false;

                                            if (lstPolicyInfo.Count < iTotalRisksByUser)
                                                lstPolicyInfo.Add(temporaryInfo);
                                        }
                                    }

                                    List<MassiveRenewalRow> massiveRenewalRowsWithEvents = DelegateService.massiveRenewalService.GetMassiveRenewalRowsByMassiveLoadIdMassiveLoadProcessStatus(massiveLoadId, MassiveLoadProcessStatus.Events, false, true);

                                    if (massiveRenewalRowsWithEvents.Count() > 0)
                                    {
                                        foreach (var item in massiveRenewalRowsWithEvents.OrderBy(x => x.RowNumber))
                                        {
                                            temporaryInfo = DelegateService.printingService.GetTemporaryByOpetarionId(item.Risk.Policy.Id);
                                            temporaryInfo.CommonProperties.UserId = SessionHelper.GetUserId();
                                            temporaryInfo.CommonProperties.UserName = SessionHelper.GetUserName();
                                            temporaryInfo.CommonProperties.IsMassive = true;
                                            temporaryInfo.CommonProperties.RiskSince = 1;
                                            temporaryInfo.CommonProperties.RiskUntil = 1;
                                            temporaryInfo.OperationId = item.Risk.Policy.Id;
                                            temporaryInfo.IsCollective = false;

                                            if (lstPolicyInfo.Count < iTotalRisksByUser)
                                                lstPolicyInfo.Add(temporaryInfo);
                                        }
                                    }
                                }

                                if (lstPolicyInfo.Count > 0)
                                    sPrintingProcess = DelegateService.printingService.GenerateReportMassiveJetForm(lstPolicyInfo, string.Empty, 1, SessionHelper.GetUserId(), lstPolicyInfo.Count, massiveLoadId, collectFormat, idCuotes);
                                else
                                    return new UifJsonResult(false, "Error en la generacion de pdf");

                                return new UifJsonResult(true, new { PrintingProcess = sPrintingProcess });
                            }
                            else
                            {
                                return new UifJsonResult(false, "No es posible imprimir en este estado de cargue");
                            }
                        }
                    case 4:
                    case 5:
                        {
                            if (massiveLoadStatus == 6)
                            {
                                PolicyInfo policyInfo = new PolicyInfo();
                                CollectiveEmission collectiveEmission = DelegateService.collectiveService.GetCollectiveEmissionByMassiveLoadId(massiveLoadId, true);

                                if (collectiveEmission != null)
                                {
                                    companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId((int)collectiveEmission.EndorsementId);
                                    PolicyInfoRisks = DelegateService.printingService.GetRisksByEndorsementId((int)collectiveEmission.EndorsementId);
                                    itotalRisksInEndorsement = (PolicyInfoRisks.CommonProperties.RiskUntil - PolicyInfoRisks.CommonProperties.RiskSince) + 1;
                                }

                                policyInfo.BranchId = companyPolicy.Branch.Id;
                                policyInfo.PolicyNumber = int.Parse(companyPolicy.DocumentNumber.ToString());
                                policyInfo.EndorsementNum = companyPolicy.Endorsement.Number;
                                policyInfo.PolicyId = companyPolicy.Endorsement.PolicyId;
                                policyInfo.EndorsementId = companyPolicy.Endorsement.Id;
                                policyInfo.TicketNumber = companyPolicy.Endorsement.TicketNumber;
                                policyInfo.IsCollective = true;
                                commonProperties.PrefixId = companyPolicy.Prefix.Id;
                                commonProperties.UserId = SessionHelper.GetUserId();
                                commonProperties.UserName = SessionHelper.GetUserName();
                                commonProperties.IsMassive = true;

                                if (iTotalRisksByUser > itotalRisksInEndorsement)
                                {
                                    return new UifJsonResult(false, App_GlobalResources.Language.RankIngressedPrinting + PolicyInfoRisks.CommonProperties.RiskSince + "-" + PolicyInfoRisks.CommonProperties.RiskUntil);
                                }
                                else
                                {
                                    commonProperties.RiskSince = PolicyInfoRisks.CommonProperties.RiskSince;
                                    commonProperties.RiskUntil = (PolicyInfoRisks.CommonProperties.RiskSince + iTotalRisksByUser) - 1;
                                }

                                policyInfo.CommonProperties = commonProperties;

                                lstPolicyInfo.Add(policyInfo);

                                if (lstPolicyInfo.Count > 0)
                                    sPrintingProcess = DelegateService.printingService.GenerateReportMassiveJetForm(lstPolicyInfo, string.Empty, 1, SessionHelper.GetUserId(), lstPolicyInfo.Count, massiveLoadId, collectFormat, idCuotes);
                                else
                                    return new UifJsonResult(false, "Error en la generacion de pdf");

                                return new UifJsonResult(true, new { PrintingProcess = sPrintingProcess });
                            }
                            else if (massiveLoadStatus == 4)
                            {
                                CollectiveEmission collectiveEmission = DelegateService.collectiveService.GetCollectiveEmissionByMassiveLoadId(massiveLoadId, true);

                                if (collectiveEmission != null)
                                {
                                    temporaryInfo = DelegateService.printingService.GetTemporaryByOpetarionId(collectiveEmission.TemporalId);
                                    itotalRisksInEndorsement = (temporaryInfo.CommonProperties.RiskUntil - temporaryInfo.CommonProperties.RiskSince) + 1;
                                }

                                temporaryInfo.CommonProperties.UserId = SessionHelper.GetUserId();
                                temporaryInfo.CommonProperties.UserName = SessionHelper.GetUserName();
                                temporaryInfo.CommonProperties.IsMassive = true;
                                temporaryInfo.IsCollective = true;
                                temporaryInfo.OperationId = collectiveEmission.TemporalId;

                                if (iTotalRisksByUser > itotalRisksInEndorsement)
                                {
                                    return new UifJsonResult(false, App_GlobalResources.Language.RankIngressedPrinting + temporaryInfo.CommonProperties.RiskSince + "-" + temporaryInfo.CommonProperties.RiskUntil);
                                }
                                else
                                {
                                    temporaryInfo.CommonProperties.RiskUntil = (temporaryInfo.CommonProperties.RiskSince + iTotalRisksByUser) - 1;
                                }

                                lstPolicyInfo.Add(temporaryInfo);

                                if (lstPolicyInfo.Count > 0)
                                    strPath = DelegateService.printingService.GenerateReportMassiveJetForm(lstPolicyInfo, string.Empty, 1, SessionHelper.GetUserId(), lstPolicyInfo.Count, massiveLoadId, collectFormat, idCuotes);
                                else
                                    return new UifJsonResult(false, "Error en la generacion de pdf");

                                return new UifJsonResult(true, new { PrintingProcess = strPath });
                            }
                            else
                            {
                                return new UifJsonResult(false, "No es posible imprimir en este estado de cargue");
                            }
                        }
                    default:
                        return new UifJsonResult(false, "No es posible imprimir en este estado de cargue");
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, "Error al consultar proceso de impresion");
            }
        }

        private string ValidatePdf(string UrlPdf, int RiskCount, int massiveLoadId)
        {
            int iIntestosImpresion = int.Parse(ConfigurationManager.AppSettings["IntentosImpresion"].ToString());
            bool bError = false;
            for (int i = 0; i < iIntestosImpresion; i++)
            {
                if (!System.IO.File.Exists(UrlPdf))
                {
                    System.Threading.Thread.Sleep(1000);
                    bError = true;
                }
                else
                {
                    bError = false;
                    break;
                }
            }

            if (!bError)
                return UrlPdf;
            else
            {
                CompanyPrinting companyPrinting = DelegateService.printingService.SavePrintingProcess(UrlPdf, 2, SessionHelper.GetUserId(), RiskCount, massiveLoadId);
                return companyPrinting.Id.ToString();
            }
        }

        private List<ReportEndorsement> GetPolicyForCollectFormat(int branchId, int prefixId, int policyNumber)
        {
            try
            {
                ReportPolicy reportPolicy = DelegateService.collectionFormBusinessService.GetPolicybyBranchPrefixDocument(branchId, prefixId, policyNumber);
                List<ReportEndorsement> listEndorsements = new List<ReportEndorsement>();
                bool bError = false;

                if (reportPolicy.Endorsements != null)
                {
                    for (int i = 0; i < reportPolicy.Endorsements.Count; i++)
                    {
                        ReportEndorsement endorsementList = new ReportEndorsement();

                        endorsementList.EndorsementNumber = reportPolicy.Endorsements[i].Id;
                        endorsementList.BranchId = Convert.ToInt32(branchId);
                        endorsementList.PrefixId = Convert.ToInt32(prefixId);
                        endorsementList.DocumentNumber = policyNumber.ToString();
                        endorsementList.FailureText = "OK";
                        endorsementList.Quotes = new List<PolicyQuote>();
                        for (int j = 0; j < reportPolicy.Endorsements[i].Payers.Count; j++)
                        {
                            for (int k = 0; k < reportPolicy.Endorsements[i].Payers[j].PaymentSchedule.Installments.Count; k++)
                            {
                                PolicyQuote policyQuote = new PolicyQuote();

                                if (Convert.ToDateTime(reportPolicy.Endorsements[i].Payers[j].PaymentSchedule.Installments[k].DueDate).Date < DateTime.Now.Date)
                                {
                                    policyQuote.PayerName = "-1";
                                    endorsementList.Quotes.Add(policyQuote);
                                    bError = true;
                                    break;
                                }
                                else
                                {
                                    policyQuote.PayerName = reportPolicy.Endorsements[i].Payers[j].Name;
                                    policyQuote.IndividualPayerId = reportPolicy.Endorsements[i].Payers[j].IndividualId;
                                    policyQuote.QuoteNumber = reportPolicy.Endorsements[i].Payers[j].PaymentSchedule.Installments[k].InstallmentNumber;
                                    policyQuote.Date = (Convert.ToDateTime(reportPolicy.Endorsements[i].Payers[j].PaymentSchedule.Installments[k].DueDate)).ToString("dd/MM/yyyy");
                                    policyQuote.TotalValue = Convert.ToDouble(reportPolicy.Endorsements[i].Payers[j].PaymentSchedule.Installments[k].Amount.Value);
                                    policyQuote.State = (!reportPolicy.Endorsements[i].Payers[j].PaymentSchedule.Installments[k].IsPartialPaid) ? "PENDIENTE" : "PARCIAL";
                                    policyQuote.QuoteValue = Convert.ToDouble(reportPolicy.Endorsements[i].Payers[j].PaymentSchedule.Installments[k].PaidAmount.Value);
                                    endorsementList.Quotes.Add(policyQuote);
                                }
                            }
                        }
                        listEndorsements.Add(endorsementList);
                    }
                }
                if (!bError)
                    return listEndorsements;
                else
                    return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string GenerateReportCollectFormat(List<ReportEndorsement> endorsementList, string[] idCuotes, int branchId, int prefixId, int endorsementId, int policyNumber)
        {
            string[] paths = new String[2];

            try
            {

                CollectionForm collectionForm = new CollectionForm();
                ReportPolicy reportPolicy = new ReportPolicy();
                List<ReportEndorsement> endorsements = new List<ReportEndorsement>();

                if (idCuotes != null)
                {
                    foreach (var endoso in endorsementList.Where(x => x.EndorsementNumber == endorsementId))
                    {
                        reportPolicy.BranchId = endoso.BranchId;
                        reportPolicy.PrefixId = endoso.PrefixId;
                        reportPolicy.DocumentNumber = policyNumber;
                        reportPolicy.EndorsementId = policyNumber;
                        ReportEndorsement endorsement = new ReportEndorsement();
                        endorsement.EndorsementNumber = endoso.EndorsementNumber;
                        List<ReportPayer> lstPayers = new List<ReportPayer>();

                        List<ReportPayer> lstTeportPayers = new List<ReportPayer>();
                        foreach (var quote in endoso.Quotes)
                        {
                            for (Int32 i = 0; i < idCuotes.Count(); i++)
                                if (Convert.ToInt32(idCuotes[i]) == quote.QuoteNumber)
                                {
                                    ReportPayer payer = new ReportPayer();
                                    payer.IndividualId = quote.IndividualPayerId;
                                    payer.Name = quote.PayerName;
                                    ReportPaymentSchedule paymentSchedule = new ReportPaymentSchedule();
                                    List<ReportInstallment> listInstallment = new List<ReportInstallment>();
                                    ReportInstallment installment = new ReportInstallment();
                                    installment.DueDate = Convert.ToDateTime(quote.Date);
                                    installment.InstallmentNumber = quote.QuoteNumber;
                                    installment.IsPaid = (quote.State.Equals("PENDIENTE")) ? Convert.ToBoolean(0) : Convert.ToBoolean(1);
                                    ReportAmount paidAmount = new ReportAmount();
                                    paidAmount.Value = Convert.ToDecimal(quote.QuoteValue);
                                    installment.PaidAmount = paidAmount;
                                    listInstallment.Add(installment);
                                    paymentSchedule.Installments = listInstallment;
                                    payer.PaymentSchedule = paymentSchedule;
                                    lstTeportPayers.Add(payer);
                                }
                        }

                        endorsement.Payers = lstTeportPayers;
                        endorsements.Add(endorsement);
                    }
                }
                reportPolicy.Endorsements = endorsements;
                reportPolicy.userId = SessionHelper.GetUserId();
                collectionForm.Policy = reportPolicy;

                paths = DelegateService.collectionFormBusinessService.GenerateCollectionForm(collectionForm);
                string strPath = paths[0];
                if (strPath != "-1")
                {
                    var filenamefromPath = strPath.Split(new char[] { '\\' }).Last();
                    return strPath;
                }
                else
                {
                    return "Error en la generacion del formato de recaudo";
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}