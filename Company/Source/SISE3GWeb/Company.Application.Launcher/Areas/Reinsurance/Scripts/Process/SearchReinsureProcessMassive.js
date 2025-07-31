
var tempReinsuranceProcessId = 0;
disableExport();

$("#btnSearchProcessMassive").click(function () {

    $('#formReinsuranceMassiveSearch').validate();
    if ($('#formReinsuranceMassiveSearch').valid()) {

        if ($("#selectTypeProcess").val() != "") {

            $("#alertMassive").UifAlert('hide');
            $('#tableSearchReinsuranceProcessDetails').dataTable().fnClearTable();

            var controller = REINS_ROOT + "Process/GetTempReinsuranceProcesses?tempReinsuranceProcessId="
                + parseInt($("#ProcessId").val()) + '&moduleId=' + parseInt($("#selectTypeProcess").val());

            $("#tableSearchReinsuranceProcess").UifDataTable({ source: controller });
        } else {
            $.UifNotify('show', { 'type': 'warning', 'message': Resources.MessageSelectProcessType, 'autoclose': true });
        }
    }
});

$("#btnCleanProcessMassive").click(function () {
    $("#alertMassive").UifAlert('hide');
    $("#selectTypeProcess").val("");
    $("#ProcessId").val("");
    $("#btnExportExcel").attr("disabled", true);
    //$("#btnExportPdf").attr("disabled", true);
    $('#tableSearchReinsuranceProcess').UifDataTable('clear');
    $('#tableSearchReinsuranceProcessDetails').UifDataTable('clear');
    $("#selectTypeProcess").focus();
});


$('#tableSearchReinsuranceProcess').on('rowSelected', function (event, selectedRow) {
    $('#tableSearchReinsuranceProcessDetails').UifDataTable('clear');

    tempReinsuranceProcessId = selectedRow.TempReinsuranceProcessId;
    if (tempReinsuranceProcessId > 0) {
        $("#btnExportExcel").removeAttr("disabled");
        //$("#btnExportPdf").removeAttr("disabled");

        $.ajax({
            type: 'POST',
            url: REINS_ROOT + 'Process/GetTempReinsuranceProcessDetails',
            data: JSON.stringify({ tempReinsuranceProcessId: selectedRow.TempReinsuranceProcessId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        }).done(function (data) {
            if (data.success) {
                if (data.result) {
                    $("#tableSearchReinsuranceProcessDetails").UifDataTable({ sourceData: data.result });
                } else {
                    $.UifNotify('show', { 'type': 'warning', 'message': Resources.NoRecordsFound, 'autoclose': true });
                }
            } else {
                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
            }
            }).fail(function (response) {
                $.UifNotify('show', { 'type': 'danger', 'message': response.result, 'autoclose': true });
        });

    }
});


//exporta a excel
$("#btnExportExcel").click(function () {
    window.location = REINS_ROOT + 'Process/ExportReinsuranceProcessDetail?tempReinsuranceProcessId=' + tempReinsuranceProcessId;
});


//exporta a excel
/*$("#btnExportPdf").click(function () {
    loadReport(tempReinsuranceProcessId);
    window.location = REINS_ROOT + "Process/ShowProcessMassiveDetailsReport";
});*/


// ******************************************************// FUNCIONES //*******************************************************

function disableExport() {

    $("#btnExportExcel").attr("disabled", true);
    //$("#btnExportPdf").attr("disabled", true);
}


//Carga reporte
function loadReport(tempReinsuranceProcessId) {

    $.ajax({
        async: false,
        type: "POST",
        url: REINS_ROOT + "Process/LoadProcessMassiveDetailsReport",
        data: { "tempReinsuranceProcessId": tempReinsuranceProcessId },
        success: function (data) { }
    });
}

