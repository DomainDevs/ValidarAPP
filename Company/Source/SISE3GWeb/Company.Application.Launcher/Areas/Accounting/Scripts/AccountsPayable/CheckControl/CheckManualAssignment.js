/*----------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*                                               DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                                                 */
/*----------------------------------------------------------------------------------------------------------------------------------------------------------*/

var Alert = 0;
var NumerrorsManual = 0;
var currencyIdManual = -1;
var numberAssociatedManual = 0;
var checkBookControlIdManual = 0;
var individualIdManual = -1;
var checkBookActiveManual = "Disabled";
var checkToManual = -1;
var beneficiaryTypeId = 0;
var beneficiaryType = "";
var paymentOrderId = -1;
var validatePayment = false;
var checkFrom = 0;
var resultValidateCheckRange = false;

var oTblChecks = {
    PaymentOrderCheck: []
};

var oCheckPaymentOrderModelManual = {
    Id: 0,
    StatusCheckBookControl: 0,
    IndividualId: 0,
    BeneficiaryTypeId: 0,
    PaymentOrdersItems: []
};

var oPaymentOrderCheckModelManual = {
    Status: 0,
    IsCheckPrinted: 0,
    PaymentOrderId: 0,
    AccountBankId: 0,
    CheckNumber: 0,
    CheckBookControlId: 0,
    BankId: 0,
    AccountBankNumber: ""
};

var oPaymentOrderCheckModelAdd = {
    BeneficiaryTypeId: 0,
    BeneficiaryType: 0,
    BeneficiaryName: "",
    EstimatedPaymentDate: "",
    Amount: 0,
    CheckNumber: 0,
    BankAccountCurrent: "",
    IndividualId: 0,
    AccountBankId: 0,
    checkBookControlIdManual: 0,
    BankId: 0,
    AccountBankNumber: 0,
};




/*---------------------------------------------------------------------------------------------------------------------------------------------------*/
/*                                                      DEFINICIÓN DE FUNCIONES                                                                      */
/*---------------------------------------------------------------------------------------------------------------------------------------------------*/

if ($("#ViewBagBranchDisableCheckManual").val() == "1") {
    setTimeout(function () {
        $("#BranchManualAssignment").attr("disabled", "disabled");
    }, 300);
}
else {
    $("#BranchManualAssignment").removeAttr("disabled");
}

setTimeout(function () {
    GetBanksManual();
}, 2000);


$(document).ready(function () {

    /*--------------------------------------------------------------------------------------------------------------------------------------------------*/
    //                                                                  ACCIONES / EVENTOS
    /*--------------------------------------------------------------------------------------------------------------------------------------------------*/

    ///////////////////////////////////////////////////////
    /// Dropdown Sucursal                               ///
    ///////////////////////////////////////////////////////
    $('#BranchManualAssignment').on('itemSelected', function (event, selectedItem) {
        $("#alertManualAssignment").UifAlert('hide');
        if (selectedItem.Id > 0) {
            var controller = ACC_ROOT + "CheckControl/GetBankBranchsByBranchId?branchId=" + $('#BranchManualAssignment').val();
            $("#NameBankManualAssignment").UifSelect({ source: controller });
        }
    });

    ///////////////////////////////////////////////////////
    /// Dropdown Bancos                               ///
    ///////////////////////////////////////////////////////
    $("#NameBankManualAssignment").on('itemSelected', function (event, selectedItem) {
        $("#alertManualAssignment").UifAlert('hide');
        if ($("#NameBankManualAssignment").val() != "") {
            var controller = ACC_ROOT + "CheckControl/GetAccountByBankIdByBranchId?bankId=" + $("#NameBankManualAssignment").val()
            + "&branchId=" + $("#BranchManualAssignment").val();
            $("#AccountNumberManualAssignment").UifSelect({ source: controller });
        }
    });

    $("#AccountNumberManualAssignment").focus(function () {
        $("#alertManualAssignment").UifAlert('hide');
        checkBookActiveManual = "Enabled";
    });

    ///////////////////////////////////////////////////////
    /// Dropdown Cuenta                               ///
    ///////////////////////////////////////////////////////
    $("#AccountNumberManualAssignment").on('itemSelected', function (event, selectedItem) {
        $("#alertManualAssignment").UifAlert('hide');

        if ($("#AccountNumberManualAssignment").val() > 0) {

            $.ajax({
                url: ACC_ROOT + "CheckControl/GetCheckBookByAccountBankId",
                data: { "accountBankId": $("#AccountNumberManualAssignment").val(), "isAutomatic": 0 },
                success: function (data) {
                    if (data.rows.length > 0) {
                        if (data.rows[0].Status == "1") {
                            if (data.rows[0].CheckAssigment <= data.rows[0].CheckFrom) {
                                $("#lastCheckManualAssignment").val(data.rows[0].CheckAssigment);
                            } else {
                                $("#lastCheckManualAssignment").val(data.rows[0].CheckAssigment - 1);
                            }                           
                            $("#checkNumberManualAssignment").val(data.rows[0].CheckAssigment);
                            checkBookControlIdManual = data.rows[0].Id;
                            checkToManual = data.rows[0].CheckTo;
                            checkFrom = data.rows[0].CheckFrom;
                        } else {
                            $("#alertManualAssignment").UifAlert('show', Resources.WarningCheckingActive, "warning");
                            $("#lastCheckManualAssignment").val("");
                            checkBookControlIdManual = 0;
                        }

                    } else {
                        if (checkBookActiveManual == "Enabled") {
                            $("#alertManualAssignment").UifAlert('show', Resources.WarningCheckingActive, "warning");
                        }
                        $("#alertManualAssignment").UifAlert('show', Resources.WarningCheckingActive, "warning");
                        $("#lastCheckManualAssignment").val("");
                        checkBookControlIdManual = 0;
                    }
                }
            });

            $.ajax({
                url: ACC_ROOT + "CheckControl/GetAccountBankByAccountBankId",
                data: { "accountBankId": $("#AccountNumberManualAssignment").val() },
                success: function (data) {
                    if (data.length > 0) {
                        $("#currencyManualAssignment").val(data[0].CurrencyName);
                        currencyIdManual = data[0].CurrencyId;
                    } else {
                        $("#currencyManualAssignment").val("");
                        currencyIdManual = -1;
                    }
                }
            });
        }
    });

    ///////////////////////////////////////////////////
    //  Limpia campos y variables                   //
    //////////////////////////////////////////////////
    $("#CleanManualAssignment").click(function () {
        $("#alertManualAssignment").UifAlert('hide');
        cleanFields();
        $("#BranchManualAssignment").focus();
    });

    /////////////////////////////////////////
    //  Botón  Cancelar                    //
    /////////////////////////////////////////
    $("#CancelManualAssignment").click(function () {
        $("#alertManualAssignment").UifAlert('hide');
        showConfirmManual();
        $("#BranchManualAssignment").focus();
    });


    /////////////////////////////////////////
    //  Botón  Agregar                     //
    /////////////////////////////////////////
    $("#AddPaymentOrderManualAssignment").click(function () {
        $("#alertManualAssignment").UifAlert('hide');
        
        NumerrorsManual = 0;

        if ((NumerrorsManual == 0) || (NumerrorsManual == undefined)) {
            Alert = 0;
        } else {
            Alert = 1;
            NumerrorsManual = 0;
        }

        if (!$("#ManualAssignment").valid()) {
            return false;
        }
        else {

            if (Alert == 0) {
                
                var ids = $("#PaymentOrdersManualAssignment").UifDataTable("getData");
                if (ids.length > 0) {
                    for (var i in ids) {
                        if (ids[i].TempPaymentOrderId == $("#numberPaymentOrderManualAssignment").val() || ids[i].IndividualId != individualIdManual) {
                            validatePayment = true;
                            $("#alertManualAssignment").UifAlert('show', Resources.WarningPaymentRepeated, "warning");
                        }
                    }
                }
            }

            if (validatePayment == false) {

                validateCheckRange(parseInt($("#checkNumberManualAssignment").val()), checkFrom, checkToManual);

                setTimeout(function () {
                
                    if (resultValidateCheckRange) {

                        $("#alertManualAssignment").UifAlert('hide');
                        addRowPayment();
                    }
                    /* else {
                       $("#alertManualAssignment").UifAlert('show', Resources.CheckRangeOut, "warning"); //WarningCheckNumberValid
                    }*/

                }, 1000);

                validatePayment = false;
                $("#BranchManualAssignment").focus();
            }
            else {
                $("#alertManualAssignment").UifAlert('show', Resources.WarningPaymentRepeated, "warning");
            }
        }

    });


    /////////////////////////////////////////
    //  Botón  Aceptar                     //
    /////////////////////////////////////////
    $("#AcceptManualAssignment").click(function () {
        $.msg({ content: Resources.MessagePleaseWait, klass: 'white-on-black' });

        $("#alertManualAssignment").UifAlert('hide');
        var rowCount = $("#PaymentOrdersManualAssignment").UifDataTable("getData");

        if (rowCount.length > 0) {
            lockScreen();
            $.ajax({
                type: "POST",
                url: ACC_ROOT + "CheckControl/SaveCheckPaymentOrder",
                data: JSON.stringify({ "checkPaymentOrderModel": setDataManualAssigmentCheck(), "typeAssignment": 0 }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    //EXCEPCION ROLLBACK
                    if (data.success == false) {

                        setDataEmptyManualAssigment();
                        setCleanFieldsManual();
                        $("#alertManualAssignment").UifAlert('show', data.result, "danger");

                    }
                    else {
                        setDataEmptyManualAssigment();
                        setCleanFieldsManual();
                        $("#BranchManualAssignment").focus();
                        $("#alertManualAssignment").UifAlert('show', Resources.SaveCheckSucces + ' ' + data.PaymentOrderMessage, "success");
                    }
                    unlockScreen();
                }
            });
        } else {
            $("#alertManualAssignment").UifAlert('show', Resources.WarningInsertedPaymentOrder, "warning");
            return false;
        }
    });


    /////////////////////////////////////////////
    //  Busca datos acorde al número ingresado //
    ////////////////////////////////////////////
    $("#numberPaymentOrderManualAssignment").blur(function () {
        $("#alertManualAssignment").UifAlert('hide');

        var paymentOrderNum = 0;

        if ($("#numberPaymentOrderManualAssignment").val() != "") {
            paymentOrderNum = parseInt($("#numberPaymentOrderManualAssignment").val());
        }

        $.ajax({
            url: ACC_ROOT + "CheckControl/GetPaymentOrderByPaymentOrderId",
            data: { "paymentOrderId": paymentOrderNum, "currencyId": currencyIdManual },
            success: function (data) {
                if (data.rows.length > 0) {
                    $("#nameBeneficiaryManualAssignment").val(data.rows[0].BeneficiaryName);
                    $("#amountPaymentOrderManualAssignment").val(data.rows[0].Amount);
                    $("#estimatedPaymentDateManualAssignment").val(data.rows[0].EstimatedPaymentDate);
                    paymentOrderId = data.rows[0].TempPaymentOrderId;
                    beneficiaryTypeId = data.rows[0].PersonTypeId;
                    beneficiaryType = data.rows[0].BeneficiaryType;
                    individualIdManual = data.rows[0].IndividualId;
                    validatePayment = false;
                    var amountPaymentOrder = $("#amountPaymentOrderManualAssignment").val();
                    $("#amountPaymentOrderManualAssignment").val(FormatCurrency(FormatDecimal(amountPaymentOrder)));

                } else {
                    setTimeout(function () {
                        $("#nameBeneficiaryManualAssignment").val("");
                        $("#amountPaymentOrderManualAssignment").val("");
                        $("#estimatedPaymentDateManualAssignment").val("");
                        $("#numberPaymentOrderManualAssignment").val("");
                        validatePayment = false;
                        $("#alertManualAssignment").UifAlert('show', Resources.WarningProcessPaymentOrder, "warning");
                    }, 500);
                }
            }
        });
    });


    /////////////////////////////////////////////
    //  validación de  número ingresado        //
    ////////////////////////////////////////////
    $("#checkNumberManualAssignment").blur(function () {
        $("#alertManualAssignment").UifAlert('hide');

        if ($("#numberPaymentOrderManualAssignment").val() != "") {

            if ($("#checkNumberManualAssignment").val() > checkToManual) {
                $("#alertManualAssignment").UifAlert('show', Resources.CheckRangeOut, "warning");
                $("#checkNumberManualAssignment").val("");
            }
            if ($("#checkNumberManualAssignment").val() <= 0) {
                $("#alertManualAssignment").UifAlert('show', Resources.CheckRangeOut, "warning");
                $("#checkNumberManualAssignment").val("");
            }
        }
    });


    /////////////////////////////////////////////
    //  Da formato como moneda                 //
    ////////////////////////////////////////////
    $("#amountPaymentOrderManualAssignment").blur(function () {
        $("#alertManualAssignment").UifAlert('hide');
        var amountPaymentOrder = $("#amountPaymentOrderManualAssignment").val();
        $("#amountPaymentOrderManualAssignment").val(FormatCurrency(FormatDecimal(amountPaymentOrder)));
    });
});

///////////////////////////////////////////////////
//  Limpia Objeto   setDataEmptyManualAssigment //
//////////////////////////////////////////////////
function setDataEmptyManualAssigment() {
    oCheckPaymentOrderModelManual = {
        Id: 0,
        IsCheckPrinted: 0,
        Status: 0,
        StatusCheckBookControl: 0,
        CheckBookControlId: 0,
        PaymentOrdersItems: []
    };
}


///////////////////////////////////////////////////
//  Limpia Objeto   oPaymentOrderCheckModelAdd   //
//////////////////////////////////////////////////
function cleanObjectManual() {
    oPaymentOrderCheckModelAdd = {
        BeneficiaryTypeId: null,
        BeneficiaryType: null,
        BeneficiaryName: null,
        EstimatedPaymentDate: null,
        Amount: null,
        CheckNumber: null,
        BankAccountCurrent: null,
        IndividualId: null,
        AccountBankId: null,
        checkBookControlIdManual: null,
        BankId: null,
        AccountBankNumber: null,
    };

    oTblChecks = {
        PaymentOrderCheck: []
    };

}

//////////////////////////////////////////////////
//  Arma Objeto   oCheckPaymentOrderModelManual       //
//////////////////////////////////////////////////
function setDataManualAssigmentCheck() {

    oCheckPaymentOrderModelManual.StatusCheckBookControl = 1;
    oCheckPaymentOrderModelManual.IndividualId = individualIdManual;
    oCheckPaymentOrderModelManual.BeneficiaryTypeId = beneficiaryTypeId;

    oCheckPaymentOrderModelManual.BankId = $("#NameBankManualAssignment").val();
    oCheckPaymentOrderModelManual.AccountBankNumber = $("#AccountNumberManualAssignment option:selected").text();
   
   var ids = $("#PaymentOrdersManualAssignment").UifDataTable("getData");

    if (ids.length > 0) {
        for (var i in ids) {
            oPaymentOrderCheckModelManual = {
                PaymentOrderId: 0,
                AccountBankId: 0,
                CheckNumber: 0,
                IsCheckPrinted: 0,
                Status: 0,
                CheckBookControlId: 0,
                BankId: 0,
                AccountBankNumber: ""
            };

            oPaymentOrderCheckModelManual.CheckBookControlId = parseInt(ids[i].checkBookControlIdManual);
            oPaymentOrderCheckModelManual.IsCheckPrinted = 0;
            oPaymentOrderCheckModelManual.Status = 1;
            oPaymentOrderCheckModelManual.AccountBankId = parseInt(ids[i].AccountBankId);
            oPaymentOrderCheckModelManual.CheckNumber = parseInt(ids[i].CheckNumber);
            oPaymentOrderCheckModelManual.PaymentOrderId = parseInt(ids[i].TempPaymentOrderId);
            oPaymentOrderCheckModelManual.BankId = ids[i].BankId;
            oPaymentOrderCheckModelManual.AccountBankNumber = ids[i].AccountBankNumber;

            oCheckPaymentOrderModelManual.PaymentOrdersItems.push(oPaymentOrderCheckModelManual);
        }
    }
    return oCheckPaymentOrderModelManual;
}

///////////////////////////////////////////////////
//  Limpia campos y variables                   //
//////////////////////////////////////////////////
function cleanFields() {
    $("#BranchManualAssignment").val("");
    $("#NameBankManualAssignment").val("");
    $("#AccountNumberManualAssignment").val("");

    $("#lastCheckManualAssignment").val("");
    $("#currencyManualAssignment").val("");
    $("#numberPaymentOrderManualAssignment").val("");
    $("#nameBeneficiaryManualAssignment").val("");
    $("#amountPaymentOrderManualAssignment").val("");
    $("#estimatedPaymentDateManualAssignment").val("");
    $("#checkNumberManualAssignment").val("");

    numberAssociatedManual = 0;
    checkBookControlIdManual = 0;
    currencyIdManual = -1;
    individualIdManual = -1;
    beneficiaryTypeId = 0;
    beneficiaryType = "";
    paymentOrderId = -1;
    checkBookActiveManual = "Disabled";
    validatePayment = false;
    checkToManual = -1;
    $("#alertManualAssignment").UifAlert('hide');
}

///////////////////////////////////////////////////
//  Limpia campos del formulario de adicion      //
//////////////////////////////////////////////////
function cleanAddFields() {
    $("#BranchManualAssignment").val("");
    $("#NameBankManualAssignment").val("");
    $("#AccountNumberManualAssignment").val("");
    $("#NameBankManualAssignment").UifSelect();
    $("#AccountNumberManualAssignment").UifSelect();

    $("#lastCheckManualAssignment").val("");
    $("#currencyManualAssignment").val("");
    $("#numberPaymentOrderManualAssignment").val("");
    $("#nameBeneficiaryManualAssignment").val("");
    $("#amountPaymentOrderManualAssignment").val("");
    $("#estimatedPaymentDateManualAssignment").val("");
    $("#checkNumberManualAssignment").val("");

    numberAssociatedManual = 0;
    checkBookControlIdManual = 0;
    currencyIdManual = -1;
    paymentOrderId = -1;
    checkBookActiveManual = "Disabled";
    validatePayment = false;
    checkToManual = -1;
    $("#alertManualAssignment").UifAlert('hide');
}

///////////////////////////////////////////////////
//  Limpia campos del formulario      //
//////////////////////////////////////////////////
function setCleanFieldsManual() {
    $("#BranchManualAssignment").val("");
    $("#NameBankManualAssignment").val("");
    $("#AccountNumberManualAssignment").val("");

    $("#lastCheckManualAssignment").val("");
    $("#currencyManualAssignment").val("");
    $("#numberPaymentOrderManualAssignment").val("");
    $("#nameBeneficiaryManualAssignment").val("");
    $("#amountPaymentOrderManualAssignment").val("");
    $("#estimatedPaymentDateManualAssignment").val("");
    $("#checkNumberManualAssignment").val("");

    numberAssociatedManual = 0;
    checkBookControlIdManual = 0;
    currencyIdManual = -1;
    individualIdManual = -1;
    beneficiaryTypeId = 0;
    beneficiaryType = "";
    paymentOrderId = -1;
    checkBookActiveManual = "Disabled";
    checkToManual = -1;
    validatePayment = false;
    //$("#PaymentOrdersManualAssignment").dataTable().fnClearTable();
    $("#PaymentOrdersManualAssignment").UifDataTable('clear');
    $("#alertManualAssignment").UifAlert('hide');
}

//funcion para convertir una fecha de formato Json a fecha corta.
function parseJsonDateManual(jsonDateString) {
    var day;
    var month;
    var year;
    var isoDate;

    isoDate = new Date(jsonDateString);
    day = ("0" + isoDate.getDate()).slice(-2);
    month = ("0" + (isoDate.getMonth() + 1)).slice(-2);
    year = isoDate.getFullYear();

    return day + "/" + month + "/" + year;
}

///////////////////////////////////////////////////
//  Añade un registro en tabla           //
//////////////////////////////////////////////////
function addRowPayment() {

    if ($("#numberPaymentOrderManualAssignment").val() != "") {
        var k = $("#PaymentOrdersManualAssignment").UifDataTable("getData");
        k++;
        cleanObjectManual();
        oPaymentOrderCheckModelAdd.TempPaymentOrderId = paymentOrderId,
            oPaymentOrderCheckModelAdd.BeneficiaryTypeId = beneficiaryTypeId,
            oPaymentOrderCheckModelAdd.BeneficiaryType = beneficiaryType,
            oPaymentOrderCheckModelAdd.BeneficiaryName = $("#nameBeneficiaryManualAssignment").val(),
            oPaymentOrderCheckModelAdd.EstimatedPaymentDate = $("#estimatedPaymentDateManualAssignment").val(),
            oPaymentOrderCheckModelAdd.Amount = $("#amountPaymentOrderManualAssignment").val(),
            oPaymentOrderCheckModelAdd.CheckNumber = $("#checkNumberManualAssignment").val(),
            oPaymentOrderCheckModelAdd.BankAccountCurrent = $("#NameBankManualAssignment option:selected").text() + " - " + $("#AccountNumberManualAssignment option:selected").text(),
            oPaymentOrderCheckModelAdd.IndividualId = individualIdManual,
            oPaymentOrderCheckModelAdd.AccountBankId = $("#AccountNumberManualAssignment").val(),
            oPaymentOrderCheckModelAdd.checkBookControlIdManual = checkBookControlIdManual,
            oPaymentOrderCheckModelAdd.BankId = $("#NameBankManualAssignment").val(),
            oPaymentOrderCheckModelAdd.AccountBankNumber = $("#AccountNumberManualAssignment option:selected").text();
            oTblChecks.PaymentOrderCheck.push(oPaymentOrderCheckModelAdd);

        for (var i in oTblChecks) {
            $('#PaymentOrdersManualAssignment').UifDataTable('addRow', oTblChecks[i]);
            cleanAddFields();

        }
    }
}



function validateCheckRange(numberCheck, checkFrom, checkTo) {
    
    if (numberCheck >= checkFrom && numberCheck <= checkTo) {
        //valida si el nùmero de cheque ya fue registrado anteriormente
        
        $.ajax({
            url: ACC_ROOT + "CheckControl/ValidateCheckAlreadyRegistered",
            data: {"branchId": $("#BranchManualAssignment").val(),
                   "accountBankId": $("#AccountNumberManualAssignment").val(),
                   "checkNumber": $("#checkNumberManualAssignment").val()},
                success: function (data) {
                    
                    if (data) {
                        $("#alertManualAssignment").UifAlert('show', Resources.WarningCheckNumberValid, "warning"); 
                        resultValidateCheckRange = false;
                       
                    } else {
                        resultValidateCheckRange = true;
                    }
                }
        });
        
    } else {
        $("#alertManualAssignment").UifAlert('show', Resources.CheckRangeOut, "warning"); 
        resultValidateCheckRange = false;
    }
}

///////////////////////////////////////////////////
//  Muestra mensaje de confirmación              //
//////////////////////////////////////////////////
function showConfirmManual() {
    $.UifDialog('confirm', { 'message': Resources.CancelApplicationMessage, 'title': 'Asignación Manual' }, function (result) {
        if (result) {
            setCleanFieldsManual();
        }
    });
};


function GetBanksManual() {
    if ($('#BranchManualAssignment').val() > 0) {
        var controller = ACC_ROOT + "CheckControl/GetBankBranchsByBranchId?branchId=" + $('#BranchManualAssignment').val();
        $("#NameBankManualAssignment").UifSelect({ source: controller });
    }
};
