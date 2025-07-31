$.ajaxSetup({ async: false });
$(document).ready(function () {
    $("#inputLoadSearch").ValidatorKey(ValidatorType.Number, ValidatorType.Number, 1);
});
$('#inputLoadSearch').on('buttonClick', function () {
    GetMassiveLoads($('#inputLoadSearch').val().trim());
});

function GetMassiveLoads(operationId) {
    if (operationId != "") {
        if (operationId != $('#inputNewTemporal').text()) {
            $.ajax({
                type: "POST",
                url: rootPath + 'Collective/Collective/GetMassiveLoadsByOperationId',
                data: JSON.stringify({ operationId: $('#inputLoadSearch').val().trim(), issueTemp: false }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {
                if (data.success) {
                    $('#TableLoad').UifDataTable('clear');
                    if (data.result.length > 0) {
                        $('#TableLoad').UifDataTable('addRow', data.result);
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.NoUploadsAssociatedTemporary });
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorQueryLoadTemporary, 'autoclose': true })
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorQueryLoadTemporary, 'autoclose': true })
            });
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.NoCanEqualTemporaryNewAndOrigin });
        }
    }
}

$("#btnImportMassive").on("click", function () {
    ImportMassive();
});

$("#btnExit").on("click", function () {
    RedirectCollective();

});


function RedirectCollective() {
    var modelCollective = {
        IdLoad: $('#inputNewTemporal').text()
    };

    $.redirect(rootPath + 'Collective/Collective/Collective', modelCollective);
}

function ImportMassive() {    
    var LoadSelected = "";
    var items = $("#TableLoad").UifDataTable("getSelected");
    if (items != null) {
        for (var i = 0; i < items.length; i++) {
            if (i == items.length - 1) {
                LoadSelected += items[i].MassiveLoadId;
            } else {
                LoadSelected += items[i].MassiveLoadId + ',';
            }
        }

        var newOperationId = $("#inputNewTemporal").text();
        if (LoadSelected == "null") {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.SelectOneCharge, 'autoclose': true });
        }
        else if (newOperationId == "") {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.EnterNewTemporaryImportUpload, 'autoclose': true })
        }
        else {
            $.UifDialog('confirm', { 'message': Resources.Language.ProcessCopyProcesses+ " " + LoadSelected + ' al temporal ' + newOperationId + ' Desea Continuar?' }, function (result) {
                if (result) {
                    $.ajax({
                        type: "GET",
                        url: rootPath + 'Collective/Collective/ValidateImportMassive',
                        data: { LoadSelected: LoadSelected, newOperationId: newOperationId, tempSearch: $('#inputLoadSearch').val() },
                        contentType: "application/json; charset=utf-8"
                    }).done(function (data) {
                        if (data.success) {
                            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ProcessNo + data.result + Resources.Language.EndBeNotified });
                        }
                        else {
                            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorImporting + ' ' + data.result, 'autoclose': true })
                        }
                    }).fail(function (jqXHR, textStatus, errorThrown) {
                        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorImportingLoad, 'autoclose': true })
                    });
                }
            });
        }
    }
    else {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.NoDownloadsImport, 'autoclose': true })
    }
    
}