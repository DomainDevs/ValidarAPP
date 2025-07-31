var dropDownSearchAdvArticleLine = null;
class ArticleLineSearch extends Uif2.Page {
    getInitialState() {

        dropDownSearchAdvArticleLine = uif2.dropDown({
            source: rootPath + 'Parametrization/JudicialSurety/ArticleLineSearch',
            element: '#inputArticleLine',
            align: 'right',
            width: 600,
            height: 500,
            loadedCallback: this.componentLoadedCallback
        });

    }
    bindEvents() {
       
    }

    componentLoadedCallback() {
        $("#lvSearchAdvArticleLine").UifListView({
            displayTemplate: "#ArticleLineTemplate",
            selectionType: "single",
            height: 400
        });
        $("#btnCancelArticleLineSearch").on("click", ArticleLineSearch.CancelSearchAdv);
        $("#btnOkArticlelineSearchAdv").on("click", ArticleLineSearch.OkSearchAdv);
    }

    static OkSearchAdv() {
        let data = $("#lvSearchAdvArticleLine").UifListView("getSelected");
        if (data != undefined && data != null && data[0] != undefined && data[0] != null) {
            if (data[0].ArticleLineCd > 0) {
                ArticleLine.setData(data[0]);
            }
        }
        dropDownSearchAdvArticleLine.hide();
    }

    static SearchAdvAdvArticleLine() {
        dropDownSearchAdvArticleLine.show();
    }
}