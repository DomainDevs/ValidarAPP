/*-----------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS
/*-----------------------------------------------------------------------------------------------------------------------------------------*/

var entryRecord = null;

//variable para la consulta del asiento en modal
/*-----------------------------------------------------------------------------------------------------------------------------------------*/
//FORMATOS BUTTONS /FECHAS/ #CARACTERES / NÚMEROS-DECIMALES
/*-----------------------------------------------------------------------------------------------------------------------------------------*/

$("#entryYear").on('keypress', function (event) {
    if (event.keyCode != 8 && event.keyCode != 46 && event.keyCode != 37 && event.keyCode != 39 && event.keyCode != 9) {
        if (event.which < 48 || event.which > 57) {
            event.preventDefault();
        } //Números
    }
});

$("#transactionNumber").on('keypress', function (event) {
    if (event.keyCode != 8 && event.keyCode != 46 && event.keyCode != 37 && event.keyCode != 39 && event.keyCode != 9) {
        if (event.which < 48 || event.which > 57) {
            event.preventDefault();
        } //Números
    }
});

/*--------------------------------------------------------------------------------------------------------------------------------------------------*/
//ACCIONES / EVENTOS
/*--------------------------------------------------------------------------------------------------------------------------------------------------*/

//carga las tablas para que no muestre las columnas ocultas
window.onload = function () {
    $('#tblEntries').UifDataTable();
    $("#tblCostCenter").UifDataTable();
    $("#tblAnalysis").UifDataTable();
    $("#tblPostdated").UifDataTable();
};

// Botones de breadcrumb
$("#accontTrack").on('click', function () {
    $('#accountingMenu').find('li').find('a').click();
    $('#accountingMenu').siblings('a').click();
});
$("#entryTrack").on('click', function () {
    $('#accountingMenu').siblings('a').click();
    $('#entryMenu').siblings('a').click();
});
window.onload = function () {
    $("#entryTrack").click();
};

//click en el botón de búsqueda
$("#searchEntryConsultation").on('click', function (event) {

    clearTablesEntryConsultation();
    $("#mainAlert").UifAlert('hide');
    $("#searchForm").validate();
    if ($("#searchForm").valid()) {
        var filtered;

        //si uno de los campos opcionales está como nulo, entonces abre el pop up para seleccionar la cuenta.
        if ($("#ddlBranches").val() == "" || $("#ddlDestinations").val() == "" || $("#ddlAccountingMovementTypes").val() == "") {
            filtered = true;
        } else {
            filtered = false;
        }

        if (filtered) {
            //dispara el modal para seleccionar el asiento.
            var filterController = GL_ROOT + "Entry/SearchEntryMovements?branchId=" + $("#ddlBranches").val() + "&year=" +
                $("#entryYear").val() + "&month=" + $("#entryMonth").val() + "&entryNumber=" +
                $("#transactionNumber").val() + "&destinationId=" + $("#ddlDestinations").val() + "&accountingMovementTypeId=" +
                $("#ddlAccountingMovementTypes").val() + "&isFiltered=" + filtered;

            $("#EntryConsultationFilter").UifDataTable({ source: filterController });
            $('#entryConsultationFilter').UifModal('showLocal', Resources.MovementsConsultationMayorSeats);
        } else {
            var controller = GL_ROOT + "Entry/SearchEntryMovements?branchId=" + $("#ddlBranches").val() + "&year=" +
                $("#entryYear").val() + "&month=" + $("#entryMonth").val() + "&entryNumber=" + $("#transactionNumber").val() +
                "&destinationId=" + $("#ddlDestinations").val() + "&accountingMovementTypeId=" +
                $("#ddlAccountingMovementTypes").val() + "&isFiltered=" + filtered;

            $("#tblEntries").UifDataTable({ source: controller });

            setTimeout(function () {
                if ($("#tblEntries").UifDataTable('getData').length == 0) {
                    $("#mainAlert").UifAlert('show', Resources.NoRecordsFound, "warning");
                }
            }, 5000);
        }
    }
});

//click en el botón de limpiar
$("#clearEntryConsultation").on('click', function (event) {
    clearFieldsEntryConsultation();
    $("#mainAlert").UifAlert('hide');
});

//click en un registro de la tabla de movimientos
$('#tblEntries').on('rowSelected', function (event, data) {
    //lleno la tabla de Centro de Costos
    var controller = GL_ROOT + "Base/GetCostCentersByEntryId?entryId=" + data.EntryId + "&isDailyEntry=" + false;
    $("#tblCostCenter").UifDataTable({ source: controller });

    var controllerAnalyses = GL_ROOT + "Base/GetEntryAnalysesByEntryId?entryId=" + data.EntryId + "&isDailyEntry=" + false;
    $("#tblAnalysis").UifDataTable({ source: controllerAnalyses });

    var controllerPostdated = GL_ROOT + "Base/GetPostdatedByEntryId?entryId=" + data.EntryId + "&isDailyEntry=" + false;
    $("#tblPostdated").UifDataTable({ source: controllerPostdated });

});

//botón de reversión
$("#btnRevertEntryConsultation").on('click', function (event) {
    if ($('#tblEntries').UifDataTable('getData').length != 0) {
        if ($('#tblEntries').UifDataTable('getData')[0].Status == $("#ViewBagReverted").val()) {
            $("#mainAlert").UifAlert('show', Resources.EntryAlreadyReversed, "warning");
        } else {
            $("#entryConsultationModalMessageTitle").text(Resources.Warning);
            $("#entryConsultationMessageModal").text(Resources.ToReverseTransaction);
            $('#entryConsultationModal').UifModal('showLocal', Resources.ConsultationReversion);
        }
    } else {
        $("#mainAlert").UifAlert('show', Resources.SelectedAccountBeforeContinuing, "warning");
    }
});

//En este caso el botón perteneciente al modal de eliminación va a ejecutar la reversión de los movimientos
$("#entryConsultationModalYes").click(function () {
    $('#entryConsultationModal').modal('hide');
    var destinationId = $("#ddlDestinations").val() === "" ? 0 : $("#ddlDestinations").val();
    var accountingMovementTypes = $("#ddlAccountingMovementTypes").val() === "" ? 0 : $("#ddlAccountingMovementTypes").val();
    $.ajax({
        type: "POST",
        url: GL_ROOT + "Entry/EntryRevertion",
        data: JSON.stringify({
            "branchId": $("#ddlBranches").val(), "year": $("#entryYear").val(), "month": $("#entryMonth").val(),
            "entryNumber": $("#transactionNumber").val(), "destinationId": destinationId,
            "accountingMovementTypeId": accountingMovementTypes
        }),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {

        if (data.success == false) {

            $("#mainAlert").UifAlert('show', data.result, "danger");
        } else {
            // if (data.success) {
            clearFieldsEntryConsultation();
            $("#mainAlert").UifAlert('show', Resources.EntrySucessfullyReversed + ": " + data.result, "success");
            // }
        }
    });
});

//botón de Impresion de reporte
$("#btnPrintEntryConsultation").on('click', function (event) {
    if ($('#tblEntries').UifDataTable('getData').length != 0) {
        var entryNumber = $('#transactionNumber').val();
        showReportEntryConsultation(entryNumber);
    } else {
        $("#mainAlert").UifAlert('show', Resources.SelectedAccountBeforeContinuing, "warning");
    }
});

//Tabla dentro de Modal para seleccionar el registro a consultar
$("#entryConsultationFilter").on('rowSelected', '#EntryConsultationFilter', function (event, selectedItem) {
    entryRecord = selectedItem;
});

//boton de detalles cuando se selecciona el registro en el pop up
$("#entryDetails").on('click', function () {
    if (entryRecord == null) {
        $("#filterAlert").UifAlert('show', Resources.SelectedRecordBeforeContinuing, "warning");
    } else {
        $("#filterAlert").UifAlert('hide');

        var accountDate = (new Date(entryRecord.Date)).get;

        var dateAccount = entryRecord.Date.split("/");
        var year = dateAccount[2].split(" ")[0];
        var month = dateAccount[1];


        var controller = GL_ROOT + "Entry/SearchEntryMovements?branchId=" + entryRecord.BranchId + "&year=" + year
            + "&month=" + month + "&entryNumber=" + entryRecord.EntryNumber + "&destinationId=" + entryRecord.EntryDestinationId + "&accountingMovementTypeId=" +
            entryRecord.AccountingMovementTypeId + "&isFiltered=" + false;

        $("#tblEntries").UifDataTable({ source: controller });
        $("#ddlBranches").val(entryRecord.BranchId);

        if (entryRecord.EntryDestinationId > 0) {
            $("#ddlDestinations").val(entryRecord.EntryDestinationId);
        }
        if (entryRecord.AccountingMovementTypeId > 0) {
            $("#ddlAccountingMovementTypes").val(entryRecord.AccountingMovementTypeId);
        }

        $('#entryConsultationFilter').UifModal('hide');

        setTimeout(function () {
            if ($("#tblEntries").UifDataTable('getData').length == 0) {
                $("#mainAlert").UifAlert('show', Resources.NoRecordsFound, "warning");
            }
        }, 5000);
    }
});

//boton de detalles cuando se cierra la modal para filtrar asientos.
$("#cancelEntryDetails").on('click', function () {
    $("#filterAlert").UifAlert('hide');
    $("#EntryConsultationFilter").dataTable().fnClearTable();
});

/*---------------------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICION DE FUNCIONES
/*---------------------------------------------------------------------------------------------------------------------------------------------------*/

function clearFieldsEntryConsultation() {
    //campos de búsqueda
    $("#ddlBranches").val("");
    $("#entryYear").val("");
    $("#entryMonth").val("");
    $("#transactionNumber").val("");

    //reinicia el formulario de búsqueda
    $("#searchForm").formReset();

    //limpia la tabla de movimientos
    $('#tblEntries').dataTable().fnClearTable();
    $('#tblCostCenter').dataTable().fnClearTable();
    $('#tblAnalysis').dataTable().fnClearTable();
    $('#tblPostdated').dataTable().fnClearTable();

    //borra el registro de asiento
    entryRecord = null;
}

function clearTablesEntryConsultation() {
    $('#tblEntries').dataTable().fnClearTable();
    $('#tblCostCenter').dataTable().fnClearTable();
    $('#tblAnalysis').dataTable().fnClearTable();
    $('#tblPostdated').dataTable().fnClearTable();
    $('#EntryConsultationFilter').dataTable().fnClearTable();
}

function GetDebitsAndCredits() {
    var result = false;
    var debits = 0;
    var credits = 0;

    var data = $("#tblEntries").UifDataTable('getData');

    if (data.length > 0) {
        for (var i = 0; i < data.length; i++) {
            if (data[i].AccountingNatureId == 2) //debito
                debits = debits + parseFloat(((parseFloat(data[i].ExchangeRate.replace(",", ".")) * parseFloat(data[i].Amount.replace(",", ".")))).toFixed(2));
            if (data[i].AccountingNatureId == 1) //credito
                credits = credits + parseFloat(((parseFloat(data[i].ExchangeRate.replace(",", ".")) * parseFloat(data[i].Amount.replace(",", ".")))).toFixed(2));
        }
    }
}

//funcion que muestra reporte en pantalla
function showReportEntryConsultation(entryNumber) {
    var destinationId = $("#ddlDestinations").val() === "" ? 0 : $("#ddlDestinations").val()
    var accountingMovementTypes = $("#ddlAccountingMovementTypes").val() === "" ? 0 : $("#ddlAccountingMovementTypes").val()

    window.open(GL_ROOT + "Reports/ShowEntryConsultationReport?branchId="
        + $("#ddlBranches").val() + '&year=' + $("#entryYear").val() + '&month='
        + $("#entryMonth").val() + '&entryNumber=' + entryNumber + '&destinationId=' + destinationId
        + '&accountingMovementTypeId=' + accountingMovementTypes, 'mywindow', 'fullscreen=yes, scrollbars=auto');
}



