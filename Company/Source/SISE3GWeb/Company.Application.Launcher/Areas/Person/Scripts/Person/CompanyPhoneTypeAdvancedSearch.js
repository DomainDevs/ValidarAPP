var dropDownSearchAdvCompanyPhoneType = null;
$(() => {
    new CompanyPhoneTypeAdvancedSearch();
});
class CompanyPhoneTypeAdvancedSearch extends Uif2.Page {
    getInitialState() {

        dropDownSearchAdvCompanyPhoneType = uif2.dropDown({
            source: rootPath + 'Person/Person/CompanyPhoneTypeAdvancedSearch',
            element: '#btnSearchAdvCompanyPhoneType',
            align: 'right',
            width: 550,
            height: 551,
            loadedCallback: this.componentLoadedCallback
        });

    }
    bindEvents() {

    }
    componentLoadedCallback() {
        $("#lvSearchAdvCompanyPhoneType").UifListView({
            displayTemplate: "#CompanyPhoneTypeTemplate",
            selectionType: "single",
            height: 450
        });

        $("#btnSearchAdvCompanyPhoneType").on("click", CompanyPhoneTypeAdvancedSearch.SearchAdvCompanyPhoneType);
        $("#btnCancelSearchAdv").on("click", CompanyPhoneTypeAdvancedSearch.CancelSearchAdv);
        $("#btnOkSearchAdv").on("click", CompanyPhoneTypeAdvancedSearch.OkSearchAdv);
    }


    static SearchAdvCompanyPhoneType() {
        dropDownSearchAdvCompanyPhoneType.show();
    }

    static CancelSearchAdv() {
        ParametrizationCompanyPhoneType.HideSearchAdv();
    }

    static OkSearchAdv() {
        let data = $("#lvSearchAdvCompanyPhoneType").UifListView("getSelected");
        if (data.length === 1) {
            ParametrizationCompanyPhoneType.ShowData(null, data, data.key);
        }
        ParametrizationCompanyPhoneType.HideSearchAdv();
    }
}