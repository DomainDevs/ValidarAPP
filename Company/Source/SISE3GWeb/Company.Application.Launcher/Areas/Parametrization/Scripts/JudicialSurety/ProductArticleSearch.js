var dropDownSearchAdvProductArticle = null;
class ProductArticleSearch extends Uif2.Page {
    getInitialState() {

        dropDownSearchAdvProductArticle = uif2.dropDown({
            source: rootPath + 'Parametrization/JudicialSurety/ArticleProductSearch',
            element: '#inputArticleProduct',
            align: 'right',
            width: 600,
            height: 500,
            loadedCallback: this.componentLoadedCallback
        });

    }
    bindEvents() {

    }

    componentLoadedCallback() {
        $("#lvSearchAdvArticleProduct").UifListView({
            displayTemplate: "#ArticleProductTemplate",
            selectionType: "single",
            height: 400
        });
        $("#btnCancelArticleProductSearch").on("click", ProductArticleSearch.CancelSearchAdv);
        $("#btnOkArticleProductSearchAdv").on("click", ProductArticleSearch.OkSearchAdv);
    }
    static SearchAdvAdvArticleProduct() {
        dropDownSearchAdvProductArticle.show();
    }
    static CancelSearchAdv() {
        dropDownSearchAdvProductArticle.hide();
    }

    static OkSearchAdv() {
        let data = $("#lvSearchAdvArticleProduct").UifListView("getSelected");
        if (data != undefined && data != null && data[0] != undefined && data[0] != null) {
            if (data[0].ArticleId > 0 && data[0].ProductId > 0) {
                ProductArticle.setData(data[0]);
            }
        }
        dropDownSearchAdvProductArticle.hide();
    }

}