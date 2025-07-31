using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Configuration;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Resources;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Application.ClaimServices.DTOs.PaymentRequest;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Application.TaxServices.DTOs;
using CrystalDecisions.Shared;
using Sistran.Core.Framework.UIF.Web.Areas.Claims.Models.Reports;
using CrystalDecisions.CrystalReports.Engine;
using Sistran.Core.Framework.UIF.Web.Areas.Claims.Models.Reports.PaymentRequest;
using Sistran.Core.Application.ClaimServices.DTOs.Claims;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Framework.UIF.Web.Areas.Claims.Models;
using Sistran.Company.Application.ExternalProxyServices.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Claims.Business;

namespace Sistran.Core.Framework.UIF.Web.Areas.Claims.Controllers.PaymentRequestController
{
    public class PaymentRequestController : Controller
    {
        private static List<Sistran.Core.Application.CommonService.Models.Parameter> parameters = new List<Sistran.Core.Application.CommonService.Models.Parameter>();

        // GET: Claims/PaymentRequest
        public ActionResult PaymentRequest()
        {
            return View();
        }

        public ActionResult RecoveryRequest()
        {
            return View();
        }

        public PartialViewResult ConceptTaxesDetail()
        {
            return PartialView();
        }

        public PartialViewResult TaxesDuplicateDetail()
        {
            return PartialView();
        }
        /// <summary>
        /// Obtiene el Ramo
        /// </summary>
        /// <returns></returns>
        public ActionResult GetPrefixes()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.paymentRequestApplicationService.GetPrefixes().OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPrefixes);
            }
        }
        /// <summary>
        /// Obtiene la Sucursal de Pago
        /// </summary>
        /// <returns></returns>
        public ActionResult GetBranches()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.paymentRequestApplicationService.GetBranchesByUserId(SessionHelper.GetUserId()).OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetBranches);
            }
        }
        /// <summary>
        /// Obtiene la Moneda de Pago
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCurrency()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.paymentRequestApplicationService.GetCurrencies());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetCurrency);
            }
        }
        /// <summary>
        /// Obtiene el Origen de Pago
        /// </summary>
        /// <returns></returns>
        public ActionResult GetPaymentSource()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.paymentRequestApplicationService.GetPaymentSource().OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPaymentSource);
            }
        }
        /// <summary>
        /// Obtiene los Tipos de Movimientos
        /// </summary>
        /// <returns></returns>
        public ActionResult GetPaymentMovementTypesByPaymentSourceId(int sourceId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.paymentRequestApplicationService.GetMovementTypesByConceptSourceId(sourceId).OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPaymentMovementTypesByPaymentSourceId);
            }
        }

        /// <summary>
        /// Obtiene los ids de los tipos de estimación para un tipo de movimiento
        /// </summary>
        /// <param name="movementTypeId"></param>
        /// <returns></returns>
        public ActionResult GetEstimationsTypesIdByMovementTypeId(int movementTypeId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.paymentRequestApplicationService.GetEstimationsTypesIdByMovementTypeId(movementTypeId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.Error);
            }
        }

        /// <summary>
        /// Obtiene a quien desea pagar
        /// </summary>
        /// <returns></returns>
        public ActionResult GetPersonTypes()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.paymentRequestApplicationService.GetPersonTypes(true).OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPersonTypes);
            }
        }
        /// <summary>
        /// Obtiene los Tipos de Comprobantes
        /// </summary>
        /// <returns></returns>
        public ActionResult GetVoucherType()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.paymentRequestApplicationService.GetVoucherTypes().OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetVoucherType);
            }
        }
        /// <summary>
        /// Obtiene los Tipos de Comprobantes
        /// </summary>
        /// <returns></returns>
        public ActionResult GetPaymentMethod()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.paymentRequestApplicationService.GetPaymentMethods().OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPaymentMethod);
            }
        }

        public ActionResult GetExchangeRate(int currencyId)
        {
            try
            {
                if (!parameters.Any())
                {
                    parameters = this.GetParameters();
                }

                if (currencyId != (int)parameters.First(x => x.Description == "Currency").Value)
                {
                    return new UifJsonResult(true, DelegateService.paymentRequestApplicationService.GetExchangeRateByRateDateCurrencyId(DateTime.Now, currencyId));
                }
                else
                {
                    return new UifJsonResult(true, new ExchangeRateDTO
                    {
                        Currency = new CurrencyDTO
                        {
                            Id = currencyId
                        },
                        RateDate = DateTime.Now,
                        SellAmount = 1
                    });
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetExchangeRate);
            }
        }

        public ActionResult GetAccountingConceptsByBranchIdMovementTypeIdPersonTypeIdIndividualId(int brachId, int movementTypeId, int personTypeId, int individualId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.paymentRequestApplicationService.GetAccountingConceptsByBranchIdMovementTypeIdPersonTypeIdIndividualId(brachId, movementTypeId, personTypeId, individualId).OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPaymentConceptsByBranchIdMovementTypeId);
            }
        }

        public ActionResult GetModuleDateByModuleTypeMovementDate()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.paymentRequestApplicationService.GetModuleDateByModuleTypeMovementDate(ModuleType.Claim, DateTime.Now));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetModuleDateByModuleTypeMovementDate);
            }
        }

        public UifJsonResult GetClaimByPrefixIdBranchIdPolicyDocumentNumberClaimNumber(int? prefixId, int? branchId, string policyDocumentNumber, int claimNumber)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetSubClaimsEstimationByPrefixIdBranchIdPolicyDocumentNumberClaimNumber(prefixId, branchId, policyDocumentNumber, claimNumber));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetClaimByPrefixIdBranchIdClaimNumber);
            }
        }

        public JsonResult GetInsuredByNameDocumentNumber(string query)
        {
            try
            {
                return Json(DelegateService.claimApplicationService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(query, InsuredSearchType.DocumentNumber, CustomerType.Individual), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(App_GlobalResources.Language.ErrorGetInsuredByNameDocumentNumber, JsonRequestBehavior.DenyGet);
            }
        }

        public JsonResult GetSuppliersByNameDocumentNumber(string query)
        {
            try
            {
                return Json(DelegateService.claimApplicationService.GetSuppliersByDescriptionInsuredSearchTypeCustomerType(query, InsuredSearchType.DocumentNumber, CustomerType.Individual), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(App_GlobalResources.Language.ErrorGetSuppliersByNameDocumentNumber, JsonRequestBehavior.DenyGet);
            }
        }

        public JsonResult GetInsuredsByIndividualId(int individualId)
        {
            try
            {
                return Json(DelegateService.claimApplicationService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(Convert.ToString(individualId), Core.Services.UtilitiesServices.Enums.InsuredSearchType.IndividualId, Core.Services.UtilitiesServices.Enums.CustomerType.Individual), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(App_GlobalResources.Language.ErrorGetInsuredsByDescriptionInsuredSearchTypeCustomerType, JsonRequestBehavior.DenyGet);
            }
        }

        public UifJsonResult GetInsuredByRiskId(int riskId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetInsuredByRiskId(riskId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetInsuredsByDescriptionInsuredSearchTypeCustomerType);
            }
        }

        public UifJsonResult SavePaymentRequest(PaymentRequestDTO paymentRequestDTO)
        {
            try
            {
                paymentRequestDTO.UserId = SessionHelper.GetUserId();
                paymentRequestDTO.PaymentRequestTypeId = (int)PaymentRequestType.Payment;
                paymentRequestDTO.RegistrationDate = DateTime.Now;

                return new UifJsonResult(true, DelegateService.paymentRequestApplicationService.CreatePaymentRequest(paymentRequestDTO));
            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSavePaymentRequest);
            }
        }

        public UifJsonResult GetPaymentRequestByPaymentRequestId(int paymentRequestId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.paymentRequestApplicationService.GetPaymentRequestByPaymentRequestId(paymentRequestId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPaymentRequestByPaymentRequestId);
            }
        }


        public UifJsonResult GetIndividualTaxesByIndividualIdRoleId(int individualId, int roleId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetIndividualTaxesByIndividualIdRoleId(individualId, roleId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetIndividualTaxesByIndividualId);
            }
        }

        public UifJsonResult GetPolicyByEndorsementIdModuleType(int endorsementId)
        {
            try
            {
                PolicyDTO policyDTO = DelegateService.claimApplicationService.GetPolicyByEndorsementIdModuleType(endorsementId);
                if (!DelegateService.commonService.GetParameterByParameterId(12191).BoolParameter.GetValueOrDefault())
                {
                    policyDTO.IsReinsurance = ClaimBusiness.GetPolicyReinsurance2G(policyDTO);
                }

                return new UifJsonResult(true, policyDTO);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPolicyByEndorsementIdModuleType);
            }
        }

        public UifJsonResult GetDefaultPaymentCurrency()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.paymentRequestApplicationService.GetDefaultPaymentCurrency());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetDefaultCurrency);
            }
        }

        public UifJsonResult GetBranchDescriptionByBranchId(int branchId)
        {
            return new UifJsonResult(true, DelegateService.paymentRequestApplicationService.GetBranchById(branchId).Description);
        }

        public UifJsonResult GetPrefixByPrefixId(int prefixId)
        {
            return new UifJsonResult(true, DelegateService.paymentRequestApplicationService.GetPrefixByPrefixId(prefixId).Description);
        }

        public ActionResult GetSearchPersonTypes(int prefixId, int searchType)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.paymentRequestApplicationService.GetClaimSearchPersonType(prefixId, searchType).OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPersonTypes);
            }
        }

        public UifJsonResult CalculatePaymentTaxesByIndividualIdAccountingConceptIdTaxAttributesAmount(int individualId, int accountingConceptId, List<TaxAttributeDTO> attributes, decimal amount)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.taxService.CalculatePaymentTaxesByIndividualIdAccountingConceptIdTaxAttributesAmount(individualId, accountingConceptId, attributes, amount));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetTax);
            }
        }

        public UifJsonResult GetTaxAttributes()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.taxService.GetTaxAttributes());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetTaxAttributes);
            }
        }

        public UifJsonResult GetCountries()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetCountries());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetCountries);
            }
        }

        public UifJsonResult GetStatesByCountryId(int countryId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetStatesByCountryId(countryId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetStatesByCountryId);
            }
        }

        public UifJsonResult GetCitiesByCountryIdStateId(int countryId, int stateId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetCitiesByCountryIdStateId(countryId, stateId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetCitiesByCountryIdStateId);
            }
        }

        public UifJsonResult GetDefaultCountry()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.commonService.GetParameterByDescription("DefaultCountryId").NumberParameter);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSetDefaultCountry);
            }

        }

        public UifJsonResult GetTaxCodeOfRetetionToIndustryAndCommerce()
        {
            return new UifJsonResult(true, DelegateService.paymentRequestApplicationService.GetTaxCodeOfRetetionToIndustryAndCommerce());
        }

        public void GetReportPaymentRequestByPaymentRequestId(int paymentRequestId)
        {
            try
            {
                List<PaymentRequestReportModel> paymentRequestReportModels = new List<PaymentRequestReportModel>();
                List<PaymentRequestDetailReportModel> paymentRequestDetailReportModels = new List<PaymentRequestDetailReportModel>();
                List<PaymentRequestTaxReportModel> paymentRequestTaxReportModels = new List<PaymentRequestTaxReportModel>();
                List<PaymentRequestAccountingReportModel> paymentRequestAccountingReportModels = new List<PaymentRequestAccountingReportModel>();
                List<PaymentRequestCoInsuranceReportModel> paymentRequestCoInsuranceReportModels = new List<PaymentRequestCoInsuranceReportModel>();

                PaymentRequestReportDTO paymentRequestReportDTO = DelegateService.paymentRequestApplicationService.GetReportPaymentRequestByPaymentRequestId(paymentRequestId);

                if (paymentRequestReportDTO != null)
                {
                    PaymentRequestReportModel paymentRequestReportModel = ModelAssembler.CreatePaymentRequestReportModel(paymentRequestReportDTO);                    
                    paymentRequestReportModels.Add(paymentRequestReportModel);
                    
                    paymentRequestDetailReportModels = ModelAssembler.CreatePaymentRequestDetailsReportModel(paymentRequestReportDTO.Claims);

                    paymentRequestTaxReportModels = ModelAssembler.CreatePaymentRequestTaxesReportModel(paymentRequestReportDTO.Taxes);
                    paymentRequestCoInsuranceReportModels = ModelAssembler.CreatePaymentRequestCoInsurancesReportModel(paymentRequestReportDTO.Coinsurances);
                    paymentRequestAccountingReportModels = ModelAssembler.CreatePaymentRequestAccountingReportModels(paymentRequestReportDTO.Accountings);
                    ReportDocument reportDocument = new ReportDocument();
                    ClaimsReportModel reportModel = new ClaimsReportModel();
                    var reportName = "Areas//Claims//Reports//PaymentRequest//PaymentRequestReport.rpt";
                    var pdfName = "Content\\file.pdf";
                    string reportPath = System.Web.HttpContext.Current.Server.MapPath("~/") + reportName;
                    string pdfPath = System.Web.HttpContext.Current.Server.MapPath("~/") + pdfName;
                    reportModel.SummaryPaymentRequestReport = paymentRequestReportModels;
                    var dataSource = reportModel.SummaryPaymentRequestReport;
                    var dataSourceCoInsured = paymentRequestCoInsuranceReportModels;
                    var dataSourceDetail = paymentRequestDetailReportModels;
                    var dataSourceTax = paymentRequestTaxReportModels;
                    var dataSourceAccounting = paymentRequestAccountingReportModels;
                    reportDocument.Load(reportPath);
                    reportDocument.SetDataSource(dataSource);
                    reportDocument.Subreports[0].SetDataSource(dataSourceAccounting);
                    reportDocument.Subreports[1].SetDataSource(dataSourceCoInsured);
                    reportDocument.Subreports[2].SetDataSource(dataSourceDetail);
                    reportDocument.Subreports[3].SetDataSource(dataSourceTax);
                    reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response,
                                                        false, "PaymentsRequest");
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        private string GetPersonTypeById(int id)
        {
            return DelegateService.paymentRequestApplicationService.GetPersonTypes(true).FirstOrDefault(x => x.Id == id).Description;
        }

        private List<Sistran.Core.Application.CommonService.Models.Parameter> GetParameters()
        {
            parameters.Add(new Sistran.Core.Application.CommonService.Models.Parameter { Description = "Currency", Value = DelegateService.commonService.GetParameterByParameterId((int)ParametersTypes.Currency).NumberParameter.Value });

            return parameters;
        }
    }
}