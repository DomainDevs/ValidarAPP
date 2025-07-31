var searchType;

$(() => {
    new MassiveRenewal();
});

class MassiveRenewalRequest {
    static GetPrefixes() {
        return $.ajax({
            type: 'POST',
            url: 'GetPrefixes',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GetAgenciesByUserIdAgentIdDescription(agentId, description) {
        return $.ajax({
            type: 'POST',
            url: 'GetAgenciesByUserIdAgentIdDescription',
            data: JSON.stringify({ agentId: agentId, description: description }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GetBranches() {
        return $.ajax({
            type: 'POST',
            url: 'GetBranches',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GetHoldersByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType) {
        return $.ajax({
            type: 'POST',
            url: 'GetHoldersByDescriptionInsuredSearchTypeCustomerType',
            data: JSON.stringify({ description: description, insuredSearchType: insuredSearchType, customerType: customerType }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GetCoRequestsByDescription(description) {
        return $.ajax({
            type: 'POST',
            url: 'GetCoRequestsByDescription',
            data: JSON.stringify({ description: description }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }


    static GetPoliciesByRenewalViewModel(renewalViewModel) {
        return $.ajax({
            type: 'POST',
            url: 'GetPoliciesByRenewalViewModel',
            data: JSON.stringify({ renewalViewModel: renewalViewModel }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GenerateFileToRenewalProcess(processId, renewalProcesses, renewalStatus) {
        return $.ajax({
            type: 'POST',
            url: 'GenerateFileToRenewalProcess',
            data: JSON.stringify({ processId: processId, renewalProcesses: renewalProcesses, renewalStatus: renewalStatus }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GenerateFileToPayrollByAgent(processId, renewalProcesses) {
        return $.ajax({
            type: 'POST',
            url: 'GenerateFileToPayrollByAgent',
            data: JSON.stringify({ processId: processId, renewalProcesses: renewalProcesses }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GenerateProcess(policies) {
        return $.ajax({
            type: 'POST',
            url: 'GenerateProcess',
            data: JSON.stringify({ policies: policies }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GetProcessById(id) {
        return $.ajax({
            type: 'POST',
            url: 'GetProcessById',
            data: JSON.stringify({ id: id }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static FinalizeProcess(id, renewalProcesses) {
        return $.ajax({
            type: 'POST',
            url: 'FinalizeMassiveRenewalProcess',
            data: JSON.stringify({ id: id, renewalProcesses: renewalProcesses }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static DeleteProcess(id) {
        return $.ajax({
            type: 'POST',
            url: 'DeleteMassiveRenewalProcess',
            data: JSON.stringify({ id: id }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}

class MassiveRenewal extends Uif2.Page {
    getInitialState() {
        MassiveRenewal.HideControls();
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        $('#tableResults').HideColums({ control: '#tableResults', colums: [0] });
        $('#inputDueDateFrom').UifDatepicker('setMinDate', GetCurrentFromDate());
        $('#inputDueDateTo').UifDatepicker('setMinDate', GetCurrentFromDate());
        $('#inputDueDateTo').UifDatepicker('setMaxDate', AddToDate(GetCurrentFromDate(), 0, 1, 0));

        MassiveRenewalRequest.GetPrefixes().done(function (data) {
            if (data.success) {
                $('#selectPrefix').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });

        MassiveRenewalRequest.GetBranches().done(function (data) {
            if (data.success) {
                $('#selectBranch').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    bindEvents() {
        $('#inputDueDateFrom').on('datepicker.change', this.SetMaxDueDate);
        $('#inputProcess').on('buttonClick', this.SearchProcess);
        $('#inputAgency').on('buttonClick', this.SearchAgency);
        $('#inputHolder').on('buttonClick', this.SearchHolder);
        $('#inputRequestGroup').on('buttonClick', this.SearchCoRequest);
        $('#tableResults tbody').on('click', 'tr', this.SelectSearch);
        $('#btnSearch').click(this.SearchPolicies);
        $('#btnExit').click(this.Exit);
        $('#btnExport').click(this.Export);
        $('#btnPayrollByAgent').click(this.PayrollByAgent);
        $('#btnNew').click(MassiveRenewal.ClearForm);
        $('#btnGenerateProcess').click(this.GenerateProcess);
        $('#btnRenovate').click(this.Renovate);
        $('#btnDeleteProcess').click(this.Delete);
        $('#btnRenewalPrint').click(this.Print);
    }

    SetMaxDueDate() {
        var maxDate = AddToDate($('#inputDueDateFrom').val(), 0, 1, 0);
        $('#inputDueDateTo').UifDatepicker('setMaxDate', maxDate);
        $('#inputDueDateTo').UifDatepicker('setMinDate', $('#inputDueDateFrom').val());

        if (CompareDates($('#inputDueDateTo').val(), FormatDate(maxDate)) == 1 || CompareDates($('#inputDueDateFrom').val(), $('#inputDueDateTo').val()) == 1) {
            $('#inputDueDateTo').UifDatepicker('setValue', maxDate);
        }
    }

    SearchProcess() {
        if ($('#inputProcess').val().trim().length > 0) {
            MassiveRenewal.HideControls();
            MassiveRenewalRequest.GetProcessById($('#inputProcess').val().trim()).done(function (data) {
                if (data.success) {
                    for (var i = 0; i < data.result.RenewalProcesses.length; i++) {
                        data.result.RenewalProcesses[i].Policy.Summary.AmountInsured = FormatMoney(data.result.RenewalProcesses[i].Policy.Summary.AmountInsured);
                        data.result.RenewalProcesses[i].Policy.Summary.FullPremium = FormatMoney(data.result.RenewalProcesses[i].Policy.Summary.FullPremium);
                    }

                    if (data.result.Status == RenewalStatusType.Temporals) {
                        $('#btnRenewalPrint').show();
                        $('#btnExport').show();
                        $('#btnDeleteProcess').show();
                        $('#btnPayrollByAgent').show();
                        $('#btnRenovate').show();
                        $('#divTemporals').show();
                        $("#btnNew").removeClass("btn-primary");
                        $("#btnNew").addClass("btn-default");

                        $('#tableTemporals').UifDataTable('clear');
                        $('#tableTemporals').UifDataTable('addRow', data.result.RenewalProcesses);
                    }
                    else if (data.result.Status == RenewalStatusType.Renewals) {
                        $('#btnRenewalPrint').show();
                        $('#btnExport').show();
                        $('#btnPayrollByAgent').show();
                        $('#divRenewals').show();
                        $("#btnNew").removeClass("btn-default");
                        $("#btnNew").addClass("btn-primary");

                        $('#tableRenewals').UifDataTable('clear');
                        $('#tableRenewals').UifDataTable('addRow', data.result.RenewalProcesses);
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
    }

    SearchAgency() {
        searchType = 1;
        var description = $('#inputAgency').val().trim();
        var number = parseInt(description, 10);
        var agencies = [];

        if ((!isNaN(number) && number != 0) || description.length > 2) {
            MassiveRenewalRequest.GetAgenciesByUserIdAgentIdDescription(0, description).done(function (data) {
                if (data.success) {
                    if (data.result.length == 1) {
                        $('#inputAgency').data('Object', data.result[0]);
                        $('#inputAgency').val(data.result[0].Agent.FullName + ' (' + data.result[0].Code + ')');
                    }
                    else {
                        for (var i = 0; i < data.result.length; i++) {
                            agencies.push({
                                Id: data.result[i].Agent.IndividualId,
                                Code: data.result[i].Code,
                                Description: data.result[i].Agent.FullName
                            });
                        }
                        MassiveRenewal.ShowDefaultResults(agencies);
                        $('#modalDefaultSearch').UifModal('showLocal', 'Seleccione un Intermediario');
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
    }

    SearchHolder() {
        searchType = 2;
        var description = $('#inputHolder').val().trim();
        var number = parseInt(description, 10);
        var holders = [];

        if ((!isNaN(number) && number != 0) || description.length > 2) {
            MassiveRenewalRequest.GetHoldersByDescriptionInsuredSearchTypeCustomerType(description, InsuredSearchType.DocumentNumber, CustomerType.Individual).done(function (data) {
                if (data.success) {
                    if (data.result.length == 1) {
                        $('#inputHolder').data('Object', data.result[0]);
                        $('#inputHolder').val(data.result[0].Name + ' (' + data.result[0].IdentificationDocument.Number + ')');
                    }
                    else {
                        for (var i = 0; i < data.result.length; i++) {
                            holders.push({
                                Id: data.result[i].IndividualId,
                                Code: data.result[i].IdentificationDocument.Number,
                                Description: data.result[i].Name
                            });
                        }
                        MassiveRenewal.ShowDefaultResults(holders);
                        $('#modalDefaultSearch').UifModal('showLocal', 'Seleccione un Tomador');
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
    }

    SearchCoRequest() {
        searchType = 3;
        var description = $('#inputRequestGroup').val().trim();
        var number = parseInt(description, 10);
        var coRequests = [];

        if ((!isNaN(number) && number != 0) || description.length > 2) {
            MassiveRenewalRequest.GetCoRequestsByDescription(description).done(function (data) {
                if (data.success) {
                    if (data.result.length == 1) {
                        $('#inputRequestGroup').data('Object', data.result[0]);
                        $('#inputRequestGroup').val(data.result[0].Description + ' (' + data.result[0].Id + ')');
                    }
                    else {
                        for (var i = 0; i < data.result.length; i++) {
                            coRequests.push({
                                Id: data.result[i].Id,
                                Code: data.result[i].Id,
                                Description: data.result[i].Description
                            });
                        }
                        MassiveRenewal.ShowDefaultResults(coRequests);
                        $('#modalDefaultSearch').UifModal('showLocal', 'Seleccione una Solicitud Agrupadora');
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
    }

    static ShowDefaultResults(dataTable) {
        $('#tableResults').UifDataTable('clear');
        $('#tableResults').UifDataTable('addRow', dataTable);
    }

    SelectSearch() {
        switch (searchType) {
            case 1:
                MassiveRenewalRequest.GetAgenciesByUserIdAgentIdDescription($(this).children()[0].innerHTML, $(this).children()[1].innerHTML).done(function (data) {
                    if (data.success) {
                        $('#inputAgency').data('Object', data.result[0]);
                        $('#inputAgency').val(data.result[0].Agent.FullName + ' (' + data.result[0].Code + ')');
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                });
                break;
            case 2:
                MassiveRenewalRequest.GetHoldersByDescriptionInsuredSearchTypeCustomerType($(this).children()[0].innerHTML, InsuredSearchType.IndividualId, CustomerType.Individual).done(function (data) {
                    if (data.success) {
                        $('#inputHolder').data('Object', data.result[0]);
                        $('#inputHolder').val(data.result[0].Name + ' (' + data.result[0].IdentificationDocument.Number + ')');
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                });
                break;
            case 3:
                MassiveRenewalRequest.GetCoRequestsByDescription($(this).children()[0].innerHTML).done(function (data) {
                    if (data.success) {
                        $('#inputRequestGroup').data('Object', data.result[0]);
                        $('#inputRequestGroup').val(data.result[0].Description + ' (' + data.result[0].Id + ')');
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                });
                break;
        }

        $('#modalDefaultSearch').UifModal('hide');
    }

    SearchPolicies() {
        if ($('#formRenewal').valid()) {
            MassiveRenewal.HideControls();

            var formRenewal = $('#formRenewal').serializeObject();
            if ($('#inputAgency').val().trim().length > 0 && $('#inputAgency').data('Object') != null) {
                formRenewal.AgentId = $('#inputAgency').data('Object').Agent.IndividualId;
                formRenewal.AgencyId = $('#inputAgency').data('Object').Id;
            }
            if ($('#inputHolder').val().trim().length > 0 && $('#inputHolder').data('Object') != null) {
                formRenewal.HolderId = $('#inputHolder').data('Object').IndividualId;
            }
            if ($('#inputRequestGroup').val().trim().length > 0 && $('#inputRequestGroup').data('Object') != null) {
                formRenewal.RequestGroupId = $('#inputRequestGroup').data('Object').Id;
            }

            MassiveRenewalRequest.GetPoliciesByRenewalViewModel(formRenewal).done(function (data) {
                if (data.success) {
                    $('#divSearch').show();
                    $('#btnGenerateProcess').show();
                    $('#inputProcess').val('');
                    for (var i = 0; i < data.result.length; i++) {
                        data.result[i].Endorsement.CurrentTo = FormatDate(data.result[i].Endorsement.CurrentTo);
                        if (data.result[i].Endorsement.TemporalId > 0) {
                            data.result[i].Observations = 'Temporal De ' + data.result[i].Endorsement.EndorsementTypeDescription + ' ' + data.result[i].Endorsement.TemporalId;
                        }
                        else {
                            data.result[i].Observations = '';
                        }
                    }

                    $('#tableSearch').UifDataTable('clear');
                    $('#tableSearch').UifDataTable('addRow', data.result);
                }
                else {
                    MassiveRenewal.ClearForm();
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
            $('#btnSearch').attr('disabled','disabled');
            $("#btnNew").removeClass("btn-primary");
            $("#btnNew").addClass("btn-default");
        }
    }

    Exit() {
        window.location = rootPath + "Home/Index";
    }

    Export() {
        var renewalStatus = 0;
        var processes = null;

        if ($("#tableTemporals").is(":visible")) {
            renewalStatus = RenewalStatusType.Temporals;
            processes = $("#tableTemporals").UifDataTable("getSelected");
        }
        else if ($("#tableRenewals").is(":visible")) {
            renewalStatus = RenewalStatusType.Renewals;
            processes = $("#tableRenewals").UifDataTable("getSelected");
        }

        if (processes != null) {
            for (var i = 0; i < processes.length; i++) {
                processes[i].Policy.Summary.AmountInsured = NotFormatMoney(processes[i].Policy.Summary.AmountInsured);
                processes[i].Policy.Summary.FullPremium = NotFormatMoney(processes[i].Policy.Summary.FullPremium);
            }

            MassiveRenewalRequest.GenerateFileToRenewalProcess($('#inputProcess').val(), processes, renewalStatus).done(function (data) {
                if (data.success) {                    
                    DownloadFile(data.result);
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
    }

    PayrollByAgent() {
        var processes = null;

        if ($("#tableTemporals").is(":visible")) {
            processes = $("#tableTemporals").UifDataTable("getSelected");
        }
        else if ($("#tableRenewals").is(":visible")) {
            processes = $("#tableRenewals").UifDataTable("getSelected");
        }

        if (processes != null) {
            for (var i = 0; i < processes.length; i++) {
                processes[i].Policy.Summary.AmountInsured = NotFormatMoney(processes[i].Policy.Summary.AmountInsured);
                processes[i].Policy.Summary.FullPremium = NotFormatMoney(processes[i].Policy.Summary.FullPremium);
                processes[i].Policy.CurrentFrom = FormatDate($("#inputDueDateFrom").val());
                processes[i].Policy.CurrentTo = FormatDate($("#inputDueDateTo   ").val());
            }

            MassiveRenewalRequest.GenerateFileToPayrollByAgent($('#inputProcess').val(), processes).done(function (data) {
                if (data.success) {                    
                    DownloadFile(data.result);
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });
        }
    }

    static ClearForm() {
        $('#formRenewal').formReset();
        $('#inputProcess').val('');
        $('#inputAgency').data('Object', null);
        $('#inputHolder').data('Object', null);
        $('#inputRequestGroup').data('Object', null);
        $('#inputDueDateFrom').UifDatepicker('setMinDate', GetCurrentFromDate());
        $('#inputDueDateTo').UifDatepicker('setMinDate', GetCurrentFromDate());
        $('#inputDueDateTo').UifDatepicker('setMaxDate', AddToDate(GetCurrentFromDate(), 0, 1, 0));
        $('#btnSearch').removeAttr('disabled');
        MassiveRenewal.HideControls();
        $("#btnNew").removeClass("btn-default");
        $("#btnNew").addClass("btn-primary");
    }

    static HideControls() {
        $('#btnRenewalPrint').hide();
        $('#btnExport').hide();
        $('#btnGenerateProcess').hide();
        $('#btnDeleteProcess').hide();
        $('#btnPayrollByAgent').hide();
        $('#btnRenovate').hide();
        $('#divSearch').hide();
        $('#divTemporals').hide();
        $('#divRenewals').hide();
    }

    GenerateProcess() {
        MassiveRenewalRequest.GenerateProcess($('#tableSearch').UifDataTable('getSelected')).done(function (data) {
            if (data.success) {
                $.UifDialog('alert', { 'message': data.result }, null);
                var message = data.result.split(' ');
                MassiveRenewal.HideControls();
                $('#inputProcess').val(message[message.length - 1]);
                $('#btnRenewalPrint').show();
                $('#btnExport').show();
                $('#btnDeleteProcess').show();
                $('#btnPayrollByAgent').show();
                $('#btnRenovate').show();
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    Delete() {
        MassiveRenewalRequest.DeleteProcess($('#inputProcess').val()).done(function (data) {
            if (data.success) {
                MassiveRenewal.ClearForm();
                $('#inputProcess').val("");
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }

    Print() {
        var items = null;

        if ($('#divTemporals').is(":visible")) {
            items = $('#tableTemporals').UifDataTable('getSelected');
            if (items != null) {
                if (items.length == 1) {
                    if (items[0].HasError) {
                        $.UifNotify('show', { 'type': 'info', 'message': 'No Puede Imprimir Un Temporal Con Errores', 'autoclose': true });
                    }
                    else {
                        PrintReportFromOutside(0, 0, 0, 0, items[0].Policy.Id);
                    }                    
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': 'Debe Seleccionar Solo Un Temporal', 'autoclose': true });
                }
            }
        }
        else if ($('#divRenewals').is(":visible")) {
            items = $('#tableRenewals').UifDataTable('getSelected');
            if (items != null) {
                if (items.length == 1) {
                    if (items[0].HasError) {
                        $.UifNotify('show', { 'type': 'info', 'message': 'No Puede Imprimir Una Póliza Con Errores', 'autoclose': true });
                    }
                    else {
                        PrintReportFromOutside(items[0].Policy.Branch.Id, items[0].Policy.Prefix.Id, items[0].Policy.DocumentNumber, items[0].Policy.Endorsement.Number, 0);
                    }
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': 'Debe Seleccionar Solo Una Póliza', 'autoclose': true });
                }
            }
        }
    }

    Renovate() {
        MassiveRenewalRequest.FinalizeProcess($('#inputProcess').val(), $('#tableTemporals').UifDataTable('getSelected')).done(function (data) {
            if (data.success) {
                $.UifDialog('alert', { 'message': data.result }, null);
                var message = data.result.split(' ');
                MassiveRenewal.HideControls();
                $('#inputProcess').val(message[message.length - 1]);
                $('#btnRenewalPrint').show();
                $('#btnExport').show();
                $('#btnPayrollByAgent').show();
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }
}