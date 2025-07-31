$.ajaxSetup({ async: true });
class ArticleLineRequest {
    static LoadArticlesLine() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Parametrization/JudicialSurety/LoadArticlesLine',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }


    static UpdateArticleLine(articleLineDelete, articleLineUpdate, articleLineInsert) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/JudicialSurety/UpdateArticleLine',
            data: JSON.stringify({ "articleLineDelete": articleLineDelete, "articleLineUpdate": articleLineUpdate, "articleLineInsert": articleLineInsert }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static LoadSearchArticleLine(smallDescription) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/JudicialSurety/LoadSearchArticleLine',
            data: JSON.stringify({ "smallDescription": smallDescription }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });

    }

}