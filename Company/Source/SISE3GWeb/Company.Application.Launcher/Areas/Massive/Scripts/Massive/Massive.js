var searchType;
var selectedFileType;
var loadValidate = false;
var steps = {};
var loadProcess = {};
var massiveLoadGlobal;
var statusLoad;
var riskToPrint = 0;

class MassiveRequest {

    static GetProcessTypes() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Massive/Massive/GetProcessTypes',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GetLoadTypes(processType) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Massive/Massive/GetLoadTypes',
            data: JSON.stringify({ processTypeId: processType }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static UploadFile(fileName) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Massive/Massive/UploadFile',
            data: JSON.stringify({ fileName: fileName }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static CreateLoad(massiveViewModel) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Massive/Massive/CreateLoad',
            data: JSON.stringify({ massiveViewModel: massiveViewModel }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static TariffedLoad(massiveLoadId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Massive/Massive/TariffedLoad',
            data: JSON.stringify({ massiveLoadId: massiveLoadId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static ValidateIssuePolicies(massiveLoadId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Massive/Massive/ValidateIssuePolicies',
            data: JSON.stringify({ massiveLoadId: massiveLoadId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static IssuePolicy(massiveLoadId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Massive/Massive/IssuePolicy',
            data: JSON.stringify({ massiveLoadId: massiveLoadId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetMassiveLoadsByDescription(description) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Massive/Massive/GetMassiveLoadsByDescription',
            data: JSON.stringify({ description: description }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetMassiveLoadDetailsByMassiveLoad(massiveLoadId, processType, status) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Massive/Massive/GetMassiveLoadDetailsByMassiveLoad',
            data: JSON.stringify({ massiveLoadId: massiveLoadId, processType: processType, status: status }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GenerateFileToMassiveLoad(massiveLoadProccessId, ProcessType) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Massive/Massive/GenerateFileToMassiveLoad',
            data: JSON.stringify({ massiveLoadId: massiveLoadProccessId, processType: ProcessType }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GenerateReportToMassiveLoad(massiveLoadId, status) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Massive/Massive/GenerateReportToMassiveLoad',
            data: JSON.stringify({ massiveLoadId: massiveLoadId, status: status }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GenerateReportByMassiveLoadIdByStatus(massiveLoadId, status) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Massive/Massive/GenerateReportByMassiveLoadIdByStatus',
            data: JSON.stringify({ massiveLoadId: massiveLoadId, status: status }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static PrintMassiveLoad(massiveLoadId, rangeFrom, rangeTo, LoadTypeId, checkIssuedDetail) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Massive/Massive/PrintLoad',
            data: JSON.stringify({ massiveLoadId: massiveLoadId, rangeFrom: rangeFrom, rangeTo: rangeTo, processType: LoadTypeId, checkIssuedDetail: checkIssuedDetail }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static DeleteProcess(massiveLoad) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Massive/Massive/DeleteProcess',
            data: JSON.stringify({ massiveLoad: massiveLoad }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static ExcludeTemporalsByLoad(massiveLoad, temps) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Massive/Massive/ExcludeTemporalsByLoad',
            data: JSON.stringify({ massiveLoadId: massiveLoad, temps: temps }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static ThirdParty(massiveLoadId, LoadTypeId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Massive/Massive/FindExternalServices',
            data: JSON.stringify({ massiveLoadId: massiveLoadId, processType: LoadTypeId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GetPrintingByLoadId(massiveLoadId, statusId, massiveLoadType, RiskSince, RiskUntil, collectFormat, coutas) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Massive/Massive/GetPrintingByLoadId',
            data: JSON.stringify({ massiveLoadId: massiveLoadId, massiveLoadStatus: statusId, massiveLoadType: massiveLoadType, RiskSince: RiskSince, RiskUntil: RiskUntil, collectFormat: collectFormat, idCuotes: coutas }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GetPrefixesToMassive() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Massive/Massive/GetPrefixesToMassive',
            data: JSON.stringify(),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GetCoveredRiskTypeByLineBusinessId(lbusinessId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Massive/Massive/GetCoveredRiskTypeByLineBusinessId',
            data: JSON.stringify({ linebusinessId: lbusinessId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static MassiveLoadHasThirdParty(massiveLoadId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Massive/Massive/MassiveLoadHasThirdParty',
            data: JSON.stringify({ massiveLoadId: massiveLoadId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}

class MassiveIssuance extends Uif2.Page {
    getInitialState() {
        loadValidate = false;
        MassiveIssuance.HideControls();
        MassiveIssuance.ShowButtons();
        $('#inputFile').UifFileInput();
        $('.progress').hide();
        $("#inputLoadId").ValidatorKey(ValidatorType.Number, 2, 0);
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        //$('#tableResults').HideColums({ control: '#tableResults', colums: [0] });
        $('#btnAddLoad').prop('disabled', true);

        MassiveRequest.GetProcessTypes().done(function (data) {
            if (data.success) {
                $('#selectProcessType').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });

        MassiveRequest.GetPrefixesToMassive().done(function (data) {
            if (data.success) {
                $('#selectPrefix').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });

        steps = uif2.steps({
            container: '#wizard',
            onStep: MassiveIssuance.stepFunction
        });

        if (GetQueryParameter("loadId") !== undefined) {
            $('#inputLoadId').val(GetQueryParameter("loadId"));
            MassiveIssuance.SearchLoad();
        }
    }

    bindEvents() {
        $('#inputLoadId').on('buttonClick', MassiveIssuance.SearchLoad);
        $('#inputLoadId').on('keydown', this.SearchLoadEnter);
        $('#selectProcessType').on('itemSelected', MassiveIssuance.SearchLoadTypes);
        $('#inputBillingGroup').on('buttonClick', this.SearchBillingGroup);
        $('#inputRequestGroup').on('buttonClick', this.SearchRequestGroup);
        $('#inputAgent').on('buttonClick', this.SearchAgent);
        $('#selectBranch').on('itemSelected', this.SearchSalePoint);
        $('#selectPrefix').on('itemSelected', this.SearchFileType);
        $('#tableResults tbody').on('click', 'tr', this.SelectSearch);
        $('#inputFile').on('change', this.AddFile);
        $('#inputRangeFrom').on('change', this.ChangeRangeFrom);
        $('#inputFile').fileupload({});
        $('#btnAddLoad').click(this.AddLoad);
        $('#btnExit').click(this.Exit);
        $('#btnNewLoad').click(this.NewLoad);
        $('#btnExclude').click(this.Exclude);
        $('#btnTariff').click(this.Tariff);
        $("#btnIssue").click(this.IssuePolicy);
        $('#btnErrors').click(this.GenerateErrorsFile);
        $('#btnPrint').click(this.Print);
        $('#btnPrintAccept').click(this.PrintAccept);
        $('#btnReport').click(this.Report);
        $('#btnDeleteLoad').click(this.DeleteProcess);
        $('#btnExcludeConfirm').click(this.ExcludeAccept);
        $('#wizard').click(this.stepFunction);
        $('#wizard').on('itemSelected', MassiveIssuance.SelectedWizard);
        $('#btnThirdParty').click(this.ThirdParty);
        $("#inputRecordsExclude").on('keypress', MassiveIssuance.checkKey);

    }

    static VisualizeControls(index) {
        if (index = 0) {
            $("#inputRecordsTotal").parent().show();
            $('#inputRecordsTotal').val(loadProcess[1].TotalRows);
            $("#inputRecordsProcessed").parent().show();
            $('#inputRecordsProcessed').val(loadProcess[1].Processeds);
            $("#inputRecordsPendings").parent().show();
            $('#inputRecordsPendings').val(loadProcess[1].Pendings);
            $("#inputStateDescription").parent().show();
            $("#inputRecordsTariffed").parent().hide();
            $("#inputRecordsForRate").parent().hide();
            $("#inputRecordsIssued").parent().hide();
            $("#inputRecordsForIssue").parent().hide();
            $("#inputRecordsEvents").parent().hide();
            $("#inputRecordsErrors").parent().show();
            $('#inputRecordsErrors').val(loadProcess[1].WithErrors);
        }
        else if (index = 1) {

            $("#inputRecordsTotal").parent().show();
            $('#inputRecordsTotal').val(loadProcess[2].TotalRows);
            $("#inputRecordsProcessed").parent().show();
            $('#inputRecordsProcessed').val(loadProcess[2].Processeds);
            $("#inputRecordsPendings").parent().show();
            $('#inputRecordsPendings').val(loadProcess[2].Pendings);
            $("#inputStateDescription").parent().show();

            $('#inputStateDescription').val(0);
            $("#inputRecordsTariffed").parent().hide();
            $("#inputRecordsForRate").parent().hide();
            $("#inputRecordsIssued").parent().hide();
            $("#inputRecordsForIssue").parent().hide();
            $("#inputRecordsEvents").parent().hide();
            $("#inputRecordsErrors").parent().hide();
        }
        else if (index = 2) {

            $("#inputRecordsTotal").parent().show();
            $('#inputRecordsTotal').val(loadProcess[3].TotalRows);

            $("#inputRecordsTariffed").parent().show();
            riskToPrint = loadProcess[3].Tariffed;
            $('#inputRecordsTariffed').val(loadProcess[3].Tariffed);

            $("#inputRecordsForRate").parent().show();
            $('#inputRecordsForRate').val(loadProcess[3].Pendings);

            $("#inputStateDescription").parent().show();
            $("#inputRecordsProcessed").parent().hide();
            $("#inputRecordsPendings").parent().hide();
            $("#inputRecordsIssued").parent().hide();
            $("#inputRecordsForIssue").parent().hide();
            $("#inputRecordsEvents").parent().show();
            $('#inputRecordsEvents').val(loadProcess[3].WithEvents);

            $("#inputRecordsErrors").parent().show();
            $('#inputRecordsErrors').val(loadProcess[3].WithErrors);
        }
        else if (index = 3) {

            $("#inputRecordsTotal").parent().show();
            $('#inputRecordsTotal').val(loadProcess[4].TotalRows);

            $("#inputRecordsIssued").parent().show();
            riskToPrint = loadProcess[4].Issued;
            $('#inputRecordsIssued').val(loadProcess[4].Issued);

            $("#inputRecordsForIssue").parent().show();
            $('#inputRecordsForIssue').val(loadProcess[4].Pendings);

            $("#inputRecordsProcessed").parent().hide();
            $("#inputRecordsPendings").parent().hide();
            $("#inputStateDescription").parent().show();
            $("#inputRecordsTariffed").parent().hide();
            $("#inputRecordsForRate").parent().hide();
            $("#inputRecordsEvents").parent().show();
            $('#inputRecordsEvents').val(loadProcess[4].WithEvents);

            $("#inputRecordsErrors").parent().show();
            $('#inputRecordsErrors').val(loadProcess[4].WithErrors);
        }
    }

    static stepFunction(event, currentIndex, newIndex) {
        //Valido el primer formulario.
        if (newIndex == 0) {
            $("#stepsGeneral").show();
            $("#inputRecordsTotal").parent().show();
            $('#inputRecordsTotal').val(((loadProcess[1] != undefined) ? loadProcess[1].TotalRows : 0));
            $("#inputRecordsProcessed").parent().show();
            $('#inputRecordsProcessed').val(((loadProcess[1] != undefined) ? loadProcess[1].Processeds : 0));
            $("#inputRecordsPendings").parent().show();
            $('#inputRecordsPendings').val(((loadProcess[1] != undefined) ? loadProcess[1].Pendings : 0));
            $("#inputStateDescription").parent().show();
            $("#inputRecordsTariffed").parent().hide();
            $("#inputRecordsForRate").parent().hide();
            $("#inputRecordsIssued").parent().hide();
            $("#inputRecordsForIssue").parent().hide();
            $("#inputRecordsEvents").parent().hide();
            $("#inputRecordsErrors").parent().show();
            $('#inputRecordsErrors').val(((loadProcess[1] != undefined) ? loadProcess[1].WithErrors : 0));
        }
        if (newIndex == 1) {
            $("#stepsGeneral").show();
            $("#inputRecordsTotal").parent().show();
            $('#inputRecordsTotal').val(loadProcess[2].TotalRows);

            $("#inputRecordsProcessed").parent().show();
            $('#inputRecordsProcessed').val(loadProcess[2].Processeds);

            $("#inputRecordsPendings").parent().show();
            $('#inputRecordsPendings').val(loadProcess[2].Pendings);

            $("#inputStateDescription").parent().show();

            $('#inputStateDescription').val(0);
            $("#inputRecordsTariffed").parent().hide();
            $("#inputRecordsForRate").parent().hide();
            $("#inputRecordsIssued").parent().hide();
            $("#inputRecordsForIssue").parent().hide();
            $("#inputRecordsEvents").parent().hide();
            $("#inputRecordsErrors").parent().hide();
        }
        if (newIndex == 2) {
            $("#stepsGeneral").show();
            $("#inputRecordsTotal").parent().show();
            $('#inputRecordsTotal').val(loadProcess[3].TotalRows);

            $("#inputRecordsTariffed").parent().show();
            $('#inputRecordsTariffed').val(loadProcess[3].Tariffed);
            riskToPrint = loadProcess[3].Tariffed;

            $("#inputRecordsForRate").parent().show();
            $('#inputRecordsForRate').val(loadProcess[3].Pendings);

            $("#inputStateDescription").parent().show();
            $("#inputRecordsProcessed").parent().hide();
            $("#inputRecordsPendings").parent().hide();
            $("#inputRecordsIssued").parent().hide();
            $("#inputRecordsForIssue").parent().hide();
            $("#inputRecordsEvents").parent().show();
            $('#inputRecordsEvents').val(loadProcess[3].WithEvents);

            $("#inputRecordsErrors").parent().show();
            $('#inputRecordsErrors').val(loadProcess[3].WithErrors);

        }
        if (newIndex == 3) {
            $("#stepsGeneral").show();
            $("#inputRecordsTotal").parent().show();
            $('#inputRecordsTotal').val(loadProcess[4].TotalRows);

            $("#inputRecordsIssued").parent().show();
            riskToPrint = loadProcess[4].Issued;
            $('#inputRecordsIssued').val(loadProcess[4].Issued);

            $("#inputRecordsForIssue").parent().show();
            $('#inputRecordsForIssue').val(loadProcess[4].Pendings);

            $("#inputRecordsProcessed").parent().hide();
            $("#inputRecordsPendings").parent().hide();
            $("#inputStateDescription").parent().show();
            $("#inputRecordsTariffed").parent().hide();
            $("#inputRecordsForRate").parent().hide();
            $("#inputRecordsEvents").parent().show();
            $('#inputRecordsEvents').val(loadProcess[4].WithEvents);

            $("#inputRecordsErrors").parent().show();
            $('#inputRecordsErrors').val(loadProcess[4].WithErrors);


        }
        //Para los demás pasos, la función devuelve true siempre, en este ejemplo.
        return true;
    };

    static hideWizardStepRecords() {
        $("#wizard").hide();
        $("#stepsGeneral").hide();
    }

    static showStepRecords() {
        loadValidate = true;
        //$("#wizard").show();
        //$("#stepsGeneral").show();
        $("#inputRecordsTotal").parent().show();
        $("#inputRecordsProcessed").parent().show();
        $("#inputRecordsPendings").parent().show();
        $("#inputStateDescription").parent().show();
        $("#inputRecordsTariffed").parent().hide();
        $("#inputRecordsForRate").parent().hide();
        $("#inputRecordsIssued").parent().hide();
        $("#inputRecordsForIssue").parent().hide();
        $("#inputRecordsEvents").parent().hide();
        $("#inputRecordsErrors").parent().show();
    }

    static checkKey(event) {
        return /\d|,/.test(event.key);
    }

    static RenewdWizard() {
        $(".title-0").trigger("click");
    }

    static finishFunction(event, currentIndex) {

        $("#alert").UifAlert('show', 'Los datos han sido guardado correctamente', 'info');

        return true;
    }

    static SearchLoadTypes() {
        if (parseInt($('#selectProcessType').val()) > 0) {
            var processType = $("#selectProcessType").UifSelect("getSelected");

            if (MassiveProcessType.Cancellation == processType) {
                MassiveIssuance.HideControlByProcessType(parseInt(processType));
            }
            else {
                MassiveIssuance.ShowControlByProcessType(parseInt(processType));
                MassiveIssuance.ClearForm();

                $("#selectProcessType").UifSelect("setSelected", processType);
                MassiveRequest.GetLoadTypes($('#selectProcessType').val()).done(function (data) {
                    if (data.success) {
                        $('#selectLoadType').UifSelect({ sourceData: data.result });
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                });
            }
        }
    }

    static HideControlByProcessType(processType) {
        switch (processType) {
            case MassiveProcessType.Cancellation:
                $("#divMassive").hide();
                $("#divLoadType").hide();
                $("#divPrefixCommercial").hide();
                break;
        }
    }
    static ShowControlByProcessType(processType) {
        switch (processType) {
            case MassiveProcessType.Emission:
            case MassiveProcessType.Renewal:
            case MassiveProcessType.Modification:
                $("#divMassive").show();
                $("#divLoadType").show();
                $("#divPrefixCommercial").show();
                break;
        }

    }
    static ShowButtons(status) {
        $('#inputLoadId').prop('disabled', '');
        switch (status) {
            case MassiveLoadStatus.Validated:
                if ($('#inputRecordsPendings').val() == 0) {

                    var requireThirdParty = false;
                    MassiveRequest.MassiveLoadHasThirdParty(loadProcess[0].Id).done(function (data) {
                        if (data.success) {
                            requireThirdParty = data.result;
                            if (requireThirdParty) {
                                $('#btnThirdParty').show();
                                $('#btnThirdParty').prop('disabled', false);
                            }
                            else {
                                //Se debe mostrar el boton cuando se habilite el servicio de external Proxy
                                $('#btnTariff').show();
                            }
                        }
                        else {
                            $('#btnTariff').show();
                        }
                    });
                }
                $('#step1').show();
                $('#btnExclude').hide();
                $('#btnErrors').show();
                $('#btnIssue').hide();
                $("#btnNewLoad").removeClass("btn-primary");
                $("#btnNewLoad").addClass("btn-default");
                $('#btnDeleteLoad').show();
                break;
            case MassiveLoadStatus.Queried:
                $('#step2').show();
                $('#btnExclude').hide();
                $('#btnThirdParty').hide();
                $('#btnTariff').hide();
                $('#btnErrors').show();
                $('#btnIssue').hide();
                $('#btnDeleteLoad').hide();
                break;
            case MassiveLoadStatus.Tariffed:
                if ($('#inputRecordsForRate').val() == 0) {
                    $('#btnIssue').show();
                    $('#btnPrint').prop('disabled', false);
                    $('#btnPrint').show();
                    $('#btnReport').show();
                }
                $('#step3').show();
                $('#btnErrors').show();
                $('#btnTariff').hide();
                $('#btnExclude').show();
                $('#btnThirdParty').hide();
                $('#btnTariff').hide();
                $("#btnNewLoad").removeClass("btn-primary");
                $("#btnNewLoad").addClass("btn-default");
                $('#btnDeleteLoad').show();
                break;
            case MassiveLoadStatus.Issued:
                $('#step4').show();
                $('#btnPrint').prop('disabled', false);
                $('#btnPrint').show();
                $('#btnErrors').show();
                $('#btnReport').show();
                $("#btnNewLoad").removeClass("btn-default");
                $("#btnNewLoad").addClass("btn-primary");
                if (parseInt($("#inputRecordsForIssue").val()) > 0) {
                    $('#btnExclude').hide();
                    $('#btnThirdParty').hide();
                    $('#btnTariff').hide();
                    $('#btnIssue').show();
                } else {
                    $('#btnExclude').hide();
                    $('#btnThirdParty').hide();
                    $('#btnTariff').hide();
                    $('#btnIssue').hide();
                }
                $('#btnDeleteLoad').hide();
                break;
            default:
                $('#btnPrint').hide();
                $('#btnErrors').show();
                $('#btnExclude').hide();
                $('#btnThirdParty').hide();
                $('#btnTariff').hide();
                $('#btnIssue').hide();
                $('#btnDeleteLoad').hide();
                $("#btnNewLoad").removeClass("btn-default");
                $("#btnNewLoad").addClass("btn-primary");
                break;
        }
    }

    static showStepRecordsByState() {

        for (var i = 1; i < loadProcess.length; i++) {
            loadProcess[i].Status
            switch (loadProcess[i].Status) {
                case MassiveLoadStatus.Validated:
                    $("#stepsGeneral").show();
                    $("#inputRecordsTotal").parent().show();
                    $('#inputRecordsTotal').val(loadProcess[0].TotalRows);
                    $("#inputRecordsProcessed").parent().show();
                    $('#inputRecordsProcessed').val(loadProcess[1].Processeds);
                    $("#inputRecordsPendings").parent().show();
                    $('#inputRecordsPendings').val(loadProcess[1].Pendings);
                    $("#inputStateDescription").parent().show();
                    $("#inputRecordsTariffed").parent().hide();
                    $("#inputRecordsForRate").parent().hide();
                    $("#inputRecordsIssued").parent().hide();
                    $("#inputRecordsForIssue").parent().hide();
                    $("#inputRecordsEvents").parent().hide();
                    $("#inputRecordsErrors").parent().show();
                    $('#inputRecordsErrors').val(loadProcess[1].WithErrors);
                    break;
                case MassiveLoadStatus.Queried:

                    $("#stepsGeneral").show();
                    $("#inputRecordsTotal").parent().show();
                    $('#inputRecordsTotal').val(loadProcess[2].TotalRows);
                    $("#inputRecordsProcessed").parent().show();
                    $('#inputRecordsProcessed').val(loadProcess[2].Processeds);
                    $("#inputRecordsPendings").parent().show();
                    $('#inputRecordsPendings').val(loadProcess[2].Pendings);
                    $("#inputStateDescription").parent().show();
                    $("#inputRecordsTariffed").parent().hide();
                    $("#inputRecordsForRate").parent().hide();
                    $("#inputRecordsIssued").parent().hide();
                    $("#inputRecordsForIssue").parent().hide();
                    $("#inputRecordsEvents").parent().hide();
                    $("#inputRecordsErrors").parent().show();
                    $('#inputRecordsErrors').val(loadProcess[2].WithErrors);

                    break;
                case MassiveLoadStatus.Tariffed:
                    $("#stepsGeneral").show();
                    $("#inputRecordsTotal").parent().show();
                    $('#inputRecordsTotal').val(loadProcess[3].TotalRows);
                    $("#inputRecordsTariffed").parent().show();
                    $('#inputRecordsTariffed').val(loadProcess[3].Tariffed);
                    $("#inputRecordsForRate").parent().show();
                    $('#inputRecordsForRate').val(loadProcess[3].Pendings);
                    $("#inputStateDescription").parent().show();
                    $("#inputRecordsProcessed").parent().hide();
                    $("#inputRecordsPendings").parent().hide();
                    $("#inputRecordsIssued").parent().hide();
                    $("#inputRecordsForIssue").parent().hide();
                    $("#inputRecordsEvents").parent().show();
                    $('#inputRecordsEvents').val(loadProcess[3].WithEvents);
                    $("#inputRecordsErrors").parent().show();
                    $('#inputRecordsErrors').val(loadProcess[3].WithErrors);

                    break;
                case MassiveLoadStatus.Issued:
                    $("#stepsGeneral").show();
                    $("#inputRecordsTotal").parent().show();
                    $('#inputRecordsTotal').val(loadProcess[4].TotalRows);
                    $("#inputRecordsIssued").parent().show();
                    $('#inputRecordsIssued').val(loadProcess[4].Issued);
                    $("#inputRecordsForIssue").parent().show();
                    $('#inputRecordsForIssue').val(loadProcess[4].Pendings);
                    $("#inputRecordsProcessed").parent().hide();
                    $("#inputRecordsPendings").parent().hide();
                    $("#inputStateDescription").parent().show();
                    $("#inputRecordsTariffed").parent().hide();
                    $("#inputRecordsForRate").parent().hide();
                    $("#inputRecordsEvents").parent().show();
                    $('#inputRecordsEvents').val(loadProcess[4].WithEvents);
                    $("#inputRecordsErrors").parent().show();
                    $('#inputRecordsErrors').val(loadProcess[4].WithErrors);
                    break;
                default:
                    $("#stepsGeneral").show();
                    $("#inputRecordsTotal").parent().show();
                    $('#inputRecordsTotal').val(0);
                    $("#inputRecordsProcessed").parent().show();
                    $('#inputRecordsProcessed').val(0);
                    $("#inputRecordsPendings").parent().show();
                    $('#inputRecordsPendings').val(0);
                    $("#inputStateDescription").parent().show();
                    $('#inputStateDescription').val(0);
                    $("#inputRecordsTariffed").parent().hide();
                    $("#inputRecordsForRate").parent().hide();
                    $("#inputRecordsIssued").parent().hide();
                    $("#inputRecordsForIssue").parent().hide();
                    $("#inputRecordsEvents").parent().hide();
                    $("#inputRecordsErrors").parent().hide();
                    break;
            }
        }
    }

    static ShowDefaultResults(dataTable) {
        $('#tableResults').UifDataTable('clear');
        $('#tableResults').UifDataTable('addRow', dataTable);
    }

    static HideControls() {
        $('#btnPrint').hide();
        $('#btnErrors').hide();
        $('#btnReport').hide();
        $('#btnExclude').hide();
        $('#btnThirdParty').hide();
        if (!loadValidate) {
            MassiveIssuance.hideWizardStepRecords()
        }

        $('#btnTariff').hide();
        $('#btnIssue').hide();
        $('#btnDeleteLoad').hide();
        $('#divErrors').hide();
        $('#divTemporals').hide();
        $('#divPolicies').hide();
        $('#divCollectiveTemporals').hide();
        $('#divCollectivePolicies').hide();
        $('#divCollectiveTemporalsIssued').hide();
        $('.progress').hide();
    }

    static DisableControls() {
        $('#selectFileType').prop('disabled', 'disabled');
        $('#inputRequestGroup').prop('disabled', 'disabled');
        $('#inputAgent').prop('disabled', 'disabled');
        $("#selectLoadType").prop('disabled', 'disabled');
        $('#selectAgency').prop('disabled', 'disabled');
        $('#selectPrefix').prop('disabled', 'disabled');
        $('#selectProduct').prop('disabled', 'disabled');
        $('#selectBranch').prop('disabled', 'disabled');
        $('#selectSalePoint').prop('disabled', 'disabled');

        $('.progress .progress-bar').css('width', 0 + '%');
    }

    static ClearForm() {
        $('#formMassive').formReset();
        $('#inputLoadId').data('Object', null);
        $('#inputLoadId').val('');
        $('#selectProcessType').UifSelect('setSelected', null);
        $('#selectLoadType').UifSelect();
        $('#selectFileType').UifSelect('setSelected', null);
        $('#selectPrefix').UifSelect('setSelected', null);
        $('#selectPrefix').prop('disabled', false);
        $("#selectProcessType").prop('disabled', '');
        $('#inputLoadName').val('');
        $('#inputLoadName').prop('disabled', '');
        $('#inputFile').prop('disabled', false);
        $('.progress .progress-bar').css('width', 0 + '%');
        $('.progress').hide();
        $('#btnAddLoad').prop('disabled', true);
        loadValidate = false;
        MassiveIssuance.HideControls();
        MassiveIssuance.ShowButtons();
        $("#btnNew").removeClass("btn-default");
        $("#btnNew").addClass("btn-primary");
    }

    static SearchLoad() {
        $('.progress').hide();
        MassiveIssuance.RenewdWizard();
        searchType = 4;
        var description = $('#inputLoadId').val().trim();
        $('#inputLoadId').prop('disabled', 'disabled');
        var number = parseInt(description, 10);
        var loads = [];
        var pr = false;
        if ((!isNaN(number) && number != 0) || description.length > 2) {
            pr = false;
            MassiveRequest.GetMassiveLoadsByDescription(description).done(function (data) {
                if (data.success) {
                    pr = true;
                    if (data.result.length == 1 || pr) {
                        loadProcess = data.result;
                        statusLoad = data.result[0].Status;
                        MassiveIssuance.LoadMassiveLoad(data.result);
                    }
                    else {
                        for (var i = 0; i < data.result.length; i++) {
                            loads.push({
                                Id: data.result[i].Id,
                                Code: data.result[i].Id,
                                Description: data.result[i].Description
                            });
                        }
                        MassiveIssuance.ShowDefaultResults(loads);
                        $('#modalDefaultSearch').UifModal('showLocal', 'Seleccione un Cargue');
                    }
                }
                else {
                    var errorMessage = "";
                    if (data.result[0] == 'N') {
                        errorMessage = data.result;
                    }
                    else {
                        errorMessage = data.result[0].ErrorDescription;
                    }

                    if (data.result[0] != null && errorMessage != null) {
                        $('#inputLoadId').data('Object', data.result[0]);
                        $('#btnDeleteLoad').show();
                    }
                    
                    $('#inputLoadId').prop('disabled', false);
                    $('#inputRecordsTotal').val(0);
                    $('#inputRecordsProcessed').val(0);
                    $('#inputRecordsPendings').val(0);
                    $('#inputRecordsErrors').val(0);
                    
                    $.UifNotify('show', { 'type': 'info', 'message': errorMessage, 'autoclose': true });
                }
            });
            pr = false;
        }
    }

    SearchLoadEnter() {
        if (event.which == 13) {
            MassiveIssuance.SearchLoad();
        }
    }

    static CleanSteps() {
        $(".title-0").html('<div class="number">1</div>' + Resources.Language.LabelChargue + ' <div class="line"></div > ');
        $(".title-1").html('<div class="number">2</div>' + Resources.Language.ButtonThirdParty + ' <div class="line"></div > ');
        $(".title-2").html('<div class="number">3</div>' + Resources.Language.ButtonTariff + ' <div class="line"></div > ');
        $(".title-3").html('<div class="number">4</div>' + Resources.Language.ButtonEmit + ' <div class="line"></div > ');
    }

    static SelectStepOfWizardByStatusId(Status) {
        var step = 0;
        switch (Status) {
            case (1):
                step = 0;
                break;
            case (2):
                step = 0;
                break;
            case (3):
                step = 2;
                break;
            case (4):
                step = 2;
                break;
            case (5):
                step = 3;
                break;
            case (6):
                //if (loadProcess[2].TotalRows > 0)
                //{
                //    step = 3;
                //}
                //else
                //{
                //    step = 2;
                //}
                step = 3;
                break;
            case (7):
                step = 1;
                break;
            case (8):
                step = 1;
                break;

        }

        $(".title-0").trigger("click");
        for (var i = 0; i <= step; i++) {
            $(".title-" + i + "").trigger("click").selector
        }

    }

    static LoadStepsByStatus(massiveLoad) {
        $('#btnTariff').prop('disabled', '');
        $('#btnIssue').prop('disabled', '');
        $('#btnExclude').prop('disabled', '');
        MassiveIssuance.CleanSteps();
        $("#wizard").show();
        $("#stepsGeneral").show();

        //MassiveIssuance.SelectStepOfWizardByStatusId(massiveLoad[0].Status);
        switch (massiveLoad[0].Status) {
            case MassiveLoadStatus.Validating:
            case MassiveLoadStatus.Validated:
                $(".title-0").html('<div class="number">1</div>' + Resources.Language.LabelChargue + '</br>' + '(' + massiveLoad[0].StatusDescription + ')' + ' <div class="line"></div > ').show();
                if (massiveLoad[2].TotalRows > 0) {
                    $(".title-1").html('<div class="number">2</div>' + Resources.Language.ButtonThirdParty + '</br>' + '(' + massiveLoad[2].StatusDescription + ')' + ' <div class="line"></div > ').hide();
                    $(".title-2").html('<div class="number">3</div>' + Resources.Language.ButtonTariff + '</br>' + '(' + massiveLoad[0].StatusDescription + ')' + ' <div class="line"></div > ').hide();
                    $(".title-3").html('<div class="number">4</div>' + Resources.Language.ButtonEmit + ' <div class="line"></div > ').hide();
                }
                else {
                    $(".title-1").html('<div class="number">2</div>' + Resources.Language.ButtonThirdParty + '</br>' + '(' + massiveLoad[2].StatusDescription + ')' + ' <div class="line"></div > ').hide();
                    $(".title-2").html('<div class="number">2</div>' + Resources.Language.ButtonTariff + '</br>' + '(' + massiveLoad[0].StatusDescription + ')' + ' <div class="line"></div > ').hide();
                    $(".title-3").html('<div class="number">3</div>' + Resources.Language.ButtonEmit + ' <div class="line"></div > ').hide();
                }
                //$(".title-2").html('<div class="number">3</div>' + Resources.Language.ButtonTariff + ' <div class="line"></div > ').hide();
                //$(".title-3").html('<div class="number">4</div>' + Resources.Language.ButtonEmit + ' <div class="line"></div > ').hide();
                $('#inputRecordsTotal').val(massiveLoad[1].TotalRows);
                $('#inputRecordsProcessed').val(massiveLoad[1].Processeds);
                $('#inputRecordsPendings').val(massiveLoad[1].Pendings);
                $('#inputRecordsEvents').val(massiveLoad[1].WithEvents);
                $('#inputRecordsErrors').val(massiveLoad[1].WithErrors);
                if (loadProcess[1].TotalRows == loadProcess[1].WithErrors) {
                    $('#btnTariff').prop('disabled', 'disabled');
                    $('#btnExclude').prop('disabled', 'disabled');
                }
                break;

            case MassiveLoadStatus.Querying:
                $(".title-0").html('<div class="number">1</div>' + Resources.Language.LabelChargue + '</br>' + '(' + massiveLoad[1].StatusDescription + ')' + ' <div class="line"></div > ');
                if (massiveLoad[2].TotalRows > 0) {
                    $(".title-1").html('<div class="number">2</div>' + Resources.Language.ButtonThirdParty + '</br>' + '(' + massiveLoad[0].StatusDescription + ')' + ' <div class="line"></div > ').show();
                    $(".title-2").html('<div class="number">3</div>' + Resources.Language.ButtonTariff + '</br>' + '(' + massiveLoad[3].StatusDescription + ')' + ' <div class="line"></div > ').hide();
                    $(".title-3").html('<div class="number">4</div>' + Resources.Language.ButtonEmit + ' <div class="line"></div > ').hide();
                }
                else {
                    //$(".title-1").html('<div class="number">2</div>' + Resources.Language.ButtonThirdParty + '</br>' + '(' + massiveLoad[2].StatusDescription + ')' + ' <div class="line"></div > ').hide();
                    $(".title-1").html('<div class="number">2</div>' + Resources.Language.ButtonTariff + '</br>' + '(' + massiveLoad[0].StatusDescription + ')' + ' <div class="line"></div > ').hide();
                    $(".title-2").html('<div class="number">3</div>' + Resources.Language.ButtonEmit + ' <div class="line"></div > ').hide();
                }
                $('#inputRecordsTotal').val(massiveLoad[2].TotalRows);
                $('#inputRecordsProcessed').val(massiveLoad[2].Processeds);
                $('#inputRecordsPendings').val(massiveLoad[2].Pendings);
                $('#inputRecordsEvents').val(massiveLoad[2].WithEvents);
                $('#inputRecordsErrors').val(massiveLoad[2].WithErrors);
                break;

                $('#inputRecordsTotal').val(massiveLoad[1].TotalRows);
                $('#inputRecordsProcessed').val(massiveLoad[1].Processeds);
                $('#inputRecordsPendings').val(massiveLoad[1].Pendings);
                $('#inputRecordsEvents').val(massiveLoad[1].WithEvents);
                $('#inputRecordsErrors').val(massiveLoad[1].WithErrors);
                break;
            case MassiveLoadStatus.Queried:
                $(".title-0").html('<div class="number">1</div>' + Resources.Language.LabelChargue + '</br>' + '(' + massiveLoad[1].StatusDescription + ')' + ' <div class="line"></div > ');
                if (massiveLoad[2].TotalRows > 0) {
                    $(".title-1").html('<div class="number">2</div>' + Resources.Language.ButtonThirdParty + '</br>' + '(' + massiveLoad[2].StatusDescription + ')' + ' <div class="line"></div > ').show();
                    $(".title-2").html('<div class="number">3</div>' + Resources.Language.ButtonTariff + '</br>' + '(' + massiveLoad[0].StatusDescription + ')' + ' <div class="line"></div > ').hide();
                    $(".title-3").html('<div class="number">4</div>' + Resources.Language.ButtonEmit + ' <div class="line"></div > ').hide();
                }
                else {
                    //$(".title-1").html('<div class="number">2</div>' + Resources.Language.ButtonThirdParty + '</br>' + '(' + massiveLoad[2].StatusDescription + ')' + ' <div class="line"></div > ').hide();
                    $(".title-1").html('<div class="number">2</div>' + Resources.Language.ButtonTariff + '</br>' + '(' + massiveLoad[0].StatusDescription + ')' + ' <div class="line"></div > ').hide();
                    $(".title-2").html('<div class="number">3</div>' + Resources.Language.ButtonEmit + ' <div class="line"></div > ').hide();
                }
                $('#inputRecordsTotal').val(massiveLoad[2].TotalRows);
                $('#inputRecordsProcessed').val(massiveLoad[2].Processeds);
                $('#inputRecordsPendings').val(massiveLoad[2].Pendings);
                $('#inputRecordsEvents').val(massiveLoad[2].WithEvents);
                $('#inputRecordsErrors').val(massiveLoad[2].WithErrors);
                break;

                $('#inputRecordsTotal').val(massiveLoad[1].TotalRows);
                $('#inputRecordsProcessed').val(massiveLoad[1].Processeds);
                $('#inputRecordsPendings').val(massiveLoad[1].Pendings);
                $('#inputRecordsEvents').val(massiveLoad[1].WithEvents);
                $('#inputRecordsErrors').val(massiveLoad[1].WithErrors);
                break;
            case MassiveLoadStatus.Tariffing:

                $(".title-0").html('<div class="number">1</div>' + Resources.Language.LabelChargue + '</br>' + '(' + massiveLoad[1].StatusDescription + ')' + ' <div class="line"></div > ');
                if (massiveLoad[2].TotalRows > 0) {
                    $(".title-1").html('<div class="number">2</div>' + Resources.Language.ButtonThirdParty + '</br>' + '(' + massiveLoad[2].StatusDescription + ')' + ' <div class="line"></div > ').show();
                    $(".title-2").html('<div class="number">3</div>' + Resources.Language.ButtonTariff + '</br>' + '(' + massiveLoad[0].StatusDescription + ')' + ' <div class="line"></div > ').show();
                    $(".title-3").html('<div class="number">4</div>' + Resources.Language.ButtonEmit + ' <div class="line"></div > ').hide();
                }
                else {
                    $(".title-1").html('<div class="number">2</div>' + Resources.Language.ButtonThirdParty + '</br>' + '(' + massiveLoad[2].StatusDescription + ')' + ' <div class="line"></div > ').hide();
                    $(".title-1").html('<div class="number">2</div>' + Resources.Language.ButtonTariff + '</br>' + '(' + massiveLoad[0].StatusDescription + ')' + ' <div class="line"></div > ').show();
                    $(".title-2").html('<div class="number">3</div>' + Resources.Language.ButtonEmit + ' <div class="line"></div > ').hide();
                }

                //$(".title-2").html('<div class="number">3</div>' + Resources.Language.ButtonTariff + '</br>' + '(' + massiveLoad[0].StatusDescription + ')' + ' <div class="line"></div > ').show();
                //$(".title-3").html('<div class="number">4</div>' + Resources.Language.ButtonEmit + ' <div class="line"></div > ').hide();
                $('#inputRecordsTotal').val(massiveLoad[3].TotalRows);
                $('#inputRecordsProcessed').val(massiveLoad[3].Tariffed);
                $('#inputRecordsPendings').val(massiveLoad[3].Pendings);
                $("#inputRecordsEvents").parent().show();
                $('#inputRecordsEvents').val(massiveLoad[3].WithEvents);
                $('#inputRecordsErrors').val(massiveLoad[3].WithErrors);
                break;
            case MassiveLoadStatus.Tariffed:

                $(".title-0").html('<div class="number">1</div>' + Resources.Language.LabelChargue + '</br>' + '(' + massiveLoad[1].StatusDescription + ')' + ' <div class="line"></div > ');
                if (massiveLoad[2].TotalRows > 0) {
                    $(".title-1").html('<div class="number">2</div>' + Resources.Language.ButtonThirdParty + '</br>' + '(' + massiveLoad[2].StatusDescription + ')' + ' <div class="line"></div > ').show();
                    $(".title-2").html('<div class="number">3</div>' + Resources.Language.ButtonTariff + '</br>' + '(' + massiveLoad[3].StatusDescription + ')' + ' <div class="line"></div > ').show();
                    $(".title-3").html('<div class="number">4</div>' + Resources.Language.ButtonEmit + ' <div class="line"></div > ').hide();
                }
                else {
                    $(".title-1").html('<div class="number">2</div>' + Resources.Language.ButtonThirdParty + '</br>' + '(' + massiveLoad[2].StatusDescription + ')' + ' <div class="line"></div > ').hide();
                    $(".title-2").html('<div class="number">2</div>' + Resources.Language.ButtonTariff + '</br>' + '(' + massiveLoad[3].StatusDescription + ')' + ' <div class="line"></div > ').show();
                    $(".title-3").html('<div class="number">3</div>' + Resources.Language.ButtonEmit + ' <div class="line"></div > ').hide();
                }

                $('#inputRecordsTotal').val(massiveLoad[3].TotalRows);
                $('#inputRecordsProcessed').val(massiveLoad[3].Tariffed);
                $('#inputRecordsPendings').val(massiveLoad[3].Pendings);
                $("#inputRecordsEvents").parent().show();
                $('#inputRecordsEvents').val(massiveLoad[3].WithEvents);
                $('#inputRecordsErrors').val(massiveLoad[3].WithErrors);
                if (massiveLoad[3].TotalRows == massiveLoad[3].WithErrors) {
                    $('#btnIssue').prop('disabled', 'disabled');
                    $('#btnExclude').prop('disabled', 'disabled');
                }
                break;
            case MassiveLoadStatus.Issuing:

                $(".title-0").html('<div class="number">1</div>' + Resources.Language.LabelChargue + '</br>' + '(' + massiveLoad[1].StatusDescription + ')' + ' <div class="line"></div > ').show();
                if (massiveLoad[2].TotalRows > 0) {
                    $(".title-1").html('<div class="number">2</div>' + Resources.Language.ButtonThirdParty + '</br>' + '(' + massiveLoad[2].StatusDescription + ')' + ' <div class="line"></div > ').show();
                    $(".title-2").html('<div class="number">3</div>' + Resources.Language.ButtonTariff + '</br>' + '(' + massiveLoad[3].StatusDescription + ')' + ' <div class="line"></div > ').show();
                    $(".title-3").html('<div class="number">4</div>' + Resources.Language.ButtonEmit + '</br>' + '(' + massiveLoad[0].StatusDescription + ')' + ' <div class="line"></div > ').show();
                }
                else {
                    $(".title-1").html('<div class="number">2</div>' + Resources.Language.ButtonThirdParty + '</br>' + '(' + massiveLoad[2].StatusDescription + ')' + ' <div class="line"></div > ').hide();
                    $(".title-1").html('<div class="number">2</div>' + Resources.Language.ButtonTariff + '</br>' + '(' + massiveLoad[3].StatusDescription + ')' + ' <div class="line"></div > ').show();
                    $(".title-2").html('<div class="number">3</div>' + Resources.Language.ButtonEmit + '</br>' + '(' + massiveLoad[0].StatusDescription + ')' + ' <div class="line"></div > ').show();
                }

                $('#inputRecordsTotal').val(massiveLoad[4].TotalRows);
                $('#inputRecordsProcessed').val(massiveLoad[4].Processeds);
                $('#inputRecordsPendings').val(massiveLoad[4].Pendings);
                $('#inputRecordsEvents').val(massiveLoad[4].WithEvents);
                $('#inputRecordsErrors').val(massiveLoad[4].WithErrors);
                break;
            case MassiveLoadStatus.Issued:

                $(".title-0").html('<div class="number">1</div>' + Resources.Language.LabelChargue + '</br>' + '(' + massiveLoad[1].StatusDescription + ')' + ' <div class="line"></div > ').show();
                if (massiveLoad[2].TotalRows > 0) {
                    $(".title-1").html('<div class="number">2</div>' + Resources.Language.ButtonThirdParty + '</br>' + '(' + massiveLoad[2].StatusDescription + ')' + ' <div class="line"></div > ').show();
                    $(".title-2").html('<div class="number">3</div>' + Resources.Language.ButtonTariff + '</br>' + '(' + massiveLoad[3].StatusDescription + ')' + ' <div class="line"></div > ').show();
                    $(".title-3").html('<div class="number">4</div>' + Resources.Language.ButtonEmit + '</br>' + '(' + massiveLoad[4].StatusDescription + ')' + ' <div class="line"></div > ').show();
                }
                else {
                    $(".title-1").html('<div class="number">2</div>' + Resources.Language.ButtonThirdParty + '</br>' + '(' + massiveLoad[2].StatusDescription + ')' + ' <div class="line"></div > ').hide();
                    $(".title-2").html('<div class="number">2</div>' + Resources.Language.ButtonTariff + '</br>' + '(' + massiveLoad[3].StatusDescription + ')' + ' <div class="line"></div > ').show();
                    $(".title-3").html('<div class="number">3</div>' + Resources.Language.ButtonEmit + '</br>' + '(' + massiveLoad[4].StatusDescription + ')' + ' <div class="line"></div > ').show();
                }

                $('#inputRecordsTotal').val(massiveLoad[4].TotalRows);
                $('#inputRecordsProcessed').val(massiveLoad[4].Issued);
                $('#inputRecordsPendings').val(massiveLoad[4].Pendings);
                $('#inputRecordsEvents').val(massiveLoad[4].WithEvents);
                $('#inputRecordsErrors').val(massiveLoad[4].WithErrors);
                break;

        }
        MassiveIssuance.SelectStepOfWizardByStatusId(massiveLoad[0].Status);
    }

    static LoadMassiveLoad(massiveLoad) {
        MassiveIssuance.ClearForm();
        $('#inputLoadId').data('Object', massiveLoad[0]);
        $('#inputLoadId').val(massiveLoad[0].Id);
        $("#selectProcessType").UifSelect("setSelected", massiveLoad[0].LoadType.ProcessType);
        MassiveRequest.GetLoadTypes(massiveLoad[0].LoadType.ProcessType).done(function (data) {
            if (data.success) {
                if (massiveLoad[0] != null) {
                    $('#selectLoadType').UifSelect({ sourceData: data.result, selectedId: massiveLoad[0].LoadType.Id });
                }
                $("#selectLoadType").prop('disabled', 'disabled');
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });

        $('#inputLoadName').val(massiveLoad[0].Description);
        $('#inputStateDescription').data('Object', massiveLoad[0].Status);
        $('#inputStateDescription').val(massiveLoad[0].StatusDescription);
        MassiveIssuance.showStepRecords();
        MassiveIssuance.LoadStepsByStatus(massiveLoad);
        MassiveIssuance.DisableControls();
        $('#selectProcessType').prop('disabled', 'disabled');
        $('#inputLoadName').prop('disabled', 'disabled');
        $('#inputFile').prop('disabled', 'disabled');
        $('#selectFileType').prop('disabled', 'disabled');
        $('#btnAddLoad').prop('disabled', true);
        MassiveIssuance.ShowButtons(massiveLoad[0].Status);
        MassiveRequest.GetMassiveLoadDetailsByMassiveLoad(massiveLoad[0].Id, massiveLoad[0].LoadType.Id, statusLoad).done(function (data) {
            if (data.success) {
                if (data.result != null) {
                    massiveLoadGlobal = data.result;
                    if (massiveLoad[0].LoadType.Id == SubMassiveProcessType.Cancellation) {
                        if (massiveLoad[0].HasError) {
                            $.UifNotify('show', { 'type': 'info', 'message': massiveLoad[0].ErrorDescription, 'autoclose': true });
                        }
                        else {
                            $("#divMassive").hide();
                            massiveLoad.Rows = data.result;
                            if (statusLoad == MassiveLoadStatus.Tariffed) {
                               
                                MassiveIssuance.ShowButtons(MassiveLoadStatus.Tariffed);
                            }
                            else if (statusLoad == MassiveLoadStatus.Issued) {
                               
                                    MassiveIssuance.ShowButtons(MassiveLoadStatus.Issued);
                                
                            }
                        }
                    }
                    else {
                        $("#divMassive").show();
                        if (massiveLoad[0].LoadType.Id == SubMassiveProcessType.MassiveRenewal) {
  
                            MassiveIssuance.DisableControls();
                            if (statusLoad == 4) {
                                MassiveIssuance.ShowButtons(MassiveLoadStatus.Tariffed);
                            } else if (statusLoad == 6) {                              
                                
                                    MassiveIssuance.ShowButtons(MassiveLoadStatus.Issued);
                            }

                            if (massiveLoadGlobal.HasError == false) {
                                MassiveRequest.GetPrefixesToMassive().done(function (data) {
                                    if (data.success) {
                                        var dataPrefix = data.result;
                                        if (dataPrefix.length > 0) {
                                            $('#selectPrefix').UifSelect({ sourceData: data.result, selectedId: massiveLoadGlobal.Prefix.Id });
                                            $('#selectPrefix').prop('disabled', 'disabled');
                                        }
                                    }
                                });
                                MassiveRequest.GetCoveredRiskTypeByLineBusinessId(massiveLoadGlobal.Prefix.Id).done(function (data) {
                                    if (data.success) {

                                        if (data.result != null) {
                                            $('#selectFileType').UifSelect({ sourceData: data.result, selectedId: massiveLoadGlobal.CoveredRiskType });
                                            if (data.result.length > 0) {
                                                $('#selectFileType').prop('disabled', 'disabled');
                                            }
                                        }
                                    }
                                    else {
                                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                                    }
                                });
                            }
                        } else {

                            var massiveEmission = data.result;
                            massiveLoad = massiveEmission;
                            if (loadProcess[0].HasError && massiveEmission.HasError) {
                                if (massiveEmission.ErrorDescription == null) {
                                    massiveEmission.ErrorDescription = loadProcess[0].ErrorDescription;
                                }
                            }
                            if (massiveEmission.HasError == false) {
                                MassiveRequest.GetPrefixesToMassive().done(function (data) {
                                    if (data.success) {
                                        var dataPrefix = data.result;
                                        if (dataPrefix.length > 0) {
                                            $('#selectPrefix').UifSelect({ sourceData: data.result, selectedId: massiveEmission.Prefix.Id });
                                            $('#selectPrefix').prop('disabled', 'disabled');
                                        }
                                    }
                                });
                                MassiveRequest.GetCoveredRiskTypeByLineBusinessId(massiveEmission.Prefix.Id).done(function (data) {
                                    if (data.success) {
                                        $('#selectFileType').UifSelect({ sourceData: data.result, selectedId: massiveEmission.CoveredRiskType });
                                        if (data.result.length > 0) {
                                            $('#selectFileType').prop('disabled', 'disabled');
                                        }
                                    }
                                    else {
                                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                                    }
                                });

                                if ($('#selectLoadType').UifSelect('getSelected') == SubMassiveProcessType.MassiveEmissionWithRequest) {
                                    if (massiveLoad.BillingGroupId != null) {
                                        BillingGroup.GetBillingGroupsByDescription(massiveLoad.BillingGroupId).done(function (data) {
                                            if (data.success) {
                                                RequestGroup.GetCompanyRequestsByBillingGroupIdDescriptionRequestNumber(massiveLoad.BillingGroupId, massiveLoad.RequestId, massiveLoad.RequestNumber).done(function (data) {
                                                    if (data.success) {
                                                        $('#inputRequestGroup').data('Object', data.result[0]);
                                                        $('#inputRequestGroup').val(data.result[0].Description != null ? data.result[0].Description : data.result[0].Id + ' (' + data.result[0].Id + ')');
                                                    }
                                                });
                                            }
                                        });
                                    }
                                }

                                MassiveIssuance.DisableControls();
                                switch (parseInt($('#selectLoadType').UifSelect('getSelected'), 0)) {
                                    case SubMassiveProcessType.MassiveEmissionWithRequest:
                                    case SubMassiveProcessType.MassiveEmissionWithoutRequest:
                                    case SubMassiveProcessType.Inclusion:
                                    case SubMassiveProcessType.Exclusion:
                                    case SubMassiveProcessType.CollectiveEmission:
                                    case SubMassiveProcessType.CollectiveRenewal:
                                        if (statusLoad == 4) {
                                            MassiveIssuance.ShowButtons(MassiveLoadStatus.Tariffed);
                                        }
                                        else if (statusLoad == 6) {                                           
                                                MassiveIssuance.ShowButtons(MassiveLoadStatus.Issued);
                                        }

                                        break;
                                }


                            }
                            else {
                                $.UifNotify('show', { 'type': 'info', 'message': massiveEmission.ErrorDescription, 'autoclose': true });
                            }
                        }
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': "No existen Datos", 'autoclose': true });
                }

            } else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
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

    SearchBillingGroup() {
        searchType = 1;
        var number = parseInt(description, 10);
        var billingGroups = [];

        if ((!isNaN(number) && number != 0) || description.length > 2) {
            $('#inputRequestGroup').data('Object', null);
            $('#inputRequestGroup').prop('disabled', '                           disabled');
            BillingGroup.GetBillingGroupsByDescription(description).done(function (data) {
                if (data.success) {
                    if (data.result.length == 1) {
                        $('#inputRequestGroup').prop('disabled', '');
                    }
                    else {
                        for (var i = 0; i < data.result.length; i++) {
                            billingGroups.push({
                                Id: data.result[i].Id,
                                Code: data.result[i].Id,
                                Description: data.result[i].Description
                            });
                        }
                        MassiveIssuance.ShowDefaultResults(billingGroups);
                        $('#modalDefaultSearch').UifModal('showLocal', 'Seleccione un Grupo de Facturación');
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
    }

    SearchRequestGroup() {
        searchType = 2;
        var description = $("#inputRequestGroup").val().trim();
        var number = parseInt(description, 10);
        var billingGroups = [];

        if ((!isNaN(number) && number != 0) || description.length > 2) {
            RequestGroup.GetCompanyRequestsByBillingGroupIdDescriptionRequestNumber($('#inputBillingGroup').data('Object').Id, description, null).done(function (data) {
                if (data.success) {
                    if (data.result.length == 1) {
                        MassiveIssuance.LoadRequestGroup(data.result[0]);
                    }
                    else {
                        for (var i = 0; i < data.result.length; i++) {
                            billingGroups.push({
                                Id: data.result[i].Id,
                                Code: data.result[i].Id,
                                Description: data.result[i].Description
                            });
                        }
                        MassiveIssuance.ShowDefaultResults(billingGroups);
                        $('#modalDefaultSearch').UifModal('showLocal', 'Seleccione una Solicitud Agrupadora');
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
    }

    static LoadRequestGroup(requestGroup) {
        $('#inputRequestGroup').data('Object', requestGroup);
        $('#inputRequestGroup').val((requestGroup.Description != null ? requestGroup.Description : requestGroup.Id) + ' (' + requestGroup.Id + ')');
        $('#inputAgent').data('Object', requestGroup.CompanyRequestEndorsements[0].Agencies[0]);
        $('#inputAgent').val(requestGroup.CompanyRequestEndorsements[0].Agencies[0].Agent.FullName);

        Agency.GetAgenciesByAgentIdUserId($('#inputAgent').data('Object').Agent.IndividualId).done(function (data) {
            if (data.success) {
                $('#selectAgency').UifSelect({ sourceData: data.result, selectedId: $('#inputAgent').data('Object').Id });
                $('#selectAgency').prop('disabled', 'disabled');
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });

        Prefix.GetPrefixesByAgentId($('#inputAgent').data('Object').Agent.IndividualId).done(function (data) {
            if (data.success) {
                $('#selectPrefix').UifSelect({ sourceData: data.result, selectedId: requestGroup.Prefix.Id });
                $('#selectPrefix').prop('disabled', 'disabled');
                var loadType = $("#selectLoadType").UifSelect("getSelected");
                if (loadType == SubMassiveProcessType.CollectiveEmission || loadType == SubMassiveProcessType.Inclusion || loadType == SubMassiveProcessType.Exclusion || loadType == SubMassiveProcessType.CollectiveRenewal) {
                    Product.GetCollectiveProductsByAgentIdPrefixId($('#inputAgent').data('Object').Agent.IndividualId, $('#selectPrefix').UifSelect('getSelected')).done(function (data) {
                        if (data.success) {
                            $('#selectProduct').UifSelect({ sourceData: data.result, selectedId: requestGroup.CompanyRequestEndorsements[0].Product.Id });
                            $('#selectProduct').prop('disabled', 'disabled');
                        }
                        else {
                            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                        }
                    });
                }
                else {
                    Product.GetProductsByAgentIdPrefixId($('#inputAgent').data('Object').Agent.IndividualId, $('#selectPrefix').UifSelect('getSelected')).done(function (data) {
                        if (data.success) {
                            $('#selectProduct').UifSelect({ sourceData: data.result, selectedId: requestGroup.CompanyRequestEndorsements[0].Product.Id });
                            $('#selectProduct').prop('disabled', 'disabled');
                        }
                        else {
                            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                        }
                    });
                }
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });

        $('#selectBranch').UifSelect('setSelected', requestGroup.Branch.Id);
        $('#selectBranch').prop('disabled', 'disabled');

        if (requestGroup.Branch.SalePoints != null && requestGroup.Branch.SalePoints.length > 0) {
            Branch.GetSalePointsByBranchId($('#selectBranch').UifSelect('getSelected')).done(function (data) {
                if (data.success) {
                    $('#selectSalePoint').UifSelect({ sourceData: data.result, selectedId: requestGroup.Branch.SalePoints[0].Id });
                    $('#selectSalePoint').prop('disabled', 'disabled');
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }

        $('#selectBusinessType').UifSelect('setSelected', requestGroup.BusinessType);
    }

    SearchAgent() {
        searchType = 3;
        var description = $('#inputAgent').val().trim();
        var number = parseInt(description, 10);
        var agencies = [];

        if ((!isNaN(number) && number != 0) || description.length > 2) {
            Agency.GetAgenciesByUserIdAgentIdDescription(0, description).done(function (data) {
                if (data.success) {
                    if (data.result.length == 1) {
                        $('#inputAgent').data('Object', data.result[0]);
                        $('#inputAgent').val(data.result[0].Agent.FullName);

                        Agency.GetAgenciesByAgentIdUserId($('#inputAgent').data('Object').Agent.IndividualId).done(function (data) {
                            if (data.success) {
                                $('#selectAgency').UifSelect({ sourceData: data.result, selectedId: $('#inputAgent').data('Object').Id });
                            }
                            else {
                                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                            }
                        });

                        Prefix.GetPrefixesByAgentId($('#inputAgent').data('Object').Agent.IndividualId).done(function (data) {
                            if (data.success) {
                                $('#selectPrefix').UifSelect({ sourceData: data.result });
                            }
                            else {
                                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                            }
                        });
                    }
                    else {
                        for (var i = 0; i < data.result.length; i++) {
                            agencies.push({
                                Id: data.result[i].Agent.IndividualId,
                                Code: data.result[i].Code,
                                Description: data.result[i].Agent.FullName
                            });
                        }
                        MassiveIssuance.ShowDefaultResults(agencies);
                        $('#modalDefaultSearch').UifModal('showLocal', 'Seleccione un Intermediario');
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
    }

    SearchProduct() {
        var loadType = $("#selectLoadType").UifSelect("getSelected");
        if (loadType == SubMassiveProcessType.CollectiveEmission || loadType == SubMassiveProcessType.Inclusion || loadType == SubMassiveProcessType.Exclusion || loadType == SubMassiveProcessType.CollectiveRenewal) {
            Product.GetCollectiveProductsByAgentIdPrefixId($('#inputAgent').data('Object').Agent.IndividualId, $('#selectPrefix').UifSelect('getSelected')).done(function (data) {
                if (data.success) {
                    $('#selectProduct').UifSelect({ sourceData: data.result });
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
        else {
            Product.GetProductsByAgentIdPrefixId($('#inputAgent').data('Object').Agent.IndividualId, $('#selectPrefix').UifSelect('getSelected')).done(function (data) {
                if (data.success) {
                    $('#selectProduct').UifSelect({ sourceData: data.result });
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
    }

    SearchFileType() {
        MassiveRequest.GetCoveredRiskTypeByLineBusinessId($("#selectPrefix").UifSelect("getSelected")).done(function (data) {
            if (data.success) {
                $('#selectFileType').UifSelect({ sourceData: data.result, selectedId: data.result[0].Value });
                $('#selectFileType').prop('disabled', 'disabled');
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    SelectFileType() {
        selectedFileType = $("#selectFileType").UifSelect("getSelected");
    }

    SelectSearch() {
        switch (searchType) {
            case 1:
                BillingGroup.GetBillingGroupsByDescription($(this).children()[0].innerHTML).done(function (data) {
                    if (data.success) {
                        if (data.result.length == 1) {
                            $('#inputBillingGroup').data('Object', data.result[0]);
                            $('#inputBillingGroup').val(data.result[0].Description + ' (' + data.result[0].Id + ')');
                            $('#inputRequestGroup').prop('disabled', '');
                        }
                    }
                });
                break;
            case 2:
                RequestGroup.GetCompanyRequestsByBillingGroupIdDescriptionRequestNumber($('#inputBillingGroup').data('Object').Id, $(this).children()[0].innerHTML, null).done(function (data) {
                    if (data.success) {
                        if (data.result.length == 1) {
                            MassiveIssuance.LoadRequestGroup(data.result[0]);
                        }
                    }
                });
                break;
            case 3:
                Agency.GetAgenciesByUserIdAgentIdDescription($(this).children()[0].innerHTML, $(this).children()[1].innerHTML).done(function (data) {
                    if (data.success) {
                        $('#inputAgent').data('Object', data.result[0]);
                        $('#inputAgent').val(data.result[0].Agent.FullName);

                        Agency.GetAgenciesByAgentIdUserId($('#inputAgent').data('Object').Agent.IndividualId).done(function (data) {
                            if (data.success) {
                                $('#selectAgency').UifSelect({ sourceData: data.result, selectedId: $('#inputAgent').data('Object').Id });
                            }
                            else {
                                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                            }
                        });

                        Prefix.GetPrefixesByAgentId($('#inputAgent').data('Object').Agent.IndividualId).done(function (data) {
                            if (data.success) {
                                $('#selectPrefix').UifSelect({ sourceData: data.result });
                            }
                            else {
                                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                            }
                        });
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                });
                break;
            case 4:
                //cargues
                break;
        }

        $('#modalDefaultSearch').UifModal('hide');
    }

    AddFile(event) {
        if (event.target.files.length > 0) {
            if ($('#formMassive').valid()) {
                $('.progress').show();
                $(this).fileupload({
                    dataType: 'json',
                    url: rootPath + 'Massive/Massive/UploadFile',
                    done: function (event, data) {
                        if (data.result.success) {
                            $('#inputFile').data('Object', data.result.result);
                            if (data.files) {
                                $('inputFile').val(data.files[0].name);
                            }
                            $('.progress .progress-bar').css('width', 100 + '%');
                            $('#btnAddLoad').prop('disabled', false);
                        }
                        else {
                            $('#inputFile').data('Object', null);
                            $('.progress .progress-bar').css('width', 0 + '%');
                            $.UifNotify('show', { 'type': 'info', 'message': data.result.result, 'autoclose': true });
                        }
                    }
                })
            }
            else {
                $('.bootstrap-filestyle > input').val('');
                $('.progress .progress-bar').css('width', 0 + '%');
            }
        }
        else {
            $('.bootstrap-filestyle > input').val('');
            $('.progress .progress-bar').css('width', 0 + '%');
        }
    }

    AddLoad() {

        if ($('#formMassive').valid()) {
            var formMassive = $('#formMassive').serializeObject();

            formMassive.FileName = $('#inputFile').data('Object');
            formMassive.MassiveFileType = $('#selectFileType').UifSelect('getSelected');
            formMassive.MassiveFileTypeDescription = $('#selectFileType').UifSelect('getSelectedText');

            MassiveRequest.CreateLoad(formMassive).done(function (data) {
                if (data.success) {
                    $('.progress').hide();
                    $.UifDialog('alert', { 'message': data.result }, null);
                    var message = data.result.split(' ');
                    $('#inputLoadId').val(message[message.length - 1]);
                    MassiveIssuance.DisableControls();
                    $('#inputLoadName').prop('disabled', 'disabled');
                    $('#inputFile').prop('disabled', 'disabled');
                    $('#btnAddLoad').prop('disabled', true);
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
        MassiveIssuance.ClearForm();
    }

    Tariff() {
        var massiveLoad = $('#inputLoadId').data('Object');
        MassiveRequest.TariffedLoad(massiveLoad.Id).done(function (data) {
            if (data.success) {
                MassiveIssuance.ShowButtons();
                $.UifDialog('alert', { 'message': data.result }, null);
            } else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    Exclude() {
        $('#inputRecordsExclude').val('');
        $("#modalExclude").UifModal('showLocal', Resources.Language.LabelRecordsToExclude);
    }

    ThirdParty() {
        $("#modalThirdParty").UifModal('showLocal', Resources.Language.LabelRecordsToThirdParty);
        var massiveLoad = $('#inputLoadId').data('Object');
        MassiveRequest.ThirdParty(massiveLoad.Id, massiveLoad.LoadType.Id).done(function (data) {
            if (data.success) {
                $.UifDialog('alert', { 'message': data.result }, null);
                $('#btnThirdParty').prop('disabled', true);
            } else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });

    }

    IssuePolicy() {
        $('#btnIssue').prop('disabled', 'disabled');
        var massiveLoad = $('#inputLoadId').data('Object');
        MassiveRequest.ValidateIssuePolicies(massiveLoad.Id).done(function (data) {
            if (data.success) {
                if (data.result.confirmationType) {
                    $.UifDialog('confirm',
                        {
                            'message': data.result.message
                        },
                        function (result) {
                            if (result) {
                                MassiveRequest.IssuePolicy(massiveLoad.Id).done(function (data) {
                                    if (data.success) {
                                        MassiveIssuance.ShowButtons();
                                        $.UifDialog('alert', { 'message': data.result }, null);
                                    } else {
                                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                                    }
                                });
                            } else {
                                $('#btnIssue').prop('disabled', false);
                            }
                        });
                } else {
                    MassiveIssuance.ShowButtons();
                    $.UifDialog('alert', { 'message': data.result }, null);
                }
            } else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    GenerateErrorsFile() {
        var massiveStatus = 0;
        MassiveRequest.GenerateFileToMassiveLoad($('#inputLoadId').val(), $('#selectLoadType').val()).done(function (data) {
            if (data.success) {
                var a = document.createElement('A');
                a.href = data.result.Url;
                a.download = data.result.FileName;
                document.body.appendChild(a);
                a.click();
                document.body.removeChild(a);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });

    }

    Report() {
        var massiveStatus = $('#inputLoadId').data('Object').Status;
        var processType = $("#selectProcessType").UifSelect("getSelected");
        processType = parseInt(processType);
        switch (processType) {
            case MassiveProcessType.Cancellation:
                MassiveRequest.GenerateReportByMassiveLoadIdByStatus($('#inputLoadId').val(), massiveStatus).done(function (data) {
                    if (data.success) {
                        var a = document.createElement('A');
                        a.href = data.result.Url;
                        a.download = data.result.FileName;
                        document.body.appendChild(a);
                        a.click();
                        document.body.removeChild(a);
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                });
                break;
            default:
                MassiveRequest.GenerateReportToMassiveLoad($('#inputLoadId').val(), massiveStatus).done(function (data) {
                    if (data.success) {
                        var a = document.createElement('A');
                        a.href = data.result.Url;
                        a.download = data.result.FileName;
                        document.body.appendChild(a);
                        a.click();
                        document.body.removeChild(a);
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                });
        }
    }

    Print() {
        $('#modalPrinting').UifModal('showLocal', 'Seleccione los registros a imprimir');

        var massiveLoad = $('#inputLoadId').data('Object');
        var RiskSince = null, RiskUntil = null;
        RiskUntil = $('#RiskUntil').val();
        RiskSince = $('#inputRangeTo').val();

        var inputRangeTo = massiveLoad.TotalRows;

        if (massiveLoad.Status == 4) {
            $('#chkIssuedDetail').hide();
        }
        else {
            $('#chkIssuedDetail').show();
        }

        $('#RiskUntil').val(1);
        $('#inputRangeTo').val(riskToPrint);
        $('#inputRangeTo').prop('disabled', false);
    }



    ChangeRangeTo() {
        var recordsIssued = $('#inputRecordsIssued').val();
        var recordsTariffed = $('#inputRecordsTariffed').val();
        var rangeTo = $('#inputRangeTo').val();
        var rangeFrom = $('#inputRangeFrom').val();
        if (recordsIssued == "") {
            if (parseInt(rangeTo, 10) > parseInt(recordsTariffed, 10)) {
                rangeTo = recordsTariffed;
            }
        }
        if (parseInt(recordsTariffed, 10) > 0) {
            if (parseInt(rangeTo, 10) > parseInt(recordsIssued, 10)) {
                rangeTo = recordsIssued;
            }
        }
        if (rangeTo < rangeFrom && rangeFrom >= 1) {
            rangeTo = rangeFrom;
        }
        $('#inputRangeTo').val(rangeTo);
        $('#inputRangeFrom').val(rangeFrom);
    }

    ChangeRangeFrom() {
        var records = $('#inputRecordsIssued').val();
        var rangeTo = $('#inputRangeTo').val();
        var rangeFrom = $('#inputRangeFrom').val();

        if (rangeFrom < 1) {
            rangeFrom = 1;
        }
        if (rangeFrom > rangeTo && rangeTo <= records) {
            rangeFrom = rangeTo;
        }
        $('#inputRangeTo').val(rangeTo);
        $('#inputRangeFrom').val(rangeFrom);
    }

    PrintAccept() {
        var massiveLoad = $('#inputLoadId').data('Object');
        var RiskSince = null, RiskUntil = null;
        RiskUntil = $('#inputRangeTo').val();
        RiskSince = $('#RiskUntil').val();

        if (RiskSince > RiskUntil) {
            $.UifNotify('show', { 'type': 'info', 'message': 'El rango de riesgos ingresados no es valido', 'autoclose': true });
        } else {

            var collectFormat = false;
            var coutas = ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"];
            if ($("#chkEnabled").is(":checked")) {
                collectFormat = true;
            }

            $('#modalPrinting').UifModal('hide')

            if ($('#inputLoadId').data('Object') != null) {
                MassiveRequest.GetPrintingByLoadId($('#inputLoadId').val(), massiveLoad.Status, massiveLoad.LoadType.Id, RiskSince, RiskUntil, collectFormat, coutas).done(function (data) {

                    if (data.success) {
                        if (data.result === '') {
                            $('#inputRangeFrom').val(1);
                            if (massiveLoad.Status == MassiveLoadStatus.Tariffed) {

                                $('#inputRangeTo').val($('#inputRecordsTariffed').val());
                                $('#chkIssuedDetail').hide();
                            } else if (massiveLoad.Status == MassiveLoadStatus.Issued) {
                                var event = parseInt($('#inputRecordsEvents').val(), 10);
                                var forIssue = parseInt($('#inputRecordsForIssue').val(), 10);
                                var issued = parseInt($('#inputRecordsIssued').val(), 10);
                                var print;
                                if (event != 0) {
                                    print = event + issued;
                                }
                                else {
                                    print = issued + forIssue;
                                }
                                $('#inputRangeTo').val(print);
                                $('#chkIssuedDetail').show();
                            }
                            $('#modalPrinting').UifModal('showLocal', 'Seleccione los registros a imprimir');
                            $('#chkEnabled').prop('checked', false);
                            $('#btnPrintAccept').prop('disabled', false);
                        }
                        else {
                            if (data.result.Url !== undefined) {

                                var a = document.createElement('A');
                                a.href = data.result.Url;
                                a.download = data.result.FileName;
                                document.body.appendChild(a);
                                a.click();
                                document.body.removeChild(a);

                            } else if (data.result.PrintingProcess !== "") {
                                $.UifNotify('show', { 'type': 'info', 'message': 'Se genero el proceso de impresion numero: ' + data.result.PrintingProcess, 'autoclose': false });
                            }
                        }
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                });
            }
        }
    }

    ExcludeAccept() {

        if ($('#inputRecordsExclude').val() == "") {
            $.UifDialog('alert',
                { 'message': Resources.Language.MessageErrorExclusion },
            );
        } else {

            $.UifDialog('confirm',
                { 'message': Resources.Language.MessageConfirmExclusion },
                function (result) {
                    if (result) {
                        var massiveLoad = $('#inputLoadId').data('Object')
                        var strTemps = $('#inputRecordsExclude').val();
                        var temps = new Array();
                        temps = strTemps.split(",");

                        var temps = temps.filter(function (x) {
                            return (x !== (undefined || null || ''));
                        });

                        if (temps.length == 0) {
                            $.UifDialog('alert',
                                { 'message': Resources.Language.MessageErrorExclusion },
                            );
                            return;
                        }

                        MassiveRequest.ExcludeTemporalsByLoad(massiveLoad.Id, temps).done(function (data) {
                            if (data.success) {
                                $('#modalExclude').UifModal('hide');
                                $('#inputRecordsExclude').val();
                                $.UifNotify('show', { 'type': 'success', 'message': Resources.Language.MessageConfirmedExclusion, 'autoclose': true });
                            } else {
                                $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
                            }
                            $('#inputRecordsExclude').val('');
                        });
                    }
                });
        }
    }
    AdvancedSearch() {
        dropDownSearch.show();
    }

    DeleteProcess() {
        var massiveLoad = $('#inputLoadId').data('Object')

        $.UifDialog('confirm',
            {
                'message': Resources.Language.MessageConfirmToRemoveToLoad
            },
            function (result) {
                if (result) {
                    MassiveRequest.DeleteProcess(massiveLoad).done(function (data) {
                        if (data.success) {
                            DownloadFile(data.result);
                        }
                        else {
                            MassiveIssuance.ClearForm();
                            $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                        }
                        var temps = "Prueba ";
                    });
                }
            });
    }


}