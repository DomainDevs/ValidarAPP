
/*----------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*                                               DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                                                 */
/*----------------------------------------------------------------------------------------------------------------------------------------------------------*/

var errorNumber = 0;
var Alert = 0;
var billControlId = $("#ViewBagBillControlId").val();
var branchId = $("#ViewBagBranchId").val();
var allowSave = true;
var closeBill = false;
var cashMessage = 0;
var invokedFrom = false;
var collectControlId = 0;

//*------------------------------------------*
var registerDateBc = "";
var summaryPaymentMethodsPromise;
var billControlPaymentPromise;
var totalCalculationDailyPromise;
/*----------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*                                                                          ACCIONES / EVENTOS                                                              */
/*----------------------------------------------------------------------------------------------------------------------------------------------------------*/

$(document).on('ready', function () {
    billControlId = $("#ViewBagBillControlId").val();
    branchId = $("#ViewBagBranchId").val();

    if ($("#ViewBagBranchDisable").val() == "1") {
        $("#BranchDrop").attr("disabled", "disabled");
    } else {
        $("#BranchDrop").removeAttr("disabled");
    }
    if ($("#ViewBagBranchDefault").val() > 0) {
        loadCashier($("#ViewBagBranchDefault").val());
    }
    $("#pnlBillingClosure").hide();

    setBranch($("#ViewBagBillControlId").val(), $("#ViewBagBranchId").val());
    
    validateCloseBill();
});



/*
 * Setea por default la sucursal y obtiene los cajeros
 */
$('#BranchDrop').on('binded', function () {
    if ($("#ViewBagBranchDisable").val() == "1") {
        $("#BranchDrop").attr("disabled", "disabled");
    }
    else {
        $("#BranchDrop").removeAttr("disabled");
    }

    loadCashier($("#BranchDrop").val());
});

$("#SaveClosure").click(function () {

    $("#alertForm").UifAlert('hide');

    //valida form
    if ($("#DailyCloseForm").valid()) {

        if (parseInt($("#CashierDrop").val()) > 0) {
            lockScreen();
            var date = "";
            if (registerDateBc != "") {
                date = registerDateBc;
            }
            var branchCode = parseInt($("#BranchDrop").val());

            DailyCashClosingRequest.ValidateCheckCardDeposited(branchCode, 0, date)
                .done(function (data) {
                    if (data > 0) {
                        errorNumber = 1;

                        $("#CardMessage").val(FormatCurrency(FormatDecimal(data)));
                        $("#alertForm").UifAlert('show', Resources.ValidateCash, "warning");
                        unlockScreen();

                    } else {
                        errorNumber = 0;
                    }

                    if (errorNumber == 0 || errorNumber == undefined) {
                        Alert = 0;
                    }
                    else {
                        Alert = 1;
                        errorNumber = 0;
                    }

                    if (Alert != 0) {
                        var msj = Resources.DailyClosingCashSaveValidateCheck + ". " + Resources.DailyClosingCashSaveValidateCashOne + ": " + $("#CardMessage").val();
                        $("#alertForm").UifAlert('show', msj, "warning");
                        unlockScreen();
                    }
                    else {
                        DailyCashClosingRequest.ValidateCashDeposited(branchCode, 0, date).done(
                            function (data) {
                                if (data != 0) {
                                    errorNumber = 1;
                                    cashMessage = data;
                                    var exceededAmount = FormatMoneySymbol(Math.abs(data));
                                    var message = Resources.ValidateCash + exceededAmount;
                                    if (data > 0) {
                                        message = Resources.DailyClosingCashSaveValidateCash + ". " + Resources.DailyClosingCashSaveValidateCashOne + ": " + exceededAmount;
                                    }
                                    $("#alertForm").UifAlert('show', message, "warning");
                                    unlockScreen();

                                } else {

                                    $("#tblDailyClosure").dataTable().fnClearTable();

                                    SummaryPaymentMethods(billControlId);
                                    summaryPaymentMethodsPromise.then(function (summaryPaymentMethodsData) {

                                        if (summaryPaymentMethodsData.aaData.length > 0) {
                                            copyDetailClosure(summaryPaymentMethodsData.aaData);
                                            var oBillControl = BillControl(billControlId, summaryPaymentMethodsData.aaData);

                                            if (allowSave == true) { //sin valores en cero
                                                SaveBillControlPayment(oBillControl);
                                                billControlPaymentPromise.then(function (billControlData) {

                                                    $.ajax({
                                                        url: ACC_ROOT + "Billing/CloseBillControl",
                                                        data: { "billControlId": billControlId, "branchId": $("#BranchDrop").val() },
                                                        success: function (data) {
                                                            if (data.success == false) {
                                                                $("#alertForm").UifAlert('show', data.result, "danger");
                                                                unlockScreen();
                                                            }
                                                            else {

                                                                $("#branchName").val($("#BranchDrop option:selected").text());
                                                                $("#branchCode").val($("#BranchDrop").val());
                                                                $("#billCode").val(-1);
                                                                $("#userNick").val($("#CashierDrop option:selected").text());
                                                                $("#currentDate").val($("#CurrentDate").val());

                                                                $("#BillingClosureTable").dataTable().fnClearTable();
                                                                $("#BalanceInquieriesTable").dataTable().fnClearTable();

                                                                $("#BranchDrop").val(" ");
                                                                $("#CashierDrop").val(" ");
                                                                $("#BranchDrop").trigger('change');

                                                                loadCashier(-1);

                                                                $('#SaveDailyCloseSuccessDialog').UifModal('showLocal', Resources.DailyClosingCashCloseBox);

                                                            }
                                                            unlockScreen();
                                                        }
                                                    });
                                                });
                                            }
                                            else {
                                                $("#alertForm").UifAlert('show', Resources.BillingClosureValuesInZero, "warning");
                                                unlockScreen();
                                            }

                                        }//fin sin regisros
                                        else {
                                            //PARA CERRAR CAJA VOLUNTARIAMENTE SIN REGISTROS SE GUARDA SOLO LA CABECERA
                                            if (closeBill == true) {
                                                $.ajax({
                                                    url: ACC_ROOT + "Billing/CloseBillControl",
                                                    data: { "billControlId": billControlId, "branchId": $("#BranchDrop").val() },
                                                    success: function (data) {

                                                        if (data.success == false) {
                                                            $("#alertForm").UifAlert('show', data.result, "danger");
                                                        }
                                                        else {
                                                            $("#branchName").val($("#BranchDrop option:selected").text());
                                                            $("#branchCode").val($("#BranchDrop").val());
                                                            $("#billCode").val(-1);
                                                            $("#userNick").val($("#CashierDrop option:selected").text());
                                                            $("#currentDate").val($("#CurrentDate").val());

                                                            $('#SaveDailyCloseSuccessDialog').UifModal('showLocal', Resources.DailyClosingCashCloseBox);

                                                            $("#BillingClosureTable").dataTable().fnClearTable();
                                                            $("#BranchDrop").val(" ");
                                                            $("#CashierDrop").val(" ");
                                                            $("#BranchDrop").trigger('change');
                                                            loadCashier(-1);
                                                        }
                                                        unlockScreen();
                                                    }
                                                });
                                            }
                                            else {
                                                unlockScreen();
                                            }

                                        }
                                    });
                                }
                            });
                    }

                });
        }
        else {
            setDataFieldsEmptyDaily();
        }
    } //valid form
});

$("#ExportExcelCollectItems").click(function () {
    $("#alertForm").UifAlert('hide');
    //valida form
    if ($("#DailyCloseForm").valid()) {   
        showReportExcelCollectItems();
    }
});

//CUANDO SE SELECCIONA EL COMBO DEL CAJERO
$("#CashierDrop").on('itemSelected', function (event, selectedItem) {
    $("#alertForm").UifAlert('hide');
    $("#CurrentDate").val("");
    if ($('#CashierDrop').val() != "") {

        if ($("#BranchDrop").val() != "") {

            $.ajax({
                async: false,
                type: "POST",
                dataType: "json",
                url: ACC_ROOT + "Billing/RequiredCloseBill",
                data: {
                    "branchId": $("#BranchDrop").val(), "userId": $("#CashierDrop").val(),
                    "accountingDatePresent": $("#ViewBagDateAccounting").val()
                }
            }).done(function (userData) {
                billControlId = userData[0].Id;
                collectControlId = userData[0].Id;

                $("#CurrentDate").val(userData[0].AccountingDate);

                if (userData[0].resp == false) {
                    $.ajax({
                        url: ACC_ROOT + "Billing/AdmitOpenBill",
                        data: {
                            "branchId": $("#BranchDrop").val(), "userId": $("#CashierDrop").val(),
                            "accountingDate": userData[0].OpenDateString
                        }
                    }).done(function (userdata) {
                        if (userdata[0].resp == true) {

                            $("#alertForm").UifAlert('show', Resources.ClosureMessage, "warning");

                            $("#BranchDrop").val(" ");
                            $("#CashierDrop").val(" ");

                            closeBill = false;
                        }
                        else {
                            closeBill = true;
                        }
                    });
                }
                else {
                    closeBill = true;
                }

                totalCalculationDaily(userData[0].OpenDateString);

                totalCalculationDailyPromise.then(function (totalDailyData) {
                    var totalIncomeAmount = 0;
                    var totalAmount = 0;

                    if (totalDailyData.aaData.length > 0) {
                        $("#ExportExcelCollectItems").show();
                    }
                    else {
                        $("#ExportExcelCollectItems").hide();
                    }

                    for (var i = 0; i <= totalDailyData.aaData.length - 1; i++) {
                        totalIncomeAmount += parseFloat(ClearFormatCurrency(totalDailyData.aaData[i].IncomeAmount.toString().replace("", ",")));
                        totalAmount += parseFloat(ClearFormatCurrency(totalDailyData.aaData[i].Amount.toString().replace("", ",")));

                        $("#BalanceInquieriesTable").UifDataTable('addRow', totalDailyData.aaData[i]);
                    }

                    $('#TotalIncomeAmount').text(totalIncomeAmount);
                    $('#TotalAmount').val(totalAmount);

                });

            });
        }
        else {
            $("#BillingClosureTable").UifDataTable('clear');
        }
    }
    else {
        $("#BalanceInquieriesTable").UifDataTable('clear');
        $("#ExportExcelCollectItems").hide();
    }
});

$('#BranchDrop').on('itemSelected', function (event, selectedItem) {

    $("#alertForm").UifAlert('hide');
    $("#BalanceInquieriesTable").dataTable().fnClearTable();
    $("#ExportExcelCollectItems").hide();
    $("#CurrentDate").val("");
    $("#CashierDrop").val("");

    if (selectedItem.Id > 0) {
        loadCashier(selectedItem.Id);
    }
});


/*----------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*                                                                       FUNCIONES                                                                              */
/*----------------------------------------------------------------------------------------------------------------------------------------------------------*/

function setDataFieldsEmptyDaily() {

    if (branchId == "") {
        $("#BranchDrop").val("");
    } else {
        $("#CashierDrop").empty();
        $("#CashierDrop").val("");
        $("#BillingClosureTable").dataTable().fnClearTable();
        $("#BalanceInquieriesTable").dataTable().fnClearTable();
    }
}

//Valida que todos los cheques y tarjetas ingresadas por caja (de la sucursal, usuario
//y hasta la fecha de proceso) estén con estado
//“asignado a boleta” (es decir depositadas)
function validateCheckCardDeposited() {
    var date = "";
    if (registerDateBc != "") {
        date = registerDateBc;
    }

    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "Billing/ValidateCheckCardDeposited",
        data: { "branchId": $("#BranchDrop").val(), "currencyId": 0, "registerDate": date },
        success: function (data) {

            if (data > 0) {
                errorNumber = 1;

                $("#CardMessage").val(FormatCurrency(FormatDecimal(data)));
                $("#alertForm").UifAlert('show', Resources.ValidateCash, "warning");
                unlockScreen();

            } else {
                errorNumber = 0;
            }
        }
    });
}

function getRegisterDate(billControlId) {
    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "Billing/GetRegisterDateBillControl",
        data: { "billControlId": billControlId }
    }).done(function (regdata) {
        registerDateBc = regdata[0].RegisterDateBC;
    });
}

function copyDetailClosure(summaryPaymentMethodsData) {

    for (var i = 0; i <= summaryPaymentMethodsData.length - 1; i++) {

        var rowid = summaryPaymentMethodsData[i];
        $("#tblDailyClosure").UifDataTable('addRow',
            {
                'Id': rowid.Id,
                'IdPaymentMethod': rowid.IdPaymentMethod,
                'PaymentMethods': rowid.PaymentMethods,
                'TotalAdmitted': rowid.TotalAdmitted,
                'TotalReceived': FormatCurrency(FormatDecimal(rowid.TotalReceived)),
                'TotalDifference': rowid.TotalDifference
            });
    }
}

function loadBranch() {

    var controller = ACC_ROOT + "Billing/GetBranchByOpenStatus";
    $("#BranchDrop").UifSelect({ source: controller });
}

function totalCalculationDaily(registerDateBc) {
    return totalCalculationDailyPromise = new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: ACC_ROOT + "Transaction/GetBalanceInquiries",
            data: {
                "branch": $("#BranchDrop").UifSelect("getSelected"),
                "user": $("#CashierDrop").UifSelect("getSelected"),
                "date": registerDateBc
            }
        }).done(function (totalDailyData) {
            resolve(totalDailyData);
        });
    });
}

function setBranch(billControlId, branchId) {
    if (billControlId > 0) {
        loadCashier(brachId);
    } else {    
        invokedFrom = false;
    }
}

function isNull(ele) {
    return ele == undefined || ele == null;
}

function loadCashier(branchId) {
    DailyCashClosingRequest.GetCashierByBranchId(branchId)
        .done(function (data) {
            if (!isNull(data) && !isNull(data.data)) {
                $("#CashierDrop").UifSelect({ sourceData: data.data });

                $("#BranchDrop").removeAttr('disabled');
                $("#BranchDrop").UifSelect('setSelected', branchId);
                invokedFrom = true;
            } else {
                $("#BranchDrop").attr('disabled', 'disabled');
            }
        })
        .fail(function() {
            $("#BranchDrop").attr('disabled', 'disabled');
        });
}

function validateCloseBill() {
    //CERRAR LA CAJA MANUALMENTE SELECCIONANDO EL BRANCH DESEADO
    if (!(billControlId == 0 || billControlId == "")) {
        closeBill = true;
    }
}

//************************************************************************************************************************
// P R O G R A M A C I Ó N    D E    B O T O N E S    D E     M O D A L E S

//BOTÓN DE ACEPTAR DEL MODAL SaveDailyCloseSuccessDialog
function AcceptDialog(event, modal) {
    $("#" + modal).UifModal('hide');

    if (invokedFrom == false) {
        loadBranch();
    }
    else {
        location.href = $("#ViewBagMainBillingLink").val();
    }
}

function showReportExcelCollectItems() {
    lockScreen();
    DailyCashClosingRequest.GenerateCollectItemsExcel(collectControlId)
        .done(function (data) {
            if (data.success) {
                DownloadFile(data.result, true, function () {
                    return "excel.xls";
                });
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        })
        .always(function () {
            $.unblockUI();
        });
}


function BillControl(billControlId, billingClousure) {

    var listBillControlPayment = {
        BillControlPayments: []
    };

    for (var i = 0; i <= billingClousure.length - 1; i++) {

        var field = {
            BillControlPaymentId: billingClousure[i].Id,
            IdPaymentMethod: billingClousure[i].IdPaymentMethod,
            PaymentTotalAdmitted: parseFloat(billingClousure[i].TotalAdmitted),
            PaymentsTotalReceived: parseFloat(billingClousure[i].TotalReceived),
            PaymentsTotalDifference: parseFloat(billingClousure[i].TotalDifference)
        }

        listBillControlPayment.BillControlPayments.push(field);
    }

    return {
        BillControlId: billControlId,
        BillControlPayments: listBillControlPayment.BillControlPayments
    }
}

function SummaryPaymentMethods(billControlId) {
    return summaryPaymentMethodsPromise = new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: ACC_ROOT + "Billing/GetSummaryPaymentMethods",
            data: { "billControlId": billControlId }
        }).done(function (summaryPaymentMethodsData) {
            resolve(summaryPaymentMethodsData);
        });
    });
}

function SaveBillControlPayment(oBillControl) {
    return billControlPaymentPromise = new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: ACC_ROOT + "Billing/SaveBillControlPayment",
            data: { "frmBillControl": oBillControl }
        }).done(function (billControlData) {
            resolve(billControlData);
        });
    });
}