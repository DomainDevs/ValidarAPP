/*-----------------------------------------------------------------------------------------------------------------------------------------*/
//                                                                      DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS
/*-----------------------------------------------------------------------------------------------------------------------------------------*/

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

/* Fin pagos siniestros y varios */


var amount = 0;
var creditAmount = 0;
var debitAmount = 0;
var tempImputationId;
var isMulticompany;
var accountingCompanyDefault;
var branchUserDefault;
var salePointBranchUserDefault;
var userId;
var userNick;

var applyBillId;
var applyReceiptNumber;
var applyDepositer;
var applyAmount;
var applyLocalAmount;
var applyBranchId;
var applyBranchDescription;
var applyIncomeConcept;
var applyPostedValue;
var applyDescription;
var applyAccountingDate;
var applyComments;
var applyTransactionNumber;
var applyIndividualId = 0;
var applyCollecId;

$(document).ready(function () {

    applyCollecId = $("#ViewBagApplyCollecId").val();
    applyAccountingDate = $("#ViewBagDateAccounting").val();

    if ($("#ViewBagImputationType").val() == Resources.ImputationTypeBill) {

        tempImputationId = $("#ViewBagTempImputationId").val();
        isMulticompany = $("#ViewBagParameterMulticompany").val();
        accountingCompanyDefault = $("#ViewBagAccountingCompanyDefault").val();
        branchUserDefault = $("#ViewBagBranchUserDefault").val();
        salePointBranchUserDefault = $("#ViewBagSalePointBranchUserDefault").val();
        userId = $("#ViewBagUserId").val();
        userNick = $("#ViewBagUserNick").val();
        GetDebitsAndCreditsMovementTypesReceipt($("#ViewBagTempImputationId").val(), 0);
        SetTotalApplicationReceipt();
        SetTotalControlReceipt();
        $("#ReceiptAmountApplication").trigger('blur');
    }
});


//blur para campo de importe de recibo
$("#ReceiptAmountApplication").on('blur', function () {
    var billAmount = $("#ReceiptAmountApplication").val();
    var index = billAmount.indexOf("$");
    if (index == -1) {
        $("#ReceiptAmountApplication").val("$ " + NumberFormat(billAmount, "2", ".", ","));
    }
    else {
        $("#ReceiptAmountApplication").val(billAmount);
    }
});


//****************************************************************************************************************
//****************************************************************************************************************
// INICIO // S E C C I Ó N    D E   P R I M A S    P O R    C O B R A R
//****************************************************************************************************************
//****************************************************************************************************************

// Botón Aceptar
$("#SaveSearchPoliciesButton").click(function () {
    if ($("#ViewBagImputationType").val() == "1") {
        var amount = 0;
        if ($("#ViewBagAmount").val() != "") {
            amount = parseFloat(ClearFormatCurrency($("#ViewBagAmount").val().replace("", ",")));
        }
        GetDebitsAndCreditsMovementTypesReceipt(tempImputationId, amount);

        //SetDataPremiumReceivableEmpty(); //DialogSearchPolicies
        //ClearFields(); //Accounting
        //HideFields(); //DialogSearchPolicies

        setTimeout(function () {
            SetTotalApplicationReceipt();

            var totalControl = SetMainTotalControl(parseFloat(ClearFormatCurrency($("#ReceiptAmountApplication").val().replace("", ","))),
                TotalDebit(), TotalCredit(), 2);

            $("#TotalControl").val(FormatCurrency(FormatDecimal(totalControl)));
        }, 3500);

        $('#ModalPremiums').UifModal('hide');
    }
});

// Botón X
$("#CloseModalButton").click(function () {

    var amount = parseFloat(ClearFormatCurrency($("#ViewBagAmount").val().replace("", ",")));
    GetDebitsAndCreditsMovementTypesReceipt(tempImputationId, amount);

    SetDataPremiumReceivableEmpty();//DialogSearchPolicies
    ClearFields(); //Accounting
    HideFields();//DialogSearchPolicies

    setTimeout(function () {
        SetTotalApplicationReceipt();
        $("#TotalControl").val(SetMainTotalControl(parseFloat(ClearFormatCurrency($("#ReceiptAmountApplication").val().replace("", ","))),
            TotalDebit(), TotalCredit(), 2));
    }, 3500);

    $('#ModalPremiums').UifModal('hide');
});


//*********************************************************************************************************************
//*********************************************************************************************************************
// FIN // S E C C I Ó N    D E   P R I M A S    P O R    C O B R A R

/////////////////////////////////////////
/// Botón para edición en el listview ///
/////////////////////////////////////////
$("#listViewAplicationReceipt").on('rowEdit', function (event, data, index) {
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
        LoadAgentBranchs(branchUserDefault);
        LoadAgentNatures();
        LoadAgentCurrencies();
        LoadAgentCompanies();
        $('#modalAgent').UifModal('showLocal', Resources.AgentMovementTitle + " " + Resources.DialogTitleTemporary + " " + tempImputationId);
        SetAgentFieldEmpty(); //AgentMovement
        SetAgentTotalMovement(); //AgentMovement
        SetAgentAccountingCompany(); //AgentMovement
    }
    // Cuenta corriente coaseguros
    if (data.Id == 5) {
        editCoinsurance = -1;
        $("#addCoinsuranceForm").find("#MovementBillNumber").val($("#ViewBagReceiptNumber").val());
        LoadCoinsuresMovementsBranchs();
        LoadCoinsuresMovementsCoinsuranceTypes();
        LoadCoinsuresMovementsNatures();
        LoadCoinsuresMovementsCurrencies();
        LoadCoinsuresMovementsAccountingCompanies(accountingCompanyDefault);
        $('#modalCoinsurance').UifModal('showLocal', Resources.DialogCoinsuranceMovementsTitle + " " +
            Resources.DialogTitleTemporary + " " + tempImputationId);

        SetCoinsuranceFieldEmpty();  //CoinsuranceMovements
        SetCoinsuranceTotalMovement();  //CoinsuranceMovements
        SetCoinsuranceAccountingCompany();  //CoinsuranceMovements
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
        $('#modalReinsurance').UifModal('showLocal', Resources.DialogReinsuranceMovementsTitle + " " +
            Resources.DialogTitleTemporary + " " + tempImputationId);


        $("#modalReinsurance").find("#OptionalOne").hide();
        $("#modalReinsurance").find("#OptionalTwo").hide();
        $("#modalReinsurance").find("#OptionalThree").hide();

        SetReinsuranceFieldEmpty();        //ReinsuranceMovements
        SetReinsuranceTotalMovement();     //ReinsuranceMovements
        SetReinsuranceAccountingCompany(); //ReinsuranceMovements
    }
    // Solicitud pagos varios
    if (data.Id == 8) {
        $("#modalVarious").find("#TitleAgentNumber").hide();
        $("#modalVarious").find("#TitleEmployeeNumber").hide();
        $("#modalVarious").find("#TitleSupplierNumber").hide();
        $("#modalVarious").find("#TitleAgentName").hide();
        $("#modalVarious").find("#TitleEmployeeName").hide();
        $("#modalVarious").find("#TitleSupplierName").hide();

        $('#modalVarious').UifModal('showLocal', Resources.VariousPaymentRequest + " " +
            Resources.DialogTitleTemporary + " " + tempImputationId);

        $("#modalVarious").find("#requestVariousListView").UifListView();
        LoadPersonTypes();
        LoadPaymentVariousBranchs();
        LoadPaymentVariousCompanies(accountingCompanyDefault)
        SetRequestVariousFieldEmpty();    //PaymentRequestVariousMovements.js
        RefreshRequestVariousMovements(); //PaymentRequestVariousMovements.js
        SetVariousTotalMovement();        //PaymentRequestVariousMovements.js
        SetVariousAccountingCompany();    //PaymentRequestVariousMovements.js
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

        $('#modalClaims').UifModal('showLocal', Resources.ClaimsPaymentRequestLabel + " " +
            Resources.DialogTitleTemporary + " " + tempImputationId);

        $("#modalClaims").find("#requestClaimsListView").UifListView();
        LoadPaymentClaimsMovementsPersonTypes();
        LoadPaymentClaimsMovementsBranchs()
        LoadPaymentClaimsMovementsPrefixes();
        LoadPaymentClaimsMovementsRequestTypes();
        LoadPaymentClaimsMovementsCompanies(accountingCompanyDefault);
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
        $('#modalInsuredLoan').UifModal('showLocal', Resources.DialogInsuredLoanMovementsTitle + "" +
            Resources.DialogTitleTemporary + " " + tempImputationId);
        SetInsuredLoanFieldEmpty();    //InsuredLoans.js
        SetInsuredLoanTotalMovement(); //InsuredLoans.js
    }
});



//////////////////////////////////////////
/// Botón aceptar aplicación de recibo ///
//////////////////////////////////////////
/*$("#ApplyAcceptReceipt").click(function () {

    if (TotalDebit() != 0 || TotalCredit() != 0) {
        //$.msg({ content: Resources.MessagePleaseWait, klass: 'white-on-black' });

        if (RemoveFormatMoney($("#TotalControl").val()) != 0) {
            //DIALOGO DE COMFIRMACION PARA GRABAR IMPUTATION EN TEMPORALES, ACTUALIZA ESTADO DE BILL
            SaveTemporalConfirmationReceipt();
        }
        else {
            // GRABA IMPUTATION A REALES, ELIMINA DE TEMPORALES, ACTUALIZA ESTADO DE BILL

            lockScreen();
            $.ajax({
                async: false,
                type: "POST",
                url: ACC_ROOT + "ReceiptApplication/SaveReceiptApplication",
                data: {
                    "sourceCode": $("#ReceiptNumber").val(),
                    "tempImputationId": tempImputationId,
                    "imputationTypeId": Resources.ImputationTypeBill,
                    "comments": $("#_Observations").val(),
                    "statusId": 3,
                    "individualId": applyIndividualId
                },
                success: function (data) {
                    //EXCEPCION ROLLBACK
                    if (data.success == false) {
                        $("#alert").UifAlert('show', data.result, "danger");
                    } else {

                        $("#modalSuccess").find("#receiptApplicationMessage").text(Resources.SaveRealSuccessTransactionMessage + " " + $("#TransactionNumber").val());

                        if (data.IsEnabledGeneralLedger == false) {
                            $("#modalSuccess").find("#accountingApplicationLabelDiv").hide();
                            $("#modalSuccess").find("#accountingApplicationMessageDiv").hide();
                        } else {
                            $("#modalSuccess").find("#accountingApplicationLabelDiv").show();
                            $("#modalSuccess").find("#accountingApplicationMessageDiv").show();
                        }
                        $("#modalSuccess").find("#receiptApplicationAccountingIntegrationMessage").text(data.Message);
                        $('#modalSuccess').UifModal('showLocal', Resources.ReceiptsApplication);
                    }
                    unlockScreen();
                }
            });
        }
    }
    else {
        $("#alert").UifAlert('show', Resources.MovementsTypeValidation, "warning");
    }
});	*/

var SaveTemporalConfirmationReceipt = function () {
    $.UifDialog('confirm', {
        'message': Resources.SaveTemporalConfirmationMessage,
        'title': Resources.ReceiptsApplication
    }, function (result) {
        if (result) {
            // ACTUALIZA FECHA Y USUARIO DE TEMP-IMPUTATION. ACTUALIZA ESTADO, FECHA Y USUARIO DE RECIBO - PARCIALMENTE APLICADO
            $.ajax({
                async: false,
                type: "POST",
                url: ACC_ROOT + "ReceiptApplication/UpdateReceiptApplication",
                data: {
                    "imputationId": tempImputationId, "imputationTypeId": Resources.ImputationTypeBill,
                    "sourceCode": parseInt($("#ReceiptNumber").val()), "comments": $("#_Observations").val(), "statusId": 2
                },
                success: function () {
                    $("#alert").UifAlert('show', Resources.TemporalSuccessfullySaved + " " + tempImputationId, "success");
                    //CleanGlobalVariablesReceipt();
                }
            });
        }
    });
};

/*---------------------------------------------------------------------------------------------------------------------------------------------------*/
//                                                            DEFINICION DE FUNCIONES
/*---------------------------------------------------------------------------------------------------------------------------------------------------*/

//////////////////////////////////////////////////////
/// Se refresca el listview resumen de movimientos ///
//////////////////////////////////////////////////////
function GetDebitsAndCreditsMovementTypesReceipt(tempImputationId, amount) {
    if (tempImputationId > 0) {
        $("#listViewAplicationReceipt").UifListView({
            autoHeight: true, theme: 'dark',
            source: ACC_ROOT + "ReceiptApplication/GetDebitsAndCreditsMovementTypes?tempImputationId=" + tempImputationId + "&amount=" + amount,
            customDelete: true,
            customEdit: true,
            edit: true,
            delete: false,
            displayTemplate: "#display-aplication-receipt-template"
        });
    }    
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
function SetTotalControlReceipt() {

    if ($("#ReceiptAmountApplication").val() != undefined) {

        $("#TotalControl").val(SetMainTotalControl(parseFloat(ClearFormatCurrency($("#ReceiptAmountApplication").val().replace("", ","))),
            TotalDebit(), TotalCredit(), 2));

        var receiptAmount = $("#TotalControl").val();
        $("#TotalControl").val("$ " + NumberFormat(receiptAmount, "2", ".", ","));
    }
}

//////////////////////////////////////////////
/// Setea el temporal de imputación a null ///
//////////////////////////////////////////////
function CleanGlobalVariablesReceipt() {
    tempImputationId = null;
}


///////////////////////////////////////////////////////////
// Setear el total de la listview resumen de movimientos //
///////////////////////////////////////////////////////////
function SetTotalApplicationReceipt() {
    var totalCreditMovement = 0;
    var totalDebitMovement = 0;

    $("#totalAplicationTable").UifDataTable('clear');

    var summaries = $("#listViewAplicationReceipt").UifListView("getData");

    if (summaries != null) {

        for (var j = 0; j < summaries.length; j++) {
            var debitAmount = String(summaries[j].Debits).replace("$", "").replace(/,/g, "").replace(" ", "");
            totalDebitMovement += parseFloat(debitAmount);

            var creditAmount = String(summaries[j].Credits).replace("$", "").replace(/,/g, "").replace(" ", "");
            totalCreditMovement += parseFloat(creditAmount);
        }

        totalDebitMovement = FormatCurrency(FormatDecimal(totalDebitMovement));
        totalCreditMovement = FormatCurrency(FormatDecimal(totalCreditMovement));
    }
    else {
        totalDebitMovement = FormatCurrency(FormatDecimal(0));
        totalCreditMovement = FormatCurrency(FormatDecimal(0));
    }

    var totalData = {
        TotalDebit: totalDebitMovement,
        TotalCredit: totalCreditMovement
    };
    $("#totalAplicationTable").UifDataTable('addRow', totalData);
}

function RequestReceiptTemporal(technicaltransaction) {

    // CONSULTAR SI EXISTE TEMPORAL
    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "ReceiptApplication/GetTempImputationBySourceCode",
        data: { "imputationTypeId": Resources.ImputationTypeBill, "sourceCode": technicaltransaction },
        success: function (data) {
            if (data.Id == 0) {
                // GRABA A TEMPORALES
                $.ajax({
                    async: false,
                    type: "POST",
                    url: ACC_ROOT + "ReceiptApplication/SaveTempImputation",
                    data: { "imputationTypeId": Resources.ImputationTypeBill, "sourceCode": technicaltransaction, "individualId": applyIndividualId },
                    success: function (dataImputation) {
                        tempImputationId = dataImputation.Id;
                    }
                });
            }
            else {
                // YA EXITE EN TEMPORALES
                tempImputationId = data.Id;
                GetDebitsAndCreditsMovementTypesReceipt(tempImputationId, 0);
            }
        }
    });
}
function ClearReceiptApplication() {
    applyIncomeConcept = "";
    applyPostedValue = "";
    applyDescription = "";
    applyComments = "";
    applyIndividualId = 0;
    applyAmount = 0;
    applyBranchDescription = "";
    applyDepositer = "";
    applyTransactionNumber = 0;
    SetReceiptValues();
    GetDebitsAndCreditsMovementTypesReceipt(0, 0);

}

function SetReceiptValues() {

    $("#ReceiptNumber").val(applyReceiptNumber);
    $("#DocumentNumberApplicationReceipt").val(applyDepositer);
    $("#ReceiptAmountApplication").val(FormatCurrency(FormatDecimal(applyAmount)));
    $("#BranchName").val(applyBranchDescription);
    $("#IncomeConcept").val(applyIncomeConcept);
    $("#PostdatedValue").val(applyPostedValue);
    $("#ReceiptDescription").val(applyDescription);
    $("#AccountingDate").val(applyAccountingDate);
    $("#_Observations").val(applyComments);
    $("#TransactionNumber").val(applyTransactionNumber);
    $("#TotalControl").val(FormatCurrency(FormatDecimal(applyAmount)));
}