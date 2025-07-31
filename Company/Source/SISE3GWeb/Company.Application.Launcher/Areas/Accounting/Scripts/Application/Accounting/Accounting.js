/*---------------------------------------------------------------------------------------------------------------------------------*/
/*                                            DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                           */
/*---------------------------------------------------------------------------------------------------------------------------------*/
var tempImputationId = $("#ViewBagTempImputationId").val();

var accountingBeneficiaryId = 0;
var percentageDifference = 0;
var accountingAccountingAccountId = 0;
var accountingBeneficiaryDocument = $('#AccountingDocumentNumber').val();
var accountingBeneficiaryName = $('#AccountingName').val();
var accountingPaymentConceptCode = $('#AccountingPaymentConceptId').val();
var accountingPaymentConceptName = $('#AccountingPaymentConceptDescription').val();

var thirdAccountingUsed = $('#ViewBagThirdAccountingUsed').val();

var time;
var loadedMovementsPromise;
var editAccouting = -1;
var oAccountCheckingAccountModel = null;

function PostDatedModel() {
    this.ValueTypeId;
    this.ValueTypeDescription;
    this.ValueNumberId;
    this.ValueNumberDescription;
    this.Amount
}

function CostCenterModel() {
    this.CostCenterId;
    this.CostCenterDescription;
    this.Percentage;
}

function AnalysisModel() {
    this.AnalysisCode;
    this.AnalysisDescription;
    this.AnalysisConcept;
    this.AnalysisConceptDescription;
    this.AnalysisKey;
    this.Description;
}

var oAccountingTransactionModel =
{
    TempDailyAccountingCode: 0,
    TempImputationCode: 0,
    BranchCode: 0,
    SalesPointCode: 0,
    CompanyCode: 0,
    PaymentConceptId: 0,
    BeneficiaryId: 0,
    BookAccountCode: 0,
    AccountingNature: 0,
    CurrencyId: 0,
    IncomeAmount: 0,
    ExchangeRate: 0,
    Amount: 0,
    Description: null,
    BankReconciliationId: 0,
    ReceiptNumber: 0,
    ReceiptDate: null,
    PostdatedAmount: 0,
    CostCenters: [],
    Analyses: []
};

function rowAccountModel() {
    this.TempDailyAccountingCode,
        this.TempImputationCode,
        this.BranchCode,
        this.SalesPointCode,
        this.CompanyCode,
        this.PaymentConceptId,
        this.BeneficiaryId,
        this.BookAccountCode,
        this.AccountingNature,
        this.CurrencyId,
        this.IncomeAmount,
        this.ExchangeRate,
        this.Amount,
        this.Description,
        this.BankReconciliationId,
        this.ReceiptNumber,
        this.ReceiptDate,
        this.PostdatedAmount,
        this.CostCenters,
        this.Analyses
};

var keyLength = 0;
var exchangeRate = 0;


$(document).ready(function () {

    /*---------------------------------------------------------------------------------------------------------------------------------*/
    /*                                                        ACCIONES / EVENTOS                                                       */
    /*---------------------------------------------------------------------------------------------------------------------------------*/
    $("#ModalPremiums").find("#closeModalPremiumsButton").hide();

    //$("#tabAnalysisCode").hide();
    $("#RowPolicy").hide();

    $("#tabCostCenter").hide();
    $("#analysisCode").prop('disabled', true);
    $("#costCenter").prop('disabled', true);

    //Xhr de autocomplete de varios parametros
    $(document).ajaxSend(function (event, xhr, settings) {

        var parameterUrl = settings.url.split("?");
        var paramConcept = settings.url.split("=");

        if (settings.url.indexOf("GetPaymentConceptById") != -1) {

            if ($("#AccountingBranch").val() == undefined) {
                settings.url = settings.url + "&param=" + $("#OtherPaymentsBranch").val() + "/" + personCode + "/1";
            }
            else {
                var concept = paramConcept[1].split("&");
                settings.url = parameterUrl[0] + "?query=" + concept[0] + "&param=" + $("#AccountingBranch").val() + "/0/1";
            }

        }
        if (settings.url.indexOf("GetPaymentConceptByDescription") != -1) {
            if ($("#AccountingBranch").val() == undefined) {
                settings.url = settings.url + "&param=" + $("#OtherPaymentsBranch").val() + "/" + personCode + "/2";
            }
            else {

                var conceptDesciption = paramConcept[1].split("&");
                settings.url = parameterUrl[0] + "?query=" + conceptDesciption[0] + "&param=" + $("#AccountingBranch").val() + "/0/2";
            }
        }

        if (settings.url.indexOf("GetAccountingAccountByNumber") != -1) {
            if ($("#AccountingBranch").val() != undefined) {

                var accountNumber = paramConcept[1].split("&");
                settings.url = parameterUrl[0] + "?query=" + accountNumber[0] + "&param=" + $("#AccountingBranch").val();
            }
        }
        if (settings.url.indexOf("GetAccountingAccountByDescription") != -1) {
            if ($("#AccountingBranch").val() != undefined) {

                var accountDescription = paramConcept[1].split("&");
                settings.url = parameterUrl[0] + "?query=" + accountDescription[0] + "&param=" + $("#AccountingBranch").val();
            }
        }


    });


    $("#AccountingMovementModal").find("#accountingPostdatedList").UifListView({
        autoHeight: true,
        theme: 'dark',
        source: null,
        customAdd: false,
        customDelete: false,
        customEdit: false,
        add: true,
        edit: false,
        delete: true,
        addCallback: savePosdatedCallback,
        deleteCallback: deletePostdated,
        addTemplate: '#postdatedAdd-template',
        displayTemplate: "#accountingPostdatedList-display-template"
    });


    $("#AccountingMovementModal").find("#AccountingBankAccount").hide();

    $("#AccountingMovementModal").find("#accountingMovementsCostCenter").UifListView({
        autoHeight: true,
        theme: 'dark',
        source: null,
        customDelete: false, //true
        customEdit: true,
        add: false,
        edit: false, //true
        delete: true, //false
        deleteCallback: deleteCostCenter,
        addTemplate: '#costCenterAdd-template',
        displayTemplate: "#accountingCostCenter-display-template"
    });

    $("#AccountingMovementModal").find("#accountingMovementsAnalysisCode").UifListView({
        autoHeight: true,
        theme: 'dark',
        source: null,
        customDelete: false, //true
        customEdit: true,
        add: false,
        edit: false, // true
        delete: true, //false
        deleteCallback: deleteAnalysis,
        addTemplate: '#analysisAdd-template',
        displayTemplate: "#accountingAnalysisCode-display-template"
    });

    //dropdown de sucursal
    $("#AccountingMovementModal").find("#AccountingBranch").on('itemSelected', function (event, selectedItem) {
        if (selectedItem.Id > 0) {
            var controller = ACC_ROOT + "Common/GetSalesPointByBranchId?branchId=" + selectedItem.Id;
            $("#AccountingSalePoint").UifSelect({ source: controller });

            $("#AccountingPaymentConceptId").val("");
            $("#AccountingPaymentConceptDescription").val("");
            $("#AccountingAccountNumber").val("");
            $("#AccountingAccountName").val("");

            $("#AccountingPaymentConceptId").UifAutoComplete({ source: null, displayKey: "Id", minLenght: 1 });
        }
        accountingPaymentConceptCode = "";
        accountingPaymentConceptName = "";

        $('#AccountingPaymentConceptId').UifAutoComplete('clean');
        $('#AccountingPaymentConceptDescription').UifAutoComplete('clean');
        $('#AccountingAccountNumber').UifAutoComplete('clean');
        $('#AccountingAccountName').UifAutoComplete('clean');

        /*limpia el cache*/
        $('#AccountingPaymentConceptId').UifAutoComplete();
        $('#AccountingPaymentConceptDescription').UifAutoComplete();
        $('#AccountingAccountNumber').UifAutoComplete();
        $('#AccountingAccountName').UifAutoComplete();

        $("#AccountingCurrency").val("");
        $("#AccountingExchangeRate").val("");

    });

    /**
        * Obtiene los conceptos de análisis a partir del identificador de análisis.
        *
        * @param {Object} event        - Seleccionar.
        * @param {Object} selectedItem - Información del registro seleccionado.
        */
    $("#AccountingMovementModal").find("#AccountingAnalysisCode").on('itemSelected', function (event, selectedItem) {
        $("#AccountingMovementModal").find("#analysisCodeAlert").UifAlert('hide');
        $("#AccountingMovementModal").find("#KeyFields").html("");
        if (selectedItem != undefined) {
            var controller = ACC_ROOT + "Common/GetAnalysisConceptByAnalysisId?analysisId=" + selectedItem.Id;
            $("#AccountingAnalysisConcept").UifSelect({ source: controller });
        }
    });

    /**
        * Obtiene las columnas que conforman la clave de análisis al seleccionar un concepto de análisis.
        *
        * @param {Object} event        - Seleccionar.
        * @param {Object} selectedItem - Información del registro seleccionado.
        */
    $("#AccountingMovementModal").find("#AccountingAnalysisConcept").on('itemSelected', function (event, selectedItem) {
        $("#AccountingMovementModal").find("#analysisCodeAlert").UifAlert('hide');
        $("#AccountingMovementModal").find("#KeyFields").html("");

        if (selectedItem.Id > 0) {

            $.ajax({
                type: "GET",
                url: ACC_ROOT + 'Common/GetConceptKeysByAnalysisConceptId',
                data: {
                    "analysisId": $("#AccountingMovementModal").find("#AccountingAnalysisCode").val(),
                    "analysisConceptId": selectedItem.Id
                },
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.length > 0) {
                        keyLength = data.length;
                        CreateTexBoxDinamic(data, 0, "");

                        if ($("#AccountingMovementModal").find("#AccountingAnalysisConcept").val() == "1" ||
                            $("#AccountingMovementModal").find("#AccountingAnalysisConcept").val() == "2") {

                            $("#AccountingMovementModal").find("#Key0").attr('onkeypress', "return JustNumbers(event, this)");
                            $("#AccountingMovementModal").find("#Key0").attr('maxlength', "12");

                            $("#AccountingMovementModal").find("#Key1").attr('onkeypress', "return JustNumbers(event, this)");
                            $("#AccountingMovementModal").find("#Key1").attr('maxlength', "12");
                        }
                        else if ($("#AccountingMovementModal").find("#AccountingAnalysisConcept").val() == "3") {
                            $("#AccountingMovementModal").find("#Key0").attr('onkeypress', "return JustNumbers(event, this)");
                            $("#AccountingMovementModal").find("#Key0").attr('maxlength', "12");
                        }
                    }
                    else {
                        $("#AccountingMovementModal").find("#analysisCodeAlert").UifAlert('show', Resources.MessageValidationKeysAnalysisConcept, "warning");
                    }
                }
            });
        }
        else {
            $("#AccountingMovementModal").find("#KeyFields").html("");
        }
    });

    /**
        * Limpia el mensaje de validación de centro de costo.
        *
        * @param {Object} event        - Seleccionar.
        * @param {Object} selectedItem - Información del registro seleccionado.
        */
    $("#AccountingMovementModal").find("#AccountingCostCenter").on('itemSelected', function (event, selectedItem) {
        $("#AccountingMovementModal").find("#costCenterAlert").UifAlert('hide');
    });

    //autocomplete por número de documento
    $("#AccountingMovementModal").find('#AccountingDocumentNumber').on('itemSelected', function (event, selectedItem) {
        accountingBeneficiaryId = selectedItem.IndividualId;
        $('#AccountingName').val(selectedItem.Name);
        $('#AccountingDocumentNumber').val(selectedItem.DocumentNumber);
        accountingBeneficiaryDocument = selectedItem.DocumentNumber;
        accountingBeneficiaryName = selectedItem.Name;
    });

    //autocomplete por nombre
    $("#AccountingMovementModal").find('#AccountingName').on('itemSelected', function (event, selectedItem) {
        accountingBeneficiaryId = selectedItem.IndividualId;
        $('#AccountingName').val(selectedItem.Name);
        $('#AccountingDocumentNumber').val(selectedItem.DocumentNumber);
        accountingBeneficiaryDocument = selectedItem.DocumentNumber;
        accountingBeneficiaryName = selectedItem.Name;
    });



    //control de borrado de autocomplete en campo de número de documento.
    $("#AccountingMovementModal").find("#AccountingDocumentNumber").on('blur', function (event) {
        setTimeout(function () {
            $('#AccountingDocumentNumber').val(accountingBeneficiaryDocument);
        }, 50);
    });

    //control de borrado de autocomplete en campo de nombre.
    $("#AccountingMovementModal").find("#AccountingName").on('blur', function (event) {
        setTimeout(function () {
            $('#AccountingName').val(accountingBeneficiaryName);
        }, 50);
    });

    //autocomplete por código de concepto
    //$("#AccountingMovementModal").find('#AccountingPaymentConceptId').on('itemSelected', function (event, selectedItem) {

    //    $('#AccountingPaymentConceptId').val(selectedItem.Id);
    //    $('#AccountingPaymentConceptDescription').val(selectedItem.Description);
    //    $('#AccountingAccountNumber').val(selectedItem.AccountingNumber);
    //    $('#AccountingAccountName').val(selectedItem.AccountingName);
    //    accountingAccountingAccountId = selectedItem.GeneralLedgerId;
    //    accountingPaymentConceptCode = selectedItem.Id;
    //    accountingPaymentConceptName = selectedItem.Description;
    //    $("#AccountingCurrency").removeAttr("disabled");
    //    if (selectedItem.CurrencyId >= 0) {
    //        $('#AccountingCurrency').val(selectedItem.CurrencyId);
    //        $('#AccountingCurrency').trigger("itemSelected");
    //        $('#AccountingCurrency').attr("disabled", "disabled");
    //    }
    //    if (isBankReconciliation(selectedItem.GeneralLedgerId)) {
    //        $("#AccountingBankAccount").show();
    //    }
    //    if (selectedItem.RequiresAnalysis) {
    //        $("#analysisCode").prop('disabled', false);
    //    }
    //    if (selectedItem.RequiresCostCenter) {
    //        $("#costCenter").prop('disabled', false);
    //    }
    //});

    ////control de borrado de autocomplete en campo de Id de concepto de pago.
    //$("#AccountingMovementModal").find("#AccountingPaymentConceptId").on('blur', function (event) {
    //    setTimeout(function () {
    //        $('#AccountingPaymentConceptId').val(accountingPaymentConceptCode);
    //    }, 50);
    //});

    //autocomplete por nombre de concepto
    $("#AccountingMovementModal").find('#AccountingPaymentConceptDescription').on('itemSelected', function (event, selectedItem) {
        $('#AccountingPaymentConceptId').val(selectedItem.Id);
        $('#AccountingPaymentConceptDescription').val(selectedItem.Description);
        $('#AccountingAccountNumber').val(selectedItem.AccountingNumber);
        $('#AccountingAccountName').val(selectedItem.AccountingName);
        accountingAccountingAccountId = selectedItem.GeneralLedgerId;
        accountingPaymentConceptCode = selectedItem.Id;
        accountingPaymentConceptName = selectedItem.Description;
        $("#AccountingCurrency").removeAttr("disabled");
        if (selectedItem.CurrencyId >= 0) {
            $('#AccountingCurrency').val(selectedItem.CurrencyId);
            $('#AccountingCurrency').trigger("itemSelected");
            $('#AccountingCurrency').attr("disabled", "disabled");
        }
        if (isBankReconciliation(selectedItem.GeneralLedgerId)) {
            $("#AccountingBankAccount").show();
        }
    });


    //autocomplete por número de cuenta
    $("#AccountingMovementModal").find('#AccountingAccountNumber').on('itemSelected', function (event, selectedItem) {

        $('#AccountingPaymentConceptId').val(selectedItem.PaymentConceptId);
        $('#AccountingPaymentConceptDescription').val(selectedItem.PaymentConceptDescription);
        $('#AccountingAccountNumber').val(selectedItem.AccountingNumber);
        $('#AccountingAccountName').val(selectedItem.AccountingName);
        accountingAccountingAccountId = selectedItem.GeneralLedgerId;
        accountingPaymentConceptCode = selectedItem.PaymentConceptId;
        accountingPaymentConceptName = selectedItem.PaymentConceptDescription;
        $("#AccountingCurrency").removeAttr("disabled");
        if (selectedItem.CurrencyId >= 0) {
            $('#AccountingCurrency').val(selectedItem.CurrencyId);
            $('#AccountingCurrency').trigger("itemSelected");
            $('#AccountingCurrency').attr("disabled", "disabled");
        }
        if (isBankReconciliation(selectedItem.GeneralLedgerId)) {
            $("#AccountingBankAccount").show();
        }
        if (selectedItem.RequiresAnalysis) {
            $("#analysisCode").prop('disabled', false);
        }
        if (selectedItem.RequiresCostCenter) {
            $("#costCenter").prop('disabled', false);
        }
    });


    //autocomplete por nombre de cuenta
    $("#AccountingMovementModal").find('#AccountingAccountName').on('itemSelected', function (event, selectedItem) {

        $('#AccountingPaymentConceptId').val(selectedItem.PaymentConceptId);
        $('#AccountingPaymentConceptDescription').val(selectedItem.PaymentConceptDescription);
        $('#AccountingAccountNumber').val(selectedItem.AccountingNumber);
        $('#AccountingAccountName').val(selectedItem.AccountingName);
        accountingAccountingAccountId = selectedItem.GeneralLedgerId;
        accountingPaymentConceptCode = selectedItem.PaymentConceptId;
        accountingPaymentConceptName = selectedItem.PaymentConceptDescription;
        $("#AccountingCurrency").removeAttr("disabled");
        if (selectedItem.CurrencyId >= 0) {
            $('#AccountingCurrency').val(selectedItem.CurrencyId);
            $('#AccountingCurrency').trigger("itemSelected");
            $('#AccountingCurrency').attr("disabled", "disabled");
        }
        if (isBankReconciliation(selectedItem.GeneralLedgerId)) {
            $("#AccountingBankAccount").show();
        }
        if (selectedItem.RequiresAnalysis) {
            $("#analysisCode").prop('disabled', false);
        }
        if (selectedItem.RequiresCostCenter) {
            $("#costCenter").prop('disabled', false);
        }
    });


    //control de borrado de autocomplete en campo de nombre de concepto de pago.
    $("#AccountingMovementModal").find("#AccountingPaymentConceptDescription").on('blur', function (event) {
        setTimeout(function () {
            $('#AccountingPaymentConceptDescription').val(accountingPaymentConceptName);
        }, 50);
    });

    //cambio en el dropdown de moneda
    $("#AccountingMovementModal").find('#AccountingCurrency').on('itemSelected', function (event, selectedItem) {

        if ($('#AccountingCurrency').val() != undefined) {
            if ($('#AccountingCurrency').val() != "") {
                getPorcentageDifferenceAccounting($('#AccountingCurrency').val());
                SetCurrency("AccountingCurrency", "AccountingExchangeRate");
            }
        }
    });

    //formato de moneda en importe de contabilidad
    $("#AccountingMovementModal").find("#AccountingAmount").blur(function () {
        if ($("#AccountingAmount").val() != "") {
            var accountingAmount = $("#AccountingAmount").val();
            $("#AccountingAmount").val("$ " + NumberFormatSearch(accountingAmount.trim(), "2", ".", ","));
        }

    });





    //añadir movimiento contable
    $("#AccountingMovementModal").find("#AddAccountingMovement").on('click', function () {
        $("#addAccountingMovementForm").validate();

        var lastAmount = $("#AccountingAmount").val();
        preValidAmount('AccountingAmount');
        var duplicate = -1;

        if ($("#addAccountingMovementForm").valid()) {

            $("#AccountingAmount").val(lastAmount);
            
                lockScreen();
                setTimeout(function () {

                    $.ajax({
                        type: "POST",
                        url: ACC_ROOT + "Accounting/SaveTempAccountingTransactionRequest",
                        data: { "accountingTransactionModel": SetDataAccountingTransaction() }
                    }).done(function (data) {
                        if (data > 0) {
                            unlockScreen();
                            ClearFields();
                            SetDataAccountingTransactionEmpty();
                            AccountingMovementsReload();
                            $("#addAccountingMovementForm").formReset();
                            LoadAccountingMovementCompany();
                            setTimeout(function () {
                                SetAccountingTotalMovement();
                            }, 2000);
                        }
                    });
                }, 500);
            


        }
    });

    /**
        * Añade movimientos de análisis al listview.
        *
        */
    $("#AccountingMovementModal").find("#AddAccountingAnalysisMovement").on('click', function () {
        var count = 0;
        $("#AccountingMovementModal").find("#analysisCodeAlert").UifAlert('hide');

        if ($("#AccountingAnalysisCode").val() == "") {
            $("#AccountingMovementModal").find("#analysisCodeAlert").UifAlert('show', Resources.SelectAnalysisCode, "warning");
            return;
        }

        if ($("#AccountingAnalysisConcept").val() == "") {
            $("#AccountingMovementModal").find("#analysisCodeAlert").UifAlert('show', Resources.SelectAnalysisConcept, "warning");
            return;
        }

        if ($("#AccountingAnalysisDescription").val() == "") {
            $("#AccountingMovementModal").find("#analysisCodeAlert").UifAlert('show', Resources.EnterAnalysisDescription, "warning");
            return;
        }

        var keyValue = "";
        for (var i = 0; i < keyLength; i++) {
            keyValue += $("#Key" + i).val() + "|";
        }

        if (keyValue.indexOf("undefined") == 0 || keyValue == "" || keyValue.indexOf("|") == 0) {
            $("#AccountingMovementModal").find("#analysisCodeAlert").UifAlert('show', Resources.EnterAnalysisConceptKey, "warning");
            return;
        }

        var movements = $('#accountingMovementsAnalysisCode').UifListView("getData");

        if (movements.length > 0) {
            for (var i in movements) {
                if (parseInt(movements[i].AnalysisCode) == parseInt($('#AccountingAnalysisCode').val())
                    && parseInt(movements[i].AnalysisConcept) == parseInt($('#AccountingAnalysisConcept').val())) {
                    count++;
                }
            }
        }

        if (count > 0) {
            $("#AccountingMovementModal").find("#analysisCodeAlert").UifAlert('show', Resources.ValidateAnalysis, "warning");
            return;
        }

        var rowModel = new AnalysisModel();
        rowModel.AnalysisCode = $('#AccountingAnalysisCode').val();
        rowModel.AnalysisDescription = $('#AccountingAnalysisCode option:selected').text();
        rowModel.AnalysisConcept = $('#AccountingAnalysisConcept').val();
        rowModel.AnalysisConceptDescription = $('#AccountingAnalysisConcept option:selected').text();
        rowModel.Description = $('#AccountingAnalysisDescription').val();

        var keyValue = "";
        for (var i = 0; i < keyLength; i++) {
            keyValue += $("#Key" + i).val() + "|";
        }
        keyValue = keyValue.substring(0, keyValue.length - 1);
        $("#AccountingAnalysisKey").val(keyValue);

        rowModel.AnalysisKey = $('#AccountingAnalysisKey').val();

        $('#accountingMovementsAnalysisCode').UifListView("addItem", rowModel);

        $("#AccountingAnalysisCode").val("");
        $('#AccountingAnalysisKey').val("");
        $("#AccountingMovementModal").find("#KeyFields").html("");
        $("#AccountingAnalysisConcept").UifSelect();
        $("#AccountingMovementModal").find("#KeyFields").hide();
        $('#AccountingAnalysisDescription').val("");
    });

    /**
        * Añade movimientos de centros de costos al listview.
        *
        */
    $("#AccountingMovementModal").find("#AddAccountingCostCenterMovement").on('click', function () {
        $("#AccountingMovementModal").find("#costCenterAlert").UifAlert('hide');

        var percentage = 0.0;
        var count = 0;

        if ($("#AccountingCostCenter").val() == "") {
            $("#AccountingMovementModal").find("#costCenterAlert").UifAlert('show', Resources.SelectCostCenter, "warning");
            return;
        }

        if ($("#AccountingPercentageAmount").val() == "") {
            $("#AccountingMovementModal").find("#costCenterAlert").UifAlert('show', Resources.EnterPercentage, "warning");
            return;
        }

        var movements = $('#accountingMovementsCostCenter').UifListView("getData");

        if (movements.length > 0) {
            for (var i in movements) {
                percentage += parseFloat(movements[i].Percentage);
            }
            for (var i in movements) {
                if (parseInt(movements[i].CostCenterId) == parseInt($('#AccountingCostCenter').val())) {
                    count++;
                    return;
                }
                else {
                    count = 0
                }
            }
        }
        percentage += parseFloat($('#AccountingPercentageAmount').val());

        if (parseFloat(percentage) > 100) {
            $("#AccountingMovementModal").find("#costCenterAlert").UifAlert('show', Resources.ValidateCostCenterPercentage, "warning");
            return;
        }
        if (count > 0) {
            $("#AccountingMovementModal").find("#costCenterAlert").UifAlert('show', Resources.ValidateCostCenter, "warning");
            return;
        }

        var rowModel = new CostCenterModel();
        rowModel.CostCenterId = $('#AccountingCostCenter').val();
        rowModel.CostCenterDescription = $('#AccountingCostCenter option:selected').text();
        rowModel.Percentage = parseFloat($('#AccountingPercentageAmount').val());

        $('#accountingMovementsCostCenter').UifListView("addItem", rowModel);

        $("#AccountingCostCenter").val("");
        $("#AccountingPercentageAmount").val("");

    });

    //click en el botón aceptar del modal de movimiento de contabilidad
    $("#AccountingMovementModal").find("#AccountingAcceptMovement").on("click", function () {
        var amount = $("#ReceiptAmount").val();
        if (isNaN(amount)) {
            amount = 0;
        }
        if (amount == "") {
            amount = 0;
        } else {
            amount = parseFloat(ClearFormatCurrency($("#ReceiptAmount").val().replace("", ",")));
        }

        if ($("#ViewBagImputationType").val() == 1) {
            GetDebitsAndCreditsMovementTypesReceipt(tempImputationId, amount);
        }
        if ($("#ViewBagImputationType").val() == 2) {
            GetDebitsAndCreditsMovementTypesJournal(tempImputationId, amount);
        }
        if ($("#ViewBagImputationType").val() == 3) {
            GetDebitsAndCreditsMovementTypesPreLiquidation(tempImputationId, amount);
        }
        if ($("#ViewBagImputationType").val() == 4) {
            GetDebitsAndCreditsMovementTypes(tempImputationId, amount);
        }

        ClearFields();
        SetDataAccountingTransactionEmpty();

        CheckLoadedMovements();
        loadedMovementsPromise.then(function (isLoaded) {
            if (isLoaded) {
                clearTimeout(time);
                if ($("#ViewBagImputationType").val() == 1) {
                    SetTotalApplicationReceipt();
                    SetTotalControlReceipt();
                }
                if ($("#ViewBagImputationType").val() == 2) {
                    SetTotalApplicationJournal();
                    SetTotalControlJournal();
                }
                if ($("#ViewBagImputationType").val() == 3) {
                    SetTotalApplicationPreLiquidation();
                    SetTotalControlPreLiquidation();
                }
                if ($("#ViewBagImputationType").val() == 4) {
                    SetTotalApplication();
                    SetTotalControl();
                }
            }
        });

        ShowAccountingForm();
        $("#AccountingMovementModal").find("#accountingMovementsCostCenter").UifListView("refresh");
        $("#AccountingMovementModal").find("#accountingMovementsAnalysisCode").UifListView("refresh");
        $("#addAccountingMovementForm").formReset();
        $('#AccountingMovementModal').UifModal('hide');
    });

    //cuando se cierra el modal de edición
    $("#AccountingMovementModal").find('#AccountingMovementModal').on('hidden.bs.modal', function () {
        $("#AccountingMovementModal").find("#AcceptAccountingMovement").trigger('click');
    });

    $("#AccountingMovementModal").find("#accountingMovementsList").on('rowDelete', function (event, data) {
        deleteRow(data);
    });

    $("#AccountingMovementModal").find("#analysisCode").on('click', function () {
        $("#addAccountingMovementForm").validate();
        var lastAmount = $("#AccountingAmount").val();
        preValidAmount('AccountingAmount');
        if ($("#addAccountingMovementForm").valid()) {
            $("#AccountingAmount").val(lastAmount);
            $("#tabAnalysisCode").fadeIn();
            $("#tabCostCenter").hide();
            $("#tabAccounting").hide();
            $("#AccountingMovementModal").find("#AccountingAcceptMovement").prop('disabled', true);
        }

    });

    $("#AccountingMovementModal").find("#costCenter").on('click', function () {
        $("#addAccountingMovementForm").validate();
        var lastAmount = $("#AccountingAmount").val();
        preValidAmount('AccountingAmount');

        if ($("#addAccountingMovementForm").valid()) {
            $("#AccountingAmount").val(lastAmount);
            //$("#tabAnalysisCode").hide();
            $("#RowPolicy").hide();
            $("#tabCostCenter").fadeIn();
            $("#tabAccounting").hide();
            $("#AccountingMovementModal").find("#AccountingAcceptMovement").prop('disabled', true);
        }
    });

    $("#AccountingMovementModal").find("#CancelAddAccountingAnalysisMovement").on('click', function () {
        ShowAccountingForm();
        ClearAnalysisCodeFields();
    });

    $("#AccountingMovementModal").find("#CancelAddAccountingCostCenterMovement").on('click', function () {
        ShowAccountingForm();
        ClearCostCenterFields();
    });

});

/*---------------------------------------------------------------------------------------------------------------------------------*/
/*                                                     DEFINICION DE FUNCIONES                                                     */
/*---------------------------------------------------------------------------------------------------------------------------------*/

if ($("#ViewBagBranchDisable").val() == "1") {
    setTimeout(function () {
        $("#AccountingBranch").attr("disabled", "disabled");
    }, 300);
}
else {
    $("#AccountingBranch").removeAttr("disabled");
}

function LoadAccountingBranchs(branchUserDefault) {
    AccountingRequest.GetBranchs().done(function (data) {
        if (branchUserDefault == null || branchUserDefault == 0) {
            $("#AccountingBranch").UifSelect({ sourceData: data.data });
        }
        else {
            $("#AccountingBranch").UifSelect({ sourceData: data.data, selectedId: branchUserDefault });
        }
    });
}

function LoadAccountingSalesPointByBranchId(branchUserDefault, salePointBranchUserDefault) {
    AccountingRequest.GetSalesPointByBranchId(branchUserDefault).done(function (data) {
        if (salePointBranchUserDefault == null || salePointBranchUserDefault == 0) {
            $("#AccountingSalePoint").UifSelect({ sourceData: data.data });
        }
        else {
            $("#AccountingSalePoint").UifSelect({ sourceData: data.data, selectedId: salePointBranchUserDefault });
        }
    });
}
function LoadAcountingCompanies(idCompany) {
    AccountingRequest.GetAccountingCompanies().done(function (data) {
        if (isMulticompany == 0)
            $("#AccountingCompany").UifSelect({ sourceData: data.data, selectedId: idCompany, enable: false });
        else
            $("#AccountingCompany").UifSelect({ sourceData: data.data });
    });
}

function LoadAcountingNatures() {
    AccountingRequest.GetNatures().done(function (data) {
        $("#AccoutingNature").UifSelect({ sourceData: data.data });
    });
}

function LoadAccountingCurrencies() {
    AccountingRequest.GetCurrencies().done(function (data) {
        $("#AccountingCurrency").UifSelect({ sourceData: data.data });
    });
}


function isBankReconciliation(generalLedgerId) {
    var result = false;
    lockScreen();
    setTimeout(function () {
        $.ajax({
            type: "POST",
            async: false,
            url: ACC_ROOT + "Accounting/IsBankReconciliation",
            data: { "generalLedgerId": generalLedgerId },
            success: function (data) {
                unlockScreen();
                result = data;
            }
        });
    }, 500);
    return result;
}

function getPorcentageDifferenceAccounting(currencyDifferenceId) {
    lockScreen();
    setTimeout(function () {
        $.ajax({
            type: "POST",
            url: ACC_ROOT + "CheckingAccountBrokers/GetPercentageDifferenceByCurrencyId",
            data: { "currencyId": currencyDifferenceId },
            success: function (data) {
                unlockScreen();
                percentageDifference = data;
            }
        });
    }, 500);
}

// se obtiene la tasa de cambio
function getCurrencyRateBilling(accountingDate, currencyId) {
    var alert = true;
    var rate;
    var resp = new Array();

    if (currencyId != undefined) {

        lockScreen();
        setTimeout(function () {

            $.ajax({
                async: false,
                type: "POST",
                url: ACC_ROOT + "Common/GetCurrencyExchangeRate",
                data: { "rateDate": accountingDate, "currencyId": currencyId },
                success: function (data) {
                    unlockScreen();
                    if (data == 1 || data == 0) {
                        $.ajax({
                            async: false,
                            type: "POST",
                            url: ACC_ROOT + "Common/GetLatestCurrencyExchangeRate",
                            data: {
                                "rateDate": accountingDate,
                                "currencyId": currencyId
                            },
                            success: function (dataRate) {
                                if (dataRate == 0 || dataRate == 1) {
                                    rate = 1;
                                    exchangeRate = rate;
                                    alert = true;
                                }
                                else {
                                    rate = dataRate;
                                    exchangeRate = rate;
                                    alert = false;
                                }
                            }
                        });
                    }
                    else {
                        rate = data;
                        alert = true;
                    }
                }
            });

        }, 500);

        resp[0] = rate;
        resp[1] = alert;

    }

    return resp;
}


function SetDataAccountingTransaction() {
    oAccountingTransactionModel.TempDailyAccountingCode = 0;
    oAccountingTransactionModel.TempImputationCode = tempImputationId;
    oAccountingTransactionModel.BranchCode = $("#AccountingBranch").val();
    oAccountingTransactionModel.SalesPointCode = $("#AccountingSalePoint").val();
    oAccountingTransactionModel.CompanyCode = $("#AccountingCompany").val();
    oAccountingTransactionModel.PaymentConceptId = $("#AccountingPaymentConceptId").val();;
    oAccountingTransactionModel.BeneficiaryId = accountingBeneficiaryId;
    oAccountingTransactionModel.BookAccountCode = accountingAccountingAccountId;
    //añadido para BE
    oAccountingTransactionModel.BookAccountNumber = $("#AccountingAccountNumber").val().trim();;

    oAccountingTransactionModel.AccountingNature = $("#AccoutingNature").val();
    oAccountingTransactionModel.CurrencyId = $("#AccountingCurrency").val();
    oAccountingTransactionModel.IncomeAmount = ReplaceDecimalPoint(ClearFormatCurrency($("#AccountingAmount").val()).replace(",", "."));
    oAccountingTransactionModel.ExchangeRate = ReplaceDecimalPoint(ClearFormatCurrency($("#AccountingExchangeRate").val()).replace(",", "."));
    oAccountingTransactionModel.Amount = 0;
    oAccountingTransactionModel.Description = $("#AccountingDescription").val();
    oAccountingTransactionModel.BankReconciliationId = $("#AccountingBankConciliation").val();
    oAccountingTransactionModel.ReceiptNumber = $("#AccountingVoucherNumber").val();
    oAccountingTransactionModel.ReceiptDate = $("#AccountingVoucherDate").val();

    //códigos de analisis.
    var analyses = $("#accountingMovementsAnalysisCode").UifListView("getData");
    oAccountingTransactionModel.analyses = analyses;

    //centro de costos
    var costCenters = $("#accountingMovementsCostCenter").UifListView("getData");
    oAccountingTransactionModel.costCenters = costCenters;

    return oAccountingTransactionModel;
}

function SetDataAccountingTransactionEmpty() {
    oAccountCheckingAccountModel = {
        ImputationId: 0,
        AccountingCheckingAccountTransactionItems: []
    };
    oAccountingTransactionModel =
        {
            TempDailyAccountingCode: 0,
            TempImputationCode: 0,
            BranchCode: 0,
            SalesPointCode: 0,
            CompanyCode: 0,
            PaymentConceptId: 0,
            BeneficiaryId: 0,
            BookAccountCode: 0,
            AccountingNature: 0,
            CurrencyId: 0,
            IncomeAmount: 0,
            ExchangeRate: 0,
            Amount: 0,
            Description: null,
            BankReconciliationId: 0,
            ReceiptNumber: 0,
            ReceiptDate: null,
            PostdatedAmount: 0,
            CostCenters: [],
            Analyses: []
        };
}

function ClearFields() {

    accountingBeneficiaryId = 0;
    percentageDifference = 0;
    accountingAccountingAccountId = 0;

    $("#AccountingBranch").val("");
    $("#AccountingSalePoint").val("");
    $("#AccountingCompany").val("");
    $("#AccountingPaymentConceptId").val("");
    $("#AccountingPaymentConceptDescription").val("");
    $("#AccountingAccountNumber").val("");
    $("#AccountingAccountName").val("");
    $("#AccoutingNature").val("");
    $("#AccountingCurrency").val("");
    $("#AccountingExchangeRate").val("");
    $("#AccountingAmount").val("");
    $("#AccountingDescription").val("");
    $("#AccountingBankConciliation").val("");
    $("#AccountingVoucherNumber").val("");
    $("#AccountingVoucherDate").val("");
    $("#AccountingDocumentNumber").val("");
    $("#AccountingName").val("");

    //Analisis
    ClearAnalysisCodeFields();
    $("#AccountingMovementModal").find("#accountingMovementsAnalysisCode").UifListView("refresh");
    $("#analysisCode").prop('disabled', true);

    //Centros de Costos
    ClearCostCenterFields();
    $("#AccountingMovementModal").find("#accountingMovementsCostCenter").UifListView("refresh");
    $("#costCenter").prop('disabled', true);
}

function deleteRow(data) {
    if (data != null) {

        lockScreen();
        setTimeout(function () {
            $.ajax({
                type: "POST",
                url: ACC_ROOT + "Accounting/DeleteTempDailyAccountingTransactionItem",
                data: { "tempDailyAccountingTransactionItemId": data.TempDailyAccountingId },
                success: function () {
                    unlockScreen();
                    $("#addAccountingMovementForm").formReset();
                    AccountingMovementsReload();
                    LoadAccountingMovementCompany();
                    setTimeout(function () {
                        SetAccountingTotalMovement();
                    }, 2000);
                }
            });
        }, 500);
    }
}

var savePosdatedCallback = function (deferred, data) {

    var postDated = new PostDatedModel();
    postDated.ValueTypeId = $("#PostdatedValueType").val();
    postDated.ValueTypeDescription = $("#PostdatedValueType option:selected").text();
    postDated.ValueNumberId = 0
    postDated.ValueNumberDescription = $("#PostdatedValueNumber").val();;
    postDated.Amount = $("#PostdatedAmount").val();

    $('#accountingPostdatedList').UifListView("addItem", postDated);
};

var deletePostdated = function (deferred, data) {

};

var deleteCostCenter = function (deferred, data) {
    deferred.resolve();
};

var deleteAnalysis = function (deferred, data) {
    deferred.resolve();
};

function LoadAccountingMovementCompany() {
    if (isMulticompany == 0) {
        $("#AccountingCompany").val(accountingCompanyDefault);
        $("#AccountingCompany").attr("disabled", "disabled");
    }
    else {
        $("#AccountingCompany").removeAttr("disabled");
    }
}

/////////////////////////////////////////////////////////////
// Setear el total de la listview movimientos contabilidad //
/////////////////////////////////////////////////////////////
function SetAccountingTotalMovement() {
    var totalMovement = 0;
    var totalDebitMovement = 0;

    var accountings = $("#accountingMovementsList").UifListView("getData");

    if (accountings != null) {

        for (var j = 0; j < accountings.length; j++) {
            if (accountings[j].NatureId == 1) {
                var accountingCredit = String(accountings[j].Amount).replace("$", "").replace(/,/g, "").replace(" ", "");
                totalMovement += parseFloat(accountingCredit);
            }
            else {
                var accountingDebit = String(accountings[j].Amount).replace("$", "").replace(/,/g, "").replace(" ", "");
                totalDebitMovement += parseFloat(accountingDebit);
            }
        }
    }
    else {
        $("#TotalAccountingMovement").text("");
        $("#TotalDebitAccountingMovement").text("");
    }
    $("#TotalAccountingMovement").text("$ " + NumberFormatSearch(totalMovement, "2", ".", ","));
    $("#TotalDebitAccountingMovement").text("$ " + NumberFormatSearch(totalDebitMovement, "2", ".", ","));

}

function AccountingMovementsReload() {
    $("#accountingMovementsList").UifListView({
        height: 460,
        source: ACC_ROOT + "Accounting/GetTempAccountingTransactionItemByTempApplicationId?tempImputationId=" + tempImputationId,
        customDelete: true,
        customEdit: true,
        edit: false,
        delete: true,
        displayTemplate: "#accountingMovementsList-display-template"
    });

    setTimeout(function () {
        SetAccountingTotalMovement();
    }, 3800);
}

///////////////////////////////////////////
/// ListView - coaseguradora            ///
///////////////////////////////////////////
$("#AccountingMovementModal").find('#accountingMovementsList').on('rowEdit', function (event, data, index) {
    $("#AccountingDescription").val(data.Description);
    $("#AccountingAmount").val(data.Amount);
    $("#AccountingExchangeRate").val(data.Exchange);
    $("#AccountingAccountName").val(data.AccountName);


    $("#AccountingAccountNumber").val(data.AccountNumber);

    $('#AccountingPaymentConceptDescription').val(data.ConceptDescription);
    $('#AccountingPaymentConceptId').val(data.ConceptId);
    $('#AccountingName').val(data.BeneficiaryName);
    $('#AccountingDocumentNumber').val(data.BeneficiaryDocumentNumber);
    $('#AccountingBranch').UifSelect("setSelected", data.BranchId);
    $('#AccountingSalePoint').UifSelect("setSelected", data.SalePointId);
    $('#AccountingCompany').UifSelect("setSelected", data.CompanyId);
    $('#AccoutingNature').UifSelect("setSelected", data.NatureId);
    $('#AccountingCurrency').UifSelect("setSelected", data.CurrencyId);

    editAccouting = index;
});
function GetAccountingSalePoints() {
    if (isMulticompany == 0) {
        $("#AccountingCompany").val(accountingCompanyDefault);
        $("#AccountingCompany").attr("disabled", true);
    }
    else {
        $("#AccountingCompany").attr("disabled", false);
    }
    if (branchUserDefault > 0) {
        $("#AccountingBranch").val(branchUserDefault);

        if (branchUserDefault) {
            LoadAccountingSalesPointByBranchId(branchUserDefault,null)//jira SMT-2000 quitar puntos de venta por defecto.
        }
    }
    else {
        $("#AccountingBranch").val("");
    }

};


/**
    * Crea inputs dinámicos en base a parametrización de análisis de conceptos.
    *
    * @param {Object} keys - Nombres de campos del registro seleccionado.
    * @param {Number} edit - Edición del registro seleccionado.
    * @param {Object} data - Valores de campos del registro seleccionado.
    */
function CreateTexBoxDinamic(keys, edit, data) {
    var field = "";
    var divRow = "<div class='uif-row'>";
    var divColumn = "<div class='uif-col-6'>";
    var endDiv = "</div>";
    var response = data.split("|");

    for (var i = 0; i < keys.length; i++) {
        if (i == 0 || i == 2) {
            field = divRow + divColumn;
        }
        else {
            field += divColumn;
        }
        field += "<label>" + keys[i].ColumnDescription + "</label>";

        if (edit == 1) {
            if (response[i] == undefined) {
                field += "<input type='text' size='20' maxlength='15' id='Key" + i + "' name='Key" + i + "' value='' />";
            }
            else {
                field += "<input type='text' size='20' maxlength='15' id='Key" + i + "' name='Key" + i + "' value='" + response[i] + "' />";
            }

            field += endDiv;
            $("#AccountingMovementModal").find("#KeyFields").append(field);
        }
        else {
            field += "<input type='text' size='20' maxlength='15' id='Key" + i + "' name='Key" + i + "' />";
            field += endDiv;
            if (i == 1 || i == 3 || (i == keys.length - 1)) {
                field += endDiv;
                $("#AccountingMovementModal").find("#KeyFields").append(field);
            }
        }
    }
    $("#AccountingMovementModal").find("#KeyFields").show();
};

function CheckLoadedMovements() {
    var isLoaded;
    return loadedMovementsPromise = new Promise(function (resolve, reject) {
        time = setInterval(function () {
            var summaries = $("#listViewAplicationReceipt").UifListView("getData");
            if (summaries.length > 0) {
                isLoaded = true;
                resolve(isLoaded);
            }
        }, 3);
    });
}


function LoadthirdAccountingUsed() {
    if (thirdAccountingUsed == 0) {
        $("#AccountingDocumentNumber").attr("disabled", true);
        $("#AccountingName").attr("disabled", true);
    }
    else {
        $("#AccountingDocumentNumber").attr("disabled", false);
        $("#AccountingName").attr("disabled", false);
    }
}

function ShowAccountingForm() {
    $("#tabAnalysisCode").hide();
    $("#tabCostCenter").hide();
    $("#tabAccounting").fadeIn();
    $("#AccountingMovementModal").find("#AccountingAcceptMovement").prop('disabled', false);
}

function ClearAnalysisCodeFields() {
    $("#AccountingMovementModal").find("#AccountingAnalysisCode").val("");
    $("#AccountingMovementModal").find("#AccountingAnalysisConcept").UifSelect({ source: null });
    $("#AccountingMovementModal").find("#KeyFields").hide();
    $("#AccountingMovementModal").find("#AccountingAnalysisDescription").val("");
}

function ClearCostCenterFields() {
    $("#AccountingMovementModal").find("#AccountingCostCenter").val("");
    $("#AccountingMovementModal").find("#AccountingPercentageAmount").val("");
}

$("#AccountingPercentageAmount").on('change', function () {
    if ($("#AccountingPercentageAmount").val() > 100) {
        $("#AccountingMovementModal").find("#costCenterAlert").UifAlert('show', Resources.ValidateCostCenterPercentage, "warning");
        $("#AccountingPercentageAmount").val("");
    } else {
        $("#AccountingMovementModal").find("#costCenterAlert").UifAlert('hide');
    }
});

function preValidAmount(identifier) {
    var clearAmount = ClearFormatCurrency($('#' + identifier).val());
    var validAmount = clearAmount.split(".");

    if (validAmount[1] == 0) {
        clearAmount = validAmount[0];
    }
    $('#' + identifier).val(clearAmount.trim());
}


