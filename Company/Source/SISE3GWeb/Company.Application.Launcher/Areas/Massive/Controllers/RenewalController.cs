//using Sistran.Company.Application.MassiveServices.Models;
//using Sistran.Company.Application.UnderwritingServices.Models;
//using Sistran.Core.Application.CommonService.Models;
//using Sistran.Core.Application.UnderwritingServices.Enums;
//using Sistran.Core.Application.UnderwritingServices.Models;
//using Sistran.Core.Application.UniquePersonService.Enums;
//using Sistran.Core.Application.UniquePersonService.Models;
//using Sistran.Core.Framework.UIF.Web.Areas.Massive.Models;
//using Sistran.Core.Framework.UIF.Web.Helpers;
//using Sistran.Core.Framework.UIF.Web.Models;
//using Sistran.Core.Framework.UIF.Web.Services;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web.Mvc;

//namespace Sistran.Core.Framework.UIF.Web.Areas.Massive.Controllers
//{
//    public class RenewalController : Controller
//    {
//        private static List<int> prefixesExcluded = new List<int>();

//        public ActionResult Renewal()
//        {
//            Parameter parameter = DelegateService.commonService.GetParameterByParameterId(10009);

//            if (parameter != null && !string.IsNullOrEmpty(parameter.TextParameter))
//            {
//                string[] prefixes = parameter.TextParameter.Split(',');

//                for (int i = 0; i < prefixes.Length; i++)
//                {
//                    prefixesExcluded.Add(Convert.ToInt32(prefixes[i]));
//                }
//            }

//            return View();
//        }

//        public ActionResult GetPrefixes()
//        {
//            try
//            {
//                List<Prefix> prefixes = DelegateService.commonService.GetPrefixes();

//                if (prefixes.Count == 0)
//                {
//                    return new UifJsonResult(false, App_GlobalResources.Language.MessageNotFoundPrefixes);
//                }
//                else
//                {
//                    prefixes = prefixes.Where(x => !prefixesExcluded.Any(y => y == x.Id)).ToList();
//                    return new UifJsonResult(true, prefixes.OrderBy(x => x.Description).ToList());
//                }
//            }
//            catch (Exception)
//            {
//                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPrefixes);
//            }
//        }

//        public ActionResult GetAgenciesByUserIdAgentIdDescription(int agentId, string description)
//        {
//            try
//            {
//                List<Agency> agencies = DelegateService.uniqueUserService.GetAgenciesByUserIdAgentIdDescription(SessionHelper.GetUserId(), agentId, description);

//                if (agencies.Count == 0)
//                {
//                    return new UifJsonResult(false, App_GlobalResources.Language.MessageNotFoundIntermediaries);
//                }
//                else if (agencies.Count == 1)
//                {
//                    if (agencies.Exists(x => x.DateDeclined.GetValueOrDefault() > DateTime.MinValue || x.Agent.DateDeclined.GetValueOrDefault() > DateTime.MinValue))
//                    {
//                        return new UifJsonResult(false, App_GlobalResources.Language.ErrorIntermediaryDisabled);
//                    }
//                    else
//                    {
//                        return new UifJsonResult(true, agencies);
//                    }
//                }
//                else
//                {
//                    return new UifJsonResult(true, agencies);
//                }
//            }
//            catch (Exception)
//            {
//                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchAgent);
//            }
//        }

//        public ActionResult GetBranches()
//        {
//            try
//            {
//                List<Branch> branches = DelegateService.uniqueUserService.GetBranchesByUserId(SessionHelper.GetUserId());

//                if (branches.Count == 0)
//                {
//                    return new UifJsonResult(false, App_GlobalResources.Language.MessageNotFoundBranchesByUser);
//                }
//                else
//                {
//                    return new UifJsonResult(true, branches.OrderBy(x => x.Description).ToList());
//                }
//            }
//            catch (Exception)
//            {
//                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryBranches);
//            }
//        }

//        public ActionResult GetHoldersByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType customerType)
//        {
//            try
//            {
//                List<Holder> holders = DelegateService.underwritingService.GetHoldersByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType);
//                holders.ForEach(x => x.Name = (x.Surname + " " + (string.IsNullOrEmpty(x.SecondSurname) ? "" : x.SecondSurname + " ") + x.Name));

//                if (holders.Count == 0)
//                {
//                    return new UifJsonResult(false, App_GlobalResources.Language.MessageSearchHolders);
//                }
//                else if (holders.Count == 1)
//                {
//                    if (holders.Exists(x => x.DeclinedDate > DateTime.MinValue))
//                    {
//                        return new UifJsonResult(false, App_GlobalResources.Language.ErrorPolicyholderDisabled);
//                    }
//                    else
//                    {
//                        return new UifJsonResult(true, holders);
//                    }
//                }
//                else
//                {
//                    return new UifJsonResult(true, holders);
//                }
//            }
//            catch (Exception)
//            {
//                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchHolders);
//            }
//        }

//        public ActionResult GetCoRequestsByDescription(string description)
//        {
//            try
//            {
//                List<CompanyRequest> coRequests = DelegateService.massiveService.GetCoRequestByDescription(description);

//                if (coRequests.Count == 0)
//                {
//                    return new UifJsonResult(false, App_GlobalResources.Language.MessageSearchRequests);
//                }
//                else
//                {
//                    return new UifJsonResult(true, coRequests);
//                }
//            }
//            catch (Exception)
//            {
//                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryingGroupRequest);
//            }
//        }

//        public ActionResult GetPoliciesByRenewalViewModel(RenewalViewModel renewalViewModel)
//        {
//            try
//            {
//                if (ModelState.IsValid)
//                {
//                    List<Policy> policies = DelegateService.massiveRenewalService.GetPoliciesByDueDate(ModelAssembler.CreatePolicyByRenewalViewModel(renewalViewModel));

//                    if (policies.Count == 0)
//                    {
//                        return new UifJsonResult(false, App_GlobalResources.Language.MessageNotFoundPolicies);
//                    }
//                    else
//                    {
//                        policies.Where(x => x.Endorsement.EndorsementType.HasValue).ToList().ForEach(y => y.Endorsement.EndorsementTypeDescription = EnumsHelper.GetItemName<EndorsementType>(y.Endorsement.EndorsementType.Value));
//                        return new UifJsonResult(true, policies);
//                    }
//                }
//                else
//                {
//                    return new UifJsonResult(false, ViewModelError.GetMessages(ModelState.Values));
//                }
//            }
//            catch (Exception)
//            {
//                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchPolicies);
//            }
//        }

//        public ActionResult GenerateProcess(List<CompanyPolicy> policies)
//        {
//            try
//            {
//                if (policies.Any(x => x.Endorsement.EndorsementType == null))
//                {
//                    int idProcess = DelegateService.massiveRenewalService.GenerateProcessRenewal(SessionHelper.GetUserId(), App_GlobalResources.Language.MassiveRenovation, policies.Where(x => x.Endorsement.EndorsementType == null).ToList());
//                    return new UifJsonResult(true, App_GlobalResources.Language.ProcessNo + " " + idProcess);
//                }
//                else
//                {
//                    return new UifJsonResult(false, App_GlobalResources.Language.MessageSelectMinimumPolicies);
//                }
//            }
//            catch (Exception)
//            {
//                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGenerateProcess);
//            }
//        }

//        //public ActionResult DeleteMassiveRenewalProcess(int id)
//        //{
//        //    try
//        //    {
//        //        AsynchronousProcess process = DelegateService.commonService.GetAsynchronousProcessById(id);
//        //        if (process.StatusId == (int)ProcessRenewalStatus.Finalized)
//        //        {
//        //            return new UifJsonResult(false, App_GlobalResources.Language.CantBeDeleted);
//        //        }
//        //        else
//        //        {
//        //            bool deleted = DelegateService.massiveRenewalService.DeleteMassiveRenewalProcess(id);
//        //            return new UifJsonResult(true, string.Format(App_GlobalResources.Language.ProcessCorrectlyDeleted, id));
//        //        }
//        //    }
//        //    catch (Exception)
//        //    {
//        //        return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetProcess);
//        //    }
//        //}

//        //public ActionResult FinalizeMassiveRenewalProcess(int id, List<RenewalProcess> renewalProcesses)
//        //{
//        //    try
//        //    {
//        //        renewalProcesses = renewalProcesses.Where(x => x.Status == ProcessTemporalStatus.Finalized).ToList();


//        //        if (id > 0 && renewalProcesses.Count > 0)
//        //        {
//        //            MassiveRenewalProcess massiveRenewal = DelegateService.massiveRenewalService.FinalizeMassiveRenewalProcess(id, renewalProcesses);

//        //            if (massiveRenewal == null)
//        //            {
//        //                return new UifJsonResult(false, App_GlobalResources.Language.MessageNotFoundProcess);
//        //            }
//        //            else if (massiveRenewal.HasError)
//        //            {
//        //                return new UifJsonResult(false, massiveRenewal.ErrorDescription);
//        //            }
//        //            else
//        //            {
//        //                return new UifJsonResult(true, App_GlobalResources.Language.ProcessNo + " " + id);
//        //            }
//        //        }
//        //        else
//        //        {
//        //            return new UifJsonResult(false, App_GlobalResources.Language.MessageNotPoliciesToRenovate);
//        //        }
//        //    }
//        //    catch (Exception)
//        //    {
//        //        return new UifJsonResult(false, App_GlobalResources.Language.ErrorRenovateTemporals);
//        //    }
//        //}

//        public ActionResult GenerateFileErrorsMassiveRenewal(int massiveLoadId)
//        {
//            try
//            {
//                string urlFile = DelegateService.massiveRenewalService.GenerateFileErrorsMassiveRenewal(massiveLoadId);

//                if (string.IsNullOrEmpty(urlFile))
//                {
//                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorFileNotFound);
//                }
//                else
//                {
//                    return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile);
//                }
//            }
//            catch (Exception)
//            {
//                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
//            }
//        }

//        //public ActionResult GenerateFileToPayrollByAgent(int processId, List<RenewalProcess> renewalProcesses)
//        //{
//        //    try
//        //    {
//        //        string urlFile = DelegateService.massiveRenewalService.GenerateFileToPayrollByAgent(renewalProcesses, App_GlobalResources.Language.PayrollAgent + processId.ToString());

//        //        if (string.IsNullOrEmpty(urlFile))
//        //        {
//        //            return new UifJsonResult(false, App_GlobalResources.Language.ErrorFileNotFound);
//        //        }
//        //        else
//        //        {
//        //            return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile);
//        //        }
//        //    }
//        //    catch (Exception)
//        //    {
//        //        return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
//        //    }
//        //}
//    }
//}
