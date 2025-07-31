var time;
var tempReinsuranceProcessId = 0;
var totalRecords = 0;
var recordsProcessed = 0;
var processTable;
var time;

//limpia tipo de procesos
$("#btnRefresh").hide();

//Botón Reasegurar pólizas
$("#btnReinsuranceMassive").click(function () {
    $("#alertMassive").UifAlert('hide');
    $("#alertMassivePrefix").UifAlert('hide');
    $('#formReinsuranceMassive').validate();
    if ($('#formReinsuranceMassive').valid()) {
        if ($("#TablePrefixMassive").UifDataTable('getSelected') == null) {
            $.UifNotify('show', { 'type': 'warning', 'message': Resources.SelecctionPrefix, 'autoclose': true });
        }
        else {
            $('#modalReinsuranceMassive').appendTo("body").UifModal('showLocal');
        }
    }
});

$("#btnModalReinsuranceMassive").click(function () {
    $('#modalReinsuranceMassive').modal('hide');
    $("#alertMassive").UifAlert('hide');
    $("#alertMassivePrefix").UifAlert('hide');
    if ($("#selectTypeProcess").val() != "") {
        if ($("#dateFrom").val() != "" && $("#dateTo").val() != "") {
            if (IsDate($("#dateFrom").val()) == false || IsDate($("#dateTo").val()) == false) {
                $.UifNotify('show', { 'type': 'warning', 'message': Resources.InvalidDates, 'autoclose': true });
            }
            else {
                lockScreen();
                if (!compare_dates($("#dateTo").val(), $("#dateFrom").val())) {

                    if ($("#dateFrom").val() == $("#dateTo").val()) {
                        saveReinsuranceMasiveHeader(parseInt($("#selectTypeProcess").val()));
                    }
                    else {
                        $.UifNotify('show', { 'type': 'warning', 'message': Resources.ValidateDateFrom, 'autoclose': true });
                    }
                }
                else {
                    saveReinsuranceMasiveHeader(parseInt($("#selectTypeProcess").val()));
                }
            }
        }
        else {
            if ($("#dateFrom").val() == "") {
                $("#dateFrom").focus();
            }
            else {
                $("#dateTo").focus();
            }
            $.UifNotify('show', { 'type': 'warning', 'message': Resources.ConfirmaData, 'autoclose': true });
        }
    }
    else {
        $.UifNotify('show', { 'type': 'warning', 'message': Resources.MessageSelectProcessType, 'autoclose': true });
    }
});

$("#btnModalMassive").click(function () {
    $("#alertMassive").UifAlert('hide');
    $("#alertMassivePrefix").UifAlert('hide');
    $('#modalMassiveTotRecords').modal('hide');
    $.ajax({
        url: REINS_ROOT + "Process/ReinsuranceMassiveExecute",
        data: { "processId": tempReinsuranceProcessId, "moduleId": parseInt($("#selectTypeProcess").val()) },
        success: function (data) {
            if (data.success == false) {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
            else if (data.success == true) {
                refreshGrid();
            }
        }
    });
});

$("#btnModalMassiveNo").click(function () {
    $("#alertMassive").UifAlert('hide');
    $("#alertMassivePrefix").UifAlert('hide');
    $('#modalMassiveTotRecords').modal('hide');
    $("#panelPrefix").collapse("show");
    var msj;
    $.ajax({
        url: REINS_ROOT + "Process/UpdateTempReinsuranceProcess",
        data: { "processId": tempReinsuranceProcessId },
        success: function (data) {
            if (data.success == false && data.result == 0) {
                msj = Resources.ErrorStoreProcedure;
                $.UifNotify('show', { 'type': 'danger', 'message': msj, 'autoclose': true });
            }
            else if (data.success == true) {
                msj = Resources.MsgCancelProcess;
                $.UifNotify('show', { 'type': 'warning', 'message': msj, 'autoclose': true });
            }
        }
    });
});

$("#btnCancel").click(function () {
    $("#alertMassive").UifAlert('hide');
    $("#alertMassivePrefix").UifAlert('hide');
    $("#dateTo").val("");
    $("#dateFrom").val("");
    $("#alertMassive").UifAlert('hide');
    $('#tableReinsuranceProcess').UifDataTable('clear');
    $("#selectTypeProcess").val("");
    $("#panelPrefix").collapse("show");
});

$("#btnRefresh").click(function () {
    refreshGrid();
});

$('#TablePrefixMassive').on('rowSelected', function (event, data, position) {
    $("#alertMassivePrefix").UifAlert('hide');
});

$('#TablePrefixMassive').on('selectAll', function (event, data, position) {
    $("#alertMassivePrefix").UifAlert('hide');
});

// Controla que la fecha final sea mayor a la inicial
$('#dateFrom').on('datepicker.change', function (event, date) {
    $("#alertMassive").UifAlert('hide');
    $("#alertMassivePrefix").UifAlert('hide');
    if ($("#dateTo").val() != "") {
        if (compare_dates($('#dateFrom').val(), $("#dateTo").val())) {
            $.UifNotify('show', { 'type': 'warning', 'message': Resources.ValidateDateTo, 'autoclose': true });
            $("#dateFrom").val('');
        }
        else {
            $("#dateFrom").val($('#dateFrom').val());
        }
    }
});

// Controla que la fecha final sea mayor a la inicial
$('#dateTo').on('datepicker.change', function (event, date) {
    $("#alertMassive").UifAlert('hide');
    $("#alertMassivePrefix").UifAlert('hide');

    if ($("#dateFrom").val() != "") {
        if (compare_dates($("#dateFrom").val(), $('#dateTo').val())) {
            $.UifNotify('show', { 'type': 'warning', 'message': Resources.ValidateDateFrom, 'autoclose': true });
            $("#dateTo").val('');
        }
        else {
            $("#dateTo").val($('#dateTo').val());
        }
    }
});

// FUNCIONES
//Graba la cabecera del proceso masivo y devuelve el processId correspondiente
function saveReinsuranceMasiveHeader(typeProcess) {
    totalRecords = 0;
    recordsProcessed = 0;
    var prefixes = PrefixSelected();
    $("#panelPrefix").collapse("hide");
    $.ajax({
        type: "POST",
        url: REINS_ROOT + "Process/SaveReinsuranceMassiveHeader",
        data: JSON.stringify({ "dateFrom": $("#dateFrom").val(), "dateTo": $("#dateTo").val(), "typeProcess": parseInt(typeProcess), "prefixes": prefixes }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var msj;
            if (data.success == false) {
                if (data.result.Option == "1") {
                    $.UifNotify('show', { 'type': 'warning', 'message': Resources.ValidModuleDateReinsMassive + ' ' + data.result.DateClose, 'autoclose': true });
                }
                else if (data.result.Option == "2") { //error de PARAMETRIZACION
                    $.UifNotify('show', { 'type': 'warning', 'message': Resources.ValidModuleDateParam, 'autoclose': true });
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                }
            }
            else {
                tempReinsuranceProcessId = data.result.ProcessId;
                $("#TempReinsuranceProcessId").val(tempReinsuranceProcessId);
                totalRecords = data.result.Records;
                msj = Resources.MsgMassiveTotRecords + ': ' + totalRecords + ' ' + Resources.MsgWantContinue;
                $('#pnlModalTotRecords').text(msj);
                if (totalRecords > 0) {
                    $('#modalMassiveTotRecords').appendTo("body").UifModal('showLocal');
                }
                else {
                    msj = Resources.MsgTotRecordsToProcess;
                    $.UifNotify('show', { 'type': 'warning', 'message': msj, 'autoclose': true });
                }
            }
            unlockScreen();
        },
        error: function (xhr, status, errorThrown) {
            unlockScreen();
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.GeneratesError + ' ' + errorThrown, 'autoclose': true });
        }
    });
}

//Refresca la grilla de proceso
function refreshGrid() {
    if (recordsProcessed < totalRecords) {
        $.ajax({
            type: "POST",
            url: REINS_ROOT + "Process/GetTempReinsuranceProcess",
            data: JSON.stringify({
                tempReinsuranceProcessId: parseInt($("#TempReinsuranceProcessId").val())
            }),
            datatype: 'json',
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data.success) {
                if (data.result.length > 0) {
                    $("#tableReinsuranceProcess").UifDataTable({ sourceData: data.result });
                    processTable = $('#tableReinsuranceProcess').UifDataTable('getData');
                    if (processTable.length > 0) { //devuelve registros porque hay un proceso corriendo
                        recordsProcessed = processTable[0].RecordsProcessed + processTable[0].RecordsFailed;
                        totalRecords = processTable[0].RecordsNumber;
                        if (processTable[0].Status == 2) {
                           time = setTimeout(() => refreshGrid(), 10000);
                        }
                        else if (processTable[0].Status != 2) {
                            clearTimeout(time);
                            $("#dateFrom").removeAttr("disabled");
                            $("#dateTo").removeAttr("disabled");
                            $("#btnReinsuranceMassive").removeAttr("disabled");
                            $("#btnCancel").removeAttr("disabled");
                            $("#selectTypeProcess").removeAttr("disabled");

                            if (recordsProcessed == totalRecords) {
                                $.UifNotify('show', { 'type': 'success', 'message': Resources.MsgSuccessProcess, 'autoclose': true });
                            }
                            return;
                        }

                        $("#TempReinsuranceProcessId").val(processTable[0].TempReinsuranceProcessId);
                        $("#dateFrom").attr("disabled", true);
                        $("#dateTo").attr("disabled", true);
                        $("#btnReinsuranceMassive").attr("disabled", true);
                        $("#btnCancel").attr("disabled", true);
                        $("#selectTypeProcess").attr("disabled", true);

                    }
                }
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
        });
    }
    else {
        $("#dateFrom").removeAttr("disabled");
        $("#dateTo").removeAttr("disabled");
        $("#btnReinsuranceMassive").removeAttr("disabled");
        $("#btnCancel").removeAttr("disabled");
        $("#selectTypeProcess").removeAttr("disabled");
        $.UifNotify('show', { 'type': 'success', 'message': Resources.MsgSuccessProcess, 'autoclose': true });
    }
}

function PrefixSelected() {
    var Prefix = new Array();
    var prefixType = {
        Description: "",
        Id: 0
    };
    var modelPrefix = {
        AbbreviatedDescription: "",
        BeforeId: 0,
        Covered: false,
        Description: "",
        FromDate: "",
        Id: 0,
        IsFromBD: 0,
        IsTraditionalPrefix: false,
        IsUniversalPrefix: false,
        PrefixType: prefixType,
        SubPrefix: [],
        ToDate: "",
        Validity: 0
    }

    var tempPrefix = $("#TablePrefixMassive").UifDataTable('getSelected');

    if (tempPrefix != null) {
        for (var i = 0; i < tempPrefix.length; i++) {
            modelPrefix = {
                Description: tempPrefix[i].Description,
                Id: tempPrefix[i].Id,
            }
            Prefix.push(modelPrefix);
        }
    }
    return Prefix
}
