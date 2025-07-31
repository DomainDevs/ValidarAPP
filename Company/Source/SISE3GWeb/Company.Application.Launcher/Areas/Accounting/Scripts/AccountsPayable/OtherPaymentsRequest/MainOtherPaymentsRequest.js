/*----------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*                                               DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                                                 */
/*----------------------------------------------------------------------------------------------------------------------------------------------------------*/
var timeOtherPaymentRequest = window.setInterval(validateCompanyOtherPaymentRequest, 1000);
var idsTax;
var paymentMethodsPromise;
var paymentMethodsTime;

var OtherPaymentsPaymentOrigin = "#OtherPaymentsPaymentOrigin";
var rowModelTax = new RowModelTax();
function RowModelTax() {
    this.TaxDescription = "";
    this.TaxGroup = "";
    this.Amount = 0;
    this.Rate = 0;
    this.TaxAmount = 0;
    this.MinimumTax = 0;
}

var payToValue = "";
var chagePayment = true;
var documentNumber = "";

var agentTypeId = -1;
var personCode = 0;
var paymentSourceOthers = $("#ViewBagPaymentSources").val();
var TaxConditionTitle = "";
var percentageDifferencePo = 0;
var maxPo = 0;
var minPo = 0;
var rateExchangePo = 0;
var VoucherType = "";
var VoucherNumber = 0;
var VoucherCurrency = "";
var PaymentConceptTypeId = 0;
var PaymentBranchId = 0;
var rate;
var index = 0;
var indexConcept = 0;
var coverageIdTax = 0;
var totalTaxValueConcept = 0;
var enableForSave = false;

var oPaymentRequestModel = {
    PaymentRequestInfoId: 0,
    PaymentRequestSource: 0,
    PaymentRequestBranch: 0,
    PaymentRequestEstimatedDate: null,
    PaymentRequestRegistrationDate: null,
    PaymentRequestPaymentDate: null,
    PaymentRequestPersonTypeId: 0,
    PaymentRequestIndividualId: 0,
    PaymentRequestPaymentMethodId: 0,
    PaymentRequestCurrencyId: 0,
    PaymentRequestUserId: 0,
    PaymentRequestIsPrinted: false,
    PaymentRequestTotalAmount: 0,
    PaymentRequestDescription: "",
    PaymentRequestPaymentMovementTypeId: 0,
    PaymentRequestPrefixId: 0,
    PaymentRequestAccountBankId: 0,
    PaymentRequestType: 0,
    PaymentClaim: [],
    PaymentTax: [],
    PaymentRequestCompanyId: 0,
    PaymentRequestSalePointId: 0
};


//***********************************
//TaxCategories
var pivot = 0;
var totCountTax = 0;
var pivotSet = false;
var j = 0;
var taxRateResult = 0;

var TaxIds = [];
var taxGrid = false;

//Objeto para almacenar los TaxCategories
var oTax = {
    TaxCategory: []
};
var oTaxCategory = {
    TaxId: 0,
    TaxCategoryId: 0,
    TaxCategoryDescript: "",
    TaxCondition: 0
};
//**********************************
//para obtener en memoria los valores de la suma

var taxtModel = {
    Concept: []
};
var taxHeadertModel = {
    Index: 0,
    TaxConcept: []
};

var taxDetailtModel = {
    Index: 0,
    TaxDescription: "",
    TaxGroup: "",
    Amount: 0,
    Rate: 0,
    TaxAmount: 0,
    MinimumTax: 0
};
//***********************************

//Variable PaymentClaim.
var oPaymentClaimModel = {
    PaymentClaimId: 0,
    PaymentClaimCoverages: []
};

//Variable PaymentClaimCoverages
var oPaymentClaimCoveragesModel = {
    PaymentClaimCoveragesId: 0,
    PaymentClaimCoveragesSubClaim: 0,
    PaymentClaimCoveragesClaimAmount: []
};

//Variable para las Estimaciones
var oPaymentClaimCoveragesClaimAmountModel = {
    PaymentClaimCoveragesClaimAmountId: 0,
    PaymentClaimCoveragesClaimAmountEstimationTypeId: 0,
    PaymentClaimCoveragesClaimAmountVersion: 0,
    PaymentClaimCoveragesClaimAmountDate: null,
    PaymentClaimCoveragesClaimAmountEstimatedAmount: 0,
    PaymentClaimCoveragesClaimAmountDeductibleAmount: 0,
    PaymentClaimCoveragesClaimAmountPaymentTypeId: 0,
    PaymentClaimCoveragesClaimAmountVoucher: []
};

//Variable Voucher
var oVoucherModel = {
    Index: 0,
    VoucherId: 0,
    VoucherType: 0,
    VoucherNumber: 0,
    VoucherDate: null,
    VoucherExchangeRate: 0,
    VoucherCurrencyId: 0,
    VoucherConcept: []
};

//Variable VoucherConcept
var oVoucherConceptModel = {
    VoucherConceptId: 0,
    VoucherConceptPaymentConcept: 0,
    VoucherConceptValue: 0,
    VoucherConceptTaxValue: 0,
    VoucherConceptDescription: "",
    VoucherConceptRetentionValue: 0,
    VoucherConceptCostCenterId: 0
};

//Variable PaymentTaxCategory
var oPaymentTaxCategoryModel = {
    TaxId: 0,
    TaxCategoryId: 0,
    TaxCondition: 0,
    TaxCategoryDescript: ""
};


var codAbona = -1;

var paymentConceptId;
var TotalPaymentAmount;
var TotalPaymentTaxes;
var TotalPaymentRetention;

var index = 0;
var individualId = -1;
var individualName = '';
var editIndex;
var factCorrela = -1;
var accountBankId = -1;

var PaymentRequestId;
var exchangeRate;

var accountingAccountId = 0;
var conceptCode = 0;
var accountingAccountName = '';

var accountingPaymentConceptId = "";
var accountingConceptText = "";

function RowPaymentConceptModel() {
    this.Index;
    this.PaymentConceptId;
    this.PaymentConceptDescription;
    this.PaymentConceptIdDescription;
    this.CostCenterId;
    this.CostCenterDescription;
    this.CostCenterIdDescription;
    this.Amount;
    this.Taxes;
    this.Retentions;
    this.PercentageTaxes;
    this.PercentageRetentions;
    this.Taxid;
    this.GroupId;
    this.ConditionId;
    this.TaxSaved;
    this.TaxDontSave;
    this.treatmentId;
    this.VoucherName;

}

function RowOtherPaymentModel() {
    this.TaxId;
    this.TaxDescription;
    this.IndividualId;
    this.TaxConditionId;
    this.TaxConditionDescription;
    this.HasNationalRate;
    this.RateDescription;
}

function RowPaymentRequest() {
    this.Index;
    this.VoucherTypeId;
    this.VoucherTypeDescription;
    this.VoucherType;
    this.VoucherNumber;
    this.Date;
    this.CurrencyId;
    this.Currency;
    this.PaymentConcept;
    this.Amount;
    this.Taxes
    this.Retentions;
    this.Id;
}

function RowTransferListView() {
    this.AccountBankId;
    this.IndividualId;
    this.BankDescription;
    this.AccountTypeDescription;
    this.Number;
    this.AccountBankEnabled;
    this.IsActive;
    this.Default;
}

function RowTaxes() {
    this.TaxId;
    this.TaxDescription;
    this.GroupCondition;
    this.GroupAmount;
    this.Rate;
    this.TaxAmount;
    this.MinimunTaxBase;
    this.MinimunTax;
    this.Branch;
}

$(document).ready(function () {

    $("#PaymentConcepts").find("#AddPayment").click(function () {
        $("#alertMainOtherPayment").UifAlert('hide');
        $("#PaymentConceptFormulario").validate();
        if ($("#PaymentConceptFormulario").valid()) {
            if (duplicateVoucherNumber($("#PaymentConcepts").find("#VoucherNumber").val()) == false) {
                addRowPaymentConcept();
                accountingAccountName = "";
                conceptCode = "";
                $("#PaymentConcepts").find("#PaymentConceptId").text("");
                $("#PaymentConcepts").find("#PaymentConceptDescription").text("");
            }
            else {
                $("#alertForm").UifAlert('show', Resources.InternalBallotDuplicateVoucher, "warning");
            }
        }
    });

    /////////////////////////////////////////
    //  Botón  Aceptar                     //
    /////////////////////////////////////////
    $("#PaymentConcepts").find("#AceptPayment").click(function () {
        $("#alertMainOtherPayment").UifAlert('hide');
        index += 1;
        indexConcept = 0;
        ids = $("#PaymentConcetpsListView").UifListView("getData");
        if (ids.length > 0) {
            SetDataPaymentRequest();
            var rowModel = new RowPaymentRequest();
            rowModel.Index = index;
            rowModel.VoucherTypeId = $("#PaymentConcepts").find("#VoucherType").val();
            rowModel.VoucherTypeDescription = $("#PaymentConcepts").find("#VoucherType option:selected").html();
            rowModel.VoucherType = $("#PaymentConcepts").find("#VoucherType").val();
            rowModel.VoucherNumber = $("#PaymentConcepts").find("#VoucherNumber").val();
            rowModel.Date = $("#PaymentConcepts").find("#CheckDate").val();
            rowModel.CurrencyId = $("#PaymentConcepts").find("#VoucherCurrency").val();
            rowModel.Currency = $("#PaymentConcepts").find("#VoucherCurrency option:selected").html();
            rowModel.PaymentConcept = ClearFormatCurrency($("#ExchangeRate").val());

            delRowOtherPayments("PaymentConcetpsListView");
            rowModel.Amount = $("#TotalPaymentAmount").val();
            rowModel.Taxes = $("#TotalPaymentTaxes").val();
            rowModel.Retentions = $("#TotalPaymentRetention").val();
            rowModel.Id = "0";

            CleanDataVoucherEmpty();
            SetDataVoucher(rowModel);

            if (editIndex == -1) {
                $('#taxesListView').UifListView("addItem", rowModel);
            }
            else {
                $('#taxesListView').UifListView("editItem", editIndex, rowModel);
                editIndex = -1;
            }

            TotalPaymentAmount = FormatCurrency(FormatDecimal(totalAmountOtherPayments("taxesListView")));
            TotalPaymentTaxes = FormatCurrency(FormatDecimal(totalTax("taxesListView")));
            TotalPaymentRetention = FormatCurrency(FormatDecimal(totalRetention("taxesListView")));

            $('#VouchersTotal').text(TotalPaymentAmount);
            $('#VouchersTotal').val(TotalPaymentAmount);

            $('#VoucherTaxesTotal').text(TotalPaymentTaxes);
            $('#VoucherTaxesTotal').val(TotalPaymentTaxes);

            $('#VoucherRetentionsTotal').text(TotalPaymentRetention);
            $('#VoucherRetentionsTotal').val(TotalPaymentRetention);
                       
            $('#PaymentConcepts').UifModal('hide');
            clearPaymentConceptsModal();

            $("#PaymentConcepts").find("#VoucherNumber").removeAttr("disabled");
            $("#PaymentConcepts").find("#CheckDate").removeAttr("disabled");
            $("#PaymentConcepts").find("#VoucherType").removeAttr("disabled");
            $("#PaymentConcepts").find("#ExchangeRate").removeAttr("disabled");
            $("#PaymentConcepts").find("#VoucherCurrency").removeAttr("disabled");
            $("#PaymentConcepts").find("#VoucherCurrencyPaymentConcept").removeAttr("disabled");
        }
        else {
            $("#alertForm").UifAlert('show', Resources.RequiredConcept, "warning");
        }
    });

    /**********Impuestos por categoria del abonador*****/
    $("#modalTaxSelection").find("#btnTaxSelection").click(function () {
        var dataSelection = $("#modalTaxSelection").find("#TaxSelectionTable").UifDataTable("getSelected");
        if (dataSelection != null) {
            if (dataSelection.length >= 1) {
                ControlCheckOtherPayments();
            }
            else {
                $("#alertTaxSelection").UifAlert('show', Resources.SelectTax, "warning");
            }
        }
        else {
            $("#alertTaxSelection").UifAlert('show', Resources.SelectTax, "warning");
        }

    });

    /////////////////////////////////////////
    //  Botón  Imprimir                     //
    /////////////////////////////////////////
    $("#ModalSuccessPrint").find("#PrintDetail").click(function () {
        loadOtherPaymentsRequestReport(PaymentRequestId);
        ClearAllOtherPayments();
        
        $('#ModalSuccessPrint').UifModal('hide');
    });

    /////////////////////////////////////////
    //  Muestra modal impresión            //
    /////////////////////////////////////////
    $("#ModalSuccessPrint").find("#PrintAcept").click(function () {
        ClearAllOtherPayments();
        
        $('#ModalSuccessPrint').UifModal('hide');
    });

    ///////////////////////////////////////////////////////
    ///  Autocomplete Descripción Concepto de Pago      ///
    ///////////////////////////////////////////////////////
    $("#PaymentConcepts").find("#PaymentConceptDescription").on('blur', function (event) {
        setTimeout(function () {
            $("#PaymentConcepts").find("#PaymentConceptDescription").val(accountingAccountName);
        }, 50);
    });

    $("#PaymentConcepts").find('#PaymentConceptDescription').on('itemSelected', function (event, selectedItem) {
        accountingAccountId = 0;
        $("#PaymentConcepts").find('#PaymentConceptId').val(selectedItem.Id);
        conceptCode = selectedItem.Id;
        accountingAccountId = selectedItem.GeneralLedgerId;
        accountingPaymentConceptId = selectedItem.Id.toString();
        accountingConceptText = selectedItem.Description;
        accountingAccountName = selectedItem.Description;
        loadCostCenter();
    });

    $("#PaymentConcepts").find("#PaymentConceptDescription").on('keypress', function (event, selectedItem) {
        $("#alertForm").UifAlert('hide');
        if (!event.altKey) {
            accountingPaymentConceptId = "";
            $("#PaymentConcepts").find('#PaymentConceptId').val("");
        }
    });

    /////////////////////////////////////////
    //  Autocomplete conceptos  de pago    //
    /////////////////////////////////////////
    $("#PaymentConcepts").find("#PaymentConceptId").on('blur', function (event) {
        $("#alertForm").UifAlert('hide');
        setTimeout(function () {
            $("#PaymentConcepts").find('#PaymentConceptId').val(conceptCode);
        }, 50);
    });

    $("#PaymentConcepts").find('#PaymentConceptId').on('itemSelected', function (event, selectedItem) {
        $("#alertForm").UifAlert('hide');
        accountingAccountId = 0;
        $("#PaymentConcepts").find('#PaymentConceptDescription').val(selectedItem.Description);
        conceptCode = selectedItem.Id;
        accountingPaymentConceptId = selectedItem.Id.toString();
        accountingConceptText = selectedItem.Description;
        accountingAccountId = selectedItem.GeneralLedgerId;
        accountingAccountName = selectedItem.Description;
        loadCostCenter();
    });

    $("#PaymentConcepts").find("#PaymentConceptId").on('keypress', function (event, selectedItem) {
        if (!event.altKey) {
            accountingPaymentConceptId = "";
            $("#PaymentConcepts").find('#PaymentConceptDescription').val("");
        }
    });

    /////////////////////////////////////////
    //  Dropdown Moneda                    //
    /////////////////////////////////////////
    $("#PaymentConcepts").find('#VoucherCurrency').on('itemSelected', function (event, selectedItem) {
        $("#alertForm").UifAlert('hide');
        if (selectedItem.Id != "") {
            $("#PaymentConcepts").find('#VoucherCurrencyPaymentConcept').val($("#PaymentConcepts").find("#VoucherCurrency option:selected").html())
            rate = getCurrencyRate($("#ViewBagAccountingDate").val(), selectedItem.Id);
            $("#PaymentConcepts").find("#ExchangeRate").val(FormatCurrency(FormatDecimal(rate[0])))

        }
    });

    GetSelectOtherPaymentsPaymentOrigin();

    $("#modalTaxSelection").find("#TaxSelectionTable").on('rowSelected', function (event, data, position) {
        $("#alertTaxSelection").UifAlert('hide');
    });
    $("#modalTaxSelection").find("#TaxSelectionTable").on('selectAll', function (event, data, position) {
        $("#alertTaxSelection").UifAlert('hide');
    });

    ////////////////////////////////////////////////////////////////
});//fin $(document).ready

//Se ocultan campos de busqueda de autocompletes
$(window).load(function () {
    cleanAutocompletesOtherPayment('SearchSuppliers');
    cleanAutocompletesOtherPayment('SearchInsured');
    cleanAutocompletesOtherPayment('SearchCoinsurance');
    cleanAutocompletesOtherPayment('SearchPerson');
    cleanAutocompletesOtherPayment('SearchAgent');
    cleanAutocompletesOtherPayment('SearchEmployee');
    cleanAutocompletesOtherPayment('SearchReinsurer');

    //Cargando los eventos
    loadAutocompleteEventOtherPayment('SearchSuppliers');
    loadAutocompleteEventOtherPayment('SearchInsured');
    loadAutocompleteEventOtherPayment('SearchCoinsurance');
    loadAutocompleteEventOtherPayment('SearchPerson');
    loadAutocompleteEventOtherPayment('SearchAgent');
    loadAutocompleteEventOtherPayment('SearchEmployee');
    loadAutocompleteEventOtherPayment('SearchReinsurer');

    $('#TransferPanel').hide();
    var controller = ACC_ROOT + "OtherPaymentsRequest/GetEnabledPaymentMethodTypes";
    $("#OtherPaymentsPaymentMethods").UifSelect({ source: controller });
});


//Xhr de autocomplete de varios parametros
 $(document).ajaxSend(function (event, xhr, settings) {

    if (settings.url.indexOf("GetPaymentConceptById") != -1) {
        if ($("#OtherPaymentsBranch").val() == undefined) {
            settings.url = settings.url + "&param=" + $("#AccountingBranch").val() + "/" + personCode + "/1";
        }
        else {
            settings.url = settings.url + "&param=" + $("#OtherPaymentsBranch").val() + "/0/1";
        }
    }
    if (settings.url.indexOf("GetPaymentConceptByDescription") != -1) {
        if ($("#OtherPaymentsBranch").val() == undefined) {
            settings.url = settings.url + "&param=" + $("#AccountingBranch").val() + "/" + personCode + "/2";
        }
        else {
            settings.url = settings.url + "&param=" + $("#OtherPaymentsBranch").val() + "/0/2";
        }
    }
});


/*-------------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*                                                      DEFINICIÓN DE FUNCIONES                                                                                */
/*-------------------------------------------------------------------------------------------------------------------------------------------------------------*/

if ($("#ViewBagBranchDisableMainOther").val() == "1") {
    setTimeout(function () {
        $("#OtherPaymentsBranch").attr("disabled", "disabled");
    }, 300);
}
else {
    $("#OtherPaymentsBranch").removeAttr("disabled");
}

setTimeout(function () {
    GetSalePoints();
}, 2000);


///////////////////////////////////////////////////
//  Borra objeto al borrarlo del listView        //
///////////////////////////////////////////////////
var deletetaxesListView = function (deferred, data) {
    if (oPaymentClaimModel.PaymentClaimCoverages[0].PaymentClaimCoveragesClaimAmount[0].PaymentClaimCoveragesClaimAmountVoucher.length > 0) {
        for (var i = 0; i <= (oPaymentClaimModel.PaymentClaimCoverages[0].PaymentClaimCoveragesClaimAmount[0].PaymentClaimCoveragesClaimAmountVoucher.length); i++) {
            if (oPaymentClaimModel.PaymentClaimCoverages[0].PaymentClaimCoveragesClaimAmount[0].PaymentClaimCoveragesClaimAmountVoucher[i].Index == data.Index) {
                oPaymentClaimModel.PaymentClaimCoverages[0].PaymentClaimCoveragesClaimAmount[0].PaymentClaimCoveragesClaimAmountVoucher.splice(i, 1);
            }
        }
    }
    //elimino un comprobante de pago seleccionado
    deferred.resolve();
    setTimeout(function () {
        delRowOtherPayments("taxesListView");
    }, 2000);

    if (taxtModel.Concept.length > 0) {
        var idelete = 0;
        for (var i = 0; i <= taxtModel.Concept.length; i++) {
            if (taxtModel.Concept[i].Index == data.Index) {
                idelete = taxtModel.Concept[i].TaxConcept.length;
                for (var j = 0; j < idelete; j++) {
                    taxtModel.Concept[i].TaxConcept.splice(j, 1);
                }
                taxtModel.Concept.splice(i, 1);
            }
        }
    }
};

//////////////////////////////////////////////////////*//////////
//  Borra objeto al borrarlo del listView Concepto de Pagos    //
/////////////////////////////////////////////////////////////////
var deletePaymentConcept = function (deferred, data) {
    if (taxHeadertModel.TaxConcept.length > 0) {
        for (var i = 0; i < taxHeadertModel.TaxConcept.length; i++) {
            if (taxHeadertModel.TaxConcept[i].Index == data.Index) {
                taxHeadertModel.TaxConcept.splice(i, 1);
            }
        }
    }
    deferred.resolve();
    setTimeout(function () {
        delRowOtherPayments("PaymentConcetpsListView");
    }, 2000);

};



///////////////////////////////////////////////////
//  Obtiene última tasa de cambio                //
///////////////////////////////////////////////////
function loadExchangeRateDeafult(id) {

    var exchangeRateDropDown;
    var rate;

    if ($('#VoucherCurrency').val() == "") {
        rate = getCurrencyRate($("#ViewBagAccountingDate").val(), 0);
    }
    else {
        rate = getCurrencyRate($("#ViewBagAccountingDate").val(), $('#VoucherCurrency').val());
    }

    $('#ExchangeRate').val(rate[0]);
    exchangeRate = $("#PaymentConcepts").find("#ExchangeRate").val();
    $("#PaymentConcepts").find("#ExchangeRate").val(FormatCurrency(FormatDecimal(exchangeRate)))

    $("#PaymentConcepts").find("#PaymentConcetpsListView").UifListView({
        autoHeight: true,
        customAdd: false, customEdit: true, customDelete: false, localMode: true, add: false, edit: false, delete: true,
        displayTemplate: "#check-display-template", deleteCallback: deletePaymentConcept
    });
    
    $('#PaymentConcepts').UifModal('showLocal', Resources.PaymentConcept);

}

// se obtiene la tasa de cambio
function getCurrencyRate(accountingDate, currencyId) {
    var alert = true;

    var resp = new Array();
    lockScreen();
    setTimeout(function () {         

        $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "Common/GetCurrencyExchangeRate",
        data: JSON.stringify({ rateDate: accountingDate, currencyId: parseInt(currencyId) }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
            success: function (data) {
                
                unlockScreen();
            if (data != null) {
                resp[0] = data;
            }
        }
        });
    }, 500);
    return resp;
}

///////////////////////////////////////////////////
//  Llena dropDown Centro de costos              //
///////////////////////////////////////////////////
function loadCostCenter() {
    var controller = ACC_ROOT + "OtherPaymentsRequest/GetCostCenterByAccountingAccountId?accountingAccountId=" + accountingAccountId;
    $("#PaymentConcepts").find("#costCenter").UifSelect({ source: controller });
}

///////////////////////////////////////////////////
//  Ingresa fila en listView C. de Pagos          //
///////////////////////////////////////////////////
function addRowPaymentConcept() {
    var amountAux;

    var conditionCode = -1;
    var branchCode = -1;
    var currencyId = -1;
    var change = 0;
    var rowModel = new RowPaymentConceptModel();

    var categories = "";
    indexConcept++;

    if (duplicateRecordsOtherPayments($("#PaymentConcepts").find("#PaymentConceptId").val(), $("#PaymentConcepts").find("#costCenter").val()) == false) {
        $("#alertForm").UifAlert('hide');
        var rowModel = new RowPaymentConceptModel();
        rowModel.Index = indexConcept;
        rowModel.PaymentConceptId = $("#PaymentConcepts").find("#PaymentConceptId").val();
        conditionCode = $("#PaymentConcepts").find("#PaymentConceptId").val();
        currencyId = $("#PaymentConcepts").find("#VoucherCurrency").val();
        rowModel.PaymentConceptDescription = $("#PaymentConcepts").find("#PaymentConceptDescription").val();
        rowModel.PaymentConceptIdDescription = $("#PaymentConcepts").find("#PaymentConceptId").val() + " - " + $("#PaymentConcepts").find("#PaymentConceptDescription").val();
        rowModel.CostCenterId = $("#PaymentConcepts").find("#costCenter").val();
        rowModel.CostCenterDescription = $("#costCenter option:selected").html();
        rowModel.CostCenterIdDescription = $("#PaymentConcepts").find("#costCenter").val() + " - " + $("#PaymentConcepts").find("#costCenter option:selected").html();
        rowModel.Amount = ClearFormatCurrency($("#PaymentConcepts").find("#ConceptAmount").val());
        amountAux = ClearFormatCurrency($("#PaymentConcepts").find("#ConceptAmount").val());
        branchCode = $("#OtherPaymentsBranch").val();
        codAbona = $("#OtherPaymentsPayTo").val();
        exchangeRate = ClearFormatCurrency($("#ExchangeRate").val());
        rowModel.VoucherNumber = $("#PaymentConcepts").find("#VoucherNumber").val();
        rowModel.Taxes = 0;
        rowModel.Retentions = 0;

        lockScreen();
        setTimeout(function () {   
            $.ajax({
            type: "POST",
            url: ACC_ROOT + "OtherPaymentsRequest/GetIndividualTaxByIndividualId",
            data: JSON.stringify({
                "individualId": individualId
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                unlockScreen();
                if (data != undefined) {
                    if (data.length > 0) {
                        if (personCode == 0 || personCode == -1) {
                            personCode = individualId;
                        }
                        if (editIndex != -1) {
                            factCorrela = editIndex;
                        }

                        if (oPaymentRequestModel.PaymentTax != undefined) {

                            oPaymentRequestModel.PaymentTax = oTax.TaxCategory;

                            if (oPaymentRequestModel.PaymentTax.length > 0) {
                                for (var i = 0; i < oPaymentRequestModel.PaymentTax.length; i++) {
                                    categories = categories + oPaymentRequestModel.PaymentTax[i].TaxId + ";";
                                    categories = categories + oPaymentRequestModel.PaymentTax[i].TaxCategoryId + ";";
                                    conditionCode = oPaymentRequestModel.PaymentTax[i].TaxCondition;
                                }
                            } else {
                                categories = "";
                            }
                        } else {
                            categories = "";
                        }

                        $.ajax({
                            type: "POST",
                            url: ACC_ROOT + "OtherPaymentsRequest/GetTotalTax",
                            data: JSON.stringify({
                                "codAbona": codAbona, "branchCode": branchCode,
                                "individualId": personCode, "amount": parseFloat(amountAux),
                                "conditionCode": conditionCode, "agentTypeCode": agentTypeId,
                                "factCorrela": factCorrela, "currencyId": currencyId,
                                "exchangeRate": parseFloat(exchangeRate),
                                "categories": categories,
                                "coverageIdTax": coverageIdTax,
                            }),
                            dataType: "json",
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                if (data != null) {
                                    totalTaxValueConcept = parseFloat(data[0].TaxValue);
                                    rowModel.Taxes = parseFloat(data[0].TaxValue);
                                    rowModel.Retentions = parseFloat(data[0].RetentionValue);
                                    if (rowModel.Taxes >= 0) {
                                        //arma el objecto para ver los impuestos cargados(depende de la seleccion de impuestos)
                                        if (idsTax != null) {
                                            for (var i in idsTax) {
                                                var row = idsTax[i];
                                                taxDetailtModel = {
                                                    Index: 0,
                                                    TaxDescription: "",
                                                    TaxGroup: "",
                                                    Amount: 0,
                                                    Rate: 0,
                                                    TaxAmount: 0,
                                                    MinimumTax: 0,
                                                };
                                                taxDetailtModel.Index = indexConcept;
                                                taxDetailtModel.TaxDescription = row.TaxDescription;
                                                taxDetailtModel.TaxGroup = row.TaxConditionDescription;
                                                taxDetailtModel.Amount = 0;
                                                taxDetailtModel.Rate = 0;
                                                taxDetailtModel.TaxAmount = 0;
                                                taxDetailtModel.MinimumTax = 0;

                                                taxHeadertModel.TaxConcept.push(taxDetailtModel);
                                            }
                                        }

                                        setTimeout(function () {
                                            $("#PaymentConcepts").find("#PaymentConcetpsListView").UifListView("addItem", rowModel);

                                            TotalPaymentAmount = FormatCurrency(FormatDecimal(totalAmountOtherPayments("PaymentConcetpsListView")))
                                            TotalPaymentTaxes = FormatCurrency(FormatDecimal(totalTax("PaymentConcetpsListView")))
                                            TotalPaymentRetention = FormatCurrency(FormatDecimal(totalRetention("PaymentConcetpsListView")))

                                            $("#PaymentConcepts").find('#TotalPaymentAmount').text(TotalPaymentAmount);
                                            $("#PaymentConcepts").find('#TotalPaymentAmount').val(TotalPaymentAmount);

                                            $("#PaymentConcepts").find('#TotalPaymentTaxes').text(TotalPaymentTaxes);
                                            $("#PaymentConcepts").find('#TotalPaymentTaxes').val(TotalPaymentTaxes);

                                            $("#PaymentConcepts").find('#TotalPaymentRetention').text(TotalPaymentRetention);
                                            $("#PaymentConcepts").find('#TotalPaymentRetention').val(TotalPaymentRetention);
                                        }, 3000);
                                    }
                                    else {
                                        //-1 indica que existe un error de parametrización.
                                        $("#alertForm").UifAlert('show', Resources.ReviewParameterization, "warning");
                                    }
                                }
                            }
                        });
                    }
                    else {
                        rowModel.Taxes = 0;
                        rowModel.Retentions = 0;

                        setTimeout(function () {
                            $("#PaymentConcepts").find("#PaymentConcetpsListView").UifListView("addItem", rowModel);

                            TotalPaymentAmount = FormatCurrency(FormatDecimal(totalAmountOtherPayments("PaymentConcetpsListView")))
                            TotalPaymentTaxes = FormatCurrency(FormatDecimal(totalTax("PaymentConcetpsListView")))
                            TotalPaymentRetention = FormatCurrency(FormatDecimal(totalRetention("PaymentConcetpsListView")))

                            $("#PaymentConcepts").find('#TotalPaymentAmount').text(TotalPaymentAmount);
                            $("#PaymentConcepts").find('#TotalPaymentAmount').val(TotalPaymentAmount);

                            $("#PaymentConcepts").find('#TotalPaymentTaxes').text(TotalPaymentTaxes);
                            $("#PaymentConcepts").find('#TotalPaymentTaxes').val(TotalPaymentTaxes);

                            $("#PaymentConcepts").find('#TotalPaymentRetention').text(TotalPaymentRetention);
                            $("#PaymentConcepts").find('#TotalPaymentRetention').val(TotalPaymentRetention);
                        }, 3000);
                    }
                } else {
                    setTimeout(function () {
                        $("#PaymentConcepts").find("#PaymentConcetpsListView").UifListView("addItem", rowModel);

                        TotalPaymentAmount = FormatCurrency(FormatDecimal(totalAmountOtherPayments("PaymentConcetpsListView")))
                        TotalPaymentTaxes = FormatCurrency(FormatDecimal(totalTax("PaymentConcetpsListView")))
                        TotalPaymentRetention = FormatCurrency(FormatDecimal(totalRetention("PaymentConcetpsListView")))

                        $("#PaymentConcepts").find('#TotalPaymentAmount').text(TotalPaymentAmount);
                        $("#PaymentConcepts").find('#TotalPaymentAmount').val(TotalPaymentAmount);

                        $("#PaymentConcepts").find('#TotalPaymentTaxes').text(TotalPaymentTaxes);
                        $("#PaymentConcepts").find('#TotalPaymentTaxes').val(TotalPaymentTaxes);

                        $("#PaymentConcepts").find('#TotalPaymentRetention').text(TotalPaymentRetention);
                        $("#PaymentConcepts").find('#TotalPaymentRetention').val(TotalPaymentRetention);
                    }, 3000);
                }
            }
        });
        }, 500);
        clearFieldsOtherPayments();
        $("#PaymentConcepts").find("#VoucherNumber").attr("disabled", "disabled");
        $("#PaymentConcepts").find("#CheckDate").attr("disabled", "disabled");
        $("#PaymentConcepts").find("#VoucherType").attr("disabled", "disabled");
        $("#PaymentConcepts").find("#ExchangeRate").attr("disabled", "disabled");
        $("#PaymentConcepts").find("#VoucherCurrency").attr("disabled", "disabled");
        $("#PaymentConcepts").find("#VoucherCurrencyPaymentConcept").attr("disabled", "disabled");
    }
    else {
        $("#alertForm").UifAlert('show', Resources.DuplicateRecords, "warning");
    }
}

////////////////////////////////////////////////////////////////////
//  Valida que no existan rejistros duplicados en C. de Pagos     //
////////////////////////////////////////////////////////////////////
function duplicateRecordsOtherPayments(paymentConceptCode, costCenterCode) {
    var result = false;

    var ids = $("#PaymentConcetpsListView").UifListView("getData");

    if (ids != null) {
        for (var i in ids) {
            if ((paymentConceptCode == ids[i].PaymentConceptId) && (ids[i].CostCenterId == costCenterCode)) {
                result = true;
                break;
            }
        }
    }

    return result;
}


////////////////////////////////////////////////////////////////////
//  Valida que no existan # voucher duplicados                  //
////////////////////////////////////////////////////////////////////
function duplicateVoucherNumber(voucherNumber) {
    var result = false;

    var ids = $("#taxesListView").UifListView("getData");

    if (ids != null) {
        for (var i in ids) {
            if ((voucherNumber == ids[i].VoucherNumber)) {
                result = true;
                break;
            }
        }
    }

    return result;
}

///////////////////////////////////////////////////
//  Limpia campos y variables                   //
//////////////////////////////////////////////////
function clearFieldsOtherPayments() {
    $("#PaymentConcepts").find("#PaymentConceptId").val("");
    $("#PaymentConcepts").find("#PaymentConceptDescription").val("");
    $("#CostCenterId").val("");
    $("#CostCenterDescription").val("");
    $("#PaymentConceptIncomeAmount").val("");
    $("#PaymentConcepts").find("#ConceptAmount").val("");

    accountingAccountId = "-1";
    loadCostCenter();
}

///////////////////////////////////////////////////
//  Calcula totales                             //
//////////////////////////////////////////////////
function totalAmountOtherPayments(listView) {
    var total = 0;

    ids = $("#" + listView).UifListView("getData");
    if (ids.length > 0) {
        for (i in ids) {
            total += parseFloat(ClearFormatCurrency(ids[i].Amount.replace("", ",")));
        }
    }
    return total;
}

///////////////////////////////////////////////////
//  Calcula totales de impuestos                //
//////////////////////////////////////////////////
function totalTax(listView) {
    var total = 0;

    ids = $("#" + listView).UifListView("getData");
    if (ids.length > 0) {
        for (i in ids) {
            total += parseFloat(ClearFormatCurrency(ids[i].Taxes.toString().replace("", ",")));
        }
    }
    return total;
}

///////////////////////////////////////////////////
//  Calcula totales de retención                //
//////////////////////////////////////////////////
function totalRetention(listView) {
    var total = 0;
    ids = $("#" + listView).UifListView("getData");
    if (ids.length > 0) {
        for (i in ids) {
            total += parseFloat(ClearFormatCurrency(ids[i].Retentions.toString().replace("", ",")));
        }
    }
    return total;
}

///////////////////////////////////////////////////
//  Limpia campos del formulario //
//////////////////////////////////////////////////
function clearPaymentConceptsModal() {
    $("#PaymentConcepts").find("#VoucherType").val("");
    $("#PaymentConcepts").find("#VoucherNumber").val("");
    $("#PaymentConcepts").find("#CheckDate").val("");
    $("#PaymentConcepts").find("#VoucherCurrency").val("");
    $("#PaymentConcepts").find("#ExchangeRate").val("");
    $("#PaymentConcepts").find("#PaymentConceptId").val("");
    $("#PaymentConcepts").find("#PaymentConceptDescription").val("");
    $("#PaymentConcepts").find("#VoucherCurrencyPaymentConcept").val("");
    $("#PaymentConcepts").find("#ConceptAmount").val("");

}

//Limpia formato de Moneda
function setHeaderPayment() {
    oPaymentRequestModel.PaymentRequestTotalAmount = ClearFormatCurrency($("#OtherPaymentsTotal").val()).replace(",", ".");
}

///////////////////////////////////////////////////
//  Arma Objeto   oPaymentClaimCoveragesModel   //
//////////////////////////////////////////////////
function SetDataPaymentRequest() {

    oPaymentRequestModel.PaymentRequestInfoId = 0;
    oPaymentRequestModel.PaymentRequestSource = $("#OtherPaymentsPaymentOrigin").val();
    oPaymentRequestModel.PaymentRequestBranch = $("#OtherPaymentsBranch").val();
    oPaymentRequestModel.PaymentRequestEstimatedDate = $("#OtherPaymentsEstimatedPaymentDate").val();

    oPaymentRequestModel.PaymentRequestPaymentDate = $("#OtherPaymentsAccountingDate").val(),
        oPaymentRequestModel.PaymentRequestPersonTypeId = $("#OtherPaymentsPayTo").val();
    oPaymentRequestModel.PaymentRequestIndividualId = individualId;
    oPaymentRequestModel.PaymentRequestPaymentMethodId = $("#OtherPaymentsPaymentMethods").val();
    oPaymentRequestModel.PaymentRequestCurrencyId = $("#OtherPaymentsAccountingCurrency").val();

    oPaymentRequestModel.PaymentRequestTotalAmount = ClearFormatCurrency($("#OtherPaymentsTotal").val()).replace(",", ".");
    oPaymentRequestModel.PaymentRequestDescription = $("#OtherPaymentsDescription").val();
    oPaymentRequestModel.PaymentRequestPaymentMovementTypeId = $("#OtherPaymentsMovementType").val();


    oPaymentRequestModel.PaymentRequestAccountBankId = 0;
    oPaymentRequestModel.PaymentRequestType = $("#OtherPaymentsMovementType").val();
    oPaymentRequestModel.PaymentRequestCompanyId = $("#OtherPaymentsAccountingCompany").val();
    oPaymentRequestModel.PaymentRequestSalePointId = $("#OtherPaymentsSalesPoint").val();

    if (accountBankId == -1) {
        oPaymentRequestModel.PaymentRequestAccountBankId = 0;
    }
    else {
        oPaymentRequestModel.PaymentRequestAccountBankId = accountBankId;
    }

    oPaymentRequestModel.PaymentTax = oTax.TaxCategory;

    oPaymentClaimModel.PaymentClaimId = $("#OtherPaymentsPayTo").val();

    /*Solo si no hay registros ingresado se llena el objecto principal*/
    var taxListview = $("#taxesListView").UifListView("getData");
    if (taxListview.length == 0) {
        oPaymentClaimCoveragesModel.PaymentClaimCoveragesId = 0;
        oPaymentClaimCoveragesModel.PaymentClaimCoveragesSubClaim = 0;

        oPaymentClaimCoveragesClaimAmountModel.PaymentClaimCoveragesClaimAmountId = 0;
        oPaymentClaimCoveragesClaimAmountModel.PaymentClaimCoveragesClaimAmountEstimationTypeId = 0;
        oPaymentClaimCoveragesClaimAmountModel.PaymentClaimCoveragesClaimAmountVersion = 0;
        oPaymentClaimCoveragesClaimAmountModel.PaymentClaimCoveragesClaimAmountDate = null;
        oPaymentClaimCoveragesClaimAmountModel.PaymentClaimCoveragesClaimAmountEstimatedAmount = 0;
        oPaymentClaimCoveragesClaimAmountModel.PaymentClaimCoveragesClaimAmountDeductibleAmount = 0;
        oPaymentClaimCoveragesClaimAmountModel.PaymentClaimCoveragesClaimAmountPaymentTypeId = 0;
        oPaymentClaimCoveragesModel.PaymentClaimCoveragesClaimAmount.push(oPaymentClaimCoveragesClaimAmountModel);

        oPaymentClaimModel.PaymentClaimCoverages.push(oPaymentClaimCoveragesModel);
        oPaymentRequestModel.PaymentClaim.push(oPaymentClaimModel);
    }
}

///////////////////////////////////////////////////
//  Arma Objeto   oPaymentRequestModel          //
///////////////////////////////////////////////////
function SetDataVoucher(rowModel) {
    oVoucherModel.Index = rowModel.Index;
    oVoucherModel.VoucherId = rowModel.Index;
    oVoucherModel.VoucherType = rowModel.VoucherType;
    oVoucherModel.VoucherNumber = rowModel.VoucherNumber;
    oVoucherModel.VoucherDate = rowModel.Date;
    oVoucherModel.VoucherExchangeRate = rowModel.PaymentConcept.replace(",", ".");
    oVoucherModel.VoucherCurrencyId = rowModel.CurrencyId;

    taxHeadertModel.Index = rowModel.Index;

    var paymentConceptsIds = $("#PaymentConcetpsListView").UifListView("getData");

    if (paymentConceptsIds.length > 0) {
        if (editIndex != -1) {
            oPaymentClaimCoveragesClaimAmountModel.PaymentClaimCoveragesClaimAmountVoucher.splice(editIndex, 1);
            taxtModel.Concept.splice(editIndex, 1);
        }

        for (var i in paymentConceptsIds) {

            oVoucherConceptModel = {
                VoucherConceptId: 0,
                VoucherConceptPaymentConcept: 0,
                VoucherConceptValue: 0,
                VoucherConceptTaxValue: 0,
                VoucherConceptDescription: "",
                VoucherConceptRetentionValue: 0,
                VoucherConceptCostCenterId: 0
            };


            oVoucherConceptModel.VoucherConceptId = 0,
                oVoucherConceptModel.VoucherConceptPaymentConcept = paymentConceptsIds[i].PaymentConceptId;
            oVoucherConceptModel.VoucherConceptValue = paymentConceptsIds[i].Amount.replace(",", ".");
            oVoucherConceptModel.VoucherConceptTaxValue = paymentConceptsIds[i].Taxes.toString().replace(",", ".");
            oVoucherConceptModel.VoucherConceptDescription = paymentConceptsIds[i].PaymentConceptDescription;
            oVoucherConceptModel.VoucherConceptRetentionValue = paymentConceptsIds[i].Retentions.toString().replace(",", ".");
            oVoucherConceptModel.VoucherConceptCostCenterId = paymentConceptsIds[i].CostCenterId;

            oVoucherModel.VoucherConcept.push(oVoucherConceptModel);
        }
        taxtModel.Concept.push(taxHeadertModel);
        taxHeadertModel = {
            Index: 0,
            TaxConcept: []
        };
    }

    oPaymentClaimCoveragesClaimAmountModel.PaymentClaimCoveragesClaimAmountVoucher.push(oVoucherModel);

    return oPaymentRequestModel;
}

//Valida totales
function validTotal(totComp, totTax, totToPay) {
    var totGen = parseFloat(totComp + totTax).toFixed(2);
    return (totGen == totToPay) ? true : false;
}

//Valida transferencias
function validTransferSelected() {
    return (accountBankId == -1) ? false : true;
}

///////////////////////////////////////////////////
//  Guarda solicitud de Pagos Varios.            //
///////////////////////////////////////////////////
function saveOtherPaymentRequest() {

    $.ajax({
        type: "POST",
        url: ACC_ROOT + "OtherPaymentsRequest/SaveOtherPaymentsRequest",
        data: JSON.stringify({ "paymentRequestModel": oPaymentRequestModel }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {

            //EXCEPCION ROLLBACK
            if (data.success == false) {

                $("#alertMainOtherPayment").UifAlert('show', data.result, "danger");
                ClearAllOtherPayments();

            } else {
                if (data.PaymentRequestId != 0) {
                    $("#otherPaymentsRequestId").text(data.PaymentRequestNumber);
                    if (data.IsEnabledGeneralLedger == false) {
                        $("#OtherPaymentRequestAccountingMessageDiv").hide();
                    } else {
                        $("#OtherPaymentRequestAccountingMessageDiv").show();
                    }

                    $("#OtherPaymentRequestAccountingMessage").text(data.SaveDailyEntryMessage);
                    PaymentRequestId = data.PaymentRequestId;
                    $('#ModalSuccessPrint').UifModal('showLocal', Resources.ReceiptSaveSuccess);
                }
                else {
                    $("#alertMainOtherPayment").UifAlert('show', Resources.PaymentRequestNumberNotParameterized, 'warninig');
                }
            }
            $.unblockUI();
        }
    });//fin de ajax
}

///////////////////////////////////////////////////
//  Limpia Objeto   oVoucherModel               //
//////////////////////////////////////////////////
function CleanDataVoucherEmpty() {
    oVoucherModel = {
        VoucherId: 0,
        VoucherType: 0,
        VoucherNumber: 0,
        VoucherDate: null,
        VoucherExchangeRate: 0,
        VoucherCurrencyId: 0,
        VoucherConcept: []
    };

    return oVoucherModel;
}


///////////////////////////////////////////////////
//  Limpia Objeto   oVoucherModel               //
//////////////////////////////////////////////////
function ValidateDateAfter(dateTo) {
    //Fecha hasta
    var dateToDate = new Date();
    var dateToDay = dateTo.substr(0, 2);
    var dateToMonth = dateTo.substr(3, 2);
    var dateToYear = dateTo.substr(6, 4);

    dateToDate.setMonth(dateToMonth - 1);
    dateToDate.setDate(dateToDay);
    dateToDate.setYear(dateToYear);

    //Fecha Hoy
    var dateNow = new Date();
    var dateSystem = GetCurrentDate();
    var dayNow = dateSystem.substr(0, 2);
    var monthNow = dateSystem.substr(3, 2);
    var yearNow = dateSystem.substr(6, 4);

    dateNow.setMonth(monthNow - 1);
    dateNow.setDate(dayNow);
    dateNow.setYear(yearNow);

    if (dateToDate >= dateNow) {
        return true;
    }
    else {
        return false;
    }
}


///////////////////////////////////////////////////
//  Borra registro de ListView genérico         //
//////////////////////////////////////////////////
function delRowOtherPayments(listviewName) {
    var rowsPaymentConcepts = $("#PaymentConcetpsListView").UifListView("getData");
    if (rowsPaymentConcepts.length == 0) {
        $("#PaymentConcepts").find("#VoucherNumber").removeAttr("disabled");
        $("#PaymentConcepts").find("#CheckDate").removeAttr("disabled");
        $("#PaymentConcepts").find("#VoucherType").removeAttr("disabled");
        $("#PaymentConcepts").find("#ExchangeRate").removeAttr("disabled");
        $("#PaymentConcepts").find("#VoucherCurrency").removeAttr("disabled");
        $("#PaymentConcepts").find("#VoucherCurrencyPaymentConcept").removeAttr("disabled");
    }
    TotalPaymentAmount = FormatCurrency(FormatDecimal(totalAmountOtherPayments(listviewName)))
    TotalPaymentTaxes = FormatCurrency(FormatDecimal(totalTax(listviewName)))
    TotalPaymentRetention = FormatCurrency(FormatDecimal(totalRetention(listviewName)))

    if (listviewName == "taxesListView") {
        $('#VouchersTotal').text(TotalPaymentAmount);
        $('#VoucherTaxesTotal').text(TotalPaymentTaxes);
        $('#VoucherRetentionsTotal').text(TotalPaymentRetention);
    }
    else {
        $("#PaymentConcepts").find('#TotalPaymentAmount').text(TotalPaymentAmount);
        $("#PaymentConcepts").find('#TotalPaymentAmount').val(TotalPaymentAmount);

        $("#PaymentConcepts").find('#TotalPaymentTaxes').text(TotalPaymentTaxes);
        $("#PaymentConcepts").find('#TotalPaymentTaxes').val(TotalPaymentTaxes);

        $("#PaymentConcepts").find('#TotalPaymentRetention').text(TotalPaymentRetention);
        $("#PaymentConcepts").find('#TotalPaymentRetention').val(TotalPaymentRetention);
    }

}

//Carga reporte
function loadOtherPaymentsRequestReport(paymentRequestId) {
    var controller = ACC_ROOT + "Report/LoadOtherPaymentsRequestReport?paymentRequestId=" + paymentRequestId;
    window.open(controller, 'mywindow', 'fullscreen=yes, scrollbars=auto');
}

///////////////////////////////////////////////////
//  Limpia campos y variables                   //
//////////////////////////////////////////////////
function ClearAllOtherPayments() {


    index = 0;
    indexConcept = 0;
    $("#OtherPaymentsMovementType").val("");
    $("#OtherPaymentsBranch").val("");
    $("#OtherPaymentsSalesPoint").val("");
    $("#OtherPaymentsPayTo").val("");
    $("#OtherPaymentsDocumentNumber").val("");
    $("#OtherPaymentsFirstLastName").val("");
    $("#OtherPaymentsPaymentMethods").val(-1);
    $("#OtherPaymentsAccountingCurrency").val(-1);
    $("#OtherPaymentsTotal").val("");
    $("#OtherPaymentsEstimatedPaymentDate").val("");
    $("#OtherPaymentsDescription").val("");

    cleanAutocompletesOtherPayment('SearchSuppliers');
    cleanAutocompletesOtherPayment('SearchInsured');
    cleanAutocompletesOtherPayment('SearchCoinsurance');
    cleanAutocompletesOtherPayment('SearchPerson');
    cleanAutocompletesOtherPayment('SearchAgent');
    cleanAutocompletesOtherPayment('SearchEmployee');
    cleanAutocompletesOtherPayment('SearchReinsurer');

    $("#BeneficiaryDocumentNumberData").show();
    $("#BeneficiaryNameData").show();

    $("#VouchersTotal").text("");
    $("#TotalTaxes").text("");
    $("#TotalRetention").text("");

    $("#taxesListView").UifListView({
        height: 200, //autoHeight: true,
        customAdd: true, customEdit: false, customDelete: false, localMode: true, add: true, edit: false, delete: true,
        displayTemplate: "#check-display",        
        deleteCallback: deletetaxesListView
    });

    //dialogo de conceptos de pago
    paymentConceptId = "";


    setTimeout(function () {
        $("#VouchersTotal").val("");
        $("#VoucherTaxesTotal").val("");
        $("#VoucherRetentionsTotal").val("");
    }, 2000);


    IndividualId = 0;
    ExchangeRate = 0;
    //************************
    taxtModel = {
        Concept: []
    };
    taxHeadertModel = {
        Index: 0,
        TaxConcept: []
    };

    taxDetailtModel = {
        TaxDescription: "",
        TaxGroup: "",
        Amount: 0,
        Rate: 0,
        TaxAmount: 0,
        MinimumTax: 0,
    };
    //*********************

    clearModel();

    //Variable PaymentTaxCategory
    oPaymentTaxCategoryModel = {
        TaxId: 0,
        TaxCategoryId: 0,
        TaxCondition: 0,
        TaxCategoryDescript: ""
    };
}

function clearModel() {
    //Variable PaymentClaimCoverages
    oPaymentClaimCoveragesModel = {
        PaymentClaimCoveragesId: 0,
        PaymentClaimCoveragesSubClaim: 0,
        PaymentClaimCoveragesClaimAmount: []
    };
    //Variable PaymentClaim.
    oPaymentClaimModel = {
        PaymentClaimId: 0,
        PaymentClaimCoverages: []
    };

    //Variable para las Estimaciones
    oPaymentClaimCoveragesClaimAmountModel = {
        PaymentClaimCoveragesClaimAmountId: 0,
        PaymentClaimCoveragesClaimAmountEstimationTypeId: 0,
        PaymentClaimCoveragesClaimAmountVersion: 0,
        PaymentClaimCoveragesClaimAmountDate: null,
        PaymentClaimCoveragesClaimAmountEstimatedAmount: 0,
        PaymentClaimCoveragesClaimAmountDeductibleAmount: 0,
        PaymentClaimCoveragesClaimAmountPaymentTypeId: 0,
        PaymentClaimCoveragesClaimAmountVoucher: []
    };
    //Variable Voucher
    oVoucherModel = {
        VoucherId: 0,
        VoucherType: 0,
        VoucherNumber: 0,
        VoucherDate: null,
        VoucherExchangeRate: 0,
        VoucherCurrencyId: 0,
        VoucherConcept: []
    };
    //Variable VoucherConcept
    oVoucherConceptModel = {
        VoucherConceptId: 0,
        VoucherConceptPaymentConcept: 0,
        VoucherConceptValue: 0,
        VoucherConceptTaxValue: 0,
        VoucherConceptDescription: "",
        VoucherConceptRetentionValue: 0,
        VoucherConceptCostCenterId: 0
    };
    oPaymentRequestModel = {
        PaymentRequestInfoId: 0,
        PaymentRequestSource: 0,
        PaymentRequestBranch: 0,
        PaymentRequestEstimatedDate: null,
        PaymentRequestRegistrationDate: null,
        PaymentRequestPaymentDate: null,
        PaymentRequestPersonTypeId: 0,
        PaymentRequestIndividualId: 0,
        PaymentRequestPaymentMethodId: 0,
        PaymentRequestCurrencyId: 0,
        PaymentRequestUserId: 0,
        PaymentRequestIsPrinted: false,
        PaymentRequestTotalAmount: 0,
        PaymentRequestDescription: "",
        PaymentRequestPaymentMovementTypeId: 0,
        PaymentRequestPrefixId: 0,
        PaymentRequestAccountBankId: 0,
        PaymentRequestType: 0,
        PaymentClaim: [],
        PaymentTax: [],
        PaymentRequestCompanyId: 0,
        PaymentRequestSalePointId: 0
    };
}
/////////////////////////////////////////
// Limpia Autocomplete Abonador        //
/////////////////////////////////////////
function cleanAutocompletesOtherPayment(identifier) {

    $('#' + identifier + 'ByDocumentNumberRequest').val("");
    $('#' + identifier + 'ByNameRequest').val("");

    journalPayerIndividualId = 0;

    /*Vacía el Objeto oTax
    ----------------------*/
    oTax = { TaxCategory: [] };
    oPaymentRequestModel.PaymentTax = [];
    oTaxCategory = {};
    /*---------------------------*/

    $('#' + identifier + 'ByDocumentNumberRequest').parent().parent().hide();
    $('#' + identifier + 'ByNameRequest').parent().parent().hide();

}

/////////////////////////////////////////
// Carga Autocomplete Abonador         //
/////////////////////////////////////////
function loadAutocompleteEventOtherPayment(identifier) {
    $('#' + identifier + 'ByDocumentNumberRequest').on('itemSelected', function (event, selectedItem) {
        fillAutocompletesOtherPayments(identifier, selectedItem);
    });

    $('#' + identifier + 'ByNameRequest').on('itemSelected', function (event, selectedItem) {
        fillAutocompletesOtherPayments(identifier, selectedItem);
    });

}

///////////////////////////////////////////////////
//  Muestra mensaje de confirmación              //
///////////////////////////////////////////////////
function showConfirmOtherPayments(id, identifier, individualName, documentNumber, value) {
    chagePayment = false;
    $.UifDialog('confirm', { 'message': Resources.ConfirmFilterMessage + '?', 'title': '' }, function (result) {
        if (result) {
            ClearAllOtherPayments();
            chagePayment = true;
        }
        else {
            if (id == 1 && value > -1) {
                $("#CreditCardType").val(value);
            }
            if (id == 2) {
                $('#' + identifier + 'ByDocumentNumber').val(documentNumber);
                $('#' + identifier + 'ByName').val(individualName);
            }
            chagePayment = false;
        }
    });
}

////////////////////////////////////////////////
// Eventos al seleccionar un  Abonador        //
///////////////////////////////////////////////
function fillAutocompletesOtherPayments(identifier, selectedItem) {
    changePayment = true;
    if (changePayment) {
        $('#' + identifier + 'ByDocumentNumberRequest').val(selectedItem.DocumentNumber);
        $('#' + identifier + 'ByNameRequest').val(selectedItem.Name);

        if (selectedItem.Id != undefined && selectedItem.Id != -1) {

            if (selectedItem.IndividualId == undefined) {

                individualId = selectedItem.Id;
            } else {
                individualId = selectedItem.IndividualId;
            }

            individualName = selectedItem.Name;
            documentNumber = selectedItem.DocumentNumber;

            if (selectedItem.InsuredCode != undefined && selectedItem.InsuredCode != -1) {
                personCode = selectedItem.InsuredCode;
            }
            if (selectedItem.SupplierCode != undefined && selectedItem.SupplierCode != -1) {
                personCode = selectedItem.SupplierCode;
            }

        }
        else if (selectedItem.AgentId != undefined && selectedItem.AgentId != -1) {
            individualId = selectedItem.IndividualId;
            agentTypeId = selectedItem.AgentTypeId;
            personCode = selectedItem.AgentId;
        } else {
            individualId = selectedItem.CoinsuranceIndividualId;
        }

        if (selectedItem.Id != undefined && selectedItem.Id != -1) {
            //Impuestos por categoría
            OpenTaxSelectionOtherPayments(individualId);
        }
    }
}

///////////////////////////////////////////////////
//  Carga uiView Impuestos del Abonador          //
///////////////////////////////////////////////////
function LoadTaxCondition(individualId) {

    $("#ModalTaxCondition").find("#TaxConditionListView").UifListView(
        {
            source: ACC_ROOT + "OtherPaymentsRequest/GetIndividualTaxByIndividualId?individualId=" + individualId,
            customDelete: false,
            customAdd: false,
            customEdit: true,
            edit: false,
            displayTemplate: "#taxCondicional-display"
        });

}

//presenta diálogo de Impuestos
function OpenTaxSelectionOtherPayments(individualId) {

    if (individualId != undefined && individualId > 0) {

        var controller = ACC_ROOT + "OtherPaymentsRequest/GetIndividualTaxCategoryCondition?individualId=" + individualId;
        $("#modalTaxSelection").find("#TaxSelectionTable").UifDataTable({ source: controller });

        setTimeout(function () {
            ControlCheckOtherPayments();
            showModalTaxSelection();
        }, 1000);
    }
}

//presenta diálogo de Impuestos
function showModalTaxSelection() {
    $("#alertTaxSelection").UifAlert('hide');
    var count = $("#modalTaxSelection").find("#TaxSelectionTable").UifDataTable('getData');
    if (count.length > 0) {
        $('#modalTaxSelection').UifModal('showLocal', Resources.Taxes);
    }else{
        //Muestra la información de pago electronico.
        ShowAccountInfo(individualId);
    }
}

///////////////////////////////////////////////////////////
//  Muestra modal de selección de categorias de impuestos //
///////////////////////////////////////////////////////////
function ControlCheckOtherPayments() {
    /*Vacía el Objeto oTax
    ----------------------*/
    oTax = { TaxCategory: [] };
    oPaymentRequestModel.PaymentTax = [];
    oTaxCategory = {};
    /*---------------------------*/

    idsTax = $("#modalTaxSelection").find("#TaxSelectionTable").UifDataTable("getSelected");

    /**************************************/

    for (var i = 0; i < $("#modalTaxSelection").find("#TaxSelectionTable").UifDataTable('getData').length; i++) {
        var row = $("#modalTaxSelection").find("#TaxSelectionTable").UifDataTable('getData')[i];
        oTaxCategory = {
            TaxId: 0,
            TaxCategoryId: 0,
            TaxCategoryDescript: "",
            TaxCondition: 0
        };
        oTaxCategory.TaxId = row.TaxCode;
        oTaxCategory.TaxCondition = row.TaxConditionCode;
        if (row.TaxCategoryCode == null) {
            oTaxCategory.TaxCategoryId = "";
        } else {
            oTaxCategory.TaxCategoryId = row.TaxCategoryCode;
        }
        if (row.TaxCategotyDescription == null) {
            oTaxCategory.TaxCategoryDescript = "";
        } else {
            oTaxCategory.TaxCategoryDescript = row.TaxCategotyDescription;
        }
        oTax.TaxCategory.push(oTaxCategory);
    }

    /**************************************/

    if (idsTax != null) {
        $("#ModalTaxes").find("#taxesView").UifListView({
            autoHeight: true,
            customAdd: false, customEdit: false, customDelete: false, localMode: true, add: false, edit: false, delete: false,
            displayTemplate: "#taxes-display"
        });
        //Arma el Objeto
        for (var i in idsTax) {
            var row = idsTax[i];
            rowTaxes = new RowTaxes();
            rowTaxes.TaxId = 0;
            rowTaxes.TaxDescription = row.TaxDescription;
            rowTaxes.GroupCondition = row.TaxConditionDescription;
            rowTaxes.GroupAmount = 0;
            rowTaxes.Rate = 0;
            rowTaxes.TaxAmount = 0;
            rowTaxes.MinimunTaxBase = 0;
            rowTaxes.MinimunTax = 0;
            rowTaxes.Branch = "";
            $('#taxesView').UifListView("addItem", rowTaxes);
        }
    }
    $('#modalTaxSelection').modal('hide');

    //Muestra la información de pago electronico.
    ShowAccountInfo(individualId);
};

function GetSalePoints() {
    if ($('#OtherPaymentsBranch').val() != "") {
        var controller = ACC_ROOT + "Common/GetSalesPointByBranchId?branchId=" + $('#OtherPaymentsBranch').val();
        $("#OtherPaymentsSalesPoint").UifSelect({ source: controller });

        //Setea el punto de venta de default
        setTimeout(function () {
            $("#OtherPaymentsSalesPoint").val($("#ViewBagSalePointBranchUserDefault").val());
        }, 500);
    }
};

//Muestra la información de pago electronico.
function ShowAccountInfo(individualId) {
    var accountInfoPromise = new Promise(function (resolve, reject) {
        if (individualId != undefined && individualId > 0) {
            $.ajax({
                type: "POST",
                url: ACC_ROOT + "OtherPaymentsRequest/GetAccountBankInformationByIndividualId",
                data: JSON.stringify({
                    "individualId": individualId
                }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (accountInfo) {
                    resolve(accountInfo);
                }
            });
        }
    });

    accountInfoPromise.then(function (accountInfo) {
        if (accountInfo.length > 0) {
            $("#AccountBankInfoModal").find("#AccountType").val(accountInfo[0].BankAccountType.Description);
            $("#AccountBankInfoModal").find("#AccountNumber").val(accountInfo[0].Number);
            $("#AccountBankInfoModal").find("#BankDescription").val(accountInfo[0].Bank.Description);

            $('#AccountBankInfoModal').UifModal('showLocal', Resources.PaymentMeans);

            //Carga la información del tipo de pago.
            $("#OtherPaymentsAccountingCurrency").val(accountInfo[0].Currency.Id);
            $("#OtherPaymentsDescription").val("Dummy Voucher 123456, 654321");

            //Indica que los datos fueron cargados del pago electrónico
            $("#alertMainOtherPayment").UifAlert('show', Resources.DataLoadedFromElectronicPayment, "warning");

            var controller = ACC_ROOT + "OtherPaymentsRequest/GetEnabledPaymentMethodTypes";
            $("#OtherPaymentsPaymentMethods").UifSelect({ source: controller });

            CheckPaymentMethodsLoaded();
            paymentMethodsPromise.then(function (isLoaded) {
                if (isLoaded) {
                    clearTimeout(paymentMethodsTime);
                    $("#OtherPaymentsPaymentMethods").val($("#ViewBagParamPaymentMethodElectronicPayment").val());
                }
            });

            //pendiente guardar la información del tipo de pago.


        } else {
            //muestra mensaje de que no existe información bancaria.
            $("#alertMainOtherPayment").UifAlert('show', Resources.BeneficiaryNotHavingAccountForElectronicPayment, "warning");

            //deshabilita la opción de "Pago electrónico"
            var newController = ACC_ROOT + "OtherPaymentsRequest/ExcludeElectronicPaymentFromEnablePaymentMethods";
            $("#OtherPaymentsPaymentMethods").UifSelect({ source: newController });
        }
    });
}

/*-------------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*                                                      DEFINICIÓN DE ACCIONES                                                                                */
/*-------------------------------------------------------------------------------------------------------------------------------------------------------------*/
conceptCode = $("#PaymentConcepts").find("#PaymentConceptId").val();
accountingAccountName = $("#PaymentConcepts").find("#PaymentConceptDescription").val();

/*********************************************************************/

/////////////////////////////////////////
//  Da formato de número               //
/////////////////////////////////////////
$("#PaymentConcepts").find('#ConceptAmount').blur(function () {
    var conceptAmount = $("#PaymentConcepts").find("#ConceptAmount").val();
    $("#PaymentConcepts").find("#ConceptAmount").val("$ " + NumberFormatDecimal(conceptAmount, "2", ".", ","));
});

//CancelTicket
$("#CancelTicket").click(function () {
    ClearAllOtherPayments();
});



/////////////////////////////////////////
//  Carga concepto de pago en listView //
/////////////////////////////////////////
$("#SaveTicketOtherPayments").click(function () {

    if (ClearFormatCurrency($("#VouchersTotal").val()) != 0) {

        var resp = validTotal(parseFloat(ClearFormatCurrency($("#VouchersTotal").val().replace("", ","))),
            parseFloat(ClearFormatCurrency($("#VoucherTaxesTotal").val().replace("", ","))),
            parseFloat(ClearFormatCurrency($("#OtherPaymentsTotal").val().replace("", ","))));

        if (resp) {
            setHeaderPayment();
            if (parseInt($("#OtherPaymentsPaymentMethods").val()) == $("#ViewBagParamPaymentMethodTransferMainOther").val()) {
                if (validTransferSelected()) {
                    SetDataPaymentRequest();
                    saveOtherPaymentRequest();
                    lockScreen();
                } else {
                    $("#alertMainOtherPayment").UifAlert('show', Resources.SelectAccountainOtherPayments, "warning");
                }
            } else {
                SetDataPaymentRequest();
                saveOtherPaymentRequest();
            }
        } else {
            $("#alertMainOtherPayment").UifAlert('show', Resources.ValidTotMainOtherPaymentsRequest, "warning");
            unlockScreen();
        }
    } else {
        $("#alertMainOtherPayment").UifAlert('show', Resources.TotalPayment, "warning");
    }
});

/////////////////////////////////////////
//   listViews                        //
/////////////////////////////////////////
$("#taxesListView").UifListView({
    height: 200, //autoHeight: true,
    customAdd: true, customEdit: false, customDelete: false, localMode: true, add: true, edit: false, delete: true,
    displayTemplate: "#check-display", deleteCallback: deletetaxesListView
});

$("#PaymentConcepts").find("#PaymentConcetpsListView").UifListView({
    autoHeight: true,
    customDelete: false,
    localMode: true,
    customEdit: false,
    edit: false,
    delete: true,
    displayTemplate: "#check-display-template",
    deleteCallback: deletePaymentConcept


});

$("#TransferInformationListView").UifListView({
    height: 50, //autoHeight: true,
    customAdd: false, customEdit: true, customDelete: false, localMode: true, add: true, edit: false, delete: false,
    displayTemplate: "#check-display-transfer"
});

$('#TransferInformationListView').on('rowEdit', function (event, selectedRow) {
    accountBankId = selectedRow.AccountBankId;
});

$('#taxesListView').on('rowAdd', function (event) {
    $("#alertForm").UifAlert('hide');

    editIndex = -1;
    indexConcept = 0;
    taxHeadertModel = {
        Index: 0,
        TaxConcept: []
    };

    $("#alertMainOtherPayment").UifAlert('hide');
    clearPaymentConceptsModal();
    $("#OtherPaymentsRequestFormulario").validate();

    if ($("#OtherPaymentsRequestFormulario").valid()) {
        if (enableForSave) {
            ids = $("#taxesListView").UifListView("getData");
            if (ids.length == 0) {
                clearModel();
                factCorrela = 1;
            }
            else {
                factCorrela = ids.length + 1;
            }

            VoucherType = $("#PaymentConcepts").find("#VoucherType option:selected").html();
            VoucherNumber = $("#PaymentConcepts").find("#VoucherNumber").val();
            VoucherCurrency = $("#PaymentConcepts").find("#VoucherCurrency option:selected").html();
            if (VoucherCurrency != "") {
                $("#PaymentConcepts").find("#VoucherCurrencyPayment").val(VoucherCurrency);
            }

            PaymentConceptTypeId = $("#OtherPaymentsMovementType").val();
            PaymentBranchId = $("#OtherPaymentsBranch").val();

            $("#PaymentConcepts").find("#PaymentConcetpsListView").UifListView({
                customAdd: false, customEdit: true, customDelete: false, localMode: true, add: false, edit: false, delete: true,
                displayTemplate: "#check-display-template", deleteCallback: deletePaymentConcept
            });

            delRowOtherPayments("PaymentConcetpsListView");

            $('#PaymentConcepts').UifModal('showLocal', Resources.PaymentConcept);

        } else {

            $("#alertMainOtherPayment").UifAlert('show', Resources.PersonTypeNotAuthorized, "warning");
        }
    }
});

$('.uif-panel').on("Add", function (event) {
    editIndex = -1;
    indexConcept = 0;
    taxHeadertModel = {
        Index: 0,
        TaxConcept: []
    };

    $("#alertMainOtherPayment").UifAlert('hide');
    clearPaymentConceptsModal();
    $("#OtherPaymentsRequestFormulario").validate();

    if ($("#OtherPaymentsRequestFormulario").valid()) {
        ids = $("#taxesListView").UifListView("getData");
        if (ids.length == 0) {
            factCorrela = 1;
        }
        else {
            factCorrela = ids.length + 1;
        }

        VoucherType = $("#PaymentConcepts").find("#VoucherType option:selected").html();
        VoucherNumber = $("#PaymentConcepts").find("#VoucherNumber").val();
        VoucherCurrency = $("#PaymentConcepts").find("#VoucherCurrency option:selected").html();
        if (VoucherCurrency != "") {
            $("#PaymentConcepts").find("#VoucherCurrencyPayment").val(VoucherCurrency);

        }
        PaymentConceptTypeId = $("#OtherPaymentsMovementType").val();
        PaymentBranchId = $("#OtherPaymentsBranch").val();

        $("#PaymentConcepts").find("#PaymentConcetpsListView").UifListView({
            customAdd: false, customEdit: true, customDelete: false, localMode: true, add: false, edit: false, delete: true,
            displayTemplate: "#check-display-template", deleteCallback: deletePaymentConcept

        });

        delRowOtherPayments("PaymentConcetpsListView");
        $('#PaymentConcepts').UifModal('showLocal', Resources.PaymentConcept);
    }
});

/////////////////////////////////////////
//  Edición de  listView               //
/////////////////////////////////////////
$("#taxesListView").on('rowEdit', function (event, data, index) {
    ids = $("#taxesListView").UifListView("getData");
    if (ids.length == 0) {
        SetDataPaymentRequest();
    }
    $("#PaymentConcepts").find("#VoucherType").val(data.VoucherType);
    $("#PaymentConcepts").find("#VoucherNumber").val(data.VoucherNumber);
    $("#PaymentConcepts").find("#CheckDate").val(data.Date);
    $("#PaymentConcepts").find("#VoucherCurrency").val(data.CurrencyId);
    $("#PaymentConcepts").find("#ExchangeRate").val(data.PaymentConcept);

    editIndex = index;
    $("#PaymentConcepts").find("#PaymentConcetpsListView").UifListView({
        autoHeight: true,
        customAdd: false, customEdit: false, customDelete: false, localMode: true, add: false, edit: false, delete: true,
        displayTemplate: "#check-display-template", deleteCallback: deletePaymentConcept
    });
    delRowOtherPayments("PaymentConcetpsListView");
    $('#PaymentConcepts').UifModal('showLocal', Resources.PaymentConcept);
});

/////////////////////////////////////////
//  Formato como Moneda
/////////////////////////////////////////
$("#OtherPaymentsTotal").blur(function () {
    var paymentsTotal = $.trim(ClearFormatCurrency($("#OtherPaymentsTotal").val()));
    if (paymentsTotal != "" && paymentsTotal != "$") {
        $("#OtherPaymentsTotal").val(FormatCurrency(paymentsTotal));
    } else {
        $("#OtherPaymentsTotal").val('');
    }
});


/////////////////////////////////////////
//  Dropdown métodos de pago          //
/////////////////////////////////////////
$('#OtherPaymentsPaymentMethods').on('itemSelected', function (event, selectedItem) {
    if (selectedItem.Id == $("#ViewBagParamPaymentMethodTransferMainOther").val()) {
        $("#TransferInformationListView").UifListView(
            {
                source: ACC_ROOT + "Common/GetAccountBanksByIndividualId?individualId=" + individualId,
                height: 50,
                customDelete: false,
                customAdd: false,
                customEdit: true,
                edit: true,
                displayTemplate: "#check-display-transfer"
            });
        $('#TransferPanel').show();
    }
    else {
        $('#TransferPanel').hide();
    }
    //se comprueba si es pago electrónico.
    if (selectedItem.Id == $("#ViewBagParamPaymentMethodElectronicPayment").val()) {
        if (individualId <= 0) {
            //mensaje de que no se a seleccionado un beneficiario
            $("#alertMainOtherPayment").UifAlert('show', Resources.InputBeneficiaryInformation, "warning");

        } else {
            //Muestra la información de pago electronico.
            ShowAccountInfo(individualId);
        }
    } else {
        $("#OtherPaymentsAccountingCurrency").val("");
        $("#OtherPaymentsDescription").val("");
        $("#alertMainOtherPayment").UifAlert("hide");
    }
});

/////////////////////////////////////////
//  Evento ocultar modal de impuestos  //
/////////////////////////////////////////
$('#modalTaxSelection').on('hidden.bs.modal', function () {
    //Muestra la información de pago electronico.
    ShowAccountInfo(individualId);
})

function GetSelectOtherPaymentsPaymentOrigin() {
    if ($("#ViewBagImputationType").val() == undefined &&
        $("#ViewBagParameterMulticompanyPayment").val() == undefined &&
        $("#ViewBagBillControlId").val() == undefined) {

        $.ajax({
            type: "POST",
            url: ACC_ROOT + "Common/GetPaymentSources",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data != null) {
                    $.each(data, function (i, value) {
                        for (var i = 0; i < value.length; i++) {
                            if (value[i].Id == paymentSourceOthers) {
                                $(OtherPaymentsPaymentOrigin).append('<option value=' + value[i].Id + '>' + value[i].Description + '</option>');
                            }
                        }
                    });
                    $(OtherPaymentsPaymentOrigin).trigger('change');
                }
            }
        });
    }

}


/////////////////////////////////////////
//  Dropdown Origen de pago          //
/////////////////////////////////////////
$('#OtherPaymentsPaymentOrigin').on('itemSelected', function (event, selectedItem) {
    if (selectedItem.Id > 0) {
        var controller = ACC_ROOT + "OtherPaymentsRequest/GetPaymentMovementTypesByPaymentSourceId?paymentSourceId=" + selectedItem.Id;
        $("#OtherPaymentsMovementType").UifSelect({ source: controller });
    }
});


/////////////////////////////////////////
//  Dropdown Sucursal de pago          //
/////////////////////////////////////////
$('#OtherPaymentsBranch').on('itemSelected', function (event, selectedItem) {
    if (selectedItem.Id > 0) {
        var controller = ACC_ROOT + "Common/GetSalesPointByBranchId?branchId=" + selectedItem.Id;
        $("#OtherPaymentsSalesPoint").UifSelect({ source: controller });
    }
});

setTimeout(function () {
    $("#OtherPaymentsPaymentOrigin").val(paymentSourceOthers);
    $("#OtherPaymentsPaymentOrigin").attr("disabled", "disabled");
    $("#OtherPaymentsPaymentOrigin").trigger('change');
}, 800);


/////////////////////////////////////////
//  Botón  Condición Impositiva        //
/////////////////////////////////////////
$("#TaxCondition").click(function () {
    TaxConditionTitle = Resources.TaxCondition + ':  ' + ' ' + individualName;
    LoadTaxCondition(individualId)
    $('#ModalTaxCondition').find('.ptitle').html(TaxConditionTitle);
    $('#ModalTaxCondition').UifModal('showLocal', TaxConditionTitle);
});


/////////////////////////////////////////
//  Dropdown Pagar a:                  //
/////////////////////////////////////////
$('#OtherPaymentsPayTo').on('itemSelected', function (event, selectedItem) {
    individualId = 0;
    individualName = "";
    chagePayment = true;
    var enableAjax = 0;

    $("#alertMainOtherPayment").UifAlert('hide');

    var lisTaxView = $("#taxesListView").UifListView("getData");
    if (lisTaxView.length > 0) {

        showConfirmOtherPayments(1, "", "", "", payToValue);
    }

    if (chagePayment) {
        payToValue = selectedItem.Id;
        cleanAutocompletesOtherPayment('SearchSuppliers');
        cleanAutocompletesOtherPayment('SearchInsured');
        cleanAutocompletesOtherPayment('SearchCoinsurance');
        cleanAutocompletesOtherPayment('SearchPerson');
        cleanAutocompletesOtherPayment('SearchAgent');
        cleanAutocompletesOtherPayment('SearchEmployee');
        cleanAutocompletesOtherPayment('SearchReinsurer');

        if (selectedItem.Id != "") {
            $("#BeneficiaryDocumentNumberData").hide();
            $("#BeneficiaryNameData").hide();

            if (selectedItem.Id == $("#ViewBagSupplierCodeMainOther").val()) { // 1 Proveedor
                $('#SearchSuppliersByDocumentNumberRequest').parent().parent().show();
                $("#SearchSuppliersByNameRequest").parent().parent().show();
                enableForSave = true;

            } else if (selectedItem.Id == $("#ViewBagProducerCodeMainOther").val()) { // 10 Productor  // agente 1 en BE
                $("#SearchAgentByDocumentNumberRequest").parent().parent().show();
                $("#SearchAgentByNameRequest").parent().parent().show();
                enableForSave = true;

            } else if (selectedItem.Id == $("#ViewBagThirdPartyCodeMainOther").val()) { //tercero - //TODO: Por el momento queda pendiente la implementación.
                $("#SearchPersonByDocumentNumberRequest").parent().parent().show();
                $("#SearchPersonByNameRequest").parent().parent().show();
                enableForSave = true;

            } else if (selectedItem.Id == $("#ViewBagAgentCodeMainOther").val()) { //agente
                $("#SearchAgentByDocumentNumberRequest").parent().parent().show();
                $("#SearchAgentByNameRequest").parent().parent().show();
                enableForSave = true;

            } else if (selectedItem.Id == $("#ViewBagEmployeeCodMainOther").val()) { // 11 Empleado  // 11 en BE
                $("#SearchEmployeeByDocumentNumberRequest").parent().parent().show();
                $("#SearchEmployeeByNameRequest").parent().parent().show();
                enableAjax = 1;
                enableForSave = true;

            } else if (selectedItem.Id == '') {
                cleanAutocompletesOtherPayment('SearchSuppliers');
                cleanAutocompletesOtherPayment('SearchInsured');
                cleanAutocompletesOtherPayment('SearchCoinsurance');
                cleanAutocompletesOtherPayment('SearchPerson');
                cleanAutocompletesOtherPayment('SearchAgent');
                cleanAutocompletesOtherPayment('SearchEmployee');
                cleanAutocompletesOtherPayment('SearchReinsurer');

                enableForSave = false;

            } else {
                $("#BeneficiaryDocumentNumberData").show();
                $("#BeneficiaryNameData").show();

                cleanAutocompletesOtherPayment('SearchSuppliers');
                cleanAutocompletesOtherPayment('SearchInsured');
                cleanAutocompletesOtherPayment('SearchCoinsurance');
                cleanAutocompletesOtherPayment('SearchPerson');
                cleanAutocompletesOtherPayment('SearchAgent');
                cleanAutocompletesOtherPayment('SearchEmployee');
                cleanAutocompletesOtherPayment('SearchReinsurer');
                enableForSave = false;
            }

            if (enableAjax > 0) {
            }
        } else {
            $("#BeneficiaryDocumentNumberData").parent().parent().show();
            $("#BeneficiaryNameData").parent().parent().show();
            cleanAutocompletesOtherPayment('SearchSuppliers');
            cleanAutocompletesOtherPayment('SearchInsured');
            cleanAutocompletesOtherPayment('SearchCoinsurance');
            cleanAutocompletesOtherPayment('SearchPerson');
            cleanAutocompletesOtherPayment('SearchAgent');
            cleanAutocompletesOtherPayment('SearchEmployee');
            cleanAutocompletesOtherPayment('SearchReinsurer');

            enableForSave = false;

        }
    }
});

/////////////////////////////////////////
//  Botón  Ver impuestos                //
/////////////////////////////////////////
$("#ShowTaxes").click(function () {

    var id1 = "";
    var TotalTaxConcept = [];
    if (taxtModel.Concept.length > 0) {
        for (var i = 0; i < taxtModel.Concept.length; i++) {
            for (var j = 0; j < taxtModel.Concept[i].TaxConcept.length; j++) {
                taxDetailtModel = {
                    TaxDescription: "",
                    TaxGroup: "",
                    Amount: 0,
                    Rate: 0,
                    TaxAmount: 0,
                    MinimumTax: 0,
                };
                id1 = taxtModel.Concept[i].TaxConcept[j].TaxDescription;
                if (TotalTaxConcept.length > 0) {
                    for (var f = 0; f < TotalTaxConcept.length; f++) {
                        if (id1 == TotalTaxConcept[f].TaxDescription) {
                            TotalTaxConcept[f].Amount = TotalTaxConcept[f].Amount + taxtModel.Concept[i].TaxConcept[j].Amount;
                            TotalTaxConcept[f].TaxAmount = TotalTaxConcept[f].TaxAmount + taxtModel.Concept[i].TaxConcept[j].TaxAmount;
                            break;
                        }
                        else {
                            if (f == TotalTaxConcept.length - 1) {
                                taxDetailtModel.TaxDescription = taxtModel.Concept[i].TaxConcept[j].TaxDescription;
                                taxDetailtModel.TaxGroup = taxtModel.Concept[i].TaxConcept[j].TaxGroup
                                taxDetailtModel.Amount = taxtModel.Concept[i].TaxConcept[j].Amount
                                taxDetailtModel.Rate = taxtModel.Concept[i].TaxConcept[j].Rate;
                                taxDetailtModel.TaxAmount = taxtModel.Concept[i].TaxConcept[j].TaxAmount;
                                taxDetailtModel.MinimumTax = taxtModel.Concept[i].TaxConcept[j].MinimumTax;

                                TotalTaxConcept.push(taxDetailtModel)
                                break;
                            }
                        }
                    }
                }
                else {

                    taxDetailtModel.TaxDescription = taxtModel.Concept[i].TaxConcept[j].TaxDescription;
                    taxDetailtModel.TaxGroup = taxtModel.Concept[i].TaxConcept[j].TaxGroup
                    taxDetailtModel.Amount = taxtModel.Concept[i].TaxConcept[j].Amount
                    taxDetailtModel.Rate = taxtModel.Concept[i].TaxConcept[j].Rate;
                    taxDetailtModel.TaxAmount = taxtModel.Concept[i].TaxConcept[j].TaxAmount;
                    taxDetailtModel.MinimumTax = taxtModel.Concept[i].TaxConcept[j].MinimumTax;

                    TotalTaxConcept.push(taxDetailtModel)
                }

            }
        }
    }
    $('#ModalTaxes').UifModal('showLocal', '');

});

/////////////////////////////////////////
//  Validación en fechas               //
/////////////////////////////////////////
$('#OtherPaymentsEstimatedPaymentDate').on("datepicker.change", function (event, date) {
    $("#alertMainOtherPayment").UifAlert('hide');
    if (IsDate($('#OtherPaymentsEstimatedPaymentDate').val())) {
        var systemDate = GetCurrentDate();
        var dateAfter = $('#OtherPaymentsEstimatedPaymentDate').val();
        if (!ValidateDateAfter(dateAfter)) {
            $("#alertMainOtherPayment").UifAlert('show', Resources.InvalidDates, "warning");
            $("#OtherPaymentsEstimatedPaymentDate").val("");
        }
    }
});

$("#PaymentConcepts").find("#CheckDate").on("datepicker.change", function (event, date) {
    $("#alertForm").UifAlert('hide');
    if (IsDate($("#PaymentConcepts").find("#CheckDate").val())) {

    }
    else {
        $("#PaymentConcepts").find("#CheckDate").val(GetCurrentDate());
    }
});

/////////////////////////////////////////
//  Asignación código de la moneda     //
//  de acuerdo a lo que se seleccione  //
/////////////////////////////////////////
$('#OtherPaymentsAccountingCurrency').on('itemSelected', function (event, selectedItem) {
    if (selectedItem.Id != "") {
        oPaymentRequestModel.PaymentRequestCurrencyId = selectedItem.Id;
    }
});

//Se debe ejecutar solo cuando es una Solicitud nueva
function validateCompanyOtherPaymentRequest() {

    if ($("#OtherPaymentsAccountingCompany").val() != "" && $("#OtherPaymentsAccountingCompany").val() != null) {

        if (parseInt($("#ViewBagParameterMulticompany").val()) == 0) {

            $("#OtherPaymentsAccountingCompany").attr("disabled", true);
        }
        else {
            $("#OtherPaymentsAccountingCompany").attr("disabled", false);
        }
        clearInterval(timeOtherPaymentRequest);
    }
}

//Revisa que el dropdown de tipo de pago haya sido cargado.
function CheckPaymentMethodsLoaded() {
    var isLoaded;
    return paymentMethodsPromise = new Promise(function (resolve, reject) {
        paymentMethodsTime = setInterval(function () {
            if ($('#OtherPaymentsPaymentMethods').children('option').length > 0) {
                isLoaded = true;
                resolve(isLoaded);
            }
        }, 3);
    });
}


