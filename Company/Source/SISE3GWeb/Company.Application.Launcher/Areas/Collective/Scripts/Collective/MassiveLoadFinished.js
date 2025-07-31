class MassiveLoadFinished extends Uif2.Page {
    getInitialState() {

    }
    bindEvents() {
    }
    static RequestCollectiveEmissionByMassiveLoadId(massiveLoadId) {
        Collective.GetCollectiveEmissionByMassiveLoadId(massiveLoadId).done(function (data) {
            if (data.success) {
                $('#tableProcessList').UifDataTable('clear');
                if (data.result.length > 0) {
                    $('#tableProcessList').UifDataTable('addRow', data.result);
                }
            }
            else {
                $('#tableProcessList').UifDataTable('clear');
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorQueryLoadTemporary, 'autoclose': true })
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorQueryTemporary, 'autoclose': true })
        });;
    }

}

function EventsLoadFinish() {
    $("#btnCloseFinish").on("click", function () {
        Collective.HidePanels(1)
        ShowPanelsCollective(MenuType.Massive);
    });

    $("#btnExcelErrors").on("click", function () {
        ExcelErrors();
    });

    $("#btnDeleteError").on("click", function () {
        DeleteErrors();
    });

    $("#btnTariffed").on("click", function () {
        TariffedDetail();
    });

}


function ExcelErrors() {
    var CountRiskErrors = $('#inputRiskError').val();
    if (CountRiskErrors != 0) {
        window.location.href = rootPath + "Collective/Collective/GenerateFileExcel?" + "massiveLoadId=" + $('#inputProcess').val() + "&errors=" + true + "&tariffed=" + false + "&temporalId=" + 0 + "&operationId=" + $('#inputLoadSearch').val();
    }
    else {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.SelectedLoadNoRisksWithError, 'autoclose': true })
    }
}

function TariffedDetail() {
    var datos = $('#TableLoad').UifDataTable('getSelected');

    if (datos) {
        if (datos[0].CountRiskProcessed > 0) {
            var massiveLoadsId = [];
            massiveLoadsId.push($('#inputProcess').val());
            $.UifDialog('confirm', { 'message': Resources.Language.ConfirmRating }, function (result) {
                if (result) {
                    $.ajax({
                        type: "GET",
                        url: rootPath + 'Collective/Collective/TariffedMassive',
                        data: { "tempId": $('#hiddenTempId').val(), "listMassiveId": JSON.stringify(massiveLoadsId), "prefixId": $("#hiddenPrefixCommercial").val(), "productId": $("#hiddenProduct").val() },
                        contentType: "application/json; charset=utf-8"
                    }).done(function (data) {
                        if (data.success) {
                            Collective.HidePanels(1)
                            ShowPanelsCollective(MenuType.Massive);
                            GetMassiveLoads($('#hiddenTempId').val().trim(), false);
                            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.RateExecutedCorrectly, 'autoclose': true });
                        }
                        else {
                            $.UifNotify('show', { 'type': 'info', 'message': data.result.ObjectId + ': ' + Resources.Language.ErrorExecutedRate, 'autoclose': true });
                        }
                    }).fail(function (jqXHR, textStatus, errorThrown) {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result.ObjectId + ': ' + Resources.Language.ErrorExecutedRate, 'autoclose': true });
                    });
                }
            });
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.NoRisksRate });
        }
    }
}


function DeleteErrors() {
    var errors = $('#inputRiskError').val();
    if (errors != 0 && errors != undefined) {
        $.ajax({
            type: "POST",
            url: rootPath + 'Collective/Collective/DeleteRiskErrorByMassiveLoadId',
            data: JSON.stringify({ massiveLoadId: $('#inputProcess').val() }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data.success) {
                $('#inputRiskError').val(0);
                Collective.HidePanels(1)
                ShowPanelsCollective(MenuType.Massive);
                GetMassiveLoads($('#hiddenTempId').val().trim(), false);
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MsgCorrectRemoval, 'autoclose': true });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorDeletingRegister, 'autoclose': true });
            }
        });
    }
    else {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.UploadNoRisksWithError, 'autoclose': true });
    }
}