var dropDownSearchAdvCompanyAddressType = null;
$(() => {
    new CompanyAddressTypeAdvancedSearch();
});
class CompanyAddressTypeAdvancedSearch extends Uif2.Page {
    getInitialState() {

        dropDownSearchAdvCompanyAddressType = uif2.dropDown({
            source: rootPath + 'Person/Person/CompanyAddressTypeAdvancedSearch',
            element: '#btnSearchAdvCompanyAddressType',
            align: 'right',
            width: 550,
            height: 551,
            loadedCallback: this.componentLoadedCallback
        });

    }
    bindEvents() {

    }
    componentLoadedCallback() {
        $("#lvSearchAdvCompanyAddressType").UifListView({
            displayTemplate: "#CompanyAddressTypeTemplate",
            selectionType: "single",
            height: 450
        });

        $("#btnSearchAdvCompanyAddressType").on("click", CompanyAddressTypeAdvancedSearch.SearchAdvCompanyAddressType);
        $("#btnCancelSearchAdv").on("click", CompanyAddressTypeAdvancedSearch.CancelSearchAdv);
        $("#btnOkSearchAdv").on("click", CompanyAddressTypeAdvancedSearch.OkSearchAdv);
    }


    static SearchAdvCompanyAddressType() {
        dropDownSearchAdvCompanyAddressType.show();
    }

    static CancelSearchAdv() {
        ParametrizationCompanyAddressType.HideSearchAdv();
    }

    static OkSearchAdv() {
        let data = $("#lvSearchAdvCompanyAddressType").UifListView("getSelected");
        if (data.length === 1) {
            ParametrizationCompanyAddressType.ShowData(null, data, data.key);
        }
        ParametrizationCompanyAddressType.HideSearchAdv();
    }
}