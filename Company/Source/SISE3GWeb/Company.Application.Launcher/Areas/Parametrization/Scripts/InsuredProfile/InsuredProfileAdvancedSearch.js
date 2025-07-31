var dropDownSearchAdvInsuredProfile = null;
$(() => {
    new InsuredProfileAdvancedSearch();
});
class InsuredProfileAdvancedSearch extends Uif2.Page {
    getInitialState() {

        dropDownSearchAdvInsuredProfile = uif2.dropDown({
            source: rootPath + 'Parametrization/InsuredProfile/InsuredProfileAdvancedSearch',
            element: '#btnSearchAdvInsuredProfile',
            align: 'right',
            width: 550,
            height: 551,
            loadedCallback: this.componentLoadedCallback
        });

    }
    bindEvents() {

    }   
    componentLoadedCallback() {
        $("#lvSearchAdvInsuredProfile").UifListView({
            displayTemplate: "#InsuredProfileTemplate",
            selectionType: "single",
            height: 450
        });

        
        $("#btnSearchAdvInsuredProfile").on("click", InsuredProfileAdvancedSearch.SearchAdvInsuredProfile);
        $("#btnCancelSearchAdv").on("click", InsuredProfileAdvancedSearch.CancelSearchAdv);
        $("#btnOkSearchAdv").on("click", InsuredProfileAdvancedSearch.OkSearchAdv);
    }


    static SearchAdvInsuredProfile() {
        dropDownSearchAdvInsuredProfile.show();
    }

    static CancelSearchAdv() {
        ParametrizationInsuredProfile.HideSearchAdv();
    }

    static OkSearchAdv() {
        let data = $("#lvSearchAdvInsuredProfile").UifListView("getSelected");
        if (data.length === 1) {
            ParametrizationInsuredProfile.ShowData(null, data, data.key);
        }
        ParametrizationInsuredProfile.HideSearchAdv();
    }
}