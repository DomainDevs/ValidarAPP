var dropDownSearch;

//$(() => {
//    new AdvancedEndoresementSearch();
//});
class AdvancedEndoresementSearch extends Uif2.Page {
    getInitialState() {
        dropDownSearch = uif2.dropDown({
            source: rootPath + 'Endorsement/Search/AdvancedSearch',
            element: '#btnAdvSearch',
            align: 'right',
            width: 600,
            height: 500,
            loadedCallback: this.AdvSearchEvents
        });
        SearchRequest.GetBranches().done(data => {
            if (data.success) {
                $('#advSelectBranch').UifSelect({ sourceData: data.result  });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        }); 
        
        SearchRequest.GetPrefixes().done(function (data) {
            if (data.success) {
                $('#advSelectPrefix').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });

        $("#inputHolderAdvanced").UifInputSearch(
            {
                placeholder: Resources.Language.PlaceHolderDocumentName
            })
        $("#dateIssueFrom").UifDatepicker();
        $("#dateIssueTo").UifDatepicker();
        $("#listAdvancedSearch").UifListView(
            {
                displayTemplate: "#searchTemplate",
                selectionType: 'single',
                source: null,
                height: 180
            });
        $('#tableResults').HideColums({ control: '#tableResults', colums: [0] });
        $("#advInputUser").UifAutoComplete({
            source: rootPath + "Underwriting/Underwriting/GetUserPersonsByAccount",
            displayKey: "Name",
            queryParameter: "account"
        });

        $('#modalDefaultSearch').UifModal('hide');
    }

    bindEvents() {
        $("#btnAdvSearch").click(() => { this.ShowAdvancedSearch(); });
        $('#inputHolderAdvanced').on('buttonClick', this.SearchHolder);
        $('#tableResults tbody').on('click', 'tr', this.SelectSearch);
        $("#btnCloseAdvSearch").click(() => { this.CloseSearch(); });
        $("#btnAcceptAdvSearch").click(() => { this.AcceptSearch(); });
        $("#btnSearchAdv").click(() => { this.Search(); });
        $("#advInputUser").on("itemSelected", function (event, selectedItem) { AdvancedSearch.AssignUserId(event, selectedItem); });
        $('#dateIssueFrom').on('datepicker.change', this.CheckDateFromRules);
        $('#dateIssueTo').on('datepicker.change', this.CheckDateToRules);

        $('#advSelectPrefix').on('itemSelected', function (event, selectedItem) {

            if (selectedItem.Id == 10) {
                $(".dvAdvPlate").removeClass('hide');
            }
            else {
                if (!$(".dvAdvPlate").hasClass('hide')) {
                    $(".dvAdvPlate").addClass('hide');
                }
            }
        });
    }
    CheckDateFromRules() {
        if ($('#dateIssueFrom').val().trim().length > 0) {
            if ($('#dateIssueTo').val().trim().length > 0) {
                if (!AdvancedEndoresementSearch.CalculateDaysAdvanced())
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorInvalidDates, 'autoclose': true });

            }
            else {
                var dateIssue = $("#dateIssueFrom").val().toString().split('/');
                var dateIssueFrom = new Date(dateIssue[2], dateIssue[1] - 1, dateIssue[0]);
                var dateIssueTo = AdvancedEndoresementSearch.AddDays(dateIssueFrom, 30);
                $("#dateIssueTo").val(dateIssueTo.getFromFormat('dd/mm/yyyy'));

                var ActualDate = new Date();
                if (new Date(dateIssueTo).getTime() > ActualDate.getTime()) {
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.TodateNotgreatherThanToday, 'autoclose': true });
                }
            }
        }
    }
    CheckDateToRules() {
        if ($('#dateIssueTo').val().trim().length > 0) {
            var dateTo = $("#dateIssueTo").val().toString().split('/');
            var dateIssueTo = new Date(dateTo[2], dateTo[1] - 1, dateTo[0]);

            if ($('#dateIssueFrom').val().trim().length > 0) {
                if (!AdvancedEndoresementSearch.CalculateDaysAdvanced())
                    $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorInvalidDates, 'autoclose': true });
            }
            else {
                $("#dateIssueFrom").val(dateIssueTo.getFromFormat('dd/mm/yyyy'));
            }

            var ActualDate = new Date();

            if (new Date(dateIssueTo).getTime() > ActualDate.getTime()) {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.TodateNotgreatherThanToday, 'autoclose': true });
            }
        }
    }
    static AddDays(date, days) {
        var result = new Date(date);
        result.setDate(result.getDate() + days);
        return result;
    }
    static CalculateDaysAdvanced() {
        var aFecha1 = $("#dateIssueFrom").val().toString().split('/');
        var aFecha2 = $("#dateIssueTo").val().toString().split('/');
        if (aFecha1 != "" && aFecha2 != "") {
            var fFecha1 = Date.UTC(aFecha1[2], aFecha1[1] - 1, aFecha1[0]);
            var fFecha2 = Date.UTC(aFecha2[2], aFecha2[1] - 1, aFecha2[0]);
            var dif = fFecha2 - fFecha1;
            var dias = Math.floor(dif / (1000 * 60 * 60 * 24));
            if (isNaN(dias) || dias > 30 || dias < 0) {
                return false;
            }
            else {
                return true;
            }
        }
        return true;
    }
    AdvSearchEvents() {

    }

    ShowAdvancedSearch() {
        this.CleanAdvancedSearch();
        dropDownSearch.show();
    }
    SearchHolder() {
        var description = $('#inputHolderAdvanced').val().trim();
        var number = parseInt(description, 10);
        var holders = [];

        if ((!isNaN(number) && number != 0) || description.length > 2) {
            Holder.GetHoldersByDescriptionInsuredSearchTypeCustomerType(description, InsuredSearchType.DocumentNumber, CustomerType.Individual).done(function (data) {
                if (data.success) {
                    if (data.result.length == 1) {
                        $('#inputHolderAdvanced').data('Object', data.result[0]);
                        $('#inputHolderAdvanced').val(data.result[0].Name + ' (' + data.result[0].IdentificationDocument.Number + ')');
                    }
                    else {
                        for (var i = 0; i < data.result.length; i++) {
                            holders.push({
                                Id: data.result[i].IndividualId,
                                Code: data.result[i].IdentificationDocument.Number,
                                Description: data.result[i].Name
                            });
                        }
                        AdvancedEndoresementSearch.ShowDefaultResults(holders);
                        $('#modalDefaultSearch').UifModal('showLocal', Resources.Language.SelectHolder);
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
        Holder.GetHoldersByDescriptionInsuredSearchTypeCustomerType($(this).children()[0].innerHTML, InsuredSearchType.IndividualId, CustomerType.Individual).done(function (data) {
            if (data.success) {
                $('#inputHolderAdvanced').data('Object', data.result[0]);
                $('#inputHolderAdvanced').val(data.result[0].Name + ' (' + data.result[0].IdentificationDocument.Number + ')');
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
        $('#modalDefaultSearch').UifModal('hide');
    }
    CloseSearch() {
        this.CleanAdvancedSearch();
        dropDownSearch.hide();
    }
    CleanAdvancedSearch() {
        $('#advSelectBranch').UifSelect('setSelected', null);
        $('#advSelectPrefix').UifSelect('setSelected', null);
        $('#advInputPolicy').val('');
        $('#inputHolderAdvanced').data('Object', null);
        $('#inputHolderAdvanced').val('');
        $("#advInputUser").data("UserId", null);
        $('#dateIssueFrom').val('');
        $('#dateIssueTo').val('');
        $("#listAdvancedSearch").UifListView({ source: null, displayTemplate: "#searchTemplate", selectionType: 'single' });

    }
    Search() {

        if (!AdvancedEndoresementSearch.CalculateDaysAdvanced()) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorInvalidDates, 'autoclose': true });
            return;
        }

        if (!(($('#dateIssueFrom').val().trim().length > 0 && $('#dateIssueTo').val().trim().length > 0)
            || ($('#dateIssueFrom').val().trim().length == 0 && $('#dateIssueTo').val().trim().length == 0))) {
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.DateRangeInvalid, 'autoclose': true });
        }

        if (!$("#dvAdvPlate").hasClass('hide') && $("#advInputPlate").val() != "") {

            var regex = /(^[A-Z]{3}\d{3}$)|(^[A-Z]{3}$)/;

            if (!regex.test($("#advInputPlate").val())) {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.ErrorValidateLicensePlate, 'autoclose': true });
                return;
            }
        }

        $("#listAdvancedSearch").UifListView({ source: null, displayTemplate: "#searchTemplate", selectionType: 'single' });

        if (($('#advSelectBranch').UifSelect('getSelected').length > 0
            && $('#advSelectPrefix').UifSelect('getSelected').length > 0)
            || $('#advInputPolicy').val().trim().length > 0
            || $('#inputHolderAdvanced').data('Object') != null
            || AdvancedEndoresementSearch.GetAutocomplete("#advInputUser", "UserId") != null
            )
        {
            var policy = $("#formAdvSearchEndorsement").serializeObject();


            policy.Endorsement = { DescriptionRisk: $("#advInputPlate").val() };
            if ($('#advSelectBranch').UifSelect('getSelected').length > 0)
                policy.Branch = { Id: $('#advSelectBranch').UifSelect('getSelected') };

            if ($('#advSelectPrefix').UifSelect('getSelected').length > 0)
                policy.Prefix = { Id: $('#advSelectPrefix').UifSelect('getSelected') };

            if ($('#advInputPolicy').val().trim().length > 0)
                policy.DocumentNumber = $('#advInputPolicy').val();

            if ($('#inputHolderAdvanced').data('Object') != null)
                policy.Holder = $('#inputHolderAdvanced').data('Object');

            if (AdvancedEndoresementSearch.GetAutocomplete("#advInputUser", "UserId") != null)
                policy.UserId = AdvancedEndoresementSearch.GetAutocomplete("#advInputUser", "UserId");

            if ($('#dateIssueFrom').val().trim().length > 0)
                policy.CurrentFrom = $('#dateIssueFrom').val();

            if ($('#dateIssueTo').val().trim().length > 0)
                policy.CurrentTo = $('#dateIssueTo').val();

            AdvancedEndoresementSearch.SearchAdvPolicy(policy);
        }
        else
            $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.SelectAtLeastTwoSearchCriteria, 'autoclose': true });
    }
    static AssignUserId(event, selectedItem) {
        if (selectedItem != null) {
            $("#advInputUser").data("UserId", selectedItem.UserId);
        }
        else {
            $("#advInputUser").data("UserId", null);
        }
    }
    static GetAutocomplete(tag, param) {
        if ($(tag).UifAutoComplete('getValue') == null || $(tag).UifAutoComplete('getValue') == "") {
            $(tag).data(param, null);
        }
        return $(tag).data(param);
    }
    static SearchAdvPolicy(policy) {

        Policy.GetpoliciesByPolicy(policy).done(function (data) {
            if (data.success) {
                AdvancedEndoresementSearch.FillListAdvSearchPolicy(data.result);
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': Resources.Language.NoItemsFound, 'autoclose': true });
            }
        });
    }
    static FillListAdvSearchPolicy(data) {
        $.each(data, function (id, item) {
            item.IssueDate = FormatFullDate(item.IssueDate);
            var value = item.Summary.FullPremium.toString();
            item.Summary.FullPremium = parseFloat(value.replace(/,/g, "")).toFixed(2).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            $("#listAdvancedSearch").UifListView("addItem", item);
        });
    }
    AcceptSearch() {
        if ($('#listAdvancedSearch').UifSelect('getSelected') != null) {
            var data = $("#listAdvancedSearch").UifListView('getSelected');
            data = data[0];
            $('#selectBranch').UifSelect("setSelected", data.Branch.Id);
            $('#selectPrefix').UifSelect("setSelected", data.Prefix.Id);
            $('#inputPolicyNumber').val(data.DocumentNumber);
            Search.HideSearchPlate();
            Search.HideItemsEndorsement();
            Search.GetPrefixEndoEnabled(data.Prefix.Id);
            Search.GetCurrentStatusPolicyByEndorsementIdIsCurrent(data.Endorsement.Id, false);
            dropDownSearch.hide();
        }
    }
}
class Branch {
    static GetBranches() {
        return $.ajax({
            type: 'POST',
            url: 'GetBranches',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}

class Holder {
    static GetHoldersByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType) {
        return $.ajax({
            type: 'POST',
            url: 'GetHoldersByDescriptionInsuredSearchTypeCustomerType',
            data: JSON.stringify({ description: description, insuredSearchType: insuredSearchType, customerType: customerType }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}
class Policy {
    static GetpoliciesByPolicy(policy) {
        return $.ajax({
            type: "POST",
            url: 'GetPoliciesByPolicy',
            data: JSON.stringify({ policy: policy }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}