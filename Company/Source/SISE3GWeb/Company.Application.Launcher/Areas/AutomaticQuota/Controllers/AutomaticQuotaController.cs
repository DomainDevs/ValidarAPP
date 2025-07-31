using Sistran.Company.Application.OperationQuotaServices.DTOs;
using Sistran.Company.Application.UniquePersonAplicationServices.DTOs;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using UPV1 = Sistran.Core.Application.UniquePersonService.V1.Models;
using PARM = Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.CommonService.Enums;
using ACM = Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UniqueUserServices.Models;

namespace Sistran.Core.Framework.UIF.Web.Areas.AutomaticQuota.Controllers
{
    public class AutomaticQuotaController : Controller
    {
        private Helpers.PostEntity entityCountry = new Helpers.PostEntity() { EntityType = "Sistran.Core.Application.Common.Entities.Country" };

        // GET: AutomaticQuota/AutomaticQuota
        public ActionResult Index()
        {
            return View();
        }
        #region AdvancedSearchAutomaticQuota
        public PartialViewResult AdvancedSearchAutomaticQuota()
        {
            return this.PartialView();
        }
        #endregion
        public ActionResult GetDocumentTypes()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.companyUniquePersonParamService.GetDocumentTypes());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetDocumentTypes);
            }
        }

        public ActionResult GetAgentProgram()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.operationQuotaCompanyService.GetAgentProgramDTOs());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetDocumentTypes);
            }
        }

        public ActionResult GetUtility()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.operationQuotaCompanyService.GetUtilityDTOs());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetDocumentTypes);
            }
        }
        public UifJsonResult GetIndicatorConcepts()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.operationQuotaCompanyService.GetIndicatorConceptsDTOs());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetDocumentTypes);
            }
        }

        public UifJsonResult GetReportListSisconc()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.operationQuotaCompanyService.GetReportListSisconcDTOs());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetDocumentTypes);
            }
        }
        public UifJsonResult GetRiskCenterList()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.operationQuotaCompanyService.GetRiskCenterListDTOs());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetDocumentTypes);
            }
        }
        public UifJsonResult GetRestrictiveList()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.operationQuotaCompanyService.GetRestrictiveListDTOs());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetDocumentTypes);
            }
        }

        public UifJsonResult GetPromissoryNoteSignature()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.operationQuotaCompanyService.GetPromissoryNoteSignatureDTOs());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetDocumentTypes);
            }
        }

        public ActionResult GetGuaranteeStatus()
        {
            try
            {
                List<GuaranteeStatus> guaranteeStatus = new List<GuaranteeStatus>();
                guaranteeStatus = DelegateService.uniquePersonServiceV1.GetGuaranteeStatus();
                var list = guaranteeStatus.Select(item => new { item.Id, item.Description }).ToList();
                return new UifJsonResult(true, list.OrderBy(x => x.Description).ToList());
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetDocumentTypes);
            }
        }

        public ActionResult GetGuaranteesTypes()
        {
            try
            {
                List<UPV1.GuaranteeType> guaranteesTypes = DelegateService.uniquePersonServiceV1.GetGuaranteesTypes();
                return new UifJsonResult(true, guaranteesTypes.OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetGuaranteeType);
            }
        }

        public UifJsonResult GetUserAgenciesByAgentIdDescription(int agentId, string description)
        {
            try
            {
                List<Sistran.Core.Application.UniqueUserServices.Models.UserAgency> agencies = DelegateService.uniqueUserService.GetCompanyUserAgenciesByAgentIdDescription(agentId, description, SessionHelper.GetUserId());
                return new UifJsonResult(true, agencies);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        public ActionResult GetEconomicActivitiesByDescription(string description)
        {
            try
            {
                List<EconomicActivityDTO> economicActivities = DelegateService.uniquePersonAplicationService.GetAplicationEconomicActivities();

                int economicActivityId = 0;
                Int32.TryParse(description, out economicActivityId);

                if (economicActivityId == 0)
                {
                    List<EconomicActivityDTO> dataDescription = economicActivities.Where(x => x.Description.ToLower().Contains(description.ToLower())).ToList();
                    if (dataDescription.Count() > 0)
                    {
                        return new UifJsonResult(true, dataDescription);
                    }
                    else
                    {
                        return new UifJsonResult(false, App_GlobalResources.LanguagePerson.EconomyActivityDoesNotExist);
                    }
                }
                else
                {
                    List<EconomicActivityDTO> dataId = economicActivities.Where(x => x.Id.ToString().Contains(economicActivityId.ToString())).ToList();
                    if (!dataId.Any())
                    {
                        return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorEconomyActivity);
                    }
                    return new UifJsonResult(true, dataId);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetEconomicActivities);
            }
        }

        public ActionResult GetCountries()
        {
            try
            {
                List<Country> countries = DelegateService.commonService.GetCountries();
                return new UifJsonResult(true, countries.OrderBy(x => x.Description));
            }
            catch (Exception)
            {

                throw;
            }

        }
        public ActionResult GetStatesByCountryId(int countryId)
        {
            try
            {
                try
                {
                    return new UifJsonResult(true, DelegateService.commonService.GetStatesByCountryId(countryId));
                }
                catch (Exception)
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetStatesByCountryId);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public UifJsonResult GetCitiesByCountryIdStateId(int countryId, int stateId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.commonService.GetCitiesByCountryIdStateId(countryId, stateId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetCitiesByCountryIdStateId);
            }
        }
        public ActionResult GetParameters()
        {
            try
            {
                List<PARM.Parameter> parameters = new List<PARM.Parameter>();
                List<ACM.Country> countries = DelegateService.commonService.GetCountries();
                if (parameters.Count == 0)
                {
                    GetCountries();

                    //parameters.Add(new Sistran.Core.Application.CommonService.Models.Parameter { Description = "Currency", Value = DelegateService.commonService.GetParameterByParameterId((int)ParametersTypes.Currency).NumberParameter.Value });

                    parameters.Add(new Sistran.Core.Application.CommonService.Models.Parameter { Description = "Country", Value = DelegateService.commonService.GetParameterByParameterId((int)ParametersTypes.Country).NumberParameter.Value });
                    parameters.Add(new Sistran.Core.Application.CommonService.Models.Parameter { Description = "DateIni", Value = DelegateService.commonService.GetParameterByParameterId(12229) });
                    parameters.Add(new Sistran.Core.Application.CommonService.Models.Parameter { Description = "DateFin", Value = DelegateService.commonService.GetParameterByParameterId(12230) });

                    ACM.Parameter pcountry = parameters.Where(x => x.Description == ParametersTypes.Country.ToString()).First();
                    pcountry.TextParameter = countries.First(x => x.Id.ToString() == pcountry.Value.ToString()).Description;
                }

                return new UifJsonResult(true, parameters);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorSearchParameter);
            }
        }

        public ActionResult GetAutomaticQuotaOperation(int Id)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.operationQuotaCompanyService.GetAutomaticQuotaOperation(Id));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetDocumentTypes);
            }
        }

        /// <summary>
        /// Guarda los datos de cupo general
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveAutomaticQuotaGeneral(AutomaticQuotaDTO automaticQuotaDto, List<DynamicConcept> dynamicProperties)
        {
            try
            {
                //validar el modelo para campos obligatorios
                AutomaticQuotaDTO automaticDTO = new AutomaticQuotaDTO();
                automaticDTO.DynamicProperties = dynamicProperties;
                automaticDTO = DelegateService.operationQuotaCompanyService.SaveAutomaticQuotaGeneral(automaticQuotaDto);
                return new UifJsonResult(true, automaticDTO);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        [HttpPost]
        public ActionResult SaveAutomaticQuotaGeneralJSON(AutomaticQuotaDTO AutomaticDTO, List<DynamicConcept> dynamicProperties)
        {
            try
            {
                AutomaticDTO.DynamicProperties = dynamicProperties;
                if (AutomaticDTO.ElaboratedName != null && AutomaticDTO.RequestedByName != null)
                {
                    List<User> userElaborated = DelegateService.uniqueUserService.GetUserByName(AutomaticDTO.ElaboratedName);
                    List<User> userRequested = DelegateService.uniqueUserService.GetUserByName(AutomaticDTO.RequestedByName);
                    if (userElaborated.Any() && userRequested.Any())
                    {
                        AutomaticDTO.ElaboratedId = userElaborated.FirstOrDefault().UserId;
                        AutomaticDTO.RequestedById = userRequested.FirstOrDefault().UserId;
                    }
                    if (dynamicProperties != null)
                    {
                        AutomaticDTO.DynamicProperties = dynamicProperties;
                    }
                }
                var exist = DelegateService.operationQuotaCompanyService.GetAutomaticQuotaOperation(AutomaticDTO.AutomaticQuotaId);
                if (exist.Any())
                {
                    AutomaticDTO = DelegateService.operationQuotaCompanyService.UpdateAutomaticQuotaGeneralJSON(AutomaticDTO);
                }
                else
                {
                    AutomaticDTO = DelegateService.operationQuotaCompanyService.SaveAutomaticQuotaGeneralJSON(AutomaticDTO);
                }
                return new UifJsonResult(true, AutomaticDTO);

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Guarda los datos de cupo general
        /// </summary>
        /// <returns></returns>
        public ActionResult SaveAutomaticQuotaThirdJSON(ThirdDTO thirdDTO)
        {
            try
            {
                List<AutomaticQuotaOperationDTO> exist = DelegateService.operationQuotaCompanyService.GetAutomaticQuotaOperationByParentId(thirdDTO.Id);
                if (exist.Any())
                {
                    AutomaticQuotaOperationDTO validateExistThird = exist.Where(x => x.AutomaticOperationType == 2).FirstOrDefault();
                    if (validateExistThird != null)
                    {
                        thirdDTO = DelegateService.operationQuotaCompanyService.UpdateAutomaticQuotaThirdJSON(thirdDTO, validateExistThird.Id);
                    }
                    else
                    {
                        thirdDTO = DelegateService.operationQuotaCompanyService.SaveAutomaticQuotaThirdJSON(thirdDTO);
                    }

                }
                else
                {
                    thirdDTO = DelegateService.operationQuotaCompanyService.SaveAutomaticQuotaThirdJSON(thirdDTO);
                }

                return new UifJsonResult(true, thirdDTO);

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Guarda los datos de cupo general
        /// </summary>
        /// <returns></returns>
        public ActionResult SaveAutomaticQuotaUtilityJSON(List<UtilityDTO> listUtlityDTO, AutomaticQuotaDTO automaticDTO)
        {
            try
            {
                var exist = DelegateService.operationQuotaCompanyService.GetAutomaticQuotaOperationByParentId(automaticDTO.AutomaticQuotaId);
                if (exist.Any())
                {
                    AutomaticQuotaOperationDTO validateJsonUtility = exist.Where(x => x.AutomaticOperationType == 3).FirstOrDefault();
                    if (validateJsonUtility != null)
                    {
                        automaticDTO = DelegateService.operationQuotaCompanyService.UpdateAutomaticQuotaUtilityJSON(listUtlityDTO, automaticDTO, validateJsonUtility.Id);
                    }
                    else
                    {
                        automaticDTO = DelegateService.operationQuotaCompanyService.SaveAutomaticQuotaUtilityJSON(listUtlityDTO, automaticDTO);
                    }

                }
                else
                {
                    automaticDTO = DelegateService.operationQuotaCompanyService.SaveAutomaticQuotaUtilityJSON(listUtlityDTO, automaticDTO);
                }

                return new UifJsonResult(true, automaticDTO);

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// GetUserByName
        /// </summary>
        /// <param name="name"></param>
        /// <returns>List<User/></returns>
        public ActionResult GetUserByName(string name)
        {
            try
            {
                if (name == string.Empty)
                {
                    throw new BusinessException("User Name is Empty");
                }


                return new UifJsonResult(true, DelegateService.uniqueUserService.GetUserByName(name));
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult GetUserSession()
        {
            try
            {
                Sistran.Core.Application.UniqueUserServices.Models.User user = new Application.UniqueUserServices.Models.User();
                int userId = SessionHelper.GetUserId();
                string UserName = SessionHelper.GetUserName();
                user.AccountName = UserName;
                user.UserId = userId;
                return new UifJsonResult(true, user);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public UifJsonResult GetPersonByDescriptionInsuredSearchTypeCustomerType(string description, CustomerType? customerType)
        {
            try
            {
                List<Holder> holders = DelegateService.underwritingService.GetHoldersByDocument(description, customerType);
                AutomaticQuotaDTO automaticQuotaDTO = new AutomaticQuotaDTO();
                if (holders != null && holders.Any())
                {
                    //foreach (var item in holders)
                    //{
                    //    List<Address> addresses = DelegateService.uniquePersonServiceV1.GetAddressesByIndividualId(item.IndividualId);
                    //    List<Agency> agencies = DelegateService.uniquePersonServiceV1.GetAgencyByIndividualId(item.IndividualId);
                    //    automaticQuotaDTO.IndividualId = item.IndividualId;
                    //}
                    return new UifJsonResult(true, holders);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorHolderNotFoundInsured);
                }


            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchPolicyholder);
            }
        }
        public ActionResult GetCountriesByDescription(string Description)
        {
            try
            {
                List<Country> countries = DelegateService.commonService.GetCountries();
                var listCountries = countries.Where(x => x.Description.Contains(Description)).Select(x => new { Id = x.Id, Description = x.Description }).ToList();

                if (listCountries.Count != 0)
                {
                    return new UifJsonResult(true, listCountries);
                }

                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.MessageNotFoundCountries);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorSearchCountry);
            }
        }
        // States
        public ActionResult GetStatesByCountryIdByDescription(int CountryId, string Description)
        {
            try
            {
                List<Country> countries = DelegateService.commonService.GetCountries();
                List<State> states = DelegateService.commonService.GetStatesByCountryId(CountryId);
                var listStates = countries.Where(x => x.Id == CountryId).ToList()[0]
                    .States.Where(y => y.Description.Contains(Description)).Select(x => new { Id = x.Id, Description = x.Description }).ToList();

                if (listStates.Count != 0)
                {
                    return new UifJsonResult(true, listStates);
                }

                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.MessageNotFoundStates);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorConsultingDepartments);
            }
        }
        // Cities
        public ActionResult GetCitiesByCountryIdByStateIdByDescription(int CountryId, int StateId, string Description)
        {
            try
            {
                List<Country> countries = DelegateService.commonService.GetCountries();
                var listCities = countries.Where(x => x.Id == CountryId).ToList()[0]
                        .States.Where(y => y.Id == StateId).ToList()[0]
                        .Cities.Where(z => z.Description.Contains(Description)).Select(x => new { Id = x.Id, Description = x.Description }).ToList();

                if (listCities.Count != 0)
                {
                    return new UifJsonResult(true, listCities);
                }
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.MessageNotFoundCities);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorQueryingCities);
            }
        }

        public ActionResult ExecuteCalculate(int id, List<DynamicConcept> dynamicProperties)
        {
            try
            {
                AutomaticQuotaDTO automaticQuotaDTO = new AutomaticQuotaDTO();

                if (id > 0)
                {
                    automaticQuotaDTO = DelegateService.operationQuotaCompanyService.ExecuteCalculate(id, dynamicProperties);
                }
                return new UifJsonResult(true, automaticQuotaDTO);

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public ActionResult GetAutomaticQuotaDeserealizado(int Id)
        {
            try
            {
                AutomaticQuotaDTO automaticQuotaDTO = new AutomaticQuotaDTO();
                automaticQuotaDTO = DelegateService.operationQuotaCompanyService.GetAutomaticQuotaDeserealizado(Id);
                return new UifJsonResult(true, automaticQuotaDTO);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.TemporaryNotFound);
            }
        }
        public ActionResult GetAddressByIndividualIdCompany(int individualId)
        {
            try
            {
                List<Address> address = DelegateService.uniquePersonServiceV1.GetAddressesByIndividualId(individualId);
                return new UifJsonResult(true, address.OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, "");
            }

        }

        public ActionResult GetAgencyByIndividualId(int individualId)
        {

            try
            {
                List<AgencyDTO> agency = new List<AgencyDTO>();
                agency = DelegateService.uniquePersonAplicationService.GetActiveAplicationAgencyByInvidualID(individualId);
                return new UifJsonResult(true, agency);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex);
            }
        }
        public ActionResult GetCountryByCountryId(int countryId)
        {
            try
            {
                List<Country> countries = DelegateService.commonService.GetCountries();
                ACM.Country country = (from c in countries where c.Id == countryId select c).FirstOrDefault();
                return new UifJsonResult(true, country);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex);
            }
        }

        public ActionResult GetStatesByCountryIdByStateId(int countryId, int stateId)
        {
            try
            {
                List<Country> countries = DelegateService.commonService.GetCountries();
                ACM.State state = countries.Where(x => x.Id == countryId).ToList()[0]
                    .States.Where(y => y.Id == (stateId)).ToList().FirstOrDefault();
                return new UifJsonResult(true, state);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex);
            }
        }

        public ActionResult GetCitiesByCountryIdByStateIdById(int countryId, int stateId, int cityId)
        {
            try
            {
                List<Country> countries = DelegateService.commonService.GetCountries();
                ACM.City cities = countries.Where(x => x.Id == countryId).ToList()[0]
                    .States.Where(y => y.Id == stateId).ToList()[0]
                    .Cities.Where(z => z.Id == (cityId)).ToList().FirstOrDefault();

                return new UifJsonResult(true, cities);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex);
            }
        }
        public ActionResult GetAutomaticQuota(int Id)
        {
            try
            {
                List<AutomaticQuotaDTO> automaticQuotaDTOs = new List<AutomaticQuotaDTO>();
                automaticQuotaDTOs = DelegateService.operationQuotaCompanyService.GetAutomaticQuota(Id);
                DelegateService.uniquePersonServiceV1.GetCompanyByIndividualId((int)automaticQuotaDTOs[0].ProspecDTO.IndividualId);
                DelegateService.uniquePersonServiceV1.GetPersonByIndividualId((int)automaticQuotaDTOs[0].ProspecDTO.IndividualId);
                DelegateService.uniquePersonServiceV1.GetProspectByIndividualId(Convert.ToString(automaticQuotaDTOs[0].ProspecDTO.IndividualId));

                return new UifJsonResult(true, automaticQuotaDTOs);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.TemporaryNotFound);

            }
        }

        public ActionResult GetPersonTypes()
        {
            try
            {
                return new UifJsonResult(true, EnumsHelper.GetItems<IndividualType>());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetPersonTypes);
            }

        }
    }
}