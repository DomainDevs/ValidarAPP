using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Framework.UIF.Web.Areas.UniqueUser.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using CPEMC = Sistran.Company.Application.UniquePersonServices.V1.Models;
using UUML = Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.UtilitiesServices.Enums;
using modelsUPersonCompany = Sistran.Company.Application.UniquePersonParamService.Models;
using modelsUUserCompany = Sistran.Company.Application.UniqueUserParamService.Models;
using Sistran.Company.Application.UniqueUserParamService.Models;
using Sistran.Company.Application.CommonServices.Models;
using COUN = Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.UniqueUserServices.Models;
using static Sistran.Core.Application.UniqueUserServices.Enums.UniqueUserTypes;
using Sistran.Company.Application.UniqueUserApplicationServices.DTO;
using Sistran.Core.Application.UniquePersonService.V1.Enums;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using System.Configuration;

namespace Sistran.Core.Framework.UIF.Web.Areas.UniqueUser.Controllers
{
    public class UniqueUserController : Controller
    {
        private static List<Branch> branches = new List<Branch>();
        private static List<UUML.Profile> profiles = new List<UUML.Profile>();


        public ActionResult UniqueUser()
        {
            return View("UniqueUser");
        }

        public PartialViewResult Profiles()
        {
            return PartialView();
        }

        public ActionResult DateNow()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.commonService.GetDate().ToString(DateHelper.FormatDate));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearch);
            }
        }

        public ActionResult GetUserByAccountName(string accountName, int personId, int userId)
        {
            try
            {
                List<UniqueUserModelsView> uniqueUserModelsView = new List<UniqueUserModelsView>();
                uniqueUserModelsView = ModelAssembler.CreateUsersModelView(DelegateService.uniqueUserService.GetUsersByAccountName(accountName, userId, personId));
                if (uniqueUserModelsView != null)
                {
                    if (uniqueUserModelsView.Count == 1)
                    {
                        return new UifJsonResult(true, uniqueUserModelsView);
                    }
                    if (uniqueUserModelsView.Count > 1)
                    {
                        return new UifJsonResult(true, uniqueUserModelsView.OrderBy(x => x.AccountName).ToList());
                    }
                    else
                    {
                        return new UifJsonResult(true, App_GlobalResources.Language.UserNotExist);
                    }
                }
                else
                {
                    return new UifJsonResult(true, App_GlobalResources.Language.UserNotExist);
                }

            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetUser);
            }

        }

        public ActionResult GetUserByName(string accountName)
        {
            try
            {
                List<UUML.User> users = new List<UUML.User>();
                users = DelegateService.uniqueUserService.GetUserByName(accountName).OrderBy(x => x.AccountName).ToList();

                return new UifJsonResult(true, users);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetUser);
            }

        }

        /// <summary>
        /// Save UniqueUsersUser
        /// </summary>
        /// <param name="user">user</param>
        public ActionResult SaveUniqueUser(CompanyUserParam user)
        {
            List<string> errorString = new List<string>();

            try
            {
                UUML.User users = DelegateService.uniqueUserService.GetUserByName(user.AccountName).OrderBy(x => x.AccountName).FirstOrDefault();
                UUML.User usersExists = DelegateService.uniqueUserService.GetCountUsersById(user.PersonId).FirstOrDefault();

                if (users == null || user.UserId != 0)
                {
                    //Only password change
                    if (user.UserId != 0 && user.AccountName != null && user.UniqueUsersLogin.Password != null)
                    {
                        Dictionary<int, int> errors = new Dictionary<int, int>();
                        bool hasChanged = DelegateService.uniqueUserService.ChangePassword(false, user.AccountName, null, user.UniqueUsersLogin.Password, out errors);

                        if (!hasChanged)
                        {
                            foreach (KeyValuePair<int, int> error in errors)
                            {
                                errorString.Add(string.Format(this.GetChangePasswordResource(error.Key), error.Value));
                            }

                            return new UifJsonResult(false, errorString);
                        }
                    }

                    if (user.Branch != null)
                    {
                        CompanyBranch defaultBranch = user.Branch.Where(x => x.IsDefault == true).FirstOrDefault();
                        if (defaultBranch == null)
                        {
                            errorString.Add(App_GlobalResources.Language.ErrorBranchDefault);
                            return new UifJsonResult(false, errorString);
                        }
                        if (user.UniqueUsersProduct == null)
                        {
                            user.UniqueUsersProduct = new List<UUML.UniqueUsersProduct>();
                        }
                    }

                    if (user.UniqueUsersLogin.Password != null)
                    {
                        int PasswordminLenght;
                        PasswordminLenght = Convert.ToInt32(ConfigurationManager.AppSettings["CreateUserPasswordminLenght"]);
                        if (user.UniqueUsersLogin.Password.Count() < PasswordminLenght)
                        {
                            errorString.Add(String.Format(App_GlobalResources.Language.CreateUserPasswordminLenght, PasswordminLenght));
                            return new UifJsonResult(false, errorString);
                        }
                    }

                    if (user.UserId == 0)
                    {
                        user.CreationDate = DateTime.Today;
                        user.CreatedUserId = SessionHelper.GetUserId();
                        user.AuthenticationType = Application.UniqueUserServices.Enums.UniqueUserTypes.AuthenticationType.Standard;
                        user.LastModificationDate = null;
                        user.LockDate = null;
                    }
                    else
                    {
                        user.ModifiedUserId = SessionHelper.GetUserId();
                        user.LastModificationDate = DateTime.Today;
                    }

                    
                    if (usersExists != null && user.UserId == 0)
                    {
                        string userExists;
                        userExists = usersExists.AccountName;
                        errorString.Add(String.Format(App_GlobalResources.Language.PersonWithUserExists, userExists));
                        return new UifJsonResult(false, errorString);
                    }

                    user.UserId = DelegateService.companyUniqueUserParamService.CreateUniqueUser(user);
                    errorString.Add(user.UserId.ToString());
                    return new UifJsonResult(true, errorString);
                }
                else
                {
                    errorString.Add(App_GlobalResources.Language.ErrorUserNameExist);
                    return new UifJsonResult(false, errorString);
                }


            }
            catch (Exception)
            {
                errorString.Add(App_GlobalResources.Language.ErrorSaveUser);
                return new UifJsonResult(false, errorString);
            }
        }

        private string GetErrorMessages()
        {
            StringBuilder sb = new StringBuilder();
            foreach (ModelState item in ModelState.Values)
            {
                if (item.Errors.Count > 0)
                {
                    sb.Append(item.Errors[0].ErrorMessage).Append(", ");
                }
            }
            return sb.ToString().Remove(sb.ToString().Length - 2);
        }
        public ActionResult GetBranches()
        {
            try
            {
                branches = DelegateService.commonService.GetBranches();
                return new UifJsonResult(true, branches.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGettingBranch);
            }
        }

        public ActionResult GetSalesPoint(List<Branch> userBranches)
        {
            try
            {
                if (userBranches == null)
                {
                    userBranches = new List<Branch>();
                }
                foreach (Branch item in userBranches)
                {
                    if (item.SalePoints == null)
                    {
                        item.SalePoints = new List<SalePoint>();
                    }
                }

                branches = DelegateService.commonService.GetBranches();

                foreach (Branch item in branches)
                {
                    Branch branch = userBranches.Where(x => x.Id == item.Id).FirstOrDefault();
                    if (branch == null)
                    {
                        item.SalePoints = new List<SalePoint>();
                        userBranches.Add(item);
                    }
                }
                return new UifJsonResult(true, userBranches.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGettingBranch);
            }
        }
        public ActionResult GetSalePointsByBranchId(int branchId)
        {
            try
            {
                List<SalePoint> salePoints = DelegateService.uniqueUserService.GetSalePointsByBranchIdUserId(branchId, 0);
                return new UifJsonResult(true, salePoints.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGettingBranch);
            }
        }

        public ActionResult GetPersonByIdDescription(string userId, string description)
        {
            try
            {
                List<CPEMC.CompanyPerson> personsExist = DelegateService.uniquePersonServiceV1.GetPersonByDocumentNumberSurnameMotherLastName(userId, "", "", description, (int)IndividualSearchType.Person);
                if (personsExist.Count > 0)
                {
                    return new UifJsonResult(true, personsExist);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.PersonNotFound);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPerson);
            }
        }

        /// <summary>
        /// Busqueda de intermediario por Codigo de agente o descripci�n.
        /// </summary>
        /// <param name="userId">Individual Id del agente</param>
        /// <param name="description">C�digo de agente o nombre del agente.</param>
        /// <returns></returns>
        public ActionResult GetAgentAgencyByIdDescription(string description)
        {
            try
            {
                List<modelsUPersonCompany.SmAgentAgency> agencyModelList = DelegateService.companyUniquePersonParamService.GetAgenAgencyByAgentIdDescription(description);
                if (agencyModelList.Count > 0)
                {
                    return new UifJsonResult(true, agencyModelList);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.AllyIntermediaryNotFound);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetAllyIntermediary);
            }
        }

        /// <summary>
        /// Busqueda de aliados por individual Id y id agencia.
        /// </summary>
        /// <param name="individualId">Individual Id del agente</param>
        /// <param name="AgentAgencyId">C�digo de agente o nombre del agente.</param>
        /// <returns>Listado de aliados.</returns>
        public ActionResult GetAllyByIntermediary(int individualId, int agentAgencyId)
        {
            try
            {
                List<modelsUPersonCompany.SmAlly> allyModelList = DelegateService.companyUniquePersonParamService.GetAllyByIntermediary(individualId, agentAgencyId);
                if (allyModelList != null)
                {
                    return new UifJsonResult(true, allyModelList);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.AllyNotFound);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.AllyNotFound);
            }
        }


        /// <summary>
        /// Busqueda de intermediario por llave primaria.
        /// </summary>
        /// <param name="individualId">Individual Id del agente</param>
        /// <param name="AgentAgencyId">C�digo de agente o nombre del agente.</param>
        /// <returns></returns>
        public ActionResult GetAgentAgencyByPrimaryKey(int individualId, int agentAgencyId)
        {
            try
            {
                modelsUPersonCompany.SmAgentAgency agentAgencyModel = DelegateService.companyUniquePersonParamService.GetAgentAgencyByPrimaryKey(individualId, agentAgencyId);
                if (agentAgencyModel != null)
                {
                    return new UifJsonResult(true, agentAgencyModel);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.AllyIntermediaryNotFound);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetAllyIntermediary);
            }
        }

        /// <summary>
        /// Obtiene el listado de puntos de venta por aliado.
        /// </summary>
        /// <param name="userId">Identificador del usuario</param>
        /// <param name="allianceId">Identificador del aliado.</param>
        /// <param name="individualId">Identificador del individuo.</param>
        /// <param name="agentAgencyId">Identificador del agente.</param>
        /// <returns>Listado de puntos de venta por aliado</returns>
        public ActionResult GetAlliedSalePoints(int userId, int allianceId, int individualId, int agentAgencyId)
        {
            try
            {
                List<modelsUUserCompany.UniqueUserSalePointBranch> uniqueUserSalePointBranchModelList = DelegateService.companyUniqueUserParamService.GetUniqueUserSalePointBranch(userId, allianceId, individualId, agentAgencyId);
                if (uniqueUserSalePointBranchModelList != null)
                {
                    return new UifJsonResult(true, uniqueUserSalePointBranchModelList);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.AllySalePointsNotFound);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetAllySalePointsNotFound);
            }
        }

        /// <summary>
        /// Obtiene el texto de validación para el formulario usuarios puntos de venta aliados. 
        /// </summary>
        /// <param name="userId">Identificador del usuario</param>
        /// <returns>Texto de validación</returns>
        public ActionResult GetUniqueUserSalePointText(int userId)
        {
            try
            {
                string uniqueUserSalePointBranchModelList = DelegateService.companyUniqueUserParamService.GetUniqueUserSalePointText(userId);
                return new UifJsonResult(true, uniqueUserSalePointBranchModelList);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetAllySalePointsNotFound);
            }
        }
        public ActionResult GetPersonByIndividualId(int individualId)
        {
            try
            {
                COUN.Person person = DelegateService.uniquePersonServiceV1.GetPersonByIndividualId(individualId);
                if (person != null)
                {
                    return new UifJsonResult(true, person);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.PersonNotFound);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPerson);
            }

        }

        public ActionResult GetAgenciesByAgentIdDescription(int agentId, string description)
        {
            try
            {
                List<Agency> agencies = DelegateService.uniquePersonServiceV1.GetAgenciesByAgentIdDescription(agentId, description);

                if (agencies.Count == 1)
                {
                    if (agencies[0].Agent.DateDeclined > DateTime.MinValue)
                    {
                        return new UifJsonResult(false, App_GlobalResources.Language.ErrorIntermediaryDischarged);
                    }
                    else if (agencies[0].DateDeclined > DateTime.MinValue)
                    {
                        return new UifJsonResult(false, App_GlobalResources.Language.ErrorAgencyDischarged);
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
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGettingAgents);
            }
        }


        #region AdvancedSearch
        public PartialViewResult UserAdvancedSearch()
        {
            return PartialView();
        }

        public ActionResult GetUserAdvancedSearch(UniqueUserAdvancedSearch user)
        {
            try
            {
                List<UUML.User> users = new List<UUML.User>();
                users = DelegateService.uniqueUserService.GetUserByAdvancedSearch(user.AccountName, user.UserId, user.IdentificationNumber, user.CreationDate, user.Status, user.LastModificationDate);
                return new UifJsonResult(true, users);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPerson);
            }
        }
        public ActionResult GetUsersByQuery(string query)
        {
            try
            {
                int personId;
                bool isNumeric = int.TryParse(query, out personId);
                if (isNumeric)
                {
                    query = "";
                }

                List<UUML.User> users = DelegateService.uniqueUserService.GetUserByTextPersonId(query, personId);
                return Json(users, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(App_GlobalResources.Language.ErrorSearchPolicyholder, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        public ActionResult GetStatusUser()
        {
            try
            {
                return new UifJsonResult(true, EnumsHelper.GetItems<EnabledStatus>());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryAccessType);
            }
        }

        public ActionResult GetBranchesByUserId(int userId)
        {
            try
            {
                List<Branch> branchesUser = DelegateService.uniqueUserService.GetBranchesByUserId(userId);

                return new UifJsonResult(true, branchesUser.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGettingBranch);
            }
        }


        /// <summary>
        /// Consulta los usuarios en la tabla CoHierarchyAccesses segun los parametros
        /// </summary>
        /// <param name="idHierarchy">id de jerarquia</param>
        /// <param name="idModule">id modulo</param>
        /// <param name="idSubmodule">id del submodulo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetUsersByHierarchyModuleSubmodule(int idHierarchy, int idModule, int idSubmodule)
        {
            try
            {
                var users = DelegateService.uniqueUserService.GetUsersByHierarchyModuleSubmodule(idHierarchy, idModule, idSubmodule);
                return new UifJsonResult(true, users);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, e.Message);
            }
        }

        public ActionResult GetUniqueUserProductsStatusByUserId(int userId)
        {
            try
            {
                List<UUML.UniqueUsersProduct> listUserPorduct = DelegateService.uniqueUserService.GetUniqueUserProductsStatusByUserId(userId);

                return new UifJsonResult(true, listUserPorduct.OrderBy(x => x.ProductDescription).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGettingProducts);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ActionResult GetAlliedsByUserId(int userId)
        {
            try
            {
                List<CptUniqueUserSalePointAlliance> uniqueUserSalePointAlliance = new List<CptUniqueUserSalePointAlliance>();
                uniqueUserSalePointAlliance = DelegateService.companyUniqueUserParamService.GetUniqueUserAlliedText(userId);
                if (uniqueUserSalePointAlliance.Count > 0)
                {
                    return new UifJsonResult(true, uniqueUserSalePointAlliance);
                }
                else
                {
                    return new UifJsonResult(false, "No se encontraron puntos de venta aliados, asociados a este usuario");
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetAllySalePointsNotFound);
            }
        }

        public ActionResult GetAlliedsText(List<CptUniqueUserSalePointAlliance> listCptUniqueUserSalePointAlliance)
        {
            if (listCptUniqueUserSalePointAlliance == null)
            {
                listCptUniqueUserSalePointAlliance = new List<CptUniqueUserSalePointAlliance>();
            }
            int countAllieds = listCptUniqueUserSalePointAlliance.Count;
            string resultText;
            try
            {
                if (listCptUniqueUserSalePointAlliance.Count > 0 && listCptUniqueUserSalePointAlliance != null)
                {

                    if (countAllieds > 1)
                    {
                        resultText = App_GlobalResources.Language.LabelVarious;
                    }
                    else if (countAllieds == 1)
                    {
                        resultText = DelegateService.companyUniquePersonParamService.GetAllyByIntermediary(listCptUniqueUserSalePointAlliance[0].IndividualId, listCptUniqueUserSalePointAlliance[0].AgentAgencyId).Where(x => x.AllianceId == listCptUniqueUserSalePointAlliance[0].AllianceId).FirstOrDefault().Description;
                    }
                    else
                    {
                        resultText = "";
                    }
                }
                else
                {
                    resultText = "";
                }
            }
            catch (Exception)
            {
                resultText = "";
            }



            return new UifJsonResult(true, resultText);
        }

        public ActionResult GetTextAllieds(int userId)
        {

            List<CptUniqueUserSalePointAlliance> uniqueUserSalePointAlliance = new List<CptUniqueUserSalePointAlliance>();
            string resultText;

            try
            {
                uniqueUserSalePointAlliance = DelegateService.companyUniqueUserParamService.GetUniqueUserAlliedText(userId);
                if (uniqueUserSalePointAlliance.Count > 0 && uniqueUserSalePointAlliance != null)
                {
                    var Allieds = uniqueUserSalePointAlliance.GroupBy(x => x.AllianceId);
                    int countAllieds = 0;
                    int uniqueAlliance = 0;

                    foreach (IGrouping<int, CptUniqueUserSalePointAlliance> item in Allieds)
                    {
                        countAllieds++;
                        uniqueAlliance = item.Key;
                    }

                    if (countAllieds > 1)
                    {
                        resultText = App_GlobalResources.Language.LabelVarious;
                    }
                    else if (countAllieds == 1)
                    {
                        resultText = DelegateService.companyUniquePersonParamService.GetAllyByIntermediary(uniqueUserSalePointAlliance[0].IndividualId, uniqueUserSalePointAlliance[0].AgentAgencyId).Where(x => x.AllianceId == uniqueAlliance).FirstOrDefault().Description;
                    }
                    else
                    {
                        resultText = "";
                    }
                }
                else
                {
                    resultText = "";
                }
            }
            catch
            {
                resultText = "";
            }



            return new UifJsonResult(true, resultText);
        }


        public ActionResult GetUniqueUserPrefixUsersByUserId(int UserId)
        {
            try
            {

                List<Prefix> prefixes = DelegateService.commonService.GetPrefixes();
                List<Prefix> PrefixUsers = DelegateService.commonService.GetPrefixesByUserId(UserId);

                List<object> retorno = new List<object>();
                retorno.Add(prefixes);
                retorno.Add(PrefixUsers);

                return new UifJsonResult(true, retorno);
            }
            catch (Exception)
            {
                return new UifJsonResult(true, App_GlobalResources.Language.ErrorSearchClauses);
            }

        }

        /// <summary>
        /// Obtain user groups in alphabetical order.
        /// </summary>
        /// <param name="UserId">Id of user.</param>
        /// <returns>List of user groups</returns>
        public ActionResult GetUserGroups(int UserId)
        {
            GroupQueryDTO groups = new GroupQueryDTO();
            UserGroupQueryDTO userGroups = new UserGroupQueryDTO();
            List<UserGroupModelsView> userGroupModel = new List<UserGroupModelsView>();

            try
            {
                groups = DelegateService.UniqueUserApplicationServices.GetApplicationGroup();
                userGroups = DelegateService.UniqueUserApplicationServices.GetApplicationUserGroup(UserId);

                if ((groups.ErrorDTO.ErrorDescription == null) && (userGroups.ErrorDTO.ErrorDescription == null))
                {
                    userGroupModel = ModelAssembler.MappUserGroupModelView(groups, userGroups);

                    return new UifJsonResult(true, userGroupModel.OrderBy(g => g.Description));
                }

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearch);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearch);
            }
        }

        /// <summary>
        /// Save user groups.
        /// </summary>
        /// <param name="groupsSelected">List of user and groups.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveUserGroups(List<UserGroupDTO> groupsSelected)
        {
            UserGroupDTO userGroupDTO = new UserGroupDTO();
            try
            {
                userGroupDTO = DelegateService.UniqueUserApplicationServices.SaveApplicationUserGroup(groupsSelected);

                if (userGroupDTO.ErrorDTO.ErrorDescription == null)
                {
                    return new UifJsonResult(true, App_GlobalResources.Language.MessageSavedSuccessfully);
                }

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveData);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveData);
            }
        }
        #region accessPermissions
        public JsonResult GetPermissionsByUserId(int UserId)
        {
            try
            {
                var permissions = DelegateService.uniqueUserService.GetAccessPermissionsByUserId(UserId);
                return new UifJsonResult(true, permissions);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, e.Message);
            }
        }

        public JsonResult GetPermissionsByProfileId(int profileId)
        {
            try
            {
                var permissions = DelegateService.uniqueUserService.GetAccessPermissionsByProfileId(profileId);
                return new UifJsonResult(true, permissions);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, e.Message);
            }
        }


        public JsonResult GetContextPermissionsByProfileIdPermissionsId(int UserId, int PermissionsId)
        {
            try
            {
                var ContextPermissions = DelegateService.uniqueUserService.GetSecurityContextByProfileIdpermissionsId(UserId, PermissionsId);
                return new UifJsonResult(true, ContextPermissions);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, e.Message);
            }
        }
        public JsonResult SaveContextPermissions(List<UserContextPermissionsModelsView> contextPermissions)
        {
            try
            {
                List<ContextProfileAccessPermissions> ContextsPermissions = ModelAssembler.CreateContextPermissions(contextPermissions);
                var id = DelegateService.uniqueUserService.SaveContextPermissions(ContextsPermissions);
                return new UifJsonResult(true, id);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, e.Message);
            }
        }
        #endregion

        #region private methods
        /// <summary>
        /// Obtener recurso de cambio de contraseña
        /// </summary>
        /// <param name="messageEnum">enum del mesaje</param>
        /// <returns>retorna el mensaje</returns>
        private string GetChangePasswordResource(int messageEnum)
        {
            switch (messageEnum)
            {
                case (int)ChangePasswordErrors.CantHaveSecuence:
                    return App_GlobalResources.Language.PasswordCantHaveSecuence;
                case (int)ChangePasswordErrors.ContainsUserName:
                    return App_GlobalResources.Language.PasswordContainsUserName;
                case (int)ChangePasswordErrors.FirstMustNonNumber:
                    return App_GlobalResources.Language.PasswordFirstMustNonNumber;
                case (int)ChangePasswordErrors.LastMustNotNumber:
                    return App_GlobalResources.Language.PasswordLastMustNotNumber;
                case (int)ChangePasswordErrors.minLenght:
                    return App_GlobalResources.Language.PasswordminLenght;
                case (int)ChangePasswordErrors.MustHaveLower:
                    return App_GlobalResources.Language.PasswordMustHaveLower;
                case (int)ChangePasswordErrors.MustHaveNumber:
                    return App_GlobalResources.Language.PasswordMustHaveNumber;
                case (int)ChangePasswordErrors.MustHaveSpecial:
                    return App_GlobalResources.Language.PasswordMustHaveSpecial;
                case (int)ChangePasswordErrors.MustHaveUpper:
                    return App_GlobalResources.Language.PasswordMustHaveUpper;
                case (int)ChangePasswordErrors.SimilarTohistorical:
                    return App_GlobalResources.Language.PasswordSimilarTohistorical;
                case (int)ChangePasswordErrors.OldPassswordDoesntMatch:
                    return App_GlobalResources.Language.OldPassswordDoesntMatch;
                default:
                    return string.Empty;
            }
        }
        #endregion
    }

}