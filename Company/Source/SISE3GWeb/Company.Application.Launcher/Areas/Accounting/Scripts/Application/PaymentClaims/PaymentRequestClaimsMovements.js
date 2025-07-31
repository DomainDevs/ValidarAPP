/*---------------------------------------------------------------------------------------------------------------------------------*/
/*                                            DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                           */
/*---------------------------------------------------------------------------------------------------------------------------------*/
var tempImputationId = 0;

setTimeout(function () {
    tempImputationId = $("#ViewBagTempImputationId").val();
}, 1000);

var claimsIndividualId = -1;
var insuredId = 0;
var editClaims = -1;
var agentId = 0;
var creditAmount = 0;
var debitAmount = 0;


/*---------------------------------------------------------------------------------------------------------------------------------*/
/*                                                     DEFINICION DE FUNCIONES                                                     */
/*---------------------------------------------------------------------------------------------------------------------------------*/

if ($("#ViewBagBranchDisable").val() == "1") {
    setTimeout(function () {
        $("#selectClaimsBranch").attr("disabled", "disabled");
    }, 300);
}
else {
    $("#selectClaimsBranch").removeAttr("disabled");
}

function LoadPaymentClaimsMovementsPersonTypes() {
    PaymentRequestClaimsMovementsRequest.GetPersonTypes().done(function (data) {
        $("#selectClaimsSearchBy").UifSelect({ sourceData: data.data });
    });
}
function LoadPaymentClaimsMovementsBranchs(branchUserDefault) {
    PaymentRequestClaimsMovementsRequest.GetBranchs().done(function (data) {
        if (branchUserDefault == null || branchUserDefault == 0) {
            $("#selectClaimsBranch").UifSelect({ sourceData: data.data });
        }
        else {
            $("#selectClaimsBranch").UifSelect({ sourceData: data.data, selectedId: branchUserDefault });
        }
    });
}

function LoadPaymentClaimsMovementsSalesPointByBranchId(branchUserDefault, salePointBranchUserDefault) {
    PaymentRequestClaimsMovementsRequest.GetSalesPointByBranchId(branchUserDefault).done(function (data) {
        if (salePointBranchUserDefault == null || salePointBranchUserDefault == 0) {
            $("#selectClaimsSalePoint").UifSelect({ sourceData: data.data });
        }
        else {
            $("#selectClaimsSalePoint").UifSelect({ sourceData: data.data, selectedId: salePointBranchUserDefault });
        }
    });
}

function LoadPaymentClaimsMovementsPrefixes() {
    PaymentRequestClaimsMovementsRequest.GetPrefixes().done(function (data) {
        $("#selectClaimsPrefix").UifSelect({ sourceData: data.data });
    });
}

function LoadPaymentClaimsMovementsRequestTypes() {
    PaymentRequestClaimsMovementsRequest.GetRequestTypes().done(function (data) {
        $("#selectClaimsRequestType").UifSelect({ sourceData: data.data });
    });
}

function LoadPaymentClaimsMovementsCompanies(idCompany) {
    PaymentRequestClaimsMovementsRequest.GetAccountingCompanies().done(function (data) {
        if (isMulticompany == 0)
            $("#selectClaimsCompany").UifSelect({ sourceData: data.data, selectedId: idCompany, enable: false });
        else
            $("#selectClaimsCompany").UifSelect({ sourceData: data.data });
    });
}
//setTimeout(function () {
//    GetClaimsRequestSalePoints();
//}, 1000);

$(document).ready(function () {
    /*---------------------------------------------------------------------------------------------------------------------------------*/
    /*                                                        ACCIONES / EVENTOS                                                       */
    /*---------------------------------------------------------------------------------------------------------------------------------*/


    $("#modalClaims").find("#requestClaimsListView").UifListView();

    $("#modalClaims").find("#claimsListView").UifListView(
    {
        autoHeight: true,
        source: ACC_ROOT + "PaymentClaims/GetTempClaimsPaymentRequest?imputationId=" + $("#ViewBagTempImputationId").val() + "&isPaymentVarious=false",
        customDelete: false,
        customAdd: false,
        customEdit: true,
        add: false,
        edit: false,
        delete: true,
        displayTemplate: "#claims-display-template",
        deleteCallback: deleteClaimsCallback
    });



    ///////////////////////////////////////////////////////
    /// Combo sucursal - pagos siniestros               ///
    ///////////////////////////////////////////////////////
    $("#modalClaims").find("#selectClaimsBranch").on('itemSelected', function (event, selectedItem) {
        $("#modalClaims").find("#alertForm").UifAlert('hide');
        // Se obtiene los puntos de venta
        if (selectedItem.Id > 0) {
            var controller = ACC_ROOT + "Common/GetSalesPointByBranchId?branchId=" + selectedItem.Id;
            $("#selectClaimsSalePoint").UifSelect({ source: controller });
        }
    });

    ///////////////////////////////////////////////////////
    /// Combo buscar por - pagos siniestros             ///
    ///////////////////////////////////////////////////////
    $("#modalClaims").find("#selectClaimsSearchBy").on('itemSelected', function (event, selectedItem) {
        $("#modalClaims").find("#alertForm").UifAlert('hide');
        if ($("#selectClaimsSearchBy").val() != "") {

            switch ($("#selectClaimsSearchBy").val()) {

                case $("#ViewBagSupplierCode").val(): // 1 Proveedor  // 10 en BE

                    $("#modalClaims").find("#TitleAgentNumber").hide();
                    $("#modalClaims").find("#TitleAgentName").hide();
                    $("#modalClaims").find("#TitleInsuredNumber").hide();
                    $("#modalClaims").find("#TitleInsuredName").hide();
                    $("#modalClaims").find("#TitleSupplierNumber").show();
                    $("#modalClaims").find("#TitleSupplierName").show();

                    break;

                case $("#ViewBagProducerCode").val(): // 10:  //PRODUCTOR = AGENTE   -- agente 1 en BE

                    $("#modalClaims").find("#TitleAgentNumber").show();
                    $("#modalClaims").find("#TitleAgentName").show();
                    $("#modalClaims").find("#TitleInsuredNumber").hide();
                    $("#modalClaims").find("#TitleInsuredName").hide();
                    $("#modalClaims").find("#TitleSupplierNumber").hide();
                    $("#modalClaims").find("#TitleSupplierName").hide();

                    break;

                case $("#ViewBagInsuredCode").val():  //2: ASEGURADO

                    $("#modalClaims").find("#TitleAgentNumber").hide();
                    $("#modalClaims").find("#TitleAgentName").hide();
                    $("#modalClaims").find("#TitleInsuredNumber").show();
                    $("#modalClaims").find("#TitleInsuredName").show();
                    $("#modalClaims").find("#TitleSupplierNumber").hide();
                    $("#modalClaims").find("#TitleSupplierName").hide();

                    break;

                default:

                    $("#modalClaims").find("#TitleAgentNumber").hide();
                    $("#modalClaims").find("#TitleAgentName").hide();
                    $("#modalClaims").find("#TitleInsuredNumber").hide();
                    $("#modalClaims").find("#TitleInsuredName").hide();
                    $("#modalClaims").find("#TitleSupplierNumber").hide();
                    $("#modalClaims").find("#TitleSupplierName").hide();

                    break;
            }
            $("#requestClaimsListView").UifListView();
        }
        else {
            $("#modalClaims").find("#TitleAgentNumber").hide();
            $("#modalClaims").find("#TitleAgentName").hide();
            $("#modalClaims").find("#TitleInsuredNumber").hide();
            $("#modalClaims").find("#TitleInsuredName").hide();
            $("#modalClaims").find("#TitleSupplierNumber").hide();
            $("#modalClaims").find("#TitleSupplierName").hide();
            SetRequestClaimsFieldEmpty();
        }
    });


    //////////////////////////////////////////////////////
    // Autocomplete documento agente - pagos siniestros //
    //////////////////////////////////////////////////////
    $("#modalClaims").find('#ClaimsAgentDocumentNumber').on('itemSelected', function (event, selectedItem) {
        $("#modalClaims").find("#alertForm").UifAlert('hide');
        agentId = selectedItem.IndividualId;
        claimsIndividualId = selectedItem.IndividualId;
        if (agentId > 0) {
            $('#ClaimsAgentDocumentNumber').val(selectedItem.DocumentNumber);
            $('#ClaimsAgentName').val(selectedItem.Name);
            agentType = selectedItem.AgentType;
            agentTypeId = selectedItem.AgentTypeId;
            agentAgencyId = selectedItem.AgentAgencyId;
            branchId = selectedItem.BranchId;
        }
        else {
            $('#ClaimsAgentDocumentNumber').val("");
            $('#ClaimsAgentName').val("");
            agentType = 0;
            agentTypeId = 0;
            agentAgencyId = 0;
            branchId = -1;
        }
    });

    ///////////////////////////////////////////////////
    // Autocomplete nombre agente - pagos siniestros //
    ///////////////////////////////////////////////////
    $("#modalClaims").find('#ClaimsAgentName').on('itemSelected', function (event, selectedItem) {
        $("#modalClaims").find("#alertForm").UifAlert('hide');
        agentId = selectedItem.IndividualId;
        claimsIndividualId = selectedItem.IndividualId;
        if (agentId > 0) {
            $('#ClaimsAgentDocumentNumber').val(selectedItem.DocumentNumber);
            $('#ClaimsAgentName').val(selectedItem.Name);
            agentType = selectedItem.AgentType;
            agentTypeId = selectedItem.AgentTypeId;
            agentAgencyId = selectedItem.AgentAgencyId;
            branchId = selectedItem.BranchId;
        }
        else {
            $('#ClaimsAgentDocumentNumber').val("");
            $('#ClaimsAgentName').val("");
            agentType = 0;
            agentTypeId = 0;
            agentAgencyId = 0;
            branchId = -1;
        }
    });

    /////////////////////////////////////////////////////////
    // Autocomplete documento asegurado - pagos siniestros //
    /////////////////////////////////////////////////////////
    $("#modalClaims").find('#ClaimsInsuredDocumentNumber').on('itemSelected', function (event, selectedItem) {
        $("#modalClaims").find("#alertForm").UifAlert('hide');
        insuredId = selectedItem.Id;
        claimsIndividualId = selectedItem.Id;
        if (insuredId > 0) {
            $('#ClaimsInsuredDocumentNumber').val(selectedItem.DocumentNumber);
            $('#ClaimsInsuredName').val(selectedItem.Name);
        }
        else {
            $('#ClaimsInsuredDocumentNumber').val("");
            $('#ClaimsInsuredName').val("");
        }
    });

    //////////////////////////////////////////////////////
    // Autocomplete nombre asegurado - pagos siniestros //
    //////////////////////////////////////////////////////
    $("#modalClaims").find('#ClaimsInsuredName').on('itemSelected', function (event, selectedItem) {
        $("#modalClaims").find("#alertForm").UifAlert('hide');
        insuredId = selectedItem.Id;
        claimsIndividualId = selectedItem.Id;
        if (insuredId > 0) {
            $('#ClaimsInsuredDocumentNumber').val(selectedItem.DocumentNumber);
            $('#ClaimsInsuredName').val(selectedItem.Name);
        }
        else {
            $('#ClaimsInsuredDocumentNumber').val("");
            $('#ClaimsInsuredName').val("");
        }
    });

    /////////////////////////////////////////////////////////
    // Autocomplete documento proveedor - pagos siniestros //
    /////////////////////////////////////////////////////////
    $("#modalClaims").find('#ClaimsSupplierDocumentNumber').on('itemSelected', function (event, selectedItem) {
        $("#modalClaims").find("#alertForm").UifAlert('hide');
        supplierId = selectedItem.IndividualId;
        claimsIndividualId = selectedItem.IndividualId;
        if (supplierId > 0) {
            $('#ClaimsSupplierDocumentNumber').val(selectedItem.DocumentNumber);
            $('#ClaimsSupplierName').val(selectedItem.Name);
        }
        else {
            $('#ClaimsSupplierDocumentNumber').val("");
            $('#ClaimsSupplierName').val("");
        }
    });

    //////////////////////////////////////////////////////
    // Autocomplete nombre proveedor - pagos siniestros //
    //////////////////////////////////////////////////////
    $("#modalClaims").find('#ClaimsSupplierName').on('itemSelected', function (event, selectedItem) {
        $("#modalClaims").find("#alertForm").UifAlert('hide');
        supplierId = selectedItem.IndividualId;
        claimsIndividualId = selectedItem.IndividualId;
        if (supplierId > 0) {
            $('#ClaimsSupplierDocumentNumber').val(selectedItem.DocumentNumber);
            $('#ClaimsSupplierName').val(selectedItem.Name);
        }
        else {
            $('#ClaimsSupplierDocumentNumber').val("");
            $('#ClaimsSupplierName').val("");
        }
    });


    /////////////////////////////////////
    /// Botón buscar                  ///
    /////////////////////////////////////
    $("#modalClaims").find("#ClaimsSearch").click(function () {
        //$("#modalClaims").find("alertForm").UifAlert('hide');

        $("#addClaimsForm").validate();

        if ($("#addClaimsForm").valid()) {

            if (ValidateClaimsSearchForm() == true) {
                var claimsBranchId = -1;
                var claimsPrefixId = -1;
                var claimsPaymentSourceId = -1;

                if ($("#selectClaimsBranch").val() == "") {
                    claimsBranchId = -1;
                }
                else {
                    claimsBranchId = $("#selectClaimsBranch").val();
                }

                if ($("#selectClaimsPrefix").val() == "") {
                    claimsPrefixId = -1;
                }
                else {
                    claimsPrefixId = $("#selectClaimsPrefix").val();
                }

                if ($("#selectClaimsRequestType").val() == "") {
                    claimsPaymentSourceId = -1;
                }
                else {
                    claimsPaymentSourceId = $("#selectClaimsRequestType").val();
                }

                $("#requestClaimsListView").UifListView(
                {
                    autoHeight: true,
                    source: ACC_ROOT + "PaymentClaims/SearchClaimsPaymentRequest?searchBy=" +
                            $("#selectClaimsSearchBy").val() + "&individualId=" + claimsIndividualId +
                            "&prefixId=" + parseInt(claimsPrefixId) + "&branchId=" + parseInt(claimsBranchId) +
                            "&claimNumber=" + $("#ClaimsDenunciationNumber").val() +
                            "&requestNumber=" + $("#ClaimsRequestNumber").val() +
                            "&dateFrom=" + $("#ClaimsDateIncomeFrom").val() +
                            "&dateTo=" + $("#ClaimsDateIncomeTo").val() +
                            "&paymentSourceId=" + parseInt(claimsPaymentSourceId),
                    customDelete: false,
                    customAdd: false,
                    customEdit: true,
                    add: false,
                    edit: true,
                    delete: false,
                    displayTemplate: "#request-claims-display-template",
                });

                $("#addClaimsForm").formReset();
                SetClaimsAccountingCompany();

                $("#modalClaims").find("#TitleAgentNumber").hide();
                $("#modalClaims").find("#TitleAgentName").hide();
                $("#modalClaims").find("#TitleInsuredNumber").hide();
                $("#modalClaims").find("#TitleInsuredName").hide();
                $("#modalClaims").find("#TitleSupplierNumber").hide();
                $("#modalClaims").find("#TitleSupplierName").hide();
            }

            setTimeout(function () {
                $("#modalClaims").find("#alertForm").UifAlert('hide');
            }, 3000);
        }
    });


    /////////////////////////////////////////////////////////////////////////////
    /// Controla que la fecha final sea mayor a la inicial - pagos siniestros ///
    /////////////////////////////////////////////////////////////////////////////
    $("#modalClaims").find("#ClaimsDateIncomeFrom").blur(function () {
        $("#modalClaims").find("#alertForm").UifAlert('hide');
        if ($("#modalClaims").find("#ClaimsDateIncomeFrom").val() != '') {
            if (IsDate($("#modalClaims").find("#ClaimsDateIncomeFrom").val())) {
                if ($("#modalClaims").find("#ClaimsDateIncomeTo").val() != "") {
                    if (CompareDates($("#modalClaims").find("#ClaimsDateIncomeFrom").val(), $("#modalClaims").find("#ClaimsDateIncomeTo").val()) == true) {
                        $("#modalClaims").find("#alertForm").UifAlert('show', Resources.ValidateDateTo, "warning");
                        $("#modalClaims").find("#ClaimsDateIncomeFrom").val("");
                    }
                }
            }
            else {
                $("#modalClaims").find("#alertForm").UifAlert('show', Resources.InvalidDates, "warning");
                $("#modalClaims").find("#ClaimsDateIncomeFrom").val("");
            }
        }
    });


    /////////////////////////////////////////////////////////////////////////////
    /// Controla que la fecha final sea mayor a la inicial - pagos siniestros ///
    /////////////////////////////////////////////////////////////////////////////
    $("#modalClaims").find("#ClaimsDateIncomeTo").blur(function () {
        $("#modalClaims").find("#alertForm").UifAlert('hide');
        if ($("#modalClaims").find("#ClaimsDateIncomeTo").val() != '') {
            if (IsDate($("#modalClaims").find("#ClaimsDateIncomeTo").val()) == true) {
                if ($("#modalClaims").find("#ClaimsDateIncomeFrom").val() != '') {
                    if (CompareDates($("#modalClaims").find("#ClaimsDateIncomeFrom").val(), $("#modalClaims").find("#ClaimsDateIncomeTo").val())) {
                        $("#modalClaims").find("#alertForm").UifAlert('show', Resources.ValidateDateFrom, "warning");
                        $("#modalClaims").find("#ClaimsDateIncomeTo").val("");
                    }
                }
            } else {
                $("#modalClaims").find("#alertForm").UifAlert('show', Resources.InvalidDates, "warning");
                $("#modalClaims").find("#ClaimsDateIncomeTo").val("");
            }
        }
    });


    ///////////////////////////////////////////////////////
    // Botón aceptar movimientos pagos siniestros modal ///
    ///////////////////////////////////////////////////////
    $("#modalClaims").find('#ClaimsAcceptMovement').click(function () {
        $("#modalClaims").find("#alertForm").UifAlert('hide');
        var movements = $("#claimsListView").UifListView("getData");

        if (movements != null) {
            for (var j = 0; j < movements.length; j++) {
                if (movements[j].DebitCreditName == "Crédito") {
                    creditAmount = creditAmount + parseFloat(ClearFormatCurrency(String(movements[j].Amount).replace("", ",")));
                } else {
                    debitAmount = debitAmount + parseFloat(ClearFormatCurrency(String(movements[j].Amount).replace("", ",")));
                }
            }
        }

        if ($("#ReceiptAmount").val() == "") {
            $("#ReceiptAmount").val("0");
        }

        amount = $("#ReceiptAmount").val();
        if (isNaN(amount)) {
            amount = 0;
        }
        else {
            amount = parseFloat(ClearFormatCurrency($("#ReceiptAmount").val()));
        }

        $('#modalClaims').UifModal('hide');

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
        //GetDebitsAndCreditsMovementTypes(tempImputationId, amount);

        

        CheckLoadedMovements();
        loadedMovementsPromise.then(function(isLoaded){
            if(isLoaded){
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
    });

    /////////////////////////////////////
    // ListView - pagos siniestros     //
    /////////////////////////////////////
    $("#modalClaims").find('#requestClaimsListView').on('rowEdit', function (event, data, index) {
        $("#modalClaims").find("#alertForm").UifAlert('hide');

        var keyValid = data.ClaimCode + "*" + data.BranchId + "*" + data.CurrencyId + "*" + data.PayerBeneficiaryId +
                       "*" + data.PaymentRequestCode + "*" + data.RegistrationDate;
        var totalAmount = data.TotalAmount;

        if (ValidateDuplicateClaims(keyValid) == false) {
            $.ajax({
                async: false,
                type: "POST",
                url: ACC_ROOT + "PaymentClaims/SaveClaimsPaymentRequestItem",
                data: { "claimsPaymentRequestItem": SetDataClaimsPaymentRequestItem(data) },
                success: function (dataClaims) {
                    if (dataClaims.success == false) {
                        $("#modalClaims").find("#alertForm").UifAlert('show', dataClaims.result, "warning");
                    }
                    else {
                        var typeImputation = "";

                        if (dataClaims.result.Id > 0) {
                            if (dataClaims.result.TypeImputation == 0) {
                                typeImputation = Resources.IsRegisteredTemporary;
                            }
                            else if (dataClaims.result.TypeImputation == 1) {
                                typeImputation = Resources.IsRegisteredReal;
                            }

                            var message = Resources.PaymentRegistered + ": " + data.PaymentRequestNumber + " " + typeImputation + ": " +
                                dataClaims.result.Id;

                            $("#modalClaims").find("#alertForm").UifAlert('show', message, "warning");
                        }
                        else {
                            RefreshRequestClaimsMovements();
                            SetDataClaimsPaymentRequestItemEmpty();

                            var totalClaimsApply = $("#TotalRequestClaimsMovement").text().replace("$", "").replace(/,/g, "").replace(" ", "");
                            totalClaimsApply = parseFloat(totalClaimsApply);

                            totalClaimsApply += totalAmount;
                            $("#TotalRequestClaimsMovement").text("$ " + NumberFormatSearch(totalClaimsApply, "2", ".", ","));
                            totalClaimsApply = 0;
                        }
                    }
                }
            });
        }
        else {
            $("#modalClaims").find("#alertForm").UifAlert('show', Resources.RecordAlreadyExists, "warning");
        }

        editClaims = index;
    });

    $("#modalClaims").find('#claimsListView').on('rowDelete', function (event, data) {
        SetClaimsTotalMovement();
    });

});


///////////////////////////////////////////////////////////////
// Valida el ingreso de campos obligatorios para la búsqueda //
///////////////////////////////////////////////////////////////
function ValidateClaimsSearchForm() {
    if ($('#selectClaimsSearchBy').val() == "") {        
        // Ajuste Jira SMT-1759 Inicio
        //$("#modalClaims").find("#alertForm").UifAlert('show', Resources.SelectCurrency, "warning");
        $("#modalClaims").find("#alertForm").UifAlert('show', Resources.SelectSearchBy, "warning");
        // Ajuste Jira SMT-1759 Fin
        return false;
    }
    return true;
}

////////////////////////////////////////////////////////////////////////////
/// Validación de duplicados movimientos solicitudes de pagos siniestros ///
////////////////////////////////////////////////////////////////////////////
function ValidateDuplicateClaims(number) {
    var duplicate = false;

    // Items a aplicar
    var requestsToApply = $("#claimsListView").UifListView("getData");

    if (requestsToApply != null) {
        for (var k = 0; k < requestsToApply.length; k++) {
            var keyValid = requestsToApply[k].ClaimCode + "*" + requestsToApply[k].BranchId + "*" + requestsToApply[k].CurrencyId +
                           "*" + requestsToApply[k].PayerBeneficiaryId + "*" + requestsToApply[k].PaymentRequestCode +
                           "*" + requestsToApply[k].RegistrationDate;
            if (keyValid == number) {
                duplicate = true;
                break;
            }
            else {
                duplicate = false;
            }
        }
    }

    return duplicate;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
/// Realiza la consulta de solicitudes de pagos siniestros asociados a un temporal de imputación ///
////////////////////////////////////////////////////////////////////////////////////////////////////
function RefreshRequestClaimsMovements() {
    $("#claimsListView").UifListView(
    {
        autoHeight: true,
        source: ACC_ROOT + "PaymentClaims/GetTempClaimsPaymentRequest?imputationId=" + tempImputationId +
                                       "&isPaymentVarious=false",
        customDelete: false,
        customAdd: false,
        customEdit: false,
        add: false,
        edit: false,
        delete: true,
        displayTemplate: "#claims-display-template",
        deleteCallback: deleteClaimsCallback
    });

    setTimeout(function () {
        SetClaimsTotalMovement();
    }, 2000);
}


//////////////////////////////////////////////////////////////////
// Setear el total de la listview solicitud de pagos siniestros //
//////////////////////////////////////////////////////////////////
function SetRequestClaimsTotalMovement() {
    var totalMovement = 0;

    var requestClaims = $("#requestClaimsListView").UifListView("getData");

    if (requestClaims != null) {

        for (var j = 0; j < requestClaims.length; j++) {
            var requestAmount = String(requestClaims[j].TotalAmount).replace("$", "").replace(/,/g, "").replace(" ", "");
            totalMovement += parseFloat(requestAmount);
        }
    }
    else {
        $("#TotalRequestClaimsMovement").text("");
    }
    $("#TotalRequestClaimsMovement").text("$ " + NumberFormatSearch(totalMovement, "2", ".", ","));
}

//////////////////////////////////////////////////////////////////
// Setear el total de la listview solicitud de pagos siniestros //
//////////////////////////////////////////////////////////////////
function SetClaimsTotalMovement() {
    var totalMovement = 0;

    var requestClaims = $("#claimsListView").UifListView("getData");

    if (requestClaims != null) {

        for (var j = 0; j < requestClaims.length; j++) {
            var requestAmount = String(requestClaims[j].Amount).replace("$", "").replace(/,/g, "").replace(" ", "");
            totalMovement += parseFloat(requestAmount);
        }
    }
    else {
        $("#TotalClaimsMovement").text("");
    }
    $("#TotalClaimsMovement").text("$ " + NumberFormatSearch(totalMovement, "2", ".", ","));
}

///////////////////////////////////////////////////////////////
/// Setea los campos a valores iniciales - pagos siniestros ///
///////////////////////////////////////////////////////////////
function SetRequestClaimsFieldEmpty() {
    $("#selectClaimsSearchBy").val("");
    $("#ClaimsAgentDocumentNumber").val("");
    $("#ClaimsInsuredDocumentNumber").val("");
    $("#ClaimsSupplierDocumentNumber").val("");
    $("#ClaimsAgentName").val("");
    $("#ClaimsInsuredName").val("");
    $("#ClaimsSupplierName").val("");
    $("#selectClaimsSalePoint").val("");
    $("#selectClaimsCompany").val("");
    $("#selectClaimsPrefix").val("");
    $("#ClaimsRequestNumber").val("");
    $("#ClaimsDenunciationNumber").val("");
    $("#selectClaimsRequestType").val("");
    $("#ClaimsDateIncomeFrom").val("");
    $("#ClaimsDateIncomeTo").val("");
}

///////////////////////////////////////////////////////////////////
/// Setea la compañía y sucursal por default - pagos siniestros ///
///////////////////////////////////////////////////////////////////
function SetClaimsAccountingCompany() {
    if (isMulticompany == 0) {
        $("#selectClaimsCompany").val(accountingCompanyDefault);
        $("#selectClaimsCompany").attr("disabled", "disabled");
    }
    else {
        $("#selectClaimsCompany").removeAttr("disabled");
    }
}


/////////////////////////////////////////////////////////////////////////////////////////
/// Setear el modelo de solicitudes de pagos siniestros con los valores seleccionados ///
/////////////////////////////////////////////////////////////////////////////////////////
function SetDataClaimsPaymentRequestItem(rowRequestApply) {

    oClaimsPaymentRequestItem.TempClaimPaymentCode = 0,
    oClaimsPaymentRequestItem.TempImputationCode = tempImputationId,
    oClaimsPaymentRequestItem.PaymentRequestCode = rowRequestApply.PaymentRequestCode,
    oClaimsPaymentRequestItem.ClaimCode = rowRequestApply.ClaimCode,
    oClaimsPaymentRequestItem.BeneficiaryId = rowRequestApply.PayerBeneficiaryId,
    oClaimsPaymentRequestItem.CurrencyCode = rowRequestApply.CurrencyId,
    oClaimsPaymentRequestItem.IncomeAmount = ReplaceDecimalPoint(ClearFormatCurrency(String(rowRequestApply.TotalAmount).replace("", ",")));
    oClaimsPaymentRequestItem.RegistrationDate = rowRequestApply.RegistrationDate,
    oClaimsPaymentRequestItem.EstimationDate = rowRequestApply.EstimatedDate,
    oClaimsPaymentRequestItem.BussinessType = rowRequestApply.BusinessTypeId,
    oClaimsPaymentRequestItem.RequestType = rowRequestApply.PaymentSourceCode,
    oClaimsPaymentRequestItem.PaymentNum = rowRequestApply.Quota,
    oClaimsPaymentRequestItem.PaymentExpirationDate = rowRequestApply.ExpirationDateQuota;
    oClaimsPaymentRequestItem.PaymentRequestNumber = rowRequestApply.PaymentRequestNumber;

    return oClaimsPaymentRequestItem;
}

///////////////////////////////////////////////////////////////////////////////
/// Setea el modelo de solictudes de pagos siniestros con valores iniciales ///
///////////////////////////////////////////////////////////////////////////////
function SetDataClaimsPaymentRequestItemEmpty() {

    oClaimsPaymentRequestItem = {
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

    return oClaimsPaymentRequestItem;
}

var saveClaimsCallback = function (deferred, data) {
    $("#claimsListView").UifListView("addItem", data);
    deferred.resolve();
};

var editClaimsCallback = function (deferred, data) {
    data.NombreEmpresa = "Sistran";
    deferred.resolve(data);
};

var deleteClaimsCallback = function (deferred, data) {
    deferred.resolve();

    setTimeout(function () {

        $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "PaymentClaims/DeleteClaimsPaymentRequestItem",
            data: { "claimsPaymentRequestId": data.TempClaimPaymentCode, "imputationId": tempImputationId },
            success: function () {
                RefreshRequestClaimsMovements();
                SetClaimsTotalMovement();
            }
        });

        SetClaimsTotalMovement();
    }, 1000);
};

function GetClaimsRequestSalePoints() {
    if (isMulticompany == 0) {
        $("#selectClaimsCompany").val(accountingCompanyDefault);
        $("#selectClaimsCompany").attr("disabled", true);
    }
    else {
        $("#selectClaimsCompany").attr("disabled", false);
    }
    if (branchUserDefault > 0) {
        $("#selectClaimsSearchBy").val(branchUserDefault);

        if (branchUserDefault) {
            LoadPaymentClaimsMovementsSalesPointByBranchId(branchUserDefault, $("#ViewBagSalePointBranchUserDefault").val())
        }
    }
    else {
        $("#selectClaimsSearchBy").val("");
    }
};

