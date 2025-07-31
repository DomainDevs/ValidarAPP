/*-------------------------------------------------------------------------------------------------------------------------------------------*/
/*														DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS														*/
/*-------------------------------------------------------------------------------------------------------------------------------------------*/

var applyCollecId = $("#ViewBagApplyCollecId").val();
var applyAccountingDate = $("#ViewBagDateAccounting").val();

var timePaymentOrder = window.setInterval(validateCompanyPaymentOrder, 1300);
var timerLoadPaymentOrder = 0;

var isLoadPaymentOrder = false;
var operationId = 0;
var tempImputationId = 0;
var percentageDifference = 0;
var maxPercentage = 0;
var minPercentage = 0;
var rateExchangePo = 0;
var viewBagDefaultValue; 
var viewBagDefaultDescription;
var isMulticompany;
var accountingCompanyDefault;
var branchDefault;
var branchUserDefault;
var salePointBranchUserDefault;
var viewBagAmount;
var isSearchPaymentOrders;
var paymentOrderNumber;
var branchIdPaymentOrder;
var viewBagUserId;
var tempSearchId;
var errorNumber = 0;
var Alert = 0;
var IsMainSearchBills = 1;
var IsMainSearchPreliquidations = 0;
var IsBilling = 0;
var debitAmount = 0;
var beneficiaryIndividualId = 0;
var accountBankCode = "";
var dataPaymentOrderId = 0;
var userId;
var nameText;
var documentNumberText;

var oPaymentOrderGeneration = {
    PaymentOrderItemId: 0,
    AccountingDate: null,
    AccountBankId: 0,
    BranchId: 0,
    BranchPayId: 0,
    CompanyId: 0,
    EstimatedPaymentDate: null,
    PaymentDate: null,
    PaymentMethodId: 0,
    PaymentSourceId: 0,
    IndividualId: 0,
    PersonTypeId: 0,
    CurrencyId: 0,
    ExchangeRate: 0,
    PaymentIncomeAmount: 0,
    PaymentAmount: 0,
    PayTo: null,
    StatusId: 0,
    Observation: null
};

var oPaymentOrdersMovementsModel = {
    Id: 0,
    PaymentOrderNumber: null,
    BranchId: 0,
    BranchName: null,
    PaymentBranchId: 0,
    PaymentBranchName: null,
    CompanyId: 0,
    CompanyName: null,
    EstimatedPaymentDate: null,
    PaymentDate: null,
    UserId: 0,
    UserName: null,
    PaymentTypeId: 0,
    PaymentTypeName: null,
    CurrencyId: 0,
    CurrencyName: null,
    PaymentIncomeAmount: 0,
    PaymentAmount: 0,
    BeneficiaryDocNumber: null,
    BeneficiaryName: null,
    PayToName: null,
    MovementSummaryItems: []
};

var oMovementSummaryModel = {
    DescriptionMovementSumary: null,
    Debit: 0,
    Credit: 0
};

/* Pagos siniestros y varios */
var oClaimsPaymentRequestItem = {
    TempClaimPaymentCode: 0,
    TempImputationCode: 0,
    PaymentRequestCode: 0,
    ClaimCode: 0,
    BeneficiaryId: 0,
    CurrencyCode: 0,
    IncomeAmount: 0,
    ExchangeRate: 0,
    Amount: 0,
    RegistrationDate: null,
    EstimationDate: null,
    BussinessType: 0,
    RequestType: 0,
    PaymentNum: 0,
    PaymentExpirationDate: null,
    PaymentRequestNumber: 0
};

var oPaymentRequestItem = {
    TempPaymentCode: 0,
    TempImputationCode: 0,
    PaymentRequestCode: 0,
    BeneficiaryId: 0,
    CurrencyCode: 0,
    IncomeAmount: 0,
    ExchangeRate: 0,
    Amount: 0,
    RegistrationDate: null,
    EstimationDate: null,
    BussinessType: 0,
    PaymentNumber: 0,
    PaymentExpirationDate: null,
    PaymentRequestNumber: 0
};

$(document).ready(function () {

    setTimeout(function () {
        isSearchPaymentOrders = $("#ViewBagTempPaymentOrder").val();
        viewBagDefaultValue = $("#ViewBagDefaultValue").val();
        viewBagDefaultDescription = $("#ViewBagDefaultDescription").val();
        isMulticompany = $("#ViewBagParameterMulticompany").val();
        accountingCompanyDefault = $("#ViewBagAccountingCompanyDefault").val();
        branchDefault = $("#ViewBagBranchDefault").val();
        branchUserDefault = $("#ViewBagBranchUserDefault").val();
        salePointBranchUserDefault = $("#ViewBagSalePointBranchUserDefault").val();
        viewBagAmount = $("#ViewBagAmount").val();
        paymentOrderNumber = $("#ViewBagPaymentOrderNumber").val();
        branchIdPaymentOrder = $("#ViewBagBranchIdPaymentOrder").val();
        viewBagUserId = $("#ViewBagUserId").val();
        tempSearchId = $("#ViewBagTempSearchId").val();
        userId = viewBagUserId;
    }, 500);
    

    setTimeout(function () {

        if ($("#ViewBagImputationType").val() == 4) {

            if (isSearchPaymentOrders != undefined) {
                if (isSearchPaymentOrders == 0) {

                    setTimeout(function () {
                        SaveTempPaymentOrderZero();
                    }, 3000);
                }
                else {

                    lockScreen();
                    timerLoadPaymentOrder = window.setInterval(loadAllFieldsPaymentOrder, 2500);
                }
            }
            setTimeout(function () {
                isLoadPaymentOrder = true;
            }, 2500);
        }
    }, 700);

    if ($("#ViewBagBranchDisable").val() == "1") {
        setTimeout(function () {
            $("#PaymentOrderBranchSelect").attr("disabled", true);
        }, 300);
    }
    else {
        $("#PaymentOrderBranchSelect").attr("disabled", false);
    }

    //Boton Aceptar de modal
    $('#modalBankAccounts').find("#AcceptMovement").click(function () {
        var rowId = $("#BankAccounts").UifDataTable("getSelected");
        if (rowId != null) {
            accountBankCode = rowId[0].AccountBankCode;
            $('#modalBankAccounts').UifModal('hide');
        }
        else {
            $("#alertModal").UifAlert('show', Resources.SelectOneBankAccount, "warning");
            return;
        }
    });

    /*Modal ModalPremiums*/
    // Botón Aceptar
    $("#SaveSearchPoliciesButton").click(function () {
        if ($("#ViewBagImputationType").val() == "4") {
            var amount = 0;
            if (viewBagAmount != "") {
                amount = parseFloat(ClearFormatCurrency(viewBagAmount.replace("", ",")));
            }

            GetDebitsAndCreditsMovementTypes(tempImputationId, amount);

            SetDataPremiumReceivableEmpty();
            ClearFieldsSearch();
            HideFields();

            setTimeout(function () {
                SetTotalApplication();

                $("#TotalControl").val(FormatCurrency(FormatDecimal(
                    SetMainTotalControl(ClearFormatCurrency($("#PaymentOrderGenerationAmount").val().replace("", ",")),
                        TotalDebit(), TotalCredit(), 1))));
            }, 2000);

            $('#ModalPremiums').UifModal('hide');
        }
    });

    //$("#PaymentOrderGenerationAmount").ValidatorKey(ValidatorType.Number);
});

/*-------------------------------------------------------------------------------------------------------------------------------------------*/
//                                                      DEFINICIÓN DE FUNCIONES GLOBALES
/*-------------------------------------------------------------------------------------------------------------------------------------------*/

//////////////////////////////////////////////////////
/// Se refresca el listview resumen de movimientos ///
//////////////////////////////////////////////////////
function GetDebitsAndCreditsMovementTypes(tempImputationId, amount) {

    $("#listViewAplicationReceipt").UifListView({
        autoHeight: true,
        theme: 'dark',
        source: ACC_ROOT + "ReceiptApplication/GetDebitsAndCreditsMovementTypes?tempImputationId=" + tempImputationId + "&amount=" + amount,
        customDelete: true, customEdit: true, edit: true, delete: false, displayTemplate: "#display-aplication-receipt-template"
    });
}

//////////////////////////////////////////////////////////////
/// Obtiene el total de débitos del resumen de movimientos ///
//////////////////////////////////////////////////////////////
function TotalDebit() {
    var total = 0;

    var debits = $("#listViewAplicationReceipt").UifListView("getData");

    if (debits != null) {

        for (var j = 0; j < debits.length; j++) {
            var debitAmount = String(debits[j].Debits).replace("$", "").replace(/,/g, "").replace(" ", "");
            total += parseFloat(debitAmount);
        }
    }

    return total;
}

///////////////////////////////////////////////////////////////
/// Obtiene el total de créditos del resumen de movimientos ///
///////////////////////////////////////////////////////////////
function TotalCredit() {
    var total = 0;

    var credits = $("#listViewAplicationReceipt").UifListView("getData");

    if (credits != null) {

        for (var j = 0; j < credits.length; j++) {
            var creditAmount = String(credits[j].Credits).replace("$", "").replace(/,/g, "").replace(" ", "");
            total += parseFloat(creditAmount);
        }
    }

    return total;
}

///////////////////////////////////////////////////////////////////////
/// Setea el total de la aplicación = total recibo - para controlar ///
///////////////////////////////////////////////////////////////////////
function SetTotalControl() {

    if ($("#ReceiptAmount").val() == 'NaN' || $("#ReceiptAmount").val() == undefined) {
        $("#ReceiptAmount").val(0);
    }

    $("#TotalControl").val(SetMainTotalControl(parseFloat(ClearFormatCurrency($("#ReceiptAmount").val())),
                                               TotalDebit(), TotalCredit(), 1));

    var receiptAmount = $("#TotalControl").val();
    $("#TotalControl").val("$ " + NumberFormatDecimal(receiptAmount, "2", ".", ","));
}

///////////////////////////////////////////////////////////
// Setear el total de la listview resumen de movimientos //
///////////////////////////////////////////////////////////
function SetTotalApplication() {
    var totalCreditMovement = 0;
    var totalDebitMovement = 0;

    var summaries = $("#listViewAplicationReceipt").UifListView("getData");

    if (summaries != null) {

        for (var j = 0; j < summaries.length; j++) {
            var debitAmount = String(summaries[j].Debits).replace("$", "").replace(/,/g, "").replace(" ", "");
            totalDebitMovement += parseFloat(debitAmount);

            var creditAmount = String(summaries[j].Credits).replace("$", "").replace(/,/g, "").replace(" ", "");
            totalCreditMovement += parseFloat(creditAmount);
        }
    } else {
        $("#TotalDebit").text("");
        $("#TotalCredit").text("");
    }
    $("#TotalDebit").text("$ " + NumberFormatDecimal(totalDebitMovement, "2", ".", ","));
    $("#TotalCredit").text("$ " + NumberFormatDecimal(totalCreditMovement, "2", ".", ","));
}

function cleanPartials() {

    RefreshAgentMovements();
    RefreshCoinsuranceMovements();
    RefreshReinsuranceMovements();
    RefreshRequestVariousMovements();
    RefreshRequestClaimsMovements();
}

///////////////////////////////////////////////////////////////////////////////////////////////
// SaveTempPaymentOrderZero - guarda temporal de orden de pago al ingresar a página          //
///////////////////////////////////////////////////////////////////////////////////////////////
function SaveTempPaymentOrderZero() {
    $("#ReceiptAmount").val("0");
    //GRABA A TEMPORALES
    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "PaymentOrders/SaveTempPaymentOrder",
        data: { "paymentOrderId": 0 },
        success: function (dataPaymentOrder) {
            //CONSULTAR SI EXISTE TEMPORAL
            $.ajax({
                async: false,
                type: "POST",
                url: ACC_ROOT + "ReceiptApplication/GetTempImputationBySourceCode",
                data: { "imputationTypeId": parseInt($("#ViewBagImputationType").val()), "sourceCode": dataPaymentOrder.Id },
                success: function (data) {

                    if (data.Id == 0) {
                        $("#PaymentOrderCode").val(dataPaymentOrder.Id);

                        //GRABA A TEMPORALES
                        $.ajax({
                            async: false,
                            type: "POST",
                            url: ACC_ROOT + "ReceiptApplication/SaveTempImputation",
                            data: { "imputationTypeId": parseInt($("#ViewBagImputationType").val()), "sourceCode": dataPaymentOrder.Id },
                            success: function (dataImputation) {
                                if (dataImputation == -1) {
                                    $("#alertPaymentOrder").UifAlert('show', Resources.ErrorTransaction, "danger");
                                }
                                else {
                                    tempImputationId = dataImputation.Id;
                                    GetDebitsAndCreditsMovementTypes(tempImputationId, 0);
                                    setDataFieldsEmptyPaymentOrder();
                                    $("#ReceiptAmount").val("0");
                                }
                            }
                        });
                    } else {
                        //YA EXISTE EN TEMPORALES
                        tempImputationId = data.Id;
                        GetDebitsAndCreditsMovementTypes(tempImputationId, 0);
                        $("#PaymentOrderCode").val(dataPaymentOrder.Id);
                    }


                    if ($("#ViewBagShowPaymentOrderId").val() == 'true') {
                        dataPaymentOrderId = dataPaymentOrder.Id;
                        $("#globalTitle").html(Resources.TemporalPaymentOrder + " " + dataPaymentOrder.Id);
                    } else {
                        $("#globalTitle").html(Resources.TemporalPaymentOrder + " " + tempImputationId);
                    }
                    $("#ReceiptAmount").val("0");
                }
            });
        }
    });
}

///////////////////////////////////////////////////////////////////////////////////////////////
// CloseDialogPaymentOrder                                                                   //
///////////////////////////////////////////////////////////////////////////////////////////////
function CloseDialogPaymentOrder(message) {

    $.UifDialog('confirm', { 'message': message, 'title': Resources.PaymentOrder }, function (result) {
        if (result) {
            if (isSearchPaymentOrders == 0) {   //Si es nueva orden y no viene desde la busqueda
                if (TotalDebit() != 0 || TotalCredit() != 0) {

                    //ACTUALIZA ESTADO DE PAYMENT ORDER
                    $.ajax({
                        async: false,
                        type: "POST",
                        url: ACC_ROOT + "PaymentOrders/UpdatePaymentOrderStatus",
                        data: { "paymentOrderId": parseInt($("#PaymentOrderCode").val()), "comments": "Payment Order", "statusId": 2 },
                        success: function () {
                            setDataFieldsEmptyPaymentOrder();
                            if (isSearchPaymentOrders != 0) {
                                if (parseInt(tempSearchId) == 0) {
                                    //setTimeout(function () {
                                    location.href = $("#ViewBagPaymentOrderSearchLink").val();
                                    //}, 3000);
                                } else {
                                    //setTimeout(function () {
                                    location.href = $("#ViewBagMainTemporarySearchLink").val();
                                    //}, 3000);
                                }
                            } else {
                                setDataFieldsEmptyPaymentOrder();
                                cleanPartials();
                                //setTimeout(function () {
                                SetTotalApplication();
                                SetTotalControl();
                                $("#ReceiptAmount").val("0");
                                //}, 2000);
                            }

                        }
                    });
                }

                //No se limpia Uiview solo se limpia la cabecera
                setDataFieldsEmptyPaymentOrder();
                cleanPartials();
                setTimeout(function () {
                    SetTotalApplication();
                    SetTotalControl();
                    $("#ReceiptAmount").val("0");
                }, 2000);
            }

            if (isSearchPaymentOrders != 0) {
                if (parseInt(tempSearchId) == 0) {
                    setTimeout(function () {
                        location.href = $("#ViewBagPaymentOrderSearchLink").val();
                    }, 3000);
                } else {
                    setTimeout(function () {
                        location.href = $("#ViewBagMainTemporarySearchLink").val();
                    }, 3000);
                }
            } else {
                setDataFieldsEmptyPaymentOrder();
                $("#ReceiptAmount").val("0");
                $("#btnPaymentOrderBankAccount").hide();
                SaveTempPaymentOrderZero();
                setTimeout(function () {
                    SetTotalApplication();
                    cleanPartials();
                    GetDebitsAndCreditsMovementTypes(0, 0);
                }, 2500);
            }
            $('#modalBankAccounts').UifModal('hide');
            $("#PaymentTypeSelect").trigger('change');
        }
    });
}

///////////////////////////////////////////////////////////////////////////////////////////////
// SavePaymentOrderTemporalConfirmation                                                      //
///////////////////////////////////////////////////////////////////////////////////////////////
function SavePaymentOrderTemporalConfirmation(message) {
   
    $.UifDialog('confirm', { 'message': message, 'title': Resources.PaymentOrder }, function (result) {
        if (result) {

            if (isSearchPaymentOrders == 0 && parseInt(tempSearchId) == 0) {  //Cuando se realiza nueva orden de pago
                //Actualiza fecha y estado parcialmente aplicado 
           
                lockScreen();
                setTimeout(function () {                    
                    $.ajax({
                    async: false,
                    type: "POST",
                    url: ACC_ROOT + "PaymentOrders/UpdateTempPaymentOrder",
                    data: { "paymentOrderModel": SetDataPaymentOrderGeneration(2) },
                    success: function (dataPaymentOrder) {
                       
                        unlockScreen();
                        if (dataPaymentOrder.success == false) {

                            $("#alertPaymentOrder").UifAlert('show', dataPaymentOrder.result, "danger");
                        } else {

                            if (dataPaymentOrder > 0) {
                           
                                lockScreen();
                                setTimeout(function () {     

                                    //ACTUALIZA FECHA Y USUARIO DE TEMP-IMPUTATION. ACTUALIZA ESTADO, FECHA Y USUARIO DE ORDENES DE PAGO
                                    $.ajax({
                                        async: false,
                                        type: "POST",
                                        url: ACC_ROOT + "ReceiptApplication/UpdateReceiptApplication",
                                        data: {
                                            "imputationId": tempImputationId,                                            
                                            "imputationTypeId": parseInt($("#ViewBagImputationType").val()),                                       
                                            "sourceCode": parseInt($("#PaymentOrderCode").val()),
                                            "comments": "Payment Order", "statusId": 2
                                        },
                                        success: function () {
                                            unlockScreen();
                                            $("#alertPaymentOrder").UifAlert('show', Resources.TemporalSuccessfullySaved + " " + tempImputationId, 'success');

                                            SetDataPaymentOrderEmpty();

                                            if (isSearchPaymentOrders == 0) {
                                            } else {
                                                GetDebitsAndCreditsMovementTypes(0, 0);
                                            }

                                            setDataFieldsEmptyPaymentOrder();
                                            $("#ReceiptAmount").val("0");
                                            $("#btnPaymentOrderBankAccount").hide();

                                            setTimeout(function () {
                                                SetTotalApplication();
                                                cleanPartials();
                                            }, 2500);

                                            //DISPARA EVENTOS........TODO: Revisar cuando y si se habilita eventos
                                            //Estos Método del controlador no estan programados para enterprise
                                            operationId = dataPaymentOrder;
                                            if ($("#ViewBagEnabledEvents").val() == 'true') {
                                                $.ajax({
                                                    type: "POST",
                                                    url: ACC_ROOT + "Events/GetsEventNotification",
                                                    data: { "operationId": operationId },
                                                    success: function (datEvent) {
                                                        if (datEvent.records > 0) {
                                                            //esta forma no ya no se usa $("#dlgEvents").dialog("open");
                                                        }
                                                    }
                                                });
                                            }

                                            if (isSearchPaymentOrders != 0) {
                                                setTimeout(function () {
                                                    location.href = $("#ViewBagPaymentOrderSearchLink").val();
                                                }, 3000);
                                            } else {
                                                setDataFieldsEmptyPaymentOrder();
                                                $("#ReceiptAmount").val("0");
                                                $("#btnPaymentOrderBankAccount").hide();
                                                SaveTempPaymentOrderZero();
                                                setTimeout(function () {
                                                    SetTotalApplication();
                                                    cleanPartials();
                                                    GetDebitsAndCreditsMovementTypes(0, 0);
                                                }, 2500);
                                            }
                                        }
                                      });
                             }, 500);
                           }
                        }
                    }
                    });
                }, 500);
            } else {
                //Se actualiza OP real en caso de redirijirse desde la busqueda y en EE
                if (isSearchPaymentOrders != 0) {
                    if (parseInt(tempSearchId) == 0) {

                        lockScreen();
                        setTimeout(function () { 
                           
                            $.ajax({
                            async: false,
                            type: "POST",
                            url: ACC_ROOT + "PaymentOrders/UpdatePaymentOrder",
                            data: JSON.stringify({ "paymentOrderModel": SetDataPaymentOrderGeneration(2) }),
                            dataType: "json",
                            contentType: "application/json; charset=utf-8",
                            success:function (dataPaymentOrder) {

                                unlockScreen();
                                if (dataPaymentOrder.success == false) {

                                    $("#alertPaymentOrder").UifAlert('show', dataPaymentOrder.result, "danger");
                                } else {

                                    if (dataPaymentOrder > 0) {

                                        lockScreen();
                                        setTimeout(function () { 

                                        //ACTUALIZA FECHA Y USUARIO DE TEMP-IMPUTATION. ACTUALIZA ESTADO, FECHA Y USUARIO DE ORDENES DE PAGO
                                            $.ajax({
                                            async: false,
                                            type: "POST",
                                            url: ACC_ROOT + "ReceiptApplication/UpdateReceiptApplication",
                                            data: {
                                                "imputationId": tempImputationId,                                                
                                                "imputationTypeId": parseInt($("#ViewBagImputationType").val()),       
                                                "sourceCode": parseInt($("#PaymentOrderCode").val()),
                                                "comments": "Payment Order", "statusId": 2
                                            },
                                            success: function () {
                                                unlockScreen();
                                                $("#alertPaymentOrder").UifAlert('show', Resources.UpdateSucces + "  " + /* Resources.TemporalSuccessfullySaved + tempImputationId + ',  ' +*/ Resources.PaymentOrder + ': ' + $("#PaymentOrderCode").val(), 'success');
                                                
                                                SetDataPaymentOrderEmpty();

                                                if (isSearchPaymentOrders == 0) {
                                                } else {
                                                    GetDebitsAndCreditsMovementTypes(0, 0);
                                                }

                                                setDataFieldsEmptyPaymentOrder();
                                                $("#ReceiptAmount").val("0");
                                                $("#btnPaymentOrderBankAccount").hide();

                                                setTimeout(function () {
                                                    SetTotalApplication();
                                                    cleanPartials();
                                                }, 2500);

                                                //DISPARA EVENTOS........TODO: Revisar cuando y si se habilita eventos
                                                operationId = dataPaymentOrder;
                                                if ($("#ViewBagEnabledEvents").val() == 'true') {
                                                    $.ajax({
                                                        type: "POST",
                                                        url: ACC_ROOT + "Events/GetsEventNotification",
                                                        data: { "operationId": operationId },
                                                        success: function (datEvent) {
                                                            if (datEvent.records > 0) {
                                                                $("#dlgEvents").dialog("open");
                                                            }
                                                        }
                                                    });
                                                }

                                                if (isSearchPaymentOrders != 0) {
                                                    if (parseInt(tempSearchId) == 0) {
                                                        setTimeout(function () {
                                                            location.href = $("#ViewBagPaymentOrderSearchLink").val();
                                                        }, 6000);
                                                    } else {
                                                        setTimeout(function () {
                                                            location.href = $("#ViewBagMainTemporarySearchLink").val();
                                                        }, 6000);
                                                    }
                                                } else {

                                                    setDataFieldsEmptyPaymentOrder();
                                                    $("#ReceiptAmount").val("0");
                                                    $("#btnPaymentOrderBankAccount").hide();
                                                    SaveTempPaymentOrderZero();
                                                    setTimeout(function () {
                                                        SetTotalApplication();
                                                        cleanPartials();
                                                        GetDebitsAndCreditsMovementTypes(0, 0);
                                                    }, 2500);
                                                }
                                            }
                                            })
                                        }, 500);
                                    }
                                }
                            }
                          });
                        }, 500);

                    } else {
                       
                        lockScreen();
                        setTimeout(function () {   
                            $.ajax({
                            async: false,
                            type: "POST",
                            url: ACC_ROOT + "PaymentOrders/UpdateTempPaymentOrder",
                            data: JSON.stringify({ "paymentOrderModel": SetDataPaymentOrderGeneration(2) }),
                            dataType: "json",
                            contentType: "application/json; charset=utf-8",
                            success: function (dataPaymentOrder) {

                                unlockScreen();
                                if (dataPaymentOrder.success == false) {

                                    $("#alertPaymentOrder").UifAlert('show', dataPaymentOrder.result, "danger");
                                } else {

                                    if (dataPaymentOrder > 0) {
                                        lockScreen();
                                        setTimeout(function () {  
                                        //ACTUALIZA FECHA Y USUARIO DE TEMP-IMPUTATION. ACTUALIZA ESTADO, FECHA Y USUARIO DE ORDENES DE PAGO 
                                            $.ajax({
                                            async: false,
                                            type: "POST",
                                            url: ACC_ROOT + "ReceiptApplication/UpdateReceiptApplication",
                                            data: {"imputationId": tempImputationId, "imputationTypeId": parseInt($("#ViewBagImputationType").val()),     
                                                   "sourceCode": parseInt($("#PaymentOrderCode").val()), "comments": "Payment Order", "statusId": 2 },
                                            success: function () {

                                                unlockScreen();
                                 
                                                $("#alertPaymentOrder").UifAlert('show', Resources.TemporalSuccessfullySaved + tempImputationId , "success");
                                                
                                                SetDataPaymentOrderEmpty();

                                                if (isSearchPaymentOrders == 0) {
                                                } else {
                                                    GetDebitsAndCreditsMovementTypes(0, 0);
                                                }

                                                setDataFieldsEmptyPaymentOrder();
                                                $("#ReceiptAmount").val("0");
                                                $("#btnPaymentOrderBankAccount").hide();

                                                setTimeout(function () {
                                                    SetTotalApplication();
                                                    cleanPartials();
                                                }, 2500);

                                                //DISPARA EVENTOS........TODO: Revisar cuando y si se habilita eventos
                                                operationId = dataPaymentOrder;
                                                if ($("#ViewBagEnabledEvents").val() == 'true') {
                                                    $.ajax({
                                                        type: "POST",
                                                        url: ACC_ROOT + "Events/GetsEventNotification",
                                                        data: { "operationId": operationId },
                                                        success: function (datEvent) {
                                                            if (datEvent.records > 0) {
                                                                $("#dlgEvents").dialog("open");
                                                            }
                                                        }
                                                    });
                                                }

                                                if (isSearchPaymentOrders != 0) {
                                                    if (parseInt(tempSearchId) == 0) {
                                                        setTimeout(function () {
                                                            location.href = $("#ViewBagPaymentOrderSearchLink").val();
                                                        }, 6000);
                                                    } else {
                                                        setTimeout(function () {
                                                            location.href = $("#ViewBagMainTemporarySearchLink").val();
                                                        }, 6000);
                                                    }
                                                } else {
                                                    setDataFieldsEmptyPaymentOrder();
                                                    $("#ReceiptAmount").val("0");
                                                    $("#btnPaymentOrderBankAccount").hide();
                                                    SaveTempPaymentOrderZero();
                                                    setTimeout(function () {
                                                        SetTotalApplication();
                                                        cleanPartials();
                                                        GetDebitsAndCreditsMovementTypes(0, 0);
                                                    }, 2500);
                                                }
                                            }
                                          });
                                        }, 500);
                                    }
                                }
                              }
                            });
                        }, 500);
                    }
                }
            }
        }
    });
}

///////////////////////////////////////////////////////////////////////////////////////////////
// setDataFieldsEmptyPaymentOrder - Borra datos de orden de pago                             //
///////////////////////////////////////////////////////////////////////////////////////////////
function setDataFieldsEmptyPaymentOrder() {
    
    $("#PaymentOrderBranchSelect").val($("#ViewBagBranchDefault").val());//pone la sucursal de dafult
    if (isMulticompany == 1) {
        $("#PaymentCompanySelect").val("");
    }
    $("#BeneficiaryTypeSelect").val("");
    $("#PaymentTypeSelect").val("");

    var defaultCurrencyCode = $("#DefaultCurrencyCode").val()
    $("#PaymentOrderCurrencySelect").UifSelect("setSelected", defaultCurrencyCode);
    $("#PaymentOrderCurrencySelect").trigger("change");

    $("#BranchOfPaymentSelect").val("");
    $("#OriginOfPaymentSelect").val("");
    $("#BeneficiaryDocumentNumberPaymentOrder").val("");
    beneficiaryIndividualId = 0;
    $("#txtDatePayment").val("");
    $("#PaymentOrderPayableTo").val("");
    $("#PaymentOrderGenerationDate").val("");
    $("#PaymentOrderGenerationAmount").val("");
    $("#PaymentOrderIncomeAmount").val("");
    $("#ReceiptAmount").val("");
    $("#TotalControl").val("");
    $("#BeneficiaryNamePaymentOrder").val("");
    $("#Observation").val("");    

    $("#PaymentTypeSelect").trigger('change');
    $("#BeneficiaryTypeSelect").trigger('change');
    cleanAutocompletes('SearchSuppliers');
    cleanAutocompletes('SearchInsured');
    cleanAutocompletes('SearchCoinsurance');
    cleanAutocompletes('SearchPerson');
    cleanAutocompletes('SearchAgent');
    cleanAutocompletes('SearchEmployee');
    cleanAutocompletes('SearchReinsurer');

    if (isSearchPaymentOrders != "0") {
        GetDebitsAndCreditsMovementTypes(tempImputationId, 0);
    }
}

///////////////////////////////////////////////////////////////////////////////////////////////
// SetDataPaymentOrder - Carga datos de ordenes de pago                                      //
///////////////////////////////////////////////////////////////////////////////////////////////
function SetDataPaymentOrderGeneration(paymentOrderStatusId) {
    SetDataPaymentOrderEmpty();
    
    if (accountBankCode != "") {
        oPaymentOrderGeneration.AccountBankId = accountBankCode;
    }
    else {
        oPaymentOrderGeneration.AccountBankId = -1;
    }

    
    oPaymentOrderGeneration.AccountingDate = $("#AccountingDate").val();        
    oPaymentOrderGeneration.PaymentAmount = ReplaceDecimalPoint(ClearFormatCurrency($("#PaymentOrderGenerationAmount").val()).replace(",", ".")); 
    oPaymentOrderGeneration.BranchId = $("#PaymentOrderBranchSelect").val();
    oPaymentOrderGeneration.BranchPayId = $("#BranchOfPaymentSelect").val();
    oPaymentOrderGeneration.CompanyId = $("#PaymentCompanySelect").val();
    oPaymentOrderGeneration.CurrencyId = $("#PaymentOrderCurrencySelect").val();      
    oPaymentOrderGeneration.ExchangeRate = ReplaceDecimalPoint(ClearFormatCurrency($("#PaymentOrderExchangeType").val()).replace(",", "."));
    oPaymentOrderGeneration.PaymentIncomeAmount = ReplaceDecimalPoint(ClearFormatCurrency($("#PaymentOrderIncomeAmount").val()).replace(",", "."));
    oPaymentOrderGeneration.IndividualId = beneficiaryIndividualId;
    oPaymentOrderGeneration.EstimatedPaymentDate = $("#PaymentOrderGenerationDate").val().substring(0, 10);
    oPaymentOrderGeneration.PaymentMethodId = $("#PaymentTypeSelect").val();
    oPaymentOrderGeneration.PaymentOrderItemId = $("#PaymentOrderCode").val();
    oPaymentOrderGeneration.PaymentSourceId = $("#OriginOfPaymentSelect").val();
    oPaymentOrderGeneration.PayTo = $("#PaymentOrderPayableTo").val();
    oPaymentOrderGeneration.PersonTypeId = $("#BeneficiaryTypeSelect").val();
    oPaymentOrderGeneration.StatusId = paymentOrderStatusId;
    oPaymentOrderGeneration.Observation = $("#Observation").val();

    if ($("#PaymentTypeSelect").val() == $("#ViewBagParamPaymentMethodTransfer").val()) {
        oPaymentOrderGeneration.AccountBankId = accountBankCode;
    }
    else {
        oPaymentOrderGeneration.AccountBankId = -1;
    }

    return oPaymentOrderGeneration;
}

///////////////////////////////////////////////////////////////////////////////////////////////
// SetDataPaymentOrderEmpty - Borra variables                                                //
///////////////////////////////////////////////////////////////////////////////////////////////
function SetDataPaymentOrderEmpty() {
    oPaymentOrderGeneration = {
        PaymentOrderItemId: 0,
        AccountingDate: null,
        AccountBankId: 0,
        BranchId: 0,
        BranchPayId: 0,
        CompanyId: 0,
        EstimatedPaymentDate: null,
        PaymentDate: null,
        PaymentMethodId: 0,
        PaymentSourceId: 0,
        IndividualId: 0,
        PersonTypeId: 0,
        CurrencyId: 0,
        ExchangeRate: 0,
        PaymentIncomeAmount: 0,
        PaymentAmount: 0,
        PayTo: null,
        StatusId: 0,
        Observation: null
    };
}

function CleanGlobalVariables() {
    tempImputationId = null;
}

///////////////////////////////////////////////////////////////////////////////////////////////
// loadPaymentOrdersReport - Carga el reporte y lo mantiene en una sesion en el controlador  //
///////////////////////////////////////////////////////////////////////////////////////////////
function loadPaymentOrdersReport(paymentOrderId, msg, nameDialog) {

        $.ajax({
            type: "POST",
            async: false,
            url: ACC_ROOT + "Report/LoadPaymentOrdersReport",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ "paymentOrdersMovementsModel": setDataReportPaymentOrders(paymentOrderId) }),
            success: function (data) {
                unlockScreen();
                if (parseInt(data) > 0) {
                    confirmDialogPaymentOrdersPrintReport(msg, Resources.PaymentOrders, true, nameDialog);
                }
            }
        });
}

///////////////////////////////////////////////////////////////////////////////////////////////
// confirmDialogPaymentOrdersPrintReport - Muestra un diálogo de Si o No imprime el reporte  //
///////////////////////////////////////////////////////////////////////////////////////////////
function confirmDialogPaymentOrdersPrintReport(messageIn, title, isNew, nameDialog) {
    $.UifDialog('confirm', { 'message': messageIn, 'title': title }, function (result) {
        if (result) {
            showPaymentOrdersReport();
            if (isSearchPaymentOrders != 0) {
                if (parseInt(tempSearchId) == 0) {
                    setTimeout(function () {
                        location.href = $("#ViewBagPaymentOrderSearchLink").val();
                    }, 3000);
                } else {
                    setTimeout(function () {
                        location.href = $("#ViewBagMainTemporarySearchLink").val();
                    }, 3000);
                }
            }

        } else {

            if (isSearchPaymentOrders != 0) {
                if (parseInt(tempSearchId) == 0) {
                    setTimeout(function () {
                        location.href = $("#ViewBagPaymentOrderSearchLink").val();
                    }, 3000);
                } else {
                    setTimeout(function () {
                        location.href = $("#ViewBagMainTemporarySearchLink").val();
                    }, 3000);
                }
            } else {
                setDataFieldsEmptyPaymentOrder();
                $("#ReceiptAmount").val("0");
                $("#btnPaymentOrderBankAccount").hide();
                SaveTempPaymentOrderZero();
                setTimeout(function () {
                    SetTotalApplication();
                    cleanPartials();
                    GetDebitsAndCreditsMovementTypes(0, 0);
                }, 2500);

            }
        }
    });
}

///////////////////////////////////////////////////////////////////////////////////
// showPaymentOrdersReport - muestra reportes si se daclic en imprimir repote   //
//////////////////////////////////////////////////////////////////////////////////
function showPaymentOrdersReport() {
    window.open(ACC_ROOT + "Report/ShowPaymentOrdersReport", 'mywindow', 'fullscreen=yes, scrollbars=auto');
    if (isSearchPaymentOrders != 0) {
        if (parseInt(tempSearchId) == 0) {
            setTimeout(function () {
                location.href = $("#ViewBagPaymentOrderSearchLink").val();
            }, 3000);
        } else {
            setTimeout(function () {
                location.href = $("#ViewBagMainTemporarySearchLink").val();
            }, 3000);
        }
    } else {
        setDataFieldsEmptyPaymentOrder();
        $("#ReceiptAmount").val("0");
        $("#btnPaymentOrderBankAccount").hide();
        SaveTempPaymentOrderZero();
        setTimeout(function () {
            SetTotalApplication();
            cleanPartials();
            GetDebitsAndCreditsMovementTypes(0, 0);
        }, 2500);

    }
}

//////////////////////////////////////////////////////////////////////////////////
// setDataReportPaymentOrders - carga datos enviados a impresion de reportes    //
//////////////////////////////////////////////////////////////////////////////////
function setDataReportPaymentOrders(paymentOrderId) {
    oPaymentOrdersMovementsModel = {
        Id: 0,
        PaymentOrderNumber: null,
        BranchId: 0,
        BranchName: null,
        PaymentBranchId: 0,
        PaymentBranchName: null,
        CompanyId: 0,
        CompanyName: null,
        EstimatedPaymentDate: null,
        PaymentDate: null,
        UserId: 0,
        UserName: null,
        PaymentTypeId: 0,
        PaymentTypeName: null,
        CurrencyId: 0,
        CurrencyName: null,
        PaymentIncomeAmount: 0,
        PaymentAmount: 0,
        BeneficiaryDocNumber: null,
        BeneficiaryName: null,
        PayToName: null,
        MovementSummaryItems: []
    };

    oPaymentOrdersMovementsModel.BeneficiaryDocNumber = $("#BeneficiaryDocumentNumberPaymentOrder").val();
    oPaymentOrdersMovementsModel.BeneficiaryName = $("#BeneficiaryNamePaymentOrder").val();
    oPaymentOrdersMovementsModel.BranchId = $("#PaymentOrderBranchSelect").val();
    oPaymentOrdersMovementsModel.BranchName = $("#PaymentOrderBranchSelect option:selected").text();
    oPaymentOrdersMovementsModel.CompanyId = $("#PaymentCompanySelect").val();
    oPaymentOrdersMovementsModel.CompanyName = $("#PaymentCompanySelect option:selected").text();
    oPaymentOrdersMovementsModel.CurrencyId = $("#PaymentOrderCurrencySelect").val();
    oPaymentOrdersMovementsModel.CurrencyName = $("#PaymentOrderCurrencySelect option:selected").text();
    oPaymentOrdersMovementsModel.Id = paymentOrderId;
    oPaymentOrdersMovementsModel.PaymentAmount = parseFloat(ClearFormatCurrency($("#PaymentOrderIncomeAmount").val()).replace(",", "."));
    oPaymentOrdersMovementsModel.PaymentBranchId = $("#BranchOfPaymentSelect").val();
    oPaymentOrdersMovementsModel.PaymentBranchName = $("#BranchOfPaymentSelect option:selected").text();
    oPaymentOrdersMovementsModel.EstimatedPaymentDate = $("#PaymentOrderGenerationDate").val();
    oPaymentOrdersMovementsModel.PaymentDate = $("#PaymentOrderGenerationDate").val();
    oPaymentOrdersMovementsModel.PaymentOrderNumber = paymentOrderId;
    oPaymentOrdersMovementsModel.PaymentTypeId = $("#PaymentTypeSelect").val();
    oPaymentOrdersMovementsModel.PaymentTypeName = $("#PaymentTypeSelect option:selected").text();
    oPaymentOrdersMovementsModel.PayToName = $("#PaymentOrderPayableTo").val();

    var ids = $("#listViewAplicationReceipt").UifListView("getData");

    if (ids.length > 0) {
        for (var i in ids) {
            var rowid = ids[i];

            oMovementSummaryModel = {
                DescriptionMovementSumary: null,
                Debit: 0,
                Credit: 0
            };

            oMovementSummaryModel.Credit = parseFloat(ClearFormatCurrency($.trim(rowid.Credits.toString()).replace(",", "")));
            oMovementSummaryModel.Debit = parseFloat(ClearFormatCurrency($.trim(rowid.Debits.toString()).replace(",", "")));
            oMovementSummaryModel.DescriptionMovementSumary = rowid.MovementType;

            oPaymentOrdersMovementsModel.MovementSummaryItems.push(oMovementSummaryModel);

        }
    }
    return oPaymentOrdersMovementsModel;
}

//Habilita / deshabilita combo Company segùn corresponda
function validateCompanyPaymentOrder() {
    
    if (isLoadPaymentOrder) {

        if ($("#PaymentCompanySelect").val() != "" && $("#PaymentCompanySelect").val() != null) {

            if (parseInt($("#ViewBagParameterMulticompany").val()) == 0) {


                $("#PaymentCompanySelect").attr("disabled", true);
            }
            else {
                $("#PaymentCompanySelect").attr("disabled", false);
            }
            clearInterval(timePaymentOrder);
        } else {
            $("#PaymentCompanySelect").val(accountingCompanyDefault);
        }
    }
}

//Verifica si están llenados todos los campos
function loadAllFieldsPaymentOrder() {

    loadPaymentOrders();

    setTimeout(function () {
        if (($("#PaymentOrderBranchSelect").val() != "" && $("#PaymentOrderBranchSelect").val() != null) &&
            ($("#PaymentCompanySelect").val() != "" && $("#PaymentCompanySelect").val() != null) &&
            ($("#PaymentTypeSelect").val() != "" && $("#PaymentTypeSelect").val() != null) &&
            ($("#PaymentOrderCurrencySelect").val() != "" && $("#PaymentOrderCurrencySelect").val() != null) &&
            ($("#BranchOfPaymentSelect").val() != "" && $("#BranchOfPaymentSelect").val() != null) &&
            ($("#OriginOfPaymentSelect").val() != "" && $("#OriginOfPaymentSelect").val() != null) &&
            ($("#BeneficiaryTypeSelect").val() != "" && $("#BeneficiaryTypeSelect").val() != null) &&
            ($("#PaymentOrderGenerationDate").val() != "" && $("#PaymentOrderGenerationDate").val() != null) &&
            ($("#ReceiptAmount").val() != "" && $("#ReceiptAmount").val() != null) &&
            ($("#BeneficiaryDocumentNumberPaymentOrder").val() != "" && $("#BeneficiaryDocumentNumberPaymentOrder").val() != null) &&
            ($("#BeneficiaryNamePaymentOrder").val() != "" && $("#BeneficiaryNamePaymentOrder").val() != null) &&
            (($("#PaymentOrderNumber").val() != "" && $("#PaymentOrderNumber").val() != null) || parseInt(tempSearchId) > 0) &&
            ($("#PaymentOrderCode").val() != "" && $("#PaymentOrderCode").val() != null)) {

            clearInterval(timerLoadPaymentOrder);
            unlockScreen();


            $("#PaymentOrderGenerationAmount").val("$ " + FormatDecimal($("#PaymentOrderGenerationAmount").val(), "2", ".", ","));
            $("#PaymentOrderIncomeAmount").val("$ " + FormatDecimal($("#PaymentOrderIncomeAmount").val(), "2", ".", ","));
        }
    }, 1000);

    if (isSearchPaymentOrders != 0) {
        clearInterval(timerLoadPaymentOrder);
        unlockScreen();
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////
// Carga los autocompletes de nombre y número de documento                                     //
/////////////////////////////////////////////////////////////////////////////////////////////////
function loadAutocompleteEventLocal(identifier) {

    var selectedDeptor;

    $('#' + identifier + 'ByDocumentNumber').on('itemSelected', function (event, selectedItem) {
        fillAutocompletesOrderGenerationLocal(identifier, selectedItem);
        selectedDeptor = selectedItem;
    });
    
    $('#' + identifier + 'ByName').on('itemSelected', function (event, selectedItem) {
        fillAutocompletesOrderGenerationLocal(identifier, selectedItem);
        selectedDeptor = selectedItem;
    });


    $('#' + identifier + 'ByDocumentNumber').on('keydown', function (event, selectedItem) {
        if (event.keyCode != 13 && event.keyCode != 9) {
            beneficiaryIndividualId = 0;
        }
    });

    $('#' + identifier + 'ByName').on('keydown', function (event, selectedItem) {

        if (event.keyCode != 13 && event.keyCode != 9) {
            beneficiaryIndividualId = 0;
        }
    });
}

/////////////////////////////////////////////////////////////////////////////////////
// cleanAutocompletes - limpia los autocompletes de nombre y número de documento   //
////////////////////////////////////////////////////////////////////////////////////
function cleanAutocompletes(identifier) {

    //setTimeout(function () {
    $('#' + identifier + 'ByDocumentNumber').val("");
    $('#' + identifier + 'ByName').val("");
    beneficiaryIndividualId = 0;
    $("#BeneficiaryDocumentNumberPaymentOrder").val("");
    $("#BeneficiaryNamePaymentOrder").val("");
    $("#PaymentOrderPayableTo").val("");
    $("#btnPaymentOrderBankAccount").hide();
    //}, 100);

    $('#' + identifier + 'ByDocumentNumber').parent().parent().hide();
    $('#' + identifier + 'ByName').parent().parent().hide();

    $("#BeneficiaryDocumentNumberPaymentOrder").show();
    $("#BeneficiaryNamePaymentOrder").show();

}

function fillAutocompletesOrderGenerationLocal(identifier, selectedItem) {

    $('#' + identifier + 'ByDocumentNumber').val(selectedItem.DocumentNumber);
    $('#' + identifier + 'ByName').val(selectedItem.Name);

    if (selectedItem.Id != undefined) {
        beneficiaryIndividualId = selectedItem.Id;
    } else if (selectedItem.AgentId != undefined) {
        beneficiaryIndividualId = selectedItem.AgentId;
    } else {
        beneficiaryIndividualId = selectedItem.CoinsuranceIndividualId;
    }

    $("#BeneficiaryDocumentNumberPaymentOrder").val(selectedItem.DocumentNumber);
    $("#BeneficiaryNamePaymentOrder").val(selectedItem.Name);
    $("#PaymentOrderPayableTo").val(selectedItem.Name);
    nameText = selectedItem.Name;
    documentNumberText = selectedItem.DocumentNumber;
    //Boton cuentas bancarias.
    if ($("#PaymentTypeSelect").val() == $("#ViewBagParamPaymentMethodTransfer").val() && beneficiaryIndividualId > 0) {

        $("#btnPaymentOrderBankAccount").show();
    }
    else {
        $("#btnPaymentOrderBankAccount").hide();
    }
}

//////////////////////////////////////////////////////////////////////////
// setCurrencyPaymentOrder calcula importes acorde a la tasa de cambio  //
//////////////////////////////////////////////////////////////////////////
function setCurrencyPaymentOrder(ddlIn, txtOut) {
    var ddlCurrency = $("#" + ddlIn).val();
    var txtExchangeRate = $("#" + txtOut);

    if (ddlCurrency >= 0) {
        var resp = getCurrencyRateBillingPaymentOrder($("#ViewBagDateAccounting").val(), ddlCurrency);
        txtExchangeRate.val(resp[0]);

        rateExchangePo = resp[0];

        maxPercentage = parseFloat(resp[0] + (resp[0] * (percentageDifference / 100))).toFixed(2);
        minPercentage = parseFloat(resp[0] - (resp[0] * (percentageDifference / 100))).toFixed(2);

        if (txtExchangeRate.val() != "") {
            txtExchangeRate.val(FormatCurrency(maxPercentage));
        }

        if (resp[1] == false) {

            $("#alertPaymentOrder").UifAlert('show', Resources.ExchageRateNotUpToDate, 'warning');
        }
    }
    else {
        txtExchangeRate.val("");
    }
}

//////////////////////////////////////////////////////////////////////////
// getCurrencyRateBillingPaymentOrder se obtiene la tasa de cambio      //
//////////////////////////////////////////////////////////////////////////
function getCurrencyRateBillingPaymentOrder(accountingDate, currencyId) {
    var alert = true;
    var rate;
    var resp = new Array();

    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "Billing/GetCurrencyExchangeRate",
        data: {
            "rateDate": accountingDate,
            "currencyId": currencyId
        },
        success: function (data) {
            if (data == 1 || data == 0) {
                $.ajax({
                    async: false,
                    type: "POST",
                    url: ACC_ROOT + "Billing/GetLatestCurrencyExchangeRate",
                    data: {
                        "rateDate": accountingDate,
                        "currencyId": currencyId
                    },
                    success: function (dataRate) {
                        if (dataRate == 0 || dataRate == 1) {
                            rate = 1;
                            exchangeRate = rate;
                            alert = true;
                        } else {
                            rate = dataRate;
                            exchangeRate = rate;
                            alert = false;
                        }
                    }
                });

            } else {
                rate = data;
                alert = true;
            }
        }
    });

    resp[0] = rate;
    resp[1] = alert;

    return resp;
}

//Recupera de temporales
function loadPaymentOrdersApplication(tempImputationSearch) {

        $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "PaymentOrders/GetTempPaymentOrderByTempPaymentOrderId",
        data: { "tempPaymentOrderId": paymentOrderNumber },
        success: function (dataPaymentOrders) {

            unlockScreen();

            $("#PaymentOrderBranchSelect").val(dataPaymentOrders.BranchId);
            $("#PaymentCompanySelect").val(dataPaymentOrders.CompanyId);
            isLoadPaymentOrder = true;
            $("#PaymentTypeSelect").val(dataPaymentOrders.PaymentMethodId);
            $("#PaymentOrderCurrencySelect").val(dataPaymentOrders.CurrencyId);
            $("#BranchOfPaymentSelect").val(dataPaymentOrders.BranchPayId);
            $("#OriginOfPaymentSelect").val(dataPaymentOrders.PaymentSourceId);
            $("#BeneficiaryTypeSelect").val(dataPaymentOrders.PersonTypeId);
            $("#BeneficiaryTypeSelect").attr("disabled", true);
            $("#PaymentOrderExchangeType").val(dataPaymentOrders.Exchange);
            $("#PaymentOrderCurrencySelect").trigger("change");
            $("#PaymentTypeSelect").trigger("change");
           
            $("#PaymentOrderGenerationDate").val(dataPaymentOrders.EstimatedPaymentDate);
            $("#PaymentOrderGenerationAmount").val(dataPaymentOrders.IncomeAmount);
            $("#PaymentOrderIncomeAmount").val(dataPaymentOrders.Amount);
            $("#ReceiptAmount").val(dataPaymentOrders.Amount);
            $("#BeneficiaryDocumentNumberPaymentOrder").val(dataPaymentOrders.BeneficiaryDocumentNumber);
            $("#BeneficiaryNamePaymentOrder").val(dataPaymentOrders.BeneficiaryName);
            $("#PaymentOrderPayableTo").val(dataPaymentOrders.PayTo);
            $("#Observation").val(dataPaymentOrders.Observation);
            beneficiaryIndividualId = dataPaymentOrders.IndividualId;
            
            $("#globalTitle").html(Resources.Edit +" "+ Resources.TemporalPaymentOrder + " "+ tempImputationSearch); 

            $("#PaymentOrderCode").val(dataPaymentOrders.TempPaymentOrderId);

            var resp = getCurrencyRateBillingPaymentOrder($("#ViewBagDateAccounting").val(), dataPaymentOrders.CurrencyId);
            $("#PaymentOrderExchangeType").val(resp[0]);

            lockScreen();
            setTimeout(function () {

                 $.ajax({
                        async: false,
                        url: ACC_ROOT + "TemporarySearch/RecalculatingForeignCurrencyAmountRequest",
                        data: { "tempImputationId": tempImputationSearch, "imputationTypeId": parseInt($("#ViewBagAplicationPaymentOrder").val()), "sourceId": paymentOrderNumber },
                     success: function () {
                         unlockScreen();
                            amount = 0;
                            GetDebitsAndCreditsMovementTypes(tempImputationSearch, dataPaymentOrders.Amount);
                        }
                    });
            }, 800);

            setTimeout(function () {
                SetTotalApplication();
                SetTotalControl();

                tempImputationId = tempImputationSearch;
            }, 2500);
        }
        });   
}

////////////////////////////////////////////////////////////////////////////////////////////////
// loadPaymentOrders carga datos de orden de pago cuando se redirecciona desde la búsqueda    //
////////////////////////////////////////////////////////////////////////////////////////////////
function loadPaymentOrders() {

    if (isSearchPaymentOrders != 0 && parseInt(tempSearchId) == 0) {

        lockScreen();
        setTimeout(function () {
            $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "PaymentOrders/GetSearchPaymentOrders",
            data: {
                    "branchId": branchIdPaymentOrder,
                    "userId": "",
                    "paymentMethodId": "",
                    "startDate": "",
                    "endDate": "",
                    "paymentOrderNumber": paymentOrderNumber,
                    "personTypeId": "",
                    "beneficiaryIndividualId": "",
                    "beneficiaryDocumentNumber": "",
                    "beneficiaryName": "",
                    "status": 1,
                    "IsDelivered": null,
                    "IsReconciled": null,
                    "IsAccounting": null
            },
            success: function (data) {
                unlockScreen();                
                $("#PaymentOrderBranchSelect").val(data.aaData[0].BranchCode);
                $("#PaymentCompanySelect").val(data.aaData[0].CompanyCode);
                isLoadPaymentOrder = true;
                $("#PaymentTypeSelect").val(data.aaData[0].PaymentMethodCode);
                $("#PaymentOrderCurrencySelect").val(data.aaData[0].CurrencyCode);
                $("#BranchOfPaymentSelect").val(data.aaData[0].BranchPayCode);
                $("#OriginOfPaymentSelect").val(data.aaData[0].PaymentSourceCode);
                $("#BeneficiaryTypeSelect").val(data.aaData[0].PersonTypeCode);
                $("#BeneficiaryTypeSelect").attr("disabled", true);
                $("#PaymentOrderCurrencySelect").trigger("change");
                $("#PaymentTypeSelect").trigger("change");
          
                $("#PaymentOrderGenerationDate").val(data.aaData[0].EstimatedPaymentDate);
                $("#PaymentOrderGenerationAmount").val(data.aaData[0].IncomeAmount);
                $("#ReceiptAmount").val(data.aaData[0].Amount);
                $("#BeneficiaryDocumentNumberPaymentOrder").val(data.aaData[0].BeneficiaryDocumentNumber);
                $("#BeneficiaryNamePaymentOrder").val(data.aaData[0].BeneficiaryName);
                $("#PaymentOrderPayableTo").val(data.aaData[0].PayTo);
                beneficiaryIndividualId = data.aaData[0].IndividualId;

                $("#globalTitle").html(Resources.Edit + " " + Resources.PaymentOrder);
                
                $("#PaymentOrderNumber").val(data.aaData[0].PaymentOrderCode);
                $("#PaymentOrderCode").val(data.aaData[0].PaymentOrderCode);
                $("#Observation").val(data.aaData[0].Observation);

                GetDebitsAndCreditsMovementTypes(isSearchPaymentOrders, parseFloat(ClearFormatCurrency(data.aaData[0].Amount)));

                setTimeout(function () {
                    SetTotalApplication();
                    SetTotalControl();
                }, 2500);
            }
          });
        }, 500);
    }
    else {
        if (parseInt(tempSearchId) > 0) {

            //Cuando viene de la bùsqueda de temporales de OP
            loadPaymentOrdersApplication(parseInt(tempSearchId));
        }
    }

    return;
};

///////////////////////////////////////////////////////////////////////////////////////////////////////////
// savePaymentOrder GRABA IMPUTATION A REALES, ELIMINA DE TEMPORALES, ACTUALIZA ESTADO DE PAYMENT ORDER ///
//////////////////////////////////////////////////////////////////////////////////////////////////////////
function savePaymentOrder() {
    //GRABA IMPUTATION A REALES, ELIMINA DE TEMPORALES, ACTUALIZA ESTADO DE PAYMENT ORDER
    
    lockScreen();

    setTimeout(function () {

     $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "PaymentOrders/SavePaymentOrderApplication",
        data: {
            "tempPaymentOrderId": $("#PaymentOrderCode").val(), "tempImputationId": tempImputationId,            
            "imputationTypeId": parseInt($("#ViewBagImputationType").val()),     
            "statusId": 3
        },
            success: function (dataEntry) {
                $("#PaymentOrderCode").val(dataEntry);
                var msg = Resources.SuccessfullySaved + " " + Resources.PaymentOrder + ": " + $("#PaymentOrderCode").val() + " " + Resources.PrintReportQuestion;
                loadPaymentOrdersReport($("#PaymentOrderCode").val(), msg, "");
                SetDataPaymentOrderEmpty();


                setTimeout(function () {
                    GetDebitsAndCreditsMovementTypes(0, 0);
                }, 500);
                setTimeout(function () {
                    SetTotalApplication();
                    SetTotalControl();
                }, 1500);

                setDataFieldsEmptyPaymentOrder();
                $("#ReceiptAmount").val("0");
                $("#btnPaymentOrderBankAccount").hide();
                setTimeout(function () {
                    cleanPartials();
                }, 1500);

                unlockScreen();
            }
        });
    }, 1000);
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////
// saveRealPaymentOrder ///
//////////////////////////////////////////////////////////////////////////////////////////////////////////
function saveRealPaymentOrder() {

    var msg = Resources.SuccessfullySaved + " " + Resources.PaymentOrder + ": " + $("#PaymentOrderCode").val() + " " + Resources.PrintReportQuestion;
    loadPaymentOrdersReport($("#PaymentOrderCode").val(), msg, "");
    SetDataPaymentOrderEmpty();

    setTimeout(function () {
        GetDebitsAndCreditsMovementTypes(0, 0);
    }, 500);
    setTimeout(function () {
        SetTotalApplication();
        SetTotalControl();
    }, 1500);

    setDataFieldsEmptyPaymentOrder();
    $("#ReceiptAmount").val("0");
    $("#btnPaymentOrderBankAccount").hide();
    setTimeout(function () {
        cleanPartials();
    }, 1500);
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////
// paymentOrderIncomeAmount calcula y mestra importes acorde a la tasa de cambio     //
//////////////////////////////////////////////////////////////////////////////////////////////////////////
function paymentOrderIncomeAmount() {
    if ($("#PaymentOrderGenerationAmount").val() != "") {
        $("#PaymentOrderGenerationAmount").val($.trim(ClearFormatCurrency($("#PaymentOrderGenerationAmount").val())));
        $("#PaymentOrderGenerationAmount").val(FormatCurrency($("#PaymentOrderGenerationAmount").val()));
    }
    $("#PaymentOrderIncomeAmount").val(parseFloat(ClearFormatCurrency($("#PaymentOrderExchangeType").val()).replace(",", ".")) * parseFloat(ClearFormatCurrency($("#PaymentOrderGenerationAmount").val()).replace(",", ".")));

    if ($("#PaymentOrderGenerationAmount").val() != "" && $("#PaymentOrderExchangeType").val() != "") {
        $("#PaymentOrderIncomeAmount").val(FormatCurrency(parseFloat($("#PaymentOrderIncomeAmount").val()).toFixed(2)));
    }

    $("#ReceiptAmount").val($("#PaymentOrderIncomeAmount").val());
    if ($("#PaymentOrderIncomeAmount").val() == 'NaN') {
        $("#PaymentOrderIncomeAmount").val('');
        $("#ReceiptAmount").val('0');
    }
}

///////////////////////////////////////////
// Setea la tasa de cambio de la moneda //
///////////////////////////////////////////
function SetCurrency(source, destination) {
    var selectCurrency = $("#" + source).val();
    var textExchangeRate = $("#" + destination);

    if (selectCurrency >= 0) {
        var response = GetCurrencyRateBilling($("#ViewBagDateAccounting").val(), selectCurrency);
        textExchangeRate.val("$ " + FormatDecimal(response[0], "6", ".", ","));

        if (response[1] == false) {
            $("#alertPaymentOrder").UifAlert('show', Resources.ExchageRateNotUpToDate, "warning");
        }
    }
    else {
        textExchangeRate.val("");
    }
}

///////////////////////////////////////////////////////////
// Obtiene la tasa de cambio de la moneda                //
///////////////////////////////////////////////////////////
function GetCurrencyRateBilling(accountingDate, currencyId) {
    var alert = true;
    var rate;
    var response = new Array();

    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "Billing/GetCurrencyExchangeRate",
        data: {
            "rateDate": accountingDate,
            "currencyId": currencyId
        },
        success: function (data) {
            if (data == 1 || data == 0) {
                $.ajax({
                    async: false,
                    type: "POST",
                    url: ACC_ROOT + "Billing/GetLatestCurrencyExchangeRate",
                    data: {
                        "rateDate": accountingDate,
                        "currencyId": currencyId
                    },
                    success: function (dataRate) {
                        if (dataRate == 0 || dataRate == 1) {
                            rate = 1;
                            exchangeRate = rate;
                            alert = true;
                        } else {
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

    response[0] = rate;
    response[1] = alert;
    exchangeRate = rate;

    return response;
}

$("#AccountingDate").val($("#ViewBagDateAccounting").val());
$("#ReceiptAmount").val("0");

if (parseInt(isSearchPaymentOrders) != 0) {
    tempImputationId = parseInt(isSearchPaymentOrders);
}

/*--------------------------------------------------------------------------------------------------------------------------------------------*/
/*										FORMATOS BOTONES /FECHAS/ #CARACTERES / NÚMEROS-DECIMALES / ACORDEONES										*/
/*--------------------------------------------------------------------------------------------------------------------------------------------*/

//Control número máximo de caracteres
$("#BeneficiaryDocumentNumberPaymentOrder").attr("maxlength", 20);
$("#BeneficiaryNamePaymentOrder").attr("maxlength", 60);
$("#PaymentOrderGenerationAmount").attr("maxlength", 15);
$("#PaymentOrderPayableTo").attr("maxlength", 60);

/*--------------------------------------------------------------------------------------------------------------------------------------------*/
/*																  ACCIONES / EVENTOS				 												*/
/*--------------------------------------------------------------------------------------------------------------------------------------------*/

//Se ocultan campos de busqueda de autocompletes
$(window).load(function () {

    //Cargando los eventos
    loadAutocompleteEventLocal('SearchSuppliers');
    loadAutocompleteEventLocal('SearchInsured');
    loadAutocompleteEventLocal('SearchEmployee');
    loadAutocompleteEventLocal('SearchCoinsurance');
    loadAutocompleteEventLocal('SearchPerson');
    loadAutocompleteEventLocal('SearchAgent');
    loadAutocompleteEventLocal('SearchReinsurer');

    //setTimeout(function () {
    cleanAutocompletes('SearchSuppliers');
    cleanAutocompletes('SearchInsured');
    cleanAutocompletes('SearchEmployee');
    cleanAutocompletes('SearchCoinsurance');
    cleanAutocompletes('SearchPerson');
    cleanAutocompletes('SearchAgent');
    cleanAutocompletes('SearchReinsurer');
    //}, 500);
});

/////////////////////////////////////////////
/// Da formato a de moneda Importe       ///
/////////////////////////////////////////////
$("#ReceiptAmount").blur(function () {
    $("#ReceiptAmount").val(FormatCurrency($("#ReceiptAmount").val()));
});

/////////////////////////////////////////////
/// Da formato a de moneda al Total       ///
/////////////////////////////////////////////
$("#TotalControl").blur(function () {
    if ($("#TotalControl").val() == 'NaN') {
        $("#TotalControl").val(0);
    }

    $("#TotalControl").val(FormatCurrency($("#TotalControl").val()));
});

/////////////////////////////////////////////
/// Da formato a de moneda Importe de pago ///
/////////////////////////////////////////////
$("#PaymentOrderGenerationAmount").blur(function () {

    paymentOrderIncomeAmount();

    if ($("#PaymentOrderIncomeAmount").val() == '') {
        $("#PaymentOrderIncomeAmount").val(0);
    }
    SetTotalControl();
    setFormatPayment();
    
});

/////////////////////////////////////////////
/// Da formato a de moneda a tasa de cambio //
/////////////////////////////////////////////
$("#PaymentOrderExchangeType").blur(function () {
    $("#alertPaymentOrder").UifAlert('hide');
    value = $("#PaymentOrderExchangeType").val();
    if (value < 0 || value > 10000) {
        setCurrencyPaymentOrder("PaymentOrderCurrencySelect", "PaymentOrderExchangeType");
        $("#PaymentOrderExchangeType").val(rateExchangePo);

        $("#alertPaymentOrder").UifAlert('show', Resources.CannotInputValueGreatherThan + ": " + maxPercentage + " " + Resources.CannotInputValueLowerThan + ": " + minPercentage, 'warning');
    }
    if ($("#PaymentOrderExchangeType").val() != "") {
        $("#PaymentOrderExchangeType").val(FormatCurrency($("#PaymentOrderExchangeType").val()));
    }
    paymentOrderIncomeAmount();
});

$("#btnPaymentOrderBankAccount").hide();

/////////////////////////////////////////////
/// Pone en mayusculas a Beneficiario     ///
/////////////////////////////////////////////
$("#PaymentOrderPayableTo").blur(function () {
    $("#PaymentOrderPayableTo").val($("#PaymentOrderPayableTo").val().toUpperCase());
});

//////////////////////////////////////////////
//Mantiene el valor del input luego de dar TAB//
//////////////////////////////////////////////
function updateInput(element) {
    setTimeout(function () {
        if (element == "SearchInsuredByName") {
            var nameInsured = document.getElementById("SearchInsuredByName").value;
            if (nameInsured == "" || nameInsured != nameText) {
                $('#SearchInsuredByName').UifAutoComplete('setValue', nameText);

            }
        } else if (element == "SearchInsuredByDocumentNumber") {
            var numberDocInsured = document.getElementById("SearchInsuredByDocumentNumber").value;
            if (numberDocInsured == "" || numberDocInsured != documentNumberText) {
                $('#SearchInsuredByDocumentNumber').UifAutoComplete('setValue', documentNumberText);

            }
        } else if (element == "SearchSuppliersByName") {
            var nameSupplier = document.getElementById("SearchSuppliersByName").value;
            if (nameSupplier == "" || nameSupplier != nameText) {
                $('#SearchSuppliersByName').UifAutoComplete('setValue', nameText);

            }
        } else if (element == "SearchSuppliersByDocumentNumber") {
            var numberDocSupplier = document.getElementById("SearchSuppliersByDocumentNumber").value;
            if (numberDocSupplier == "" || numberDocSupplier != documentNumberText) {
                $('#SearchSuppliersByDocumentNumber').UifAutoComplete('setValue', documentNumberText);

            }
        } else if (element == "SearchEmployeeByName") {
            var nameEmployee = document.getElementById("SearchEmployeeByName").value;
            if (nameEmployee == "" || nameEmployee != nameText) {
                $('#SearchEmployeeByName').UifAutoComplete('setValue', nameText);

            }
        } else if (element == "SearchEmployeeByDocumentNumber") {
            var numberDocEmployee = document.getElementById("SearchEmployeeByDocumentNumber").value;
            if (numberDocEmployee == "" || numberDocEmployee != documentNumberText) {
                $('#SearchEmployeeByDocumentNumber').UifAutoComplete('setValue', documentNumberText);

            }
        }
    }, 10);
}

//////////////////////////////////////////////
//PaymentOrderCancel botón que cancela Op//
//////////////////////////////////////////////
$("#PaymentOrderCancel").click(function () {
    CloseDialogPaymentOrder(Resources.CancelApplicationMessage);
    $("#alertPaymentOrder").UifAlert('hide');
});

$("#PaymentOrderCurrencySelect").on('itemSelected', function (event, selectedItem) {
    $.ajax({
        async: false,
        url: ACC_ROOT + "CheckingAccountBrokers/GetCurrencyDiferenceByCurrencyId",
        data: { "currencyId": $("#PaymentOrderCurrencySelect").val() },
        success: function (data) {
            percentageDifference = data;
            setCurrencyPaymentOrder("PaymentOrderCurrencySelect", "PaymentOrderExchangeType");
        }
    });

    paymentOrderIncomeAmount();
    $("#PaymentOrderGenerationAmount").blur();
    setFormatPayment();
});

/////////////////////////////////////////
//  dropdown  Tipo de Pago             //
/////////////////////////////////////////
$("#PaymentTypeSelect").on('itemSelected', function (event, selectedItem) {
    if ($("#PaymentTypeSelect").val() == $("#ViewBagParamPaymentMethodTransfer").val() && beneficiaryIndividualId > 0) { // 2 transferencia
        $("#btnPaymentOrderBankAccount").show();
    }
    else {
        $("#btnPaymentOrderBankAccount").hide();
    }
});

//Selecciona tipo de Beneficiario
$("#BeneficiaryTypeSelect").on('itemSelected', function (event, selectedItem) {
    
    cleanAutocompletes('SearchSuppliers');
    cleanAutocompletes('SearchInsured');
    cleanAutocompletes('SearchEmployee');
    cleanAutocompletes('SearchCoinsurance');
    cleanAutocompletes('SearchPerson');
    cleanAutocompletes('SearchAgent');
    cleanAutocompletes('SearchReinsurer');

    setTimeout(function () {

        if (selectedItem.Id != "") {

            $("#BeneficiaryDocumentNumberPaymentOrder").hide();
            $("#BeneficiaryNamePaymentOrder").hide();

            if (selectedItem.Id == $("#ViewBagSupplierCode").val()) { // 1 Proveedor  

                $('#SearchSuppliersByDocumentNumber').parent().parent().show();
                $("#SearchSuppliersByName").parent().parent().show();

            } else if (selectedItem.Id == $("#ViewBagInsuredCode").val()) { // 2 Asegurado  

                $("#SearchInsuredByDocumentNumber").parent().parent().show();
                $("#SearchInsuredByName").parent().parent().show();

            } else if (selectedItem.Id == $("#ViewBagCoinsurerCode").val()) { // 7 Coaseguradora  

                $("#SearchCoinsuranceByDocumentNumber").parent().parent().show();
                $("#SearchCoinsuranceByName").parent().parent().show();

            } else if (selectedItem.Id == $("#ViewBagThirdPartyCode").val()) { //8 Tercero   

                $("#SearchPersonByDocumentNumber").parent().parent().show();
                $("#SearchPersonByName").parent().parent().show();

            } else if (selectedItem.Id == $("#ViewBagProducerCode").val()) { // 10 Productor  

                $("#SearchAgentByDocumentNumber").parent().parent().show();
                $("#SearchAgentByName").parent().parent().show();

            } else if (selectedItem.Id == $("#ViewBagEmployeeCode").val()) { // 11 Empleado  

                $("#SearchEmployeeByDocumentNumber").parent().parent().show();
                $("#SearchEmployeeByName").parent().parent().show();

            } else if (selectedItem.Id == $("#ViewBagReinsurerCode").val()) { //13 Reaseguradora  

                $("#SearchReinsurerByDocumentNumber").parent().parent().show();
                $("#SearchReinsurerByName").parent().parent().show();

            } else if (selectedItem.Id == $("#ViewBagTradeConsultant").val()) { // 14 Asesor comercial   

                $("#SearchPersonByDocumentNumber").parent().parent().show();
                $("#SearchPersonByName").parent().parent().show();

            } else if (selectedItem.Id == $("#ViewBagAgentCode").val()) { //15 Agente   

                $("#SearchAgentByDocumentNumber").parent().parent().show();
                $("#SearchAgentByName").parent().parent().show();
            }
        } else {

            cleanAutocompletes('SearchSuppliers');
            cleanAutocompletes('SearchInsured');
            cleanAutocompletes('SearchEmployee');
            cleanAutocompletes('SearchCoinsurance');
            cleanAutocompletes('SearchPerson');
            cleanAutocompletes('SearchAgent');
            cleanAutocompletes('SearchReinsurer');
        }
    }, 300);
});

/////////////////////////////////////////
/// Botón para edición en el listview ///
/////////////////////////////////////////
$("#listViewAplicationReceipt").on('rowEdit', function (event, data, index) {
    $("#paymentForm").validate();
    var lastAmount = $("#PaymentOrderGenerationAmount").val();
    preReceiptAmount('PaymentOrderGenerationAmount');
    if ($("#paymentForm").valid()) {
        $("#PaymentOrderGenerationAmount").val(lastAmount);

        if (beneficiaryIndividualId > 0) {
            $("#alertPaymentOrder").UifAlert("hide");

            lockScreen();
            setTimeout(function () {   
             $.ajax({
                async: false,
                type: "POST",
                url: ACC_ROOT + "PaymentOrders/UpdateTempPaymentOrder",
                data: { "paymentOrderModel": SetDataPaymentOrderGeneration(2) },
                 success: function () {
                     unlockScreen();
                 }
            });
            }, 500);
            // Primas por cobrar
            if (data.Id == 1) {

                refreshApplyView();
                ClearSearchFields();
                $('#ModalPremiums').UifModal('showLocal', Resources.PremiumReceivableLabel + " " + Resources.DialogTitleTemporary + " " + tempImputationId);
                LoadSearchByForPolicies();
                LoadBranch();
                LoadPrefix();
            }
            // Primas en depósito
            if (data.Id == 2) {
                $('#modalPremiumDeposit').UifModal('showLocal', Resources.AgentMovementTitle);
            }
            // Comisiones descontadas
            if (data.Id == 3) {
                $('#modalDiscountedCommission').UifModal('showLocal', Resources.AgentMovementTitle);
            }
            // Cuenta corriente agentes
            if (data.Id == 4) {
                editAgent = -1;
                $("#addAgentForm").find("#MovementBillNumber").val($("#ViewBagReceiptNumber").val());
                LoadAgentBranchs();
                LoadAgentNatures();
                LoadAgentCurrencies();
                LoadAgentCompanies();
                $('#modalAgent').UifModal('showLocal', Resources.AgentMovementTitle + '. ' + Resources.DialogTitleTemporary + tempImputationId);
                SetAgentFieldEmpty();
                SetAgentTotalMovement();
                SetAgentAccountingCompany();
            }
            // Cuenta corriente coaseguros
            if (data.Id == 5) {
                editCoinsurance = -1;
                $("#addCoinsuranceForm").find("#MovementBillNumber").val($("#ViewBagReceiptNumber").val());
                LoadCoinsuresMovementsBranchs();
                LoadCoinsuresMovementsCoinsuranceTypes();
                LoadCoinsuresMovementsNatures();
                LoadCoinsuresMovementsCurrencies();
                LoadCoinsuresMovementsAccountingCompanies(oPaymentOrderGeneration.CompanyId);
                $('#modalCoinsurance').UifModal('showLocal', Resources.DialogCoinsuranceMovementsTitle + '. ' + Resources.DialogTitleTemporary + tempImputationId);
                SetCoinsuranceFieldEmpty();
                SetCoinsuranceTotalMovement();
                SetCoinsuranceAccountingCompany();
            }
            // Cuenta corriente reaseguros
            if (data.Id == 6) {
                editReinsurance = -1;
                $("#addReinsuranceForm").find("#MovementBillNumber").val($("#ViewBagReceiptNumber").val());
                LoadReinsuraceMovementsBranchs();
                LoadTechnicalPrefixes();
                LoadContractTypeEnabled();
                LoadReinsuraceMovementsNature();
                LoadReinsuraceMovementsCurrencies();
                LoadReinsuraceMovementsYearMonths();
                LoadReinsuraceMovementsCompanies(oPaymentOrderGeneration.CompanyId);
                $('#modalReinsurance').UifModal('showLocal', Resources.DialogReinsuranceMovementsTitle + '. ' + Resources.DialogTitleTemporary + tempImputationId);
                $("#modalReinsurance").find("#OptionalOne").hide();
                $("#modalReinsurance").find("#OptionalTwo").hide();
                $("#modalReinsurance").find("#OptionalThree").hide();
                SetReinsuranceFieldEmpty();
                SetReinsuranceTotalMovement();
                SetReinsuranceAccountingCompany();
            }
            // Contabilidad
            if (data.Id == 7) {
                $("#addAccountingMovementForm").formReset();   
                LoadAccountingBranchs();
                LoadAcountingNatures();
                LoadAccountingCurrencies();
                LoadAcountingCompanies(accountingCompanyDefault);
                AccountingMovementsReload();
                LoadAccountingMovementCompany();
                LoadthirdAccountingUsed(); // DANC 2018-06-12
                SetAccountingTotalMovement();
                GetAccountingSalePoints();
                $('#AccountingMovementModal').UifModal('showLocal', Resources.DialogAccountingMovementsTitle + '. ' + Resources.DialogTitleTemporary + tempImputationId);
            }
            // Solicitud pagos varios
            if (data.Id == 8) {
                $("#modalVarious").find("#TitleAgentNumber").hide();
                $("#modalVarious").find("#TitleEmployeeNumber").hide();
                $("#modalVarious").find("#TitleSupplierNumber").hide();
                $("#modalVarious").find("#TitleAgentName").hide();
                $("#modalVarious").find("#TitleEmployeeName").hide();
                $("#modalVarious").find("#TitleSupplierName").hide();
                $('#modalVarious').UifModal('showLocal', Resources.VariousPaymentRequest + '. ' + Resources.DialogTitleTemporary + tempImputationId);
                LoadPersonTypes();
                LoadPaymentVariousBranchs(branchUserDefault);
                LoadPaymentVariousCompanies(oPaymentOrderGeneration.CompanyId)
                SetRequestVariousFieldEmpty();
                RefreshRequestVariousMovements();
                SetVariousTotalMovement();
                SetVariousAccountingCompany();
                GetVariousRequestSalePoints();
            }
            // Solicitud pagos siniestros
            if (data.Id == 9) {
                $("#modalClaims").find("#TitleAgentNumber").hide();
                $("#modalClaims").find("#TitleInsuredNumber").hide();
                $("#modalClaims").find("#TitleSupplierNumber").hide();
                $("#modalClaims").find("#TitleAgentName").hide();
                $("#modalClaims").find("#TitleInsuredName").hide();
                $("#modalClaims").find("#TitleSupplierName").hide();

                $('#modalClaims').UifModal('showLocal', Resources.ClaimsPaymentRequestLabel + '. ' + Resources.DialogTitleTemporary + tempImputationId);
                $("#modalClaims").find("#requestClaimsListView").UifListView();
                LoadPaymentClaimsMovementsPersonTypes();
                LoadPaymentClaimsMovementsBranchs()
                LoadPaymentClaimsMovementsPrefixes();
                LoadPaymentClaimsMovementsRequestTypes();
                LoadPaymentClaimsMovementsCompanies(oPaymentOrderGeneration.CompanyId);
                SetRequestClaimsFieldEmpty();
                RefreshRequestClaimsMovements();
                SetClaimsTotalMovement();
                SetClaimsAccountingCompany();
                GetClaimsRequestSalePoints();
            }
            // Préstamos asegurados
            if (data.Id == 10) {
                editInsuredLoan = -1;
                $("#addCoinsuranceForm").find("#MovementBillNumber").val($("#ViewBagReceiptNumber").val());
                $('#modalInsuredLoan').UifModal('showLocal', Resources.DialogInsuredLoanMovementsTitle + '. ' + Resources.DialogTitleTemporary + tempImputationId);
                SetInsuredLoanFieldEmpty();
                SetInsuredLoanTotalMovement();
            }
        } else {

            var msj = Resources.RequiredFieldsMissing + " " + Resources.Beneficiary + " : " + Resources.DocumentNumber + " / " + Resources.PayerName;
            $.UifDialog('alert', { 'message': msj, 'title': Resources.Validation_message });

        }
    }
});

//////////////////////////////////////////
/// Botón aceptar aplicación de recibo ///
//////////////////////////////////////////
$("#PaymentOrderBranchSelect").on('itemSelected', function (event, selectedItem) {
    $("#alertPaymentOrder").UifAlert('hide');
});

$("#PaymentOrderCurrencySelect").on('itemSelected', function (event, selectedItem) {
    $("#alertPaymentOrder").UifAlert('hide');
});

$("#PaymentOrderExchangeType").on('keypress', function (event, selectedItem) {
    $("#alertPaymentOrder").UifAlert('hide');
});

$("#PaymentOrderGenerationAmount").on('keypress', function (event, selectedItem) {
    $("#alertPaymentOrder").UifAlert('hide');
});

//////////////////////////////////////////
/// Botón aceptar aplicación de recibo ///
//////////////////////////////////////////
$("#AcceptPaymentOrder").click(function () {
 
    $("#paymentForm").validate();
    var lastAmount = $("#PaymentOrderGenerationAmount").val();
    preReceiptAmount('PaymentOrderGenerationAmount');
    if ($("#paymentForm").valid()) {
        $("#PaymentOrderGenerationAmount").val(lastAmount);  
            if (beneficiaryIndividualId > 0) {

                if (TotalDebit() != 0 || TotalCredit() != 0) {
                    if (parseFloat(ClearFormatCurrency($("#TotalControl").val().replace("", ","))) != 0) {

                        var deployingMessage = Resources.SaveTemporalConfirmationMessage;
                        if (isSearchPaymentOrders != 0) {
                            if (parseInt(tempSearchId) == 0) {
                                deployingMessage = Resources.UpdatePaymentOrder;
                            }
                        }
                        //DIALOGO DE COMFIRMACION PARA GRABAR IMPUTATION EN TEMPORALES, ACTUALIZA ESTADO DE PAYMENT ORDER
                        SavePaymentOrderTemporalConfirmation(deployingMessage);
                        Resources.TemporalSuccessfullySaved;
                    }
                    else {
                        if (isSearchPaymentOrders == 0 && parseInt(tempSearchId) == 0) {  //Cuando se realiza nueva orden de pago
                            //Actualiza fecha y a estado como aplicado 

                            lockScreen();
                            setTimeout(function () {   
                                $.ajax({
                                async: false,
                                type: "POST",
                                url: ACC_ROOT + "PaymentOrders/UpdateTempPaymentOrder",
                                data: { "paymentOrderModel": SetDataPaymentOrderGeneration(2) },
                                success: function (dataTempId) {
                                    unlockScreen();
                                    if (dataTempId.success == false) {

                                        $("#alertPaymentOrder").UifAlert('show', dataTempId.result, "danger");
                                    } else {

                                        operationId = dataTempId;

                                        if ($("#ViewBagEnabledEvents").val() == 'true') {
                                            $.ajax({
                                                type: "POST",
                                                url: ACC_ROOT + "Events/GetsEventNotification",
                                                data: { "operationId": operationId },
                                                success: function (datEvent) {
                                                    if (datEvent.records > 0) {
                                                        $.ajax({
                                                            type: "POST",
                                                            url: ACC_ROOT + "Events/GetsEventAuthorization",
                                                            data: { "operationId": operationId },
                                                            success: function (data) {
                                                                if (data.records > 0) {
                                                                    $("#dlgEventsAuthorization").dialog("open");
                                                                }
                                                            }
                                                        });
                                                    }
                                                    else {
                                                        savePaymentOrder();
                                                    }
                                                }
                                            });
                                        }
                                        else {
                                            savePaymentOrder();
                                        }
                                    }
                                }
                                });
                            }, 500);
                        } else {   //CUANDO SE HACE 0 y es redirecionado desde búsqueda en EE
                            if (isSearchPaymentOrders != 0) {
                                if (parseInt(tempSearchId) == 0) {
                                    //Actualiza fecha y estado aplicado
                                
                                    lockScreen();
                                    setTimeout(function () {  
                                        $.ajax({
                                            async: false,
                                            type: "POST",
                                            url: ACC_ROOT + "PaymentOrders/UpdatePaymentOrder",
                                            data: JSON.stringify({ "paymentOrderModel": SetDataPaymentOrderGeneration(2) }),
                                            dataType: "json",
                                            contentType: "application/json; charset=utf-8",
                                            success: function () {
                                                unlockScreen();
                                                saveRealPaymentOrder();
                                            }
                                        });
                                    }, 500);
                                } else {
                             
                                    lockScreen();
                                    setTimeout(function () {  
                                    //Actualiza fecha y a estado como aplicado
                                        $.ajax({
                                        async: false,
                                        type: "POST",
                                        url: ACC_ROOT + "PaymentOrders/UpdateTempPaymentOrder",
                                        data: { "paymentOrderModel": SetDataPaymentOrderGeneration(2) },
                                        success: function (dataTempId) {
                                            unlockScreen();
                                            if (dataTempId.success == false) {

                                                $("#alertPaymentOrder").UifAlert('show', dataTempId.result, "danger");
                                            } else {

                                                operationId = dataTempId;
                                                if ($("#ViewBagEnabledEvents").val() == 'true') {
                                                    $.ajax({
                                                        type: "POST",
                                                        url: ACC_ROOT + "Events/GetsEventNotification",
                                                        data: { "operationId": operationId },
                                                        success: function (datEvent) {
                                                            if (datEvent.records > 0) {
                                                                $.ajax({
                                                                    type: "POST",
                                                                    url: ACC_ROOT + "Events/GetsEventAuthorization",
                                                                    data: { "operationId": operationId },
                                                                    success: function (data) {
                                                                        if (data.records > 0) {
                                                                            $("#dlgEventsAuthorization").dialog("open");
                                                                        }
                                                                    }
                                                                });
                                                            }
                                                            else {
                                                                savePaymentOrder();
                                                            }
                                                        }
                                                    });
                                                }
                                                else {
                                                    savePaymentOrder();
                                                }
                                            }
                                        }
                                    });
                                    }, 500);
                                }
                            }
                        }
                    }
                }
                else {

                    //MSJ NO EXISTEN MOVIMIENTOS INGRESADOS                    
                    $.UifDialog('alert', { 'message': Resources.MovementsTypeValidation, 'title': Resources.PaymentOrder });

                }

            }
            else {
                var msj = Resources.RequiredFieldsMissing + " " + Resources.Beneficiary + " : " + Resources.DocumentNumber + " / " + Resources.PayerName;
                $.UifDialog('alert', { 'message': msj, 'title': Resources.Validation_message });
            }
        //}
                
        
    } //validaciòn del form
});



//****************************************************************************************************************
//****************************************************************************************************************
// INICIO // S E C C I Ó N    D E   P R I M A S    P O R    C O B R A R
//****************************************************************************************************************
//****************************************************************************************************************

// Botón X
$("#CloseModalButton").click(function () {
    var amount = 0;
    if (viewBagAmount != "") {
        amount = parseFloat(ClearFormatCurrency(viewBagAmount.replace("", ",")));
    }

    GetDebitsAndCreditsMovementTypes(tempImputationId, amount);

    SetDataPremiumReceivableEmpty();
    ClearFields();
    HideFields();

    setTimeout(function () {
        SetTotalApplication();
        $("#TotalControl").val(FormatCurrency(FormatDecimal(
              SetMainTotalControl(ClearFormatCurrency($("#PaymentOrderGenerationAmount").val().replace("", ",")),
                                                      TotalDebit(), TotalCredit(), 1))));
    }, 2000);
    $('#ModalPremiums').UifModal('hide');
});

//*********************************************************************************************************************
//*********************************************************************************************************************
// FIN // S E C C I Ó N    D E   P R I M A S    P O R    C O B R A R

$("#btnPaymentOrderBankAccount").click(function () {
    if (beneficiaryIndividualId > 0) {
        var controller = ACC_ROOT + "PaymentOrders/GetBankAccountByBeneficiaryId?beneficiaryId=" +
                          beneficiaryIndividualId + "&isSearchPaymentOrders=" + isSearchPaymentOrders;

        $('#modalBankAccounts').find(".modal-title").text(Resources.BankAccountsOf + ' ' + $("#BeneficiaryNamePaymentOrder").val());
        $('#modalBankAccounts').UifModal('showLocal', Resources.BankAccountsOf + ' ' + $("#BeneficiaryNamePaymentOrder").val());
        $("#modalBankAccounts").find("#BankAccounts").UifDataTable({ source: controller });
    }
});

function preReceiptAmount(identifier) {
    var clearAmount = ClearFormatCurrency($('#' + identifier).val());
    var validAmount = clearAmount.split(".");

    if (validAmount[1] == 0) {
        clearAmount = validAmount[0];
    }
    $('#' + identifier).val(clearAmount.trim());
}

function setFormatPayment() {

    if ($("#PaymentOrderGenerationAmount").val() != "") {

        var paymentAmount = $("#PaymentOrderGenerationAmount").val();
        var index = paymentAmount.indexOf("$");

        if (index == 0) {

            preReceiptAmount('PaymentOrderGenerationAmount'); 
            paymentAmount = $("#PaymentOrderGenerationAmount").val();
        }

        $("#PaymentOrderGenerationAmount").val("$ " + FormatDecimal(paymentAmount, "2", ".", ","));
        $("#PaymentOrderIncomeAmount").val("$ " + FormatDecimal(paymentAmount, "2", ".", ","));

        if (paymentAmount.length <= 15) {
            $('#PaymentOrderGenerationAmount-error').remove();
        }

    }
}

