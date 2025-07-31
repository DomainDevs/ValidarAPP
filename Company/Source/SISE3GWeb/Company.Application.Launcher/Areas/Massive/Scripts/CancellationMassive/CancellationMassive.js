var searchType;
var CurrentLoadStatus = {
    New: 0,
    Loaded: 1,
    Tariffed: 2,
    Issued: 3
}
var LoadStatus;

$(() => {
    new CancellationMassive();
});

class CancellationMassiveLoad {
    static CreateLoad(CancellationMassiveViewModel) {
        return $.ajax({
            type: 'POST',
            url: 'CreateLoad',
            data: JSON.stringify({ cancellationMassiveViewModel: CancellationMassiveViewModel }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    static GetMassiveLoadsByDescription(description) {
        return $.ajax({
            type: 'POST',
            url: 'GetMassiveLoadsByDescription',
            data: JSON.stringify({ description: description }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    static TariffedLoad(massiveLoadId) {
        return $.ajax({
            type: 'POST',
            url: 'TariffedLoad',
            data: JSON.stringify({ massiveLoadId: massiveLoadId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
}

class CancellationMassive extends Uif2.Page {
    getInitialState() {
        CancellationMassive.ClearForm();
        CancellationMassive.HideControls();
        CancellationMassive.ShowButtons(CurrentLoadStatus.New);
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
    }

    bindEvents() {
        $('#inputLoadId').on('buttonClick', this.SearchLoad);
        $('#inputFile').on('change', this.AddFile);
        $('#inputFile').fileupload({});
        $('#btnAddLoad').click(this.AddLoad);
        $('#btnExit').click(this.Exit);
        $('#btnNewLoad').click(this.NewLoad);
        $('#btnTariff').click(this.Tariff);
        $("#btnCancelPolicy").click(this.IssuePolicy);
        $('#btnExport').click(this.Export);
        $('#btnMassivePrint').click(this.Print);
    }

    static ShowButtons(currentLoadStatus) {
        switch (currentLoadStatus) {
            case CurrentLoadStatus.New:
                $('#btnMassivePrint').hide();
                $('#btnExport').hide();
                $('#btnTariff').hide();
                $('#btnCancelPolicy').hide();
                break;
            case CurrentLoadStatus.Loaded:
                $('#btnTariff').show();
                $('#btnExport').show();
                $('#btnCancelPolicy').hide();
                $('#btnMassivePrint').hide();
                break;
            case CurrentLoadStatus.Tariffed:
                $('#btnCancelPolicy').show();
                $('#btnExport').show();
                $('#btnTariff').hide();
                $('#btnMassivePrint').hide();
                break;
            case CurrentLoadStatus.Issued:
                $('#btnMassivePrint').show();
                $('#btnExport').show();
                $('#btnTariff').hide();
                $('#btnCancelPolicy').hide();
                break;
        }
    }

    static ShowDefaultResults(dataTable) {
        $('#tableResults').UifDataTable('clear');
        $('#tableResults').UifDataTable('addRow', dataTable);
    }

    static HideControls() {
        $('#btnExport').hide();
        $('#btnMassivePrint').hide();
        $('#btnTariff').hide();
        $('#btnCancelPolicy').hide();
        $('#divErrors').hide();
        $('#divTemporals').hide();
        $('#divPolicies').hide();
    }

    static DisableControls() {
        $('.progress .progress-bar').css('width', 0 + '%');
    }

    static ClearForm() {
        $('#formCancellationMassive').formReset();
        $('#inputFile').prop('disabled', false);
        $('#btnAddLoad').prop('disabled', 'disabled');
        CancellationMassive.HideControls();
        CancellationMassive.ShowButtons(CurrentLoadStatus.New);
        $("#btnNewLoad").removeClass("btn-default");
        $("#btnNewLoad").addClass("btn-primary");
    }

    SearchLoad() {
        searchType = 4;
        var description = $('#inputLoadId').val().trim();
        var number = parseInt(description, 10);
        var loads = [];

        if ((!isNaN(number) && number !== 0) || description.length > 2) {
            CancellationMassiveLoad.GetMassiveLoadsByDescription(description).done(function (data) {
                if (data.success) {
                    if (data.result !== null) {
                        if (data.result.length == 1) ///Object.prototype.toString.call(data.result) =="[object Object]")
                        {
                            CancellationMassive.LoadMassiveLoad(data.result[0]);
                        }
                        else {
                            for (var i = 0; i < data.result.length; i++) {
                                loads.push({
                                    Id: data.result[i].Id,
                                    Code: data.result[i].Id,
                                    Description: data.result[i].Description
                                });
                            }
                            CancellationMassive.ShowDefaultResults(loads);
                            $('#modalDefaultSearch').UifModal('showLocal', 'Seleccione un Cargue');
                        }
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': 'Cargue no existe', 'autoclose': true });
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
    }

    static LoadMassiveLoad(massiveLoad) {
        CancellationMassive.ClearForm();
        $('#inputLoadId').data('Object', massiveLoad);
        $('#inputLoadId').val(massiveLoad.Id);
        CancellationMassive.ShowButtons(CurrentLoadStatus.Loaded);
        $('#inputLoadName').val(massiveLoad.Description);
        $('#inputStateDescription').data('Object', massiveLoad.Status);
        $('#inputStateDescription').val(massiveLoad.StatusDescription);
        $('#inputRecordsTotal').val(massiveLoad.TotalRows);
        $('#inputRecordsProcessed').val(massiveLoad.Summary.Processeds);
        $('#inputRecordsPendings').val(massiveLoad.Summary.Pendings);
        $('#inputRecordsEvents').val(massiveLoad.Summary.WithEvent);
        $('#inputRecordsErrors').val(massiveLoad.Summary.WithError);
        CancellationMassive.DisableControls();
        $('#inputFile').prop('disabled', 'disabled');
        $('#btnAddLoad').prop('disabled', 'disabled');
        if (massiveLoad.Status == MassiveLoadStatus.Validating) {
            $("#btnNewLoad").removeClass("btn-primary");
            $("#btnNewLoad").addClass("btn-default");
            $('#btnTariff').show();
        }
        else if (massiveLoad.Status == MassiveLoadStatus.Tariffed) {
            $("#btnNewLoad").removeClass("btn-primary");
            $("#btnNewLoad").addClass("btn-default");
            $('#btnCancelPolicy').show();
            CancellationMassive.GetTariffResults(massiveLoad.Id).done(function (data) {
                if (data.success) {
                    $('#tableTemporals').UifDataTable('clear');
                    if (data.result.length > 0) {
                        $.each(data.result, function (index, value) {
                            this.Risk.Policy.Summary.FullPremium = FormatMoney(this.Risk.Policy.Summary.FullPremium);
                            this.TotalCommission = FormatMoney(this.TotalCommission);

                        });
                        $('#tableTemporals').UifDataTable('addRow', data.result);
                        $('#divTemporals').show();
                        CancellationMassive.ShowButtons(CurrentLoadStatus.Tariffed);
                    }

                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
        else if (massiveLoad.Status == MassiveLoadStatus.Issued) {
            CancellationMassive.GetIssuanceResults(massiveLoad.Id).done(function (data) {
                if (data.success) {

                    $('#TableIssue').UifDataTable('clear');
                    if (data.result.length > 0) {
                        $('#TableIssue').UifDataTable('addRow', data.result);
                        $('#divPolicies').show();
                        CancellationMassive.ShowButtons(CurrentLoadStatus.Issued);
                    }

                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
    }

    static GetIssuanceResults(description) {
        return $.ajax({
            type: 'POST',
            url: 'GetIssuanceResults',
            data: JSON.stringify({ description: description }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetTariffResults(description) {
        return $.ajax({
            type: 'POST',
            url: 'GetTariffResults',
            data: JSON.stringify({ description: description }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }


    static EnableControls() {
        CancellationMassive.ClearForm();
    }

    AddFile(event) {
        if (event.target.files.length > 0) {
            if ($('#formCancellationMassive').valid()) {
                $(this).fileupload({
                    dataType: 'json',
                    url: 'UploadFile',
                    done: function (event, data) {
                        if (data.result.success) {
                            $('#inputFile').data('Object', data.result.result);
                            $('.progress .progress-bar').css('width', 100 + '%');
                            $('#btnAddLoad').prop('disabled', false);
                        }
                        else {

                            $('#btnAddLoad').prop('disabled', true);
                            $('#inputFile').data('Object', null);
                            $('.progress .progress-bar').css('width', 0 + '%');
                            $.UifNotify('show', { 'type': 'info', 'message': data.result.result, 'autoclose': true });
                        }
                    }
                })
            }
        }
    }

    AddLoad() {
        if ($('#formCancellationMassive').valid()) {
            var formCancellationMassive = $('#formCancellationMassive').serializeObject();
            formCancellationMassive.FileName = $('#inputFile').data('Object');

            CancellationMassiveLoad.CreateLoad(formCancellationMassive).done(function (data) {
                if (data.success) {
                    $.UifDialog('alert', { 'message': data.result }, null);
                    var message = data.result.split(' ');
                    $('#inputLoadId').val(message[message.length - 1]);
                    CancellationMassive.DisableControls();
                    CancellationMassive.ShowButtons(CurrentLoadStatus.Loaded);
                    $('#inputLoadName').prop('disabled', 'disabled');
                    $('#inputFile').prop('disabled', 'disabled');
                    $('#btnAddLoad').prop('disabled', 'disabled');
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
    }

    Exit() {
        window.location = rootPath + "Home/Index";
    }

    NewLoad() {
        CancellationMassive.ClearForm();
    }
    Tariff() {

        var massiveLoad = $('#inputLoadId').data('Object');
        if (massiveLoad.TotalRows == $('#inputRecordsProcessed').val() && massiveLoad.TotalRows > 0) {
            if ($('#inputStateDescription').val() == Resources.Language.Validated) {
                CancellationMassiveLoad.TariffedLoad(massiveLoad.Id).done(function (data) {
                    if (data.success) {
                        $.UifDialog('alert', { 'message': data.result }, null);
                    } else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ProcessTariffed, 'autoclose': true });
            }
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.PendingProcesses, 'autoclose': true });
        }

    }
    IssuePolicy() {
        var massiveLoad = $('#inputLoadId').data('Object');
        if (massiveLoad.TotalRows == $('#inputRecordsProcessed').val() && massiveLoad.TotalRows > 0) {
            if ($('#inputStateDescription').val() == Resources.Language.Tariffed) {
                if (massiveLoad != null) {
                    Massive.IssuePolicy(massiveLoad.Id).done(function (data) {
                        if (data.success) {
                            $.UifDialog('alert', { 'message': data.result }, null);
                        } else {
                            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                        }
                    });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ProcessIssued, 'autoclose': true });
            }
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.PendingProcesses, 'autoclose': true });
        }
    }

    Export() {
        var massiveStatus = 0;

        Massive.GenerateFileToMassiveLoad($('#inputLoadId').val()).done(function (data) {
            if (data.success) {
                DownloadFile(data.result);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });

    }

    AdvancedSearch() {
        dropDownSearch.show();
    }

    Print() {
        var printerModelsView = {
            IdLoad: $('#inputLoadId').val()
        };

        $.redirect(rootPath + 'Printing/Printer/Printer', printerModelsView);
    }
}