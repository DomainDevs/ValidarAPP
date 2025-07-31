var dropDownSearch;
var dateRange = {};

class TemporalAdvancedSearch extends Uif2.Page {
    getInitialState() {        
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        $("#inputFromIssueDate").UifDatepicker();
        $("#inputToIssueDate").UifDatepicker();
        $("#inputHolderAdvanced").ValidatorKey(ValidatorType.Onlylettersandnumbers, 3, 0);
        dropDownSearch = uif2.dropDown({
            source: rootPath + 'Underwriting/Underwriting/TemporalAdvancedSearch',
            element: '#btnShowAdvanced',
            align: 'right',
            width: 600,
            height: 500,
            container: "#main",
            loadedCallback: TemporalAdvancedSearch.componentLoadedCallback
        });      
    }

    bindEvents() {
        $("#btnShowAdvanced").on("click", this.ShowAdvanced);     
    }

    ShowAdvanced() {
        TemporalAdvancedSearch.componentLoadedCallback();
        dropDownSearch.show();
        TemporalAdvancedSearch.ClearAdvanced();        
    }    


    static ChangeHolder(event, selectedItem) {
        TemporalAdvancedSearch.assignHolderId(event, selectedItem);
    }

    static ChangeUserAdv(event, selectedItem) {
        TemporalAdvancedSearch.assignUserId(event, selectedItem);
    }

    static assignHolderId(event, selectedItem) {
        if (selectedItem != null) {
            $("#inputHolderAdvanced").data("HolderAdvanced", selectedItem.IndividualId);
        }
        else {
            $("#inputHolderAdvanced").data("HolderAdvanced", null);
        }
    }

    static getAutocomplete(tag, param) {
        if ($(tag).UifAutoComplete('getValue') == null || $(tag).UifAutoComplete('getValue') == "") {
            $(tag).data(param, null);
        }
        return $(tag).data(param);
    }

    static componentLoadedCallback() {

        UnderwritingRequest.GetBranches().done(function (data) {
            if (data.success) {
                $("#selectBranchAdvanced").UifSelect({ sourceData: data.result });
            }
        });

        TemporalRequest.GetPrefixes().done(function (data) {
            if (data.success) {
                $("#selectPrefixCommercialAdvanced").UifSelect({ sourceData: data.result });
            }
        });

        $("#inputUserAdv").UifAutoComplete({
            source: rootPath + "Underwriting/Underwriting/GetUserPersonsByAccount",
            displayKey: "AccountName",
            queryParameter: "account"
        });

        $("#btnLoadTemporal").on("click", function () {
            dropDownSearch.hide();
            var temporalSelected = $("#listViewSearchAdvanced").UifListView("getSelected");
            if (temporalSelected != "") {
                UnderwritingTemporal.GetTemporalById(temporalSelected[0].Id);
            }
        });

        $("#listViewSearchAdvanced").UifListView({
            displayTemplate: "#advancedSearchTemplate",
            selectionType: 'single',
            source: null,
            height: 200
        });

        $("#btnCancelSearchAdvTemp").on('click', function () {
            TemporalAdvancedSearch.ClearAdvanced();
            dropDownSearch.hide();
        })

        $('#btnAdvancedSearch').on("click", function myfunction() {
            var individualId = TemporalAdvancedSearch.getAutocomplete("#inputHolderAdvanced", "HolderAdvanced");
            var policyAdvancedSearch = {
                Branch: { Id: $("#selectBranchAdvanced").UifSelect("getSelected") },
                Prefix: { Id: $("#selectPrefixCommercialAdvanced").UifSelect("getSelected") },
                Id: $("#inputTemporalAdvanced").val().trim(),
                Holder: { IndividualId: individualId == null ? 0 : individualId },
                UserId: TemporalAdvancedSearch.getAutocomplete("#inputUserAdv", "UserId"),
                CurrentFrom: $("#inputFromIssueDate").val(),
                CurrentTo: $("#inputToIssueDate").val(),
                TemporalType: glbPolicy.TemporalType
            };

            var send = TemporalAdvancedSearch.CalculateDaysAdvanced();
            if (send) {
                TemporalRequest.GetTemporalAdvancedSearch(policyAdvancedSearch).done(function (data) {
                    if (data.success) {
                        TemporalAdvancedSearch.LoadPolicyAdvanced(data.result);
                    }
                    else {
                        $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                    }
                }).fail(function (data) {
                    $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorSearchTemporary, 'autoclose': true });
                });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorInvalidDates, 'autoclose': true });
            }
        });

        $("#inputHolderAdvanced").UifAutoComplete({
            source: rootPath + "Underwriting/Underwriting/GetHoldersByQuery",
            displayKey: "Name",
            queryParameter: "insuredSearchType=" + InsuredSearchType.DocumentNumber + "&query"
        });

        $("#inputUserAdv").on("itemSelected", TemporalAdvancedSearch.ChangeUserAdv);
        $("#inputHolderAdvanced").on("itemSelected", TemporalAdvancedSearch.ChangeHolder);
    }

    static LoadPolicyAdvanced(policies) {
        $("#listViewSearchAdvanced").UifListView("refresh");
        $.each(policies, function (index, val) {
            policies[index].SubLimitAmount = FormatMoney(policies[index].SubLimitAmount);
            policies[index].PremiumAmount = FormatMoney(policies[index].PremiumAmount);
            policies[index].Rate = FormatMoney(policies[index].Rate);
            policies[index].IssueDate = FormatDate(policies[index].IssueDate);
            $("#listViewSearchAdvanced").UifListView("addItem", policies[index]);
        });
    }

    static CalculateDaysAdvanced() {
        var aFecha1 = $("#inputFromIssueDate").val().toString().split('/');
        var aFecha2 = $("#inputToIssueDate").val().toString().split('/');
        if (aFecha1 != "" && aFecha2 != "") {
            var fFecha1 = Date.UTC(aFecha1[2], aFecha1[1] - 1, aFecha1[0]);
            var fFecha2 = Date.UTC(aFecha2[2], aFecha2[1] - 1, aFecha2[0]);
            var dif = fFecha2 - fFecha1;
            var dias = Math.floor(dif / (1000 * 60 * 60 * 24));
            if (isNaN(dias) || dias > 30 || dias < 0) {
                return false;
            }
            else {
                dateRange.from = aFecha1;
                dateRange.to = aFecha2;
                return true;
            }
        }
        return true;
    }

    static ClearAdvanced() {
        $("#listViewSearchAdvanced").UifListView("refresh");
        $("#selectPrefixCommercialAdvanced").UifSelect("setSelected", null);
        $("#selectBranchAdvanced").UifSelect("setSelected", null);
        $("#inputTemporalAdvanced").val('');
        $("#inputFromIssueDate").UifDatepicker('clear');
        $("#inputToIssueDate").UifDatepicker('clear');
        $("#inputHolderAdvanced").data("HolderAdvanced", null);
        $('#inputHolderAdvanced').UifAutoComplete('setValue', '');
        $("#inputUserAdv").val('');
    }

    static assignUserId(event, selectedItem) {
        if (selectedItem != null) {
            $("#inputUserAdv").data("UserId", selectedItem.UserId);
        }
        else {
            $("#inputUserAdv").data("UserId", null);
        }
    }
}