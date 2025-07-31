/*--------------------------------------------------------------------------------------------------------------------------------------*/
/*                                              DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                              */
/*--------------------------------------------------------------------------------------------------------------------------------------*/
var timeCommissionBalance = window.setInterval(validateCompanyCommissionBalance, 800);
var individualId = 0;

var AgentCommissionBalanceModel = {
    AgentCommissionBalanceCode: 0,
    AgentId: 0,
    StartDate: null,
    EndDate: null,
    RegisterDate: null,
    Status: 0,
    BranchCode: 0,
    CompanyCode: 0
};
      
if ($("#ViewBagBranchDisable").val() == "1") {
    setTimeout(function () {
        $("#Branch").attr("disabled", "disabled");
    }, 300);
}
else {
    $("#Branch").removeAttr("disabled");
}

////////////////////////////////////////
/// Autocomplete documento agente    ///
////////////////////////////////////////
$("#AgentDocumentNumber").on('itemSelected', function (event, selectedItem) {
    $("#alertForm").UifAlert('hide');
    individualId = selectedItem.IndividualId;
    if (individualId > 0) {
        $("#AgentDocumentNumber").val(selectedItem.DocumentNumber);
        $('#AgentName').UifAutoComplete('setValue', selectedItem.Name);
    }
    else {
        $("#AgentDocumentNumber").val("");
        $('#AgentName').UifAutoComplete('setValue', '');
    }
});

////////////////////////////////////////
/// Autocomplete nombre agente       ///
////////////////////////////////////////
$("#AgentName").on('itemSelected', function (event, selectedItem) {
    $("#alertForm").UifAlert('hide');
    individualId = selectedItem.IndividualId;
    if (individualId > 0) {        
        $('#AgentDocumentNumber').UifAutoComplete('setValue', selectedItem.DocumentNumber);
        $("#AgentName").val(selectedItem.Name);
    }
    else {
        $('#AgentDocumentNumber').UifAutoComplete('setValue', '');        
        $("#AgentName").val("");
    }
});

//////////////////////////////////////
/// Fecha generación desde         ///
//////////////////////////////////////
$('#DateFrom').on("datepicker.change", function (event, date) {
    $("#alertForm").UifAlert('hide');
    if (IsDate($('#DateFrom').val())) {
        if ($("#DateTo").val() != "") {
            if (CompareDates($('#DateFrom').val(), $("#DateTo").val()) == true) {
                $("#alertForm").UifAlert('show', Resources.MessageValidateDateFrom, "warning");
                $('#DateFrom').val('');
                return true;
            }
        }
    }
    else {
        $("#alertForm").UifAlert('show', Resources.EntryDateFrom, "warning");
    }

    setTimeout(function () {
        $("#alertForm").UifAlert('hide');
    }, 3000);
});

$("#DateFrom").blur(function () {
    $("#alertForm").UifAlert('hide');

    if ($("#DateFrom").val() != '') {

        if (IsDate($("#DateFrom").val()) == true) {
            if ($("#DateTo").val() != '') {
                if (CompareDates($('#DateFrom').val(), $("#DateTo").val())) {
                    $("#alertForm").UifAlert('show', Resources.MessageValidateDateFrom, "warning");
                    $('#DateFrom').val('');
                    return true;
                }
            }
        } else {
            $("#alertForm").UifAlert('show', Resources.EntryDateFrom, "danger");
            $("#DateFrom").val("");
        }
    }
});

//////////////////////////////////////
/// Fecha generación hasta         ///
//////////////////////////////////////
$('#DateTo').on("datepicker.change", function (event, date) {
    $("#alertForm").UifAlert('hide');
    if (IsDate($('#DateTo').val())) {
        if ($('#DateFrom').val() != "") {
            if (CompareDates($('#DateFrom').val(), $("#DateTo").val()) == true) {
                $("#alertForm").UifAlert('show', Resources.MessageValidateDateTo, "warning");
                $("#DateTo").val('');
                return true;
            }
        }
    }
    else {
        $("#alertForm").UifAlert('show', Resources.EntryDateTo, "warning");
    }

    setTimeout(function () {
        $("#alertForm").UifAlert('hide');
    }, 3000);
});

$("#DateTo").blur(function () {
    $("#alertForm").UifAlert('hide');

    if ($("#DateTo").val() != '') {

        if (IsDate($("#DateTo").val()) == true) {
            if ($("#DateFrom").val() != '') {
                if (CompareDates($('#DateFrom').val(), $("#DateTo").val())) {
                    $("#alertForm").UifAlert('show', Resources.MessageValidateDateTo, "warning");
                    $("#DateTo").val('');
                    return true;
                }
            }
        } else {
            $("#alertForm").UifAlert('show', Resources.EntryDateTo, "danger");
            $("#DateTo").val("");
        }
    }
});

////////////////////////////////////////////
/// Selección un agente                  ///
////////////////////////////////////////////
$('#optionAgent').click(function () {
    if ($(this).is(':checked')) {
        $('#AgentDocumentNumber').attr("disabled", false);
        $('#AgentName').attr("disabled", false);
    }
});

///////////////////////////////////////////////
/// Selección todas los agentes             ///
///////////////////////////////////////////////
$('#optionAllAgent').click(function () {
    if ($(this).is(':checked')) {
        $('#AgentDocumentNumber').attr("disabled", "disabled");
        $('#AgentName').attr("disabled", "disabled");
    }
    $('#AgentDocumentNumber').val("");
    $('#AgentName').val("");
    individualId = 0;
});

//////////////////////////////////////////////////
// Botón generar reporte balance de comisiones ///
//////////////////////////////////////////////////
$("#GenerateCommissionBalance").click(function () {
    $("#alertForm").UifAlert('hide');
    $("#formCommissionBalance").validate();
    $("#alertForm").UifAlert('hide');

    if ($("#formCommissionBalance").valid()) {
        var reportType = $('input:radio[name=options]:checked').val();

        if ($("#optionAgent").is(':checked')) {  //para un Agente

            if ($("#AgentDocumentNumber").val() == "" || $("#AgenName").val() == "") {
                $("#alertForm").UifAlert('show', Resources.EntryAgent, "warning");
                return;
            }
        }

        $.msg({ content: Resources.MessagePleaseWait, klass: 'white-on-black' });

        $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "Report/LoadCommissionBalanceReport",
            data: { "agentCommissionBalance": SetDataCommissionBalance(), "type": reportType },
            success: function (data) {
                if (data > 0) {
                    setTimeout(function () {
                        ShowReport();
                        CleanFieldsCommissionBalance();
                        // TODO ACE-1486 SE COMENTA PORQUE NO EXISTE CODIGO DE METODO
                       // SetAccountingCompanyCommission();
                    }, 5000);

                }
                else {
                        
                    // No hay coincidencias con los filtros ingresados
                    $("#alertForm").UifAlert('show', Resources.MovementsTypeValidation, "warning");
                }
            }
        });

        setTimeout(function () {
            $("#alertForm").UifAlert('hide');
        }, 3500);
    }
});


//////////////////////////////////////////////////////
// Botón cancelar generación balance de comisiones ///
//////////////////////////////////////////////////////
$("#CancelCommissionBalance").click(function (e) {
    showConfirmCommissionBalance();
});

function showConfirmCommissionBalance () {
    $.UifDialog('confirm', { 'message': Resources.CancelGenerationQuestion + "?", 'title': '@Global.Cancel' }, function (result) {
        if (result) {
            CleanFieldsCommissionBalance();
            setTimeout(function () {
                // TODO ACE-1486 SE COMENTA PORQUE NO EXISTE CODIGO DE METODO
                //SetAccountingCompanyCommission();
            }, 4500);

        }
    });
};

/*--------------------------------------------------------------------------------------------------------------------------------------*/
/*                                                   DEFINICIÓN DE FUNCIONES                                                 */
/*--------------------------------------------------------------------------------------------------------------------------------------*/

/////////////////////////////////////////////////////
/// Visualiza el reporte de balance de comisiones ///
/////////////////////////////////////////////////////
function ShowCommissionBalanceReport(reportType) {
    var controller = ACC_ROOT + "Report/ShowCommissionBalanceReport?branchId=" + $("#Branch").val() +
    "&companyId=" + $("#Company").val() + "&agentId=" + individualId + "&startDate=" +
    $("#DateFrom").val() + "&endDate=" + $("#DateTo").val() + "&type=" + reportType;
    window.open(controller, 'mywindow', 'fullscreen=yes, scrollbars=auto');
}


//////////////////////////////////////////////////////////////////////
/// Setea los valores para la generación del balance de comisiones ///
//////////////////////////////////////////////////////////////////////
function SetDataCommissionBalance() {

    AgentCommissionBalanceModel = {
        AgentCommissionBalanceCode: 0,
        AgentId: individualId,
        StartDate: $("#DateFrom").val(),
        EndDate: $("#DateTo").val(),
        RegisterDate: null,
        Status: 0,
        BranchCode: $("#Branch").val(),
        CompanyCode: $("#Company").val()
    };

    return AgentCommissionBalanceModel;
}


//////////////////////////////////////////////////
/// Genera el reporte de balance de comisiones ///
//////////////////////////////////////////////////
function ShowReport() {
    window.open(ACC_ROOT + "Report/ShowReportCommissionBalance", 'mywindow', 'fullscreen=yes, scrollbars=auto');
}

//////////////////////////////////////////////////
/// Limpia los campos de balance de comisiones ///
//////////////////////////////////////////////////
function CleanFieldsCommissionBalance() {
    $("#Branch").val($("#ViewBagBranchDefault").val());
    $("#Company").val($("#ViewBagAccountingCompanyDefault").val());
    $("#DateFrom").val('');
    $("#DateTo").val('');
    $("#AgentDocumentNumber").val('');
    $("#AgentName").val('');
    $("#Branch").focus();
    individualId = 0;
}


//Valida y setea combo de Compañía
function validateCompanyCommissionBalance() {

    if ($("#Company").val() != "" && $("#Company").val() != null) {

        if (parseInt($("#ViewBagParameterMulticompany").val()) == 0) {

            $("#Company").attr("disabled", true);
        }
        else {
            $("#Company").attr("disabled", false);
        }
        clearInterval(timeCommissionBalance);
    }
}
