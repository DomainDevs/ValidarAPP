/*----------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*                                               DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                                                 */
/*----------------------------------------------------------------------------------------------------------------------------------------------------------*/


var Alert = 0;
var Numerrors = 0;
var currencyIdCheckAutomatic = -1;
var numberAssociated = 0;
var checkBookControlId = 0;
var individualIdCheckAutomatic = -1;
var resultGrid = 0;
var checkBookActive = false;

var oPaymentOrderCheckModelAutomatic = {
    CheckBookControlId: 0,
    BeneficiaryTypeId: 0,
    IndividualId: 0,
    Status: 0,
    IsCheckPrinted: 0,
    PaymentOrderId: 0,
    AccountBankId: 0,
    PaymentDate: "",
    TotalAmount: 0,
    TempImputationCode: 0,
    CurrencyId: 0,
    ExchangeRate: 0.0,
    CheckNumber: 0,
    BankId: 0,
    AccountBankNumber: "",
}

var oCheckPaymentOrderModelAutomatic = {
    Id: 0,
    StatusCheckBookControl: 0,
    CheckBookControlId: 0,
    CourierName: "",
    CheckNumber: 0,
    AccountBankId: 0,
    IsCheckPrinted: 0,
    Status: 0,
    IndividualId: 0,
    BeneficiaryTypeId: 0,
    BankId: 0,
    AccountBankNumber: "",
    PaymentOrdersItems: []
}


/*---------------------------------------------------------------------------------------------------------------------------------------------------*/
/*                                                      DEFINICIÓN DE FUNCIONES                                                                      */
/*---------------------------------------------------------------------------------------------------------------------------------------------------*/

if ($("#ViewBagBranchDisableChecKAutomatic").val() == "1") {
    setTimeout(function () {
        $("#BranchCheckAutomatic").attr("disabled", "disabled");
    }, 300);
}
else {
    $("#BranchCheckAutomatic").removeAttr("disabled");
}

setTimeout(function () {
    GetBanksCheckAutomatic();
}, 2000);


///////////////////////////////////////////////////
//  Limpia Objeto   oCheckPaymentOrderModelAutomatic     //
//////////////////////////////////////////////////
function setDataEmptyAutomaticAssigment() {
    oCheckPaymentOrderModelAutomatic = {
        Id: 0,
        StatusCheckBookControl: 0,
        CheckBookControlId: 0,
        CourierName: "",
        CheckNumber: 0,
        AccountBankId: 0,
        IsCheckPrinted: 0,
        Status: 0,
        IndividualId: 0,
        BeneficiaryTypeId: 0,
        BankId: 0,
        AccountBankNumber: "",
        PaymentOrdersItems: []
    }
}


///////////////////////////////////////////////////
//  Seleciona items de grilla                   //
//////////////////////////////////////////////////
function SetGridValueCheckAutomatic(operationNumber, associatedNumber) {
    
    var rows = $("#PaymentOrdersCheckAutomatic").UifDataTable("getData");

    for (var j in rows) {
        var item = rows[j];

        if (item.TempPaymentOrderId == operationNumber) {
            item.NumberAssociate = associatedNumber;
            var result = $('#PaymentOrdersCheckAutomatic').UifDataTable('getRowIndex', { label: 'TempPaymentOrderId', values: [operationNumber] })
            $("#PaymentOrdersCheckAutomatic").UifDataTable('editRow', item, result[0].index);
            var value = { label: "TempPaymentOrderId", values: [operationNumber] }
            if (associatedNumber == "") {
                $("#PaymentOrdersCheckAutomatic").UifDataTable('setUnselect', value);
            }
            else {
                 $("#PaymentOrdersCheckAutomatic").UifDataTable('setSelect', value);
            }
        }
    }
}


/////////////////////////////////////////////////////////
//  Carga datos en  Objeto  oCheckPaymentOrderModelAutomatic    //
////////////////////////////////////////////////////////
function setDataAutomaticAssigmentCheck() {

    oCheckPaymentOrderModelAutomatic.AccountBankId = parseInt($("#AccountNumberCheckAutomatic").val());
    oCheckPaymentOrderModelAutomatic.CheckBookControlId = parseInt(checkBookControlId);
    oCheckPaymentOrderModelAutomatic.CheckNumber = parseInt($("#CheckAsigment").val());
    oCheckPaymentOrderModelAutomatic.IsCheckPrinted = 0;
    oCheckPaymentOrderModelAutomatic.Status = 1;
    oCheckPaymentOrderModelAutomatic.StatusCheckBookControl = 1;
    oCheckPaymentOrderModelAutomatic.BankId = parseInt($("#BankName").val());
    oCheckPaymentOrderModelAutomatic.AccountBankNumber = $("#AccountNumberCheckAutomatic option:selected").text();

    var rowids = $("#PaymentOrdersCheckAutomatic").UifDataTable("getSelected");

    if (rowids != null) {
        for (var i in rowids) {

          if (rowids[i].NumberAssociate >= 1) {
            oPaymentOrderCheckModelAutomatic = {
                CheckBookControlId: 0,
                BeneficiaryTypeId: 0,
                IndividualId: 0,
                Status: 0,
                IsCheckPrinted: 0,
                PaymentOrderId: 0,
                AccountBankId: 0,
                PaymentDate: "",
                TotalAmount: 0,
                TempImputationCode: 0,
                CurrencyId: 0,
                ExchangeRate: 0.0,
                CheckNumber: 0,
                BankId: 0,
                AccountBankNumber: "",
            };
        
            oPaymentOrderCheckModelAutomatic.PaymentOrderId = parseInt(rowids[i].TempPaymentOrderId);
            oPaymentOrderCheckModelAutomatic.BeneficiaryTypeId = rowids[i].PersonTypeId,
            oPaymentOrderCheckModelAutomatic.IndividualId = rowids[i].IndividualId;

            oCheckPaymentOrderModelAutomatic.PaymentOrdersItems.push(oPaymentOrderCheckModelAutomatic);

          }
        }
    }
    return oCheckPaymentOrderModelAutomatic;
}

///////////////////////////////////////////////////
//  Limpia campos del formulario                //
//////////////////////////////////////////////////
function setCleanAllFields() {

    $("#BranchCheckAutomatic").val('');
    $("#BankName").val('');
    setCleanFields();
    $("#Accept").attr("disabled", false);
}

///////////////////////////////////////////////////
//  Limpia campos y variables                   //
//////////////////////////////////////////////////
function setCleanFields() {

    $("#PaymentOrdersCheckAutomatic").dataTable().fnClearTable();
    $("#AccountNumberCheckAutomatic").val('');
    $("#OriginPayment").val('');
    $("#DatePayment").val('');
    $("#CheckAsigment").val('');
    $("#Currency").val('');
    $("#alertCheckAutomatic").UifAlert('hide');

    numberAssociated = 0;
    checkBookControlId = 0;
    currencyIdCheckAutomatic = -1;
    individualIdCheckAutomatic = -1;
    countSelect = 0;
}

///////////////////////////////////////////////////
//  Muestra mensaje de confirmación              //
//////////////////////////////////////////////////
function showConfirmCheckAutomatic() {
    $.UifDialog('confirm', { 'message': Resources.CancelApplicationMessage, 'title': Resources.MenuAutomaticAssignment },
        function (result) {
        if (result) {
            setCleanAllFields();
        }
    });
};

/*--------------------------------------------------------------------------------------------------------------------------------------------------*/
//                                                                  ACCIONES / EVENTOS
/*--------------------------------------------------------------------------------------------------------------------------------------------------*/

/////////////////////////////////////////
//  Dropdown Sucursal
/////////////////////////////////////////
$('#BranchCheckAutomatic').on('itemSelected', function (event, selectedItem) {
    $("#alertCheckAutomatic").UifAlert('hide');
    if (selectedItem.Id > 0) {
        var controller = ACC_ROOT + "CheckControl/GetBankBranchsByBranchId?branchId=" + selectedItem.Id;
        $("#BankName").UifSelect({ source: controller });
    }
    setCleanFields();
});

/////////////////////////////////////////
//  Dropdown Banco
/////////////////////////////////////////
$('#BankName').on('itemSelected', function (event, selectedItem) {
    
    $("#alertCheckAutomatic").UifAlert('hide');
    if ($("#BankName").val() != "") {
        var controller = ACC_ROOT + "CheckControl/GetAccountByBankIdByBranchId?bankId=" + selectedItem.Id + "&branchId=" + $('#BranchCheckAutomatic').val();
        $("#AccountNumberCheckAutomatic").UifSelect({ source: controller });
    }

    $("#PaymentOrdersCheckAutomatic").dataTable().fnClearTable();
});

/////////////////////////////////////////
//  Consulta Número de Cuentas
/////////////////////////////////////////
$('#AccountNumberCheckAutomatic').on('itemSelected', function (event, selectedItem) {
    
    $("#alertCheckAutomatic").UifAlert('hide');

    $.ajax({
        url: ACC_ROOT + "CheckControl/GetAccountBankByAccountBankId",
        data: { "accountBankId": $("#AccountNumberCheckAutomatic").val() },
        success: function (data) {

            if (data.length > 0) {
                $("#Currency").val(data[0].CurrencyName);
                currencyIdCheckAutomatic = data[0].CurrencyId;
            } else {
                $("#Currency").val("");
                currencyIdCheckAutomatic = -1;
            }
        }
    });


    $.ajax({
        url: ACC_ROOT + "CheckControl/GetCheckBookByAccountBankId",
        data: { "accountBankId": $("#AccountNumberCheckAutomatic").val(), "isAutomatic": 1 },
        success: function (data) {
            
            if (data.rows.length > 0) {
                if (data.rows[0].Status == "1") {

                    checkBookActive = true;
                    if (data.rows[0].CheckAssigment > 0) {

                        $("#CheckAsigment").val(data.rows[0].CheckAssigment);
                        checkBookControlId = data.rows[0].Id;
                        $("#alertCheckAutomatic").UifAlert('hide');
                        $("#Accept").attr("disabled", false);
                    } else {

                        $("#Accept").attr("disabled", true);

                        $("#alertCheckAutomatic").UifAlert('show', Resources.ControlAutomaticCheckTop, "warning");
                    }

                } else {
                    $("#alertCheckAutomatic").UifAlert('show', Resources.WarningCheckingActive, "warning");
                    $("#CheckAsigment").val("");
                    checkBookControlId = 0;
                }
            }
            else {
                checkBookActive = false;
                $("#alertCheckAutomatic").UifAlert('show', Resources.WarningCheckingActive, "warning");
                $("#CheckAsigment").val("");
                checkBookControlId = 0;
                $("#Accept").attr("disabled", true);
            }
        }
    });

    
    $("#PaymentOrdersCheckAutomatic").dataTable().fnClearTable();

});//fin $('#AccountNumber')

/////////////////////////////////////////
//  Botón de Búsqueda                  //
/////////////////////////////////////////
$("#SearchCheckAutomatic").click(function () {
    refreshPaymentOrdersCheck();
});

function refreshPaymentOrdersCheck() {
    $("#alertCheckAutomatic").UifAlert('hide');
    $("#CheckAutomaticAssignmentForm").validate();
    if ($("#CheckAutomaticAssignmentForm").valid()) {

        $("#PaymentOrdersCheckAutomatic").UifDataTable();
        var controller = ACC_ROOT + "CheckControl/GetPaymentOrderByPaymentSourceIdPayByDate?paymentSourceId=" + $('#OriginPayment').val() +
                    "&payDate=" + $('#DatePayment').val() + "&currencyId=" + currencyIdCheckAutomatic;
        $("#PaymentOrdersCheckAutomatic").UifDataTable({ source: controller });
    }
}





/////////////////////////////////////////
//  Botón de Asociar                    //
/////////////////////////////////////////
$("#Associate").click(function () {
    
    $("#alertCheckAutomatic").UifAlert('hide');
    var rowid = $("#PaymentOrdersCheckAutomatic").UifDataTable("getSelected");
        
    if (rowid == null) {
        $("#alertCheckAutomatic").UifAlert('show', Resources.SelectOneItem, "warning");
        return;
    }

    for (var i in rowid) {
        if (rowid[i].NumberAssociate != "") {
            $("#alertCheckAutomatic").UifAlert('show', Resources.DuplicateRecordsAssociate, "warning");
            individualIdCheckAutomatic = rowid[i].IndividualId;
        }
        else {
            if (individualIdCheckAutomatic == rowid[i].IndividualId || individualIdCheckAutomatic == -1) {
                numberAssociated = numberAssociated + 1;
                individualIdCheckAutomatic = rowid[i].IndividualId;
                rowid[i].NumberAssociate = numberAssociated;
                SetGridValueCheckAutomatic(rowid[i].TempPaymentOrderId, numberAssociated);
            } else {
                if (individualIdCheckAutomatic == -1) {
                    individualIdCheckAutomatic = rowid[i].IndividualId;
                }
                $("#alertCheckAutomatic").UifAlert('show', Resources.YouCanNotAsocciateFromOther, "warning");

                var value = {
                    label: 'TempPaymentOrderId',//El campo por el cual se desea buscar
                    values: [rowid[i].TempPaymentOrderId] //Los valores a deseleccionar.
                };

                $("#PaymentOrdersCheckAutomatic").UifDataTable('setUnselect', value)
            }
        }
    }
});

/////////////////////////////////////////
//  Botón  Liberar                     //
/////////////////////////////////////////
$("#Liberate").click(function () {
    $("#alertCheckAutomatic").UifAlert('hide');

    var rowid = $("#PaymentOrdersCheckAutomatic").UifDataTable("getSelected");

    if (rowid == null) return;

    for (var i in rowid) {

        if (rowid[i].NumberAssociate != null) {
            numberAssociated = numberAssociated - 1;
            rowid[i].NumberAssociate = "";
            SetGridValueCheckAutomatic(rowid[i].TempPaymentOrderId, "");
            if (numberAssociated == -1 || numberAssociated == 0) {
                numberAssociated = 0;
                individualIdCheckAutomatic = -1;
            }
        }
    }
});


function maxAssociated() {

    var rowid = $("#PaymentOrdersCheckAutomatic").UifDataTable("getSelected");

    if (rowid == null) return;
    for (var i in rowid) {

        if (rowid[i].NumberAssociate != null) {
            numberAssociated = numberAssociated - 1;
            rowid[i].NumberAssociate = "";
            SetGridValueCheckAutomatic(rowid[i].TempPaymentOrderId, "");
            if (numberAssociated == -1 || numberAssociated == 0) {
                numberAssociated = 0;
                individualIdCheckAutomatic = -1;
            }
        }
    }

}


/////////////////////////////////////////
//  Botón  Aceptar                     //
/////////////////////////////////////////
$("#Accept").click(function () {
    $("#alertCheckAutomatic").UifAlert('hide');

    if (checkBookActive) {
        var resultGrid = 0;
        var rowid = $("#PaymentOrdersCheckAutomatic").UifDataTable("getSelected");

        for (var i in rowid) {
            if (rowid[i].NumberAssociate > 0) {
                resultGrid = resultGrid + 1;
            }
        }

        if (rowid == null || resultGrid == 0) {
            $("#alertCheckAutomatic").UifAlert('show', Resources.ValidationAssociatedPaymentOrder, "warning");
            resultGrid = 0;
            return;
        }

        //si existen registros seleccionados, procedo a asignar cheques.
        if (resultGrid > 0) {
            lockScreen();
            $.ajax({
                type: "POST",
                url: ACC_ROOT + "CheckControl/SaveCheckPaymentOrder",
                data: JSON.stringify({ "checkPaymentOrderModel": setDataAutomaticAssigmentCheck(), "typeAssignment": 1 }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {

                    //EXCEPCION ROLLBACK
                    if (data.success == false) {

                        $("#alertCheckAutomatic").UifAlert('show', data.result, "danger");

                    }
                    else {

                        if (data.isEnabledGeneralLedger == false) {
                            $("#accountingLabelDiv").hide();
                            $("#accountingMessageDiv").hide();
                        } else {
                            $("#accountingLabelDiv").show();
                            $("#accountingMessageDiv").show();
                        }                        
                        $("#alertCheckAutomatic").UifAlert('show', data.PaymentOrderMessage, "success");
                    }
                    unlockScreen();
                }
            });
            setDataEmptyAutomaticAssigment();
            setCleanAllFields();
        }

    } else {
        $("#alertCheckAutomatic").UifAlert('show', Resources.WarningCheckingActive, "warning");
    }
});

/////////////////////////////////////////
//  Botón  Cancelar                     //
/////////////////////////////////////////
$("#Cancel").click(function () {
    $("#alertCheckAutomatic").UifAlert('hide');
    showConfirmCheckAutomatic();
    $("#BranchCheckAutomatic").focus();
});



//Valida que no ingresen una fecha invalida.
$("#DatePayment").blur(function () {
    $("#alertCheckAutomatic").UifAlert('hide');
    if ($("#DatePayment").val() != '') {
        if (IsDate($("#DatePayment").val()) == true) {
            if (CompareDates($("#DatePayment").val(), getCurrentDate()) == 2) {
                $("#DatePayment").val(getCurrentDate);
            }
        }
        else {
            $("#alertCheckAutomatic").UifAlert('show', Resources.InvalidDates, "warning");
            setTimeout(function () { $("#alertCheckAutomatic").UifAlert('hide'); }, 3000);
            $("#DatePayment").val("");
        }
    }

});

/////////////////////////////////////////
//  Botón  Limpiar                     //
/////////////////////////////////////////
$("#CleanSearchCheckAutomatic").click(function () {
    setCleanAllFields();
});

function GetBanksCheckAutomatic() {
    if ($('#BranchCheckAutomatic').val() > 0) {
        var controller = ACC_ROOT + "CheckControl/GetBankBranchsByBranchId?branchId=" + $('#BranchCheckAutomatic').val();
        $("#BankName").UifSelect({ source: controller });
    }
};