var dropDownSearchQuotation;
class QuotationAdvancedSearch extends Uif2.Page {
    getInitialState() {
        dropDownSearchQuotation = uif2.dropDown({
            source: rootPath + 'Underwriting/Underwriting/QuotationAdvancedSearch',
            element: '#btnShowAdvQuot',
            align: 'right',
            width: 600,
            height: 500,
            container: "#main",
            loadedCallback: QuotationAdvancedSearch.componentLoadedCallback
        });
    }
    bindEvents() {
        $("#btnShowAdvQuot").on("click", QuotationAdvancedSearch.showDropDownQuotationAdv);
    }

    static showDropDownQuotationAdv() {
        dropDownSearchQuotation.show();
        QuotationAdvancedSearch.clearFieldsSearchAdv();
    }

    static loadQuotation() {
        var quotationSelected = $("#listVSearchAdvancedQuot").UifListView("getSelected")[0];
        if (quotationSelected != undefined) {
            if (quotationSelected.DocumentNumber == 0) {
                if (quotationSelected != null) {
                    if (QuotationAdvancedSearch.ValidateBranchId(quotationSelected.Branch.Id)) {
                        $("#inputHolder").data("Object", null);
                        glbPolicy = quotationSelected;
                        Underwriting.LoadTemporal(quotationSelected);
                        $('#btnIssuePolicy').prop('disabled', false);
                        $('#btnNewVersion').prop('disabled', false);
                        $('#btnSaveTempQuotation').prop('disabled', true);
                        $('#btnSaveQuotation').prop('disabled', true);
                    }
                    else
                        $.UifNotify('show', { 'type': 'danger', 'message': 'El usuario no tiene asignada la sucursal de la cotización', 'autoclose': true });
                }
                QuotationAdvancedSearch.closeAndClear();
            }
            else {
                $.UifNotify('show', { 'type': 'danger', 'message': AppResources.NotCanCheckQuotation, 'autoclose': true });
            }
        }
    }
    static ValidateBranchId(BranchIDQt) {

        var exist = $("#selectBranch option[value='" + BranchIDQt + "']").text();

        if (exist != null && exist.length > 1)
            return true;

        var exist = $("#selectBranch option[value='" + BranchIDQt + "']").text();

        if (exist != null && exist.length > 1)
            return true;

        return false;
    }
    static clearFieldsSearchAdv() {
        $("#idAdvQuotation").val("");
        $('#idAdvQuotation').ValidatorKey(ValidatorType.Number, 2, 0);
        $("#holderAdvQuotation").val("");
        $('#holderAdvQuotation').ValidatorKey(ValidatorType.Number, 2, 0);
        $("#userIdAdvQuotation").val("");
        $("#dateIssueFromQuotation").UifDatepicker("clear");
        $("#dateIssueToQuotation").UifDatepicker("clear");
        $("#listVSearchAdvancedQuot").UifListView("clear");
        $("#holderAdvQuotation").data("IndividualId", null);
        $("#userIdAdvQuotation").data("UserId", null);
    }

    static closeAndClear() {
        dropDownSearchQuotation.hide();
        QuotationAdvancedSearch.clearFieldsSearchAdv();
    }

    static componentLoadedCallback() {
        $("#holderAdvQuotation").UifAutoComplete({
            source: rootPath + "Underwriting/Underwriting/GetHoldersByQueryAdv",
            displayKey: "Name",
            queryParameter: "insuredSearchType=" + InsuredSearchType.DocumentNumber + "&query"
        });
        $("#userIdAdvQuotation").UifAutoComplete({
            source: rootPath + "Underwriting/Underwriting/GetUserPersonsByAccount",
            displayKey: "AccountName",
            queryParameter: "account"
        });
        //$("#dateIssueFromQuotation").UifDatepicker();
        //$("#dateIssueToQuotation").UifDatepicker();
        $("#listVSearchAdvancedQuot").UifListView({
            displayTemplate: "#advancedSearchQuotationTemplate",
            selectionType: 'single',
            source: null,
            height: 200
        });

        $("#btnLoadAdvQuot").on("click", function () {
            QuotationAdvancedSearch.loadQuotation();
        });

        $("#btnCancelSearchAdvQuot").on("click", function () {
            QuotationAdvancedSearch.closeAndClear();
        });

        $("#btnSearchAdvQuot").on("click", function () {
            QuotationAdvancedSearch.startSearchAdvQuotation();
        });

        $("#holderAdvQuotation").on("itemSelected", function (event, selectedItem) {
            QuotationAdvancedSearch.assignHolderId(event, selectedItem);
        });

        $("#userIdAdvQuotation").on("itemSelected", function (event, selectedItem) {
            QuotationAdvancedSearch.assignUserId(event, selectedItem);
        });
    }

    static assignHolderId(event, selectedItem) {
        if (selectedItem != null) {
            $("#holderAdvQuotation").data("IndividualId", selectedItem.IndividualId);
        }
        else {
            $("#holderAdvQuotation").data("IndividualId", null);
        }
    }

    static assignUserId(event, selectedItem) {
        if (selectedItem != null) {
            $("#userIdAdvQuotation").data("UserId", selectedItem.UserId);
        }
        else {
            $("#userIdAdvQuotation").data("UserId", null);
        }
    }

    static startSearchAdvQuotation() {
        var quotation = $("#formAdvancedQuotation").serializeObject();
        quotation.Endorsement = { QuotationId: quotation.Id };
        quotation.Id = null;
        quotation.Holder = { IndividualId: QuotationAdvancedSearch.getAutocomplete("#holderAdvQuotation", "IndividualId") };
        quotation.UserId = QuotationAdvancedSearch.getAutocomplete("#userIdAdvQuotation", "UserId");
        quotation.CurrentFrom = quotation.IssueFrom;
        quotation.CurrentTo = quotation.IssueTo;
        if (QuotationAdvancedSearch.validateQuotationAdv(quotation)) {
            QuotationAdvancedSearch.searchAdvQuotation(quotation);
        }
        else {
            $("#listVSearchAdvancedQuot").UifListView("clear");
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.SelectAtLeastTwoSearchCriteria, 'autoclose': true });
        }
    }

    static validateQuotationAdv(quotation) {
        var propertiesQuotationAvd = ["Endorsement", "Holder", "UserId", "CurrentFrom", "CurrentTo"];
        var subPropertiesQA = ["QuotationId", "IndividualId"];
        var band = false;
        $.each(propertiesQuotationAvd, function (id, item) {
            if (id == 0 || id == 1) {
                if (quotation[item][subPropertiesQA[id]] != "" && quotation[item][subPropertiesQA[id]] != null) {
                    band = true;
                    return true;
                }
            }
            else if (quotation[item] != "" && quotation[item] != null) {
                band = true;
                return true;
            }
        })
        return band;
    }

    static getAutocomplete(tag, param) {
        if ($(tag).UifAutoComplete('getValue') == null || $(tag).UifAutoComplete('getValue') == "") {
            $(tag).data(param, null);
        }
        return $(tag).data(param);
    }

    static fillListAdvSearchQuotation(data) {
        $("#listVSearchAdvancedQuot").UifListView("clear");
        $.each(data, function (id, item) {
            item.IssueDate = FormatDate(item.IssueDate);
            $("#listVSearchAdvancedQuot").UifListView("addItem", item);
        });
    }

    static searchAdvQuotation(quotation) {
        QuotationRequest.GetQuotationById(quotation).done(function (data) {
            if (data.success) {
                QuotationAdvancedSearch.fillListAdvSearchQuotation(data.result);
            }
            else {
                $("#listVSearchAdvancedQuot").UifListView("clear");
                $.UifNotify('show', { 'type': 'info', 'message': AppResources.NoItemsFound, 'autoclose': true });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', { 'type': 'info', 'message': AppResources.ErrorGetQuotationAdv, 'autoclose': true });
        });
    }
}