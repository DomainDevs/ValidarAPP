class Massive {
    static GetLoadTypes(endorsementType) {
        return $.ajax({
            type: 'POST',
            url: 'GetLoadTypes',
            data: JSON.stringify({ endorsementType: endorsementType }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
    static AddMassive(collectiveModelView) {
        return $.ajax({
            type: "POST",
            url: 'AddMassive',
            data: JSON.stringify({ collectiveModelView: collectiveModelView }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
    static GetTemporal(tempId) {
        return $.ajax({
            type: "POST",
            url: 'GetTemporalByTempId',
            data: JSON.stringify({ tempId: tempId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}

$(() => {
    new Collective();
    new MassiveLoadFinished();
});

class Collective extends Uif2.Page {
    getInitialState() {

        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        $("#inputLoadSearch").ValidatorKey(ValidatorType.Number, 2, 0);
        Collective.HidePanels();
        if (CollectiveModel.TempId != null && CollectiveModel.TempId !== "") {
            $('#inputLoadSearch').val(CollectiveModel.TempId)
            Collective.CollectiveLoadReady();
            $("#labelLoadSearch").text($('#inputLoadSearch').val().trim());
        }
        else {
            Collective.ShowButtons();
        }
        if ($('#inputLoadSearch').val().trim() === "0") {
            $('#inputLoadSearch').val("");
        }
        Collective.LoadTitle();
    }

    bindEvents() {
        $("#btnTariff").on('click', this.Tariff);
        $("#btnIssue").click(this.CreateIssuePolicy);
        $('#inputLoadSearch').on("search", this.CollectiveLoadSearch);
        $("#btnImportProcess").click(this.RedirectImportProcess);
        $("#btnDeleteLoad").click(this.DeteleMassiveLoad);
        $('#btnAddProcess').click(this.AddMassive);
        $("#btnExit").click(this.Exit);
        $("#inputExcelFile").on('change', this.AddFile);
        $('#inputExcelFile').fileupload({});
        $('#TableLoad tbody').on('dblclick', 'tr', this.TableLoad);
        $("#btnCreateIssue").click(this.IssuePolicy);
        $("#btnPrinter").on('click', this.Print);
        $('#btnErrors').click(this.GenerateErrors);
        $('#btnReport').click(this.Report);
    }
    Print() {
        Collective.RedirectPrinting('Printer/Printer')
    }

    AddFile(event) {
        if (event.target.files.length > 0) {
            if ($('#formFile').valid()) {
                $(this).fileupload({
                    dataType: 'json',
                    url: 'UploadFile',
                    done: function (event, data) {
                        if (data.result.success) {
                            $('#inputExcelFile').data('Object', data.result.result);
                            $('.progress .progress-bar').css('width', 100 + '%');
                        }
                        else {
                            $('#inputFile').data('Object', null);
                            $('.progress .progress-bar').css('width', 0 + '%');
                            $.UifNotify('show', { 'type': 'info', 'message': data.result.result, 'autoclose': true });
                        }
                    }
                })
            }
        }
    }

    TableLoad() {
        if ($("#TableLoad").UifDataTable("getSelected") != null) {
            $.each($("#TableLoad").UifDataTable("getData"), function (key, value) {
                $('#TableLoad tbody tr:eq(' + key + ')').removeClass('row-selected');
                $('#TableLoad tbody tr:eq(' + key + ') td button span').removeClass('glyphicon glyphicon-check').addClass('glyphicon glyphicon-unchecked');
            });
        }
        $(this).addClass("row-selected");
        $('td button span', this).removeClass();
        $('td button span', this).addClass("glyphicon glyphicon-check");

        var datos = $("#TableLoad").UifDataTable("getSelected")
        if (datos) {
            $.each($("#TableLoad").UifDataTable("getSelected"), function (key, value) {
                if (value.State == MassiveLoadStatus.Tariffing
                    || value.State == MassiveLoadStatus.Tariffed) {
                    Collective.HidePanels();
                    Collective.ShowPanelsCollective();
                    $('#inputDescriptionTariffed').val(value.Description);
                    $('#inputProcessTariffed').val(value.MassiveLoadId);
                    $('#inputRiskTariffed').val(value.CountRisk);
                    GetMassiveLoadTariff();
                }
                else if (value.State == MassiveLoadStatus.Issuing || value.State == MassiveLoadStatus.Issued || value.State == MassiveLoadStatus.Validated) {
                    Collective.HidePanels();
                    Collective.ShowPanelsCollective(MenuType.LoadFinished);
                    $('#inputTemporalFinish').val($('#inputLoadSearch').val());
                    $('#inputDescription').val(value.Description);
                    $('#inputProcess').val(value.MassiveLoadId);
                    $('#inputRisk').val(value.CountRisk);
                    $('#inputRiskError').val(value.CountRiskErrors);
                    $('#inputRiskProcessed').val(value.CountRiskProcessed);
                    MassiveLoadFinished.RequestCollectiveEmissionByMassiveLoadId(value.Id);

                }
            })
        }
    }

    Exit() {
        try {
            window.location = rootPath + "Home/Index";
        }
        catch (err) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorReturn, 'autoclose': true });
        }
    }

    Close() {
        Collective.HidePanels();
    }

    RedirectImportProcess() {
        if (CollectiveModel.TempId != null && CollectiveModel.TempId != "") {
            var tempIdImport = $('#inputLoadSearch').val().trim();
            if (tempIdImport != "") {
                $.ajax({
                    type: "POST",
                    url: rootPath + 'Collective/Collective/RedirectImportProcess',
                    data: JSON.stringify({ tempId: tempIdImport }),
                    contentType: "application/json; charset=utf-8"
                }).done(function (data) {
                    if (data.success) {
                        var modelCollective = {
                            TempId: tempIdImport,
                            EndorsementType: $('#hiddenEndorsement').val()
                        };
                        $.redirect(rootPath + 'Collective/Collective/ImportMassive', modelCollective);
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorImportingTemporary, 'autoclose': true })
                });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.PlaceHolderTemporary, 'autoclose': true });
            }
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.InvalidTemporal, 'autoclose': true });
        }
    }

    CollectiveLoadSearch() {
        Collective.CollectiveLoadReady();
    }

    DeteleMassiveLoad() {
        if ($('#hiddenTempId').val() == "0" || $('#hiddenTempId').val() == "") {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.PlaceHolderTemporary, 'autoclose': true });
        }
        else {
            var datos = $('#TableLoad').UifDataTable('getSelected');
            if (datos) {
                if (datos.length === 1) {
                    $.UifDialog('confirm',
                        { 'message': Resources.Language.ConfirmDelete },
                        function (result) {
                            if (result) {
                                $.ajax({
                                    type: "POST",
                                    url: rootPath + 'Collective/Collective/DeleteProcess',
                                    data: { massiveLoad: datos[0] }
                                }).done(function (data) {
                                    if (data.success) {
                                        Collective.CollectiveLoadReady();
                                        $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.MsgCorrectRemoval });
                                    } else {
                                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                                    }
                                }).fail(function () {
                                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErroDeleteInformation, 'autoclose': true });
                                });
                            }
                        });
                } else {
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErroDeleteInformation, 'autoclose': true });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.DeleteLoadStateCompletedChecked, 'autoclose': true });
            }
        }
    }

    AddMassive() {
        if (CollectiveModel.TempId == null || CollectiveModel.TempId == '' || CollectiveModel.TempId == 0) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.InvalidTemporal, 'autoclose': true });
        }
        else {
            Collective.ShowPanelsCollective(MenuType.AddMassive)
            Collective.ClearFormAdd();
            $('#inputTemporal').val($('#inputLoadSearch').val());
        }
    }

    Tariff() {
        var loads = $('#TableLoad').UifDataTable('getSelected');
        if (loads != null && loads.length > 0) {
            var allValidated = true;
            var collectiveLoads = [];
            $.each(loads, function (key, value) {
                if (this.StateDescription != Resources.Language.Validated.toUpperCase()) {
                    allValidated = false;
                    return;
                }
                else {
                    collectiveLoads.push(this.Id);
                }
            });
            if (allValidated) {
                CollectiveModel.CollectiveLoads = collectiveLoads;
                Collective.TariffedLoad(CollectiveModel).done(function (data) {
                    if (data.success) {
                        Collective.ShowButtons(MassiveLoadStatus.Tariffed);
                        $.UifDialog('alert', { 'message': data.result }, null);
                    } else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.PendingProcesses, 'autoclose': true });
            }
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.NotSelectedLoad, 'autoclose': true });
        }
    }

    CreateIssuePolicy() {
        var loads = $('#TableLoad').UifDataTable('getSelected');
        if (loads != null && loads.length > 0) {
            var allValidated = true;
            var collectiveLoads = [];
            $.each(loads, function (key, value) {
                if (this.StateDescription != Resources.Language.Tariffed.toUpperCase()) {
                    allValidated = false;
                    return;
                }
                else {
                    collectiveLoads.push(this.Id);
                }
            });
            if (allValidated == true) {
                CollectiveModel.CollectiveLoads = collectiveLoads;
                Collective.CreateIssuePolicy(CollectiveModel).done(function (data) {
                    if (data.success) {
                        if (data.result != null) {
                            Collective.ShowSummaryIssuedPolicy(data.result);
                        }
                        Collective.ShowButtons(MassiveLoadStatus.Issued);
                    } else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                });
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.PendingProcesses, 'autoclose': true });
            }
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.NotSelectedLoad, 'autoclose': true });
        }
    }
    IssuePolicy() {
        if (CollectiveModel.TempId != null) {
            Collective.IssuePolicy(CollectiveModel).done(function (data) {
                if (data.success) {
                    if (data.result != null) {
                        var resultado = data.result;
                        var mensageLength = Resources.Language.PolicyNumber.length - 3;
                        CollectiveModel.PolicyNumber = resultado.substring(mensageLength);
                        $.UifDialog('alert', { 'message': data.result }, function () {
                            Collective.HidePanels()
                        });
                    }
                } else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
    }

    GenerateErrors() {
        var loads = $('#TableLoad').UifDataTable('getSelected');
        if (loads != null && loads.length > 0) {
            if (loads.length == 1) {
                var allValidated = true;
                var collectiveLoads = [];
                $.each(loads, function (key, value) {
                    collectiveLoads.push(this.Id);
                });
                if (allValidated) {
                    CollectiveModel.CollectiveLoads = collectiveLoads;
                    CollectiveModel.LoadTypeId = $("#selectTypeLoad").UifSelect("getSelected");
                    Collective.GenerateFileToCollectiveLoad(CollectiveModel).done(function (data) {
                        if (data.success) {
                            DownloadFile(data.result);
                        }
                        else {
                            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                        }
                    });
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.PendingProcesses, 'autoclose': true });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.SelectJustOneLoad, 'autoclose': true });
            }
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.NotSelectedLoad, 'autoclose': true });
        }
    }

    //oculta los paneles    
    static HidePanels() {
        $("#modalAddMassive").UifModal("hide");
        $("#modalIssuePolicy").UifModal("hide");
        $("#modalMassiveLoadFinished").hide();
    }

    static ShowPanelMassiveLoadIssue() {
        $("#modalIssuePolicy").UifModal("showLocal", "Emisión de póliza - Temporal " + $('#inputLoadSearch').val());
    }

    static LoadTitle() {
        var titlePrincipal = Resources.Language.LabelTemporal + ":" + $('#inputLoadSearch').val() + " - " + $("#hiddenEndorsement").val();
        $.uif2.helpers.setGlobalTitle(titlePrincipal);
    }

    static CollectiveLoadReady() {
        if ($('#inputLoadSearch').val().trim() != "") {
            Collective.GetTemporal($('#inputLoadSearch').val().trim());
            Massive.GetLoadTypes(MassiveProcessType.Emission).done(function (data) {
                if (data.success) {
                    if (CollectiveModel.PolicyNum == "Emisión") {
                        $("#selectTypeLoad").UifSelect({ sourceData: data.result, selectedId: SubMassiveProcessType.CollectiveEmission });
                        $("#selectTypeLoad").prop('disabled', 'disabled');
                        CollectiveModel.LoadTypeId = SubMassiveProcessType.CollectiveEmission;
                    }
                    else if (CollectiveModel.PolicyNum == "Renovación") {
                        $("#selectTypeLoad").UifSelect({ sourceData: data.result, selectedId: SubMassiveProcessType.CollectiveRenewal });
                        $("#selectTypeLoad").prop('disabled', 'disabled');
                        CollectiveModel.LoadTypeId = SubMassiveProcessType.CollectiveRenewal;
                    }
                    else {
                        var loadTypes = [];
                        $.each(data.result, function (index, value) {
                            if (this.Id != SubMassiveProcessType.CollectiveEmission || this.Id != SubMassiveProcessType.CollectiveRenewal) {
                                loadTypes.push({
                                    Id: this.Id,
                                    Description: this.Description
                                });
                            }
                        });
                        $("#selectTypeLoad").UifSelect({ sourceData: loadTypes });
                        if (CollectiveModel.LoadTypeId != 0 && CollectiveModel.LoadTypeId != null) {
                            $("#selectTypeLoad").UifSelect({ sourceData: data.result, selectedId: CollectiveModel.LoadTypeId });
                            $("#selectTypeLoad").prop('disabled', 'disabled');
                        }
                    }

                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
            $('#TableLoad').UifDataTable('clear');
            $.ajax({
                type: "POST",
                url: rootPath + "Collective/Collective/CollectiveLoadsByTempId",
                data: JSON.stringify({
                    tempId: CollectiveModel.TempId
                }),
                dataType: "json",
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {
                if (data.success) {
                    if (data.result.length > 0) {
                        var collectiveLoad = [];
                        var status = "";
                        $.each(data.result, function (index, value) {
                            if (this.Status == 1) {
                                status = Resources.Language.Validating;
                            }
                            else if (this.Status == 2) {
                                status = Resources.Language.Validated;
                            }
                            else if (this.Status == 3) {
                                status = Resources.Language.Tariffing;
                            }
                            else if (this.Status == 4) {
                                status = Resources.Language.Tariffed;
                            }
                            else if (this.Status == 5) {
                                status = Resources.Language.Issuing;
                            }
                            else if (this.Status == 6) {
                                status = Resources.Language.Issued;
                            }
                            else {
                                status = this.Status;
                            }
                            collectiveLoad.push({
                                Id: this.Id,
                                TempId: this.TemporalId,
                                Description: this.Description,
                                StateDescription: status.toUpperCase(),
                                AccountName: this.User.UserId,
                                CountRisk: this.TotalRows,
                                CountRiskErrors: this.WithErrors,
                                CountRiskProcessed: this.Rows.length,
                                State: this.Status,
                                FieldSetId: '',
                                LoadType: this.LoadType
                            });
                        });
                        $('#TableLoad').UifDataTable('clear');
                        $('#TableLoad').UifDataTable('addRow', collectiveLoad);
                    }
                    $("#modalAgentsCommissions").UifModal('showLocal', Resources.Language.LabelCommissions);
                }
            });
        }
    }

    static GetTemporal(tempId) {
        if (tempId != 0 && tempId != "") {
            Massive.GetTemporal(tempId).done(function (data) {
                if (data.success) {
                    Collective.ShowButtons(MassiveLoadStatus.Validated);
                    Collective.LoadTemporal(data.result);
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result });
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorQueryTemporary, 'autoclose': true })
            });
        }
        else {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.PlaceHolderTemporary, 'autoclose': true });
        }
    }

    static RedirectPrinting(controller) {
        if (CollectiveModel.PolicyNumber != undefined) var PolicyNumber = CollectiveModel.PolicyNumber;
        else var PolicyNumber = CollectiveModel.TempId;
        var printerModelsView = {
            PolicyNumber: PolicyNumber,
            BranchId: CollectiveModel.BranchId,
            PrefixId: CollectiveModel.PrefixId
        };

        $.redirect(rootPath + 'Printing/' + controller, printerModelsView);
    }

    static ClearFormAdd() {
        $('#inputTemporal').val("");
        $("#inputNameLoad").val("");
        $('.progress .progress-bar').css('width', '0%');
        $('#divFile :input[type=text]').val('');
    }

    static LoadTemporal(tempData) {
        CollectiveModel.Id = $('#inputLoadSearch').val().trim();
        CollectiveModel.TempId = tempData.Id;
        CollectiveModel.PrefixId = tempData.Prefix.Id;
        CollectiveModel.BranchId = tempData.Branch.Id;
        CollectiveModel.EndorsementType = tempData.Endorsement.EndorsementTypeDescription;
        CollectiveModel.EndorsementTypeId = tempData.Endorsement.EndorsementType;
        CollectiveModel.AgencyId = tempData.Agencies[0].Code;
        CollectiveModel.AgentId = tempData.Agencies[0].Agent.IndividualId;
        CollectiveModel.PolicyNum = tempData.Endorsement.EndorsementTypeDescription;
        CollectiveModel.EndorsementType = tempData.DocumentNumber;
        CollectiveModel.ProductId = tempData.Product.Id;
        CollectiveModel.LoadTypeId = tempData.SubEndorsementType;
    }

    static CollectiveLoads() {
        $.ajax({
            type: "POST",
            url: rootPath + "Collective/Collective/CollectiveLoadsByTempId",
            data: JSON.stringify({
                tempId: CollectiveModel.TempId
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static TariffedLoad(collective) {
        return $.ajax({
            type: 'POST',
            url: 'TariffedLoad',
            data: JSON.stringify({ collectiveViewModel: collective }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static CreateIssuePolicy(collective) {
        return $.ajax({
            type: 'POST',
            url: 'CreateIssuePolicy',
            data: JSON.stringify({ collectiveModelView: collective }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    static IssuePolicy(collective) {
        return $.ajax({
            type: 'POST',
            url: 'IssuePolicy',
            data: JSON.stringify({ collectiveModelView: collective }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GenerateFileToCollectiveLoad(collective) {
        return $.ajax({
            type: 'POST',
            url: 'GenerateFileToCollectiveLoad',
            data: JSON.stringify({ collectiveModelView: collective }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GetCollectiveEmissionByMassiveLoadId(massiveLoadId) {
        return $.ajax({
            type: 'POST',
            url: 'GetCollectiveEmissionByMassiveLoadId',
            data: JSON.stringify({ massiveLoadId: massiveLoadId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static ShowButtons(status) {
        switch (status) {
            case MassiveLoadStatus.Validated:
                $('#btnPrinter').show();
                $('#btnErrors').show();
                $('#btnReport').show();
                $('#btnIssue').show();
                $('#btnTariff').show();
                $('#btnDeleteLoad').show();
                $('#btnAddProcess').show();
                $('#btnDeleteProcess').show();
                break;
            case MassiveLoadStatus.Tariffed:
                $('#btnIssue').show();
                $('#btnErrors').show();
                $('#btnPrinter').show();
                $('#btnReport').show();
                $('#btnDeleteLoad').show();
                break;
            case MassiveLoadStatus.Issued:

                $('#btnPrinter').show();
                $('#btnErrors').show();
                $('#btnReport').show();
                $('#btnIssue').show();
                $('#btnTariff').hide();
                $('#btnDeleteLoad').hide();
                break;
            default:
                $('#btnPrinter').hide();
                $('#btnErrors').hide();
                $('#btnReport').hide();
                $('#btnIssue').hide();
                $('#btnTariff').hide();
                $('#btnDeleteLoad').hide();
                $('#btnAddProcess').hide();
                $('#btnDeleteProcess').hide();

                break;
        }
    }
    static ShowSummaryIssuedPolicy(result) {
        if (CollectiveModel.TempId == '' || CollectiveModel.TempId == 0) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.InvalidTemporal, 'autoclose': true });
        }
        else {
            Collective.ClearFormSummary();
            $('#inputTemporalIssue').val($('#inputLoadSearch').val());
            $('#inputProcessIssue').val(result.CollectiveEmissions.length);
            $('#MassiveCountRisk').text(result.CollectiveEmissionRows.length);
            $("#MassiveSumPremiun").text(result.Premium);
            $('#MassiveSumLimit').text(result.AmmountInsured);
            $('#MassiveCountRiskErrors').text(result.Errors);
            $('#MassiveCountRiskEvents').text(result.RiskEvents);
            $('#MassiveCountEventPolicy').text(result.PolicyEvents);

            Collective.ShowPanelMassiveLoadIssue();
        }
    }
    static ClearFormSummary() {
        $('#MassiveCountRisk').val("");
        $("#MassiveSumPremiun").val("");
        $('#MassiveSumLimit').val("");
        $('#MassiveCountRiskErrors').val("");
        $('#MassiveCountRiskEvents').val("");
        $('#MassiveCountEventPolicy').val("");
    }

    static ShowPanelsCollective(Menu) {
        switch (Menu) {
            case MenuType.Massive:
                Collective.ShowTitleCollective(MenuType.Massive)
                $("#modalCollective").show();
                $("#buttonsCollective").show();
                break;
            case MenuType.AddMassive:
                $("#modalAddMassive").UifModal("showLocal", "Cargue de Riesgos - Temporal " + $('#inputLoadSearch').val());
                break;
            case MenuType.LoadFinished:
                Collective.ShowTitleCollective(MenuType.LoadFinished)
                $("#modalMassiveLoadFinished").show();
                $("#buttonsFinished").show();
                break;
            case MenuType.LoadTariffed:
                Collective.ShowTitleCollective(MenuType.LoadTariffed)
                $("#modalMassiveLoadTariffed").show();
                $("#buttonsTariffed").show();
                break;
            case MenuType.Issue:
                Collective.ShowTitleCollective(MenuType.Issue)
                $("#modalMassiveLoadIssue").show();
                $("#buttonsIssue").show();
                break;
            default:
                break;
        }
    }
    //Mostrar Titulo de acuerdo a seccion
    static ShowTitleCollective(Title) {
        $("#contentInputLoadSearch").hide();
        $("#contentLabelLoadSearch").show();
        switch (Title) {
            case MenuType.Massive:
                $("#titleCollective").text(Resources.Language.TemporalUploadQuery);
                $("#contentInputLoadSearch").show();
                $("#contentLabelLoadSearch").hide();
                break;
            case MenuType.LoadFinished:
                $("#titleCollective").text(Resources.Language.QueryMassiveProcess);
                break;
            case MenuType.LoadTariffed:
                $("#titleCollective").text(Resources.Language.QueryChargeRate);
                break;
            case MenuType.Issue:
                $("#titleCollective").text(Resources.Language.IssuePolicyCollective);
                break;
            default:
                break;
        }
    }

    Report() {
        var loads = $('#TableLoad').UifDataTable('getSelected');
        if (loads != null && loads.length > 0) {
            if (loads.length == 1) {
                var allValidated = true;
                var collectiveLoads = [];
                $.each(loads, function (key, value) {
                    collectiveLoads.push(this.Id);
                });
                if (allValidated) {
                    CollectiveModel.CollectiveLoads = collectiveLoads;
                    CollectiveModel.LoadTypeId = $("#selectTypeLoad").UifSelect("getSelected");
                    Collective.GenerateReportByMassiveLoadId(loads[0].Id).done(function (data) {
                        if (data.success) {
                            DownloadFile(data.result);
                        }
                        else {
                            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                        }
                    });
                }
                else {
                    $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.PendingProcesses, 'autoclose': true });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.SelectJustOneLoad, 'autoclose': true });
            }
        }
        else {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.NotSelectedLoad, 'autoclose': true });
        }        
    }
    static GenerateReportByMassiveLoadId(massiveLoadId) {
        return $.ajax({
            type: 'POST',
            url:  'GenerateReportByMassiveLoadId',
            data: JSON.stringify({ massiveLoadId: massiveLoadId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}
