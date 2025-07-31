using Sistran.Company.Application.SarlaftApplicationServices.DTO;
using Sistran.Company.Application.SarlaftBusinessServices.Enum;

using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using CMSM =Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.EventsServices.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Events.Controllers;
using Sistran.Core.Framework.UIF.Web.Areas.Events.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CMSE=Sistran.Core.Application.CommonService.Enums;

namespace Sistran.Core.Framework.UIF.Web.Areas.Sarlaft.Controllers
{
    /// <summary>
    /// Control de coberturas
    /// </summary>
    public class SarlaftController : Controller
    {
        private static List<Sistran.Core.Application.CommonService.Models.Parameter> parameters = new List<Sistran.Core.Application.CommonService.Models.Parameter>();
        #region PostEntity
        private Helpers.PostEntity entityBranch = new Helpers.PostEntity() { EntityType = "Sistran.Core.Application.Common.Entities.Branch" };
        private Helpers.PostEntity entityCountry = new Helpers.PostEntity() { EntityType = "Sistran.Core.Application.Common.Entities.Country" };
        private Helpers.PostEntity entityState = new Helpers.PostEntity() { EntityType = "Sistran.Core.Application.Common.Entities.State" };
        private Helpers.PostEntity entityCity = new Helpers.PostEntity() { EntityType = "Sistran.Core.Application.Common.Entities.City" };
        private Helpers.PostEntity entityOperationType = new Helpers.PostEntity() { EntityType = "Sistran.Core.Application.Common.Entities.OperationType" };
        private Helpers.PostEntity entityProductType = new Helpers.PostEntity() { EntityType = "Sistran.Company.Application.UniquePerson.Entities.ProductType" };
        private Helpers.PostEntity entityCurrency = new Helpers.PostEntity() { EntityType = "Sistran.Core.Application.Common.Entities.Currency" };
        private Helpers.PostEntity entityDocumentType = new Helpers.PostEntity() { EntityType = "Sistran.Core.Application.UniquePerson.Entities.DocumentType" };
        private Helpers.PostEntity entityResult = new Helpers.PostEntity() { EntityType = "Sistran.Core.Application.Common.Entities.InterviewResult" };
        private Helpers.PostEntity entityLinkType = new Helpers.PostEntity() { EntityType = "Sistran.Company.Application.UniquePerson.Entities.LinkType" };



        List<GenericViewModel> li_Branch = new List<GenericViewModel>();
        List<GenericViewModel> li_Country = new List<GenericViewModel>();
        List<GenericViewModel> li_State = new List<GenericViewModel>();
        List<GenericViewModel> li_City = new List<GenericViewModel>();
        List<GenericViewModel> li_OperationType = new List<GenericViewModel>();
        List<GenericViewModel> li_ProductType = new List<GenericViewModel>();
        List<GenericViewModel> li_Currency = new List<GenericViewModel>();
        List<GenericViewModel> li_Document = new List<GenericViewModel>();
        List<GenericViewModel> li_Result = new List<GenericViewModel>();
        List<GenericViewModel> li_LinkType = new List<GenericViewModel>();

        #endregion

        #region SARLAFT
        /// <summary>
        /// Vista principal de Sarlaft
        /// </summary>
        /// <returns>vista de Sarlaft</returns>
        public ActionResult Sarlaft()
        {
            return View();
        }

        /// <summary>
        /// Consulta el tipo de persona
        /// </summary>
        /// <returns></returns>
        public ActionResult GetPersonTypes()
        {
            try
            {
                int PersonNatural = (int)IndividualType.Person;
                int PersonJuridica = (int)IndividualType.Company;

                return new UifJsonResult(true, EnumsHelper.GetItems<IndividualType>().Where(x => x.Value == PersonNatural.ToString() || x.Value == PersonJuridica.ToString()));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPersonTypes);
            }
        }

        /// <summary>
        /// Consulta la actividades economicas
        /// </summary>
        /// <returns></returns>
        public JsonResult GetEconomicActivities(string query)
        {
            try
            {
                var economicActivities = DelegateService.SarlaftApplicationServices.GetEconomicActivities(query);

                return new UifJsonResult(true, economicActivities);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.Error);
            }
        }

        /// <summary>
        /// Consulta el Usuario
        /// </summary>
        /// <returns></returns>
        public ActionResult GetUserByUserId()
        {
            try
            {
                var userId = SessionHelper.GetUserId();
                var userName = SessionHelper.GetUserName();

                UserDTO user = DelegateService.SarlaftApplicationServices.GetUserByUserId(userId, userName);

                return new UifJsonResult(true, user);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetUsers);
            }
        }

        /// <summary>
        /// Consulta la persona por tipo de persona y numero de documento 
        /// </summary>
        /// <param name="documentNum"></param>
        /// <param name="searchType"></param>
        /// <returns></returns>
        public ActionResult GetPersonByDocumentNumberAndSearchType(string documentNum, int searchType)
        {
            try
            {
                PersonDTO person = DelegateService.SarlaftApplicationServices.GetPersonByDocumentNumberAndSearchType(documentNum, searchType);
                if (person.DocumentNumber == null)
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.PersonNotFound);
                }
                return new UifJsonResult(true, person);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetInsured);
            }

        }

        /// <summary>
        /// Consulta la persona por tipo de persona y numero de documento 
        /// </summary>
        /// <param name="documentNum"></param>
        /// <param name="searchType"></param>
        /// <returns></returns>
        public ActionResult GetPersonByDocumentNumberAndSearchTypeList(string documentNum, int searchType)
        {
            try
            {
                List<PersonDTO> personas = DelegateService.SarlaftApplicationServices.GetPersonByDocumentNumberAndSearchTypeList(documentNum, searchType);
                return new UifJsonResult(true, personas);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }

        }

        public ActionResult GetInsuredsByDescriptionInsuredSearchTypeCustomerType(string description)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.underwritingService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(description, Core.Services.UtilitiesServices.Enums.InsuredSearchType.DocumentNumber, Core.Services.UtilitiesServices.Enums.CustomerType.Individual).FirstOrDefault());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetInsured);
            }

        }

        /// <summary>
        /// Guarda los datos del Sarlaft
        /// </summary>
        /// <returns></returns>
        public ActionResult SaveSarlaft(CustomerKnowledgeDTO customerKnowledgeDTO)
        {
            try
            {
                customerKnowledgeDTO.UserId = SessionHelper.GetUserId();
                if (customerKnowledgeDTO.SarlaftDTO.Id > 0)
                {
                    customerKnowledgeDTO = DelegateService.SarlaftApplicationServices.UpdateSarlaft(customerKnowledgeDTO);
                }
                else
                {
                    customerKnowledgeDTO = DelegateService.SarlaftApplicationServices.CreateSarlaft(customerKnowledgeDTO);
                }

                return new UifJsonResult(true, customerKnowledgeDTO);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveData);
            }
        }

        /// <summary>
        /// Obtiene el historial Sarlaft 
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public ActionResult GetSarlaft(int individualId)
        {
            try
            {
                List<SarlaftDTO> sarlaft = DelegateService.SarlaftApplicationServices.GetSarlaft(individualId);
                return new UifJsonResult(true, sarlaft);

            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.QueryNotData);
            }

        }

        public UifJsonResult GetLastSarlaftId(PersonDTO person)
        {
            try
            {
                List<AuthorizationRequest> authorequest = DelegateService.SarlaftBusinessServices.GetSarlaftAuthorizationRequestByIndividualId(person.IndividualId);
                if (authorequest?.Count > 0)
                {
                    return new UifJsonResult(true, new { IsAuthorizationRequest = true, AuthorizationRequests = authorequest });
                }
                else
                {
                    SarlaftDTO sarlaftDTO = DelegateService.SarlaftApplicationServices.GetLastSarlaftId(person, SessionHelper.GetUserId());
                    return new UifJsonResult(true, sarlaftDTO);
                }

            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetSarlaft);
            }
        }


        /// <summary>
        /// Consulta un sarlaft específico
        /// </summary>
        /// <returns></returns>
        public ActionResult GetSarlaftBySarlaftId(int sarlaftId)
        {
            try
            {
                CustomerKnowledgeDTO sarlaft = DelegateService.SarlaftApplicationServices.GetSarlaftBySarlaftId(sarlaftId);
                return new UifJsonResult(true, sarlaft);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetSarlaft);
            }
        }

        public UifJsonResult GetSarlaftExonerationByIndividualId(int individualId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.SarlaftApplicationServices.GetSarlaftExoneration(individualId));
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.Error);
            }
        }

        public PartialViewResult SarlaftSimpleSearch()
        {
            return PartialView();
        }

        //public ActionResult FillModelView(List<SarlaftSearchModelView> listData)
        //{
        //    List<SarlaftSearchModelView> listmodel = new List<SarlaftSearchModelView>();
        //    listmodel = listData;
        //    return Json(listmodel, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult ExecuteEvents(string tempId)
        {
            try
            {
                int IdUser = SessionHelper.GetUserId();
                NotificationController notification = new NotificationController();
                var Events = notification.GetEventsSForAutorize(IdUser, 20, 32, tempId, "individual");
                var dataEvents = (List<DelegationResultModelView>)Events.Data;
                if (dataEvents.Count() > 0)   /// si existen eventos 
                {
                    // redirige al conntrolador que me lista los eventos para autorizar
                    return RedirectToAction("GetEventsSummary", "Notification",
                        new { area = "Events", IdModule = 20, IdSubmodule = 32, IdTemp = tempId, Type = "individual" });
                }
                else
                {
                    return new UifJsonResult(true, App_GlobalResources.Language.ErrorGetInterwievResults);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetInterwievResults);
            }
        }

        public UifJsonResult ValidationAccessAndHierarchysByUser()
        {
            try
            {
                var isValid = DelegateService.SarlaftBusinessServices.ValidationAccessAndHierarchyByUser(SessionHelper.GetUserId());
                return new UifJsonResult(true, isValid);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.AccessNotFound);
            }

        }

        #endregion

        #region CRUDServices


        public ActionResult GetLinkType()
        {
            try
            {
                this.GetLinkTypes();
                return new UifJsonResult(true, this.li_LinkType);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetInterwievResults);
            }
        }

        private void GetLinkTypes()
        {
            if (this.li_LinkType.Count == 0)
            {
                this.li_LinkType = ModelAssembler.CreateLinkType(
                    ModelAssembler.DynamicToDictionaryList(
                        this.entityLinkType.CRUDCliente.Find(
                            this.entityLinkType.EntityType, null, null
                            )
                        )
                    ).OrderBy(x => x.DescriptionLong).ToList();
            }
        }

        public ActionResult GetInterwievResult()
        {
            try
            {
                this.GetInterwievResults();
                return new UifJsonResult(true, this.li_Result);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetInterwievResults);
            }
        }

        private void GetInterwievResults()
        {
            if (this.li_Result.Count == 0)
            {
                this.li_Result = ModelAssembler.CreateInterviewResult(
                    ModelAssembler.DynamicToDictionaryList(
                        this.entityResult.CRUDCliente.Find(
                            this.entityResult.EntityType, null, null
                            )
                        )
                    ).OrderBy(x => x.DescriptionLong).ToList();
            }
        }
        public ActionResult GetSignatureBranch()
        {
            try
            {
                this.GetSignatureBranchs();
                return new UifJsonResult(true, this.li_Branch);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetBranchs);
            }
        }

        private void GetSignatureBranchs()
        {
            if (this.li_Branch.Count == 0)
            {
                this.li_Branch = ModelAssembler.CreateSignatureBranch(
                    ModelAssembler.DynamicToDictionaryList(
                        this.entityBranch.CRUDCliente.Find(
                            this.entityBranch.EntityType, null, null
                            )
                        )
                    ).OrderBy(x => x.DescriptionLong).ToList();
            }
        }

        public ActionResult GetCountry()
        {
            try
            {
                this.GetCountries();
                this.li_Country.RemoveAll(x => x.DescriptionLong.Trim().Replace(" ", "")
                .Contains(("PAIS DE ALTO RIESGO GAFI").Replace(" ", "")));
                return new UifJsonResult(true, this.li_Country);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchCountry);
            }
        }

        private void GetCountries()
        {
            if (this.li_Country.Count == 0)
            {
                this.li_Country = ModelAssembler.CreateCountry(
                    ModelAssembler.DynamicToDictionaryList(
                        this.entityCountry.CRUDCliente.Find(
                            this.entityCountry.EntityType, null, null
                           )
                        )
                    ).OrderBy(x => x.DescriptionLong).ToList();
            }
        }

        public ActionResult GetState(int CountryId)
        {
            try
            {
                this.GetStates(CountryId);
                return new UifJsonResult(true, this.li_State);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetStates);
            }
        }

        private void GetStates(int CountryId)
        {
            if (this.li_State.Count == 0)
            {
                this.li_State = ModelAssembler.CreateState(
                    ModelAssembler.DynamicToDictionaryList(
                        this.entityState.CRUDCliente.Find(
                            this.entityState.EntityType, null, null
                            )
                        )
                    ).Where(x => x.IdC == CountryId).OrderBy(x => x.DescriptionLong).ToList();
            }
        }

        public ActionResult GetCity(int CountryId, int StateId)
        {
            try
            {
                this.GetCities(CountryId, StateId);
                return new UifJsonResult(true, this.li_City);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetListCities);
            }
        }

        private void GetCities(int CountryId, int StateId)
        {
            if (this.li_City.Count == 0)
            {
                this.li_City = ModelAssembler.CreateCity(
                    ModelAssembler.DynamicToDictionaryList(
                        this.entityCity.CRUDCliente.Find(
                            this.entityCity.EntityType, null, null
                            )
                        )
                    ).Where(x => x.IdC == CountryId && x.IdD == StateId).OrderBy(x => x.DescriptionLong).ToList();
            }
        }
        public ActionResult GetDocumentType()
        {
            try
            {
                this.GetDocumentTypes();
                return new UifJsonResult(true, this.li_Document);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetDocumentTypes);
            }
        }

        private void GetDocumentTypes()
        {
            if (this.li_Document.Count == 0)
            {
                this.li_Document = ModelAssembler.CreateDocumentType(
                    ModelAssembler.DynamicToDictionaryList(
                        this.entityDocumentType.CRUDCliente.Find(
                            this.entityDocumentType.EntityType, null, null
                            )
                        )
                    ).OrderBy(x => x.DescriptionLong).ToList();
            }
        }

        public ActionResult GetOperationType()
        {
            try
            {
                this.GetOperationTypes();
                return new UifJsonResult(true, this.li_OperationType);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetOperationTypes);
            }
        }

        private void GetOperationTypes()
        {
            if (this.li_OperationType.Count == 0)
            {
                this.li_OperationType = ModelAssembler.CreateOperationType(
                    ModelAssembler.DynamicToDictionaryList(
                        this.entityOperationType.CRUDCliente.Find(
                            this.entityOperationType.EntityType, null, null
                            )
                        )
                    ).OrderBy(x => x.DescriptionShort).ToList();
            }
        }
        public ActionResult GetProductType()
        {
            try
            {
                this.GetProductTypes();
                return new UifJsonResult(true, this.li_ProductType);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetProductTypes);
            }
        }

        private void GetProductTypes()
        {
            if (this.li_ProductType.Count == 0)
            {
                this.li_ProductType = ModelAssembler.CreateProductType(
                    ModelAssembler.DynamicToDictionaryList(
                        this.entityProductType.CRUDCliente.Find(
                            this.entityProductType.EntityType, null, null
                            )
                        )
                    ).OrderBy(x => x.DescriptionLong).ToList();
            }
        }

        public ActionResult GetCurrency()
        {
            try
            {
                this.GetCurrencies();
                return new UifJsonResult(true, this.li_Currency);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetCurrencies);
            }
        }

        private void GetCurrencies()
        {
            if (this.li_Currency.Count == 0)
            {
                this.li_Currency = ModelAssembler.CreateCurrency(
                    ModelAssembler.DynamicToDictionaryList(
                        this.entityCurrency.CRUDCliente.Find(
                            this.entityCurrency.EntityType, null, null
                            )
                        )
                    ).OrderBy(x => x.DescriptionLong).ToList();
            }
        }

        #endregion

        #region Links

        public UifJsonResult GetRelationship()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.SarlaftApplicationServices.GetRelationship());
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, "Error");
            }
        }

        public UifJsonResult ExecuteOperationLink(List<LinkDTO> linkDTOs)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.SarlaftApplicationServices.ExecuteOperationLink(linkDTOs));
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, "Error");
            }
        }

        public UifJsonResult GetIndividualLinksByIndividualId(int individualId, int sarlaftId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.SarlaftApplicationServices.GetIndividualLinksByIndividualId(individualId, sarlaftId));
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, "Error");
            }
        }

        #endregion

        #region LegalRepresentative

        public UifJsonResult SaveLegalRepresent(LegalRepresentativeDTO legalRepresentative)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.SarlaftApplicationServices.SaveLegalRepresentative(legalRepresentative));
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveLegalRepresent);
            }
        }

        public UifJsonResult GetLegalRepresentByIndividualId(int individualId, int sarlaftId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.SarlaftApplicationServices.GetLegalRepresentativeByIndividualId(individualId, sarlaftId));
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetLegalRepresentative);
            }
        }
        #endregion

        #region Partners

        public UifJsonResult SavePartner(PartnersDTO partner, int individualId)
        {
            try
            {
                //validar si ya existe el # documento entre los Asociados
                List<PartnersDTO> actualPartners = DelegateService.SarlaftApplicationServices.GetPartnersByIndividualId(individualId, partner.SarlaftId);
                var exist = actualPartners.FirstOrDefault(x => x.IdCardNumero == partner.IdCardNumero);
                if (partner.Status == (int)SarlaftStatus.Create && exist != null) // solo aplica a crear
                {
                    return new UifJsonResult(false, "Ya existe número de documento(accionista/asociado) relacionado al sarlaft");
                }
                return new UifJsonResult(true, partner);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorAddPartner);
            }
        }

        public UifJsonResult GetPartnersByIndividualId(int individualId, int sarlaftId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.SarlaftApplicationServices.GetPartnersByIndividualId(individualId, sarlaftId));
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPartner + "s");
            }
        }

        public UifJsonResult GetPepsByIndividualId(int individualId, int sarlaftId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.SarlaftApplicationServices.GetPepsByIndividualId(individualId, sarlaftId));
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.Peps + "s");
            }
        }

        public ActionResult LoadInitialLegalData(int typeDocument)
        {
            try
            {
                var result = DelegateService.uniquePersonAplicationService.LoadInitialLegalData(typeDocument);
                return new UifJsonResult(true, result);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorPersonLoadInitialData);
            }

        }

        public ActionResult LoadInitialData(bool isEmail)
        {
            try
            {
                var result = DelegateService.uniquePersonAplicationService.LoadInitialData(isEmail);

                return new UifJsonResult(true, result);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorPersonLoadInitialData);
            }

        }

        #endregion

        #region InternationalOperations
        public UifJsonResult GetInternationalOperationsBySarlaftId(int sarlaftId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.SarlaftApplicationServices.GetInternationalOperationsBySarlaftId(sarlaftId));
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.Error);
            }
        }

        public UifJsonResult ExecuteOperation(InternationalOperationDTO internationalOperationDTO)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.SarlaftApplicationServices.ExecuteOperation(internationalOperationDTO));
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.Error);
            }
        }

        #endregion


        #region IndividualRole
        /// <summary>
        /// Valida los roles del usuario
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTypeRolByIndividual(int individualId)
        {
            try
            {
                List<Company.Application.UniquePersonAplicationServices.DTOs.IndividualRoleDTO> individualRoleDTO = DelegateService.uniquePersonAplicationService.GetAplicationIndividualRoleByIndividualId(individualId);
                return new UifJsonResult(true, individualRoleDTO);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, "Error GetTypeRolByIndividual." + ex);
            }
        }
        #endregion

        public ActionResult GetCountrie()
        {
            try
            {

                List<CMSM.Country> countries = DelegateService.commonService.GetCountries();
                countries.RemoveAll(x => x.Description.Trim().Replace(" ", "")
                .Contains(("PAIS DE ALTO RIESGO GAFI").Replace(" ", "")));
                return new UifJsonResult(true, countries.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorSearchCountry);
            }

        }

        public ActionResult GetStatesByCountry(int idCountry)
        {
            try
            {

                List<CMSM.State> states = DelegateService.commonService.GetStatesByCountryId(idCountry);
                return new UifJsonResult(true, states.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorSearchCountry);
            }

        }

        public ActionResult GetCitiesByCountryIdStateId(int countryId, int stateId)
        {
            try
            {

                List<CMSM.City> cities = DelegateService.commonService.GetCitiesByCountryIdStateId(countryId, stateId);
                return new UifJsonResult(true, cities.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorSearchCountry);
            }

        }

        public UifJsonResult GetDefaultCountry()
        {
            try
            {
                if (parameters.Count == 0)
                {
                    GetParameters();
                }
                return new UifJsonResult(true, parameters.Where(x => x.Description == "EmisionDefaultCountry"));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, null);
            }

        }

        public List<Sistran.Core.Application.CommonService.Models.Parameter> GetParameters()
        {
            try
            {
                parameters.Add(new Sistran.Core.Application.CommonService.Models.Parameter { Description = "EmisionDefaultCountry", Value = DelegateService.commonService.GetParameterByParameterId((int)CMSE.ParametersTypes.EmisionDefaultCountry).NumberParameter.Value });
                return parameters;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #region Combos
        public UifJsonResult GetRoles()
        {
            try
            {
                List<RolDTO> Perfiles = DelegateService.SarlaftApplicationServices.GetRoles();
                return new UifJsonResult(true, Perfiles.OrderBy(x => x.Description).ToList());
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, null);
            }

        }


        public UifJsonResult GetCategoria()
        {
            try
            {
                List<EntityDTO> Category = DelegateService.SarlaftApplicationServices.GetCategory();
                return new UifJsonResult(true, Category.OrderBy(x => x.Description).ToList());
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, null);
            }

        }


        public UifJsonResult GetConsanguinidad()
        {
            try
            {
                List<EntityDTO> affinity = DelegateService.SarlaftApplicationServices.GetAffinity();
                return new UifJsonResult(true, affinity.OrderBy(x => x.Description).ToList());
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, null);
            }

        }


        public UifJsonResult GetRelacion()
        {
            try
            {
                List<EntityDTO> relation = DelegateService.SarlaftApplicationServices.GetRelation();
                return new UifJsonResult(true, relation.OrderBy(x => x.Description).ToList());
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, null);
            }

        }

        #endregion

        public ActionResult SaveExoneration(SarlaftExonerationtDTO sarlaftExonerationtDTO)
        {
            try
            {
                Sistran.Company.Application.UniquePersonServices.V1.Models.CompanySarlaftExoneration sarlaftExonerationt = new Company.Application.UniquePersonServices.V1.Models.CompanySarlaftExoneration();

                sarlaftExonerationt.ExonerationType = new Company.Application.UniquePersonServices.V1.Models.CompanyExonerationType();
                sarlaftExonerationt.ExonerationType.Id = sarlaftExonerationtDTO.ExonerationType;
                sarlaftExonerationt.IsExonerated = sarlaftExonerationtDTO.IsExonerated;
                sarlaftExonerationt.UserId = SessionHelper.GetUserId();
                sarlaftExonerationt.EnteredDate = DateTime.Now;



                sarlaftExonerationt = DelegateService.uniquePersonServiceV1.UpdateSarlaftExoneration(sarlaftExonerationt, sarlaftExonerationtDTO.IndividualId);
               

                return new UifJsonResult(true, sarlaftExonerationtDTO);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveData);
            }
        }


        public UifJsonResult GetOppositor()
        {
            try
            {
                List<EntityDTO> Oppositor = DelegateService.SarlaftApplicationServices.GetOppositor();
                return new UifJsonResult(true, Oppositor.OrderBy(x => x.Description).ToList());
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, null);
            }

        }

        public UifJsonResult GetSociety()
        {
            try
            {
                List<EntityDTO> Society = DelegateService.SarlaftApplicationServices.GetSociety();
                return new UifJsonResult(true, Society.OrderBy(x => x.Description).ToList());
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, null);
            }

        }

        public UifJsonResult GetNationality()
        {
            try
            {
                List<EntityDTO> Nationality = DelegateService.SarlaftApplicationServices.GetNationality();
                return new UifJsonResult(true, Nationality.OrderBy(x => x.Description).ToList());
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, null);
            }

        }



        public ActionResult GetInterviewManagerByDescription(string InterviewManager)
        {
            try
            {
                List<String> interviewManager = DelegateService.SarlaftApplicationServices.GetInterviewManagerByDescriptionSarlaft(InterviewManager);
                if(interviewManager != null && interviewManager.Count>0)
                 return new UifJsonResult(true, interviewManager);
                else
                    return new UifJsonResult(false, App_GlobalResources.Language.PersonNotFound);


            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.PersonNotFound);
            }

        }

    }
}

