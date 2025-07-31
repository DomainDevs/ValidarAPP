var dropDownSearchQuotation;

$(() => {
    new QuotationAdvancedSearch();
});
class QuotationSearch {
    static GetPoliciesByPolicy(policy) {
        return $.ajax({
            type: 'POST',
            url: 'GetPoliciesByPolicy',
            data: JSON.stringify({ policy: policy }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    static GetPersonsByQuery(query) {
        return $.ajax({
            type: 'POST',
            url: 'GetPersonsByQuery',
            data: JSON.stringify({ query: query }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
}
class QuotationAdvancedSearch extends Uif2.Page {
    getInitialState() {
        dropDownSearchQuotation = uif2.dropDown({
            source: 'QuotationAdvancedSearch',
            element: '#btnShowAdvQuot',
            align: 'right',
            width: 600,
            height: 500,
            container: "#main",
            loadedCallback: this.componentLoadedCallback
        });

        Quotation.GetProducts().done(function (data) {
            if (data.success) {
                //$('#selectProduct').UifSelect({ sourceData: data.result });
                $('#selectSearchProduct').UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
            }
        });
    }
    bindEvents() {
        $("#inputName").ValidatorKey(ValidatorType.Onlylettersandnumbers, 3, 0);
        
        $("#inputSearchDocumentNumber").ValidatorKey(ValidatorType.Onlylettersandnumbers, 3, 0);
        this.componentLoadedCallback();
    }

    static assignUserId(event, selectedItem) {
        if (selectedItem != null) {
            $("#inputName").data("UserId", selectedItem.UserId);
        }
        else {
            $("#inputName").data("UserId", null);
        }
    }
    static assignPersonId(event, selectedItem) {
        if (selectedItem != null) {
            $("#inputSearchDocumentNumber").data("PersonId", selectedItem.Id);
        }
        else {
            $("#inputSearchDocumentNumber").data("PersonId", null);
        }
    }
    static startSearchAdvQuotation() {
        $("#listVSearchAdvancedQuot").UifListView("refresh");
        var quotation = {};       
        quotation.Policy = {};
        quotation.Policy.Holder = { IndividualId: QuotationAdvancedSearch.getAutocomplete("#inputSearchDocumentNumber", "PersonId") };
        quotation.Policy.UserId = QuotationAdvancedSearch.getAutocomplete("#inputName", "UserId");
        if ($('#dateIssueFromQuotation').val() != "") {
            quotation.Policy.CurrentFrom = $('#dateIssueFromQuotation').val();
        }
        if ($('#dateIssueToQuotation').val() != "") {
            quotation.Policy.CurrentTo = $('#dateIssueToQuotation').val();
        }
        quotation.Policy.Product = { Id: $('#selectSearchProduct').val() }
        QuotationSearch.GetPoliciesByPolicy(quotation).done(function (data) {
            if (data.success) {
                QuotationAdvancedSearch.fillListAdvSearchQuotation(data.result);
            }
            else {
                $("#listVSearchAdvancedQuot").UifListView("refresh");
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            $.UifNotify('show', {
                'type': 'info', 'message': 'Error al consultar cotización', 'autoclose': true
            });
        });

    }

    static showDropDownQuotationAdv() {
        //  QuotationAdvancedSearch.componentLoadedCallback();
        QuotationAdvancedSearch.clearFieldsSearchAdv();
        dropDownSearchQuotation.show();

        //  QuotationAdvancedSearch.componentLoadedCallback();
    }

    static loadQuotation() {
        var quotationSelected = $("#listVSearchAdvancedQuot").UifListView("getSelected")[0];
        if (quotationSelected != null) {
            Quotation.GetQuotationByTemporalId(quotationSelected.Policy.Id).done(function (data) {
                if (data.success) {
                    QuotationView.LoadQuotation(data.result);
                }
                else {
                    $.UifNotify('show', { 'type': 'info', 'message': data.result, 'autoclose': true });
                }
            });

            QuotationView.LoadQuotation(quotationSelected);
        }
        QuotationAdvancedSearch.closeAndClear();
    }

    static clearFieldsSearchAdv() {
        //$("#idAdvQuotation").val("");
        $("#inputName").val("");
        $("#inputSearchDocumentNumber").val("");
        //$("#dateIssueFromQuotation").UifDatepicker("clear");
        //$("#dateIssueToQuotation").UifDatepicker("clear");
        $("#listVSearchAdvancedQuot").UifListView("refresh");
        // $("#holderAdvQuotation").data("IndividualId", null);
        // $("#userIdAdvQuotation").data("UserId", null);
        $("#inputSearchDocumentNumber").data("PersonId", null);
    }

    static closeAndClear() {
        dropDownSearchQuotation.hide();
        QuotationAdvancedSearch.clearFieldsSearchAdv();
    }

    componentLoadedCallback() {
     
        $("#btnShowAdvQuot").on("click", function () {
            QuotationAdvancedSearch.showDropDownQuotationAdv();
        });

        $("#inputName").UifAutoComplete({
            source: "GetUsersByQuery",
            displayKey: "AccountName",
            queryParameter: "&query"
        });
        $("#inputSearchDocumentNumber").UifAutoComplete({
            source: "GetPersonsByQuery",
            displayKey: "TradeName",
            queryParameter: "&query"
        });
        $("#dateIssueFromQuotation").UifDatepicker();
        $("#dateIssueToQuotation").UifDatepicker();
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
        
        $("#inputName").on("itemSelected", QuotationAdvancedSearch.assignUserId);
        $("#inputSearchDocumentNumber").on("itemSelected", QuotationAdvancedSearch.assignPersonId);
        $('#dateIssueFromQuotation').focusout(function () {
            if ($('#dateIssueToQuotation').val() == "" && $('#dateIssueFromQuotation').val() != "") {
                $("#dateIssueToQuotation").UifDatepicker('setValue', AddToDate($("#dateIssueFromQuotation").val(), 1));
            }
        });
        //$("#inputSearchDocumentNumber").on("click", function () {         
        //        QuotationAdvancedSearch.getPersons();
        //});
    }

    static assignHolderId(event, selectedItem) {
        if (selectedItem != null) {
            $("#holderAdvQuotation").data("IndividualId", selectedItem.IndividualId);
        }
        else {
            $("#holderAdvQuotation").data("IndividualId", null);
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
        $("#listVSearchAdvancedQuot").UifListView("refresh");
        $.each(data, function (id, item) {
            item.Policy.CurrentFrom = FormatDate(item.Policy.CurrentFrom);
            $("#listVSearchAdvancedQuot").UifListView("addItem", item);
        });
    }

    //static getPersons() {
    //    if ($("#inputSearchDocumentNumber").val() != "") {        
    //        $("#inputSearchDocumentNumber").data("PersonId", '');
    //        $("#inputSearchDocumentNumber").val('');
    //        QuotationSearch.GetPersonsByQuery($("#inputSearchDocumentNumber").val()).done(function (data) {
    //            if (data) {
    //                if (data.length == 1) {
    //                    $("#inputSearchDocumentNumber").data("PersonId", data[0].IndividualId);
    //                    $("#inputSearchDocumentNumber").val(data[0].TradeName);
    //                }                
    //            }
    //        });

    //    }        
    //}

}