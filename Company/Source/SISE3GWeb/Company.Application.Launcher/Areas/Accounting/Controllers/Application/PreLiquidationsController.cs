//System
using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Threading;
using System.Configuration;
using System.Globalization;
using System.Collections.Generic;
// Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Exceptions;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Resources;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Framework.UIF2.Services;
// Sistran Core
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.TempCommonServices.DTOs;
using Sistran.Core.Application.UniqueUserServices.Models;
using CoreUniquePersonModels = Sistran.Core.Application.UniquePersonService.V1.Models;
using AccountingPaymentModels = Sistran.Core.Application.AccountingServices.DTOs.Payments;
using DTOs = Sistran.Core.Application.AccountingServices.DTOs;
using AccountingConceptModel = Sistran.Core.Application.GeneralLedgerServices.DTOs.AccountingConcepts;
//Sistran Company
using CompanyUniquePersonModels = Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Application.UniquePersonService.V1.Models;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers.Application
{
    [Authorize]
    [HandleError]
    [FilterConfigHelper.NoDirectAccessAttribute]
    public class PreLiquidationsController : Controller
    {
        #region Instance variables
        readonly BillingController _billingController = new BillingController();
        readonly CommonController _commonController = new CommonController();
        readonly int _imputationTypePreliquidation = Convert.ToInt16(Core.Application.AccountingServices.Enums.ApplicationTypes.PreLiquidation);
        #endregion

        #region PreLiquidation

        /// <summary>
        /// MainPreLiquidations
        /// Invoca a la vista de Preliquidaciones
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainPreLiquidations()
        {
            try
            {         
                int defaultAccounting = DelegateService.accountingApplicationService.GetAccountingCompanyDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name));

                if (defaultAccounting <= 0)
                {
                    ViewBag.AccountingCompanyDefault = 1;
                }
                else
                {
                    ViewBag.AccountingCompanyDefault = defaultAccounting;
                }

                ViewBag.AccountingCompany = _billingController.LoadAccountingCompanies(_commonController.GetUserIdByName(User.Identity.Name));
                ViewBag.ParameterMulticompany = _billingController.GetParameterMulticompany();

                ViewBag.ImputationType = _imputationTypePreliquidation;

                List<AccountingConceptModel.ConceptSourceDTO> conceptSources = _commonController.GetConceptSources();
                ViewBag.TypeRequest = conceptSources;

                List<DTOs.CollectConceptDTO> billingConcepts = DelegateService.accountingParameterService.GetCollectConcepts();
                ViewBag.IncomeConcept = billingConcepts;

                List<AccountingPaymentModels.PaymentMethodDTO> paymentMethods = new List<AccountingPaymentModels.PaymentMethodDTO>();
                ViewBag.PaymentMethod = paymentMethods;

                List<DTOs.CollectConceptDTO> billingIncomeConcepts = DelegateService.accountingParameterService.GetCollectConcepts();
                ViewBag.IncomeConcept = billingIncomeConcepts;

                // Recupera fecha contable
                DateTime dateAccounting = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now);
                ViewBag.DateAccounting = _commonController.DateFormat(dateAccounting.Date, 2);

                List<DTOs.Search.ValueTypeDTO> valueTypes = new List<DTOs.Search.ValueTypeDTO>();

                valueTypes.Add(new DTOs.Search.ValueTypeDTO() { Id = 1, Description = "CHEQUE POSTFECHADO" });
                valueTypes.Add(new DTOs.Search.ValueTypeDTO() { Id = 2, Description = "TARJETA POSTFECHADA" });
                ViewBag.valueTypes = valueTypes;

                int branchUserDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchUserDefault = branchUserDefault;
                ViewBag.SalePointBranchUserDefault = DelegateService.accountingApplicationService.GetSalePointDefaultByUserIdAndBranchId(_commonController.GetUserIdByName(User.Identity.Name), branchUserDefault);

                // Se utilizan los parámetros definidos en el web.config en lugar de los definidos en el archivo de recursos.
                ViewBag.SupplierCode = ConfigurationManager.AppSettings["SupplierCode"];
                ViewBag.InsuredCode = ConfigurationManager.AppSettings["InsuredCode"];
                ViewBag.CoinsurerCode = ConfigurationManager.AppSettings["CoinsurerCode"];
                ViewBag.ThirdPartyCode = ConfigurationManager.AppSettings["ThirdPartyCode"];
                ViewBag.AgentCode = ConfigurationManager.AppSettings["AgentCode"];
                ViewBag.ProducerCode = ConfigurationManager.AppSettings["ProducerCode"];
                ViewBag.EmployeeCode = ConfigurationManager.AppSettings["EmployeeCode"];
                ViewBag.ReinsurerCode = ConfigurationManager.AppSettings["ReinsurerCode"];
                ViewBag.TradeConsultant = ConfigurationManager.AppSettings["TradeConsultant"];
                ViewBag.ContractorCode = ConfigurationManager.AppSettings["ContractorCode"];
                ViewBag.PointSalesPreliquidation = ConfigurationManager.AppSettings["PointSalesPreliquidation"];

                // Variables para la edición de preliquidaciones
                ViewBag.TempImputationId = TempData["TempImputationId"] ?? 0;
                ViewBag.BranchId = TempData["BranchId"] ?? 0;
                ViewBag.IsEdition = TempData["IsEdition"] ?? 0;
                ViewBag.SalePointId = TempData["SalePointId"] ?? 0;
                ViewBag.CompanyId = TempData["CompanyId"] ?? 0;
                ViewBag.GenerationDate = TempData["GenerationDate"] ?? 0;
                ViewBag.PersonTypeId = TempData["PersonTypeId"] ?? 0;
                ViewBag.DocumentNumber = TempData["DocumentNumber"] ?? "";
                ViewBag.Name = TempData["Name"] ?? "";
                ViewBag.BeneficiaryId = TempData["BeneficiaryId"] ?? 0;
                ViewBag.TempImputationId = TempData["TempImputationId"] ?? 0;
                ViewBag.PreliquidationId = TempData["PreliquidationId"] ?? 0;
                ViewBag.Description = TempData["Description"] ?? "";
                ViewBag.IsPreliquidation = TempData["IsPreliquidation"] ?? 0;
                ViewBag.ApplyCollecId = TempData["ApplyCollecId"] ?? 0;
                ViewBag.ReceiptNumber = TempData["ReceiptNumber"] ?? 0;
                ViewBag.DepositPrimes = Convert.ToString(ConfigurationManager.AppSettings["DepositPrimes"]);

                // Setear valor por default de la sucursal de acuerdo al usuario que se conecta 
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);

                //Obtiene si es el uso del tercero en 1 TRUE 0 FALSE
                ViewBag.ThirdAccountingUsed = (int)DelegateService.commonService.GetParameterByParameterId(Convert.ToInt32(ConfigurationManager.AppSettings["ThirdAccountingUsed"])).NumberParameter;

                return View("~/Areas/Accounting/Views/PreLiquidations/MainPreLiquidations.cshtml");

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// GetAccountingCompany
        /// Obtiene las compañías contables
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetAccountingCompany()
        {
            List<DTOs.CompanyDTO> companies = DelegateService.accountingCollectService.GetAccountingCompany();
            return new UifSelectResult(companies);
        }

        /// <summary>
        /// SaveTempPreLiquidation
        /// Grabado del temporal de la preliquidación
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult SaveTempPreLiquidation()
        {
            DTOs.Imputations.PreLiquidationDTO preLiquidation = new DTOs.Imputations.PreLiquidationDTO()
            {
                Id = 0,
                RegisterDate = DateTime.Now
            };

            preLiquidation = DelegateService.accountingApplicationService.SaveTempPreLiquidation(preLiquidation);

            var preLiquidationResponse = new
            {
                Id = preLiquidation.Id
            };

            return Json(preLiquidationResponse, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// UpdateTempPreLiquidation
        /// Actualiza preliquidación temporal
        /// </summary>
        /// <param name="preLiquidationModel"></param>
        /// <returns>JsonResult</returns>
        public JsonResult UpdateTempPreLiquidation(Models.Imputation.PreLiquidationModel preLiquidationModel)
        {
            DTOs.Imputations.PreLiquidationDTO preLiquidation = new DTOs.Imputations.PreLiquidationDTO();

            preLiquidation.Id = preLiquidationModel.Id;
            preLiquidation.RegisterDate = DateTime.Now;
            preLiquidation.Company = new DTOs.CompanyDTO() { IndividualId = preLiquidationModel.CompanyId };
            preLiquidation.Branch = new DTOs.Search.BranchDTO() { Id = preLiquidationModel.BranchId };

            if (String.IsNullOrEmpty(preLiquidationModel.Description))
            {
                preLiquidation.Description = " ";
            }
            else
            {
                preLiquidation.Description = preLiquidationModel.Description;
            }
            preLiquidation.Imputation = new DTOs.Imputations.ApplicationDTO();
            preLiquidation.Imputation.Id = preLiquidationModel.TempImputationId;
            preLiquidation.Payer = new DTOs.IndividualDTO() { IndividualId = preLiquidationModel.IndividualId };
            preLiquidation.PersonType = new DTOs.PersonTypeDTO() { Id = preLiquidationModel.PersonTypeId };
            preLiquidation.SalePoint = new DTOs.SalePointDTO() { Id = preLiquidationModel.SalePointId };
            preLiquidation.Status = preLiquidationModel.StatusId;
            preLiquidation.Imputation.UserId = _commonController.GetUserIdByName(User.Identity.Name);
            preLiquidation.IsTemporal = Convert.ToBoolean(preLiquidationModel.IsTemporal);
            preLiquidation = DelegateService.accountingApplicationService.UpdateTempPreLiquidation(preLiquidation);

            var preLiquidationResponse = new
            {
                Id = preLiquidation.Id
            };

            return Json(preLiquidationResponse, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// UpdatePreLiquidation
        /// </summary>
        /// <param name="preLiquidationModel"></param>
        /// <returns>JsonResult</returns>
        public JsonResult UpdatePreLiquidation(Models.Imputation.PreLiquidationModel preLiquidationModel)
        {
            DTOs.Imputations.PreLiquidationDTO preLiquidation = new DTOs.Imputations.PreLiquidationDTO();

            preLiquidation.Id = preLiquidationModel.Id;
            preLiquidation.RegisterDate = DateTime.Now;
            preLiquidation.Company = new DTOs.CompanyDTO () { IndividualId = preLiquidationModel.CompanyId };
            preLiquidation.Branch = new DTOs.Search.BranchDTO() { Id = preLiquidationModel.BranchId };
            preLiquidation.Description = preLiquidationModel.Description;
            preLiquidation.Imputation = new DTOs.Imputations.ApplicationDTO();
            preLiquidation.Imputation.Id = preLiquidationModel.ImputationId;
            preLiquidation.Payer = new DTOs.IndividualDTO() { IndividualId = preLiquidationModel.IndividualId };
            preLiquidation.PersonType = new DTOs.PersonTypeDTO() { Id = preLiquidationModel.PersonTypeId };
            preLiquidation.SalePoint = new DTOs.SalePointDTO() { Id = preLiquidationModel.SalePointId };
            preLiquidation.Status = preLiquidationModel.StatusId;
            preLiquidation.Imputation.UserId = _commonController.GetUserIdByName(User.Identity.Name);

            preLiquidation = DelegateService.accountingApplicationService.UpdatePreLiquidation(preLiquidation);

            var preLiquidationResponse = new
            {
                Id = preLiquidation.Id
            };

            return Json(preLiquidationResponse, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetTempPreLiquidationByTempPreliquidationId
        /// Obtiene preliquidaciones temporales por el Id
        /// </summary>
        /// <param name="tempPreliquidationId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTempPreLiquidationByTempPreliquidationId(int tempPreliquidationId)
        {
            DTOs.Imputations.PreLiquidationDTO preLiquidation = new DTOs.Imputations.PreLiquidationDTO() { Id = tempPreliquidationId };

            preLiquidation = DelegateService.accountingApplicationService.GetTempPreLiquidation(preLiquidation);

            CoreUniquePersonModels.Person person = DelegateService.uniquePersonServiceV1.GetPersonByIndividualId(preLiquidation.Payer.IndividualId);

            string documentNumber = String.Empty;
            string name = String.Empty;

            if (person == null)
            {
                CompanyUniquePersonModels.CompanyCompany companys  = DelegateService.uniquePersonServiceV1.GetCompanyByIndividualId(preLiquidation.Payer.IndividualId);
                if (companys != null)
                {
                    if (preLiquidation.Payer.IndividualId == companys.IndividualId)
                    {
                        documentNumber = companys.IdentificationDocument.Number;
                        name = companys.FullName;
                    }
                }
            }
            else {
                if (preLiquidation.Payer.IndividualId == person.IndividualId)
                {
                    documentNumber = person.IdentificationDocument.Number;
                    name = person.Name;
                }
            }
            
            List<SalePoint> salePoints = DelegateService.commonService.GetSalePointsByBranchId(preLiquidation.Branch.Id);

            string salePointDescript = String.Empty;

            foreach (SalePoint salePoint in salePoints)
            {
                if (preLiquidation.SalePoint.Id == salePoint.Id)
                {
                    salePointDescript = salePoint.Description;
                    break;
                }
            }

            var preLiquidations = new
            {
                Id = preLiquidation.Id,
                BranchId = preLiquidation.Branch.Id,
                SalesPointId = preLiquidation.SalePoint.Id,
                SalePointDescription = salePointDescript,
                AccountingCompanyId = preLiquidation.Company.IndividualId,
                RegisterDate = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(preLiquidation.RegisterDate)),
                PersonTypeId = preLiquidation.PersonType.Id,
                BeneficiaryDocumentNumber = documentNumber,
                BeneficiaryName = name,
                Description = preLiquidation.Description,
                BeneficiaryIndividualId = preLiquidation.Payer.IndividualId
            };

            return Json(preLiquidations, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ConvertTempPreLiquidationToPreLiquidation
        /// Convierte un preliquidación temporal en real
        /// </summary>
        /// <param name="tempPreLiquidationId"></param>
        /// <param name="tempImputationId"></param>
        /// <param name="imputationTypeId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ConvertTempPreLiquidationToPreLiquidation(int tempPreLiquidationId,
                                                                    int tempImputationId, int imputationTypeId)
        {
            try
            {
                int preLiquidationId = DelegateService.accountingApplicationService.ConvertTempPreLiquidationToPreLiquidation(tempPreLiquidationId,
                                                                                                           tempImputationId,
                                                                                                           imputationTypeId);
                var preLiquidation = new
                {
                    Id = preLiquidationId
                };

                return Json(preLiquidation, JsonRequestBehavior.AllowGet);
            }

            catch (BusinessException businessException)
            {
                return Json(new { success = false, result = businessException.ExceptionMessages }, JsonRequestBehavior.AllowGet);
            }
            catch (UnhandledException)
            {
                return Json(new { success = false, result = Global.UnhandledExceptionMsj }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region SearchPreliquidations

        /// <summary>
        /// MainPreLiquidationsSearch
        /// Invoca a la vista de búsqueda de preliquidaciones
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainPreLiquidationsSearch()
        {
            try
            {        
                // Se utilizan los parámetros definidos en el web.config en lugar de los definidos en el archivo de recursos.
                ViewBag.SupplierCode = ConfigurationManager.AppSettings["SupplierCode"];
                ViewBag.InsuredCode = ConfigurationManager.AppSettings["InsuredCode"];
                ViewBag.CoinsurerCode = ConfigurationManager.AppSettings["CoinsurerCode"];
                ViewBag.ThirdPartyCode = ConfigurationManager.AppSettings["ThirdPartyCode"];
                ViewBag.AgentCode = ConfigurationManager.AppSettings["AgentCode"];
                ViewBag.ProducerCode = ConfigurationManager.AppSettings["ProducerCode"];
                ViewBag.EmployeeCode = ConfigurationManager.AppSettings["EmployeeCode"];
                ViewBag.ReinsurerCode = ConfigurationManager.AppSettings["ReinsurerCode"];
                ViewBag.TradeConsultant = ConfigurationManager.AppSettings["TradeConsultant"];
                ViewBag.ContractorCode = ConfigurationManager.AppSettings["ContractorCode"];
                // Setear valor por default de la sucursal de acuerdo al usuario que se conecta 
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);
                ViewBag.ParameterMulticompany = _billingController.GetParameterMulticompany();
                // OBTIENE LA COMPANIA POR DEFECTO SEGÚN EL USUARIO
                ViewBag.AccountingCompanyDefault = DelegateService.accountingApplicationService.GetAccountingCompanyDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name));
                ViewBag.BranchId = TempData["BranchId"];
                ViewBag.PreliquidationId = TempData["PreliquidationId"];
                ViewBag.SalePointBranchUserDefault = DelegateService.accountingApplicationService.GetSalePointDefaultByUserIdAndBranchId(_commonController.GetUserIdByName(User.Identity.Name), ViewBag.BranchDefault);
                return View();
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// GetUserByUserName
        /// Obtiene un usuario por el nombre
        /// </summary>
        /// <param name="query"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetUserByUserName(string query)
        {
            List<object> usersResponses = new List<object>();
            List<User> users = DelegateService.uniqueUserService.GetUserByName(query);

            if (users != null)
            {
                for (int i = 0; i < users.Count; i++)
                {
                    usersResponses.Add(new
                    {
                        nick = users[i].AccountName.Trim(),
                        id = users[i].UserId
                    });
                }
            }
            return Json(usersResponses, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Obtiene las preliquidaciones
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="accountingCompanyId"></param>
        /// <param name="salesPointId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="preliquidationId"></param>
        /// <param name="personTypeId"></param>
        /// <param name="beneficiaryIndividualId"></param>
        /// <param name="userId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetPreliquidations(string branchId, string accountingCompanyId, string salesPointId,
                                               string startDate, string endDate, string preliquidationId,
                                               string personTypeId, string beneficiaryIndividualId,
                                               string userId)
        {

            List<object> preliquidationsResponses = new List<object>();

            if (preliquidationId == String.Empty)
            {
                preliquidationId = "-1";
            }

            DTOs.Search.PreliquidationsDTO preliquidation = new DTOs.Search.PreliquidationsDTO();
            preliquidation.BranchId = Convert.ToInt32(branchId);
            preliquidation.AccountingCompanyId = Convert.ToInt32(accountingCompanyId);
            preliquidation.SalesPointId = Convert.ToInt32(salesPointId);
            preliquidation.StartDate = startDate == "" ? startDate : startDate + " 00:00:00";
            preliquidation.EndDate = endDate == "" ? endDate : endDate + " 23:59:59";
            preliquidation.PreliquidationId = Convert.ToInt32(preliquidationId);
            preliquidation.PersonTypeId = Convert.ToInt32(personTypeId);
            preliquidation.BeneficiaryIndividualId = Convert.ToInt32(beneficiaryIndividualId);
            preliquidation.UsertId = Convert.ToInt32(userId);

            List<DTOs.Search.PreliquidationsDTO> preliquidationDTOs = DelegateService.accountingApplicationService.GetPreliquidations(preliquidation);

            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyNegativePattern = 1;

            // Se ordena para mostrar desde la más reciente hasta la primera
            List<DTOs.Search.PreliquidationsDTO> preliquidationOrders = (from DTOs.Search.PreliquidationsDTO item in preliquidationDTOs
                                                             orderby item.PreliquidationId descending
                                                             select item).ToList();

            if (preliquidationDTOs.Count > 0)
            {
                foreach (DTOs.Search.PreliquidationsDTO preliquidationsResult in preliquidationOrders)
                {
                    preliquidationsResponses.Add(new
                    {
                        PreliquidationId = preliquidationsResult.PreliquidationId,
                        Status = preliquidationsResult.Status,
                        StatusDescription = preliquidationsResult.StatusDescription,
                        BranchId = preliquidationsResult.BranchId,
                        BranchDescription = preliquidationsResult.BranchDescription,
                        UsertId = preliquidationsResult.UsertId,
                        UserName = preliquidationsResult.UserName.ToUpper(),
                        SalesPointId = preliquidationsResult.SalesPointId,
                        SalesPointDescription = preliquidationsResult.SalesPointDescription,
                        RegisterDate = preliquidationsResult.RegisterDate,
                        AccountingCompanyId = preliquidationsResult.AccountingCompanyId,
                        AccountingCompanyDescription = preliquidationsResult.AccountingCompanyDescription,
                        PersonTypeId = preliquidationsResult.PersonTypeId,
                        PersonTypeDescription = preliquidationsResult.PersonTypeDescription,
                        BeneficiaryIndividualId = preliquidationsResult.BeneficiaryIndividualId,
                        BeneficiaryDocumentNumber = preliquidationsResult.BeneficiaryDocumentNumber,
                        BeneficiaryName = preliquidationsResult.BeneficiaryName,
                        Description = preliquidationsResult.Description,
                        TempImputationId = preliquidationsResult.TempImputationId,
                        SourceId = preliquidationsResult.SourceId,
                        ImputationTypeId = preliquidationsResult.ImputationTypeId,
                        ImputationTypeDescription = preliquidationsResult.ImputationTypeDescription,
                        Rows = preliquidationsResult.Rows,
                        IsRealSource = -1,                        
                        TotalAmount= preliquidationsResult.TotalAmount < 0 ? preliquidationsResult.TotalAmount.ToString("C") : string.Format(new CultureInfo("en-US"), "{0:C}", preliquidationsResult.TotalAmount),
                    });
                }
            }
            return Json(new { aaData = preliquidationsResponses, total = preliquidationsResponses.Count }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// CancelPreliquidation
        /// Cancela una preliquidación por su Id
        /// </summary>
        /// <param name="preliquidationId"></param>
        /// <param name="tempImputationId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult CancelPreliquidation(int preliquidationId, int tempImputationId)
        {
            bool isCancelPreliquidation = false;
            isCancelPreliquidation = DelegateService.accountingApplicationService.CancelPreliquidation(preliquidationId, tempImputationId);
            return Json(isCancelPreliquidation, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetBeneficiary
        /// Método para obtener el Beneficiario en la búsqueda de preliquidaciones (AUTOCOMPLETE)
        /// </summary>
        /// <param name="query"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetBeneficiary(string query, string param)
        {
            int beneficiaryType = 0;
            bool isByDocumentNumber = true;
            string searchData = (query == "" ? "0" : Convert.ToString(query));
            if (param != null)
            {
                string[] data = param.Split('/');
                beneficiaryType = data[0] == "" ? 0 : Convert.ToInt32(data[0]);
                isByDocumentNumber = data[1] == "" ? true : Convert.ToBoolean(data[1]);
            }

            List<object> personResults = new List<object>();

            try
            {
                if (beneficiaryType != 0)
                {
                    #region SupplierCode

                    if (beneficiaryType == Convert.ToInt32(ConfigurationManager.AppSettings["SupplierCode"]))
                    {
                        List<IndividualDTO> suppliers;

                        if (isByDocumentNumber)
                        {
                            suppliers = DelegateService.tempCommonService.GetSuppliersByDocumentNumber(searchData);
                        }
                        else
                        {
                            suppliers = DelegateService.tempCommonService.GetSuppliersByName(searchData);
                        }

                        if (suppliers.Count > 0)
                        {
                            foreach (var supplier in suppliers)
                            {
                                personResults.Add(new
                                {
                                    IndividualId = supplier.IndividualId,
                                    Name = supplier.Name,
                                    DocumentNumber = supplier.DocumentNumber,
                                    DisplayValue = supplier.DocumentNumber + ": " + supplier.Name
                                });
                            }
                        }
                    }
                    #endregion

                    #region Insured

                    if (beneficiaryType == Convert.ToInt32(ConfigurationManager.AppSettings["InsuredCode"]))
                    {
                        List<IndividualDTO> insureds;

                        if (isByDocumentNumber)
                        {
                            insureds = DelegateService.tempCommonService.GetInsuredByDocumentNumber(searchData); // Por número de documento
                        }
                        else
                        {
                            insureds = DelegateService.tempCommonService.GetInsuredByName(searchData); // Por nombre
                        }

                        if (insureds.Count > 0)
                        {
                            foreach (var insured in insureds)
                            {
                                personResults.Add(new
                                {
                                    IndividualId = insured.IndividualId,
                                    Name = insured.Name.Trim(),
                                    DocumentNumber = insured.DocumentNumber,
                                    DisplayValue = insured.DocumentNumber + ": " + insured.Name.Trim()
                                });
                            }
                        }
                    }

                    #endregion

                    #region Coinsurer

                    if (beneficiaryType == Convert.ToInt32(ConfigurationManager.AppSettings["CoinsurerCode"]))
                    {
                        List<IndividualDTO> reinsurers;

                        if (isByDocumentNumber)
                        {
                            reinsurers = DelegateService.tempCommonService.GetReinsurerByDocumentNumber(searchData, 14);
                        }
                        else
                        {
                            reinsurers = DelegateService.tempCommonService.GetReinsurerByName(searchData, 14,0);
                        }

                        if (reinsurers.Count > 0)
                        {
                            foreach (var company in reinsurers)
                            {
                                personResults.Add(new
                                {
                                    IndividualId = company.IndividualId,
                                    Name = company.Name,
                                    DocumentNumber = company.DocumentNumber,
                                    DisplayValue = company.DocumentNumber + ": " + company.Name
                                });
                            }
                        }
                    }

                    #endregion

                    #region ThirdParty

                    if (beneficiaryType == Convert.ToInt32(ConfigurationManager.AppSettings["ThirdPartyCode"]))
                    {
                        List<IndividualDTO> persons;

                        if (isByDocumentNumber)
                        {
                            persons = DelegateService.tempCommonService.GetPersonsByDocumentNumber(searchData);
                        }
                        else
                        {
                            persons = DelegateService.tempCommonService.GetPersonsByName(searchData);
                        }

                        if (persons.Count > 0)
                        {
                            foreach (var person in persons)
                            {
                                personResults.Add(new
                                {
                                    IndividualId = person.IndividualId,
                                    Name = person.Name,
                                    DocumentNumber = person.DocumentNumber,
                                    DisplayValue = person.DocumentNumber + ": " + person.Name
                                });
                            }
                        }
                    }
                    #endregion

                    #region Agent

                    if (beneficiaryType == Convert.ToInt32(ConfigurationManager.AppSettings["AgentCode"]))
                    {
                        List<AgentDTO> agents;

                        if (isByDocumentNumber)
                        {
                            agents = DelegateService.tempCommonService.GetAgentByDocumentNumber(searchData);
                        }
                        else
                        {
                            agents = DelegateService.tempCommonService.GetAgentByName(searchData);
                        }

                        if (agents.Count > 0)
                        {
                            foreach (AgentDTO agent in agents)
                            {
                                personResults.Add(new
                                {
                                    IndividualId = agent.IndividualId,
                                    Name = agent.Name,
                                    DocumentNumber = agent.DocumentNumber,
                                    DisplayValue = agent.DocumentNumber + ": " + agent.Name
                                });
                            }
                        }
                    }
                    #endregion

                    #region Producer

                    if (beneficiaryType == Convert.ToInt32(ConfigurationManager.AppSettings["ProducerCode"]))
                    {
                        List<AgentDTO> agents;

                        if (isByDocumentNumber)
                        {
                            agents = DelegateService.tempCommonService.GetAgentByDocumentNumber(searchData);
                        }
                        else
                        {
                            agents = DelegateService.tempCommonService.GetAgentByName(searchData);
                        }

                        if (agents.Count > 0)
                        {
                            foreach (AgentDTO agent in agents)
                            {
                                personResults.Add(new
                                {
                                    IndividualId = agent.IndividualId,
                                    Name = agent.Name,
                                    DocumentNumber = agent.DocumentNumber,
                                    DisplayValue = agent.DocumentNumber + ": " + agent.Name
                                });
                            }
                        }
                    }
                    #endregion

                    #region Employee

                    if (beneficiaryType == Convert.ToInt32(ConfigurationManager.AppSettings["EmployeeCode"]))
                    {
                        List<CoreUniquePersonModels.EmployeePerson> persons;

                        if (isByDocumentNumber)
                        {
                            //persons = _uniquePersonService.GetEmployee(searchData, 1); // Por número de documento
                            persons = DelegateService.uniquePersonServiceV1.GetEmployeePersons().Where(em => em.IdCardNo.Contains(query)).ToList();
                        }
                        else
                        {
                            //persons = _uniquePersonService.GetEmployee(searchData, 2); // Por nombre
                            persons = DelegateService.uniquePersonServiceV1.GetEmployeePersons().Where(em => em.Name.Contains(query)).ToList();
                        }

                        if (persons.Count > 0)
                        {
                            foreach (CoreUniquePersonModels.Base.BaseEmployeePerson person in persons)
                            {
                                personResults.Add(new
                                {
                                    Name = person.Name.Trim(),
                                    IndividualId = person.Id,
                                    DocumentNumber = person.IdCardNo,
                                    DisplayValue = person.IdCardNo + ": " + person.Name.Trim()
                                });
                            }
                        }
                    }
                    #endregion

                    #region Reinsurer

                    if (beneficiaryType == Convert.ToInt32(ConfigurationManager.AppSettings["ReinsurerCode"]))
                    {
                        List<IndividualDTO> reinsurers;

                        if (isByDocumentNumber)
                        {
                            reinsurers = DelegateService.tempCommonService.GetReinsurerByDocumentNumber(searchData, 14);
                        }
                        else
                        {
                            reinsurers = DelegateService.tempCommonService.GetReinsurerByName(searchData, 14,0);
                        }

                        if (reinsurers.Count > 0)
                        {
                            foreach (var company in reinsurers)
                            {
                                personResults.Add(new
                                {
                                    Name = company.Name.Trim(),
                                    IndividualId = company.IndividualId,
                                    DocumentNumber = company.DocumentNumber,
                                    DisplayValue = company.DocumentNumber + ": " + company.Name.Trim()
                                });
                            }
                        }
                    }
                    #endregion

                    #region TradeConsultant

                    if (beneficiaryType == Convert.ToInt32(ConfigurationManager.AppSettings["TradeConsultant"]))
                    {
                        List<IndividualDTO> persons;

                        if (isByDocumentNumber)
                        {
                            persons = DelegateService.tempCommonService.GetPersonsByDocumentNumber(searchData);
                        }
                        else
                        {
                            persons = DelegateService.tempCommonService.GetPersonsByName(searchData);
                        }

                        if (persons.Count > 0)
                        {
                            foreach (var person in persons)
                            {
                                personResults.Add(new
                                {
                                    IndividualId = person.IndividualId,
                                    Name = person.Name,
                                    DocumentNumber = person.DocumentNumber,
                                    DisplayValue = person.DocumentNumber + ": " + person.Name.Trim()
                                });
                            }
                        }
                    }
                    #endregion

                    #region Contractor - default Beneficiary

                    if (beneficiaryType == Convert.ToInt32(ConfigurationManager.AppSettings["ContractorCode"]))
                    {
                        List<IndividualDTO> persons;

                        if (isByDocumentNumber)
                        {
                            persons = DelegateService.tempCommonService.GetPersonsByDocumentNumber(searchData);
                        }
                        else
                        {
                            persons = DelegateService.tempCommonService.GetPersonsByName(searchData);
                        }

                        if (persons.Count > 0)
                        {
                            foreach (var person in persons)
                            {
                                personResults.Add(new
                                {
                                    IndividualId = person.IndividualId,
                                    Name = person.Name,
                                    DocumentNumber = person.DocumentNumber,
                                    DisplayValue = person.DocumentNumber + ": " + person.Name.Trim()
                                });
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    personResults.Add(new
                    {
                        IndividualId = 0,
                        Name = "",
                        DocumentNumber = 0,
                        DisplayValue = @Global.RegisterNotFound
                    });
                }
            }
            catch (Exception)
            {
                personResults.Add(new
                {
                    IndividualId = 0,
                    Name = "",
                    DocumentNumber = 0,
                    DisplayValue = @Global.RegisterNotFound
                });
            }
            return Json(personResults, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// LoadPreliquidation
        /// Método para edición de preliquidaciones
        /// </summary>
        /// <param name="preliquidationId"></param>
        /// <param name="branchId"></param>
        /// <param name="salePointId"></param>
        /// <param name="companyId"></param>
        /// <param name="generationDate"></param>
        /// <param name="personTypeId"></param>
        /// <param name="documentNumber"></param>
        /// <param name="name"></param>
        /// <param name="beneficiaryId"></param>
        /// <param name="tempImputationId"></param>
        /// <param name="description"></param>
        /// <param name="isPreliquidation"></param>
        /// <returns>RedirectToRouteResult</returns>
        public RedirectToRouteResult LoadPreliquidation(string preliquidationId, string branchId, string salePointId, string companyId,
                                                        string generationDate, string personTypeId, string documentNumber,
                                                        string name, string beneficiaryId, string tempImputationId,
                                                        string description, string isPreliquidation)
        {
            //Flag para indicar que es edición
            TempData["IsEdition"] = 1;
            //Flag para indicar si la edicion es por preliquidacion o temporal
            TempData["IsPreliquidation"] = isPreliquidation;
            TempData["BranchId"] = branchId;
            TempData["SalePointId"] = salePointId;
            TempData["CompanyId"] = companyId;
            TempData["GenerationDate"] = generationDate;
            TempData["PersonTypeId"] = personTypeId;
            TempData["DocumentNumber"] = documentNumber;
            TempData["Name"] = name;
            TempData["BeneficiaryId"] = beneficiaryId;
            TempData["TempImputationId"] = tempImputationId;
            TempData["PreliquidationId"] = preliquidationId;
            TempData["Description"] = description;
            return RedirectToAction("MainPreLiquidations");
        }

        /// <summary>
        /// LoadPreliquidationSearch
        /// </summary>
        /// <returns>RedirectToRouteResult</returns>
        public RedirectToRouteResult LoadPreliquidationSearch(int branchId, int preliquidationId)
        {
            TempData["BranchId"] = branchId;
            TempData["PreliquidationId"] = preliquidationId;
            return RedirectToAction("MainPreLiquidationsSearch");
        }
        #endregion

    }
}