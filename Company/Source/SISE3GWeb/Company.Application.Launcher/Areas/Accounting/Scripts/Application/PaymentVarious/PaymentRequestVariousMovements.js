/*---------------------------------------------------------------------------------------------------------------------------------*/
/*                                            DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                           */
/*---------------------------------------------------------------------------------------------------------------------------------*/

var tempImputationId = 0;

setTimeout(function () {
    tempImputationId = $("#ViewBagTempImputationId").val();
   
}, 1000);


function RowVariousModel() {
    this.RowNumberUnique;
    this.BranchId;
    this.BranchName;
    this.PrefixId;
    this.PrefixName;
    this.BusinessTypeId;
    this.BusinessTypeName;
    this.CurrencyId;
    this.CurrencyName;
    this.BeneficiaryDocumentNumber;
    this.TotalAmount;
    this.RegistrationDate;
    this.EstimatedDate;
    this.IncomeReceiptNumber;
    this.CheckPayable;
    this.PaymentRequestCode;
    this.PaymentRequestNumber;
    this.PayerBeneficiaryDocumentNumber;
    this.PayerBeneficiaryName;
    this.PaymentSourceCode;
    this.PayerBeneficiaryId;
    this.ClaimCode;
}

var variousIndividualId = 0;
var employeeId = 0;
var supplierId = 0;
var editVarious = -1;
var agentDocumentNumber = $('#VariousAgentDocumentNumber').val();
var agentName = $('#VariousAgentName').val();
var supplierDocumentNumber = $('#VariousSupplierDocumentNumber').val();
var supplierName = $('#VariousSupplierName').val();
var employeeDocumentNumber = $('#VariousEmployeeDocumentNumber').val();
var employeeName = $('#VariousEmployeeName').val();

$(document).ready(function () {
    /*---------------------------------------------------------------------------------------------------------------------------------*/
    /*                                                        ACCIONES / EVENTOS                                                       */
    /*---------------------------------------------------------------------------------------------------------------------------------*/

    $("#modalVarious").find("#requestVariousListView").UifListView();

    PaymentRequestVariousMovementsRequest.RequestVariousMovements($("#ViewBagTempImputationId").val()).done(function (data) {
        if (data) {
            $("#variousListView").UifListView(
                {
                    autoHeight: true,
                    sourceData: data,
                    customDelete: false,
                    customAdd: false,
                    customEdit: false,
                    add: false,
                    edit: false,
                    delete: true,
                    displayTemplate: "#various-display-template",
                    deleteCallback: deleteVariousCallback
                });
        }
    });

    ///////////////////////////////////////////////////////
    /// Combo sucursal - pagos varios                   ///
    ///////////////////////////////////////////////////////
    $("#modalVarious").find("#selectVariousBranch").on('itemSelected', function (event, selectedItem) {
        $("#modalVarious").find("#alertForm").UifAlert('hide');
        // Se obtiene los puntos de venta
        if (selectedItem.Id > 0) {
            var controller = ACC_ROOT + "Common/GetSalesPointByBranchId?branchId=" + selectedItem.Id;
            $("#selectVariousSalePoint").UifSelect({ source: controller });
        }

    });

    ///////////////////////////////////////////////////////
    /// Combo buscar por - pagos varios                 ///
    ///////////////////////////////////////////////////////
    $("#modalVarious").find("#selectVariousSearchBy").on('itemSelected', function (event, selectedItem) {
        
        $("#modalVarious").find("#alertForm").UifAlert('hide');
        variousIndividualId = 0;
        if ($("#selectVariousSearchBy").val() != "") {
            
            switch ($("#selectVariousSearchBy").val()) {

                case $("#ViewBagSupplierCode").val(): // 1 Proveedor 

                    $("#TitleAgentNumber").hide();
                    $("#TitleAgentName").hide();
                    $("#TitleEmployeeNumber").hide();
                    $("#TitleEmployeeName").hide();
                    $("#TitleSupplierNumber").show();
                    $("#TitleSupplierName").show();

                    break;

                case $("#ViewBagProducerCode").val(): // 10:  //PRODUCTOR = AGENTE   

                    $("#TitleAgentNumber").show();
                    $("#TitleAgentName").show();
                    $("#TitleEmployeeNumber").hide();
                    $("#TitleEmployeeName").hide();
                    $("#TitleSupplierNumber").hide();
                    $("#TitleSupplierName").hide();

                    break;

                case $("#ViewBagEmployeeCode").val(): //11:EMPLEADO 

                    $("#TitleAgentNumber").hide();
                    $("#TitleAgentName").hide();
                    $("#TitleEmployeeNumber").show();
                    $("#TitleEmployeeName").show();
                    $("#TitleSupplierNumber").hide();
                    $("#TitleSupplierName").hide();

                    break;

                
                default:

                    $("#TitleAgentNumber").hide();
                    $("#TitleAgentName").hide();
                    $("#TitleEmployeeNumber").hide();
                    $("#TitleEmployeeName").hide();
                    $("#TitleSupplierNumber").hide();
                    $("#TitleSupplierName").hide();

                    break;
            }
        }
        else {
            $("#TitleAgentNumber").hide();
            $("#TitleAgentName").hide();
            $("#TitleEmployeeNumber").hide();
            $("#TitleEmployeeName").hide();
            $("#TitleSupplierNumber").hide();
            $("#TitleSupplierName").hide();
            SetRequestVariousFieldEmpty();
        }

    });

    //////////////////////////////////////////////////
    // Autocomplete documento agente - pagos varios //
    //////////////////////////////////////////////////
    $("#modalVarious").find('#VariousAgentDocumentNumber').on('itemSelected', function (event, selectedItem) {
        $("#modalVarious").find("#alertForm").UifAlert('hide');
        agentId = selectedItem.IndividualId;
        variousIndividualId = selectedItem.IndividualId;
        if (agentId > 0) {
            $('#VariousAgentDocumentNumber').val(selectedItem.DocumentNumber);
            $('#VariousAgentName').val(selectedItem.Name);
            agentType = selectedItem.AgentType;
            agentTypeId = selectedItem.AgentTypeId;
            agentAgencyId = selectedItem.AgentAgencyId;
            branchId = selectedItem.BranchId;
            agentDocumentNumber = selectedItem.DocumentNumber;
            agentName = selectedItem.Name;
        }
        else {
            $('#VariousAgentDocumentNumber').val("");
            $('#VariousAgentName').val("");
            agentType = 0;
            agentTypeId = 0;
            agentAgencyId = 0;
            branchId = -1;
        }
    });

    ////////////////////////////////////////////////////////////////////////
    // Control de borrado de autocomplete en campo de número de documento //
    ////////////////////////////////////////////////////////////////////////
    $("#modalVarious").find("#VariousAgentDocumentNumber").on('blur', function (event) {
        setTimeout(function () {
            $('#VariousAgentDocumentNumber').val(agentDocumentNumber);
        }, 50);
    });

    ///////////////////////////////////////////////
    // Autocomplete nombre agente - pagos varios //
    ///////////////////////////////////////////////
    $("#modalVarious").find('#VariousAgentName').on('itemSelected', function (event, selectedItem) {
        $("#modalVarious").find("#alertForm").UifAlert('hide');
        agentId = selectedItem.IndividualId;
        variousIndividualId = selectedItem.IndividualId;
        if (agentId > 0) {
            $('#VariousAgentDocumentNumber').val(selectedItem.DocumentNumber);
            $('#VariousAgentName').val(selectedItem.Name);
            agentType = selectedItem.AgentType;
            agentTypeId = selectedItem.AgentTypeId;
            agentAgencyId = selectedItem.AgentAgencyId;
            branchId = selectedItem.BranchId;
            agentDocumentNumber = selectedItem.DocumentNumber;
            agentName = selectedItem.Name;
        }
        else {
            $('#VariousAgentDocumentNumber').val("");
            $('#VariousAgentName').val("");
            agentType = 0;
            agentTypeId = 0;
            agentAgencyId = 0;
            branchId = -1;
        }
    });

    ////////////////////////////////////////////////////////
    // Control de borrado de autocomplete en campo nombre //
    ////////////////////////////////////////////////////////
    $("#modalVarious").find("#VariousAgentName").on('blur', function (event) {
        setTimeout(function () {
            $('#VariousAgentName').val(agentName);
        }, 50);
    });

    //////////////////////////////////////////////////
    // Autocomplete documento empleado - pagos varios //
    //////////////////////////////////////////////////
    $("#modalVarious").find('#VariousEmployeeDocumentNumber').on('itemSelected', function (event, selectedItem) {
        $("#modalVarious").find("#alertForm").UifAlert('hide');
        employeeId = selectedItem.Id;
        variousIndividualId = selectedItem.Id;

        if (employeeId > 0) {
            $('#VariousEmployeeDocumentNumber').val(selectedItem.DocumentNumber);
            $('#VariousEmployeeName').val(selectedItem.Name);
            employeeDocumentNumber = selectedItem.DocumentNumber;
            employeeName = selectedItem.Name;
        } else {
            $('#VariousEmployeeDocumentNumber').val("");
            $('#VariousEmployeeName').val("");
        }
    });

    ////////////////////////////////////////////////////////////////////////
    // Control de borrado de autocomplete en campo de número de documento //
    ////////////////////////////////////////////////////////////////////////
    $("#modalVarious").find("#VariousEmployeeDocumentNumber").on('blur', function (event) {
        setTimeout(function () {
            $('#VariousEmployeeDocumentNumber').val(employeeDocumentNumber);
        }, 50);
    });

    /////////////////////////////////////////////////
    // Autocomplete nombre empleado - pagos varios //
    /////////////////////////////////////////////////
    $("#modalVarious").find('#VariousEmployeeName').on('itemSelected', function (event, selectedItem) {
        $("#modalVarious").find("#alertForm").UifAlert('hide');
        employeeId = selectedItem.Id;
        variousIndividualId = selectedItem.Id;
        if (employeeId > 0) {
            $('#VariousEmployeeDocumentNumber').val(selectedItem.DocumentNumber);
            $('#VariousEmployeeName').val(selectedItem.Name);
            employeeDocumentNumber = selectedItem.DocumentNumber;
            employeeName = selectedItem.Name;
        }
        else {
            $('#VariousEmployeeDocumentNumber').val("");
            $('#VariousEmployeeName').val("");
        }
    });

    ////////////////////////////////////////////////////////
    // Control de borrado de autocomplete en campo nombre //
    ////////////////////////////////////////////////////////
    $("#modalVarious").find("#VariousEmployeeName").on('blur', function (event) {
        setTimeout(function () {
            $('#VariousEmployeeName').val(employeeName);
        }, 50);
    });

    /////////////////////////////////////////////////////
    // Autocomplete documento proveedor - pagos varios //
    /////////////////////////////////////////////////////
    $("#modalVarious").find('#VariousSupplierDocumentNumber').on('itemSelected', function (event, selectedItem) {
        $("#modalVarious").find("#alertForm").UifAlert('hide');
        supplierId = selectedItem.IndividualId;
        variousIndividualId = selectedItem.IndividualId;
        if (supplierId > 0) {
            $('#VariousSupplierDocumentNumber').val(selectedItem.DocumentNumber);
            $('#VariousSupplierName').val(selectedItem.Name);
            supplierDocumentNumber = selectedItem.DocumentNumber;
            supplierName = selectedItem.Name;
        }
        else {
            $('#VariousSupplierDocumentNumber').val("");
            $('#VariousSupplierName').val("");
        }
    });

    ////////////////////////////////////////////////////////////////////////
    // Control de borrado de autocomplete en campo de número de documento //
    ////////////////////////////////////////////////////////////////////////
    $("#modalVarious").find("#VariousSupplierDocumentNumber").on('blur', function (event) {
        setTimeout(function () {
            $('#VariousSupplierDocumentNumber').val(supplierDocumentNumber);
        }, 50);
    });

    //////////////////////////////////////////////////
    // Autocomplete nombre proveedor - pagos varios //
    //////////////////////////////////////////////////
    $("#modalVarious").find('#VariousSupplierName').on('itemSelected', function (event, selectedItem) {
        $("#modalVarious").find("#alertForm").UifAlert('hide');
        supplierId = selectedItem.IndividualId;
        variousIndividualId = selectedItem.IndividualId;
        if (supplierId > 0) {
            $('#VariousSupplierDocumentNumber').val(selectedItem.DocumentNumber);
            $('#VariousSupplierName').val(selectedItem.Name);
            supplierDocumentNumber = selectedItem.DocumentNumber;
            supplierName = selectedItem.Name;
        }
        else {
            $('#VariousSupplierDocumentNumber').val("");
            $('#VariousSupplierName').val("");
        }

    });

    ////////////////////////////////////////////////////////
    // Control de borrado de autocomplete en campo nombre //
    ////////////////////////////////////////////////////////
    $("#modalVarious").find("#VariousSupplierName").on('blur', function (event) {
        setTimeout(function () {
            $('#VariousSupplierName').val(supplierName);
        }, 50);
    });


    ////////////////////////////////////////////////////////
    //// Controla que la fecha final sea mayor a la inicial ///
    ////////////////////////////////////////////////////////
    $("#modalVarious").find("#VariousDateIncomeFrom").blur(function () {
        $("#modalVarious").find("#alertForm").UifAlert('hide');
        if ($("#modalVarious").find("#VariousDateIncomeFrom").val() != '') {
            if (IsDate($("#modalVarious").find("#VariousDateIncomeFrom").val())) {
                if ($("#modalVarious").find("#VariousDateIncomeTo").val() != "") {
                    if (CompareDates($("#modalVarious").find("#VariousDateIncomeFrom").val(), $("#modalVarious").find("#VariousDateIncomeTo").val()) == true) {
                        $("#modalVarious").find("#alertForm").UifAlert('show', Resources.ValidateDateTo, "warning");
                        $("#modalVarious").find("#VariousDateIncomeFrom").val("");
                    }
                }
            }
            else {
                $("#modalVarious").find("#alertForm").UifAlert('show', Resources.InvalidDates, "warning");
                $("#modalVarious").find("#VariousDateIncomeFrom").val("");
            }
        }
    });

    ////////////////////////////////////////////////////////
    //// Controla que la fecha final sea mayor a la inicial ///
    ////////////////////////////////////////////////////////
    $("#modalVarious").find("#VariousDateIncomeTo").blur(function () {
        $("#modalVarious").find("#alertForm").UifAlert('hide');
        if ($("#modalVarious").find("#VariousDateIncomeTo").val() != '') {
            if (IsDate($("#modalVarious").find("#VariousDateIncomeTo").val()) == true) {
                if ($("#modalVarious").find("#VariousDateIncomeFrom").val() != '') {
                    if (CompareDates($("#modalVarious").find("#VariousDateIncomeFrom").val(), $("#modalVarious").find("#VariousDateIncomeTo").val())) {
                        $("#modalVarious").find("#alertForm").UifAlert('show', Resources.ValidateDateFrom, "warning");
                        $("#modalVarious").find("#VariousDateIncomeTo").val("");
                    }
                }
            } else {
                $("#modalVarious").find("#alertForm").UifAlert('show', Resources.InvalidDates, "warning");
                $("#modalVarious").find("#VariousDateIncomeTo").val("");
            }
        }
    });


    /////////////////////////////////////
    // ListView - pagos varios         //
    /////////////////////////////////////
    $("#modalVarious").find('#requestVariousListView').on('rowEdit', function (event, data, index) {
        $("#modalVarious").find("#alertForm").UifAlert('hide');
        

        var keyValid = data.ClaimCode + "*" + data.BranchId + "*" + data.CurrencyId + "*" + data.PayerBeneficiaryId + "*" +
                       data.PaymentRequestCode + "*" + data.RegistrationDate;

        if (ValidateDuplicateVarious(keyValid) == false) {
            
            lockScreen();
            setTimeout(function () {
                $.ajax({
                async: false,
                type: "POST",                
                url: ACC_ROOT + "PaymentVarious/SavePaymentRequestItem",
                data: { "paymentRequestItem": SetDataPaymentRequestItem(data) },
                success: function (data) {
                    unlockScreen();
                    var typeImputation = "";
                    if (data.Id > 0) {
                        if (data.TypeImputation == 0) {
                            typeImputation = Resources.IsRegisteredTemporary;
                        }
                        else if (data.TypeImputation == 1) {
                            typeImputation = Resources.IsRegisteredReal;
                        }

                        var message = Resources.PaymentRegistered + ": " + data.PaymentRequestNumber + " " + typeImputation + ": " +
                                      data.Id;

                        $("#modalVarious").find("#alertForm").UifAlert('show', message, "warning");
                    }
                    else {
                        RefreshRequestVariousMovements();
                        SetDataVariousPaymentRequestItemEmpty();
                     }
                }
                });
            }, 500);
        }
        else {
            $("#modalVarious").find("#alertForm").UifAlert('show', Resources.RecordAlreadyExists, "warning");
        }

        editVarious = index;
    });

    $("#modalVarious").find('#requestVariousListView').on('rowDelete', function (event, data, index) {
        var veamos = index;
        var vem = veamos;
    });

    $("#modalVarious").find('#variousListView').on('rowDelete', function (event, data) {
        SetVariousTotalMovement();
    });

    //////////////////////////
    // Botón buscar modal  ///
    //////////////////////////
    $("#modalVarious").find('#VariousSearch').click(function () {
        $("#modalVarious").find("#alertForm").UifAlert('hide')

        $("#addVariousForm").validate();

        if ($("#addVariousForm").valid()) {

            if (ValidateVariousSearchForm() == true) {
                var variousBranchId = -1;

                if ($("#selectVariousBranch").val() == "") {
                    variousBranchId = -1;
                }
                else {
                    variousBranchId = $("#selectVariousBranch").val();
                }

                // Ajuste Jira SMT-1559 Inicio
                var searchBy = $("#selectVariousSearchBy").val();
                var branchId = parseInt(variousBranchId);
                var requestNumber = $("#VariousRequestNumber").val();
                var voucherNumber = $("#VariousVoucherNumber").val();
                var dateFrom = $("#VariousDateIncomeFrom").val();
                var dateTo = $("#VariousDateIncomeTo").val();

                PaymentRequestVariousMovementsRequest.GetSearchPaymentRequestVarious(searchBy, variousIndividualId, branchId, requestNumber, voucherNumber, dateFrom, dateTo).done(function (data) {
                    if (data.success) {
                        $("#requestVariousListView").UifListView(
                            {
                                autoHeight: true,
                                sourceData: data.result,
                                customDelete: false,
                                customAdd: false,
                                customEdit: true,
                                add: false,
                                edit: true,
                                delete: true,
                                displayTemplate: "#request-various-display-template",
                                deleteCallback: deleteVariousRequestCallback
                            });
                    }
                    else {
                        $("#modalVarious").find("#alertForm").UifAlert('show', data.result, "warning");
                    }
                });

                $("#addVariousForm").formReset();
                SetRequestVariousFieldEmpty();
                SetVariousAccountingCompany();
                $("#TitleAgentNumber").hide();
                $("#TitleAgentName").hide();
                $("#TitleEmployeeNumber").hide();
                $("#TitleEmployeeName").hide();
                $("#TitleSupplierNumber").hide();
                $("#TitleSupplierName").hide();
            }

            setTimeout(function () {
                SetRequestVariousTotalMovement();
            }, 1500);
            // Ajuste Jira SMT-1559 Fin
        }
    });



    ///////////////////////////////////////////////////
    // Botón aceptar movimientos pagos varios modal ///
    ///////////////////////////////////////////////////
    $("#modalVarious").find('#VariousAcceptMovement').click(function () {
        $("#modalVarious").find("#alertForm").UifAlert('hide');
        var movements = $("#variousListView").UifListView("getData");

        if (movements != null) {
            for (var j = 0; j < movements.length; j++) {
                if (movements[j].DebitCreditName == "Crédito") {
                    creditAmount = creditAmount + parseFloat(ClearFormatCurrency(String(movements[j].Amount).replace("", ",")));
                }
                else {
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
        
        $('#modalVarious').UifModal('hide');
        $("#modalVarious").find("#requestVariousListView").UifListView();

        SetRequestVariousTotalMovement();        
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
        $("#requestVariousListView").UifListView("clear");
        $('#TotalRequestVariousMovement').text("$0.00");

    });
    $('#modalVarious').on('closed.modal', function () {
        $("#requestVariousListView").UifListView("clear");
        $('#TotalRequestVariousMovement').text("$0.00");
    });
});



/*---------------------------------------------------------------------------------------------------------------------------------*/
/*                                                     DEFINICION DE FUNCIONES                                                     */
/*---------------------------------------------------------------------------------------------------------------------------------*/

if ($("#ViewBagBranchDisable").val() == "1") {
    setTimeout(function () {
        $("#selectVariousBranch").attr("disabled", "disabled");
    }, 300);
}
else {
    $("#selectVariousBranch").removeAttr("disabled");
}

function LoadPersonTypes() {
    PaymentRequestVariousMovementsRequest.GetPersonTypes().done(function (data) {
        $("#selectVariousSearchBy").UifSelect({ sourceData: data.data });
    });
}

function LoadPaymentVariousBranchs(branchUserDefault) {
    PaymentRequestVariousMovementsRequest.GetBranchs().done(function (data) {
        if (branchUserDefault == null || branchUserDefault == 0) {
            $("#selectVariousBranch").UifSelect({ sourceData: data.data });
        }
        else {
            $("#selectVariousBranch").UifSelect({ sourceData: data.data, selectedId: branchUserDefault });
        }
    });
}
function LoadPaymentVariousCompanies(idCompany) {
    PaymentRequestVariousMovementsRequest.GetAccountingCompanies().done(function (data) {
        if (isMulticompany == 0) {
            $("#selectVariousCompany").UifSelect({ sourceData: data.data, selectedId: idCompany, enable: false });
        }
        else {
            $("#selectVariousCompany").UifSelect({ sourceData: data.data});
        }
    });
}
///////////////////////////////////////////////////////////////
// Valida el ingreso de campos obligatorios para la búsqueda //
///////////////////////////////////////////////////////////////
function ValidateVariousSearchForm() {
    if ($('#selectVariousSearchBy').val() == "") {
        $("#modalVarious").find("#alertForm").UifAlert('show', Resources.SelectSearchBy, "warning");
        return false;
    }
    return true;
}

////////////////////////////////////////////////////////////////////////
/// Validación de duplicados movimientos solicitudes de pagos varios ///
////////////////////////////////////////////////////////////////////////
function ValidateDuplicateVarious(number) {
    var duplicate = false;

    // Items a aplicar
    var requestsToApply = $("#variousListView").UifListView("getData");

    if (requestsToApply != null) {
        for (var k = 0; k < requestsToApply.length; k++) {
            var keyValid = requestsToApply[k].ClaimCode + "*" + requestsToApply[k].BranchId + "*" + requestsToApply[k].CurrencyId +
                           "*" + requestsToApply[k].PayerBeneficiaryId + "*" + requestsToApply[k].PaymentRequestCode + "*" +
                           requestsToApply[k].RegistrationDate;
            if (keyValid == number) {
                duplicate = true;
                break;
            } else {
                duplicate = false;
            }
        }
    }

    return duplicate;
}

////////////////////////////////////////////////////////////////////////////////////////////////
/// Realiza la consulta de solicitudes de pagos varios asociados a un temporal de imputación ///
////////////////////////////////////////////////////////////////////////////////////////////////
function RefreshRequestVariousMovements() {
    PaymentRequestVariousMovementsRequest.RequestVariousMovements(tempImputationId).done(function (data) {
        if (data) {
            $("#variousListView").UifListView(
                {
                    autoHeight: true,
                    sourceData: data,
                    customDelete: false,
                    customAdd: false,
                    customEdit: false,
                    add: false,
                    edit: false,
                    delete: true,
                    displayTemplate: "#various-display-template",
                    deleteCallback: deleteVariousCallback
                });
        }
    });
        
    setTimeout(function () {
        SetVariousTotalMovement();
    }, 2000);
}

//////////////////////////////////////////////////////////////
// Setear el total de la listview solicitud de pagos varios //
//////////////////////////////////////////////////////////////
function SetRequestVariousTotalMovement() {
    var totalMovement = 0;
    
    var requestVarious = $("#requestVariousListView").UifListView("getData");

    if (requestVarious != null) {

        for (var j = 0; j < requestVarious.length; j++) {
            var requestAmount = String(requestVarious[j].TotalAmount).replace("$", "").replace(/,/g, "").replace(" ", "");
            totalMovement += parseFloat(requestAmount);
        }
    }
    else {
        $("#TotalRequestVariousMovement").text("");
    }
    $("#TotalRequestVariousMovement").text("$ " + NumberFormatSearch(totalMovement, "2", ".", ","));
}

//////////////////////////////////////////////////////////////
// Setear el total de la listview solicitud de pagos varios //
//////////////////////////////////////////////////////////////
function SetVariousTotalMovement() {
    var totalMovement = 0;

    var requestVarious = $("#variousListView").UifListView("getData");

    if (requestVarious != null) {

        for (var j = 0; j < requestVarious.length; j++) {
            var requestAmount = String(requestVarious[j].Amount).replace("$", "").replace(/,/g, "").replace(" ", "");
            totalMovement += parseFloat(requestAmount);
        }
    }
    else {
        $("#TotalVariousMovement").text("");
    }
    $("#TotalVariousMovement").text("$ " + NumberFormatSearch(totalMovement, "2", ".", ","));
}

/////////////////////////////////////////////////////////////
/// Setea los campos a valores iniciales - pagos varios   ///
/////////////////////////////////////////////////////////////
function SetRequestVariousFieldEmpty() {
    $("#selectVariousSearchBy").val("");
    $("#VariousAgentDocumentNumber").val("");
    $("#VariousEmployeeDocumentNumber").val("");
    $("#VariousSupplierDocumentNumber").val("");
    $("#VariousAgentName").val("");
    $("#VariousEmployeeName").val("");
    $("#VariousSupplierName").val("");
    $("#selectVariousSalePoint").val("");
    $("#selectVariousCompany").val("");
    $("#VariousRequestNumber").val("");
    $("#VariousVoucherNumber").val("");
    $("#VariousDateIncomeFrom").val("");
    $("#VariousDateIncomeTo").val("");
}

/////////////////////////////////////////////////////////////////
/// Setea la compañía y sucursal por default - pagos varios   ///
/////////////////////////////////////////////////////////////////
function SetVariousAccountingCompany() {
    if (isMulticompany == 0) {
        $("#selectVariousCompany").val(accountingCompanyDefault);
        $("#selectVariousCompany").attr("disabled", "disabled");
    }
    else {
        $("#selectVariousCompany").removeAttr("disabled");
    }
}


var saveVariousCallback = function (deferred, data) {
    $("#variousListView").UifListView("addItem", data);
    deferred.resolve();
};

var editVariousCallback = function (deferred, data) {
    data.NombreEmpresa = "Sistran";
    deferred.resolve(data);
};

var deleteVariousCallback = function (deferred, data) {
    deferred.resolve();

    lockScreen();
    setTimeout(function () {

        $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "PaymentVarious/DeletePaymentRequestItem",
            data: { "paymentRequestId": data.TempClaimPaymentCode, "imputationId": tempImputationId },
            success: function () {
                unlockScreen();
                RefreshRequestVariousMovements();
                SetVariousTotalMovement();
            }
        });

        SetVariousTotalMovement();
    }, 500);
};

var deleteVariousRequestCallback = function (deferred, data) {
    deferred.resolve();
};

/////////////////////////////////////////////////////////////////////////////////////
/// Setear el modelo de solicitudes de pagos varios con los valores seleccionados ///
/////////////////////////////////////////////////////////////////////////////////////
function SetDataVariousPaymentRequestItem(rowRequestApply) {

    oClaimsPaymentRequestItem.TempClaimPaymentCode = 0,
    oClaimsPaymentRequestItem.TempImputationCode = tempImputationId,
    oClaimsPaymentRequestItem.PaymentRequestCode = rowRequestApply.PaymentRequestCode,
    oClaimsPaymentRequestItem.BeneficiaryId = rowRequestApply.PayerBeneficiaryId, //rowRequestApply.IdPayBeneficiary,
    oClaimsPaymentRequestItem.CurrencyCode = rowRequestApply.CurrencyId, //rowRequestApply.CurrencyCode,
    oClaimsPaymentRequestItem.IncomeAmount = parseFloat(ClearFormatCurrency(String(rowRequestApply.TotalAmount).replace("", ",")));
    oClaimsPaymentRequestItem.RegistrationDate = rowRequestApply.RegistrationDate,
    oClaimsPaymentRequestItem.EstimationDate = rowRequestApply.EstimatedDate,
    oClaimsPaymentRequestItem.RequestType = rowRequestApply.PaymentSourceCode,
    oClaimsPaymentRequestItem.PaymentRequestNumber = rowRequestApply.PaymentRequestNumber;

    return oClaimsPaymentRequestItem;
}

function SetDataPaymentRequestItem(rowRequestApply) {
    oPaymentRequestItem.TempPaymentCode = 0;
    oPaymentRequestItem.TempImputationCode = tempImputationId;
    oPaymentRequestItem.PaymentRequestCode = rowRequestApply.PaymentRequestCode;
    oPaymentRequestItem.BeneficiaryId = rowRequestApply.PayerBeneficiaryId;
    oPaymentRequestItem.CurrencyCode = rowRequestApply.CurrencyId;
    oPaymentRequestItem.IncomeAmount = ReplaceDecimalPoint(ClearFormatCurrency(String(rowRequestApply.TotalAmount).replace("", ",")));
    oPaymentRequestItem.ExchangeRate = 0;
    oPaymentRequestItem.Amount = ReplaceDecimalPoint(ClearFormatCurrency(String(rowRequestApply.TotalAmount).replace("", ",")));
    oPaymentRequestItem.RegistrationDate = rowRequestApply.RegistrationDate;
    oPaymentRequestItem.EstimationDate = rowRequestApply.EstimatedDate;
    oPaymentRequestItem.BussinessType = 0;
    oPaymentRequestItem.PaymentNumber = 0;
    oPaymentRequestItem.PaymentExpirationDate = null;
    oPaymentRequestItem.PaymentRequestNumber = rowRequestApply.PaymentRequestNumber;

    return oPaymentRequestItem;
}



///////////////////////////////////////////////////////////////////////////
/// Setea el modelo de solictudes de pagos varios con valores iniciales ///
///////////////////////////////////////////////////////////////////////////
function SetDataVariousPaymentRequestItemEmpty() {

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


function GetVariousRequestSalePoints() {

    if ($("#modalVarious").find("#selectVariousBranch").val() > 0) {

        var controller = ACC_ROOT + "Common/GetSalesPointByBranchId?branchId=" + $("#modalVarious").find("#selectVariousBranch").val();
        $("#modalVarious").find("#selectVariousSalePoint").UifSelect({ source: controller });

        //Setea el punto de venta de default
        setTimeout(function () {
            $("#selectVariousSalePoint").val($("#ViewBagSalePointBranchUserDefault").val());
        }, 800);
    }
};

