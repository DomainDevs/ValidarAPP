var dropDownSearchAdvInsuredObject = null;
var index;


class InsuredObjectAdvancedSearch extends Uif2.Page {
    getInitialState() {
        $.ajaxSetup({ async: true });
        dropDownSearchAdvInsuredObject = uif2.dropDown({
            source: rootPath + 'Parametrization/InsuredObject/InsuredObjectAdvancedSearch',
            element: '#inputInsuredObject',
            align: 'right',
            width: 600,
            height: 500,
            loadedCallback: this.componentLoadedCallback
        });

    }
    bindEvents() {

    }
    componentLoadedCallback() {
        $("#lvSearchAdvInsuredObject").UifListView({
            displayTemplate: "#InsuredObjectTemplate",
            selectionType: "single",
            height: 400
        });

        $("#btnSearchAdvInsuredObject").on("click", InsuredObjectAdvancedSearch.SearchAdvInsuredObject);
        $("#btnCancelSearch").on("click", InsuredObjectAdvancedSearch.CancelSearchAdv);
        $("#btnOkSearchAdvInsured").on("click", InsuredObjectAdvancedSearch.OkSearchAdv);
    }

    static SearchAdvInsuredObject() {
        dropDownSearchAdvInsuredObject.show();
    }

    static CancelSearchAdv() {
        dropDownSearchAdvInsuredObject.hide();
    }

    static OkSearchAdv() {
        let data = $("#lvSearchAdvInsuredObject").UifListView("getSelected");
         var list = $("#listViewInsuredObject").UifListView('getData');
        $.each(list, function (key, value) {
            if (value.Id == data[0].Id) {
                index = key;
            }
        });
        InsuredObject.ShowData(null, data[0], index);
        dropDownSearchAdvInsuredObject.hide();
    }
}