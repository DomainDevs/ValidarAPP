var dropDownSearchAdvDt = null;
var index;
$.ajaxSetup({ async: true });
class BranchAdvancedSearch extends Uif2.Page {
    getInitialState() {

        dropDownSearchAdvDt = uif2.dropDown({
            source: rootPath + 'Parametrization/Branch/BranchAdvancedSearch',
            element: '#inputBranch',
            align: 'right',
            width: 600,
            height: 500,
            loadedCallback: this.componentLoadedCallback
        });

    }
    bindEvents() {

    }
    componentLoadedCallback() {
        $("#lvSearchAdvBranch").UifListView({
            displayTemplate: "#BranchTemplate",
            selectionType: "single",
            height: 400
        });

        $("#btnSearchAdvBranch").on("click", BranchAdvancedSearch.SearchAdvBranch);
        $("#btnCancelSearch").on("click", BranchAdvancedSearch.CancelSearchAdv);
        $("#btnOkBranchSearchAdv").on("click", BranchAdvancedSearch.OkSearchAdv);
    }

    static SearchAdvBranch() {
        dropDownSearchAdvDt.show();
    }

    static CancelSearchAdv() {
        dropDownSearchAdvDt.hide();
    }

    static OkSearchAdv() {
        let data = $("#lvSearchAdvBranch").UifListView("getSelected");
        var list = $("#listViewBranch").UifListView('getData');
        $.each(list, function (key, value) {
            if (value.Id == data[0].Id) {
                index = key;
            }
        });

        BranchParametrization.ShowData(null, data[0], index);

        dropDownSearchAdvDt.hide();
    }
}