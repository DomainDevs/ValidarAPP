using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using UUMDL = Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Guarantees.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using UPV1 = Sistran.Core.Application.UniquePersonService.V1.Models;
using ACM = Sistran.Core.Application.CommonService.Models;

namespace Sistran.Core.Framework.UIF.Web.Areas.Guarantees.Controllers
{
    using Company.Application.UniquePersonAplicationServices.DTOs;

    public class GuaranteesController : Controller
    {
        private static List<Country> countries = new List<Country>();
        private static List<Currency> currencies = new List<Currency>();
        private static List<Branch> branches = new List<Branch>();
        private static List<UPV1.GuaranteeStatus> guaranteeStatus = new List<UPV1.GuaranteeStatus>();
        private static List<UUMDL.ProfileGuaranteeStatus> ProfileGuaranteeStatus = new List<UUMDL.ProfileGuaranteeStatus>();
        private static List<Prefix> prefix = new List<Prefix>();
        private static List<UPV1.MeasurementType> measurementType = new List<UPV1.MeasurementType>();
        private static List<UPV1.PromissoryNoteType> promissoryNoteType = new List<UPV1.PromissoryNoteType>();
        private static List<Guarantee> guarantees = new List<Guarantee>();
        private static List<UPV1.CoInsuranceCompany> coInsuranceCompanies = new List<UPV1.CoInsuranceCompany>();
        private static List<UPV1.GuaranteeRequiredDocument> guaranteeRequiredDocument = new List<UPV1.GuaranteeRequiredDocument>();
        private static List<UPV1.AssetType> assetType = new List<UPV1.AssetType>();

        //private static List<HardRiskType> hardRiskType = new List<HardRiskType>();

        private static List<UPV1.Guarantee> listGuaranteeOld = new List<UPV1.Guarantee>();
        private static List<UPV1.GuaranteeType> guaranteesTypes = new List<UPV1.GuaranteeType>();
        private static List<UPV1.Guarantee> guaranteesGlb = new List<UPV1.Guarantee>();

        ///// <summary>
        ///// Contructor: Llamado de la vista inicial.
        ///// </summary>
        ///// <returns>Vista inicial de Cobertura aliada</returns>
        //public ActionResult Guarantees()
        //{
        //    return this.View();
        //}

        public ActionResult SearchInsured()
        {
            return this.View();
        }
        #region Obtener
        /// <summary>
        ///     Consulta las contragarantías asociadas a un afianzado
        /// </summary>
        /// <param name="individualId">Id del afianzado</param>
        /// <returns> Listado seralizado con listado de contragarantías</returns>
        public ActionResult GetInsuredGuaranteeByIndividualId(int individualId)
        {
            try
            {
                List<UPV1.Guarantee> guarantees = DelegateService.uniquePersonServiceV1.GetInsuredGuaranteesByIndividualId(individualId);
                guarantees.ForEach((x) => { x.InsuredGuarantee.InsuredGuaranteeLog.ForEach(y => y.UserName = DelegateService.uniqueUserService.GetUserById(y.UserId).AccountName); });
                listGuaranteeOld = guarantees;
                return new UifJsonResult(true, guarantees);
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchGuarantees);
            }
        }

        /// <summary>
        /// Obtiene las sucursales configuradas en la base de datos
        /// </summary>
        /// <returns> Listado de sucursales </returns>
        public ActionResult GetBranches()
        {
            try
            {
                if (branches.Count == 0)
                {
                    branches = DelegateService.commonService.GetBranches();
                }

                var list = branches.Select(item => new { item.Id, item.Description }).ToList();

                return new UifSelectResult(list.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifSelectResult(null);
            }
        }

        /// <summary>
        /// Obtiene las compañías aseguradoras de la base de datos
        /// </summary>
        /// <param name="query"> Parámetro de búsqueda </param>
        /// <returns> Listado de compañías aseguradoras </returns>
        public JsonResult GetCoInsuranceCompanies(string query)
        {
            try
            {
                if (coInsuranceCompanies.Count == 0)
                {
                    coInsuranceCompanies = DelegateService.uniquePersonServiceV1.GetCoInsuranceCompanies();
                }

                var list = coInsuranceCompanies.Where(c => c.Description.Contains(query.ToUpper())).Select(item => new { item.Id, item.Description }).ToList();

                return Json(list.OrderBy(x => x.Description).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Obtiene los ramos comerciales
        /// </summary>
        /// <returns> Listado de ramos comerciales </returns>
        public ActionResult GetPrefixCommercial()
        {
            try
            {
                if (prefix.Count == 0)
                {
                    prefix = DelegateService.commonService.GetPrefixes();
                }

                var list = prefix.Select(item => new { item.Id, item.Description }).ToList();

                return new UifSelectResult(list.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifSelectResult(null);
            }
        }

        /// <summary>
        /// Obtiene los tipos de bien
        /// </summary>
        /// <returns> Listado de tipos de bien </returns>
        public ActionResult GetAssetType()
        {
            try
            {
                if (assetType.Count == 0)
                {
                    assetType = DelegateService.uniquePersonServiceV1.GetAssetType();
                }

                var list = assetType.Select(item => new { item.Code, item.Description }).ToList();

                return new UifSelectResult(list.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifSelectResult(null);
            }
        }

        /// <summary>
        /// Obtiene las unidades de medida
        /// </summary>
        /// <returns> Listado de unidades de medida </returns>
        public ActionResult GetUnitsOfMeasure()
        {
            try
            {
                if (measurementType.Count == 0)
                {
                    measurementType = DelegateService.uniquePersonServiceV1.GetMeasurementType();
                }

                var list = measurementType.Select(item => new { item.Id, item.Description }).ToList();

                return new UifSelectResult(list.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifSelectResult(null);
            }
        }

        /// <summary>
        /// Obtiene los tipos de pagaré
        /// </summary>
        /// <returns> Listado de tipos de pagaré </returns>
        public ActionResult GetPromissoryNoteType()
        {
            try
            {
                if (promissoryNoteType.Count == 0)
                {
                    promissoryNoteType = DelegateService.uniquePersonServiceV1.GetPromissoryNoteType();
                }

                var list = promissoryNoteType.Select(item => new { item.Id, item.Description }).ToList();

                return new UifSelectResult(list.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifSelectResult(null);
            }
        }

        /// <summary>
        /// Obtiene los estados de contragarantías
        /// </summary>
        /// <returns> Listado de estados </returns>
        public ActionResult GetGuaranteeStatus()
        {
            try
            {
                guaranteeStatus = new List<GuaranteeStatus>();
                guaranteeStatus = DelegateService.uniquePersonServiceV1.GetGuaranteeStatus();

                var list = guaranteeStatus.Select(item => new { item.Id, item.Description }).ToList();

                return new UifSelectResult(list.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifSelectResult(null);
            }
        }

        /// <summary>
        /// Obtiene los estados de contragarantías
        /// </summary>
        /// <returns> Listado de estados </returns>
        public ActionResult GetProfileGuaranteeStatus(int profileId)
        {
            try
            {
                ProfileGuaranteeStatus = DelegateService.uniqueUserService.GetProfileGuaranteeStatus(profileId);
                return new UifJsonResult(true, ProfileGuaranteeStatus.OrderBy(x => x.StatusId));
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }


        /// <summary>
        /// Ontiene el listado de monedas configuradas en el sistema
        /// </summary>
        /// <returns> Listado de monedas </returns>
        public ActionResult GetCurrencies()
        {
            try
            {
                if (currencies.Count == 0)
                {
                    currencies = DelegateService.commonService.GetCurrencies();
                }

                var list = currencies.Select(item => new { item.Id, item.Description }).ToList();

                return new UifSelectResult(list.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifSelectResult(null);
            }
        }

        /// <suOmmary>
        /// Obtiene los paises configurados en la base de datos
        /// </summary>
        /// <returns>Listado de países</returns>
        public ActionResult GetCountries()
        {
            try
            {

                if (countries.Count == 0)
                {
                    countries = DelegateService.commonService.GetCountries();
                }

                var list = countries.Select(item => new { item.Id, item.Description }).ToList();

                return new UifSelectResult(list.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifSelectResult(null);
            }

        }

        /// <summary>
        /// Obtiene el listado de ciudades dado un país y un departamento
        /// </summary>
        /// <param name="countryId"> Identificador del país </param>
        /// <param name="stateId"> Identificador del departamento </param>
        /// <returns> Listado de ciudades filtrado </returns>
        public ActionResult GetCitiesByCountryIdStateId(int countryId, int stateId)
        {
            List<City> cities = (from c in countries
                                 from s in c.States
                                 where c.Id == countryId && s.Id == stateId
                                 select s.Cities).FirstOrDefault();

            if (cities != null)
            {
                var list = cities.Select(item => new { item.Id, item.Description }).ToList();
                return new UifSelectResult(list.OrderBy(x => x.Description));
            }
            else
            {
                return new UifSelectResult(new List<City>());
            }
        }

        /// <summary>
        /// Obtiene las ciudades
        /// </summary>
        /// <param name="query"> parámetro de la búsqueda </param>
        /// <returns> Listado de ciudades </returns>
        public JsonResult GetCities(string query)
        {
            List<State> states = countries.SelectMany(b => b.States).Distinct().ToList();
            List<City> cities = states.SelectMany(b => b.Cities).Distinct().ToList();

            if (cities != null)
            {
                var list = cities.Where(c => c.Description.Contains(query.ToUpper())).Select(item => new { item.Id, item.Description }).ToList();
                return Json(list.OrderBy(x => x.Description), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new List<City>(), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Obtiene los departamentos filtrados por país
        /// </summary>
        /// <param name="countryId"> Identificador del país </param>
        /// <returns> Listado de departamentos filtrado </returns>
        public ActionResult GetStatesByCountryId(int countryId)
        {
            try
            {
                Country country = (from c in countries where c.Id == countryId select c).FirstOrDefault();

                if (country != null)
                {
                    var list = country.States.Select(item => new { item.Id, item.Description }).ToList();
                    return new UifSelectResult(list.OrderBy(x => x.Description));
                }
                else
                {
                    return new UifSelectResult(new List<State>());
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Página prinicipal de contragarantías que permite listar y agregar nuevas contragarantías de un afianzado
        /// </summary>
        /// <returns> Vista de contragarantías </returns>
        public ActionResult Guarantees(GuaranteeViewModel guaranteeViewModel)
        {
            GuaranteeViewModel model = new GuaranteeViewModel();
            model.ContractorId = guaranteeViewModel.ContractorId;
            model.ContractorName = guaranteeViewModel.ContractorName;
            model.ContractorNumber = guaranteeViewModel.ContractorNumber;
            model.searchType = guaranteeViewModel.searchType;
            model.Address = guaranteeViewModel.Address;
            model.PhoneNumber = guaranteeViewModel.PhoneNumber;
            model.CityText = guaranteeViewModel.CityText;

            model.PrefixAssociated = new PrefixAssociatedViewModel();
            model.PrefixAssociated.ContractorId = guaranteeViewModel.ContractorId;
            model.PrefixAssociated.SecureName = model.ContractorName;

            model.DocumentationReceived = new DocumentationReceivedViewModel();
            model.DocumentationReceived.ContractorId = guaranteeViewModel.ContractorId;
            model.DocumentationReceived.SecureName = model.ContractorName;

            model.Binnacle = new BinnacleViewModel();
            model.Binnacle.ContractorId = guaranteeViewModel.ContractorId;
            model.Binnacle.SecureName = model.ContractorName;

            model.BindPolicy = new BindPolicyViewModel();
            model.BindPolicy.ContractorId = guaranteeViewModel.ContractorId;
            model.BindPolicy.SecureName = model.ContractorName;

            model.PromissoryNote = new PromissoryNoteViewModel();
            model.Pledge = new PledgeViewModel();
            model.Mortage = new MortageViewModel();
            model.Actions = new ActionsViewModel();
            model.Others = new OthersViewModel();
            model.FixedTermDeposit = new FixedTermDepositViewModel();
            model.ParamGuarantee = guaranteeViewModel.ParamGuarantee;

            ViewData.Model = model;
            ViewBag.HardRiskType_Code = (int)Application.UniquePersonService.V1.Enums.HardRiskTypes.Bail;
            return View(model);
        }

        public ActionResult GetInsuredGuaranteeLogs(int individualId, int guaranteeId)
        {
            List<UPV1.InsuredGuaranteeLog> guaranteesLog = new List<UPV1.InsuredGuaranteeLog>();
            try
            {
                if (guaranteeId > 0)
                {
                    guaranteesLog = DelegateService.uniquePersonServiceV1.GetInsuredGuaranteeLogs(individualId, guaranteeId);
                    return new UifJsonResult(true, guaranteesLog);

                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryBitacoraGuarantees);
            }
        }

        public ActionResult GetDocumentationReceivedByGuaranteeId(int guaranteeId)
        {
            try
            {
                guaranteeRequiredDocument = DelegateService.uniquePersonServiceV1.GetDocumentationReceivedByGuaranteeId(guaranteeId);
                return new UifJsonResult(true, guaranteeRequiredDocument);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, guaranteeRequiredDocument);
            }
        }

        public ActionResult GetPrefixAssociated(int coveredRiskType)
        {
            try
            {
                List<LineBusiness> lineBusiness = DelegateService.commonService.GetHardRiskTypeByCoveredRiskType((CoveredRiskType)coveredRiskType);
                return new UifJsonResult(true, lineBusiness.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, null);
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

        public ActionResult GetGuaranteesStatusByUserId()
        {
            try
            {
                guaranteeStatus = new List<GuaranteeStatus>();
                List<GuaranteeStatus> guaranteeStatusFE = new List<GuaranteeStatus>();
                List<UUMDL.User> userData = DelegateService.uniqueUserService.GetUsersByAccountName("", SessionHelper.GetUserId(), 0);
                List<UUMDL.ProfileGuaranteeStatus> guaranteeStatuses = new List<UUMDL.ProfileGuaranteeStatus>();
                foreach (UUMDL.Profile profile in userData.FirstOrDefault().Profiles)
                { 
                    List<UUMDL.ProfileGuaranteeStatus> guaranteeStatusesIn = DelegateService.uniqueUserService.GetProfileGuaranteeStatus(profile.Id);
                    foreach(UUMDL.ProfileGuaranteeStatus guaranteeStatus in guaranteeStatusesIn)
                    {
                        if (guaranteeStatus.Enabled) { guaranteeStatuses.Add(guaranteeStatus); }
                    }
                }
                foreach (UUMDL.ProfileGuaranteeStatus item in guaranteeStatuses)
                {
                    List<GuaranteeStatus> guarantee = DelegateService.uniquePersonServiceV1.GetGuaranteeStatusByGuaranteeStatusId(item.StatusId);
                    foreach (GuaranteeStatus itemGuarantee in guarantee)
                    {
                        if (guaranteeStatus.Count > 0) { guaranteeStatusFE = guaranteeStatus.Where(x => x.Id == itemGuarantee.Id).ToList(); }
                        if (guaranteeStatusFE.Count == 0)
                        {
                            guaranteeStatus.Add(itemGuarantee);
                        }
                    }
                }
                var list = guaranteeStatus.Select(item => new { item.Id, item.Description }).ToList();

                return new UifSelectResult(list.OrderBy(x => x.Description).ToList());
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, null);
            }
        }
        #endregion

        #region grabar crear
        public ActionResult SaveInsuredGuarantees(List<UPV1.Guarantee> listGuarantee)
        {
            try
            {
                foreach (var guarante in listGuarantee)
                {
                    List<UPV1.InsuredGuaranteeLog> insuredGuaranteeLog = guarante.InsuredGuarantee.InsuredGuaranteeLog;
                    if (insuredGuaranteeLog != null && insuredGuaranteeLog.Count > 0)
                    {
                        insuredGuaranteeLog.ForEach((x) => { x.UserId = SessionHelper.GetUserId(); x.GuaranteeStatusCode = guarante.InsuredGuarantee.Status.Id; });
                        //insuredGuaranteeLog.UserId = SessionHelper.GetUserId();
                        //insuredGuaranteeLog.GuaranteeStatusCode = guarante.InsuredGuarantee.GuaranteeStatus.Code;
                    }
                }
                List<UPV1.Guarantee> listGuaranteeSave = DelegateService.uniquePersonServiceV1.SaveInsuredGuarantees(listGuarantee);
                return new UifJsonResult(true, listGuaranteeSave);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, null);
            }
        }

        public ActionResult SaveInsuredGuarantee(UPV1.Guarantee guarantee)
        {
            try
            {
                List<UPV1.InsuredGuaranteeLog> insuredGuaranteeLog = new List<UPV1.InsuredGuaranteeLog>();
                string text = "";

                if (guarantee.InsuredGuarantee.Id > 0)
                {
                    foreach (UPV1.Guarantee item in listGuaranteeOld)
                    {
                        if (String.Equals(item.InsuredGuarantee.Id, guarantee.InsuredGuarantee.Id))
                        {
                            if (guarantee.InsuredGuarantee.Country.Id != item.InsuredGuarantee.Country.Id)
                                text += App_GlobalResources.Language.New + " " + App_GlobalResources.Language.LabelCountry + " " + App_GlobalResources.Language.BinnacleGuarantee + " " + guarantee.InsuredGuarantee.Country.Description + "\n";
                            if (guarantee.InsuredGuarantee.State.Id != item.InsuredGuarantee.State.Id)
                                text += App_GlobalResources.Language.New + " " + App_GlobalResources.Language.LabelState + " " + App_GlobalResources.Language.BinnacleGuarantee + " " + guarantee.InsuredGuarantee.State.Description + Environment.NewLine;
                            if (guarantee.InsuredGuarantee.City.Id != item.InsuredGuarantee.City.Id)
                                text += App_GlobalResources.Language.New + " " + App_GlobalResources.Language.LabelCity + " " + App_GlobalResources.Language.BinnacleGuarantee + " " + guarantee.InsuredGuarantee.City.Description + "\n";
                            if (guarantee.InsuredGuarantee.ConstitutionDate != item.InsuredGuarantee.ConstitutionDate)
                                text += App_GlobalResources.Language.New + " " + App_GlobalResources.Language.LabelConstitutionDate + " " + App_GlobalResources.Language.BinnacleGuarantee + " " + guarantee.InsuredGuarantee.ConstitutionDate + "\n";
                            if (guarantee.InsuredGuarantee.ExpirationDate != item.InsuredGuarantee.ExpirationDate)
                                text += App_GlobalResources.Language.New + " " + App_GlobalResources.Language.LabelDueDate + " " + App_GlobalResources.Language.BinnacleGuarantee + " " + guarantee.InsuredGuarantee.ExpirationDate + "\n";
                            if (guarantee.InsuredGuarantee.Currency.Id != item.InsuredGuarantee.Currency.Id)
                                text += App_GlobalResources.Language.New + " " + App_GlobalResources.Language.LabelCoin + " " + App_GlobalResources.Language.BinnacleGuarantee + " " + guarantee.InsuredGuarantee.Currency.Description + "\n";
                            if (guarantee.InsuredGuarantee.PromissoryNoteType != null)
                            {
                                if (guarantee.InsuredGuarantee.PromissoryNoteType.Id !=(int)(item.InsuredGuarantee.PromissoryNoteType?.Id ?? 0))
                                    text += App_GlobalResources.Language.New + " " + App_GlobalResources.Language.GuaranteeType + " " + App_GlobalResources.Language.BinnacleGuarantee + " " + item.InsuredGuarantee.PromissoryNoteType?.Description ?? "" + "\n";
                            }
                            if (guarantee.InsuredGuarantee.Branch.Id != item.InsuredGuarantee.Branch.Id)    //Sucursal
                                text += App_GlobalResources.Language.New + " " + App_GlobalResources.Language.LabelBranch + " " + App_GlobalResources.Language.BinnacleGuarantee + " " + guarantee.InsuredGuarantee.Branch.Id + "\n"; //Estado
                            if (guarantee.InsuredGuarantee.Status.Id != item.InsuredGuarantee.Status.Id)
                                text += App_GlobalResources.Language.LabelUpdate + App_GlobalResources.Language.LabelStatus + ": "+ item.InsuredGuarantee.Status.Description + " - "+ guarantee.InsuredGuarantee.Status.Description + Environment.NewLine;
                            if (guarantee.InsuredGuarantee.RegistrationNumber != item.InsuredGuarantee.RegistrationNumber)
                                text += App_GlobalResources.Language.New + " " + App_GlobalResources.Language.LabelDeedNumber + " " + App_GlobalResources.Language.BinnacleGuarantee + " " + item.InsuredGuarantee.RegistrationNumber + "\n";
                            if (guarantee.InsuredGuarantee.Address != item.InsuredGuarantee.Address)
                                text += App_GlobalResources.Language.New + " " + App_GlobalResources.Language.LabelAddress + " " + App_GlobalResources.Language.BinnacleGuarantee + " " + item.InsuredGuarantee.Address + "\n";
                            if (guarantee.InsuredGuarantee.MeasurementType != null )
                            {
                                if (guarantee.InsuredGuarantee.MeasurementType.Id != (int)(item.InsuredGuarantee.MeasurementType?.Id ?? 0))
                                    text += App_GlobalResources.Language.New + " " + App_GlobalResources.Language.LabelMortageType + " " + App_GlobalResources.Language.BinnacleGuarantee + " " + item.InsuredGuarantee.MeasurementType?.Description ?? ""  + "\n";
                            }
                            if (guarantee.InsuredGuarantee.InsuranceAmount != item.InsuredGuarantee.InsuranceAmount)
                                text += App_GlobalResources.Language.LabelUpdate + App_GlobalResources.Language.LabelValuationValue + ": " + item.InsuredGuarantee.InsuranceAmount     + " - " + guarantee.InsuredGuarantee.InsuranceAmount + Environment.NewLine;
                                
                            if (guarantee.InsuredGuarantee.AppraisalDate != item.InsuredGuarantee.AppraisalDate)
                                text += App_GlobalResources.Language.New + " " + App_GlobalResources.Language.LabelValuationDate + " " + App_GlobalResources.Language.BinnacleGuarantee + " " + item.InsuredGuarantee.AppraisalDate + "\n";
                            if (guarantee.InsuredGuarantee.LicensePlate != item.InsuredGuarantee.LicensePlate)
                                text += App_GlobalResources.Language.New + " " + App_GlobalResources.Language.LabelLicencesePlate + " " + App_GlobalResources.Language.BinnacleGuarantee + " " + item.InsuredGuarantee.LicensePlate + Environment.NewLine;
                            if (guarantee.InsuredGuarantee.ChassisNro != item.InsuredGuarantee.ChassisNro)
                                text += App_GlobalResources.Language.New + " " + App_GlobalResources.Language.LabelChassis + " " + App_GlobalResources.Language.BinnacleGuarantee + " " + item.InsuredGuarantee.ChassisNro + "\n";
                            if (guarantee.InsuredGuarantee.EngineNro != item.InsuredGuarantee.EngineNro)
                                text += App_GlobalResources.Language.New + " " + App_GlobalResources.Language.LabelEngine + " " + App_GlobalResources.Language.BinnacleGuarantee + " " + item.InsuredGuarantee.LicensePlate + "\n";
                            if (guarantee.InsuredGuarantee.IssuerName != item.InsuredGuarantee.IssuerName)
                                text += App_GlobalResources.Language.New + " " + App_GlobalResources.Language.LabelIssuingEntity + " " + App_GlobalResources.Language.BinnacleGuarantee + " " + item.InsuredGuarantee.IssuerName + "\n";
                            if (guarantee.InsuredGuarantee.RegistrationDate != item.InsuredGuarantee.RegistrationDate)
                                text += App_GlobalResources.Language.New + " " + App_GlobalResources.Language.LabelConstitutionDate + " " + App_GlobalResources.Language.BinnacleGuarantee + " " + item.InsuredGuarantee.RegistrationDate + "\n";
                            if (guarantee.InsuredGuarantee.DocumentValueAmount != item.InsuredGuarantee.DocumentValueAmount)
                                text += App_GlobalResources.Language.LabelUpdate + App_GlobalResources.Language.LabelNominalValue + ": " + item.InsuredGuarantee.DocumentValueAmount + " - " + guarantee.InsuredGuarantee.DocumentValueAmount + Environment.NewLine;
                                
                            var logObjectGuarantee = guarantee.InsuredGuarantee.InsuredGuaranteeLogObject ?? new UPV1.InsuredGuaranteeLog();
                            logObjectGuarantee.UserName = User.Identity.Name;
                            logObjectGuarantee.IndividualId = guarantee.InsuredGuarantee.IndividualId;
                            logObjectGuarantee.LogDate = DateTime.Now;
                            logObjectGuarantee.GuaranteeId = guarantee.InsuredGuarantee.Id;
                            logObjectGuarantee.GuaranteeStatusCode = guarantee.InsuredGuarantee.Status.Id;
                            logObjectGuarantee.UserId = SessionHelper.GetUserId();
                            logObjectGuarantee.Description = logObjectGuarantee?.Description ?? string.Empty;

                            if (logObjectGuarantee.Description.Trim().Length > 0)
                            {
                                //insuredGuaranteeLog.First().Description = guarantee.InsuredGuarantee.InsuredGuaranteeLog[0].Description;
                                if (text != "")
                                {
                                    logObjectGuarantee.Description = string.Format("{0} | {1}", logObjectGuarantee.Description, text);
                                }
                                else
                                    logObjectGuarantee.Description += " | " + App_GlobalResources.Language.AutomaticMsj + text;
                            }
                            else
                            {
                                logObjectGuarantee.Description = string.Format("{0} {1} {2}", "- ", Environment.NewLine, text);
                                //logObjectGuarantee.Description = string.Format("{0} {1}", App_GlobalResources.Language.AutomaticMsj, text);
                            }
                            guarantee.InsuredGuarantee.InsuredGuaranteeLogObject = logObjectGuarantee;
                            break;
                        }
                        //else //prueba
                        //{
                        //    var logObjectGuarantee = guarantee.InsuredGuarantee.InsuredGuaranteeLogObject ?? new UPV1.InsuredGuaranteeLog();
                        //    if (logObjectGuarantee != null)
                        //    {
                        //        logObjectGuarantee.UserName = User.Identity.Name;
                        //        logObjectGuarantee.IndividualId = guarantee.InsuredGuarantee.IndividualId;
                        //        logObjectGuarantee.LogDate = DateTime.Now;
                        //        logObjectGuarantee.GuaranteeId = guarantee.InsuredGuarantee.Id;
                        //        logObjectGuarantee.GuaranteeStatusCode = guarantee.InsuredGuarantee.Status.Id;
                        //        logObjectGuarantee.UserId = SessionHelper.GetUserId();
                        //        logObjectGuarantee.Description = logObjectGuarantee?.Description ?? string.Empty;

                        //        if (logObjectGuarantee.Description.Trim().Length > 0)
                        //        {
                        //            logObjectGuarantee.Description =
                        //            string.Format("{0} | {1}",
                        //            logObjectGuarantee.Description, (
                        //            App_GlobalResources.Language.AutomaticMsj + "\n" + App_GlobalResources.Language.NewGuarantee));
                        //        }
                        //        else
                        //        {
                        //            logObjectGuarantee.Description = App_GlobalResources.Language.AutomaticMsj + "\n" + App_GlobalResources.Language.NewGuarantee;
                        //        }

                        //        guarantee.InsuredGuarantee.InsuredGuaranteeLogObject = logObjectGuarantee;
                        //    }
                        //}
                    }
                }
                else
                {
                    var logObjectGuarantee = guarantee.InsuredGuarantee.InsuredGuaranteeLogObject ?? new UPV1.InsuredGuaranteeLog();

                    text += " " + App_GlobalResources.Language.LabelStatus + ": " + guarantee.InsuredGuarantee.Status.Description + Environment.NewLine;
                    if (logObjectGuarantee != null)
                    {
                        logObjectGuarantee.UserName = User.Identity.Name;
                        logObjectGuarantee.IndividualId = guarantee.InsuredGuarantee.IndividualId;
                        logObjectGuarantee.LogDate = DateTime.Now;
                        logObjectGuarantee.GuaranteeId = guarantee.InsuredGuarantee.Id;
                        logObjectGuarantee.GuaranteeStatusCode = guarantee.InsuredGuarantee.Status.Id;
                        logObjectGuarantee.UserId = SessionHelper.GetUserId();
                        logObjectGuarantee.Description = logObjectGuarantee?.Description ?? string.Empty; 

                        if (logObjectGuarantee.Description.Trim().Length > 0)
                        {
                            logObjectGuarantee.Description =
                            string.Format("{0} | {1}",
                            logObjectGuarantee.Description, (
                            App_GlobalResources.Language.AutomaticMsj + "\n" + App_GlobalResources.Language.NewGuarantee + text));
                        }
                        else
                        {
                            logObjectGuarantee.Description = App_GlobalResources.Language.AutomaticMsj + "\n" + App_GlobalResources.Language.NewGuarantee + text;
                        }

                        guarantee.InsuredGuarantee.InsuredGuaranteeLogObject = logObjectGuarantee;
                    }

                }

                //Save
                GuaranteeDto guaranteeDto = new GuaranteeDto
                {
                    Guarantee = guarantee,
                    UserId = SessionHelper.GetUserId()
                };

                guaranteeDto = DelegateService.uniquePersonAplicationService.SaveApplicationInsuredGuarantee(guaranteeDto);
                return new UifJsonResult(true, guaranteeDto);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }
        #endregion

        public ActionResult GetGuaranteesTypes()
        {
            try
            {
                //if (guaranteesTypes.Count == 0)
                //{
                    guaranteesTypes = DelegateService.uniquePersonServiceV1.GetGuaranteesTypes();
                //}
                return new UifSelectResult(guaranteesTypes.OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifSelectResult(false, App_GlobalResources.Language.ErrorGetGuaranteeType);
            }
        }

        public ActionResult GetGuarantees()
        {
            try
            {
                //if (guaranteesGlb.Count == 0)
                //{
                    guaranteesGlb = DelegateService.uniquePersonServiceV1.GetGuarantees();
                //}
                return new UifJsonResult(true, guaranteesGlb.OrderBy(x => x.Type.Description));
            }
            catch (Exception)
            {
                return new UifSelectResult(false, App_GlobalResources.Language.ErrorGetGuaranteeType);
            }
        }

        public ActionResult GetGuaranteesRoutesByGuaranteeStatusId(int guaranteeStatusId)
        {
            try
            {
                guaranteeStatus = new List<GuaranteeStatus>();
                List<GuaranteeStatus> guaranteeStatusFE = new List<GuaranteeStatus>();
                List<UUMDL.User> userData = DelegateService.uniqueUserService.GetUsersByAccountName("", SessionHelper.GetUserId(), 0);
                List<UUMDL.ProfileGuaranteeStatus> guaranteeStatuses = new List<UUMDL.ProfileGuaranteeStatus>();
                foreach (UUMDL.Profile profile in userData.FirstOrDefault().Profiles)
                {
                    List<UUMDL.ProfileGuaranteeStatus> guaranteeStatusesIn = DelegateService.uniqueUserService.GetProfileGuaranteeStatus(profile.Id);
                    foreach (UUMDL.ProfileGuaranteeStatus guaranteeStatus in guaranteeStatusesIn)
                    {
                        if (guaranteeStatus.Enabled) { guaranteeStatuses.Add(guaranteeStatus); }
                    }
                }
                var statuses = DelegateService.uniquePersonServiceV1.GetGuaranteeStatusRoutesByGuaranteeStatusId(guaranteeStatusId);

                foreach (GuaranteeStatus status in statuses)
                {
                    if(guaranteeStatuses.Any(x=>x.StatusId == status.Id))
                    {
                        guaranteeStatus.Add(status);
                    }
                }
                return new UifJsonResult(true, guaranteeStatus.OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifSelectResult(false, App_GlobalResources.Language.ErrorGetGuaranteeType);
            }
        }

        #region EESGE-172 Control De Busqueda
        // Countries
        public ActionResult SetInitialCountriesByCountryId(int countryParemeter)
        {
            try
            {
                var listCountries = countries.Where(x => x.Id == countryParemeter).Select(x => new { Id = x.Id, Description = x.Description }).ToList();

                if (listCountries.Count != 0)
                {
                    return new UifJsonResult(true, listCountries);
                }

                return new UifJsonResult(true, null);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, new { ErrorMsg = ex.Message });
            }
        }
        public ActionResult GetCountriesByDescription(string Description)
        {
            try
            {
                GetCountries();
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
        // States
        public ActionResult GetStatesByCountryIdByDescription(int CountryId, string Description)
        {
            try
            {
                GetCountries();
                var listStates = countries.Where(x => x.Id == CountryId).ToList()[0]
              .States.Where(y => y.Description.Contains(Description)).Select(x => new { Id = x.Id, Description = x.Description }).ToList();

                if (listStates.Count != 0)
                {
                    return new UifJsonResult(true, listStates);
                }

                return new UifJsonResult(true, null);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, new { ErrorMsg = ex.Message });
            }
        }
        // Cities
        public ActionResult GetCitiesByCountryIdByStateIdByDescription(int CountryId, int StateId, string Description)
        {
            try
            {
                GetCountries();
                var listCities = countries.Where(x => x.Id == CountryId).ToList()[0]
                    .States.Where(y => y.Id == StateId).ToList()[0]
                    .Cities.Where(z => z.Description.Contains(Description)).Select(x => new { Id = x.Id, Description = x.Description }).ToList();

                if (listCities.Count != 0)
                {
                    return new UifJsonResult(true, listCities);
                }

                return new UifJsonResult(true, null);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, new { ErrorMsg = ex.Message });
            }
        }
        // DaneCode
        public ActionResult GetCountryAndStateAndCityByDaneCode(int CountryId, string DaneCode)
        {
            try
            {
                GetCountries();
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

                return new UifJsonResult(true, null);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, new { ErrorMsg = ex.Message });
            }
        }
        #endregion
    }
}
