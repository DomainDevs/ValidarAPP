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
//    using Sistran.Core.Application.UnderwritingServices.Enums;
//    using Sistran.Core.Application.UnderwritingServices.Models;
//    using Sistran.Core.Framework.UIF.Web.Helpers;    
//    public class RequestGroupingController : Controller
//    {        
//        /// <summary>
//        /// Obtener agente
//        /// </summary>
//        public ActionResult GetAgentByIndividualId(int individualId)
//        {
//            Agent agent = DelegateService.uniquePersonService.GetAgentByIndividualId(individualId);

//            return new UifJsonResult(true, agent);
//        }

//        /// <summary>
//        /// Consulta una solicitud agrupadora dado su identificador
//        /// </summary>
//        /// <param name="requestId"> Identificador de la solicitud agrupadora </param>
//        /// <returns></returns>
//        public ActionResult GetCoRequestByRequestId(int requestId)
//        {
//            try
//            {
//                CompanyRequest model = DelegateService.massiveService.GetCoRequestByRequestId(requestId);

//                CoRequestViewModel viewModel = new CoRequestViewModel();
//                //CoRequestEndorsement endorsement = model.CoRequestEndorsement.SingleOrDefault();
//                //CoRequestAgent principalAgent = endorsement.CoRequestAgent.Where(c => c.Agency.IsPrincipal == true).SingleOrDefault();

//                //viewModel.Request = model;
//                //viewModel.RequestEndorsement = endorsement;
//                //viewModel.PrincipalAgent = principalAgent;

//                return new UifJsonResult(true, viewModel);
//            }

//            catch (Exception)
//            {
//                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryingGroupRequest);
//            }
//        }

//        /// <summary>
//        /// Consulta una solicitud agrupadora dado su identificador ó descripción
//        /// </summary>
//        /// <param name="requestId"> Identificador de la solicitud agrupadora </param>
//        /// <returns></returns>
//        public ActionResult GetCoRequestByRequestIdDescription(string description, int billingGroupId)
//        {
//            try
//            {
//                int requestId = 0;
//                Int32.TryParse(description, out requestId);
//                List<CoRequestViewModel> viewModels = new List<CoRequestViewModel>();

//                List<CompanyRequest> models = DelegateService.massiveService.GetCompanyRequestsByBillingGroupIdDescriptionRequestNumber(billingGroupId, description, null);
                
//                foreach (CompanyRequest item in models) 
//                {
//                    CoRequestViewModel viewModel = new CoRequestViewModel();
//                    CompanyRequestEndorsement endorsement = item.CompanyRequestEndorsements.Last();
//                    Agency principalAgent = endorsement.Agencies.Where(c => c.IsPrincipal == true).FirstOrDefault();                    
//                    viewModel.Request = item;
//                    //viewModel.RequestEndorsement = endorsement;
//                    //viewModel.PrincipalAgent = principalAgent;
//                    viewModels.Add(viewModel);
//                }
//                return new UifJsonResult(true, viewModels);
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
//        public ActionResult SaveCoRequest(CompanyRequest model)
//        {
//            try
//            {
//                if (ModelState.IsValid)
//                {
//                    int userId = SessionHelper.GetUserId();
//                    CompanyRequest newModel = DelegateService.massiveService.CreateCompanyRequest(model);
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
//        /// Vista para crear solicitud agrupadora
//        /// </summary>
//        /// <returns>Vista para crear solicitud agrupadora</returns>
//        public ActionResult RequestGrouping()
//        {
//            RequestGroupingViewModel model = new RequestGroupingViewModel();
//            model.CurrentFrom = DateTime.Now;
//            model.CurrentTo = model.CurrentFrom.AddYears(1);

//            return View(model);
//        }

//        /// <summary>
//        /// Obtiene las sucursales de un usuario
//        /// </summary>
//        /// <returns> Listado de sucursales de un usuario </returns>
//        public ActionResult GetBranches()
//        {
//            int userId = SessionHelper.GetUserId();
//            List<Branch> branches = DelegateService.uniqueUserService.GetBranchesByUserId(userId);

//            return new UifSelectResult(branches.OrderBy(x => x.Description).ToList());
//        }

//        /// <summary>
//        /// Obtiene ramos comerciales por Id del intermediario
//        /// </summary>
//        /// <param name="agentId"> Identificador del intermediario </param>
//        /// <returns> Listado de ramos comerciales asociados al intermediario </returns>
//        //TODO: Filtrar por habilitado para agrupadora
//        public List<Sistran.Core.Application.CommonService.Models.Prefix> GetPrefixesByAgentIdList (int agentId)
//        {
//            List<Sistran.Core.Application.CommonService.Models.Prefix> prefixes = DelegateService.massiveService.GetPrefixesByAgentId(agentId);
//            return prefixes.OrderBy(x => x.Description).ToList();
//        }

//        /// <summary>
//        /// Obtiene ramos comerciales por Id del intermediario
//        /// </summary>
//        /// <param name="agentId"> Identificador del intermediario </param>
//        /// <returns> Listado de ramos comerciales asociados al intermediario </returns>
//        //TODO: Filtrar por habilitado para agrupadora
//        public ActionResult GetPrefixesByAgentId(int agentId)
//        {
//            return new UifSelectResult(GetPrefixesByAgentIdList(agentId));
//        }

//        /// <summary>
//        /// Consulta los productos por intermediario y ramo comercial
//        /// </summary>
//        /// <param name="agentId">Identificador del intermediario </param>
//        /// <param name="prefixId"> Identificador del ramo comercial </param>
//        /// <returns> Listado de productos asociados a un intermediario y ramo comercial </returns>
//        public ActionResult GetProductsByAgentIdPrefixId(int agentId, int prefixId)
//        {
//            List<Product> products = DelegateService.massiveService.GetProductsByAgentIdPrefixId(agentId, prefixId);
//            return new UifSelectResult(products.OrderBy(x => x.Description).ToList());
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
//                if (policyTypes.Count > 0)
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
//                if (paymentPlans.Count > 0)
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

//        /// <summary>
//        /// Consulta las agencias dado el id de un intermediario
//        /// </summary>
//        /// <param name="agentId">Identificador del intermediario</param>
//        /// <returns>Agencias del intermediario</returns>
//        public ActionResult GetAgenciesByAgentId(int agentId)
//        {
//            List<Agency> agencies = DelegateService.uniquePersonService.GetAgenciesByAgentId(agentId);
//            return new UifSelectResult(agencies.OrderBy(x => x.FullName).ToList());
//        }

//        public ActionResult GetHoldersByDescription(string description, InsuredSearchType insuredSearchType = InsuredSearchType.DocumentNumber)
//        {
//            try
//            {
//                List<Holder> holders = DelegateService.underwritingService.GetHoldersByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, CustomerType.Individual);
//                holders.ForEach(x => x.Name = (x.Surname + " " + (string.IsNullOrEmpty(x.SecondSurname) ? "" : x.SecondSurname + " ") + x.Name));

//                if (holders.Count == 1)
//                {
//                    if (holders[0].InsuredId == 0)
//                    {
//                        return new UifJsonResult(false, App_GlobalResources.Language.ErrorPolicyholderWithoutRol);
//                    }
//                    else if (holders[0].DeclinedDate > DateTime.MinValue)
//                    {
//                        return new UifJsonResult(false, App_GlobalResources.Language.ErrorHolderDisabledIssue);
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
//                return new UifJsonResult(false, App_GlobalResources.Language.ErrorConsultPolicyholder);
//            }
//        }

//        public UifJsonResult GetAgenciesByAgentIdDescription(int agentId, string description)
//        {
//            List<Agency> agencies = DelegateService.uniquePersonService.GetAgenciesByAgentIdDescription(agentId, description);

//            if (agencies.Count == 1)
//            {
//                if (agencies[0].Agent.DateDeclined > DateTime.MinValue)
//                {
//                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorIntermediaryDischarged);
//                }
//                else if (agencies[0].DateDeclined > DateTime.MinValue)
//                {
//                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorAgencyDischarged);
//                }
//                else
//                {
//                    return new UifJsonResult(true, agencies);
//                }
//            }
//            else
//            {
//                return new UifJsonResult(true, agencies);
//            }
//        }

//        public ActionResult GetAgencyByAgentIdAgencyId(int agentId, int agencyId)
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

//        #region BllingGroup
//        /// <summary>
//        /// Guarda un grupo de facturación
//        /// </summary>
//        /// <param name="billingGroup"> Modelo con la información del nuevo grupo de facturacion </param>
//        /// <returns> Solicitud agrupadora creada </returns>
//        public ActionResult SaveBillingGroup(BillingGroup billingGroup)
//        {
//            try
//            {
//                BillingGroup billingGroupNew = DelegateService.underwritingService.CreateBillingGroup(billingGroup);
//                return new UifJsonResult(true, billingGroupNew);
//            }
//            catch (Exception)
//            {
//                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveBillingGroup);
//            }
//        }

//        /// <summary>
//        /// Consulta grupo de facturacion dado su identificador ó descripción
//        /// </summary>        
//        /// <param name="description">Codigo o descripcion del grupo</param>
//        /// <returns></returns>
//        public ActionResult GetBillingGroupByDescription(string description)
//        {
//            try
//            {
//                List<BillingGroup> billingGroups = DelegateService.underwritingService.GetBillingGroupsByDescription(description);
//                return new UifJsonResult(true, billingGroups);
//            }

//            catch (Exception)
//            {
//                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveBillingGroup);
//            }
//        }
//        #endregion 


//        /// <summary>
//        /// Consulta el producto por Id
//        /// </summary>
//        /// <param name="productId">Identificador del producto </param>
//        /// <returns>Producto</returns>
//        public ActionResult GetProductsByProductId(int productId)
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
//    }
//}
