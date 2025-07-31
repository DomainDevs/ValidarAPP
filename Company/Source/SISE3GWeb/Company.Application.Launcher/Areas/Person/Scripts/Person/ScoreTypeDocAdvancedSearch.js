var dropDownSearchAdvScoreTypeDoc = null;
$(() => {
    new ScoreTypeDocAdvancedSearch();
});
class ScoreTypeDocAdvancedSearch extends Uif2.Page {
    getInitialState() {

        dropDownSearchAdvScoreTypeDoc = uif2.dropDown({
            source: rootPath + 'Person/Person/ScoreTypeDocAdvancedSearch',
            element: '#btnSearchAdvScoreTypeDoc',
            align: 'right',
            width: 550,
            height: 551,
            loadedCallback: this.componentLoadedCallback
        });

    }
    bindEvents() {

    }
    componentLoadedCallback() {
        $("#lvSearchAdvScoreTypeDoc").UifListView({
            displayTemplate: "#ScoreTypeDocTemplate",
            selectionType: "single",
            height: 450
        });

        $("#btnSearchAdvScoreTypeDoc").on("click", ScoreTypeDocAdvancedSearch.SearchAdvScoreTypeDoc);
        $("#btnCancelSearchAdv").on("click", ScoreTypeDocAdvancedSearch.CancelSearchAdv);
        $("#btnOkSearchAdv").on("click", ScoreTypeDocAdvancedSearch.OkSearchAdv);
    }


    static SearchAdvScoreTypeDoc() {
        dropDownSearchAdvScoreTypeDoc.show();
    }

    static CancelSearchAdv() {
        ParametrizationScoreTypeDoc.HideSearchAdv();
    }

    static OkSearchAdv() {
        let data = $("#lvSearchAdvScoreTypeDoc").UifListView("getSelected");
        if (data.length === 1) {
            ParametrizationScoreTypeDoc.ShowData(null, data, data.key);
        }
        ParametrizationScoreTypeDoc.HideSearchAdv();
    }
}