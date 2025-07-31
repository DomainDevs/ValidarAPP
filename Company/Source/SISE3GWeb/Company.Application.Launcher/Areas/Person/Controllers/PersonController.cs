// -----------------------------------------------------------------------
// <copyright file="PersonController.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Desconocido</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Framework.UIF.Web.Areas.Person.Controllers
{
    using Sistran.Company.Application.ModelServices.Models;
    using Sistran.Company.Application.ModelServices.Models.Param;
    using Sistran.Company.Application.UniquePersonAplicationServices.DTOs;
    using Sistran.Core.Application.CommonService.Enums;
    using Sistran.Core.Application.CommonService.Models;
    using Sistran.Core.Application.ModelServices.Enums;
    using Sistran.Core.Application.OperationQuotaServices.DTOs.OperationQuota;
    using CONS = Sistran.Core.Application.OperationQuotaServices.DTOs.Consortium;
    using Sistran.Core.Application.UnderwritingServices.Models;
    using Sistran.Core.Framework.UIF.Web.Areas.Person.Enums;
    using Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Models;
    using Sistran.Core.Framework.UIF.Web.Helpers;
    using Sistran.Core.Framework.UIF.Web.Models;
    using Sistran.Core.Framework.UIF.Web.Services;
    using Sistran.Core.Framework.UIF2.Controls.UifSelect;
    using Sistran.Core.Framework.UIF2.Controls.UifTable;
    using Sistran.Core.Services.UtilitiesServices.Enums;
    using Sistran.Core.Services.UtilitiesServices.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Application.AuthorizationPoliciesServices.Models;
    using Sistran.Core.Application.UnderwritingServices.Enums;
    using ACM = Sistran.Core.Application.CommonService.Models;
    using ASEPER = Sistran.Core.Framework.UIF.Web.Areas.Person.Models;
    using CPEMCV1 = Sistran.Company.Application.UniquePersonServices.V1.Models;
    using CPEMV1 = Sistran.Core.Application.UniquePersonService.V1.Models;
    using TaxModel = Sistran.Core.Application.TaxServices.Models;
    using Sistran.Company.Application.UniquePersonServices.V1.Models;
    using COAP = Company.Application.UnderwritingParamApplicationService.DTOs;
    using Sistran.Core.Application.UniquePersonListRiskApplicationServices.DTO;
    using System.Threading.Tasks;
    using Sistran.Core.Application.UniquePersonService.V1.Models;


    /// <summary>
    /// Acciones de person
    /// </summary>
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class PersonController : Controller
    {
        object look = new object();
        private static List<Sistran.Core.Application.CommonService.Models.Parameter> parameterPersonType = new List<Sistran.Core.Application.CommonService.Models.Parameter>();

        private static List<ACM.Country> countries = DelegateService.commonService.GetCountries();
        private static List<ACM.State> states = new List<ACM.State>();
        private static List<CPEMV1.Agent> agents = new List<CPEMV1.Agent>();
        private static List<ACM.EconomicActivity> economicActivities = new List<ACM.EconomicActivity>();
        private static List<ACM.Currency> currencies = new List<ACM.Currency>();

        private static List<CPEMV1.PaymentAccountType> paymentAccountTypes = new List<CPEMV1.PaymentAccountType>();
        private static List<ACM.PaymentMethod> paymentMethods = new List<ACM.PaymentMethod>();
        private static List<Sistran.Core.Application.CommonService.Models.Parameter> parameters = new List<Sistran.Core.Application.CommonService.Models.Parameter>();
        private static List<CPEMV1.OriginType> originType = new List<CPEMV1.OriginType>();
        private static List<ACM.PaymentConcept> paymentConcept = new List<ACM.PaymentConcept>();
        private static List<CPEMV1.Speciality> specialties = new List<CPEMV1.Speciality>();
        private static List<TaxModel.Tax> tax = new List<TaxModel.Tax>();
        private static Dictionary<int, List<ACM.DefaultValue>> defaultValues = new Dictionary<int, List<ACM.DefaultValue>>();
        private static List<CPEMV1.CommissionAgent> ComissionAgent = new List<CPEMV1.CommissionAgent>();
        private static string defaultCountryId = DelegateService.commonService.GetParameterByParameterId(2135).NumberParameter.ToString();
        private static GenericModelsServicesQueryModel addressCompany = new GenericModelsServicesQueryModel();

        #region Controller Vistas
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Email()
        {
            return View("Email");
        }

        public ActionResult Partner(int? partNerId)
        {

            return View("Partner");
        }

        public ActionResult Insured()
        {
            return View("Insured");
        }

        public ActionResult Prefix()
        {
            return View("Prefix");
        }

        public ActionResult PaymentConcepts()
        {
            return View("PaymentConcepts");
        }

        public ActionResult Sarlaft()
        {
            return View("Sarlaft");
        }

        public ActionResult ConsortiumMembers()
        {
            return View();
        }

        public ActionResult MeansPayment()
        {
            return View("MeansPayment");
        }

        public ActionResult Representlegal()
        {
            return View("Representlegal");
        }

        public ActionResult StaffLabour()
        {
            return View("StaffLabour");
        }

        public ActionResult BusinessName()
        {
            return View("BusinessName");
        }

        public ActionResult Agency()
        {
            return View();
        }

        public ActionResult Agent(int IndividualId)
        {
            return View();
        }

        public ActionResult ComissionAgency()
        {
            return View("ComissionAgency");
        }

        public ActionResult Persons(CPEMCV1.CompanyPerson person = null)
        {
            ViewBag.IndividualId = IndividualNew.New;
            if (person != null)
            {
                ViewBag.IndividualId = person.IndividualId;
            }
            return View();
        }

        public ActionResult PersonsOnline(PolicyModelsView policyModel)
        {
            ViewBag.IndividualId = IndividualNew.New;
            return View("Persons", policyModel);
        }

        public PartialViewResult AdvancedSearch()
        {
            return PartialView();
        }

        public ActionResult GetOriginTypes()
        {
            try
            {
                if (originType.Count == 0)
                {
                    originType = DelegateService.uniquePersonServiceV1.GetOriginTypes();
                }
                return new UifJsonResult(true, originType);

            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetOriginTypes);
            }
        }

        public ActionResult GetEnumsRoles()
        {
            try
            {
                EnumRoles enumRoles = DelegateService.uniquePersonService.GetEnumsRoles();
                return new UifJsonResult(true, enumRoles);

            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetEnumsRoles);
            }
        }

        public PartialViewResult BasicInformation()
        {
            return PartialView();
        }

        public PartialViewResult BasicInformationAdvancedSearch()
        {
            return PartialView();
        }
        #endregion

        #region Funciones Core

        #region Funciones Obtener
        public ActionResult GetPhoneTypes()
        {
            try
            {
                List<PhoneTypeDTO> phoneTypesDTO = DelegateService.uniquePersonAplicationService.GetAplicationPhoneTypes();

                return new UifJsonResult(true, phoneTypesDTO.OrderBy(x => x.Description));
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetPhoneTypes);
            }

        }

        public ActionResult GetAddressesTypes()
        {
            try
            {
                List<AddressTypeDTO> addressTypesDTO = DelegateService.uniquePersonAplicationService.GetAplicationAddressesTypes();

                return new UifJsonResult(true, addressTypesDTO.OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorAddressesTypes);
            }
        }

        public ActionResult GetAddressesTypesByIsEmail(Boolean? isEmail)
        {
            try
            {

                if (addressCompany.GenericModelServicesQueryModel == null || addressCompany.GenericModelServicesQueryModel.Count == 0)
                {
                    addressCompany = DelegateService.companyUniquePersonParamService.GetAddress(isEmail);

                }
                return new UifJsonResult(true, addressCompany.GenericModelServicesQueryModel.OrderBy(x => x.description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorAddressesTypes);
            }
        }

        public ActionResult GetCountries()
        {
            try
            {
                var list = countries.Select(item => new { item.Id, item.Description }).ToList();
                return new UifJsonResult(true, list.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorSearchCountry);
            }

        }

        public ActionResult GetStatesByCountryId(int countryId = 0)
        {
            try
            {
                ACM.Country country = (from c in countries where c.Id == countryId select c).FirstOrDefault();

                if (country != null)
                {
                    var list = country.States.Select(item => new { item.Id, item.Description }).ToList();
                    return new UifJsonResult(true, list.OrderBy(x => x.Description));
                }
                else
                {
                    return new UifJsonResult(false, new List<ACM.State>());
                }
            }
            catch (Exception)
            {

                return new UifJsonResult(false, "");
            }

        }

        public ActionResult GetCountryByCountryId(string country_Id)
        {
            int countryId = 0;
            try
            {
                if (country_Id != "")
                {
                    countryId = Convert.ToInt32(country_Id);
                }

                ACM.Country country = (from c in countries where c.Id == countryId select c).FirstOrDefault();

                return new UifJsonResult(true, country);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, "");
            }
        }

        public ActionResult GetCitiesByCountryIdStateId(int countryId, int stateId)
        {
            try
            {
                List<ACM.City> cities = (from c in countries
                                         from s in c.States
                                         where c.Id == countryId && s.Id == stateId
                                         select s.Cities).FirstOrDefault();

                if (cities != null)
                {
                    return new UifJsonResult(true, cities.OrderBy(x => x.Description));
                }
                else
                {
                    return new UifJsonResult(false, new List<ACM.City>());
                }
            }
            catch
            {

                return new UifJsonResult(false, "");
            }

        }

        public ActionResult GetPhonesByIndividualId(int individualId)
        {
            try
            {
                List<CPEMV1.Phone> phones = DelegateService.uniquePersonServiceV1.GetPhonesByIndividualId(individualId);
                return new UifTableResult(phones);
            }
            catch (Exception)
            {

                return new UifTableResult("");
            }

        }

        public ActionResult GetEmailTypes()
        {
            try
            {
                List<EmailTypeDTO> emailTypesDTO = DelegateService.uniquePersonAplicationService.GetAplicationEmailTypes();

                return new UifJsonResult(true, emailTypesDTO.OrderBy(x => x.Description));
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetEmailTypes);
            }

        }

        public ActionResult GetEmailsByIndividualId(int individualId)
        {
            try
            {
                List<CPEMV1.Email> emails = DelegateService.uniquePersonServiceV1.GetEmailsByIndividualId(individualId);
                return new UifTableResult(emails);

            }
            catch (Exception)
            {

                return new UifTableResult("");
            }

        }

        public ActionResult GetEducativeLevels()
        {

            try
            {
                List<CPEMV1.EducativeLevel> educationLevel = DelegateService.uniquePersonServiceV1.GetEducativeLevels();
                return new UifJsonResult(true, educationLevel);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorEducativeLevel);
            }
        }

        public ActionResult GetSocialLayers()
        {
            try
            {
                List<CPEMV1.SocialLayer> socialLayers = DelegateService.uniquePersonServiceV1.GetSocialLayers();
                return new UifJsonResult(true, socialLayers);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetSocialLayers);
            }
        }

        public ActionResult GetOccupations()
        {
            try
            {
                List<CPEMV1.Occupation> occupations = DelegateService.uniquePersonServiceV1.GetOccupations();
                return new UifJsonResult(true, occupations.OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetOccupations);
            }

        }

        public ActionResult GetSpecialties()
        {
            try
            {
                List<CPEMV1.Speciality> specialties = DelegateService.uniquePersonServiceV1.GetSpecialties();
                return new UifJsonResult(true, specialties);

            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetSpecialties);
            }

        }

        public ActionResult GetIncomeLevels()
        {
            try
            {
                List<CPEMV1.IncomeLevel> incomeLevels = DelegateService.uniquePersonServiceV1.GetIncomeLevels();
                return new UifJsonResult(true, incomeLevels);
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetIncomeLevels);
            }

        }
        public ActionResult GetCurrencies()
        {
            try
            {
                if (currencies.Count == 0)
                {
                    currencies = DelegateService.commonService.GetCurrencies();
                }
                return new UifJsonResult(true, currencies.OrderBy(x => x.Description));
            }
            catch
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorCurrency);
            }
        }
        public ActionResult GetDocumentType(int typeDocument)
        {
            try
            {
                List<DocumentTypeDTO> documentTypes = DelegateService.uniquePersonAplicationService.GetAplicationDocumentTypes(typeDocument);
                return new UifJsonResult(true, documentTypes.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetDocumentTypes);
            }

        }

        public ActionResult GetPartner(int IndividualId)
        {
            try
            {
                return new UifJsonResult(true, GetAplicationPartnerByIndividualId(IndividualId));
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetPartner);
            }

        }

        public ActionResult GetAplicationPartnerByIndividualId(int IndividualId)
        {

            try
            {
                List<PartnerDTO> partner = new List<PartnerDTO>();
                partner = DelegateService.uniquePersonAplicationService.GetAplicationPartnerByIndividualId(IndividualId);
                return new UifJsonResult(true, partner);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetPartner);
            }
        }


        public ActionResult GetAplicationPartnerByDocumentIdDocumentTypeIndividualId(String documentId, int documentType, int IndividualId)
        {

            try
            {
                PartnerDTO partner = new PartnerDTO();
                partner = DelegateService.uniquePersonAplicationService.GetAplicationPartnerByDocumentIdDocumentTypeIndividualId(documentId, documentType, IndividualId);
                return new UifJsonResult(true, partner);

            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetPartner);
            }
        }


        public ActionResult GetLegalRepresentByIndividualId(int individualId)
        {
            try
            {
                LegalRepresentativeDTO legalRepresent = DelegateService.uniquePersonAplicationService.GetLegalRepresentativeByIndividualId(individualId);
                return new UifJsonResult(true, legalRepresent);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, null);
            }
        }

        public ActionResult GetAddressByIndividualIdCompany(int individualId)
        {
            try
            {
                List<CPEMV1.Address> address = DelegateService.uniquePersonServiceV1.GetAddressesByIndividualId(individualId);
                return new UifSelectResult(address.OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifSelectResult("");
            }

        }
        public ActionResult GetPhonesByIndividualIdCompany(int individualId)
        {
            try
            {
                List<CPEMV1.Phone> phones = DelegateService.uniquePersonServiceV1.GetPhonesByIndividualId(individualId);
                return new UifSelectResult(phones);
            }
            catch (Exception)
            {

                return new UifSelectResult("");
            }

        }
        public ActionResult GetEmailsByIndividualIdCompany(int individualId)
        {
            try
            {
                List<CPEMV1.Email> emails = DelegateService.uniquePersonServiceV1.GetEmailsByIndividualId(individualId);
                return new UifSelectResult(emails);
            }
            catch (Exception)
            {

                return new UifSelectResult("");
            }

        }
        public ActionResult GetHouseTypes()
        {

            try
            {

                var houseTypes = DelegateService.uniquePersonServiceV1.GetHouseTypes();
                return new UifJsonResult(true, houseTypes);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetHouseTypes);
            }

        }
        public ActionResult GetPersonJobByIndividualId(int individualId)
        {
            try
            {
                CPEMV1.LabourPerson personJobModel = DelegateService.uniquePersonServiceV1.GetPersonJobByIndividualId(individualId);
                return new UifJsonResult(true, personJobModel);
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetPersonJob);
            }

        }

        public ActionResult GetBanks()
        {
            try
            {
                List<ACM.Bank> banks = DelegateService.commonService.GetBanks();
                return new UifJsonResult(true, banks.OrderBy(x => x.Description));
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetBanks);
            }

        }

        public ActionResult GetBankById(int bankId)
        {
            try
            {
                ACM.Bank bank = DelegateService.commonService.GetBanksByBankId(bankId);
                return new UifJsonResult(true, bank);
            }
            catch
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetBanks);
            }
        }

        public ActionResult GetExonerationTypes()
        {
            try
            {
                List<CPEMV1.ExonerationType> exonerationtypes = DelegateService.uniquePersonServiceV1.GetExonerationTypes();
                return new UifJsonResult(true, exonerationtypes.OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetExonerationTypes);
            }
        }

        public ActionResult GetPaymentMethods()
        {
            try
            {
                if (paymentMethods.Count == 0)
                {
                    paymentMethods = DelegateService.commonService.GetPaymentMethods();
                }

                return new UifJsonResult(true, paymentMethods.OrderBy(x => x.Description));
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetPaymentMethods);
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

        public ActionResult GetPaymentTypes()
        {
            try
            {
                if (paymentAccountTypes.Count == 0)
                {
                    paymentAccountTypes = DelegateService.uniquePersonServiceV1.GetPaymentTypes();
                }
                return new UifJsonResult(true, paymentAccountTypes.OrderBy(x => x.Description));
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetPaymentTypes);
            }
        }

        //public ActionResult GetPaymentMethodAccountByIndividualId(int individualId)
        //{
        //    try
        //    {
        //        List<CPEMV1.PaymentMethodAccount> paymentMethodAccount = DelegateService.uniquePersonServiceV1.GetPaymentMethodAccountByIndividualId(individualId);
        //        return new UifJsonResult(true, paymentMethodAccount);
        //    }
        //    catch (Exception)
        //    {

        //        return new UifJsonResult(false, null);
        //    }
        //}

        public ActionResult GetIndividualpaymentMethodByIndividualId(int individualId)
        {
            try
            {
                List<IndividualPaymentMethodDTO> IndividualPaymentMethod = DelegateService.uniquePersonAplicationService.GetIndividualpaymentMethodByIndividualId(individualId);
                return new UifJsonResult(true, IndividualPaymentMethod);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }



        public ActionResult GetAddressesByIndividualId(int individualId)
        {
            try
            {
                if (countries.Count > 0)
                {
                    List<CPEMV1.Address> address = DelegateService.uniquePersonServiceV1.GetAddressesByIndividualId(individualId);

                    if (address != null)
                    {
                        List<ACM.Country> temporalcountries = (from c in countries select c).ToList();
                        List<ACM.State> state = (from c in countries from s in c.States select s).ToList();
                        List<ACM.City> Cities = (from c in countries from s in c.States from cy in s.Cities select cy).ToList();

                        foreach (CPEMV1.Address item in address)
                        {
                            item.City.DANECode = "";
                            item.AddressType.Description += " <br /><b> " + temporalcountries.First(c => c.Id == item.City.State.Country.Id).Description;
                            if (item.City.State.Id > 0)
                            {
                                item.AddressType.Description += " - " + state.FirstOrDefault(s => s.Id == item.City.State.Id).Description;
                                item.AddressType.Description += " - " + Cities.FirstOrDefault(cy => cy.Id == item.City.Id && item.City.State.Id == cy.State.Id).Description + "</b>";
                                item.City.DANECode += Cities.FirstOrDefault(cy => cy.Id == item.City.Id).DANECode;
                            }
                            if (item.IsPrincipal)
                            {
                                item.AddressType.Description += " <br /><b>" + "Principal </b>";
                            }
                        }

                        return new UifTableResult(address);
                    }
                    else
                    {
                        return new UifTableResult(new List<CPEMV1.Address>());
                    }
                }
                else
                {
                    return new UifTableResult(new List<CPEMV1.Address>());
                }
            }
            catch (Exception)
            {
                return new UifTableResult("");
            }
        }
        public ActionResult GetBranchs()
        {
            try
            {
                List<ACM.Branch> branch = DelegateService.commonService.GetBranches();
                return new UifJsonResult(true, branch.OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorQueryBranches);
            }
        }

        public ActionResult GetAgenciesByAgentId(int agentId)
        {
            try
            {
                List<AgencyDTO> agencies = DelegateService.uniquePersonAplicationService.GetAplicationAgenciesByAgentId(agentId);

                return new UifJsonResult(true, agencies.OrderBy(x => x.FullName));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetAgentAgency);
            }

        }

        public ActionResult GetAgentByAgentCodeFullName(string description)
        {
            int codeAgent = 0;
            try
            {
                Int32.TryParse(description, out codeAgent);
                List<CPEMV1.Agent> agent = new List<CPEMV1.Agent>();
                if (codeAgent == 0)
                {
                    agent = DelegateService.uniquePersonServiceV1.GetAgentByAgentCodeFullName(0, description);
                }
                else
                {
                    agent = DelegateService.uniquePersonServiceV1.GetAgentByAgentCodeFullName(codeAgent, "");
                }

                if (agent.Count == 1)
                {
                    if (agent[0].DateDeclined != null)
                    {
                        agent = null;


                    }
                }
                return new UifJsonResult(true, agent);
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetAgent);
            }

        }

        public UifJsonResult GetAgenciesByAgentIdDescription(int agentId, string description)
        {
            try
            {
                List<AgencyDTO> agencies = DelegateService.uniquePersonAplicationService.GetAplicationAgenciesByAgentIdDescription(agentId, description);

                if (agencies.Count == 1)
                {
                    if (agencies[0].DateDeclined != null)
                    {
                        agencies = null;
                    }
                }

                return new UifJsonResult(true, agencies);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetAgentAgency);
            }
        }

        public ActionResult GetAgentByName(string nameAgent)
        {
            try
            {
                agents = DelegateService.uniquePersonServiceV1.GetAgentByName(nameAgent);
                return new UifJsonResult(true, agents);
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetAgent);
            }
        }

        public ActionResult GetAgencyByIndividualId(int individualId)
        {

            try
            {
                List<AgencyDTO> agency = new List<AgencyDTO>();
                agency = DelegateService.uniquePersonAplicationService.GetActiveAplicationAgencyByInvidualID(individualId);
                return new UifSelectResult(agency);
            }
            catch
            {
                return new UifSelectResult("");
            }
        }

        public ActionResult GetCompanyTypes()
        {
            try
            {
                List<CompanyTypeDTO> companyTypeDTO = DelegateService.uniquePersonAplicationService.GetAplicationCompanyTypes();

                return new UifJsonResult(true, companyTypeDTO);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetCompanyTypes);
            }
        }

        public ActionResult GetAgents(string query)
        {
            try
            {
                agents = DelegateService.uniquePersonServiceV1.GetAgentByQuery(query);
                int insuranceId = 0;
                Int32.TryParse(query, out insuranceId);
                if (insuranceId == 0)
                {
                    var dataQuery = agents.Where(x => x.FullName.Contains(query));
                    return new UifJsonResult(true, dataQuery);
                }
                else
                {
                    var dataId = agents.Where(x => x.IndividualId == insuranceId);
                    return new UifJsonResult(true, dataId);
                }

            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetAgent);
            }
        }

        public JsonResult GetEconomicActivitiesTax()
        {
            try
            {
                List<TaxModel.EconomicActivityTax> economicActivitiesTax = DelegateService.taxService.GetEconomicActivitiesTax();
                return new UifJsonResult(true, economicActivitiesTax);
            }
            catch
            {
                return new UifSelectResult(false, App_GlobalResources.Language.ErrorEconomyActivity);
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

        public ActionResult GetInsuredDeclinedTypes()
        {
            try
            {
                List<InsuredDeclinedTypeDTO> insuredDeclinedTypes = DelegateService.uniquePersonAplicationService.GetAplicationInsuredDeclinedTypes();
                return new UifJsonResult(true, insuredDeclinedTypes.OrderBy(x => x.Description));
            }
            catch
            {

                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetInsuredDeclinedTypes);
            }

        }

        public ActionResult GetDefaultBranchesByUserId()
        {
            ACM.Branch BranchesByUser;
            try
            {
                BranchesByUser = DelegateService.uniqueUserService.GetDefaultBranchesByUserId(SessionHelper.GetUserId());
                return new UifJsonResult(true, BranchesByUser);

            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetDefaultBranches);
            }

        }

        public ActionResult GetBankBranches(int bankId)
        {
            try
            {
                List<ACM.BankBranch> bankBranches = DelegateService.commonService.GetBankBranches(bankId);
                return new UifJsonResult(true, bankBranches?.OrderBy(x => x.Description));
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetBankBranches);
            }
        }

        public ActionResult GetPersonTypesInformationPersonal()
        {
            try
            {

                List<CPEMV1.PersonType> personTypes = DelegateService.uniquePersonServiceV1.GetPersonTypes();

                return new UifJsonResult(true, personTypes.OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetPersonTypes);
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
        public ActionResult GetGenderTypes()
        {
            try
            {
                return new UifJsonResult(true, EnumsHelper.GetItems<Application.UniquePersonService.V1.Enums.GenderType>());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetGenderTypes);
            }
        }
        //public ActionResult GetOperatingQuotaByIndividualId(int individualId)
        public ActionResult GetOperatingQuotaByIndividualId(int individualId)
        {
            try
            {
                List<OperatingQuotaDTO> operatingQuotaDTOs = new List<OperatingQuotaDTO>();
                operatingQuotaDTOs = DelegateService.uniquePersonAplicationService.GetOperatingQuotaByIndividualId(individualId);
                return new UifJsonResult(true, operatingQuotaDTOs);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetOperatingQuota);
            }
        }

        public ActionResult GetLinesBusiness()
        {
            try
            {
                ACM.Parameter parameter = DelegateService.commonService.GetParameterByParameterId((int)parameterType.HardRiskType);
                return new UifJsonResult(true, DelegateService.commonService.GetLinesBusinessByCoveredRiskType((CoveredRiskType)parameter.NumberParameter).OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorSearchHardRiskType);
            }
        }

        public ActionResult GetCoveragesByLinesBusinessId(int prefixId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.underwritingService.GetCoveragesByLineBusinessId(prefixId));
            }
            catch
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetCoveragesByLineBusinessIdSubLineBusinessId);
            }

        }

        public UifJsonResult GetPrefixes()
        {
            List<ACM.Prefix> prefix;
            try
            {
                prefix = DelegateService.commonService.GetPrefixes().OrderBy(x => x.Description).ToList();
                return new UifJsonResult(true, prefix);

            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetPrefixes);
            }

        }

        public UifJsonResult GetAgentPrefixByIndividualId(int IndividualId)
        {
            List<PrefixDTO> prefix;
            try
            {
                prefix = DelegateService.uniquePersonAplicationService.GetPrefixAgentByInvidualId(IndividualId);
                return new UifJsonResult(true, prefix);

            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetAgentPrefix);
            }

        }

        public UifJsonResult GetAgentCommissionIndividualId(int IndividualId)
        {
            List<ComissionAgentDTO> comissionAgentDTOs;
            try
            {

                comissionAgentDTOs = DelegateService.uniquePersonAplicationService.GetcommissionPorIndividualId(IndividualId);
                return new UifJsonResult(true, comissionAgentDTOs);

            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetAgentPrefix);
            }

        }

        public JsonResult GetCompanyByIndividualId(int individualId)
        {
            return null;
        }

        public JsonResult GetProspectLegalByIndividualId(int individualId)
        {
            try
            {
                ProspectLegalDTO prospectLegal = DelegateService.uniquePersonAplicationService.GetProspectPersonLegal(individualId);

                if (prospectLegal != null)
                {
                    return new UifJsonResult(true, prospectLegal);
                }
                else
                {
                    return new UifJsonResult(false, null);
                }
            }
            catch
            {

                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetProspectLegal);
            }

        }

        public JsonResult GetProspectNaturalByIndividualId(int individualId)
        {
            try
            {
                ProspectPersonNaturalDTO prospectoNatural = DelegateService.uniquePersonAplicationService.GetProspectPersonNatural(individualId);

                if (prospectoNatural != null)
                {
                    return new UifJsonResult(true, prospectoNatural);
                }
                else
                {
                    return new UifJsonResult(false, null);
                }
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetProspectNatural);
            }

        }

        public JsonResult GetProspectByDocumentNum(string documentNum, int searchType)
        {
            try
            {
                if (searchType == 3)
                {
                    ProspectPersonNaturalDTO prospects = DelegateService.uniquePersonAplicationService.GetProspectNaturalByDocumentNumber(documentNum);
                    if (prospects != null)
                    {
                        return new UifJsonResult(true, prospects);
                    }
                    else
                    {
                        return new UifJsonResult(false, App_GlobalResources.LanguagePerson.MessageNotFoundProspects);
                    }
                }
                else
                {
                    ProspectLegalDTO prospectLegal = DelegateService.uniquePersonAplicationService.GetProspectLegalByDocumentNumber(documentNum);
                    if (prospectLegal != null && prospectLegal.TributaryIdNumber != null)
                    {
                        return new UifJsonResult(true, prospectLegal);
                    }
                    else
                    {
                        return new UifJsonResult(false, App_GlobalResources.LanguagePerson.MessageNotFoundProspects);
                    }
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetProspect);
            }
        }

        public ActionResult GetAgentTypes()
        {
            try
            {
                List<AgentTypeDTO> agentType = DelegateService.uniquePersonAplicationService.GetAplicationAgentTypes();
                return new UifJsonResult(true, agentType.OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetAgentTypes);
            }
        }

        public ActionResult GetAgentDeclinedTypes()
        {
            try
            {
                List<AgentDeclinedTypeDTO> agentDeclinedTypes = DelegateService.uniquePersonAplicationService.GetAplicationAgentDeclinedTypes();
                return new UifJsonResult(true, agentDeclinedTypes.OrderBy(x => x.Description));
            }
            catch
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetInsuredDeclinedTypes);
            }
        }

        public ActionResult GetMaritalStatus()
        {
            try
            {
                List<MaritalStatusDTO> maritalStatus = new List<MaritalStatusDTO>();
                maritalStatus = DelegateService.uniquePersonAplicationService.GetAplicationMaritalStatus();

                return new UifJsonResult(true, maritalStatus.OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetMaritalStatus);
            }
        }

        public UifJsonResult GetAgentByIndividualId(int IndividualId)
        {
            try
            {
                AgentDTO agentByIndividulId = DelegateService.uniquePersonAplicationService.GetAplicationAgentByIndividualId(IndividualId);
                if (agentByIndividulId != null)
                {
                    return new UifJsonResult(true, agentByIndividulId);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorNotAgents);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetAgent);
            }
        }




        public UifJsonResult GetAplicationCompanyByDocument(string documentNumber)
        {
            try
            {
                List<CompanyDTO> personsModels = DelegateService.uniquePersonAplicationService.GetAplicationCompanyByDocument(documentNumber);


                if (personsModels == null || personsModels.Count == 0)
                {
                    return new UifJsonResult(true, personsModels);
                }


                List<AuthorizationRequest> autorequest = DelegateService.uniquePersonServiceV1.GetAuthorizationRequestByIndividualId(personsModels[0].Id);

                if (autorequest.Count > 0)
                {
                    return new UifJsonResult(true, new { IsAuthorizationRequest = true, AuthorizationRequests = autorequest });
                }

                return new UifJsonResult(true, personsModels);

            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetCompany);
            }

        }

        [HttpPost]
        public JsonResult GetAplicationPersonByDocument(string documentNumber)
        {
            try
            {
                List<PersonDTO> personsModels = DelegateService.uniquePersonAplicationService.GetAplicationPersonByDocument(documentNumber);

                if (personsModels == null || personsModels.Count == 0)
                {
                    return new UifJsonResult(true, personsModels);
                }

                List<AuthorizationRequest> autorequest = DelegateService.uniquePersonServiceV1.GetAuthorizationRequestByIndividualId(personsModels[0].Id);

                if (autorequest.Count > 0)
                {
                    return new UifJsonResult(true, new { IsAuthorizationRequest = true, AuthorizationRequests = autorequest });
                }
                return new UifJsonResult(true, personsModels);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetPerson);
            }
        }


        [HttpPost]
        public JsonResult GetPerson2gByDocumentNumber(string documentNumber, bool company)
        {
            try
            {
                List<CompanyPerson> companyPeople = new List<CompanyPerson>();
                companyPeople = DelegateService.uniquePersonService.GetPerson2gByDocumentNumber(documentNumber, company);
                return new UifJsonResult(true, companyPeople);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetPerson);
            }
        }


        [HttpPost]
        public JsonResult GetPerson2gByPersonId(int personId, bool company)
        {
            try
            {
                //List<CompanyPerson> companyPeople = new List<CompanyPerson>();
                var companyPeople = DelegateService.uniquePersonService.GetPerson2gByPersonId(personId, company);
                return new UifJsonResult(true, companyPeople);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetPerson);
            }
        }

        [HttpPost]
        public JsonResult GetCompany2gByPersonId(int personId, bool company)
        {
            try
            {
                //List<CompanyPerson> companyPeople = new List<CompanyPerson>();
                var companyPeople = DelegateService.uniquePersonService.GetCompany2gByPersonId(personId, company);
                return new UifJsonResult(true, companyPeople);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetPerson);
            }
        }

        [HttpPost]
        public JsonResult GetAllProspectByDocumentNumberSurnameMotherLastNameTradeName(string documentNumber, string surname, string motherLastName, string name, string tradeName, int searchType, int documentType, int individualId)
        {
            try
            {
                List<CPEMV1.ProspectNatural> personsModels = DelegateService.uniquePersonServiceV1.GetAllProspectByDocumentNumberSurnameMotherLastNameTradeName(documentNumber, surname, motherLastName, name, tradeName, searchType);
                if (personsModels.Count > 0)
                {
                    return new UifJsonResult(true, personsModels);
                }
                else
                {
                    return new UifJsonResult(false, null);
                }
            }
            catch (Exception)
            {

                return new UifJsonResult(false, null);
            }

        }
        public JsonResult GetIndividualTypes()
        {
            try
            {
                var individualType = DelegateService.uniquePersonServiceV1.GetIndividualTypes();
                return new UifSelectResult(individualType);
            }
            catch (Exception)
            {

                return new UifSelectResult("");
            }

        }
        public UifJsonResult GetDate()
        {
            try
            {
                DateTime dateNow = DelegateService.commonService.GetDate();
                return new UifJsonResult(true, dateNow);

            }
            catch (Exception ex)
            {

                return new UifJsonResult(false, ex.Message);
            }
        }

        //public ActionResult GetAplicationPartnerByDocumentIdDocumentTypeIndividualId( string documentId, int documentType, int IndividualId)
        //{
        //    try
        //    {
        //        PartnerDTO partnerId;
        //        partnerId = DelegateService.uniquePersonAplicationService.GetAplicationPartnerByDocumentIdDocumentTypeIndividualId(documentId, documentType, IndividualId);
        //        if (partnerId != null)
        //        {
        //            return new UifJsonResult(true, partnerId);
        //        }
        //        else
        //        {
        //            return new UifJsonResult(false, App_GlobalResources.Language.PartnerEmpty);
        //        }

        //    }
        //    catch (Exception)
        //    {
        //        return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPartnerByDocumentIdDocumentTypeIndividualId);
        //    }

        //}
        #endregion

        #region Grabar o Crear
        [HttpPost]

        public UifJsonResult CreateAplicationPartner(List<PartnerDTO> partnerDTO)

        {
            try
            {
                PartnerDTO partner = new PartnerDTO();
                foreach (var item in partnerDTO)
                {

                    switch (item.statusTypeService)
                    {
                        case Sistran.Company.Application.ModelServices.Enums.StatusTypeService.Create:
                            partner = DelegateService.uniquePersonAplicationService.CreateAplicationPartner(item);

                            break;
                        case Sistran.Company.Application.ModelServices.Enums.StatusTypeService.Update:
                            partner = DelegateService.uniquePersonAplicationService.UpdateAplicationPartner(item);
                            break;
                    }

                }


                return new UifJsonResult(true, partner);

            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorCreatePartner);
            }


        }

        public ActionResult CreateLegalRepresent(LegalRepresentativeDTO legalRepresentative)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.uniquePersonAplicationService.CreateLegalRepresentative(legalRepresentative));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorCreateLegalRepresent);
            }

        }
        [AcceptVerbs(HttpVerbs.Post)]
        public UifJsonResult CreateAgent(AgentDTO agentDTOs)
        {
            try
            {
                agentDTOs.UserId = SessionHelper.GetUserId();
                var AgentDto = DelegateService.uniquePersonAplicationService.ProcessAplicationAgent(agentDTOs);
                return new UifJsonResult(true, AgentDto);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorCreateAgent);
            }
        }
        public ActionResult CreateInsuredConsortium(CPEMCV1.CompanyInsured insured)
        {
            try
            {
                ACM.Branch BranchesByUser;
                BranchesByUser = DelegateService.uniqueUserService.GetDefaultBranchesByUserId(SessionHelper.GetUserId());
                if (BranchesByUser == null)
                {
                    return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorBranchDefault);
                }
                else
                {
                    if (insured.Branch == null)
                    {
                        insured.Branch = new Sistran.Company.Application.CommonServices.Models.CompanyBranch();
                    }
                    insured.Branch.Id = BranchesByUser.Id;

                    CPEMCV1.CompanyInsured insuredModel = DelegateService.uniquePersonServiceV1.CreateCompanyInsured(insured);
                    return new UifJsonResult(true, insuredModel);
                }

            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorCreateInsured);
            }
        }

        public UifJsonResult CreateCompany(CompanyDTO companyDTO)
        {
            try
            {
                companyDTO.UserId = SessionHelper.GetUserId();
                CompanyDTO company;

                if (companyDTO.Id == (int)IndividualNew.New || companyDTO.Id == (int)IndividualNew.Prospect)
                {
                    if (companyDTO.Sarlaft != null)
                    {
                        for (int i = 0; i < companyDTO.Sarlaft.Count; i++)
                        {
                            companyDTO.Sarlaft[i] = GetPersonDatesUser(companyDTO.Sarlaft[i]);
                        }
                    }
                    company = DelegateService.uniquePersonAplicationService.CreateAplicationCompany(companyDTO);
                    if (company.Addresses != null && company.Addresses.Count() > 0)
                    {
                        foreach (AddressDTO item in company.Addresses)
                        {
                            string stateDescription = GetStatesByCountryIdByDescriptionByStateId(item.CountryId, item.StateId);
                            item.StateDescription = stateDescription;
                            string cityDescription = GetCitiesByCountryIdByStateIdById(item.CountryId, item.StateId, item.CityId);
                            item.CityDescription = cityDescription;
                            string countryDescription = GetCountryByCountryIdReturnDescription(item.CountryId.ToString());
                            item.CountryDescription = countryDescription;
                        }
                    }
                }
                else
                {
                    company = DelegateService.uniquePersonAplicationService.UpdateAplicationCompany(companyDTO);
                    foreach (AddressDTO item in company?.Addresses)
                    {
                        string stateDescription = GetStatesByCountryIdByDescriptionByStateId(item.CountryId, item.StateId);
                        item.StateDescription = stateDescription;
                        string cityDescription = GetCitiesByCountryIdByStateIdById(item.CountryId, item.StateId, item.CityId);
                        item.CityDescription = cityDescription;
                        string countryDescription = GetCountryByCountryIdReturnDescription(item.CountryId.ToString());
                        item.CountryDescription = countryDescription;
                    }
                }
                return new UifJsonResult(true, company);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorCreateCompany);
            }

        }
        [HttpPost]
        public ActionResult CreateAplicationPerson(PersonDTO personDTO)
        {
            try
            {

                personDTO.UserId = SessionHelper.GetUserId();
                if (personDTO.Id == (int)IndividualNew.New)
                {
                    if (personDTO.Sarlaft != null)
                    {
                        for (int i = 0; i < personDTO.Sarlaft.Count; i++)
                        {
                            personDTO.Sarlaft[i] = GetPersonDatesUser(personDTO.Sarlaft[i]);
                        }
                    }
                    var personDTOCreate = DelegateService.uniquePersonAplicationService.CreateAplicationPerson(personDTO);

                    if (personDTOCreate.Addresses != null && personDTOCreate.Addresses.Count() > 0)
                    {
                        foreach (AddressDTO item in personDTOCreate.Addresses)
                        {
                            string stateDescription = GetStatesByCountryIdByDescriptionByStateId(item.CountryId, item.StateId);
                            item.StateDescription = stateDescription;
                            string cityDescription = GetCitiesByCountryIdByStateIdById(item.CountryId, item.StateId, item.CityId);
                            item.CityDescription = cityDescription;
                            string countryDescription = GetCountryByCountryIdReturnDescription(item.CountryId.ToString());
                            item.CountryDescription = countryDescription;
                        }
                    }

                    return new UifJsonResult(true, personDTOCreate);
                }
                else
                {
                    var personDTOUpdate = DelegateService.uniquePersonAplicationService.UpdateAplicationPerson(personDTO);
                    foreach (AddressDTO item in personDTOUpdate?.Addresses)
                    {
                        string stateDescription = GetStatesByCountryIdByDescriptionByStateId(item.CountryId, item.StateId);
                        item.StateDescription = stateDescription;
                        string cityDescription = GetCitiesByCountryIdByStateIdById(item.CountryId, item.StateId, item.CityId);
                        item.CityDescription = cityDescription;
                        string countryDescription = GetCountryByCountryIdReturnDescription(item.CountryId.ToString());
                        item.CountryDescription = countryDescription;
                    }
                    return new UifJsonResult(true, personDTOUpdate);
                }

            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorCreatePerson);
            }
        }

        public ActionResult CreateProspectPersonNatural(ProspectPersonNaturalDTO prospectModel)
        {
            try
            {


                CPEMCV1.CompanyProspectNatural prospectPersonNatural = DelegateService.uniquePersonServiceV1.CreateProspectPersonNatural(ASEPER.ModelAssembler.CreateCompanyProspecNatural(prospectModel));
                if (prospectPersonNatural == null)
                {

                    return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorExistNaturalProspectIdentification);
                }
                else
                {
                    return new UifJsonResult(true, ASEPER.ModelAssembler.CreateCompanyProspecNaturalDto(prospectPersonNatural));
                }
            }
            catch
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorCreateProspect);
            }

        }

        public ActionResult CreateProspectPersonLegal(ProspectLegalDTO prospectLegal)
        {
            try
            {
                ProspectLegalDTO prospectPersonLegal = DelegateService.uniquePersonAplicationService.CreateProspectLegal(prospectLegal);

                if (prospectPersonLegal == null)
                {
                    return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorExistNaturalProspectIdentification);
                }
                else
                {
                    return new UifJsonResult(true, prospectPersonLegal);
                }
            }
            catch
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorCreateProspect);
            }
        }

        #endregion

        #region Actualizacion

        public ActionResult UpdateOperatingQuota(CPEMV1.OperatingQuota OperatingQuota)
        {
            try
            {
                CPEMV1.OperatingQuota operatingQuotaSave = DelegateService.uniquePersonServiceV1.UpdateOperatingQuota(OperatingQuota);
                return new UifJsonResult(true, operatingQuotaSave);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorUpdateOperatingQuota);
            }
        }
        #endregion

        #region Eliminar Borrar
        //public ActionResult DeleteOperatingQuota(CPEM.OperatingQuota operatingQuota)
        public ActionResult DeleteOperatingQuota(OperatingQuotaDTO operatingQuotaDTO)
        {
            try
            {
                bool res = DelegateService.uniquePersonAplicationService.DeleteOperatingQuota(operatingQuotaDTO);
                return new UifJsonResult(true, operatingQuotaDTO);
                //bool res = DelegateService.uniquePersonServiceV1.DeleteOperatingQuota(operatingQuota);
                //return new UifJsonResult(true, operatingQuota);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorDeleteOperatingQuota);
            }
        }
        #endregion

        #endregion

        #region Capa Pais Compañia

        #region Obtener
        [HttpPost]
        public ActionResult GetDaneCodeByCountryIdByStateIdByCityId(int countryId, int stateId, int cityId)
        {
            try
            {
                var city = DelegateService.commonService.GetCityByCountryIdByStateIdByCityId(countryId, stateId, cityId);


                if (!string.IsNullOrEmpty(city?.DANECode))
                {
                    return new UifJsonResult(true, city?.DANECode);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorDaneCodeEmpty);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorConsultingCodeDane);
            }
        }

        public JsonResult GetDaneCodeByQuery(string query)
        {
            try
            {
                List<ACM.City> city = (from c in countries
                                       from s in c.States
                                       from cy in s.Cities
                                       select cy).ToList();

                var dataFiltered = city.Where(d => d.DANECode != null && d.DANECode.Contains(query));

                if (!String.IsNullOrEmpty(query))
                {
                    return Json(dataFiltered, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(city, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpPost]
        public JsonResult GetStateCityByDaneCode(string daneCode, int countryId)
        {
            try
            {
                ACM.City city = (from c in countries
                                 from s in c.States
                                 from cy in s.Cities
                                 where c.Id == countryId && cy.DANECode == daneCode
                                 select cy).FirstOrDefault();

                if (city != null)
                {
                    return new UifJsonResult(true, city);
                }
                else
                {
                    return new UifJsonResult(false, null);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, new { ErrorMsg = ex.Message });
            }
        }
        public ActionResult GetAssociationTypes()
        {
            try
            {
                List<AssociationTypeDTO> associationTypesDTO = DelegateService.uniquePersonAplicationService.GetAplicationAssociationTypes();

                return new UifJsonResult(true, associationTypesDTO.OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetCoAssociationType);
            }
        }

        public ActionResult GetCoConsortiumsByInsuredCode(int insureCode)
        {
            try
            {
                if (insureCode != 0)
                {
                    var coConsortiums = DelegateService.uniquePersonServiceV1.GetCoConsortiumsByInsuredCode(insureCode);
                    return new UifJsonResult(true, coConsortiums);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetConsortiums);
                }
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetConsortiums);
            }

        }

        public ActionResult GetConsortiumEventByIndividualId(int individualId)
        {
            try
            {
                if (individualId != 0)
                {
                    var coConsortiums = DelegateService.consortiumService.GetConsortiumEventByIndividualId(individualId);
                    return new UifJsonResult(true, coConsortiums);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetConsortiums);
                }
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetConsortiums);
            }

        }

        public UifJsonResult GetFinancialSarlaftBySarlaftId(int sarlaftId)
        {

            try
            {
                CPEMCV1.FinancialSarlaf sarlaftModel = DelegateService.uniquePersonServiceV1.GetCompanyFinancialSarlaftBySarlaftId(sarlaftId);
                return new UifJsonResult(true, sarlaftModel);
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetFinancialSarlaft);
            }

        }

        public UifJsonResult GetCoCompanyNameByIndividualId(int individualId)
        {
            try
            {
                List<CPEMV1.CompanyName> CompanyName = DelegateService.uniquePersonServiceV1.GetCompanyNamesByIndividualId(individualId);
                return new UifJsonResult(true, CompanyName);
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetCoCompanyNameByIndividualId);
            }

        }

        //public UifJsonResult GetCoInsurerByIndividualId(int individualId)
        public UifJsonResult GetCompanyCoInsuredIndivualID(int IndividualId)
        {
            try
            {
                CompanyCoInsuredDTO companyCoInsuredDTO = DelegateService.uniquePersonAplicationService.GetCompanyCoInsuredIndivualID(IndividualId);
                if (companyCoInsuredDTO != null)
                {
                    return new UifJsonResult(true, companyCoInsuredDTO);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetCompanyCoInsuredIndivualIDNull);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetCompanyCoInsuredIndivualID);
            }
        }

        public UifJsonResult UpdateCompanyCoInsured(CompanyCoInsuredDTO companyCoInsuredDTO)
        {
            try
            {
                if (companyCoInsuredDTO != null)
                {
                    CompanyCoInsuredDTO companyCoInsured = DelegateService.uniquePersonAplicationService.UpdateCompanyCoInsured(companyCoInsuredDTO);
                    return new UifJsonResult(true, companyCoInsured);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorUpdateCompanyCoInsuredNull);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorUpdateCompanyCoInsured);
            }
        }

        public UifJsonResult GetAplicationReInsurerByIndividualId(int IndividualId)
        {
            try
            {
                ReInsurerDTO reInsurer = DelegateService.uniquePersonAplicationService.GetAplicationReInsurerByIndividualId(IndividualId);
                return new UifJsonResult(true, reInsurer);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetAplicationReInsurerByIndividualId);
            }
        }
        #endregion

        #region Crear

        public ActionResult CreateIndividualSarlaft(CPEMCV1.IndividualSarlaft individualSarlaft, int individualId, int economicActivity)
        {
            try
            {
                ACM.Branch BranchesByUser;
                BranchesByUser = DelegateService.uniqueUserService.GetDefaultBranchesByUserId(SessionHelper.GetUserId());
                individualSarlaft.IndividualId = 1;
                individualSarlaft.InterviewerPlace = BranchesByUser.Description;
                individualSarlaft.AuthorizedBy = User.Identity.Name;
                individualSarlaft.VerifyingEmployee = User.Identity.Name;
                individualSarlaft.InterviewerName = User.Identity.Name;
                individualSarlaft.InterviewResultCode = 1;
                individualSarlaft.InternationalOperations = Convert.ToBoolean(1);
                individualSarlaft.PendingEvents = Convert.ToBoolean(0);
                CPEMCV1.IndividualSarlaft individualSarlaftModel = DelegateService.uniquePersonServiceV1.CreateIndividualSarlaft(individualSarlaft, individualId, economicActivity);
                return new UifJsonResult(true, individualSarlaftModel);
            }
            catch
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorCreateSarlaft);
            }
        }

        public UifJsonResult CreateCoInsurer(CompanyCoInsuredDTO companyCoInsuredDTO)
        {
            try
            {
                companyCoInsuredDTO.UserId = SessionHelper.GetUserId();
                CompanyCoInsuredDTO companyCoInsured = DelegateService.uniquePersonAplicationService.CreateCompanyCoInsured(companyCoInsuredDTO);
                return new UifJsonResult(true, companyCoInsured);
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorSaveCoinsurance);
            }
        }

        public UifJsonResult CreateAplicationReInsurer(ReInsurerDTO reInsurerDTO)
        {
            try
            {
                ReInsurerDTO reInsurer = null;
                reInsurerDTO.UserId = SessionHelper.GetUserId();
                if (reInsurerDTO.Id == 0)
                {
                    reInsurer = DelegateService.uniquePersonAplicationService.CreateAplicationReInsurer(reInsurerDTO);
                }
                else if (reInsurerDTO.IndividualId > 0)
                {
                    reInsurer = DelegateService.uniquePersonAplicationService.UpdateAplicationReInsurer(reInsurerDTO);
                }

                return new UifJsonResult(true, reInsurer);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }


        }


        public UifJsonResult createIndividualPayment(List<IndividualPaymentMethodDTO> individualPaymentMethods, int individualId)
        {
            try
            {
                individualPaymentMethods.First().UserId = SessionHelper.GetUserId();
                individualPaymentMethods = DelegateService.uniquePersonAplicationService.CreateIndividualpaymentMethods(individualPaymentMethods, individualId);
                return new UifJsonResult(true, individualPaymentMethods);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, e.Message);
            }
        }

        public ActionResult GetOthersDeclinedTypes()
        {
            try
            {
                List<AllOthersDeclinedTypeDTO> othersDeclinedTypes = DelegateService.uniquePersonAplicationService.GetAplicationAllOthersDeclinedTypes();

                return new UifJsonResult(true, othersDeclinedTypes);

            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetProviderDeclinedType);
            }

        }
        #endregion

        #region Eliminar
        #endregion
        #endregion

        #region AdvancedSearch
        public UifJsonResult GetAplicationPersonAdv(PersonDTO personDTO)
        {
            try
            {
                var personsModels = DelegateService.uniquePersonAplicationService.GetAplicationPersonAdv(personDTO);
                return new UifJsonResult(true, personsModels);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetProvider);
            }
        }


        public UifJsonResult GetAplicationCompanyAdv(CompanyDTO companyDTO)
        {
            try
            {
                var personsCompanyModels = DelegateService.uniquePersonAplicationService.GetAplicationCompanyAdv(companyDTO);
                return new UifJsonResult(true, personsCompanyModels);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetProvider);
            }
        }

        public UifJsonResult GetAplicationProspectLegalAdv(ProspectLegalDTO companyDTO)
        {
            try
            {
                var personsCompanyModels = DelegateService.uniquePersonAplicationService.GetAplicationProspectLegalAdv(companyDTO);
                return new UifJsonResult(true, personsCompanyModels);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetProvider);
            }
        }

        public UifJsonResult GetAplicationProspectnNaturalAdv(ProspectPersonNaturalDTO companyDTO)
        {
            try
            {
                var personsCompanyModels = DelegateService.uniquePersonAplicationService.GetAplicationProspectNaturalAdv(companyDTO);
                return new UifJsonResult(true, personsCompanyModels);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetProvider);
            }
        }


        public UifJsonResult GetAplicationCompanyById(int id)
        {
            try
            {
                var autorequest = DelegateService.uniquePersonServiceV1.GetAuthorizationRequestByIndividualId(id);
                if (autorequest?.Count > 0)
                {
                    return new UifJsonResult(true, new { IsAuthorizationRequest = true, AuthorizationRequests = autorequest });
                }
                else
                {
                    var companiesModels = DelegateService.uniquePersonAplicationService.GetAplicationCompanyById(id);
                    return new UifJsonResult(true, companiesModels);
                }

            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetProvider);
            }

        }

        public UifJsonResult GetOperatingQuotaEventByIndividualId(int individualId)
        {
            try
            {
                var operatingQuotaEvent = DelegateService.operationQuotaService.GetOperatingQuotaEventByIndividualId(individualId);
                return new UifJsonResult(true, operatingQuotaEvent);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetCompany);
            }
        }

        public UifJsonResult GetAplicationPersonById(int individualId)
        {
            try
            {
                var autorequest = DelegateService.uniquePersonServiceV1.GetAuthorizationRequestByIndividualId(individualId);

                if (autorequest?.Count > 0)
                {
                    return new UifJsonResult(true, new { IsAuthorizationRequest = true, AuthorizationRequests = autorequest });
                }
                else
                {
                    var personsModels = DelegateService.uniquePersonAplicationService.GetAplicationPersonById(individualId);
                    return new UifJsonResult(true, personsModels);
                }

            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetProvider);
            }
        }

        public UifJsonResult GetIndividualNotification(int individualId)
        {
            try
            {
                var dtoModel = DelegateService.uniquePersonAplicationService.GetPersonByIndividualId(individualId);

                return new UifJsonResult(true, dtoModel);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetProvider);
            }
        }
        #endregion

        public ActionResult GetProspectByDocumenNumberOrDescription(string description, Sistran.Core.Services.UtilitiesServices.Enums.InsuredSearchType insuredSearchType)
        {
            try
            {
                List<CPEMV1.Prospect> prospects = DelegateService.uniquePersonServiceV1.GetProspectByDescription(description, insuredSearchType);
                return new UifJsonResult(true, prospects);

            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorQueryProspect);
            }
        }

        public ActionResult GetInsuredsByDescription(string description, InsuredSearchType insuredSearchType)
        {
            try
            {
                List<IssuanceInsured> issuanceInsureds = DelegateService.underwritingServiceCore.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, Core.Services.UtilitiesServices.Enums.CustomerType.Individual);
                return new UifJsonResult(true, issuanceInsureds);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public PartialViewResult PersonNatural()
        {
            return PartialView();
        }

        public PartialViewResult PersonLegal()
        {
            return PartialView();
        }

        public PartialViewResult ProspectusLegal()
        {
            return PartialView();
        }

        public PartialViewResult ProspectusNatural()
        {
            return PartialView();
        }

        public ActionResult DateNow()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.commonService.GetDate().ToString("dd/MM/yyyy"));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorSearch);
            }
        }

        public ActionResult GetParameters()
        {
            try
            {
                if (parameters.Count == 0)
                {
                    GetCountries();

                    parameters.Add(new Sistran.Core.Application.CommonService.Models.Parameter { Description = "Currency", Value = DelegateService.commonService.GetParameterByParameterId((int)ParametersTypes.Currency).NumberParameter.Value });

                    parameters.Add(new Sistran.Core.Application.CommonService.Models.Parameter { Description = "Country", Value = DelegateService.commonService.GetParameterByParameterId((int)ParametersTypes.Country).NumberParameter.Value });

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

        public ActionResult GetParametersRouter()
        {
            try
            {
                if (parameterPersonType.Count == 0)
                {
                    parameterPersonType.Add(new Sistran.Core.Application.CommonService.Models.Parameter { Description = "PersonType", Value = DelegateService.commonService.GetParameterByParameterId((int)ParametersTypes.PersonType).NumberParameter.Value });
                }

                return new UifJsonResult(true, parameterPersonType);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorSearchParameter);
            }
        }

        #region Provider





        #endregion

        //public ActionResult GetPaymentConcept()
        //{
        //    try
        //    {
        //        if (paymentConcept.Count == 0)
        //        {
        //            paymentConcept = DelegateService.commonService.GetPaymentConcept();
        //        }
        //        return new UifJsonResult(true, paymentConcept);

        //    }
        //    catch (Exception)
        //    {
        //        return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetPaymentConcept);
        //    }

        //}

        public ActionResult GetSpecialtiesTable()
        {
            try
            {
                if (specialties.Count == 0)
                {
                    specialties = DelegateService.uniquePersonServiceV1.GetSpecialties();
                }
                return new UifJsonResult(true, specialties);

            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetSpecialties);
            }

        }

        #region Impuestos

        public UifJsonResult GetIndividualTaxExeptionByIndividualId(int individualId)
        {
            try
            {
                List<IndividualTaxExeptionDTO> IndividualTaxExeption = DelegateService.uniquePersonAplicationService.GetIndividualTaxExeptionByIndividualId(individualId);
                return new UifJsonResult(true, IndividualTaxExeption);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorTaxExists);
            }
        }

        public JsonResult CreateIndividualTax(List<IndividualTaxExeptionDTO> listIndividualTaxExeption)
        {
            try
            {
                if (listIndividualTaxExeption != null)
                {
                    List<IndividualTaxExeptionDTO> individualTaxExeptiondDTO = new List<IndividualTaxExeptionDTO>();
                    listIndividualTaxExeption.ForEach(x => x.UserId = SessionHelper.GetUserId());

                    List<IndividualTaxExeptionDTO> updindividualTaxExeptiondDTO = listIndividualTaxExeption.Where(m => m.Id > 0).ToList();
                    List<IndividualTaxExeptionDTO> insindividualTaxExeptiondDTO = listIndividualTaxExeption.Where(m => m.Id == 0).ToList();

                    if (insindividualTaxExeptiondDTO.Count > 0)
                    {
                        individualTaxExeptiondDTO.AddRange(DelegateService.uniquePersonAplicationService.CreateIndividualTax(insindividualTaxExeptiondDTO));
                    }

                    if (updindividualTaxExeptiondDTO.Count > 0)
                    {
                        individualTaxExeptiondDTO.AddRange(DelegateService.uniquePersonAplicationService.UpdateIndividualTaxExeption(updindividualTaxExeptiondDTO));
                    }

                    return new UifJsonResult(true, individualTaxExeptiondDTO);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorNullTax);
                }


            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorCreateTax);
            }
        }

        public ActionResult DeleteIndividualTaxExeption(IndividualTaxExeptionDTO IndividualTaxExeptionDTO)
        {
            try
            {
                IndividualTaxExeptionDTO IndividualTaxException = new IndividualTaxExeptionDTO();
                if (IndividualTaxExeptionDTO.IndividualId != 0)
                {
                    DelegateService.uniquePersonAplicationService.DeleteIndividualTaxExeption(IndividualTaxExeptionDTO);
                }

                return new UifJsonResult(true, IndividualTaxExeptionDTO);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorTaxExists);
            }
        }

        public ActionResult GetTax()
        {
            try
            {
                if (tax.Count == 0)
                {
                    tax = DelegateService.taxService.GetTax();
                }
                return new UifJsonResult(true, tax);

            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetTax);
            }

        }

        public ActionResult GetCompanyIndividualRoleByIndividualId(int individualId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.uniquePersonService.GetCompanyIndividualRoleByIndividualId(individualId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorRolInsured);
            }

        }

        public ActionResult GetTaxConditionByTaxId(int taxId)
        {

            try
            {
                List<TaxModel.TaxCondition> taxCondition = DelegateService.taxService.GetTaxConditionByTaxId(taxId);
                return new UifSelectResult(taxCondition);

            }
            catch (Exception)
            {
                return new UifSelectResult("");
            }

        }

        public ActionResult GetTaxCategoryByTaxId(int taxId)
        {
            try
            {
                List<TaxModel.TaxCategory> taxCategorie = DelegateService.taxService.GetTaxCategoryByTaxId(taxId);
                return new UifSelectResult(taxCategorie);

            }
            catch (Exception)
            {
                return new UifSelectResult("");
            }

        }



        public JsonResult getTaxRateByTaxIdByAttributes(int taxId, int? taxConditionId, int? taxCategoryId, int? countryCode, int? stateCode, int? cityCode, int? economicActivityCode, int? prefixId, int? coverageId, int? technicalBranchId)
        {
            try
            {
                COAP.TaxRateDTO taxRateDTO = DelegateService.CompanyUnderwritingParamApplicationService.getApplicationTaxRateByTaxIdByAttributes(taxId, taxConditionId, taxCategoryId, countryCode, stateCode, cityCode, economicActivityCode, prefixId, coverageId, technicalBranchId);
                if (taxRateDTO.Id > 0)
                {
                    return new UifJsonResult(true, taxRateDTO);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.TaxWithoutAttributes);
                }

            }
            catch
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorTaxRate);
            }

        }

        public JsonResult getTaxRateById(int taxRateId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.CompanyUnderwritingParamApplicationService.GetApplicationTaxRateById(taxRateId));
            }
            catch
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorTaxRate);
            }
        }

        public JsonResult getTaxAttributeTypeByTaxId(int taxId)
        {
            try
            {
                List<Application.TaxServices.DTOs.TaxAttributeDTO> taxAttributes = DelegateService.taxService.GetTaxAttributesByTaxId(taxId);
                return new UifJsonResult(true, taxAttributes);
            }
            catch
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetTaxAttributes);
            }


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CountryId"></param>
        /// <returns></returns>
        public ActionResult GetStatesByStateTaxId()
        {

            try
            {
                ACM.Country country = (from c in countries where c.Id == Convert.ToInt32(defaultCountryId) select c).FirstOrDefault();

                if (country != null)
                {
                    var list = country.States.Select(item => new { item.Id, item.Description }).ToList();


                    return new UifJsonResult(true, list.OrderBy(x => x.Description));
                }
                else
                {
                    return new UifJsonResult(false, new List<ACM.State>());
                }
            }
            catch (Exception)
            {

                return new UifJsonResult(false, "");
            }

        }


        public ActionResult SaveTax(List<CPEMV1.IndividualTaxExeption> taxs)
        {

            try
            {
                foreach (var item in taxs)
                {
                    item.CountryCode = Convert.ToInt32(defaultCountryId);
                }

                List<CPEMV1.IndividualTaxExeption> taxSave = new List<CPEMV1.IndividualTaxExeption>();
                taxSave = DelegateService.uniquePersonServiceV1.CreateIndivualExemptionTaxs(taxs);
                return new UifJsonResult(true, taxSave);

            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorCreateTax);
            }
        }
        #endregion

        public ActionResult GetDefaultValues()
        {
            try
            {
                if (!defaultValues.Keys.Contains(SessionHelper.GetUserId()))
                {
                    ACM.DefaultValue defaultValue = new ACM.DefaultValue();
                    defaultValue.UserId = SessionHelper.GetUserId();
                    defaultValue.ModuleId = (int)Modules.Comercial;
                    defaultValue.SubModuleId = (int)SubModules.Administration;
                    defaultValue.ViewName = "/Person/Persons";
                    defaultValues.Add(SessionHelper.GetUserId(), DelegateService.commonService.GetDefaultValueByDefaultValue(defaultValue));
                }

                return new UifJsonResult(true, defaultValues[SessionHelper.GetUserId()]);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorSearchParameter);
            }
        }

        #region EESGE-172 Control De Busqueda
        public ActionResult GetCountriesByDescription(string Description)
        {
            try
            {
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
        // DaneCode
        public ActionResult GetCountryAndStateAndCityByDaneCode(int CountryId, string DaneCode)
        {
            try
            {
                var country = countries.Where(x => x.Id == CountryId).ToList()[0];

                foreach (var itemState in country.States)
                {
                    foreach (var itemCity in itemState.Cities)
                    {
                        if (itemCity.DANECode == DaneCode)
                        {
                            itemCity.State.Id = itemState.Id;
                            itemCity.State.Description = itemState.Description;
                            itemCity.State.Country.Id = country.Id;
                            itemCity.State.Country.Description = country.Description;
                            itemCity.State.Id = itemState.Id;
                            itemCity.State.Country.Id = country.Id;

                            return new UifJsonResult(true, itemCity);
                        }
                    }
                }

                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorDaneCode);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorConsultingCodeDane);
            }
        }
        #endregion

        public ActionResult GetParameterFutureSociety()
        {
            try
            {
                int userId = SessionHelper.GetUserId();
                ACM.Parameter parameter = DelegateService.uniquePersonAplicationService.GetParameterFutureSociety((int)parameterType.FutureSociety);
                return new UifJsonResult(true, parameter);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorSearchParameter);
            }

        }

        public JsonResult GetProspectByIndividualId(string individualId)
        {
            try
            {
                List<CPEMV1.ProspectNatural> prospects = DelegateService.uniquePersonServiceV1.GetProspectByIndividualId(individualId);
                if (prospects.Count > 0)
                {
                    return new UifJsonResult(true, prospects);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.LanguagePerson.MessageNotFoundProspects);
                }
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetProspect);
            }
        }

        public UifJsonResult GetPrefixesByAgentId(int IndividualId)
        {
            List<PrefixDTO> prefix;
            try
            {
                prefix = DelegateService.uniquePersonAplicationService.GetPrefixAgentByInvidualId(IndividualId);
                return new UifJsonResult(true, prefix);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetPrefixes);
            }
        }

        public ActionResult GetLineBusinessByPrefixId(int prefixId)
        {
            List<ACM.LineBusiness> lineBusiness;
            try
            {
                lineBusiness = DelegateService.commonService.GetLineBusinessByPrefixId(prefixId);
                return new UifSelectResult(lineBusiness);

            }
            catch (Exception)
            {
                return new UifSelectResult(false, App_GlobalResources.LanguagePerson.ErrorGetPrefixes);
            }

        }

        public ActionResult GetSubLinesBusinessByLineBusinessId(int LinesBusiness)
        {
            List<ACM.SubLineBusiness> subLineBusiness;
            try
            {
                subLineBusiness = DelegateService.commonService.GetSubLinesBusinessByLineBusinessId(LinesBusiness);
                return new UifSelectResult(subLineBusiness);

            }
            catch (Exception)
            {
                return new UifSelectResult(false, App_GlobalResources.LanguagePerson.ErrorGetPrefixes);
            }

        }

        public ActionResult GetAgentCommissionByIndividualId(int individualId)
        {
            try
            {
                List<CPEMV1.Commission> commisionAgent = new List<CPEMV1.Commission>();
                commisionAgent = DelegateService.uniquePersonServiceV1.GetAgentCommissionByIndividualId(individualId);
                return new UifSelectResult(commisionAgent);
            }
            catch
            {
                return new UifSelectResult("");
            }
        }
        public ActionResult GetAgentCommissionByAgentId(int agentId)
        {
            try
            {
                ComissionAgent = DelegateService.uniquePersonServiceV1.GetAgentCommissionByAgentId(agentId);
                return new UifJsonResult(true, ComissionAgent.OrderBy(x => x.Id));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetAgentAgency);
            }

        }

        public ActionResult DeleteAgentCommission(CPEMV1.Commission commissionAgent)
        {
            try
            {
                bool res = DelegateService.uniquePersonServiceV1.DeleteAgentCommission(commissionAgent);

                return new UifJsonResult(true, commissionAgent);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorDeleteAgentCommission);
            }
        }

        #region Company.ScoreTypeDoc
        /// <summary>
        /// Vista de tipo documento datacrédito
        /// </summary>
        /// <returns>Vista obtenida</returns>
        public ActionResult ScoreTypeDoc()
        {
            return this.View();
        }

        /// <summary>
        /// Vista del resultado de búsqueda de la vista tipo documento datacrédito
        /// </summary>
        /// <returns>Resultado de la vista</returns>
        [HttpGet]
        public ActionResult ScoreTypeDocAdvancedSearch()
        {
            return this.View();
        }

        /// <summary>
        /// Obtiene la lista de tipos de documento datacrédito
        /// </summary>
        /// <returns>Lista de tipos de documento datacrédito consultadas</returns>
        public ActionResult GetScoreTypeDocs()
        {
            try
            {
                List<Models.ScoreTypeDocViewModel> scoreTypeDocList = Models.ModelAssembler.GetScoreTypeDocs(DelegateService.uniquePersonServiceV1.GetAllScoreTypeDoc());
                return new UifJsonResult(true, scoreTypeDocList.OrderBy(x => x.Description).ToList());
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetDocumentTypes);
            }
        }

        /// <summary>
        /// Realiza los procesos del CRUD para los tipos documento datacrédito
        /// </summary>
        /// <param name="listAdded"> Lista de tipos documento datacrédito para ser agregados</param>
        /// <param name="listModified">Lista de tipos documento datacrédito para ser modificados</param>
        /// <param name="listDeleted">Lista de tipos documento datacrédito para ser eliminados</param>
        /// <returns>Respuesta con el total de procesos realizados </returns>
        public ActionResult SaveScoreTypeDocs(List<Models.ScoreTypeDocViewModel> listAdded, List<Models.ScoreTypeDocViewModel> listModified, List<Models.ScoreTypeDocViewModel> listDeleted)
        {
            try
            {
                ParametrizationResponse<CPEMCV1.ScoreTypeDoc> scoreTypeDocResponse = Services.DelegateService.uniquePersonServiceV1.CreateScoreTypeDocs(Models.ModelAssembler.CreateScoreTypeDocs(listAdded), Models.ModelAssembler.CreateScoreTypeDocs(listModified), Models.ModelAssembler.CreateScoreTypeDocs(listDeleted));

                // Formato del mensaje de operaciones realizadas
                string added = string.Empty;
                string edited = string.Empty;
                string deleted = string.Empty;
                string message;

                if (!string.IsNullOrEmpty(scoreTypeDocResponse.ErrorAdded))
                {
                    scoreTypeDocResponse.ErrorAdded = App_GlobalResources.LanguagePerson.ResourceManager.GetString(scoreTypeDocResponse.ErrorAdded);
                }

                if (!string.IsNullOrEmpty(scoreTypeDocResponse.ErrorModify))
                {
                    scoreTypeDocResponse.ErrorModify = App_GlobalResources.LanguagePerson.ResourceManager.GetString(scoreTypeDocResponse.ErrorModify);
                }

                if (!string.IsNullOrEmpty(scoreTypeDocResponse.ErrorDeleted))
                {
                    scoreTypeDocResponse.ErrorDeleted = App_GlobalResources.LanguagePerson.ResourceManager.GetString(scoreTypeDocResponse.ErrorDeleted);
                }

                if (scoreTypeDocResponse.TotalAdded > 0)
                {
                    added = App_GlobalResources.Language.ReturnSaveAddedScoreTypeDocs;
                }
                else
                {
                    scoreTypeDocResponse.TotalAdded = null;
                }

                if (scoreTypeDocResponse.TotalModify > 0)
                {
                    edited = App_GlobalResources.Language.ReturnSaveEditedScoreTypeDocs;
                }
                else
                {
                    scoreTypeDocResponse.TotalModify = null;
                }

                if (scoreTypeDocResponse.TotalDeleted > 0)
                {
                    deleted = App_GlobalResources.Language.ReturnSaveDeletedScoreTypeDocs;
                }
                else
                {
                    scoreTypeDocResponse.TotalDeleted = null;
                }

                message = string.Format(
                    added + edited + deleted + "{3}{4}{5}",
                    scoreTypeDocResponse.TotalAdded.ToString() ?? string.Empty,
                    scoreTypeDocResponse.TotalModify.ToString() ?? string.Empty,
                    scoreTypeDocResponse.TotalDeleted.ToString() ?? string.Empty,
                    scoreTypeDocResponse.ErrorAdded ?? string.Empty,
                    scoreTypeDocResponse.ErrorModify ?? string.Empty,
                    scoreTypeDocResponse.ErrorDeleted ?? string.Empty);
                var result = Models.ModelAssembler.GetScoreTypeDocs(scoreTypeDocResponse.ReturnedList.OrderBy(x => x.Description).ToList());

                return new UifJsonResult(true, new { message = message, data = result });
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorSaveScoreTypeDocs);
            }
        }

        /// <summary>
        /// Obtiene tipos documento datcrédito
        /// </summary>
        public void GetListScoreTypeDocs()
        {
            List<CPEMCV1.ScoreTypeDoc> scoreTypeDocs = new List<CPEMCV1.ScoreTypeDoc>();
            if (scoreTypeDocs.Count == 0)
            {
                scoreTypeDocs = DelegateService.uniquePersonServiceV1.GetAllScoreTypeDoc().ToList();
            }
        }

        /// <summary>
        /// Genera archivo excel tipos de documento datacrédito
        /// </summary>
        /// <returns>Archivo con el resultado de la consulta</returns>
        public ActionResult GenerateScoreTypeDocFileToExport()
        {
            try
            {
                var urlFile = DelegateService.uniquePersonServiceV1.GenerateFileToScoreTypeDoc();

                return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGeneratingFile);
            }
        }
        #endregion

        #region InsuredV1

        public ActionResult CreateInsured(InsuredDTO insured)
        {
            try
            {
                InsuredDTO insuredModel = null;
                insured.UserId = SessionHelper.GetUserId();

                if (insured.Id == 0)

                {
                    insuredModel = DelegateService.uniquePersonAplicationService.CreateAplicationInsured(insured);

                }
                else
                {
                    insuredModel = DelegateService.uniquePersonAplicationService.UpdateAplicationInsured(insured);
                }
                return new UifJsonResult(true, insuredModel);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorCreateInsured);
            }
        }
        public ActionResult GetInsuredByIndividualId(int individualId)
        {
            try
            {

                InsuredDTO insured = DelegateService.uniquePersonAplicationService.GetAplicationInsuredByIndividualId(individualId);
                if (insured != null)
                {
                    return new UifJsonResult(true, insured);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorRolInsured);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetInsured);
            }
        }

        #endregion

        public ActionResult GetGroupAgent()
        {
            try
            {
                List<GroupAgentDTO> agentGroup = DelegateService.uniquePersonAplicationService.GetAplicationGroupAgent();
                return new UifJsonResult(true, agentGroup.OrderBy(x => x.Description));
            }
            catch
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetInsuredDeclinedTypes);
            }
        }

        public ActionResult GetSalesChannel()
        {
            try
            {
                List<SalesChannelDTO> agentSaleChannel = DelegateService.uniquePersonAplicationService.GetAplicationSalesChannel();
                return new UifJsonResult(true, agentSaleChannel.OrderBy(x => x.Description));
            }
            catch
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetInsuredDeclinedTypes);
            }
        }

        public ActionResult GetEmployeePersonByFullName(string description)
        {
            try
            {
                List<EmployeePersonDTO> employeePersonsDTO = DelegateService.uniquePersonAplicationService.GetAplicationEmployeePersons();

                int employeePersonId = 0;
                Int32.TryParse(description, out employeePersonId);

                if (employeePersonId == 0)
                {
                    var dataName = employeePersonsDTO.Where(x => x.Name.ToLower().Contains(description.ToLower()));
                    var dataLastName = employeePersonsDTO.Where(x => x.MotherLastName.ToLower().Contains(description.ToLower()));

                    if (dataName.Count() > 0)
                    {
                        return new UifJsonResult(true, dataName);
                    }
                    else if (dataLastName.Count() > 0)
                    {
                        return new UifJsonResult(true, dataLastName);
                    }
                    else
                    {
                        return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorAccountExecutive);
                    }
                }
                else
                {
                    var dataId = employeePersonsDTO.Where(x => x.IdCardNo.ToString() == (employeePersonId.ToString()));
                    return new UifJsonResult(true, dataId);
                }

            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetAccountExecutive);
            }
        }

        public ActionResult GetInsuredSegment()
        {
            try
            {
                List<InsuredSegmentDTO> insuredSegmentsList = DelegateService.uniquePersonAplicationService.GetAplicationInsuredSegment();
                return new UifJsonResult(true, insuredSegmentsList);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, "Error al consultar");
            }

        }

        public ActionResult GetInsuredProfile()
        {
            try
            {
                List<InsuredProfileDTO> insuredProfileList = DelegateService.uniquePersonAplicationService.GetAplicationInsuredProfile();
                return new UifJsonResult(true, insuredProfileList);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, "Error al consultar");
            }
        }

        public ActionResult GetInsuredsByName(string name)
        {
            try
            {
                List<CPEMCV1.CompanyInsuredMain> CompanyInsuredMainsList = DelegateService.uniquePersonServiceV1.GetCompanyInsuredsByName(name);
                return new UifJsonResult(true, CompanyInsuredMainsList);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, "Error al consultar");
            }
        }

        #region "PersonInterestGroup"

        public ActionResult GetInterestGroupTypes()
        {
            try
            {
                List<CPEMV1.InterestGroupsType> interestGroupsTypes = DelegateService.uniquePersonServiceV1.GetInterestGroupTypes();
                return new UifTableResult(interestGroupsTypes);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, "Error al consultar");
            }
        }

        public ActionResult GetPersonInterestGroups(int individualId)
        {
            try
            {
                List<CPEMV1.PersonInterestGroup> personInterestGroups = DelegateService.uniquePersonServiceV1.GetPersonInterestGroups(individualId);
                return new UifJsonResult(true, personInterestGroups);
            }
            catch
            {
                return new UifJsonResult(false, "Error al consultar");
            }
        }

        #endregion

        public ActionResult SaveBasicCompany(BasicCompanyServiceModel companyViewModel)
        {
            companyViewModel.UpdateBy = User.Identity.Name;
            companyViewModel.LastUpdate = DateTime.Now;
            BasicCompanysServiceModel basicCompanys = DelegateService.companyUniquePersonParamService.SaveCompanyBasic(companyViewModel);
            ErrorTypeService errorTypeProcess = basicCompanys.ErrorTypeService;
            if (errorTypeProcess == ErrorTypeService.Ok)
            {
                return new UifJsonResult(true, basicCompanys);
            }
            else if (errorTypeProcess == ErrorTypeService.NotFound || errorTypeProcess == ErrorTypeService.BusinessFault || errorTypeProcess == ErrorTypeService.TechnicalFault)
            {
                return new UifJsonResult(false, this.ErrorMessages(basicCompanys.ErrorDescription));
            }
            else
            {
                return null;
            }
        }

        public ActionResult SaveBasicPerson(BasicPersonServiceModel personViewModel)
        {
            personViewModel.UpdateBy = User.Identity.Name;
            personViewModel.LastUpdate = DateTime.Now;
            BasicPersonsServiceModel basicPersons = DelegateService.companyUniquePersonParamService.SavePersonBasic(personViewModel);
            ErrorTypeService errorTypeProcess = basicPersons.ErrorTypeService;
            if (errorTypeProcess == ErrorTypeService.Ok)
            {
                return new UifJsonResult(true, basicPersons);
            }
            else if (errorTypeProcess == ErrorTypeService.NotFound || errorTypeProcess == ErrorTypeService.BusinessFault || errorTypeProcess == ErrorTypeService.TechnicalFault)
            {
                return new UifJsonResult(false, this.ErrorMessages(basicPersons.ErrorDescription));
            }
            else
            {
                return null;
            }
        }
        [HttpPost]
        public JsonResult GetPersonBasicByCodePersonByFirtsNameByLastNameByNameByTypeDocumentByDocumentNumber(string codePerson, string firstName, string lastName, string name, string documentNumber, string typeDocument)
        {
            BasicPersonsServiceModel basicPersons = DelegateService.companyUniquePersonParamService.GetPersonBasicByCodePersonByFirtsNameByLastNameByNameByTypeDocumentByDocumentNumber(codePerson, firstName, lastName, name, documentNumber, typeDocument);
            ErrorTypeService errorTypeProcess = basicPersons.ErrorTypeService;
            if (errorTypeProcess == ErrorTypeService.Ok)
            {
                List<BasicPersonServiceModel> basicPersonTypesServiceModel = basicPersons.BasicPersonServiceModel;
                return new UifJsonResult(true, basicPersons);
            }
            else if (errorTypeProcess == ErrorTypeService.NotFound || errorTypeProcess == ErrorTypeService.BusinessFault || errorTypeProcess == ErrorTypeService.TechnicalFault)
            {
                return new UifJsonResult(false, this.ErrorMessages(basicPersons.ErrorDescription));
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult GetCompanyBasicByCodeCompanyByTradeNameByTypeDocumentByDocumentNumber(string codeCompany, string tradeName, string documentNumber, string typeDocument)
        {

            BasicCompanysServiceModel basicCompanys = DelegateService.companyUniquePersonParamService.GetCompanyBasicByCodeCompanyByTradeNameByTypeDocumentByDocumentNumber(codeCompany, tradeName, documentNumber, typeDocument);
            ErrorTypeService errorTypeProcess = basicCompanys.ErrorTypeService;
            if (errorTypeProcess == ErrorTypeService.Ok)
            {
                List<BasicCompanyServiceModel> basicCopmanyTypesServiceModel = basicCompanys.BasicCompanyServiceModel;
                return new UifJsonResult(true, basicCompanys);
            }
            else if (errorTypeProcess == ErrorTypeService.NotFound || errorTypeProcess == ErrorTypeService.BusinessFault || errorTypeProcess == ErrorTypeService.TechnicalFault)
            {
                return new UifJsonResult(false, this.ErrorMessages(basicCompanys.ErrorDescription));
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult GetPersonBasicByDocumentNumber(string documentNumber)
        {
            BasicPersonsServiceModel basicPersons = DelegateService.companyUniquePersonParamService.GetPersonBasicByDocumentNumber(documentNumber);
            ErrorTypeService errorTypeProcess = basicPersons.ErrorTypeService;
            if (errorTypeProcess == ErrorTypeService.Ok)
            {
                List<BasicPersonServiceModel> basicPersonTypesServiceModel = basicPersons.BasicPersonServiceModel;
                return new UifJsonResult(true, basicPersons);
            }
            else if (errorTypeProcess == ErrorTypeService.NotFound || errorTypeProcess == ErrorTypeService.BusinessFault || errorTypeProcess == ErrorTypeService.TechnicalFault)
            {
                return new UifJsonResult(false, this.ErrorMessages(basicPersons.ErrorDescription));
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult GetCompanyBasicByDocumentNumber(string documentNumber)
        {

            BasicCompanysServiceModel basicCompanys = DelegateService.companyUniquePersonParamService.GetCompanyBasicByDocumentNumber(documentNumber);
            ErrorTypeService errorTypeProcess = basicCompanys.ErrorTypeService;
            if (errorTypeProcess == ErrorTypeService.Ok)
            {
                List<BasicCompanyServiceModel> basicCopmanyTypesServiceModel = basicCompanys.BasicCompanyServiceModel;
                return new UifJsonResult(true, basicCompanys);
            }
            else if (errorTypeProcess == ErrorTypeService.NotFound || errorTypeProcess == ErrorTypeService.BusinessFault || errorTypeProcess == ErrorTypeService.TechnicalFault)
            {
                return new UifJsonResult(false, this.ErrorMessages(basicCompanys.ErrorDescription));
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// Metodo para retornar los mensajes de error.
        /// </summary>
        /// <param name="errorList">Lista de errores</param>
        /// <returns>Mensajes de error.</returns>
        private string ErrorMessages(List<string> errorList)
        {
            if (errorList != null)
            {
                string errorMessages = string.Empty;
                foreach (string errorMessageItem in errorList)
                {
                    errorMessages = errorMessages + errorMessageItem + " <br>";
                }

                return errorMessages;
            }
            return "";
        }

        #region Company.AddressType
        /// <summary>
        /// Vista tipo de dirección        
        /// </summary>
        /// <returns>Vista obtenida</returns>
        public ActionResult CompanyAddressType()
        {
            Models.CompanyAddressTypeViewModel model = new Models.CompanyAddressTypeViewModel();
            return this.View(model);
        }

        /// <summary>
        /// Obtiene la vista del resultado de búsqueda para la vista tipo de dirección
        /// </summary>
        /// <returns>Resultado de la búsqueda</returns>
        [HttpGet]
        public ActionResult CompanyAddressTypeAdvancedSearch()
        {
            return this.View();
        }

        /// <summary>
        /// Obtiene la lista de tipos de dirección
        /// </summary>
        /// <returns>Lista de tipos de dirección consultados</returns>
        public ActionResult GetCompanyAddressTypes()
        {
            try
            {
                List<Models.CompanyAddressTypeViewModel> companyAddressTypeList = Models.ModelAssembler.GetCompanyAddressTypes(DelegateService.uniquePersonServiceV1.GetAllCompanyAddressType());
                return new UifJsonResult(true, companyAddressTypeList.OrderBy(x => x.SmallDescription).ToList());
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetAddressTypes);
            }
        }

        /// <summary>
        /// Realiza los procesos del CRUD para los tipos de dirección
        /// </summary>
        /// <param name="listAdded">Lista de tipos dirección para ser agregados</param>
        /// <param name="listModified">Lista de tipos dirección para ser modificados</param>
        /// <param name="listDeleted">Lista de tipos dirección para ser eliminados</param>
        /// <returns>Respuesta con el total de procesos realizados </returns>
        public ActionResult SaveCompanyAddressTypes(List<Models.CompanyAddressTypeViewModel> listAdded, List<Models.CompanyAddressTypeViewModel> listModified, List<Models.CompanyAddressTypeViewModel> listDeleted)
        {
            try
            {
                ParametrizationResponse<CPEMCV1.CompanyAddressType> companyAddressTypeResponse = Services.DelegateService.uniquePersonServiceV1.CreateCompanyAddressTypes(Models.ModelAssembler.CreateCompanyAddressTypes(listAdded), Models.ModelAssembler.CreateCompanyAddressTypes(listModified), Models.ModelAssembler.CreateCompanyAddressTypes(listDeleted));

                // Formato del mensaje de operaciones realizadas
                string added = string.Empty;
                string edited = string.Empty;
                string deleted = string.Empty;
                string message;

                if (!string.IsNullOrEmpty(companyAddressTypeResponse.ErrorAdded))
                {
                    companyAddressTypeResponse.ErrorAdded = App_GlobalResources.Language.ResourceManager.GetString(companyAddressTypeResponse.ErrorAdded);
                }

                if (!string.IsNullOrEmpty(companyAddressTypeResponse.ErrorModify))
                {
                    companyAddressTypeResponse.ErrorModify = App_GlobalResources.Language.ResourceManager.GetString(companyAddressTypeResponse.ErrorModify);
                }

                if (!string.IsNullOrEmpty(companyAddressTypeResponse.ErrorDeleted))
                {
                    companyAddressTypeResponse.ErrorDeleted = App_GlobalResources.Language.ResourceManager.GetString(companyAddressTypeResponse.ErrorDeleted);
                }

                if (companyAddressTypeResponse.TotalAdded > 0)
                {
                    added = App_GlobalResources.Language.ReturnSaveAddedAddressTypes;
                }
                else
                {
                    companyAddressTypeResponse.TotalAdded = null;
                }

                if (companyAddressTypeResponse.TotalModify > 0)
                {
                    edited = App_GlobalResources.Language.ReturnSaveEditedAddressTypes;
                }
                else
                {
                    companyAddressTypeResponse.TotalModify = null;
                }

                if (companyAddressTypeResponse.TotalDeleted > 0)
                {
                    deleted = App_GlobalResources.Language.ReturnSaveDeletedAddressTypes;
                }
                else
                {
                    companyAddressTypeResponse.TotalDeleted = null;
                }

                message = string.Format(
                    added + edited + deleted + "{3}{4}{5}",
                    companyAddressTypeResponse.TotalAdded.ToString() ?? string.Empty,
                    companyAddressTypeResponse.TotalModify.ToString() ?? string.Empty,
                    companyAddressTypeResponse.TotalDeleted.ToString() ?? string.Empty,
                    companyAddressTypeResponse.ErrorAdded ?? string.Empty,
                    companyAddressTypeResponse.ErrorModify ?? string.Empty,
                    companyAddressTypeResponse.ErrorDeleted ?? string.Empty);
                var result = Models.ModelAssembler.GetCompanyAddressTypes(companyAddressTypeResponse.ReturnedList.OrderBy(x => x.SmallDescription).ToList());

                return new UifJsonResult(true, new { message = message, data = result });
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveAddressTypes);
            }
        }

        /// <summary>
        /// Obtiene tipos de dirección
        /// </summary>
        public void GetListCompanyAddressTypes()
        {
            List<CPEMCV1.CompanyAddressType> scoreTypeDocs = new List<CPEMCV1.CompanyAddressType>();
            if (scoreTypeDocs.Count == 0)
            {
                scoreTypeDocs = DelegateService.uniquePersonServiceV1.GetAllCompanyAddressType().ToList();
            }
        }

        /// <summary>
        /// Genera archivo excel tipos de dirección
        /// </summary>
        /// <returns>Archivo con los tipos de dirección obtenidos</returns>
        public ActionResult GenerateCompanyAddressTypeFileToExport()
        {
            try
            {
                var urlFile = DelegateService.uniquePersonServiceV1.GenerateFileToAddressType();

                return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
            }
        }
        #endregion

        #region Company.PhoneType
        /// <summary>
        /// Obtiene la Vista del tipo de teléfono
        /// </summary>
        /// <returns>Vista del tipo de teléfono</returns>
        public ActionResult CompanyPhoneType()
        {
            Models.CompanyPhoneTypeViewModel model = new Models.CompanyPhoneTypeViewModel();
            return this.View(model);
        }

        /// <summary>
        /// Obtiene la vista con el resultado de la búsqueda de la vista tipo de teléfono 
        /// </summary>
        /// <returns>Resultado de la búsqueda</returns>
        [HttpGet]
        public ActionResult CompanyPhoneTypeAdvancedSearch()
        {
            return this.View();
        }

        /// <summary>
        /// Obtiene la lista de tipos de teléfono
        /// </summary>
        /// <returns>Lista de tipos de teléfono consultadas</returns>
        public ActionResult GetCompanyPhoneTypes()
        {
            try
            {
                List<Models.CompanyPhoneTypeViewModel> companyPhoneTypeList = Models.ModelAssembler.GetCompanyPhoneTypes(DelegateService.uniquePersonServiceV1.GetAllCompanyPhoneType());
                return new UifJsonResult(true, companyPhoneTypeList.OrderBy(x => x.SmallDescription).ToList());
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetPhoneTypes);
            }
        }

        /// <summary>
        /// Realiza los procesos del CRUD para los tipos de teléfono
        /// </summary>
        /// <param name="listAdded"> Lista de teléfonos para ser agregados</param>
        /// <param name="listModified">Lista de teléfonos para ser modificados</param>
        /// <param name="listDeleted">Lista de teléfonos para ser eliminados</param>
        /// <returns>Respuesta con el total de procesos realizados </returns>
        public ActionResult SaveCompanyPhoneTypes(List<Models.CompanyPhoneTypeViewModel> listAdded, List<Models.CompanyPhoneTypeViewModel> listModified, List<Models.CompanyPhoneTypeViewModel> listDeleted)
        {
            try
            {
                ParametrizationResponse<CPEMCV1.CompanyPhoneType> companyPhoneTypeResponse = Services.DelegateService.uniquePersonServiceV1.CreateCompanyPhoneTypes(Models.ModelAssembler.CreateCompanyPhoneTypes(listAdded), Models.ModelAssembler.CreateCompanyPhoneTypes(listModified), Models.ModelAssembler.CreateCompanyPhoneTypes(listDeleted));

                // Formato del mensaje de operaciones realizadas
                string added = string.Empty;
                string edited = string.Empty;
                string deleted = string.Empty;
                string message;

                if (!string.IsNullOrEmpty(companyPhoneTypeResponse.ErrorAdded))
                {
                    companyPhoneTypeResponse.ErrorAdded = App_GlobalResources.LanguagePerson.ResourceManager.GetString(companyPhoneTypeResponse.ErrorAdded);
                }

                if (!string.IsNullOrEmpty(companyPhoneTypeResponse.ErrorModify))
                {
                    companyPhoneTypeResponse.ErrorModify = App_GlobalResources.LanguagePerson.ResourceManager.GetString(companyPhoneTypeResponse.ErrorModify);
                }

                if (!string.IsNullOrEmpty(companyPhoneTypeResponse.ErrorDeleted))
                {
                    companyPhoneTypeResponse.ErrorDeleted = App_GlobalResources.Language.ResourceManager.GetString(companyPhoneTypeResponse.ErrorDeleted);
                }

                if (companyPhoneTypeResponse.TotalAdded > 0)
                {
                    added = App_GlobalResources.LanguagePerson.ReturnSaveAddedPhoneTypes;
                }
                else
                {
                    companyPhoneTypeResponse.TotalAdded = null;
                }

                if (companyPhoneTypeResponse.TotalModify > 0)
                {
                    edited = App_GlobalResources.LanguagePerson.ReturnSaveEditedPhoneTypes;
                }
                else
                {
                    companyPhoneTypeResponse.TotalModify = null;
                }

                if (companyPhoneTypeResponse.TotalDeleted > 0)
                {
                    deleted = App_GlobalResources.LanguagePerson.ReturnSaveDeletedPhoneTypes;
                }
                else
                {
                    companyPhoneTypeResponse.TotalDeleted = null;
                }

                message = string.Format(
                    added + edited + deleted + "{3}{4}{5}",
                    companyPhoneTypeResponse.TotalAdded.ToString() ?? string.Empty,
                    companyPhoneTypeResponse.TotalModify.ToString() ?? string.Empty,
                    companyPhoneTypeResponse.TotalDeleted.ToString() ?? string.Empty,
                    companyPhoneTypeResponse.ErrorAdded ?? string.Empty,
                    companyPhoneTypeResponse.ErrorModify ?? string.Empty,
                    companyPhoneTypeResponse.ErrorDeleted ?? string.Empty);
                var result = Models.ModelAssembler.GetCompanyPhoneTypes(companyPhoneTypeResponse.ReturnedList.OrderBy(x => x.SmallDescription).ToList());

                return new UifJsonResult(true, new { message = message, data = result });
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorSavePhoneTypes);
            }
        }

        /// <summary>
        /// Obtiene tipos de teléfono
        /// </summary>
        public void GetListCompanyPhoneTypes()
        {
            List<CPEMCV1.CompanyPhoneType> scoreTypeDocs = new List<CPEMCV1.CompanyPhoneType>();
            if (scoreTypeDocs.Count == 0)
            {
                scoreTypeDocs = DelegateService.uniquePersonServiceV1.GetAllCompanyPhoneType().ToList();
            }
        }

        /// <summary>
        /// Genera archivo excel tipos de teléfono
        /// </summary>
        /// <returns>Archivo excel con los tipos de teléfono</returns>
        public ActionResult GenerateCompanyPhoneTypeFileToExport()
        {
            try
            {
                var urlFile = DelegateService.uniquePersonServiceV1.GenerateFileToPhoneType();

                return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGeneratingFile);
            }
        }
        #endregion

        #region Person.Sarlaft

        public ActionResult GetPersonSarlaftByIndividualID(int IndividualId)
        {
            try
            {
                List<IndividualSarlaftDTO> SarlfatIndividualId = DelegateService.uniquePersonAplicationService.GetIndividualSarlaft(IndividualId);
                var e = new UifJsonResult(true, SarlfatIndividualId);
                return e;
            }
            catch (Exception)
            {
                return new UifJsonResult(false, "Error al consultar");
            }
        }

        public ActionResult CreatePersonNaturalSarlaft(List<IndividualSarlaftDTO> individualSarlaftDTO)
        {

            IndividualSarlaftDTO individualSarlaftDTOCo = new IndividualSarlaftDTO();
            List<IndividualSarlaftDTO> companyIndividualSarlaft = new List<IndividualSarlaftDTO>();
            try
            {
                foreach (IndividualSarlaftDTO item in individualSarlaftDTO)
                {
                    IndividualSarlaftDTO individualSarlaftDTOs = GetPersonDatesUser(item);
                    companyIndividualSarlaft.Add(individualSarlaftDTOs);

                }
                companyIndividualSarlaft = DelegateService.uniquePersonAplicationService.CreateIndividualSarlaft(companyIndividualSarlaft);
                return new UifJsonResult(true, companyIndividualSarlaft);
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorCreateSarlaft);
            }


        }

        public IndividualSarlaftDTO GetPersonDatesUser(IndividualSarlaftDTO individualSarlaftDTO)
        {
            ACM.Branch BranchesByUser;
            BranchesByUser = DelegateService.uniqueUserService.GetDefaultBranchesByUserId(SessionHelper.GetUserId());
            individualSarlaftDTO.InterviewerPlace = BranchesByUser.Description;
            individualSarlaftDTO.RegistrationDate = DelegateService.commonService.GetDate();
            individualSarlaftDTO.UserId = SessionHelper.GetUserId();
            individualSarlaftDTO.BranchCode = BranchesByUser.Id;
            individualSarlaftDTO.AuthorizedBy = User.Identity.Name;
            individualSarlaftDTO.VerifyingEmployee = User.Identity.Name;
            individualSarlaftDTO.InterviewerName = User.Identity.Name;
            individualSarlaftDTO.InterviewResultCode = 1;
            individualSarlaftDTO.InternationalOperations = true;
            individualSarlaftDTO.PendingEvents = false;
            individualSarlaftDTO.Year = DelegateService.commonService.GetDate().Year;
            //sarlaft.EconomicActivity = new EconomicActivity() { Id = person.EconomicActivity.Id };
            return individualSarlaftDTO;
        }
        #endregion



        #region Supplier

        public ActionResult CreateSupplier(ProviderDTO provider)
        {
            try
            {
                ProviderDTO providerCreate = new ProviderDTO();
                provider.UserId = SessionHelper.GetUserId();

                if (provider.Id > 0)
                {
                    provider.ModificationDate = DateTime.Now;
                    providerCreate = DelegateService.uniquePersonAplicationService.UpdateAplicationSupplier(provider);
                }
                else
                {
                    providerCreate = DelegateService.uniquePersonAplicationService.CreateAplicationSupplier(provider);
                }

                return new UifJsonResult(true, providerCreate);

            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorCreateProvider);
            }

        }


        [HttpPost]
        public JsonResult GetSupplierByIndividualId(int IndividualId)
        {
            try
            {
                ProviderDTO providerDTO = DelegateService.uniquePersonAplicationService.GetAplicationSupplierByIndividualId(IndividualId);
                return new UifJsonResult(true, providerDTO);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, "Error al crear el provedor." + ex);
            }

        }

        [HttpPost]
        public JsonResult GetSupplierProfiles(int suppilierTypeId)
        {
            try
            {
                List<SupplierProfileDTO> supplierProfileDTO = DelegateService.uniquePersonAplicationService.GetAplicationSupplierProfiles(suppilierTypeId);
                return new UifJsonResult(true, supplierProfileDTO);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, "Error obteniendo SupplierProfiles." + ex);
            }

        }

        [HttpPost]
        public JsonResult GetGroupSupplier()
        {
            try
            {
                List<GroupSupplierDTO> groupSupplierDTO = DelegateService.uniquePersonAplicationService.GetAplicationGroupSupplierDTO();
                return new UifJsonResult(true, groupSupplierDTO);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, "Error obteniendo SupplierProfiles." + ex);
            }

        }

        [HttpPost]
        public JsonResult GetAccountingConcepts()
        {
            try
            {
                List<AccountingConceptDTO> supplierProfileDTO = DelegateService.uniquePersonAplicationService.GetAplicationAccountingConcepts();
                return new UifJsonResult(true, supplierProfileDTO);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, "Error obteniendo SupplierProfiles." + ex);
            }

        }

        [HttpPost]
        public JsonResult GetSupplierAccountingConceptsBySupplierId(int SupplierId)
        {
            try
            {
                List<SupplierAccountingConceptDTO> supplierAccountingConceptDTO = DelegateService.uniquePersonAplicationService.GetAplicationSupplierAccountingConceptsBySupplierId(SupplierId);
                return new UifJsonResult(true, supplierAccountingConceptDTO);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, "Error obteniendo SupplierProfiles." + ex);
            }

        }

        public ActionResult GetSupplierTypes()
        {
            try
            {
                List<SupplierTypeDTO> SupplierTypes = new List<SupplierTypeDTO>();
                SupplierTypes = DelegateService.uniquePersonAplicationService.GetAplicationSupplierTypes();
                return new UifJsonResult(true, SupplierTypes);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetProviderTypes);
            }

        }

        public ActionResult GetSupplierDeclinedType()
        {
            try
            {
                List<SupplierDeclinedTypeDTO> supplierDeclinedType = new List<SupplierDeclinedTypeDTO>();
                supplierDeclinedType = DelegateService.uniquePersonAplicationService.GetAplicationSupplierDeclinedTypes();
                return new UifJsonResult(true, supplierDeclinedType);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetProviderTypes);
            }

        }


        #endregion Supplier

        #region AgentByIndividualId
        public UifJsonResult GetAgenAgencytByIndividualId(int invidualId)
        {
            try
            {
                List<AgencyDTO> agencyDTO = DelegateService.uniquePersonAplicationService.GetAplicationAgencyByInvidualID(invidualId);

                if (agencyDTO != null)
                {
                    return new UifJsonResult(true, agencyDTO);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorNotAgents);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetAgent);
            }
        }
        #endregion

        #region LabourPerson

        public ActionResult GetLabourPersonByIndividuald(int IndividualId)
        {
            try
            {
                PersonInformationAndLabourDTO LabourPersonDTO = DelegateService.uniquePersonAplicationService.GetApplicationLabourPersonByIndividualId(IndividualId);

                return new UifJsonResult(true, LabourPersonDTO);
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.Error);
            }

        }


        public ActionResult SaveLabourPerson(PersonInformationAndLabourDTO labourperson)
        {
            try
            {
                PersonInformationAndLabourDTO labourpersonDTO = new PersonInformationAndLabourDTO();
                labourperson.UserId = SessionHelper.GetUserId();

                if (labourperson.Id > 0)
                {
                    labourpersonDTO = DelegateService.uniquePersonAplicationService.UpdateApplicationLabourPerson(labourperson);
                }
                else
                {
                    labourpersonDTO = DelegateService.uniquePersonAplicationService.CreateApplicationLabourPerson(labourperson, labourperson.IndividualId);
                }

                return new UifJsonResult(true, labourpersonDTO);

            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorCreatePerson);
            }

        }
        #endregion LabourPerson

        #region Consortium
        public UifJsonResult GetConsortiumIndividualId(int individual)
        {
            try
            {
                List<ConsorciatedDTO> agencyDTO = DelegateService.uniquePersonAplicationService.GetConsortiumByIndividualId(individual);

                if (agencyDTO != null)
                {
                    return new UifJsonResult(true, agencyDTO);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorNotAgents);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetAgent);
            }
        }
        public JsonResult CreateConsortium(List<ConsorciatedDTO> consorciateds, int individualId)
        {
            try
            {
                List<ConsorciatedDTO> consorciatedDTO = new List<ConsorciatedDTO>();
                consorciateds.First().UserId = SessionHelper.GetUserId();
                consorciatedDTO.AddRange(DelegateService.uniquePersonAplicationService.CreateConsortium(consorciateds, individualId));
                return new UifJsonResult(true, consorciatedDTO);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorCreateConsortium);
            }
        }

        public JsonResult GetConsortiumInsuredCodeAndIndividualID(int InsureCode, int IndividualId)
        {
            try
            {
                ConsorciatedDTO consorciatedDTO = new ConsorciatedDTO();
                //consorciatedDTO = DelegateService.uniquePersonAplicationService.GetConsortiumInsuredCodeAndIndividualID(InsureCode, IndividualId);
                return new UifJsonResult(true, consorciatedDTO);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetConsortiumInsuredCodeAndIndividualID);
            }
        }

        public JsonResult GetConsortiumInsuredCode(int InsureCode)
        {
            try
            {
                List<ConsorciatedDTO> consorciatedDTOs = new List<ConsorciatedDTO>();
                //consorciatedDTOs = DelegateService.uniquePersonAplicationService.GetConsortiumInsuredCode(InsureCode);
                return new UifJsonResult(true, consorciatedDTOs);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetConsortiumInsuredCode);
            }
        }



        public UifJsonResult DeleteConsortium(ConsorciatedDTO InsuredIdDto)
        {
            try
            {
                bool result = DelegateService.uniquePersonAplicationService.DeleteConsortium(InsuredIdDto);
                return new UifJsonResult(true, result);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorDeleteConsortium);
            }
        }

        #endregion Consortium

        #region IndividualRole
        /// <summary>
        /// Valida los roles del usuario
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTypeRolByIndividual(int IndividualId)
        {
            try
            {
                List<IndividualRoleDTO> individualRoleDTO = DelegateService.uniquePersonAplicationService.GetAplicationIndividualRoleByIndividualId(IndividualId);
                return new UifJsonResult(true, individualRoleDTO);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, "Error GetTypeRolByIndividual." + ex);
            }
        }
        #endregion


        public string GetStatesByCountryIdByDescriptionByStateId(int CountryId, int stateId)
        {
            try
            {
                var stateById = countries.Where(x => x.Id == CountryId).ToList()[0]
                    .States.Where(y => y.Id == (stateId)).Select(x => new { Id = x.Id, Description = x.Description }).ToList().FirstOrDefault().Description;

                if (stateById != null)
                {
                    return stateById;
                }
                return stateById;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public string GetCitiesByCountryIdByStateIdById(int CountryId, int StateId, int CityId)
        {
            try
            {
                var citieById = countries.Where(x => x.Id == CountryId).ToList()[0]
                    .States.Where(y => y.Id == StateId).ToList()[0]
                    .Cities.Where(z => z.Id == (CityId)).Select(x => new { Id = x.Id, Description = x.Description }).ToList().FirstOrDefault().Description;

                if (citieById != null)
                {
                    return citieById;
                }
                return citieById;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public string GetCountryByCountryIdReturnDescription(string country_Id)
        {
            int countryId = 0;
            try
            {
                if (country_Id != "")
                {
                    countryId = Convert.ToInt32(country_Id);
                }

                ACM.Country country = (from c in countries where c.Id == countryId select c).FirstOrDefault();

                return country.Description;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        #region ThirdPerson
        public ActionResult GetThirdDeclinedType()
        {
            try
            {
                List<CPEMCV1.CompanyThirdDeclinedType> thirdDeclinedTypes = DelegateService.uniquePersonServiceV1.GetAllThirdDeclinedTypes();
                return new UifJsonResult(true, thirdDeclinedTypes.OrderBy(x => x.Description));
            }
            catch
            {

                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetThirdDeclinedTypes);
            }

        }

        /// <summary>
        /// Crear tercero
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public ActionResult CreateThird(ThirdPartyDTO thirdParty)
        {
            try
            {
                ThirdPartyDTO thirdPartyCreate = new ThirdPartyDTO();
                thirdParty.UserId = SessionHelper.GetUserId();

                if (thirdParty.Id > 0)
                {
                    thirdParty.ModificationDate = DateTime.Now;
                    thirdPartyCreate = DelegateService.uniquePersonAplicationService.UpdateAplicationThird(thirdParty);
                }
                else
                {
                    thirdPartyCreate = DelegateService.uniquePersonAplicationService.CreateAplicationThird(thirdParty);
                }

                return new UifJsonResult(true, thirdPartyCreate);

            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorCreateProvider);
            }

        }

        [HttpPost]
        public JsonResult GetThirdByIndividualId(int IndividualId)
        {
            try
            {
                ThirdPartyDTO thirdDTO = DelegateService.uniquePersonAplicationService.GetAplicationThirdByIndividualId(IndividualId);
                return new UifJsonResult(true, thirdDTO);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, "Error al crear tercero." + ex);
            }

        }
        #endregion
        #region BusinessName
        public JsonResult CreateBusinessName(List<CompanyNameDTO> listCompanyName)
        {
            try
            {
                if (listCompanyName != null)
                {
                    List<CompanyNameDTO> companyNameDTO = new List<CompanyNameDTO>();

                    listCompanyName.First().UserId = SessionHelper.GetUserId();
                    companyNameDTO.AddRange(DelegateService.uniquePersonAplicationService.CreateBusinessName(listCompanyName));

                    return new UifJsonResult(true, companyNameDTO);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorNullTax);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorCreateTax);
            }
        }

        public UifJsonResult GetCompanyBusinnesByIndividualId(int individualId)
        {
            try
            {
                List<CompanyNameDTO> CompanyBusiness = DelegateService.uniquePersonAplicationService.GetCompanyBusinessByIndividualId(individualId);
                return new UifJsonResult(true, CompanyBusiness);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorCompanyExists);
            }
        }
        //bankTransfers BankTransfers
        public JsonResult CreateBankTransfers(List<BankTransfersDTO> listBankTransfers)
        {
            try
            {
                if (listBankTransfers != null)
                {
                    List<BankTransfersDTO> companyBankTransfersDTO = new List<BankTransfersDTO>();
                    var updateBankTransfers = listBankTransfers.Where(m => m.Id > 0).ToList();
                    var createBankTransfers = listBankTransfers.Where(m => m.Id == 0).ToList();
                    listBankTransfers.First().UserId = SessionHelper.GetUserId();
                    if (updateBankTransfers.Count > 0)
                    {
                        companyBankTransfersDTO = DelegateService.uniquePersonAplicationService.UpdateAplicationBankTransfers(updateBankTransfers);
                    }
                    if (createBankTransfers.Count > 0)
                    {
                        companyBankTransfersDTO = DelegateService.uniquePersonAplicationService.CreateBankTransfers(createBankTransfers);
                    }
                    return new UifJsonResult(true, companyBankTransfersDTO);

                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorNullTax);
                }


            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorCreateTax);
            }
        }

        public JsonResult UpdateBankTransfers(List<BankTransfersDTO> listBankTransfers)
        {
            try
            {
                if (listBankTransfers != null)
                {

                    List<BankTransfersDTO> companyBankTransfersDTO = new List<BankTransfersDTO>();
                    companyBankTransfersDTO = DelegateService.uniquePersonAplicationService.UpdateAplicationBankTransfers(listBankTransfers);
                    return new UifJsonResult(true, companyBankTransfersDTO);

                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorNullTax);
                }


            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorCreateTax);
            }
        }

        public UifJsonResult GetCompanyBankTransfersByIndividualId(int individualId)
        {
            try
            {
                List<BankTransfersDTO> CompanyBank = DelegateService.uniquePersonAplicationService.GetCompanyBankTransfersByIndividualId(individualId);
                return new UifJsonResult(true, CompanyBank);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorCompanyExists);
            }
        }
        #endregion


        #region ElectronicBilling

        public JsonResult CreateElectronicBilling(List<InsuredFiscalResponsibilityDTO> listInsuredFiscalResponsibility, bool responsibleForVat, bool electronicBiller)
        {
            try
            {
                if (listInsuredFiscalResponsibility != null)
                {
                    List<InsuredFiscalResponsibilityDTO> companyInsuredFiscalResponsibilityDTO = new List<InsuredFiscalResponsibilityDTO>();
                    
                    InsuredDTO insured = DelegateService.uniquePersonAplicationService.GetAplicationInsuredElectronicBillingByIndividualId(listInsuredFiscalResponsibility[0].IndividualId);
                    if (insured != null)
                    {
                        foreach (InsuredFiscalResponsibilityDTO fiscalResponsibility in listInsuredFiscalResponsibility)
                        {
                            fiscalResponsibility.InsuredCode = insured.Id;
                        }
                        insured.RegimeType = responsibleForVat;
                        insured.ElectronicBiller = electronicBiller;
                        DelegateService.uniquePersonAplicationService.UpdateAplicationInsuredElectronicBilling(insured);
                    
                        var updateFiscalRespondibility = listInsuredFiscalResponsibility.Where(m => m.Id > 0).ToList();
                        var createFiscalResponsibilities = listInsuredFiscalResponsibility.Where(m => m.Id == 0).ToList();
                        if (updateFiscalRespondibility.Count > 0)
                        {
                            companyInsuredFiscalResponsibilityDTO = DelegateService.uniquePersonAplicationService.UpdateAplicationFiscalRespondibility(updateFiscalRespondibility);
                        }
                        if (createFiscalResponsibilities.Count > 0)
                        {
                            companyInsuredFiscalResponsibilityDTO = DelegateService.uniquePersonAplicationService.CreateIndividualFiscalResponsibility(createFiscalResponsibilities);
                        }
                       
                    }
                    else
                    {
                        companyInsuredFiscalResponsibilityDTO = new List<InsuredFiscalResponsibilityDTO>();
                        return new UifJsonResult(false, App_GlobalResources.LanguagePerson.PersonIsNotInsured);
                       
                    }
                    return new UifJsonResult(true, companyInsuredFiscalResponsibilityDTO);

                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorElectronicBilling);
                }


            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorElectronicBilling);
            }
        }


        public UifJsonResult DeleteFiscalResponsibility(InsuredFiscalResponsibilityDTO fiscalDto)
        {
            try
            {
                bool result = DelegateService.uniquePersonAplicationService.DeleteFiscalResponsibility(fiscalDto);
                return new UifJsonResult(result, null);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorDeleteFiscalResponsibility);
            }
        }


        public UifJsonResult GetCompanyElectronicBillingByIndividualId(int individualId)
        {
            try
            {
                List<InsuredFiscalResponsibilityDTO> CompanyFiscal = DelegateService.uniquePersonAplicationService.GetCompanyFiscalResponsibilityByIndividualId(individualId);
                foreach (InsuredFiscalResponsibilityDTO fiscal in CompanyFiscal)
                {
                    fiscal.IndividualId = individualId;
                    fiscal.FiscalResponsibilityDescription = fiscal.FiscalResponsibilityDescription + " (" + fiscal.Code + ")";
                }
                return new UifJsonResult(true, CompanyFiscal);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorCompanyExists);
            }
        }


        public ActionResult GetFiscalResponsibilityById(int Id)
        {
            try
            {
                FiscalResponsibility fiscal = DelegateService.uniquePersonServiceCore.GetFiscalResponsibilityById(Id);
                fiscal.Description = fiscal.Description + " (" + fiscal.Code + ")";
                return new UifJsonResult(true, fiscal);
            }
            catch
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetBanks);
            }
        }
        #endregion ElectronicBilling

        public UifJsonResult SaveOperatingQuotaByEvent(List<OperatingQuotaDTO> operatingQuotaDTOs, List<OperatingQuotaEventDTO> operatingQuotaEventDTOs)
        {
            try
            {
                if (operatingQuotaDTOs.Count() > 0 && operatingQuotaEventDTOs.Count() > 0)
                {
                    operatingQuotaDTOs.ForEach(x => x.UserId = SessionHelper.GetUserId());

                    operatingQuotaDTOs = DelegateService.uniquePersonAplicationService.CreateOperatingQuota(operatingQuotaDTOs, operatingQuotaEventDTOs);

                    return new UifJsonResult(true, operatingQuotaDTOs);
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorCreateOperatingQuota);
            }
        }

        public UifJsonResult CreateConsortiumEvent(CONS.ConsortiumEventDTO consortiumEventDTO)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.consortiumService.CreateConsortiumEvent(consortiumEventDTO));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorCreateConsortium);
            }
        }

        public UifJsonResult AssigendIndividualToConsotiumEvent(List<CONS.ConsortiumEventDTO> consortiumEventDTOs)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.consortiumService.AssigendIndividualToConsotium(consortiumEventDTOs));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorCreateConsortium);
            }
        }

        public UifJsonResult GetCumuloCupoConsortiumEventByConsortiumIdByLineBusinessId(int IndividualId, int LineBusinessId,bool? Endorsement,int Id)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.consortiumService.GetCumuloCupoConsortiumEventByConsortiumIdByLineBusinessId(IndividualId, LineBusinessId, Endorsement,Id));
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex);
            }
        }

        public JsonResult GetCities(int countryId)
        {
            Country country = new Country();
            country.Id = countryId;
            List<City> cities = DelegateService.commonService.GetCitiesByCountry(country);
            return new UifJsonResult(true, cities.OrderBy(x => x.Description));
        }


        /// <summary>
        /// GetAccountTypes
        /// Obtiene los tipos de cuentas
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetAccountTypes()
        {
            List<CompanyPaymentAccountType> accountTypes = DelegateService.uniquePersonServiceV1.getCompanyPaymentAccountTypes();
            return new UifJsonResult(true, accountTypes.OrderBy(x => x.Description));
        }

        public JsonResult loadAccountTypes()
        {
            return new UifJsonResult(true, DelegateService.uniquePersonServiceV1.getCompanyAccountType().OrderBy(x => x.Description));
        }

        public ActionResult EditBasicInfo()
        {
            return View();
        }

        public UifJsonResult GetPersonOrCompanyByDescription(string description, InsuredSearchType insuredSearchType, CustomerType? customerType, TemporalType temporalType)
        {
            try
            {
                List<Holder> holders = DelegateService.underwritingService.GetPersonOrCompanyByDescription(description, customerType);
                if (holders != null && holders.Any())
                {
                    return new UifJsonResult(true, holders);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorHolderNotFound);
                }


            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchPolicyholder);
            }
        }

        public UifJsonResult GetHoldersByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType, TemporalType temporalType)
        {
            try
            {
                List<Holder> holders = DelegateService.underwritingService.GetHoldersByDocument(description, customerType);
                if (holders != null && holders.Any())
                {
                    return new UifJsonResult(true, holders);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorHolderNotFound);
                }


            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchPolicyholder);
            }
        }

        public UifJsonResult UpdateApplicationPersonBasicInfo(PersonDTO person)
        {
            try
            {
                CPEMCV1.CompanyPerson personModel = ASEPER.ModelAssembler.CreatePerson(person);
                var personDTOUpdate = DelegateService.uniquePersonServiceV1.UpdateApplicationPersonBasicInfo(personModel);
                return new UifJsonResult(true, personDTOUpdate);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorCreatePerson);
            }
        }

        public UifJsonResult UpdateApplicationCompanyBasicData(CompanyDTO company)
        {
            try
            {
                CPEMCV1.CompanyCompany companyModel = ASEPER.ModelAssembler.CreateCompany(company);
                var personDTOUpdate = DelegateService.uniquePersonServiceV1.UpdateApplicationCompanyBasicInfo(companyModel);
                return new UifJsonResult(true, personDTOUpdate);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorCreatePerson);
            }
        }

        public UifJsonResult GetPersonByDocumentByDocumentType(string document, int documentType)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.uniquePersonServiceCore.GetPersonByDocumentByDocumentType(document, documentType));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetPerson);
            }

        }

        public UifJsonResult GetCompanyByDocumentByDocumentType(string document, int documentType)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.uniquePersonServiceCore.GetCompanyByDocumentByDocumentType(document, documentType));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetCompany);
            }

        }

        public UifJsonResult ValidateListRiskPerson(string documentNumber, string fullName)
        {
            try
            {
                List<ListRiskMatchDTO> matchRiskList = DelegateService.uniquePersonListRiskApplication.ValidateListRiskPerson(documentNumber, fullName, null);
                if (matchRiskList.Any())
                {
                    string menssage = App_GlobalResources.LanguagePerson.MatchesRestrictiveLists;
                    menssage += " " + string.Join(", ", matchRiskList.Select(x => x.listType).Distinct());

                    return new UifJsonResult(true, menssage);
                }
                return new UifJsonResult(true, null);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorConsultingRiskLists);
            }

        }

        public ActionResult GetParameterEmployee()
        {
            try
            {
                ACM.Parameter updateParameter = new ACM.Parameter();
                ACM.Parameter parameter = DelegateService.commonService.GetParameterByDescription("Employee");
                updateParameter = parameter;
                updateParameter.NumberParameter = updateParameter.NumberParameter + 1;
                updateParameter.Id = parameter.Id;
                DelegateService.commonService.UpdateParameter(updateParameter);
                return new UifJsonResult(true, parameter);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorSearchParameter);
            }

        }

        public UifJsonResult GetUserAssignedConsortiumByparameterFutureSocietyByuserId()
        {
            try
            {
                int userId = SessionHelper.GetUserId();
                return new UifJsonResult(true, DelegateService.uniquePersonServiceV1.GetUserAssignedConsortiumByparameterFutureSocietyByuserId((int)parameterType.FutureSociety, userId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetCompany);
            }

        }

        #region AgentAgency

        public UifJsonResult GetAgenAgencytByAgentCodeAndType(int agentCode, int agentType)
        {
            try 
            {
                CompanyAgency Agency = DelegateService.uniquePersonServiceV1.GetCompanyAgencyByAgentCodeAgentTypeCode(agentCode, agentType);

                if (Agency == null)
                {
                    return new UifJsonResult(true, Agency);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorAgentCodeAndTypeCode);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorAgentCodeAndTypeCode);
            }
        }
        #endregion AgentAgency


        public ActionResult GetFiscalResponsibility()
        {
            try
            {
                
                List<FiscalResponsibilityDTO> fiscalResponsibilityDTO = DelegateService.uniquePersonAplicationService.GetAplicationCompanyFiscalResponsibility();
                fiscalResponsibilityDTO.ForEach(x => x.Description = x.Description + " (" + x.Code + ")");
                return new UifJsonResult(true, fiscalResponsibilityDTO);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetCompanyTypes);
            }

        }
        
    }
}
