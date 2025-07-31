/*----------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*                                               DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                                                 */
/*----------------------------------------------------------------------------------------------------------------------------------------------------------*/

var transferSelection = 0;
var BeneficiaryId = 0;
var ExchangeRate = 0;
var AccountBankId = 0;
var IsGeneratingFile = 0;
var AccountBankCode;
var accountBankNumber = 0;
var individualId = 0;
var isSearchPaymentOrdersTransfer = 0;
var transferDatePromise;

var oTransferPaymentOrderModel = {
    Id: 0,
    AccountBankId: 0,
    DeliveryDate: null,
    CancellationDate: null,
    StatusId: 0,
    UserId: 0,
    PaymentOrdersItems: []
};

var oPaymentOrderTransferModel = {
    PaymentOrderId: 0,
    AccountBankId: 0,
    BankId: 0,
    AccountBankNumber: ""
};

$(document).ready(function () {
    ////////////////////////////////////////
    // Autocomplete Cuenta Bancaria       //
    ////////////////////////////////////////
    $(document).ajaxSend(function (event, xhr, settings) {
        if (event.currentTarget.activeElement.id == "MainTransferAccountBankNumber") {
            if (settings.url.indexOf("SearchAccountBank") != -1) {
                settings.url = settings.url + "&param=1";
            }
        }
        if (event.currentTarget.activeElement.id == "MainTransferBankName") {
            if (settings.url.indexOf("SearchAccountBank") != -1) {
                settings.url = settings.url + "&param=2";
            }
        }
        //console.log(settings.url);
    });

    /////////////////////////////////////////
    //  Botón  Aceptar                     //
    /////////////////////////////////////////
    $("#modalBankAccountsTransfer").find("#AcceptMovement").click(function () {
        var rowId = $("#modalBankAccountsTransfer").find("#BankAccounts").UifDataTable("getSelected");
        if (rowId != null) {
            accountBankNumber = rowId[0].AccountNumber;
        }
        else {
            $("#modalBankAccountsTransfer").find("#alertModal").UifAlert('show', Resources.SelectOneBankAccount, "warning");
            return;
        }

        $("#modalBankAccountsTransfer").UifModal('hide');

    });

    //Oculta  modal
    $("#modalTransferSave").find("#AcceptMovementTrans").click(function () {
        $("#modalTransferSave").UifModal('hide');
    });
});

/*-------------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*                                                      DEFINICIÓN DE FUNCIONES GLOBALES                                                                       */
/*-------------------------------------------------------------------------------------------------------------------------------------------------------------*/

///////////////////////////////////////////////////
//  Limpia campos y variables                   //
//////////////////////////////////////////////////
function setCleanFieldsTransfer() {
    $("#MainTransferAccountBankNumber").val('');
    $("#MainTransferBankName").val('');
    $("#CurrencyMainTransfer").val('');
    $("#DatePaymentMainTransfer").val('');
    $("#TotalSelectedMainTransfer").val('');
    $("#MainTransfer").dataTable().fnClearTable();
    IsGeneratingFile = 0;
}


function SetGridValue(individualId, accountBankNumber) {
    var rows = $("#MainTransfer tbody .row-selected");
    for (var i in rows) {
        var item = $(rows[i]).find("td");
        if ($(item[21]).text() == individualId) {
            $(item[5]).text(accountBankNumber);
            return;
        }
    }
}


///////////////////////////////////////////////////
//  Obtiene fecha del servidor                   //
//////////////////////////////////////////////////
function getDateTransfer() {

    return transferDatePromise = new Promise(function (resolve, reject) {
        if ($("#ViewBagImputationType").val() == undefined &&
            $("#ViewBagParameterMulticompanyPayment").val() == undefined &&
            $("#ViewBagBillControlId").val() == undefined) {
            $.ajax({
                type: "GET",
                url: ACC_ROOT + "Common/GetDate",
                success: function (transferDate) {
                    resolve(transferDate);
                }
            });
        }
    });

}

///////////////////////////////////////////////////
//  Muestra mensaje de confirmación              //
//////////////////////////////////////////////////
function ConfirmTransfer() {
    $.UifDialog('confirm', { 'message': Resources.CancelApplicationMessage, 'title': Resources.BankTransfers }, function (result) {
        if (result) {
            setCleanFieldsTransfer();
        }
    });
}

///////////////////////////////////////////////////
//  Muestra mensaje de confirmación              //
//////////////////////////////////////////////////
function showConfirmTransfer() {
    $.UifDialog('confirm', { 'message': Resources.TransferGenerateFile, 'title': Resources.BankTransfers }, function (result) {
        if (!result) {
            setCleanFieldsTransfer();
        }
        else {
            if (IsGeneratingFile == 1) {

                lockScreen();
                setTimeout(function () {
                    $.ajax({
                        type: "POST",
                        url: ACC_ROOT + "Transfer/SaveTransferRequest",
                        data: JSON.stringify({ "transferPaymentOrderModel": SetDataTransfer() }),
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            //EXCEPCION ROLLBACK
                            if (data.success == false) {
                                                            
                                $("#alert").UifAlert('show', data.result, "danger");
                            } 
                            else{
                                if (data.TransferPaymentOrder > 0) {
                                    GenerateFile(data.TransferPaymentOrder, data.PaymenOrderId);
                                    $("#modalTransferSave").find("#transferId").val(data.TransferPaymentOrder);
                                    if (data.IsEnabledGeneralLedger == false) {
                                        $("#modalTransferSave").find("#accountingLabelDiv").hide();
                                        $("#modalTransferSave").find("#accountingMessage").hide();
                                    } 
                                    else {
                                        $("#modalTransferSave").find("#accountingLabelDiv").show();
                                        $("#modalTransferSave").find("#accountingMessage").show();
                                    }
                                    $("#modalTransferSave").find("#accountingMessage").val(data.TransferPaymentOrderMessage);
                                    SetDataTransferEmpty();
                                    setCleanFieldsTransfer();
                                }
                                else {
                                    $("#alert").UifAlert('show', data.Message, "danger");
                                }
                                SetDataTransferEmpty();
                                setCleanFieldsTransfer();
                            }

                           unlockScreen();
                        }
                    });
                }, 1000);
            }
        }
    });
}

///////////////////////////////////////////////////
//  Calcula total transferencia                 //
//////////////////////////////////////////////////
function getTotalTransfer() {
    transferSelection = 0;
    var rowid = $("#MainTransfer").UifDataTable("getSelected");
    if (rowid != null) {
        for (var i in rowid) {            
            transferSelection += parseFloat(ClearFormatCurrency(rowid[i].Amount.replace("", ",")));
        };
    }

    $("#TotalSelectedMainTransfer").val("$ " + NumberFormatDecimal(transferSelection, "2", ".", ","));
};

//////////////////////////////////////////////////
//  Arma Objeto   oTransferPaymentOrderModel   //
//////////////////////////////////////////////////
function SetDataTransfer() {
    oTransferPaymentOrderModel.Id = 0; //autonumerico
    oTransferPaymentOrderModel.AccountBankId = AccountBankId;
    oTransferPaymentOrderModel.DeliveryDate = $("#DeliveryDateMainTransfer").val();

    var rowid = $("#MainTransfer").UifDataTable("getSelected");
    if (rowid != null) {
        for (var i in rowid) {
            oPaymentOrderTransferModel = {
                PaymentOrderId: 0,
                AccountBankId: 0,
                BankId: 0,
                AccountBankNumber: ""
            };

            oPaymentOrderTransferModel.PaymentOrderId = rowid[i].PaymentOrderCode;
            oPaymentOrderTransferModel.AccountBankId = rowid[i].AccountBankCode;
            oPaymentOrderTransferModel.BankId = rowid[i].BankCode;
            oPaymentOrderTransferModel.AccountBankNumber = rowid[i].BankAccountNumber;

            oTransferPaymentOrderModel.PaymentOrdersItems.push(oPaymentOrderTransferModel);
        }
    }


    return oTransferPaymentOrderModel;
}

///////////////////////////////////////////////////
//  Limpia Objeto   oTransferPaymentOrderModel //
//////////////////////////////////////////////////
function SetDataTransferEmpty() {
    oTransferPaymentOrderModel = {
        Id: 0,
        AccountBankId: 0,
        DeliveryDate: null,
        CancellationDate: null,
        StatusId: 0,
        UserId: 0,
        PaymentOrdersItems: []
    };

    oPaymentOrderTransferModel = {
        PaymentOrderId: 0,
        AccountBankId: 0,
        BankId: 0,
        AccountBankNumber: ""
    };
}


///////////////////////////////////////////////////
//  Lanza proceso de generación de archivo      //
//////////////////////////////////////////////////
function GenerateFile(transferPaymentOrderId, paymentOrderId) {
    $.ajax({
        async: false,
        url: ACC_ROOT + "Transfer/GenerateTransferFile",
        data: { "transferPaymentOrderId": transferPaymentOrderId, "paymentOrderId": paymentOrderId },
        success: function (data) {

            SetDownloadLink(data);
            $('#modalTransferSave').UifModal('showLocal', Resources.BankTransfers);
        }
    });
}


///////////////////////////////////////////////////
//  Muestra link de descarga                    //
//////////////////////////////////////////////////
function SetDownloadLink(fileName) {

    var result = ACC_ROOT + "Transfer/Download?fileName=" + fileName;    
    $('#modalTransferSave').find("#dynamicLink").attr("href", result);
}



/*--------------------------------------------------------------------------------------------------------------------------------------------------*/
//                                                                  ACCIONES / EVENTOS
/*--------------------------------------------------------------------------------------------------------------------------------------------------*/


////////////////////////////////////////
// Autocomplete banco emisor - cheque //
////////////////////////////////////////
$('#MainTransferAccountBankNumber').on('itemSelected', function (event, selectedItem) {

    AccountBankId = selectedItem.AccountBankId;

    $("#CurrencyMainTransfer").val(selectedItem.CurrencyDescription);
    $("#MainTransferBankName").val(selectedItem.LongDescription);
});


$('#MainTransferBankName').on('itemSelected', function (event, selectedItem) {
    AccountBankId = selectedItem.AccountBankId;
    $("#CurrencyMainTransfer").val(selectedItem.CurrencyDescription);
    $("#MainTransferAccountBankNumber").val(selectedItem.AccountNumber);
});


//Seleccionar y quitar item de la tabla y realizar una accion
$('body').delegate('#MainTransfer tbody tr', "click", function () {

    if ($(this).hasClass('selected')) {
        $(this).removeClass('selected');
    }
    else {
        $(this).siblings('.selected').removeClass('selected');
        $(this).addClass('selected');
    }

    var pendings = $("#MainTransfer").UifDataTable("getSelected");
    if (pendings != null) {
        getTotalTransfer();
    }
    else {
        $("#TotalSelectedMainTransfer").val("$ 0.00")
    }
});

////////////////////////////////////////
// Selecciona una Cuenta Bancaria     //
////////////////////////////////////////
$('#MainTransfer').on('rowEdit', function (event, data, position) {
    var controller = ACC_ROOT + "PaymentOrders/GetBankAccountByBeneficiaryId?beneficiaryId=" +
                            data.IndividualId + "&isSearchPaymentOrders=" + isSearchPaymentOrdersTransfer;

    $('#modalBankAccountsTransfer').UifModal('showLocal', Resources.BankAccountsOf + ' ' + data.BeneficiaryName);
    $("#modalBankAccountsTransfer").find("#BankAccounts").UifDataTable({ source: controller });
});


getDateTransfer();
transferDatePromise.then(function (transferDate) {
    $("#DeliveryDateMainTransfer").val(transferDate);
});


$("#SearchMainTransfer").click(function () {
    $("#alert").UifAlert('hide');
    $("#MainTransferForm").validate();
    if ($("#MainTransferForm").valid()) {
        var controller = ACC_ROOT + "Transfer/SearchTransferPaymentOrders?paymentDate=" + $('#DatePaymentMainTransfer').val();
        $("#MainTransfer").UifDataTable({ source: controller });
    }
});

$("#CleanMainTransfer").click(function () {
    setCleanFieldsTransfer();
});

$("#GenerateMainTransfer").click(function () {
    if (AccountBankId > 0) {
        var rowid = $("#MainTransfer").UifDataTable("getSelected");
        if (rowid != null) {
            IsGeneratingFile = 1;
            
            showConfirmTransfer();
        } else {
            $("#alert").UifAlert('show', Resources.WarningSelectOneRecord, "warning");
        }
    } else {
        $("#alert").UifAlert('show', Resources.SelectOneBankAccount, "warning");
    }        
});

$("#CancelMainTransfer").click(function () {
    IsGeneratingFile = 0;
    ConfirmTransfer();
});