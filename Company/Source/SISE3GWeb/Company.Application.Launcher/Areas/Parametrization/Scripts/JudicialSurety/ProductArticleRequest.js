$.ajaxSetup({ async: true });
class ProductArticleRequest {
    static LoadProductArticles() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Parametrization/JudicialSurety/LoadProductArticles',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static UpdateProductArticle(productArticleDelete, productArticleUpdate, productArticleInsert) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/JudicialSurety/UpdateProductArticle',
            data: JSON.stringify({  "productArticleDelete": productArticleDelete, "productArticleUpdate": productArticleUpdate, "productArticleInsert": productArticleInsert}),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static LoadProducts() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/JudicialSurety/LoadProducts',
            data: JSON.stringify({ "prefixCode": PrefixType.CAUCION_JUDICIAL}),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static LoadSearchProductArticles(smallDescription) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/JudicialSurety/LoadSearchProductArticles',
            data: JSON.stringify({ "smallDescription": smallDescription }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static LoadArticles() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Parametrization/JudicialSurety/LoadArticles',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}