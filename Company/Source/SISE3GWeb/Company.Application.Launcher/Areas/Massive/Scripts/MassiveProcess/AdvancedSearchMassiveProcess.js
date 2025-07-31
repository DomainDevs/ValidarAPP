var dropDownSearchMassiveProcess;


$(() => {
    new SearchAdvancedMassiveProcess();
});

class MassiveProcessSearch {

    static SearchAdvancedMassiveProcess(fromDate, toDate, massive) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Massive/MassiveProcess/GetMassiveProcessesAdvancedSearch',
            data: JSON.stringify({ rangeFrom: fromDate, rangeTo: toDate, massiveLoad: massive }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetProcessType() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Massive/MassiveProcess/GetProcessType',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GetStatusProcess() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Massive/MassiveProcess/GetStatusProcess',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static GetUsers() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Massive/MassiveProcess/GetUsers',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}

class SearchAdvancedMassiveProcess extends Uif2.Page {

    getInitialState() {
        dropDownSearchMassiveProcess = uif2.dropDown({
            source: rootPath + 'Massive/MassiveProcess/SearchAdvancedMassiveProcess',
            element: '#btnShowAdvanced',
            align: 'right',
            width: 600,
            height: 500,
            loadedCallback: this.AdvancedSearchEvents
        });
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
    }

    AdvancedSearchEvents() {
        MassiveProcessSearch.GetProcessType().done(function (data) {
            if (data.success) {
                $('#selectProcessTypes').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });

        MassiveProcessSearch.GetStatusProcess().done(function (data) {
            if (data.success) {
                $('#selectMassiveLoadStatus').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });

       

        $("#btnCancelSearchAdv").on("click", SearchAdvancedMassiveProcess.cancelSearch);
        $("#btnShowAdvancedSearch").on("click", SearchAdvancedMassiveProcess.SearchMassiveProcessAdvanced);
    }

    bindEvents() {

    }

    static cancelSearch() {
        dropDownSearchMassiveProcess.hide();
    }

    static SearchMassiveProcessAdvanced() {

         massive =
            {
                LoadType:
                {
                    ProcessType: $("#selectProcessTypes").val()
                },
                Status: $("#selectMassiveLoadStatus").val(),
                Description: $("#inputDescriptionSearch").val().trim(),
                User:
                {
                    UserId: $("#selectUsers").val()
                }
            };

         toDate = $("#inputToDate").val();
         fromDate = $("#inputFromDate").val();

        SearchAdvancedMassiveProcess.ClearControls();
        MassiveProcessSearch.SearchAdvancedMassiveProcess(fromDate, toDate, massive).done(function (data) {
            $('#TableLoadProcess').UifDataTable('clear');
            if (data.success) {
                MassiveProcess.LoadTable(data.result);
                SearchAdvancedMassiveProcess.cancelSearch();
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ResultNotFound, 'autoclose': true })
            }

        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'danger', 'message': Resources.Language.ErrorSearchFile, 'autoclose': true })
        });
    }

    static ClearControls() {
        $("#inputFrom").val('');
        $('#selectProcessTypes').UifSelect('setSelected', null);
        $('#selectMassiveLoadStatus').UifSelect('setSelected', null);
        $("#inputDescriptionSearch").val(''); 
        $("#inputFromDate").val('');
        $("#inputToDate").val('');
        $('#selectUsers').UifSelect('setSelected', null);
    }

}