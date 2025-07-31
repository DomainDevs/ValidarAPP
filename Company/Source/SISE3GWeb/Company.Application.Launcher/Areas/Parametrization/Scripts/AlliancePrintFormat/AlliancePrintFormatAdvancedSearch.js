var dropDownSearchAdvAlliancePrintFormat = null;
$(() => {
    new AlliancePrintFormatAdvancedSearch();
});
class AlliancePrintFormatAdvancedSearch extends Uif2.Page {
    getInitialState() {

        dropDownSearchAdvAlliancePrintFormat = uif2.dropDown({
            source: rootPath + 'Parametrization/AlliancePrintFormat/AlliancePrintFormatAdvancedSearch',
            element: '#btnSearchAdvAlliancePrintFormat',
            align: 'right',
            width: 600,
            height: 551,
            loadedCallback: this.componentLoadedCallback
        });

    }
    bindEvents() {

    }   
    componentLoadedCallback() {
        $("#lvSearchAdvAlliancePrintFormat").UifListView({
            displayTemplate: "#AlliancePrintFormatTemplate",
            selectionType: "single",
            height: 450
        });

        
        $("#btnSearchAdvAlliancePrintFormat").on("click", AlliancePrintFormatAdvancedSearch.SearchAdvAlliancePrintFormat);
        $("#btnCancelSearchAdv").on("click", AlliancePrintFormatAdvancedSearch.CancelSearchAdv);
        $("#btnOkSearchAdv").on("click", AlliancePrintFormatAdvancedSearch.OkSearchAdv);
    }


    static SearchAdvAlliancePrintFormat() {
        dropDownSearchAdvAlliancePrintFormat.show();
    }

    static CancelSearchAdv() {
        ParametrizationAlliancePrintFormat.HideSearchAdv();
    }

    static OkSearchAdv() {
        let data = $("#lvSearchAdvAlliancePrintFormat").UifListView("getSelected");
        if (data.length === 1) {
            ParametrizationAlliancePrintFormat.ShowData(null, data, data.key);
        }
        ParametrizationAlliancePrintFormat.HideSearchAdv();
    }
}