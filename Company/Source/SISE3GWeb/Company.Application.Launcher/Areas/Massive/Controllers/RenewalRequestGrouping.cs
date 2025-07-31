//using Sistran.Core.Framework.UIF2.Controls.UifSelect;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web.Mvc;
//using Sistran.Core.Application.CommonService.Models;
//using Sistran.Core.Application.UniquePersonService.Models;
//using Sistran.Core.Application.UniquePersonService.Enums;
//using Sistran.Company.Application.MassiveServices.Models;
//using Sistran.Core.Framework.UIF.Web.Areas.Massive.Models;
//using Sistran.Core.Framework.UIF.Web.Models;
//using Sistran.Core.Framework.UIF.Web.Services;

//namespace Sistran.Core.Framework.UIF.Web.Areas.Massive.Controllers
//{
//    using Sistran.Core.Application.UnderwritingServices.Models;
//    using Sistran.Core.Framework.UIF.Web.Helpers;
//    using Sistran.Core.Application.UnderwritingServices.Enums;    
//    public class RenewalRequestGroupingController : Controller
//    {        
//        /// <summary>
//        /// Vista para crear solicitud agrupadora
//        /// </summary>
//        /// <returns>Vista para crear solicitud agrupadora</returns>
//        public ActionResult RenewalRequestGrouping()
//        {
//            RequestGroupingViewModel model = new RequestGroupingViewModel();
//            model.CurrentTo = DelegateService.commonService.GetDate();
//            return View(model);
//        }

//        /// <summary>
//        /// Consulta una solicitud agrupadora dado su identificador
//        /// </summary>
//        /// <param name="requestId"> Identificador de la solicitud agrupadora </param>
//        /// <returns></returns>
//        public ActionResult GetRequestByRequestId(int requestId)
//        {
//            try
//            {
//                CompanyRequest request = DelegateService.massiveService.GetCoRequestByRequestId(requestId);
//                if (request == null)
//                {
//                    return new UifJsonResult(false, App_GlobalResources.Language.NoExistHolderRequest);
//                }

//                return new UifJsonResult(true, request);
//            }

//            catch (Exception)
//            {
//                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryingGroupRequest);
//            }
//        }

//        /// <summary>
//        /// Guarda una nueva solicitud agrupadora
//        /// </summary>
//        /// <param name="model"> Modelo con la información de la nueva solicitud agrupadora </param>
//        /// <returns> Solicitud agrupadora creada </returns>
//        public ActionResult SaveRenewalRequest(CompanyRequest model)
//        {
//            try
//            {
//                if (ModelState.IsValid)
//                {
//                    int userId = SessionHelper.GetUserId();
//                    model.CompanyRequestEndorsements[0].EndorsementDate = DelegateService.commonService.GetDate();
//                    model.CompanyRequestEndorsements[0].EndorsementType = EndorsementType.Renewal;
//                    CompanyRequest newModel = DelegateService.massiveService.SaveRenewalRequest(model, userId);
//                    return new UifJsonResult(true, newModel);
//                }
//                else
//                {
//                    string errorMessage = App_GlobalResources.Language.ErrorCheckRequiredFields;
//                    return new UifJsonResult(false, errorMessage);
//                }
//            }
//            catch (Exception)
//            {
//                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryingGroupRequest);
//            }
//        }


//        /// <summary>
//        /// Obtiene las sucursales de un usuario
//        /// </summary>
//        /// <returns> Listado de sucursales de un usuario </returns>
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
//                    return new UifJsonResult(true, branches.OrderBy(x => x.Description));
//                }
//            }
//            catch (Exception)
//            {
//                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryBranches);
//            }
//        }

//        /// <summary>
//        /// Consulta los tipos de pólizas por producto
//        /// </summary>
//        /// <param name="productId"> Identificador del producto </param>
//        /// <returns> Listado de pólizas dado el identificador de un producto </returns>
//        public ActionResult GetPolicyTypesByProductId(int productId)
//        {
//            try
//            {
//                List<PolicyType> policyTypes = new List<PolicyType>();
//                policyTypes = DelegateService.commonService.GetPolicyTypesByProductId(productId);
//                if (policyTypes != null && policyTypes.Count > 0)
//                {
//                    return new UifJsonResult(true, policyTypes.OrderBy(x => x.Description));
//                }
//                else
//                {
//                    return new UifJsonResult(false, App_GlobalResources.Language.NoPolicyTypesFound);
//                }
//            }
//            catch (Exception)
//            {
//                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchPolicyType);
//            }
//        }

//        public ActionResult GetPaymentPlanByProductId(int productId)
//        {
//            try
//            {
//                List<PaymentPlan> paymentPlans = DelegateService.underwritingService.GetPaymentPlansByProductId(productId);
//                if (paymentPlans != null && paymentPlans.Count > 0)
//                {
//                    return new UifJsonResult(true, paymentPlans.OrderBy(x => x.Description));
//                }
//                else
//                {
//                    return new UifJsonResult(false, App_GlobalResources.Language.NoFoundPaymentPlans);
//                }
//            }
//            catch (Exception)
//            {
//                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchPaymentPlan);
//            }
//        }

//        /// <summary>
//        /// Consulta tomador por id
//        /// </summary>
//        /// <param name="id"> Identificador del tomador </param>
//        /// <param name="searchTypeId"> Identificador del tipo de búsqueda </param>
//        /// <returns> Tomador </returns>
//        public ActionResult GetHoldersById(string id, InsuredSearchType searchTypeId)
//        {
//            try
//            {
//                List<Insured> insureds = DelegateService.uniquePersonService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(id, searchTypeId, CustomerType.Individual);
//                insureds.ForEach(x => x.Name = (x.Surname + " " + (string.IsNullOrEmpty(x.SecondSurname) ? "" : x.SecondSurname + " ") + x.Name));

//                if (insureds.Count == 1)
//                {
//                    if (insureds[0].InsuredId == 0)
//                    {
//                        return new UifJsonResult(false, App_GlobalResources.Language.ErrorPolicyholderWithoutRol);
//                    }
//                    else if (insureds[0].DeclinedDate > DateTime.MinValue)
//                    {
//                        return new UifJsonResult(false, App_GlobalResources.Language.ErrorPolicyholderDisabled);
//                    }
//                    else if (insureds[0].CompanyName == null)
//                    {
//                        return new UifJsonResult(false, App_GlobalResources.Language.ErrorHolderCorrespondenceAddress);
//                    }

//                    Holder holder = new Holder();
//                    holder.IndividualId = insureds[0].IndividualId;
//                    holder.InsuredId = insureds[0].InsuredId;
//                    holder.Name = insureds[0].Name;
//                    holder.CustomerType = Sistran.Core.Application.UniquePersonService.Enums.CustomerType.Individual;
//                    holder.EconomicActivity = insureds[0].EconomicActivity;
//                    holder.PaymentMethod = insureds[0].PaymentMethod;
//                    holder.BirthDate = insureds[0].BirthDate;
//                    holder.IdentificationDocument = insureds[0].IdentificationDocument;

//                    if (insureds[0].DeclinedDate.HasValue)
//                    {
//                        holder.DeclinedDate = insureds[0].DeclinedDate.Value;
//                    }

//                    return new UifJsonResult(true, holder);
//                }
//                else
//                {
//                    return new UifJsonResult(false, App_GlobalResources.Language.SelectSingleHolder);
//                }

//            }
//            catch (Exception)
//            {
//                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchHolders);
//            }
//        }
//        #region Agentes
//        /// <summary>
//        /// Gets the agency by agent identifier by agency identifier.
//        /// </summary>
//        /// <param name="agentId">The agent identifier.</param>
//        /// <param name="agencyId">The agency identifier.</param>
//        /// <returns></returns>
//        public ActionResult GetAgencyByAgentIdByAgencyId(int agentId, int agencyId)
//        {
//            try
//            {
//                Agency agency = DelegateService.uniquePersonService.GetAgencyByAgentIdAgentAgencyId(agentId, agencyId);

//                if (agency != null)
//                {
//                    if (agency.DateDeclined > DateTime.MinValue)
//                    {
//                        return new UifJsonResult(false, App_GlobalResources.Language.ErrorIntermediaryDischarged);
//                    }
//                    else
//                    {
//                        return new UifJsonResult(true, agency);
//                    }
//                }
//                else
//                {
//                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorIntermediaryNoExist);
//                }
//            }
//            catch (Exception)
//            {
//                return new UifJsonResult(false, App_GlobalResources.Language.ErrorValidIntermediary);
//            }
//        }

//        /// <summary>
//        /// Gets the agencies by agent identifier description.
//        /// </summary>
//        /// <param name="agentId">The agent identifier.</param>
//        /// <param name="description">The description.</param>
//        /// <returns></returns>
//        public UifJsonResult GetAgenciesByAgentIdDescription(int agentId, string description)
//        {
//            try
//            {
//                List<Agency> agencies = DelegateService.uniquePersonService.GetAgenciesByAgentIdDescription(agentId, description);

//                if (agencies.Count == 1)
//                {
//                    if (agencies[0].Agent.DateDeclined > DateTime.MinValue)
//                    {
//                        return new UifJsonResult(false, App_GlobalResources.Language.ErrorIntermediaryDischarged);
//                    }
//                    else if (agencies[0].DateDeclined > DateTime.MinValue)
//                    {
//                        return new UifJsonResult(false, App_GlobalResources.Language.ErrorAgencyDischarged);
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

//                return new UifJsonResult(false, App_GlobalResources.Language.ErrorValidIntermediary);
//            }

//        }
//        #endregion Agentes
//        /// <summary>
//        /// Consulta el producto por Id
//        /// </summary>
//        /// <param name="productId">Identificador del producto </param>
//        /// <returns>Producto</returns>
//        public ActionResult GetProductByProductId(int productId)
//        {
//            Product product = DelegateService.underwritingService.GetProductById(productId);
//            if (product != null)
//            {
//                return new UifJsonResult(true, product);
//            }
//            else
//            {
//                return new UifJsonResult(false, product);
//            }

//        }

//        /// <summary>
//        /// Obtener los Ramos  
//        /// </summary>
//        /// <returns></returns>
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
//                    return new UifJsonResult(true, prefixes.OrderBy(x => x.Description));
//                }
//            }
//            catch (Exception)
//            {
//                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPrefixes);
//            }
//        }
//        public ActionResult GetBusinessTypeById(int id)
//        {
//            try
//            {
//                return new UifJsonResult(true, EnumsHelper.GetItemName<BusinessType>(id));
//            }
//            catch (Exception)
//            {

//                return new UifJsonResult(false, "");
//            }
//        }
//        /// <summary>
//        /// Consulta las agencias dado el id de un intermediario
//        /// </summary>
//        /// <param name="agentId">Identificador del intermediario</param>
//        /// <returns>Agencias del intermediario</returns>
//        public ActionResult GetAgenciesByAgentId(int agentId)
//        {
//            try
//            {
//                List<Agency> agencies = DelegateService.uniquePersonService.GetAgenciesByAgentId(agentId);
//                return new UifJsonResult(true, agencies.OrderBy(x => x.FullName).ToList());
//            }
//            catch (Exception)
//            {

//                return new UifJsonResult(false, "");
//            }

//        }
//    }
//}
