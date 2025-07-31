var JuridicPerson = 0;
var paymentRequest = {};
var selectedClaim = {};
var rowsConcept = [];
var rowsVoucher = [];
var estimationsTypesId = [];
var currentEditConceptIndex = 0;
var currentEditVoucherIndex = 0;
var currentEditSubClaimSummaryIndex = 0;
var currentPaymentConceptId = 0;
var currentInputConceptValue = 0;
var currentPaymentVoucherNumber = 0;
var currentPositionClaim = 0;
var currentPositionVoucher = 0;
var currentCoverageId = 0;
var currentClaim = {};
var ClaimId = 0;
var SubClaimId = 0;
var ClaimNumber = 0;
var EstimationTypeId = 0;
var IndividualId = 0;
var IndividualDocumentTypeId = 0;
var isCreatePaymentRequestModel = 0;
var endorsementId = null;
var policyProductId = 0;
var policySalePointId = 0;
var policyId = null;
var Estimation = null;
var IsTotalParticipation = null;
var TaxAttributes = [];
var RTEICACode = 0;
var IndividualTaxCondition = [];
var paymentCurrencyExchangeRate = 0;
var claimCurrencyExchangeRate = 0;
//Objeto para almacenar los TaxCategories
var oTax = { TaxCategory: [] };
var defaultPaymentCurrency = 0;
var exchangeRate = null;

//Modelo para edición de Múltiples Conceptos de Pago
function RowMultiPaymentConcept() {
    this.VoucherType;
    this.VoucherTypeDescription;
    this.VoucherNumber;
    this.Date;
    this.Currency;
    this.CurrencyDescription;
    this.ExchangeRate;
    this.ConceptValue;
    this.TaxValue;
    this.Id;
}

var ViewListConceptTaxes;
var ViewListConceptTaxesDuplicates;

var taxesDuplicates = [];
var conceptWithTaxesDuplicates = {};
var allTaxes = [];

class Payment extends Uif2.Page {
    getInitialState() {
        ViewListConceptTaxes = uif2.dropDown({
            source: rootPath + "Claims/PaymentRequest/ConceptTaxesDetail",
            element: "#tblPaymentConcept",
            align: "left",
            direction: "bottom",
            container: '#modalSingleBillingInformationDialog',
            width: 500,
            height: 300,
            loadedCallback: function () {
                $("#btnAcceptConceptTaxesDetails").on('click', Payment.HideConceptTaxDetail);

                $('#conceptTaxDetail').on('visibility', function () {
                    var $element = $(this);
                    var timer = setInterval(function () {
                        if ($element.is(':hidden')) {
                            $("#tblSingleBillingInformation").UifDataTable('unselect');
                        }
                    }, 100);
                }).trigger('visibility');
            }
        });

        ViewListConceptTaxesDuplicates = uif2.dropDown({
            source: rootPath + "Claims/PaymentRequest/TaxesDuplicateDetail",
            element: "#tblPaymentConcept",
            align: "left",
            direction: "bottom",
            container: '#modalSingleBillingInformationDialog',
            width: 800,
            height: 300,
            loadedCallback: function () {
                $("#btnAcceptTaxesDuplicatesDetails").on('click', Payment.ValidateSelectedTaxes);
            }
        });

        Payment.GetMethods();
        $("#inpuntTotalAmount").OnlyDecimals(2);
        $("#inputConceptValue").OnlyDecimals(2);
        $("#inpuntTotalAmount").ValidatorKey(ValidatorType.Decimal, 2, 1);
        $("#inputConceptValue").ValidatorKey(ValidatorType.Decimal, 2, 1);
        $("#inputPolicyDocumentNumber").ValidatorKey(ValidatorType.Number, 2, 1);
        $("#inputClaimNumber").ValidatorKey(ValidatorType.Number, 2, 1);

        $('#inputRegistrationDate').val(moment().format("DD/MM/YYYY"));
        $("#btnTaxStatus").prop('disabled', true);
        $('#inputDateAssignment').OnlyDecimals(0);
        $('#inputAuthorizedDocument1').OnlyDecimals(0);
        $('#inputAuthorizedDocument2').OnlyDecimals(0);
        $('#inputAccountnumber').OnlyDecimals(0);
        $('#inputConfirmDocument').OnlyDecimals(0);
        $('#inputAccountNumber').OnlyDecimals(0);
        $('#inputValuePayment').OnlyDecimals(0);
        $('.hideSearchClaim').hide();
        $("#divRteICA").hide();
    }

    /** EVENTOS**/
    bindEvents() {
        $("#inpuntTotalAmount").focusin(Payment.NotFormatMoneyIn);
        $("#inpuntTotalAmount").focusout(Payment.FormatMoneyOut);
        $("#inputConceptValue").focusin(Payment.NotFormatMoneyIn);
        $("#inputConceptValue").focusout(Payment.FormatMoneyOut);
        $("#_chkMultipleBilling").on('click', Payment.ShowFormBilling);
        $('#btnSavePaymentRequest').on('click', Payment.CreatePaymentRequest);
        $('#btnPrintPaymentRequest').on('click', Payment.CreatePrintPaymentRequest);
        $('#btnSearchClaim').on('click', Payment.GetClaimByPrefixIdBranchIdPolicyDocumentNumberClaimNumber);

        $('#tblClaims').on('rowSelected', Payment.ShowModalPaymentConcept);
        $('#tblPaymentConcept').on('rowSelected', Payment.CreateConcepts);
        $('#tblPaymentConcept').on('rowDeselected', Payment.DeselectConcept);
        $('#tblSingleBillingInformation').on('rowSelected', Payment.ViewConceptTaxDetails);
        $('#tblSingleBillingInformation').on('rowDelete', Payment.DeleteConcept);
        $('#tblSingleBillingInformation').on('rowEdit', Payment.EditConcept);
        $('#tblSummaryClaims').on('rowSelected', Payment.EditClaimSummaryVouchers);
        $('#editAction').on('Save', Payment.AddConcepts);
        $('#editActionMulti').on('Save', Payment.CreateVoucher);

        $('#btnPaymentConcept').on('click', Payment.SaveConcepts);
        $('#btnPaymentConceptClose').on('click', Payment.CloseConcepts);
        $("#DocumentNumberInsured").on('itemSelected', Payment.SetInsuredInformation);
        $("#FullNameInsured").on('itemSelected', Payment.SetInsuredInformation);
        $("#DocumentNumberSupplier").on('itemSelected', Payment.SetSupplierInformation);
        $("#FullNameSupplier").on('itemSelected', Payment.SetSupplierInformation);
        $("#InputDocumentNumberThirdParty").on("keydown", Payment.ClearParticipationInformation);
        $("#InputFullNameThirdParty").on("keydown", Payment.ClearParticipationInformation);
        $("#InputDocumentNumberThirdParty").on('itemSelected', Payment.SetParticipationInformation);
        $("#InputFullNameThirdParty").on('itemSelected', Payment.SetParticipationInformation);
        $('#tblBillingInformation').on('rowAdd', Payment.AddVouchers);
        $('#tblBillingInformation').on('rowEdit', Payment.EditVouchers);
        $('#tblBillingInformation').on('rowDelete', Payment.DeleteVoucher);
        $('#tblBillingInformation').on('rowSelected', Payment.ShowModalPaymentConceptVoucher);
        $('#tblBillingInformation').on('rowEdit', Payment.EditSubClaimSummaryVouchers);
        $("#modalSingleBillingInformationDialog").on('hidden.bs.modal', Payment.ClearItemSelect);
        $('#btnMultiplePaymentConcept').on('click', Payment.SaveVouchers);
        $('#btnAcceptPaymentMethods').on('click', Payment.AcceptPaymentMethods);
        $('#btnAcceptClaimCheck').on('click', Payment.AcceptClaimCheck);
        $('#btnAcceptClaimViaBaloto').on('click', Payment.AcceptClaimViaBaloto);
        $('#btnAcceptClaimpaymentoffice').on('click', Payment.AcceptClaimpaymentoffice);
        $('#btnTaxStatus').on('click', Payment.OpenTaxCondition);

        $('#selectPayTo').on('itemSelected', Payment.ShowDivPerson);
        $('#selectMovementType').on('itemSelected', Payment.GetAccountingConceptsByBranchIdMovementTypeIdPersonTypeIdIndividualId);
        $('#selectPaymentBranch').on('itemSelected', Payment.GetAccountingConceptsByBranchIdMovementTypeIdPersonTypeIdIndividualId);
        $('#selectInvoiceCurrency').on('itemSelected', Payment.GetExchangeRate);
        $('#Currency').on('itemSelected', Payment.GetExchangeRate);
        $('#selectPaymentCurrency').on('itemSelected', Payment.ChangePaymentCurrency);
        $('#CountryLiquidationICA').on('itemSelected', Payment.GetStatesByCountry);
        $('#StateLiquidationICA').on('itemSelected', Payment.GetCititesByCountryState);
        $('#inputDescription').TextTransform(ValidatorType.UpperCase);
    }

    /** METODOS**/
    static GetMethods() {
        if (modelSearchCriteria.claimNumber == null && modelSearchCriteria.policyDocumentNumber == null && modelSearchCriteria.prefixId == null && modelSearchCriteria.branchId == null && modelSearchCriteria.paymentRequestId == null) {
            Payment.GetPrefixes();
            Payment.GetBranches();
        }
        
        if (modelSearchCriteria.paymentRequestId == null) {
            Payment.GetCurrency();
            Payment.GetPaymentSource();
            Payment.GetPersonTypes();
            Payment.GetVoucherType();
            Payment.GetPaymentMethod();
            $("#tblSingleBillingInformation").UifDataTable({ edit: true, delete: true });
            $("#tblBillingInformation").UifDataTable({ add: true, edit: true, delete: true });
        }

        Payment.DoPayClaim();
        Payment.DoConsultPaymentRequest();
        Payment.MovementDate();
        Payment.GetTaxAttributes();
        Payment.GetTaxCodeOfRetetionToIndustryAndCommerce();
    }

    static GetPrefixes(callback) {
        ClaimsPaymentRequest.GetPrefixes().done(function (response) {
            if (response.success) {
                if (callback)
                    return callback(response.result);
                $('#selectPrefixesofClaims').UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static GetBranches(callback) {
        ClaimsPaymentRequest.GetBranches().done(function (response) {
            if (response.success) {
                if (callback)
                    return callback(response.result);

                $('#selectPaymentBranch').UifSelect({ sourceData: response.result });
                $('#selectBranch').UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static GetCurrency(callback) {
        ClaimsPaymentRequest.GetCurrency().done(function (response) {
            if (response.success) {
                if (callback)
                    return callback(response.result);
                var currencies = response.result;
                ClaimsPaymentRequest.GetDefaultPaymentCurrency().done(function (data) {
                    if (data.success) {
                        $('#selectPaymentCurrency').UifSelect({ sourceData: currencies });
                        $('#selectPaymentCurrency').UifSelect('setSelected', data.result);
                        $('#selectPaymentCurrency').trigger('change');
                        $('#selectInvoiceCurrency').UifSelect({ sourceData: currencies, enable: false });
                        $('#selectInvoiceCurrency').UifSelect('setSelected', data.result);
                        $('#selectInvoiceCurrency').trigger('change');
                        $('#Currency').UifSelect({ sourceData: currencies, enable: false});
                        $('#Currency').UifSelect('setSelected', data.result);
                        $('#Currency').trigger('change');
                        defaultPaymentCurrency = data.result;
                    }
                });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorGetCurrencies, 'autoclose': true });
            }
        });
    }

    static GetPaymentSource(callback) {
        ClaimsPaymentRequest.GetPaymentSource().done(function (response) {
            if (response.success) {
                if (callback)
                    return callback(response.result);
                $('#selectPaymentSource').UifSelect({ sourceData: response.result, enable: false });
                $("#selectPaymentSource").UifSelect('setSelected', 1); //informacion por defecto para ese modulo
                Payment.GetPaymentMovementTypesByPaymentSourceId();
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorGettingSourcePayment, 'autoclose': true });
            }
        });
    }

    static GetClaimExchangeRate(currencyId) {
        return new Promise(function (success) {
            var paymentRequestCurrency = $("#selectPaymentCurrency").UifSelect('getSelected');
            if (paymentRequestCurrency != currencyId) {
                var currencyExchange = paymentRequestCurrency > 0 ? paymentRequestCurrency : currencyId;
                ClaimsPaymentRequest.GetExchangeRate(currencyExchange).done(function (data) {
                    if (data.success) {
                        exchangeRate = data.result;
                    } else {
                        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ExchangeRateNotParametrizedUseIssueCurrecy, 'autoclose': true });
                        $('#selectPaymentCurrency').UifSelect('setSelected', paymentRequestCurrency);
                        $('#selectPaymentCurrency').trigger('change');
                    }
                    success();
                });
            } else if (currencyId == paymentRequestCurrency) {
                exchangeRate = {
                    SellAmount: 1
                };
                success();
            }
        });
    }

    static GetExchangeRate() {
        var currency = "";
        if ($("#_chkMultipleBilling").is(':checked')) {
            currency = $("#Currency").UifSelect("getSelected");
        } else {
            currency = $("#selectInvoiceCurrency").UifSelect("getSelected");
        }
        if (currency != "") {
            ClaimsPaymentRequest.GetExchangeRate(currency).done(function (response) {
                if (response.success) {

                    if (FormatDate(response.result.RateDate) != moment().format("DD/MM/YYYY")) {
                        $.UifNotify('show', { 'type': 'danger', 'message': AppResources.OutdatedExchangeRate + ' ' + FormatDate(response.result.RateDate), 'autoclose': true });
                    }

                    $('#inputExchangeRate').val(FormatMoney(response.result.SellAmount));
                    $('#ExchangeRate').val(FormatMoney(response.result.SellAmount));
                }
                else {
                    $('#inputExchangeRate').val("");
                    $('#ExchangeRate').val("");
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorGetRateChange, 'autoclose': true });

                    $("#selectInvoiceCurrency").UifSelect('setSelected', null);
                }
            });
        }
        else {
            $("#inputExchangeRate").val("");
            $("#ExchangeRate").val("");
        }
    }

    static GetAccountingConceptsByBranchIdMovementTypeIdPersonTypeIdIndividualId(callback) {
        var branchId = $("#selectPaymentBranch").UifSelect("getSelected");
        var movementTypeId = $("#selectMovementType").UifSelect("getSelected");
        var personTypeId = $('#selectPayTo').UifSelect("getSelected");
        var personId = IndividualId;

        if (branchId == "" || movementTypeId == "" || personTypeId == "" || IndividualId == 0) {
            return;
        }
        ClaimsPaymentRequest.GetAccountingConceptsByBranchIdMovementTypeIdPersonTypeIdIndividualId(branchId, movementTypeId, personTypeId, personId).done(function (response) {
            if (response.success) {
                if (typeof callback === "function")
                    return callback(response.result);

                $("#tblPaymentConcept").UifDataTable({ sourceData: response.result, selectionType: 'multiple' });
                $("#tblPaymentConcept").find('.selectall-button').hide();
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorPaymentConcept, 'autoclose': true });
            }
        });

        ClaimsPaymentRequest.GetEstimationsTypesIdByMovementTypeId($("#selectMovementType").UifSelect("getSelected")).done(function (response) {
            if (response.success) {
                if (response.result.length > 0) {
                    estimationsTypesId = response.result;
                }
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });

    }

    static GetPaymentMovementTypesByPaymentSourceId(callback) {
        var sourceId = $("#selectPaymentSource").UifSelect("getSelected");
        ClaimsPaymentRequest.GetPaymentMovementTypesByPaymentSourceId(sourceId).done(function (response) {
            if (response.success) {
                if (callback)
                    return callback(response.result);
                $('#selectMovementType').UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorGetMovementType, 'autoclose': true });
            }
        });
    }

    static GetPersonTypes(callback) {
        ClaimsPaymentRequest.GetPersonTypes().done(function (response) {
            if (response.success) {
                if (callback)
                    return callback(response.result);
                $('#selectPayTo').UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorGettingWhomPayment, 'autoclose': true });
            }
        });
    }

    static GetVoucherType(callback) {
        ClaimsPaymentRequest.GetVoucherType().done(function (response) {
            if (response.success) {
                if (callback)
                    return callback(response.result);
                $('#selectVoucherType').UifSelect({ sourceData: response.result });
                $('#VoucherType').UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorGettingInvoiceType, 'autoclose': true });
            }
        });
    }

    static GetPaymentMethod(callback) {
        ClaimsPaymentRequest.GetPaymentMethod().done(function (response) {
            if (response.success) {
                if (callback)
                    return callback(response.result);
                $('#selectPaymentMethod').UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorGetVoucherType, 'autoclose': true });
            }
        });
    }

    static GetClaimByPrefixIdBranchIdPolicyDocumentNumberClaimNumber(callback) {
        $("#formClaims").validate();
        if ($("#formClaims").valid()) {
            $("#tblBillingInformation").UifDataTable('clear');
            $("#tblSingleBillingInformation").UifDataTable('clear');
            rowsConcept = [];
            var prefixId = $("#selectPrefixesofClaims").UifSelect("getSelected");
            var branchId = $("#selectBranch").UifSelect("getSelected");
            var policyDocumentNumber = $("#inputPolicyDocumentNumber").val();
            var claimNumber = $("#inputClaimNumber").val();
            ClaimsPaymentRequest.GetClaimByPrefixIdBranchIdPolicyDocumentNumberClaimNumber(prefixId, branchId, policyDocumentNumber, claimNumber).done(function (data) {
                if (data.success) {
                    if (data.result.length > 0) {
                        if (typeof callback === "function")
                            return callback(data.result);

                        $("#selectPrefixesofClaims").UifSelect("setSelected", data.result[0].PrefixId);
                        $("#selectBranch").UifSelect("setSelected", data.result[0].BranchCode);

                        $('#tblClaims').UifDataTable({ sourceData: data.result });

                        $('#inputClaimBranch').text($("#selectPrefixesofClaims").UifSelect('getSelectedText').substring(0, 3) + '-' + policyDocumentNumber + '-' + claimNumber);
                        $('.hideSearchClaim').show();
                    } else {
                        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ClaimNotFound, 'autoclose': true });
                        $("#selectPrefixesofClaims").UifSelect("disabled", false);
                        $("#selectBranch").UifSelect("disabled", false);
                        $("#inputClaimNumber").prop("disabled", false);
                        $('#inputPolicyDocumentNumber').prop('disabled', false);
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ClaimFindDontExist, 'autoclose': true });
                }
            });
        }
    }
    static DoConsultPaymentRequest() {
        if (modelSearchCriteria.paymentRequestId == null)
            return;

        $("#btnSavePaymentRequest").hide();

        ClaimsPaymentRequest.GetPaymentRequestByPaymentRequestId(modelSearchCriteria.paymentRequestId).done(function (response) {
            if (response.success) {                
                
                $("#_chkMultipleBilling").prop("disabled", true);
                $("#btnPrintPaymentRequest").show();

                $("#btnPaymentConceptClose").hide();
                $("#btnPaymentConcept").hide();
                $("#btnSearchClaim").hide();

                $("#tabClaims").hide();
                $("#addClaims").hide();
                $('#summaryClaims').show();

                response.result.Claims.forEach(function (item, index) {
                    item.Total = (item.TotalConcept + item.TotalTax + item.TotalRetention)
                });

                $('#tblSummaryClaims').UifDataTable({ sourceData: response.result.Claims });

                Payment.GetVoucherType(function (voucherTypes) {
                    $('#selectVoucherType').UifSelect({ sourceData: voucherTypes, enable: false });
                    $('#selectVoucherType').UifSelect('setSelected', response.result.Claims[0].Vouchers[0].VoucherTypeId);
                });

                $('#inputVoucherNumber').prop('disabled', true);
                $('#inputVoucherNumber').val(response.result.Claims[0].Vouchers[0].Number);

                $('#inputVoucherDate').prop('disabled', true);
                $('#inputVoucherDate').val(FormatDate(response.result.Claims[0].Vouchers[0].Date));

                Payment.GetCurrency(function (currencies) {
                    $('#selectPaymentCurrency').UifSelect({ sourceData: currencies, enable: false });
                    $('#selectPaymentCurrency').UifSelect('setSelected', response.result.CurrencyId);
                    $('#selectPaymentCurrency').trigger('change');

                    $('#selectInvoiceCurrency').UifSelect({ sourceData: currencies, enable: false });
                    $('#selectInvoiceCurrency').UifSelect('setSelected', response.result.Claims[0].Vouchers[0].CurrencyId);
                    $("#inputExchangeRate").val(FormatMoney(response.result.Claims[0].Vouchers[0].ExchangeRate));                    
                });

                Payment.GetBranches(function (branches) {
                    $('#selectPaymentBranch').UifSelect({ sourceData: branches, enable: false });
                    $('#selectPaymentBranch').UifSelect('setSelected', response.result.BranchId);

                    $('#selectBranch').UifSelect({ sourceData: branches, enable: false });
                    $('#selectBranch').UifSelect('setSelected', response.result.ClaimBranchId);

                    Payment.GetPrefixes(function (prefixes) {
                        $('#selectPrefixesofClaims').UifSelect({ sourceData: prefixes, enable: false });
                        $('#selectPrefixesofClaims').UifSelect('setSelected', response.result.PrefixId);

                        $("#inputPolicyDocumentNumber").val(response.result.PolicyDocumentNumber);
                        $("#inputPolicyDocumentNumber").prop("disabled", true);

                        $("#inputClaimNumber").val(response.result.ClaimNumber);
                        $("#inputClaimNumber").prop("disabled", true);

                    });

                    Payment.GetPaymentSource(function (paymentSources) {
                        $('#selectPaymentSource').UifSelect({ sourceData: paymentSources, enable: false });
                        $('#selectPaymentSource').UifSelect('setSelected', response.result.PaymentSourceId);

                        Payment.GetPaymentMovementTypesByPaymentSourceId(function (movementTypes) {
                            $('#selectMovementType').UifSelect({ sourceData: movementTypes, enable: false });
                            $('#selectMovementType').UifSelect('setSelected', response.result.MovementTypeId);

                            Payment.GetPersonTypes(function (personTypes) {
                                $('#selectPayTo').UifSelect({ sourceData: personTypes, enable: false });
                                $('#selectPayTo').UifSelect('setSelected', response.result.PersonTypeId);

                                Payment.ShowDivPerson();
                                IndividualId = response.result.IndividualId;

                                switch ($('#selectPayTo').UifSelect('getSelectedText')) {
                                    case "PROVEEDOR":
                                        $('#divSupplier').show();
                                        $('#divInsured').hide();
                                        $('#divThirdparty').hide();
                                        $("#DocumentNumberSupplier").val(response.result.BeneficiaryDocumentNumber);
                                        $("#DocumentNumberSupplier").prop("disabled", true);
                                        $("#FullNameSupplier").val(response.result.BeneficiaryFullName);
                                        $("#FullNameSupplier").prop("disabled", true);
                                        break;
                                    case "ASEGURADO":
                                        $('#divSupplier').hide();
                                        $('#divInsured').show();
                                        $('#divThirdparty').hide();
                                        $("#DocumentNumberInsured").val(response.result.BeneficiaryDocumentNumber);
                                        $("#DocumentNumberInsured").prop("disabled", true);
                                        $("#FullNameInsured").val(response.result.BeneficiaryFullName);
                                        $("#FullNameInsured").prop("disabled", true);
                                        break;
                                    case 'TERCERO':
                                        $("#btnTaxStatus").prop('disabled', true);
                                        $('#divSupplier').hide();
                                        $('#divInsured').hide();
                                        $('#divThirdparty').show();
                                        $("#InputDocumentNumberThirdParty").val(response.result.BeneficiaryDocumentNumber);
                                        $("#InputDocumentNumberThirdParty").prop("disabled", true);
                                        $("#InputFullNameThirdParty").val(response.result.BeneficiaryFullName);
                                        $("#InputFullNameThirdParty").prop("disabled", true);
                                        break;
                                    default:
                                        $('#divSupplier').hide();
                                        $('#divInsured').hide();
                                        $('#divThirdparty').hide();

                                }
                            });
                        });
                    });
                });

                $('#inputEstimatedDate').prop('disabled', true);
                $('#inputEstimatedDate').val(FormatDate(response.result.EstimatedDate));

                $('#inpuntTotalAmount').prop('disabled', true);
                $('#inpuntTotalAmount').val(FormatMoney(response.result.TotalAmount));
                $('#inpuntTotalFinish').val(FormatMoney(response.result.TotalAmount));

                $('#inputDescription').prop('disabled', true);
                $('#inputDescription').val(response.result.Description);

                $('#inputRegistrationDate').prop('disabled', true);
                $('#inputRegistrationDate').val(FormatDate(response.result.RegistrationDate));

                Payment.GetPaymentMethod(function (paymentMethods) {
                    $('#selectPaymentMethod').UifSelect({ sourceData: paymentMethods, enable: false });
                    $('#selectPaymentMethod').UifSelect('setSelected', response.result.PaymentMethodId);
                });

            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorGetPaymentRequest, 'autoclose': true });
            }
        });
    }

    static ShowModal() {
        $('#modalClaimCheck').UifModal('showLocal', 'Reclama Cheque en Banco');
        $('#modalClaimCheck .modal-dialog.modal-lg').attr('style', 'width: 25%');
        $('#modalClaimCheck .modal-dialog.modal-lg .modal-content .modal-header .modal-title').attr('style', 'margin-top: 11px; margin-bottom: -20px; margin-left: 10px; font-weight: bold');
    }

    static CreateVoucher(isCreatePaymentRequestModel) {

        var voucher = {};

        if ($("#_chkMultipleBilling").is(':checked')) {
            voucher.Id = 0;
            voucher.VoucherTypeId = $("#VoucherType").UifSelect("getSelected");
            voucher.VoucherTypeDescription = $("#VoucherType").UifSelect("getSelectedText");
            voucher.CurrencyId = $("#Currency").UifSelect("getSelected");
            voucher.CurrencyDescription = $("#Currency").UifSelect("getSelectedText");
            voucher.Date = $("#Date").val();
            voucher.Number = $("#VoucherNumber").val();
            voucher.ExchangeRate = parseFloat(NotFormatMoney($("#ExchangeRate").val()));
            voucher.EstimationTypeId = currentClaim.EstimationTypeId;

            voucher.ConceptValue = 0;
            voucher.Concepts = [];
            voucher.TaxValue = 0;
            voucher.Retention = 0;

            if (typeof (isCreatePaymentRequestModel) == 'object') {
                if (Payment.ValidateVoucher(voucher)) {
                    rowsVoucher = [];
                    $('#tblBillingInformation').UifDataTable('editRow', voucher, currentEditVoucherIndex);
                    rowsVoucher = $("#tblBillingInformation").UifDataTable('getData');
                } else {
                    rowsVoucher = [];
                    $("#tblBillingInformation").UifDataTable('addRow', voucher);
                    rowsVoucher = $("#tblBillingInformation").UifDataTable('getData');
                }
            }

            $('#editActionMulti').UifInline('hide');
        }
        else {
            rowsVoucher = [];
            voucher.Id = 0;
            voucher.VoucherTypeId = $("#selectVoucherType").UifSelect("getSelected");
            voucher.CurrencyId = $("#selectInvoiceCurrency").UifSelect("getSelected");
            voucher.Number = $("#inputVoucherNumber").val();
            voucher.Date = $("#inputVoucherDate").val();
            voucher.ExchangeRate = NotFormatMoney($("#inputExchangeRate").val());
            voucher.ClaimNumber = $("#inputClaimNumber").val();
            voucher.Concepts = rowsConcept;
            voucher.EstimationTypeId = currentClaim.EstimationTypeId;
            rowsVoucher.push(voucher);
        }
    }

    static ShowModalPaymentConcept(event, data, position) {
        $('#btnSavePaymentRequest').prop('disabled', false);
        var payto = $("#selectPayTo").UifSelect("getSelectedText");

        if (payto === ("ASEGURADO")) {
            if (data.IsInsured) {
                Payment.SetInsuredAutomatically(data.IndividualId);
            }
            else {
                Payment.SetInsuredAutomaticallyByRiskId(data.RiskId);
            }
        }

        Payment.GetClaimExchangeRate(data.CurrencyId);

        if (modelSearchCriteria.paymentRequestId == null) {
            Payment.GetCurrency(function (currencies) {
                ClaimsPaymentRequest.GetDefaultPaymentCurrency().done(function (response) {
                    if (response.success) {
                        defaultPaymentCurrency = response.result;
                        var filterCurrencies = currencies.filter(x => { return x.Id == data.CurrencyId || x.Id == defaultPaymentCurrency });

                        var paymentCurrencySelected = $('#selectPaymentCurrency').UifSelect("getSelected");

                        if (!filterCurrencies.find(x => x.Id == paymentCurrencySelected)) {
                            $('#selectPaymentCurrency').UifSelect({ sourceData: filterCurrencies });
                            $('#selectPaymentCurrency').UifSelect('setSelected', defaultPaymentCurrency);
                            $('#selectPaymentCurrency').trigger('change');
                        }

                        var invoiceCurrencySelected = $('#selectPaymentCurrency').UifSelect("getSelected");

                        if (!filterCurrencies.find(x => x.Id == invoiceCurrencySelected)) {
                            $('#selectInvoiceCurrency').UifSelect({ sourceData: filterCurrencies, enable: false });
                            $('#selectInvoiceCurrency').UifSelect('setSelected', defaultPaymentCurrency);
                            $('#selectInvoiceCurrency').trigger('change');
                        }

                        var multipleinvoiceCurrencySelected = $('#Currency').UifSelect("getSelected");

                        if (!filterCurrencies.find(x => x.Id == multipleinvoiceCurrencySelected)) {
                            $('#Currency').UifSelect({ sourceData: filterCurrencies, enable: false });
                            $('#Currency').UifSelect('setSelected', defaultPaymentCurrency);
                            $('#Currency').trigger('change');
                        }
                    }
                });
            });
        }

        $("#formPaymentRequest").validate();
        if (!$("#formPaymentRequest").valid()) {
            $("#tblClaims").UifDataTable('unselect');
            ScrollTop();
            return;
        }

        $("#formRteICA").validate();
        if (!$("#formRteICA").valid()) {
            $("#tblClaims").UifDataTable('unselect');
            ScrollTop();
            return;
        }

        if (data.EstimationTypeEstatus == EstimationTypeStatuses.Closed) {
            //Cerrado
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.CannotPayAConceptClosed, 'autoclose': true });
            $("#tblClaims").UifDataTable('unselect');
            return;
        }

        if (data.EstimationTypeId == EstimateType.Salvages || data.EstimationTypeId == EstimateType.Recoveries) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.CannotPayAConceptSalvageOrRecovery, 'autoclose': true });
            $("#tblClaims").UifDataTable('unselect');
            return;
        }        

        if (!estimationsTypesId.some(x => x == data.EstimationTypeId)) {
            $.UifNotify('show', { 'type': 'danger', 'message': String.format(Resources.Language.CannotPayAConceptType, $("#selectMovementType").UifSelect("getSelectedText"), data.EstimationType), 'autoclose': true });
            $("#tblClaims").UifDataTable('unselect');
            return;
        }                        

        currentClaim.ClaimId = data.ClaimId;
        currentClaim.RiskDescription = data.RiskDescription;
        currentClaim.SubClaimId = data.SubClaim;
        currentClaim.EstimationType = data.EstimationType;
        currentClaim.EstimationTypeId = data.EstimationTypeId;
        currentClaim.ClaimNumber = data.Number;
        currentClaim.LineBusinessId = data.LineBusinessId;
        currentClaim.ClaimCreationDate = moment(data.CreationDate).format("DD/MM/YYYY HH:mm:ss");
        currentClaim.CoverageId = data.CoverageId;
        currentClaim.CoverageDescription = data.CoverageDescription;
        currentClaim.EstimateAmount = data.EstimateAmount;
        currentClaim.BranchCode = data.BranchCode;
        currentClaim.PaymentValue = data.PaymentValue;
        currentClaim.CurrencyId = data.CurrencyId;
        currentClaim.PolicyDocumentNumber = data.DocumentNumber;
        currentClaim.EstimationTypeStatusReasonId = data.EstimationTypeEstatusReasonCode;
        currentClaim.EstimationDate = data.EstimationDate;
        currentClaim.EstimationTypeStatusId = data.EstimationTypeEstatus;
        currentClaim.RiskId = data.RiskId;
        endorsementId = data.EndorsementId;
        policyId = data.policyId;
        IsTotalParticipation = data.IsTotalParticipation;
        currentCoverageId = data.CoverageId;
        currentPositionClaim = position;
        currentClaim.CoverageId = data.CoverageId;
        currentClaim.OccurrenceDate = moment(data.OccurrenceDate).format("DD/MM/YYYY HH:mm:ss");
        currentClaim.JudicialDecisionDate = moment(data.JudicialDecisionDate).format("DD/MM/YYYY HH:mm:ss");
        currentClaim.BusinessTypeId = data.BusinessTypeId;
        $("#tblSingleBillingInformation").UifDataTable('clear');
        rowsConcept = [];
        if ($("#_chkMultipleBilling").is(':checked')) {
            selectedClaim = $("#tblClaims").UifDataTable('getSelected')[0];
            $('#modalMultipleBillingInformationDialog').UifModal('showLocal', Resources.Language.Invoices);
        } else {
            $("#formVoucher").validate();
            if ($("#formVoucher").valid()) {
                $('#modalSingleBillingInformationDialog').UifModal('showLocal', Resources.Language.LabelPaymentConcepts);
                if (data.Vouchers != null) {
                    $.each(data.Vouchers[0].Concepts, function (index, value) {
                        $('#tblSingleBillingInformation').UifDataTable('addRow', value);
                        rowsConcept.push(value);
                    });
                }
                $('#tblPaymentConcept').UifDataTable('unselect');
                $('#editAction').UifInline('hide');
            }
            else {
                ScrollTop();
                $("#tblClaims").UifDataTable('unselect');
            }
        }
        
    }

    static SetInsuredAutomatically(individualId) {
        ClaimsPaymentRequest.GetInsuredsByIndividualId(individualId).done(function (response) {
            if (response.length > 0) {

                $("#DocumentNumberInsured").UifAutoComplete("setValue", response[0].DocumentNumber);
                $("#FullNameInsured").UifAutoComplete("setValue", response[0].FullName);

                $("#DocumentNumberInsured").blur();
                $("#FullNameInsured").blur();

                $("#DocumentNumberInsured").UifAutoComplete('disabled', true);
                $("#FullNameInsured").UifAutoComplete('disabled', true);
                IndividualId = response[0].IndividualId;
                IndividualDocumentTypeId = response[0].DocumentTypeId;
                Payment.GetAccountingConceptsByBranchIdMovementTypeIdPersonTypeIdIndividualId();
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.InsuredNotPerson, 'autoclose': true });
            }
        });
    }

    static SetInsuredAutomaticallyByRiskId(riskId) {
        ClaimsPaymentRequest.GetInsuredByRiskId(riskId).done(function (response) {
            if (response.result != null) {

                $("#DocumentNumberInsured").UifAutoComplete("setValue", response.result.DocumentNumber);
                $("#FullNameInsured").UifAutoComplete("setValue", response.result.FullName);

                $("#DocumentNumberInsured").blur();
                $("#FullNameInsured").blur();

                $("#DocumentNumberInsured").UifAutoComplete('disabled', true);
                $("#FullNameInsured").UifAutoComplete('disabled', true);
                IndividualId = response.result.IndividualId;
                IndividualDocumentTypeId = response.result.DocumentTypeId;
                Payment.GetAccountingConceptsByBranchIdMovementTypeIdPersonTypeIdIndividualId();
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.InsuredNotPerson, 'autoclose': true });
            }
        });
    }

    static ShowModalPaymentConceptVoucher(event, data, position) {
        $('#modalSingleBillingInformationDialog').UifModal('showLocal', Resources.Language.LabelPaymentConcepts);
        $("#tblPaymentConcept").UifDataTable('unselect');
        $("#tblSingleBillingInformation").UifDataTable('clear');
        rowsConcept = data.Concepts;
        if (modelSearchCriteria.paymentRequestId != null) {
            $("#tblSingleBillingInformation").UifDataTable({ sourceData: data.Concepts, add: false, delete: false, edit: false });
        }

        currentPositionVoucher = position;
    }

    static ShowDivPerson() {
        var personType = $("#selectPayTo").UifSelect("getSelectedText");

        $("#DocumentNumberSupplier").removeAttr('required');
        $("#FullNameSupplier").removeAttr('required');
        $("#DocumentNumberInsured").removeAttr('required');
        $("#FullNameInsured").removeAttr('required');

        switch (personType) {
            case 'PROVEEDOR':
                $("#btnTaxStatus").prop('disabled', false);
                $('#divInsured').hide();
                $('#divThirdparty').hide();
                $('#divSupplier').show();
                $('#DocumentNumberSupplier').attr("required", "required");
                $('#FullNameSupplier').attr("required", "required");
                break;
            case 'ASEGURADO':
                $("#btnTaxStatus").prop('disabled', true);
                $('#divSupplier').hide();
                $('#divThirdparty').hide();
                $('#divInsured').show();
                $('#DocumentNumberInsured').attr("required", "required");
                $('#FullNameInsured').attr("required", "required");
                $("#DocumentNumberInsured").UifAutoComplete("clean");
                $("#FullNameInsured").UifAutoComplete("clean");
                $("#DocumentNumberInsured").UifAutoComplete('disabled', false);
                $("#FullNameInsured").UifAutoComplete('disabled', false);

                Payment.ValidateSubClaims();
                break;
            case 'TERCERO':
                $("#btnTaxStatus").prop('disabled', true);
                $('#divSupplier').hide();
                $('#divInsured').hide();
                $('#divThirdparty').show();
                $('#InputDocumentNumberThirdParty').attr("required", "required");
                $('#InputFullNameThirdParty').attr("required", "required");
                break;
            default:
                $('#divInsured').hide();
                $('#divSupplier').hide();
                $('#divThirdparty').hide();
        }

        IndividualId = 0;

    }

    static ValidateSubClaims() {
        if ($("#tblClaims").UifDataTable('getData').length > 0) {
            var subClaims = $("#tblClaims").UifDataTable('getData');
            if (subClaims.some(x => x.IsInsured)) {
                Payment.SetInsuredAutomatically(subClaims.filter(x => x.IsInsured)[0].IndividualId);
            } else {
                Payment.SetInsuredAutomaticallyByRiskId(subClaims[0].RiskId)
            }
        }
    }

    static SetInsuredInformation(event, insured) {
        $('#DocumentNumberInsured').UifAutoComplete('setValue', insured.DocumentNumber);
        $('#FullNameInsured').UifAutoComplete('setValue', insured.FullName);
        IndividualId = insured.IndividualId;
        IndividualDocumentTypeId = insured.DocumentTypeId;
        Payment.GetAccountingConceptsByBranchIdMovementTypeIdPersonTypeIdIndividualId();
    }

    static SetSupplierInformation(event, supplier) {
        $('#DocumentNumberSupplier').UifAutoComplete('setValue', supplier.DocumentNumber);
        $('#FullNameSupplier').UifAutoComplete('setValue', supplier.Name);
        IndividualId = supplier.IndividualId;
        IndividualDocumentTypeId = supplier.DocumentTypeId;
        Payment.GetIndividualTaxesByIndividualIdRoleId(supplier.IndividualId, $("#selectPayTo").UifSelect('getSelected'));
        Payment.GetAccountingConceptsByBranchIdMovementTypeIdPersonTypeIdIndividualId();
    }

    static MovementDate() {
        ClaimsPaymentRequest.GetModuleDateByModuleTypeMovementDate().done(function (response) {
            if (response.success) {
                $('#inputAccountingDate').val(FormatDate(response.result));
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorGettingAccountingDate, 'autoclose': true });
            }
        });
    }

    static GetIndividualTaxesByIndividualIdRoleId(IndividualId, roleId) {
        ClaimsPaymentRequest.GetIndividualTaxesByIndividualIdRoleId(IndividualId, roleId).done(function (response) {
            if (response.success) {

                IndividualTaxCondition = response.result.map(function (data) {
                    data.HasNationalRate = true;
                    data.RateDescription = data.RateTypeDescription;
                    data.StateDescription = (data.StateId == 0) ? Resources.Language.National : Resources.Language.Local;

                    return data;
                });

                Payment.ValidationRetetionICA();
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorGettingTaxes, 'autoclose': true });
            }
        });
    }

    static ValidationRetetionICA() {
        if (IndividualTaxCondition.some(x => x.TaxId == RTEICACode)) {
            $("#divRteICA").show();
            Payment.GetCountries();
        }
    }

    static OpenTaxCondition() {
        let taxConditionTitle = '';

        switch ($('#selectPayTo').UifSelect('getSelectedText')) {
            case "ASEGURADO":
                taxConditionTitle = Resources.Language.TaxCondition + ':  ' + ' ' + $('#FullNameInsured').UifAutoComplete('getValue');
                break;
            case "PROVEEDOR":
                taxConditionTitle = Resources.Language.TaxCondition + ':  ' + ' ' + $('#FullNameSupplier').UifAutoComplete('getValue');
                break;
        }

        $("#tblTaxCondition").UifDataTable({ sourceData: IndividualTaxCondition });
        $('#modalTaxConditionDialog').UifModal('showLocal', taxConditionTitle);
    }

    static GetCountries() {
        ClaimsPaymentRequest.GetCountries().done(function (response) {
            if (response.success) {
                $('#CountryLiquidationICA').UifSelect({ sourceData: response.result });

                ClaimsPaymentRequest.GetDefaultCountry().done(function (data) {
                    if (data.success) {
                        $("#CountryLiquidationICA").UifSelect("setSelected", data.result);
                        $('#CountryLiquidationICA').trigger("change");
                    }
                    else {
                        $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                    }
                });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static GetStatesByCountryId(countryId) {
        if (countryId !== "" && countryId !== undefined && countryId !== null) {
            ClaimsPaymentRequest.GetStatesByCountryId(countryId).done(function (response) {
                if (response.success) {
                    $('#StateLiquidationICA').UifSelect({ sourceData: response.result });
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
                }
            });
        }
        else {
            $("#StateLiquidationICA").UifSelect('setSelected', null);
            $('#CityLiquidationICA').UifSelect('setSelected', null);
        }
    }

    static GetCitiesByCountryIdStateId(countryId, stateId) {
        ClaimsPaymentRequest.GetCitiesByCountryIdStateId(countryId, stateId).done(function (response) {
            if (response.success) {
                $('#CityLiquidationICA').UifSelect({ sourceData: response.result });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static GetStatesByCountry(event, selectedItem) {
        Payment.GetStatesByCountryId(selectedItem.Id);
    }

    static GetCititesByCountryState(event, selectedItem) {
        var countryId = $('#CountryLiquidationICA').UifSelect('getSelected');
        var stateId = selectedItem.Id;
        Payment.GetCitiesByCountryIdStateId(countryId, stateId);
    }

    static CreatePaymentRequest() {
        $('#btnSavePaymentRequest').prop('disabled', true);
        $("#formPaymentRequest").validate();
        $("#formVoucher").validate();
        $("#formClaims").validate();
        if ($("#formPaymentRequest").valid() && $("#formVoucher").valid() && $("#formClaims").valid()) {
            if (IndividualId != 0) {
                lockScreen();
                if ($("#tblSummaryClaims").UifDataTable('getData').length > 0 || rowsVoucher.length > 0) {
                    ClaimsPaymentRequest.GetPolicyByEndorsementIdModuleType(endorsementId).done(function (data) {
                        if (data.success) {
                            lockScreen();
                            policySalePointId = data.result.SalePointId;
                            policyProductId = data.result.ProductId;
                            var paymentRequestModel = null;

                            if (parseFloat(NotFormatMoney($("#inpuntTotalFinish").val()).replace(',', '.')) === parseFloat(NotFormatMoney($("#inpuntTotalAmount").val()).replace(',', '.'))) {

                                var counttblClaims = $("#tblClaims").UifDataTable('getData');
                                if (counttblClaims.length == 0) {
                                    unlockScreen();
                                    $('#btnSavePaymentRequest').prop('disabled', false);
                                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.AtLeastOneClaim, 'autoclose': true });
                                    return;
                                }

                                paymentRequestModel = Payment.CreatePaymentRequestModel();
                                ClaimsPaymentRequest.SavePaymentRequest(paymentRequestModel).done(function (response) {
                                    if (response.success) {
                                        LaunchPolicies.ValidateInfringementPolicies(response.result.AuthorizationPolicies, true);
                                        var countAutorizationPolicies = response.result.AuthorizationPolicies.filter(x => x.Type == TypeAuthorizationPolicies.Authorization || x.Type == TypeAuthorizationPolicies.Restrictive).length;
                                        if (countAutorizationPolicies > 0) {
                                            LaunchPolicies.RenderViewAuthorizationPolicies(response.result.AuthorizationPolicies, response.result.TemporalId, FunctionType.PaymentRequest);
                                        } else {
                                            $.UifDialog('confirm', {
                                                message: String.format(Resources.Language.NumberPaymentRequest, response.result.Number, response.result.SaveDailyEntryMessage == null ? "" : " " + response.result.SaveDailyEntryMessage)
                                            }, function (result) {
                                                if (result) {
                                                    Payment.CreatePrintPaymentRequestById(response.result.Id);
                                                }
                                                Payment.ClearForm();
                                            });
                                        }
                                    }
                                    else {
                                        $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
                                    }
                                    $('#btnSavePaymentRequest').prop('disabled', false);
                                }).always(function () {
                                    unlockScreen();
                                });

                            } else {
                                unlockScreen();
                                $('#btnSavePaymentRequest').prop('disabled', false);
                                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.TotalValueClaimEqualTotalFinal, 'autoclose': true });
                            }
                        }
                    }).always(function () {
                        unlockScreen();
                    });
                } else {
                    $('#btnSavePaymentRequest').prop('disabled', false);
                    unlockScreen();
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.FormConceptsRequired, 'autoclose': true });
                }

            }
            else {
                ScrollTop();
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSelectedCompanyPersonBasic, 'autoclose': true });
            }

        } else {
            ScrollTop();
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.RequiredAllFields, 'autoclose': true });
        }
    }

    static CreatePaymentRequestModel() {
        paymentRequest.Id = 0;
        paymentRequest.Number = 0;
        paymentRequest.RegistrationDate = $("#inputRegistrationDate").val();
        paymentRequest.LineBusinessId = currentClaim.LineBusinessId;
        paymentRequest.PrefixId = $("#selectPrefixesofClaims").UifSelect("getSelected");
        paymentRequest.PaymentDate = null; //cuando se paga la solicitud
        paymentRequest.MovementTypeId = $("#selectMovementType").UifSelect("getSelected");
        paymentRequest.IsPrinted = false;
        paymentRequest.IndividualId = IndividualId;
        paymentRequest.BeneficiaryDocumentType = IndividualDocumentTypeId;

        switch ($("#selectPayTo").UifSelect("getSelectedText")) {
            case 'PROVEEDOR':
                paymentRequest.BeneficiaryDocumentNumber = $("#DocumentNumberSupplier").UifAutoComplete('getValue');
                paymentRequest.BeneficiaryFullName = $("#FullNameSupplier").UifAutoComplete('getValue');
                break;
            case 'ASEGURADO':
                paymentRequest.BeneficiaryDocumentNumber = $("#DocumentNumberInsured").UifAutoComplete('getValue');
                paymentRequest.BeneficiaryFullName = $("#FullNameInsured").UifAutoComplete('getValue');
                break;
            case 'TERCERO':
                paymentRequest.BeneficiaryDocumentNumber = $("#InputDocumentNumberThirdParty").UifAutoComplete('getValue');
                paymentRequest.BeneficiaryFullName = $("#InputFullNameThirdParty").UifAutoComplete('getValue');
                break;
        }

        paymentRequest.PersonTypeId = $("#selectPayTo").UifSelect("getSelected");
        paymentRequest.EstimatedDate = $("#inputEstimatedDate").val();
        paymentRequest.CurrencyId = $("#selectPaymentCurrency").UifSelect("getSelected");
        paymentRequest.ExchangeRate = paymentCurrencyExchangeRate;
        paymentRequest.BranchId = $("#selectPaymentBranch").UifSelect("getSelected");
        paymentRequest.AccountBankId = 2; //no se viene de un metodo consultado por indivualId
        paymentRequest.Description = $("#inputDescription").val();
        paymentRequest.PaymentRequestTypeId = 0;
        paymentRequest.PaymentSourceId = $("#selectPaymentSource").UifSelect("getSelected");
        paymentRequest.PaymentMethodId = $("#selectPaymentMethod").UifSelect("getSelected");
        paymentRequest.ClaimId = currentClaim.ClaimId;
        paymentRequest.ClaimOccurrenceDate = currentClaim.OccurrenceDate;
        paymentRequest.JudicialDecisionDate = currentClaim.JudicialDecisionDate;
        paymentRequest.SubClaim = currentClaim.SubClaimId;
        paymentRequest.ClaimBranchId = currentClaim.BranchCode;
        paymentRequest.ClaimNumber = currentClaim.ClaimNumber;
        paymentRequest.AccountingDate = $('#inputAccountingDate').val();
        paymentRequest.TotalAmount = NotFormatMoney($('#inpuntTotalAmount').val());
        paymentRequest.IsTotal = $("#radioPaymentTotal").is(':checked');
        paymentRequest.EstimationCurrencyId = currentClaim.CurrencyId;
        paymentRequest.EstimationTypeStatusReasonId = currentClaim.EstimationTypeStatusReasonId;
        paymentRequest.EstimationDate = FormatDate(currentClaim.EstimationDate);
        paymentRequest.EstimationTypeStatusId = currentClaim.EstimationTypeStatusId;
        paymentRequest.EstimationTypeId = currentClaim.EstimationTypeId;
        paymentRequest.EstimationAmount = currentClaim.EstimateAmount;
        paymentRequest.PolicyDocumentNumber = currentClaim.PolicyDocumentNumber;
        paymentRequest.PolicyProductId = policyProductId;
        paymentRequest.PolicySalePointId = policySalePointId;
        paymentRequest.ClaimCreationDate = currentClaim.ClaimCreationDate;
        let summaryClaims = $("#tblSummaryClaims").UifDataTable('getData');
        paymentRequest.Claims = summaryClaims;
        
        paymentRequest.Claims.forEach(function (item, index) {
            item.CreationDate = FormatDate(item.CreationDate);
            item.EstimationDate = FormatDate(item.EstimationDate);
            item.JudicialDecisionDate = FormatDate(item.JudicialDecisionDate);
            item.OccurrenceDate = FormatDate(item.OccurrenceDate);
            item.PolicyProductId = policyProductId;
        });

        return paymentRequest;
    }

    static CreateConcepts(event, data) {
        var rowConcept = {};
        rowConcept.PaymentConceptId = data.Id;
        rowConcept.PaymentConcept = data.Description;
        rowConcept.Value = 0;
        rowConcept.TaxValue = 0;
        rowConcept.Retention = 0;
        Payment.ValidateConcept(rowConcept);
    }

    static DeselectConcept(event, data) {
        if ($("#tblSingleBillingInformation").UifDataTable('getData').some(x => x.PaymentConceptId == data.Id)) {
            var value = {
                label: 'Id',
                values: [data.Id]
            };
            $("#tblPaymentConcept").UifDataTable('setSelect', value);
        }
    }

    static ValidateConcept(rowConcept) {
        if ($("#tblSingleBillingInformation").UifDataTable('getData').length == 1) {

            var value = {
                label: 'Id',
                values: [rowConcept.PaymentConceptId]
            };

            $("#tblPaymentConcept").UifDataTable('setUnselect', value);
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.CannotIncludeMoreThanOneConcepts, 'autoclose': true });
        }
        else {
            if (rowsConcept.length > 0) {
                if (rowsConcept.some(x => x.PaymentConceptId == rowConcept.PaymentConceptId)) {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.PaymentConceptSelected, 'autoclose': true });
                }
                else {
                    $('#tblSingleBillingInformation').UifDataTable('addRow', rowConcept);
                    rowsConcept.push(rowConcept);
                }
            }
            else {
                $('#tblSingleBillingInformation').UifDataTable('addRow', rowConcept);
                rowsConcept.push(rowConcept);
            }
        }
    }

    static ValidateVoucher(rowVoucher) {
        if (rowsVoucher.length > 0) {
            var exist = 0;
            for (var i = 0; i < rowsVoucher.length; i++) {
                if (rowsVoucher[i].Number === rowVoucher.Number) {
                    exist = exist + 1;
                }
            }
            return (exist > 0);
        }
    }

    static DeleteConcept(event, data, position) {
        var index = rowsConcept.findIndex(x => x.PaymentConceptId === data.PaymentConceptId);
        rowsConcept.splice(index, 1);

        var value = {
            label: 'Id',
            values: [data.PaymentConceptId]
        };

        $("#tblPaymentConcept").UifDataTable('setUnselect', value);

        $('#tblSingleBillingInformation').UifDataTable('deleteRow', position);
    }

    static EditConcept(event, data, position) {
        currentEditConceptIndex = position;
        currentPaymentConceptId = data.PaymentConceptId;

        $('#editAction').UifInline('show');
        Payment.bindConcept(data);
    }

    static ViewConceptTaxDetails(event, data, position) {
        if (data.ConceptTaxes != null) {
            $.each(data.ConceptTaxes, function (index, value) {
                if (!this.TaxRate.toString().includes("%")) {
                    this.TaxRate = this.TaxRate + "%";
                }
            });
            $("#tblConceptTaxDetail").UifDataTable({ sourceData: data.ConceptTaxes });
            ViewListConceptTaxes.show();
        } else {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ConceptWithoutSettledTaxes, 'autoclose': true });
        }
    }

    static HideConceptTaxDetail() {
        ViewListConceptTaxes.hide();
        $("#tblSingleBillingInformation").UifDataTable('unselect');
    }

    static DeleteVoucher(event, data, position) {
        var index = rowsVoucher.findIndex(x => x.Number === data.Number);
        rowsVoucher.splice(index, 1);
        $("#tblBillingInformation").UifDataTable('deleteRow', position);
    }

    static EditVouchers(event, data, position) {
        currentEditVoucherIndex = position;
        currentPaymentVoucherNumber = data.Number;
        $('#editActionMulti').UifInline('show');
        Payment.binVoucher(data);
    }
    static EditSubClaimSummaryVouchers(event, data, position) {
        currentEditSubClaimSummaryIndex = position;
        currentPaymentVoucherNumber = data.Number;
        if (data.Vouchers.length > 1) {
            $("#_chkMultipleBilling").prop("checked", true);
            rowsVoucher = response.result.Vouchers;
            Payment.ShowFormBilling();
        }
        else {
            var voucher = response.result.Vouchers[0];

            Payment.GetVoucherType(function (voucherTypes) {
                $('#selectVoucherType').UifSelect({ sourceData: voucherTypes, enable: false });
                $('#selectVoucherType').UifSelect('setSelected', voucher.VoucherTypeId);
            });
            Payment.binVoucher(data);
        }


    }

    static EditClaimSummaryVouchers(event, data, position) {
        var voucher = data.Vouchers[0];
        if (voucher != null) {
                                    
            Payment.GetAccountingConceptsByBranchIdMovementTypeIdPersonTypeIdIndividualId(function (response) {
                $("#tblPaymentConcept").UifDataTable('unselect');
                $('#tblPaymentConcept').prop('disabled', true);

                if (voucher.Concepts != null) {
                    $.each(voucher.Concepts, function (i, value) {
                        this.PaymentConcept = response.find(x => x.Id == this.PaymentConceptId).Description
                    });

                    Payment.GetAccountingConceptsByBranchIdMovementTypeIdPersonTypeIdIndividualId(function (paymentConcepts) {
                        $("#tblPaymentConcept").UifDataTable({ sourceData: paymentConcepts.filter(x => { return voucher.Concepts.some(y => y.PaymentConceptId == x.Id) }), selectionType: 'none' });
                    });

                    $("#tblSingleBillingInformation").UifDataTable({ sourceData: voucher.Concepts, add: false, delete: false, edit: false });
                    
                }

                $('#modalSingleBillingInformationDialog').UifModal('showLocal', Resources.Language.LabelPaymentConcepts);

            });
        }
    }

    static AddConcepts() {
        var index = rowsConcept.findIndex(x => x.PaymentConceptId == currentPaymentConceptId);

        rowsConcept[index].PaymentConceptId = $("#editForm").find("#inputPaymentConceptId").val();
        rowsConcept[index].PaymentConcept = $("#editForm").find("#inputPaymentConcept").val();
        rowsConcept[index].Value = parseFloat(NotFormatMoney($("#editForm").find("#inputConceptValue").val()).replace(',', '.'));

        var rowModel = {};
        rowModel.PaymentConceptId = rowsConcept[index].PaymentConceptId;
        rowModel.PaymentConcept = rowsConcept[index].PaymentConcept;
        rowModel.Value = rowsConcept[index].Value;

        if (!rowModel.hasOwnProperty('ConceptTaxes')) {
            rowModel.ConceptTaxes = [];
            rowsConcept[index].ConceptTaxes = [];
        }
        var conceptValue = rowsConcept[index].Value;

        if ($('#selectPayTo').UifSelect('getSelectedText') == 'PROVEEDOR') {
            Payment.FillTaxAttributes();

            ClaimsPaymentRequest.CalculatePaymentTaxesByIndividualIdAccountingConceptIdTaxAttributesAmount(IndividualId, rowsConcept[index].PaymentConceptId, TaxAttributes, conceptValue).done(function (response) {
                if (response.success) {

                    if (response.result.filter(x => { return response.result.filter(y => y.TaxId == x.TaxId).length > 1 }).length > 0) {
                        Payment.SelectDuplicateTaxes(rowModel, response.result);
                    }
                    else {
                        Payment.SetTaxesToConcept(rowModel, response.result);
                    }
                }
                else {
                    rowModel.ConceptTaxes = null;
                    $('#tblSingleBillingInformation').UifDataTable('editRow', rowModel, currentEditConceptIndex);
                    $("#editForm").formReset();
                    $('#editAction').UifInline('hide');

                    $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
                }
            });
        }
        else {
            rowModel.ConceptTaxes = null;
            $('#tblSingleBillingInformation').UifDataTable('editRow', rowModel, currentEditConceptIndex);
            $("#editForm").formReset();
            $('#editAction').UifInline('hide');
        }
    }

    static ValidateSelectedTaxes() {
        var allTaxesSelected = true;
        var taxesSelecteds = $("#tblTaxDuplicateDetail").UifDataTable('getSelected');
        var unambiguousTaxes = [];

        taxesSelecteds = taxesDuplicates.filter(x => { return !taxesSelecteds.some(y => y.TaxId == x.TaxId && y.TaxCategoryId == x.TaxCategoryId && y.TaxConditionId == x.TaxConditionId) });

        if (taxesSelecteds != null && taxesSelecteds.length > 0) {
            if (taxesSelecteds.filter(x => { return taxesSelecteds.filter(y => y.TaxId == x.TaxId).length > 1 }).length > 0) {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.SelectDuplicatesTaxesByConcepts, 'autoclose': true });
                return;
            }

            $.each(taxesDuplicates, function (index, value) {
                if (!taxesSelecteds.some(x => x.TaxId == this.TaxId)) {
                    allTaxesSelected = false;
                }
            });

            if (!allTaxesSelected) {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.SelectDuplicatesTaxesByConcepts, 'autoclose': true });
                return;
            }
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.SelectDuplicatesTaxesByConcepts, 'autoclose': true });
            return;
        }

        unambiguousTaxes = allTaxes.filter(x => { return !taxesSelecteds.some(y => y.TaxId == x.TaxId && y.TaxCategoryId == x.TaxCategoryId && y.TaxConditionId == x.TaxConditionId) });
        Payment.SetTaxesToConcept(conceptWithTaxesDuplicates, unambiguousTaxes);
        ViewListConceptTaxesDuplicates.hide();
    }

    static SelectDuplicateTaxes(concept, taxes) {
        allTaxes = taxes;
        taxesDuplicates = taxes.filter(x => { return taxes.filter(y => y.TaxId == x.TaxId).length > 1 });
        conceptWithTaxesDuplicates = concept;

        $("#tblTaxDuplicateDetail").UifDataTable({ sourceData: taxesDuplicates });
        ViewListConceptTaxesDuplicates.show();
    }

    static SetTaxesToConcept(concept, taxes) {
        var index = rowsConcept.findIndex(x => x.PaymentConceptId == currentPaymentConceptId);
        concept.ConceptTaxes = taxes;

        concept.ConceptTaxes.map(function (data) {
            data.ConditionId = data.TaxConditionId;
            data.CategoryId = data.TaxCategoryId;
            data.Retention = data.RetentionValue;
            data.TaxBaseAmount = data.BaseAmount;
        });

        rowsConcept[index].TaxValue = concept.ConceptTaxes.reduce((acc, x) => acc + x.TaxValue, 0);
        rowsConcept[index].Retention = concept.ConceptTaxes.reduce((acc, x) => acc + x.RetentionValue, 0);
        rowsConcept[index].ConceptTaxes = concept.ConceptTaxes;
        concept.TaxValue = concept.ConceptTaxes.reduce((acc, x) => acc + x.TaxValue, 0);
        concept.Retention = concept.ConceptTaxes.reduce((acc, x) => acc + x.RetentionValue, 0);

        $('#tblSingleBillingInformation').UifDataTable('editRow', concept, currentEditConceptIndex);
        $("#editForm").formReset();
        $('#editAction').UifInline('hide');
    }

    static ChangePaymentCurrency() {
        if (rowsConcept.length > 0)
            Payment.ClearConcepts();

        var currency = $('#selectPaymentCurrency').UifSelect('getSelected');

        if (!$("#_chkMultipleBilling").is(':checked')) {
            $('#selectInvoiceCurrency').UifSelect('setSelected', currency);
            $('#selectInvoiceCurrency').trigger('change');
        }
        else {
            $('#Currency').UifSelect('setSelected', currency);
            $('#Currency').trigger('change');
        }

        $("#lblTotalFinish").text(Resources.Language.TotalFinish + " (" + $("#selectPaymentCurrency").UifSelect("getSelectedText") + ")");
    }

    static AddVouchers(event, data) {
        $('#editActionMulti').UifInline('show');
        Payment.GetVoucherType();

        $("#VoucherType").val("");
        $('#Currency').UifSelect('setSelected', defaultPaymentCurrency);
        $('#Currency').trigger('change');
        $("#Date").val("");
        $("#VoucherNumber").val("");
        $("#ExchangeRate").val("");
    }

    static SaveVouchers() {
        if (rowsVoucher.length > 0) {
            var totalConcept = 0;
            var totalTax = 0;
            var totalRetention = 0;

            selectedClaim.Vouchers = []; var list = [];
            for (var i = 0; i < rowsVoucher.length; i++) {
                totalConcept += parseFloat(rowsVoucher[i].ConceptValue);
                totalTax += parseFloat(rowsVoucher[i].TaxValue);
                totalRetention += parseFloat(rowsVoucher[i].Retention);
                selectedClaim.Vouchers.push(rowsVoucher[i]);
            }

            if ((totalConcept + currentClaim.PaymentValue) > currentClaim.EstimateAmount) {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ConceptsValueExceedEstimation, 'autoclose': true });
            } else {
                selectedClaim.TotalConcept = totalConcept;
                selectedClaim.TotalTax = totalTax;
                selectedClaim.Retention = totalRetention;
                selectedClaim.Total = (totalTax + totalConcept + totalRetention);
                Payment.AddPaymentAndSummary(selectedClaim);
                Payment.SetTotalFinish();
                $("#tblClaims").UifDataTable('unselect');
                $('#tblClaims').UifDataTable('editRow', selectedClaim, currentPositionClaim);
                $('#modalMultipleBillingInformationDialog').UifModal('hide');
            }

            var descriptionVoucher = $("#inputDescription").val() + " Facturas: ";

            var vouchersNumber = [];

            $.each(rowsVoucher, function (index, value) {
                vouchersNumber.push(this.Number);
            });

            descriptionVoucher += vouchersNumber.join(", ");
            $("#inputDescription").val(descriptionVoucher);
        }
    }

    static SaveConcepts() {
        if (rowsConcept.length > 0) {
            var totalConcept = 0;
            var totalTax = 0;
            var totalRetention = 0;
            var selectCurrencyId = -1;

            for (var i = 0; i < rowsConcept.length; i++) {
                totalConcept += parseFloat(rowsConcept[i].Value);
                totalTax += parseFloat(rowsConcept[i].TaxValue);
                totalRetention += parseFloat(rowsConcept[i].Retention);
                if (rowsConcept[i].ConceptTaxes != null) {
                    $.each(rowsConcept[i].ConceptTaxes, function (index, value) {
                        if (this.TaxRate.toString().includes("%")) {
                            this.TaxRate = parseFloat(this.TaxRate.toString().slice(0, this.TaxRate.toString().length - 1));
                        }
                    });
                }
            }

            var totalValue = (totalConcept + totalTax + totalRetention);

            if ($("#_chkMultipleBilling").is(':checked')) {
                var selected = $("#tblBillingInformation").UifDataTable('getSelected')[0];
            }

            selectCurrencyId = $("#selectPaymentCurrency").UifSelect("getSelected");


            if (currentClaim.CurrencyId != selectCurrencyId) {
                totalValue = totalValue / exchangeRate.SellAmount;
                totalConcept = totalConcept / exchangeRate.SellAmount;
            }

            if ((totalConcept + currentClaim.PaymentValue) > currentClaim.EstimateAmount) {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ConceptsValueExceedEstimation, 'autoclose': true });
            }
            else {                
                var selected = null;
                if ($("#_chkMultipleBilling").is(':checked')) {
                    selected = $("#tblBillingInformation").UifDataTable('getSelected')[0];
                    selected.ConceptValue = totalConcept;
                    selected.TaxValue = totalTax;
                    selected.Retention = totalRetention;
                    selected.Total = (totalConcept + totalTax + totalRetention);

                    selected.Concepts = rowsConcept;
                    $("#tblBillingInformation").UifDataTable('unselect');
                    $('#tblBillingInformation').UifDataTable('editRow', selected, currentPositionVoucher);
                    $('#modalSingleBillingInformationDialog').UifModal('hide');
                    rowsConcept = [];
                } else {
                    selected = $("#tblClaims").UifDataTable('getSelected')[0];
                    selected.TotalConcept = totalConcept;
                    selected.TotalTax = totalTax;
                    selected.TotalRetention = totalRetention;
                    selected.Total = (totalConcept + totalTax + totalRetention);
                    selected.Vouchers = Payment.CreateVoucherHead(rowsConcept);
                    Payment.AddPaymentAndSummary(selected);
                    Payment.SetTotalFinish();
                    $("#tblClaims").UifDataTable('unselect');
                    $('#tblClaims').UifDataTable('editRow', selected, currentPositionClaim);
                    $('#modalSingleBillingInformationDialog').UifModal('hide');
                }
                $("#tblSummaryClaims .div-button-single").hide();
            }
        } else {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.SelectPaymentConcept, 'autoclose': true });
        }
    }

    static SetTotalFinish() {
        var totalConcept = 0;
        var totalTax = 0;
        var totalRetention = 0;
        /*****TOTAL CONCEPTOS*****/
        $.each($("#tblSummaryClaims").UifDataTable('getData'), function (index, valueSummary) {
            $.each(valueSummary.Vouchers, function (index, valueVoucher) {
                $.each(valueVoucher.Concepts, function (index, valueConcepts) {
                    totalConcept += parseFloat(valueConcepts.Value);
                    totalTax += parseFloat(valueConcepts.TaxValue);
                    totalRetention += parseFloat(valueConcepts.Retention);
                    if (valueConcepts.ConceptTaxes != null) {
                        $.each(valueConcepts.ConceptTaxes, function (index, value) {
                            if (this.TaxRate.toString().includes("%")) {
                                this.TaxRate = parseFloat(this.TaxRate.toString().slice(0, this.TaxRate.toString().length - 1));
                            }
                        });
                    }
                });
            });
        });
        let Total = (totalConcept + totalTax + totalRetention);
        $("#inpuntTotalFinish").val(FormatMoney(Total.toFixed(2)));
    }

    static AddPaymentAndSummary(paymentConcept) {
        var update = false;
        var summaryList = $("#tblSummaryClaims").UifDataTable('getData');
        /***SUMMARY***/
        if (summaryList.length > 0) {
            $.each(summaryList, function (index, value) {
                if (value.ClaimId == paymentConcept.ClaimId && value.EstimationTypeId == paymentConcept.EstimationTypeId && paymentConcept.SubClaim == value.SubClaim) {
                    $('#tblSummaryClaims').UifDataTable('editRow', paymentConcept, index);
                    update = true;
                }
            });
            if (!update) {
                $('#tblSummaryClaims').UifDataTable('addRow', paymentConcept);
            }
        } else {
            $('#tblSummaryClaims').UifDataTable('addRow', paymentConcept);
        }

        update = false;
    }

    static CreateVoucherHead(concepts) {
        var vouchers = [];
        var voucher = {
            Id: 0,
            VoucherTypeId: $("#selectVoucherType").UifSelect("getSelected"),
            CurrencyId: $("#selectInvoiceCurrency").UifSelect("getSelected"),
            Number: $("#inputVoucherNumber").val(),
            Date: $("#inputVoucherDate").val(),
            ExchangeRate: NotFormatMoney($("#inputExchangeRate").val()),
            ClaimNumber: $("#inputClaimNumber").val(),
            Concepts: concepts
        }
        vouchers.push(voucher);
        return vouchers;
    }

    static CloseConcepts() {
        if (rowsConcept.length > 0) {
            rowsConcept = [];
            paymentRequest.paymentConcepts = [];
            $('#tblSingleBillingInformation').UifDataTable('clear');
            $("#tblClaims").UifDataTable('unselect');
            $("#tblSummaryClaims").UifDataTable('unselect');
            $('#modalSingleBillingInformationDialog').UifModal('hide');
            $("#tblBillingInformation").UifDataTable('unselect');
            $("#tblBillingInformation").UifDataTable('clear');
        }
        else {
            $("#tblClaims").UifDataTable('unselect');
        }
    }

    static ClearItemSelect() {
        $("#tblClaims").UifDataTable('unselect');
        $("#tblSummaryClaims").UifDataTable('unselect');
    }

    static ClearConcepts() {
        if ($("#_chkMultipleBilling").is(':checked')) {
            $.each(rowsVoucher, function (index, value) {
                this.Concepts = [];
            });
        }
        else {
            Payment.CloseConcepts();
        }
    }

    static bindConcept(vm) {
        $("#editForm").find("#inputPaymentConceptId").val(vm.PaymentConceptId);
        $("#editForm").find("#inputPaymentConcept").val(vm.PaymentConcept);
        $("#editForm").find("#inputConceptValue").val(FormatMoney(vm.Value));
        $("#editForm").find("#inputTaxValue").val(FormatMoney(vm.TaxValue));
    }

    static binVoucher(vm) {
        $("#editFormMultiPaymentConcept").find("#VoucherType").val(vm.VoucherTypeId);
        $("#editFormMultiPaymentConcept").find("#VoucherNumber").val(vm.Number);
        $("#editFormMultiPaymentConcept").find("#Date").val(FormatDate(vm.Date));
        $("#editFormMultiPaymentConcept").find("#Currency").val(vm.CurrencyId);
        $("#editFormMultiPaymentConcept").find("#ExchangeRate").val(FormatMoney(vm.ExchangeRate));
    }

    static binOneVoucher(vm) {
        $("#selectVoucherType").UifSelect('setSelected', vm.VoucherTypeId);
        $("#inputVoucherNumber").val(vm.Number);
        $("#inputVoucherDate").val(FormatDate(vm.Date));
        $("#selectInvoiceCurrency").val(vm.CurrencyId);
        $("#inputExchangeRate").val(FormatMoney(vm.ExchangeRate));
    }

    static DoPayClaim() {
        if (modelSearchCriteria.claimNumber == null || modelSearchCriteria.policyDocumentNumber == null || modelSearchCriteria.branchId == null || modelSearchCriteria.prefixId == null)
            return;

        Payment.GetPrefixes(function (prefixes) {
            $('#selectPrefixesofClaims').UifSelect({ sourceData: prefixes, enable: false });
            $('#selectPrefixesofClaims').UifSelect('setSelected', modelSearchCriteria.prefixId);

            Payment.GetBranches(function (branches) {
                $('#selectPaymentBranch').UifSelect({ sourceData: branches });
                $('#selectBranch').UifSelect({ sourceData: branches, enable: false });
                $('#selectBranch').UifSelect('setSelected', modelSearchCriteria.branchId);

                $('#inputPolicyDocumentNumber').prop('disabled', true);
                $('#inputPolicyDocumentNumber').val(modelSearchCriteria.policyDocumentNumber);

                $('#inputClaimNumber').prop('disabled', true);
                $('#inputClaimNumber').val(modelSearchCriteria.claimNumber);

                Payment.GetClaimByPrefixIdBranchIdPolicyDocumentNumberClaimNumber();
            });
        });
    }

    static GetTaxAttributes() {
        ClaimsPaymentRequest.GetTaxAttributes().done(function (response) {
            if (response.success) {
                TaxAttributes = response.result;
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
            }
        });
    }

    static GetTaxCodeOfRetetionToIndustryAndCommerce() {
        ClaimsPaymentRequest.GetTaxCodeOfRetetionToIndustryAndCommerce().done(function (response) {
            RTEICACode = response.result;
        });
    }

    static FillTaxAttributes() {

        if (!TaxAttributes.some(x => x.Description == 'ROLE_CODE')) {
            TaxAttributes.push({
                Id: TaxAttributes.length,
                Description: 'ROLE_CODE',
                Value: 0
            });
        }

        $.each(TaxAttributes, function (index, value) {
            switch (this.Description) {
                case "BRANCH_CODE":
                    this.Value = $("#selectPaymentBranch").UifSelect('getSelected');
                    break;
                case "LINE_BUSINESS_CODE":
                    this.Value = $("#selectPrefixesofClaims").UifSelect('getSelected');
                    break;
                case "COUNTRY_CODE":
                    this.Value = $("#CountryLiquidationICA").UifSelect('getSelected');
                    break;
                case "STATE_CODE":
                    this.Value = $("#StateLiquidationICA").UifSelect('getSelected');
                    break;
                case "CITY_CODE":
                    this.Value = $("#CityLiquidationICA").UifSelect('getSelected');
                    break;
                case "ROLE_CODE":
                    this.Value = $("#selectPayTo").UifSelect('getSelected');
                    break;
                default:
            }
        });
    }

    static NotFormatMoneyIn() {
        $(this).val(NotFormatMoney($(this).val()));
    }

    static FormatMoneyOut() {
        if ($(this).val() != '')
            $(this).val(FormatMoney($(this).val().includes(',') ? $(this).val().replace(',', '.') : $(this).val()));
    }

    static ShowFormBilling() {
        if ($("#_chkMultipleBilling").is(':checked')) {
            $("#divBilling").hide();
        }
        else {
            $("#divBilling").show();
        }
    }

    static AcceptPaymentMethods() {
        $('#formPaymentMethods').valid();
        if ($("#formPaymentMethods").valid()) {
            $('#modalPaymentMethods').UifModal('hide');
        }
    }

    static AcceptClaimCheck() {
        $('#formClaimCheck').valid();
        if ($("#formClaimCheck").valid()) {
            $('#modalClaimCheck').UifModal('hide');
        }
    }
    static AcceptClaimViaBaloto() {
        $('#formClaimViaBaloto').valid();
        if ($("#formClaimViaBaloto").valid()) {
            $('#modalClaimViaBaloto').UifModal('hide');
        }
    }
    static AcceptClaimpaymentoffice() {
        $('#formClaimCheck').valid();
        if ($("#formClaimpaymentoffice").valid()) {
            $('#modalClaimpaymentoffice').UifModal('hide');
        }
    }

    static ClearForm() {
        $('#selectPaymentSource').UifSelect('setSelected', 1);
        $('#selectMovementType').UifSelect('setSelected', null);
        $('#selectPaymentBranch').UifSelect('setSelected', null);
        $('#inputEstimatedDate').val("");
        $('#selectPayTo').UifSelect('setSelected', null);
        $('#selectPaymentCurrency').UifSelect('setSelected', null);
        $('#selectPaymentMethod').UifSelect('setSelected', null);
        $('#inpuntTotalAmount').val("");
        $('#inputDescription').val("");
        $('#selectVoucherType').UifSelect('setSelected', null);
        $('#inputVoucherNumber').val("");
        $('#inputVoucherDate').val("");
        $('#selectInvoiceCurrency').UifSelect('setSelected', null);
        $('#inputExchangeRate').val("");
        $('#selectBranch').UifSelect("disabled", false);
        $('#selectPrefixesofClaims').UifSelect("disabled", false);
        $('#inputClaimNumber').prop("disabled", false);
        $('#inputPolicyDocumentNumber').prop('disabled', false);
        $('#selectBranch').UifSelect('setSelected', null);
        $('#selectPrefixesofClaims').UifSelect('setSelected', null);
        $('#inputPolicyDocumentNumber').val("");
        $('#inputClaimNumber').val("");
        $('#DocumentNumberInsured').UifAutoComplete('clean');
        $('#FullNameInsured').UifAutoComplete('clean');
        $('#DocumentNumberSupplier').UifAutoComplete('clean');
        $('#FullNameSupplier').UifAutoComplete('clean');
        $('#tblClaims').UifDataTable('clear');
        $('#tblSingleBillingInformation').UifDataTable('clear');
        $('#tblBillingInformation').UifDataTable('clear');
        $("#tblSummaryClaims").UifDataTable('clear');
        $('#inputConfirmDocument').val("");
        $("#inpuntTotalFinish").val("");
        $("#InputDocumentNumberThirdParty").UifAutoComplete('clean');
        $("#InputFullNameThirdParty").UifAutoComplete('clean');
        rowsConcept = [];
        paymentRequest = {};
        policyProductId = 0;
        policySalePointId = 0;
        IndividualId = 0;
        IndividualDocumentTypeId = 0;
        paymentCurrencyExchangeRate = 0;
        $('.hideSearchClaim').hide();
        $("#divRteICA").hide();
        RTEICACode = 0;
        IndividualTaxCondition = [];
        $('#CountryLiquidationICA').UifSelect('setSelected', null);
        $('#StateLiquidationICA').UifSelect('setSelected', null);
        $('#CityLiquidationICA').UifSelect('setSelected', null);
    }

    static SetParticipationInformation(event, participant) {
        $('#InputDocumentNumberThirdParty').UifAutoComplete('setValue', participant.DocumentNumber);
        $('#InputFullNameThirdParty').UifAutoComplete('setValue', participant.FullName);
        IndividualId = participant.IndividualId;
        IndividualDocumentTypeId = participant.DocumentTypeId;
        Payment.GetAccountingConceptsByBranchIdMovementTypeIdPersonTypeIdIndividualId();
    }

    static ClearParticipationInformation() {
        if ($('#InputDocumentNumberThirdParty').UifAutoComplete('getValue') != "") {
            IndividualId = 0;
            IndividualDocumentTypeId = 0;
        }
    }

    static CreatePrintPaymentRequestById(paymentRequestId) {
        var controller = rootPath + "Claims/PaymentRequest/GetReportPaymentRequestByPaymentRequestId?paymentRequestId=" + paymentRequestId;
        window.open(controller, 'mywindow', 'fullscreen=yes, scrollbars=auto');
    }

    static CreatePrintPaymentRequest() {
        if (modelSearchCriteria.paymentRequestId == null)
            return;
        var controller = rootPath + "Claims/PaymentRequest/GetReportPaymentRequestByPaymentRequestId?paymentRequestId=" + modelSearchCriteria.paymentRequestId;
        window.open(controller, 'mywindow', 'fullscreen=yes, scrollbars=auto');
    }
}