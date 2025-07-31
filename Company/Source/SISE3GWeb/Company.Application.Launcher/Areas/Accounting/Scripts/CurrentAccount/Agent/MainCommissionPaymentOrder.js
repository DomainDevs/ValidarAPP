/*--------------------------------------------------------------------------------------------------------------------------------------*/
/*                                              DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                              */
/*--------------------------------------------------------------------------------------------------------------------------------------*/
var timeCommissionPayment = window.setInterval(validateCompanyCommissionPayment, 800);
var individualId = 0;
var agentDocumentNumberCommission = "";
var agentNameCommission = "";


$("#GenerateCommissionPaymentOrder").attr("disabled", true);

if ($("#ViewBag.BranchDisable").val() == "1") {
    setTimeout(function () {
        $("#selectBranch").attr("disabled", "disabled");
    }, 300);
}
else {
    $("#selectBranch").removeAttr("disabled");
}

setTimeout(function () {
    GetDate();
}, 4000);


//VALIDACION DE AGENTE POR NUMERO DE DOCUMENTO   
$("#AgentDocumentNumber").on('blur', function (event) {

    validAgentForProcess(agentDocumentNumberCommission, agentNameCommission);
});

$("#AgentDocumentNumber").on('mouseover',  function (event) {

    validAgentForProcess(agentDocumentNumberCommission, agentNameCommission);
});

$("#GenerateCommissionPaymentOrder").on('mouseover', function (event) {

    validAgentForProcess(agentDocumentNumberCommission, agentNameCommission);
});

//VALIDACION DE AGENTE POR NOMBRE 
$("#AgentName").on('blur', function (event) {

    validAgentForProcess(agentDocumentNumberCommission, agentNameCommission);
});

$("#AgentName").on('mouseover', function (event) {

    validAgentForProcess(agentDocumentNumberCommission, agentNameCommission);
});

$("#AgentName").on('mouseover', function (event) {

    validAgentForProcess(agentDocumentNumberCommission, agentNameCommission);
});



////////////////////////////////////////
/// Autocomplete documento agente    ///
////////////////////////////////////////
$("#AgentDocumentNumber").on('itemSelected', function (event, selectedItem) {
    $("#alertForm").UifAlert('hide');
    individualId = selectedItem.IndividualId;
    if (individualId > 0) {
        agentDocumentNumberCommission = selectedItem.DocumentNumber;
        agentNameCommission = selectedItem.Name;
        $("#AgentDocumentNumber").val(selectedItem.DocumentNumber);
        $('#AgentName').UifAutoComplete('setValue', selectedItem.Name);        
        $("#GenerateCommissionPaymentOrder").attr("disabled", false);
    }
    else {
        $("#AgentDocumentNumber").val("");        
        $('#AgentName').UifAutoComplete('setValue', '');
        $("#GenerateCommissionPaymentOrder").attr("disabled", true);
    }
});

////////////////////////////////////////
/// Autocomplete nombre agente       ///
////////////////////////////////////////
$("#AgentName").on('itemSelected', function (event, selectedItem) {
    $("#alertForm").UifAlert('hide');
    individualId = selectedItem.IndividualId;
    if (individualId > 0) {
        agentDocumentNumberCommission = selectedItem.DocumentNumber;
        agentNameCommission = selectedItem.Name;        
        $('#AgentDocumentNumber').UifAutoComplete('setValue', selectedItem.DocumentNumber);
        $("#AgentName").val(selectedItem.Name);
        $("#GenerateCommissionPaymentOrder").attr("disabled", false);
    }
    else {
        $('#AgentDocumentNumber').UifAutoComplete('setValue', '');    
        $("#AgentName").val("");
        $("#GenerateCommissionPaymentOrder").attr("disabled", true);
    }
});


/////////////////////////////////////////////////////////////////////
/// Valida que no se ingresen fechas inferiores a la fecha actual ///
/////////////////////////////////////////////////////////////////////
$('#PaymentEstimatedDate').on("datepicker.change", function (event, date) {
    $("#alertForm").UifAlert('hide');
    if (IsDate($('#PaymentEstimatedDate').val())) {
        var systemDate = GetCurrentDate();

        if (CompareDates(systemDate, $("#PaymentEstimatedDate").val()) == true) {
            $("#alertForm").UifAlert('show', Resources.MessagePaymentEstimatedDate, "warning");
            GetDate();
        }
    }
    else {
        $("#alertForm").UifAlert('show', Resources.EntryPaymentEstimatedDate, "warning");
    }

    setTimeout(function () {
        $("#alertForm").UifAlert('hide');
    }, 3000);
});


//////////////////////////////////////////////////
// Botón generar órdenes de pago comisiones    ///
//////////////////////////////////////////////////
$("#GenerateCommissionPaymentOrder").click(function () {
    $("#formCommissionPaymentOrder").validate();
    $("#alertForm").UifAlert('hide');

    if ($("#formCommissionPaymentOrder").valid()) {

        if ($("#GenerationYear").val() != "" && $("#ValidThruMonth").val() == "")
        {
            $("#alertForm").UifAlert('show', Resources.MonthRequired, "warning");
            return;
        }
        else if ($("#GenerationYear").val() == "" && $("#ValidThruMonth").val() != ""){
            $("#alertForm").UifAlert('show', Resources.YearRequired, "warning");
            return;
        }

        //var agentType = $('input:radio[name=agoptions]:checked').val();

        if ($("#optionAgent").is(':checked')) {  //para un Agente

            if ($("#AgentDocumentNumber").val() == "" || $("#AgenName").val() == "") {
                $("#alertForm").UifAlert('show', Resources.EntryAgent, "warning");
                return;
            }
            else {
                // Se verifica si el agente tiene convenio de descuento de comisiones

                lockScreen();
                setTimeout(function () {
                    $.ajax({
                    async: false,
                    type: "POST",
                    url: ACC_ROOT + "Agent/ValidateCommissionDiscountAgreement",
                    data: { "agentId": individualId },
                    success: function (data) {

                        unlockScreen();
                        if (data > 0) {
                            var message = Resources.Agent+ " " + $("#AgentName").val() + ", " + Resources.CommissionPaymentOrderAgentMessage;
                            $("#alertForm").UifAlert('show', message, "warning");
                        }
                        else {
                            showConfirmCommission(individualId);
                        }
                    }
                    });
                }, 500);
            }
        }
        else if ($("#optionAllAgent").is(':checked')) {

            showConfirmCommission(0);
        }
    }
});

////////////////////////////////////////////
/// Selección un agente                  ///
////////////////////////////////////////////
$("#optionAgent").change(function () {
    $("#AgentDocumentNumber").val('');
    $("#AgentName").val('');
    $("#AgentDocumentNumber").removeAttr("disabled");
    $("#AgentName").removeAttr("disabled");
    $("#GenerateCommissionPaymentOrder").attr("disabled", true);
    $("#alertForm").UifAlert('hide');
});

////////////////////////////////////////////
/// Selección todos los agentes          ///
////////////////////////////////////////////
$("#optionAllAgent").change(function () {
    $("#AgentDocumentNumber").val('');
    $("#AgentName").val('');
    $("#AgentDocumentNumber").attr("disabled", "disabled");
    $("#AgentName").attr("disabled", "disabled");
    $("#GenerateCommissionPaymentOrder").attr("disabled", false);
    agentDocumentNumberCommission = "";
    agentNameCommission = "";
    $("#alertForm").UifAlert('hide');

});


///////////////////////////////////////////////////////
// Año de búsqueda de información                    //
///////////////////////////////////////////////////////
$("#GenerationYear").blur(function () {
    $("#alertForm").UifAlert('hide');

    if ($("#GenerationYear").val() != "") {
        if (parseInt($("#GenerationYear").val()) < 1900) {
            $("#alertForm").UifAlert('show', Resources.MessageYear, "warning");
            $("#GenerationYear").val("");
        }
    }
});


////////////////////////////////////////////////////////////
// Botón cancelar generación orden de pago de comisiones ///
////////////////////////////////////////////////////////////
$("#CancelCommissionPaymentOrder").click(function (e) {
    SetDataFieldsEmptyPaymentOrder();
    $("#alertForm").UifAlert('hide');
});

function showConfirmCommission (agentId) {
    $.UifDialog('confirm', {
        'message': Resources.CommissionPaymentOrderConfirmationMessage,'title': Resources.CommissionsPaymentOrder
    }, function (result) {
        if (result) {
            var year = -1;
            var month = -1;

            if ($("#GenerationYear").val() != "")
            {
                year = $("#GenerationYear").val();
            }
            if ($("#ValidThruMonth").val() != "")
            {
                month = $("#ValidThruMonth").val();
            }
            lockScreen();
            setTimeout(function () {
   
            // Genera la orden de pago comisiones para uno o todos los agentes
            $.ajax({
                async: false,
                type: "POST",
                url: ACC_ROOT + "Agent/SaveCommissionPaymentOrder",
                data: {
                    "branchId": $("#selectBranch").val(), "companyId": $("#selectCompany").val(),
                    "estimatedPaymentDate": $("#PaymentEstimatedDate").val(), "agentId": agentId,
                    "agentName": $("#AgentName").val(), "year": year, "month": month
                },
                success: function (dataPaymentOrder)
                {
                    unlockScreen();

                    if (dataPaymentOrder.length > 0) {

                        // Genera el reporte de orden de pago comisiones
                        var reportBranchId = $("#selectBranch").val();
                        var reportBranchName = $("#selectBranch option:selected").text();
                        var reportCompanyId = $("#selectCompany").val();
                        var reportCompanyName = $("#selectCompany option:selected").text();
                        var estimatedPaymentDate = $("#PaymentEstimatedDate").val();
                        var paymentOrderModelList = { PaymentOrderItem: [] };
                        var paymentOrderItem = { PaymentOrderItemId: 0 };

                        for (var j = 0; j < dataPaymentOrder.length; j++) {
                            paymentOrderItem = { PaymentOrderItemId: 0 };
                            paymentOrderItem.PaymentOrderItemId = dataPaymentOrder[j].Id;
                            paymentOrderModelList.PaymentOrderItem.push(paymentOrderItem);
                        }

                        lockScreen();
                        setTimeout(function () {

                            $.ajax({
                                async: false,
                                type: "POST",
                                dataType: "json",
                                url: ACC_ROOT + "Report/LoadPaymentOrdersCommissionReport",
                                data: {
                                    "branchId": reportBranchId, "branchName": reportBranchName, "companyId": reportCompanyId,
                                    "companyName": reportCompanyName, "estimatedPaymentDate": estimatedPaymentDate,
                                    "agentId": agentId, "paymentOrderModelList": paymentOrderModelList
                                },
                                success: function () {
                                    unlockScreen();
                                    ShowPaymentOrdersCommissionReport();
                                    SetDataFieldsEmptyPaymentOrder();
                                }
                                });
                        }, 500);
                        
                    }
                    else {
                        $("#alertForm").UifAlert('show', Resources.CommissionPaymentOrderMessageAgentNotFound, "warning");
                        SetDataFieldsEmptyPaymentOrder();
                    }
                }
            });
                
            }, 500);

        }
    });
};



/*--------------------------------------------------------------------------------------------------------------------------------------*/
/*                                   D E F I N I C I Ó N     D E    F U N C I O N E S                                                   */
/*--------------------------------------------------------------------------------------------------------------------------------------*/

//Valida y setea combo de Compañía
function validateCompanyCommissionPayment() {

    if ($("#selectCompany").val() != "" && $("#selectCompany").val() != null) {

        if (parseInt($("#ViewBagParameterMulticompany").val()) == 0) {

            $("#selectCompany").attr("disabled", true);
        }
        else {
            $("#selectCompany").attr("disabled", false);
        }
        clearInterval(timeCommissionPayment);
    }
}

/////////////////////////////////////
/// Obtiene la fecha del servidor ///
/////////////////////////////////////
function GetDate() {

    if ($("#ViewBagImputationType").val() == undefined &&
        $("#ViewBagParameterMulticompanyPayment").val() == undefined &&
        $("#ViewBagBillControlId").val() == undefined) {
        $.ajax({
            type: "GET",
            url: ACC_ROOT + "Agent/GetSystemDate",
            success: function (data) {
                $("#PaymentEstimatedDate").val(data);
            }
        });
    }
}


///////////////////////////////////////////////////////
/// Genera el reporte de órdenes de pago comisiones ///
///////////////////////////////////////////////////////
function ShowPaymentOrdersCommissionReport() {
    window.open(ACC_ROOT + "Report/ShowPaymentOrdersCommissionReport", 'mywindow', 'fullscreen=yes, scrollbars=auto');
}

///////////////////////////////////////////////////////
/// Limpia los campos de órdenes de pago comisiones ///
///////////////////////////////////////////////////////
function SetDataFieldsEmptyPaymentOrder() {
    $("#selectBranch").val($("#ViewBagBranchUserDefault").val());
    $("#selectCompany").val($("#ViewBagAccountingCompanyDefault").val());
    $("#AgentDocumentNumber").val("");
    $("#AgentName").val("");
    $("#GenerationYear").val("");
    $("#ValidThruMonth").val("");
    $("#PaymentEstimatedDate").val("");
    agentDocumentNumberCommission = "";
    agentNameCommission = "";
    individualId = 0;

}

//////////////////////////////////////////////////////////
/// Visualiza el reporte de órdenes de pago comisiones ///
//////////////////////////////////////////////////////////
function ShowCommissionsPaymentOrderReport() {
    var controller = ACC_ROOT + "Report/ ?branchId=" + $("#selectBranch").val() +
                     "&branchName=" + $("#selectBranch option:selected").text() + "&companyId=" + $("#selectCompany").val() +
                     "&companyName=" + $("#selectCompany option:selected").text() + "&estimatedPaymentDate=" +
                     $("#PaymentEstimatedDate").val() + "&agentId=" + individualId + "&paymentOrderModelList=" +
    JSON.stringify(paymentOrderModelList);
    window.open(controller, 'mywindow', 'fullscreen=yes, scrollbars=auto');
}

function validAgentForProcess(number, name) {

    if (number != "" && name != "") {

        if (number != $("#AgentDocumentNumber").val() || name != $("#AgentName").val()) {
            $("#alertForm").UifAlert('show', Resources.NoRegisteredPerson, "warning");
            $("#GenerateCommissionPaymentOrder").attr("disabled", true);
        } else {
            $("#alertForm").UifAlert('hide');
            $("#GenerateCommissionPaymentOrder").attr("disabled", false);
        }
    }
}
