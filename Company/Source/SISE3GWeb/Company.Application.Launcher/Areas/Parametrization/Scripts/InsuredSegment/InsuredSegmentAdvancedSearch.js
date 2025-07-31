var dropDownSearchAdvInsuredSegment = null;
$(() => {
    new InsuredSegmentAdvancedSearch();
});
class InsuredSegmentAdvancedSearch extends Uif2.Page {
    getInitialState() {

        dropDownSearchAdvInsuredSegment = uif2.dropDown({
            source: rootPath + 'Parametrization/InsuredSegment/InsuredSegmentAdvancedSearch',
            element: '#btnSearchAdvInsuredSegment',
            align: 'right',
            width: 550,
            height: 551,
            loadedCallback: this.componentLoadedCallback
        });

    }
    bindEvents() {

    }   
    componentLoadedCallback() {
        $("#lvSearchAdvInsuredSegment").UifListView({
            displayTemplate: "#InsuredSegmentTemplate",
            selectionType: "single",
            height: 450
        });

        
        $("#btnSearchAdvInsuredSegment").on("click", InsuredSegmentAdvancedSearch.SearchAdvInsuredSegment);
        $("#btnCancelSearchAdv").on("click", InsuredSegmentAdvancedSearch.CancelSearchAdv);
        $("#btnOkSearchAdv").on("click", InsuredSegmentAdvancedSearch.OkSearchAdv);
    }


    static SearchAdvInsuredSegment() {
        dropDownSearchAdvInsuredSegment.show();
    }

    static CancelSearchAdv() {
        ParametrizationInsuredSegment.HideSearchAdv();
    }

    static OkSearchAdv() {
        let data = $("#lvSearchAdvInsuredSegment").UifListView("getSelected");
        if (data.length === 1) {
            ParametrizationInsuredSegment.ShowData(null, data, data.key);
        }
        ParametrizationInsuredSegment.HideSearchAdv();
    }
}