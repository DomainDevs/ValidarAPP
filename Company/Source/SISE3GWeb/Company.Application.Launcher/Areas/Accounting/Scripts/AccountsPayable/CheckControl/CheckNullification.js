
/*----------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*                                               DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                                                 */
/*----------------------------------------------------------------------------------------------------------------------------------------------------------*/

var checkBookControlId = 0;
var currencyId;

var oCheckPaymentOrderModel = {
    Id: 0,
    CheckNumber: 0,
    AccountBankId: 0,
    IsCheckPrinted: 0,
    Status: 0,
    StatusCheckBookControl: 0,
    CheckBookControlId: 0,
    PaymentOrdersItems: []
};

var oPaymentOrderCheckModel = {
    PaymentOrderId: 0,
    AccountBankId: 0,
    PaymentDate: null,
    TotalAmount: 0,
    TempImputationCode: 0,
    CurrencyId: 0,
    ExchangeRate: 0
};

/*---------------------------------------------------------------------------------------------------------------------------------------------------*/
/*                                                      DEFINICIÓN DE FUNCIONES                                                                      */
/*---------------------------------------------------------------------------------------------------------------------------------------------------*/


if ($("#ViewBagBranchDisableCheckNull").val() == "1") {
    setTimeout(function () {
        $("#BranchCheckNullification").attr("disabled", "disabled");
    }, 300);
}
else {
    $("#BranchCheckNullification").removeAttr("disabled");
}

setTimeout(function () {
    GetBanksCheckNull();
}, 2000);


///////////////////////////////////////////////////
//  Limpia Objeto   oCheckPaymentOrderModel     //
//////////////////////////////////////////////////
function setDataEmptyCancelAssigment() {
    oCheckPaymentOrderModel = {
        Id: 0,
        CheckNumber: 0,
        AccountBankId: 0,
        IsCheckPrinted: 0,
        Status: 0,
        StatusCheckBookControl: 0,
        CheckBookControlId: 0,
        PaymentOrdersItems: []
    };
}

///////////////////////////////////////////////////
//  Arma Objeto   oCheckPaymentOrderModel     //
//////////////////////////////////////////////////
function setDataCancelAssigmentCheck() {

    oCheckPaymentOrderModel.AccountBankId = $("#AccountNumberCheckNullification").val();
    oCheckPaymentOrderModel.CheckBookControlId = parseInt(checkBookControlId);
    oCheckPaymentOrderModel.CheckNumber = $("#CheckNumberCheckNullification").val();
    oCheckPaymentOrderModel.IsCheckPrinted = 0;
    oCheckPaymentOrderModel.Status = 0;
    oCheckPaymentOrderModel.StatusCheckBookControl = 0;


    var rowid = $("#CheckNullification").UifDataTable("getSelected");

    if (rowid != null) {
        for (var i in rowid) {

            oPaymentOrderCheckModel = {
                PaymentOrderId: 0,
                AccountBankId: 0,
                PaymentDate: null,
                TotalAmount: 0,
                TempImputationCode: 0,
                CurrencyId: 0,
                ExchangeRate: 0
            };
            oCheckPaymentOrderModel.Id = rowid[i].CheckPaymentOrderCode;
            oPaymentOrderCheckModel.PaymentOrderId = rowid[i].PaymentOrderCode;
            oPaymentOrderCheckModel.AccountBankId = -1;
            oPaymentOrderCheckModel.PaymentDate = null;

            if (rowid[i].Amount != "") {
                oPaymentOrderCheckModel.TotalAmount = parseInt(rowid[i].Amount.replace(/[^0-9.-]+/g, ""));
            }

            oPaymentOrderCheckModel.TempImputationCode = parseInt(rowid[i].TempImputationCode);
            oPaymentOrderCheckModel.CurrencyId = currencyId;
            oPaymentOrderCheckModel.ExchangeRate = rowid[i].ExchangeRate;

            oCheckPaymentOrderModel.PaymentOrdersItems.push(oPaymentOrderCheckModel);

        }
    }
    return oCheckPaymentOrderModel;
}


///////////////////////////////////////////////////
//  Limpia campos del formulario                //
//////////////////////////////////////////////////
function setCleanFieldsCheckNull() {

    $("#BranchCheckNullification").val('');
    $("#BankNameCheckNullification").val('');
    $("#AccountNumberCheckNullification").val('');
    $("#CheckNumberCheckNullification").val('');
    $("#CurrencyCheckNullification").val('');
    $("#CheckNullification").dataTable().fnClearTable();
}


///////////////////////////////////////////////////
//  Muestra mensaje de confirmación              //
//////////////////////////////////////////////////
function showConfirmCheckNull() {
    $.UifDialog('confirm', { 'message': Resources.CancelApplicationMessage, 'title': Resources.PathCancelCheck }, function (result) {
        if (result) {
            setCleanFieldsCheckNull();
        }

    });
};



/*-------------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*                                                      DEFINICIÓN DE ACCIONES                                                                                */
/*-------------------------------------------------------------------------------------------------------------------------------------------------------------*/

///////////////////////////////////////////////////
//  Boton limpiar - Limpia campos del formulario  //
//////////////////////////////////////////////////
$("#CleanCheckNullification").click(function () {
    CleanCheckNullificationFunction();
    $("#BranchCheckNullification").val('');
});

function CleanCheckNullificationFunction() {
    $("#CheckNullification").dataTable().fnClearTable();

    $("#BankNameCheckNullification").val('');
    $("#AccountNumberCheckNullification").val('');
    $("#CheckNumberCheckNullification").val('');
    $("#CurrencyCheckNullification").val('');
    $("#alertCheckullification").UifAlert('hide');
    $("#BankNameCheckNullification").UifSelect("disabled", true);
    $("#AccountNumberCheckNullification").UifSelect("disabled", true);
}

///////////////////////////////////////////////////////
/// Dropdown Sucursal                               ///
///////////////////////////////////////////////////////
$('#BranchCheckNullification').on('itemSelected', function (event, selectedItem) {
    $("#alertCheckullification").UifAlert('hide');
    if ($('#BranchCheckNullification').val() != "" && $('#BranchCheckNullification').val() != null) {
        var controller = ACC_ROOT + "CheckControl/GetBankBranchsByBranchId?branchId=" + selectedItem.Id;
        $("#BankNameCheckNullification").UifSelect({ source: controller });
    }
    CleanCheckNullificationFunction();
});


///////////////////////////////////////////////////////
/// Dropdown Bancos                                 ///
///////////////////////////////////////////////////////
$('#BankNameCheckNullification').on('itemSelected', function (event, selectedItem) {
    $("#alertCheckullification").UifAlert('hide');
    if ($('#BankNameCheckNullification').val() != "" && $('#BankNameCheckNullification').val() != null) {
        var controller = ACC_ROOT + "CheckControl/GetAccountNullifyByBankIdByBranchId?bankId=" + selectedItem.Id + "&branchId=" + $('#BranchCheckNullification').val();
        $("#AccountNumberCheckNullification").UifSelect({ source: controller });

    }
});

///////////////////////////////////////////////////////
/// Dropdown Cuenta                                 ///
///////////////////////////////////////////////////////
$('#AccountNumberCheckNullification').on('itemSelected', function (event, selectedItem) {
    $("#alertCheckullification").UifAlert('hide');
    $.ajax({
        type: "POST",
        url: ACC_ROOT + "CheckControl/GetAccountBankByAccountBankId",
        data: { "accountBankId": ($("#AccountNumberCheckNullification").val() == "") ? -1 : $("#AccountNumberCheckNullification").val() },
        success: function (data) {
            if (data.length > 0) {
                $("#CurrencyCheckNullification").val(data[0].CurrencyName);
                currencyId = data[0].CurrencyId;
            } else {
                $("#CurrencyCheckNullification").val("");
                currencyId = -1;
            }
        }
    });
});


/// Se oculta alert al perder foco
$('#CheckNumberCheckNullification').blur(function () {
    $("#alertCheckullification").UifAlert('hide');
});

/////////////////////////////////////////
//  Botón  Buscar                      //
/////////////////////////////////////////
$("#SearchCheckNullification").click(function () {
    $("#alertCheckullification").UifAlert('hide');
    $("#CheckNullificationForm").validate();
    if ($("#CheckNullificationForm").valid()) {
        //if ($("#BankNameCheckNullification").val() > 0)
        if ($("#BankNameCheckNullification").val() != "" && $("#BankNameCheckNullification").val() != null) {
            $("#CheckNullification").UifDataTable();
            var controller = ACC_ROOT + "CheckControl/GetCanceledCheckByAccountBankIdByCheckNumber?accountBankId=" + $('#AccountNumberCheckNullification').val() +
                "&checkNumber=" + $('#CheckNumberCheckNullification').val();
            $("#CheckNullification").UifDataTable({ source: controller });
        }
        else {
            $("#alertCheckullification").UifAlert('show', Resources.SelectBranchBankAssociate, "warning");
        }

    }

});

/////////////////////////////////////////
//  Botón  Aceptar                     //
/////////////////////////////////////////
$("#AcceptCheckNullification").click(function () {
    var rowid = $("#CheckNullification").UifDataTable("getSelected");
    if (rowid != null) {

        if (rowid.every(x => x.PaymetOrderStatusDescription != Resources.Global.Applied && x.PaymetOrderStatusDescription != Resources.Global.Paid && x.PaymetOrderStatusDescription != Resources.Global.Authorized)) {

            lockScreen();
            setTimeout(function () {
                $.ajax({
                    type: "POST",
                    url: ACC_ROOT + "CheckControl/SaveCancelCheckRequest",
                    data: JSON.stringify({ "checkPaymentOrderModel": setDataCancelAssigmentCheck() }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        unlockScreen();
                        //EXCEPCION ROLLBACK
                        if (data.success == false) {

                            $("#alertCheckullification").UifAlert('show', data.result, "danger");
                        } else {

                            $("#alertCheckullification").UifAlert('show', Resources.SuccessAnnulment, "success");
                        }

                        setDataEmptyCancelAssigment();
                        setCleanFieldsCheckNull();
                    }
                });

            }, 500);

        } else {
            $("#alertCheckullification").UifAlert('show', Resources.CancelPostedStatus, "danger");
        }
    } else {
        $("#alertCheckullification").UifAlert('show', Resources.WarningSelectedPaymentOrder, "warning");
    }

});


/////////////////////////////////////////
//  Botón  Cancelar                    //
/////////////////////////////////////////
$("#CancelCheckNullification").click(function () {
    showConfirmCheckNull();
});

function GetBanksCheckNull() {
    if ($('#BranchCheckNullification').val() > 0) {
        var controller = ACC_ROOT + "CheckControl/GetBankBranchsByBranchId?branchId=" + $('#BranchCheckNullification').val();
        $("#BankNameCheckNullification").UifSelect({ source: controller });
    }
};

var select = false;

$("#CheckNullification").on('rowSelected', function (data) {

    select = !select;
    var rows = $("#CheckNullification").UifDataTable("getData");
    var value = { label: "PaymentOrderCode", values: [] }

    rows.map(function (item) {
        value.values.push(item.PaymentOrderCode)
    })
    if (select) {
        $("#CheckNullification").UifDataTable('setSelect', value);
    }
    else {
        $("#CheckNullification").UifDataTable('setUnselect', value);
    }
});

$("#CheckNullification").on('rowDeselected', function (data) {

    select = !select;
    var rows = $("#CheckNullification").UifDataTable("getData");
    var value = { label: "PaymentOrderCode", values: [] }

    rows.map(function (item) {
        value.values.push(item.PaymentOrderCode)
    })
    if (select) {
        $("#CheckNullification").UifDataTable('setSelect', value);
    }
    else {
        $("#CheckNullification").UifDataTable('setUnselect', value);
    }
});

