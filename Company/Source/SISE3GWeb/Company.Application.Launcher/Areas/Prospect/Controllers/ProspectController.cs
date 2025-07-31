// -----------------------------------------------------------------------
// <copyright file="PersonController.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Desconocido</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Framework.UIF.Web.Areas.Prospect.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using System.Collections.Generic;
    using Sistran.Core.Application.CommonService.Models;
    using Sistran.Core.Application.UniquePersonService.Enums;
    using Sistran.Core.Framework.UIF.Web.Areas.Person.Enums;
    using Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Models;
    using Sistran.Core.Framework.UIF.Web.Helpers;
    using Sistran.Core.Framework.UIF.Web.Models;
    using Sistran.Core.Framework.UIF.Web.Services;
    using Sistran.Core.Framework.UIF2.Controls.UifSelect;
    using Sistran.Core.Framework.UIF2.Controls.UifTable;
    using CPEM = Sistran.Core.Application.UniquePersonService.Models;
    using CPEMC = Sistran.Company.Application.UniquePersonServices.Models;
    using EnPerson = Sistran.Core.Application.UniquePersonService.Enums;
    using Sistran.Core.Application.UniquePersonService.Models;
    using Sistran.Core.Application.CommonService.Enums;
    using TaxModel = Sistran.Core.Application.TaxServices.Models;
    using Sistran.Company.Application.UniquePersonServices.Models;
    using UniquePersonModel = Sistran.Core.Application.UniquePersonServiceIndividual.Models;
    using Sistran.Core.Application.UnderwritingServices.Models;
    using Sistran.Core.Services.UtilitiesServices.Models;
    using AutoMapper;
    using Sistran.Core.Application.CommonService.Models.Base;
    using Sistran.Core.Application.UniquePersonService.Models.Base;
    using Sistran.Core.Framework.UIF.Web.Areas.Person.Models;
    using Sistran.Company.Application.ModelServices.Models;
    using Sistran.Core.Application.ModelServices.Enums;
    using Sistran.Company.Application.ModelServices.Models.Param;
    using Sistran.Company.Application.UniquePersonAplicationServices.DTOs;
    using STATUS = Sistran.Company.Application.ModelServices.Enums.StatusTypeService;
    using CPEMV1 = Sistran.Core.Application.UniquePersonService.V1.Models;
    using CPEMCV1 = Sistran.Company.Application.UniquePersonServices.V1.Models;
  

    /// <summary>
    /// Acciones de person
    /// </summary>
    public class ProspectController : Controller
    {
        private static List<Country> countries = new List<Country>();
        private static List<State> states = new List<State>();
        private static List<Country> countriesCommon = new List<Country>();
        private static List<CPEM.Agency> agencies = new List<CPEM.Agency>();
        private static List<CPEMV1.Agent> agents = new List<CPEMV1.Agent>();
        private static List<CPEM.EconomicActivity> economicActivities = new List<CPEM.EconomicActivity>();
        private static List<CPEM.PhoneType> phoneTypes = new List<CPEM.PhoneType>();
        private static List<CPEM.AddressType> addressTypes = new List<CPEM.AddressType>();
        private static List<Currency> currencies = new List<Currency>();
        private static List<HouseType> houseType = new List<HouseType>();
        private static List<CPEM.EmailType> emailTypes = new List<CPEM.EmailType>();
        private static List<CPEMV1.PaymentAccountType> paymentAccountTypes = new List<CPEMV1.PaymentAccountType>();
        private static List<PaymentMethod> paymentMethods = new List<PaymentMethod>();
        private static List<CPEM.AssociationType> associationTypes = new List<CPEM.AssociationType>();
        private static List<CPEM.CompanyType> companyTypes = new List<CPEM.CompanyType>();
        private static List<Sistran.Core.Application.CommonService.Models.Parameter> parameters = new List<Sistran.Core.Application.CommonService.Models.Parameter>();
        private static List<CPEM.ProviderDeclinedType> providerDeclinedTypes = new List<CPEM.ProviderDeclinedType>();
        private static List<CPEMC.OthersDeclinedTypes> otrhesDeclinedTypes = new List<CPEMC.OthersDeclinedTypes>();
        private static List<CPEM.ProviderType> providerTypes = new List<CPEM.ProviderType>();
        private static List<CPEMV1.OriginType> originType = new List<CPEMV1.OriginType>();
        private static List<PaymentConcept> paymentConcept = new List<PaymentConcept>();
        private static List<CPEMV1.Speciality> specialties = new List<CPEMV1.Speciality>();
        private static List<TaxModel.Tax> tax = new List<TaxModel.Tax>();
        private static Dictionary<int, List<DefaultValue>> defaultValues = new Dictionary<int, List<DefaultValue>>();
        private static List<CPEM.CommissionAgent> ComissionAgent = new List<CPEM.CommissionAgent>();
        private static List<CPEM.Base.BaseEmployeePerson> employeePersons = new List<CPEM.Base.BaseEmployeePerson>();
        private static string defaultCountryId = DelegateService.commonService.GetParameterByParameterId(2135).NumberParameter.ToString();
        private static GenericModelsServicesQueryModel addressCompany = new GenericModelsServicesQueryModel();

        public ActionResult GetProspectByDocumenNumberOrDescription(string description, Sistran.Core.Services.UtilitiesServices.Enums.InsuredSearchType insuredSearchType)
        {
            try
            {
                List<CPEMV1.Prospect> prospects = DelegateService.uniquePersonServiceV1.GetProspectByDescription(description, insuredSearchType);
                return new UifJsonResult(true, prospects);

            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryProspect);
            }
        }
        public ActionResult CreateProspectPersonNatural(ProspectPersonNaturalDTO prospectModel)
        {
            try
            {
                ProspectPersonNaturalDTO prospectPersonNatural = DelegateService.uniquePersonAplicationService.CreateProspectPersonNatural(prospectModel);
                if (prospectPersonNatural == null)
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorExistNaturalProspectIdentification);
                }
                else
                {
                    return new UifJsonResult(true, prospectPersonNatural);
                }
            }
            catch
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateProspect);
            }

        }
        public ActionResult CreateProspectPersonLegal(ProspectLegalDTO prospectLegal)
        {
            try
            {
                ProspectLegalDTO prospectPersonLegal = DelegateService.uniquePersonAplicationService.CreateProspectLegal(prospectLegal);

                if (prospectPersonLegal == null)
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorExistNaturalProspectIdentification);
                }
                else
                {
                    return new UifJsonResult(true, prospectPersonLegal);
                }
            }
            catch
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateProspect);
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
                    return new UifJsonResult(false, App_GlobalResources.Language.MessageNotFoundProspects);
                }
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetProspect);
            }
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

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetProspectLegal);
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
                        return new UifJsonResult(false, App_GlobalResources.Language.MessageNotFoundProspects);
                    }
                }
                else
                {
                    ProspectLegalDTO prospectLegal = DelegateService.uniquePersonAplicationService.GetProspectLegalByDocumentNumber(documentNum);
                    if (prospectLegal != null)
                    {
                        return new UifJsonResult(true, prospectLegal);
                    }
                    else
                    {
                        return new UifJsonResult(false, App_GlobalResources.Language.MessageNotFoundProspects);
                    }
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetProspect);
            }
        }
        public ActionResult GetDocumentType(int typeDocument)
        {
            try
            {
                List<CPEM.DocumentType> documentTypes = DelegateService.uniquePersonService.GetDocumentTypes(typeDocument);
                return new UifJsonResult(true, documentTypes.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetDocumentTypes);
            }

        }
        public JsonResult GetDaneCodeByQuery(string query)
        {
            try
            {
                List<City> city = (from c in countriesCommon
                                   from s in c.States
                                   from cy in s.Cities
                                   select cy).ToList();

                var dataFiltered = city.Where(d => d.DANECode != null && d.DANECode.Contains(query));

                if (!String.IsNullOrEmpty(query))
                    return Json(dataFiltered, JsonRequestBehavior.AllowGet);
                else
                    return Json(city, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public ActionResult GetAddressesTypes()
        {
            try
            {
                if (addressTypes.Count == 0)
                {
                    addressTypes = DelegateService.uniquePersonService.GetAddressesTypes();
                }
                return new UifJsonResult(true, addressTypes.OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorAddressesTypes);
            }
        }
        public ActionResult GetMaritalStatus()
        {
            try
            {
                List<CPEM.MaritalStatus> maritalStatus = new List<CPEM.MaritalStatus>();
                maritalStatus = DelegateService.uniquePersonService.GetMaritalStatus();

                return new UifJsonResult(true, maritalStatus.OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetMaritalStatus);
            }
        }
        public ActionResult GetGenderTypes()
        {
            try
            {
                return new UifJsonResult(true, EnumsHelper.GetItems<EnPerson.GenderType>());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetGenderTypes);
            }
        }
        public ActionResult GetCountries()
        {
            try
            {
                if (countries.Count == 0)
                {
                    countries = DelegateService.commonService.GetCountries();
                    countriesCommon = countries;

                }
                var list = countries.Select(item => new { item.Id, item.Description }).ToList();
                return new UifJsonResult(true, list.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchCountry);
            }

        }
        [HttpPost]
        public ActionResult GetDaneCodeByCountryIdByStateIdByCityId(int countryId, int stateId, int cityId)
        {
            try
            {
                List<Country> country = DelegateService.commonService.GetCountries();
                string daneCode = (from c in country
                                   from s in c.States
                                   from cy in s.Cities
                                   where c.Id == countryId && s.Id == stateId && cy.Id == cityId
                                   select cy.DANECode).FirstOrDefault();

                if (!string.IsNullOrEmpty(daneCode))
                {
                    return new UifJsonResult(true, daneCode);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorDaneCodeEmpty);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorConsultingCodeDane);
            }
        }
        public ActionResult GetCountriesByDescription(string Description)
        {
            GetCountries();
            try
            {
                var listCountries = countries.Where(x => x.Description.Contains(Description)).Select(x => new { Id = x.Id, Description = x.Description }).ToList();

                if (listCountries.Count != 0)
                {
                    return new UifJsonResult(true, listCountries);
                }

                return new UifJsonResult(false, App_GlobalResources.Language.MessageNotFoundCountries);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchCountry);
            }
        }
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

                return new UifJsonResult(false, App_GlobalResources.Language.MessageNotFoundStates);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorConsultingDepartments);
            }
        }
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
                return new UifJsonResult(false, App_GlobalResources.Language.MessageNotFoundCities);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryingCities);
            }
        }
        public ActionResult GetCountryAndStateAndCityByDaneCode(int CountryId, string DaneCode)
        {
            try
            {
                var country = countriesCommon.Where(x => x.Id == CountryId).ToList()[0];

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

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorDaneCode);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorConsultingCodeDane);
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

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetProspectNatural);
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
        public PartialViewResult ProspectusLegal()
        {
            return PartialView();
        }
        public PartialViewResult ProspectusNatural()
        {
            return PartialView();
        }
    }
}
