$.ajaxSetup({ async: false });
//Codigo para la pantalla LoadTariffed
$(document).ready(function () {
    EventsLoadTariffed();
});

function GetMassiveLoadTariff() {
    if ($('#inputTemporalTariffed').val() != "" || $('#inputProcessTariffed').val() != "") {
        $.ajax({
            type: "POST",
            url: rootPath + 'Collective/Collective/GetMassiveLoadTariffed',
            data: JSON.stringify({ tempId: $('#hiddenTempId').val(), massiveLoadId: $('#inputProcessTariffed').val() }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data.success) {
                $('#tableConsultLoadTariffed').UifDataTable('clear');
                if (data.result.length > 0) {
                    data.result[0].CountRisk = data.result[0].CountRisk - data.result[0].CountRiskErrors;
                    data.result[0].SumPremiun = FormatMoney(data.result[0].SumPremiun);
                    data.result[0].SumLimit = FormatMoney(data.result[0].SumLimit);                    
                    $('#tableConsultLoadTariffed').UifDataTable('addRow', data.result);
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.NoUploadsAssociatedTemporary, 'autoclose': true });
                }
            }
            else {
                $('#tableConsultLoadTariffed').UifDataTable('clear');
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorQueryLoadTemporary, 'autoclose': true })
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorQueryTemporary, 'autoclose': true })
        });

    }
}

function EventsLoadTariffed() {
    $("#btnCloseTariffed").on("click", function () {
        Collective.HidePanels(1)
        ShowPanelsCollective(MenuType.Massive);
    });

    $("#btnDeleteRisks").on("click", function () {
        DeteleMassiveRisk();
    });

    $("#btnRiskError").on("click", function () {
        ExcelError();
    });
 
    $("#btnRiskTarif").on("click", function () {
        ExcelRiskTarif();
    });

 
}



function DeteleMassiveRisk() {
    var datos = $('#TableLoad').UifDataTable('getSelected');
    if (datos && datos.length > 0 && datos[0].CountRiskErrors > 0) {
        var carguesId = [];
        var msj = "";
        $.each(datos, function () {
            carguesId.push(this.MassiveLoadId);
            msj += "<b>" + this.MassiveLoadId + " - " + this.Description + "</b><br />";
        });

        $.UifDialog('confirm', { 'message': Resources.Language.ConfirmDelete }, function (result) {
            if (result) {                            
                $.ajax({
                    type: "POST",
                    url: rootPath + 'Collective/Collective/DeleteRiskErrorTariffedByMassiveLoadId',
                    data: JSON.stringify({ massiveLoadId: $('#inputProcessTariffed').val() }),                    
                    dataType: "json",
                    contentType: "application/json; charset=utf-8"
                }).done(function (data) {
                    if (data.success) {
                        Collective.HidePanels(1)
                        ShowPanelsCollective(MenuType.Massive);                        
                        GetMassiveLoads($('#hiddenTempId').val().trim(), false);
                        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MsgCorrectRemoval });
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result.ObjectId + ': ' + Resources.Language.ErrorDeleteRecords, 'autoclose': true });
                    }
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result.ObjectId + ': ' + Resources.Language.ErroDeleteInformation, 'autoclose': true });
                });
            }
        });
    }
    else {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.UploadNoRecordsWithError, 'autoclose': true });
    }
}

function ExcelRiskTarif() {
    var tableData = $('#tableConsultLoadTariffed').UifDataTable('getData');
    var CountRisk = parseInt(tableData[0].CountRisk);
    var CountRiskErrors = parseInt(tableData[0].CountRiskErrors);
    if (CountRisk == 0) {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.SelectedLoadNoRatedRisks, 'autoclose': true })
    }
    else {
        window.location.href = rootPath + "Collective/Collective/GenerateFileExcel?" + "massiveLoadId=" + $('#inputProcessTariffed').val() + "&errors=" + false + "&tariffed=" + true + "&temporalId=" + 0 + "&operationId=" + $('#inputLoadSearch').val();
    }
}

function ExcelError() {
    var tableData = $('#tableConsultLoadTariffed').UifDataTable('getData');
    var totErrores = parseInt(tableData[0].CountRiskErrors);
    if (totErrores <= 0) {
        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.SelectedLoadNoRisksWithError, 'autoclose': true })
    }
    else {
        window.location.href = rootPath + "Collective/Collective/GenerateFileExcel?" + "massiveLoadId=" + $('#inputProcessTariffed').val() + "&errors=" + true + "&tariffed=" + true + "&temporalId=" + 0 + "&operationId=" + $('#inputLoadSearch').val();
    }
}

   
