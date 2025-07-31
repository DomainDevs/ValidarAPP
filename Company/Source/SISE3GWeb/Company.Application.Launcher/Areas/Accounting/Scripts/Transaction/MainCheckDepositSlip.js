/*----------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*                                               DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                                                 */
/*----------------------------------------------------------------------------------------------------------------------------------------------------------*/

var percentageMinQuota = $("#ViewBagPercentageParameter").val();
var userId = $("#ViewBagUserId").val();
$('#bankDate').val(GetCurrentDate());

var oPaymentBallotModel = {
    PaymentBallotId: 0,
    PaymentBallotNumber: null,
    PaymentAccountNumber: null,
    PaymentBallotBankId: 0,
    PaymentBallotAmount: 0,
    PaymentBallotBankAmount: 0,
    PaymentCurrency: null,
    PaymentStatus: 0,
    PaymentBankDate: null,

    PaymentTicketBallotModels: []
};

var oPaymentTicketBallotModel = {
    PaymentTicketBallotId: 0
};

/*----------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*                                                                          ACCIONES / EVENTOS                                                              */
/*----------------------------------------------------------------------------------------------------------------------------------------------------------*/
$(document).on('ready', function () {
    MainCheckDepositSlipRequest.GeBranchesforUser().done(function (data) {
        if (data != undefined && data.data != undefined) {
            $("#Branch").UifSelect({ sourceData: data.data });
            if ($('#BranchDefault').val() != '') {
                $("#Branch").UifSelect('setSelected', $("#BranchDefault").val());
                $("#Branch").trigger('change');
            }
        }
    });
});

//Carga datos de Número de Cuenta acorde a Banco Receptor
$('#ddlReceivingBank').on('itemSelected', function (event, selectedItem) {
    $("#ballotTotal").val('');
    if (selectedItem.Id != "") {
        var controller = ACC_ROOT + "Common/GetAccountDistinctByBankId?bankId=" + parseInt(selectedItem.Id);
        $("#ddlReceivingAccountNumber").UifSelect({ source: controller });
        $("#ddlReceivingAccountNumber").removeProp("disabled");
    }
    else {
        $("#ddlReceivingAccountNumber").UifSelect();
        setDataFieldsEmptyBank();

    }
});

//buscar banco por sucursal
$('#Branch').on('itemSelected', function (event, selectedItem) {
    if (selectedItem.Id > 0) {
        var controller = ACC_ROOT + "CheckControl/GetBankBranchsByBranchId?branchId=" + $("#Branch").val();
        $("#ddlReceivingBank").UifSelect({ source: controller });
        $('#ddlReceivingAccountNumber').UifSelect();
        "&branchId=" + $("#Branch option:selected").val();
    }
    else {
        $("#ddlReceivingBank").UifSelect();
        $('#ddlReceivingAccountNumber').UifSelect();
        setDataFieldsEmptyBank();
    }
});

// Datos de moneda al cambiar cuenta bancaria
$('#ddlReceivingAccountNumber').on('itemSelected', function (event, selectedItem) {

    $("#ballotTotal").val('');
    if ($("#ddlReceivingAccountNumber").val() != "") {

        $("#checksDialog").UifDataTable();

        $.ajax({
            url: ACC_ROOT + "Common/GetAccountCurrencyByBankIdSelect",
            data: { "bankId": $("#ddlReceivingBank").val(), "accountNumber": $("#ddlReceivingAccountNumber option:selected").text() },
            async: true,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $("#accountCurrency").val(data.Currency.Description);
                $("#accountCurrencyCd").val(data.Currency.Id);
            }
        });
        var controller = ACC_ROOT + "Transaction/GetCheckBallots?bankCode=" +
            $("#ddlReceivingBank").val() + "&accountNumber=" + $("#ddlReceivingAccountNumber option:selected").text() + "&branchId=" + $("#Branch option:selected").val();
        $("#checksDialog").UifDataTable({ source: controller });
    }
    else {
        setDataFieldsEmptyBank();
    }

});

//botón Guardar
$("#SaveTicketDepositSlip").click(function () {
    
    var percentage;
    var totReceivedMin;
    var totReceivedMax;
    var totalDataBallot;
    var currenteDate = GetCurrentDate();
    var branchId;

    $("#alert").UifAlert('hide');

    if ($("#ballotForm").valid()) {

        if (currenteDate < $('#bankDate').val()) {
            $("#alert").UifAlert('show', Resources.DateInvalid, 'warning');
        }
        else {
            if ($("#ddlReceivingAccountNumber").val() != "-1" && $("#ddlReceivingBank").val() != "-1" &&
                $("#bankBallotNumber").val() != "" && $("#bankDate").val() != "" && $("#bankTotal").val() != "") {
                if (gridValidationCheckDeposit() == true) {
                    percentage = parseFloat(percentageMinQuota) / 100;

                    var ballotTotal = $("#ballotTotal").val().replace("$", "").replace(/,/g, "").replace(" ", "");
                    var bankTotal = $("#bankTotal").val().replace("$", "").replace(/,/g, "").replace(" ", "");

                    totReceivedMin = parseFloat(ballotTotal) - (parseFloat(ballotTotal) * percentage);
                    totReceivedMax = parseFloat(ballotTotal) + (parseFloat(ballotTotal) * percentage);

                    if ((parseFloat(bankTotal) >= totReceivedMin) &&
                        (parseFloat(bankTotal) <= totReceivedMax)) {

                        totalDataBallot = setDataBallotCheckDeposit();
                        branchId = $("#Branch option:selected").val();

                        lockScreen();

                        setTimeout(function () {

                            $.ajax({
                                async: false,
                                dataType: "json",
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                url: ACC_ROOT + "Transaction/SavePaymentBallotRequest",
                                data: JSON.stringify({ frmPaymentBallot: totalDataBallot, userId: parseInt(userId), typeId: 1, branchId: branchId }),
                                success: function (data) {

                                    unlockScreen();

                                    //EXCEPCION ROLLBACK
                                    if (data.success == false) {

                                        $("#alert").UifAlert('show', data.result, "danger");

                                    } else {

                                        $("#BallotId").val(("0000000" + data[0].Id));
                                        $("#TechnicalTransaction").val(("0000000" + data[0].TechnicalTransaction));
                                        $("#BallotNumber").val(data[0].BallotId);
                                        $("#totAmountBallot").val("$ " + NumberFormatSearch(data[0].Total, "2", ".", ","));
                                        $("#BillingDateBallot").val(data[0].Date);
                                        $("#user").val(data[0].User);
                                        $("#accountingMessage").val(data[0].Message);

                                        if (!data[0].ShowMessage) {

                                            $("#accountingMessageDiv").hide();

                                        } else {

                                            $("#accountingMessageDiv").show();
                                        }


                                        $('#modalSaveSuccess').UifModal('showLocal', Resources.DepositSlip + " " + Resources.SuccessfullySaved);
                                    }

                                    cleanObject();
                                    setDataFieldsEmptyDepositSlip();
                                }
                            });
                        }, 1200);
                    }
                    else {
                        if ((parseFloat(ballotTotal) < totReceivedMin)) {
                            $("#alert").UifAlert('show', Resources.BankAndBallotAmountDiffer, 'warning');

                        }
                        if ((parseFloat(bankTotal) > totReceivedMax)) {
                            $("#alert").UifAlert('show', Resources.BankAndBallotAmountDiffer, 'warning');
                        }
                        if ((parseFloat(bankTotal) < totReceivedMax)) {
                            $("#alert").UifAlert('show', Resources.BankAndBallotAmountDiffer, 'warning');
                        }
                    }
                }
                else {
                    $("#alert").UifAlert('show', Resources.SelectAtLeastOne + " " + Resources.InternalBallot, 'warning');
                }
            }
            else {
                $("#alert").UifAlert('show', Resources.RequiredFieldsMissing, 'warning');
            }
        }
        
    }
});

//botón Cancelar
$("#CancelTicket").click(function () {
    setDataFieldsEmptyDepositSlip();
    cleanObject();
    $("#alert").UifAlert('hide');
});

// Llena objeto para enviar al controlador

$('#checksDialog').on('rowSelected', function (event, data, position) {
    totalBallotDepositSlip();
});


// Llena objeto de la tabla completa para enviar al controlador

$('#checksDialog').on('selectAll', function (event) {
    totalBallotDepositSlip();
});

// Atacha evento click al seleccionar un registro de la tabla y al des seleccionarlo

$('body').delegate('#checksDialog tbody tr', "click", function () {

    if ($(this).hasClass('selected')) {
        $(this).removeClass('selected');
        totalBallotDepositSlip();
    }
    else {
        $(this).siblings('.selected').removeClass('selected');
        $(this).addClass('selected');
        totalBallotDepositSlip();
    }
});

// Atacha evento click al seleccionar todos registros de la tabla y al des seleccionarlos todos
$('body').delegate('#checksDialog thead tr th button span', "click", function () {
    totalBallotDepositSlip();
});

//Formato Moneda (se activa cuando pierde el foco)
$("#bankTotal").blur(function () {
    var total = $.trim(ClearFormatCurrency($("#bankTotal").val()));
    if (total != "" && total != "$") {
      $("#bankTotal").val(FormatCurrency(FormatDecimal(total)))
    } else {
        $("#bankTotal").val('');
    }
});


// Validación de campos solo números y punto

$("#bankTotal").keypress(function (event) {
    if (event.which != 32 && event.keyCode != 8 && event.keyCode != 46 && event.keyCode != 37 && event.keyCode != 39 && event.keyCode != 9) {
        if (event.which < 48 || event.which > 57) {
            event.preventDefault();
        }
    }
});


// Validación de campos solo números

$("#bankBallotNumber").keypress(function (event) {
    if (event.which != 32 && event.keyCode != 8 && event.keyCode != 37 && event.keyCode != 39 && event.keyCode != 9) {
        if (event.which < 48 || event.which > 57) {
            event.preventDefault();
        }
    }
});

$("#bankDate").blur(function () {
    var dateBank = $("#bankDate").val();
    
    if (IsDate(dateBank)) {    
        $("#alert").UifAlert('hide');
    } else {
        
        $("#bankDate").val("");
        $("#alert").UifAlert('show', Resources.IncorrectDate, 'warning');
    }

});

/*-------------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*                                                      DEFINICIÓN DE FUNCIONES                                                                                */
/*-------------------------------------------------------------------------------------------------------------------------------------------------------------*/

// Llena objeto para enviar al controlador

function setDataBallotCheckDeposit() {
    oPaymentBallotModel = {
        PaymentBallotId: 0,
        PaymentBallotNumber: null,
        PaymentAccountNumber: null,
        PaymentBallotBankId: 0,
        PaymentBallotAmount: 0,
        PaymentBallotBankAmount: 0,
        PaymentCurrency: null,
        PaymentStatus: 0,
        PaymentBankDate: null,

        PaymentTicketBallotModels: []
    };

    var ballotTotal = $("#ballotTotal").val().replace("$", "").replace(/,/g, "").replace(" ", "");
    var bankTotal = $("#bankTotal").val().replace("$", "").replace(/,/g, "").replace(" ", "");

    oPaymentBallotModel.PaymentBallotId = 0;
    oPaymentBallotModel.PaymentBallotNumber = $("#bankBallotNumber").val();
    oPaymentBallotModel.PaymentAccountingAccountId = $("#ddlReceivingAccountNumber").UifSelect("getSelected");
    oPaymentBallotModel.PaymentAccountNumber = $('#ddlReceivingAccountNumber option:selected').text();
    oPaymentBallotModel.PaymentBallotBankId = $("#ddlReceivingBank").val();
    oPaymentBallotModel.PaymentBallotAmount = parseFloat(ballotTotal);
    oPaymentBallotModel.PaymentBallotBankAmount = parseFloat(bankTotal);
    oPaymentBallotModel.PaymentCurrency = $("#accountCurrencyCd").val();
    oPaymentBallotModel.PaymentBankDate = $("#bankDate").val();
    oPaymentBallotModel.PaymentStatus = 1;

    var selectedChecks = $("#checksDialog").UifDataTable("getSelected");
    if (selectedChecks != null) {
        $.each(selectedChecks, function (index, value) {
            {
                oPaymentTicketBallotModel = {
                    PaymentTicketBallotId: 0
                };
                oPaymentTicketBallotModel.PaymentTicketBallotId = value.DepositBallotId;
                oPaymentBallotModel.PaymentTicketBallotModels.push(oPaymentTicketBallotModel);
            }
        });
    }
    return oPaymentBallotModel;
}

// valida datos seleccionados en tabla
function gridValidationCheckDeposit() {
    var result = false;
    var ids = [];
    var table = $('#checksDialog').DataTable();
    ids = $("#checksDialog").UifDataTable("getSelected");

    if (ids != null) {
        if (ids.length > 0)
            result = true;
    }

    return result;
}

// obtiene el total de la boleta y de las internas elegidas

function totalBallotDepositSlip() {
    var total = 0;
    var selRowIds = $("#checksDialog").UifDataTable("getSelected");

    $("#alert").UifAlert('hide');

    for (var i in selRowIds) {
        var DepositBallotAmount = ClearFormatCurrency(selRowIds[i].DepositBallotAmount).replace(" ", "");
        var DepositBallotCashAmount = ClearFormatCurrency(selRowIds[i].DepositBallotCashAmount).replace(" ", "");

        total += parseFloat(DepositBallotAmount) + parseFloat(DepositBallotCashAmount);
        i++;
    }
    total = decimalAdjust('round', total, -3);
    $("#ballotTotal").val("$ " + NumberFormatSearch(total, "2", ".", ","));
    $("#ballotTotal").blur();
}

// limpia campos dle formulario

function setDataFieldsEmptyDepositSlip() {
    $("#ddlReceivingBank").val('');
    $("#accountCurrency").val('');
    $("#bankBallotNumber").val('');
    $("#bankDate").val('');
    $("#ballotTotal").val('');
    $("#bankTotal").val('');
    $("#checksDialog").dataTable().fnClearTable();

    $('#ddlReceivingAccountNumber').UifSelect();
    $("#Branch").UifSelect('setSelected', $("#BranchDefault").val());
    $("#Branch").trigger('change');
}


function setDataFieldsEmptyBank() {
    $("#accountCurrency").val('');
    $("#bankBallotNumber").val('');
    $("#bankDate").val('');
    $("#ballotTotal").val('');
    $("#bankTotal").val('');
    var controller = ACC_ROOT + "Transaction/GetCheckBallots?bankCode=" + 0 + "&accountNumber=" + "0" + "&branchId=" + 0;
    $("#checksDialog").UifDataTable({ source: controller });
}

// limpia campos dle formulario

function cleanObject() {
    oPaymentBallotModel = {
        PaymentBallotId: 0,
        PaymentBallotNumber: null,
        PaymentAccountNumber: null,
        PaymentBallotBankId: 0,
        PaymentBallotAmount: 0,
        PaymentBallotBankAmount: 0,
        PaymentCurrency: null,
        PaymentStatus: 0,
        PaymentBankDate: null,

        PaymentTicketBallotModels: []
    };

    oPaymentTicketBallotModel = {
        PaymentTicketBallotId: 0
    };
}


